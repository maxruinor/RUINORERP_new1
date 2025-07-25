﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.Extensions.Logging;
using RUINORERP.Business;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
using RUINORERP.Model.Context;
using RUINORERP.Common.Extensions;

namespace RUINORERP.Business.CommService
{
    /// <summary>
    /// 目前的方法不合理。写死的 ,暂时没有想到好办法 
    /// 为了 比方传入采购订单类型 及单号就处理对应数据库的事
    /// </summary>
    [Obsolete("用EnhancedBizTypeMapper等相关代替")]
    public class BillConverterFactory
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<BillConverterFactory> _logger;

        public BillConverterFactory(
        ApplicationContext context,
        ILogger<BillConverterFactory> logger)
        {
            _context = context;
            _logger = logger;
        }

        private List<tb_MenuInfo> _UserMenuList = null;
        public List<tb_MenuInfo> UserMenuList
        {
            set
            {
                _UserMenuList = value;
            }
            get
            {

                return _UserMenuList;
            }
        }


        /// <summary>
        /// 得到相关单据信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public CommBillData GetBillData<T>(T Entity) where T : class
        {
            return GetBillData(typeof(T), Entity);
        }



        /// <summary>
        /// 得到相关单据信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public CommBillData GetBillData(Type type, object Entity)
        {
            CommBillData cbd = new CommBillData();
            BizType bizType = new BizType();
            if (UserMenuList == null)
            {
                this.UserMenuList = _context.UserMenuList;
            }
            if (UserMenuList == null)
            {
                _logger.Error($"请联系管理员配置对应的菜单GetBillData");
                UserMenuList = new List<tb_MenuInfo>();
            }
            tb_MenuInfo menuInfo = UserMenuList.Where(c => c.EntityName == type.Name).FirstOrDefault();
            if (menuInfo != null)
            {
                // cbd.BizEntityType=
                cbd.BizName = menuInfo.CaptionCN;
                if (!menuInfo.BizType.HasValue)
                {
                    //throw new Exception("请联系管理员配置对应的业务类型" + menuInfo.MenuName);
                    //_logger.Error("请联系管理员配置对应的业务类型" + menuInfo.MenuName);
                    return cbd;
                }
                bizType = (BizType)menuInfo.BizType;
                //if (bizType==null)
                //{
                //    _logger.Error($"业务类型未配置: {menuInfo.MenuName}");
                //    return cbd;
                //}

                cbd.BizType = bizType;

            }
            //if (Entity == null)
            //{
            //    Entity = Activator.CreateInstance(type);
            //}
            Entity = Entity ?? Activator.CreateInstance(type);

            switch (bizType)
            {
                case BizType.其他费用收入:
                case BizType.其他费用支出:
                    var FMOtherExpenseEntry = Entity as tb_FM_OtherExpense;
                    cbd.BillNo = FMOtherExpenseEntry.ExpenseNo;
                    cbd.BillID = FMOtherExpenseEntry.ExpenseMainID;
                    cbd.BillNoColName = GetExpressionColumnName<tb_FM_OtherExpense>(c => c.ExpenseNo);
                    break;

                case BizType.其他入库单:
                    var otherEntry = Entity as tb_StockIn;
                    cbd.BillNo = otherEntry.BillNo;
                    cbd.BillID = otherEntry.MainID;
                    cbd.BillNoColName = GetExpressionColumnName<tb_StockIn>(c => c.BillNo);
                    break;
                case BizType.其他出库单:
                    var StockOut = Entity as tb_StockOut;
                    cbd.BillNo = StockOut.BillNo;
                    cbd.BillID = StockOut.MainID;
                    cbd.BillNoColName = GetExpressionColumnName<tb_StockOut>(c => c.BillNo);
                    break;

                case BizType.销售订单:
                    var saleOrder = Entity as tb_SaleOrder;
                    if (saleOrder == null)
                    {
                        //这种情况是默认给到了销售订单 因为是枚举第一个。-1没有起到作用。
                        return cbd;
                    }
                    cbd.BillNo = saleOrder.SOrderNo;
                    cbd.BillID = saleOrder.SOrder_ID;
                    cbd.BillNoColName = GetExpressionColumnName<tb_SaleOrder>(c => c.SOrderNo);
                    break;
                case BizType.销售出库单:
                    var saleOut = Entity as tb_SaleOut;
                    cbd.BillNo = saleOut.SaleOutNo;
                    cbd.BillID = saleOut.SaleOut_MainID;
                    cbd.BillNoColName = GetExpressionColumnName<tb_SaleOut>(c => c.SaleOutNo);
                    break;
                case BizType.销售退回单:
                    var saleOutRe = Entity as tb_SaleOutRe;
                    cbd.BillNo = saleOutRe.ReturnNo;
                    cbd.BillID = saleOutRe.SaleOutRe_ID;
                    cbd.BillNoColName = GetExpressionColumnName<tb_SaleOutRe>(c => c.ReturnNo);
                    break;
                case BizType.采购订单:
                    var PurOrder = Entity as tb_PurOrder;
                    cbd.BillNo = PurOrder.PurOrderNo;
                    cbd.BillID = PurOrder.PurOrder_ID;
                    cbd.BillNoColName = GetExpressionColumnName<tb_PurOrder>(c => c.PurOrderNo);

                    break;
                case BizType.采购入库单:
                    var PurEntry = Entity as tb_PurEntry;
                    cbd.BillNo = PurEntry.PurEntryNo;
                    cbd.BillID = PurEntry.PurEntryID;
                    cbd.BillNoColName = GetExpressionColumnName<tb_PurEntry>(c => c.PurEntryNo);
                    break;

                case BizType.采购退货单:
                    var purEntryRe = Entity as tb_PurEntryRe;
                    cbd.BillID = purEntryRe.PurEntryRe_ID;
                    cbd.BillNo = purEntryRe.PurEntryReNo;
                    cbd.BillNoColName = GetExpressionColumnName<tb_PurEntryRe>(c => c.PurEntryReNo);
                    break;

                case BizType.采购退货入库:
                    var PurReturnEntry = Entity as tb_PurReturnEntry;
                    cbd.BillID = PurReturnEntry.PurReEntry_ID;
                    cbd.BillNo = PurReturnEntry.PurReEntryNo;
                    cbd.BillNoColName = GetExpressionColumnName<tb_PurReturnEntry>(c => c.PurReEntryNo);
                    break;
                case BizType.返工退库单:
                    var Return = Entity as tb_MRP_ReworkReturn;
                    cbd.BillID = Return.ReworkReturnID;
                    cbd.BillNo = Return.ReworkReturnNo;
                    cbd.BillNoColName = GetExpressionColumnName<tb_MRP_ReworkReturn>(c => c.ReworkReturnNo);
                    break;
                case BizType.返工入库单:
                    var ReworkEntry = Entity as tb_MRP_ReworkEntry;
                    cbd.BillID = ReworkEntry.ReworkEntryID;
                    cbd.BillNo = ReworkEntry.ReworkEntryNo;
                    cbd.BillNoColName = GetExpressionColumnName<tb_MRP_ReworkEntry>(c => c.ReworkEntryNo);
                    break;
                case BizType.付款申请单:
                    var paymentapp = Entity as tb_FM_PaymentApplication;
                    cbd.BillID = paymentapp.ApplicationID;
                    cbd.BillNo = paymentapp.ApplicationNo;
                    cbd.BillNoColName = GetExpressionColumnName<tb_FM_PaymentApplication>(c => c.ApplicationNo);
                    break;
                /*
            case BizType.返厂出库:
                var Return = Entity as tb_Return;
                cbd.BillID = otherEntry.MainID;
                break;
            case BizType.售后入库:
                var Return = Entity as tb_Return;
                cbd.BillID = Return.MainID;
                cbd.BillNo = Return.ReturnNo;
                break;
            case BizType.售后出库:
                var Return = Entity as tb_Return;
                cbd.BillID = Return.MainID;
                cbd.BillNo = Return.ReturnNo;
                break;
            case BizType.报损单:
                var Return = Entity as tb_Return;
                cbd.BillID = Return.MainID;
                cbd.BillNo = Return.ReturnNo;
                break;
            case BizType.报溢单:
                var Return = Entity as tb_Return;
                cbd.BillID = Return.MainID;
                cbd.BillNo = Return.ReturnNo;
                break;
                */


                case BizType.盘点单:
                    var stocktake = Entity as tb_Stocktake;
                    cbd.BillNo = stocktake.CheckNo;
                    cbd.BillID = stocktake.MainID;
                    cbd.BillNoColName = GetExpressionColumnName<tb_Stocktake>(c => c.CheckNo);
                    break;
                case BizType.生产计划单:
                    var ProductionPlan = Entity as tb_ProductionPlan;
                    cbd.BillNo = ProductionPlan.PPNo;
                    cbd.BillID = ProductionPlan.PPID;
                    cbd.BillNoColName = GetExpressionColumnName<tb_ProductionPlan>(c => c.PPNo);
                    break;
                case BizType.需求分析:
                    var ProductionDemand = Entity as tb_ProductionDemand;
                    cbd.BillNo = ProductionDemand.PDNo;
                    cbd.BillID = ProductionDemand.PDID;
                    cbd.BillNoColName = GetExpressionColumnName<tb_ProductionDemand>(c => c.PDNo);
                    break;

                case BizType.制令单:
                    var ManufacturingOrder = Entity as tb_ManufacturingOrder;
                    cbd.BillID = ManufacturingOrder.MOID;
                    cbd.BillNo = ManufacturingOrder.MONO;
                    cbd.BillNoColName = GetExpressionColumnName<tb_ManufacturingOrder>(c => c.MONO);
                    break;
                case BizType.BOM物料清单:
                    var bomList = Entity as tb_BOM_S;
                    cbd.BillNo = bomList.BOM_No;
                    cbd.BillID = bomList.BOM_ID;
                    cbd.BillNoColName = GetExpressionColumnName<tb_BOM_S>(c => c.BOM_No);
                    break;
                case BizType.生产领料单:
                    var MaterialRequisitions = Entity as tb_MaterialRequisition;
                    cbd.BillID = MaterialRequisitions.MR_ID;
                    cbd.BillNo = MaterialRequisitions.MaterialRequisitionNO;
                    cbd.BillNoColName = GetExpressionColumnName<tb_MaterialRequisition>(c => c.MaterialRequisitionNO);
                    break;
                case BizType.生产退料单:
                    var MaterialReturn = Entity as tb_MaterialReturn;
                    cbd.BillID = MaterialReturn.MRE_ID;
                    cbd.BillNo = MaterialReturn.BillNo;
                    cbd.BillNoColName = GetExpressionColumnName<tb_MaterialReturn>(c => c.BillNo);
                    break;
                case BizType.产品分割单:
                    var ProdSplit = Entity as tb_ProdSplit;
                    cbd.BillID = ProdSplit.SplitID;
                    cbd.BillNo = ProdSplit.SplitNo;
                    cbd.BillNoColName = GetExpressionColumnName<tb_ProdSplit>(c => c.SplitNo);
                    break;
                case BizType.产品组合单:
                    var ProdMerge = Entity as tb_ProdMerge;
                    cbd.BillID = ProdMerge.MergeID;
                    cbd.BillNo = ProdMerge.MergeNo;
                    cbd.BillNoColName = GetExpressionColumnName<tb_ProdMerge>(c => c.MergeNo);
                    break;
                case BizType.缴库单:
                    var FinishedGoodsInv = Entity as tb_FinishedGoodsInv;
                    cbd.BillID = FinishedGoodsInv.FG_ID;
                    cbd.BillNo = FinishedGoodsInv.DeliveryBillNo;
                    cbd.BillNoColName = GetExpressionColumnName<tb_FinishedGoodsInv>(c => c.DeliveryBillNo);
                    break;
                case BizType.借出单:
                    var ProdBorrowing = Entity as tb_ProdBorrowing;
                    cbd.BillID = ProdBorrowing.BorrowID;
                    cbd.BillNo = ProdBorrowing.BorrowNo;
                    cbd.BillNoColName = GetExpressionColumnName<tb_ProdBorrowing>(c => c.BorrowNo);
                    break;
                case BizType.归还单:
                    var ProdReturning = Entity as tb_ProdReturning;
                    cbd.BillID = ProdReturning.ReturnID;
                    cbd.BillNo = ProdReturning.ReturnNo;
                    cbd.BillNoColName = GetExpressionColumnName<tb_ProdReturning>(c => c.ReturnNo);
                    break;
                //case BizType.发料计划单:
                //    var Return = Entity as tb_Return;
                //    cbd.BillID = Return.MainID;
                //    cbd.BillNo = Return.ReturnNo;
                //    break;
                case BizType.请购单:
                    var BuyingRequisition = Entity as tb_BuyingRequisition;
                    cbd.BillID = BuyingRequisition.PuRequisition_ID;
                    cbd.BillNo = BuyingRequisition.PuRequisitionNo;
                    cbd.BillNoColName = GetExpressionColumnName<tb_BuyingRequisition>(c => c.PuRequisitionNo);
                    break;
                case BizType.套装组合:
                    var ProdBundle = Entity as tb_ProdBundle;
                    cbd.BillID = ProdBundle.BundleID;
                    cbd.BillNo = ProdBundle.BundleName;
                    cbd.BillNoColName = GetExpressionColumnName<tb_ProdBundle>(c => c.BundleName);
                    break;
                case BizType.包装信息:
                    var Packing = Entity as tb_Packing;
                    cbd.BillID = Packing.Pack_ID;
                    cbd.BillNo = Packing.PackagingName;
                    cbd.BillNoColName = GetExpressionColumnName<tb_Packing>(c => c.PackagingName);
                    break;
                case BizType.费用报销单:
                    var FM_ExpenseClaim = Entity as tb_FM_ExpenseClaim;
                    cbd.BillID = FM_ExpenseClaim.ClaimMainID;
                    cbd.BillNo = FM_ExpenseClaim.ClaimNo;
                    cbd.BillNoColName = GetExpressionColumnName<tb_FM_ExpenseClaim>(c => c.ClaimNo);
                    break;
                case BizType.产品转换单:
                    var ProdConversion = Entity as tb_ProdConversion;
                    cbd.BillID = ProdConversion.ConversionID;
                    cbd.BillNo = ProdConversion.ConversionNo;
                    cbd.BillNoColName = GetExpressionColumnName<tb_ProdConversion>(c => c.ConversionNo);
                    break;
                case BizType.调拨单:
                    var StockTransfer = Entity as tb_StockTransfer;
                    cbd.BillID = StockTransfer.StockTransferID;
                    cbd.BillNo = StockTransfer.StockTransferNo;
                    cbd.BillNoColName = GetExpressionColumnName<tb_StockTransfer>(c => c.StockTransferNo);
                    break;

                case BizType.预付款单:
                case BizType.预收款单:
                    var PreReceivedPayment = Entity as tb_FM_PreReceivedPayment;
                    if (PreReceivedPayment != null)
                    {
                        if (PreReceivedPayment.ReceivePaymentType == (int)ReceivePaymentType.付款)
                        {

                        }
                        else
                        {
                        }

                        cbd.BillID = PreReceivedPayment.PreRPID;
                        cbd.BillNo = PreReceivedPayment.PreRPNO;
                        cbd.BillNoColName = GetExpressionColumnName<tb_FM_PreReceivedPayment>(c => c.PreRPNO);
                    }
                    break;
                case BizType.付款单:
                case BizType.收款单:
                    var Paymentrecord = Entity as tb_FM_PaymentRecord;
                    if (Paymentrecord != null)
                    {
                        if (Paymentrecord.ReceivePaymentType == (int)ReceivePaymentType.付款)
                        {

                        }
                        else
                        {
                        }

                        cbd.BillID = Paymentrecord.PaymentId;
                        cbd.BillNo = Paymentrecord.PaymentNo;
                        cbd.BillNoColName = GetExpressionColumnName<tb_FM_PaymentRecord>(c => c.PaymentNo);
                    }
                    break;
                case BizType.销售价格调整单:
                case BizType.采购价格调整单:
                    var PriceAdjustment = Entity as tb_FM_PriceAdjustment;
                    if (PriceAdjustment != null)
                    {
                        cbd.BillID = PriceAdjustment.AdjustId;
                        cbd.BillNo = PriceAdjustment.AdjustNo;
                        cbd.BillNoColName = GetExpressionColumnName<tb_FM_PriceAdjustment>(c => c.AdjustNo);
                    }
                    break;

                case BizType.应付款单:
                case BizType.应收款单:
                    var ReceivablePayable = Entity as tb_FM_ReceivablePayable;
                    if (ReceivablePayable != null)
                    {
                        if (ReceivablePayable.ReceivePaymentType == (int)ReceivePaymentType.付款)
                        {

                        }
                        else
                        {
                        }

                        cbd.BillID = ReceivablePayable.ARAPId;
                        cbd.BillNo = ReceivablePayable.ARAPNo;
                        cbd.BillNoColName = GetExpressionColumnName<tb_FM_ReceivablePayable>(c => c.ARAPNo);
                    }
                    break;

                case BizType.售后申请单:
                    var AfterSaleApply = Entity as tb_AS_AfterSaleApply;
                    if (AfterSaleApply != null)
                    {
                        cbd.BillID = AfterSaleApply.ASApplyID;
                        cbd.BillNo = AfterSaleApply.ASApplyNo;
                        cbd.BillNoColName = GetExpressionColumnName<tb_AS_AfterSaleApply>(c => c.ASApplyNo);
                    }
                    break;

                case BizType.售后交付单:
                    var AfterSaleDelivery = Entity as tb_AS_AfterSaleDelivery;
                    if (AfterSaleDelivery != null)
                    {
                        cbd.BillID = AfterSaleDelivery.ASDeliveryID;
                        cbd.BillNo = AfterSaleDelivery.ASDeliveryNo;
                        cbd.BillNoColName = GetExpressionColumnName<tb_AS_AfterSaleDelivery>(c => c.ASDeliveryNo);
                    }
                    break;

                case BizType.维修工单:
                    var RepairOrder = Entity as tb_AS_RepairOrder;
                    if (RepairOrder != null)
                    {
                        cbd.BillID = RepairOrder.RepairOrderID;
                        cbd.BillNo = RepairOrder.RepairOrderNo;
                        cbd.BillNoColName = GetExpressionColumnName<tb_AS_RepairOrder>(c => c.RepairOrderNo);
                    }
                    break;

                case BizType.维修入库单:
                    var RepairInStock = Entity as tb_AS_RepairInStock;
                    if (RepairInStock != null)
                    {
                        cbd.BillID = RepairInStock.RepairInStockID;
                        cbd.BillNo = RepairInStock.RepairInStockNo;
                        cbd.BillNoColName = GetExpressionColumnName<tb_AS_RepairInStock>(c => c.RepairInStockNo);
                    }
                    break;
                case BizType.维修领料单:
                    var RepairMaterialPickup = Entity as tb_AS_RepairMaterialPickup;
                    if (RepairMaterialPickup != null)
                    {
                        cbd.BillID = RepairMaterialPickup.RMRID;
                        cbd.BillNo = RepairMaterialPickup.MaterialPickupNO;
                        cbd.BillNoColName = GetExpressionColumnName<tb_AS_RepairMaterialPickup>(c => c.MaterialPickupNO);
                    }
                    break;
                case BizType.报废单:
                    //var RepairInStock = Entity as tb_AS_ScrapDoc;
                    //if (RepairInStock != null)
                    //{
                    //    cbd.BillID = RepairInStock.RepairInStockID;
                    //    cbd.BillNo = RepairInStock.RepairInStockNo;
                    //    cbd.BillNoColName = GetExpressionColumnName<tb_AS_ScrapDoc>(c => c.RepairInStockNo);
                    //}
                    break;

                /*
        case BizType.托外退料单:
            var Return = Entity as tb_Return;
            cbd.BillID = Return.MainID;
            cbd.BillNo = Return.ReturnNo;
            break;
        case BizType.托外补料单:
            var Return = Entity as tb_Return;
            cbd.BillID = Return.MainID;
            cbd.BillNo = Return.ReturnNo;
            break;
        case BizType.托外加工缴回单:
            var Return = Entity as tb_Return;
            cbd.BillID = Return.MainID;
            cbd.BillNo = Return.ReturnNo;
            break;*/
                case BizType.采购入库统计:
                    var PurEntryStatistics = Entity as View_PurEntryItems;
                    //cbd.BillID = PurEntryStatistics.id;
                    //cbd.BillNo = PurEntryStatistics.BillNo;
                    break;
                case BizType.无对应数据:
                default:
                    _logger.LogError($"GetBillData未实现的业务类型处理: {bizType}");
                    return cbd; ;
                    //throw new Exception($"GetBillData未实现的业务类型处理: {bizType}");

            }
            cbd.BizName = bizType.ObjToString();
            return cbd;

        }

        private string GetExpressionColumnName<T>(Expression<Func<T, object>> propertyExpression)
        {
            var propertyName = propertyExpression.GetMemberInfo().Name;
            return propertyName;
        }


        private string GetExpressionColumnName<T, TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            var propertyName = propertyExpression.GetMemberInfo().Name;
            return propertyName;
        }

    }
}
