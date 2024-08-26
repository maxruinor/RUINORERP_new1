using RUINORERP.Global;
using RUINORERP.Model.QueryDto;
using RUINORERP.Model;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoMapper;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.Processor;
using RUINORERP.Business;
using RUINORERP.Common.Helper;
using RUINORERP.Model.Dto;
using RUINORERP.UI.UCSourceGrid;
using SqlSugar;
using System.Linq.Expressions;
using RUINOR.Core;
using RUINORERP.UI.ToolForm;
using MathNet.Numerics.LinearAlgebra.Factorization;
using FastReport.Utils;
using Krypton.Toolkit.Suite.Extended.TreeGridView;
using HLH.Lib.List;
using RUINORERP.Common.Extensions;
using System.Collections.Concurrent;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using MySqlX.XDevAPI.Common;
using RUINORERP.UI.PSI.PUR;
using System.Security.Cryptography;
using ZXing.Common;
using FastReport.DevComponents.DotNetBar.Controls;
using Krypton.Toolkit;
using FastReport.DevComponents.DotNetBar;
using Command = RUINOR.Core.Command;
using NPOI.SS.Formula.Functions;
using NPOI.Util;
using NPOI.POIFS.Properties;

namespace RUINORERP.UI.MRP.MP
{

    //bindingSourceSub  在基在中。是普通的主子表。这里有多个子表暂时不使用基类个的这个组件。全部在子类中新建数据源。并且实现 保存  删除等功能。
    [MenuAttrAssemblyInfo("生产需求分析", ModuleMenuDefine.模块定义.生产管理, ModuleMenuDefine.生产管理.制造规划, BizType.生产需求分析)]
    public partial class UCProduceRequirement : BaseBillEditGeneric<tb_ProductionDemand, tb_ProductionDemand>
    {
        public UCProduceRequirement()
        {
            InitializeComponent();
            // InitDataToCmbByEnumDynamicGeneratedDataSource<tb_ProductionDemand>(typeof(Priority), e => e.Priority, cmbOrderPriority, false);
            base.OnBindDataToUIEvent += UcSaleOrderEdit_OnBindDataToUIEvent;
        }


        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_ProductionDemand).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();

