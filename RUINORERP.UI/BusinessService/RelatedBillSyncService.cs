using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Business;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Message;
using RUINORERP.UI.UserCenter.DataParts;

namespace RUINORERP.UI.BusinessService
{
    /// <summary>
    /// 关联单据同步服务
    /// 负责处理下游单据状态变更后,自动更新上游单据的工作台待办事项
    /// 
    /// 使用场景:
    /// 1. 销售出库单审核 → 更新销售订单的"待出库"数量
    /// 2. 采购入库单审核 → 更新采购订单的"待入库"数量
    /// 3. 收款单审核 → 更新应收款单的"待收款"状态
    /// 4. 付款单审核 → 更新应付款单的"待付款"状态
    /// </summary>
    public class RelatedBillSyncService : IExcludeFromRegistration
    {
        private readonly ILogger<RelatedBillSyncService> _logger;

        public RelatedBillSyncService(ILogger<RelatedBillSyncService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 处理下游单据状态变更,同步更新上游单据待办
        /// 该方法应在下游单据(如出库单、入库单)审核/结案成功后调用
        /// </summary>
        /// <param name="downstreamUpdate">下游单据的TodoUpdate对象</param>
        /// <returns>是否成功处理</returns>
        public async Task<bool> HandleDownstreamBillStatusChangeAsync(TodoUpdate downstreamUpdate)
        {
            if (downstreamUpdate == null)
            {
                _logger.LogWarning("下游单据更新数据为空,跳过关联同步");
                return false;
            }

            try
            {
                _logger.LogDebug(
                    "开始处理关联单据同步 - 下游单据:{BizType}, 单据ID:{BillId}, 操作类型:{UpdateType}",
                    downstreamUpdate.BusinessType,
                    downstreamUpdate.BillId,
                    downstreamUpdate.UpdateType
                );

                // 检查该下游单据类型是否有配置的上游关联关系
                var upstreamRelations = RelatedBillMappingConfig.GetUpstreamRelations(downstreamUpdate.BusinessType);

                if (upstreamRelations == null || !upstreamRelations.Any())
                {
                    _logger.LogDebug("下游单据{BizType}未配置上游关联关系,跳过同步", downstreamUpdate.BusinessType);
                    return true; // 没有关联关系不算失败
                }

                bool allSuccess = true;

                // 遍历所有上游关联关系
                foreach (var relation in upstreamRelations)
                {
                    try
                    {
                        var success = await ProcessSingleUpstreamRelationAsync(downstreamUpdate, relation);
                        if (!success)
                        {
                            allSuccess = false;
                            _logger.LogWarning(
                                "处理上游关联关系失败 - 下游:{DownstreamBizType}:{DownstreamBillId}, 上游:{UpstreamBizType}",
                                downstreamUpdate.BusinessType,
                                downstreamUpdate.BillId,
                                relation.UpstreamBizType
                            );
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            ex,
                            "处理上游关联关系异常 - 下游:{DownstreamBizType}:{DownstreamBillId}, 上游:{UpstreamBizType}",
                            downstreamUpdate.BusinessType,
                            downstreamUpdate.BillId,
                            relation.UpstreamBizType
                        );
                        allSuccess = false;
                    }
                }

                _logger.LogDebug(
                    "关联单据同步完成 - 下游单据:{BizType}:{BillId}, 结果:{Result}",
                    downstreamUpdate.BusinessType,
                    downstreamUpdate.BillId,
                    allSuccess ? "成功" : "部分失败"
                );

                return allSuccess;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "处理关联单据同步失败 - 下游单据:{BizType}:{BillId}",
                    downstreamUpdate.BusinessType,
                    downstreamUpdate.BillId
                );
                return false;
            }
        }

