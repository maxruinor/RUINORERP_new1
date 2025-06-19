using AutoMapper;
using Krypton.Toolkit;
using Microsoft.Extensions.Logging;
using RUINOR.Core;
using RUINORERP.Business;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.Processor;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.Dto;

using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using RUINORERP.UI.UCSourceGrid;
using SourceGrid;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SourceGridDefine = RUINORERP.UI.UCSourceGrid.SourceGridDefine;
using SourceGridHelper = RUINORERP.UI.UCSourceGrid.SourceGridHelper;

namespace RUINORERP.UI.PSI.PUR
{
    [MenuAttrAssemblyInfo("缴库单", ModuleMenuDefine.模块定义.生产管理, ModuleMenuDefine.生产管理.制程生产, BizType.缴库单)]
    public partial class UCFinishedGoodsInv : BaseBillEditGeneric<tb_FinishedGoodsInv, tb_FinishedGoodsInvDetail>
    {
        public UCFinishedGoodsInv()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 用于其他UI传入的数据载入。并不是刷新数据
        /// </summary>
        /// <param name="Entity"></param>
        internal override void LoadDataToUI(object Entity)
        {
            ActionStatus actionStatus = ActionStatus.无操作;
            BindData(Entity as tb_FinishedGoodsInv, actionStatus);
        }
        /// <summary>
        /// 加载下拉值
        /// </summary>
        public void InitDataTocmbbox()
        {
            lblPrintStatus.Text = "";
            lblReview.Text = "";
            DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.InitDataToCmb<tb_Department>(k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);
        }


        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_FinishedGoodsInv).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }



        public override void BindData(tb_FinishedGoodsInv entity, ActionStatus actionStatus)
        {
            if (entity == null)
            {
                return;
            }
            EditEntity = entity;
            if (entity.FG_ID > 0)
            {
                entity.PrimaryKeyID = entity.FG_ID;
                entity.ActionStatus = ActionStatus.加载;

                //如果审核了，审核要灰色
            }
            else
            {
                entity.ActionStatus = ActionStatus.新增;
                entity.DataStatus = (int)DataStatus.草稿;
                entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                if (string.IsNullOrEmpty(entity.DeliveryBillNo))
                {
                    entity.DeliveryBillNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.缴库单);
                }
                entity.DeliveryDate = System.DateTime.Now;
                if (entity.tb_FinishedGoodsInvDetails != null && entity.tb_FinishedGoodsInvDetails.Count > 0)
                {
                    entity.tb_FinishedGoodsInvDetails.ForEach(c => c.FG_ID = 0);
                    entity.tb_FinishedGoodsInvDetails.ForEach(c => c.Sub_ID = 0);
                }
            }


            DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInv>(entity, t => t.DeliveryBillNo, txtDeliveryBillNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_FinishedGoodsInv>(entity, t => t.IsOutSourced, chkIsOutSourced, false);
            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);

            //创建表达式 外发工厂
            var lambdaOut = Expressionable.Create<tb_CustomerVendor>()
                            .And(t => t.IsOther == true)
                            .ToExpression();

            BaseProcessor baseProcessorOut = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CustomerVendor).Name + "Processor");
            QueryFilter queryFilterCOut = baseProcessorOut.GetQueryFilter();
            queryFilterCOut.FilterLimitExpressions.Add(lambdaOut);
            //第三个参数 k => k.CustomerVendor_ID.Value  不是out这个。只是为了取参数名。用于绑定控件
            DataBindingHelper.BindData4Cmb<tb_CustomerVendor, tb_ManufacturingOrder>(entity, k => k.CustomerVendor_ID, v => v.CVName, k => k.CustomerVendor_ID.Value, cmbCustomerVendor_ID, true, queryFilterCOut.GetFilterExpression<tb_CustomerVendor>());
            DataBindingHelper.InitFilterForControlByExp<tb_CustomerVendor>(entity, cmbCustomerVendor_ID, c => c.CVName, queryFilterCOut);

            //DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID);

            DataBindingHelper.BindData4DataTime<tb_FinishedGoodsInv>(entity, t => t.DeliveryDate, dtpDeliveryDate, false);
            DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInv>(entity, t => t.ShippingWay, txtShippingWay, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInv>(entity, t => t.TrackNo, txtTrackNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_FinishedGoodsInv>(entity, t => t.GeneEvidence, chkGeneEvidence, false);
            DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInv>(entity, t => t.TotalNetWorkingHours, txtTotalNetWorkingHours, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInv>(entity, t => t.TotalApportionedCost.ToString(), txtTotalApportionedCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInv>(entity, t => t.TotalManuFee.ToString(), txtTotalManuFee, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInv>(entity, t => t.TotalNetMachineHours.ToString(), txtTotalNetMachineHours, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInv>(entity, t => t.TotalProductionCost.ToString(), txtTotalProductionCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInv>(entity, t => t.TotalMaterialCost.ToString(), txtTotalMaterialCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);

            DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInv>(entity, t => t.TotalQty.ToString(), txtTotalQty, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInv>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInv>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4ControlByEnum<tb_FinishedGoodsInv>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_FinishedGoodsInv>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));
            //DataBindingHelper.BindData4Cmb<tb_PurOrder>(entity, k => k.PurOrder_ID, v => v.PurOrderNo, cmbPOID);
            if (entity.tb_FinishedGoodsInvDetails != null && entity.tb_FinishedGoodsInvDetails.Count > 0)
            {
                sgh.LoadItemDataToGrid<tb_FinishedGoodsInvDetail>(grid1, sgd, entity.tb_FinishedGoodsInvDetails, c => c.ProdDetailID);
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_FinishedGoodsInvDetail>(grid1, sgd, new List<tb_FinishedGoodsInvDetail>(), c => c.ProdDetailID);
            }

            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {
                //权限允许
                if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
                {

                }
                //如果是销售订单引入变化则加载明细及相关数据
                if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
                {
                    if (entity.MOID > 0 && s2.PropertyName == entity.GetPropertyName<tb_FinishedGoodsInv>(c => c.MOID))
                    {
                        LoadSubLines(entity.MOID);
                    }

                    if (s2.PropertyName == entity.GetPropertyName<tb_FinishedGoodsInv>(c => c.DepartmentID))
                    {
                        if (cmbDepartmentID.SelectedIndex == 0)
                        {
                            entity.DepartmentID = null;
                        }
                    }
                    if (s2.PropertyName == entity.GetPropertyName<tb_FinishedGoodsInv>(c => c.CustomerVendor_ID))
                    {
                        if (cmbCustomerVendor_ID.SelectedIndex == 0)
                        {
                            entity.CustomerVendor_ID = null;
                        }
                    }
                    if (s2.PropertyName == entity.GetPropertyName<tb_FinishedGoodsInv>(c => c.IsOutSourced))
                    {
                        cmbCustomerVendor_ID.Visible = entity.IsOutSourced;
                        if (entity.IsOutSourced)
                        {
                            entity.DepartmentID = null;
                        }
                        else
                        {
                            cmbCustomerVendor_ID.Visible = false;
                        }
                    }
                    ToolBarEnabledControl(entity);
                }
                ShowPrintStatus(lblPrintStatus, EditEntity);

            };


            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_FinishedGoodsInvValidator>(), kryptonSplitContainer1.Panel1.Controls);
            }
            ShowPrintStatus(lblPrintStatus, EditEntity);

            ////创建表达式
            //var lambda = Expressionable.Create<tb_CustomerVendor>()
            //                .And(t => t.IsVendor == true)
            //                .ToExpression();//注意 这一句 不能少
            //BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CustomerVendor).Name + "Processor");
            //QueryFilter queryFilterC = baseProcessor.GetQueryFilter();
            //queryFilterC.FilterLimitExpressions.Add(lambda);
            //DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, queryFilterC.GetFilterExpression<tb_CustomerVendor>(), true);
            //DataBindingHelper.InitFilterForControlByExp<tb_CustomerVendor>(entity, cmbCustomerVendor_ID, c => c.CVName, queryFilterC);


            tb_PurOrderController<tb_PurOrder> ctrPurorder = Startup.GetFromFac<tb_PurOrderController<tb_PurOrder>>();

            //先绑定这个。InitFilterForControl 这个才生效
            DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInv>(entity, v => v.MONo, txtRef_BillNo, BindDataType4TextBox.Text, true);
            //if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            //{
            //创建表达式  草稿 结案 和没有提交的都不显示
            var lambdaOrder = Expressionable.Create<tb_ManufacturingOrder>()
                        .And(t => t.DataStatus == (int)DataStatus.确认)
                         .And(t => t.isdeleted == false)
                        .ToExpression();//注意 这一句 不能少
                                        //base.InitFilterForControl<tb_PurOrder, tb_PurOrderQueryDto>(entity, txtPurOrderNO, c => c.PurOrderNo, lambdaOrder, ctrPurorder.GetQueryParameters());

            BaseProcessor basePro = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_ManufacturingOrder).Name + "Processor");
            QueryFilter queryFilter = basePro.GetQueryFilter();

            queryFilter.FilterLimitExpressions.Add(lambdaOrder);//意思是只有审核确认的。没有结案的。才能查询出来。

            ControlBindingHelper.ConfigureControlFilter<tb_FinishedGoodsInv, tb_ManufacturingOrder>(entity, txtRef_BillNo, t => t.MONo,
             f => f.MONO, queryFilter, a => a.MOID, b => b.MOID, null, false);


            //}
            ToolBarEnabledControl(entity);
        }


        SourceGridDefine sgd = null;
        SourceGridHelper sgh = new SourceGridHelper();


        private void UCStockIn_Load(object sender, EventArgs e)
        {


            base.ToolBarEnabledControl(MenuItemEnums.刷新);


            grid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;


            List<SGDefineColumnItem> listCols = new List<SGDefineColumnItem>();
            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<ProductSharePart, tb_FinishedGoodsInvDetail>(c => c.ProdDetailID, false);

            listCols.SetCol_NeverVisible<tb_FinishedGoodsInvDetail>(c => c.ProdDetailID);
            listCols.SetCol_NeverVisible<tb_FinishedGoodsInvDetail>(c => c.FG_ID);
            listCols.SetCol_NeverVisible<tb_FinishedGoodsInvDetail>(c => c.Sub_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Standard_Price);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.TransPrice);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Inv_Cost);
            //这个小计可以删除。全是单个的。不用小计了。
            //listCols.SetCol_NeverVisible<tb_FinishedGoodsInvDetail>(c => c.SubtotalMaterialCost);

            // listCols.SetCol_Width<tb_FinishedGoodsInvDetail>(c => c.ApportionedCost, 200);

            if (!AppContext.SysConfig.UseBarCode)
            {
                listCols.SetCol_NeverVisible<ProductSharePart>(c => c.BarCode);
            }
            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);

            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Unit_ID);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Brand);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.prop);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.CNName);
            // listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Inv_Cost);
            listCols.SetCol_ReadOnly<tb_FinishedGoodsInvDetail>(c => c.PayableQty);
            listCols.SetCol_ReadOnly<tb_FinishedGoodsInvDetail>(c => c.UnpaidQty);
            listCols.SetCol_ReadOnly<tb_FinishedGoodsInvDetail>(c => c.UnitCost);
            //listCols.SetCol_ReadOnly<tb_FinishedGoodsInvDetail>(c => c.UnitCost);
            //listCols.SetCol_ReadOnly<tb_FinishedGoodsInvDetail>(c => c.ProductionAllCost);
            // listCols.SetCol_ReadOnly<tb_FinishedGoodsInvDetail>(c => c.MaterialCost);

            // listCols.SetCol_ReadOnly<tb_FinishedGoodsInvDetail>(c => c.ApportionedCost);
            //listCols.SetCol_Format<tb_FinishedGoodsInvDetail>(c => c.r, CustomFormatType.PercentFormat);

            listCols.SetCol_Format<tb_FinishedGoodsInvDetail>(c => c.UnitCost, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_FinishedGoodsInvDetail>(c => c.ProductionAllCost, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_FinishedGoodsInvDetail>(c => c.MaterialCost, CustomFormatType.CurrencyFormat);
            //listCols.SetCol_Format<tb_FinishedGoodsInvDetail>(c => c.SubtotalMaterialCost, CustomFormatType.CurrencyFormat);

            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridMasterData = EditEntity;
            /*
            //具体审核权限的人才显示
            if (!AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {
                //listCols.SetCol_NeverVisible<tb_FinishedGoodsInvDetail>(c => c.UnitPrice);
                //listCols.SetCol_NeverVisible<tb_FinishedGoodsInvDetail>(c => c.TransactionPrice);
                //listCols.SetCol_NeverVisible<tb_FinishedGoodsInvDetail>(c => c.SubtotalPirceAmount);
            }*/

            listCols.SetCol_Summary<tb_FinishedGoodsInvDetail>(c => c.Qty);
            listCols.SetCol_Summary<tb_FinishedGoodsInvDetail>(c => c.UnpaidQty);
            listCols.SetCol_Summary<tb_FinishedGoodsInvDetail>(c => c.PayableQty);
            listCols.SetCol_Summary<tb_FinishedGoodsInvDetail>(c => c.NetWorkingHours);
            listCols.SetCol_Summary<tb_FinishedGoodsInvDetail>(c => c.NetMachineHours);
            listCols.SetCol_Summary<tb_FinishedGoodsInvDetail>(c => c.ApportionedCost);
            // listCols.SetCol_Summary<tb_FinishedGoodsInvDetail>(c => c.SubtotalMaterialCost);
            listCols.SetCol_Summary<tb_FinishedGoodsInvDetail>(c => c.ManuFee);
            listCols.SetCol_Summary<tb_FinishedGoodsInvDetail>(c => c.ProductionAllCost);

            listCols.SetCol_Formula<tb_FinishedGoodsInvDetail>((a, b) => a.PayableQty - b.Qty, c => c.UnpaidQty);
            //除数不能为0
            //测试条件行不行。测试abc 只用a。多个参数行不行。
            // listCols.SetCol_FormulaReverse<tb_FinishedGoodsInvDetail>((a) => a.Qty != 0,(a, b, c) => a.NetWorkingHours * (b.Qty / c.PayableQty), c => c.NetWorkingHours);
            // listCols.SetCol_FormulaReverse<tb_FinishedGoodsInvDetail>((a) => a.Qty != 0, (a) => a.NetMachineHours * (a.Qty / a.PayableQty), c => c.NetMachineHours);
            // listCols.SetCol_FormulaReverse<tb_FinishedGoodsInvDetail>((a) => a.Qty != 0, (a) => a.ApportionedCost * (a.Qty / a.PayableQty), c => c.ApportionedCost);
            // listCols.SetCol_FormulaReverse<tb_FinishedGoodsInvDetail>((a) => a.Qty != 0, (a) => a.ManuFee * (a.Qty / a.PayableQty), c => c.ManuFee);
            // listCols.SetCol_Formula<tb_FinishedGoodsInvDetail>((a, b) => a.Qty * b.MaterialCost, c => c.SubtotalMaterialCost);
            listCols.SetCol_Formula<tb_FinishedGoodsInvDetail>((a) => a.Qty * a.MaterialCost + a.Qty * a.ApportionedCost + a.Qty * a.ManuFee, c => c.ProductionAllCost);
            listCols.SetCol_Formula<tb_FinishedGoodsInvDetail>((a) => a.MaterialCost + a.ApportionedCost + a.ManuFee, c => c.UnitCost);

            sgh.SetPointToColumnPairs<ProductSharePart, tb_FinishedGoodsInvDetail>(sgd, f => f.Location_ID, t => t.Location_ID);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_FinishedGoodsInvDetail>(sgd, f => f.prop, t => t.property);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_FinishedGoodsInvDetail>(sgd, f => f.Rack_ID, t => t.Rack_ID);

            //应该只提供一个结构
            List<tb_FinishedGoodsInvDetail> lines = new List<tb_FinishedGoodsInvDetail>();
            bindingSourceSub.DataSource = lines;
            sgd.BindingSourceLines = bindingSourceSub;

            sgd.SetDependencyObject<ProductSharePart, tb_FinishedGoodsInvDetail>(MainForm.Instance.list);

            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_FinishedGoodsInvDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnLoadMultiRowData += Sgh_OnLoadMultiRowData;
            UIHelper.ControlMasterColumnsInvisible(CurMenuInfo, this);
        }
        private void Sgh_OnLoadMultiRowData(object rows, Position position)
        {
            List<View_ProdDetail> RowDetails = new List<View_ProdDetail>();
            var rowss = ((IEnumerable<dynamic>)rows).ToList();
            foreach (var item in rowss)
            {
                RowDetails.Add(item);
            }
            if (RowDetails != null)
            {
                List<tb_FinishedGoodsInvDetail> details = new List<tb_FinishedGoodsInvDetail>();
                
                foreach (var item in RowDetails)
                {
                    tb_FinishedGoodsInvDetail bOM_SDetail = MainForm.Instance.mapper.Map<tb_FinishedGoodsInvDetail>(item);
                    bOM_SDetail.Qty = 0;
                    details.Add(bOM_SDetail);
                }
                sgh.InsertItemDataToGrid<tb_FinishedGoodsInvDetail>(grid1, sgd, details, c => c.ProdDetailID, position);
            }
        }

        private void Sgh_OnCalculateColumnValue(object _rowObj, SourceGridDefine myGridDefine, SourceGrid.Position position)
        {

            if (EditEntity == null)
            {
                //都不是正常状态
                MainForm.Instance.uclog.AddLog("请先使用新增或查询加载数据");
                return;
            }
            try
            {

                //计算总金额  这些逻辑是不是放到业务层？后面要优化
                List<tb_FinishedGoodsInvDetail> details = sgd.BindingSourceLines.DataSource as List<tb_FinishedGoodsInvDetail>;
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }
                EditEntity.TotalNetWorkingHours = details.Sum(c => c.Qty * c.NetMachineHours);
                EditEntity.TotalNetMachineHours = details.Sum(c => c.Qty * c.NetWorkingHours);
                EditEntity.TotalMaterialCost = details.Sum(c => c.Qty * c.MaterialCost);
                EditEntity.TotalApportionedCost = details.Sum(c => c.Qty * c.ApportionedCost);
                EditEntity.TotalManuFee = details.Sum(c => c.Qty * c.ManuFee);
                EditEntity.TotalQty = details.Sum(c => c.Qty);
                EditEntity.TotalProductionCost = details.Sum(c => c.ProductionAllCost);
            }
            catch (Exception ex)
            {
                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }


        }

        List<tb_FinishedGoodsInvDetail> details = new List<tb_FinishedGoodsInvDetail>();
        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }
            // var eer = errorProviderForAllInput.GetError(txtto);
            bindingSourceSub.EndEdit();
            List<tb_FinishedGoodsInvDetail> detailentity = bindingSourceSub.DataSource as List<tb_FinishedGoodsInvDetail>;
            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.ProdDetailID > 0).ToList();
                //details = details.Where(t => t.ProdDetailID > 0).ToList();
                //如果没有有效的明细。直接提示
                if (NeedValidated && details.Count == 0 && NeedValidated)
                {
                    MessageBox.Show("请录入有效明细记录！");
                    return false;
                }
                //var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                //if (aa.Count > 0)
                //{
                //    System.Windows.Forms.MessageBox.Show("明细中，相同的产品不能多行录入,如有需要,请另建单据保存!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}

                EditEntity.tb_FinishedGoodsInvDetails = details;
                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }

                if (EditEntity.TotalQty != details.Sum(c => c.Qty))
                {
                    System.Windows.Forms.MessageBox.Show("单据总数量和明细数量的和不相等，请检查记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }


                ///如果缴库数量大于制令单数据是错误的。TODO::
                //if (_purorder != null)
                //{    
                //     if (EditEntity > _purorder.TotalQty)
                //     {
                //         MainForm.Instance.uclog.AddLog("入库总数量不可能大于订单数量，请检查数据是否正确！PurOrderNo:" + _purorder.PurOrderNo, UILogType.错误);
                //         MessageBox.Show("入库总数量不可能大于订单数量，请检查数据是否正确！");
                //         return;
                //     }
                //}


                if (NeedValidated && !base.Validator<tb_FinishedGoodsInvDetail>(details))
                {
                    return false;
                }
                //设置目标ID成功后就行头写上编号？
                //   表格中的验证提示
                //   其他输入条码验证


                ReturnMainSubResults<tb_FinishedGoodsInv> SaveResult = new ReturnMainSubResults<tb_FinishedGoodsInv>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.DeliveryBillNo}。");
                    }
                    else
                    {
                        MainForm.Instance.PrintInfoLog($"保存失败,{SaveResult.ErrorMsg}。", Color.Red);
                    }
                }
                return SaveResult.Succeeded;

            }
            return false;

        }

        /*
/// <summary>
/// 采购入库审核成功后。如果有对应的采购订单引入，则将其结案，并把数量回写？
/// </summary>
/// <returns></returns>
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
                MainForm.Instance.uclog.AddLog("已经审核,且【同意】的单据不能重复审核。");
                return null;
            }
        }
    }

    RevertCommand command = new RevertCommand();
    //缓存当前编辑的对象。如果撤销就回原来的值
    tb_FinishedGoodsInv oldobj = CloneHelper.DeepCloneObject<tb_FinishedGoodsInv>(EditEntity);
    command.UndoOperation = delegate ()
    {
        //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
        CloneHelper.SetValues<tb_FinishedGoodsInv>(EditEntity, oldobj);
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
    tb_FinishedGoodsInvController<tb_FinishedGoodsInv> ctr = Startup.GetFromFac<tb_FinishedGoodsInvController<tb_FinishedGoodsInv>>();
    List<tb_FinishedGoodsInv> tb_PurEntries = new List<tb_FinishedGoodsInv>();
    tb_PurEntries.Add(EditEntity);
    ReturnResults<tb_FinishedGoodsInv> rrs = await ctr.ApprovalAsync(EditEntity);
    if (rrs.Succeeded)
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
        //如果审核结果为不通过时，审核不是灰色。
        if (!ae.ApprovalResults)
        {
            toolStripbtnReview.Enabled = true;
        }
    }
    else
    {
        //审核失败 要恢复之前的值
        command.Undo();
        MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核失败,{rrs.ErrorMsg},请联系管理员！", Color.Red);
        toolStripbtnReview.Enabled = true;
    }

    return ae;
}


/// <summary>
/// 反审核
/// </summary>
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


    if (EditEntity.tb_FinishedGoodsInvDetails == null || EditEntity.tb_FinishedGoodsInvDetails.Count == 0)
    {
        MainForm.Instance.uclog.AddLog("单据中没有明细数据，请确认录入了完整数量和金额。", UILogType.警告);
        return ae;
    }

    RevertCommand command = new RevertCommand();

    tb_FinishedGoodsInv oldobj = CloneHelper.DeepCloneObject<tb_FinishedGoodsInv>(EditEntity);
    command.UndoOperation = delegate ()
    {
        CloneHelper.SetValues<tb_FinishedGoodsInv>(EditEntity, oldobj);
    };

    tb_FinishedGoodsInvController<tb_FinishedGoodsInv> ctr = Startup.GetFromFac<tb_FinishedGoodsInvController<tb_FinishedGoodsInv>>();

    ReturnResults<bool> rs = await ctr.AntiApprovalAsync(EditEntity);
    if (rs.Succeeded)
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
        MainForm.Instance.PrintInfoLog($"{EditEntity.DeliveryBillNo}反审失败,{rs.ErrorMsg},请联系管理员！", Color.Red);
    }
    return ae;
}

*/


        string orderid = string.Empty;


        //后面再优化。相当于多个制令单的批量转单
        private async void LoadSubLines(long? MainID)
        {
            ButtonSpecAny bsa = (txtRef_BillNo as KryptonTextBox).ButtonSpecs.FirstOrDefault(c => c.UniqueName == "btnQuery");
            if (bsa == null)
            {
                return;
            }
            var SourceBill = bsa.Tag as tb_ManufacturingOrder;
            SourceBill = await MainForm.Instance.AppContext.Db.Queryable<tb_ManufacturingOrder>()
                // .Includes(c => c.tb_MaterialRequisitions, b => b.tb_MaterialRequisitionDetails)
                // .Includes(c => c.tb_FinishedGoodsInvs, b => b.tb_FinishedGoodsInvDetails)
                .Where(c => c.MOID == MainID && c.DataStatus == (int)DataStatus.确认)
            .SingleAsync();
            //新增时才可以转单
            if (SourceBill != null && SourceBill.ManufacturingQty != SourceBill.QuantityDelivered
                && SourceBill.QuantityDelivered < SourceBill.ManufacturingQty)
            {

                
                tb_FinishedGoodsInv entity = MainForm.Instance.mapper.Map<tb_FinishedGoodsInv>(SourceBill);
                entity.MONo = SourceBill.MONO;
                entity.MOID = SourceBill.MOID;
                entity.DeliveryDate = System.DateTime.Now;
                entity.tb_manufacturingorder = SourceBill;
                entity.IsOutSourced = SourceBill.IsOutSourced;
                if (entity.IsOutSourced)
                {
                    entity.CustomerVendor_ID = SourceBill.CustomerVendor_ID_Out;
                }
                else
                {
                    entity.CustomerVendor_ID = null;
                }
                entity.DepartmentID = SourceBill.DepartmentID;
                List<tb_FinishedGoodsInvDetail> NewDetails = new List<tb_FinishedGoodsInvDetail>(); //这里是多行>
                List<string> tipsMsg = new List<string>();
                //一个制令就一个成品，就一行数据。将来优化同时 多个制令的批量转单
                //可以多次缴库
                #region 每行产品ID唯一

                tb_FinishedGoodsInvDetail NewDetail = MainForm.Instance.mapper.Map<tb_FinishedGoodsInvDetail>(SourceBill);

                NewDetail.PayableQty = SourceBill.ManufacturingQty - SourceBill.QuantityDelivered;
                NewDetail.Qty = 0;
                NewDetail.UnpaidQty = NewDetail.PayableQty - NewDetail.Qty;// 已经交数量去掉
                NewDetail.Location_ID = SourceBill.Location_ID;

                //NewDetail.SubtotalMaterialCost = SourceBill.TotalMaterialCost;
                //这里根据制令单的时间 费用假设全缴库时算出单位时间
                //再手动输入实缴时再算

                NewDetail.NetWorkingHours = decimal.Round(SourceBill.WorkingHour / SourceBill.ManufacturingQty, 4);
                NewDetail.NetMachineHours = decimal.Round(SourceBill.MachineHour / SourceBill.ManufacturingQty, 4);

                NewDetail.MaterialCost = decimal.Round(SourceBill.TotalMaterialCost / SourceBill.ManufacturingQty, 4);
                NewDetail.ManuFee = decimal.Round(SourceBill.TotalManuFee / SourceBill.ManufacturingQty, 4);
                NewDetail.ApportionedCost = decimal.Round(SourceBill.ApportionedCost / SourceBill.ManufacturingQty, 4);

                NewDetail.UnitCost = NewDetail.MaterialCost + NewDetail.ManuFee + NewDetail.ApportionedCost;
                NewDetail.ProductionAllCost = decimal.Round(NewDetail.UnitCost * NewDetail.Qty, 4);

                #endregion
                NewDetails.Add(NewDetail);
                if (NewDetails.Count == 0)
                {
                    tipsMsg.Add($"制令单:{entity.MONo}已全部缴库，请检查数据！");
                }
                StringBuilder msg = new StringBuilder();
                foreach (var item in tipsMsg)
                {
                    msg.Append(item).Append("\r\n");
                }
                if (tipsMsg.Count > 0)
                {
                    MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                entity.tb_FinishedGoodsInvDetails = NewDetails;
                //entity.PurOrder_ID = purorder.PurOrder_ID;
                //entity.PurOrder_NO = purorder.PurOrderNo;
                //entity.TotalAmount = NewDetails.Sum(c => c.SubtotalAmount);
                //entity.tot = NewDetails.Sum(c => c.Quantity);
                //entity.ActualAmount = entity.ShippingCost + entity.TotalAmount;

                entity.DataStatus = (int)DataStatus.草稿;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                entity.ApprovalResults = null;
                entity.ApprovalOpinions = "";
                entity.Modified_at = null;
                entity.Modified_by = null;
                entity.Approver_at = null;
                entity.Approver_by = null;
                entity.PrintStatus = 0;
                entity.ActionStatus = ActionStatus.新增;

                BusinessHelper.Instance.InitEntity(entity);

                ActionStatus actionStatus = ActionStatus.无操作;
                BindData(entity, actionStatus);
                base.BindData(entity);
            }
        }

        private void lblTotalNetMachineHours_Click(object sender, EventArgs e)
        {

        }


    }
}
