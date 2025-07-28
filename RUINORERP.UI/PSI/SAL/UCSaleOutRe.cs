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
using FluentValidation.Results;
using System.Linq.Expressions;
using Krypton.Toolkit;
using AutoMapper;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using SourceGrid;
using EnumsNET;
using RUINORERP.UI.PSI.PUR;
using RUINORERP.Business.CommService;
using RUINORERP.Global.EnumExt;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.UI.SysConfig;
using RUINORERP.Model.CommonModel;
using RUINORERP.Common.Extensions;

namespace RUINORERP.UI.PSI.SAL
{
    [MenuAttrAssemblyInfo("销售退回单", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.销售管理, BizType.销售退回单)]
    public partial class UCSaleOutRe : BaseBillEditGeneric<tb_SaleOutRe, tb_SaleOutReDetail>, IPublicEntityObject
    {
        public UCSaleOutRe()
        {
            InitializeComponent();
            //InitDataToCmbByEnumDynamicGeneratedDataSource<tb_SaleOutRe>(typeof(Priority), e => e.OrderPriority, cmbOrderPriority);
            AddPublicEntityObject(typeof(ProductSharePart));
        }

        #region 平台退款动作

        ToolStripButton toolStripButton平台退款 = new System.Windows.Forms.ToolStripButton();

        /// <summary>
        /// 添加回收
        /// </summary>
        /// <returns></returns>
        public override ToolStripItem[] AddExtendButton(tb_MenuInfo menuInfo)
        {

            toolStripButton平台退款.Text = "平台退款";
            toolStripButton平台退款.Image = global::RUINORERP.UI.Properties.Resources.Assignment;
            toolStripButton平台退款.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton平台退款.Name = "平台退款";
            toolStripButton平台退款.Visible = false;//默认隐藏
            UIHelper.ControlButton<ToolStripButton>(CurMenuInfo, toolStripButton平台退款);
            toolStripButton平台退款.ToolTipText = "平台订单退款时，根据实际情况确认销售退回单的退货退款状态。";
            toolStripButton平台退款.Click += new System.EventHandler(this.toolStripButton平台退款_Click);

            System.Windows.Forms.ToolStripItem[] extendButtons = new System.Windows.Forms.ToolStripItem[]
            { toolStripButton平台退款};

            this.BaseToolStrip.Items.AddRange(extendButtons);
            return extendButtons;
        }

