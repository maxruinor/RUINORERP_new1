using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace RUINORERP.UI.SysConfig.BasicDataCleanup
{
    /// <summary>
    /// 数据清理方案配置类
    /// 管理一组清理规则的集合
    /// </summary>
    [Serializable]
    [XmlRoot("CleanupConfiguration")]
    public class CleanupConfiguration
    {
        /// <summary>
        /// 配置唯一标识
        /// </summary>
        [XmlElement("ConfigId")]
        public string ConfigId { get; set; }

        /// <summary>
        /// 配置名称
        /// </summary>
        [XmlElement("ConfigName")]
        public string ConfigName { get; set; }

        /// <summary>
        /// 配置描述
        /// </summary>
        [XmlElement("Description")]
        public string Description { get; set; }

        /// <summary>
        /// 目标实体类型名称
        /// </summary>
        [XmlElement("TargetEntityType")]
        public string TargetEntityType { get; set; }

        /// <summary>
        /// 目标表名
        /// </summary>
        [XmlElement("TargetTable")]
        public string TargetTable { get; set; }

        /// <summary>
        /// 清理规则列表
        /// </summary>
        [XmlArray("CleanupRules")]
        [XmlArrayItem("CleanupRule")]
        public List<CleanupRule> CleanupRules { get; set; }

        /// <summary>
        /// 是否启用事务
        /// </summary>
        [XmlElement("EnableTransaction")]
        public bool EnableTransaction { get; set; }

        /// <summary>
        /// 事务批量大小（每处理多少条提交一次）
        /// </summary>
        [XmlElement("TransactionBatchSize")]
        public int TransactionBatchSize { get; set; }

        /// <summary>
        /// 是否在执行前备份数据
        /// </summary>
        [XmlElement("EnableBackup")]
        public bool EnableBackup { get; set; }

        /// <summary>
        /// 备份表名后缀
        /// </summary>
        [XmlElement("BackupTableSuffix")]
        public string BackupTableSuffix { get; set; }

        /// <summary>
        /// 是否记录详细日志
        /// </summary>
        [XmlElement("EnableDetailedLog")]
        public bool EnableDetailedLog { get; set; }

        /// <summary>
        /// 是否允许测试模式（不实际执行删除）
        /// </summary>
        [XmlElement("AllowTestMode")]
        public bool AllowTestMode { get; set; }

        /// <summary>
        /// 最大处理记录数（0表示无限制）
        /// </summary>
        [XmlElement("MaxProcessCount")]
        public int MaxProcessCount { get; set; }

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
        /// 配置版本
        /// </summary>
        [XmlElement("Version")]
        public string Version { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [XmlElement("CreatedBy")]
        public string CreatedBy { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        [XmlElement("ModifiedBy")]
        public string ModifiedBy { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CleanupConfiguration()
        {
            ConfigId = Guid.NewGuid().ToString("N");
            ConfigName = string.Empty;
            Description = string.Empty;
            CleanupRules = new List<CleanupRule>();
            EnableTransaction = true;
            TransactionBatchSize = 1000;
            EnableBackup = true;
            BackupTableSuffix = "_Backup_" + DateTime.Now.ToString("yyyyMMdd");
            EnableDetailedLog = true;
            AllowTestMode = true;
            MaxProcessCount = 0;
            CreateTime = DateTime.Now;
            UpdateTime = DateTime.Now;
            Version = "1.0";
        }

        /// <summary>
        /// 获取已启用的规则列表（按执行顺序排序）
        /// </summary>
        /// <returns>已启用的规则列表</returns>
        public List<CleanupRule> GetEnabledRules()
        {
            var enabledRules = new List<CleanupRule>();
            foreach (var rule in CleanupRules)
            {
                if (rule.IsEnabled)
                {
                    enabledRules.Add(rule);
                }
            }
            enabledRules.Sort((a, b) => a.ExecutionOrder.CompareTo(b.ExecutionOrder));
            return enabledRules;
        }

        /// <summary>
        /// 添加清理规则
        /// </summary>
        /// <param name="rule">清理规则</param>
        public void AddRule(CleanupRule rule)
        {
            if (rule == null)
            {
                throw new ArgumentNullException(nameof(rule));
            }

            // 自动设置执行顺序
            if (rule.ExecutionOrder == 0)
            {
                int maxOrder = 0;
                foreach (var r in CleanupRules)
                {
                    if (r.ExecutionOrder > maxOrder)
                    {
                        maxOrder = r.ExecutionOrder;
                    }
                }
                rule.ExecutionOrder = maxOrder + 1;
            }

            CleanupRules.Add(rule);
            UpdateTime = DateTime.Now;
        }

        /// <summary>
        /// 移除清理规则
        /// </summary>
        /// <param name="ruleId">规则ID</param>
        /// <returns>是否成功移除</returns>
        public bool RemoveRule(string ruleId)
        {
            for (int i = CleanupRules.Count - 1; i >= 0; i--)
            {
                if (CleanupRules[i].RuleId == ruleId)
                {
                    CleanupRules.RemoveAt(i);
                    UpdateTime = DateTime.Now;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 根据ID获取规则
        /// </summary>
        /// <param name="ruleId">规则ID</param>
        /// <returns>清理规则，未找到返回null</returns>
        public CleanupRule GetRuleById(string ruleId)
        {
            foreach (var rule in CleanupRules)
            {
                if (rule.RuleId == ruleId)
                {
                    return rule;
                }
            }
            return null;
        }

        /// <summary>
        /// 更新规则
        /// </summary>
        /// <param name="rule">清理规则</param>
        /// <returns>是否成功更新</returns>
        public bool UpdateRule(CleanupRule rule)
        {
            if (rule == null)
            {
                return false;
            }

            for (int i = 0; i < CleanupRules.Count; i++)
            {
                if (CleanupRules[i].RuleId == rule.RuleId)
                {
                    CleanupRules[i] = rule;
                    CleanupRules[i].UpdateTime = DateTime.Now;
                    UpdateTime = DateTime.Now;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 调整规则执行顺序
        /// </summary>
        /// <param name="ruleId">规则ID</param>
        /// <param name="newOrder">新顺序</param>
        /// <returns>是否成功调整</returns>
        public bool ReorderRule(string ruleId, int newOrder)
        {
            var rule = GetRuleById(ruleId);
            if (rule == null)
            {
                return false;
            }

            rule.ExecutionOrder = newOrder;

            // 重新排序其他规则
            var sortedRules = new List<CleanupRule>(CleanupRules);
            sortedRules.Sort((a, b) => a.ExecutionOrder.CompareTo(b.ExecutionOrder));

            // 重新分配顺序号
            for (int i = 0; i < sortedRules.Count; i++)
            {
                sortedRules[i].ExecutionOrder = i + 1;
            }

            CleanupRules = sortedRules;
            UpdateTime = DateTime.Now;
            return true;
        }

        /// <summary>
        /// 验证配置是否有效
        /// </summary>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>是否有效</returns>
        public bool Validate(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(ConfigName))
            {
                errorMessage = "配置名称不能为空";
                return false;
            }

            if (string.IsNullOrWhiteSpace(TargetEntityType))
            {
                errorMessage = "目标实体类型不能为空";
                return false;
            }

            if (CleanupRules == null || CleanupRules.Count == 0)
            {
                errorMessage = "至少需要配置一条清理规则";
                return false;
            }

            // 检查是否有启用的规则
            bool hasEnabledRule = false;
            foreach (var rule in CleanupRules)
            {
                if (rule.IsEnabled)
                {
                    hasEnabledRule = true;
                    break;
                }
            }

            if (!hasEnabledRule)
            {
                errorMessage = "至少需要启用一条清理规则";
                return false;
            }

            return true;
        }

        /// <summary>
        /// 创建配置副本
        /// </summary>
        /// <param name="newName">新配置名称</param>
        /// <returns>配置副本</returns>
        public CleanupConfiguration Clone(string newName)
        {
            var clone = new CleanupConfiguration
            {
                ConfigId = Guid.NewGuid().ToString("N"),
                ConfigName = newName,
                Description = this.Description,
                TargetEntityType = this.TargetEntityType,
                TargetTable = this.TargetTable,
                EnableTransaction = this.EnableTransaction,
                TransactionBatchSize = this.TransactionBatchSize,
                EnableBackup = this.EnableBackup,
                BackupTableSuffix = this.BackupTableSuffix,
                EnableDetailedLog = this.EnableDetailedLog,
                AllowTestMode = this.AllowTestMode,
                MaxProcessCount = this.MaxProcessCount,
                Version = this.Version,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };

            // 复制规则
            foreach (var rule in this.CleanupRules)
            {
                var ruleClone = new CleanupRule
                {
                    RuleId = Guid.NewGuid().ToString("N"),
                    RuleName = rule.RuleName,
                    Description = rule.Description,
                    RuleType = rule.RuleType,
                    ActionType = rule.ActionType,
                    TargetEntityType = rule.TargetEntityType,
                    IsEnabled = rule.IsEnabled,
                    ExecutionOrder = rule.ExecutionOrder,
                    DuplicateCheckFields = new List<string>(rule.DuplicateCheckFields ?? new List<string>()),
                    DuplicateStrategy = rule.DuplicateStrategy,
                    EmptyCheckFields = new List<string>(rule.EmptyCheckFields ?? new List<string>()),
                    EmptyValueMode = rule.EmptyValueMode,
                    AbnormalCheckField = rule.AbnormalCheckField,
                    AbnormalCondition = rule.AbnormalCondition,
                    DateFieldName = rule.DateFieldName,
                    ExpireDays = rule.ExpireDays,
                    DateBaseType = rule.DateBaseType,
                    ForeignKeyField = rule.ForeignKeyField,
                    ReferenceTable = rule.ReferenceTable,
                    ReferenceKeyField = rule.ReferenceKeyField,
                    StandardizationField = rule.StandardizationField,
                    StandardizationOperation = rule.StandardizationOperation,
                    TruncationField = rule.TruncationField,
                    MaxLength = rule.MaxLength,
                    TruncationMode = rule.TruncationMode,
                    UpdateFieldName = rule.UpdateFieldName,
                    UpdateFieldValue = rule.UpdateFieldValue,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };

                // 复制自定义条件
                if (rule.CustomConditions != null)
                {
                    foreach (var condition in rule.CustomConditions)
                    {
                        var conditionClone = new ConditionItem
                        {
                            FieldName = condition.FieldName,
                            FieldDisplayName = condition.FieldDisplayName,
                            Operator = condition.Operator,
                            Value = condition.Value,
                            ValueList = new List<string>(condition.ValueList ?? new List<string>()),
                            LogicalOperator = condition.LogicalOperator
                        };
                        ruleClone.CustomConditions.Add(conditionClone);
                    }
                }

                clone.CleanupRules.Add(ruleClone);
            }

            return clone;
        }
    }
}
