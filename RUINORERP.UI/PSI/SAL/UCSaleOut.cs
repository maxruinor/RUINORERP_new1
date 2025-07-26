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
using RUINORERP.Global.EnumExt;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.UI.Monitoring.Auditing;
using SourceGrid;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using RUINORERP.UI.BusinessService.CalculationService;
using System.Threading;

namespace RUINORERP.UI.PSI.SAL
{
    [MenuAttrAssemblyInfo("销售出库单", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.销售管理, BizType.销售出库单)]
    public partial class UCSaleOut : BaseBillEditGeneric<tb_SaleOut, tb_SaleOutDetail>, IPublicEntityObject
    {
        public UCSaleOut()
        {
            InitializeComponent();
            base.toolStripButton结案.Visible = true;
            AddPublicEntityObject(typeof(ProductSharePart));
        }


        private SaleOutCoordinator _coordinator;
        internal override void LoadDataToUI(object Entity)
        {
            ActionStatus actionStatus = ActionStatus.无操作;
            BindData(Entity as tb_SaleOut, actionStatus);
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
            toolStripButton平台退款.ToolTipText = "平台订单退款时，会强制校验是否生成销售退货单，如果没有，则会自动预生成。";
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
                tb_SaleOut saleOut = EditEntity as tb_SaleOut;
                //只有审核状态才可以转换
                if (EditEntity.DataStatus >= (int)DataStatus.确认 && EditEntity.ApprovalStatus == (int)ApprovalStatus.已审核 && EditEntity.ApprovalResults.HasValue && EditEntity.ApprovalResults.Value)
                {
                    //判断是否为平台订单
                    if (!saleOut.IsFromPlatform || saleOut.PlatformOrderNo.IsNullOrEmpty())
                    {
                        MessageBox.Show($"当前【销售出库单】对应订单为不是平台订单，无法进行平台退款", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        toolStripButton平台退款.Enabled = false;
                        return;
                    }

                    //判断是否已经退款
                    //if (EditEntity.RefundStatus.HasValue && EditEntity.RepairStatus.Value != (int)RepairStatus.待维修)
                    //{
                    //    MessageBox.Show($"当前【维修工单】的维修状态为:{(RepairStatus)EditEntity.RepairStatus.Value}，无法重复进行平台退款", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    toolStripButton平台退款.Enabled = false;
                    //    return;
                    //}

                    //if (EditEntity.PayStatus == (int)PayStatus.未付款)
                    //{
                    //    if (MessageBox.Show($"当前【维修工单】的付款状态为:{(PayStatus)EditEntity.PayStatus}，你确定仍要进行【平台退款】吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No)
                    //    {
                    //        return;
                    //    }
                    //}

                    //var ctr = Startup.GetFromFac<tb_AS_RepairOrderController<tb_AS_RepairOrder>>();
                    //ReturnResults<tb_AS_RepairOrder> rrs = await ctr.RepairProcessAsync(EditEntity);
                    //if (rrs.Succeeded)
                    //{
                    //    toolStripButton平台退款.Enabled = false;
                    //    MainForm.Instance.AuditLogHelper.CreateAuditLog<tb_AS_RepairOrder>("平台退款", EditEntity);
                    //    MessageBox.Show($"当前【维修工单】的产品，将从【售后暂存仓】出库，【全部】交由维修人员处理", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    //}
                    //else
                    //{
                    //    MessageBox.Show($"当前【维修工单】平台退款失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //}
                    return;
                }
                else
                {
                    MessageBox.Show($"当前【维修工单】未审核，无法进行【平台退款】", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }


        #endregion
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

        protected override void LoadRelatedDataToDropDownItems()
        {
            //加载关联的单据
            if (base.EditEntity is tb_SaleOut saleOut)
            {
                if (saleOut.SOrder_ID.HasValue)
                {
                    var rqp = new Model.CommonModel.RelatedQueryParameter();
                    rqp.bizType = BizType.销售订单;
                    rqp.billId = saleOut.SOrder_ID.Value;
                    ToolStripMenuItem RelatedMenuItem = new ToolStripMenuItem();
                    RelatedMenuItem.Name = $"{rqp.billId}";
                    RelatedMenuItem.Tag = rqp;
                    RelatedMenuItem.Text = $"{rqp.bizType}:{saleOut.SaleOrderNo}";
                    RelatedMenuItem.Click += base.MenuItem_Click;
                    if (!toolStripbtnRelatedQuery.DropDownItems.ContainsKey(saleOut.SOrder_ID.Value.ToString()))
                    {
                        toolStripbtnRelatedQuery.DropDownItems.Add(RelatedMenuItem);
                    }

                    if (saleOut.tb_SaleOutRes != null && saleOut.tb_SaleOutRes.Count > 0)
                    {
                        foreach (var item in saleOut.tb_SaleOutRes)
                        {
                            var rqpSub = new Model.CommonModel.RelatedQueryParameter();
                            rqpSub.bizType = BizType.销售退回单;
                            rqpSub.billId = item.SaleOutRe_ID;
                            ToolStripMenuItem RelatedMenuItemSub = new ToolStripMenuItem();
                            RelatedMenuItemSub.Name = $"{rqpSub.billId}";
                            RelatedMenuItemSub.Tag = rqpSub;
                            RelatedMenuItemSub.Text = $"{rqpSub.bizType}:{item.ReturnNo}";
                            RelatedMenuItemSub.Click += base.MenuItem_Click;
                            if (!toolStripbtnRelatedQuery.DropDownItems.ContainsKey(rqpSub.billId.ToString()))
                            {
                                toolStripbtnRelatedQuery.DropDownItems.Add(RelatedMenuItemSub);
                            }
                        }

                    }
                }
            }
            base.LoadRelatedDataToDropDownItems();
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
                    if (entity.Currency_ID != AppContext.BaseCurrency.Currency_ID)
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

            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.TotalCommissionAmount.ToString(), txtTotalCommissionAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.CustomerPONo, txtCustomerPONo, BindDataType4TextBox.Text, false);
            //DataBindingHelper.BindData4Cmb<tb_SaleOrder>(entity, k => k.SOrder_ID, v => v.SOrderNo, cmbOrder_ID);
            DataBindingHelper.BindData4CmbByEnum<tb_SaleOut>(entity, k => k.PayStatus, typeof(PayStatus), cmbPayStatus, false);

            DataBindingHelper.BindData4Cmb<tb_PaymentMethod>(entity, k => k.Paytype_ID, v => v.Paytype_Name, cmbPaytype_ID);
            DataBindingHelper.BindData4CheckBox<tb_SaleOut>(entity, t => t.IsFromPlatform, chkIsFromPlatform, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.PlatformOrderNo, txtPlatformOrderNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.SaleOutNo, txtSaleOutNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.FreightIncome.ToString(), txtFreightIncome, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.FreightCost.ToString(), txtFreightCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v => v.CurrencyName, cmbCurrency_ID);
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
                details = entity.tb_SaleOutDetails;
            }
            sgh.LoadItemDataToGrid<tb_SaleOutDetail>(grid1, sgd, details, c => c.ProdDetailID);


            //缓存一下结果下次如果一样，就忽略？
            //object tempcopy = entity.Clone();
            //如果属性变化 则状态为修改

            entity.PropertyChanged += async (sender, s2) =>
            {
                //如果是销售订单引入变化则加载明细及相关数据
                if ((entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改) && entity.SOrder_ID.HasValue && entity.SOrder_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_SaleOut>(c => c.SOrder_ID))
                {
                    await OrderToOutBill(entity.SOrder_ID.Value);
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
                                if (EditEntity.Currency_ID != AppContext.BaseCurrency.Currency_ID)
                                {
                                    EditEntity.ForeignTotalAmount = EditEntity.TotalAmount / EditEntity.ExchangeRate;
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
                                entity.ExchangeRate = 1;
                                entity.ForeignTotalAmount = 0;
                            }
                        }

                    }

                    if (s2.PropertyName == entity.GetPropertyName<tb_SaleOut>(c => c.Paytype_ID) && entity.Paytype_ID > 0)
                    {
                        if (cmbPaytype_ID.SelectedItem is tb_PaymentMethod paymentMethod)
                        {
                            EditEntity.tb_paymentmethod = paymentMethod;
                        }
                    }
                    /*

                    #region 计算运费成本分摊
                    if (entity.FreightCost >= 0 && s2.PropertyName == entity.GetPropertyName<tb_SaleOut>(c => c.FreightCost))
                    {
                        // 如果正在计算中，则跳过本次处理，避免循环
                        if (_isCalculating) return;
                        try
                        {
                            // 设置计算状态
                            _isCalculating = true;
                            Expression<Func<tb_SaleOutDetail, object>> colNameExp = c => c.AllocatedFreightCost;
                            string colName = colNameExp.GetMemberInfo().Name;
                            var coltarget = sgh.SGDefine[colName];
                            int colIndex = sgh.SGDefine.grid.Columns.GetColumnInfo(coltarget.UniqueId).Index;


                            //默认认为 订单中的运费收入 就是实际发货的运费成本， 可以手动修改覆盖
                            //根据系统设置中的分摊规则来分配运费收入到明细。
                            if (MainForm.Instance.AppContext.SysConfig.FreightAllocationRules == (int)FreightAllocationRules.产品数量占比)
                            {
                                // 单个产品分摊运费 = 整单运费 ×（该产品数量 ÷ 总产品数量） 
                                foreach (var item in entity.tb_SaleOutDetails)
                                {
                                    item.AllocatedFreightCost = EditEntity.FreightCost * (item.Quantity.ToDecimal() / EditEntity.TotalQty.ToDecimal());
                                    item.AllocatedFreightCost = item.AllocatedFreightCost.ToRoundDecimalPlaces(authorizeController.GetMoneyDataPrecision());
                                    item.FreightAllocationRules = MainForm.Instance.AppContext.SysConfig.FreightAllocationRules;
                                    for (int i = 0; i < sgh.SGDefine.grid.Rows.Count; i++)
                                    {
                                        if (sgh.SGDefine.grid.Rows[i].RowData != null && sgh.SGDefine.grid.Rows[i].RowData is tb_SaleOutDetail line)
                                        {
                                            if (line.ProdDetailID == item.ProdDetailID)
                                            {
                                                Position position = new Position(i, colIndex);
                                                sgh.SetCellValueForCurrentUICell(coltarget, colName, position, item);
                                                continue;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        finally
                        {
                            // 无论是否发生异常，都要重置计算状态
                            _isCalculating = false;
                        }
                    }

                    #endregion

                    #region 计算运费收入分摊
                    if (entity.FreightIncome >= 0 && s2.PropertyName == entity.GetPropertyName<tb_SaleOut>(c => c.FreightIncome))
                    {// 如果正在计算中，则跳过本次处理，避免循环
                        if (_isCalculating) return;
                        try
                        {
                            // 设置计算状态
                            _isCalculating = true;
                            Expression<Func<tb_SaleOutDetail, object>> colNameExp = c => c.AllocatedFreightIncome;
                            string colName = colNameExp.GetMemberInfo().Name;
                            var coltarget = sgh.SGDefine[colName];
                            int colIndex = sgh.SGDefine.grid.Columns.GetColumnInfo(coltarget.UniqueId).Index;
                            //默认认为 订单中的运费收入 就是实际发货的运费成本， 可以手动修改覆盖
                            //根据系统设置中的分摊规则来分配运费收入到明细。
                            if (MainForm.Instance.AppContext.SysConfig.FreightAllocationRules == (int)FreightAllocationRules.产品数量占比)
                            {
                                // 单个产品分摊运费 = 整单运费 ×（该产品数量 ÷ 总产品数量） 
                                foreach (var item in entity.tb_SaleOutDetails)
                                {
                                    item.AllocatedFreightIncome = EditEntity.FreightIncome * (item.Quantity.ToDecimal() / EditEntity.TotalQty.ToDecimal());
                                    item.AllocatedFreightIncome = item.AllocatedFreightIncome.ToRoundDecimalPlaces(authorizeController.GetMoneyDataPrecision());
                                    item.FreightAllocationRules = MainForm.Instance.AppContext.SysConfig.FreightAllocationRules;
                                    for (int i = 0; i < sgh.SGDefine.grid.Rows.Count; i++)
                                    {
                                        if (sgh.SGDefine.grid.Rows[i].RowData != null && sgh.SGDefine.grid.Rows[i].RowData is tb_SaleOutDetail line)
                                        {
                                            if (line.ProdDetailID == item.ProdDetailID)
                                            {
                                                Position position = new Position(i, colIndex);
                                                sgh.SetCellValueForCurrentUICell(coltarget, colName, position, item);
                                                continue;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        finally
                        {
                            // 无论是否发生异常，都要重置计算状态
                            _isCalculating = false;
                        }
                    }

                    #endregion
                    */

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


            BaseProcessor basePro = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_SaleOrder).Name + "Processor");
            QueryFilter queryFilter = basePro.GetQueryFilter();

