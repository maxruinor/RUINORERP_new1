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
using RUINORERP.Model.CommonModel;
using RUINORERP.Business.FMService;

namespace RUINORERP.UI.FM
{

    /// <summary>
    /// 应收应付
    /// </summary>
    public partial class UCReceivablePayable : BaseBillEditGeneric<tb_FM_ReceivablePayable, tb_FM_ReceivablePayableDetail>, IPublicEntityObject, IToolStripMenuInfoAuth
    {
        public UCReceivablePayable()
        {
            InitializeComponent();
            AddPublicEntityObject(typeof(ProductSharePart));
        }

        public override void AddExcludeMenuList()
        {
            base.AddExcludeMenuList(MenuItemEnums.反结案);
            base.AddExcludeMenuList(MenuItemEnums.结案);
        }
        protected override void LoadRelatedDataToDropDownItems()
        {
            if (base.EditEntity is tb_FM_ReceivablePayable  receivablePayable)
            {
                if (receivablePayable.SourceBillId.HasValue)
                {
                    RelatedQueryParameter rqp = new RelatedQueryParameter();
                    rqp.bizType = (BizType)receivablePayable.SourceBizType;
                    rqp.billId = receivablePayable.SourceBillId.Value;
                    ToolStripMenuItem RelatedMenuItem = new ToolStripMenuItem();
                    RelatedMenuItem.Name = $"{rqp.billId}";
                    RelatedMenuItem.Tag = rqp;
                    RelatedMenuItem.Text = $"{rqp.bizType}:{receivablePayable.SourceBillNo}";
                    RelatedMenuItem.Click += base.MenuItem_Click;
                    if (!toolStripbtnRelatedQuery.DropDownItems.ContainsKey(receivablePayable.SourceBillId.Value.ToString()))
                    {
                        toolStripbtnRelatedQuery.DropDownItems.Add(RelatedMenuItem);
                    }
                }
            }
            base.LoadRelatedDataToDropDownItems();
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
        //    BindData(Entity as tb_FM_ReceivablePayable);
        //}
        public override void BindData(tb_FM_ReceivablePayable entity, ActionStatus actionStatus)
        {

            if (entity == null)
            {
                return;
            }

            EditEntity = entity;
            if (entity.ARAPId > 0)
                {
                if (entity.Currency_ID == MainForm.Instance.AppContext.BaseCurrency.Currency_ID)
                {
                    //隐藏外币相关
                    UIHelper.ControlForeignFieldInvisible<tb_FM_ReceivablePayable>(this, false);
                    if (listCols != null)
                    {
                        listCols.SetCol_DefaultHide<tb_FM_ReceivablePayableDetail>(c => c.ExchangeRate);
                    }
                }
                entity.PrimaryKeyID = entity.ARAPId;
                entity.ActionStatus = ActionStatus.加载;
                //如果审核了，审核要灰色
                if (entity.CustomerVendor_ID > 0)
                {
                    //如果线索引入相关数据
                    #region 收款信息可以根据往来单位带出 ，并且可以添加

                    //创建表达式
                    var lambdaPayeeInfo = Expressionable.Create<tb_FM_PayeeInfo>()
                                .And(t => t.Is_enabled == true)
                                .And(t => t.CustomerVendor_ID == entity.CustomerVendor_ID)
                                .ToExpression();//注意 这一句 不能少
                    BaseProcessor baseProcessorPayeeInfo = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_FM_PayeeInfo).Name + "Processor");
                    QueryFilter queryFilterPayeeInfo = baseProcessorPayeeInfo.GetQueryFilter();
                    queryFilterPayeeInfo.FilterLimitExpressions.Add(lambdaPayeeInfo);

                    DataBindingHelper.BindData4Cmb<tb_FM_PayeeInfo>(entity, k => k.PayeeInfoID, v => v.Account_name, cmbPayeeInfoID, queryFilterPayeeInfo.GetFilterExpression<tb_FM_PayeeInfo>(), true);
                    DataBindingHelper.InitFilterForControlByExpCanEdit<tb_FM_PayeeInfo>(entity, cmbPayeeInfoID, c => c.Account_name, queryFilterPayeeInfo, true);


                    #endregion
                }
                else
                {
                    //清空
                    cmbPayeeInfoID.DataBindings.Clear();
                }


                #region 应收单 不用显示 收款信息 ，付款时才要显示对方的信息。

                if (PaymentType == ReceivePaymentType.收款)
                {
                    lblAccount_type.Visible = false;
                    cmbAccount_type.Visible = false;
                    btnInfo.Visible = false;
                    lblPayeeInfoID.Visible = false;
                    cmbPayeeInfoID.Visible = false;
                    lblPayeeAccountNo.Visible = false;
                    txtPayeeAccountNo.Visible = false;
                }



                #endregion

                //如果状态是已经生效才可能有审核，如果是待收款 才可能有反审
                if (entity.ARAPStatus == (int)ARAPStatus.待审核)
                {
                    base.toolStripbtnReview.Visible = true;
                }
                else
                {
                    base.toolStripbtnReview.Visible = false;
                }

                if (entity.ARAPStatus == (int)ARAPStatus.待支付)
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
                entity.ARAPStatus = (int)ARAPStatus.草稿;
                entity.ReceivePaymentType = (int)PaymentType;
                entity.ActionStatus = ActionStatus.新增;

                //到期日期应该是根据对应客户的账期的天数来算

                //entity.DueDate = System.DateTime.Now;
                if (string.IsNullOrEmpty(entity.ARAPNo))
                {
                    if (PaymentType == ReceivePaymentType.收款)
                    {
                        entity.ARAPNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.应收款单);
                    }
                    else
                    {
                        entity.ARAPNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.应付款单);
                    }
                }

