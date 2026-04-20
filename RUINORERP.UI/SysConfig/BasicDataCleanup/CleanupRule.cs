using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace RUINORERP.UI.SysConfig.BasicDataCleanup
{
    /// <summary>
    /// 清理规则类型枚举
    /// </summary>
    public enum CleanupRuleType
    {
        /// <summary>
        /// 重复数据清理
        /// </summary>
        [Description("重复数据清理")]
        DuplicateRemoval = 0,

        /// <summary>
        /// 空值数据清理
        /// </summary>
        [Description("空值数据清理")]
        EmptyValueRemoval = 1,

        /// <summary>
        /// 异常数据清理
        /// </summary>
        [Description("异常数据清理")]
        AbnormalDataRemoval = 2,

        /// <summary>
        /// 过期数据清理
        /// </summary>
        [Description("过期数据清理")]
        ExpiredDataRemoval = 3,

        /// <summary>
        /// 无效关联数据清理
        /// </summary>
        [Description("无效关联数据清理")]
        InvalidReferenceRemoval = 4,

        /// <summary>
        /// 自定义条件清理
        /// </summary>
        [Description("自定义条件清理")]
        CustomConditionRemoval = 5,

        /// <summary>
        /// 数据标准化
        /// </summary>
        [Description("数据标准化")]
        DataStandardization = 6,

        /// <summary>
        /// 数据截断清理
        /// </summary>
        [Description("数据截断清理")]
        DataTruncation = 7
    }

    /// <summary>
    /// 清理操作类型枚举
    /// </summary>
    public enum CleanupActionType
    {
        /// <summary>
        /// 删除记录
        /// </summary>
        [Description("删除记录")]
        Delete = 0,

        /// <summary>
        /// 标记为无效
        /// </summary>
        [Description("标记为无效")]
        MarkAsInvalid = 1,

        /// <summary>
        /// 归档到历史表
        /// </summary>
        [Description("归档到历史表")]
        Archive = 2,

        /// <summary>
        /// 更新字段值
        /// </summary>
        [Description("更新字段值")]
        UpdateField = 3,

        /// <summary>
        /// 仅记录不执行
        /// </summary>
        [Description("仅记录不执行")]
        LogOnly = 4
    }

    /// <summary>
    /// 重复数据处理策略
    /// </summary>
    public enum DuplicateHandlingStrategy
    {
        /// <summary>
        /// 保留第一条
        /// </summary>
        [Description("保留第一条")]
        KeepFirst = 0,

        /// <summary>
        /// 保留最后一条
        /// </summary>
        [Description("保留最后一条")]
        KeepLast = 1,

        /// <summary>
        /// 保留最新（按更新时间）
        /// </summary>
        [Description("保留最新")]
        KeepNewest = 2,

        /// <summary>
        /// 合并记录
        /// </summary>
        [Description("合并记录")]
        Merge = 3
    }

    /// <summary>
    /// 比较运算符枚举
    /// </summary>
    public enum ComparisonOperator
    {
        /// <summary>
        /// 等于
        /// </summary>
        [Description("等于")]
        Equals = 0,

        /// <summary>
        /// 不等于
        /// </summary>
        [Description("不等于")]
        NotEquals = 1,

        /// <summary>
        /// 大于
        /// </summary>
        [Description("大于")]
        GreaterThan = 2,

        /// <summary>
        /// 大于等于
        /// </summary>
        [Description("大于等于")]
        GreaterThanOrEqual = 3,

        /// <summary>
        /// 小于
        /// </summary>
        [Description("小于")]
        LessThan = 4,

        /// <summary>
        /// 小于等于
        /// </summary>
        [Description("小于等于")]
        LessThanOrEqual = 5,

        /// <summary>
        /// 包含
        /// </summary>
        [Description("包含")]
        Contains = 6,

        /// <summary>
        /// 不包含
        /// </summary>
        [Description("不包含")]
        NotContains = 7,

        /// <summary>
        /// 开头是
        /// </summary>
        [Description("开头是")]
        StartsWith = 8,

        /// <summary>
        /// 结尾是
        /// </summary>
        [Description("结尾是")]
        EndsWith = 9,

        /// <summary>
        /// 为空
        /// </summary>
        [Description("为空")]
        IsEmpty = 10,

        /// <summary>
        /// 不为空
        /// </summary>
        [Description("不为空")]
        IsNotEmpty = 11,

        /// <summary>
        /// 在列表中
        /// </summary>
        [Description("在列表中")]
        InList = 12,

        /// <summary>
        /// 不在列表中
        /// </summary>
        [Description("不在列表中")]
        NotInList = 13
    }

    /// <summary>
    /// 逻辑运算符枚举
    /// </summary>
    public enum LogicalOperator
    {
        /// <summary>
        /// 并且
        /// </summary>
        [Description("并且")]
        And = 0,

        /// <summary>
        /// 或者
        /// </summary>
        [Description("或者")]
        Or = 1
    }

    /// <summary>
    /// 条件项配置
    /// 用于自定义条件清理规则
    /// </summary>
    [Serializable]
    public class ConditionItem
    {
        /// <summary>
        /// 字段名
        /// </summary>
        [XmlElement("FieldName")]
        public string FieldName { get; set; }

        /// <summary>
        /// 字段显示名称
        /// </summary>
        [XmlElement("FieldDisplayName")]
        public string FieldDisplayName { get; set; }

        /// <summary>
        /// 比较运算符
        /// </summary>
        [XmlElement("Operator")]
        public ComparisonOperator Operator { get; set; }

        /// <summary>
        /// 比较值
        /// </summary>
        [XmlElement("Value")]
        public string Value { get; set; }

        /// <summary>
        /// 值列表（用于InList/NotInList操作符）
        /// </summary>
        [XmlArray("ValueList")]
        [XmlArrayItem("Item")]
        public List<string> ValueList { get; set; }

        /// <summary>
        /// 与下一个条件的逻辑关系
        /// </summary>
        [XmlElement("LogicalOperator")]
        public LogicalOperator LogicalOperator { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ConditionItem()
        {
            ValueList = new List<string>();
            LogicalOperator = LogicalOperator.And;
        }
    }

    /// <summary>
    /// 数据清理规则配置类
    /// </summary>
    [Serializable]
    public class CleanupRule
    {
        /// <summary>
        /// 规则唯一标识
        /// </summary>
        [XmlElement("RuleId")]
        public string RuleId { get; set; }

        /// <summary>
        /// 规则名称
        /// </summary>
        [XmlElement("RuleName")]
        public string RuleName { get; set; }

        /// <summary>
        /// 规则描述
        /// </summary>
        [XmlElement("Description")]
        public string Description { get; set; }

        /// <summary>
        /// 规则类型
        /// </summary>
        [XmlElement("RuleType")]
        public CleanupRuleType RuleType { get; set; }

        /// <summary>
        /// 清理操作类型
        /// </summary>
        [XmlElement("ActionType")]
        public CleanupActionType ActionType { get; set; }

        /// <summary>
        /// 目标实体类型名称
        /// </summary>
        [XmlElement("TargetEntityType")]
        public string TargetEntityType { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [XmlElement("IsEnabled")]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 执行顺序
        /// </summary>
        [XmlElement("ExecutionOrder")]
        public int ExecutionOrder { get; set; }

        // === 重复数据清理配置 ===

        /// <summary>
        /// 重复判断字段列表
        /// </summary>
        [XmlArray("DuplicateCheckFields")]
        [XmlArrayItem("Field")]
        public List<string> DuplicateCheckFields { get; set; }

        /// <summary>
        /// 重复数据处理策略
        /// </summary>
        [XmlElement("DuplicateStrategy")]
        public DuplicateHandlingStrategy DuplicateStrategy { get; set; }

        // === 空值清理配置 ===

        /// <summary>
        /// 空值检查字段列表
        /// </summary>
        [XmlArray("EmptyCheckFields")]
        [XmlArrayItem("Field")]
        public List<string> EmptyCheckFields { get; set; }

        /// <summary>
        /// 空值判定方式（是否包含空字符串、空白字符等）
        /// </summary>
        [XmlElement("EmptyValueMode")]
        public EmptyValueCheckMode EmptyValueMode { get; set; }

        // === 异常数据清理配置 ===

        /// <summary>
        /// 异常检查字段
        /// </summary>
        [XmlElement("AbnormalCheckField")]
        public string AbnormalCheckField { get; set; }

        /// <summary>
        /// 异常判定条件
        /// </summary>
        [XmlElement("AbnormalCondition")]
        public string AbnormalCondition { get; set; }

        // === 过期数据清理配置 ===

        /// <summary>
        /// 日期字段名
        /// </summary>
        [XmlElement("DateFieldName")]
        public string DateFieldName { get; set; }

        /// <summary>
        /// 过期天数
        /// </summary>
        [XmlElement("ExpireDays")]
        public int ExpireDays { get; set; }

        /// <summary>
        /// 过期日期基准（创建时间、更新时间等）
        /// </summary>
        [XmlElement("DateBaseType")]
        public DateBaseType DateBaseType { get; set; }

        // === 无效关联数据清理配置 ===

        /// <summary>
        /// 外键字段名
        /// </summary>
        [XmlElement("ForeignKeyField")]
        public string ForeignKeyField { get; set; }

        /// <summary>
        /// 关联表名
        /// </summary>
        [XmlElement("ReferenceTable")]
        public string ReferenceTable { get; set; }

        /// <summary>
        /// 关联表主键字段
        /// </summary>
        [XmlElement("ReferenceKeyField")]
        public string ReferenceKeyField { get; set; }

        // === 自定义条件清理配置 ===

        /// <summary>
        /// 自定义条件列表
        /// </summary>
        [XmlArray("CustomConditions")]
        [XmlArrayItem("Condition")]
        public List<ConditionItem> CustomConditions { get; set; }

        // === 数据标准化配置 ===

        /// <summary>
        /// 标准化字段
        /// </summary>
        [XmlElement("StandardizationField")]
        public string StandardizationField { get; set; }

        /// <summary>
        /// 标准化操作
        /// </summary>
        [XmlElement("StandardizationOperation")]
        public StandardizationOperation StandardizationOperation { get; set; }

        // === 数据截断配置 ===

        /// <summary>
        /// 截断字段
        /// </summary>
        [XmlElement("TruncationField")]
        public string TruncationField { get; set; }

        /// <summary>
        /// 最大长度
        /// </summary>
        [XmlElement("MaxLength")]
        public int MaxLength { get; set; }

        /// <summary>
        /// 截断方式
        /// </summary>
        [XmlElement("TruncationMode")]
        public TruncationMode TruncationMode { get; set; }

        // === 更新字段配置（当ActionType为UpdateField时使用）===

        /// <summary>
        /// 更新字段名
        /// </summary>
        [XmlElement("UpdateFieldName")]
        public string UpdateFieldName { get; set; }

        /// <summary>
        /// 更新字段值
        /// </summary>
        [XmlElement("UpdateFieldValue")]
        public string UpdateFieldValue { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [XmlElement("CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [XmlElement("UpdateTime")]
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CleanupRule()
        {
            RuleId = Guid.NewGuid().ToString("N");
            RuleName = string.Empty;
            Description = string.Empty;
            IsEnabled = true;
            ExecutionOrder = 0;
            DuplicateCheckFields = new List<string>();
            EmptyCheckFields = new List<string>();
            CustomConditions = new List<ConditionItem>();
            CreateTime = DateTime.Now;
            UpdateTime = DateTime.Now;
        }
    }

    /// <summary>
    /// 空值检查模式
    /// </summary>
    public enum EmptyValueCheckMode
    {
        /// <summary>
        /// 仅NULL值
        /// </summary>
        [Description("仅NULL值")]
        NullOnly = 0,

        /// <summary>
        /// NULL或空字符串
        /// </summary>
        [Description("NULL或空字符串")]
        NullOrEmpty = 1,

        /// <summary>
        /// NULL、空字符串或空白字符
        /// </summary>
        [Description("NULL、空字符串或空白字符")]
        NullOrWhiteSpace = 2
    }

    /// <summary>
    /// 日期基准类型
    /// </summary>
    public enum DateBaseType
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        CreateTime = 0,

        /// <summary>
        /// 更新时间
        /// </summary>
        [Description("更新时间")]
        UpdateTime = 1,

        /// <summary>
        /// 指定日期字段
        /// </summary>
        [Description("指定日期字段")]
        CustomField = 2
    }

    /// <summary>
    /// 标准化操作
    /// </summary>
    public enum StandardizationOperation
    {
        /// <summary>
        /// 去除前后空格
        /// </summary>
        [Description("去除前后空格")]
        Trim = 0,

        /// <summary>
        /// 转换为大写
        /// </summary>
        [Description("转换为大写")]
        ToUpper = 1,

        /// <summary>
        /// 转换为小写
        /// </summary>
        [Description("转换为小写")]
        ToLower = 2,

        /// <summary>
        /// 去除所有空格
        /// </summary>
        [Description("去除所有空格")]
        RemoveAllSpaces = 3,

        /// <summary>
        /// 标准化日期格式
        /// </summary>
        [Description("标准化日期格式")]
        StandardizeDate = 4,

        /// <summary>
        /// 标准化数字格式
        /// </summary>
        [Description("标准化数字格式")]
        StandardizeNumber = 5
    }

    /// <summary>
    /// 截断方式
    /// </summary>
    public enum TruncationMode
    {
        /// <summary>
        /// 从右侧截断
        /// </summary>
        [Description("从右侧截断")]
        FromRight = 0,

        /// <summary>
        /// 从左侧截断
        /// </summary>
        [Description("从左侧截断")]
        FromLeft = 1,

        /// <summary>
        /// 中间省略
        /// </summary>
        [Description("中间省略")]
        MiddleEllipsis = 2
    }
}
