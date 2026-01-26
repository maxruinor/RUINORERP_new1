using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model;
using log4net;
using System;
using System.Windows.Media.TextFormatting;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Business.BizMapperService
{
    public class BizEntityInfoConfig
    {
        private readonly ILogger<BizEntityInfoConfig> _logger;
        private readonly ConcurrentDictionary<BizType, BizEntityInfo> _bizTypeToEntityInfo = new ConcurrentDictionary<BizType, BizEntityInfo>();
        private readonly ConcurrentDictionary<Type, BizEntityInfo> _entityTypeToEntityInfo = new ConcurrentDictionary<Type, BizEntityInfo>();
        private readonly ConcurrentDictionary<string, BizEntityInfo> _tableNameToEntityInfo = new ConcurrentDictionary<string, BizEntityInfo>(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<Type, BizEntityInfo> _sharedTableConfigs = new ConcurrentDictionary<Type, BizEntityInfo>();

        /// <summary>
        /// 业务类型到实体信息的映射字典
        /// </summary>
        public ConcurrentDictionary<BizType, BizEntityInfo> BizTypeToEntityInfo => _bizTypeToEntityInfo;

        /// <summary>
        /// 实体类型到实体信息的映射字典
        /// </summary>
        public ConcurrentDictionary<Type, BizEntityInfo> EntityTypeToEntityInfo => _entityTypeToEntityInfo;

        /// <summary>
        /// 表名到实体信息的映射字典（不区分大小写）
        /// </summary>
        public ConcurrentDictionary<string, BizEntityInfo> TableNameToEntityInfo => _tableNameToEntityInfo;

        /// <summary>
        /// 共用表配置字典
        /// </summary>
        public ConcurrentDictionary<Type, BizEntityInfo> SharedTableConfigs => _sharedTableConfigs;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public BizEntityInfoConfig(ILogger<BizEntityInfoConfig> logger)
        {
            _logger = logger;
            // 不再在构造函数中自动注册映射，而是通过外部调用RegisterCommonMappings方法来触发注册
        }

        public void RegisterCommonMappings()
        {
            try
            {
                RegisterEntityMappings();
                RegisterSharedTableMappings();
            }
            catch (Exception ex)
            {
                _logger.LogDebug("注册实体映射时发生错误", ex);
            }
        }

        private void RegisterEntityMappings()
        {
            int successCount = 0;
            int errorCount = 0;

            void SafeRegister<TEntity>(BizType bizType,
                                Expression<Func<TEntity, object>> idField,
                                Expression<Func<TEntity, string>> noField,
                                Expression<Func<TEntity, object>> detailProperty = null) where TEntity : class
            {
                try
                {
                    RegisterEntity<TEntity>(bizType, builder =>
                    {
                        builder.WithTableName(typeof(TEntity).Name)
                               .WithDescription(bizType.ToString())
                               .WithIdField(idField)
                               .WithNoField(noField);

                        if (detailProperty != null)
                            builder.WithDetailProperty(detailProperty);
                    });
                    successCount++;
                }
                catch (Exception ex)
                {
                    errorCount++;
                    _logger.LogError(ex, $"注册实体映射失败: {typeof(TEntity).Name}, 业务类型: {bizType}");
                }
            }

            try
            {
                // 销售
                SafeRegister<tb_SaleOrder>(BizType.销售订单, e => e.SOrder_ID, e => e.SOrderNo, e => e.tb_SaleOrderDetails);
                SafeRegister<tb_SaleOut>(BizType.销售出库单, e => e.SaleOut_MainID, e => e.SaleOutNo, e => e.tb_SaleOutDetails);
                SafeRegister<tb_SaleOutRe>(BizType.销售退回单, e => e.SaleOutRe_ID, e => e.ReturnNo, e => e.tb_SaleOutReDetails);

                // 采购
                SafeRegister<tb_PurOrder>(BizType.采购订单, e => e.PurOrder_ID, e => e.PurOrderNo, e => e.tb_PurOrderDetails);
                SafeRegister<tb_PurEntry>(BizType.采购入库单, e => e.PurEntryID, e => e.PurEntryNo, e => e.tb_PurEntryDetails);
                SafeRegister<tb_PurEntryRe>(BizType.采购退货单, e => e.PurEntryRe_ID, e => e.PurEntryReNo, e => e.tb_PurEntryReDetails);
                SafeRegister<tb_PurReturnEntry>(BizType.采购退货入库, e => e.PurReEntry_ID, e => e.PurReEntryNo, e => e.tb_PurReturnEntryDetails);

                // 生产
                SafeRegister<tb_BOM_S>(BizType.BOM物料清单, e => e.BOM_ID, e => e.BOM_No, e => e.tb_BOM_SDetails);
                SafeRegister<tb_ManufacturingOrder>(BizType.制令单, e => e.MOID, e => e.MONO, e => e.tb_ManufacturingOrderDetails);
                SafeRegister<tb_ProductionPlan>(BizType.生产计划单, e => e.PPID, e => e.PPNo, e => e.tb_ProductionPlanDetails);
                SafeRegister<tb_MaterialRequisition>(BizType.生产领料单, e => e.MR_ID, e => e.MaterialRequisitionNO, e => e.tb_MaterialRequisitionDetails);
                SafeRegister<tb_MaterialReturn>(BizType.生产退料单, e => e.MRE_ID, e => e.BillNo, e => e.tb_MaterialReturnDetails);
                SafeRegister<tb_ProductionDemand>(BizType.需求分析, e => e.PDID, e => e.PDNo, e => e.tb_ProductionDemandDetails);
                SafeRegister<tb_FinishedGoodsInv>(BizType.缴库单, e => e.FG_ID, e => e.DeliveryBillNo, e => e.tb_FinishedGoodsInvDetails);

                // 库存
                SafeRegister<tb_Stocktake>(BizType.盘点单, e => e.MainID, e => e.CheckNo, e => e.tb_StocktakeDetails);
                SafeRegister<tb_StockIn>(BizType.其他入库单, e => e.MainID, e => e.BillNo, e => e.tb_StockInDetails);
                SafeRegister<tb_StockOut>(BizType.其他出库单, e => e.MainID, e => e.BillNo, e => e.tb_StockOutDetails);
                SafeRegister<tb_StockTransfer>(BizType.调拨单, e => e.StockTransferID, e => e.StockTransferNo, e => e.tb_StockTransferDetails);

                // 费用
                SafeRegister<tb_FM_ExpenseClaim>(BizType.费用报销单, e => e.ClaimMainID, e => e.ClaimNo, e => e.tb_FM_ExpenseClaimDetails);


                // 请购
                SafeRegister<tb_BuyingRequisition>(BizType.请购单, e => e.PuRequisition_ID, e => e.PuRequisitionNo, e => e.tb_BuyingRequisitionDetails);

                // 借出/归还
                SafeRegister<tb_ProdBorrowing>(BizType.借出单, e => e.BorrowID, e => e.BorrowNo, e => e.tb_ProdBorrowingDetails);
                SafeRegister<tb_ProdReturning>(BizType.归还单, e => e.ReturnID, e => e.ReturnNo, e => e.tb_ProdReturningDetails);

                // 产品组合/分割/套装/转换
                SafeRegister<tb_ProdMerge>(BizType.产品组合单, e => e.MergeID, e => e.MergeNo, e => e.tb_ProdMergeDetails);
                SafeRegister<tb_ProdSplit>(BizType.产品分割单, e => e.SplitID, e => e.SplitNo, e => e.tb_ProdSplitDetails);
                SafeRegister<tb_ProdBundle>(BizType.套装组合, e => e.BundleID, e => e.BundleName, e => e.tb_ProdBundleDetails);
                SafeRegister<tb_ProdConversion>(BizType.产品转换单, e => e.ConversionID, e => e.ConversionNo, e => e.tb_ProdConversionDetails);

                // 返工
                SafeRegister<tb_MRP_ReworkReturn>(BizType.返工退库单, e => e.ReworkReturnID, e => e.ReworkReturnNo, e => e.tb_MRP_ReworkReturnDetails);
                SafeRegister<tb_MRP_ReworkEntry>(BizType.返工入库单, e => e.ReworkEntryID, e => e.ReworkEntryNo, e => e.tb_MRP_ReworkEntryDetails);

                // 付款申请（无明细）
                SafeRegister<tb_FM_PaymentApplication>(BizType.付款申请单, e => e.ApplicationID, e => e.ApplicationNo, e => e.tb_fm_payeeinfo);
                SafeRegister<tb_FM_Statement>(BizType.对账单, e => e.StatementId, e => e.StatementNo, e => e.tb_FM_StatementDetails);

                // 售后
                SafeRegister<tb_AS_AfterSaleApply>(BizType.售后申请单, e => e.ASApplyID, e => e.ASApplyNo, e => e.tb_AS_AfterSaleApplyDetails);
                SafeRegister<tb_AS_AfterSaleDelivery>(BizType.售后交付单, e => e.ASDeliveryID, e => e.ASDeliveryNo, e => e.tb_AS_AfterSaleDeliveryDetails);
                SafeRegister<tb_AS_RepairOrder>(BizType.维修工单, e => e.RepairOrderID, e => e.RepairOrderNo, e => e.tb_AS_RepairOrderDetails);
                SafeRegister<tb_AS_RepairMaterialPickup>(BizType.维修领料单, e => e.RMRID, e => e.MaterialPickupNO, e => e.tb_AS_RepairMaterialPickupDetails);
                SafeRegister<tb_AS_RepairInStock>(BizType.维修入库单, e => e.RepairInStockID, e => e.RepairInStockNo, e => e.tb_AS_RepairInStockDetails);

                // CRM相关
                SafeRegister<tb_CRM_FollowUpPlans>(BizType.CRM跟进计划, e => e.PlanID, e => e.PlanSubject);
                SafeRegister<tb_CRM_FollowUpRecords>(BizType.CRM跟进记录, e => e.RecordID, e => e.FollowUpSubject);

                // 包装信息
                SafeRegister<tb_Packing>(BizType.包装信息, e => e.Pack_ID, e => e.PackagingName);
                SafeRegister<tb_Prod>(BizType.产品档案, e => e.ProdBaseID, e => e.CNName);
                SafeRegister<tb_EOP_WaterStorage>(BizType.蓄水订单, e => e.WSR_ID, e => e.WSRNo);

                _logger.Debug("普通实体映射注册完成，成功：{SuccessCount}，失败：{ErrorCount}", successCount, errorCount);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "注册实体映射时发生错误");
                errorCount++;
            }
        }

        private void RegisterSharedTableMappings()
        {
            int successCount = 0;
            int errorCount = 0;

            void SafeRegister<TEntity, TDiscriminator>(
                IDictionary<TDiscriminator, BizType> map,
                Expression<Func<TEntity, TDiscriminator>> discriminatorExpr,
                Expression<Func<TEntity, object>> idField,
                Expression<Func<TEntity, string>> noField,
                Expression<Func<TEntity, object>> detailProperty = null)
                where TEntity : class
            {
                try
                {
                    RegisterSharedTable<TEntity, TDiscriminator>(
                        map,
                        discriminatorExpr,
                        builder =>
                        {
                            builder.WithTableName(typeof(TEntity).Name)
                                   .WithDescription(string.Join("/", map.Values))
                                   .WithIdField(idField)
                                   .WithNoField(noField)
                                   .WithAddMaper<TDiscriminator>(map)
                                   ;

                            if (detailProperty != null)
                                builder.WithDetailProperty(detailProperty);

                            builder.WithDiscriminator(discriminatorExpr, v => map[v]);


                        });

                    successCount++;
                }
                catch (Exception ex)
                {
                    _logger.LogDebug(ex, "注册共用表 {EntityName} 时发生错误", typeof(TEntity).Name);
                    errorCount++;
                }
            }

            try
            {
                _logger.Debug("开始注册共用表实体映射...");

                // 损溢单
                SafeRegister<tb_FM_ProfitLoss, int>(
                    new Dictionary<int, BizType>
                    {
                        {(int)ProfitLossDirection.损失, BizType.损失确认单},
                        {(int)ProfitLossDirection.溢余, BizType.溢余确认单}
                    },
                    e => e.ProfitLossDirection,
                    e => e.ProfitLossId,
                    e => e.ProfitLossNo,
                    e => e.tb_FM_ProfitLossDetails);

                // 价格调整单
                SafeRegister<tb_FM_PriceAdjustment, int>(
                    new Dictionary<int, BizType>
                    {
                        {(int)ReceivePaymentType.收款, BizType.销售价格调整单},
                        {(int)ReceivePaymentType.付款, BizType.采购价格调整单}
                    },
                    e => e.ReceivePaymentType,
                    e => e.AdjustId,
                    e => e.AdjustNo,
                    e => e.tb_FM_PriceAdjustmentDetails);

                // 收/付款单
                SafeRegister<tb_FM_PaymentRecord, int>(
                    new Dictionary<int, BizType>
                    {
                        {(int)ReceivePaymentType.收款, BizType.收款单},
                        {(int)ReceivePaymentType.付款, BizType.付款单}
                    },
                    e => e.ReceivePaymentType,
                    e => e.PaymentId,
                    e => e.PaymentNo,
                    e => e.tb_FM_PaymentRecordDetails);

                // 预收/预付款
                SafeRegister<tb_FM_PreReceivedPayment, int>(
                    new Dictionary<int, BizType>
                    {
                        {(int)ReceivePaymentType.收款, BizType.预收款单},
                        {(int)ReceivePaymentType.付款, BizType.预付款单}
                    },
                    e => e.ReceivePaymentType,
                    e => e.PreRPID,
                    e => e.PreRPNO);

                // 应收/应付
                SafeRegister<tb_FM_ReceivablePayable, int>(
                    new Dictionary<int, BizType>
                    {
                        {(int)ReceivePaymentType.收款, BizType.应收款单},
                        {(int)ReceivePaymentType.付款, BizType.应付款单}
                    },
                    e => e.ReceivePaymentType,
                    e => e.ARAPId,
                    e => e.ARAPNo,
                    e => e.tb_FM_ReceivablePayableDetails);

                // 收款/付款核销
                SafeRegister<tb_FM_PaymentSettlement, int>(
                    new Dictionary<int, BizType>
                    {
                        {(int)ReceivePaymentType.收款, BizType.收款核销},
                        {(int)ReceivePaymentType.付款, BizType.付款核销}
                    },
                    e => e.ReceivePaymentType,
                    e => e.SettlementId,
                    e => e.SettlementNo,
                    e => e.tb_FM_PaymentSettlements);

                // 其他费用收入/支出
                SafeRegister<tb_FM_OtherExpense, bool>(
                    new Dictionary<bool, BizType>
                    {
                        { false, BizType.其他费用支出 },
                        { true, BizType.其他费用收入 }
                    },
                    e => e.EXPOrINC,
                    e => e.ExpenseMainID,
                    e => e.ExpenseNo,
                    e => e.tb_FM_OtherExpenseDetails);

                _logger.Debug("共用表实体映射注册完成，成功：{SuccessCount}，失败：{ErrorCount}", successCount, errorCount);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "注册共用表实体映射时发生错误");
            }
        }


        #region 注册方法

        public void RegisterEntity<TEntity>(BizType bizType, Action<EntityInfoBuilder<TEntity>> configure = null) where TEntity : class
        {
            var builder = new EntityInfoBuilder<TEntity>();
            builder.WithBizType(bizType);

            configure?.Invoke(builder);

            var entityInfo = builder.Build();

            _bizTypeToEntityInfo.TryAdd(bizType, entityInfo);
            _entityTypeToEntityInfo.TryAdd(typeof(TEntity), entityInfo);
            _tableNameToEntityInfo.TryAdd(entityInfo.TableName, entityInfo);

            _logger.LogDebug("已注册实体信息: BizType={0}, EntityType={1}, TableName={2}",
                bizType, typeof(TEntity).Name, entityInfo.TableName);
        }

        /// <summary>
        /// 这里是注册共享类型的实体信息，根据业务类型如付款方向 。会在_bizTypeToEntityInfo 业务信息集合对应的中添加两条
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDiscriminator"></typeparam>
        /// <param name="typeMapping"></param>
        /// <param name="discriminatorExpr"></param>
        /// <param name="configure"></param>
        public void RegisterSharedTable<TEntity, TDiscriminator>(
            IDictionary<TDiscriminator, BizType> typeMapping,
            Expression<Func<TEntity, TDiscriminator>> discriminatorExpr,
            Action<EntityInfoBuilder<TEntity>> configure = null) where TEntity : class
        {
            var builder = new EntityInfoBuilder<TEntity>();
            var entityType = typeof(TEntity);


            builder.WithTableName(entityType.Name);

            configure?.Invoke(builder);

            var entityInfo = builder.Build();

            // 设置鉴别器和类型解析器
            var fieldName = GetMemberName(discriminatorExpr);
            entityInfo.DiscriminatorField = fieldName;
            entityInfo.TypeResolver = value =>
            {
                if (value is TDiscriminator discriminatorValue && typeMapping.TryGetValue(discriminatorValue, out var bizType))
                {
                    return bizType;
                }
                return BizType.无对应数据;
            };

            // 预注册所有可能的业务类型 收款，付款，对应一个基本信息
            foreach (var mapping in typeMapping)
            {
                var newEntityInfo = entityInfo.DeepClone();
                newEntityInfo.BizType = mapping.Value;
                _bizTypeToEntityInfo.TryAdd(mapping.Value, newEntityInfo);
            }

            _entityTypeToEntityInfo.TryAdd(entityType, entityInfo);
            _tableNameToEntityInfo.TryAdd(entityInfo.TableName, entityInfo);
            _sharedTableConfigs.TryAdd(entityType, entityInfo);

            _logger.LogDebug("已注册共用表实体信息: EntityType={0}, TableName={1}, 预注册业务类型数量={2}",
                entityType.Name, entityInfo.TableName, typeMapping.Count);
        }


        private string GetMemberName<TEntity, T>(Expression<Func<TEntity, T>> expression)
        {
            if (expression.Body is MemberExpression memberExpr)
            {
                return memberExpr.Member.Name;
            }

            if (expression.Body is UnaryExpression unaryExpr && unaryExpr.Operand is MemberExpression memberExpr2)
            {
                return memberExpr2.Member.Name;
            }

            throw new ArgumentException("Expression must be a member access expression.", nameof(expression));
        }



        #endregion
    }
}