                //entity.InvoiceDate = System.DateTime.Now;


                // 清空 DataSource（如果适用）
                cmbPayeeInfoID.DataSource = null;
                cmbPayeeInfoID.DataBindings.Clear();
                cmbPayeeInfoID.Items.Clear();
                cmbAccount_type.DataSource = null;
                cmbAccount_type.Items.Clear();
                cmbAccount_type.DataBindings.Clear();
                txtPayeeAccountNo.Text = "";
            }

            DataBindingHelper.BindData4Cmb<tb_FM_Account>(entity, k => k.Account_id, v => v.Account_name, cmbAccount_id);
            DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.ARAPNo, txtARAPNo, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.ExchangeRate.ToString(), txtExchangeRate, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.TotalForeignPayableAmount.ToString(), txtTotalForeignPayableAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.TotalLocalPayableAmount.ToString(), txtTotalLocalPayableAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.ForeignPaidAmount.ToString(), txtForeignPaidAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.LocalPaidAmount.ToString(), txtLocalPaidAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.ForeignBalanceAmount.ToString(), txtForeignBalanceAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.LocalBalanceAmount.ToString(), txtLocalBalanceAmount, BindDataType4TextBox.Money, false);
            
            // DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.SourceBillId, txtSourceBillId, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.SourceBillNo, txtSourceBillNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CmbByEnum<tb_FM_ReceivablePayable, BizType>(entity, k => k.SourceBizType, cmbBizType, false);
            DataBindingHelper.BindData4DataTime<tb_FM_ReceivablePayable>(entity, t => t.DueDate, dtpDueDate, false);
            DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);
            DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v => v.ProjectGroupName, cmbProjectGroup_ID);

            DataBindingHelper.BindData4CheckBox<tb_FM_ReceivablePayable>(entity, t => t.IsIncludeTax, chkIsIncludeTax, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.TaxTotalAmount.ToString(), txtTaxTotalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.UntaxedTotalAmont.ToString(), txtUntaxedTotalAmont, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v => v.CurrencyName, cmbCurrency_ID);

            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            cmbCurrency_ID.SelectedIndex = 1;//默认第一个人民币
            DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.Remark, txtRemark, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4CheckBox<tb_FM_ReceivablePayable>(entity, t => t.ApprovalResults, chkApprovalResults, false);
            DataBindingHelper.BindData4Cmb<tb_FM_PayeeInfo>(entity, k => k.PayeeInfoID, v => v.Account_name, cmbPayeeInfoID, c => c.CustomerVendor_ID.HasValue && c.CustomerVendor_ID.Value == entity.CustomerVendor_ID);
            DataBindingHelper.BindData4ControlByEnum<tb_FM_ReceivablePayable>(entity, t => t.ARAPStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(ARAPStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_FM_ReceivablePayable>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));
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
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_FM_ReceivablePayableValidator>(), kryptonPanel1.Controls);
                //UIBaseTool uIBaseTool = new();
                //uIBaseTool.CurMenuInfo = CurMenuInfo;
                //uIBaseTool.AddEditableQueryControl<tb_Employee>(cmbEmployee_ID, false);

                #region 收款信息 ，并且可以添加

                //创建表达式
                var lambdaPayeeInfo = Expressionable.Create<tb_FM_PayeeInfo>()
                                .And(t => t.Is_enabled == true)
                                .And(t => t.Employee_ID == AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了只能处理自己 的收款信息
                                .ToExpression();//注意 这一句 不能少

                BaseProcessor baseProcessorPayeeInfo = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_FM_PayeeInfo).Name + "Processor");
                QueryFilter queryFilterPayeeInfo = baseProcessorPayeeInfo.GetQueryFilter();
                queryFilterPayeeInfo.FilterLimitExpressions.Add(lambdaPayeeInfo);

                DataBindingHelper.InitFilterForControlByExpCanEdit<tb_FM_PayeeInfo>(entity, cmbPayeeInfoID, c => c.Account_name, queryFilterPayeeInfo, true);

                #endregion


            }

            if (entity.tb_FM_ReceivablePayableDetails != null && entity.tb_FM_ReceivablePayableDetails.Count > 0)
            {
                //新建和草稿时子表编辑也可以保存。
                foreach (var item in entity.tb_FM_ReceivablePayableDetails)
                {
                    item.PropertyChanged += (sender, s1) =>
                    {
                        //权限允许
                        if ((true && entity.ARAPStatus == (int)ARAPStatus.草稿) ||
                        (true && entity.ARAPStatus == (int)ARAPStatus.待审核))
                        {
                            EditEntity.ActionStatus = ActionStatus.修改;
                        }
                    };
                }
                sgh.LoadItemDataToGrid<tb_FM_ReceivablePayableDetail>(grid1, sgd, entity.tb_FM_ReceivablePayableDetails, c => c.ProdDetailID);
                // 模拟按下 Tab 键
                SendKeys.Send("{TAB}");//为了显示远程图片列
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_FM_ReceivablePayableDetail>(grid1, sgd, new List<tb_FM_ReceivablePayableDetail>(), c => c.ProdDetailID);
            }

            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {
                //权限允许
                if ((true && entity.ARAPStatus == (int)ARAPStatus.草稿)
                || (true && entity.ARAPStatus == (int)ARAPStatus.待审核))
                {
                    EditEntity.ActionStatus = ActionStatus.修改;
                }

                //到期日期应该是根据对应客户的账期的天数来算
                if (entity.CustomerVendor_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_FM_ReceivablePayable>(c => c.CustomerVendor_ID))
                {
                    var obj = BizCacheHelper.Instance.GetEntity<tb_CustomerVendor>(entity.CustomerVendor_ID);
                    if (obj != null && obj.ToString() != "System.Object")
                    {
                        if (obj is tb_CustomerVendor cv)
                        {
                            // entity.DueDate = System.DateTime.Now.AddDays(cv.CreditDays);
                        }
                    }
                    //entity.DueDate = System.DateTime.Now;
                }
                if (entity.Currency_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_FM_ReceivablePayable>(c => c.Currency_ID))
                {
                    //如果币别是本位币则不显示汇率列
                    if (EditEntity != null && EditEntity.Currency_ID == MainForm.Instance.AppContext.BaseCurrency.Currency_ID)
                    {
                        // listCols.SetCol_NeverVisible<tb_FM_ReceivablePayableDetail>(c => c.ExchangeRate);
                        //隐藏外币相关
                        UIHelper.ControlForeignFieldInvisible<tb_FM_ReceivablePayable>(this, false);
                        if (listCols != null)
                        {
                            listCols.SetCol_DefaultHide<tb_FM_ReceivablePayableDetail>(c => c.ExchangeRate);
                        }
                    }
                    else
                    {
                        //显示外币相关
                        UIHelper.ControlForeignFieldInvisible<tb_FM_ReceivablePayable>(this, true);
                        //需要有一个方法。通过外币代码得到换人民币的汇率
                        // entity.ExchangeRate = BizService.GetExchangeRateFromCache(cv.Currency_ID, AppContext.BaseCurrency.Currency_ID);


                    }


                }

                base.ToolBarEnabledControl(entity);

            };

            //显示 打印状态 如果是草稿状态 不显示打印
            if ((ARAPStatus)EditEntity.ARAPStatus != ARAPStatus.草稿)
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
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_FM_ReceivablePayable).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();

            //创建表达式
            var lambda = Expressionable.Create<tb_FM_ReceivablePayable>()
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
            AddExtendButton(CurMenuInfo);
            #region
            switch (PaymentType)
            {
                case ReceivePaymentType.收款:
                    lblBillText.Text = "应收款单";
                    lblAccount_id.Text = "收款账号";
                    lblCustomerVendor_ID.Text = "应付单位";
                    lblAccount_type.Visible = false;
                    cmbAccount_type.Visible = false;
                    btnInfo.Visible = false;
                    lblPayeeInfoID.Visible = false;
                    cmbPayeeInfoID.Visible = false;
                    lblPayeeAccountNo.Visible = false;
                    txtPayeeAccountNo.Visible = false;
                    break;
                case ReceivePaymentType.付款:
                    lblBillText.Text = "应付款单";
                    lblAccount_id.Text = "付款账号";
                    lblCustomerVendor_ID.Text = "应收单位";
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
            listCols = sgh.GetGridColumns<ProductSharePart, tb_FM_ReceivablePayableDetail>(c => c.ProdDetailID, false);


            listCols.SetCol_NeverVisible<tb_FM_ReceivablePayableDetail>(c => c.ARAPDetailID);
            listCols.SetCol_NeverVisible<tb_FM_ReceivablePayableDetail>(c => c.ProdDetailID);
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
            //listCols.SetCol_DefaultValue<tb_FM_ReceivablePayableDetail>(c => c.ForeignPayableAmount, 0.00M);

            //listCols.SetCol_DisplayFormatText<tb_FM_ReceivablePayableDetail>(c => c.SourceBizType, 1);

            listCols.SetCol_Format<tb_FM_ReceivablePayableDetail>(c => c.TaxRate, CustomFormatType.PercentFormat);
            listCols.SetCol_Format<tb_FM_ReceivablePayableDetail>(c => c.LocalPayableAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_FM_ReceivablePayableDetail>(c => c.TaxLocalAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_FM_ReceivablePayableDetail>(c => c.UnitPrice, CustomFormatType.CurrencyFormat);
            //listCols.SetCol_Format<tb_FM_ReceivablePayableDetail>(c => c.ForeignPayableAmount, CustomFormatType.CurrencyFormat);

            sgd = new SourceGridDefine(grid1, listCols, true);
            listCols.SetCol_Formula<tb_FM_ReceivablePayableDetail>((a, b) => a.UnitPrice * b.Quantity, c => c.LocalPayableAmount);//-->成交价是结果列

            sgd.GridMasterData = EditEntity;
            /*
            //具体审核权限的人才显示
            if (!AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {
                //listCols.SetCol_NeverVisible<tb_PurEntryDetail>(c => c.UnitPrice);
                //listCols.SetCol_NeverVisible<tb_PurEntryDetail>(c => c.TransactionPrice);
                //listCols.SetCol_NeverVisible<tb_PurEntryDetail>(c => c.SubtotalPirceAmount);
            }*/


            //listCols.SetCol_NeverVisible<tb_FM_ReceivablePayableDetail>(c => c.EvidenceImage);//后面会删除这一列
            //listCols.SetCol_Summary<tb_FM_ReceivablePayableDetail>(c => c.ForeignPayableAmount);
            listCols.SetCol_Summary<tb_FM_ReceivablePayableDetail>(c => c.LocalPayableAmount);
            listCols.SetCol_Summary<tb_FM_ReceivablePayableDetail>(c => c.TaxLocalAmount);

            listCols.SetCol_Formula<tb_FM_ReceivablePayableDetail>((a, b, c) => a.TaxLocalAmount / (1 + b.TaxRate) * c.TaxRate, d => d.TaxLocalAmount);
            //listCols.SetCol_Formula<tb_FM_ReceivablePayableDetail>((a, b) => a.TotalAmount - b.TaxAmount, c => cLocalPayableAmount);

            ////反算成交单价，目标列能重复添加。已经优化好了。
            //listCols.SetCol_Formula<tb_FM_ReceivablePayableDetail>((a, b) => a.SubtotalAmount / b.Quantity, c => c.TransactionPrice);//-->成交价是结果列
            ////反算折扣
            //listCols.SetCol_Formula<tb_FM_ReceivablePayableDetail>((a, b) => a.TransactionPrice / b.UnitPrice, c => c.Discount);
            //listCols.SetCol_Formula<tb_FM_ReceivablePayableDetail>((a, b) => a.TransactionPrice / b.Discount, c => c.UnitPrice);


            //公共到明细的映射 源 ，左边会隐藏
            sgh.SetPointToColumnPairs<ProductSharePart, tb_FM_ReceivablePayableDetail>(sgd, f => f.Specifications, t => t.Specifications);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_FM_ReceivablePayableDetail>(sgd, f => f.Unit_ID, t => t.Unit_ID);
            //sgh.SetPointToColumnPairs<ProductSharePart, tb_FM_ReceivablePayableDetail>(sgd, f => f.Standard_Price, t => t.UnitPrice);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_FM_ReceivablePayableDetail>(sgd, f => f.prop, t => t.property);

            //应该只提供一个结构
            List<tb_FM_ReceivablePayableDetail> lines = new List<tb_FM_ReceivablePayableDetail>();
            bindingSourceSub.DataSource = lines; //  ctrSub.Query(" 1>2 ");
            sgd.BindingSourceLines = bindingSourceSub;

            list = MainForm.Instance.list;
            sgd.SetDependencyObject<ProductSharePart, tb_FM_ReceivablePayableDetail>(list);

            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_FM_ReceivablePayableDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnAddDataRow += Sgh_OnAddDataRow;
            UIHelper.ControlMasterColumnsInvisible(CurMenuInfo, this);


            //隐藏外币相关
            UIHelper.ControlForeignFieldInvisible<tb_FM_ReceivablePayable>(this, false);
            if (listCols != null)
            {
                listCols.SetCol_DefaultHide<tb_FM_ReceivablePayableDetail>(c => c.ExchangeRate);
            }

        }
        #region 坏账处理
        ToolStripButton toolStripButton坏账处理 = new System.Windows.Forms.ToolStripButton();


        /// <summary>
        /// 添加回收
        /// </summary>
        /// <returns></returns>
        public ToolStripItem[] AddExtendButton(tb_MenuInfo menuInfo)
        {
            toolStripButton坏账处理.Text = "坏账处理";
            toolStripButton坏账处理.Image = global::RUINORERP.UI.Properties.Resources.Assignment;
            toolStripButton坏账处理.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton坏账处理.Name = "坏账处理";
            toolStripButton坏账处理.Visible = false;//默认隐藏
            UIHelper.ControlButton<ToolStripButton>(CurMenuInfo, toolStripButton坏账处理);
            toolStripButton坏账处理.ToolTipText = "无法完成支付的账款，需标记为坏账时，使用本功能。";
            toolStripButton坏账处理.Click += new System.EventHandler(this.toolStripButton坏账处理_Click);

            System.Windows.Forms.ToolStripItem[] extendButtons = new System.Windows.Forms.ToolStripItem[]
            { toolStripButton坏账处理};

            this.BaseToolStrip.Items.AddRange(extendButtons);
            return extendButtons;
        }



        private async void toolStripButton坏账处理_Click(object sender, EventArgs e)
        {
            if (EditEntity == null)
            {
                return;
            }
            if (!FMPaymentStatusHelper.CanWriteOffBadDebt((ARAPStatus)EditEntity.ARAPStatus))
            {
                MessageBox.Show($"当前单据,状态为【{(ARAPStatus)EditEntity.ARAPStatus}】不允许标记为坏账。");
                return;
            }

            BillConverterFactory bcf = Startup.GetFromFac<BillConverterFactory>();
            CommonUI.frmGenericOpinion<tb_FM_ReceivablePayable> frm = new();
            frm.FormTitle = "坏账处理";
            frm.OpinionLabelText = "坏账原因：";
            frm.BindData(
                 EditEntity,
                 e => e.ARAPNo,
                 e => e.ARAPNo,
                 e => e.Remark
            );
            if (frm.ShowDialog() == DialogResult.OK)//审核了。不管是同意还是不同意
            {
                //已经审核的,未完结的才能标记坏账
                var ctr = Startup.GetFromFac<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
                ReturnResults<tb_FM_ReceivablePayable> rs = await ctr.WriteOffBadDebt(EditEntity, frm.txtOpinion.Text);
                if (rs.Succeeded)
                {
                    //if (MainForm.Instance.WorkflowItemlist.ContainsKey(""))
                    //{

                    //}
                    //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                    //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                    //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                    //MainForm.Instance.ecs.AddSendData(od);
                    MainForm.Instance.AuditLogHelper.CreateAuditLog<tb_FM_ReceivablePayable>("坏账处理", EditEntity, $"原因:{EditEntity.Remark}");
                    Refreshs();
                }
                else
                {
                    MainForm.Instance.PrintInfoLog($"{EditEntity.ARAPNo}坏账处理操作失败,原因是{rs.ErrorMsg},如果无法解决，请联系管理员！", Color.Red);
                }
            }
            else
            {

            }


        }




        #endregion
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
                List<tb_FM_ReceivablePayableDetail> details = sgd.BindingSourceLines.DataSource as List<tb_FM_ReceivablePayableDetail>;
                details = details.Where(c => c.LocalPayableAmount != 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("金额必须大于0");
                    return;
                }
                //EditEntity.TotalForeignPayableAmount = details.Sum(c => c.ForeignPayableAmount);
                EditEntity.TotalLocalPayableAmount = details.Sum(c => c.LocalPayableAmount);
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
            foreach (tb_FM_ReceivablePayableDetail detail in EditEntity.tb_FM_ReceivablePayableDetails)
            {
                PropertyInfo[] props = typeof(tb_FM_ReceivablePayableDetail).GetProperties();
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

        List<tb_FM_ReceivablePayableDetail> details = new List<tb_FM_ReceivablePayableDetail>();
        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }

            var eer = errorProviderForAllInput.GetError(txtLocalBalanceAmount);
            bindingSourceSub.EndEdit();
            List<tb_FM_ReceivablePayableDetail> detailentity = bindingSourceSub.DataSource as List<tb_FM_ReceivablePayableDetail>;
            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.LocalPayableAmount != 0).ToList();
                //如果没有有效的明细。直接提示
                if (NeedValidated && details.Count == 0)
                {
                    MessageBox.Show("请录入有效明细记录！");
                    return false;
                }

                EditEntity.tb_FM_ReceivablePayableDetails = details;


                //收付款单中的  收款或付款账号中的币别是否与选的币别一致。
                if (NeedValidated && EditEntity.Currency_ID > 0 && EditEntity.Account_id > 0)
                {
                    tb_FM_Account bizcatch = BizCacheHelper.Instance.GetEntity<tb_FM_Account>(EditEntity.Account_id);
                    if (bizcatch != null && bizcatch.Currency_ID != EditEntity.Currency_ID)
                    {
                        MessageBox.Show("收付款账号中的币别与当前单据的币别不一致。");
                        return false;
                    }
                }


                //如果主表的总金额和明细金额加总后不相等，则提示
                if (NeedValidated && EditEntity.TotalLocalPayableAmount != details.Sum(c => c.LocalPayableAmount))
                {
                    if (MessageBox.Show("总金额和明细金额总计不相等，你确定要保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.No)
                    {
                        return false;
                    }
                }
                if (NeedValidated && EditEntity.UntaxedTotalAmont != details.Sum(c => c.TaxLocalAmount))
                {
                    if (MessageBox.Show("未税总金额和明细未税金额总计不相等，你确定要保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.No)
                    {
                        return false;
                    }
                }

                if (NeedValidated && EditEntity.TotalLocalPayableAmount != details.Sum(c => c.LocalPayableAmount))
                {
                    if (MessageBox.Show("核准总金额和明细金额总计不相等，你确定要保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.No)
                    {
                        return false;
                    }
                }

                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_FM_ReceivablePayableDetail>(details))
                {
                    return false;
                }
                if (NeedValidated)
                {//处理图片
                    bool uploadImg = await base.SaveFileToServer(sgd, EditEntity.tb_FM_ReceivablePayableDetails);
                    if (uploadImg)
                    {
                        ////更新图片名后保存到数据库
                        //int ImgCounter = await MainForm.Instance.AppContext.Db.Updateable<tb_FM_ReceivablePayableDetail>(EditEntity.tb_FM_ReceivablePayableDetails)
                        //    .UpdateColumns(t => new { t.EvidenceImagePath })
                        //    .ExecuteCommandAsync();
                        //if (ImgCounter > 0)
                        //{
                        MainForm.Instance.PrintInfoLog($"图片保存成功,。");
                        //}
                    }
                    else
                    {
                        MainForm.Instance.uclog.AddLog("图片上传出错。");
                        return false;
                    }
                }

                ReturnMainSubResults<tb_FM_ReceivablePayable> SaveResult = new ReturnMainSubResults<tb_FM_ReceivablePayable>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {

                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.ARAPNo}。");
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
            bool result = await Submit(ARAPStatus.待审核);
            if (result)
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



        protected async override Task<ReturnResults<tb_FM_ReceivablePayable>> Delete()
        {
            ReturnResults<tb_FM_ReceivablePayable> rss = new ReturnResults<tb_FM_ReceivablePayable>();
            if (EditEntity == null)
            {
                //提示一下删除成功
                MainForm.Instance.uclog.AddLog("提示", "没有要删除的数据");
                return rss;
            }

            if (MessageBox.Show("系统不建议删除单据资料\r\n确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                //https://www.runoob.com/w3cnote/csharp-enum.html
                var dataStatus = (ARAPStatus)(EditEntity.GetPropertyValue(typeof(ARAPStatus).Name).ToLong());
                if (dataStatus == ARAPStatus.待审核 || dataStatus == ARAPStatus.草稿)
                {
                    //如果草稿。都可以删除。如果是新建，则提交过了。要创建人或超级管理员才能删除
                    if (dataStatus == ARAPStatus.待审核 && !AppContext.IsSuperUser)
                    {
                        if (EditEntity.Created_by.Value != AppContext.CurUserInfo.Id)
                        {
                            MessageBox.Show("只有创建人才能删除提交的单据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            rss.ErrorMsg = "只有创建人才能删除提交的单据。";
                            rss.Succeeded = false;
                            return rss;
                        }
                    }

                    tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable> ctr = Startup.GetFromFac<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
                    bool rs = await ctr.BaseLogicDeleteAsync(EditEntity as tb_FM_ReceivablePayable);
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
                        //base.OnBindDataToUIEvent(EditEntity as tb_FM_ReceivablePayable, ActionStatus.删除);
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

        private void btnInfo_Click(object sender, EventArgs e)
        {
            if (sender is KryptonButton btninfo)
            {
                if (btninfo.Tag != null)
                {
                    if (!string.IsNullOrWhiteSpace(btninfo.Tag.ToString()))
                    {
                        //tb_FM_PayeeInfo payeeInfo = btninfo.Tag as tb_FM_PayeeInfo;

                        #region 显示收款详情信息

                        object frm = Activator.CreateInstance(typeof(UCFMPayeeInfoEdit));
                        if (frm.GetType().BaseType.Name.Contains("BaseEditGeneric"))
                        {
                            BaseEditGeneric<tb_FM_PayeeInfo> frmaddg = frm as BaseEditGeneric<tb_FM_PayeeInfo>;
                            frmaddg.CurMenuInfo = this.CurMenuInfo;
                            frmaddg.Text = "收款账号详情";
                            frmaddg.bindingSourceEdit.DataSource = new List<tb_FM_PayeeInfo>();
                            object obj = frmaddg.bindingSourceEdit.AddNew();
                            obj = btninfo.Tag;
                            tb_FM_PayeeInfo payeeInfo = obj as tb_FM_PayeeInfo;
                            BaseEntity bty = payeeInfo as BaseEntity;
                            bty.ActionStatus = ActionStatus.加载;
                            frmaddg.BindData(bty);
                            if (frmaddg.ShowDialog() == DialogResult.OK)
                            {

                            }
                        }
                        #endregion
                        //HttpWebService httpWebService = Startup.GetFromFac<HttpWebService>();
                        //try
                        //{
                        //    byte[] img = await httpWebService.DownloadImgFileAsync(btninfo.Tag.ToString());
                        //    frmPictureViewer pictureViewer = new frmPictureViewer();
                        //    pictureViewer.PictureBoxViewer.Image = UI.Common.ImageHelper.byteArrayToImage(img);
                        //    pictureViewer.ShowDialog();
                        //}
                        //catch (Exception ex)
                        //{
                        //    MainForm.Instance.uclog.AddLog(ex.Message, Global.UILogType.错误);
                        //}
                    }

                }
            }
        }
    }
}