            var lambdaOrder = Expressionable.Create<tb_SaleOrder>()
             .And(t => t.DataStatus == (int)DataStatus.确认)
             .And(t => t.ApprovalStatus.HasValue && t.ApprovalStatus.Value == (int)ApprovalStatus.已审核)
             .And(t => t.ApprovalResults.HasValue && t.ApprovalResults.Value == true)
              .And(t => t.isdeleted == false)
             .ToExpression();
            //如果有限制则设置一下 但是注意 不应该在这设置，灵活的应该是在调用层设置
            queryFilter.SetFieldLimitCondition(lambdaOrder);
            ControlBindingHelper.ConfigureControlFilter<tb_SaleOut, tb_SaleOrder>(entity, txtSaleOrder, t => t.SaleOrderNo,
                f => f.SOrderNo, queryFilter, a => a.SOrder_ID, b => b.SOrder_ID, null, false);

            sgd.GridMasterData = entity;
            sgd.GridMasterDataType = entity.GetType();

            _coordinator = new SaleOutCoordinator(EditEntity, EditEntity.tb_SaleOutDetails, sgh);
            if (EditEntity.FreightCost != details.Sum(c => c.AllocatedFreightCost))
            {
                _coordinator.HandleMasterPropertyChange(c => c.FreightCost);
            }
            if (EditEntity.FreightIncome != details.Sum(c => c.AllocatedFreightIncome))
            {
                _coordinator.HandleMasterPropertyChange(c => c.FreightIncome);
            }
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