            ////创建表达式
            //var lambda = Expressionable.Create<tb_FM_OtherExpense>()
            //                 //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
            //                 // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
            //                 .And(t => t.isdeleted == false)
            //                    .And(t => t.EXPOrINC == false)
            //                // .And(t => t.Is_enabled == true)
            //                //报销人员限制，财务不限制
            //                //  .AndIF(MainForm.Instance.AppContext.SysConfig.SaleBizLimited && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
            //                .ToExpression();//注意 这一句 不能少
            //QueryConditionFilter.SetFieldLimitCondition(lambda);
        }

        private void UcSaleOrderEdit_OnBindDataToUIEvent(tb_ProductionDemand entity)
        {
            ClearData();
            BindData(entity as BaseEntity);
        }


        internal override void LoadDataToUI(object Entity)
        {
            BindData(Entity as BaseEntity);
        }

        DateTime RequirementDate = System.DateTime.Now;
        public override void BindData(BaseEntity entityPara)
        {
            tb_ProductionDemand entity = entityPara as tb_ProductionDemand;
            if (entity == null)
            {
                return;
            }

            if (entity != null)
            {
                if (entity.PDID > 0)
                {
                    entity.PrimaryKeyID = entity.PDID;
                    entity.ActionStatus = ActionStatus.加载;
                    // entity.DataStatus = (int)DataStatus.确认;
                    //如果审核了，审核要灰色
                }
                else
                {
                    entity.ActionStatus = ActionStatus.新增;
                    entity.DataStatus = (int)DataStatus.草稿;
                    if (entity.PDNo.IsNullOrEmpty())
                    {
                        entity.PDNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.生产需求分析);
                    }
                    entity.AnalysisDate = System.DateTime.Now;
                }
            }

            if (entity.ApprovalStatus.HasValue)
            {
                lblReview.Text = ((ApprovalStatus)entity.ApprovalStatus).ToString();
            }
            EditEntity = entity;
            DataBindingHelper.BindData4TextBox<tb_ProductionDemand>(entity, t => t.PDNo, txtPDNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4DataTime<tb_ProductionDemand>(entity, t => t.AnalysisDate, dtpAnalysisDate, false);
            DataBindingHelper.BindData4TextBox<tb_ProductionDemand>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ProductionDemand>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_ProductionDemand>(entity, t => t.PurAllItems, chkPurAllItems, false);

            base.errorProviderForAllInput.DataSource = entity;
            base.errorProviderForAllInput.ContainerControl = this;
            //this.ValidateChildren();
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            DataBindingHelper.BindData4ControlByEnum<tb_ProductionDemand>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_ProductionDemand>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));
            //目标
            if (entity.tb_ProductionDemandTargetDetails != null && entity.tb_ProductionDemandTargetDetails.Count > 0)
            {
                sghTarget.LoadItemDataToGrid<tb_ProductionDemandTargetDetail>(gridTargetItems, sgdTarget, entity.tb_ProductionDemandTargetDetails, c => c.ProdDetailID);
            }
            else
            {
                sghTarget.LoadItemDataToGrid<tb_ProductionDemandTargetDetail>(gridTargetItems, sgdTarget, new List<tb_ProductionDemandTargetDetail>(), c => c.ProdDetailID);
            }

            //库存不足
            if (entity.tb_ProductionDemandDetails != null && entity.tb_ProductionDemandDetails.Count > 0)
            {
                LoadTreeGridViewStockLess(entity.tb_ProductionDemandDetails);
            }

            //采购建议
            if (entity.tb_PurGoodsRecommendDetails != null && entity.tb_PurGoodsRecommendDetails.Count > 0)
            {
                sghPur.LoadItemDataToGrid<tb_PurGoodsRecommendDetail>(gridPurItems, sgdPur, entity.tb_PurGoodsRecommendDetails, c => c.ProdDetailID);
                foreach (var PurItem in entity.tb_PurGoodsRecommendDetails)
                {
                    //Command commandPurItem = new Command();
                    ////缓存当前编辑的对象。如果撤销就回原来的值
                    //tb_PurGoodsRecommendDetail oldobj = CloneHelper.DeepCloneObject<tb_PurGoodsRecommendDetail>(PurItem);
                    //commandPurItem.UndoOperation = delegate ()
                    //{
                    //    //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                    //    CloneHelper.SetValues<tb_PurGoodsRecommendDetail>(PurItem, oldobj);
                    //};

                    PurItem.PropertyChanged += (sender, s2) =>
                    {
                        //如果是计划生产单引入变化则加载明细及相关数据
                        if ((entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改) && PurItem.RequirementQty > 0 && s2.PropertyName == PurItem.GetPropertyName<tb_PurGoodsRecommendDetail>(c => c.RequirementQty))
                        {
                            if (PurItem.RequirementQty < PurItem.RecommendQty)
                            {
                                MessageBox.Show($"{PurItem.ProdDetailID}请购量不能小于建议数量！");
                                PurItem.RequirementQty = PurItem.RecommendQty;
                            }
                        }
                    };
                }
                //控制是否能再次生成请购单

            }
            else
            {
                sghPur.LoadItemDataToGrid<tb_PurGoodsRecommendDetail>(gridPurItems, sgdPur, new List<tb_PurGoodsRecommendDetail>(), c => c.ProdDetailID);
            }

            //自建品建议 也以树形结构展示
            if (entity.tb_ProduceGoodsRecommendDetails != null && entity.tb_ProduceGoodsRecommendDetails.Count > 0)
            {
                LoadTreeGridViewProduceItems();
                if (entity.ApprovalResults.HasValue && entity.ApprovalResults.Value)
                {
                    btnCreatePurRequisition.Enabled = true;
                    btnCreateProduction.Enabled = true;
                }
            }

            DataBindingHelper.BindData4RadioGroupTrueFalse<tb_ProductionDemand>(entity, t => t.SuggestBasedOn, rdbis_available净需求, rdbis_available毛需求);

            //先绑定这个。InitFilterForControl 这个才生效
            DataBindingHelper.BindData4TextBox<tb_ProductionDemand>(entity, v => v.PPNo, txtRefBillNO, BindDataType4TextBox.Text, true);

            DataBindingHelper.BindData4TextBoxWithTagQuery<tb_ProductionDemand>(entity, v => v.PPID, txtRefBillNO, true);


            BaseProcessor basePro = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_ProductionPlan).Name + "Processor");
            QueryFilter queryFilter = basePro.GetQueryFilter();

            var lambdaOrder = Expressionable.Create<tb_ProductionPlan>()
             .And(t => t.DataStatus == (int)DataStatus.确认)
              .And(t => t.isdeleted == false)
                 .And(t => t.Analyzed == false)
             .ToExpression();//注意 这一句 不能少
            //如果有限制则设置一下 但是注意 不应该在这设置，灵活的应该是在调用层设置
            queryFilter.SetFieldLimitCondition(lambdaOrder);

            DataBindingHelper.InitFilterForControlByExp<tb_ProductionPlan>(entity, txtRefBillNO, c => c.PPNo, queryFilter, null);


            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(new tb_ProductionDemandValidator(), kryptonPanelMainInfo.Controls);
                // base.InitEditItemToControl(entity, kryptonPanelMainInfo.Controls);
                rdbis_available净需求.Enabled = true;
                rdbis_available毛需求.Enabled = true;
            }
            else
            {
                rdbis_available净需求.Enabled = false;
                rdbis_available毛需求.Enabled = false;
            }
            ToolBarEnabledControl(entity);

            //如果属性变化 则状态为修改
            entity.PropertyChanged += async (sender, s2) =>
            {
                //权限允许
                if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
                {
                    entity.ActionStatus = ActionStatus.修改;
                    base.ToolBarEnabledControl(MenuItemEnums.修改);
                }
                else
                {
                    rdbis_available净需求.Enabled = false;
                    rdbis_available毛需求.Enabled = false;
                }


                //如果是计划生产单引入变化则加载明细及相关数据
                if ((entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改) && entity.PPID > 0 && s2.PropertyName == entity.GetPropertyName<tb_ProductionDemand>(c => c.PPID))
                {
                    await LoadPlanChildItems(entity.PPID);
                }
                if ((entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改) && s2.PropertyName == entity.GetPropertyName<tb_ProductionDemand>(c => c.SuggestBasedOn))
                {
                    //在切换前保存一下是否折叠，
                    //bool isExpand = kryptonTreeGridViewMaking.;// kryptonTreeGridViewMaking.ExpandAll();
                    //净重毛重依据变化时会影响
                    AnalysisTargetItems();
                    kryptonTreeGridViewMaking.ExpandAll();
                }




                //数据状态变化会影响按钮变化
                if (s2.PropertyName == entity.GetPropertyName<tb_ProductionDemand>(c => c.DataStatus))
                {
                    ToolBarEnabledControl(entity);

                    ControlBtnFuncation(entity, s2);
                }


                //如果客户有变化，带出对应有业务员

                //显示 打印状态 如果是草稿状态 不显示打印
                if ((DataStatus)EditEntity.DataStatus != DataStatus.草稿)
                {
                    toolStripbtnPrint.Enabled = true;
                    if (EditEntity.PrintStatus == 0)
                    {
                        lblPrintStatus.Text = "未打印";
                    }
                    else
                    {
                        lblPrintStatus.Text = $"打印{EditEntity.PrintStatus}次";
                    }

                }
                else
                {
                    toolStripbtnPrint.Enabled = false;
                }
            };

            ControlBtnFuncation(entity);
        }


        private void ControlBtnFuncation(tb_ProductionDemand demand, PropertyChangedEventArgs s2 = null)
        {
            //如果审核过的，不允许修改
            if (demand.ApprovalStatus.HasValue)
            {
                if (demand.ApprovalStatus.Value == (int)ApprovalStatus.已审核)
                {
                    btnCreatePurRequisition.Enabled = false;
                    btnCreateProduction.Enabled = false;
                    btnAnalysis.Enabled = false;
                    btn产生建议.Enabled = false;
                }
            }
            else
            {
                btnAnalysis.Enabled = true;
                btn产生建议.Enabled = true;
            }

            DataStatus dataStatus = (DataStatus)demand.DataStatus;
            switch (dataStatus)
            {
                case DataStatus.草稿:
                    btnCreatePurRequisition.Enabled = false;
                    btnCreateProduction.Enabled = false;

                    btnAnalysis.Enabled = true;
                    btn产生建议.Enabled = true;
                    break;
                case DataStatus.新建:
                    btnAnalysis.Enabled = true;
                    btn产生建议.Enabled = true;
                    btnCreatePurRequisition.Enabled = false;
                    btnCreateProduction.Enabled = false;
                    break;
                case DataStatus.确认:
                    //同时 要查询是否有已经生成的请购单和制令单


                    btnCreatePurRequisition.Enabled = true;



                    btnCreateProduction.Enabled = true;



                    //审核过的。不能再分析和产生建议
                    btnAnalysis.Enabled = false;
                    btn产生建议.Enabled = false;

                    break;
                case DataStatus.完结:
                    btnCreatePurRequisition.Enabled = false;
                    btnCreateProduction.Enabled = false;

                    btnAnalysis.Enabled = false;
                    btn产生建议.Enabled = false;
                    break;
                default:
                    break;
            }
            if (dataStatus == DataStatus.草稿 || dataStatus == DataStatus.完结)
            {
                return;
            }
            //变化时
            if (s2 != null)
            {
                ////只有保存过才会显示生成请购单和制令单
                //if ((demand.actionStatus == ActionStatus.新增 || demand.actionStatus == ActionStatus.修改) && demand.PDID > 0 && s2.PropertyName == demand.GetPropertyName<tb_ProductionDemand>(c => c.PDID))
                //{
                //    btnCreatePurRequisition.Enabled = true;
                //    btnCreateProduction.Enabled = true;
                //}


                //if (demand.ApprovalResults.HasValue && demand.ApprovalResults.Value)
                //{
                //    btnCreatePurRequisition.Enabled = true;
                //    btnCreateProduction.Enabled = true;
                //}
                //else
                //{
                //    btnCreatePurRequisition.Enabled = false;
                //    btnCreateProduction.Enabled = false;
                //}
            }


        }


        //设计关联列和目标列
        View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
        List<View_ProdDetail> list = new List<View_ProdDetail>();

        #region for 目标分析

        SourceGridDefine sgdTarget = null;
        //        SourceGridHelper<View_ProdDetail, tb_ProductionPlanDetail> sgh = new SourceGridHelper<View_ProdDetail, tb_ProductionPlanDetail>();
        SourceGridHelper sghTarget = new SourceGridHelper();
        List<SourceGridDefineColumnItem> listColsTarget = new List<SourceGridDefineColumnItem>();

        public void LoadTargetItems()
        {
            gridTargetItems.BorderStyle = BorderStyle.FixedSingle;
            gridTargetItems.Selection.EnableMultiSelection = false;

            //指定了关键字段ProdDetailID
            listColsTarget = sghTarget.GetGridColumns<ProductSharePart, tb_ProductionDemandTargetDetail>(c => c.ProdDetailID, false);

            listColsTarget.SetCol_NeverVisible<ProductSharePart>(c => c.Rack_ID);
            listColsTarget.SetCol_NeverVisible<ProductSharePart>(c => c.Standard_Price);
            listColsTarget.SetCol_NeverVisible<ProductSharePart>(c => c.Inv_Cost);
            listColsTarget.SetCol_NeverVisible<tb_ProductionDemandTargetDetail>(c => c.ProdDetailID);
            listColsTarget.SetCol_NeverVisible<tb_ProductionDemandTargetDetail>(c => c.AvailableStock);
            //listCols.SetCol_NeverVisible<tb_ProductionDemandDetail>(c => c.BOM_ID);
            if (!AppContext.SysConfig.UseBarCode)
            {
                listColsTarget.SetCol_NeverVisible<ProductSharePart>(c => c.BarCode);
            }
            ControlChildColumnsInvisible(listColsTarget);


            //如果库位为只读  暂时只会显示 ID
            //listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Location_ID);

            //listCols.SetCol_ReadOnly<tb_ProductionDemandDetail>(c => c.CompletedQuantity);

            sgdTarget = new SourceGridDefine(gridTargetItems, listColsTarget, true);


            listColsTarget.SetCol_ReadOnly<tb_ProductionDemandTargetDetail>(c => c.AvailableStock);
            listColsTarget.SetCol_ReadOnly<tb_ProductionDemandTargetDetail>(c => c.BookInventory);
            listColsTarget.SetCol_ReadOnly<tb_ProductionDemandTargetDetail>(c => c.InTransitInventory);
            listColsTarget.SetCol_ReadOnly<tb_ProductionDemandTargetDetail>(c => c.MakeProcessInventory);
            listColsTarget.SetCol_ReadOnly<tb_ProductionDemandTargetDetail>(c => c.NotIssueMaterialQty);
            listColsTarget.SetCol_ReadOnly<tb_ProductionDemandTargetDetail>(c => c.SaleQty);


            listColsTarget.SetCol_Summary<tb_ProductionDemandTargetDetail>(c => c.NeedQuantity);
            listColsTarget.SetCol_Summary<tb_ProductionDemandTargetDetail>(c => c.AvailableStock);
            listColsTarget.SetCol_Summary<tb_ProductionDemandTargetDetail>(c => c.BookInventory);
            listColsTarget.SetCol_Summary<tb_ProductionDemandTargetDetail>(c => c.InTransitInventory);
            listColsTarget.SetCol_Summary<tb_ProductionDemandTargetDetail>(c => c.MakeProcessInventory);
            listColsTarget.SetCol_Summary<tb_ProductionDemandTargetDetail>(c => c.NotIssueMaterialQty);
            listColsTarget.SetCol_Summary<tb_ProductionDemandTargetDetail>(c => c.SaleQty);

            //需求日期，默认加5天
            listColsTarget.SetCol_DefaultValue<tb_ProductionDemandTargetDetail>(c => c.RequirementDate, DateTime.Now.AddDays(5).ToString("yyyy-MM-dd"));

            listColsTarget.SetCol_DefaultHide<ProductSharePart>(c => c.Image);
            listColsTarget.SetCol_DefaultHide<ProductSharePart>(c => c.Brand);
            listColsTarget.SetCol_DefaultHide<ProductSharePart>(c => c.BarCode);
            listColsTarget.SetCol_DefaultHide<ProductSharePart>(c => c.ShortCode);
            if (CurMenuInfo.tb_P4Fields != null)
            {
                foreach (var item in CurMenuInfo.tb_P4Fields.Where(p => p.tb_fieldinfo.IsChild && !p.IsVisble))
                {
                    //listCols.SetCol_NeverVisible(item.tb_fieldinfo.FieldName);
                    listColsTarget.SetCol_NeverVisible(item.tb_fieldinfo.FieldName, typeof(tb_ProductionDemandTargetDetail));
                }

            }
            /*
            //具体审核权限的人才显示
            if (AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {

            }*/

            sghTarget.SetQueryItemToColumnPairs<View_ProdDetail, tb_ProductionDemandTargetDetail>(sgdTarget, f => f.BOM_ID, t => t.BOM_ID);
            sghTarget.SetQueryItemToColumnPairs<View_ProdDetail, tb_ProductionDemandTargetDetail>(sgdTarget, f => f.Quantity, t => t.BookInventory);
            sghTarget.SetQueryItemToColumnPairs<View_ProdDetail, tb_ProductionDemandTargetDetail>(sgdTarget, f => f.MakingQty, t => t.MakeProcessInventory);
            sghTarget.SetQueryItemToColumnPairs<View_ProdDetail, tb_ProductionDemandTargetDetail>(sgdTarget, f => f.On_the_way_Qty, t => t.InTransitInventory);
            sghTarget.SetQueryItemToColumnPairs<View_ProdDetail, tb_ProductionDemandTargetDetail>(sgdTarget, f => f.Sale_Qty, t => t.SaleQty);
            sghTarget.SetQueryItemToColumnPairs<View_ProdDetail, tb_ProductionDemandTargetDetail>(sgdTarget, f => f.NotOutQty, t => t.NotIssueMaterialQty);
            //bomid的下拉值。受当前行选择时会改变下拉范围
            sghTarget.SetCol_LimitedConditionsForSelectionRange<tb_ProductionDemandTargetDetail>(sgdTarget, t => t.ProdDetailID, f => f.BOM_ID);

            //公共到明细的映射 源 ，左边会隐藏
            sghTarget.SetPointToColumnPairs<ProductSharePart, tb_ProductionDemandTargetDetail>(sgdTarget, f => f.Location_ID, t => t.Location_ID);
            sghTarget.SetPointToColumnPairs<ProductSharePart, tb_ProductionDemandTargetDetail>(sgdTarget, f => f.prop, t => t.property);
            sghTarget.SetPointToColumnPairs<ProductSharePart, tb_ProductionDemandTargetDetail>(sgdTarget, f => f.Specifications, t => t.Specifications);
            listColsTarget.SetCol_NeverVisible<tb_ProductionDemandTargetDetail>(c => c.property);

            //应该只提供一个结构
            List<tb_ProductionDemandTargetDetail> lines = new List<tb_ProductionDemandTargetDetail>();
            bindingSourceTarget.DataSource = lines;
            sgdTarget.BindingSourceLines = bindingSourceTarget;

            sgdTarget.SetDependencyObject<ProductSharePart, tb_ProductionDemandTargetDetail>(list);
            sgdTarget.HasRowHeader = true;
            sghTarget.InitGrid(gridTargetItems, sgdTarget, true, nameof(tb_ProductionDemandTargetDetail));
        }


        #endregion

        #region for 库存不足



        #endregion

        #region for 采购建议

        SourceGridDefine sgdPur = null;
        //        SourceGridHelper<View_ProdDetail, tb_ProductionPlanDetail> sgh = new SourceGridHelper<View_ProdDetail, tb_ProductionPlanDetail>();
        SourceGridHelper sghPur = new SourceGridHelper();

        List<SourceGridDefineColumnItem> listColsPur = new List<SourceGridDefineColumnItem>();

        public void LoadPurItems()
        {

            gridPurItems.BorderStyle = BorderStyle.FixedSingle;
            gridPurItems.Selection.EnableMultiSelection = false;


            //指定了关键字段ProdDetailID
            //listColsPur = sghPur.GetGridColumns<ProductSharePart, tb_PurGoodsRecommendDetail>(c => c.ProdDetailID, false);
            listColsPur = sghPur.GetGridColumns<ProductSharePart, tb_PurGoodsRecommendDetail, InventoryInfo>(c => c.ProdDetailID, false);

            listColsPur.SetCol_NeverVisible<ProductSharePart>(c => c.Rack_ID);
            listColsPur.SetCol_NeverVisible<ProductSharePart>(c => c.Standard_Price);
            listColsPur.SetCol_NeverVisible<ProductSharePart>(c => c.Inv_Cost);
            listColsPur.SetCol_NeverVisible<tb_PurGoodsRecommendDetail>(c => c.ProdDetailID);
            listColsPur.SetCol_ReadOnly<tb_PurGoodsRecommendDetail>(c => c.RecommendQty);

            //listCols.SetCol_NeverVisible<tb_ProductionDemandDetail>(c => c.BOM_ID);
            if (!AppContext.SysConfig.UseBarCode)
            {
                listColsPur.SetCol_NeverVisible<ProductSharePart>(c => c.BarCode);
            }
            ControlChildColumnsInvisible(listColsPur);
            //listCols.SetCol_DefaultValue<tb_ProductionPlanDetail>(a => a.TaxRate, 0.13m);//m =>decial d=>double

            //如果库位为只读  暂时只会显示 ID
            //listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Location_ID);

            //listCols.SetCol_ReadOnly<tb_ProductionDemandDetail>(c => c.CompletedQuantity);
            //为了计划请购量是否小于建议量

            sgdPur = new SourceGridDefine(gridPurItems, listColsPur, true);
            //必须在定下后面
            listColsPur.SetCol_Formula<tb_PurGoodsRecommendDetail>((a) => a.RequirementQty * 1, c => c.RequirementQty);
            listColsPur.SetCol_Summary<tb_PurGoodsRecommendDetail>(c => c.RequirementQty);



            if (CurMenuInfo.tb_P4Fields != null)
            {
                foreach (var item in CurMenuInfo.tb_P4Fields.Where(p => p.tb_fieldinfo.IsChild && !p.IsVisble))
                {
                    //listCols.SetCol_NeverVisible(item.tb_fieldinfo.FieldName);
                    listColsPur.SetCol_NeverVisible(item.tb_fieldinfo.FieldName, typeof(tb_PurGoodsRecommendDetail));
                }

            }
            /*
            //具体审核权限的人才显示
            if (AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {

            }*/

            //公共到明细的映射 源 ，左边会隐藏
            sghPur.SetPointToColumnPairs<ProductSharePart, tb_PurGoodsRecommendDetail>(sgdPur, f => f.Location_ID, t => t.Location_ID);
            sghPur.SetPointToColumnPairs<ProductSharePart, tb_PurGoodsRecommendDetail>(sgdPur, f => f.prop, t => t.property);


            //sghPur.SetPointToColumnPairs<ProductSharePart, InventoryInfo>(sgdPur, f => f., t => t.property);
            //sghPur.SetPointToColumnPairs<ProductSharePart, InventoryInfo>(sgdPur, f => f.prop, t => t.property);
            //sghPur.SetPointToColumnPairs<ProductSharePart, InventoryInfo>(sgdPur, f => f.prop, t => t.property);
            //sghPur.SetPointToColumnPairs<ProductSharePart, InventoryInfo>(sgdPur, f => f.prop, t => t.property);
            //sghPur.SetPointToColumnPairs<ProductSharePart, InventoryInfo>(sgdPur, f => f.prop, t => t.property);

            // sgh.SetPointToColumnPairs<ProductSharePart, tb_ProductionDemandDetail>(sgd, f => f.prop, t => t.property);






            //应该只提供一个结构
            List<tb_PurGoodsRecommendDetail> lines = new List<tb_PurGoodsRecommendDetail>();
            bindingSourcePurchase.DataSource = lines;
            sgdPur.BindingSourceLines = bindingSourcePurchase;

            sgdPur.SetDependencyObject<ProductSharePart, tb_PurGoodsRecommendDetail>(list);
            sgdPur.HasRowHeader = true;
            sghPur.InitGrid(gridPurItems, sgdPur, true, nameof(tb_PurGoodsRecommendDetail));
            sghPur.OnCalculateColumnValue += SghPur_OnCalculateColumnValue;

        }


        #endregion

        #region for 自制品建议
        /*
     SourceGridDefine sgdMaking = null;
     //        SourceGridHelper<View_ProdDetail, tb_ProductionPlanDetail> sgh = new SourceGridHelper<View_ProdDetail, tb_ProductionPlanDetail>();
     SourceGridHelper sghMaking = new SourceGridHelper();
     //设计关联列和目标列


     List<SourceGridDefineColumnItem> listColsMaking = new List<SourceGridDefineColumnItem>();

     public void LoadMakingItems()
     {

         gridMakeItems.BorderStyle = BorderStyle.FixedSingle;
         gridMakeItems.Selection.EnableMultiSelection = false;


         //指定了关键字段ProdDetailID
         listColsMaking = sghMaking.GetGridColumns<ProductSharePart, tb_ProduceGoodsRecommendDetail>(c => c.ProdDetailID, false);

         listColsMaking.SetCol_NeverVisible<ProductSharePart>(c => c.Rack_ID);
         listColsMaking.SetCol_NeverVisible<ProductSharePart>(c => c.Standard_Price);
         listColsMaking.SetCol_NeverVisible<ProductSharePart>(c => c.Inv_Cost);
         listColsMaking.SetCol_NeverVisible<tb_ProduceGoodsRecommendDetail>(c => c.ProdDetailID);
         //listCols.SetCol_NeverVisible<tb_ProduceGoodsRecommendDetail>(c => c.BOM_ID);
         if (!AppContext.SysConfig.UseBarCode)
         {
             listColsMaking.SetCol_NeverVisible<ProductSharePart>(c => c.BarCode);
         }
         ControlChildColumnsInvisible(listColsMaking);
         //listCols.SetCol_DefaultValue<tb_ProduceGoodsRecommendDetail>(a => a.TaxRate, 0.13m);//m =>decial d=>double

         //如果库位为只读  暂时只会显示 ID
         //listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Location_ID);

         //listCols.SetCol_ReadOnly<tb_ProductionDemandDetail>(c => c.CompletedQuantity);

         sgdMaking = new SourceGridDefine(gridMakeItems, listColsMaking, true);




         //listCols.SetCol_Summary<tb_ProductionDemandDetail>(c => c.CompletedQuantity);



         if (CurMenuInfo.tb_P4Fields != null)
         {
             foreach (var item in CurMenuInfo.tb_P4Fields.Where(p => p.tb_fieldinfo.IsChild && !p.IsVisble))
             {
                 //listCols.SetCol_NeverVisible(item.tb_fieldinfo.FieldName);
                 listColsMaking.SetCol_NeverVisible(item.tb_fieldinfo.FieldName, typeof(tb_ProduceGoodsRecommendDetail));
             }

         }


         //公共到明细的映射 源 ，左边会隐藏
         sghMaking.SetPointToColumnPairs<ProductSharePart, tb_ProduceGoodsRecommendDetail>(sgdMaking, f => f.Location_ID, t => t.Location_ID);


         // sgh.SetPointToColumnPairs<ProductSharePart, tb_ProduceGoodsRecommendDetail>(sgd, f => f.prop, t => t.property);




         //应该只提供一个结构
         List<tb_ProduceGoodsRecommendDetail> lines = new List<tb_ProduceGoodsRecommendDetail>();
         bindingSourceMaking.DataSource = lines;
         sgdMaking.BindingSourceLines = bindingSourceMaking;

         Expression<Func<View_ProdDetail, bool>> exp = Expressionable.Create<View_ProdDetail>() //创建表达式
          .AndIF(true, w => w.CNName.Length > 0)
         // .AndIF(txtSpecifications.Text.Trim().Length > 0, w => w.Specifications.Contains(txtSpecifications.Text.Trim()))
         .ToExpression();//注意 这一句 不能少
                         // StringBuilder sb = new StringBuilder();
         /// sb.Append(string.Format("{0}='{1}'", item.ColName, valValue));
         var list = dc.BaseQueryByWhere(exp);
         sgdMaking.SetDependencyObject<ProductSharePart, tb_ProduceGoodsRecommendDetail>(list);

         sgdMaking.HasRowHeader = true;
         sghMaking.InitGrid(gridMakeItems, sgdMaking, true, nameof(tb_ProduceGoodsRecommendDetail));
         sghMaking.OnCalculateColumnValue += SghMaking_OnCalculateColumnValue;
         sghMaking.OnLoadMultiRowData += Sgh_OnLoadMultiRowData;
     }
     */
        #endregion



        private void UcSaleOrderEdit_Load(object sender, EventArgs e)
        {


            //btnCreateProduction.ToolTipValues.Text = "创建生产单";//需求单据保存审核后才能生成制令单！并且确保不要重复生成。
            base.ToolBarEnabledControl(MenuItemEnums.刷新);

            //提出来。因为下面两个加载
            Expression<Func<View_ProdDetail, bool>> exp = Expressionable.Create<View_ProdDetail>() //创建表达式
            .AndIF(true, w => w.CNName.Length > 0)
            // .AndIF(txtSpecifications.Text.Trim().Length > 0, w => w.Specifications.Contains(txtSpecifications.Text.Trim()))
            .ToExpression();//注意 这一句 不能少
                            // StringBuilder sb = new StringBuilder();
            /// sb.Append(string.Format("{0}='{1}'", item.ColName, valValue));
            list = dc.BaseQueryByWhere(exp);

            LoadTargetItems();
            LoadPurItems();
            kryptonNavigator1.SelectedPage = KP分析目标;
        }



        private void Sgh_OnLoadMultiRowData(object rows, SourceGrid.Position position)
        {
            List<View_ProdDetail> RowDetails = new List<View_ProdDetail>();
            var rowss = ((IEnumerable<dynamic>)rows).ToList();
            foreach (var item in rowss)
            {
                RowDetails.Add(item);
            }
            if (RowDetails != null)
            {
                List<tb_ProductionDemandDetail> details = new List<tb_ProductionDemandDetail>();
                IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
                foreach (var item in RowDetails)
                {
                    tb_ProductionDemandDetail Detail = mapper.Map<tb_ProductionDemandDetail>(item);
                    details.Add(Detail);
                }
                sghPur.InsertItemDataToGrid<tb_ProductionDemandDetail>(gridPurItems, sgdPur, details, c => c.ProdDetailID, position);
            }

        }

        private void SghPur_OnCalculateColumnValue(object rowObj, SourceGridDefine griddefine, SourceGrid.Position Position)
        {
            if (rowObj == null)
            {
                return;
            }

            if (EditEntity == null)
            {
                //都不是正常状态
                MainForm.Instance.uclog.AddLog("请先使用新增或查询加载数据");
                return;
            }
            try
            {
                //if (EditEntity.actionStatus == ActionStatus.加载)
                //{
                //    return;
                //}
                //计算总金额  这些逻辑是不是放到业务层？后面要优化
                //Command commandPurItem = new Command();
                ////缓存当前编辑的对象。如果撤销就回原来的值
                //tb_PurGoodsRecommendDetail oldobj = CloneHelper.DeepCloneObject<tb_PurGoodsRecommendDetail>(PurItem);
                //commandPurItem.UndoOperation = delegate ()
                //{
                //    //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                //    CloneHelper.SetValues<tb_PurGoodsRecommendDetail>(PurItem, oldobj);
                //};
                int RequirementQty = ReflectionHelper.GetPropertyValue(rowObj, "RequirementQty").ToInt();
                int RecommendQty = ReflectionHelper.GetPropertyValue(rowObj, "RecommendQty").ToInt();
                if (RequirementQty < RecommendQty)
                {
                    MessageBox.Show($"请购量不能小于建议数量！");
                    ReflectionHelper.SetPropertyValue(rowObj, "RequirementQty", RecommendQty);
                    sghPur.SetCellValue(sgdPur.GetColumnDefineInfo<tb_PurGoodsRecommendDetail>(c => c.RequirementQty), Position, rowObj, false, true);
                }
                List<tb_PurGoodsRecommendDetail> details = sgdPur.BindingSourceLines.DataSource as List<tb_PurGoodsRecommendDetail>;
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先生成要采购的产品数据");
                    return;
                }

            }
            catch (Exception ex)
            {

                logger.LogError("计算出错SghPur_OnCalculateColumnValue", ex);
                MainForm.Instance.uclog.AddLog("SghPur_OnCalculateColumnValue" + ex.Message);
            }
        }


        List<tb_ProductionDemandTargetDetail> TargetDetails = new List<tb_ProductionDemandTargetDetail>();

        List<tb_ProductionDemandDetail> StockLessDetails = new List<tb_ProductionDemandDetail>();

        List<tb_PurGoodsRecommendDetail> PurDetails = new List<tb_PurGoodsRecommendDetail>();

        List<tb_ProduceGoodsRecommendDetail> MakingDetails = new List<tb_ProduceGoodsRecommendDetail>();



        protected override async Task<ReturnResults<tb_ProductionDemand>> Delete()
        {
            ReturnResults<tb_ProductionDemand> rss = new ReturnResults<tb_ProductionDemand>();
            if (EditEntity == null)
            {
                //提示一下删除成功
                MainForm.Instance.uclog.AddLog("提示", "没有要删除的数据");
                return rss;
            }

            //弱引用时，不能直接删除
            tb_ProductionDemandController<tb_ProductionDemand> ctr = Startup.GetFromFac<tb_ProductionDemandController<tb_ProductionDemand>>();
            //如果当前数据被制令单引用，则不能删除。
            if (!ctr.IsReferencedCanDelete(EditEntity))
            {
                rss.Succeeded = false;
                //提示一下删除成功
                MainForm.Instance.uclog.AddLog("提示", "当前数据被制令单或请购单使用，不能删除");
                return rss;
            }
            rss = await base.Delete();
            if (!rss.Succeeded)
            {
                return rss;
            }
            sgdTarget.BindingSourceLines.Clear();
            sgdPur.BindingSourceLines.Clear();

            TargetDetails.Clear();
            StockLessDetails.Clear();
            PurDetails.Clear();
            MakingDetails.Clear();
            kryptonTreeGridViewMaking.DataSource = null;
            kryptonTreeGridViewMaking.GridNodes.Clear();

            kryptonTreeGridViewStockLess.DataSource = null;
            kryptonTreeGridViewStockLess.GridNodes.Clear();
            return rss;
        }

        protected override void Cancel()
        {
            base.Cancel();
            ClearData();
        }


        private void ClearData()
        {
            sgdTarget.BindingSourceLines.Clear();
            sgdPur.BindingSourceLines.Clear();
            TargetDetails.Clear();
            StockLessDetails.Clear();
            PurDetails.Clear();
            MakingDetails.Clear();
            kryptonTreeGridViewMaking.DataSource = null;
            kryptonTreeGridViewMaking.GridNodes.Clear();

            kryptonTreeGridViewStockLess.DataSource = null;
            kryptonTreeGridViewStockLess.GridNodes.Clear();
        }

        /// <summary>
        /// 查询结果 选中行的变化事件
        /// </summary>
        /// <param name="entity"></param>

        protected async override void Save()
        {
            if (EditEntity == null)
            {
                return;
            }
            var eer = errorProviderForAllInput.GetError(txtPDNo);

            bindingSourceTarget.EndEdit();
            bindingSourcePurchase.EndEdit();

            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {

                //没有通过主表验证，下面先不计算
                if (!base.Validator(EditEntity))
                {
                    return;
                }

                #region 目标分析
                List<tb_ProductionDemandTargetDetail> targetDetailsTemp = sgdTarget.BindingSourceLines.DataSource as List<tb_ProductionDemandTargetDetail>;
                //产品ID有值才算有效值
                TargetDetails = targetDetailsTemp.Where(t => t.ProdDetailID > 0).ToList();
                //如果没有有效的明细。直接提示
                if (TargetDetails.Count == 0)
                {
                    MessageBox.Show("请录入有效的分析目标明细记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var aa = TargetDetails.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                if (aa.Count > 0)
                {
                    System.Windows.Forms.MessageBox.Show("分析目标明细中，相同的产品不能多行录入,如有需要,请另建单据保存!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (!base.Validator<tb_ProductionDemandTargetDetail>(TargetDetails))
                {
                    return;
                }
                EditEntity.tb_ProductionDemandTargetDetails = TargetDetails;


                #endregion

                #region  库存不足 可以为空？ 或者只保存库存不足的数据？还是全部保存呢？  暂时全部保存

                List<tb_ProductionDemandDetail> StockLessDetailsTemp = EditEntity.tb_ProductionDemandDetails;
                if (StockLessDetailsTemp != null)
                {
                    StockLessDetails = StockLessDetailsTemp.Where(t => t.ProdDetailID > 0).ToList();
                    EditEntity.tb_ProductionDemandDetails = StockLessDetails;
                    //没有经验通过下面先不计算
                    if (!base.Validator(EditEntity))
                    {
                        return;
                    }
                    if (!base.Validator<tb_ProductionDemandDetail>(StockLessDetails))
                    {
                        return;
                    }


                }
                #endregion

                #region   采购建议
                List<tb_PurGoodsRecommendDetail> PurDetailsTemp = sgdPur.BindingSourceLines.DataSource as List<tb_PurGoodsRecommendDetail>;
                //产品ID有值才算有效值
                PurDetails = PurDetailsTemp.Where(t => t.ProdDetailID > 0).ToList();
                //如果没有有效的明细。直接提示
                //if (PurDetails.Count == 0)
                //{
                //    MessageBox.Show("请录入有效采购建议明细记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}
                var pur = PurDetails.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                if (pur.Count > 0)
                {
                    System.Windows.Forms.MessageBox.Show("采购建议中，相同的产品不能多行录入\r\n，库位或需求日期不一致要拆分为两个需求分析单!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                EditEntity.tb_PurGoodsRecommendDetails = PurDetails;
                //没有经验通过下面先不计算
                if (!base.Validator(EditEntity))
                {
                    return;
                }
                if (!base.Validator<tb_PurGoodsRecommendDetail>(PurDetails))
                {
                    return;
                }

                EditEntity.tb_PurGoodsRecommendDetails = PurDetails;

                #endregion

                #region 自制品建议
                List<tb_ProduceGoodsRecommendDetail> MakingDetailsTemp = EditEntity.tb_ProduceGoodsRecommendDetails;
                if (MakingDetailsTemp != null)
                {
                    //产品ID有值才算有效值
                    MakingDetails = MakingDetailsTemp.Where(t => t.ProdDetailID > 0).ToList();
                    //如果没有有效的明细。直接提示
                    //if (MakingDetails.Count == 0)
                    //{
                    //    MessageBox.Show("请录入有效制成品明细记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    return;
                    //}
                    //var making = MakingDetails.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                    //if (making.Count > 0)
                    //{
                    //    System.Windows.Forms.MessageBox.Show("明细中，自制品建议中相同的产品不能多行录入,如有需要,请另建单据保存!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    return;
                    //}

                    //没有经验通过下面先不计算
                    if (!base.Validator(EditEntity))
                    {
                        return;
                    }
                    if (!base.Validator<tb_ProduceGoodsRecommendDetail>(MakingDetails))
                    {
                        return;
                    }
                    EditEntity.tb_ProduceGoodsRecommendDetails = MakingDetails;
                }

                #endregion


                if (EditEntity.ApprovalStatus == null)
                {
                    EditEntity.ApprovalStatus = (int)ApprovalStatus.未审核;
                }

                if (EditEntity.PDID > 0)
                {
                    //更新式
                    await base.Save(EditEntity);
                }
                else
                {
                    ReturnMainSubResults<tb_ProductionDemand> SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.PDNo}。");
                    }
                    else
                    {
                        MainForm.Instance.PrintInfoLog($"保存失败,{SaveResult.ErrorMsg}。", Color.Red);
                    }
                }


            }
        }

        /*
protected async override Task<ApprovalEntity> Review()
{
    if (EditEntity == null)
    {
        return null;
    }

    //如果已经审核通过，则不能重复审核
    if (EditEntity.ApprovalStatus.HasValue)
    {
        if (EditEntity.ApprovalStatus.Value == (int)ApprovalStatus.已审核)
        {
            if (EditEntity.ApprovalResults.HasValue && EditEntity.ApprovalResults.Value)
            {
                MainForm.Instance.uclog.AddLog("已经审核,且【同意】的单据不能重复审核。", UILogType.警告);
                return null;
            }
        }
    }

    Save(); //有可能出现 自制品还没有生成。就已经审核了。所以先保存。如果不保存一下。自制品就没 有保存生成ID，后面调用会出错。无法保存制令单

    if (EditEntity.tb_ProductionDemandDetails == null || EditEntity.tb_ProductionDemandDetails.Count == 0)
    {
        MainForm.Instance.uclog.AddLog("单据中没有明细数据，请确认录入了完整产品数量和金额数据。", UILogType.警告);
        return null;
    }

    Command command = new Command();
    //缓存当前编辑的对象。如果撤销就回原来的值
    tb_ProductionDemand oldobj = CloneHelper.DeepCloneObject<tb_ProductionDemand>(EditEntity);
    command.UndoOperation = delegate ()
    {
        //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
        CloneHelper.SetValues<tb_ProductionDemand>(EditEntity, oldobj);
    };
    ApprovalEntity ae = await base.Review();
    if (EditEntity == null)
    {
        return null;
    }
    if (ae.ApprovalStatus == (int)ApprovalStatus.未审核)
    {
        return null;
    }
    //ReturnResults<tb_Stocktake> rmr = new ReturnResults<tb_Stocktake>();
    // BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
    //因为只需要更新主表
    //rmr = await ctr.BaseSaveOrUpdate(EditEntity);
    // rmr = await ctr.BaseSaveOrUpdateWithChild<T>(EditEntity);
    tb_ProductionDemandController<tb_ProductionDemand> ctr = Startup.GetFromFac<tb_ProductionDemandController<tb_ProductionDemand>>();
    ReturnResults<bool> rmrs = await ctr.ApprovalAsync(EditEntity);
    if (rmrs.Succeeded)
    {
        //if (MainForm.Instance.WorkflowItemlist.ContainsKey(""))
        //{

        //}
        //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
        //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
        //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
        //MainForm.Instance.ecs.AddSendData(od);

        //审核成功
        base.ToolBarEnabledControl(MenuItemEnums.审核);
        //如果审核结果为不通过时，审核不是灰色。
        if (!ae.ApprovalResults)
        {
            toolStripbtnReview.Enabled = true;
        }
        //
        if (EditEntity.ApprovalResults.Value)
        {

        }
    }
    else
    {
        //审核失败 要恢复之前的值
        command.Undo();
        MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核失败,原因是：{rmrs.ErrorMsg},如果无法解决，请联系管理员！", Color.Red);
    }
    return ae;
}



protected async override Task<ApprovalEntity> ReReview()
{
    ApprovalEntity ae = new ApprovalEntity();
    if (EditEntity == null)
    {
        return ae;
    }

    //反审，要审核过，并且通过了，才能反审。
    if (EditEntity.ApprovalStatus.Value == (int)ApprovalStatus.已审核 && !EditEntity.ApprovalResults.HasValue)
    {
        MainForm.Instance.uclog.AddLog("已经审核,且【同意】的单据才能反审核。");
        return ae;
    }


    //if (EditEntity.tb_ProductionDemandDetails == null || EditEntity.tb_ProductionDemandDetails.Count == 0)
    //{
    //    MainForm.Instance.uclog.AddLog("单据中没有明细数据，请确认录入了完整数量和金额。", UILogType.警告);
    //    return;
    //}

    Command command = new Command();
    //缓存当前编辑的对象。如果撤销就回原来的值
    tb_ProductionDemand oldobj = CloneHelper.DeepCloneObject<tb_ProductionDemand>(EditEntity);
    command.UndoOperation = delegate ()
    {
        //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
        CloneHelper.SetValues<tb_ProductionDemand>(EditEntity, oldobj);
    };

    tb_ProductionDemandController<tb_ProductionDemand> ctr = Startup.GetFromFac<tb_ProductionDemandController<tb_ProductionDemand>>();
    ReturnResults<bool> rmrs = await ctr.AntiApprovalAsync(EditEntity);
    if (rmrs.Succeeded)
    {

        //if (MainForm.Instance.WorkflowItemlist.ContainsKey(""))
        //{

        //}
        //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
        //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
        //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
        //MainForm.Instance.ecs.AddSendData(od);

        //审核成功
        base.ToolBarEnabledControl(MenuItemEnums.反审);
        toolStripbtnReview.Enabled = true;

    }
    else
    {
        //审核失败 要恢复之前的值
        command.Undo();
        MainForm.Instance.PrintInfoLog($"{EditEntity.PDNo}反审失败{rmrs.ErrorMsg},请联系管理员！", Color.Red);
    }
    return ae;
}
*/

        protected async override Task<bool> CloseCaseAsync()
        {
            if (EditEntity == null)
            {
                return false;
            }
            List<tb_ProductionDemand> EditEntitys = new List<tb_ProductionDemand>();
            EditEntitys.Add(EditEntity);
            //已经审核的并且通过的情况才能结案
            List<tb_ProductionDemand> needCloseCases = EditEntitys.Where(c => c.DataStatus == (int)DataStatus.确认 && c.ApprovalStatus == (int)ApprovalStatus.已审核 && c.ApprovalResults.HasValue && c.ApprovalResults.Value).ToList();
            if (needCloseCases.Count == 0)
            {
                MainForm.Instance.PrintInfoLog($"要结案的数据为：{needCloseCases.Count}:请检查数据！");
                return false;
            }

            tb_ProductionDemandController<tb_ProductionDemand> ctr = Startup.GetFromFac<tb_ProductionDemandController<tb_ProductionDemand>>();
            ReturnResults<bool> rs = await ctr.BatchCloseCaseAsync(needCloseCases);
            if (rs.Succeeded)
            {
                //if (MainForm.Instance.WorkflowItemlist.ContainsKey(""))
                //{

                //}
                //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                //MainForm.Instance.ecs.AddSendData(od);
                base.Query();
            }
            else
            {
                MainForm.Instance.PrintInfoLog($"{EditEntity.PDNo}结案操作失败,原因是{rs.ErrorMsg},如果无法解决，请联系管理员！", Color.Red);
            }

            return true;
        }

        /// <summary>
        /// 由计划单据加载明细,明细是库存不足的明细,这里有逻辑处理
        /// </summary>
        /// <param name="sourceid"></param>
        private async Task<List<tb_ProductionDemandTargetDetail>> LoadPlanChildItems(long? sourceid)
        {
            List<tb_ProductionDemandTargetDetail> rslist = new List<tb_ProductionDemandTargetDetail>();
            var SourceBill = MainForm.Instance.AppContext.Db.Queryable<tb_ProductionPlan>().Where(c => c.PPID == sourceid)
            .Includes(a => a.tb_department)
            .Includes(a => a.tb_projectgroup)
            .Includes(a => a.tb_saleorder, b => b.tb_SaleOrderDetails)
            .Includes(a => a.tb_ProductionPlanDetails, b => b.tb_proddetail, c => c.tb_prod)
            .Includes(a => a.tb_ProductionPlanDetails, b => b.tb_proddetail, c => c.tb_bom_s)
            .Includes(a => a.tb_ProductionPlanDetails, b => b.tb_proddetail, c => c.tb_Inventories)
            .Single();
            //新增时才可以转单
            if (SourceBill != null)
            {
                IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
                tb_ProductionDemand entity = mapper.Map<tb_ProductionDemand>(SourceBill);
                List<tb_ProductionDemandTargetDetail> details = mapper.Map<List<tb_ProductionDemandTargetDetail>>(SourceBill.tb_ProductionPlanDetails);
                entity.AnalysisDate = System.DateTime.Now;


                List<string> tipsMsg = new List<string>();
                for (global::System.Int32 i = 0; i < details.Count; i++)
                {
                    //分析过的目标项。不能再次分析。
                    tb_ProductionPlanDetail planDetail = SourceBill.tb_ProductionPlanDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID && c.Location_ID == details[i].Location_ID);
                    if (planDetail.IsAnalyzed.HasValue && planDetail.IsAnalyzed.Value)
                    {
                        continue;
                    }

                    tb_Inventory inventory = new tb_Inventory();
                    try
                    {
                        inventory = SourceBill.tb_ProductionPlanDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID).tb_proddetail.tb_Inventories.FirstOrDefault(c => c.Location_ID == details[i].Location_ID);
                    }
                    catch (Exception)
                    {

                    }
                    if (inventory == null)
                    {
                        inventory = new tb_Inventory();
                        inventory.Quantity = 0;
                        inventory.InitInventory = 0;
                        inventory.Notes = "";//后面修改数据库是不需要？
                        BusinessHelper.Instance.InitEntity(inventory);
                        inventory.ProdDetailID = details[i].ProdDetailID;
                        inventory.Location_ID = details[i].Location_ID;
                        inventory.Notes = "";//后面修改数据库是不需要？
                        inventory.LatestStorageTime = System.DateTime.Now;
                        //采购订单时添加 。这里减掉在路上的数量
                        inventory.On_the_way_Qty = 0;

                        // 直接输入成本：在录入库存记录时，直接输入该产品或物品的成本价格。这种方式适用于成本价格相对稳定或容易确定的情况。
                        //平均成本法：通过计算一段时间内该产品或物品的平均成本来确定成本价格。这种方法适用于成本价格随时间波动的情况，可以更准确地反映实际成本。
                        //先进先出法（FIFO）：按照先入库的产品先出库的原则，计算库存成本。这种方法适用于库存流转速度较快，成本价格相对稳定的情况。
                        //后进先出法（LIFO）：按照后入库的产品先出库的原则，计算库存成本。这种方法适用于库存流转速度较慢，成本价格波动较大的情况。
                        //数据来源可以是多种多样的，例如：
                        //采购价格：从供应商处购买产品或物品时的价格。
                        //生产成本：自行生产产品时的成本，包括原材料、人工和间接费用等。
                        //市场价格：参考市场上类似产品或物品的价格。

                        //CommService.CostCalculations.CostCalculation(_appContext, inv, child.TransactionPrice);
                        tb_InventoryController<tb_Inventory> ctrinv = MainForm.Instance.AppContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                        ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inventory);
                        if (rr.Succeeded)
                        {

                        }
                    }
                    details[i].RequirementDate = SourceBill.RequirementDate;
                    details[i].BookInventory = inventory.Quantity;
                    details[i].SaleQty = inventory.Sale_Qty;
                    details[i].NotIssueMaterialQty = inventory.NotOutQty;
                    // details[i].AvailableStock = inventory.Quantity - inventory.Sale_Qty;
                    details[i].MakeProcessInventory = inventory.MakingQty;
                    details[i].InTransitInventory = inventory.On_the_way_Qty;
                    if (planDetail.tb_proddetail.BOM_ID.HasValue)
                    {
                        details[i].BOM_ID = planDetail.tb_proddetail.BOM_ID.Value;
                    }

                    rslist.Add(details[i]);

                    StringBuilder msg = new StringBuilder();
                    foreach (var item in tipsMsg)
                    {
                        msg.Append(item).Append("\r\n");

                    }
                    if (tipsMsg.Count > 0)
                    {
                        MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    entity.tb_ProductionDemandTargetDetails = rslist;
                    entity.tb_productionplan = SourceBill;

                    entity.DataStatus = (int)DataStatus.草稿;
                    entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                    entity.ApprovalOpinions = "";
                    entity.ApprovalResults = null;
                    entity.Approver_at = null;
                    entity.Approver_by = null;
                    entity.ActionStatus = ActionStatus.新增;
                    entity.PrintStatus = 0;

                    if (entity.PPID > 0)
                    {
                        entity.PPID = SourceBill.PPID;
                        entity.PPNo = SourceBill.PPNo;
                    }
                    BusinessHelper.Instance.InitEntity(entity);
                    //编号已经生成，在新点开一个页面时，自动生成。
                    if (EditEntity.PDNo.IsNotEmptyOrNull())
                    {
                        entity.PDNo = EditEntity.PDNo;
                    }
                    BusinessHelper.Instance.InitEntity(entity);
                    BindData(entity);
                }
                kryptonNavigator1.SelectedPage = KP分析目标;
            }
            return rslist;
        }



        /// <summary>
        /// 要生产的明细，要以树的形式展示，不能修改。修改在制令单中修改。
        /// </summary>

        List<tb_ProduceGoodsRecommendDetail> MakingListitems = new List<tb_ProduceGoodsRecommendDetail>();


        private void btnAnalysis_Click(object sender, EventArgs e)
        {
            if (EditEntity == null)
            {
                return;
            }

            //库存不足不能修改。只是显示，
            kryptonTreeGridViewStockLess.ReadOnly = true;

            AnalysisTargetItems();
        }

        /// <summary>
        /// 生成库存不足的列表。用树形式显示
        /// </summary>
        private void AnalysisTargetItems()
        {
            List<tb_ProductionDemandDetail> lastNeeditems = new List<tb_ProductionDemandDetail>();

            //通过计划明细中成品的需求数量找到对应的BOM。再找他BOM明细及配方数量对比库存数量
            //算出库存量，如果有BOM则是自制品，如果没有。则是外购
            if (sgdTarget.BindingSourceLines.DataSource is List<tb_ProductionDemandTargetDetail> _targetDetails)
            {

                _targetDetails = _targetDetails.Where(c => c.ProdDetailID > 0).ToList();
                tb_ProductionDemandController<tb_ProductionDemand> ctr = Startup.GetFromFac<tb_ProductionDemandController<tb_ProductionDemand>>();

                //先取最外层的数据，下面通过 GetBOMNextNodeInventoryInfo 循环取所有子集合
                //因为是通过 BOM树显示的。多个材料不需要合并。后面建议时再合。只是数量 需要特殊处理。
                //比方 第一行 A  500，库存600，少0个A。  第二行A  要500，就少 400.因为上面用掉500，只有100了。
                //上面逻辑只是外层实现。内层再理清。 解决办法 是 缓存已经用过的数量。即帐面数量累减
                ConcurrentDictionary<long, tb_ProductionDemandDetail> AlreadyReducedQtyList = new ConcurrentDictionary<long, tb_ProductionDemandDetail>();
                var mlist = ctr.GetTopInventoryInfo(EditEntity, _targetDetails, ref AlreadyReducedQtyList);

                /*// Lambda表达式实现Comparison委托
                mlist.Sort((p1, p2) =>
                {
                    if (p1.ParentId != p2.ParentId)
                    {
                        return p2.ParentId.Value.CompareTo(p1.ParentId);
                    }
                    else if (p2.BOM_ID.HasValue && p1.BOM_ID != p2.BOM_ID)
                    {
                        return p2.BOM_ID.Value.CompareTo(p1.BOM_ID);
                    }
                    else if (p1.MissingQuantity != p2.MissingQuantity)
                    {
                        return p2.MissingQuantity.CompareTo(p1.MissingQuantity);
                    }
                    else return 0;
                });*/
                //排序为了美观？


                lastNeeditems.AddRange(mlist);

                ////如果存在裸机等半成品 ，实际以他是否有bom为标记找下级需要的材料
                //foreach (tb_ProductionDemandDetail item in mlist)
                //{
                //    if (item.BOM_ID.HasValue)
                //    {
                //        //找他的次级，里面的业务逻辑已经构建了树型结构
                //        var nextlist = ctr.GetNextBOMInventoryInfo(item.NeedQuantity, item.RequirementDate, item.ID.Value, item.BOM_ID.Value, AlreadyReducedQtyList, item.Location_ID);
                //        lastNeeditems.AddRange(nextlist);
                //    }
                //}

                EditEntity.tb_ProductionDemandDetails = lastNeeditems;
                LoadTreeGridViewStockLess(lastNeeditems);


            }
        }

        private void LoadTreeGridViewStockLess(List<tb_ProductionDemandDetail> lastNeedIitems)
        {

            kryptonTreeGridViewStockLess.DataSource = null;
            kryptonTreeGridViewStockLess.GridNodes.Clear();

            //主要业务表的列头
            ConcurrentDictionary<string, string> colNames = UIHelper.GetFieldNameList<tb_ProductionDemandDetail>();
            //必须添加这两个列进DataTable,后面会通过这两个列来树型结构
            colNames.TryAdd("ID", "ID");
            colNames.TryAdd("ParentId", "上级ID");

            //要排除的列头
            List<Expression<Func<BaseProductInfo, object>>> expressions = new List<Expression<Func<BaseProductInfo, object>>>();
            expressions.Add(c => c.ProductNo);

            //基本信息的列头
            ConcurrentDictionary<string, string> BaseProductInfoColNames = UIHelper.GetFieldNameList<BaseProductInfo>()
                .exclude<BaseProductInfo>(expressions);



            IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
            //将产品详情转换为基本信息列表
            List<BaseProductInfo> BaseProductInfoList = mapper.Map<List<BaseProductInfo>>(list);

            //合并的实体中有指定的业务主键关联，不然无法给值  TODO:不科学，后面要修改完善！！！数据太多查出来性能不好。
            DataTable dtAll = lastNeedIitems.ToDataTable<BaseProductInfo, tb_ProductionDemandDetail>(BaseProductInfoList, BaseProductInfoColNames, colNames, c => c.ProdDetailID);

            kryptonTreeGridViewStockLess.GridNodes.Clear();
            kryptonTreeGridViewStockLess.Columns.Clear();
            kryptonTreeGridViewStockLess.FontParentBold = true;
            kryptonTreeGridViewStockLess.UseParentRelationship = true;

            dtAll.DefaultView.Sort = "ParentId";
            dtAll = dtAll.DefaultView.ToTable();

            //注意指定这个ID和父ID，为了树型结构的展现
            kryptonTreeGridViewStockLess.IdColumnName = "ID";
            ////注意指定这个ID和父ID，为了树型结构的展现
            kryptonTreeGridViewStockLess.ParentIdColumnName = "ParentId";

            kryptonTreeGridViewStockLess.ParentIdRootValue = 0;

            //排第一列
            kryptonTreeGridViewStockLess.GroupByColumnIndex = dtAll.Columns.IndexOf("CNName");

            kryptonTreeGridViewStockLess.IsOneLevel = true;
            kryptonTreeGridViewStockLess.HideColumns.Clear();
            //要在datatable的列中，可以不显示出来。因为只是一个结构显示
            kryptonTreeGridViewStockLess.SetHideColumns(kryptonTreeGridViewStockLess.IdColumnName);
            kryptonTreeGridViewStockLess.SetHideColumns(kryptonTreeGridViewStockLess.ParentIdColumnName);
            kryptonTreeGridViewStockLess.SetHideColumns<tb_ProductionDemandDetail>(c => c.ProdDetailID);
            kryptonTreeGridViewStockLess.SetHideColumns<tb_ProductionDemandDetail>(c => c.BOM_ID);


            kryptonTreeGridViewStockLess.DataSource = dtAll;
            kryptonTreeGridViewStockLess.Columns[kryptonTreeGridViewStockLess.IdColumnName].Visible = false;


        }


        /// <summary>
        /// 加载自制品建议
        /// </summary>
        private void LoadTreeGridViewProduceItems()
        {
            List<tb_ProduceGoodsRecommendDetail> lastNeeditems = EditEntity.tb_ProduceGoodsRecommendDetails;
            kryptonTreeGridViewMaking.DataSource = null;
            kryptonTreeGridViewMaking.GridNodes.Clear();

            //主要业务表的列头
            ConcurrentDictionary<string, string> colNames = UIHelper.GetFieldNameList<tb_ProduceGoodsRecommendDetail>();
            //必须添加这两个列进DataTable,后面会通过这两个列来树型结构
            colNames.TryAdd("ID", "ID");
            colNames.TryAdd("ParentId", "上级ID");
            colNames.TryAdd("Selected", "选择");
            //要排除的列头
            List<Expression<Func<BaseProductInfo, object>>> expressions = new List<Expression<Func<BaseProductInfo, object>>>();
            expressions.Add(c => c.ProductNo);

            //基本信息的列头
            ConcurrentDictionary<string, string> BaseProductInfoColNames = UIHelper.GetFieldNameList<BaseProductInfo>()
                .exclude<BaseProductInfo>(expressions);

            IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
            //将产品详情转换为基本信息列表
            List<BaseProductInfo> BaseProductInfoList = mapper.Map<List<BaseProductInfo>>(list);

            //合并的实体中有指定的业务主键关联，不然无法给值
            DataTable dtAll = lastNeeditems.ToDataTable<BaseProductInfo, tb_ProduceGoodsRecommendDetail>(BaseProductInfoList, BaseProductInfoColNames, colNames, c => c.ProdDetailID);

            kryptonTreeGridViewMaking.GridNodes.Clear();
            kryptonTreeGridViewMaking.Columns.Clear();
            kryptonTreeGridViewMaking.FontParentBold = true;
            kryptonTreeGridViewMaking.UseParentRelationship = true;

            dtAll.DefaultView.Sort = "ParentId";
            dtAll = dtAll.DefaultView.ToTable();

            //注意指定这个ID和父ID，为了树型结构的展现
            kryptonTreeGridViewMaking.IdColumnName = "ID";
            ////注意指定这个ID和父ID，为了树型结构的展现
            kryptonTreeGridViewMaking.ParentIdColumnName = "ParentId";

            kryptonTreeGridViewMaking.ParentIdRootValue = 0;

            //排第一列
            kryptonTreeGridViewMaking.GroupByColumnIndex = dtAll.Columns.IndexOf("CNName");

            kryptonTreeGridViewMaking.IsOneLevel = true;
            kryptonTreeGridViewMaking.HideColumns.Clear();
            //要在datatable的列中，可以不显示出来。因为只是一个结构显示
            kryptonTreeGridViewMaking.SetHideColumns(kryptonTreeGridViewMaking.IdColumnName);
            kryptonTreeGridViewMaking.SetHideColumns(kryptonTreeGridViewMaking.ParentIdColumnName);
            kryptonTreeGridViewMaking.SetHideColumns<tb_ProduceGoodsRecommendDetail>(c => c.ProdDetailID);
            kryptonTreeGridViewMaking.SetHideColumns<tb_ProduceGoodsRecommendDetail>(c => c.BOM_ID);
            kryptonTreeGridViewMaking.ReadOnly = false;



            kryptonTreeGridViewMaking.DataSource = dtAll;
            kryptonTreeGridViewMaking.ExpandAll();
            kryptonTreeGridViewMaking.EditMode = DataGridViewEditMode.EditProgrammatically;
            kryptonTreeGridViewMaking.Columns[kryptonTreeGridViewMaking.IdColumnName].Visible = false;


            kryptonTreeGridViewMaking.Columns["Selected"].DisplayIndex = 1;
            kryptonTreeGridViewMaking.Columns["Selected"].ReadOnly = false;
            // 设置列头的对齐方式为居中
            kryptonTreeGridViewMaking.Columns["Selected"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //为了不出错具体字段名

            Expression<Func<tb_ProduceGoodsRecommendDetail, object>> expRequirementQty = c => c.RequirementQty;
            Expression<Func<tb_ProduceGoodsRecommendDetail, object>> expSelected = c => c.Selected;

            foreach (DataGridViewColumn dgCol in kryptonTreeGridViewMaking.Columns)
            {
                if (dgCol.DataPropertyName == expRequirementQty.GetMemberInfo().Name || dgCol.DataPropertyName == expSelected.GetMemberInfo().Name)
                {
                    dgCol.ReadOnly = false;
                }
                else
                {
                    dgCol.ReadOnly = true;
                }
            }
            kryptonTreeGridViewMaking.Columns["Selected"].Visible = true;
            //设置自制品数量的列背景色突出显示

            //Expression<Func<tb_ProduceGoodsRecommendDetail, object>> expPlanNeedQty = c => c.PlanNeedQty;
            //设置列的单元格背景色为浅绿色
            kryptonTreeGridViewMaking.Columns[expRequirementQty.GetMemberInfo().Name].DefaultCellStyle.BackColor = Color.LightGreen;
            kryptonTreeGridViewMaking.Columns[expSelected.GetMemberInfo().Name].DefaultCellStyle.BackColor = Color.LightGreen;

            //如果自制品中的行 有生成过。则不可以选择。
            foreach (DataGridViewRow dr in kryptonTreeGridViewMaking.Rows)
            {
                if (dr.Tag != null)
                {
                    try
                    {
                        long id = dr.Tag.ToLong();
                        //这个id只是为了树型结构的展现建的。不过，也是唯一的，并且保存在数据库中的
                        tb_ProduceGoodsRecommendDetail item = EditEntity.tb_ProduceGoodsRecommendDetails.FirstOrDefault(c => c.ID == id);
                        if (item != null)
                        {
                            if (item.RefBillID.HasValue && item.RefBillID.Value > 0)
                            {
                                item.Selected = false;
                                kryptonTreeGridViewMaking.Rows[dr.Index].Cells["Selected"].ReadOnly = true;
                                kryptonTreeGridViewMaking.Rows[dr.Index].Cells["Selected"].Value = item.Selected.Value;
                                kryptonTreeGridViewMaking.Rows[dr.Index].Cells["Selected"].Style.BackColor = Color.HotPink;
                                kryptonTreeGridViewMaking.Rows[dr.Index].Cells["Selected"].Style.SelectionBackColor = Color.HotPink;
                                kryptonTreeGridViewMaking.Rows[dr.Index].Cells["Selected"].ToolTipText = "已经生成过制令单，无法重复生成。或刷新后重试！";

                            }
                        }
                        //kryptonTreeGridViewMaking.Rows[dr.Index].Cells["Selected"].GetEditedFormattedValue
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

        }



        private void kryptonTreeGridViewStockLess_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //rowID能控制編輯 可能集成到控件中？
            //DataTable dtAll = kryptonTreeGridViewStockLess.DataSource as DataTable;
            //dtAll  kryptonTreeGridViewStockLess[e.ColumnIndex, e.RowIndex].Value;
        }


        //三级 还是两级呢。  反向来 一是 KEY VALUE  然后是列名
        ConcurrentDictionary<string, List<KeyValuePair<object, string>>> _DataDictionary = new ConcurrentDictionary<string, List<KeyValuePair<object, string>>>();

        /// <summary>
        /// 固定的值显示，入库ture 出库false
        /// 每个列表对应的值 ，单独设置
        /// </summary>
        public ConcurrentDictionary<string, List<KeyValuePair<object, string>>> ColNameDataDicStockLess { get => _DataDictionary; set => _DataDictionary = value; }


        private void kryptonTreeGridViewStockLess_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //如果列是隐藏的是不是可以不需要控制显示了呢? 后面看是否是导出这块需要不需要 不然可以隐藏的直接跳过
            if (!kryptonTreeGridViewStockLess.Columns[e.ColumnIndex].Visible)
            {
                return;
            }
            if (e.Value == null)
            {
                e.Value = "";
                return;
            }
            //固定字典值显示
            string colDbName = kryptonTreeGridViewStockLess.Columns[e.ColumnIndex].Name;
            if (ColNameDataDicStockLess.ContainsKey(colDbName))
            {
                List<KeyValuePair<object, string>> kvlist = new List<KeyValuePair<object, string>>();
                //意思是通过列名找，再通过值找到对应的文本
                ColNameDataDicStockLess.TryGetValue(colDbName, out kvlist);
                if (kvlist != null)
                {
                    KeyValuePair<object, string> kv = kvlist.FirstOrDefault(t => t.Key.ToString().ToLower() == e.Value.ToString().ToLower());
                    if (kv.Value != null)
                    {
                        e.Value = kv.Value;
                        return;
                    }

                }
            }

            //动态字典值显示
            string colName = UIHelper.ShowGridColumnsNameValue<tb_Prod>(colDbName, e.Value);
            if (!string.IsNullOrEmpty(colName))
            {
                e.Value = colName;
            }

            //图片特殊处理
            if (kryptonTreeGridViewStockLess.Columns[e.ColumnIndex].Name == "Image")
            {
                if (e.Value != null)
                {
                    System.IO.MemoryStream buf = new System.IO.MemoryStream((byte[])e.Value);
                    Image image = Image.FromStream(buf, true);
                    e.Value = image;
                    //这里用缓存
                }
            }

            //处理创建人 修改人，因为这两个字段没有做外键。固定的所以可以统一处理

        }



        /// <summary>
        /// 保存后才产生建议
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn产生建议_Click(object sender, EventArgs e)
        {
            //自建品建议也以树形结构展示,并且都是由库存不足明细转换过来的。
            if (EditEntity == null)
            {
                return;
            }

            tb_ProductionDemandController<tb_ProductionDemand> ctr = Startup.GetFromFac<tb_ProductionDemandController<tb_ProductionDemand>>();


            //采购建议 累计了相同的物料,
            List<tb_PurGoodsRecommendDetail> tb_PurGoodsRecommendDetails = ctr.GeneratePurSuggestions(EditEntity, chkPurAllItems.Checked);

            tb_PurGoodsRecommendDetails.Sort((p1, p2) =>
            {
                if (p1.RecommendQty != p2.RecommendQty)
                {
                    return p2.RecommendQty.CompareTo(p1.RecommendQty);
                }
                else if (p1.RequirementQty != p2.RequirementQty)
                {
                    return p2.RequirementQty.CompareTo(p1.RequirementQty);
                }
                else return 0;
            });

            EditEntity.tb_PurGoodsRecommendDetails = tb_PurGoodsRecommendDetails;
            sghPur.LoadItemDataToGrid<tb_PurGoodsRecommendDetail>(gridPurItems, sgdPur, tb_PurGoodsRecommendDetails, c => c.ProdDetailID);


            //生成自制品建议
            GenerateProductionSuggestionsNew();

            btnCreateProduction.Enabled = true;
            btnCreatePurRequisition.Enabled = true;

        }


        private void kryptonNavigator1_SelectedPageChanged(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                if (kryptonNavigator1.SelectedPage.Name == "kp库存不足量需求")
                {
                    kp库存不足量需求.Update();
                }
            }
        }

        MenuPowerHelper menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();


        /// <summary>
        /// 生成请购单  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnCreatePurRequisition_Click(object sender, EventArgs e)
        {
            if (EditEntity == null)
            {
                return;
            }

            if (EditEntity.PDID == 0 || !EditEntity.ApprovalResults.HasValue || !EditEntity.ApprovalResults.Value)
            {
                btnCreatePurRequisition.Enabled = false;
                MessageBox.Show("请先保存单据并审核成功后才能生成请购单。");
                return;
            }
            else
            {
                btnCreatePurRequisition.Enabled = true;

            }

            //生成请购单  请购单，可以来自于其它入口。所以这里不能用导航查询。只能主动查询
            //如果这个已经审核并且没有生成过请购单。这里可以显示，否则为灰色
            tb_BuyingRequisitionController<tb_BuyingRequisition> ctrBuy = Startup.GetFromFac<tb_BuyingRequisitionController<tb_BuyingRequisition>>();
            bool IsCreatePurRequisition = await ctrBuy.IsExistAsync(c => c.RefBillID == EditEntity.PDID && c.RefBizType == (int)BizType.生产需求分析);
            if (IsCreatePurRequisition)
            {
                MainForm.Instance.uclog.AddLog("当前单据已经生成过请购单，无法重复生成");
                btnCreatePurRequisition.Enabled = false;
                return;
            }

            tb_ProductionDemandController<tb_ProductionDemand> ctr = Startup.GetFromFac<tb_ProductionDemandController<tb_ProductionDemand>>();
            tb_BuyingRequisition buyingRequisition = ctr.GenerateBuyingRequisition(EditEntity, EditEntity.tb_PurGoodsRecommendDetails);


            //要把单据信息传过去RUINORERP.UI.PSI.PUR.UCBuyingRequisition
            tb_MenuInfo RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == typeof(tb_BuyingRequisition).Name && m.ClassPath.Contains("RUINORERP.UI.PSI.PUR." + typeof(UCBuyingRequisition).Name)).FirstOrDefault(); ;
            if (RelatedBillMenuInfo != null && EditEntity.tb_PurGoodsRecommendDetails.Count > 0)
            {
                menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, buyingRequisition);
            }
            else
            {
                MainForm.Instance.uclog.AddLog("没有采购建议数据，无法生成");
            }
        }

        /*

        /// <summary>
        /// 通过BOM找到不足的库存后。算出要制作的数量 。比方 如果PCBA仓库有的。制的数量少1，则要仓库发出1，一定会在制令单中体现
        /// </summary>
        private void GenerateProductionSuggestions()
        {
            if (EditEntity == null)
            {
                return;
            }

            MakingListitems.Clear();
            ConcurrentDictionary<long, tb_ProduceGoodsRecommendDetail> AlreadyReducedQtyList = new ConcurrentDictionary<long, tb_ProduceGoodsRecommendDetail>();

            List<tb_ProduceGoodsRecommendDetail> MakingProditems = new List<tb_ProduceGoodsRecommendDetail>();
            //通过计划明细中成品的需求数量找到对应的BOM。再找他BOM明细及配方数量对比库存数量
            //算出库存量，如果有BOM则是自制品，如果没有。则是外购
            if (sgdTarget.BindingSourceLines.DataSource is List<tb_ProductionDemandTargetDetail> _targetDetails)
            {
                foreach (var target in _targetDetails)
                {
                    tb_ProduceGoodsRecommendDetail MakingProd = new tb_ProduceGoodsRecommendDetail();
                    IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
                    MakingProd = mapper.Map<tb_ProduceGoodsRecommendDetail>(target);
                    long sid = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId();
                    MakingProd.ID = sid;
                    MakingProd.ParentId = 0;//一级数据
                    MakingProd.BOM_ID = target.BOM_ID;
                    MakingProd.RequirementQty = target.NeedQuantity; //顶级是来自计划中实际指定的数量。
                    MakingProd.RecommendQty = target.NeedQuantity - 0;//建议的。就是系统推荐计算的。是不是减去库存数量？
                    MakingProd.Location_ID = target.Location_ID;

                    //实际以他是否有bom为标记找下级需要的材料
                    if (MakingProd.BOM_ID.HasValue)
                    {
                        var nextlist = GetNextBomToMakingItem(target.NeedQuantity, target.RequirementDate, MakingProd.ID.Value, target.BOM_ID, AlreadyReducedQtyList, target.Location_ID);
                        MakingProditems.AddRange(nextlist);
                    }
                    MakingProditems.Add(MakingProd);
                }

                EditEntity.tb_ProduceGoodsRecommendDetails = MakingProditems;
                LoadTreeGridViewProduceItems();

            }
        }
        */

        /// <summary>
        /// 通过BOM找到不足的库存后。算出要制作的数量 。比方 如果PCBA仓库有的。制的数量少1，则要仓库发出1，一定会在制令单中体现
        /// 由库存不足数据组成
        /// </summary>
        private void GenerateProductionSuggestionsNew()
        {
            if (EditEntity == null)
            {
                return;
            }

            MakingListitems.Clear();
            ConcurrentDictionary<long, tb_ProduceGoodsRecommendDetail> AlreadyReducedQtyList = new ConcurrentDictionary<long, tb_ProduceGoodsRecommendDetail>();
            List<tb_ProduceGoodsRecommendDetail> MakingProditems = new List<tb_ProduceGoodsRecommendDetail>();
            //库存已经是树结构，通过他去找下级，排除最后一级。

            foreach (tb_ProductionDemandDetail target in EditEntity.tb_ProductionDemandDetails.Where(c => c.ParentId == 0).ToList())
            {
                tb_ProduceGoodsRecommendDetail MakingProd = new tb_ProduceGoodsRecommendDetail();
                IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
                MakingProd = mapper.Map<tb_ProduceGoodsRecommendDetail>(target);
                //要用原始的ID及父ID
                //long sid = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId();
                MakingProd.ID = target.ID;
                MakingProd.ParentId = 0;//一级数据
                MakingProd.BOM_ID = target.BOM_ID;
                if (EditEntity.SuggestBasedOn)
                {
                    MakingProd.RequirementQty = target.NetRequirement;
                }
                else
                {
                    MakingProd.RequirementQty = target.GrossRequirement;
                }

                MakingProd.RecommendQty = target.NeedQuantity - 0;//建议的。就是系统推荐计算的。是不是减去库存数量？
                MakingProd.PlanNeedQty = target.NeedQuantity;
                MakingProd.Location_ID = target.Location_ID;

                //实际以他是否有bom为标记找下级需要的材料
                if (MakingProd.BOM_ID.HasValue)
                {
                    var nextlist = GetNextBomToMakingItemNew(target.NeedQuantity, target.RequirementDate, MakingProd.ID.Value, target.BOM_ID.Value, AlreadyReducedQtyList, target.Location_ID);
                    MakingProditems.AddRange(nextlist);
                }
                MakingProditems.Add(MakingProd);
            }

            EditEntity.tb_ProduceGoodsRecommendDetails = MakingProditems;
            LoadTreeGridViewProduceItems();


        }

        /// <summary>
        /// 获取下一级要的制成品
        /// </summary>
        /// <param name="NeedQuantity">需要的数量</param>
        /// <param name="RequirementDate">需要的日期</param>
        /// <param name="PID">父级ID</param>
        /// <param name="BOM_ID">父级bom的ID,由这个查出BOM详情的相关子组件</param>
        /// <param name="locationID"></param>
        /// <returns></returns>
        public List<tb_ProduceGoodsRecommendDetail> GetNextBomToMakingItem(int NeedQuantity,
            DateTime RequirementDate, long PID, long BOM_ID,
            ConcurrentDictionary<long, tb_ProduceGoodsRecommendDetail> AlreadyReducedQtyList,
            long Location_ID = 0)
        {
            List<tb_ProduceGoodsRecommendDetail> SubMakingProditems = new List<tb_ProduceGoodsRecommendDetail>();
            tb_BOM_SDetailController<tb_BOM_SDetail> sDetailController = Startup.GetFromFac<tb_BOM_SDetailController<tb_BOM_SDetail>>();

            List<tb_BOM_SDetail> bomDetailsOnlyWithBOM = new List<tb_BOM_SDetail>();
            bomDetailsOnlyWithBOM = sDetailController.QueryByNavWithSubBom(c => c.BOM_ID == BOM_ID);
            foreach (tb_BOM_SDetail detail in bomDetailsOnlyWithBOM)
            {
                if (detail.tb_proddetail.BOM_ID.HasValue)
                {
                    tb_ProduceGoodsRecommendDetail subMaking = new tb_ProduceGoodsRecommendDetail();
                    long sid = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId();
                    subMaking.ID = sid;
                    subMaking.ParentId = PID;//一级数据
                    subMaking.BOM_ID = detail.tb_proddetail.BOM_ID;
                    subMaking.ProdDetailID = detail.ProdDetailID;
                    subMaking.property = detail.property;
                    subMaking.Location_ID = Location_ID;
                    //  subMaking.PreEndDate=
                    //subMaking.UnitCost
                    decimal needQty = NeedQuantity * detail.UsedQty;
                    subMaking.PlanNeedQty = needQty.ToInt();//配方用量和实际请制量决定
                    subMaking.RequirementQty = needQty.ToInt();//配方用量和实际请制量决定
                    subMaking.RecommendQty = subMaking.RequirementQty - detail.tb_proddetail.tb_Inventories.Where(c => c.Location_ID == Location_ID).Sum(d => d.Quantity);//库存？
                    if (subMaking.BOM_ID.HasValue)
                    {
                        var nextSublist = GetNextBomToMakingItem(NeedQuantity, RequirementDate, subMaking.ID.Value, detail.tb_proddetail.BOM_ID.Value, AlreadyReducedQtyList, Location_ID);
                        SubMakingProditems.AddRange(nextSublist);
                    }

                    SubMakingProditems.Add(subMaking);
                }
            }
            return SubMakingProditems;
        }



        /// <summary>
        /// 获取下一级要的制成品
        /// </summary>
        /// <param name="NeedQuantity">需要的数量</param>
        /// <param name="RequirementDate">需要的日期</param>
        /// <param name="PID">父级ID</param>
        /// <param name="BOM_ID">父级bom的ID,由这个查出BOM详情的相关子组件</param>
        /// <param name="locationID"></param>
        /// <returns></returns>
        public List<tb_ProduceGoodsRecommendDetail> GetNextBomToMakingItemNew(int NeedQuantity,
            DateTime RequirementDate, long PID, long BOM_ID,
            ConcurrentDictionary<long, tb_ProduceGoodsRecommendDetail> AlreadyReducedQtyList,
            long Location_ID = 0)
        {
            List<tb_ProduceGoodsRecommendDetail> SubMakingProditems = new List<tb_ProduceGoodsRecommendDetail>();
            tb_BOM_SDetailController<tb_BOM_SDetail> sDetailController = Startup.GetFromFac<tb_BOM_SDetailController<tb_BOM_SDetail>>();
            //找到他的次级
            List<tb_BOM_SDetail> bomDetailsOnlyWithBOM = new List<tb_BOM_SDetail>();
            bomDetailsOnlyWithBOM = sDetailController.QueryByNavWithSubBom(c => c.BOM_ID == BOM_ID);
            foreach (tb_ProductionDemandDetail SubItem in EditEntity.tb_ProductionDemandDetails.Where(c => c.ParentId.Value == PID).ToList())
            {
                if (SubItem.BOM_ID.HasValue)
                {
                    tb_BOM_SDetail detail = bomDetailsOnlyWithBOM.Where(c => c.ProdDetailID == SubItem.ProdDetailID).FirstOrDefault();
                    if (detail != null)
                    {
                        #region 生成一个中间件
                        tb_ProduceGoodsRecommendDetail subMaking = new tb_ProduceGoodsRecommendDetail();
                        subMaking.ID = SubItem.ID;
                        subMaking.ParentId = PID;//一级数据
                        subMaking.BOM_ID = detail.tb_proddetail.BOM_ID;
                        subMaking.ProdDetailID = detail.ProdDetailID;
                        subMaking.property = detail.property;
                        subMaking.Location_ID = Location_ID;
                        //subMaking.PreEndDate= SubItem.RequirementDate
                        subMaking.UnitCost = detail.MaterialCost;
                        decimal needQty = NeedQuantity * detail.UsedQty;

                        subMaking.PlanNeedQty = needQty.ToInt();//配方用量和实际请制量决定
                        subMaking.RequirementQty = needQty.ToInt();//配方用量和实际请制量决定

                        subMaking.RecommendQty = subMaking.RequirementQty - detail.tb_proddetail.tb_Inventories.Where(c => c.Location_ID == Location_ID).Sum(d => d.Quantity);//库存？
                        //建议数量=需求数量-已经实际库存。需要100-200时，建议为0， 需要200-100时，建议为100，需要100-（-100）时，建议为200
                        if (subMaking.RecommendQty < 0)
                        {
                            subMaking.RecommendQty = 0;
                        }
                        if (subMaking.BOM_ID.HasValue)
                        {
                            var nextSublist = GetNextBomToMakingItemNew(NeedQuantity, RequirementDate, subMaking.ID.Value, detail.tb_proddetail.BOM_ID.Value, AlreadyReducedQtyList, Location_ID);
                            SubMakingProditems.AddRange(nextSublist);
                        }

                        SubMakingProditems.Add(subMaking);
                        #endregion
                    }

                }
                else
                {
                    //最后一级忽略。只要制成品
                    continue;
                }

            }


            return SubMakingProditems;
        }


        /// <summary>
        /// 生成制令单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateProduction_Click(object sender, EventArgs e)
        {
            this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);
            //CreateProduction(rdb中间件式.Checked);
            CreateProductionNew2024(rdb中间件式.Checked);

        }


        /*
        /// <summary>
        /// 生成制令单,如果为中间件式，意思是：没有生成过的所有行，只能选择一个。
        /// 如果为上层驱动模式，意思是：没有生成过的所有行，可以选择一个后，他的子项全选中
        /// </summary>
        ///<param name="MiddlewareType">中间件式:true，上层驱动模式:false</param>
        private async void CreateProduction(bool MiddlewareType = false)
        {

            if (EditEntity == null)
            {
                return;
            }
            if (EditEntity.PDID == 0 || !EditEntity.ApprovalResults.HasValue || !EditEntity.ApprovalResults.Value)
            {
                btnCreateProduction.Enabled = false;
                MessageBox.Show("请先保存单据并审核成功后才能生成制令单。");
                return;
            }
            else
            {
                btnCreateProduction.Enabled = true;
            }

            tb_ProductionDemandController<tb_ProductionDemand> ctrPD = Startup.GetFromFac<tb_ProductionDemandController<tb_ProductionDemand>>();

            //生成请购单  请购单，可以来自于其它入口。所以这里不能用导航查询。只能主动查询
            //如果这个已经审核并且没有生成过请购单。这里可以显示，否则为灰色
            tb_ManufacturingOrderController<tb_ManufacturingOrder> ctrMO = Startup.GetFromFac<tb_ManufacturingOrderController<tb_ManufacturingOrder>>();


            //按成品生成时只能生成一次
            if (!MiddlewareType)
            {
                bool IsCreatectrMO = await ctrMO.IsExistAsync(c => c.PDID == EditEntity.PDID && c.isdeleted == false);
                if (IsCreatectrMO)
                {
                    MainForm.Instance.uclog.AddLog("当前单据已经生成过制令单，无法重复生成");
                    btnCreatePurRequisition.Enabled = false;
                    return;
                }
            }


            //生成制令单  以目标为基准。
            tb_ProductionDemandController<tb_ProductionDemand> ctr = Startup.GetFromFac<tb_ProductionDemandController<tb_ProductionDemand>>();

            if (EditEntity.tb_productionplan != null)
            {
                tb_SaleOrderController<tb_SaleOrder> ctrSO = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
                if (EditEntity.tb_productionplan.tb_saleorder == null)
                {
                    EditEntity.tb_productionplan.tb_saleorder = await ctrSO.BaseQueryByIdNavAsync(EditEntity.tb_productionplan.SOrder_ID);
                }
            }

            //一次只会生成一个单据
            if (MiddlewareType)
            {
                #region  生成制令单  拆分
                List<tb_ManufacturingOrder> MOList = new List<tb_ManufacturingOrder>();
                if (kryptonTreeGridViewMaking.Columns["Selected"] != null)
                {
                    int selectedColIndex = 0;
                    selectedColIndex = kryptonTreeGridViewMaking.Columns["Selected"].Index;
                    for (int i = 0; i < kryptonTreeGridViewMaking.Rows.Count; i++)
                    {
                        if (kryptonTreeGridViewMaking.Rows[i].Cells[selectedColIndex].Value.ToBool() == true)
                        {
                            List<tb_ManufacturingOrder> ManufacturingOrders = new List<tb_ManufacturingOrder>();

                            //拆分的。只会对选择中的BOM下一级处理生成制令单
                            List<tb_ProduceGoodsRecommendDetail> MakingProditems = EditEntity.tb_ProduceGoodsRecommendDetails;
                            var MakingItem = MakingProditems.FirstOrDefault(c => c.ID == kryptonTreeGridViewMaking.Rows[i].Tag.ToLong());
                            if (MakingItem != null)
                            {
                                //按裸机时，要查单号和裸机也只能一次
                                bool IsCreatectrMO = await ctrMO.IsExistAsync(c => c.PDID == EditEntity.PDID

                                && c.ProdDetailID == MakingItem.ProdDetailID
                                );
                                if (IsCreatectrMO)
                                {
                                    string cname = kryptonTreeGridViewMaking.Rows[i].Cells["CNName"].Value.ToString();

                                    MainForm.Instance.uclog.AddLog($"当前单据中{cname}已经生成过制令单，无法重复生成", UILogType.警告);
                                    btnCreatePurRequisition.Enabled = false;
                                    return;
                                }

                                if (MakingItem != null)
                                {
                                    tb_ManufacturingOrder ManufacturingOrder = await ctrPD.InitManufacturingOrder(EditEntity, MakingItem, false);
                                    ManufacturingOrders.Add(ManufacturingOrder);
                                }
                            }
                            MOList.AddRange(ManufacturingOrders);
                        }
                    }
                }

                tb_MenuInfo RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == typeof(tb_ManufacturingOrder).Name && m.ClassPath.Contains("RUINORERP.UI.MRP.MP." + typeof(UCManufacturingOrder).Name)).FirstOrDefault(); ;
                if (RelatedBillMenuInfo != null && MOList.Count > 0)
                {
                    foreach (var MO in MOList)
                    {
                        //如果是给值。不在这处理。在生成时处理的。 这里只是调用到UI
                        menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, MO);
                    }
                }
                else
                {
                    MainForm.Instance.uclog.AddLog("没有自制品建议数据，无法生成");
                }
                #endregion
            }
            else
            {
                #region  生成制令单按成品 上层模式，则选择其中一个节点，将他所有次级次级都处理，所有材料发出来
                //by watson
                List<tb_ManufacturingOrder> MOList = await ctr.GenerateManufacturingOrderNew(EditEntity, MakingItem, true);

                tb_MenuInfo RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == typeof(tb_ManufacturingOrder).Name && m.ClassPath.Contains("RUINORERP.UI.MRP.MP." + typeof(UCManufacturingOrder).Name)).FirstOrDefault(); ;
                if (RelatedBillMenuInfo != null && MOList.Count > 0)
                {
                    foreach (var MO in MOList)
                    {
                        //如果是给值。不在这处理。在生成时处理的。 这里只是调用到UI
                        menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, MO);
                    }
                }
                else
                {
                    MainForm.Instance.uclog.AddLog("没有自制品建议数据，无法生成");
                }
                #endregion
            }




        }
        */


        /// <summary>
        /// 生成制令单,如果为中间件式，意思是：没有生成过的所有行，只能选择一个。
        /// 如果为上层驱动模式，意思是：没有生成过的所有行，可以选择一个后，他的子项全选中
        /// </summary>
        ///<param name="MiddlewareType">中间件式:true，上层驱动模式:false</param>
        private async void CreateProductionNew2024(bool MiddlewareType = false)
        {

            if (EditEntity == null)
            {
                return;
            }
            if (EditEntity.PDID == 0 || !EditEntity.ApprovalResults.HasValue || !EditEntity.ApprovalResults.Value)
            {
                btnCreateProduction.Enabled = false;
                MessageBox.Show("请先保存单据并审核成功后才能生成制令单。");
                return;
            }
            else
            {
                btnCreateProduction.Enabled = true;
            }

            if (EditEntity.tb_ProduceGoodsRecommendDetails.Where(c => c.Selected == true).ToList().Count == 0)
            {
                MainForm.Instance.uclog.AddLog("请选择要生成制令单的行。");
                return;
            }

            tb_ProductionDemandController<tb_ProductionDemand> ctrPD = Startup.GetFromFac<tb_ProductionDemandController<tb_ProductionDemand>>();

            //生成请购单  请购单，可以来自于其它入口。所以这里不能用导航查询。只能主动查询
            //如果这个已经审核并且没有生成过请购单。这里可以显示，否则为灰色
            tb_ManufacturingOrderController<tb_ManufacturingOrder> ctrMO = Startup.GetFromFac<tb_ManufacturingOrderController<tb_ManufacturingOrder>>();

            //判断他是不是能再次生成制令单，是看选择的行中有没有已经生成过
            //先看是否选中，再看是中间件式还是 上层驱动，如果是中间件，
            //   如果是上层式，则找PID？
            List<tb_ProduceGoodsRecommendDetail> MakingItems = EditEntity.tb_ProduceGoodsRecommendDetails.Where(c => c.Selected == true).ToList();
            if (MiddlewareType)
            {
                if (MakingItems.Count > 1)
                {
                    MessageBox.Show("中间件模式下，一次只能选择一个目标生成制令单。");
                    return;
                }
                else
                {
                    tb_ProduceGoodsRecommendDetail target = MakingItems.FirstOrDefault();
                    bool IsCreatectrMO = await ctrMO.IsExistAsync(c => c.PDID == EditEntity.PDID && c.ProdDetailID == target.ProdDetailID && c.PDCID == target.PDCID);
                    if (IsCreatectrMO)
                    {
                        MainForm.Instance.uclog.AddLog($"当前单据的制成品建议中，当前选中目标行产品，已经生成过制令单，无法重复生成", UILogType.警告);
                        btnCreatePurRequisition.Enabled = false;
                        return;
                    }
                }
            }
            else
            {
                //上层驱动模式
                List<tb_ProduceGoodsRecommendDetail> MakingItemsByTop = EditEntity.tb_ProduceGoodsRecommendDetails.Where(c => c.Selected == true).ToList().OrderBy(c => c.ParentId).ToList();
                tb_ProduceGoodsRecommendDetail target = MakingItems.FirstOrDefault();
                bool IsCreatectrMO = await ctrMO.IsExistAsync(c => c.PDID == EditEntity.PDID && c.ProdDetailID == target.ProdDetailID && c.PDCID == target.PDCID);
                if (IsCreatectrMO)
                {
                    MainForm.Instance.uclog.AddLog($"当前单据的制成品建议中，当前选中目标行产品已经生成过制令单，无法重复生成", UILogType.警告);
                    btnCreatePurRequisition.Enabled = false;
                    return;
                }
            }

            //上面为验证是否可以再次生成制令单

            tb_ProductionDemandController<tb_ProductionDemand> ctr = Startup.GetFromFac<tb_ProductionDemandController<tb_ProductionDemand>>();

            if (EditEntity.tb_productionplan != null)
            {
                tb_SaleOrderController<tb_SaleOrder> ctrSO = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
                if (EditEntity.tb_productionplan.tb_saleorder == null)
                {
                    EditEntity.tb_productionplan.tb_saleorder = await ctrSO.BaseQueryByIdNavAsync(EditEntity.tb_productionplan.SOrder_ID);
                }
            }

            //一次只会生成一个单据,中间件时，生成目标是选中的行。明细是选中行对应的BOM的子级

            #region  生成制令单   

            //找到选择列
            int selectedColIndex = 0;
            Expression<Func<tb_ProduceGoodsRecommendDetail, object>> expSelected = c => c.Selected;
            selectedColIndex = kryptonTreeGridViewMaking.Columns[expSelected.GetMemberInfo().Name].Index;
            //找到选择行再按行号排序，因为是树形，要找到最小行号
            long TopID = 0;
            List<KeyValuePair<int, long>> kvList = new List<KeyValuePair<int, long>>();
            for (int i = 0; i < kryptonTreeGridViewMaking.Rows.Count; i++)
            {
                if (kryptonTreeGridViewMaking.Rows[i].Cells[selectedColIndex].Value.ToBool() == true)
                {
                    TopID = kryptonTreeGridViewMaking.Rows[i].Tag.ToLong();
                    kvList.Add(new KeyValuePair<int, long>(i, TopID));
                }
            }

            // 找到键最小行号的对应值
            long minKeyValue = kvList.OrderBy(kv => kv.Key).First().Value;
            //中间件式只会对选择中的BOM下一级处理生成制令单，即：明细中只包含下一级的BOM明细，所以needloop=false
            //上层驱动模式，需要循环所有明细，所以needloop=true   
            List<tb_ProduceGoodsRecommendDetail> MakingProditems = EditEntity.tb_ProduceGoodsRecommendDetails;
            var MakingItem = MakingProditems.FirstOrDefault(c => c.ID == minKeyValue);
            tb_ManufacturingOrder ManufacturingOrder = await ctrPD.InitManufacturingOrder(EditEntity, MakingItem, !MiddlewareType);
            tb_MenuInfo RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == typeof(tb_ManufacturingOrder).Name && m.ClassPath.Contains("RUINORERP.UI.MRP.MP." + typeof(UCManufacturingOrder).Name)).FirstOrDefault();
            if (RelatedBillMenuInfo != null && ManufacturingOrder != null)
            {
                //如果是给值。不在这处理。在生成时处理的。 这里只是调用到UI
                menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, ManufacturingOrder);
            }
            else
            {
                MainForm.Instance.uclog.AddLog("没有自制品建议数据，无法生成");
            }
            #endregion








        }


        private void kryptonTreeGridViewMaking_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //如果列是隐藏的是不是可以不需要控制显示了呢? 后面看是否是导出这块需要不需要 不然可以隐藏的直接跳过
            if (!kryptonTreeGridViewMaking.Columns[e.ColumnIndex].Visible)
            {
                return;
            }
            if (e.Value == null)
            {
                e.Value = "";
                return;
            }
            //固定字典值显示
            string colDbName = kryptonTreeGridViewMaking.Columns[e.ColumnIndex].Name;
            if (ColNameDataDicStockLess.ContainsKey(colDbName))
            {
                List<KeyValuePair<object, string>> kvlist = new List<KeyValuePair<object, string>>();
                //意思是通过列名找，再通过值找到对应的文本
                ColNameDataDicStockLess.TryGetValue(colDbName, out kvlist);
                if (kvlist != null)
                {
                    KeyValuePair<object, string> kv = kvlist.FirstOrDefault(t => t.Key.ToString().ToLower() == e.Value.ToString().ToLower());
                    if (kv.Value != null)
                    {
                        e.Value = kv.Value;
                        return;
                    }

                }
            }

            //动态字典值显示
            string colName = UIHelper.ShowGridColumnsNameValue<tb_Prod>(colDbName, e.Value);
            if (!string.IsNullOrEmpty(colName))
            {
                e.Value = colName;
            }

            //图片特殊处理
            if (kryptonTreeGridViewStockLess.Columns[e.ColumnIndex].Name == "Image")
            {
                if (e.Value != null)
                {
                    System.IO.MemoryStream buf = new System.IO.MemoryStream((byte[])e.Value);
                    Image image = Image.FromStream(buf, true);
                    e.Value = image;
                    //这里用缓存
                }
            }

            //处理创建人 修改人，因为这两个字段没有做外键。固定的所以可以统一处理
        }

        private void kryptonTreeGridViewMaking_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (EditEntity == null)
            {
                return;
            }
            //return;//都不让修改.能改的话太复杂。中间件数量 变化 除了影响对应 采购物料。还会影响中间件的子有中间件
            Expression<Func<tb_ProduceGoodsRecommendDetail, object>> expRequirementQty = c => c.RequirementQty;
            Expression<Func<tb_ProduceGoodsRecommendDetail, object>> expSelected = c => c.Selected;

            KryptonTreeGridView ktgv = sender as KryptonTreeGridView;
            object ModifyValue = ktgv.CurrentCell.FormattedValue;
            string ModifyColName = ktgv.CurrentCell.OwningColumn.Name;
            DataTable dt = kryptonTreeGridViewMaking.DataSource as DataTable;
            long ID = ktgv.CurrentNode.Tag.ToLong();//id不是产品ID，也不是明细主键。是为了树形结构生成的id也能当行号

            //自制品建议时，如果修改的是顶级。则无法修改。因为顶级就是设计。这里改就不如计划目标这边。没有意义。
            //如果是中间件改。可以改小。不能改大。改大说明要多做。如果真要多做如裸机。就应该是计划中做。
            //可以改小的意思是：如裸机或主板。以前做多的。有现成的。刚改小的数量则要发料出去。则其子级需要的材料就要减少。少发料。
            tb_ProduceGoodsRecommendDetail row = EditEntity.tb_ProduceGoodsRecommendDetails.FirstOrDefault(c => c.ID == ID);
            if (row != null)
            {
                if (ModifyColName == expRequirementQty.GetMemberInfo().Name)
                {
                    int ModifyValueQty = ModifyValue.ToInt();
                    if (ModifyValueQty != row.PlanNeedQty)
                    {
                        if (ModifyValueQty < row.PlanNeedQty)
                        {
                            //如果已经生成请购单，则修改无效。否则要同步更新采购的建议。
                            ReflectionHelper.SetPropertyValue(row, ModifyColName, ModifyValue);
                            //同时要更新其子级的材料用量
                            KryptonTreeGridNodeRow tgn = ktgv.CurrentNode.Parent;
                            tb_ProduceGoodsRecommendDetail Prow = EditEntity.tb_ProduceGoodsRecommendDetails.FirstOrDefault(c => c.ParentId == ID);
                            updateSubItem(row, Prow);
                        }
                        else
                        {
                            MessageBox.Show("请制量不能大于计划实际量，如果仅想多生产【半成品】,请在计划目标中体现数量！");
                            ktgv.CancelEdit();
                            ktgv.CurrentCell.Value = row.RecommendQty;
                        }
                    }

                }

                if (ModifyColName == expSelected.GetMemberInfo().Name)
                {

                    //目前只支持一次生成一个制令单所以选中时，其它就不能勾选。不选就不管。
                    if (ModifyValue.ToBool())
                    {
                        foreach (var item in kryptonTreeGridViewMaking.Rows)
                        {

                        }
                    }

                    //更新到行值中，
                    ReflectionHelper.SetPropertyValue(row, ModifyColName, ModifyValue);
                }

            }

        }


        private void updateSubItem(tb_ProduceGoodsRecommendDetail row, tb_ProduceGoodsRecommendDetail Prow)
        {
            decimal DiffQty = row.RecommendQty - row.RequirementQty;
            tb_BOM_SDetailController<tb_BOM_SDetail> sDetailController = Startup.GetFromFac<tb_BOM_SDetailController<tb_BOM_SDetail>>();

            List<tb_BOM_SDetail> subBomDetails = sDetailController.QueryByNavWithSubBom(c => c.BOM_ID == row.BOM_ID);
            //Prow.BOM_ID.ToBool
            //要通过上级BOM找到BOM明细再算出对应的数量,里面可能存在已经累计过的。相同的物料。这里只是算出差值。直接减掉。
            foreach (var item in EditEntity.tb_PurGoodsRecommendDetails)
            {
                tb_BOM_SDetail sDetail = subBomDetails.FirstOrDefault(c => c.ProdDetailID == item.ProdDetailID);
                if (sDetail != null)
                {
                    decimal totalDiffQty = sDetail.UsedQty * DiffQty;
                    item.RequirementQty = item.RequirementQty - totalDiffQty.ToInt();
                    item.RecommendQty = item.RecommendQty - totalDiffQty.ToInt();

                }
            }
            //重新加载采购建议
            sghPur.LoadItemDataToGrid<tb_PurGoodsRecommendDetail>(gridPurItems, sgdPur, EditEntity.tb_PurGoodsRecommendDetails, c => c.ProdDetailID);
        }




        private void kryptonTreeGridViewMaking_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            Expression<Func<tb_ProduceGoodsRecommendDetail, object>> expSelected = c => c.Selected;
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                KryptonTreeGridView ktgv = sender as KryptonTreeGridView;
                object ModifyValue = ktgv.CurrentCell.FormattedValue;
                string ModifyColName = ktgv.CurrentCell.OwningColumn.Name;
                long ID = ktgv.CurrentNode.Tag.ToLong();//id不是产品ID，也不是明细主键。是为了树形结构生成的id也能当行号
                if (ModifyColName == expSelected.GetMemberInfo().Name)
                {
                    kryptonTreeGridViewMaking.BeginEdit(true);
                    //同时更新到对应实体，这里可能只是datasource是datatable
                    tb_ProduceGoodsRecommendDetail item = EditEntity.tb_ProduceGoodsRecommendDetails.FirstOrDefault(c => c.ID == ID);
                    if (item != null)
                    {
                        item.Selected = kryptonTreeGridViewMaking.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToBool();
                    }

                }
                else
                {
                    kryptonTreeGridViewMaking.BeginEdit(false);
                }
            }
        }

        private void kryptonTreeGridViewMaking_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1)
            {
                return;
            }

            Expression<Func<tb_ProduceGoodsRecommendDetail, object>> expSelected = c => c.Selected;
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                KryptonTreeGridView ktgv = sender as KryptonTreeGridView;
                object ModifyValue = ktgv.CurrentCell.FormattedValue;
                string ModifyColName = ktgv.CurrentCell.OwningColumn.Name;
                long ID = ktgv.CurrentNode.Tag.ToLong();//id不是产品ID，也不是明细主键。是为了树形结构生成的id也能当行号
                tb_ProduceGoodsRecommendDetail row = EditEntity.tb_ProduceGoodsRecommendDetails.FirstOrDefault(c => c.ID == ID);
                if (ModifyColName == expSelected.GetMemberInfo().Name)
                {
                    //提供要处理的行数。以数据行为标准，再反应到控件，因为控件中没有绑定父列
                    List<tb_ProduceGoodsRecommendDetail> needProcessList = new List<tb_ProduceGoodsRecommendDetail>();
                    for (int i = 0; i < EditEntity.tb_ProduceGoodsRecommendDetails.Count; i++)
                    {
                        if (EditEntity.tb_ProduceGoodsRecommendDetails[i].ID != ID)
                        {
                            needProcessList.Add(EditEntity.tb_ProduceGoodsRecommendDetails[i].DeepClone());
                        }
                    }

                    DataGridViewCheckBoxCell checkBoxCell = (DataGridViewCheckBoxCell)kryptonTreeGridViewMaking.Rows[e.RowIndex].Cells[e.ColumnIndex];

                    bool isChecked = (bool)checkBoxCell.EditingCellFormattedValue;
                    //这个时间就直接更新值
                    kryptonTreeGridViewMaking.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = isChecked;
                    //更新到行值中，
                    ReflectionHelper.SetPropertyValue(row, ModifyColName, isChecked);
                    if (isChecked)
                    {
                        rdb中间件式.Enabled = false;
                        rdb上层驱动.Enabled = false;
                        //当选择了当前行时，看属于哪种情况
                        // 执行选中时的操作
                        //其他就不选 
                        if (rdb中间件式.Checked && isChecked)
                        {
                            #region   中间件式 如果是中间件式，则其它的不能勾选，除已经生成过的。
                            for (int i = 0; i < kryptonTreeGridViewMaking.Rows.Count; i++)
                            {
                                if (i != e.RowIndex)
                                {
                                    kryptonTreeGridViewMaking.Rows[i].Cells[ktgv.CurrentCell.OwningColumn.Index].Value = false;

                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region  上层驱动模式
                            tb_ProduceGoodsRecommendDetail item = EditEntity.tb_ProduceGoodsRecommendDetails.FirstOrDefault(c => c.ID == ID);
                            if (item != null)
                            {


                                SelectChildsLoop(needProcessList, item.ID.Value, e.RowIndex, true);
                            }
                            #endregion
                        }


                    }
                    else
                    {
                        #region 取消时

                        //当选择了当前行时，看属于哪种情况
                        // 执行选中时的操作
                        //其他就不选 
                        if (rdb中间件式.Checked)
                        {

                        }
                        else
                        {
                            //如果取消时，看取消的上面有不有选中的，也要取消
                            //当前行 上面已找到 row

                            SelectParentsLoop(needProcessList, row.ParentId.Value, e.RowIndex, false);

                            #region  上层驱动模式 取消子次
                            tb_ProduceGoodsRecommendDetail item = EditEntity.tb_ProduceGoodsRecommendDetails.FirstOrDefault(c => c.ID == ID);
                            if (item != null)
                            {
                                SelectChildsLoop(needProcessList, item.ID.Value, e.RowIndex, false);
                            }
                            #endregion
                        }

                        #endregion
                        bool s = false;
                        for (int i = 0; i < kryptonTreeGridViewMaking.Rows.Count; i++)
                        {
                            if (kryptonTreeGridViewMaking.Rows[i].Cells[expSelected.GetMemberInfo().Name].Value.ToBool() == false)
                            {
                                s = false;
                            }
                        }
                        if (!s)
                        {
                            rdb中间件式.Enabled = true;
                            rdb上层驱动.Enabled = true;
                        }
                        if (EditEntity.tb_ProduceGoodsRecommendDetails.Where(c => c.Selected.Value).ToList().Count == 0 || s)
                        {
                            rdb中间件式.Enabled = true;
                            rdb上层驱动.Enabled = true;
                        }


                    }


                }
            }
        }


        private void SelectChildsLoop(List<tb_ProduceGoodsRecommendDetail> needProcessList, long Pid, int currentRowIndex, bool selected)
        {
            Expression<Func<tb_ProduceGoodsRecommendDetail, object>> expSelected = c => c.Selected;
            List<tb_ProduceGoodsRecommendDetail> templist = needProcessList.Where(c => c.ParentId == Pid).ToList();
            foreach (var drTemp in templist)
            {
                for (int i = 0; i < kryptonTreeGridViewMaking.Rows.Count; i++)
                {
                    if (i != currentRowIndex)
                    {
                        if (kryptonTreeGridViewMaking.Rows[i].Tag.ToLong() == drTemp.ID)
                        {
                            if (selected && rdb上层驱动.Checked && drTemp.RefBillID.HasValue && drTemp.RefBillID.Value > 0)
                            {
                                MessageBox.Show("【上层驱动】模式下，次级已经生成过制令单，无法再次生成\r\n系统自动切换为【中间件】模式。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                rdb中间件式.Checked = true;
                                return;
                            }

                            kryptonTreeGridViewMaking.Rows[i].Cells[expSelected.GetMemberInfo().Name].Value = selected;
                            needProcessList.Remove(drTemp);//设置过了。
                            if (needProcessList.Count > 0)
                            {
                                SelectChildsLoop(needProcessList, drTemp.ID.Value, i, selected);
                            }
                        }
                    }
                }
            }



        }


        private void SelectParentsLoop(List<tb_ProduceGoodsRecommendDetail> needProcessList, long Pid, int currentRowIndex, bool selected)
        {
            Expression<Func<tb_ProduceGoodsRecommendDetail, object>> expSelected = c => c.Selected;
            List<tb_ProduceGoodsRecommendDetail> templist = needProcessList.Where(c => c.ID == Pid).ToList();
            foreach (var drTemp in templist)
            {
                for (int i = 0; i < kryptonTreeGridViewMaking.Rows.Count; i++)
                {
                    if (i != currentRowIndex)
                    {
                        if (kryptonTreeGridViewMaking.Rows[i].Tag.ToLong() == drTemp.ID)
                        {
                            if (selected && rdb上层驱动.Checked && drTemp.RefBillID.HasValue && drTemp.RefBillID.Value > 0)
                            {
                                MessageBox.Show("【上层驱动】模式下，次级已经生成过制令单，无法再次生成\r\n系统自动切换为【中间件】模式。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                rdb中间件式.Checked = true;
                                return;
                            }

                            kryptonTreeGridViewMaking.Rows[i].Cells[expSelected.GetMemberInfo().Name].Value = selected;
                            needProcessList.Remove(drTemp);//设置过了。
                            if (needProcessList.Count > 0)
                            {
                                SelectChildsLoop(needProcessList, drTemp.ID.Value, i, selected);
                            }
                        }
                    }
                }
            }



        }



    }
}
