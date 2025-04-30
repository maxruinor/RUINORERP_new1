using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common;
using RUINORERP.UI.Common;
using RUINORERP.Model;
using RUINORERP.Business;
using RUINORERP.UI.UCSourceGrid;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Common.CollectionExtension;
using static RUINORERP.UI.Common.DataBindingHelper;
using static RUINORERP.UI.Common.GUIUtils;
using RUINORERP.Model.Dto;
using DevAge.Windows.Forms;
using RUINORERP.Common.Helper;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Global;
using RUINORERP.UI.Report;
using RUINORERP.UI.BaseForm;

using Microsoft.Extensions.Logging;
using RUINOR.Core;
using SqlSugar;
using AutoMapper;
using RUINORERP.Business.AutoMapper;
using System.Linq.Expressions;
using Krypton.Toolkit;
using RUINORERP.Business.Processor;
using FastReport.Fonts;
using RUINORERP.UI.PSI.PUR;
using static StackExchange.Redis.Role;
using RUINORERP.Business.CommService;
using RUINORERP.Common.Extensions;
using RUINORERP.Business.Security;
using NPOI.SS.Formula.Functions;
using RUINORERP.Global.EnumExt;
namespace RUINORERP.UI.PSI.SAL
{
    [MenuAttrAssemblyInfo("销售出库单", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.销售管理, BizType.销售出库单)]
    public partial class UCSaleOut : BaseBillEditGeneric<tb_SaleOut, tb_SaleOutDetail>
    {
        public UCSaleOut()
        {
            InitializeComponent();
            //InitDataToCmbByEnumDynamicGeneratedDataSource<tb_SaleOut>(typeof(Priority), e => e.OrderPriority, cmbOrderPriority);

            base.toolStripButton结案.Visible = true;
        }