        private async void toolStripButton平台退款_Click(object sender, EventArgs e)
        {
            if (EditEntity == null)
            {
                return;
            }

            if (EditEntity != null)
            {
                tb_SaleOutRe saleOutRe = EditEntity as tb_SaleOutRe;
                //只有审核状态才可以转换
                if (EditEntity.DataStatus >= (int)DataStatus.确认 && EditEntity.ApprovalStatus == (int)ApprovalStatus.已审核 && EditEntity.ApprovalResults.HasValue && EditEntity.ApprovalResults.Value)
                {
                    //判断是否为平台订单
                    if (!saleOutRe.IsFromPlatform || saleOutRe.PlatformOrderNo.IsNullOrEmpty())
                    {
                        MessageBox.Show($"当前【销售出库单】对应的销售退回单为不是平台订单，无法进行平台退款", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        toolStripButton平台退款.Enabled = false;
                        return;
                    }
                    if (!EditEntity.RefundStatus.HasValue)
                    {
                        System.Windows.Forms.MessageBox.Show("退货退款状态不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (saleOutRe.RefundStatus.HasValue && saleOutRe.RefundStatus.Value >= (int)RefundStatus.已退款等待退货)
                    {
                        // 更新出库单状态
                        var last = await MainForm.Instance.AppContext.Db.Updateable<tb_SaleOutRe>(saleOutRe).UpdateColumns(it => new
                        {
                            it.RefundStatus
                        }).ExecuteCommandAsync();
                        toolStripButton平台退款.Enabled = false;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show($"平台退款后，退货退款状态不能为{(RefundStatus)saleOutRe.RefundStatus}，请重试", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        toolStripButton平台退款.Enabled = true;
                        return;
                    }

                }
                else
                {
                    MessageBox.Show($"当前【销售出库单】的状态为{(DataStatus)EditEntity.DataStatus}，无法进行【平台退款】", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }


        #endregion


        public override void QueryConditionBuilder()
        {
            base.QueryConditionBuilder();
            var lambda = Expressionable.Create<tb_SaleOutRe>()
            .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
            .ToExpression();
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);

        }


        internal override void LoadDataToUI(object Entity)
        {
            BindData(Entity as tb_SaleOutRe);
        }


        protected override void LoadRelatedDataToDropDownItems()
        {
            if (base.EditEntity is tb_SaleOutRe saleOutRe)
            {
                if (saleOutRe.SaleOut_MainID.HasValue)
                {
                    RelatedQueryParameter rqp = new RelatedQueryParameter();
                    rqp.bizType = BizType.销售出库单;
                    rqp.billId = saleOutRe.SaleOut_MainID.Value;
                    rqp.billNo = saleOutRe.SaleOut_NO;
                    ToolStripMenuItem RelatedMenuItem = new ToolStripMenuItem();
                    RelatedMenuItem.Name = $"{rqp.billId}";
                    RelatedMenuItem.Tag = rqp;
                    RelatedMenuItem.Text = $"{rqp.bizType}:{rqp.billNo}";
                    RelatedMenuItem.Click += base.MenuItem_Click;
                    if (!toolStripbtnRelatedQuery.DropDownItems.ContainsKey(rqp.billId.ToString()))
                    {
                        toolStripbtnRelatedQuery.DropDownItems.Add(RelatedMenuItem);
                    }
                }
            }
            base.LoadRelatedDataToDropDownItems();
        }
        public override void BindData(tb_SaleOutRe entity, ActionStatus actionStatus = ActionStatus.无操作)
        {
            if (entity == null)
            {

                return;
            }
            cmbPayStatus.Enabled = true;
            cmbPaytype_ID.Enabled = true;
            base.EditEntity = entity;
            if (entity != null)
            {
                if (entity.SaleOutRe_ID > 0)
                {
                    entity.PrimaryKeyID = entity.SaleOutRe_ID;
                    entity.ActionStatus = ActionStatus.加载;
                    //entity.DataStatus = (int)DataStatus.确认;
                    //如果审核了，审核要灰色
                }
                else
                {
                    entity.ActionStatus = ActionStatus.新增;
                    entity.DataStatus = (int)DataStatus.草稿;
                    if (string.IsNullOrEmpty(entity.ReturnNo))
                    {
                        entity.ReturnNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.销售退回单);
                    }

                    if (entity.tb_SaleOutReDetails != null && entity.tb_SaleOutReDetails.Count > 0)
                    {
                        entity.tb_SaleOutReDetails.ForEach(c => c.SaleOutRe_ID = 0);
                        entity.tb_SaleOutReDetails.ForEach(c => c.SOutReturnDetail_ID = 0);
                    }
                    if (AppContext.BaseCurrency != null)
                    {
                        entity.Currency_ID = AppContext.BaseCurrency.Currency_ID;
                    }

                }

            }

            if (entity.ApprovalStatus.HasValue)
            {
                lblReview.Text = ((ApprovalStatus)entity.ApprovalStatus).ToString();
            }
            EditEntity = entity;

            DataBindingHelper.BindData4CmbByEnum<tb_SaleOutRe, RefundStatus>(entity, k => k.RefundStatus, cmbRefundStatus, false);

            DataBindingHelper.BindData4Cmb<tb_PaymentMethod>(entity, k => k.Paytype_ID, v => v.Paytype_Name, cmbPaytype_ID);
            DataBindingHelper.BindData4CmbByEnum<tb_SaleOutRe, PayStatus>(entity, k => k.PayStatus, cmbPayStatus, false, PayStatus.全额预付, PayStatus.部分预付);
            DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.TotalCommissionAmount.ToString(), txtTotalCommissionAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v => v.CurrencyName, cmbCurrency_ID);
            DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.ExchangeRate.ToString(), txtExchangeRate, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.ReturnNo, txtReturnNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, c => c.IsCustomer == true);
            DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, v => v.SaleOut_NO, txtSaleOutNo, BindDataType4TextBox.Text, true);
            DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.FreightIncome.ToString(), txtFreightIncome, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.TotalQty, txtTotalQty, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4DataTime<tb_SaleOutRe>(entity, t => t.ReturnDate, dtpDeliveryDate, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.TrackNo, txtTrackNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.ReturnReason, txtReturnReason, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4DataTime<tb_SaleOutRe>(entity, t => t.Approver_at, dtpApprover_at, false);
            DataBindingHelper.BindData4CheckBox<tb_SaleOutRe>(entity, t => t.ApprovalResults, chkApprovalResults, false);
            DataBindingHelper.BindData4CheckBox<tb_SaleOutRe>(entity, t => t.IsIncludeTax, chkIsIncludeTax, false);
            DataBindingHelper.BindData4CheckBox<tb_SaleOutRe>(entity, t => t.RefundOnly, chkRefundOnly, false);
            DataBindingHelper.BindData4CheckBox<tb_SaleOutRe>(entity, t => t.GenerateVouchers, chkGenerateVouchers, false);
            DataBindingHelper.BindData4CheckBox<tb_SaleOutRe>(entity, t => t.IsCustomizedOrder, chkIsCustomizedOrder, false);
            DataBindingHelper.BindData4CheckBox<tb_SaleOutRe>(entity, t => t.IsFromPlatform, chkIsFromPlatform, false);
            DataBindingHelper.BindData4CheckBox<tb_SaleOutRe>(entity, t => t.OfflineRefund, chkOfflineRefund, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.PlatformOrderNo, txtPlatformOrderNo, BindDataType4TextBox.Text, false);


            base.errorProviderForAllInput.DataSource = entity;
            base.errorProviderForAllInput.ContainerControl = this;
            //this.ValidateChildren();
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            DataBindingHelper.BindData4ControlByEnum<tb_SaleOutRe>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_SaleOutRe>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));
            if (entity.tb_SaleOutReDetails != null && entity.tb_SaleOutReDetails.Count > 0)
            {
                //LoadDataToGrid(entity.tb_SaleOutReDetails);
                sgh.LoadItemDataToGrid<tb_SaleOutReDetail>(grid1, sgd, entity.tb_SaleOutReDetails, c => c.ProdDetailID);
            }
            else
            {
                //LoadDataToGrid(new List<tb_SaleOutReDetail>());
                sgh.LoadItemDataToGrid<tb_SaleOutReDetail>(grid1, sgd, new List<tb_SaleOutReDetail>(), c => c.ProdDetailID);
            }

            if (entity.tb_SaleOutReRefurbishedMaterialsDetails != null && entity.tb_SaleOutReRefurbishedMaterialsDetails.Count > 0)
            {
                //LoadDataToGrid(entity.tb_SaleOutReDetails);
                sgh2.LoadItemDataToGrid<tb_SaleOutReRefurbishedMaterialsDetail>(grid2, sgd2, entity.tb_SaleOutReRefurbishedMaterialsDetails, c => c.ProdDetailID);
            }
            else
            {
                //LoadDataToGrid(new List<tb_SaleOutReDetail>());
                sgh2.LoadItemDataToGrid<tb_SaleOutReRefurbishedMaterialsDetail>(grid2, sgd2, new List<tb_SaleOutReRefurbishedMaterialsDetail>(), c => c.ProdDetailID);
            }




            //创建表达式
            var lambda = Expressionable.Create<tb_CustomerVendor>()
                              .And(t => t.IsCustomer == true)
                              .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && !AppContext.IsSuperUser, t => t.Employee_ID == AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户
                            .ToExpression();//注意 这一句 不能少
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CustomerVendor).Name + "Processor");
            QueryFilter queryFilterC = baseProcessor.GetQueryFilter();
            queryFilterC.FilterLimitExpressions.Add(lambda);
            DataBindingHelper.InitFilterForControlByExp<tb_CustomerVendor>(entity, cmbCustomerVendor_ID, c => c.CVName, queryFilterC);


