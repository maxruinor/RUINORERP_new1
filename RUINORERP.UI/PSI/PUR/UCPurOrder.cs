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
using RUINORERP.Business.AutoMapper;
using AutoMapper;
using RUINORERP.Business.Processor;
using RUINORERP.Model.CommonModel;
using RUINORERP.Business.CommService;
using ZXing.Common;
using RUINORERP.Business.Security;
using RUINORERP.Global.EnumExt;
using RUINORERP.UI.AdvancedUIModule;
using NPOI.SS.Formula.Functions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using Krypton.Toolkit;
using Fireasy.Common.Extensions;
using RUINORERP.Global.Model;
using RUINORERP.UI.BI;
using RUINORERP.UI.WorkFlowDesigner.Entities;

namespace RUINORERP.UI.PSI.PUR
{
    [MenuAttrAssemblyInfo("采购订单", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.采购管理, BizType.采购订单)]
    public partial class UCPurOrder : BaseBillEditGeneric<tb_PurOrder, tb_PurOrderDetail>, IPublicEntityObject
    {
        public UCPurOrder()
        {
            InitializeComponent();
            AddPublicEntityObject(typeof(ProductSharePart));
        }


        internal override void LoadDataToUI(object Entity)
        {
            ActionStatus actionStatus = ActionStatus.无操作;
            BindData(Entity as tb_PurOrder, actionStatus);
        }
        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {

            base.QueryConditionBuilder();
            var lambda = Expressionable.Create<tb_PurOrder>()
            .AndIF(AuthorizeController.GetPurBizLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
            .ToExpression();
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }

        /// <summary>
        /// 加载下拉值
        /// </summary>
        public void InitDataTocmbbox()
        {
            lblPrintStatus.Text = "";
            lblReview.Text = "";
            DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.InitDataToCmb<tb_CustomerVendor>(k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, c => c.IsVendor == true);
            DataBindingHelper.InitDataToCmb<tb_Department>(k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);
            DataBindingHelper.InitDataToCmb<tb_PaymentMethod>(k => k.Paytype_ID, v => v.Paytype_Name, cmbPaytype_ID);
        }
        protected override async Task LoadRelatedDataToDropDownItemsAsync()
        {
            if (base.EditEntity is tb_PurOrder purOrder)
            {
                if (purOrder.SOrder_ID.HasValue)
                {
                    RelatedQueryParameter rqp = new RelatedQueryParameter();
                    rqp.bizType = BizType.销售订单;
                    rqp.billId = purOrder.SOrder_ID.Value;
                    ToolStripMenuItem RelatedMenuItem = new ToolStripMenuItem();
                    RelatedMenuItem.Name = $"{rqp.billId}";
                    RelatedMenuItem.Tag = rqp;
                    RelatedMenuItem.Text = $"{rqp.bizType}:{purOrder.SOrderNo}";
                    RelatedMenuItem.Click += base.MenuItem_Click;
                    if (!toolStripbtnRelatedQuery.DropDownItems.ContainsKey(purOrder.SOrder_ID.Value.ToString()))
                    {
                        toolStripbtnRelatedQuery.DropDownItems.Add(RelatedMenuItem);
                    }
                }

                if (purOrder.tb_PurEntries != null && purOrder.tb_PurEntries.Count > 0)
                {
                    foreach (var item in purOrder.tb_PurEntries)
                    {
                        RelatedQueryParameter rqp = new RelatedQueryParameter();
                        rqp.bizType = BizType.采购入库单;
                        rqp.billId = item.PurEntryID;
                        ToolStripMenuItem RelatedMenuItem = new ToolStripMenuItem();
                        RelatedMenuItem.Name = $"{rqp.billId}";
                        RelatedMenuItem.Tag = rqp;
                        RelatedMenuItem.Text = $"{rqp.bizType}:{item.PurEntryNo}";
                        RelatedMenuItem.Click += base.MenuItem_Click;
                        if (!toolStripbtnRelatedQuery.DropDownItems.ContainsKey(item.PurEntryID.ToString()))
                        {
                            toolStripbtnRelatedQuery.DropDownItems.Add(RelatedMenuItem);
                        }
                    }
                }
                if (purOrder.Deposit > 0)
                {
                    var PreReceivedPayments = await MainForm.Instance.AppContext.Db.Queryable<tb_FM_PreReceivedPayment>()
                                                                    .Where(c => c.PrePaymentStatus >= (int)PrePaymentStatus.待审核
                                                                    && c.CustomerVendor_ID == purOrder.CustomerVendor_ID
                                                                    && c.SourceBillId == purOrder.PurOrder_ID)
                                                                    .ToListAsync();
                    foreach (var item in PreReceivedPayments)
                    {
                        var rqp = new Model.CommonModel.RelatedQueryParameter();
                        rqp.bizType = BizType.预付款单;
                        rqp.billId = item.PreRPID;
                        ToolStripMenuItem RelatedMenuItem = new ToolStripMenuItem();
                        RelatedMenuItem.Name = $"{rqp.billId}";
                        RelatedMenuItem.Tag = rqp;
                        RelatedMenuItem.Text = $"{rqp.bizType}:{item.PreRPNO}";
                        RelatedMenuItem.Click += base.MenuItem_Click;
                        if (!toolStripbtnRelatedQuery.DropDownItems.ContainsKey(item.PreRPID.ToString()))
                        {
                            toolStripbtnRelatedQuery.DropDownItems.Add(RelatedMenuItem);
                        }
                    }
                }
            }
            await base.LoadRelatedDataToDropDownItemsAsync();
        }
        public override void BindData(tb_PurOrder entity, ActionStatus actionStatus)
        {
            if (entity == null)
            {

                return;
            }
            EditEntity = entity;
            if (entity.PurOrder_ID > 0)
            {
                entity.PrimaryKeyID = entity.PurOrder_ID;
                entity.ActionStatus = ActionStatus.加载;
                if (entity.Currency_ID != AppContext.BaseCurrency.Currency_ID)
                {
                    lblExchangeRate.Visible = true;
                    txtExchangeRate.Visible = true;
                    UIHelper.ControlForeignFieldInvisible<tb_PurOrder>(this, true);

                }
                else
                {
                    lblExchangeRate.Visible = false;
                    txtExchangeRate.Visible = false;
                    UIHelper.ControlForeignFieldInvisible<tb_PurOrder>(this, false);
                }

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

                    DataBindingHelper.BindData4Cmb<tb_FM_PayeeInfo>(entity, k => k.PayeeInfoID, v => v.DisplayText, cmbPayeeInfoID, queryFilterPayeeInfo.GetFilterExpression<tb_FM_PayeeInfo>(), true);
                    DataBindingHelper.InitFilterForControlByExpCanEdit<tb_FM_PayeeInfo>(entity, cmbPayeeInfoID, c => c.DisplayText, queryFilterPayeeInfo, true);


                    #endregion
                }
                else
                {
                    //清空
                    cmbPayeeInfoID.DataBindings.Clear();
                }

            }
            else
            {
                entity.ActionStatus = ActionStatus.新增;
                entity.DataStatus = (int)DataStatus.草稿;
                if (string.IsNullOrEmpty(entity.PurOrderNo))
                {
                    entity.PurOrderNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.采购订单);
                }
                entity.CloseCaseOpinions = string.Empty;
                entity.ApprovalOpinions = string.Empty;
                entity.PurDate = System.DateTime.Now;
                entity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                if (entity.tb_PurOrderDetails != null && entity.tb_PurOrderDetails.Count > 0)
                {
                    entity.tb_PurOrderDetails.ForEach(c => c.PurOrder_ID = 0);
                    entity.tb_PurOrderDetails.ForEach(c => c.PurOrder_ChildID = 0);
                }
                if (AppContext.BaseCurrency != null)
                {
                    entity.Currency_ID = AppContext.BaseCurrency.Currency_ID;
                }

                lblExchangeRate.Visible = false;
                txtExchangeRate.Visible = false;
                BusinessHelper.Instance.InitEntity(entity);

                UIHelper.ControlForeignFieldInvisible<tb_PurOrder>(this, false);
            }