        internal override void LoadDataToUI(object Entity)
        {
            ActionStatus actionStatus = ActionStatus.无操作;
            BindData(Entity as tb_SaleOut, actionStatus);
        }


        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            base.QueryConditionBuilder();
            var lambda = Expressionable.Create<tb_SaleOut>()
           .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
            .ToExpression();
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);

        }


        public override void BindData(tb_SaleOut entity, ActionStatus actionStatus)
        {
            if (entity == null)
            {

                return;
            }

            if (entity != null)
            {
                if (entity.SaleOut_MainID > 0)
                {
                    entity.PrimaryKeyID = entity.SaleOut_MainID;
                    entity.ActionStatus = ActionStatus.加载;
                    if (entity.Currency_ID.HasValue && entity.Currency_ID != AppContext.BaseCurrency.Currency_ID && entity.ExchangeRate.HasValue)
                    {
                        lblExchangeRate.Visible = true;
                        txtExchangeRate.Visible = true;
                        UIHelper.ControlForeignFieldInvisible<tb_SaleOrder>(this, true);

                    }
                    else
                    {
                        lblExchangeRate.Visible = false;
                        txtExchangeRate.Visible = false;
                        UIHelper.ControlForeignFieldInvisible<tb_SaleOrder>(this, false);
                    }
                    //entity.DataStatus = (int)DataStatus.确认;
                    //如果审核了，审核要灰色

                }
                else
                {
                    entity.ActionStatus = ActionStatus.新增;
                    entity.DataStatus = (int)DataStatus.草稿;
                    entity.OutDate = System.DateTime.Now;
                    if (string.IsNullOrEmpty(entity.SaleOutNo))
                    {
                        entity.SaleOutNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.销售出库单);
                    }

                    if (entity.tb_SaleOutDetails != null && entity.tb_SaleOutDetails.Count > 0)
                    {
                        entity.tb_SaleOutDetails.ForEach(c => c.SaleOut_MainID = 0);
                        entity.tb_SaleOutDetails.ForEach(c => c.SaleOutDetail_ID = 0);
                    }
                    entity.Currency_ID = AppContext.BaseCurrency.Currency_ID;
                    lblExchangeRate.Visible = false;
                    txtExchangeRate.Visible = false;
                }
            }
            if (entity.ApprovalStatus.HasValue)
            {
                lblReview.Text = ((ApprovalStatus)entity.ApprovalStatus).ToString();
            }

            EditEntity = entity;

            DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v => v.ProjectGroupName, cmbProjectGroup);
            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, c => c.IsCustomer == true);
            DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v => v.CurrencyName, cmbCurrency_ID);
            //DataBindingHelper.BindData4Cmb<tb_SaleOrder>(entity, k => k.SOrder_ID, v => v.SOrderNo, cmbOrder_ID);
            DataBindingHelper.BindData4CmbByEnum<tb_SaleOut>(entity, k => k.PayStatus, typeof(PayStatus), cmbPayStatus, false);
            DataBindingHelper.BindData4Cmb<tb_PaymentMethod>(entity, k => k.Paytype_ID, v => v.Paytype_Name, cmbPaytype_ID);
            DataBindingHelper.BindData4CheckBox<tb_SaleOut>(entity, t => t.IsFromPlatform, chk平台单, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.PlatformOrderNo, txtPlatformOrderNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.SaleOutNo, txtSaleOutNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.ShipCost.ToString(), txtShipCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.FreightCost.ToString(), txtFreightCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.ExchangeRate.ToString(), txtExchangeRate, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4DataTime<tb_SaleOut>(entity, t => t.DeliveryDate, dtpDeliveryDate, false);
            DataBindingHelper.BindData4DataTime<tb_SaleOut>(entity, t => t.OutDate, dtpOutDate, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.ShippingAddress, txtShippingAddress, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.ShippingWay, txtShippingWay, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.TrackNo, txtTrackNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_SaleOut>(entity, t => t.ApprovalResults, chkApprovalResults, false);
            DataBindingHelper.BindData4CheckBox<tb_SaleOut>(entity, t => t.ReplaceOut, chk替代品出库, false);

            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.TotalCost.ToString(), txtTotalCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.TotalTaxAmount.ToString(), txtTaxAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.TotalQty.ToString(), txtTotalQty, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.ForeignTotalAmount.ToString(), txtForeignTotalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4CheckBox<tb_SaleOut>(entity, t => t.GenerateVouchers, chkGenerateVouchers, false);

            DataBindingHelper.BindData4CheckBox<tb_SaleOut>(entity, t => t.IsCustomizedOrder, chkIsCustomizedOrder, false);

            base.errorProviderForAllInput.DataSource = entity;
            base.errorProviderForAllInput.ContainerControl = this;
            //this.ValidateChildren();
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            DataBindingHelper.BindData4ControlByEnum<tb_SaleOut>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_SaleOut>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));
            if (entity.tb_SaleOutDetails != null && entity.tb_SaleOutDetails.Count > 0)
            {
                //LoadDataToGrid(entity.tb_SaleOutDetails);
                sgh.LoadItemDataToGrid<tb_SaleOutDetail>(grid1, sgd, entity.tb_SaleOutDetails, c => c.ProdDetailID);
            }
            else
            {
                //LoadDataToGrid(new List<tb_SaleOutDetail>());
                sgh.LoadItemDataToGrid<tb_SaleOutDetail>(grid1, sgd, new List<tb_SaleOutDetail>(), c => c.ProdDetailID);
            }

            //缓存一下结果下次如果一样，就忽略？
            //object tempcopy = entity.Clone();
            //如果属性变化 则状态为修改

            entity.PropertyChanged += async (sender, s2) =>
            {
                //如果是销售订单引入变化则加载明细及相关数据
                if ((entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改) && entity.SOrder_ID.HasValue && entity.SOrder_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_SaleOut>(c => c.SOrder_ID))
                {
                    await OrderToOutBill(entity.SOrder_ID.Value);
                    MainForm.Instance.PrintInfoLog("一次");
                }


                //权限允许
                if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
                {
                    if (s2.PropertyName == entity.GetPropertyName<tb_SaleOut>(c => c.Currency_ID) && entity.Currency_ID > 0)
                    {

                        if (cmbCurrency_ID.SelectedItem is tb_Currency cv)
                        {
                            if (cv.CurrencyCode.Trim() != DefaultCurrency.RMB.ToString())
                            {
                                //显示外币相关
                                UIHelper.ControlForeignFieldInvisible<tb_SaleOut>(this, true);
                                entity.ExchangeRate = BizService.GetExchangeRateFromCache(cv.Currency_ID, AppContext.BaseCurrency.Currency_ID);
                                if (EditEntity.Currency_ID != AppContext.BaseCurrency.Currency_ID && EditEntity.ExchangeRate.HasValue)
                                {
                                    EditEntity.ForeignTotalAmount = EditEntity.TotalAmount / EditEntity.ExchangeRate.Value;
                                    //
                                    EditEntity.ForeignTotalAmount = Math.Round(EditEntity.ForeignTotalAmount, 2); // 四舍五入到 2 位小数
                                }
                                lblExchangeRate.Visible = true;
                                txtExchangeRate.Visible = true;
                                lblForeignTotalAmount.Text = $"金额({cv.CurrencyCode})";
                            }
                            else
                            {
                                //隐藏外币相关
                                UIHelper.ControlForeignFieldInvisible<tb_SaleOut>(this, false);
                                lblExchangeRate.Visible = false;
                                txtExchangeRate.Visible = false;
                                entity.ExchangeRate = null;
                                entity.ForeignTotalAmount = 0;
                            }
                        }

                    }

                    if (s2.PropertyName == entity.GetPropertyName<tb_SaleOut>(c => c.Paytype_ID) && entity.Paytype_ID.HasValue && entity.Paytype_ID > 0)
                    {
                        if (cmbPaytype_ID.SelectedItem is tb_PaymentMethod paymentMethod)
                        {
                            EditEntity.tb_paymentmethod = paymentMethod;
                        }
                    }
                }


                if (entity.CustomerVendor_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_SaleOut>(c => c.CustomerVendor_ID))
                {
                    var obj = BizCacheHelper.Instance.GetEntity<tb_CustomerVendor>(entity.CustomerVendor_ID);
                    if (obj != null && obj.ToString() != "System.Object")
                    {
                        if (obj is tb_CustomerVendor cv)
                        {
                            EditEntity.Employee_ID = cv.Employee_ID;
                        }
                    }
                }

                //显示 打印状态 如果是草稿状态 不显示打印
                if ((DataStatus)entity.DataStatus != DataStatus.草稿)
                {
                    toolStripbtnPrint.Enabled = true;
                    if (entity.PrintStatus == 0)
                    {
                        lblPrintStatus.Text = "未打印";
                    }
                    else
                    {
                        lblPrintStatus.Text = $"打印{entity.PrintStatus}次";
                    }

                }
                else
                {
                    toolStripbtnPrint.Enabled = false;
                }

            };


            tb_CustomerVendorController<tb_CustomerVendor> cvctr = Startup.GetFromFac<tb_CustomerVendorController<tb_CustomerVendor>>();
            //创建表达式
            var lambda = Expressionable.Create<tb_CustomerVendor>()
                            .And(t => t.IsCustomer == true)
                            .ToExpression();//注意 这一句 不能少

            // base.InitFilterForControl<tb_CustomerVendor, tb_CustomerVendorQueryDto>(entity, cmbCustomerVendor_ID, c => c.CVName, lambda, cvctr.GetQueryParameters());
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CustomerVendor).Name + "Processor");
            QueryFilter queryFilterC = baseProcessor.GetQueryFilter();
            queryFilterC.FilterLimitExpressions.Add(lambda);
            DataBindingHelper.InitFilterForControlByExp<tb_CustomerVendor>(entity, cmbCustomerVendor_ID, c => c.CVName, queryFilterC);

            //先绑定这个。InitFilterForControl 这个才生效
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, v => v.SaleOrderNo, txtSaleOrder, BindDataType4TextBox.Text, true);
            DataBindingHelper.BindData4TextBoxWithTagQuery<tb_SaleOut>(entity, v => v.SOrder_ID, txtSaleOrder, true);

            //tb_SaleOrderController<tb_SaleOrder> ctrsaleorder = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();

            //创建表达式  草稿 结案 和没有提交的都不显示
            //var lambdaOrder = Expressionable.Create<tb_SaleOrder>()
            //                .And(t => t.DataStatus == (int)DataStatus.确认)
            //                 .And(t => t.isdeleted == false)
            //                .ToExpression();//注意 这一句 不能少
            //base.InitFilterForControl<tb_SaleOrder, tb_SaleOrderQueryDto>(entity, txtSaleOrder, c => c.SOrderNo, lambdaOrder, ctrsaleorder.GetQueryParameters());

            BaseProcessor basePro = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_SaleOrder).Name + "Processor");
            QueryFilter queryFilter = basePro.GetQueryFilter();

            var lambdaOrder = Expressionable.Create<tb_SaleOrder>()
             .And(t => t.DataStatus == (int)DataStatus.确认)
             .And(t => t.ApprovalStatus.HasValue && t.ApprovalStatus.Value == (int)ApprovalStatus.已审核)
             .And(t => t.ApprovalResults.HasValue && t.ApprovalResults.Value == true)
              .And(t => t.isdeleted == false)
             .ToExpression();//注意 这一句 不能少
            //如果有限制则设置一下 但是注意 不应该在这设置，灵活的应该是在调用层设置
            queryFilter.SetFieldLimitCondition(lambdaOrder);

            DataBindingHelper.InitFilterForControlByExp<tb_SaleOrder>(entity, txtSaleOrder, c => c.SOrderNo, queryFilter);


            sgd.GridMasterData = entity;
            sgd.GridMasterDataType = entity.GetType();

            base.BindData(entity);
        }

        public void InitDataTocmbbox()
        {
            DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.InitDataToCmb<tb_CustomerVendor>(k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, c => c.IsCustomer == true);
        }

        /*
         重要思路提示，这个表格控件，数据源的思路是，
        产品共享表 ProductSharePart+tb_SaleOutDetail 有合体，SetDependencyObject 根据字段名相同的给值，值来自产品明细表
         并且，表格中可以编辑的需要查询的列是唯一能查到详情的

        SetDependencyObject<P, S, T>   P包含S字段包含T字段--》是有且包含
         */



        SourceGridDefine sgd = null;
        //        SourceGridHelper<View_ProdDetail, tb_SaleOutDetail> sgh = new SourceGridHelper<View_ProdDetail, tb_SaleOutDetail>();
        SourceGridHelper sgh = new SourceGridHelper();
        //设计关联列和目标列
        View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
        List<View_ProdDetail> list = new List<View_ProdDetail>();

        List<SGDefineColumnItem> listCols = new List<SGDefineColumnItem>();
        private void UcSaleOrderEdit_Load(object sender, EventArgs e)
        {
            InitDataTocmbbox();
            base.ToolBarEnabledControl(MenuItemEnums.刷新);
            grid1.BorderStyle = BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;

            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<ProductSharePart, tb_SaleOutDetail>(c => c.ProdDetailID, false);

            listCols.SetCol_NeverVisible<tb_SaleOutDetail>(c => c.SaleOutDetail_ID);
            listCols.SetCol_NeverVisible<tb_SaleOutDetail>(c => c.ProdDetailID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.VendorModelCode);

            listCols.SetCol_ReadOnly<tb_SaleOutDetail>(c => c.Cost);
            if (!AppContext.CurUserInfo.UserInfo.IsSuperUser)
            {
                listCols.SetCol_ReadOnly<tb_SaleOutDetail>(c => c.UnitPrice);
                listCols.SetCol_ReadOnly<tb_SaleOutDetail>(c => c.TransactionPrice);
            }
            listCols.SetCol_ReadOnly<tb_SaleOutDetail>(c => c.SubtotalTransAmount);
            listCols.SetCol_ReadOnly<tb_SaleOutDetail>(c => c.CustomizedCost);
            listCols.SetCol_ReadOnly<tb_SaleOutDetail>(c => c.SubtotalCostAmount);


            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);
            listCols.SetCol_Format<tb_SaleOutDetail>(c => c.Discount, CustomFormatType.PercentFormat);
            listCols.SetCol_Format<tb_SaleOutDetail>(c => c.TaxRate, CustomFormatType.PercentFormat);
            sgd = new SourceGridDefine(grid1, listCols, true);

            /*
            //具体审核权限的人才显示
            if (AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {
                listCols.SetCol_Summary<tb_SaleOutDetail>(c => c.TotalCostAmount, true);
            }
            */

            listCols.SetCol_Formula<tb_SaleOutDetail>((a, b) => a.UnitPrice * b.Discount, c => c.TransactionPrice);
            //listCols.SetCol_Formula<tb_SaleOutDetail>((a, b) => a.Cost * b.Quantity, c => c.SubtotalCostAmount);
            listCols.SetCol_Formula<tb_SaleOutDetail>((a, b) => (a.Cost + a.CustomizedCost) * b.Quantity, c => c.SubtotalCostAmount);
            listCols.SetCol_Formula<tb_SaleOutDetail>((a, b) => a.TransactionPrice * b.Quantity, c => c.SubtotalTransAmount);
            listCols.SetCol_Formula<tb_SaleOutDetail>((a, b, c) => a.SubtotalTransAmount / (1 + b.TaxRate) * c.TaxRate, d => d.SubtotalTaxAmount);
            //将数量默认为已出库数量  这个逻辑不对这个是订单累计 的出库数量只能是在出库审核时才累计数据，这里最多只读
            //listCols.SetCol_Formula<tb_SaleOutDetail>((a, b) => a.Quantity, c => c.TotalDeliveredQty);

            listCols.SetCol_Summary<tb_SaleOutDetail>(c => c.Quantity);
            listCols.SetCol_Summary<tb_SaleOutDetail>(c => c.CommissionAmount);

            sgh.SetPointToColumnPairs<ProductSharePart, tb_SaleOutDetail>(sgd, f => f.Location_ID, t => t.Location_ID);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_SaleOutDetail>(sgd, f => f.Rack_ID, t => t.Rack_ID);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_SaleOutDetail>(sgd, f => f.Inv_Cost, t => t.Cost);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_SaleOutDetail>(sgd, f => f.Standard_Price, t => t.UnitPrice);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_SaleOutDetail>(sgd, f => f.prop, t => t.property);

            //应该只提供一个结构
            List<tb_SaleOutDetail> lines = new List<tb_SaleOutDetail>();
            bindingSourceSub.DataSource = lines;
            sgd.BindingSourceLines = bindingSourceSub;

            //Expression<Func<View_ProdDetail, bool>> exp = Expressionable.Create<View_ProdDetail>() //创建表达式
            //     .AndIF(true, w => w.CNName.Length > 0)
            //    // .AndIF(txtSpecifications.Text.Trim().Length > 0, w => w.Specifications.Contains(txtSpecifications.Text.Trim()))
            //    .ToExpression();//注意 这一句 不能少
            //                    // StringBuilder sb = new StringBuilder();
            ///// sb.Append(string.Format("{0}='{1}'", item.ColName, valValue));
            //list = dc.BaseQueryByWhere(exp);
            list = MainForm.Instance.list;
            sgd.SetDependencyObject<ProductSharePart, tb_SaleOutDetail>(list);
            sgd.HasRowHeader = true;

            sgh.InitGrid(grid1, sgd, true, nameof(tb_SaleOutDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnGetTransferDataHandler += Sgh_OnGetTransferDataHandler;
            UIHelper.ControlMasterColumnsInvisible(CurMenuInfo, this);
        }

        private tb_ProdConversion Sgh_OnGetTransferDataHandler(ToolStripItem sender, object rowObj, SourceGridDefine CurrGridDefine)
        {
            if (rowObj == null || !(rowObj is tb_SaleOutDetail))
            {
                return null;
            }
            tb_ProdConversion prodConversion = new tb_ProdConversion();

            prodConversion.tb_ProdConversionDetails = new List<tb_ProdConversionDetail>();
            //销售的明细作为转换的明细中的来源
            tb_SaleOutDetail outDetail = rowObj as tb_SaleOutDetail;
            tb_ProdConversionDetail conversionDetail = new tb_ProdConversionDetail();
            prodConversion.Location_ID = outDetail.Location_ID;
            View_ProdDetail ViewDetail = list.FirstOrDefault(c => c.ProdDetailID == outDetail.ProdDetailID && c.Location_ID == outDetail.Location_ID);

            conversionDetail.property_from = outDetail.property;
            conversionDetail.ConversionQty = outDetail.Quantity;
            conversionDetail.CNName_from = ViewDetail.CNName;
            conversionDetail.Specifications_from = ViewDetail.Specifications;
            conversionDetail.ProdDetailID_from = outDetail.ProdDetailID;
            conversionDetail.Model_from = ViewDetail.Model;
            if (ViewDetail.Type_ID.HasValue)
            {
                conversionDetail.Type_ID_from = ViewDetail.Type_ID.Value;
                conversionDetail.Type_ID_to = ViewDetail.Type_ID.Value;//因为不好处理为null。就认为和来源一样
            }
            conversionDetail.BarCode_from = ViewDetail.BarCode;
            conversionDetail.SKU_from = ViewDetail.SKU;

            prodConversion.tb_ProdConversionDetails.Add(conversionDetail);
            return prodConversion;
        }

        private void Sgh_OnCalculateColumnValue(object rowObj, SourceGridDefine griddefine, SourceGrid.Position Position)
        {
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
                List<tb_SaleOutDetail> details = sgd.BindingSourceLines.DataSource as List<tb_SaleOutDetail>;
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }
                EditEntity.TotalQty = details.Sum(c => c.Quantity);
                EditEntity.TotalCost = details.Sum(c => c.Cost * c.Quantity);
                EditEntity.TotalCost = EditEntity.TotalCost + EditEntity.FreightCost;

                EditEntity.TotalTaxAmount = details.Sum(c => c.SubtotalTaxAmount);
                EditEntity.TotalTaxAmount = EditEntity.TotalTaxAmount.ToRoundDecimalPlaces(MainForm.Instance.authorizeController.GetMoneyDataPrecision());

                EditEntity.TotalAmount = details.Sum(c => c.TransactionPrice * c.Quantity);
                EditEntity.TotalAmount = EditEntity.TotalAmount + EditEntity.ShipCost;
                if (EditEntity.Currency_ID.HasValue && EditEntity.Currency_ID != AppContext.BaseCurrency.Currency_ID && EditEntity.ExchangeRate.HasValue)
                {
                    EditEntity.ForeignTotalAmount = EditEntity.TotalAmount / EditEntity.ExchangeRate.Value;
                    //
                    EditEntity.ForeignTotalAmount = Math.Round(EditEntity.ForeignTotalAmount, 2); // 四舍五入到 2 位小数
                }

            }
            catch (Exception ex)
            {
                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }
        }

        List<tb_SaleOutDetail> details = new List<tb_SaleOutDetail>();
        /// <summary>
        /// 查询结果 选中行的变化事件
        /// </summary>
        /// <param name="entity"></param>

        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }
            var eer = errorProviderForAllInput.GetError(txtTotalAmount);

            bindingSourceSub.EndEdit();

            List<tb_SaleOutDetail> oldOjb = new List<tb_SaleOutDetail>(details.ToArray());

            List<tb_SaleOutDetail> detailentity = bindingSourceSub.DataSource as List<tb_SaleOutDetail>;


            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.ProdDetailID > 0).ToList();


                //如果没有有效的明细。直接提示
                if (NeedValidated && details.Count == 0)
                {
                    MessageBox.Show("请录入有效明细记录！");
                    return false;
                }

                //var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                //if (aa.Count > 1)
                //{
                //    System.Windows.Forms.MessageBox.Show("明细中，相同的产品不能多行录入,如有需要,请另建单据保存!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}

                EditEntity.tb_SaleOutDetails = details;
                if (NeedValidated && (EditEntity.tb_SaleOutDetails == null || EditEntity.tb_SaleOutDetails.Count == 0))
                {
                    MainForm.Instance.uclog.AddLog("单据中没有明细数据，请确认录入了完整数量和金额。", UILogType.警告);
                    return false;
                }

                //如果所有数据都出库。金额也要一致。否则提醒
                if (NeedValidated && EditEntity.tb_saleorder != null)
                {
                    //如果总数量一致。金额不一致。提示
                    if (EditEntity.TotalQty == EditEntity.tb_saleorder.TotalQty && EditEntity.TotalAmount != EditEntity.tb_saleorder.TotalAmount)
                    {
                        MessageBox.Show($"出库总金额{EditEntity.TotalAmount}与订单总金额{EditEntity.tb_saleorder.TotalAmount}不一致，请检查数据后再试。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }

                //如果没有有效的明细。直接提示
                if (NeedValidated && details.Count == 0)
                {
                    MessageBox.Show("请录入有效明细记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (NeedValidated && (EditEntity.TotalQty == 0 || detailentity.Sum(c => c.Quantity) == 0))
                {
                    MessageBox.Show("单据及明细总数量不为能0！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (NeedValidated && EditEntity.TotalQty != details.Sum(c => c.Quantity))
                {
                    MessageBox.Show($"单据总数量{EditEntity.TotalQty}和明细总数量{detailentity.Sum(c => c.Quantity)}不相同，请检查后再试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (NeedValidated && (EditEntity.TotalAmount == 0 || detailentity.Sum(c => c.TransactionPrice * c.Quantity) == 0))
                {
                    if (MessageBox.Show("单据总金额或明细总金额为零，你确定吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return false;
                    }
                }
                //if (NeedValidated && (EditEntity.TotalAmount != detailentity.Sum(c => c.TransactionPrice * c.Quantity)))
                //{
                //    MessageBox.Show("单据总金额与明细总金额不相等！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return false;
                //}
                if (NeedValidated && (EditEntity.TotalAmount + EditEntity.ShipCost < detailentity.Sum(c => c.TransactionPrice * c.Quantity)))
                {
                    MessageBox.Show("单据总金额不能小于明细总金额！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_SaleOutDetail>(details))
                {
                    return false;
                }

                //二选中，验证机制还没有弄好。先这里处理
                if (NeedValidated && (EditEntity.CustomerVendor_ID == 0 || EditEntity.CustomerVendor_ID == -1))
                {
                    System.Windows.Forms.MessageBox.Show("往来单位选择不能为空，请检查记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (EditEntity.Employee_ID == 0 || EditEntity.Employee_ID == -1)
                {
                    EditEntity.Employee_ID = null;
                }


                EditEntity.TotalQty = details.Sum(c => c.Quantity);
                if (NeedValidated && EditEntity.TotalQty != details.Sum(c => c.Quantity))
                {
                    System.Windows.Forms.MessageBox.Show("单据总数量和明细数量的和不相等，请检查记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                if (NeedValidated)
                {
                    if (EditEntity.tb_SaleOutRes != null && EditEntity.tb_SaleOutRes.Count > 0)
                    {
                        MessageBox.Show("当前销售出库单已有销售出库退回数据，无法修改保存。请联系仓库处理。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }

                ReturnMainSubResults<tb_SaleOut> SaveResult = new ReturnMainSubResults<tb_SaleOut>();
                if (NeedValidated)
                {
                    //await MainForm.Instance.AppContext.Db.UpdateNav<tb_SaleOut>(EditEntity,
                    //new UpdateNavRootOptions() { IsInsertRoot = true })
                    //   .Include(b => b.tb_SaleOutDetails, new UpdateNavOptions()
                    //   {
                    //       OneToManyInsertOrUpdate = true
                    //   })
                    //   .ExecuteCommandAsync();

                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.SaleOutNo}。");
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

        protected async override Task<ReturnResults<tb_SaleOut>> Delete()
        {
            ReturnResults<tb_SaleOut> rss = new ReturnResults<tb_SaleOut>();
            rss = await base.Delete();
            if (rss.Succeeded)
            {
                string msg = string.Empty;
                msg = $"订单号：{EditEntity.SaleOrderNo} 对应的出库单 {EditEntity.SaleOutNo} 删除成功。";
                AuditLogHelper.Instance.CreateAuditLog<tb_SaleOut>("删除细节", EditEntity, msg);
            }
            return rss;
        }

        protected async override Task<bool> CloseCaseAsync()
        {
            if (EditEntity == null)
            {
                return false;
            }


            //CommonUI.frmOpinion frm = new CommonUI.frmOpinion();
            //if (frm.ShowDialog() == DialogResult.OK)//审核了。不管是同意还是不同意
            //{
            List<tb_SaleOut> EditEntitys = new List<tb_SaleOut>();
            // EditEntity.CloseCaseOpinions = frm.txtOpinion.Text;
            EditEntitys.Add(EditEntity);
            //已经审核的并且通过的情况才能结案
            List<tb_SaleOut> needCloseCases = EditEntitys.Where(c => c.DataStatus == (int)DataStatus.确认 && c.ApprovalStatus == (int)ApprovalStatus.已审核 && c.ApprovalResults.HasValue && c.ApprovalResults.Value).ToList();
            if (needCloseCases.Count == 0)
            {
                MainForm.Instance.PrintInfoLog($"要结案的数据为：{needCloseCases.Count}:请检查数据！");
                return false;
            }
            tb_SaleOutController<tb_SaleOut> ctr = Startup.GetFromFac<tb_SaleOutController<tb_SaleOut>>();
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
                MainForm.Instance.PrintInfoLog($"结案操作失败,原因是{rs.ErrorMsg},如果无法解决，请联系管理员！", Color.Red);
            }
            return true;
            //}
            //else
            //{
            //    return false;
            //}

        }



        string saleorderid = string.Empty;
        private async Task<tb_SaleOut> OrderToOutBill(long _sorderid)
        {
            tb_SaleOrder saleorder;
            ButtonSpecAny bsa = txtSaleOrder.ButtonSpecs.FirstOrDefault(c => c.UniqueName == "btnQuery");
            if (bsa == null)
            {
                return null;
            }
            //saleorder = bsa.Tag as tb_SaleOrder;
            saleorder = await MainForm.Instance.AppContext.Db.Queryable<tb_SaleOrder>()
            .Includes(a => a.tb_SaleOuts)
            .Includes(a => a.tb_SaleOrderDetails, b => b.tb_proddetail, c => c.tb_prod)
            .Where(c => c.SOrder_ID == _sorderid)
            .SingleAsync();
            tb_SaleOrderController<tb_SaleOrder> ctr = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
            //tb_SaleOut saleOut = SaleOrderToSaleOut(item);
            tb_SaleOut saleOut = ctr.SaleOrderToSaleOut(saleorder);
            ActionStatus actionStatus = ActionStatus.无操作;
            BindData(saleOut, actionStatus);
            return saleOut;
        }
        /*
        private async Task<tb_SaleOut> OrderToOutBill(long _sorderid)
        {
            tb_SaleOrder saleorder;
            ButtonSpecAny bsa = txtSaleOrder.ButtonSpecs.FirstOrDefault(c => c.UniqueName == "btnQuery");
            if (bsa == null)
            {
                return null;
            }
            //saleorder = bsa.Tag as tb_SaleOrder;
            saleorder = await MainForm.Instance.AppContext.Db.Queryable<tb_SaleOrder>()
            .Includes(a => a.tb_SaleOuts)
            .Includes(a => a.tb_SaleOrderDetails, b => b.tb_proddetail, c => c.tb_prod)
            .Where(c => c.SOrder_ID == _sorderid)
            .SingleAsync();

            IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
            tb_SaleOut entity = mapper.Map<tb_SaleOut>(saleorder);
            List<string> tipsMsg = new List<string>();
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

            //如果这个订单已经有出库单 则第二次运费为0
            if (saleorder.tb_SaleOuts != null && saleorder.tb_SaleOuts.Count > 0)
            {
                if (saleorder.ShipCost > 0)
                {
                    tipsMsg.Add($"当前订单已经有出库记录，运费收入已经计入前面出库单，当前出库运费收入为零！");
                    entity.ShipCost = 0;
                }
                else
                {
                    tipsMsg.Add($"当前订单已经有出库记录！");
                }
            }

            if (saleorder.DeliveryDate.HasValue)
            {
                entity.OutDate = saleorder.DeliveryDate.Value;
                entity.DeliveryDate = saleorder.DeliveryDate;
            }
            else
            {
                entity.OutDate = System.DateTime.Now;
                entity.DeliveryDate = System.DateTime.Now;
            }

            if (entity.SOrder_ID.HasValue && entity.SOrder_ID > 0)
            {
                entity.CustomerVendor_ID = saleorder.CustomerVendor_ID;
                entity.SaleOrderNo = saleorder.SOrderNo;
                entity.PlatformOrderNo = saleorder.PlatformOrderNo;
                entity.IsFromPlatform = saleorder.IsFromPlatform;
            }

            List<tb_SaleOutDetail> details = mapper.Map<List<tb_SaleOutDetail>>(saleorder.tb_SaleOrderDetails);
            List<tb_SaleOutDetail> NewDetails = new List<tb_SaleOutDetail>();

            for (global::System.Int32 i = 0; i < details.Count; i++)
            {
                var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                if (aa.Count > 0 && details[i].SaleOrderDetail_ID > 0)
                {
                    #region 产品ID可能大于1行，共用料号情况
                    tb_SaleOrderDetail item = saleorder.tb_SaleOrderDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID 
                    && c.Location_ID==details[i].Location_ID
                    && c.SaleOrderDetail_ID == details[i].SaleOrderDetail_ID);
                    details[i].Cost = item.Cost;
                    details[i].CustomizedCost = item.CustomizedCost;
                    //这时有一种情况就是订单时没有成本。没有产品。出库前有类似采购入库确定的成本
                    if (details[i].Cost == 0)
                    {
                        View_ProdDetail obj = BizCacheHelper.Instance.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                        if (obj != null && obj.GetType().Name != "Object" && obj is View_ProdDetail prodDetail)
                        {
                            details[i].Cost = obj.Inv_Cost.Value;
                        }
                    }
                    details[i].Quantity = item.Quantity - item.TotalDeliveredQty;// 已经出数量去掉
                    details[i].SubtotalTransAmount = details[i].TransactionPrice * details[i].Quantity;
                    details[i].SubtotalCostAmount = (details[i].Cost + details[i].CustomizedCost) * details[i].Quantity;
                    if (details[i].Quantity > 0)
                    {
                        NewDetails.Add(details[i]);
                    }
                    else
                    {
                        tipsMsg.Add($"销售订单{saleorder.SOrderNo}，{item.tb_proddetail.tb_prod.CNName + item.tb_proddetail.tb_prod.Specifications}已出库数为{item.TotalDeliveredQty}，可出库数为{details[i].Quantity}，当前行数据忽略！");
                    }

                    #endregion
                }
                else
                {
                    #region 每行产品ID唯一
                    tb_SaleOrderDetail item = saleorder.tb_SaleOrderDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID
                      && c.Location_ID == details[i].Location_ID
                    && c.SaleOrderDetail_ID == details[i].SaleOrderDetail_ID);
                    details[i].Cost = item.Cost;
                    details[i].CustomizedCost = item.CustomizedCost;
                    //这时有一种情况就是订单时没有成本。没有产品。出库前有类似采购入库确定的成本
                    if (details[i].Cost == 0)
                    {
                        View_ProdDetail obj = BizCacheHelper.Instance.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                        if (obj != null && obj.GetType().Name != "Object" && obj is View_ProdDetail prodDetail)
                        {
                            if (obj.Inv_Cost == null)
                            {
                                obj.Inv_Cost = 0;
                            }
                            details[i].Cost = obj.Inv_Cost.Value;
                        }
                    }
                    details[i].Quantity = details[i].Quantity - item.TotalDeliveredQty;// 减掉已经出库的数量
                    details[i].SubtotalTransAmount = details[i].TransactionPrice * details[i].Quantity;
                    details[i].SubtotalCostAmount = (details[i].Cost + details[i].CustomizedCost) * details[i].Quantity;

                    if (details[i].Quantity > 0)
                    {
                        NewDetails.Add(details[i]);
                    }
                    else
                    {
                        tipsMsg.Add($"当前订单的SKU:{item.tb_proddetail.SKU}已出库数量为{details[i].Quantity}，当前行数据将不会加载到明细！");
                    }
                    #endregion
                }

            }

            if (NewDetails.Count == 0)
            {
                tipsMsg.Add($"订单:{entity.SaleOrderNo}已全部出库，请检查是否正在重复出库！");
            }

            entity.tb_SaleOutDetails = NewDetails;

            entity.TotalQty = NewDetails.Sum(c => c.Quantity);
            entity.TotalCost = NewDetails.Sum(c => c.Cost * c.Quantity);
            entity.TotalCost = entity.TotalCost + entity.FreightCost;

            entity.TotalTaxAmount = NewDetails.Sum(c => c.SubtotalTaxAmount);
            entity.TotalTaxAmount = entity.TotalTaxAmount.ToRoundDecimalPlaces(MainForm.Instance.authorizeController.GetMoneyDataPrecision());

            entity.TotalUntaxedAmount = NewDetails.Sum(c => c.SubtotalUntaxedAmount);
            entity.TotalUntaxedAmount = entity.TotalUntaxedAmount + entity.ShipCost;

            entity.TotalAmount = NewDetails.Sum(c => c.TransactionPrice * c.Quantity);
            entity.TotalAmount = entity.TotalAmount + entity.ShipCost;
            entity.CollectedMoney = entity.TotalAmount;

            if (saleorder.tb_SaleOrderDetails.Sum(c => c.SubtotalTransAmount) != NewDetails.Sum(c => c.SubtotalTransAmount) &&
                saleorder.tb_SaleOuts != null && saleorder.tb_SaleOuts.Count == 0)
            {
                tipsMsg.Add($"当前引用订单:{entity.SaleOrderNo}与当前出库明细累计金额不同，请注意检查！");
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

            entity.tb_saleorder = saleorder;
            BusinessHelper.Instance.InitEntity(entity);
            ActionStatus actionStatus = ActionStatus.无操作;
            BindData(entity, actionStatus);
            return entity;
        }

        */

        private void chk替代品出库_CheckedChanged(object sender, EventArgs e)
        {
            if (chk替代品出库.Checked)
            {
                //提示 如果将当前订单以另一个产品替代出库时，将不会对订单进行数据检测更新。录入数据时，请仔细核对数据。
                MessageBox.Show("将当前订单以其它产品替代出库时，将不会对订单进行数据检测更新。录入数据时，请仔细核对数据。");
            }
        }
    }
}