            //不允许编辑的字段  单价成交价不能修改不然会乱。实在要调整。就是价格调整单
            //可能导致订单与出库记录脱节，引发财务对账混乱、库存成本核算偏差，甚至产生合规风险（如税务审计时数据不一致）
            //if (!AppContext.CurUserInfo.UserInfo.IsSuperUser)
            //{
            listCols.SetCol_ReadOnly<tb_SaleOutDetail>(c => c.UnitPrice);
            listCols.SetCol_ReadOnly<tb_SaleOutDetail>(c => c.TransactionPrice);
            //}
            listCols.SetCol_ReadOnly<tb_SaleOutDetail>(c => c.SubtotalTransAmount);
            if (!AppContext.CurUserInfo.UserInfo.IsSuperUser)
            {
                listCols.SetCol_ReadOnly<tb_SaleOutDetail>(c => c.CustomizedCost);
            }
            listCols.SetCol_ReadOnly<tb_SaleOutDetail>(c => c.SubtotalCostAmount);

            //订单指定了仓库时。更新了 在途 拟销 等 数量 所以这里不能修改了。要改前面订单也要改
            listCols.SetCol_ReadOnly<tb_SaleOutDetail>(c => c.Location_ID);

            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);

            listCols.SetCol_Format<tb_SaleOutDetail>(c => c.FreightAllocationRules, CustomFormatType.EnumOptions, null, typeof(FreightAllocationRules));