            if (entity.DataStatus >= (int)DataStatus.确认)
            {
                DataBindingHelper.BindData4CmbByEnum<tb_PurOrder, PayStatus>(entity, k => k.PayStatus, cmbPayStatus, false);
            }
            else
            {
                DataBindingHelper.BindData4CmbByEnum<tb_PurOrder, PayStatus>(entity, k => k.PayStatus, cmbPayStatus, false, PayStatus.全部付款, PayStatus.部分付款);
            }



            //EnumBindingHelper bindingHelper = new EnumBindingHelper();
            ////https://www.cnblogs.com/cdaniu/p/15236857.html
            ////加载枚举，并且可以过虑不需要的项 , 订单不需要用全部付款。只有在财务模块中 确认收货后。才是全部付款
            //List<int> exclude = new List<int>();
            //exclude.Add((int)PayStatus.全部付款);
            //bindingHelper.InitDataToCmbByEnumOnWhere<tb_PurOrder>(typeof(PayStatus).GetListByEnum<PayStatus>(null, exclude.ToArray()), e => e.PayStatus, cmbPayStatus);

            DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, c => c.IsVendor == true);
            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);
            DataBindingHelper.BindData4Cmb<tb_PaymentMethod>(entity, k => k.Paytype_ID, v => v.Paytype_Name, cmbPaytype_ID);
            DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v => v.CurrencyName, cmbCurrency_ID);
            DataBindingHelper.BindData4CheckBox<tb_PurOrder>(entity, t => t.IsCustomizedOrder, chkIsCustomizedOrder, false);
            //不是业务，不用指定组
            //if (AppContext.projectGroups != null && AppContext.projectGroups.Count > 0)
            //{
            //    #region 项目组
            //    cmbProjectGroup.DataSource = null;
            //    cmbProjectGroup.DataBindings.Clear();
            //    BindingSource bs = new BindingSource();
            //    bs.DataSource = AppContext.projectGroups;
            //    ComboBoxHelper.InitDropList(bs, cmbProjectGroup, "ProjectGroup_ID", "ProjectGroupName", ComboBoxStyle.DropDownList, false);
            //    var depa = new Binding("SelectedValue", entity, "ProjectGroup_ID", true, DataSourceUpdateMode.OnValidation);
            //    //数据源的数据类型转换为控件要求的数据类型。
            //    depa.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //    //将控件的数据类型转换为数据源要求的数据类型。
            //    depa.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //    cmbProjectGroup.DataBindings.Add(depa);
            //    #endregion
            //}
            //else
            //{
            DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v => v.ProjectGroupName, cmbProjectGroup);
            //}


            DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.ExchangeRate.ToString(), txtExchangeRate, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.PurOrderNo, txtPurOrderNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4DataTime<tb_PurOrder>(entity, t => t.PurDate, dtpPurDate, false);
            DataBindingHelper.BindData4DataTime<tb_PurOrder>(entity, t => t.PreDeliveryDate, dtpPreDeliveryDate, false);
            DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.ShipCost.ToString(), txtShipCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.TotalUntaxedAmount.ToString(), txtTotalUntaxedAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.TotalTaxAmount.ToString(), txtTotalTaxAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.TotalQty.ToString(), txtTotalQty, BindDataType4TextBox.Qty, false);

            //到货日期 是入库单的时间写回 逻辑后面再定
            //            DataBindingHelper.BindData4DataTime<tb_PurOrder>(entity, t => t.Arrival_date, dtpArrival_date, false);
            DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            //DataBindingHelper.BindData4CheckBox<tb_PurOrder>(entity, t => t.IsIncludeTax, chkIsIncludeTax, false);
            DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.ForeignTotalAmount.ToString(), txtForeignTotalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.ForeignDeposit.ToString(), txtForeignDeposit, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.Deposit.ToString(), txtDeposit, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.SOrderNo, txtSorderNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4ControlByEnum<tb_PurOrder>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_PurOrder>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));

            //显示 打印状态 如果是草稿状态 不显示打印
            ShowPrintStatus(lblPrintStatus, EditEntity);

            if (entity.tb_PurOrderDetails != null && entity.tb_PurOrderDetails.Count > 0)
            {
                sgh.LoadItemDataToGrid<tb_PurOrderDetail>(grid1, sgd, entity.tb_PurOrderDetails, c => c.ProdDetailID);
            }
            else
            {
                sgh.LoadItemDataToGrid<tb_PurOrderDetail>(grid1, sgd, new List<tb_PurOrderDetail>(), c => c.ProdDetailID);
            }
            DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, v => v.SOrderNo, txtSorderNo, BindDataType4TextBox.Text, true);
            #region  引用销售订单转为采购订单
            //创建表达式  草稿 结案 和没有提交的都不显示
            BaseProcessor basePro = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_SaleOrder).Name + "Processor");
            QueryFilter queryFilter = basePro.GetQueryFilter();

            var lambdaSaleOut = Expressionable.Create<tb_SaleOrder>()
         .And(t => t.DataStatus == (int)DataStatus.确认)
         .And(t => t.ApprovalStatus.HasValue && t.ApprovalStatus.Value == (int)ApprovalStatus.已审核)
         .And(t => t.ApprovalResults.HasValue && t.ApprovalResults.Value == true)
          .And(t => t.isdeleted == false)
         .ToExpression();
            queryFilter.SetFieldLimitCondition(lambdaSaleOut);
            ControlBindingHelper.ConfigureControlFilter<tb_PurOrder, tb_SaleOrder>(entity, txtSorderNo, t => t.SOrderNo,
                f => f.SOrderNo, queryFilter, a => a.SOrder_ID, b => b.SOrder_ID, null, false);

            #endregion

            //如果属性变化 则状态为修改
            entity.PropertyChanged += async (sender, s2) =>
            {
                if (s2.PropertyName == entity.GetPropertyName<tb_PurOrder>(c => c.CustomerVendor_ID))
                {
                    #region 收款信息可以根据往来单位带出 ，并且可以添加

                    //创建表达式
                    var lambdaPayeeInfo = Expressionable.Create<tb_FM_PayeeInfo>()
                                .And(t => t.Is_enabled == true)
                                .And(t => t.CustomerVendor_ID == entity.CustomerVendor_ID)
                                .ToExpression();//注意 这一句 不能少
                    BaseProcessor baseProcessorPayeeInfo = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_FM_PayeeInfo).Name + "Processor");
                    QueryFilter queryFilterPayeeInfo = baseProcessorPayeeInfo.GetQueryFilter();
                    queryFilterPayeeInfo.FilterLimitExpressions.Add(lambdaPayeeInfo);

                    DataBindingHelper.BindData4Cmb<tb_FM_PayeeInfo>(entity, k => k.PayeeInfoID, v => v.DisplayText, cmbPayeeInfoID, queryFilterPayeeInfo.GetFilterExpression<tb_FM_PayeeInfo>(), true);


                    DataBindingHelper.InitFilterForControlByExpCanEdit<tb_FM_PayeeInfo>(entity, cmbPayeeInfoID, c => c.DisplayText, queryFilterPayeeInfo, true);


                    #endregion

                    if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
                    {
                        if ((entity.PayStatus == (int)PayStatus.全额预付 || entity.PayStatus == (int)PayStatus.部分预付))
                        {
                            //设置一个默认值 如果有默认收款账号时
                            //如果原来的值，在下拉集合中，则不变。说明是对应厂商的收款信息。如果不是，则看有不有设置默认值
                            var payeeInfoList = cmbPayeeInfoID.Items.CastToList<tb_FM_PayeeInfo>().Where(c => c.PayeeInfoID != -1).ToList();
                            if (!payeeInfoList.Any(c => c.PayeeInfoID == entity.PayeeInfoID))
                            {
                                if (payeeInfoList.FirstOrDefault(c => c.IsDefault) != null)
                                {
                                    entity.PayeeInfoID = payeeInfoList.FirstOrDefault(c => c.IsDefault).PayeeInfoID;
                                    btnInfo.Tag = payeeInfoList.FirstOrDefault(c => c.IsDefault);
                                }
                            }
                        }

                        if (s2.PropertyName == entity.GetPropertyName<tb_PurOrder>(c => c.PayStatus) || s2.PropertyName == entity.GetPropertyName<tb_PurOrder>(c => c.Paytype_ID))
                        {
                            //如果全额预付 自动设置为订金额就是全款
                            Summation();

                        }


                    }
                }



                //如果是销售订单引入变化则加载明细及相关数据
                if ((entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
                && entity.SOrder_ID.HasValue && entity.SOrder_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_PurOrder>(c => c.SOrder_ID))
                {
                    await OrderToOutBill(entity.SOrder_ID.Value);
                }

                if (s2.PropertyName == entity.GetPropertyName<tb_PurOrder>(c => c.PayStatus) && entity.PayStatus == (int)PayStatus.未付款)
                {
                    //默认为账期
                    entity.Paytype_ID = MainForm.Instance.AppContext.PaymentMethodOfPeriod.Paytype_ID;
                }
                if (s2.PropertyName == entity.GetPropertyName<tb_PurOrder>(c => c.Paytype_ID) && entity.Paytype_ID == MainForm.Instance.AppContext.PaymentMethodOfPeriod.Paytype_ID)
                {
                    //默认为未付款
                    entity.PayStatus = (int)PayStatus.未付款;
                }


                //权限允许
                if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
                {

                    if ((entity.PayStatus == (int)PayStatus.全额预付 || entity.PayStatus == (int)PayStatus.部分预付) && s2.PropertyName == entity.GetPropertyName<tb_FM_PayeeInfo>(c => c.PayeeInfoID))
                    {
                        cmbPayeeInfoID.Enabled = true;
                        //加载收款信息
                        if (entity.PayeeInfoID > 0)
                        {
                            tb_FM_PayeeInfo payeeInfo = null;
                            var obj = BizCacheHelper.Instance.GetEntity<tb_FM_PayeeInfo>(entity.PayeeInfoID);
                            if (obj != null && obj.ToString() != "System.Object")
                            {
                                if (obj is tb_FM_PayeeInfo cv)
                                {
                                    payeeInfo = cv;
                                }
                            }
                            else
                            {
                                //直接加载 不用缓存
                                payeeInfo = await MainForm.Instance.AppContext.Db.Queryable<tb_FM_PayeeInfo>().Where(c => c.PayeeInfoID == entity.PayeeInfoID).FirstAsync();
                            }
                            if (payeeInfo != null)
                            {
                                btnInfo.Tag = payeeInfo;

                                //DataBindingHelper.BindData4CmbByEnum<tb_FM_PayeeInfo>(payeeInfo, k => k.Account_type, typeof(AccountType), cmbAccount_type, false);
                                //添加收款信息。展示给财务看
                                //entity.PayeeAccountNo = payeeInfo.Account_No;
                                //lblBelongingBank.Text = payeeInfo.BelongingBank;
                                //lblOpeningbank.Text = payeeInfo.OpeningBank;
                                //cmbAccount_type.SelectedItem = payeeInfo.Account_type;
                                //if (!string.IsNullOrEmpty(payeeInfo.PaymentCodeImagePath))
                                //{
                                //    btnInfo.Tag = payeeInfo;
                                //    btnInfo.Visible = true;
                                //}
                                //else
                                //{
                                //    btnInfo.Tag = string.Empty;
                                //    btnInfo.Visible = false;
                                //}
                            }
                        }
                        else
                        {
                            //entity.PayeeAccountNo = string.Empty;
                            //txtPayeeAccountNo.Text = "";
                            //cmbAccount_type.SelectedIndex = -1;
                            //lblBelongingBank.Text = "";
                            //lblOpeningbank.Text = "";
                        }
                    }


                    //根据币别如果是外币才显示外币相关的字段
                    if (s2.PropertyName == entity.GetPropertyName<tb_PurOrder>(c => c.Currency_ID) && entity.Currency_ID > 0)
                    {
                        if (cmbCurrency_ID.SelectedItem is tb_Currency cv)
                        {
                            if (cv.CurrencyCode.Trim() != DefaultCurrency.RMB.ToString())
                            {
                                //显示外币相关
                                UIHelper.ControlForeignFieldInvisible<tb_PurOrder>(this, true);
                                entity.ExchangeRate = BizService.GetExchangeRateFromCache(cv.Currency_ID, AppContext.BaseCurrency.Currency_ID);
                                if (EditEntity.Currency_ID != AppContext.BaseCurrency.Currency_ID)
                                {
                                    EditEntity.ForeignTotalAmount = EditEntity.TotalAmount / EditEntity.ExchangeRate;
                                    //
                                    EditEntity.ForeignTotalAmount = Math.Round(EditEntity.ForeignTotalAmount, MainForm.Instance.authorizeController.GetMoneyDataPrecision()); // 四舍五入到 2 位小数
                                }
                                lblExchangeRate.Visible = true;
                                txtExchangeRate.Visible = true;
                                lblForeignTotalAmount.Text = $"金额({cv.CurrencyCode})";
                            }
                            else
                            {
                                //隐藏外币相关
                                UIHelper.ControlForeignFieldInvisible<tb_PurOrder>(this, false);
                                lblExchangeRate.Visible = false;
                                txtExchangeRate.Visible = false;
                                entity.ExchangeRate = 1;
                                entity.ForeignTotalAmount = 0;
                            }
                        }

                    }

                    if (s2.PropertyName == entity.GetPropertyName<tb_PurOrder>(c => c.Paytype_ID) && entity.Paytype_ID.HasValue && entity.Paytype_ID > 0)
                    {
                        if (cmbPaytype_ID.SelectedItem is tb_PaymentMethod paymentMethod)
                        {
                            EditEntity.tb_paymentmethod = paymentMethod;
                        }
                    }

                    if (s2.PropertyName == entity.GetPropertyName<tb_PurOrder>(c => c.ProjectGroup_ID) && entity.ProjectGroup_ID.HasValue && entity.ProjectGroup_ID > 0)
                    {
                        if (cmbProjectGroup.SelectedItem is tb_ProjectGroup ProjectGroup)
                        {
                            EditEntity.tb_projectgroup = ProjectGroup;
                        }
                    }

                    if (s2.PropertyName == entity.GetPropertyName<tb_PurOrder>(c => c.ShipCost))
                    {
                        Summation();
                    }

                }

                if (s2.PropertyName == entity.GetPropertyName<tb_PurOrder>(c => c.PreDeliveryDate))
                {
                    if (EditEntity.PreDeliveryDate.HasValue)
                    {
                        PreDeliveryDate = EditEntity.PreDeliveryDate.Value;
                        //预交日期来自于主表
                        listCols.SetCol_DefaultValue<tb_PurOrderDetail>(c => c.PreDeliveryDate, PreDeliveryDate);
                        if (entity.tb_PurOrderDetails != null)
                        {
                            entity.tb_PurOrderDetails.ForEach(c => c.PreDeliveryDate = PreDeliveryDate);
                            sgh.SetCellValue<tb_PurOrderDetail>(sgd, colNameExp => colNameExp.PreDeliveryDate, PreDeliveryDate);
                        }
                    }
                }
              

                //如果客户有变化，带出对应有业务员
                if (entity.CustomerVendor_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_PurOrder>(c => c.CustomerVendor_ID))
                {
                    var obj = BizCacheHelper.Instance.GetEntity<tb_CustomerVendor>(entity.CustomerVendor_ID);
                    if (obj != null && obj.ToString() != "System.Object")
                    {
                        if (obj is tb_CustomerVendor cv)
                        {
                            if (cv.Employee_ID.HasValue)
                            {
                                EditEntity.Employee_ID = cv.Employee_ID.Value;
                            }
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

            ShowPrintStatus(lblPrintStatus, EditEntity);

            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_PurOrderValidator>(), kryptonSplitContainer1.Panel1.Controls);
                //  base.InitEditItemToControl(entity, kryptonPanel1.Controls);
            }

            //创建表达式
            var lambda = Expressionable.Create<tb_CustomerVendor>()
                            .And(t => t.IsVendor == true)
                            .ToExpression();//注意 这一句 不能少
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CustomerVendor).Name + "Processor");
            QueryFilter queryFilterC = baseProcessor.GetQueryFilter();
            queryFilterC.FilterLimitExpressions.Add(lambda);
            DataBindingHelper.InitFilterForControlByExp<tb_CustomerVendor>(entity, cmbCustomerVendor_ID, c => c.CVName, queryFilterC);


            base.BindData(entity);
        }

     

        SourceGridDefine sgd = null;
        SourceGridHelper sgh = new SourceGridHelper();

        DateTime PreDeliveryDate = System.DateTime.Now;

        List<SGDefineColumnItem> listCols = new List<SGDefineColumnItem>();

        private void UCStockIn_Load(object sender, EventArgs e)
        {
            InitDataTocmbbox();
            base.ToolBarEnabledControl(MenuItemEnums.刷新);

            grid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;

            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<ProductSharePart, tb_PurOrderDetail, InventoryInfo>(c => c.ProdDetailID, false);

            listCols.SetCol_NeverVisible<tb_PurOrderDetail>(c => c.ProdDetailID);
            listCols.SetCol_NeverVisible<tb_PurOrderDetail>(c => c.PurOrder_ChildID);
            listCols.SetCol_NeverVisible<tb_PurOrderDetail>(c => c.PurOrder_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Inv_Cost);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Standard_Price);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Rack_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.TransPrice);

            //保存时自动判断的
            listCols.SetCol_NeverVisible<tb_PurOrderDetail>(c => c.IncludingTax);
            if (!AppContext.SysConfig.UseBarCode)
            {
                listCols.SetCol_NeverVisible<ProductSharePart>(c => c.BarCode);
            }
            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Unit_ID);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Brand);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.prop);
            listCols.SetCol_ReadOnly<ProductSharePart>(c => c.CNName);
            listCols.SetCol_ReadOnly<tb_PurOrderDetail>(c => c.IncludingTax);
            listCols.SetCol_ReadOnly<tb_PurOrderDetail>(c => c.DeliveredQuantity);
            listCols.SetCol_ReadOnly<tb_PurOrderDetail>(c => c.TotalReturnedQty);
            listCols.SetCol_ReadOnly<tb_PurOrderDetail>(c => c.TaxAmount);
            //listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Location_ID);


            //https://blog.csdn.net/m0_46426259/article/details/120265783  格式化

            listCols.SetCol_Format<tb_PurOrderDetail>(c => c.TaxRate, CustomFormatType.PercentFormat);
            listCols.SetCol_Format<tb_PurOrderDetail>(c => c.UnitPrice, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_PurOrderDetail>(c => c.CustomizedCost, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_PurOrderDetail>(c => c.SubtotalAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_PurOrderDetail>(c => c.TaxAmount, CustomFormatType.CurrencyFormat);

            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridMasterData = EditEntity;
             

            //设置总计列
            BaseProcessor baseProcessor = BusinessHelper._appContext.GetRequiredServiceByName<BaseProcessor>(typeof(tb_PurOrderDetail).Name + "Processor");
            var summaryCols = baseProcessor.GetSummaryCols();
            foreach (var item in summaryCols)
            {
                foreach (var col in listCols)
                {
                    col.SetCol_Summary<tb_PurOrderDetail>(item);
                }
            }
            listCols.SetCol_Summary<tb_PurOrderDetail>(c => c.SubtotalUntaxedAmount);

            listCols.SetCol_Formula<tb_PurOrderDetail>((a, b, c) => (a.CustomizedCost + a.UnitPrice) * c.Quantity, c => c.SubtotalAmount);
            listCols.SetCol_Formula<tb_PurOrderDetail>((a, b, c) => a.SubtotalAmount / (1 + b.TaxRate) * c.TaxRate, d => d.TaxAmount);
            listCols.SetCol_Formula<tb_PurOrderDetail>((a, b, c) => a.UnitPrice / (1 + b.TaxRate), d => d.UntaxedUnitPrice);
            listCols.SetCol_Formula<tb_PurOrderDetail>((a, b, c) => a.CustomizedCost / (1 + b.TaxRate), d => d.UntaxedCustomizedCost);
            listCols.SetCol_Formula<tb_PurOrderDetail>((a, b, c) => (a.UntaxedCustomizedCost + a.UntaxedUnitPrice) * c.Quantity, c => c.SubtotalUntaxedAmount);


            listCols.SetCol_FormulaReverse<tb_PurOrderDetail>(d => d.UnitPrice == 0, (a, b) => a.SubtotalAmount / b.Quantity, c => c.UnitPrice);//-->成交价是结果列
            sgh.SetPointToColumnPairs<ProductSharePart, tb_PurOrderDetail>(sgd, f => f.Location_ID, t => t.Location_ID);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_PurOrderDetail>(sgd, f => f.prop, t => t.property);

            sgh.SetPointToColumnPairs<ProductSharePart, tb_PurOrderDetail>(sgd, f => f.VendorModelCode, t => t.VendorModelCode);

            //新建时默认数量就是未交数量，入库时对应减少, 但是 数量不隐藏
            // sgh.SetPointToColumnPairs<tb_PurOrderDetail, tb_PurOrderDetail>(sgd, f => f.Quantity, t => t.UndeliveredQty, false);
            // listCols.SetCol_RelatedValue<tb_PurOrderDetail>(a => a.Quantity, b => b.UndeliveredQty, "{0}", c => c.Quantity);
            listCols.SetCol_Formula<tb_PurOrderDetail>((a, b) => a.Quantity - b.DeliveredQuantity, c => c.UndeliveredQty);

            //应该只提供一个结构
            List<tb_PurOrderDetail> lines = new List<tb_PurOrderDetail>();
            bindingSourceSub.DataSource = lines; //  ctrSub.Query(" 1>2 ");
            sgd.BindingSourceLines = bindingSourceSub;
            sgd.SetDependencyObject<ProductSharePart, tb_PurOrderDetail>(MainForm.Instance.View_ProdDetailList);

            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_PurOrderDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnLoadMultiRowData += Sgh_OnLoadMultiRowData;
            sgh.OnLoadRelevantFields += Sgh_OnLoadRelevantFields;
            UIHelper.ControlMasterColumnsInvisible(CurMenuInfo, this);
            UIHelper.ControlForeignFieldInvisible<tb_PurOrder>(this, false);
            lblExchangeRate.Visible = false;
            txtExchangeRate.Visible = false;
        }

        /// <summary>
        /// 这里实现的是价格历史自动给值
        /// </summary>
        /// <param name="_View_ProdDetail"></param>
        /// <param name="rowObj"></param>
        /// <param name="griddefine"></param>
        /// <param name="Position"></param>
        private void Sgh_OnLoadRelevantFields(object _View_ProdDetail, object rowObj, SourceGridDefine griddefine, Position Position)
        {
            if (EditEntity == null)
            {
                return;
            }
            View_ProdDetail vp = (View_ProdDetail)_View_ProdDetail;
            tb_PurOrderDetail _SDetail = (tb_PurOrderDetail)rowObj;
            //通过产品查询页查出来后引过来才有值，如果直接在输入框输入SKU这种唯一的。就没有则要查一次。这时是缓存了？
            if (vp.ProdDetailID > 0 && EditEntity.Employee_ID > 0)
            {
                tb_PriceRecord pr = MainForm.Instance.AppContext.Db.Queryable<tb_PriceRecord>().Where(a => a.Employee_ID == EditEntity.Employee_ID && a.ProdDetailID == vp.ProdDetailID).Single();
                if (pr != null && _SDetail != null)
                {
                    _SDetail.UnitPrice = pr.PurPrice;
                    var Col = griddefine.grid.Columns.GetColumnInfo(griddefine.DefineColumns.FirstOrDefault(c => c.ColName == nameof(tb_PurOrderDetail.UnitPrice)).UniqueId);
                    if (Col != null)
                    {
                        griddefine.grid[Position.Row, Col.Index].Value = _SDetail.UnitPrice;
                    }

                }
            }
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
                List<tb_PurOrderDetail> details = new List<tb_PurOrderDetail>();

                foreach (var item in RowDetails)
                {
                    tb_PurOrderDetail bOM_SDetail = MainForm.Instance.mapper.Map<tb_PurOrderDetail>(item);
                    if (EditEntity.PreDeliveryDate.HasValue)
                    {
                        bOM_SDetail.PreDeliveryDate = EditEntity.PreDeliveryDate.Value;
                    }
                    bOM_SDetail.Quantity = 0;
                    details.Add(bOM_SDetail);
                }
                sgh.InsertItemDataToGrid<tb_PurOrderDetail>(grid1, sgd, details, c => c.ProdDetailID, position);
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
            Summation();
        }
        private void Summation()
        {
            try
            {
                //计算总金额  这些逻辑是不是放到业务层？后面要优化
                List<tb_PurOrderDetail> details = sgd.BindingSourceLines.DataSource as List<tb_PurOrderDetail>;
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }

                EditEntity.TotalQty = details.Sum(c => c.Quantity);
                EditEntity.TotalAmount = details.Sum(c => (c.CustomizedCost + c.UnitPrice) * c.Quantity);
                EditEntity.TotalAmount = EditEntity.TotalAmount + EditEntity.ShipCost;
                EditEntity.TotalTaxAmount = details.Sum(c => c.TaxAmount);
                EditEntity.TotalUntaxedAmount = details.Sum(c => (c.UntaxedCustomizedCost + c.UntaxedUnitPrice) * c.Quantity);

                //不含税的总金额+不含税运费
                if (EditEntity.ShipCost > 0 && EditEntity.TotalTaxAmount > 0)
                {
                    var FreightTaxRate = EditEntity.tb_PurOrderDetails.FirstOrDefault(c => c.TaxRate > 0);
                    if (FreightTaxRate != null)
                    {
                        var UntaxedShippingCost = (EditEntity.ShipCost / (1 + FreightTaxRate.TaxRate)); //计算列：不含税运费
                        EditEntity.TotalUntaxedAmount = EditEntity.tb_PurOrderDetails.Sum(c => c.SubtotalUntaxedAmount);
                        EditEntity.TotalUntaxedAmount += Math.Round(UntaxedShippingCost, MainForm.Instance.authorizeController.GetMoneyDataPrecision());
                    }
                }


                if (EditEntity.Currency_ID != AppContext.BaseCurrency.Currency_ID)
                {
                    EditEntity.ForeignTotalAmount = EditEntity.TotalAmount / EditEntity.ExchangeRate;
                    //
                    EditEntity.ForeignTotalAmount = Math.Round(EditEntity.ForeignTotalAmount, MainForm.Instance.authorizeController.GetMoneyDataPrecision()); // 四舍五入到 2 位小数
                }

                if (EditEntity.PayStatus == (int)PayStatus.全额预付)
                {
                    EditEntity.Deposit = EditEntity.TotalAmount;
                }
                if (EditEntity.PayStatus == (int)PayStatus.未付款)
                {
                    EditEntity.Deposit = 0;
                }

            }
            catch (Exception ex)
            {

                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }
        }
        protected async override Task<bool> Save(bool NeedValidated)
        {
            if (EditEntity == null)
            {
                return false;
            }

            List<tb_PurOrderDetail> details = new List<tb_PurOrderDetail>();
            var eer = errorProviderForAllInput.GetError(txtTotalAmount);
            bindingSourceSub.EndEdit();

            //如果订单 选择了未付款，但是又选择了非账期的即实收账方式。则审核不通过。
            //如果订单选择了 非未付款，但又选择了账期也不能通过。
            if (NeedValidated && EditEntity.Paytype_ID.HasValue)
            {
                if (EditEntity.PayStatus == (int)PayStatus.未付款)
                {
                    var paytype = EditEntity.Paytype_ID.Value;
                    var paymethod = BizCacheHelper.Instance.GetEntity<tb_PaymentMethod>(EditEntity.Paytype_ID.Value);
                    if (paymethod != null && paymethod.ToString() != "System.Object")
                    {
                        if (paymethod is tb_PaymentMethod pm)
                        {

                            if (pm.Cash || pm.Paytype_Name != DefaultPaymentMethod.账期.ToString())
                            {
                                MessageBox.Show("未付款时，【付款方式】错误,请选择【账期】。");
                                return false;
                            }
                        }
                    }
                }
                else
                {
                    //付过时不能选账期  要选部分付款时使用的方式
                    var paytype = EditEntity.Paytype_ID.Value;
                    var paymethod = BizCacheHelper.Instance.GetEntity<tb_PaymentMethod>(EditEntity.Paytype_ID.Value);
                    if (paymethod != null && paymethod.ToString() != "System.Object")
                    {
                        if (paymethod is tb_PaymentMethod pm)
                        {
                            //如果是账期，但是又选择的是非 未付款
                            if (pm.Paytype_Name == DefaultPaymentMethod.账期.ToString())
                            {
                                MessageBox.Show("【付款方式】错误,全部付款或部分付款时，请选择【付款方式】为非账期的方式。");
                                return false;
                            }
                        }
                    }
                }
            }
            if (NeedValidated)
            {
                if (EditEntity.PayStatus == (int)PayStatus.未付款)
                {
                    //如果订金大于零时，则不能是未付款
                    if (EditEntity.Deposit > 0 || EditEntity.ForeignDeposit > 0)
                    {
                        MessageBox.Show("未付款时，订金不能大于零。");
                        return false;
                    }
                    if (EditEntity.Paytype_ID == 0)
                    {

                    }
                }
                if (EditEntity.PayStatus == (int)PayStatus.部分预付)
                {
                    //如果订金大于零时，则不能是未付款
                    if (EditEntity.Deposit > 0 || EditEntity.ForeignDeposit > 0)
                    {

                    }
                    else
                    {
                        MessageBox.Show("部分预付时，请输入正确的订金金额。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }
                if (EditEntity.PayStatus == (int)PayStatus.全额预付)
                {
                    //超付情况时，只是提示
                    if (EditEntity.Deposit > EditEntity.TotalAmount)
                    {
                        if (MessageBox.Show("全额预付时，订金大于总金额。你确定客户要超额付款吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No)
                        {
                            return false;
                        }
                    }
                }
            }
            List<tb_PurOrderDetail> detailentity = bindingSourceSub.DataSource as List<tb_PurOrderDetail>;
            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.ProdDetailID > 0).ToList();
                //details = details.Where(t => t.ProdDetailID > 0).ToList();
                //如果没有有效的明细。直接提示
                if (NeedValidated && details.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("请录入有效明细记录！");
                    return false;
                }
                if (EditEntity.ApprovalStatus == null)
                {
                    EditEntity.ApprovalStatus = (int)ApprovalStatus.未审核;
                }
                //设置目标ID成功后就行头写上编号？
                //   表格中的验证提示
                //   其他输入条码验证

                if (NeedValidated && EditEntity.TotalQty != details.Sum(c => c.UndeliveredQty) || details.Sum(c => c.Quantity) != details.Sum(c => c.UndeliveredQty))
                {
                    if (EditEntity.DataStatus == (int)DataStatus.草稿 || EditEntity.DataStatus == (int)DataStatus.新建)
                    {
                        details.ForEach(c =>
                        {
                            c.UndeliveredQty = c.Quantity;
                        });
                    }
                    //System.Windows.Forms.MessageBox.Show("订购数量和明细未交数量的和不相等，请检查记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //return false;
                }

                EditEntity.tb_PurOrderDetails = details;
                EditEntity.tb_PurOrderDetails.ForEach(c =>
                {
                    if (c.TaxAmount > 0)
                    {
                        c.IncludingTax = true;
                    }
                }
                    );

                foreach (var item in details)
                {
                    item.tb_purorder = EditEntity;
                }
                if (EditEntity.TotalTaxAmount > 0)
                {
                    EditEntity.IsIncludeTax = true;
                }
                else
                {
                    EditEntity.IsIncludeTax = false;
                }
                //var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                //if (aa.Count > 0)
                //{
                //    System.Windows.Forms.MessageBox.Show("明细中，相同的产品不能多行录入,如有需要,请另建单据保存!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}

                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_PurOrderDetail>(details))
                {
                    return false;
                }
                EditEntity.TotalQty = details.Sum(c => c.Quantity);

                //含税的
                EditEntity.TotalAmount = details.Sum(c => (c.UnitPrice + c.CustomizedCost) * c.Quantity);
                EditEntity.TotalAmount = EditEntity.TotalAmount + EditEntity.ShipCost;

                //产品税额
                EditEntity.TotalTaxAmount = details.Sum(c => c.TaxAmount);

                //默认认为运费含税，税率随明细

                EditEntity.TotalUntaxedAmount = details.Sum(c => (c.UntaxedCustomizedCost + c.UntaxedUnitPrice) * c.Quantity);


                //不含税的总金额+不含税运费
                decimal UntaxedShippingCost = 0;
                if (EditEntity.ShipCost > 0 && EditEntity.TotalTaxAmount > 0)
                {
                    decimal FreightTaxRate = details.FirstOrDefault(c => c.TaxRate > 0).TaxRate;
                    UntaxedShippingCost = (EditEntity.ShipCost / (1 + FreightTaxRate)); //计算列：不含税运费
                    EditEntity.TotalUntaxedAmount += Math.Round(UntaxedShippingCost, MainForm.Instance.authorizeController.GetMoneyDataPrecision());
                }

                if (NeedValidated && EditEntity.TotalQty != details.Sum(c => c.Quantity))
                {
                    System.Windows.Forms.MessageBox.Show("单据总数量和明细数量的和不相等，请检查记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (NeedValidated)
                {
                    if (EditEntity.tb_PurEntries != null && EditEntity.tb_PurEntries.Count > 0)
                    {
                        MessageBox.Show("当前订单已有采购入库数据，无法修改保存。请联系仓库处理。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }




                ReturnMainSubResults<tb_PurOrder> SaveResult = new ReturnMainSubResults<tb_PurOrder>();
                if (NeedValidated)
                {
                    if (EditEntity.TotalTaxAmount > 0)
                    {
                        EditEntity.IsIncludeTax = true;
                    }
                    else
                    {
                        EditEntity.IsIncludeTax = false;
                    }

                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.PurOrderNo}。");
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



        protected async override Task<bool> Submit()
        {
            if (!base.Validator(EditEntity))
            {
                return false;
            }
            bool rs = await base.Submit();
            return rs;
        }


        tb_PurOrderController<tb_PurOrder> ctr = Startup.GetFromFac<tb_PurOrderController<tb_PurOrder>>();




        /// <summary>
        /// 结案
        /// </summary>
        protected override async Task<bool> CloseCaseAsync()
        {
            if (EditEntity == null)
            {
                return false;
            }

            //要审核过，并且通过了，才能结案。
            if (EditEntity.ApprovalStatus.Value == (int)ApprovalStatus.未审核 && EditEntity.DataStatus == (int)DataStatus.确认)
            {
                MainForm.Instance.uclog.AddLog("已经审核的单据才能结案。");
                return false;
            }
            if (EditEntity.tb_PurOrderDetails == null || EditEntity.tb_PurOrderDetails.Count == 0)
            {
                MainForm.Instance.uclog.AddLog("单据中没有明细数据，请确认录入了完整数量和金额。", UILogType.警告);
                return false;
            }

            CommonUI.frmOpinion frm = new CommonUI.frmOpinion();
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<tb_PurOrder>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
            ApprovalEntity ae = new ApprovalEntity();
            ae.BillID = pkid;
            BillConverterFactory bcf = Startup.GetFromFac<BillConverterFactory>();
            CommBillData cbd = bcf.GetBillData<tb_PurOrder>(EditEntity);
            ae.BillNo = cbd.BillNo;
            ae.bizType = cbd.BizType;
            ae.bizName = cbd.BizName;
            frm.BindData(ae);
            if (frm.ShowDialog() == DialogResult.OK)//审核了。不管是同意还是不同意
            {
                EditEntity.CloseCaseOpinions = frm.txtOpinion.Text;
                RevertCommand command = new RevertCommand();
                tb_PurOrder oldobj = CloneHelper.DeepCloneObject_maxnew<tb_PurOrder>(EditEntity);
                command.UndoOperation = delegate ()
                {
                    CloneHelper.SetValues<tb_PurOrder>(EditEntity, oldobj);
                };
                List<tb_PurOrder> _PurOrders = [EditEntity];
                ReturnResults<bool> returnResults = await ctr.BatchCloseCaseAsync(_PurOrders);
                if (returnResults.Succeeded)
                {

                    //if (MainForm.Instance.WorkflowItemlist.ContainsKey(""))
                    //{

                    //}
                    //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                    //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                    //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                    //MainForm.Instance.ecs.AddSendData(od);

                    //审核成功
                    base.ToolBarEnabledControl(MenuItemEnums.结案);
                    toolStripbtnReview.Enabled = true;

                }
                else
                {
                    //审核失败 要恢复之前的值
                    command.Undo();
                    MainForm.Instance.PrintInfoLog($"{EditEntity.PurOrderNo}结案失败,请联系管理员！", Color.Red);
                }
                return true;
            }
            else
            {
                return false;
            }

        }

        private async Task<tb_PurOrder> OrderToOutBill(long _sorderid)
        {
            tb_SaleOrder saleorder;
            ButtonSpecAny bsa = txtSorderNo.ButtonSpecs.FirstOrDefault(c => c.UniqueName == "btnQuery");
            if (bsa == null)
            {
                return null;
            }
            saleorder = await MainForm.Instance.AppContext.Db.Queryable<tb_SaleOrder>()
            //.Includes(a => a.tb_SaleOuts)
            .Includes(a => a.tb_SaleOrderDetails, b => b.tb_proddetail, c => c.tb_prod)
            .Where(c => c.SOrder_ID == _sorderid)
            .SingleAsync();
            tb_SaleOrderController<tb_SaleOrder> ctr = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
            tb_PurOrder TargetBill = ctr.SaleOrderToPurOrder(saleorder);
            ActionStatus actionStatus = ActionStatus.无操作;
            BindData(TargetBill, actionStatus);
            return TargetBill;
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

                    }

                }
            }
        }
    }
}

