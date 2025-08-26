using AutoMapper.Internal;
using SourceGrid.Conditions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static OfficeOpenXml.ExcelErrorValue;

namespace RUINORERP.UI.UCSourceGrid
{

    /// <summary>
    /// 计算公式
    /// 目标列就也是结果列，
    /// 一个目标列可能来自两组公式
    /// 计算公式可能是在一定条件下，才会参与计算
    /// 在一定条件下，才参与计算的，带公式的参数都可以用这个传递
    /// </summary>
    public class CalculateFormula
    {

        /// <summary>
        /// 冗余了一个名称，用于判断是否重复
        /// </summary>
        public string TagetColName { get; set; }

        public SGDefineColumnItem TagetCol { get; set; }

        private int _OperandQty;

        private string _StringFormula = string.Empty;

        public string StringFormula { get => _StringFormula; set => _StringFormula = value; }

        /// <summary>
        /// 操作数个数
        /// </summary>
        public int OperandQty { get => _OperandQty; set => _OperandQty = value; }
        public List<string> Parameter { get => parameter; set => parameter = value; }


        private List<string> parameter = new List<string>();


        /// <summary>
        /// 保存原始的公式，因为以这个公式和目标或叫结果列为条件判断是否重复
        /// </summary>
        string originalExpression = string.Empty;

        /// <summary>
        /// 原始公式
        /// </summary>
        public string OriginalExpression { get => originalExpression; set => originalExpression = value; }


        /// <summary>
        /// 条件判断公式
        /// </summary>
        public CalculationCondition CalcCondition { get; set; }

    }

    /// <summary>
    /// 公式计算条件可能也是一些列的值，这些列的值可能是固定的，也可能是动态的
    /// 目前解决：如果单价为0时才参与计算
    /// c=>c.Unitprice==0
    /// </summary>
    public class CalculationCondition
    {
        public Type CalculationTargetType { get; set; }

        public CalculationCondition()
        {

        }


        /// <summary>
        /// 最重要的表达式条件
        /// </summary>
        public Expression expCondition { get; set; }
        private string ConditionLeftColName { get; set; }
        private object ConditionLeftValue { get; set; }
        private string ConditionRightColName { get; set; }
        private object ConditionRightValue { get; set; }

        /// <summary>
        /// 获取条件判断结果
        /// 结果为真是才参与条件计算
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool GetConditionResult(object obj)
        {
            bool result = false;
            result = GetConditionSubResultLoop(obj, expCondition);
            return result;
        }

        public bool GetConditionSubResultLoop(object obj, Expression subExp)
        {
            bool result = false;
            BinaryExpression binaryExpression = (BinaryExpression)subExp;


            if (binaryExpression.NodeType == ExpressionType.AndAlso)
            {
                return GetConditionSubResultLoop(obj, binaryExpression.Left) && GetConditionSubResultLoop(obj, binaryExpression.Right);

            }
            else if (binaryExpression.NodeType == ExpressionType.OrElse)
            {
                return GetConditionSubResultLoop(obj, binaryExpression.Left) || GetConditionSubResultLoop(obj, binaryExpression.Right);
            }
            else
            {
                return GetBasicCalculationResult(obj, binaryExpression);
            }
        }


        public bool GetBasicCalculationResult_old(object obj, Expression exp)
        {
            bool result = false;
            BinaryExpression binaryExpression = (BinaryExpression)exp;
            ConditionLeftColName = binaryExpression.Left.ToString().Split('.')[1];
            PropertyInfo propertyInfoLeft = CalculationTargetType.GetProperty(ConditionLeftColName);
            if (propertyInfoLeft == null)
            {
                return false;
            }
            object ConditionLeftValue = propertyInfoLeft.GetValue(obj);

            // 尝试将字符串值转换为属性类型  左右边应该都一样的类型
            Type valueType = ConditionLeftValue.GetType();

            object compareValue = 0;
            //右边如果不是常量时，
            if (binaryExpression.Right is ConstantExpression constant)
            {
                ConditionRightValue = binaryExpression.Right;
                compareValue = Convert.ChangeType(constant.Value, valueType);

            }
            else if (binaryExpression.Right is MemberExpression)
            {
                ConditionRightColName = binaryExpression.Right.ToString().Split('.')[1];
            }
            if (!string.IsNullOrWhiteSpace(ConditionRightColName))
            {
                PropertyInfo propertyInfoRight = CalculationTargetType.GetProperty(ConditionRightColName);
                ConditionRightValue = propertyInfoRight.GetValue(obj);
            }

            if (valueType.IsEnum)
            {
                compareValue = Enum.Parse(valueType, ConditionLeftValue.ToString());
            }
            else
            {
                compareValue = ConditionRightValue;
            }

            switch (binaryExpression.NodeType)
            {
                case ExpressionType.Equal:
                    result = ConditionLeftValue.Equals(compareValue);
                    break;
                case ExpressionType.NotEqual:
                    result = !ConditionLeftValue.Equals(compareValue);
                    break;
                case ExpressionType.GreaterThan:
                    result = (decimal)ConditionLeftValue > (decimal)compareValue;
                    break;
                case ExpressionType.LessThan:
                    result = (decimal)ConditionLeftValue < (decimal)compareValue;
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    result = (decimal)ConditionLeftValue >= (decimal)compareValue;
                    break;
                case ExpressionType.LessThanOrEqual:
                    result = (decimal)ConditionLeftValue <= (decimal)compareValue;
                    break;
                default:
                    break;
            }

            return result;

        }

