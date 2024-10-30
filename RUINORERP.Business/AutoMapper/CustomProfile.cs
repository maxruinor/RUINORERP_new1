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





            CreateMap<tb_SaleOut, tb_SaleOutRe>();
            CreateMap<tb_SaleOutRe, tb_SaleOut>();
            CreateMap<tb_SaleOutDetail, tb_SaleOutReDetail>();



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

        }
    }
}