            //先绑定这个。InitFilterForControl 这个才生效
            DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, v => v.SaleOut_NO, txtSaleOutNo, BindDataType4TextBox.Text, true);

            //创建表达式  草稿 结案 和没有提交的都不显示
            var lambdaSO = Expressionable.Create<tb_SaleOut>()
                            .And(t => t.DataStatus == (int)DataStatus.确认)
                             .And(t => t.isdeleted == false)
                             .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && !AppContext.IsSuperUser, t => t.Employee_ID == AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的
                            .ToExpression();//注意 这一句 不能少
            BaseProcessor baseProcessorSO = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_SaleOut).Name + "Processor");
            QueryFilter queryFilterSO = baseProcessorSO.GetQueryFilter();
            queryFilterSO.FilterLimitExpressions.Add(lambdaSO);

            ControlBindingHelper.ConfigureControlFilter<tb_SaleOutRe, tb_SaleOut>(entity, txtSaleOutNo, t => t.SaleOut_NO,
              f => f.SaleOutNo, queryFilterSO, a => a.SaleOut_MainID, b => b.SaleOut_MainID, null, false);

            ToolBarEnabledControl(entity);

            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {
                //权限允许
                if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
                {
                    if (s2.PropertyName == entity.GetPropertyName<tb_SaleOutRe>(c => c.PayStatus) && entity.PayStatus == (int)PayStatus.未付款)
                    {
                        //默认为账期
                        entity.Paytype_ID = MainForm.Instance.AppContext.PaymentMethodOfPeriod.Paytype_ID;
                    }
                    if (s2.PropertyName == entity.GetPropertyName<tb_SaleOutRe>(c => c.Paytype_ID) && entity.Paytype_ID == MainForm.Instance.AppContext.PaymentMethodOfPeriod.Paytype_ID)
                    {
                        //默认为未付款
                        entity.PayStatus = (int)PayStatus.未付款;
                    }
                    if (s2.PropertyName == entity.GetPropertyName<tb_SaleOutRe>(c => c.Paytype_ID) && entity.Paytype_ID > 0)
                    {
                        if (cmbPaytype_ID.SelectedItem is tb_PaymentMethod paymentMethod)
                        {
                            EditEntity.tb_paymentmethod = paymentMethod;
                        }
                    }

                }
                //如果是销售订单引入变化则加载明细及相关数据
                if ((entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改) && entity.SaleOut_MainID.HasValue && entity.SaleOut_MainID.Value > 0 && s2.PropertyName == entity.GetPropertyName<tb_SaleOutRe>(c => c.SaleOut_MainID))
                {
                    LoadSaleOutBillData(entity.SaleOut_MainID);
                }

                if (entity.CustomerVendor_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_SaleOrder>(c => c.CustomerVendor_ID))
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
            base.BindData(entity);
        }

        public void InitDataTocmbbox()
        {
            DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.InitDataToCmb<tb_CustomerVendor>(k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, c => c.IsCustomer == true);
        }

        protected async override Task<ReturnResults<tb_SaleOutRe>> Delete()
        {
            ReturnResults<tb_SaleOutRe> rss = new ReturnResults<tb_SaleOutRe>();
            if (EditEntity == null)
            {
                //提示一下删除成功
                MainForm.Instance.uclog.AddLog("提示", "没有要删除的数据");
                return rss;
            }

            if (MessageBox.Show("系统不建议删除单据资料\r\n确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                //https://www.runoob.com/w3cnote/csharp-enum.html
                var dataStatus = (DataStatus)(EditEntity.GetPropertyValue(typeof(DataStatus).Name).ToInt());
                if (dataStatus == DataStatus.新建 || dataStatus == DataStatus.草稿)
                {
                    //如果草稿。都可以删除。如果是新建，则提交过了。要创建人或超级管理员才能删除
                    if (dataStatus == DataStatus.新建 && !AppContext.IsSuperUser)
                    {
                        if (EditEntity.Created_by.Value != AppContext.CurUserInfo.Id)
                        {
                            MessageBox.Show("只能删除自己创建的销售退回单。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            rss.ErrorMsg = "只能删除自己创建的销售退回单。";
                            rss.Succeeded = false;
                            return rss;
                        }
                    }

                    tb_SaleOutReController<tb_SaleOutRe> ctr = Startup.GetFromFac<tb_SaleOutReController<tb_SaleOutRe>>();
                    bool rs = await ctr.BaseLogicDeleteAsync(EditEntity as tb_SaleOutRe);
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
                        //base.OnBindDataToUIEvent(EditEntity as tb_SaleOutRe, ActionStatus.删除);
                        Exit(this);
                    }
                }
                else
                {
                    //
                    MainForm.Instance.uclog.AddLog("提示", $"单据状态为{dataStatus},无法删除");
                }
            }
            return rss;
        }

        /*
         重要思路提示，这个表格控件，数据源的思路是，
        产品共享表 ProductSharePart+tb_SaleOutReDetail 有合体，SetDependencyObject 根据字段名相同的给值，值来自产品明细表
         并且，表格中可以编辑的需要查询的列是唯一能查到详情的

        SetDependencyObject<P, S, T>   P包含S字段包含T字段--》是有且包含
         */



        SourceGridDefine sgd = null;
        SourceGridDefine sgd2 = null;

        SourceGridHelper sgh = new SourceGridHelper();
        SourceGridHelper sgh2 = new SourceGridHelper();
        //设计关联列和目标列
        View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
        List<View_ProdDetail> list = new List<View_ProdDetail>();
        private void UcSaleOrderEdit_Load(object sender, EventArgs e)
        {
            InitDataTocmbbox();
            base.ToolBarEnabledControl(MenuItemEnums.刷新);
            LoadGrid1();
            LoadGrid2();
            UIHelper.ControlMasterColumnsInvisible(CurMenuInfo, this);
        }

        private void LoadGrid1()
        {
            ///显示列表对应的中文
            //base.FieldNameList = UIHelper.GetFieldNameList<tb_SaleOutReDetail>();

            grid1.BorderStyle = BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;

            List<SGDefineColumnItem> listCols = new List<SGDefineColumnItem>();
            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<ProductSharePart, tb_SaleOutReDetail>(c => c.ProdDetailID, false);

            listCols.SetCol_NeverVisible<tb_SaleOutReDetail>(c => c.SaleOutRe_ID);
            listCols.SetCol_NeverVisible<tb_SaleOutReDetail>(c => c.SOutReturnDetail_ID);
            listCols.SetCol_NeverVisible<tb_SaleOutReDetail>(c => c.ProdDetailID);
            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);
            listCols.SetCol_Format<tb_SaleOutReDetail>(c => c.TaxRate, CustomFormatType.PercentFormat);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Location_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.VendorModelCode);

            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Rack_ID);
            if (!AppContext.SysConfig.UseBarCode)
            {
                listCols.SetCol_NeverVisible<ProductSharePart>(c => c.BarCode);
            }
            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridMasterData = EditEntity;
            /*
            //具体审核权限的人才显示
            if (AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {
                listCols.SetCol_Summary<tb_SaleOutReDetail>(c => c.TotalCostAmount, true);
                listCols.SetCol_NeverVisible<tb_SaleOutReDetail>(c => c.Cost);
                listCols.SetCol_NeverVisible<tb_SaleOutReDetail>(c => c.TotalCostAmount);
            }*/
            listCols.SetCol_Summary<tb_SaleOutReDetail>(c => c.Quantity);
            listCols.SetCol_Summary<tb_SaleOutReDetail>(c => c.SubtotalTransAmount);
            listCols.SetCol_Summary<tb_SaleOutReDetail>(c => c.CommissionAmount);
            listCols.SetCol_Summary<tb_SaleOutReDetail>(c => c.SubtotalCostAmount);
            listCols.SetCol_Summary<tb_SaleOutReDetail>(c => c.SubtotalUntaxedAmount);
            listCols.SetCol_Summary<tb_SaleOutReDetail>(c => c.SubtotalTaxAmount);

            listCols.SetCol_Formula<tb_SaleOutReDetail>((a, b) => (a.Cost + a.CustomizedCost) * b.Quantity, c => c.SubtotalCostAmount);
            listCols.SetCol_Formula<tb_SaleOutReDetail>((a, b) => a.TransactionPrice * b.Quantity, c => c.SubtotalTransAmount);
            listCols.SetCol_Formula<tb_SaleOutReDetail>((a, b) => a.SubtotalTransAmount - b.SubtotalTaxAmount, c => c.SubtotalUntaxedAmount);
            listCols.SetCol_Formula<tb_SaleOutReDetail>((a, b, c) => a.SubtotalTransAmount / (1 + b.TaxRate) * c.TaxRate, d => d.SubtotalTaxAmount);
            listCols.SetCol_Formula<tb_SaleOutReDetail>((a, b) => a.UnitCommissionAmount * b.Quantity, c => c.CommissionAmount);
            listCols.SetCol_FormulaReverse<tb_SaleOutReDetail>(d => d.Quantity != 0, (a, b) => a.CommissionAmount / b.Quantity, c => c.UnitCommissionAmount);


            // listCols.SetCol_Formula<tb_SaleOutReDetail>((a, b, c) => a.TransactionPrice * b.Quantity - c.TaxSubtotalAmount, d => d.UntaxedAmount);

            //应该是审核时要处理的逻辑暂时隐藏
            //将数量默认为已出库数量
            //listCols.SetCol_Formula<tb_SaleOutReDetail>((a, b) => a.Quantity, c => c.TotalReturnedQty);
            //listCols.SetCol_NeverVisible<tb_SaleOutReDetail>(c => c.TotalReturnedQty);

            sgh.SetPointToColumnPairs<ProductSharePart, tb_SaleOutReDetail>(sgd, f => f.Inv_Cost, t => t.Cost);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_SaleOutReDetail>(sgd, f => f.Standard_Price, t => t.TransactionPrice);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_SaleOutReDetail>(sgd, f => f.prop, t => t.property);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_SaleOutReDetail>(sgd, f => f.Location_ID, t => t.Location_ID);



            //应该只提供一个结构
            List<tb_SaleOutReDetail> lines = new List<tb_SaleOutReDetail>();
            bindingSourceSub.DataSource = lines;
            sgd.BindingSourceLines = bindingSourceSub;
            //    Expression<Func<View_ProdDetail, bool>> exp = Expressionable.Create<View_ProdDetail>() //创建表达式
            // .AndIF(true, w => w.CNName.Length > 0)
            //// .AndIF(txtSpecifications.Text.Trim().Length > 0, w => w.Specifications.Contains(txtSpecifications.Text.Trim()))
            //.ToExpression();//注意 这一句 不能少
            //                // StringBuilder sb = new StringBuilder();
            //    /// sb.Append(string.Format("{0}='{1}'", item.ColName, valValue));
            //    list = dc.BaseQueryByWhere(exp);
            list = MainForm.Instance.list;
            sgd.SetDependencyObject<ProductSharePart, tb_SaleOutReDetail>(list);
            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_SaleOutReDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnLoadMultiRowData += Sgh_OnLoadMultiRowData;
        }
        private void LoadGrid2()
        {

            ///显示列表对应的中文

            grid2.BorderStyle = BorderStyle.FixedSingle;
            grid2.Selection.EnableMultiSelection = false;
            List<SGDefineColumnItem> listCols = new List<SGDefineColumnItem>();
            //指定了关键字段ProdDetailID
            listCols = sgh2.GetGridColumns<ProductSharePart, tb_SaleOutReRefurbishedMaterialsDetail>(c => c.ProdDetailID, false);

            listCols.SetCol_NeverVisible<tb_SaleOutReRefurbishedMaterialsDetail>(c => c.SaleOutRe_ID);
            listCols.SetCol_NeverVisible<tb_SaleOutReRefurbishedMaterialsDetail>(c => c.SOutReturnDetail_ID);
            listCols.SetCol_NeverVisible<tb_SaleOutReRefurbishedMaterialsDetail>(c => c.ProdDetailID);
            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Location_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Rack_ID);
            if (!AppContext.SysConfig.UseBarCode)
            {
                listCols.SetCol_NeverVisible<ProductSharePart>(c => c.BarCode);
            }
            sgd2 = new SourceGridDefine(grid2, listCols, true);

            /*
            //具体审核权限的人才显示
            if (AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {
                listCols.SetCol_Summary<tb_SaleOutReDetail>(c => c.TotalCostAmount, true);
                listCols.SetCol_NeverVisible<tb_SaleOutReDetail>(c => c.Cost);
                listCols.SetCol_NeverVisible<tb_SaleOutReDetail>(c => c.TotalCostAmount);
            }*/
            listCols.SetCol_Summary<tb_SaleOutReRefurbishedMaterialsDetail>(c => c.Quantity);
            listCols.SetCol_Summary<tb_SaleOutReRefurbishedMaterialsDetail>(c => c.SubtotalTransAmount);



            listCols.SetCol_Formula<tb_SaleOutReRefurbishedMaterialsDetail>((a, b) => a.Cost * b.Quantity, c => c.SubtotalCostAmount);
            listCols.SetCol_Formula<tb_SaleOutReRefurbishedMaterialsDetail>((a, b) => a.TransactionPrice * b.Quantity, c => c.SubtotalTransAmount);
            listCols.SetCol_Formula<tb_SaleOutReRefurbishedMaterialsDetail>((a, b) => a.SubtotalTransAmount - b.SubtotalTaxAmount, c => c.SubtotalUntaxedAmount);
            listCols.SetCol_Formula<tb_SaleOutReRefurbishedMaterialsDetail>((a, b, c) => a.SubtotalTransAmount / (1 + b.TaxRate) * c.TaxRate, d => d.SubtotalTaxAmount);
            // listCols.SetCol_Formula<tb_SaleOutReDetail>((a, b, c) => a.TransactionPrice * b.Quantity - c.TaxSubtotalAmount, d => d.UntaxedAmount);

            //应该是审核时要处理的逻辑暂时隐藏
            //将数量默认为已出库数量
            //listCols.SetCol_Formula<tb_SaleOutReDetail>((a, b) => a.Quantity, c => c.TotalReturnedQty);
            //listCols.SetCol_NeverVisible<tb_SaleOutReDetail>(c => c.TotalReturnedQty);

            sgh2.SetPointToColumnPairs<ProductSharePart, tb_SaleOutReRefurbishedMaterialsDetail>(sgd2, f => f.Inv_Cost, t => t.Cost);
            sgh2.SetPointToColumnPairs<ProductSharePart, tb_SaleOutReRefurbishedMaterialsDetail>(sgd2, f => f.Standard_Price, t => t.TransactionPrice);
            sgh2.SetPointToColumnPairs<ProductSharePart, tb_SaleOutReRefurbishedMaterialsDetail>(sgd2, f => f.prop, t => t.property);
            sgh2.SetPointToColumnPairs<ProductSharePart, tb_SaleOutReRefurbishedMaterialsDetail>(sgd2, f => f.Location_ID, t => t.Location_ID);



            //应该只提供一个结构
            List<tb_SaleOutReRefurbishedMaterialsDetail> lines = new List<tb_SaleOutReRefurbishedMaterialsDetail>();
            bindingSourceOtherSub.DataSource = lines;
            sgd2.BindingSourceLines = bindingSourceOtherSub;
            //    Expression<Func<View_ProdDetail, bool>> exp = Expressionable.Create<View_ProdDetail>() //创建表达式
            // .AndIF(true, w => w.CNName.Length > 0)
            //// .AndIF(txtSpecifications.Text.Trim().Length > 0, w => w.Specifications.Contains(txtSpecifications.Text.Trim()))
            //.ToExpression();//注意 这一句 不能少
            //                // StringBuilder sb = new StringBuilder();
            //    /// sb.Append(string.Format("{0}='{1}'", item.ColName, valValue));
            //    list = dc.BaseQueryByWhere(exp);
            list = MainForm.Instance.list;
            sgd2.SetDependencyObject<ProductSharePart, tb_SaleOutReRefurbishedMaterialsDetail>(list);
            sgd2.HasRowHeader = true;
            sgh2.InitGrid(grid2, sgd2, true, nameof(tb_SaleOutReRefurbishedMaterialsDetail));
            sgh2.OnLoadMultiRowData += Sgh2_OnLoadMultiRowData;
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
                List<tb_SaleOutReDetail> details = new List<tb_SaleOutReDetail>();

                foreach (var item in RowDetails)
                {
                    tb_SaleOutReDetail bOM_SDetail = MainForm.Instance.mapper.Map<tb_SaleOutReDetail>(item);
                    bOM_SDetail.Quantity = 0;
                    details.Add(bOM_SDetail);
                }
                sgh.InsertItemDataToGrid<tb_SaleOutReDetail>(grid1, sgd, details, c => c.ProdDetailID, position);
            }
        }

        private void Sgh2_OnLoadMultiRowData(object rows, Position position)
        {
            List<View_ProdDetail> RowDetails = new List<View_ProdDetail>();
            var rowss = ((IEnumerable<dynamic>)rows).ToList();
            foreach (var item in rowss)
            {
                RowDetails.Add(item);
            }
            if (RowDetails != null)
            {
                List<tb_SaleOutReRefurbishedMaterialsDetail> details = new List<tb_SaleOutReRefurbishedMaterialsDetail>();

                foreach (var item in RowDetails)
                {
                    tb_SaleOutReRefurbishedMaterialsDetail bOM_SDetail = MainForm.Instance.mapper.Map<tb_SaleOutReRefurbishedMaterialsDetail>(item);
                    bOM_SDetail.Quantity = 0;
                    details.Add(bOM_SDetail);
                }
                sgh2.InsertItemDataToGrid<tb_SaleOutReRefurbishedMaterialsDetail>(grid2, sgd2, details, c => c.ProdDetailID, position);
            }

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
                //计算总金额  这些逻辑是不是放到业务层？后面要优化
                List<tb_SaleOutReDetail> details = sgd.BindingSourceLines.DataSource as List<tb_SaleOutReDetail>;
                if (details == null)
                {
                    return;
                }
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }
                EditEntity.TotalQty = details.Sum(c => c.Quantity);
                EditEntity.TotalCommissionAmount = details.Sum(c => c.CommissionAmount);
                EditEntity.TotalAmount = details.Sum(c => c.TransactionPrice * c.Quantity) + EditEntity.FreightIncome;
                EditEntity.TotalAmount = EditEntity.TotalAmount - EditEntity.TotalCommissionAmount;
            }
            catch (Exception ex)
            {
                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }
        }

        List<tb_SaleOutReDetail> details = new List<tb_SaleOutReDetail>();
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

            List<tb_SaleOutReDetail> oldOjb = new List<tb_SaleOutReDetail>(details.ToArray());

            List<tb_SaleOutReDetail> detailentity = bindingSourceSub.DataSource as List<tb_SaleOutReDetail>;
            List<tb_SaleOutReRefurbishedMaterialsDetail> RefurbishedMaterials = bindingSourceOtherSub.DataSource as List<tb_SaleOutReRefurbishedMaterialsDetail>;
            List<tb_SaleOutReRefurbishedMaterialsDetail> LastRefurbishedMaterials = bindingSourceOtherSub.DataSource as List<tb_SaleOutReRefurbishedMaterialsDetail>;


            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.ProdDetailID > 0).ToList();
                var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                if (NeedValidated && aa.Count > 1)
                {
                    System.Windows.Forms.MessageBox.Show("明细中，相同的产品不能多行录入,如有需要,请另建单据保存!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                //因为有一个特殊验证： RuleFor(tb_SaleOutReDetail => tb_SaleOutReDetail.Quantity).NotEqual(0).When(c => c.tb_saleoutre.RefundOnly == false).WithMessage("非仅退款时，退回数量不能为0为零。");
                foreach (var item in details)
                {
                    item.tb_saleoutre = EditEntity;
                }


                EditEntity.tb_SaleOutReDetails = details;
                if (details.Sum(c => c.SubtotalTaxAmount) > 0)
                {
                    EditEntity.IsIncludeTax = true;
                }
                else
                {
                    EditEntity.IsIncludeTax = false;
                }
                EditEntity.TotalQty = details.Sum(c => c.Quantity);
                EditEntity.TotalAmount = details.Sum(c => c.TransactionPrice * c.Quantity) + EditEntity.FreightIncome;
                EditEntity.TotalCommissionAmount = details.Sum(c => c.CommissionAmount);
                EditEntity.TotalAmount = EditEntity.TotalAmount - EditEntity.TotalCommissionAmount;

                //产品ID有值才算有效值
                LastRefurbishedMaterials = RefurbishedMaterials.Where(t => t.ProdDetailID > 0).ToList();
                var bb = LastRefurbishedMaterials.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                if (NeedValidated && bb.Count > 1)
                {
                    System.Windows.Forms.MessageBox.Show("翻新物料明细中，相同的产品不能多行录入,如有需要,请另建单据保存!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                EditEntity.tb_SaleOutReRefurbishedMaterialsDetails = LastRefurbishedMaterials;

                if (NeedValidated && !EditEntity.ReturnDate.HasValue)
                {
                    if (System.Windows.Forms.MessageBox.Show("退回日期为空，你确定无法确认退回日期吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                    {
                        return false;
                    }

                }

                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_SaleOutReDetail>(details))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_SaleOutReRefurbishedMaterialsDetail>(LastRefurbishedMaterials))
                {
                    return false;
                }
                if (NeedValidated && EditEntity.TotalQty != details.Sum(c => c.Quantity))
                {
                    System.Windows.Forms.MessageBox.Show("单据总数量和明细数量的和不相等，请检查记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                //计算总金额
                if (NeedValidated && (!EditEntity.RefundOnly && (EditEntity.TotalQty == 0 || details.Sum(c => c.Quantity) == 0 || EditEntity.TotalAmount == 0)))
                {
                    System.Windows.Forms.MessageBox.Show(" 退回总数量和金额不能为零，请检查记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                ReturnMainSubResults<tb_SaleOutRe> SaveResult = new ReturnMainSubResults<tb_SaleOutRe>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.ReturnNo}。");
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

        protected override async Task<bool> Submit()
        {
            if (EditEntity != null)
            {
                if (EditEntity.ReturnDate == null)
                {
                    //退货日期不能为空。
                    System.Windows.Forms.MessageBox.Show("退货日期不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (!EditEntity.RefundStatus.HasValue)
                {
                    System.Windows.Forms.MessageBox.Show("退货退款状态不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                bool rs = await base.Submit();
                return rs;
            }
            else
            {
                return false;
            }

        }

        private void cmbCustomerVendor_ID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCustomerVendor_ID.SelectedIndex > 0)
            {
                if (cmbCustomerVendor_ID.SelectedItem is tb_CustomerVendor cv)
                {
                    if (cv.Employee_ID.HasValue)
                    {
                        cmbEmployee_ID.SelectedValue = cv.Employee_ID.Value;
                    }
                }
            }
        }

        /*
          protected override void Preview()
        {
            //RptMainForm PRT = new RptMainForm();
            //PRT.ShowDialog();
            //return;
            //RptPreviewForm pForm = new RptPreviewForm();
            //pForm.ReprotfileName = "SOB.frx";
            //List<tb_SaleOutRe> main = new List<tb_SaleOutRe>();
            //main.Add(_EditEntity);
            //pForm.Show();
            tb_Stocktake testmain = new tb_Stocktake();
            //要给值
            //if (EditEntity == null)
            //{
            //    return;
            //    EditEntity = new tb_Stocktake();
            //}

            List<T> main = new List<T>();
            testmain.tb_StocktakeDetails = details;
            main.Add(testmain);
            FastReport.Report FReport;
            FReport = new FastReport.Report();
            FReport.RegisterData(main, "Main");
            String reportFile = "SOB.frx";
            RptPreviewForm frm = new RptPreviewForm();
            frm.ReprotfileName = reportFile;
            frm.MyReport = FReport;
            frm.ShowDialog();
        }
         */

        /*
        protected async override void ReReview()
        {
            _EditEntity = bindingSourceMain.Current as tb_Stocktake;
            if (_EditEntity == null)
            {
                return;
            }

            _EditEntity.actionStatus = ActionStatus.修改;
            if (_EditEntity.status == BillStateEnums.已审核.ToString() && _EditEntity.actionStatus == ActionStatus.修改)
            {
                //反审 TODO 这里有详细逻辑，比方，如果出库了，则无法反审，需要将对应关联的出库单反审核，才可以

                _EditEntity.status = BillStateEnums.未审核.ToString();
                bool rs = await soc.UpdateAsync(_EditEntity);
                if (rs)
                {
                    //显示审核状态
                    lblReview.Text = _EditEntity.status;
                }
            }
            base.ToolBarEnabledControl(MenuItemEnums.保存);
            base.Save();
        }
        */


        /// <summary>
        /// 将销售订单转换为销售退货单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void LoadSaleOutBillData(long? saleoutid)
        {
            //要加一个判断 值是否有变化
            //新增时才可以
            //转单
            ButtonSpecAny bsa = (txtSaleOutNo as KryptonTextBox).ButtonSpecs.FirstOrDefault(c => c.UniqueName == "btnQuery");
            if (bsa == null)
            {
                return;
            }
            var saleout = bsa.Tag as tb_SaleOut;//这个tag值。赋值会比较当前方法晚，所以失效
            saleout = await MainForm.Instance.AppContext.Db.Queryable<tb_SaleOut>().Where(c => c.SaleOut_MainID == saleoutid)
            .Includes(t => t.tb_SaleOutDetails, d => d.tb_proddetail)
            .Includes(t => t.tb_SaleOutRes, a => a.tb_SaleOutReDetails)
            .SingleAsync();
            if (saleout != null)
            {
                //如果这个销售出库单，已经有提交或审核过的。并且数量等于出库总数量则无法再次录入退回单。应该是不会显示出来了。
                if (saleout.tb_SaleOutRes.Sum(c => c.TotalQty) == saleout.tb_SaleOutDetails.Sum(c => c.Quantity))
                {
                    MainForm.Instance.uclog.AddLog("当前出库单已经全部退回，无法再次退回。");
                    return;
                }

                tb_SaleOutController<tb_SaleOut> ctr = Startup.GetFromFac<tb_SaleOutController<tb_SaleOut>>();
                tb_SaleOutRe saleOutre = ctr.SaleOutToSaleOutRe(saleout);

                BindData(saleOutre as tb_SaleOutRe);
            }
            else
            {
                MainForm.Instance.PrintInfoLog("选取的对象为空！");
            }
        }
    }
}
