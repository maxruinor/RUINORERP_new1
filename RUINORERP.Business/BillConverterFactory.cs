using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
using RUINORERP.Model.Context;

namespace RUINORERP.Business
{
    /// <summary>
    /// 目前的方法不合理。写死的 ,暂时没有想到好办法 
    /// 为了 比方传入采购订单类型 及单号就处理对应数据库的事
    /// </summary>
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
            get
            {
                if (_UserMenuList == null)
                {
                    _UserMenuList = this._context.Db.CopyNew().Queryable<tb_MenuInfo>().Where(r => r.IsVisble == true)
                //.Includes(t => t.)
                //.Includes(t => t.tb_roleinfo)
                .ToList();
                }
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
            tb_MenuInfo menuInfo = UserMenuList.Where(c => c.EntityName == type.Name).FirstOrDefault();
            if (menuInfo != null)
            {
                // cbd.BizEntityType=
                cbd.BizName = menuInfo.CaptionCN;
                if (!menuInfo.BizType.HasValue)
                {
                    //throw new Exception("请联系管理员配置对应的业务类型" + menuInfo.MenuName);
                    _logger.Error("请联系管理员配置对应的业务类型" + menuInfo.MenuName);
                    return cbd;
                }
                bizType = (BizType)menuInfo.BizType;
                cbd.BizType = bizType;
            }
            if (Entity == null)
            {
                Entity = Activator.CreateInstance(type);
            }

            switch (bizType)
            {
                case BizType.其他费用收入:
                    var FMOtherExpenseEntryinc = Entity as tb_FM_OtherExpense;
                    cbd.BillNo = FMOtherExpenseEntryinc.ExpenseNo;
                    cbd.BillID = FMOtherExpenseEntryinc.ExpenseMainID;
                    break;

                case BizType.其他费用支出:
                    var FMOtherExpenseEntry = Entity as tb_FM_OtherExpense;
                    cbd.BillNo = FMOtherExpenseEntry.ExpenseNo;
                    cbd.BillID = FMOtherExpenseEntry.ExpenseMainID;
                    break;

                case BizType.其他入库单:
                    var otherEntry = Entity as tb_StockIn;
                    cbd.BillNo = otherEntry.BillNo;
                    cbd.BillID = otherEntry.MainID;
                    break;
                case BizType.其他出库单:
                    var StockOut = Entity as tb_StockOut;
                    cbd.BillNo = StockOut.BillNo;
                    cbd.BillID = StockOut.MainID;
                    break;

                case BizType.销售订单:
                    var saleOrder = Entity as tb_SaleOrder;
                    cbd.BillNo = saleOrder.SOrderNo;
                    cbd.BillID = saleOrder.SOrder_ID;
                    break;
                case BizType.销售出库单:
                    var saleOut = Entity as tb_SaleOut;
                    cbd.BillNo = saleOut.SaleOutNo;
                    cbd.BillID = saleOut.SaleOut_MainID;
                    break;
                case BizType.销售退回单:
                    var saleOutRe = Entity as tb_SaleOutRe;
                    cbd.BillNo = saleOutRe.ReturnNo;
                    cbd.BillID = saleOutRe.SaleOutRe_ID;
                    break;
                case BizType.采购订单:
                    var PurOrder = Entity as tb_PurOrder;
                    cbd.BillNo = PurOrder.PurOrderNo;
                    cbd.BillID = PurOrder.PurOrder_ID;

                    break;
                case BizType.采购入库单:
                    var PurEntry = Entity as tb_PurEntry;
                    cbd.BillNo = PurEntry.PurEntryNo;
                    cbd.BillID = PurEntry.PurEntryID;
                    break;
                case BizType.返厂出库:
                    var Return = Entity as tb_Return;
                    cbd.BillID = Return.MainID;
                    cbd.BillNo = Return.RetrunNo;
                    break;
                case BizType.采购入库退回单:
                    var purEntryRe = Entity as tb_PurEntryRe;
                    cbd.BillID = purEntryRe.PurEntryRe_ID;
                    cbd.BillNo = purEntryRe.PurEntryRENo;
                    break;
                /*
            case BizType.返厂出库:
                var Return = Entity as tb_Return;
                cbd.BillID = otherEntry.MainID;
                break;
            case BizType.售后入库:
                var Return = Entity as tb_Return;
                cbd.BillID = Return.MainID;
                cbd.BillNo = Return.RetrunNo;
                break;
            case BizType.售后出库:
                var Return = Entity as tb_Return;
                cbd.BillID = Return.MainID;
                cbd.BillNo = Return.RetrunNo;
                break;
            case BizType.报损单:
                var Return = Entity as tb_Return;
                cbd.BillID = Return.MainID;
                cbd.BillNo = Return.RetrunNo;
                break;
            case BizType.报溢单:
                var Return = Entity as tb_Return;
                cbd.BillID = Return.MainID;
                cbd.BillNo = Return.RetrunNo;
                break;
                */


                case BizType.盘点单:
                    var stocktake = Entity as tb_Stocktake;
                    cbd.BillNo = stocktake.CheckNo;
                    cbd.BillID = stocktake.MainID;
                    break;
                case BizType.生产计划单:
                    var ProductionPlan = Entity as tb_ProductionPlan;
                    cbd.BillNo = ProductionPlan.PPNo;
                    cbd.BillID = ProductionPlan.PPID;
                    break;
                case BizType.生产需求分析:
                    var ProductionDemand = Entity as tb_ProductionDemand;
                    cbd.BillNo = ProductionDemand.PDNo;
                    cbd.BillID = ProductionDemand.PDID;
                    break;

                case BizType.制令单:
                    var ManufacturingOrder = Entity as tb_ManufacturingOrder;
                    cbd.BillID = ManufacturingOrder.MOID;
                    cbd.BillNo = ManufacturingOrder.MONO;
                    break;
                case BizType.BOM物料清单:
                    var bomList = Entity as tb_BOM_S;
                    cbd.BillNo = bomList.BOM_No;
                    cbd.BillID = bomList.BOM_ID;
                    break;
                case BizType.生产领料单:
                    var MaterialRequisitions = Entity as tb_MaterialRequisition;
                    cbd.BillID = MaterialRequisitions.MR_ID;
                    cbd.BillNo = MaterialRequisitions.MaterialRequisitionNO;
                    break;
                case BizType.生产退料单:
                    var MaterialReturn = Entity as tb_MaterialReturn;
                    cbd.BillID = MaterialReturn.MRE_ID;
                    cbd.BillNo = MaterialReturn.BillNo;
                    break;
                case BizType.产品分割单:
                    var ProdSplit = Entity as tb_ProdSplit;
                    cbd.BillID = ProdSplit.SplitID;
                    cbd.BillNo = ProdSplit.SplitNo;
                    break;
                case BizType.产品组合单:
                    var ProdMerge = Entity as tb_ProdMerge;
                    cbd.BillID = ProdMerge.MergeID;
                    cbd.BillNo = ProdMerge.MergeNo;
                    break;
                case BizType.缴库单:
                    var FinishedGoodsInv = Entity as tb_FinishedGoodsInv;
                    cbd.BillID = FinishedGoodsInv.FG_ID;
                    cbd.BillNo = FinishedGoodsInv.DeliveryBillNo;
                    break;
                case BizType.借出单:
                    var ProdBorrowing = Entity as tb_ProdBorrowing;
                    cbd.BillID = ProdBorrowing.BorrowID;
                    cbd.BillNo = ProdBorrowing.BorrowNo;
                    break;
                case BizType.归还单:
                    var ProdReturning = Entity as tb_ProdReturning;
                    cbd.BillID = ProdReturning.ReturnID;
                    cbd.BillNo = ProdReturning.ReturnNo;
                    break;
                //case BizType.发料计划单:
                //    var Return = Entity as tb_Return;
                //    cbd.BillID = Return.MainID;
                //    cbd.BillNo = Return.RetrunNo;
                //    break;
                case BizType.请购单:
                    var BuyingRequisition = Entity as tb_BuyingRequisition;
                    cbd.BillID = BuyingRequisition.PuRequisition_ID;
                    cbd.BillNo = BuyingRequisition.PuRequisitionNo;
                    break;
                case BizType.套装组合:
                    var ProdBundle = Entity as tb_ProdBundle;
                    cbd.BillID = ProdBundle.BundleID;
                    cbd.BillNo = ProdBundle.BundleName;
                    break;
                /*
            case BizType.托外加工单:
                var Return = Entity as tb_Return;
                cbd.BillID = Return.MainID;
                cbd.BillNo = Return.RetrunNo;
                break;
            case BizType.托外领料单:
                var Return = Entity as tb_Return;
                cbd.BillID = Return.MainID;
                cbd.BillNo = Return.RetrunNo;
                break;
            case BizType.托外退料单:
                var Return = Entity as tb_Return;
                cbd.BillID = Return.MainID;
                cbd.BillNo = Return.RetrunNo;
                break;
            case BizType.托外补料单:
                var Return = Entity as tb_Return;
                cbd.BillID = Return.MainID;
                cbd.BillNo = Return.RetrunNo;
                break;
            case BizType.托外加工缴回单:
                var Return = Entity as tb_Return;
                cbd.BillID = Return.MainID;
                cbd.BillNo = Return.RetrunNo;
                break;*/
                case BizType.采购入库统计:
                    var PurEntryStatistics = Entity as View_PurEntryItems;
                    //cbd.BillID = PurEntryStatistics.MainID;
                    //cbd.BillNo = PurEntryStatistics.BillNo;
                    break;
                default:
                    throw new Exception("请实现对应业务类型的单号提取！");
                    break;
            }
            cbd.BizName = bizType.ObjToString();
            return cbd;

        }
    }
}
