using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;

namespace RUINORERP.UI.UCSourceGrid
{
    public static class CalculationExecutor
    {
        /// <summary>
        /// 执行计算
        /// </summary>
        /// <param name="formula">计算公式</param>
        /// <param name="dataItem">数据对象（如tb_StocktakeDetail实例）</param>
        /// <returns>计算结果</returns>
        public static object Execute(CalculateFormula formula, object dataItem)
        {
            if (dataItem == null || formula == null || string.IsNullOrEmpty(formula.StringFormula))
                return null;

            try
            {
                // 检查是否需要执行计算（满足条件）
                if (!CheckCalculationCondition(formula, dataItem))
                    return GetCurrentValue(formula, dataItem);

                // 获取所有参数值
                var parameterValues = GetParameterValues(formula, dataItem);

                // 构建计算表达式并执行
                return EvaluateExpression(formula.StringFormula, parameterValues);
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog("计算错误",
                    $"公式 [{formula.StringFormula}] 执行失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 检查计算条件是否满足
        /// </summary>
        private static bool CheckCalculationCondition(CalculateFormula formula, object dataItem)
        {
            if (formula.CalcCondition == null)
                return true; // 无条件时始终执行

            return formula.CalcCondition.GetConditionResult(dataItem);
        }

        /// <summary>
        /// 获取当前目标列的值（用于不满足条件时返回原值）
        /// </summary>
        private static object GetCurrentValue(CalculateFormula formula, object dataItem)
        {
            if (formula.TagetCol == null)
                return null;

            PropertyInfo property = dataItem.GetType().GetProperty(formula.TagetCol.ColName);
            return property?.GetValue(dataItem);
        }

        /// <summary>
        /// 获取公式所需的所有参数值
        /// </summary>
        private static Dictionary<string, object> GetParameterValues(CalculateFormula formula, object dataItem)
        {
            var values = new Dictionary<string, object>();
            Type dataType = dataItem.GetType();

            foreach (string paramName in formula.Parameter)
            {
                PropertyInfo property = dataType.GetProperty(paramName);
                if (property != null)
                {
                    object value = property.GetValue(dataItem);
                    // 处理null值，转换为0
                    values[paramName] = value ?? 0;
                }
                else
                {
                    throw new ArgumentException($"属性 {paramName} 在类型 {dataType.Name} 中不存在");
                }
            }

            return values;
        }

        /// <summary>
        /// 解析并执行表达式
        /// </summary>
        private static object EvaluateExpression(string expression, Dictionary<string, object> parameters)
        {
            // 简单表达式解析器实现
            // 实际项目中可考虑使用更完善的表达式计算库
            var evaluator = new ExpressionEvaluator(expression, parameters);
            return evaluator.Evaluate();
        }
    }

    /// <summary>
    /// 简单表达式计算器
    /// </summary>
    internal class ExpressionEvaluator
    {
        private string _expression;
        private Dictionary<string, object> _parameters;

        public ExpressionEvaluator(string expression, Dictionary<string, object> parameters)
        {
            _expression = expression;
            _parameters = parameters;
        }

        public object Evaluate()
        {
            // 替换参数为实际值
            string evaluatedExpression = _expression;
            foreach (var param in _parameters)
            {
                evaluatedExpression = evaluatedExpression.Replace(param.Key,
                    Convert.ToString(param.Value, CultureInfo.InvariantCulture));
            }

            // 使用现有计算引擎执行表达式
            // 这里可以集成你现有的表达式计算逻辑
            return EvaluateMathematicalExpression(evaluatedExpression);
        }

        // 简单的数学表达式计算
        private object EvaluateMathematicalExpression(string expression)
        {
            // 实现基本的数学运算解析
            // 实际应用中可使用更完善的实现
            var dataTable = new System.Data.DataTable();
            return dataTable.Compute(expression, string.Empty);
        }
    }
}
