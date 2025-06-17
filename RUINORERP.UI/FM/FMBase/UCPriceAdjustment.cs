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
using SqlSugar;
using SourceGrid;
using System.Linq.Expressions;
using RUINORERP.Common.Extensions;
using TransInstruction;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;
using RUINOR.Core;
using AutoMapper;
using RUINORERP.Business.AutoMapper;
using Krypton.Toolkit;
using Netron.GraphLib;
using RUINORERP.UI.SysConfig;
using SourceGrid.Cells.Editors;
using RUINORERP.UI.MRP.MP;
using SourceGrid.Cells.Models;
using RUINORERP.UI.BI;
using RUINORERP.Global.EnumExt;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using System.Configuration;
using RUINORERP.UI.AdvancedUIModule;

namespace RUINORERP.UI.FM
{

    /// <summary>
    /// 价格调整单
    /// </summary>
    public partial class UCPriceAdjustment : BaseBillEditGeneric<tb_FM_PriceAdjustment, tb_FM_PriceAdjustmentDetail>, IPublicEntityObject
    {
        public UCPriceAdjustment()
        {
            InitializeComponent();
            AddPublicEntityObject(typeof(ProductSharePart));
        }
      
        /// <summary>
        /// 收付款方式决定对应的菜单功能
        /// </summary>
        public ReceivePaymentType PaymentType { get; set; }
        /// <summary>
        /// 加载下拉值
        /// </summary>
        public void InitDataTocmbbox()
        {
            lblPrintStatus.Text = "";
            lblReview.Text = "";
            DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.InitDataToCmb<tb_Currency>(k => k.Currency_ID, v => v.CurrencyName, cmbCurrency_ID);
        }
        //internal override void LoadDataToUI(object Entity)
        //{
        //    BindData(Entity as tb_FM_PriceAdjustment);
        //}

        tb_FM_PriceAdjustmentController<tb_FM_PriceAdjustment> ctr = Startup.GetFromFac<tb_FM_PriceAdjustmentController<tb_FM_PriceAdjustment>>();
        public override void BindData(tb_FM_PriceAdjustment entity, ActionStatus actionStatus)
        {
            if (entity == null)
            {
                return;
            }
            EditEntity = entity;
            if (entity.AdjustId > 0)
            {
                entity.PrimaryKeyID = entity.AdjustId;
                entity.ActionStatus = ActionStatus.加载;

                //如果状态是已经生效才可能有审核，如果是待收款 才可能有反审
                if (entity.DataStatus == (long)DataStatus.新建)
                {
                    base.toolStripbtnReview.Visible = true;
                }
                else
                {
                    base.toolStripbtnReview.Visible = false;
                }

                if (entity.DataStatus == (long)DataStatus.确认)
                {
                    base.toolStripBtnReverseReview.Visible = true;
                }
                else
                {
                    base.toolStripBtnReverseReview.Visible = false;
                }
            }
            else
            {
                entity.ActionStatus = ActionStatus.新增;
                entity.DataStatus = (int)DataStatus.草稿;
                entity.ReceivePaymentType = (int)PaymentType;
                entity.ActionStatus = ActionStatus.新增;
                entity.AdjustDate = System.DateTime.Now;
                //到期日期应该是根据对应客户的账期的天数来算
                entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                if (string.IsNullOrEmpty(entity.AdjustNo))
                {
                    if (PaymentType == ReceivePaymentType.收款)
                    {
                        entity.AdjustNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.销售价格调整单);
                    }
                    else
                    {
                        entity.AdjustNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.采购价格调整单);
                    }
                }
            }

            DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustment>(entity, t => t.AdjustNo, txtAdjustNo, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustment>(entity, t => t.ExchangeRate.ToString(), txtExchangeRate, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustment>(entity, t => t.TotalForeignDiffAmount.ToString(), txtTotalForeignDiffAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustment>(entity, t => t.TotalLocalDiffAmount.ToString(), txtTotalLocalDiffAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustment>(entity, t => t.AdjustReason, txtAdjustReason, BindDataType4TextBox.Text, false);
            

            //先绑定这个。InitFilterForControl 这个才生效
            DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustment>(entity, v => v.SourceBillNo, txtSourceBillNo, BindDataType4TextBox.Text, true, false);

            if (PaymentType == ReceivePaymentType.收款)
            {
                entity.SourceBizType = (int)BizType.销售出库单;
                BaseProcessor baseProSaleOut = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_SaleOut).Name + "Processor");
                QueryFilter queryFilterSaleOut = baseProSaleOut.GetQueryFilter();
                //出库了，审核过的才用价格调整单
                var lambdaSaleOut = Expressionable.Create<tb_SaleOut>()
             .And(t => t.DataStatus == (int)DataStatus.确认)
             .And(t => t.ApprovalStatus.HasValue && t.ApprovalStatus.Value == (int)ApprovalStatus.已审核)
             .And(t => t.ApprovalResults.HasValue && t.ApprovalResults.Value == true)
              .And(t => t.isdeleted == false)
             .ToExpression();
                queryFilterSaleOut.SetFieldLimitCondition(lambdaSaleOut);
                ControlBindingHelper.ConfigureControlFilter<tb_FM_PriceAdjustment, tb_SaleOut>(entity, txtSourceBillNo,
                    t => t.SourceBillNo,
                    f => f.SaleOutNo, queryFilterSaleOut, a => a.SourceBillId, b => b.SaleOut_MainID, null, false);
            }
            else
            {
                entity.SourceBizType = (int)BizType.采购入库单;
                BaseProcessor baseProPurEntry = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_PurEntry).Name + "Processor");
                QueryFilter queryFilterPurEntry = baseProPurEntry.GetQueryFilter();
                //出库了，审核过的才用价格调整单
                var lambdaPurEntry = Expressionable.Create<tb_PurEntry>()
             .And(t => t.DataStatus == (int)DataStatus.确认)
             .And(t => t.ApprovalStatus.HasValue && t.ApprovalStatus.Value == (int)ApprovalStatus.已审核)
             .And(t => t.ApprovalResults.HasValue && t.ApprovalResults.Value == true)
              .And(t => t.isdeleted == false)
             .ToExpression();
                queryFilterPurEntry.SetFieldLimitCondition(lambdaPurEntry);

                ControlBindingHelper.ConfigureControlFilter<tb_FM_PriceAdjustment, tb_PurEntry>(entity, txtSourceBillNo,
                    t => t.SourceBillNo,
                    f => f.PurEntryNo, queryFilterPurEntry, a => a.SourceBillId, b => b.PurEntryID, null, false);

            }

