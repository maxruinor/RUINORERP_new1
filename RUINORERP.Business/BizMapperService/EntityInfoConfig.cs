using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model;
using log4net;
using System;
using System.Windows.Media.TextFormatting;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace RUINORERP.Business.BizMapperService
{
    /// <summary>
    /// 实体信息配置类 - 用于注册新体系中的实体映射规则
    /// </summary>
    public class EntityInfoConfig
    {
        private readonly IEntityInfoService _entityInfoService;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(EntityInfoConfig));

        public EntityInfoConfig(IEntityInfoService entityInfoService)
        {
            _entityInfoService = entityInfoService;
        }

        /// <summary>
        /// 注册所有常用实体映射 - 通过新的实体信息服务体系
        /// </summary>
        public void RegisterCommonMappings()
        {
            try
            {
                // 注册普通实体映射
                RegisterEntityMappings();
                // 注册共用表实体映射
                RegisterSharedTableMappings();
            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("注册实体映射时发生错误: {0}", ex.Message);
                _logger.Debug("异常详情:", ex);
            }
        }

        /// <summary>
        /// 注册普通实体映射
        /// </summary>
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
                    // 关键：显式指定 TEntity
                    _entityInfoService.RegisterEntity<TEntity>(bizType, builder =>
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
                    _logger.ErrorFormat("注册实体 {0}({1}) 时发生错误：{2}", typeof(TEntity).Name, bizType, ex.Message);
                    _logger.Debug("异常详情:", ex);
                    errorCount++;
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
                SafeRegister<tb_FM_PaymentApplication>(BizType.付款申请单, e => e.ApplicationID, e => e.ApplicationNo);

                // 售后
                SafeRegister<tb_AS_AfterSaleApply>(BizType.售后申请单, e => e.ASApplyID, e => e.ASApplyNo, e => e.tb_AS_AfterSaleApplyDetails);
                SafeRegister<tb_AS_AfterSaleDelivery>(BizType.售后交付单, e => e.ASDeliveryID, e => e.ASDeliveryNo, e => e.tb_AS_AfterSaleDeliveryDetails);
                SafeRegister<tb_AS_RepairOrder>(BizType.维修工单, e => e.RepairOrderID, e => e.RepairOrderNo, e => e.tb_AS_RepairOrderDetails);
                SafeRegister<tb_AS_RepairInStock>(BizType.维修入库单, e => e.RepairInStockID, e => e.RepairInStockNo, e => e.tb_AS_RepairInStockDetails);

                _logger.InfoFormat("普通实体映射注册完成，成功：{0}，失败：{1}", successCount, errorCount);

                // 销售订单
                //_entityInfoService.RegisterEntity<tb_SaleOrder>(BizType.销售订单, builder =>
                //{
                //    builder.WithTableName(nameof(tb_SaleOrder))
                //           .WithDescription("销售订单")
                //           .WithIdField(e => e.SOrder_ID)
                //           .WithNoField(e => e.SOrderNo)
                //           .WithDetailProperty(e => e.tb_SaleOrderDetails);
                //});
                //successCount++;

            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("注册实体映射时发生错误: {0}", ex.Message);
                _logger.Debug("异常详情:", ex);
                errorCount++;
            }
        }

        /// <summary>
        /// 注册共用表实体映射
        /// </summary>
        private void RegisterSharedTableMappings()
        {
            int successCount = 0;
            int errorCount = 0;

            void SafeRegister<TEntity, TDiscriminator>(
                IDictionary<TDiscriminator, BizType> map,        // 关键：值->BizType 字典
                Expression<Func<TEntity, TDiscriminator>> discriminatorExpr,
                Expression<Func<TEntity, object>> idField,
                Expression<Func<TEntity, string>> noField,
                Expression<Func<TEntity, object>> detailProperty = null)
                where TEntity : class
            {
                try
                {
                    _entityInfoService.RegisterSharedTable<TEntity, TDiscriminator>(
                        v => map[v],                                   // 运行时查字典
                        builder =>
                        {
                            builder.WithTableName(typeof(TEntity).Name)
                                   .WithDescription(string.Join("/", map.Values)) // 描述拼一下
                                   .WithIdField(idField)
                                   .WithNoField(noField);

                            if (detailProperty != null) builder.WithDetailProperty(detailProperty);

                            builder.WithDiscriminator(discriminatorExpr, v => map[v]);
                        });
                    successCount++;
                }
                catch (Exception ex)
                {
                    _logger.ErrorFormat("注册共用表 {0} 时发生错误：{1}", typeof(TEntity).Name, ex.Message);
                    _logger.Debug("异常详情:", ex);
                    errorCount++;
                }
            }


            try
            {
                _logger.Info("开始注册共用表实体映射...");


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
                    new Dictionary<bool, BizType> { { false, BizType.其他费用支出 }, { true, BizType.其他费用收入 } },
                    e => e.EXPOrINC,
                    e => e.ExpenseMainID,
                    e => e.ExpenseNo,
                    e => e.tb_FM_OtherExpenseDetails);

                _logger.InfoFormat("共用表实体映射注册完成，成功：{0}，失败：{1}", successCount, errorCount);


                // 其他费用收入/支出单
                //_entityInfoService.RegisterSharedTable<tb_FM_OtherExpense, bool>(
                //    typeResolver: value => value == false ? BizType.其他费用收入 : BizType.其他费用支出,
                //    configure: builder =>
                //    {
                //        builder.WithTableName("tb_FM_OtherExpense")
                //               .WithDescription("其他费用收入/支出单")
                //               .WithIdField(e => e.ExpenseMainID)
                //               .WithNoField(e => e.ExpenseNo)
                //               .WithDetailProperty(e => e.tb_FM_OtherExpenseDetails)
                //               .WithDiscriminator(e => e.EXPOrINC, type =>
                //               {
                //                   return type == false ? BizType.其他费用收入 : BizType.其他费用支出;
                //               });
                //    });
                //successCount++;

                 
            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("注册共用表实体映射时发生错误: {0}", ex.Message);
                _logger.Debug("异常详情:", ex);
            }
        }
    }
}