        public bool GetBasicCalculationResult(object obj, Expression exp)
        {
            bool result = false;
            BinaryExpression binaryExpression = (BinaryExpression)exp;
            string conditionLeftColName = binaryExpression.Left.ToString().Split('.')[1];
            PropertyInfo propertyInfoLeft = CalculationTargetType.GetProperty(conditionLeftColName);
            if (propertyInfoLeft == null)
            {
                return false;
            }
            object conditionLeftValue = propertyInfoLeft.GetValue(obj);

            // 获取左侧属性的值类型
            Type valueType = conditionLeftValue.GetType();

            object compareValue = null;
            string conditionRightColName = null;

            // 处理右侧表达式
            if (binaryExpression.Right is ConstantExpression constant)
            {
                compareValue = Convert.ChangeType(constant.Value, valueType);
            }
            else if (binaryExpression.Right is MemberExpression member)
            {
                conditionRightColName = member.ToString().Split('.')[1];
                PropertyInfo propertyInfoRight = CalculationTargetType.GetProperty(conditionRightColName);
                compareValue = propertyInfoRight.GetValue(obj);
            }

            // 处理枚举类型
            if (valueType.IsEnum)
            {
                conditionLeftValue = Enum.Parse(valueType, conditionLeftValue.ToString());
                compareValue = Enum.Parse(valueType, compareValue.ToString());
            }
            else
            {
                // 确保比较值和左侧值类型一致
                compareValue = Convert.ChangeType(compareValue, valueType);
            }

            // 处理可能的null值
            if (conditionLeftValue == null || compareValue == null)
            {
                // 根据业务逻辑处理null值的情况
                result = conditionLeftValue == compareValue;
            }
            else
            {
                // 比较值
                switch (binaryExpression.NodeType)
                {
                    case ExpressionType.Equal:
                        result = conditionLeftValue.Equals(compareValue);
                        break;
                    case ExpressionType.NotEqual:
                        result = !conditionLeftValue.Equals(compareValue);
                        break;
                    case ExpressionType.GreaterThan:
                        result = (decimal)conditionLeftValue > (decimal)compareValue;
                        break;
                    case ExpressionType.LessThan:
                        result = (decimal)conditionLeftValue < (decimal)compareValue;
                        break;
                    case ExpressionType.GreaterThanOrEqual:
                        result = (decimal)conditionLeftValue >= (decimal)compareValue;
                        break;
                    case ExpressionType.LessThanOrEqual:
                        result = (decimal)conditionLeftValue <= (decimal)compareValue;
                        break;
                    default:
                        break;
                }
            }

            return result;
        }
        public static bool CheckCondition(object obj, Type objType, string propertyName, Func<object, bool> condition)
        {

            // 使用反射获取对象的属性值
            PropertyInfo propertyInfo = objType.GetProperty(propertyName);
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Property '{propertyName}' not found on type '{objType.FullName}'.");
            }

            var value = propertyInfo.GetValue(obj);

            // 将属性值转换为可以用于条件检查的类型（这里假设是decimal）
            if (value is decimal)
            {
                // 使用提供的委托来检查条件
                return condition(value);
            }

            return false;
        }

       

    }



    //a影响 B ,C, D， 每个都 有一个值。可能是固定的。也可能是动态的。用占位符来确定。如果是占位，则要指定多个{0}的指定的列的名。再取值
    /// <summary>
    /// 新值的相关参数设置
    /// </summary>
    public class RelatedColumnParameter
    {
        public RelatedColumnParameter(string colTargetName, string newValue, TargetValueParameter[] valueParameters)
        {
            ColTargetName = colTargetName;
            NewValue = newValue;
            ValueParameters = valueParameters;
        }

        public string ColTargetName { get; set; }
        public string NewValue { get; set; }
        public TargetValueParameter[] ValueParameters { get; set; }
    }

    /// <summary>
    /// 控制点位符值中的新的值的参数
    /// </summary>
    public class TargetValueParameter
    {
        string _parameterColName;

        string _pointToColName;

        /// <summary>
        /// 通过这个列名找到GRID中对象对应的值，有时下拉选择的是ID，但是又想提取到名称。用于新值的占位。则要指定另一个列名。并且要在缓存中
        /// </summary>
        public string ParameterColName { get => _parameterColName; set => _parameterColName = value; }
        /// <summary>
        /// 通过上面的ID列指定的另一列。如名称
        /// </summary>
        public string PointToColName { get => _pointToColName; set => _pointToColName = value; }


        Type _fkTableType;
        public Type FkTableType { get => _fkTableType; set => _fkTableType = value; }
    }


}
