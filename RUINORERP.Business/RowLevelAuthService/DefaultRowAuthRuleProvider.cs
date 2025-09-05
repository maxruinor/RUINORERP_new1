using Mapster;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RUINORERP.Business.RowLevelAuthService
{
    /// <summary>
    /// 默认行级权限规则提供者实现
    /// 提供基于业务类型的默认规则选项和创建权限策略的功能
    /// </summary>
    public class DefaultRowAuthRuleProvider : IDefaultRowAuthRuleProvider
    {
        private readonly IEntityInfoService _entityBizMappingService;
        private readonly ApplicationContext _context;
        private readonly ILogger<DefaultRowAuthRuleProvider> _logger;
        private readonly Dictionary<RowLevelAuthRule, string> _ruleDescriptions;
        private readonly Dictionary<RowLevelAuthRule, RuleConfiguration> _ruleConfigurations;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="entityBizMappingService">实体业务映射服务</param>
        /// <param name="context">应用程序上下文</param>
        /// <param name="logger">日志记录器</param>
        public DefaultRowAuthRuleProvider(
            IEntityInfoService entityBizMappingService,
            ApplicationContext context,
            ILogger<DefaultRowAuthRuleProvider> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _entityBizMappingService = entityBizMappingService ?? throw new ArgumentNullException(nameof(entityBizMappingService));

            // 初始化规则描述映射
            _ruleDescriptions = new Dictionary<RowLevelAuthRule, string>
            {
                { RowLevelAuthRule.OnlyCustomer, "只能查看和处理与客户相关的单据" },
                { RowLevelAuthRule.OnlySupplier, "只能查看和处理与供应商相关的单据" },
                { RowLevelAuthRule.AllDataForOtherInOut, "可以查看和处理所有数据" },
                { RowLevelAuthRule.OnlyReceivable, "只能查看和处理收款相关的数据" },
                { RowLevelAuthRule.OnlyPayable, "只能查看和处理付款相关的数据" },
                { RowLevelAuthRule.AllData, "可以查看和处理所有数据" }
            };
            
            // 初始化规则配置映射
            _ruleConfigurations = new Dictionary<RowLevelAuthRule, RuleConfiguration>
            {
                { 
                    RowLevelAuthRule.OnlyCustomer, 
                    new RuleConfiguration {
                        IsJoinRequired = true,
                        JoinTable = "tb_CustomerVendor",
                        JoinType = "INNER",
                        JoinOnClauseTemplate = "{0}.CustomerVendor_ID = tb_CustomerVendor.CustomerVendor_ID",
                        TargetTableJoinField = "CustomerVendor_ID",
                        JoinTableJoinField = "CustomerVendor_ID",
                        FilterClause = "tb_CustomerVendor.IsCustomer = 1",
                        Name = "仅客户数据",
                        Description = _ruleDescriptions[RowLevelAuthRule.OnlyCustomer]
                    }
                },
                { 
                    RowLevelAuthRule.OnlySupplier, 
                    new RuleConfiguration {
                        IsJoinRequired = true,
                        JoinTable = "tb_CustomerVendor",
                        JoinType = "INNER",
                        JoinOnClauseTemplate = "{0}.CustomerVendor_ID = tb_CustomerVendor.CustomerVendor_ID",
                        TargetTableJoinField = "CustomerVendor_ID",
                        JoinTableJoinField = "CustomerVendor_ID",
                        FilterClause = "tb_CustomerVendor.IsVendor = 1",
                        Name = "仅供应商数据",
                        Description = _ruleDescriptions[RowLevelAuthRule.OnlySupplier]
                    }
                },
                { 
                    RowLevelAuthRule.OnlyReceivable, 
                    new RuleConfiguration {
                        IsJoinRequired = false,
                        FilterClause = "ReceivePaymentType = 1",
                        Name = "仅收款数据",
                        Description = _ruleDescriptions[RowLevelAuthRule.OnlyReceivable]
                    }
                },
                { 
                    RowLevelAuthRule.OnlyPayable, 
                    new RuleConfiguration {
                        IsJoinRequired = false,
                        FilterClause = "ReceivePaymentType = 2",
                        Name = "仅付款数据",
                        Description = _ruleDescriptions[RowLevelAuthRule.OnlyPayable]
                    }
                },
                { 
                    RowLevelAuthRule.AllDataForOtherInOut, 
                    CreateAllDataConfig("全部数据", _ruleDescriptions[RowLevelAuthRule.AllDataForOtherInOut])
                },
                { 
                    RowLevelAuthRule.AllData, 
                    CreateAllDataConfig("全部数据", _ruleDescriptions[RowLevelAuthRule.AllData])
                }
            };
        }

        /// <summary>
        /// 获取指定业务类型的所有可用的默认规则选项
        /// 这里只是在初始化加载时使用。并不能用于 具体的业务中 配置关联权限时
        /// </summary>
        /// <param name="bizType">业务类型</param>
        /// <returns>默认规则选项列表</returns>
        public List<DefaultRuleOption> GetDefaultRuleOptions(BizType bizType)
        {
            var options = new List<DefaultRuleOption>();

            try
            {
                // 获取该业务类型支持的所有规则
                var supportedRules = BizTypeRuleManager.GetRulesForBizType(bizType);

                // 将规则转换为DefaultRuleOption
                foreach (var rule in supportedRules)
                {
                    options.Add(new DefaultRuleOption
                    {
                        Key = (long)rule,
                        Name = GetRuleName(rule),
                        Description = GetRuleDescription(rule)
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取默认规则选项失败: {BizType}", bizType);
                // 返回默认的"全部数据"选项作为备用
                return new List<DefaultRuleOption>
                {
                    new DefaultRuleOption
                    {
                        Key = (long)RowLevelAuthRule.AllData,
                        Name = "全部数据",
                        Description = "可以查看和处理所有数据"
                    }
                };
            }

            return options;
        }

        /// <summary>
        /// 获取规则的名称
        /// </summary>
        /// <param name="rule">规则枚举值</param>
        /// <returns>规则名称</returns>
        private string GetRuleName(RowLevelAuthRule rule)
        {
            if (_ruleConfigurations.TryGetValue(rule, out var config))
            {
                return config.Name;
            }
            
            return rule.ToString();
        }

        /// <summary>
        /// 获取规则的描述
        /// </summary>
        /// <param name="rule">规则枚举值</param>
        /// <returns>规则描述</returns>
        private string GetRuleDescription(RowLevelAuthRule rule)
        {
            if (_ruleDescriptions.TryGetValue(rule, out var description))
            {
                return description;
            }
            return "未知规则描述";
        }
        
        /// <summary>
        /// 注册新的行级权限规则配置
        /// </summary>
        /// <param name="rule">规则枚举值</param>
        /// <param name="config">规则配置</param>
        public void RegisterRuleConfiguration(RowLevelAuthRule rule, RuleConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            
            // 验证配置有效性
            if (!config.IsValid())
            {
                throw new ArgumentException("规则配置无效", nameof(config));
            }
            
            _ruleConfigurations[rule] = config;
            
            // 如果是新规则，同时添加描述
            if (!_ruleDescriptions.ContainsKey(rule))
            {
                _ruleDescriptions[rule] = config.Description;
            }
            
            _logger.LogInformation("已注册新的行级权限规则配置: {RuleName}", config.Name);
        }

        /// <summary>
        /// 根据默认规则选项生成完整的行级权限策略对象
        /// </summary>
        /// <param name="bizType">业务类型</param>
        /// <param name="option">选择的默认规则选项</param>
        /// <param name="roleId">要应用的角色ID</param>
        /// <returns>可用于保存的RowAuthPolicy对象</returns>
        /// <exception cref="ArgumentNullException">当参数为空时抛出</exception>
        /// <exception cref="Exception">当找不到实体映射时抛出</exception>
        public tb_RowAuthPolicy CreatePolicyFromDefaultOption(BizType bizType, DefaultRuleOption option, long roleId)
        {
            if (option == null)
            {
                throw new ArgumentNullException(nameof(option), "规则选项不能为空");
            }
            var entityInfoService = _context.GetRequiredService<IEntityInfoService>();
            try
            {
                // 通过映射服务获取实体信息
                var entityInfo = _entityBizMappingService.GetEntityInfo(bizType);
                
                if (entityInfo == null)
                {
                    var errorMessage = $"未找到业务类型 {bizType} 对应的实体映射";
                    _logger.LogError(errorMessage);
                    throw new Exception(errorMessage);
                }

                var policy = new tb_RowAuthPolicy
                {
                    PolicyName = $"{entityInfo.EntityName}-{option.Name}",
                    TargetTable = entityInfo.TableName, // 设置必填的TargetTable字段
                    TargetEntity = entityInfo.EntityName,
                    EntityType = entityInfo.EntityType.FullName,
                    IsEnabled = true, // 默认启用策略
                    PolicyDescription = option.Description, // 设置策略描述
                    Created_at = DateTime.Now,
                    Created_by = GetCurrentUserId()
                };

                // 根据选项Key设置不同的过滤条件
                ConfigurePolicyFilter(policy, option.Key, entityInfo.TableName);

                return policy;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException || ex is ArgumentException)
                {
                    throw;
                }

                _logger.LogError(ex, "创建行级权限策略失败: {OptionKey}, {BizType}", option.Key, bizType);
                throw;
            }
        }

        /// <summary>
        /// 根据选项Key配置策略的过滤条件
        /// </summary>
        /// <param name="policy">权限策略对象</param>
        /// <param name="optionKey">选项Key</param>
        /// <param name="tableName">实体表名</param>
        private void ConfigurePolicyFilter(tb_RowAuthPolicy policy, long optionKey, string tableName)
        {
            // 确保policy不为null
            if (policy == null)
            {
                _logger.LogWarning("配置策略过滤条件时，policy参数为null");
                return;
            }

            // 确保tableName不为空
            if (string.IsNullOrEmpty(tableName))
            {
                _logger.LogWarning("配置策略过滤条件时，tableName参数为空，使用默认值");
                tableName = "";
            }

            // 优化：使用泛型版本的Enum.TryParse，这是类型安全且简洁的方式
                try
                {
                    if (Enum.TryParse(optionKey.ToString(), out RowLevelAuthRule rule))
                    {
                        ConfigurePolicyFilterByRule(policy, rule, tableName);
                        return;
                    }
                    _logger.LogWarning("值 {OptionKey} 无法转换为RowLevelAuthRule枚举", optionKey);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "转换规则Key值时发生异常: {OptionKey}", optionKey);
                }
            
            // 默认设置为全部数据
            policy.IsJoinRequired = false;
            policy.FilterClause = "1=1"; // 全部数据
        }

        /// <summary>
        /// 根据规则枚举配置策略的过滤条件
        /// </summary>
        /// <param name="policy">权限策略对象</param>
        /// <param name="rule">规则枚举值</param>
        /// <param name="tableName">实体表名</param>
        private void ConfigurePolicyFilterByRule(tb_RowAuthPolicy policy, RowLevelAuthRule rule, string tableName)
        {
            if (_ruleConfigurations.TryGetValue(rule, out var config))
            {
                policy.IsJoinRequired = config.IsJoinRequired;
                
                if (config.IsJoinRequired)
                {
                    policy.JoinTable = config.JoinTable;
                    policy.JoinType = config.JoinType;
                    policy.JoinOnClause = config.GenerateJoinOnClause(tableName);
                    // 添加关联字段信息
                    policy.TargetTableJoinField = config.TargetTableJoinField;
                    policy.JoinTableJoinField = config.JoinTableJoinField;
                }
                
                policy.FilterClause = config.FilterClause;
                
                _logger.LogDebug("已为策略配置过滤条件: {RuleName}", config.Name);
            }
            else
            {
                // 默认设置为全部数据
                policy.IsJoinRequired = false;
                policy.FilterClause = "1=1"; // 全部数据
                _logger.LogWarning("未找到规则的配置: {Rule}", rule);
            }
        }
        
        /// <summary>
        /// 创建全数据访问权限的规则配置
        /// </summary>
        /// <param name="name">规则名称</param>
        /// <param name="description">规则描述</param>
        /// <returns>全数据访问权限的规则配置</returns>
        private RuleConfiguration CreateAllDataConfig(string name, string description)
        {
            return new RuleConfiguration
            {
                IsJoinRequired = false,
                FilterClause = "1=1",
                Name = name,
                Description = description
            };
        }

        /// <summary>
        /// 验证规则配置的有效性
        /// </summary>
        /// <param name="config">规则配置</param>
        /// <returns>配置是否有效</returns>
        private bool ValidateRuleConfiguration(RuleConfiguration config)
        {
            if (config == null)
                return false;
            
            return config.IsValid();
        }

        /// <summary>
        /// 获取当前用户ID
        /// </summary>
        /// <returns>当前用户ID</returns>
        private long GetCurrentUserId()
        {
            try
            {
                return _context.CurUserInfo?.Id ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "获取当前用户ID失败，使用默认值0");
                return 0;
            }
        }
    }
}
