using Mapster;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;

namespace RUINORERP.Business.RowLevelAuthService
{
    /// <summary>
    /// 行级权限规则类型枚举
    /// 用于表示系统支持的所有默认行级权限规则
    /// </summary>
    public enum RowLevelAuthRule
    {
        /// <summary>
        /// 仅客户数据
        /// 适用于：销售订单、销售出库单、销售退回单等销售相关单据
        /// </summary>
        OnlyCustomer = 1,

        /// <summary>
        /// 仅供应商数据
        /// 适用于：采购订单、采购入库单、采购退货单等采购相关单据
        /// </summary>
        OnlySupplier = 2,

        /// <summary>
        /// 其他出入库单全部数据
        /// 适用于：其他入库单、其他出库单等特殊出入库单据
        /// </summary>
        AllDataForOtherInOut = 3,

        /// <summary>
        /// 仅收款数据
        /// 适用于：应收款单、收款单等收款相关单据
        /// </summary>
        OnlyReceivable = 4,

        /// <summary>
        /// 仅付款数据
        /// 适用于：应付款单、付款单等付款相关单据
        /// </summary>
        OnlyPayable = 5,

        /// <summary>
        /// 全部数据
        /// 适用于：所有业务类型的默认选项
        /// </summary>
        AllData = 99
    }

    /// <summary>
    /// 业务类型与权限规则关系管理器
    /// 用于管理业务类型与可用权限规则之间的从属关系
    /// </summary>
    public static class BizTypeRuleManager
    {
        private static readonly ConcurrentDictionary<BizType, List<RowLevelAuthRule>> _bizTypeToRulesMap = 
            new ConcurrentDictionary<BizType, List<RowLevelAuthRule>>();

        static BizTypeRuleManager()
        {
            // 初始化业务类型与规则的映射关系
            InitializeBizTypeRuleMap();
        }

        private static void InitializeBizTypeRuleMap()
        {

            // 应收应付相关业务类型 用于对账时要共用。所以用行级权限控制
            RegisterRulesForBizTypes(
                new List<BizType> { BizType.应付款单, BizType.应收款单 },
                new List<RowLevelAuthRule> { RowLevelAuthRule.OnlyCustomer, RowLevelAuthRule.OnlySupplier, RowLevelAuthRule.OnlyReceivable, RowLevelAuthRule.OnlyPayable, RowLevelAuthRule.AllData });

            // 销售相关业务类型
            RegisterRulesForBizTypes(
                new List<BizType> { BizType.销售订单, BizType.销售出库单, BizType.销售退回单 },
                new List<RowLevelAuthRule> { RowLevelAuthRule.OnlyCustomer, RowLevelAuthRule.AllData });

            // 采购相关业务类型
            RegisterRulesForBizTypes(
                new List<BizType> { BizType.采购订单, BizType.采购入库单, BizType.采购退货单 },
                new List<RowLevelAuthRule> { RowLevelAuthRule.OnlySupplier, RowLevelAuthRule.AllData });

            // 其他出入库相关业务类型
            RegisterRulesForBizTypes(
                new List<BizType> { BizType.其他入库单, BizType.其他出库单 },
                new List<RowLevelAuthRule> { RowLevelAuthRule.AllDataForOtherInOut, RowLevelAuthRule.AllData });
        }

        /// <summary>
        /// 为一组业务类型注册可用的规则
        /// </summary>
        /// <param name="bizTypes">业务类型列表</param>
        /// <param name="rules">可用规则列表</param>
        public static void RegisterRulesForBizTypes(List<BizType> bizTypes, List<RowLevelAuthRule> rules)
        {
            foreach (var bizType in bizTypes)
            {
                _bizTypeToRulesMap[bizType] = rules;
            }
        }

        /// <summary>
        /// 获取指定业务类型支持的所有规则
        /// </summary>
        /// <param name="bizType">业务类型</param>
        /// <returns>规则列表</returns>
        public static List<RowLevelAuthRule> GetRulesForBizType(BizType bizType)
        {
            if (_bizTypeToRulesMap.TryGetValue(bizType, out var rules))
            {
                return rules;
            }

            // 默认返回只有全部数据的规则
            return new List<RowLevelAuthRule> { RowLevelAuthRule.AllData };
        }
    }

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
        }

        /// <summary>
        /// 获取指定业务类型的所有可用的默认规则选项
        /// 这里只是在初始化加载时使用。并不能用于 具体的业务中 配置关联权限时
        /// 
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
            switch (rule)
            {
                case RowLevelAuthRule.OnlyCustomer:
                    return "仅客户数据";
                case RowLevelAuthRule.OnlySupplier:
                    return "仅供应商数据";
                case RowLevelAuthRule.AllDataForOtherInOut:
                case RowLevelAuthRule.AllData:
                    return "全部数据";
                case RowLevelAuthRule.OnlyReceivable:
                    return "仅收款数据";
                case RowLevelAuthRule.OnlyPayable:
                    return "仅付款数据";
                default:
                    return "未知规则";
            }
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

            // 将long类型的Key转换为RowLevelAuthRule枚举
            try
            {
                // 先检查optionKey是否可以安全地转换为int（枚举的底层类型）
                if (optionKey >= int.MinValue && optionKey <= int.MaxValue)
                {
                    int intOptionKey = Convert.ToInt32(optionKey);
                    // 现在检查这个整数值是否在枚举定义中
                    if (Enum.IsDefined(typeof(RowLevelAuthRule), intOptionKey))
                    {
                        var rule = (RowLevelAuthRule)intOptionKey;
                        ConfigurePolicyFilterByRule(policy, rule, tableName);
                        return;
                    }
                    else
                    {
                        _logger.LogWarning("值 {OptionKey} 不在RowLevelAuthRule枚举定义中", optionKey);
                    }
                }
                else
                {
                    _logger.LogWarning("值 {OptionKey} 超出int范围，无法转换为RowLevelAuthRule枚举", optionKey);
                }
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
            switch (rule)
            {
                case RowLevelAuthRule.OnlyCustomer:
                    policy.IsJoinRequired = true;
                    policy.JoinTable = "tb_CustomerVendor";
                    policy.JoinType = "INNER";
                    policy.JoinOnClause = $"{tableName}.CustomerVendor_ID = tb_CustomerVendor.Id";
                    policy.FilterClause = "tb_CustomerVendor.Type = '客户'";
                    break;

                case RowLevelAuthRule.OnlySupplier:
                    policy.IsJoinRequired = true;
                    policy.JoinTable = "tb_CustomerVendor";
                    policy.JoinType = "INNER";
                    policy.JoinOnClause = $"{tableName}.CustomerVendor_ID = tb_CustomerVendor.Id";
                    policy.FilterClause = "tb_CustomerVendor.Type = '供应商'";
                    break;

                case RowLevelAuthRule.OnlyReceivable:
                    policy.IsJoinRequired = false;
                    // 对于应收款单表，过滤出收款类型的数据
                    policy.FilterClause = "ReceivePaymentType = 1";
                    break;

                case RowLevelAuthRule.OnlyPayable:
                    policy.IsJoinRequired = false;
                    // 对于应付款单表，过滤出付款类型的数据
                    policy.FilterClause = "ReceivePaymentType = 2";
                    break;

                case RowLevelAuthRule.AllDataForOtherInOut:
                case RowLevelAuthRule.AllData:
                default:
                    policy.IsJoinRequired = false;
                    policy.FilterClause = "1=1"; // 全部数据
                    break;
            }
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
