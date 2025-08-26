using Fireasy.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace RUINORERP.UI.UCSourceGrid
{
 

    /*
    public static class MappingExtensions
    {
        // 原有简单映射保持不变
        public static void SetQueryItemToColumnPairs<From, To>(
            this SourceGridHelper sgh,
            SourceGridDefine define,
            Expression<Func<From, object>> fromExp,
            Expression<Func<To, object>> toExp)
        {
            SetQueryItemToColumnPairs(sgh, define, fromExp, toExp, null, null);
        }

        // 带条件和公式的扩展映射
        public static void SetQueryItemToColumnPairs<From, To>(
            this SourceGridHelper sgh,
            SourceGridDefine define,
            Expression<Func<From, object>> fromExp,
            Expression<Func<To, object>> toExp,
            Expression<Func<To, bool>> conditionExp = null,  // 条件表达式：基于To类型的属性
            Expression<Func<From, To, object>> calculationExp = null)  // 计算公式：使用From和To的属性
        {
            // 获取源字段和目标字段名称
            string fromColName = GetMemberName(fromExp);
            string toColName = GetMemberName(toExp);

            // 获取目标列定义
            var targetColumn = define.DefineColumns
                .Where(c => c.ColName == toColName && c.BelongingObjectType == typeof(To))
                .FirstOrDefault();

            if (targetColumn == null)
            {
                MainForm.Instance.uclog.AddLog("提醒",
                    $"当前字段{toColName}没有提取到,请确认在单据明细{typeof(To).Name}中描述是否正确");
                return;
            }

            // 提取表达式中用到的属性名
            var requiredProps = new List<string>();
            if (conditionExp != null)
            {
                requiredProps.AddRange(ExtractPropertyNames(conditionExp.Body));
            }
            if (calculationExp != null)
            {
                requiredProps.AddRange(ExtractPropertyNames(calculationExp.Body));
            }

            // 创建扩展映射
            var mapping = new ExtendedQueryColumnMapping
            {
                SourceColumnName = fromColName,
                TargetColumn = targetColumn,
                ConditionExpression = conditionExp,
                CalculationExpression = calculationExp,
                RequiredPropertyNames = requiredProps.Distinct().ToList()
            };

            // 保存映射关系
            define.QueryColumnMappings.TryAdd(fromColName, mapping);
        }

        // 应用映射并计算值
        public static void ApplyMappings<From, To>(
            this SourceGridDefine define,
            From fromObj,
            To toObj)
        {
            foreach (var mapping in define.QueryColumnMappings.Values)
            {
                // 检查是否需要应用此映射
                bool shouldApply = true;

                // 执行条件判断
                if (mapping.ConditionExpression != null)
                {
                    var conditionFunc = ((Expression<Func<To, bool>>)mapping.ConditionExpression).Compile();
                    shouldApply = conditionFunc(toObj);
                }

                if (shouldApply)
                {
                    object value;

                    // 执行计算
                    if (mapping.CalculationExpression != null)
                    {
                        var calcFunc = ((Expression<Func<From, To, object>>)mapping.CalculationExpression).Compile();
                        value = calcFunc(fromObj, toObj);
                    }
                    else
                    {
                        // 没有计算公式则直接取源字段值
                        var fromProp = typeof(From).GetProperty(mapping.SourceColumnName);
                        value = fromProp.GetValue(fromObj);
                    }

                    // 赋值到目标对象
                    var toProp = typeof(To).GetProperty(mapping.TargetColumn.ColName);
                    if (toProp != null && value != null)
                    {
                        // 处理类型转换
                        var targetType = toProp.PropertyType;
                        var valueType = value.GetType();

                        if (targetType != valueType && targetType.IsAssignableFrom(valueType))
                        {
                            value = Convert.ChangeType(value, targetType);
                        }

                        toProp.SetValue(toObj, value);
                    }
                }
            }
        }

        // 辅助方法：获取表达式中的成员名称
        private static string GetMemberName<TSource, TMember>(Expression<Func<TSource, TMember>> expression)
        {
            if (expression.Body is UnaryExpression unary && unary.Operand is MemberExpression member)
            {
                return member.Member.Name;
            }
            if (expression.Body is MemberExpression memberExpr)
            {
                return memberExpr.Member.Name;
            }
            throw new ArgumentException("Invalid expression", nameof(expression));
        }

        // 辅助方法：提取表达式中用到的属性名
        private static List<string> ExtractPropertyNames(Expression exp)
        {
            var propertyNames = new List<string>();

            if (exp is MemberExpression memberExp && memberExp.Member is PropertyInfo)
            {
                propertyNames.Add(memberExp.Member.Name);
            }
            else if (exp is BinaryExpression binaryExp)
            {
                propertyNames.AddRange(ExtractPropertyNames(binaryExp.Left));
                propertyNames.AddRange(ExtractPropertyNames(binaryExp.Right));
            }
            else if (exp is ParameterExpression)
            {
                // 忽略参数本身
            }
            else if (exp is UnaryExpression unaryExp)
            {
                propertyNames.AddRange(ExtractPropertyNames(unaryExp.Operand));
            }

            return propertyNames;
        }
    }
    */
}