        /// <summary>
        /// 处理单个上游关联关系
        /// </summary>
        private async Task<bool> ProcessSingleUpstreamRelationAsync(TodoUpdate downstreamUpdate, UpstreamBillRelation relation)
        {
            // 查找关联的上游单据
            var upstreamBills = await FindRelatedUpstreamBillsAsync(downstreamUpdate, relation);

            if (upstreamBills == null || !upstreamBills.Any())
            {
                _logger.LogDebug(
                    "未找到关联的上游单据 - 下游:{DownstreamBizType}:{DownstreamBillId}, 上游:{UpstreamBizType}",
                    downstreamUpdate.BusinessType,
                    downstreamUpdate.BillId,
                    relation.UpstreamBizType
                );
                return true; // 没有找到关联单据不算失败
            }

            bool allSuccess = true;

            // 对每个上游单据重新计算并推送待办更新
            foreach (var upstreamBill in upstreamBills)
            {
                try
                {
                    var upstreamUpdate = await RecalculateUpstreamTodoAsync(upstreamBill, downstreamUpdate, relation);

                    if (upstreamUpdate != null)
                    {
                        // 推送到TodoSyncManager
                        TodoSyncManager.Instance.PublishUpdate(upstreamUpdate);

                        _logger.LogInformation(
                            "关联单据待办已同步 - 上游:{UpstreamType}:{UpstreamId}, 下游:{DownstreamType}:{DownstreamId}, 描述:{Description}",
                            upstreamUpdate.BusinessType,
                            upstreamUpdate.BillId,
                            downstreamUpdate.BusinessType,
                            downstreamUpdate.BillId,
                            upstreamUpdate.OperationDescription
                        );
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "重新计算上游单据待办失败 - 上游单据ID:{UpstreamBillId}",
                        upstreamBill.PrimaryKeyID
                    );
                    allSuccess = false;
                }
            }

            return allSuccess;
        }

