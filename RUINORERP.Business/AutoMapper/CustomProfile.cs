using AutoMapper;
using RUINORERP.Model;
using RUINORERP.Model.Dto;
using RUINORERP.Model.Models;

using System;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.Business.AutoMapper
{

    /// <summary>
    /// 使用中配置构造函数，用来创建关系映射 bestnew
    /// </summary>
    public class CustomProfile : Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public CustomProfile()
        {

            // 应用全局配置
            //IgnoreBaseEntityProperties();
            //ApplySmartConventions();

            //销售退货单时，从主仓出，退到维修仓库时 更新库存时得新建。参考主仓信息
            CreateMap<tb_Inventory, tb_Inventory>();

            #region 财务模块

            CreateMap<tb_FM_ReceivablePayable, tb_FM_StatementDetail>()
            .ForMember(a => a.IncludedForeignAmount, o => o.MapFrom(d => d.ForeignBalanceAmount))
            .ForMember(a => a.IncludedLocalAmount, o => o.MapFrom(d => d.LocalBalanceAmount))
            .ForMember(a => a.Summary, o => o.MapFrom(d => d.Remark));

            CreateMap<tb_PurReturnEntry, tb_FM_ReceivablePayable>();
            CreateMap<tb_PurReturnEntryDetail, tb_FM_ReceivablePayableDetail>()
           .ForMember(a => a.UnitPrice, o => o.MapFrom(d => d.UnitPrice))
           .ForMember(a => a.Quantity, o => o.MapFrom(d => d.Quantity))
           .ForMember(a => a.ProdDetailID, o => o.MapFrom(d => d.ProdDetailID))
           .ForMember(a => a.SourceItemRowID, o => o.MapFrom(d => d.PurReEntry_CID))
           .ForMember(a => a.property, o => o.MapFrom(d => d.property))
           .ForMember(a => a.TaxRate, o => o.MapFrom(d => d.TaxRate))
           .ForMember(a => a.TaxLocalAmount, o => o.MapFrom(d => d.TaxAmount))
           .ForMember(a => a.LocalPayableAmount, o => o.MapFrom(d => d.SubtotalTrPriceAmount))
           .ForMember(a => a.Summary, o => o.MapFrom(d => d.Summary));



            //维修物料明细收费 转成应收明细
            CreateMap<tb_AS_RepairOrderMaterialDetail, tb_FM_ReceivablePayableDetail>()
          .ForMember(a => a.UnitPrice, o => o.MapFrom(d => d.UnitPrice))
          .ForMember(a => a.Quantity, o => o.MapFrom(d => d.ShouldSendQty))
          .ForMember(a => a.ProdDetailID, o => o.MapFrom(d => d.ProdDetailID))
          .ForMember(a => a.SourceItemRowID, o => o.MapFrom(d => d.RepairMaterialDetailID))
          .ForMember(a => a.property, o => o.MapFrom(d => d.property))
          .ForMember(a => a.TaxRate, o => o.MapFrom(d => d.TaxRate))
          .ForMember(a => a.TaxLocalAmount, o => o.MapFrom(d => d.SubtotalTaxAmount))
          .ForMember(a => a.LocalPayableAmount, o => o.MapFrom(d => d.SubtotalTransAmount))
          .ForMember(a => a.Summary, o => o.MapFrom(d => d.Summary));

            CreateMap<tb_AS_RepairOrder, tb_FM_ReceivablePayable>();


            CreateMap<tb_AS_RepairOrder, tb_AS_RepairMaterialPickup>();
            CreateMap<tb_AS_RepairOrderMaterialDetail, tb_AS_RepairMaterialPickupDetail>();


            CreateMap<View_ProdDetail, tb_AS_RepairInStockDetail>();
            CreateMap<View_ProdDetail, tb_AS_RepairOrderDetail>();
            CreateMap<View_ProdDetail, tb_AS_RepairOrderMaterialDetail>();
            CreateMap<View_ProdDetail, tb_AS_AfterSaleApplyDetail>();
            CreateMap<View_ProdDetail, tb_AS_AfterSaleDeliveryDetail>();

            CreateMap<View_ProdDetail, tb_PurEntryReDetail>();
            CreateMap<View_ProdDetail, tb_PurReturnEntryDetail>();





            //由售后申请转为售后交付单
            CreateMap<tb_AS_AfterSaleApplyDetail, tb_AS_RepairOrderDetail>();
            CreateMap<tb_AS_AfterSaleApply, tb_AS_RepairOrder>();
            CreateMap<tb_AS_RepairOrder, tb_AS_RepairInStock>();
            CreateMap<tb_AS_RepairOrderDetail, tb_AS_RepairInStockDetail>();

            //由售后申请转为售后交付单
            CreateMap<tb_AS_AfterSaleApply, tb_AS_AfterSaleDelivery>();
            CreateMap<tb_AS_AfterSaleApplyDetail, tb_AS_AfterSaleDeliveryDetail>();

            //生成预付单
            CreateMap<tb_PurOrder, tb_FM_PreReceivedPayment>();
            //生成预收单
            CreateMap<tb_SaleOrder, tb_FM_PreReceivedPayment>();

            //费用报销单生成收款记录表
            CreateMap<tb_FM_ExpenseClaim, tb_FM_PaymentRecord>();

            //其它费用收入和支出生成收款记录表
            CreateMap<tb_FM_OtherExpense, tb_FM_PaymentRecord>();

            //预收单生成收款记录表
            CreateMap<tb_FM_PreReceivedPayment, tb_FM_PaymentRecord>();

            //生成核销表
            CreateMap<tb_FM_PaymentRecord, tb_FM_PaymentSettlement>();
            CreateMap<tb_FM_PaymentRecordDetail, tb_FM_PaymentSettlement>();

            //生成应收及明细
            CreateMap<tb_SaleOut, tb_FM_ReceivablePayable>();
            CreateMap<tb_SaleOutDetail, tb_FM_ReceivablePayableDetail>()
            .ForMember(a => a.UnitPrice, o => o.MapFrom(d => d.TransactionPrice))
            .ForMember(a => a.Quantity, o => o.MapFrom(d => d.Quantity))
            .ForMember(a => a.ProdDetailID, o => o.MapFrom(d => d.ProdDetailID))
              .ForMember(a => a.SourceItemRowID, o => o.MapFrom(d => d.SaleOutDetail_ID))
            .ForMember(a => a.property, o => o.MapFrom(d => d.property))
            .ForMember(a => a.CustomerPartNo, o => o.MapFrom(d => d.CustomerPartNo))
            .ForMember(a => a.TaxRate, o => o.MapFrom(d => d.TaxRate))
            .ForMember(a => a.TaxLocalAmount, o => o.MapFrom(d => d.SubtotalTaxAmount))
            .ForMember(a => a.LocalPayableAmount, o => o.MapFrom(d => d.SubtotalTransAmount))
            .ForMember(a => a.Summary, o => o.MapFrom(d => d.Summary));


            //生成应收及明细
            CreateMap<tb_SaleOutRe, tb_FM_ReceivablePayable>();
            CreateMap<tb_SaleOutReDetail, tb_FM_ReceivablePayableDetail>()
            .ForMember(a => a.UnitPrice, o => o.MapFrom(d => d.TransactionPrice))
            .ForMember(a => a.Quantity, o => o.MapFrom(d => d.Quantity))
            .ForMember(a => a.ProdDetailID, o => o.MapFrom(d => d.ProdDetailID))
              .ForMember(a => a.SourceItemRowID, o => o.MapFrom(d => d.SOutReturnDetail_ID))
            .ForMember(a => a.property, o => o.MapFrom(d => d.property))
            //.ForMember(a => a.IncludeTax, o => o.MapFrom(d => d.incl))
            .ForMember(a => a.CustomerPartNo, o => o.MapFrom(d => d.CustomerPartNo))
            .ForMember(a => a.TaxRate, o => o.MapFrom(d => d.TaxRate))
            .ForMember(a => a.TaxLocalAmount, o => o.MapFrom(d => d.SubtotalTaxAmount))
            .ForMember(a => a.LocalPayableAmount, o => o.MapFrom(d => d.SubtotalTransAmount))
            .ForMember(a => a.Summary, o => o.MapFrom(d => d.Summary));

            //生成应付及明细
            CreateMap<tb_FM_PriceAdjustment, tb_FM_ReceivablePayable>();
            CreateMap<tb_FM_PriceAdjustmentDetail, tb_FM_ReceivablePayableDetail>()
                  .ForMember(a => a.SourceItemRowID, o => o.MapFrom(d => d.AdjustDetailID))
                ;

            CreateMap<tb_SaleOut, tb_FM_PriceAdjustment>();
            CreateMap<tb_SaleOutDetail, tb_FM_PriceAdjustmentDetail>();
            CreateMap<tb_PurEntry, tb_FM_PriceAdjustment>();
            CreateMap<tb_PurEntryDetail, tb_FM_PriceAdjustmentDetail>();


            //生成应付及明细
            CreateMap<tb_PurEntry, tb_FM_ReceivablePayable>();
            CreateMap<tb_PurEntryDetail, tb_FM_ReceivablePayableDetail>()
                  .ForMember(a => a.SourceItemRowID, o => o.MapFrom(d => d.PurEntryDetail_ID));

            //生成应付及明细
            CreateMap<tb_PurEntryRe, tb_FM_ReceivablePayable>();
            CreateMap<tb_PurEntryReDetail, tb_FM_ReceivablePayableDetail>()
                  .ForMember(a => a.SourceItemRowID, o => o.MapFrom(d => d.PurEntryRe_CID));


            //应收付单生成收款记录表
            CreateMap<tb_FM_ReceivablePayable, tb_FM_PaymentRecord>();

            //应收付单生成收款记录表
            CreateMap<tb_FM_ReceivablePayableDetail, tb_FM_PaymentRecordDetail>();

            CreateMap<tb_FM_ReceivablePayable, tb_FM_ProfitLoss>();
            CreateMap<tb_FM_ReceivablePayableDetail, tb_FM_ProfitLossDetail>()
                .ForMember(a => a.ProdDetailID, o => o.MapFrom(d => d.ProdDetailID))
                .ForMember(a => a.property, o => o.MapFrom(d => d.property))
                .ForMember(a => a.SourceItemRowID, o => o.MapFrom(d => d.ARAPDetailID))
                .ForMember(a => a.ExpenseDescription, o => o.MapFrom(d => d.ExpenseDescription))
                .ForMember(a => a.UnitPrice, o => o.MapFrom(d => d.UnitPrice))
                .ForMember(a => a.Quantity, o => o.MapFrom(d => d.Quantity))
                .ForMember(a => a.SubtotalAmont, o => o.MapFrom(d => d.LocalPayableAmount))
                .ForMember(a => a.UntaxedSubtotalAmont, o => o.MapFrom(d => d.LocalPayableAmount - d.TaxLocalAmount))
                .ForMember(a => a.TaxRate, o => o.MapFrom(d => d.TaxRate))
                .ForMember(a => a.TaxSubtotalAmont, o => o.MapFrom(d => d.TaxLocalAmount))
               .ForMember(a => a.Summary, o => o.MapFrom(d => d.Summary));


            CreateMap<tb_Stocktake, tb_FM_ProfitLoss>();
            CreateMap<tb_StocktakeDetail, tb_FM_ProfitLossDetail>()
                .ForMember(a => a.ProdDetailID, o => o.MapFrom(d => d.ProdDetailID))
                .ForMember(a => a.property, o => o.MapFrom(d => d.property))
                .ForMember(a => a.SourceItemRowID, o => o.MapFrom(d => d.SubID))
                .ForMember(a => a.UnitPrice, o => o.MapFrom(d => d.Cost))
                .ForMember(a => a.Quantity, o => o.MapFrom(d => d.DiffQty))
                .ForMember(a => a.SubtotalAmont, o => o.MapFrom(d => d.DiffSubtotalAmount));

            CreateMap<tb_ProdBorrowing, tb_FM_ProfitLoss>();
            CreateMap<tb_ProdBorrowingDetail, tb_FM_ProfitLossDetail>()
                .ForMember(a => a.ProdDetailID, o => o.MapFrom(d => d.ProdDetailID))
                .ForMember(a => a.property, o => o.MapFrom(d => d.property))
                .ForMember(a => a.SourceItemRowID, o => o.MapFrom(d => d.BorrowDetaill_ID))
                .ForMember(a => a.UnitPrice, o => o.MapFrom(d => d.Cost))
                .ForMember(a => a.Quantity, o => o.MapFrom(d => d.Qty - d.ReQty))
                .ForMember(a => a.SubtotalAmont, o => o.MapFrom(d => d.SubtotalCostAmount));


            //多个应收应付 合并生成一个 付款单
            CreateMap<tb_FM_ReceivablePayable, tb_FM_PaymentRecordDetail>()
                .ForMember(a => a.SourceBilllId, o => o.MapFrom(d => d.ARAPId))
               .ForMember(a => a.SourceBillNo, o => o.MapFrom(d => d.ARAPNo))
               .ForMember(a => a.DepartmentID, o => o.MapFrom(d => d.DepartmentID))
               .ForMember(a => a.ExchangeRate, o => o.MapFrom(d => d.ExchangeRate))
               .ForMember(a => a.ForeignAmount, o => o.MapFrom(d => d.ForeignBalanceAmount))
               .ForMember(a => a.LocalAmount, o => o.MapFrom(d => d.LocalBalanceAmount))
               .ForMember(a => a.ProjectGroup_ID, o => o.MapFrom(d => d.ProjectGroup_ID))
               .ForMember(a => a.Currency_ID, o => o.MapFrom(d => d.Currency_ID));

            #endregion


            //销售订单转采购订单
            CreateMap<tb_SaleOrder, tb_PurOrder>()
            .ForMember(a => a.TotalQty, o => o.MapFrom(d => d.TotalQty));
            CreateMap<tb_SaleOrderDetail, tb_PurOrderDetail>()
              .ForMember(a => a.Quantity, o => o.MapFrom(d => d.Quantity))
              .ForMember(a => a.UnitPrice, o => o.MapFrom(d => d.Cost + d.CustomizedCost));



            //返工退库引用到返工入库的主单和明细中
            CreateMap<tb_MRP_ReworkReturn, tb_MRP_ReworkEntry>();
            CreateMap<tb_MRP_ReworkReturnDetail, tb_MRP_ReworkEntryDetail>();


            //缴库单引用到返工退库主单和明细中
            CreateMap<tb_FinishedGoodsInv, tb_MRP_ReworkReturn>();
            CreateMap<tb_FinishedGoodsInvDetail, tb_MRP_ReworkReturnDetail>();

            CreateMap<tb_ManufacturingOrder, tb_FinishedGoodsInv>();
            CreateMap<tb_ManufacturingOrder, tb_FinishedGoodsInvDetail>();
            //测试了订单转成入库单列表集合不用单设置也可以成功,List<T>  这种转。反而有问题？
            CreateMap<tb_PurOrder, tb_PurEntry>();

            CreateMap<BlogArticle, BlogViewModels>();
            CreateMap<BlogViewModels, BlogArticle>();
            CreateMap<tb_UnitDto, tb_Unit>();
            CreateMap<tb_Unit, tb_UnitDto>();
            //产品编辑时用 注意主键被覆盖问题
            CreateMap<tb_ProdDetail, Eav_ProdDetails>();

            CreateMap<tb_PurOrderDetail, tb_PurEntryDetail>();


            CreateMap<tb_PurEntry, tb_PurOrder>();
            CreateMap<tb_PurEntryDetail, tb_PurOrderDetail>();


            //从前到后，由前到后,将借出转为要归还的数据
            CreateMap<tb_ProdBorrowing, tb_ProdReturning>();
            CreateMap<tb_ProdBorrowingDetail, tb_ProdReturningDetail>();



            CreateMap<tb_SaleOrder, tb_SaleOut>();
            CreateMap<tb_SaleOrderDetail, tb_SaleOutDetail>();

            CreateMap<tb_MaterialRequisitionDetail, View_ProdDetail>();
            CreateMap<View_ProdDetail, tb_MaterialRequisitionDetail>();

            CreateMap<View_ProdDetail, tb_MaterialReturnDetail>();
            CreateMap<View_ProdDetail, tb_PackingDetail>();
            CreateMap<View_ProdDetail, tb_ProdBundleDetail>();

            CreateMap<View_ProdDetail, tb_ProdBorrowingDetail>();

            CreateMap<View_ProdDetail, tb_ProdReturningDetail>();

            CreateMap<View_ProdDetail, tb_StockTransferDetail>();
            CreateMap<View_ProdDetail, tb_PurEntryDetail>();


            CreateMap<tb_SaleOut, tb_SaleOutRe>();
            CreateMap<tb_SaleOutRe, tb_SaleOut>();
            CreateMap<tb_SaleOutDetail, tb_SaleOutReDetail>();

            CreateMap<tb_ManufacturingOrderDetail, View_ProdDetail>();
            CreateMap<View_ProdDetail, tb_ManufacturingOrderDetail>();

            CreateMap<tb_FinishedGoodsInvDetail, View_ProdDetail>();
            CreateMap<View_ProdDetail, tb_FinishedGoodsInvDetail>();


            CreateMap<tb_BOM_SDetail, View_ProdDetail>();
            CreateMap<View_ProdDetail, tb_BOM_SDetail>();

            CreateMap<tb_SaleOrderDetail, View_ProdDetail>();
            CreateMap<View_ProdDetail, tb_SaleOrderDetail>();


            CreateMap<tb_SaleOutReRefurbishedMaterialsDetail, View_ProdDetail>();
            CreateMap<View_ProdDetail, tb_SaleOutReRefurbishedMaterialsDetail>();

            CreateMap<tb_SaleOutReDetail, View_ProdDetail>();
            CreateMap<View_ProdDetail, tb_SaleOutReDetail>();

            CreateMap<tb_StocktakeDetail, View_ProdDetail>();
            CreateMap<View_ProdDetail, tb_StocktakeDetail>();

            CreateMap<tb_PurOrderDetail, View_ProdDetail>();
            CreateMap<View_ProdDetail, tb_PurOrderDetail>();


            CreateMap<tb_BOM_SDetail, tb_ProdSplitDetail>();
            CreateMap<tb_ProdSplitDetail, tb_BOM_SDetail>();

            CreateMap<tb_BOM_SDetail, tb_ProdMergeDetail>();
            CreateMap<tb_ProdMergeDetail, tb_BOM_SDetail>();

            CreateMap<tb_ProdSplitDetail, View_ProdDetail>();
            CreateMap<View_ProdDetail, tb_ProdSplitDetail>();

            CreateMap<tb_ProdMergeDetail, View_ProdDetail>();
            CreateMap<View_ProdDetail, tb_ProdMergeDetail>();


            CreateMap<tb_PurEntry, tb_PurEntryRe>();
            CreateMap<tb_PurEntryDetail, tb_PurEntryReDetail>();


            CreateMap<tb_PurEntryRe, tb_PurEntry>();
            CreateMap<tb_PurEntryReDetail, tb_PurEntryDetail>();


            CreateMap<tb_PurEntryRe, tb_PurReturnEntry>();
            CreateMap<tb_PurEntryReDetail, tb_PurReturnEntryDetail>();


            CreateMap<tb_StockInDetail, View_ProdDetail>();
            CreateMap<View_ProdDetail, tb_StockInDetail>();

            CreateMap<tb_StockOutDetail, View_ProdDetail>();
            CreateMap<View_ProdDetail, tb_StockOutDetail>();
            CreateMap<View_ProdDetail, tb_PackingDetail>();


            CreateMap<tb_SaleOrder, tb_ProductionPlan>();
            CreateMap<tb_SaleOrderDetail, tb_ProductionPlanDetail>();
            CreateMap<tb_ProductionPlan, tb_SaleOrder>();
            CreateMap<tb_ProductionPlanDetail, tb_SaleOrderDetail>();


            CreateMap<tb_ProductionPlan, tb_ProductionDemand>();
            CreateMap<tb_ProductionPlanDetail, tb_ProductionDemandDetail>();

            CreateMap<tb_BOM_SDetail, tb_BOM_SDetailTree>()
            .ForMember(dest => dest.FieldNameList, opt => opt.Ignore());//排除字段 忽略指定集合字段


            //ui TO ENTITY
            CreateMap<Eav_ProdDetails, tb_ProdDetail>()
            //如果希望忽略具有空值的所有源属性,则可以使用:
            .ForAllMembers(opt => opt.Condition(srs => !srs.IsNullOrEmpty()));

            CreateMap<tb_ProductionDemandTargetDetail, tb_ProductionDemandDetail>()
            .ForMember(a => a.NeedQuantity, o => o.MapFrom(d => d.NeedQuantity))
            //毛需求是原始的需求数量，而净需求是经过库存调整后的实际需求数量。   净需求 = 毛需求 - 库存 + 已预订量 - 在途量
            .ForMember(a => a.NetRequirement, o => o.MapFrom(d => (d.NeedQuantity - d.BookInventory) > 0 ? d.NeedQuantity - d.BookInventory : 0))
            .ForMember(a => a.GrossRequirement, o => o.MapFrom(d => d.NeedQuantity))
            .ForMember(a => a.Sale_Qty, o => o.MapFrom(d => d.SaleQty))
            .ForMember(a => a.NotOutQty, o => o.MapFrom(d => d.NotIssueMaterialQty))
            .ForMember(a => a.MakeProcessInventory, o => o.MapFrom(d => d.MakeProcessInventory))
            ;

            CreateMap<tb_ProductionPlanDetail, tb_ProductionDemandTargetDetail>()
            .ForMember(a => a.NeedQuantity, o => o.MapFrom(d => d.Quantity));


            CreateMap<View_Inventory, tb_ProductionDemandDetail>()
            .ForMember(a => a.BookInventory, o => o.MapFrom(d => d.Quantity))
            .ForMember(a => a.AvailableStock, o => o.MapFrom(d => d.Quantity - d.Sale_Qty))
            .ForMember(a => a.InTransitInventory, o => o.MapFrom(d => d.Quantity - d.On_the_way_Qty))
            .ForMember(a => a.MakeProcessInventory, o => o.MapFrom(d => d.Quantity - d.MakingQty))
            ;

            CreateMap<tb_ProductionDemandDetail, tb_ManufacturingOrderDetail>()
            .ForMember(a => a.ShouldSendQty, o => o.MapFrom(d => d.NeedQuantity));

            CreateMap<tb_ProductionDemandDetail, tb_PurGoodsRecommendDetail>()
            .ForMember(a => a.PDCID_RowID, o => o.MapFrom(d => d.ID))//这个ID不是主键，只是用于库存不足中树的显示，以及用于合并时关联ID,因为生成时主键还没有保存到DB。没值
            .ForMember(a => a.RecommendQty, o => o.MapFrom(d => d.NeedQuantity))
            .ForMember(a => a.RequirementQty, o => o.MapFrom(d => d.MissingQuantity));


            CreateMap<tb_ProductionDemandDetail, tb_ProduceGoodsRecommendDetail>()
            .ForMember(a => a.RecommendQty, o => o.MapFrom(d => d.MissingQuantity))
            .ForMember(a => a.PlanNeedQty, o => o.MapFrom(d => d.NeedQuantity))
            .ForMember(a => a.RequirementQty, o => o.MapFrom(d => d.MissingQuantity));


            CreateMap<tb_ProductionDemand, tb_BuyingRequisition>()
            .ForMember(a => a.RefBillID, o => o.MapFrom(d => d.PDID))
            .ForMember(a => a.RefBillNO, o => o.MapFrom(d => d.PDNo));



            CreateMap<tb_PurGoodsRecommendDetail, tb_BuyingRequisitionDetail>()
            .ForMember(a => a.RequirementDate, o => o.MapFrom(d => d.RequirementDate))
            .ForMember(a => a.Quantity, o => o.MapFrom(d => d.RequirementQty));

            CreateMap<tb_ProductionPlanDetail, View_ProdDetail>();
            CreateMap<View_ProdDetail, tb_ProductionPlanDetail>();

            CreateMap<tb_ProductionDemandDetail, View_ProdDetail>();
            CreateMap<View_ProdDetail, tb_ProductionDemandDetail>();

            CreateMap<View_ProdDetail, BaseProductInfo>();

            CreateMap<View_ProdDetail, tb_BuyingRequisitionDetail>();



            //由需求分析主表生成制令单主表？
            CreateMap<tb_ProduceGoodsRecommendDetail, tb_ManufacturingOrder>();

            //由需求分析主表生成制令单主表？
            CreateMap<tb_ProductionDemandDetail, tb_ManufacturingOrder>();

            //由请购单生成采购订单明细
            CreateMap<tb_BuyingRequisitionDetail, tb_PurOrderDetail>();


            //由目标生成只有BOM的，不需要到最低一层级的制成品
            CreateMap<tb_ProductionDemandTargetDetail, tb_ProduceGoodsRecommendDetail>();

            CreateMap<tb_ProduceGoodsRecommendDetail, tb_ManufacturingOrderDetail>();





            //如果需求分析单已经建好保存后，再修改BOM添加了材料后。按BOM明细到分析单明细中找不到时。则直接由BOM明细生成制令单明细行
            CreateMap<tb_BOM_SDetail, tb_ManufacturingOrderDetail>();

            //库存不足的 生成
            CreateMap<tb_ManufacturingOrderDetail, tb_MaterialRequisitionDetail>();
            CreateMap<tb_ManufacturingOrder, tb_MaterialRequisition>();


            //领取料单转退料单
            CreateMap<tb_MaterialRequisition, tb_MaterialReturn>();

            //领取料单明细转退料明细
            CreateMap<tb_MaterialRequisitionDetail, tb_MaterialReturnDetail>();

            //领取料单明细转退料明细
            CreateMap<tb_MaterialReturnDetail, tb_MaterialRequisitionDetail>();

            //意思是转换时为空则给默认值?
            //CreateMap<tb_Unit, tb_UnitDto>().ForMember(destination => destination.UnitName, opt => opt.NullSubstitute("缺少值名字")); ;

            //线索转目标客户
            CreateMap<tb_CRM_Leads, tb_CRM_Customer>()
            .ForMember(a => a.CustomerName, o => o.MapFrom(d => d.CustomerName));
            //目标客户转销售客户
            CreateMap<tb_CRM_Customer, tb_CustomerVendor>()
                 .ForMember(a => a.CVName, o => o.MapFrom(d => d.CustomerName))
                 .ForMember(a => a.Employee_ID, o => o.MapFrom(d => d.Employee_ID))
                 .ForMember(a => a.Customer_id, o => o.MapFrom(d => d.Customer_id))
                 .ForMember(a => a.Contact, o => o.MapFrom(d => d.Contact_Name))
                 .ForMember(a => a.Phone, o => o.MapFrom(d => d.Contact_Phone))
                 .ForMember(a => a.Address, o => o.MapFrom(d => d.CustomerAddress))
                 .ForMember(a => a.Website, o => o.MapFrom(d => d.Website))
                 .ForMember(a => a.Notes, o => o.MapFrom(d => d.Notes))
                ;

            //计划转为记录
            CreateMap<tb_CRM_FollowUpPlans, tb_CRM_FollowUpRecords>();


            //为了修复数据=销售客户转目标客户
            CreateMap<tb_CustomerVendor, tb_CRM_Customer>()
                .ForMember(a => a.CustomerName, o => o.MapFrom(d => d.CVName))
                .ForMember(a => a.Employee_ID, o => o.MapFrom(d => d.Employee_ID))
                .ForMember(a => a.Contact_Name, o => o.MapFrom(d => d.Contact))
                 .ForMember(a => a.Contact_Phone, o => o.MapFrom(d => d.Phone))
                 .ForMember(a => a.CustomerAddress, o => o.MapFrom(d => d.Address))
                 .ForMember(a => a.Website, o => o.MapFrom(d => d.Website))
                 .ForMember(a => a.Notes, o => o.MapFrom(d => d.Notes))
            ;
        }





    }
}

