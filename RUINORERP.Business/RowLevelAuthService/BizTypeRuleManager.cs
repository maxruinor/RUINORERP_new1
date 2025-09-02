using RUINORERP.Global;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace RUINORERP.Business.RowLevelAuthService
{
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

        /// <summary>
        /// 初始化业务类型与规则的映射关系
        /// </summary>
        private static void InitializeBizTypeRuleMap()
        {
            // 应收应付相关业务类型 用于对账时要共用。所以用行级权限控制
            RegisterRulesForBizTypes(
                new List<BizType> { BizType.应付款单, BizType.应收款单 },
                new List<RowLevelAuthRule> { 
                    RowLevelAuthRule.OnlyCustomer, 
                    RowLevelAuthRule.OnlySupplier, 
                    RowLevelAuthRule.OnlyReceivable, 
                    RowLevelAuthRule.OnlyPayable, 
                    RowLevelAuthRule.AllData 
                });

            // 销售相关业务类型
            RegisterRulesForBizTypes(
                new List<BizType> { BizType.销售订单, BizType.销售出库单, BizType.销售退回单 },
                new List<RowLevelAuthRule> { 
                    RowLevelAuthRule.OnlyCustomer, 
                    RowLevelAuthRule.AllData 
                });

            // 采购相关业务类型
            RegisterRulesForBizTypes(
                new List<BizType> { BizType.采购订单, BizType.采购入库单, BizType.采购退货单 },
                new List<RowLevelAuthRule> { 
                    RowLevelAuthRule.OnlySupplier, 
                    RowLevelAuthRule.AllData 
                });

            // 其他出入库相关业务类型
            RegisterRulesForBizTypes(
                new List<BizType> { BizType.其他入库单, BizType.其他出库单 },
                new List<RowLevelAuthRule> { 
                    RowLevelAuthRule.AllDataForOtherInOut, 
                    RowLevelAuthRule.AllData 
                });
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

        /// <summary>
        /// 检查指定业务类型是否支持某个规则
        /// </summary>
        /// <param name="bizType">业务类型</param>
        /// <param name="rule">要检查的规则</param>
        /// <returns>是否支持该规则</returns>
        public static bool IsRuleSupportedForBizType(BizType bizType, RowLevelAuthRule rule)
        {
            var supportedRules = GetRulesForBizType(bizType);
            return supportedRules.Contains(rule);
        }
    }
}