            listCols.SetCol_Format<tb_SaleOutDetail>(c => c.Discount, CustomFormatType.PercentFormat);
            listCols.SetCol_Format<tb_SaleOutDetail>(c => c.TaxRate, CustomFormatType.PercentFormat);
            sgd = new SourceGridDefine(grid1, listCols, true);


            listCols.SetCol_Formula<tb_SaleOutDetail>((a, b) => a.UnitPrice * b.Discount, c => c.TransactionPrice);
            //listCols.SetCol_Formula<tb_SaleOutDetail>((a, b) => a.Cost * b.Quantity, c => c.SubtotalCostAmount);
            listCols.SetCol_Formula<tb_SaleOutDetail>((a, b) => (a.Cost + a.CustomizedCost) * b.Quantity, c => c.SubtotalCostAmount);
            listCols.SetCol_Formula<tb_SaleOutDetail>((a, b) => a.TransactionPrice * b.Quantity, c => c.SubtotalTransAmount);
            listCols.SetCol_Formula<tb_SaleOutDetail>((a, b, c) => a.SubtotalTransAmount / (1 + b.TaxRate) * c.TaxRate, d => d.SubtotalTaxAmount);


            listCols.SetCol_Formula<tb_SaleOutDetail>((a, b) => a.UnitCommissionAmount * b.Quantity, c => c.CommissionAmount);
            listCols.SetCol_FormulaReverse<tb_SaleOutDetail>(d => d.Quantity != 0, (a, b) => a.CommissionAmount / b.Quantity, c => c.UnitCommissionAmount);


