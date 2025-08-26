using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace RUINORERP.UI.UCSourceGrid
{
    /// <summary>
    /// 扩展的字段映射关系，包含条件和计算公式
    /// </summary>
    public class QueryColumnMapping
    {
        // 条件表达式：判断是否需要执行计算
        public LambdaExpression ConditionExpression { get; set; }

        // 计算公式：基于From和To类型的属性计算值
        public LambdaExpression CalculationExpression { get; set; }

        // 存储表达式中用到的属性名，用于后续处理
        public List<string> RequiredPropertyNames { get; set; } = new List<string>();

        /// <summary>
        /// 源字段名称
        /// </summary>
        public string SourceColumnName { get; set; }

        /// <summary>
        /// 目标列定义
        /// </summary>
        public SGDefineColumnItem TargetColumn { get; set; }

        /// <summary>
        /// 公式计算
        /// </summary>
        public CalculateFormula calculateFormula { get; set; }

        /// <summary>
        /// 计算委托：接收当前明细对象，返回计算后的值
        /// </summary>
        public Func<object, object> ValueCalculator { get; set; }
    }


    /*
    /// <summary>
    /// 映射扩展方法类
    /// </summary>
    public static class QueryItemToColumnPairExtensions
    {
        /// <summary>
        /// 扩展方法：设置带条件和计算公式的字段映射
        /// </summary>
        public static void SetQueryItemToColumnPairs<TSource, TTarget>(
            this SourceGridHelper sgh,
            SourceGridDefine define,
            Expression<Func<TSource, object>> fromExp,
            Expression<Func<TTarget, object>> toExp,
            string conditionExpression = null,
            string calculationFormula = null,
            params string[] requiredPropertyNames)
        {
            // 获取源字段和目标字段名称
            string fromColName = GetMemberName(fromExp);
            string toColName = GetMemberName(toExp);

            // 查找目标列定义
            var targetColumn = define.DefineColumns
                .FirstOrDefault(c => c.ColName == toColName &&
                                    c.BelongingObjectType == typeof(TTarget));

            if (targetColumn == null)
            {
                MainForm.Instance.uclog.AddLog("提醒",
                    $"未找到目标字段{toColName}（类型：{typeof(TTarget).Name}）");
                return;
            }

            // 创建扩展映射关系
            var mapping = new QueryColumnMapping
            {
                SourceColumnName = fromColName,
                TargetColumn = targetColumn,
                ConditionExpression = conditionExpression,
                CalculationFormula = calculationFormula,
                RequiredPropertyNames = requiredPropertyNames.ToList()
            };

            // 确保映射字典已初始化
            if (define.QueryColumnMappings == null)
            {
                define.QueryColumnMappings = new ConcurrentDictionary<string, QueryColumnMapping>();
            }

            // 添加或更新映射关系
            define.QueryColumnMappings.AddOrUpdate(
                $"{typeof(TSource).Name}_{fromColName}_{typeof(TTarget).Name}_{toColName}",
                mapping,
                (key, existing) => mapping
            );
        }

        /// <summary>
        /// 从表达式中获取成员名称
        /// </summary>
        private static string GetMemberName<T>(Expression<Func<T, object>> exp)
        {
            if (exp.Body is UnaryExpression unary && unary.Operand is MemberExpression member)
            {
                return member.Member.Name;
            }
            if (exp.Body is MemberExpression memberExp)
            {
                return memberExp.Member.Name;
            }
            throw new ArgumentException("无效的表达式", nameof(exp));
        }

        /// <summary>
        /// 应用映射关系，根据条件和公式计算并赋值
        /// </summary>
        public static void ApplyMappings(this SourceGridDefine define, object sourceObj, object targetObj)
        {
            if (define.QueryColumnMappings == null || sourceObj == null || targetObj == null)
                return;

            foreach (var mapping in define.QueryColumnMappings.Values)
            {
                // 检查是否需要应用此映射
                if (!ShouldApplyMapping(mapping, sourceObj, targetObj))
                    continue;

                // 计算目标值
                object targetValue = CalculateTargetValue(mapping, sourceObj, targetObj);

                // 赋值给目标对象
                if (targetValue != null)
                {
                    var targetProperty = targetObj.GetType().GetProperty(mapping.TargetColumn.ColName);
                    if (targetProperty != null)
                    {
                        targetProperty.SetValue(targetObj, targetValue);
                    }
                }
            }
        }

        /// <summary>
        /// 检查是否应该应用映射（条件判断）
        /// </summary>
        private static bool ShouldApplyMapping(QueryColumnMapping mapping, object sourceObj, object targetObj)
        {
            // 没有条件表达式则直接应用
            if (string.IsNullOrEmpty(mapping.ConditionExpression))
                return true;

            // 使用现有CalculationCondition类解析条件表达式
            var condition = new CalculationCondition
            {
                CalculationTargetType = targetObj.GetType(),
                expCondition = ParseExpression(mapping.ConditionExpression, targetObj.GetType())
            };

            return condition.GetConditionResult(targetObj);
        }

        /// <summary>
        /// 计算目标值（应用公式）
        /// </summary>
        private static object CalculateTargetValue(QueryColumnMapping mapping, object sourceObj, object targetObj)
        {
            // 收集所有需要的属性值
            var parameters = new Dictionary<string, object>();

            // 获取源对象属性值
            var sourceProperty = sourceObj.GetType().GetProperty(mapping.SourceColumnName);
            if (sourceProperty != null)
            {
                parameters[mapping.SourceColumnName] = sourceProperty.GetValue(sourceObj) ?? 0;
            }

            // 获取目标对象的额外属性值
            foreach (var propName in mapping.RequiredPropertyNames)
            {
                var targetProperty = targetObj.GetType().GetProperty(propName);
                if (targetProperty != null)
                {
                    parameters[propName] = targetProperty.GetValue(targetObj) ?? 0;
                }
            }

            // 如果没有计算公式，直接使用源值
            if (string.IsNullOrEmpty(mapping.CalculationFormula))
            {
                return parameters.TryGetValue(mapping.SourceColumnName, out var value) ? value : null;
            }

            // 使用现有ExpressionEvaluator计算结果
            var evaluator = new ExpressionEvaluator(mapping.CalculationFormula, parameters);
            return evaluator.Evaluate();
        }

        /// <summary>
        /// 解析表达式字符串为Expression对象
        /// </summary>
        private static Expression ParseExpression(string expression, Type targetType)
        {
            // 这里可以使用现有表达式解析逻辑
            // 简化实现，实际应使用更完善的解析
            var parameter = Expression.Parameter(targetType, "t");
            var parser = new ExpressionParser(expression, parameter);
            return parser.Parse();
        }
    }
    */

}
