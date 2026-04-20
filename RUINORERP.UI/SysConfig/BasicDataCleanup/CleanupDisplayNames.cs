using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace RUINORERP.UI.SysConfig.BasicDataCleanup
{
    /// <summary>
    /// 显示名称管理工具类
    /// 统一管理清理规则相关的显示名称
    /// </summary>
    public static class CleanupDisplayNames
    {
        #region 规则类型显示名称

        /// <summary>
        /// 规则类型显示名称字典
        /// </summary>
        private static readonly Dictionary<CleanupRuleType, string> RuleTypeDisplayNames = new Dictionary<CleanupRuleType, string>
        {
            { CleanupRuleType.DuplicateRemoval, "重复数据清理" },
            { CleanupRuleType.EmptyValueRemoval, "空值数据清理" },
            { CleanupRuleType.AbnormalDataRemoval, "异常数据清理" },
            { CleanupRuleType.ExpiredDataRemoval, "过期数据清理" },
            { CleanupRuleType.InvalidReferenceRemoval, "无效关联清理" },
            { CleanupRuleType.CustomConditionRemoval, "自定义条件清理" },
            { CleanupRuleType.DataStandardization, "数据标准化" },
            { CleanupRuleType.DataTruncation, "数据截断" }
        };

        /// <summary>
        /// 获取规则类型显示名称
        /// </summary>
        /// <param name="ruleType">规则类型</param>
        /// <returns>显示名称</returns>
        public static string GetRuleTypeDisplayName(CleanupRuleType ruleType)
        {
            return RuleTypeDisplayNames.TryGetValue(ruleType, out string displayName) 
                ? displayName 
                : ruleType.ToString();
        }

        /// <summary>
        /// 获取所有规则类型及其显示名称
        /// </summary>
        /// <returns>规则类型和显示名称的字典</returns>
        public static Dictionary<CleanupRuleType, string> GetAllRuleTypeDisplayNames()
        {
            return new Dictionary<CleanupRuleType, string>(RuleTypeDisplayNames);
        }

        #endregion

        #region 操作类型显示名称

        /// <summary>
        /// 操作类型显示名称字典
        /// </summary>
        private static readonly Dictionary<CleanupActionType, string> ActionTypeDisplayNames = new Dictionary<CleanupActionType, string>
        {
            { CleanupActionType.Delete, "删除记录" },
            { CleanupActionType.MarkAsInvalid, "标记为无效" },
            { CleanupActionType.Archive, "归档到历史表" },
            { CleanupActionType.UpdateField, "更新字段值" },
            { CleanupActionType.LogOnly, "仅记录不执行" }
        };

        /// <summary>
        /// 获取操作类型显示名称
        /// </summary>
        /// <param name="actionType">操作类型</param>
        /// <returns>显示名称</returns>
        public static string GetActionTypeDisplayName(CleanupActionType actionType)
        {
            return ActionTypeDisplayNames.TryGetValue(actionType, out string displayName) 
                ? displayName 
                : actionType.ToString();
        }

        /// <summary>
        /// 获取所有操作类型及其显示名称
        /// </summary>
        /// <returns>操作类型和显示名称的字典</returns>
        public static Dictionary<CleanupActionType, string> GetAllActionTypeDisplayNames()
        {
            return new Dictionary<CleanupActionType, string>(ActionTypeDisplayNames);
        }

        #endregion

        #region 比较运算符显示名称

        /// <summary>
        /// 比较运算符显示名称字典
        /// </summary>
        private static readonly Dictionary<ComparisonOperator, string> ComparisonOperatorDisplayNames = new Dictionary<ComparisonOperator, string>
        {
            { ComparisonOperator.Equals, "等于" },
            { ComparisonOperator.NotEquals, "不等于" },
            { ComparisonOperator.GreaterThan, "大于" },
            { ComparisonOperator.GreaterThanOrEqual, "大于等于" },
            { ComparisonOperator.LessThan, "小于" },
            { ComparisonOperator.LessThanOrEqual, "小于等于" },
            { ComparisonOperator.Contains, "包含" },
            { ComparisonOperator.NotContains, "不包含" },
            { ComparisonOperator.StartsWith, "开头是" },
            { ComparisonOperator.EndsWith, "结尾是" },
            { ComparisonOperator.IsEmpty, "为空" },
            { ComparisonOperator.IsNotEmpty, "不为空" },
            { ComparisonOperator.InList, "在列表中" },
            { ComparisonOperator.NotInList, "不在列表中" }
        };

        /// <summary>
        /// 获取比较运算符显示名称
        /// </summary>
        /// <param name="operator">比较运算符</param>
        /// <returns>显示名称</returns>
        public static string GetComparisonOperatorDisplayName(ComparisonOperator @operator)
        {
            return ComparisonOperatorDisplayNames.TryGetValue(@operator, out string displayName) 
                ? displayName 
                : @operator.ToString();
        }

        /// <summary>
        /// 获取所有比较运算符及其显示名称
        /// </summary>
        /// <returns>比较运算符和显示名称的字典</returns>
        public static Dictionary<ComparisonOperator, string> GetAllComparisonOperatorDisplayNames()
        {
            return new Dictionary<ComparisonOperator, string>(ComparisonOperatorDisplayNames);
        }

        #endregion

        #region 逻辑运算符显示名称

        /// <summary>
        /// 逻辑运算符显示名称字典
        /// </summary>
        private static readonly Dictionary<LogicalOperator, string> LogicalOperatorDisplayNames = new Dictionary<LogicalOperator, string>
        {
            { LogicalOperator.And, "并且" },
            { LogicalOperator.Or, "或者" }
        };

        /// <summary>
        /// 获取逻辑运算符显示名称
        /// </summary>
        /// <param name="operator">逻辑运算符</param>
        /// <returns>显示名称</returns>
        public static string GetLogicalOperatorDisplayName(LogicalOperator @operator)
        {
            return LogicalOperatorDisplayNames.TryGetValue(@operator, out string displayName) 
                ? displayName 
                : @operator.ToString();
        }

        #endregion

        #region 重复处理策略显示名称

        /// <summary>
        /// 重复处理策略显示名称字典
        /// </summary>
        private static readonly Dictionary<DuplicateHandlingStrategy, string> DuplicateStrategyDisplayNames = new Dictionary<DuplicateHandlingStrategy, string>
        {
            { DuplicateHandlingStrategy.KeepFirst, "保留第一条" },
            { DuplicateHandlingStrategy.KeepLast, "保留最后一条" },
            { DuplicateHandlingStrategy.KeepNewest, "保留最新" },
            { DuplicateHandlingStrategy.Merge, "合并记录" }
        };

        /// <summary>
        /// 获取重复处理策略显示名称
        /// </summary>
        /// <param name="strategy">重复处理策略</param>
        /// <returns>显示名称</returns>
        public static string GetDuplicateStrategyDisplayName(DuplicateHandlingStrategy strategy)
        {
            return DuplicateStrategyDisplayNames.TryGetValue(strategy, out string displayName) 
                ? displayName 
                : strategy.ToString();
        }

        #endregion

        #region 空值检查模式显示名称

        /// <summary>
        /// 空值检查模式显示名称字典
        /// </summary>
        private static readonly Dictionary<EmptyValueCheckMode, string> EmptyValueModeDisplayNames = new Dictionary<EmptyValueCheckMode, string>
        {
            { EmptyValueCheckMode.NullOnly, "仅NULL值" },
            { EmptyValueCheckMode.NullOrEmpty, "NULL或空字符串" },
            { EmptyValueCheckMode.NullOrWhiteSpace, "NULL、空字符串或空白字符" }
        };

        /// <summary>
        /// 获取空值检查模式显示名称
        /// </summary>
        /// <param name="mode">空值检查模式</param>
        /// <returns>显示名称</returns>
        public static string GetEmptyValueModeDisplayName(EmptyValueCheckMode mode)
        {
            return EmptyValueModeDisplayNames.TryGetValue(mode, out string displayName) 
                ? displayName 
                : mode.ToString();
        }

        #endregion

        #region 日期基准类型显示名称

        /// <summary>
        /// 日期基准类型显示名称字典
        /// </summary>
        private static readonly Dictionary<DateBaseType, string> DateBaseTypeDisplayNames = new Dictionary<DateBaseType, string>
        {
            { DateBaseType.CreateTime, "创建时间" },
            { DateBaseType.UpdateTime, "更新时间" },
            { DateBaseType.CustomField, "指定日期字段" }
        };

        /// <summary>
        /// 获取日期基准类型显示名称
        /// </summary>
        /// <param name="dateBaseType">日期基准类型</param>
        /// <returns>显示名称</returns>
        public static string GetDateBaseTypeDisplayName(DateBaseType dateBaseType)
        {
            return DateBaseTypeDisplayNames.TryGetValue(dateBaseType, out string displayName) 
                ? displayName 
                : dateBaseType.ToString();
        }

        #endregion

        #region 标准化操作显示名称

        /// <summary>
        /// 标准化操作显示名称字典
        /// </summary>
        private static readonly Dictionary<StandardizationOperation, string> StandardizationOperationDisplayNames = new Dictionary<StandardizationOperation, string>
        {
            { StandardizationOperation.Trim, "去除前后空格" },
            { StandardizationOperation.ToUpper, "转换为大写" },
            { StandardizationOperation.ToLower, "转换为小写" },
            { StandardizationOperation.RemoveAllSpaces, "去除所有空格" },
            { StandardizationOperation.StandardizeDate, "标准化日期格式" },
            { StandardizationOperation.StandardizeNumber, "标准化数字格式" }
        };

        /// <summary>
        /// 获取标准化操作显示名称
        /// </summary>
        /// <param name="operation">标准化操作</param>
        /// <returns>显示名称</returns>
        public static string GetStandardizationOperationDisplayName(StandardizationOperation operation)
        {
            return StandardizationOperationDisplayNames.TryGetValue(operation, out string displayName) 
                ? displayName 
                : operation.ToString();
        }

        #endregion

        #region 截断方式显示名称

        /// <summary>
        /// 截断方式显示名称字典
        /// </summary>
        private static readonly Dictionary<TruncationMode, string> TruncationModeDisplayNames = new Dictionary<TruncationMode, string>
        {
            { TruncationMode.FromRight, "从右侧截断" },
            { TruncationMode.FromLeft, "从左侧截断" },
            { TruncationMode.MiddleEllipsis, "中间省略" }
        };

        /// <summary>
        /// 获取截断方式显示名称
        /// </summary>
        /// <param name="mode">截断方式</param>
        /// <returns>显示名称</returns>
        public static string GetTruncationModeDisplayName(TruncationMode mode)
        {
            return TruncationModeDisplayNames.TryGetValue(mode, out string displayName) 
                ? displayName 
                : mode.ToString();
        }

        #endregion

        #region 通用方法

        /// <summary>
        /// 从Description特性获取显示名称
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">枚举值</param>
        /// <returns>显示名称</returns>
        public static string GetDisplayNameFromDescription<T>(T value) where T : Enum
        {
            var fieldInfo = typeof(T).GetField(value.ToString());
            if (fieldInfo != null)
            {
                var descriptionAttribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
                if (descriptionAttribute != null)
                {
                    return descriptionAttribute.Description;
                }
            }
            return value.ToString();
        }

        #endregion
    }
}