            DataBindingHelper.BindData4DataTime<tb_FM_PriceAdjustment>(entity, t => t.AdjustDate, dtpAdjustDate, false);
            DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);
            DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v => v.ProjectGroupName, cmbProjectGroup_ID);

            DataBindingHelper.BindData4CheckBox<tb_FM_PriceAdjustment>(entity, t => t.IsIncludeTax, chkIsIncludeTax, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustment>(entity, t => t.TaxTotalDiffLocalAmount.ToString(), txtTaxTotalDiffLocalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustment>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v => v.CurrencyName, cmbCurrency_ID);

            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            cmbCurrency_ID.SelectedIndex = 1;//默认第一个人民币
            DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustment>(entity, t => t.Remark, txtRemark, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4CheckBox<tb_FM_PriceAdjustment>(entity, t => t.ApprovalResults, chkApprovalResults, false);
            DataBindingHelper.BindData4ControlByEnum<tb_FM_PriceAdjustment>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_FM_PriceAdjustment>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));
            //显示 打印状态 如果是草稿状态 不显示打印
            ShowPrintStatus(lblPrintStatus, entity);


            if (PaymentType == ReceivePaymentType.收款)
            {
                //创建表达式
                var lambda = Expressionable.Create<tb_CustomerVendor>()
                            .And(t => t.IsCustomer == true)//供应商和第三方
                            .And(t => t.isdeleted == false)
                            .And(t => t.Is_enabled == true)
                            .ToExpression();//注意 这一句 不能少

                BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CustomerVendor).Name + "Processor");
                QueryFilter queryFilterC = baseProcessor.GetQueryFilter();
                queryFilterC.FilterLimitExpressions.Add(lambda);

                //带过滤的下拉绑定要这样
                DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, queryFilterC.GetFilterExpression<tb_CustomerVendor>(), true);
                DataBindingHelper.InitFilterForControlByExp<tb_CustomerVendor>(entity, cmbCustomerVendor_ID, c => c.CVName, queryFilterC);

            }
            else
            {
                //应付  付给供应商
                //创建表达式
                var lambda = Expressionable.Create<tb_CustomerVendor>()
                            .And(t => t.IsVendor == true)//供应商
                            .And(t => t.isdeleted == false)
                            .And(t => t.Is_enabled == true)
                            .ToExpression();//注意 这一句 不能少

                BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CustomerVendor).Name + "Processor");
                QueryFilter queryFilterC = baseProcessor.GetQueryFilter();
                queryFilterC.FilterLimitExpressions.Add(lambda);

                //带过滤的下拉绑定要这样
                DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, queryFilterC.GetFilterExpression<tb_CustomerVendor>(), true);
                DataBindingHelper.InitFilterForControlByExp<tb_CustomerVendor>(entity, cmbCustomerVendor_ID, c => c.CVName, queryFilterC);
            }

            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_FM_PriceAdjustmentValidator>(), kryptonPanel1.Controls);
            }

            if (entity.tb_FM_PriceAdjustmentDetails != null && entity.tb_FM_PriceAdjustmentDetails.Count > 0)
            {
                //新建和草稿时子表编辑也可以保存。
                foreach (var item in entity.tb_FM_PriceAdjustmentDetails)
                {
                    item.PropertyChanged += (sender, s1) =>
                    {
                        //权限允许
                        if ((true && entity.DataStatus == (long)DataStatus.草稿) ||
                        (true && entity.DataStatus == (long)DataStatus.新建))
                        {
                            EditEntity.ActionStatus = ActionStatus.修改;
                        }
                    };
                }
                sgh.LoadItemDataToGrid<tb_FM_PriceAdjustmentDetail>(grid1, sgd, entity.tb_FM_PriceAdjustmentDetails, c => c.ProdDetailID);
                // 模拟按下 Tab 键
                SendKeys.Send("{TAB}");//为了显示远程图片列
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_FM_PriceAdjustmentDetail>(grid1, sgd, new List<tb_FM_PriceAdjustmentDetail>(), c => c.ProdDetailID);
            }

            //如果属性变化 则状态为修改
            entity.PropertyChanged += async (sender, s2) =>
            {
                //权限允许
                if ((true && entity.DataStatus == (long)DataStatus.草稿)
                || (true && entity.DataStatus == (long)DataStatus.新建))
                {
                    if (entity.SourceBillId.HasValue && entity.SourceBillId.Value > 0 && s2.PropertyName == entity.GetPropertyName<tb_FM_PriceAdjustment>(c => c.SourceBillId))
                    {
                        var newEntity = await ctr.BuildPriceAdjustment(PaymentType, entity.SourceBillId.Value, entity.AdjustNo);
                        if (newEntity != null)
                        {
                            BindData(newEntity, actionStatus: ActionStatus.无操作);
                            EditEntity.ActionStatus = ActionStatus.修改;
                        }
                    }
                }

                //到期日期应该是根据对应客户的账期的天数来算
                if (entity.CustomerVendor_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_FM_PriceAdjustment>(c => c.CustomerVendor_ID))
                {
                    var obj = BizCacheHelper.Instance.GetEntity<tb_CustomerVendor>(entity.CustomerVendor_ID);
                    if (obj != null && obj.ToString() != "System.Object")
                    {
                        if (obj is tb_CustomerVendor cv)
                        {
                            entity.tb_customervendor = cv;
                        }
                    }
                }
                if (entity.Currency_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_FM_PriceAdjustment>(c => c.Currency_ID))
                {
                    //如果币别是本位币则不显示汇率列
                    if (EditEntity != null && EditEntity.Currency_ID == MainForm.Instance.AppContext.BaseCurrency.Currency_ID)
                    {
                        listCols.SetCol_NeverVisible<tb_FM_PriceAdjustmentDetail>(c => c.ExchangeRate);
                    }
                }

                base.ToolBarEnabledControl(entity);

            };

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

            base.BindData(entity);
        }


        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_FM_PriceAdjustment).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();

            //创建表达式
            var lambda = Expressionable.Create<tb_FM_PriceAdjustment>()
                             //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
                             // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
                             .And(t => t.isdeleted == false)
                            //报销人员限制，财务不限制 自己的只能查自己的
                            .AndIF(AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext), t => t.Created_by == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                            .ToExpression();//注意 这一句 不能少
            QueryConditionFilter.SetFieldLimitCondition(lambda);

        }

        private void Grid1_BindingContextChanged(object sender, EventArgs e)
        {

        }

        SourceGridDefine sgd = null;
        SourceGridHelper sgh = new SourceGridHelper();
        //设计关联列和目标列

        List<SGDefineColumnItem> listCols = new List<SGDefineColumnItem>();

        //设计关联列和目标列
        View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
        List<View_ProdDetail> list = new List<View_ProdDetail>();

        private void UCStockIn_Load(object sender, EventArgs e)
        {

            #region
            switch (PaymentType)
            {
                case ReceivePaymentType.收款:
                    lblBillText.Text = "销售价格调整单";
                    lblCustomerVendor_ID.Text = "客户";
                    break;
                case ReceivePaymentType.付款:
                    lblBillText.Text = "采购价格调整单";
                    lblCustomerVendor_ID.Text = "供应商";
                    break;
                default:
                    break;
            }

            #endregion

            MainForm.Instance.LoginWebServer();
            if (CurMenuInfo != null)
            {
                lblBillText.Text = CurMenuInfo.CaptionCN;
            }
            InitDataTocmbbox();
            base.ToolBarEnabledControl(MenuItemEnums.刷新);

            grid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;

            listCols = new List<SGDefineColumnItem>();
            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<ProductSharePart, tb_FM_PriceAdjustmentDetail>(c => c.ProdDetailID, false);


            listCols.SetCol_NeverVisible<tb_FM_PriceAdjustmentDetail>(c => c.AdjustDetailID);
            listCols.SetCol_NeverVisible<tb_FM_PriceAdjustmentDetail>(c => c.ProdDetailID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Rack_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.ShortCode);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Brand);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Location_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Model);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.VendorModelCode);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Images);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Inv_Cost);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Standard_Price);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.TransPrice);


            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);
            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);
            if (!AppContext.SysConfig.UseBarCode)
            {
                listCols.SetCol_NeverVisible<ProductSharePart>(c => c.BarCode);
            }
            //listCols.SetCol_DefaultValue<tb_FM_PriceAdjustmentDetail>(c => c.ForeignPayableAmount, 0.00M);

            //listCols.SetCol_DisplayFormatText<tb_FM_PriceAdjustmentDetail>(c => c.SourceBizType, 1);

            listCols.SetCol_Format<tb_FM_PriceAdjustmentDetail>(c => c.TaxRate, CustomFormatType.PercentFormat);
            listCols.SetCol_Format<tb_FM_PriceAdjustmentDetail>(c => c.SubtotalDiffLocalAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_FM_PriceAdjustmentDetail>(c => c.TaxDiffLocalAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_FM_PriceAdjustmentDetail>(c => c.TaxSubtotalDiffLocalAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_FM_PriceAdjustmentDetail>(c => c.OriginalUnitPrice, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_FM_PriceAdjustmentDetail>(c => c.AdjustedUnitPrice, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_FM_PriceAdjustmentDetail>(c => c.DiffUnitPrice, CustomFormatType.CurrencyFormat);

            //listCols.SetCol_Format<tb_FM_PriceAdjustmentDetail>(c => c.ForeignPayableAmount, CustomFormatType.CurrencyFormat);

            sgd = new SourceGridDefine(grid1, listCols, true);

            listCols.SetCol_Formula<tb_FM_PriceAdjustmentDetail>((a, b) => b.AdjustedUnitPrice - a.OriginalUnitPrice, c => c.DiffUnitPrice);
            listCols.SetCol_Formula<tb_FM_PriceAdjustmentDetail>((a, b) => (a.AdjustedUnitPrice - a.OriginalUnitPrice) * b.Quantity, c => c.SubtotalDiffLocalAmount);

            listCols.SetCol_FormulaReverse<tb_FM_PriceAdjustmentDetail>(d => d.AdjustedUnitPrice == 0 && d.DiffUnitPrice > 0, (a, b) => a.DiffUnitPrice + a.OriginalUnitPrice, c => c.AdjustedUnitPrice);


            listCols.SetCol_Formula<tb_FM_PriceAdjustmentDetail>((a, b) => a.DiffUnitPrice * b.Quantity, c => c.SubtotalDiffLocalAmount);
            listCols.SetCol_FormulaReverse<tb_FM_PriceAdjustmentDetail>(d => d.Quantity != 0, (a, b) => (a.SubtotalDiffLocalAmount / b.Quantity) - a.OriginalUnitPrice, c => c.AdjustedUnitPrice);
            sgd.GridMasterData = EditEntity;
            listCols.SetCol_Summary<tb_FM_PriceAdjustmentDetail>(c => c.SubtotalDiffLocalAmount);
            listCols.SetCol_Summary<tb_FM_PriceAdjustmentDetail>(c => c.TaxSubtotalDiffLocalAmount);
            listCols.SetCol_Formula<tb_FM_PriceAdjustmentDetail>((a, b, c) => a.DiffUnitPrice / (1 + b.TaxRate) * c.TaxRate, d => d.TaxDiffLocalAmount);
            listCols.SetCol_Formula<tb_FM_PriceAdjustmentDetail>((a, b) => a.TaxDiffLocalAmount * b.Quantity, c => c.TaxSubtotalDiffLocalAmount);
            /*
            listCols.SetCol_FormulaReverse<tb_SaleOrderDetail>(d => d.UnitPrice == 0 && d.Discount != 0 && d.TransactionPrice != 0, (a, b) => a.TransactionPrice / b.Discount, c => c.UnitPrice);//-->成交价是结果列
            //单价和成交价不一样时，并且单价不能为零时，可以计算出折扣的值 TODO:!!!!
            listCols.SetCol_FormulaReverse<tb_SaleOrderDetail>(d => d.UnitPrice != d.TransactionPrice || d.UnitPrice != 0, (a, b) => a.TransactionPrice / b.UnitPrice, c => c.Discount);//-->折扣
            listCols.SetCol_FormulaReverse<tb_SaleOrderDetail>(d => d.UnitPrice != 0, (a, b) => a.TransactionPrice / b.UnitPrice, c => c.Discount);//-->折扣 

            listCols.SetCol_FormulaReverse<tb_SaleOrderDetail>(d => d.Quantity != 0 && d.SubtotalTransAmount != 0, (a, b) => a.SubtotalTransAmount / b.Quantity, c => c.TransactionPrice);//-->成交价是结果列
            //listCols.SetCol_Formula<tb_SaleOrderDetail>(d => d.Quantity != 0,(a, b) => a.SubtotalTransAmount / b.Quantity, c => c.UnitPrice);//-->成交价是结果列

            listCols.SetCol_Formula<tb_SaleOrderDetail>((a, b) => a.TransactionPrice * b.Quantity, c => c.SubtotalTransAmount);

            //反算时还要加更复杂的逻辑：如果单价为0时，则可以反算到单价。折扣不变。（默认为1）， 如果单价有值。则反算折扣？成交价？



            listCols.SetCol_Formula<tb_SaleOrderDetail>((a, b, c) => a.SubtotalTransAmount / (1 + b.TaxRate) * c.TaxRate, d => d.SubtotalTaxAmount);
            listCols.SetCol_Formula<tb_SaleOrderDetail>((a, b) => (a.Cost + a.CustomizedCost) * b.Quantity, c => c.SubtotalCostAmount);

             */

            //公共到明细的映射 源 ，左边会隐藏
            sgh.SetPointToColumnPairs<ProductSharePart, tb_FM_PriceAdjustmentDetail>(sgd, f => f.Specifications, t => t.Specifications);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_FM_PriceAdjustmentDetail>(sgd, f => f.Specifications, t => t.CustomerPartNo);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_FM_PriceAdjustmentDetail>(sgd, f => f.Unit_ID, t => t.Unit_ID);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_FM_PriceAdjustmentDetail>(sgd, f => f.prop, t => t.property);

            //应该只提供一个结构
            List<tb_FM_PriceAdjustmentDetail> lines = new List<tb_FM_PriceAdjustmentDetail>();
            bindingSourceSub.DataSource = lines; //  ctrSub.Query(" 1>2 ");
            sgd.BindingSourceLines = bindingSourceSub;

            list = MainForm.Instance.list;
            sgd.SetDependencyObject<ProductSharePart, tb_FM_PriceAdjustmentDetail>(list);

            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_FM_PriceAdjustmentDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnAddDataRow += Sgh_OnAddDataRow;
            UIHelper.ControlMasterColumnsInvisible(CurMenuInfo, this);
        }

        private void Sgh_OnAddDataRow(object rowObj)
        {



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
                List<tb_FM_PriceAdjustmentDetail> details = sgd.BindingSourceLines.DataSource as List<tb_FM_PriceAdjustmentDetail>;
                details = details.Where(c => c.SubtotalDiffLocalAmount != 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("差异小计金额必须大于0");
                    return;
                }
                EditEntity.TotalLocalDiffAmount = details.Sum(c => c.SubtotalDiffLocalAmount);
                EditEntity.TaxTotalDiffLocalAmount = details.Sum(c => c.TaxSubtotalDiffLocalAmount);
            }
            catch (Exception ex)
            {

                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }


        }

        /// <summary>
        /// 保存图片到服务器。所有图片都保存到服务器。即使草稿换电脑还可以看到
        /// </summary>
        /// <param name="RemoteSave"></param>
        /// <returns></returns>
        private async Task<bool> SaveImage(bool RemoteSave)
        {
            bool result = true;
            foreach (tb_FM_PriceAdjustmentDetail detail in EditEntity.tb_FM_PriceAdjustmentDetails)
            {
                PropertyInfo[] props = typeof(tb_FM_PriceAdjustmentDetail).GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    var col = sgd[prop.Name];
                    if (col != null)
                    {
                        int realIndex = grid1.Columns.GetColumnInfo(col.UniqueId).Index;
                        if (col.CustomFormat == CustomFormatType.WebPathImage)
                        {
                            //保存图片到本地临时目录，图片数据保存在grid1控件中，所以要循环控件的行，控件真实数据行以1为起始
                            int totalRowsFlag = grid1.RowsCount;
                            if (grid1.HasSummary)
                            {
                                totalRowsFlag--;
                            }
                            for (int i = 1; i < totalRowsFlag; i++)
                            {
                                var model = grid1[i, realIndex].Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
                                SourceGrid.Cells.Models.ValueImageWeb valueImageWeb = (SourceGrid.Cells.Models.ValueImageWeb)model;

                                if (grid1[i, realIndex].Value == null)
                                {
                                    continue;
                                }
                                string fileName = string.Empty;
                                if (grid1[i, realIndex].Value.ToString().Contains(".jpg") && grid1[i, realIndex].Value.ToString() == detail.GetPropertyValue(prop.Name).ToString())
                                {
                                    fileName = grid1[i, realIndex].Value.ToString();
                                    //  fileName = System.IO.Path.Combine(Application.StartupPath + @"\temp\", fileName);
                                    //if (grid1[i, realIndex].Tag == null && valueImageWeb.CellImageBytes != null)
                                    if (valueImageWeb.CellImageBytes != null)
                                    {
                                        //保存到本地
                                        //if (EditEntity.DataStatus == (int)DataStatus.草稿)
                                        //{
                                        //    //保存在本地临时目录
                                        //    ImageProcessor.SaveBytesAsImage(valueImageWeb.CellImageBytes, fileName);
                                        //    grid1[i, realIndex].Tag = ImageHashHelper.GenerateHash(valueImageWeb.CellImageBytes);
                                        //}
                                        //else
                                        //{
                                        //上传到服务器，删除本地
                                        //实际应该可以直接传二进制数据，但是暂时没有实现，所以先保存到本地，再上传
                                        //ImageProcessor.SaveBytesAsImage(valueImageWeb.CellImageBytes, fileName);
                                        HttpWebService httpWebService = Startup.GetFromFac<HttpWebService>();
                                        ConfigManager configManager = Startup.GetFromFac<ConfigManager>();
                                        var upladurl = configManager.GetValue("WebServerUploadUrl");
                                        string uploadRsult = await httpWebService.UploadImageAsyncOK(upladurl, fileName, valueImageWeb.CellImageBytes, "upload");
                                        //string uploadRsult = await HttpHelper.UploadImageAsyncOK("http://192.168.0.99:8080/upload/", fileName, "upload");
                                        if (uploadRsult.Contains("上传成功"))
                                        {
                                            MainForm.Instance.PrintInfoLog(uploadRsult);
                                        }
                                        else
                                        {
                                            MainForm.Instance.PrintInfoLog("请重试！ " + uploadRsult);
                                            MainForm.Instance.LoginWebServer();
                                        }


                                        //}
                                    }
                                }

                                //UploadImage("http://127.0.0.1/upload", "D:/test.jpg", "upload");
                                // string uploadRsult = await HttpHelper.UploadImageAsync(AppContext.WebServerUrl + @"/upload", fileName, "amw");
                                //                            string uploadRsult = await HttpHelper.UploadImage(AppContext.WebServerUrl + @"/upload", fileName, "upload");

                            }


                        }
                    }
                }
            }
            return result;
        }

        List<tb_FM_PriceAdjustmentDetail> details = new List<tb_FM_PriceAdjustmentDetail>();
        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }

            var eer = errorProviderForAllInput.GetError(txtTotalLocalDiffAmount);
            bindingSourceSub.EndEdit();
            List<tb_FM_PriceAdjustmentDetail> detailentity = bindingSourceSub.DataSource as List<tb_FM_PriceAdjustmentDetail>;
            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.SubtotalDiffLocalAmount != 0 || t.ProdDetailID > 0).ToList();
                //如果没有有效的明细。直接提示
                if (NeedValidated && details.Count == 0)
                {
                    MessageBox.Show("请录入有效明细记录！");
                    return false;
                }

                EditEntity.tb_FM_PriceAdjustmentDetails = details;
                if (details.Sum(c => c.TaxSubtotalDiffLocalAmount) > 0)
                {
                    EditEntity.IsIncludeTax = true;
                }

                //如果主表的总金额和明细金额加总后不相等，则提示
                if (NeedValidated && EditEntity.TotalLocalDiffAmount != details.Sum(c => c.SubtotalDiffLocalAmount))
                {
                    if (MessageBox.Show("差异总金额和明细中差异金额小计之和不相等，你确定要保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.No)
                    {
                        return false;
                    }
                }
                if (NeedValidated && EditEntity.TaxTotalDiffLocalAmount != details.Sum(c => c.TaxSubtotalDiffLocalAmount))
                {
                    if (MessageBox.Show("总差异税额和明细差异税额小计之和不相等，你确定要保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.No)
                    {
                        return false;
                    }
                }


                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_FM_PriceAdjustmentDetail>(details))
                {
                    return false;
                }

                ReturnMainSubResults<tb_FM_PriceAdjustment> SaveResult = new ReturnMainSubResults<tb_FM_PriceAdjustment>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {

                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.AdjustNo}。");
                    }
                    else
                    {
                        MainForm.Instance.PrintInfoLog($"保存失败,{SaveResult.ErrorMsg}。", Color.Red);
                    }
                }
                return SaveResult.Succeeded;
            }
            else
            {
                MainForm.Instance.uclog.AddLog("加载状态下无法保存");
                return false;
            }
        }



        protected override async Task<bool> Submit()
        {
            bool rs = await base.Submit();
            if (rs)
            {
                ConfigManager configManager = Startup.GetFromFac<ConfigManager>();
                var temppath = configManager.GetValue("WebServerUrl");
                if (string.IsNullOrEmpty(temppath))
                {
                    MainForm.Instance.uclog.AddLog("请先配置图片服务器路径", UILogType.错误);
                }
            }
            return true;
        }



        protected async override Task<ReturnResults<tb_FM_PriceAdjustment>> Delete()
        {
            ReturnResults<tb_FM_PriceAdjustment> rss = new ReturnResults<tb_FM_PriceAdjustment>();
            if (EditEntity == null)
            {
                //提示一下删除成功
                MainForm.Instance.uclog.AddLog("提示", "没有要删除的数据");
                return rss;
            }

            if (MessageBox.Show("系统不建议删除单据资料\r\n确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                //https://www.runoob.com/w3cnote/csharp-enum.html
                var dataStatus = (DataStatus)(EditEntity.GetPropertyValue(typeof(DataStatus).Name).ToLong());
                if (dataStatus == DataStatus.新建 || dataStatus == DataStatus.草稿)
                {
                    //如果草稿。都可以删除。如果是新建，则提交过了。要创建人或超级管理员才能删除
                    if (dataStatus == DataStatus.新建 && !AppContext.IsSuperUser)
                    {
                        if (EditEntity.Created_by.Value != AppContext.CurUserInfo.Id)
                        {
                            MessageBox.Show("只有创建人才能删除【提交】的单据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            rss.ErrorMsg = "只有创建人才能删除【提交】的单据。";
                            rss.Succeeded = false;
                            return rss;
                        }
                    }

                    tb_FM_PriceAdjustmentController<tb_FM_PriceAdjustment> ctr = Startup.GetFromFac<tb_FM_PriceAdjustmentController<tb_FM_PriceAdjustment>>();
                    bool rs = await ctr.BaseLogicDeleteAsync(EditEntity as tb_FM_PriceAdjustment);
                    if (rs)
                    {
                        //MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("删除", EditEntity);
                        //if (MainForm.Instance.AppContext.SysConfig.IsDebug)
                        //{
                        //    //MainForm.Instance.logger.Debug($"单据显示中删除:{typeof(T).Name}，主键值：{PKValue.ToString()} "); //如果要生效 要将配置文件中 <add key="log4net.Internal.Debug" value="true " /> 也许是：logn4net.config <log4net debug="false"> 改为true
                        //}
                        // bindingSourceSub.Clear();

                        ////删除远程图片及本地图片
                        ///暂时使用了逻辑删除所以不执行删除远程图片操作。
                        //await DeleteRemoteImages();

                        //提示一下删除成功
                        MainForm.Instance.uclog.AddLog("提示", "删除成功");

                        //加载一个空的显示的UI
                        // bindingSourceSub.Clear();
                        //base.OnBindDataToUIEvent(EditEntity as tb_FM_PriceAdjustment, ActionStatus.删除);
                        Exit(this);
                    }
                }
                else
                {
                    //
                    MainForm.Instance.uclog.AddLog("提示", "已【确认】【审核】的生效单据无法删除");
                }
            }
            return rss;
        }



    }
}