        /// <summary>
        /// 查找关联的上游单据
        /// </summary>
        private async Task<List<BaseEntity>> FindRelatedUpstreamBillsAsync(TodoUpdate downstreamUpdate, UpstreamBillRelation relation)
        {
            var relatedBills = new List<BaseEntity>();

            try
            {
                // 如果需要特殊处理(如通过中间表查询)
                if (relation.RequiresSpecialHandling)
                {
                    return await FindUpstreamBillsWithSpecialHandlingAsync(downstreamUpdate, relation);
                }

                // 标准处理: 通过外键字段直接查询
                if (string.IsNullOrEmpty(relation.ForeignKeyField))
                {
                    _logger.LogWarning("关联关系未配置外键字段且未标记为特殊处理 - 上游:{UpstreamBizType}", relation.UpstreamBizType);
                    return relatedBills;
                }

                // 查询下游单据实体,获取外键值
                var downstreamEntity = await QueryBillByIdAsync(downstreamUpdate.BusinessType, downstreamUpdate.BillId);

                if (downstreamEntity == null)
                {
                    _logger.LogWarning("无法查询到下游单据实体 - 类型:{BizType}, ID:{BillId}", downstreamUpdate.BusinessType, downstreamUpdate.BillId);
                    return relatedBills;
                }

                // 获取外键字段的值
                if (!downstreamEntity.ContainsProperty(relation.ForeignKeyField))
                {
                    _logger.LogWarning("下游单据不存在外键字段:{FieldName}", relation.ForeignKeyField);
                    return relatedBills;
                }

                var foreignKeyValue = downstreamEntity.GetPropertyValue(relation.ForeignKeyField);

                if (foreignKeyValue == null || Convert.ToInt64(foreignKeyValue) <= 0)
                {
                    _logger.LogDebug("下游单据外键字段值为空或无效 - 字段:{FieldName}", relation.ForeignKeyField);
                    return relatedBills;
                }

                long upstreamBillId = Convert.ToInt64(foreignKeyValue);

                // 查询上游单据
                var upstreamEntity = await QueryBillByIdAsync(relation.UpstreamBizType, upstreamBillId);

                if (upstreamEntity != null)
                {
                    relatedBills.Add(upstreamEntity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "查找关联上游单据失败");
            }

            return relatedBills;
        }

        /// <summary>
        /// 特殊处理: 通过中间表或复杂逻辑查找上游单据
        /// </summary>
        private async Task<List<BaseEntity>> FindUpstreamBillsWithSpecialHandlingAsync(TodoUpdate downstreamUpdate, UpstreamBillRelation relation)
        {
            var relatedBills = new List<BaseEntity>();

            // TODO: 根据具体业务需求实现特殊处理逻辑
            // 例如: 收款单 → 应收款单需要通过收款明细表查询

            _logger.LogDebug("执行特殊处理逻辑 - 下游:{DownstreamBizType}, 上游:{UpstreamBizType}",
                downstreamUpdate.BusinessType, relation.UpstreamBizType);

            // 示例: 这里可以添加具体的特殊处理逻辑
            // switch (downstreamUpdate.BusinessType)
            // {
            //     case BizType.收款单:
            //         // 查询收款单关联的所有应收款单
            //         break;
            //     case BizType.付款单:
            //         // 查询付款单关联的所有应付款单
            //         break;
            // }

            return relatedBills;
        }

        /// <summary>
        /// 重新计算上游单据的待办状态
        /// </summary>
        private async Task<TodoUpdate> RecalculateUpstreamTodoAsync(BaseEntity upstreamBill, TodoUpdate downstreamUpdate, UpstreamBillRelation relation)
        {
            try
            {
                var bizType = EntityMappingHelper.GetBizType(upstreamBill.GetType());

                // 根据不同的上游单据类型,执行不同的待办状态计算逻辑
                switch (bizType)
                {
                    case BizType.销售订单:
                        return await RecalculateSalesOrderTodoAsync(upstreamBill as tb_SaleOrder, downstreamUpdate);

                    case BizType.采购订单:
                        return await RecalculatePurchaseOrderTodoAsync(upstreamBill as tb_PurOrder, downstreamUpdate);

                    case BizType.应收款单:
                        return await RecalculateReceivableTodoAsync(upstreamBill, downstreamUpdate);

                    case BizType.应付款单:
                        return await RecalculatePayableTodoAsync(upstreamBill, downstreamUpdate);

                    default:
                        _logger.LogDebug("未实现该单据类型的待办重计算逻辑 - 类型:{BizType}", bizType);
                        return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "重新计算上游单据待办状态失败 - 单据ID:{BillId}", upstreamBill.PrimaryKeyID);
                return null;
            }
        }

        #region 具体业务类型的待办重计算逻辑

        /// <summary>
        /// 重新计算销售订单的待办状态
        /// 逻辑: 检查订单是否已全部出库,如果是则移除"待出库"待办
        /// </summary>
        private async Task<TodoUpdate> RecalculateSalesOrderTodoAsync(tb_SaleOrder saleOrder, TodoUpdate downstreamUpdate)
        {
            if (saleOrder == null)
                return null;

            try
            {
                // 查询该订单的所有销售出库单
                var db = MainForm.Instance.AppContext.Db.CopyNew();
                var outbounds = await db.Queryable<tb_SaleOut>()
                    .Where(o => o.SOrder_ID == saleOrder.SOrder_ID)
                    .ToListAsync();

                // 计算已确认的出库数量
                decimal totalOutboundQty = outbounds
                    .Where(o => o.DataStatus == (int)DataStatus.确认)
                    .Sum(o => o.TotalQty);

                // 判断是否全部出库
                bool isFullyOutbound = totalOutboundQty >= (saleOrder.TotalQty);

                if (isFullyOutbound)
                {
                    // 订单已全部出库,推送状态变更通知
                    var update = CreateTodoUpdate(saleOrder, TodoUpdateType.StatusChanged, "销售订单已全部出库");
                    _logger.LogDebug("销售订单{OrderNo}已全部出库,推送待办更新", saleOrder.SOrderNo);
                    return update;
                }
                else
                {
                    // 部分出库,仍然有待出库待办,不需要推送更新
                    _logger.LogDebug("销售订单{OrderNo}部分出库({OutboundQty}/{TotalQty}),保持待办",
                        saleOrder.SOrderNo, totalOutboundQty, saleOrder.TotalQty);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "重新计算销售订单待办状态失败 - 订单ID:{OrderId}", saleOrder.SOrder_ID);
                return null;
            }
        }

        /// <summary>
        /// 重新计算采购订单的待办状态
        /// 逻辑: 检查订单是否已全部入库,如果是则移除"待入库"待办
        /// </summary>
        private async Task<TodoUpdate> RecalculatePurchaseOrderTodoAsync(tb_PurOrder purOrder, TodoUpdate downstreamUpdate)
        {
            if (purOrder == null)
                return null;

            try
            {
                // 查询该订单的所有采购入库单
                var db = MainForm.Instance.AppContext.Db.CopyNew();
                var inbounds = await db.Queryable<tb_PurEntry>()
                    .Where(i => i.PurOrder_ID == purOrder.PurOrder_ID)
                    .ToListAsync();

                // 计算已确认的入库数量
                decimal totalInboundQty = inbounds
                    .Where(i => i.DataStatus == (int)DataStatus.确认)
                    .Sum(i => i.TotalQty);

                // 判断是否全部入库
                bool isFullyInbound = totalInboundQty >= (purOrder.TotalQty);

                if (isFullyInbound)
                {
                    // 订单已全部入库,推送状态变更通知
                    var update = CreateTodoUpdate(purOrder, TodoUpdateType.StatusChanged, "采购订单已全部入库");
                    _logger.LogDebug("采购订单{OrderNo}已全部入库,推送待办更新", purOrder.PurOrderNo);
                    return update;
                }
                else
                {
                    // 部分入库,仍然有待入库待办,不需要推送更新
                    _logger.LogDebug("采购订单{OrderNo}部分入库({InboundQty}/{TotalQty}),保持待办",
                        purOrder.PurOrderNo, totalInboundQty, purOrder.TotalQty);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "重新计算采购订单待办状态失败 - 订单ID:{OrderId}", purOrder.PurOrder_ID);
                return null;
            }
        }

        /// <summary>
        /// 重新计算应收款单的待办状态
        /// TODO: 根据实际业务逻辑实现
        /// </summary>
        private async Task<TodoUpdate> RecalculateReceivableTodoAsync(BaseEntity receivable, TodoUpdate downstreamUpdate)
        {
            // TODO: 实现应收款单的待办重计算逻辑
            // 例如: 检查是否已全额收款

            _logger.LogDebug("应收款单待办重计算逻辑待实现");
            return null;
        }

        /// <summary>
        /// 重新计算应付款单的待办状态
        /// TODO: 根据实际业务逻辑实现
        /// </summary>
        private async Task<TodoUpdate> RecalculatePayableTodoAsync(BaseEntity payable, TodoUpdate downstreamUpdate)
        {
            // TODO: 实现应付款单的待办重计算逻辑
            // 例如: 检查是否已全额付款

            _logger.LogDebug("应付款单待办重计算逻辑待实现");
            return null;
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 创建TodoUpdate对象
        /// </summary>
        private TodoUpdate CreateTodoUpdate(BaseEntity entity, TodoUpdateType updateType, string description)
        {
            try
            {
                var entityInfo = EntityMappingHelper.GetEntityInfo(entity.GetType());
                if (entityInfo == null)
                {
                    _logger.LogWarning("无法获取实体信息 - 类型:{EntityType}", entity.GetType().Name);
                    return null;
                }

                string billNo = entity.GetPropertyValue(entityInfo.NoField)?.ToString();
                long billId = entity.PrimaryKeyID;

                var update = TodoUpdate.Create(
                    updateType,
                    entityInfo.BizType,
                    billId,
                    billNo,
                    entity,
                    entity.StateManager?.GetStatusType(entity),
                    entity.StateManager?.GetBusinessStatus(entity)
                );

                update.InitiatorUserId = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID.ToString();
                update.OperationDescription = description;

                return update;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建TodoUpdate对象失败");
                return null;
            }
        }

        /// <summary>
        /// 根据业务类型和ID查询单据实体
        /// </summary>
        private async Task<BaseEntity> QueryBillByIdAsync(BizType bizType, long billId)
        {
            try
            {
                var entityType = EntityMappingHelper.GetEntityType(bizType);
                if (entityType == null)
                {
                    _logger.LogWarning("无法获取业务类型对应的实体类型 - BizType:{BizType}", bizType);
                    return null;
                }

                // 使用反射调用泛型方法
                var method = typeof(RelatedBillSyncService).GetMethod(nameof(QueryBillByIdAsyncGeneric), 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var genericMethod = method.MakeGenericMethod(entityType);
                
                var task = genericMethod.Invoke(this, new object[] { billId }) as Task<BaseEntity>;
                if (task != null)
                {
                    return await task;
                }
                
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "查询单据实体失败 - BizType:{BizType}, BillId:{BillId}", bizType, billId);
                return null;
            }
        }
        
        /// <summary>
        /// 根据业务类型和ID查询单据实体(泛型内部方法)
        /// </summary>
        private async Task<BaseEntity> QueryBillByIdAsyncGeneric<T>(long billId) where T : BaseEntity, new()
        {
            try
            {
                var db = MainForm.Instance.AppContext.Db.CopyNew();
                var entity = await db.Queryable<T>()
                    .Where(e => e.PrimaryKeyID == billId)
                    .FirstAsync();
                
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "查询单据实体失败 - 类型:{EntityType}, BillId:{BillId}", typeof(T).Name, billId);
                return null;
            }
        }

        /// <summary>
        /// 根据业务类型和ID查询单据实体(泛型版本)
        /// </summary>
        private async Task<T> QueryBillByIdAsync<T>(long billId) where T : BaseEntity
        {
            try
            {
                var db = MainForm.Instance.AppContext.Db.CopyNew();
                var entity = await db.Queryable<T>()
                    .Where(e => e.PrimaryKeyID == billId)
                    .FirstAsync();

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "查询单据实体失败 - 类型:{EntityType}, BillId:{BillId}", typeof(T).Name, billId);
                return null;
            }
        }

        #endregion
    }
}