            //将数量默认为已出库数量  这个逻辑不对这个是订单累计 的出库数量只能是在出库审核时才累计数据，这里最多只读
            //listCols.SetCol_Formula<tb_SaleOutDetail>((a, b) => a.Quantity, c => c.TotalDeliveredQty);

            //listCols.SetCol_Summary<tb_SaleOutDetail>(c => c.Quantity);
            //listCols.SetCol_Summary<tb_SaleOutDetail>(c => c.CommissionAmount);
            //listCols.SetCol_Summary<tb_SaleOutDetail>(c => c.SubtotalCostAmount);
            //listCols.SetCol_Summary<tb_SaleOutDetail>(c => c.SubtotalTransAmount);
            //listCols.SetCol_Summary<tb_SaleOutDetail>(c => c.SubtotalTaxAmount);
            BaseProcessor baseProcessor = BusinessHelper._appContext.GetRequiredServiceByName<BaseProcessor>(typeof(tb_SaleOutDetail).Name + "Processor");
            var summaryCols = baseProcessor.GetSummaryCols();
            foreach (var item in summaryCols)
            {
                foreach (var col in listCols)
                {
                    col.SetCol_Summary<tb_SaleOutDetail>(item);
                }
            }

            listCols.SetCol_Summary<tb_SaleOutDetail>(c => c.AllocatedFreightCost);
            listCols.SetCol_Summary<tb_SaleOutDetail>(c => c.AllocatedFreightIncome);

            sgh.SetPointToColumnPairs<ProductSharePart, tb_SaleOutDetail>(sgd, f => f.Location_ID, t => t.Location_ID);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_SaleOutDetail>(sgd, f => f.Rack_ID, t => t.Rack_ID);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_SaleOutDetail>(sgd, f => f.Inv_Cost, t => t.Cost);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_SaleOutDetail>(sgd, f => f.Standard_Price, t => t.UnitPrice);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_SaleOutDetail>(sgd, f => f.prop, t => t.property);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_SaleOutDetail>(sgd, f => f.Model, t => t.CustomerPartNo, false);

