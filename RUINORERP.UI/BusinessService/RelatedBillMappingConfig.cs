using System;
using System.Collections.Generic;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.PacketSpec.Enums.Core;

namespace RUINORERP.UI.BusinessService
{
    /// <summary>
    /// 关联单据映射配置
    /// 定义下游单据到上游单据的关联关系,用于工作台待办事项同步
    /// </summary>
    public static class RelatedBillMappingConfig
    {
        /// <summary>
        /// 关联关系定义
        /// Key: 下游单据类型(触发更新的单据)
        /// Value: 上游单据列表(需要被更新待办的单据)
        /// </summary>
        public static Dictionary<BizType, List<UpstreamBillRelation>> Relations { get; } 
            = new Dictionary<BizType, List<UpstreamBillRelation>>
        {
            // ==================== 销售模块 ====================
            
            // 销售出库单 → 销售订单
            { 
                BizType.销售出库单, 
                new List<UpstreamBillRelation> 
                { 
                    new UpstreamBillRelation 
                    { 
                        UpstreamBizType = BizType.销售订单, 
                        ForeignKeyField = "SOrder_ID",
                        Description = "销售出库单审核后,更新销售订单的待出库状态"
                    }
                } 
            },
            
            // 销售退回单 → 销售订单 (可选,根据业务需求)
            { 
                BizType.销售退回单, 
                new List<UpstreamBillRelation> 
                { 
                    new UpstreamBillRelation 
                    { 
                        UpstreamBizType = BizType.销售订单, 
                        ForeignKeyField = "SOrder_ID",
                        Description = "销售退回单审核后,更新销售订单的退货状态"
                    }
                } 
            },
            
            // ==================== 采购模块 ====================
            
            // 采购入库单 → 采购订单
            { 
                BizType.采购入库单, 
                new List<UpstreamBillRelation> 
                { 
                    new UpstreamBillRelation 
                    { 
                        UpstreamBizType = BizType.采购订单, 
                        ForeignKeyField = "PurOrder_ID",
                        Description = "采购入库单审核后,更新采购订单的待入库状态"
                    }
                } 
            },
            
            // 采购退货单 → 采购订单 (可选)
            { 
                BizType.采购退货单, 
                new List<UpstreamBillRelation> 
                { 
                    new UpstreamBillRelation 
                    { 
                        UpstreamBizType = BizType.采购订单, 
                        ForeignKeyField = "PurOrder_ID",
                        Description = "采购退货单审核后,更新采购订单的退货状态"
                    }
                } 
            },
            
            // ==================== 财务模块 ====================
            
            // 收款单 → 应收款单/对账单
            { 
                BizType.收款单, 
                new List<UpstreamBillRelation> 
                { 
                    new UpstreamBillRelation 
                    { 
                        UpstreamBizType = BizType.应收款单, 
                        ForeignKeyField = null, // 需要通过中间表查询
                        RequiresSpecialHandling = true,
                        Description = "收款单审核后,更新应收款单的待收款状态"
                    },
                    new UpstreamBillRelation 
                    { 
                        UpstreamBizType = BizType.对账单, 
                        ForeignKeyField = null,
                        RequiresSpecialHandling = true,
                        Description = "收款单审核后,更新对账单的收款状态"
                    }
                } 
            },
            
            // 付款单 → 应付款单
            { 
                BizType.付款单, 
                new List<UpstreamBillRelation> 
                { 
                    new UpstreamBillRelation 
                    { 
                        UpstreamBizType = BizType.应付款单, 
                        ForeignKeyField = null,
                        RequiresSpecialHandling = true,
                        Description = "付款单审核后,更新应付款单的待付款状态"
                    }
                } 
            },
            
            // 预收款单核销 → 应收款单
            { 
                BizType.预收款单, 
                new List<UpstreamBillRelation> 
                { 
                    new UpstreamBillRelation 
                    { 
                        UpstreamBizType = BizType.应收款单, 
                        ForeignKeyField = null,
                        RequiresSpecialHandling = true,
                        Description = "预收款单核销后,更新应收款单的核销状态"
                    }
                } 
            },
            
            // 预付款单核销 → 应付款单
            { 
                BizType.预付款单, 
                new List<UpstreamBillRelation> 
                { 
                    new UpstreamBillRelation 
                    { 
                        UpstreamBizType = BizType.应付款单, 
                        ForeignKeyField = null,
                        RequiresSpecialHandling = true,
                        Description = "预付款单核销后,更新应付款单的核销状态"
                    }
                } 
            },
        };
        
        /// <summary>
        /// 获取指定下游单据类型的上游关联关系
        /// </summary>
        /// <param name="downstreamBizType">下游单据类型</param>
        /// <returns>上游关联关系列表,如果不存在返回空列表</returns>
        public static List<UpstreamBillRelation> GetUpstreamRelations(BizType downstreamBizType)
        {
            if (Relations.TryGetValue(downstreamBizType, out var relations))
            {
                return relations;
            }
            return new List<UpstreamBillRelation>();
        }
        
        /// <summary>
        /// 检查是否存在关联关系
        /// </summary>
        /// <param name="downstreamBizType">下游单据类型</param>
        /// <returns>是否存在关联关系</returns>
        public static bool HasRelations(BizType downstreamBizType)
        {
            return Relations.ContainsKey(downstreamBizType);
        }
    }
    
    /// <summary>
    /// 上游单据关联关系定义
    /// </summary>
    public class UpstreamBillRelation
    {
        /// <summary>
        /// 上游单据业务类型
        /// </summary>
        public BizType UpstreamBizType { get; set; }
        
        /// <summary>
        /// 外键字段名(下游单据中指向主单据的字段)
        /// 如果为null,表示需要通过中间表或特殊逻辑查询
        /// </summary>
        public string ForeignKeyField { get; set; }
        
        /// <summary>
        /// 是否需要特殊处理(如通过中间表查询)
        /// </summary>
        public bool RequiresSpecialHandling { get; set; } = false;
        
        /// <summary>
        /// 关联关系描述
        /// </summary>
        public string Description { get; set; }
        
        public override string ToString()
        {
            return $"{Description} (外键:{ForeignKeyField ?? "特殊处理"})";
        }
    }
}
