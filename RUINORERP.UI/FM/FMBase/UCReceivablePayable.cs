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
using RUINORERP.UI.Network.Services;
using SqlSugar;
using SourceGrid;
using System.Linq.Expressions;
using RUINORERP.Common.Extensions;

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

using FastReport;
using FastReport.Data;

using NPOI.SS.Formula.Functions;
using RUINORERP.Business.Cache;

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
            picBox红字.Visible = false;
        }

        public override void AddExcludeMenuList()
        {
            base.AddExcludeMenuList(MenuItemEnums.反结案);
            base.AddExcludeMenuList(MenuItemEnums.结案);
        }
        protected override async Task LoadRelatedDataToDropDownItemsAsync()
        {
            if (base.EditEntity is tb_FM_ReceivablePayable receivablePayable)
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
                //查是否有收付款单
                if (receivablePayable.ARAPStatus >= (int)ARAPStatus.待支付)
                {
                    var PaymentList = await MainForm.Instance.AppContext.Db.Queryable<tb_FM_PaymentRecord>()
                               .Includes(a => a.tb_FM_PaymentRecordDetails)
                              .Where(c => c.tb_FM_PaymentRecordDetails.Any(d => d.SourceBilllId == receivablePayable.ARAPId)).ToListAsync();
                    if (PaymentList != null && PaymentList.Count > 0)
                    {
                        foreach (var item in PaymentList)
                        {
                            var rqp = new Model.CommonModel.RelatedQueryParameter();
                            if (item.ReceivePaymentType == (int)ReceivePaymentType.付款)
                            {
                                rqp.bizType = BizType.付款单;
                            }
                            else
                            {
                                rqp.bizType = BizType.收款单;
                            }


                            rqp.billId = item.PaymentId;
                            ToolStripMenuItem RelatedMenuItem = new ToolStripMenuItem();
                            RelatedMenuItem.Name = $"{rqp.billId}";
                            RelatedMenuItem.Tag = rqp;
                            RelatedMenuItem.Text = $"{rqp.bizType}:{item.PaymentNo}";
                            RelatedMenuItem.ToolTipText = $"{item.PaymentNo}支付金额【{item.TotalLocalAmount}】";
                            RelatedMenuItem.Click += base.MenuItem_Click;
                            if (!toolStripbtnRelatedQuery.DropDownItems.ContainsKey(item.PaymentId.ToString()))
                            {
                                toolStripbtnRelatedQuery.DropDownItems.Add(RelatedMenuItem);
                            }
                        }
                    }

                    //应收款单可能还会核销预收款,如何查询？
                    #region

                    var Settlements = await MainForm.Instance.AppContext.Db.Queryable<tb_FM_PaymentSettlement>()
                           .Where(c => c.CustomerVendor_ID == receivablePayable.CustomerVendor_ID)
                           .Where(c => c.Currency_ID == receivablePayable.Currency_ID)
                           .Where(c => c.ReceivePaymentType == receivablePayable.ReceivePaymentType)
                           //.Where(c => c.SourceBizType == (int)BizType.预收款单)
                           //.Where(c => c.TargetBizType == (int)BizType.应收款单)
                           .Where(c => c.TargetBillId == receivablePayable.ARAPId && c.isdeleted == false)
                           .OrderBy(c => c.SettleDate)
                           .ToListAsync();

                    ////通过核销记录找到应收付的预收付记录表
                    foreach (var item in Settlements)
                    {
                        if (!item.SourceBizType.HasValue)
                        {
                            //坏账时，没有核销记录，将来可以做无形费用单
                            continue;
                        }

                        var rqp = new Model.CommonModel.RelatedQueryParameter();

                        rqp.bizType = (BizType)item.SourceBizType.Value;
                        rqp.billId = item.SourceBillId.Value;
                        ToolStripMenuItem RelatedMenuItem = new ToolStripMenuItem();
                        RelatedMenuItem.Name = $"{rqp.billId}";
                        RelatedMenuItem.Tag = rqp;
                        RelatedMenuItem.Text = $"{rqp.bizType}:{item.SourceBillNo}[核销]";
                        RelatedMenuItem.ToolTipText = $"{item.SourceBillNo}核销金额【{item.SettledLocalAmount}】";
                        RelatedMenuItem.Click += base.MenuItem_Click;
                        if (!toolStripbtnRelatedQuery.DropDownItems.ContainsKey(rqp.billId.ToString()))
                        {
                            toolStripbtnRelatedQuery.DropDownItems.Add(RelatedMenuItem);
                        }
                    }

                    #endregion

                }

            }
            await base.LoadRelatedDataToDropDownItemsAsync();
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

        /// <summary>
        /// 统一处理收款信息的加载逻辑
        /// </summary>
        /// <param name="entity">应收应付实体</param>
        private void LoadPayeeInfo(tb_FM_ReceivablePayable entity)
        {
            // 创建表达式 - 使用SqlSugar的Expressionable
            var expressionable = Expressionable.Create<tb_FM_PayeeInfo>()
                            .And(t => t.Is_enabled == true);

            if (entity.CustomerVendor_ID > 0)
            {
                // 使用SqlSugar的Expressionable继续添加条件
                expressionable = expressionable.And(t => t.CustomerVendor_ID == entity.CustomerVendor_ID);
            }


            // 最后转换为表达式
            Expression<Func<tb_FM_PayeeInfo, bool>> lambdaPayeeInfo = expressionable.ToExpression();

            BaseProcessor baseProcessorPayeeInfo = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_FM_PayeeInfo).Name + "Processor");
            QueryFilter queryFilterPayeeInfo = baseProcessorPayeeInfo.GetQueryFilter();
            queryFilterPayeeInfo.SetFieldLimitCondition(lambdaPayeeInfo);
            // 根据单据状态决定是否允许编辑
            bool canEdit = entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改 ||
                          (entity.ARAPStatus == (int)ARAPStatus.草稿) || (entity.ARAPStatus == (int)ARAPStatus.待审核);


            #region 测试
            // 初始化筛选后的列表
            List<tb_FM_PayeeInfo> filteredList = new List<tb_FM_PayeeInfo>();

            // 优先从缓存获取数据
            var EntityList = _cacheManager.GetEntityList<tb_FM_PayeeInfo>(nameof(tb_FM_PayeeInfo));
            if (EntityList != null && EntityList.Any())
            {
                // 使用完全避免编译的筛选方法
                filteredList = SafeFilterList(EntityList, lambdaPayeeInfo);

                //过滤失败时用原始的缓存数据
                if (filteredList.Count == 0 && filteredList.Count < EntityList.Count)
                {
                    filteredList = EntityList;
                }
            }

            #endregion


            DataBindingHelper.BindData4Cmb<tb_FM_PayeeInfo>(entity, k => k.PayeeInfoID, v => v.DisplayText, cmbPayeeInfoID, queryFilterPayeeInfo.GetFilterExpression<tb_FM_PayeeInfo>(), true);
            DataBindingHelper.InitFilterForControlByExpCanEdit<tb_FM_PayeeInfo>(entity, cmbPayeeInfoID, c => c.DisplayText, queryFilterPayeeInfo, canEdit);

            // 设置默认值（如果允许编辑且没有选择值）
            if (canEdit && entity.PayeeInfoID == null)
            {
                //设置一个默认值 如果有默认收款账号时
                var payeeInfoList = cmbPayeeInfoID.Items.CastToList<tb_FM_PayeeInfo>().Where(c => c.PayeeInfoID != -1).ToList();
                if (!payeeInfoList.Any(c => c.PayeeInfoID == entity.PayeeInfoID) && entity.CustomerVendor_ID > 0)
                {
                    if (payeeInfoList.FirstOrDefault(c => c.IsDefault) != null)
                    {
                        entity.PayeeInfoID = payeeInfoList.FirstOrDefault(c => c.IsDefault).PayeeInfoID;
                        btnInfo.Tag = payeeInfoList.FirstOrDefault(c => c.IsDefault);
                    }
                }
            }
        }

        /// <summary>
        /// 加载收款信息详情
        /// </summary>
        /// <param name="entity">应收应付实体</param>
        private async Task LoadPayeeInfoDetailAsync(tb_FM_ReceivablePayable entity)
        {
            if (entity.PayeeInfoID > 0)
            {
                tb_FM_PayeeInfo payeeInfo = null;
                var obj = _cacheManager.GetEntity<tb_FM_PayeeInfo>(entity.PayeeInfoID);
                if (obj != null && obj is tb_FM_PayeeInfo)
                {
                    payeeInfo = (tb_FM_PayeeInfo)obj;
                }
                else
                {
                    //直接加载 不用缓存
                    payeeInfo = await MainForm.Instance.AppContext.Db.Queryable<tb_FM_PayeeInfo>().Where(c => c.PayeeInfoID == entity.PayeeInfoID).FirstAsync();
                }

                if (payeeInfo != null)
                {
                    btnInfo.Tag = payeeInfo;
                    btnInfo.Visible = !string.IsNullOrEmpty(payeeInfo.PaymentCodeImagePath);
                }
            }
            else
            {
                btnInfo.Tag = string.Empty;
                btnInfo.Visible = false;
            }
        }

        public override void BindData(tb_FM_ReceivablePayable entity, ActionStatus actionStatus)
        {
            if (entity == null)
            {
                return;
            }

            chkIsExpenseType.Enabled = false;
            txtForeignBalanceAmount.Enabled = false;
            txtLocalBalanceAmount.Enabled = false;
            txtForeignPaidAmount.Enabled = false;
            txtLocalPaidAmount.Enabled = false;

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
                //如果转单过来的。业务日期已经有值了。就不能赋值为当前
                if (!entity.BusinessDate.HasValue)
                {
                    entity.BusinessDate = System.DateTime.Now;
                }
                entity.DocumentDate = System.DateTime.Now;


                //默认新建的都是费用，
                //明细一开始就是大于0的，则是转单过来的。则不是费用
                if (entity.tb_FM_ReceivablePayableDetails != null && entity.tb_FM_ReceivablePayableDetails.Count > 0)
                {
                    entity.IsExpenseType = false;
                }
                else
                {
                    entity.IsExpenseType = true;
                }


                //到期日期应该是根据对应客户的账期的天数来算
                chkIsExpenseType.Enabled = true;
                //entity.DueDate = System.DateTime.Now;
                if (string.IsNullOrEmpty(entity.ARAPNo))
                {
                    if (PaymentType == ReceivePaymentType.收款)
                    {
                        entity.ARAPNo = ClientBizCodeService.GetBizBillNo(BizType.应收款单);
                        chkIsForCommission.Visible = false;
                        chkIsFromPlatform.Visible = true;

                    }
                    else
                    {
                        entity.ARAPNo = ClientBizCodeService.GetBizBillNo(BizType.应付款单);
                        chkIsForCommission.Visible = true;
                        chkIsFromPlatform.Visible = false;
                    }
                }

                //entity.InvoiceDate = System.DateTime.Now;
                entity.ARAPStatus = (int)ARAPStatus.草稿;

                // 清空 DataSource（如果适用）
                cmbPayeeInfoID.DataSource = null;
                cmbPayeeInfoID.DataBindings.Clear();
                cmbPayeeInfoID.Items.Clear();

                // 根据付款类型设置控件可见性
                if (entity.ReceivePaymentType == (int)ReceivePaymentType.收款)
                {
                    cmbPayeeInfoID.Visible = false;
                    lblPayeeInfoID.Visible = false;
                    btnInfo.Visible = false;
                }
                else
                {
                    cmbPayeeInfoID.Visible = true;
                    lblPayeeInfoID.Visible = true;
                    btnInfo.Visible = true;
                }

            }
            if (entity.TotalLocalPayableAmount < 0)
            {
                picBox红字.Visible = true;
            }
            else
            {
                picBox红字.Visible = false;
            }

            DataBindingHelper.BindData4CheckBox<tb_FM_ReceivablePayable>(entity, t => t.IsExpenseType, chkIsExpenseType, false);

            DataBindingHelper.BindData4Cmb<tb_FM_Account>(entity, k => k.Account_id, v => v.Account_name, cmbAccount_id);
            DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.ARAPNo, txtARAPNo, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.ExchangeRate.ToString(), txtExchangeRate, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.TotalForeignPayableAmount.ToString(), txtTotalForeignPayableAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.TotalLocalPayableAmount.ToString(), txtTotalLocalPayableAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.ForeignPaidAmount.ToString(), txtForeignPaidAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.LocalPaidAmount.ToString(), txtLocalPaidAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.ForeignBalanceAmount.ToString(), txtForeignBalanceAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.LocalBalanceAmount.ToString(), txtLocalBalanceAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.LocalReconciledAmount.ToString(), txtLocalReconciledAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.ShippingFee.ToString(), txtShippingFee, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.PlatformOrderNo, txtPlatformOrderNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_FM_ReceivablePayable>(entity, t => t.IsForCommission, chkIsForCommission, false);
            DataBindingHelper.BindData4CheckBox<tb_FM_ReceivablePayable>(entity, t => t.IsFromPlatform, chkIsFromPlatform, false);
            // DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.SourceBillId, txtSourceBillId, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.SourceBillNo, txtSourceBillNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CmbByEnum<tb_FM_ReceivablePayable, BizType>(entity, k => k.SourceBizType, cmbBizType, false);
            DataBindingHelper.BindData4DataTime<tb_FM_ReceivablePayable>(entity, t => t.DueDate, dtpDueDate, false);
            DataBindingHelper.BindData4DataTime<tb_FM_ReceivablePayable>(entity, t => t.BusinessDate, dtpBusinessDate, false);
            DataBindingHelper.BindData4DataTime<tb_FM_ReceivablePayable>(entity, t => t.DocumentDate, dtpDocumentDate, false);



            DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);
            DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v => v.ProjectGroupName, cmbProjectGroup_ID);

            DataBindingHelper.BindData4CheckBox<tb_FM_ReceivablePayable>(entity, t => t.IsIncludeTax, chkIsIncludeTax, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.TaxTotalAmount.ToString(), txtTaxTotalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.UntaxedTotalAmont.ToString(), txtUntaxedTotalAmont, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v => v.CurrencyName, cmbCurrency_ID);

            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID, c => c.Is_enabled == true);
            if (cmbCurrency_ID.Items.Count > 1)
            {
                cmbCurrency_ID.SelectedIndex = 1;//默认第一个人民币
            }
            DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.Remark, txtRemark, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4CheckBox<tb_FM_ReceivablePayable>(entity, t => t.ApprovalResults, chkApprovalResults, false);
            DataBindingHelper.BindData4Cmb<tb_FM_PayeeInfo>(entity, k => k.PayeeInfoID, v => v.DisplayText, cmbPayeeInfoID, c => c.CustomerVendor_ID.HasValue && c.CustomerVendor_ID.Value == entity.CustomerVendor_ID);
            DataBindingHelper.BindData4ControlByEnum<tb_FM_ReceivablePayable>(entity, t => t.ARAPStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(ARAPStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_FM_ReceivablePayable>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));
            // 注意：ShowPrintStatus 已在基类 LoadDataToUI -> UpdateAllUIStates 中统一处理
            // 移除重复的打印状态更新逻辑

            LoadCustomerVendor(entity);

            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_FM_ReceivablePayableValidator>(), kryptonPanel1.Controls);


                #region 收款信息 ，并且可以添加
                // 使用统一方法加载收款信息
                LoadPayeeInfo(entity);
                #endregion


            }
            LoadItems(entity.IsExpenseType);
            InitLoadGrid();

            if (entity.tb_FM_ReceivablePayableDetails != null && entity.tb_FM_ReceivablePayableDetails.Count > 0)
            {
                details = entity.tb_FM_ReceivablePayableDetails;
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
            UIBizService.SynchronizeColumnOrder(sgd, listCols.Select(c => c.DisplayController).ToList());
            // 根据单据状态决定是否启用下拉控件
            var IsEdit = entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改 ||
                                  (entity.ARAPStatus == (int)ARAPStatus.草稿) || (entity.ARAPStatus == (int)ARAPStatus.待审核);


            //如果属性变化 则状态为修改
            entity.PropertyChanged += async (sender, s2) =>
            {
                //权限允许
                if ((true && entity.ARAPStatus == (int)ARAPStatus.草稿)
                || (true && entity.ARAPStatus == (int)ARAPStatus.待审核))
                {
                    EditEntity.ActionStatus = ActionStatus.修改;
                }




                //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
                if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
                {

                    if (PaymentType == ReceivePaymentType.付款 && s2.PropertyName == entity.GetPropertyName<tb_FM_ReceivablePayable>(c => c.IsForCommission))
                    {
                        LoadCustomerVendor(entity);
                    }

                    if (s2.PropertyName == entity.GetPropertyName<tb_FM_ReceivablePayable>(c => c.IsExpenseType))
                    {
                        LoadItems(entity.IsExpenseType);
                        UIBizService.SynchronizeColumnOrder(sgd, listCols.Select(c => c.DisplayController).ToList());
                    }

                    if (s2.PropertyName == entity.GetPropertyName<tb_FM_ReceivablePayable>(c => c.ShippingFee))
                    {
                        TotalSum();
                    }
                    if (s2.PropertyName == entity.GetPropertyName<tb_FM_ReceivablePayable>(c => c.TotalLocalPayableAmount))
                    {
                        TotalSum();
                    }

                    if (s2.PropertyName == entity.GetPropertyName<tb_FM_ReceivablePayable>(c => c.PayeeInfoID))
                    {
                        // 加载收款信息详情
                        await LoadPayeeInfoDetailAsync(entity);
                    }

                    //到期日期应该是根据对应客户的账期的天数来算
                    if (entity.CustomerVendor_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_FM_ReceivablePayable>(c => c.CustomerVendor_ID))
                    {
                        var obj = _cacheManager.GetEntity<tb_CustomerVendor>(entity.CustomerVendor_ID);
                        if (obj != null)
                        {
                            if (obj is tb_CustomerVendor cv)
                            {
                                if (entity.ReceivePaymentType == (int)ReceivePaymentType.收款 && cv.CustomerCreditDays.HasValue)
                                {
                                    entity.DueDate = System.DateTime.Now.AddDays(cv.CustomerCreditDays.Value);
                                }
                                if (entity.ReceivePaymentType == (int)ReceivePaymentType.付款 && cv.SupplierCreditDays.HasValue)
                                {
                                    entity.DueDate = System.DateTime.Now.AddDays(cv.SupplierCreditDays.Value);
                                }

                            }
                        }


                        //换往来单位了。对应的收款信息要重置 
                        entity.PayeeInfoID = null;
                        LoadPayeeInfo(entity);
                    }
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

                // 注意：ToolBarEnabledControl 已在基类 LoadDataToUI 中统一调用，移除重复调用
                // base.ToolBarEnabledControl(entity);

            };

            #region 应收单 不用显示 收款信息 ，付款时才要显示对方的信息。

            if (PaymentType == ReceivePaymentType.收款)
            {

                btnInfo.Visible = false;
                lblPayeeInfoID.Visible = false;
                cmbPayeeInfoID.Visible = false;

            }
            else
            {
                // 付款单时明确设置所有收款信息控件为可见
                btnInfo.Visible = true;
                lblPayeeInfoID.Visible = true;
                cmbPayeeInfoID.Visible = true;

                if (entity.tb_fm_payeeinfo != null)
                {
                    if (!string.IsNullOrEmpty(entity.tb_fm_payeeinfo.PaymentCodeImagePath))
                    {
                        btnInfo.Tag = entity.tb_fm_payeeinfo;
                    }
                    else
                    {
                        btnInfo.Tag = string.Empty;
                    }
                }
            }



            #endregion


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
        /// 根据是否为佣金加载往来单位
        /// 分客户和供应商
        /// </summary>
        /// <param name="entity"></param>
        private void LoadCustomerVendor(tb_FM_ReceivablePayable entity)
        {
            if (PaymentType == ReceivePaymentType.收款 || entity.IsForCommission)
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

                chkIsFromPlatform.Visible = true;
                txtPlatformOrderNo.Visible = true;

            }
            else
            {
                chkIsFromPlatform.Visible = false;
                txtPlatformOrderNo.Visible = false;
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
        }

        public void LoadItems(bool? IsExpenseTypeValue)
        {
            bool IsExpenseType = IsExpenseTypeValue.GetValueOrDefault();


            //产品
            //listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Location_ID);
            //listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Rack_ID);
            //listCols.FirstOrDefault(c => c.ColName == nameof(ProductSharePart.Location_ID)).Visible = false;
            //listCols.FirstOrDefault(c => c.ColName == nameof(ProductSharePart.Rack_ID)).Visible = false;



            listCols.SetCol_DefaultHide<ProductSharePart>(c => c.CNName, !IsExpenseType);
            listCols.SetCol_DefaultHide<ProductSharePart>(c => c.Location_ID, !IsExpenseType);
            listCols.SetCol_DefaultHide<ProductSharePart>(c => c.Rack_ID, !IsExpenseType);
            listCols.SetCol_DefaultHide<ProductSharePart>(c => c.prop, !IsExpenseType);
            listCols.SetCol_DefaultHide<ProductSharePart>(c => c.SKU, !IsExpenseType);
            listCols.SetCol_DefaultHide<ProductSharePart>(c => c.Specifications, !IsExpenseType);
            listCols.SetCol_DefaultHide<ProductSharePart>(c => c.Model, !IsExpenseType);
            listCols.SetCol_DefaultHide<ProductSharePart>(c => c.Unit_ID, !IsExpenseType);
            listCols.SetCol_DefaultHide<ProductSharePart>(c => c.ProductNo, !IsExpenseType);
            listCols.SetCol_DefaultHide<ProductSharePart>(c => c.Type_ID, !IsExpenseType);
            listCols.SetCol_DefaultHide<tb_FM_ReceivablePayableDetail>(c => c.property, !IsExpenseType);
            listCols.SetCol_DefaultHide<tb_FM_ReceivablePayableDetail>(c => c.CustomerPartNo, !IsExpenseType);

            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.CNName, IsExpenseType);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Location_ID, IsExpenseType);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Rack_ID, IsExpenseType);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.prop, IsExpenseType);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.SKU, IsExpenseType);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Specifications, IsExpenseType);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Model, IsExpenseType);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Unit_ID, IsExpenseType);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.ProductNo, IsExpenseType);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Type_ID, IsExpenseType);
            listCols.SetCol_NeverVisible<tb_FM_ReceivablePayableDetail>(c => c.property, IsExpenseType);
            listCols.SetCol_NeverVisible<tb_FM_ReceivablePayableDetail>(c => c.CustomerPartNo, IsExpenseType);


            listCols.FirstOrDefault(c => c.ColName == nameof(ProductSharePart.CNName)).Visible = !IsExpenseType;
            listCols.FirstOrDefault(c => c.ColName == nameof(ProductSharePart.Location_ID)).Visible = !IsExpenseType;
            listCols.FirstOrDefault(c => c.ColName == nameof(ProductSharePart.Rack_ID)).Visible = !IsExpenseType;
            listCols.FirstOrDefault(c => c.ColName == nameof(ProductSharePart.prop)).Visible = !IsExpenseType;
            listCols.FirstOrDefault(c => c.ColName == nameof(ProductSharePart.SKU)).Visible = !IsExpenseType;
            listCols.FirstOrDefault(c => c.ColName == nameof(ProductSharePart.Specifications)).Visible = !IsExpenseType;
            listCols.FirstOrDefault(c => c.ColName == nameof(ProductSharePart.Model)).Visible = !IsExpenseType;
            listCols.FirstOrDefault(c => c.ColName == nameof(ProductSharePart.Unit_ID)).Visible = !IsExpenseType;
            listCols.FirstOrDefault(c => c.ColName == nameof(ProductSharePart.ProductNo)).Visible = !IsExpenseType;
            listCols.FirstOrDefault(c => c.ColName == nameof(ProductSharePart.Type_ID)).Visible = !IsExpenseType;
            listCols.FirstOrDefault(c => c.ColName == nameof(tb_FM_ReceivablePayableDetail.property)).Visible = !IsExpenseType;
            listCols.FirstOrDefault(c => c.ColName == nameof(tb_FM_ReceivablePayableDetail.CustomerPartNo)).Visible = !IsExpenseType;

            listCols.SetCol_DefaultHide<tb_FM_ReceivablePayableDetail>(c => c.ExpenseType_id, !IsExpenseType);
            listCols.SetCol_DefaultHide<tb_FM_ReceivablePayableDetail>(c => c.ExpenseDescription, !IsExpenseType);

            listCols.SetCol_NeverVisible<tb_FM_ReceivablePayableDetail>(c => c.ExpenseType_id, !IsExpenseType);
            listCols.SetCol_NeverVisible<tb_FM_ReceivablePayableDetail>(c => c.ExpenseDescription, !IsExpenseType);



            listCols.FirstOrDefault(c => c.ColName == nameof(tb_FM_ReceivablePayableDetail.ExpenseType_id)).Visible = IsExpenseType;
            listCols.FirstOrDefault(c => c.ColName == nameof(tb_FM_ReceivablePayableDetail.ExpenseDescription)).Visible = IsExpenseType;

        }

        private void InitLoadGrid()
        {

            sgd = new SourceGridDefine(grid1, listCols, true);
            listCols.SetCol_Formula<tb_FM_ReceivablePayableDetail>((a, b) => a.UnitPrice * b.Quantity, c => c.LocalPayableAmount);//-->成交价是结果列

            sgd.GridMasterData = EditEntity;

            listCols.SetCol_Summary<tb_FM_ReceivablePayableDetail>(c => c.LocalPayableAmount);
            listCols.SetCol_Summary<tb_FM_ReceivablePayableDetail>(c => c.TaxLocalAmount);
            listCols.SetCol_Summary<tb_FM_ReceivablePayableDetail>(c => c.Quantity);
            listCols.SetCol_Formula<tb_FM_ReceivablePayableDetail>((a, b, c) => a.LocalPayableAmount / (1 + b.TaxRate) * c.TaxRate, d => d.TaxLocalAmount);


            //bool IsExpenseType = EditEntity.IsExpenseType.GetValueOrDefault();
            //公共到明细的映射 源 ，左边会隐藏
            sgh.SetPointToColumnPairs<ProductSharePart, tb_FM_ReceivablePayableDetail>(sgd, f => f.Standard_Price, t => t.UnitPrice);
            //sgh.SetPointToColumnPairs<ProductSharePart, tb_FM_ReceivablePayableDetail>(sgd, f => f.prop, t => t.property);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_FM_ReceivablePayableDetail>(sgd, f => f.Model, t => t.CustomerPartNo);

            //应该只提供一个结构
            List<tb_FM_ReceivablePayableDetail> lines = new List<tb_FM_ReceivablePayableDetail>();
            bindingSourceSub.DataSource = lines; //  ctrSub.Query(" 1>2 ");
            sgd.BindingSourceLines = bindingSourceSub;

            list = MainForm.Instance.View_ProdDetailList;
            sgd.SetDependencyObject<ProductSharePart, tb_FM_ReceivablePayableDetail>(list);

            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_FM_ReceivablePayableDetail));
        }


        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            base.QueryConditionBuilder();
            //创建表达式
            var lambda = Expressionable.Create<tb_FM_ReceivablePayable>()
                              //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
                              // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
                              .And(t => t.ReceivePaymentType == (int)PaymentType)
                             .And(t => t.isdeleted == false)
                            //报销人员限制，财务不限制 自己的只能查自己的
                            .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext), t => t.Created_by == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
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

        private void UCReceivablePayable_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }
            AddExtendButton(CurMenuInfo);
            #region
            switch (PaymentType)
            {
                case ReceivePaymentType.收款:
                    lblBillText.Text = "应收款单";
                    lblAccount_id.Text = "收款账号";
                    lblCustomerVendor_ID.Text = "应付单位";


                    btnInfo.Visible = false;
                    lblPayeeInfoID.Visible = false;
                    cmbPayeeInfoID.Visible = false;

                    break;
                case ReceivePaymentType.付款:
                    lblBillText.Text = "应付款单";
                    lblAccount_id.Text = "付款账号";
                    lblCustomerVendor_ID.Text = "应收单位";
                    // 付款单时明确设置收款信息控件为可见
                    btnInfo.Visible = true;
                    lblPayeeInfoID.Visible = true;
                    cmbPayeeInfoID.Visible = true;
                    break;
                default:
                    break;
            }

            #endregion

            if (CurMenuInfo != null)
            {
                lblBillText.Text = CurMenuInfo.CaptionCN;
            }
            InitDataTocmbbox();


            grid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;

            listCols = new List<SGDefineColumnItem>();
            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<ProductSharePart, tb_FM_ReceivablePayableDetail>(c => c.ProdDetailID, false);


            listCols.SetCol_NeverVisible<tb_FM_ReceivablePayableDetail>(c => c.ARAPDetailID);
            listCols.SetCol_NeverVisible<tb_FM_ReceivablePayableDetail>(c => c.ProdDetailID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Rack_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Location_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.ShortCode);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Brand);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Model);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.VendorModelCode);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Images);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Inv_Cost);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Standard_Price);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.TransPrice);
            //listCols.SetCol_NeverVisible<tb_FM_ReceivablePayableDetail>(c => c.ExpenseType_id);
            //listCols.SetCol_NeverVisible<tb_FM_ReceivablePayableDetail>(c => c.ExpenseDescription);

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

            InitLoadGrid();



            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnAddDataRow += Sgh_OnAddDataRow;
            UIHelper.ControlMasterColumnsInvisible(CurMenuInfo, this);


            //隐藏外币相关
            UIHelper.ControlForeignFieldInvisible<tb_FM_ReceivablePayable>(this, false);
            if (listCols != null)
            {
                listCols.SetCol_DefaultHide<tb_FM_ReceivablePayableDetail>(c => c.ExchangeRate);
            }
            UIBizService.SynchronizeColumnOrder(sgd, listCols.Select(c => c.DisplayController).ToList());
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

            if (!StateManager.IsFinalStatus(EditEntity))
            {
                MessageBox.Show($"当前单据,状态为【{(ARAPStatus)EditEntity.ARAPStatus}】不允许标记为坏账。");
                return;
            }
            //    // 使用V3状态管理系统检查是否可以标记坏账
            //    try
            //{
            //    if (UIController != null && StatusContext != null)
            //    {
            //        bool canWriteOffBadDebt = UIController.CanExecuteAction("坏账处理", StatusContext);
            //        if (!canWriteOffBadDebt)
            //        {
            //            MessageBox.Show($"当前单据状态为【{(ARAPStatus)EditEntity.ARAPStatus}】不允许标记为坏账。");
            //            return;
            //        }
            //    }
            //    else
            //    {
            //        // 回退到旧逻辑
            //        if (!FMPaymentStatusHelper.CanWriteOffBadDebt((ARAPStatus)EditEntity.ARAPStatus))
            //        {
            //            MessageBox.Show($"当前单据状态为【{(ARAPStatus)EditEntity.ARAPStatus}】不允许标记为坏账。");
            //            return;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    // 出错时回退到旧逻辑
            //    logger.LogError($"V3状态管理系统检查坏账处理权限失败: {ex.Message}");
            //    if (!FMPaymentStatusHelper.CanWriteOffBadDebt((ARAPStatus)EditEntity.ARAPStatus))
            //    {
            //        MessageBox.Show($"当前单据状态为【{(ARAPStatus)EditEntity.ARAPStatus}】不允许标记为坏账。");
            //        return;
            //    }
            //}


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
                    await MainForm.Instance.AuditLogHelper.CreateAuditLog<tb_FM_ReceivablePayable>("坏账处理", EditEntity, $"原因:{EditEntity.Remark}");
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
            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                TotalSum();
            }

        }


        void TotalSum()
        {
            try
            {

                //计算总金额  这些逻辑是不是放到业务层？后面要优化
                List<tb_FM_ReceivablePayableDetail> details = sgd.BindingSourceLines.DataSource as List<tb_FM_ReceivablePayableDetail>;
                details = details.Where(c => c.LocalPayableAmount != 0 || c.ProdDetailID.HasValue).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("金额必须大于0");
                    return;
                }
                EditEntity.TotalLocalPayableAmount = details.Sum(c => c.LocalPayableAmount) + EditEntity.ShippingFee;
                EditEntity.LocalBalanceAmount = EditEntity.TotalLocalPayableAmount - EditEntity.LocalPaidAmount;
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
                                        UIConfigManager configManager = Startup.GetFromFac<UIConfigManager>();
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
            if (bindingSourceSub.DataSource == null)
            {
                return false;
            }

            var eer = errorProviderForAllInput.GetError(txtLocalBalanceAmount);
            bindingSourceSub.EndEdit();
            List<tb_FM_ReceivablePayableDetail> detailentity = bindingSourceSub.DataSource as List<tb_FM_ReceivablePayableDetail>;
            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值,这里样品 可能金额为0.也显示。方便对账
                details = detailentity.Where(t => t.LocalPayableAmount != 0 || t.ProdDetailID.HasValue).ToList();
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
                    tb_FM_Account bizcatch = _cacheManager.GetEntity<tb_FM_Account>(EditEntity.Account_id);
                    if (bizcatch != null && bizcatch.Currency_ID != EditEntity.Currency_ID)
                    {
                        MessageBox.Show("收付款账号中的币别与当前单据的币别不一致。");
                        return false;
                    }
                }


                //如果主表的总金额和明细金额加总后不相等，则提示
                if (NeedValidated && EditEntity.TotalLocalPayableAmount != details.Sum(c => c.LocalPayableAmount) + EditEntity.ShippingFee)
                {
                    if (MessageBox.Show("总金额和明细金额总计不相等，你确定要保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.No)
                    {
                        return false;
                    }
                }


                if (NeedValidated && EditEntity.TaxTotalAmount != details.Sum(c => c.TaxLocalAmount))
                {
                    if (MessageBox.Show("税额总计和明细税额不相等，你确定要保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.No)
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
                UIConfigManager configManager = Startup.GetFromFac<UIConfigManager>();
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
                            MessageBox.Show("只有创建人或超级管理员才能删除提交的单据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            rss.ErrorMsg = "只有创建人或超级管理员才能删除提交的单据。";
                            rss.Succeeded = false;
                            return rss;
                        }
                    }

                    tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable> ctr = Startup.GetFromFac<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
                    //bool rs = await ctr.BaseLogicDeleteAsync(EditEntity as tb_FM_ReceivablePayable);
                    bool rs = await ctr.BaseDeleteByNavAsync(EditEntity as tb_FM_ReceivablePayable);
                    if (rs)
                    {
                        MainForm.Instance.FMAuditLogHelper.CreateAuditLog<tb_FM_ReceivablePayable>("删除", EditEntity);
                        

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