            //应该只提供一个结构
            List<tb_SaleOutDetail> lines = new List<tb_SaleOutDetail>();
            bindingSourceSub.DataSource = lines;
            sgd.BindingSourceLines = bindingSourceSub;


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
        // 声明一个状态标志，用于控制计算逻辑
        private bool _isCalculating = false;
        AuthorizeController authorizeController = null;
        private void Sgh_OnCalculateColumnValue(object rowObj, SourceGridDefine griddefine, SourceGrid.Position Position)
        {
            if (EditEntity == null)
            {
                //都不是正常状态
                MainForm.Instance.uclog.AddLog("请先使用新增或查询加载数据");
                return;
            }
            if (authorizeController == null)
            {
                authorizeController = MainForm.Instance.AppContext.GetRequiredService<AuthorizeController>();
            }

            try
            {
                //if (EditEntity.actionStatus == ActionStatus.加载)
                //{
                //计算总金额  这些逻辑是不是放到业务层？后面要优化
                List<tb_SaleOutDetail> details = sgd.BindingSourceLines.DataSource as List<tb_SaleOutDetail>;
                //    return;
                //}
                if (details == null || details.Count == 0)
                {
                    return;
                }
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }
                foreach (var item in details)
                {
                    item.SubtotalCostAmount = (item.Cost + item.CustomizedCost) * item.Quantity;
                }

                if (_coordinator != null)
                {
                    _coordinator.Master = EditEntity;
                    _coordinator.Details = details;
                }

                EditEntity.TotalQty = details.Sum(c => c.Quantity);
                EditEntity.TotalCost = details.Sum(c => (c.Cost + c.CustomizedCost) * c.Quantity) + EditEntity.FreightCost;
                EditEntity.TotalTaxAmount = details.Sum(c => c.SubtotalTaxAmount);
                EditEntity.TotalCommissionAmount = details.Sum(c => c.CommissionAmount);
                EditEntity.TotalTaxAmount = EditEntity.TotalTaxAmount.ToRoundDecimalPlaces(MainForm.Instance.authorizeController.GetMoneyDataPrecision());
                /*
                #region 计算运费总额
                try
                {
                    // 如果正在计算中，则跳过本次处理，避免循环
                    if (!_isCalculating)
                    {
                        // 设置计算状态
                        _isCalculating = true;
                        EditEntity.FreightCost = details.Sum(c => c.AllocatedFreightCost);
                        EditEntity.FreightIncome = details.Sum(c => c.AllocatedFreightIncome);
                    }

                }
                catch (Exception ex)
                {
                    logger.LogError("计算出错", ex);
                    MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
                }
                finally
                {
                    // 无论是否发生异常，都要重置计算状态
                    _isCalculating = false;
                }
                #endregion
                */

                EditEntity.TotalAmount = details.Sum(c => c.TransactionPrice * c.Quantity);
                EditEntity.TotalAmount = EditEntity.TotalAmount + EditEntity.FreightIncome;
                if (EditEntity.Currency_ID != AppContext.BaseCurrency.Currency_ID)
                {
                    EditEntity.ForeignTotalAmount = EditEntity.TotalAmount / EditEntity.ExchangeRate;
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

            if (NeedValidated && (!EditEntity.SOrder_ID.HasValue || EditEntity.SOrder_ID.Value == 0))
            {
                MessageBox.Show("请选择正确的销售订单，或从销售订单查询中转为出库单！");
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
                details.ForEach(
                   c =>
                   {
                       if (c.ProdDetailID > 0)
                       {
                           if (c.SubtotalCostAmount != (c.Cost + c.CustomizedCost) * c.Quantity)
                           {
                               c.SubtotalCostAmount = (c.Cost + c.CustomizedCost) * c.Quantity;
                           }


                           if (c.SubtotalTransAmount != c.TransactionPrice * c.Quantity)
                           {
                               c.SubtotalTransAmount = c.TransactionPrice * c.Quantity;
                           }

                           decimal tempSubTaxAmount = c.SubtotalTransAmount / (1 + c.TaxRate) * c.TaxRate;
                           decimal diffpirce = Math.Abs(tempSubTaxAmount - c.SubtotalTaxAmount);
                           if (diffpirce > 0.01m)
                           {
                               c.SubtotalTaxAmount = c.SubtotalTransAmount / (1 + c.TaxRate) * c.TaxRate;
                               c.SubtotalTaxAmount = c.SubtotalTaxAmount.ToRoundDecimalPlaces(MainForm.Instance.authorizeController.GetMoneyDataPrecision());
                           }

                           if (c.CustomizedCost > 0)
                           {
                               EditEntity.IsCustomizedOrder = true;
                           }
                       }
                   }
               );
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


                //订单中gift 分开出库时。就可能出现金额为0
                //if (NeedValidated && ((EditEntity.TotalAmount == 0 || detailentity.Sum(c => c.TransactionPrice * c.Quantity) == 0)) && EditEntity.tb_saleorder.TotalQty != detailentity.Sum(c => c.Quantity))
                //{
                //    if (MessageBox.Show("单据总金额或明细总金额为零，你确定吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                //    {
                //        return false;
                //    }
                //}
                //if (NeedValidated && (EditEntity.TotalAmount != detailentity.Sum(c => c.TransactionPrice * c.Quantity)))
                //{
                //    MessageBox.Show("单据总金额与明细总金额不相等！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return false;
                //}
                if (NeedValidated && ((EditEntity.TotalAmount + EditEntity.FreightIncome) < detailentity.Sum(c => c.TransactionPrice * c.Quantity)))
                {
                    MessageBox.Show("单据总金额不能小于明细总金额！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                //如果所有数据都出库。运费成本也要一致。否则提醒
                if (NeedValidated)
                {
                    if (EditEntity.FreightCost != details.Sum(c => c.AllocatedFreightCost))
                    {
                        _coordinator.HandleMasterPropertyChange(c => c.FreightCost);

                        if (MessageBox.Show($"运费成本{EditEntity.FreightCost}与明细运费成本分摊的总金额{details.Sum(c => c.AllocatedFreightCost)}不一致\r\n" +
                            $"系统已经自动分摊，确定要保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.No)
                            return false;

                    }
                    if (EditEntity.FreightIncome != details.Sum(c => c.AllocatedFreightIncome))
                    {
                        _coordinator.HandleMasterPropertyChange(c => c.FreightIncome);
                        if (MessageBox.Show($"运费收入{EditEntity.FreightIncome}与明细运费收入分摊总金额{details.Sum(c => c.AllocatedFreightIncome)}不一致\r\n" +
                            $"系统已经自动分摊，确定要保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                            return false;
                    }
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
                EditEntity.TotalCost = details.Sum(c => (c.Cost + c.CustomizedCost) * c.Quantity) + EditEntity.FreightCost;
                EditEntity.TotalTaxAmount = details.Sum(c => c.SubtotalTaxAmount);
                EditEntity.TotalCommissionAmount = details.Sum(c => c.CommissionAmount);
                EditEntity.TotalTaxAmount = EditEntity.TotalTaxAmount.ToRoundDecimalPlaces(MainForm.Instance.authorizeController.GetMoneyDataPrecision());

                EditEntity.TotalAmount = details.Sum(c => c.TransactionPrice * c.Quantity) + EditEntity.FreightIncome;
                if (EditEntity.Currency_ID != AppContext.BaseCurrency.Currency_ID)
                {
                    EditEntity.ForeignTotalAmount = EditEntity.TotalAmount / EditEntity.ExchangeRate;
                    //
                    EditEntity.ForeignTotalAmount = Math.Round(EditEntity.ForeignTotalAmount, 2); // 四舍五入到 2 位小数
                }


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

        //protected async override Task<ReturnResults<tb_SaleOut>> Delete()
        //{
        //    ReturnResults<tb_SaleOut> rss = new ReturnResults<tb_SaleOut>();
        //    rss = await base.Delete();
        //    if (rss.Succeeded)
        //    {
        //        string msg = string.Empty;
        //        msg = $"订单号：{EditEntity.SaleOrderNo} 对应的出库单 {EditEntity.SaleOutNo} 删除成功。";
        //        MainForm.Instance.AuditLogHelper.CreateAuditLog<tb_SaleOut>("删除细节", EditEntity, msg);
        //    }
        //    return rss;
        //}

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
