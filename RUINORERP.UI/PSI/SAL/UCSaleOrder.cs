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
using System.Linq.Expressions;
using AutoMapper;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.Security;
using RUINORERP.Business.Processor;
using RUINORERP.Model.CommonModel;
using SourceGrid;
using RUINORERP.Business.CommService;
using NPOI.POIFS.Properties;
using System.Diagnostics;
using RUINORERP.Common.Extensions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using MySqlX.XDevAPI.Common;
using RUINORERP.UI.AdvancedUIModule;
using FastReport.DevComponents.DotNetBar.Controls;
using RUINORERP.UI.CommonUI;

using RUINORERP.Global.EnumExt;
using Fireasy.Common.Configuration;
using RUINORERP.UI.Monitoring.Auditing;
using NPOI.SS.Formula.Functions;


namespace RUINORERP.UI.PSI.SAL
{


    /// <summary>
    /// 销售订单时：有运费外币，总金额外币，订单外币。反而出库时不用这么多。外币只是用于记账。出库时只要根据本币和外币及汇率。生成应收时自动算出来。
    /// </summary>
    [MenuAttrAssemblyInfo("销售订单", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.销售管理, BizType.销售订单)]
    public partial class UCSaleOrder : BaseBillEditGeneric<tb_SaleOrder, tb_SaleOrderDetail>, IPublicEntityObject
    {
        public UCSaleOrder()
        {
            InitializeComponent();
            //InitDataToCmbByEnumDynamicGeneratedDataSource<tb_SaleOrder>(typeof(Priority), e => e.OrderPriority, cmbOrderPriority, false);
            AddPublicEntityObject(typeof(ProductSharePart));
        }


        internal override void LoadDataToUI(object Entity)
        {
            ActionStatus actionStatus = ActionStatus.无操作;
            BindData(Entity as tb_SaleOrder, actionStatus);
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            //BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_SaleOrder).Name + "Processor");
            //QueryConditionFilter = baseProcessor.GetQueryFilter();
            base.QueryConditionBuilder();
            //创建表达式
            var lambda = Expressionable.Create<tb_SaleOrder>()
                             .And(t => t.isdeleted == false)
                            //自己的只能查自己的
                            .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext), t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                            .ToExpression();//注意 这一句 不能少
            QueryConditionFilter.SetFieldLimitCondition(lambda);
        }


        protected async void UpdatePaymentStatus()
        {
            if (EditEntity == null)
            {
                return;
            }
            //反审，要审核过，并且通过了，才能反审。
            if (EditEntity.ApprovalStatus.Value == (int)ApprovalStatus.已审核 && !EditEntity.ApprovalResults.HasValue)
            {
                MainForm.Instance.uclog.AddLog("已经审核,且【同意】的单据才能更新付款状态。");
                return;
            }

            RevertCommand command = new RevertCommand();
            //缓存当前编辑的对象。如果撤销就回原来的值
            tb_SaleOrder oldobj = CloneHelper.DeepCloneObject_maxnew<tb_SaleOrder>(EditEntity);
            command.UndoOperation = delegate ()
            {
                //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                CloneHelper.SetValues<tb_SaleOrder>(EditEntity, oldobj);
            };

            tb_SaleOrderController<tb_SaleOrder> ctr = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();

            if (MessageBox.Show("你确定要调整当前订单的付款状态吗？", "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            {
                return;
            }

            if (AppContext.IsSuperUser)
            {
                await Save(true);
                await ctr.BaseSaveOrUpdate(EditEntity);
                return;
            }

            ReturnResults<bool> rr = await ctr.UpdatePaymentStatus(EditEntity);
            if (rr.Succeeded)
            {
                MainForm.Instance.AuditLogHelper.CreateAuditLog<tb_SaleOrder>("付款调整", EditEntity);
                //if (MainForm.Instance.WorkflowItemlist.ContainsKey(""))
                //{

                //}
                //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                //MainForm.Instance.ecs.AddSendData(od);
                base.ToolBarEnabledControl(MenuItemEnums.结案);
            }
            else
            {
                //付款调整失败 要恢复之前的值
                command.Undo();
                MainForm.Instance.PrintInfoLog($"{EditEntity.SOrderNo}付款调整失败{rr.ErrorMsg},请联系管理员！", Color.Red);
            }
            MainForm.Instance.AuditLogHelper.CreateAuditLog<tb_SaleOrder>("付款调整", EditEntity, $"结果:{(rr.Succeeded ? "成功" : "失败")},{rr.ErrorMsg}");
        }

        protected override void LoadRelatedDataToDropDownItems()
        {
            if (base.EditEntity is tb_SaleOrder saleOrder)
            {
                if (saleOrder.RefBillID.HasValue)
                {
                    RelatedQueryParameter rqp = new RelatedQueryParameter();
                    rqp.bizType = (BizType)saleOrder.RefBizType;
                    rqp.billId = saleOrder.RefBillID.Value;
                    ToolStripMenuItem RelatedMenuItem = new ToolStripMenuItem();
                    RelatedMenuItem.Name = $"{rqp.billId}";
                    RelatedMenuItem.Tag = rqp;
                    RelatedMenuItem.Text = $"{rqp.bizType}:{saleOrder.RefNO}";
                    RelatedMenuItem.Click += base.MenuItem_Click;
                    if (!toolStripbtnRelatedQuery.DropDownItems.ContainsKey(saleOrder.RefBillID.Value.ToString()))
                    {
                        toolStripbtnRelatedQuery.DropDownItems.Add(RelatedMenuItem);
                    }
                }

                if (saleOrder.tb_SaleOuts != null && saleOrder.tb_SaleOuts.Count > 0)
                {
                    foreach (var item in saleOrder.tb_SaleOuts)
                    {
                        RelatedQueryParameter rqp = new RelatedQueryParameter();
                        rqp.bizType = BizType.销售出库单;
                        rqp.billId = item.SaleOut_MainID;
                        ToolStripMenuItem RelatedMenuItem = new ToolStripMenuItem();
                        RelatedMenuItem.Name = $"{rqp.billId}";
                        RelatedMenuItem.Tag = rqp;
                        RelatedMenuItem.Text = $"{rqp.bizType}:{item.SaleOutNo}";
                        RelatedMenuItem.Click += base.MenuItem_Click;
                        if (!toolStripbtnRelatedQuery.DropDownItems.ContainsKey(item.SaleOut_MainID.ToString()))
                        {
                            toolStripbtnRelatedQuery.DropDownItems.Add(RelatedMenuItem);
                        }
                    }
                }

            }
            base.LoadRelatedDataToDropDownItems();
        }

        public override void BindData(tb_SaleOrder entityPara, ActionStatus actionStatus)
        {
            tb_SaleOrder entity = entityPara as tb_SaleOrder;

            if (entity == null)
            {

                return;
            }

            if (entity != null)
            {
                if (entity.SOrder_ID > 0)
                {
                    entity.PrimaryKeyID = entity.SOrder_ID;
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
                    // entity.DataStatus = (int)DataStatus.确认;
                    //如果审核了，审核要灰色
                }
                else
                {
                    entity.ActionStatus = ActionStatus.新增;
                    entity.DataStatus = (int)DataStatus.草稿;
                    if (string.IsNullOrEmpty(entity.SOrderNo))
                    {
                        entity.SOrderNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.销售订单);
                    }
                    entity.CloseCaseOpinions = string.Empty;
                    entity.ApprovalOpinions = string.Empty;
                    entity.SaleDate = System.DateTime.Now;
                    //通过动态参数来设置这个默认值。这样每个公司不同设置按自己的来。
                    entity.IsFromPlatform = AppContext.GlobalVariableConfig.IsFromPlatform;

                    // 监听配置变化
                    MainForm.Instance.Globalconfig.OnChange((config, value) =>
                    {
                        entity.IsFromPlatform = config.IsFromPlatform;
                    });

                    if (entity.tb_SaleOrderDetails != null && entity.tb_SaleOrderDetails.Count > 0)
                    {
                        entity.tb_SaleOrderDetails.ForEach(c => c.SOrder_ID = 0);
                        entity.tb_SaleOrderDetails.ForEach(c => c.SaleOrderDetail_ID = 0);
                    }
                    if (AppContext.BaseCurrency != null)
                    {
                        entity.Currency_ID = AppContext.BaseCurrency.Currency_ID;
                    }
                    lblExchangeRate.Visible = false;
                    txtExchangeRate.Visible = false;
                    UIHelper.ControlForeignFieldInvisible<tb_SaleOrder>(this, false);
                }
            }
            //StatusMachine.CurrentDataStatus = (DataStatus)entity.DataStatus;
            //StatusMachine.ApprovalStatus = (ApprovalStatus)entity.ApprovalStatus;
            if (entity.ApprovalStatus.HasValue)
            {
                lblReview.Text = ((ApprovalStatus)entity.ApprovalStatus).ToString();
            }
            EditEntity = entity;
            DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v => v.CurrencyName, cmbCurrency_ID);
            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.SOrderNo, txtOrderNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.CustomerPONo, txtCustomerPONo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4Cmb<tb_PaymentMethod>(entity, k => k.Paytype_ID, v => v.Paytype_Name, cmbPaytype_ID);
            DataBindingHelper.BindData4Cmb<tb_FM_Account>(entity, k => k.Account_id, v => v.Account_name, cmbAccount_id);
            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID, true);
            if (AppContext.projectGroups != null && AppContext.projectGroups.Count > 0)
            {
                #region 项目组 如果有设置则按设置。没有则全部
                cmbProjectGroup.DataSource = null;
                cmbProjectGroup.DataBindings.Clear();
                BindingSource bs = new BindingSource();
                bs.DataSource = AppContext.projectGroups;
                ComboBoxHelper.InitDropList(bs, cmbProjectGroup, "ProjectGroup_ID", "ProjectGroupName", ComboBoxStyle.DropDownList, false);
                var depa = new Binding("SelectedValue", entity, "ProjectGroup_ID", true, DataSourceUpdateMode.OnValidation);
                //数据源的数据类型转换为控件要求的数据类型。
                depa.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
                //将控件的数据类型转换为数据源要求的数据类型。
                depa.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
                cmbProjectGroup.DataBindings.Add(depa);
                #endregion
            }
            else
            {
                DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v => v.ProjectGroupName, cmbProjectGroup);
            }
            if (entity.DataStatus >= (int)DataStatus.确认)
            {
                DataBindingHelper.BindData4CmbByEnum<tb_SaleOrder, PayStatus>(entity, k => k.PayStatus, cmbPayStatus, false);
            }
            else
            {
                DataBindingHelper.BindData4CmbByEnum<tb_SaleOrder, PayStatus>(entity, k => k.PayStatus, cmbPayStatus, false, PayStatus.全部付款, PayStatus.部分付款);
            }


            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.FreightIncome.ToString(), txtFreightIncome, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.ForeignFreightIncome.ToString(), txtForeignFreightIncome, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.TotalCommissionAmount.ToString(), txtTotalCommissionAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4DataTime<tb_SaleOrder>(entity, t => t.PreDeliveryDate, dtpPreDeliveryDate, false);
            DataBindingHelper.BindData4DataTime<tb_SaleOrder>(entity, t => t.SaleDate, dtpSaleDate, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.ExchangeRate.ToString(), txtExchangeRate, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.ShippingAddress, txtShippingAddress, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.ShippingWay, txtshippingWay, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.ForeignTotalAmount.ToString(), txtForeignTotalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.ForeignTotalAmount.ToString(), txtForeignTotalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.ForeignDeposit.ToString(), txtForeignDeposit, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4CmbByEnum<tb_SaleOrder>(entity, k => k.OrderPriority, typeof(Priority), cmbOrderPriority, true);
            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.Deposit.ToString(), txtDeposit, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.PlatformOrderNo, txtPlatformOrderNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.TotalQty, txtTotalQty, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.TotalTaxAmount, txtTaxAmount, BindDataType4TextBox.Money, false);

            DataBindingHelper.BindData4CheckBox<tb_SaleOrder>(entity, t => t.IsFromPlatform, chk平台单, false);
            DataBindingHelper.BindData4CheckBox<tb_SaleOrder>(entity, t => t.IsCustomizedOrder, chkIsCustomizedOrder, false);

            base.errorProviderForAllInput.DataSource = entity;
            base.errorProviderForAllInput.ContainerControl = this;

            //this.ValidateChildren();
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            DataBindingHelper.BindData4ControlByEnum<tb_SaleOrder>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_SaleOrder>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));

            if (entity.tb_SaleOrderDetails != null && entity.tb_SaleOrderDetails.Count > 0)
            {
                //LoadDataToGrid(entity.tb_SaleOrderDetails);
                sgh.LoadItemDataToGrid<tb_SaleOrderDetail>(grid1, sgd, entity.tb_SaleOrderDetails, c => c.ProdDetailID);
            }
            else
            {
                //LoadDataToGrid(new List<tb_SaleOrderDetail>());
                sgh.LoadItemDataToGrid<tb_SaleOrderDetail>(grid1, sgd, new List<tb_SaleOrderDetail>(), c => c.ProdDetailID);
            }
            toolStripButton付款调整.ToolTipText = "当付款状态或付款方式发生变化时，需要进行付款调整才会显示。";

            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {
                if (EditEntity == null)
                {
                    return;
                }

                if (s2.PropertyName == entity.GetPropertyName<tb_SaleOrder>(c => c.PayStatus) || s2.PropertyName == entity.GetPropertyName<tb_SaleOrder>(c => c.Paytype_ID))
                {
                    toolStripButton付款调整.Enabled = true;
                    UIHelper.ControlButton<ToolStripButton>(CurMenuInfo, toolStripButton付款调整);
                }

                //权限允许
                if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
                {
                    if (s2.PropertyName == entity.GetPropertyName<tb_SaleOrder>(c => c.Account_id) && entity.Account_id.HasValue && entity.Account_id > 0)
                    {
                        if (cmbAccount_id.SelectedItem is tb_FM_Account ac)
                        {
                            entity.tb_fm_account = ac;
                            if (ac.Currency_ID.HasValue)
                            {
                                entity.Currency_ID = ac.Currency_ID.Value;
                            }
                        }
                    }

                    //根据币别如果是外币才显示外币相关的字段
                    if (s2.PropertyName == entity.GetPropertyName<tb_SaleOrder>(c => c.Currency_ID) && entity.Currency_ID > 0)
                    {
                        // var obj = BizCacheHelper.Instance.GetEntity<tb_Currency>(entity.Currency_ID);
                        //if (obj != null && obj.ToString() != "System.Object")
                        //{
                        if (cmbCurrency_ID.SelectedItem is tb_Currency cv)
                        {
                            if (entity.Account_id.HasValue && entity.Account_id.Value > 0)
                            {
                                if (entity.tb_fm_account.Currency_ID.HasValue && entity.tb_fm_account.Currency_ID.Value != cv.Currency_ID)
                                {
                                    MessageBox.Show("当前收款账户的币别与选择的币别不一致。请联系管理员或财务确认后操作。");
                                    return;
                                }
                            }

                            if (cv.CurrencyCode.Trim() != DefaultCurrency.RMB.ToString())
                            {
                                //显示外币相关
                                UIHelper.ControlForeignFieldInvisible<tb_SaleOrder>(this, true);
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
                                UIHelper.ControlForeignFieldInvisible<tb_SaleOrder>(this, false);
                                lblExchangeRate.Visible = false;
                                txtExchangeRate.Visible = false;
                                entity.ExchangeRate = 1;
                                entity.ForeignTotalAmount = 0;
                            }
                        }
                        //}
                    }

                    if (s2.PropertyName == entity.GetPropertyName<tb_SaleOrder>(c => c.PayStatus) && entity.PayStatus == (int)PayStatus.未付款)
                    {
                        //默认为账期
                        entity.Paytype_ID = MainForm.Instance.AppContext.PaymentMethodOfPeriod.Paytype_ID;
                    }
                    if (s2.PropertyName == entity.GetPropertyName<tb_SaleOrder>(c => c.Paytype_ID) && entity.Paytype_ID == MainForm.Instance.AppContext.PaymentMethodOfPeriod.Paytype_ID)
                    {
                        //默认为未付款
                        entity.PayStatus = (int)PayStatus.未付款;
                    }
                    if (s2.PropertyName == entity.GetPropertyName<tb_SaleOrder>(c => c.Paytype_ID) && entity.Paytype_ID > 0)
                    {
                        if (cmbPaytype_ID.SelectedItem is tb_PaymentMethod paymentMethod)
                        {
                            EditEntity.tb_paymentmethod = paymentMethod;
                        }
                    }

                    if (s2.PropertyName == entity.GetPropertyName<tb_SaleOrder>(c => c.ProjectGroup_ID) && entity.ProjectGroup_ID.HasValue && entity.ProjectGroup_ID > 0)
                    {
                        if (cmbProjectGroup.SelectedItem is tb_ProjectGroup ProjectGroup)
                        {
                            if (ProjectGroup.tb_ProjectGroupAccountMappers != null && ProjectGroup.tb_ProjectGroupAccountMappers.Count > 0)
                            {
                                EditEntity.Account_id = ProjectGroup.tb_ProjectGroupAccountMappers[0].tb_fm_account.Account_id;
                                EditEntity.tb_fm_account = ProjectGroup.tb_ProjectGroupAccountMappers[0].tb_fm_account;
                                EditEntity.tb_projectgroup = ProjectGroup;
                            }
                        }
                    }
                }

                //数据状态变化会影响按钮变化
                if (s2.PropertyName == entity.GetPropertyName<tb_SaleOrder>(c => c.DataStatus))
                {
                    ToolBarEnabledControl(entity);
                }

                //如果客户有变化，带出对应有业务员
                if (entity.CustomerVendor_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_SaleOrder>(c => c.CustomerVendor_ID))
                {
                    var obj = BizCacheHelper.Instance.GetEntity<tb_CustomerVendor>(entity.CustomerVendor_ID);
                    if (obj != null && obj.ToString() != "System.Object")
                    {
                        if (obj is tb_CustomerVendor cv)
                        {
                            if (!string.IsNullOrEmpty(cv.SpecialNotes))
                            {
                                entity.Notes = $"【{cv.SpecialNotes}】";
                            }
                            if (cv.Employee_ID.HasValue)
                            {
                                EditEntity.Employee_ID = cv.Employee_ID.Value;
                            }
                            //客户的 地址 电话 联系人都显示到收货地址中。
                            //如果手机为空则显示座机
                            if (string.IsNullOrEmpty(cv.MobilePhone))
                            {
                                cv.MobilePhone = cv.Phone;
                            }
                            EditEntity.ShippingAddress = cv.Address + " " + cv.MobilePhone + " " + cv.Contact;
                        }
                    }
                }


                if (entity.FreightIncome >= 0 && s2.PropertyName == entity.GetPropertyName<tb_SaleOrder>(c => c.FreightIncome))
                {
                    if (EditEntity.tb_SaleOrderDetails != null)
                    {
                        EditEntity.TotalTaxAmount = entity.tb_SaleOrderDetails.Sum(c => c.SubtotalTaxAmount);
                        EditEntity.TotalAmount = entity.tb_SaleOrderDetails.Sum(c => c.TransactionPrice * c.Quantity);
                        EditEntity.TotalAmount = EditEntity.TotalAmount + EditEntity.FreightIncome;
                        if (EditEntity.Currency_ID != AppContext.BaseCurrency.Currency_ID)
                        {
                            EditEntity.ForeignTotalAmount = EditEntity.TotalAmount / EditEntity.ExchangeRate;
                            //
                            EditEntity.ForeignTotalAmount = Math.Round(EditEntity.ForeignTotalAmount, 2); // 四舍五入到 2 位小数
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

            if (EditEntity.PrintStatus == 0)
            {
                lblPrintStatus.Text = "未打印";
            }
            else
            {
                lblPrintStatus.Text = $"打印{EditEntity.PrintStatus}次";
            }
            //创建表达式
            var lambda = Expressionable.Create<tb_CustomerVendor>()
                            .And(t => t.IsCustomer == true)
                            .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && !AppContext.IsSuperUser, t => t.Employee_ID == AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户
                            .ToExpression();//注意 这一句 不能少

            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CustomerVendor).Name + "Processor");
            QueryFilter queryFilterC = baseProcessor.GetQueryFilter();
            queryFilterC.FilterLimitExpressions.Add(lambda);
            DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, queryFilterC.GetFilterExpression<tb_CustomerVendor>(), true);
            DataBindingHelper.InitFilterForControlByExp<tb_CustomerVendor>(entity, cmbCustomerVendor_ID, c => c.CVName, queryFilterC);

            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_SaleOrderValidator>(), kryptonPanelMainInfo.Controls);
                // base.InitEditItemToControl(entity, kryptonPanelMainInfo.Controls);
            }
            base.BindData(entity);

            /*
            #region 状态管理
         
            // 初始化
            var notificationService = new WorkflowNotificationService();
            var statusMachine = new StatusMachine(
                (DataStatus)entity.DataStatus,
                (ApprovalStatus)entity.ApprovalStatus,
                entity.ApprovalResults ?? false,//这里是如果有值则显示他的值，如果没有则false。要修复
                notificationService);


            // 创建带自定义规则的UI绑定器
            var binder = new UIStateBinder(statusMachine, BaseToolStrip, MainForm.Instance.AppContext.workflowHost,
                (data, approval, result, op) =>
            {
                // 示例：增加财务专属操作
                if (op == MenuItemEnums.数据特殊修正)
                {
                    return new ControlState
                    {
                        Visible = data == DataStatus.确认 || true,
                        Enabled = MainForm.Instance.AppContext.IsSuperUser
                    };
                }
                return StatusEvaluator.GetControlState(data, approval, result, op);
            });


            // 手动注册特殊按钮
            // binder.RegisterControl(btnFinanceApprove, MenuItemEnums.财务审核);
         
            #endregion
            */





            /*
             * 
             * // 初始化状态机和UI绑定
var workflowHost = new WorkflowHost();
var statusMachine = new StatusMachine(...);
var mainForm = Application.OpenForms[0];

using var binder = new UIStateBinder(
    statusMachine,
    mainForm.Controls["toolStrip1"], // 传入实际的ToolStrip控件
    workflowHost);

// 手动注册特殊控件
binder.RegisterControl(btnSpecialOperation, MenuItemEnums.数据特殊修正);


             var customEvaluator = (DataStatus ds, ApprovalStatus aps, bool result, MenuItemEnums op) =>
{
    if (op == MenuItemEnums.数据特殊修正)
    {
        return new ControlState
        {
            Visible = ds == DataStatus.确认,
            Enabled = aps == ApprovalStatus.已审核 && result
        };
    }
    return StatusEvaluator.GetControlState(ds, aps, result, op);
};

using var binder = new UIStateBinder(..., customEvaluator);
             
             */
        }
        //protected override tb_SaleOrder AddByCopy()
        //{
        //    EditEntity = base.AddByCopy();
        //    EditEntity.ActionStatus = ActionStatus.新增;
        //    EditEntity.SOrder_ID = 0;
        //    EditEntity.ApprovalStatus = (int)ApprovalStatus.未审核;
        //    EditEntity.DataStatus = (int)DataStatus.草稿;
        //    EditEntity.Approver_at = null;
        //    EditEntity.ApprovalResults = null;
        //    EditEntity.tb_SaleOuts = null;
        //    EditEntity.SOrderNo = string.Empty;
        //    EditEntity.tb_PurOrders = null;
        //    EditEntity.PrintStatus = 0;
        //    BusinessHelper.Instance.InitEntity(EditEntity);
        //    foreach (var item in EditEntity.tb_SaleOrderDetails)
        //    {
        //        item.SOrder_ID = 0;
        //        item.SaleOrderDetail_ID = 0;
        //        item.PrimaryKeyID = 0;
        //        item.tb_saleorder = null;
        //    }
        //    return EditEntity;
        //}

        //public void InitDataTocmbbox()
        //{
        //    DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
        //    DataBindingHelper.InitDataToCmb<tb_PaymentMethod>(k => k.Paytype_ID, v => v.Paytype_Name, cmbPaytype_ID);
        //    DataBindingHelper.InitDataToCmbByEnumDynamicGeneratedDataSource<tb_SaleOrder>(typeof(PayStatus), e => e.PayStatus, cmbPayStatus, false);
        //}

        /*
         重要思路提示，这个表格控件，数据源的思路是，
        产品共享表 ProductSharePart+tb_SaleOrderDetail 有合体，SetDependencyObject 根据字段名相同的给值，值来自产品明细表
         并且，表格中可以编辑的需要查询的列是唯一能查到详情的

        SetDependencyObject<P, S, T>   P包含S字段包含T字段--》是有且包含
         */

        // 在基类中定义静态属性

        SourceGridDefine sgd = null;
        //        SourceGridHelper<View_ProdDetail, tb_SaleOrderDetail> sgh = new SourceGridHelper<View_ProdDetail, tb_SaleOrderDetail>();
        SourceGridHelper sgh = new SourceGridHelper();
        //设计关联列和目标列
        View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
        List<View_ProdDetail> list = new List<View_ProdDetail>();
        private void UcSaleOrderEdit_Load(object sender, EventArgs e)
        {
            AddExtendButton(CurMenuInfo);
            var sw = new Stopwatch();
            sw.Start();

            grid1.BorderStyle = BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;

            List<SGDefineColumnItem> listCols = new List<SGDefineColumnItem>();
            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<ProductSharePart, tb_SaleOrderDetail>(c => c.ProdDetailID, false);

            listCols.SetCol_NeverVisible<tb_SaleOrderDetail>(c => c.SaleOrderDetail_ID);
            listCols.SetCol_NeverVisible<tb_SaleOrderDetail>(c => c.ProdDetailID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Rack_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.VendorModelCode);

            if (!AppContext.SysConfig.UseBarCode)
            {
                listCols.SetCol_NeverVisible<ProductSharePart>(c => c.BarCode);
            }
            listCols.SetCol_DefaultValue<tb_SaleOrderDetail>(a => a.Discount, 1m);

            //listCols.SetCol_DefaultValue<tb_SaleOrderDetail>(a => a.TaxRate, 0.13m);//m =>decial d=>double

            //如果库位为只读  暂时只会显示 ID
            //listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Location_ID);
            listCols.SetCol_ReadOnly<tb_SaleOrderDetail>(c => c.TotalDeliveredQty);
            listCols.SetCol_ReadOnly<tb_SaleOrderDetail>(c => c.SubtotalTaxAmount);
            listCols.SetCol_ReadOnly<tb_SaleOrderDetail>(c => c.TotalReturnedQty);
            listCols.SetCol_ReadOnly<tb_SaleOrderDetail>(c => c.Cost);

            listCols.SetCol_Format<tb_SaleOrderDetail>(c => c.Discount, CustomFormatType.PercentFormat);
            listCols.SetCol_Format<tb_SaleOrderDetail>(c => c.TaxRate, CustomFormatType.PercentFormat);
            listCols.SetCol_Format<tb_SaleOrderDetail>(c => c.UnitPrice, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_SaleOrderDetail>(c => c.Cost, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_SaleOrderDetail>(c => c.SubtotalCostAmount, CustomFormatType.CurrencyFormat);
            listCols.SetCol_Format<tb_SaleOrderDetail>(c => c.CustomizedCost, CustomFormatType.CurrencyFormat);
            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridMasterData = EditEntity;
            listCols.SetCol_Formula<tb_SaleOrderDetail>((a, b) => a.UnitPrice * b.Discount, c => c.TransactionPrice);//-->成交价是结果列
            //listCols.SetCol_Formula<tb_SaleOrderDetail>((a, b) => a.TransactionPrice / b.Discount, c => c.UnitPrice);

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

            listCols.SetCol_Formula<tb_SaleOrderDetail>((a, b) => a.UnitCommissionAmount * b.Quantity, c => c.CommissionAmount);
            listCols.SetCol_FormulaReverse<tb_SaleOrderDetail>(d => d.Quantity != 0, (a, b) => a.CommissionAmount / b.Quantity, c => c.UnitCommissionAmount);

            //listCols.SetCol_Summary<tb_SaleOrderDetail>(c => c.SubtotalTransAmount);
            //listCols.SetCol_Summary<tb_SaleOrderDetail>(c => c.SubtotalTaxAmount);

            //设置总计列
            BaseProcessor baseProcessor = BusinessHelper._appContext.GetRequiredServiceByName<BaseProcessor>(typeof(tb_SaleOrderDetail).Name + "Processor");
            var summaryCols = baseProcessor.GetSummaryCols();
            foreach (var item in summaryCols)
            {
                foreach (var col in listCols)
                {
                    col.SetCol_Summary<tb_SaleOrderDetail>(item);
                }
            }

            //公共到明细的映射 源 ，左边会隐藏
            sgh.SetPointToColumnPairs<ProductSharePart, tb_SaleOrderDetail>(sgd, f => f.Location_ID, t => t.Location_ID);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_SaleOrderDetail>(sgd, f => f.Inv_Cost, t => t.Cost);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_SaleOrderDetail>(sgd, f => f.Standard_Price, t => t.UnitPrice);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_SaleOrderDetail>(sgd, f => f.prop, t => t.property);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_SaleOrderDetail>(sgd, f => f.Model, t => t.CustomerPartNo, false);

            //应该只提供一个结构
            List<tb_SaleOrderDetail> lines = new List<tb_SaleOrderDetail>();
            bindingSourceSub.DataSource = lines;
            sgd.BindingSourceLines = bindingSourceSub;

            //list = dc.BaseQueryByWhere(exp);
            list = MainForm.Instance.list;

            sgd.SetDependencyObject<ProductSharePart, tb_SaleOrderDetail>(list);
            sgd.HasRowHeader = true;
            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);
            sgh.InitGrid(grid1, sgd, true, nameof(tb_SaleOrderDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnLoadMultiRowData += Sgh_OnLoadMultiRowData;
            sgh.OnLoadRelevantFields += Sgh_OnLoadRelevantFields;
            sw.Stop();
            MainForm.Instance.uclog.AddLog("加载数据耗时：" + sw.ElapsedMilliseconds + "毫秒");

            UIHelper.ControlMasterColumnsInvisible(CurMenuInfo, this);
            UIHelper.ControlForeignFieldInvisible<tb_SaleOrder>(this, false);

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
            try
            {
                View_ProdDetail vp = (View_ProdDetail)_View_ProdDetail;
                tb_SaleOrderDetail _SDetail = (tb_SaleOrderDetail)rowObj;
                //通过产品查询页查出来后引过来才有值，如果直接在输入框输入SKU这种唯一的。就没有则要查一次。这时是缓存了？
                if (vp.ProdDetailID > 0 && EditEntity.Employee_ID > 0)
                {
                    tb_PriceRecord pr = MainForm.Instance.AppContext.Db.Queryable<tb_PriceRecord>().Where(a => a.Employee_ID == EditEntity.Employee_ID && a.ProdDetailID == vp.ProdDetailID).Single();
                    if (pr != null)
                    {
                        _SDetail.UnitPrice = pr.SalePrice;

                        var Col = griddefine.grid.Columns.GetColumnInfo(griddefine.DefineColumns.FirstOrDefault(c => c.ColName == nameof(tb_SaleOrderDetail.UnitPrice)).UniqueId);
                        if (Col != null)
                        {
                            griddefine.grid[Position.Row, Col.Index].Value = _SDetail.UnitPrice;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("价格历史自动给值出错", ex);
            }
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
                List<tb_SaleOrderDetail> details = new List<tb_SaleOrderDetail>();

                foreach (var item in RowDetails)
                {
                    tb_SaleOrderDetail Detail = MainForm.Instance.mapper.Map<tb_SaleOrderDetail>(item);
                    details.Add(Detail);
                }
                sgh.InsertItemDataToGrid<tb_SaleOrderDetail>(grid1, sgd, details, c => c.ProdDetailID, position);
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
            Summation();
        }

        private void Summation()
        {
            try
            {
                //计算总金额  这些逻辑是不是放到业务层？后面要优化
                List<tb_SaleOrderDetail> details = sgd.BindingSourceLines.DataSource as List<tb_SaleOrderDetail>;
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }
                EditEntity.TotalQty = details.Sum(c => c.Quantity);
                EditEntity.TotalCost = details.Sum(c => (c.Cost + c.CustomizedCost) * c.Quantity);
                EditEntity.TotalTaxAmount = details.Sum(c => c.SubtotalTaxAmount);
                EditEntity.TotalCommissionAmount = details.Sum(c => c.CommissionAmount);
                EditEntity.TotalAmount = details.Sum(c => c.TransactionPrice * c.Quantity);
                EditEntity.TotalAmount = EditEntity.TotalAmount + EditEntity.FreightIncome;
                if (EditEntity.Currency_ID != AppContext.BaseCurrency.Currency_ID)
                {
                    EditEntity.ForeignTotalAmount = EditEntity.TotalAmount / EditEntity.ExchangeRate;
                    EditEntity.ForeignTotalAmount = Math.Round(EditEntity.ForeignTotalAmount, 2); // 四舍五入到 2 位小数
                    //外币时，如果收了运费。算换成外币。反之
                    if (EditEntity.FreightIncome > 0)
                    {
                        EditEntity.ForeignFreightIncome = EditEntity.FreightIncome / EditEntity.ExchangeRate;
                        EditEntity.ForeignFreightIncome = Math.Round(EditEntity.ForeignFreightIncome, 2); // 四舍五入到 2 位小数
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }
        }

        List<tb_SaleOrderDetail> details = new List<tb_SaleOrderDetail>();
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

            if (EditEntity.PlatformOrderNo != null)
            {
                EditEntity.PlatformOrderNo = EditEntity.PlatformOrderNo.Trim();//去空格
            }

            //如果订单 选择了未付款，但是又选择了非账期的即实收账方式。则审核不通过。
            //如果订单选择了 非未付款，但又选择了账期也不能通过。
            if (NeedValidated)
            {

                if (EditEntity.SOrderNo.Trim().Length == 0)
                {
                    MessageBox.Show("订单编号由系统自动生成，如果不小心清除，请重新生成单据的订单编号。");
                    return false;
                }

                if (EditEntity.ProjectGroup_ID.HasValue && EditEntity.ProjectGroup_ID.Value <= 0)
                {
                    EditEntity.ProjectGroup_ID = null;
                }

                if (EditEntity.Paytype_ID > 0)
                {
                    var paytype = EditEntity.Paytype_ID;
                    var paymethod = BizCacheHelper.Instance.GetEntity<tb_PaymentMethod>(EditEntity.Paytype_ID);
                    if (paymethod != null && paymethod.ToString() != "System.Object")
                    {
                        if (paymethod is tb_PaymentMethod pm)
                        {
                            if (EditEntity.PayStatus == (int)PayStatus.未付款)
                            {
                                if (pm.Cash || pm.Paytype_Name != DefaultPaymentMethod.账期.ToString())
                                {
                                    MessageBox.Show("未付款时，付款方式错误,请选择【账期】。");
                                    return false;
                                }
                            }
                            else
                            {
                                //如果是账期，但是又选择的是非 未付款
                                if (pm.Paytype_Name == DefaultPaymentMethod.账期.ToString())
                                {
                                    MessageBox.Show("付款方式错误,全部预付或部分预付时，请选择付款时使用的方式。");
                                    return false;
                                }
                            }
                        }
                    }
                }

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
                    if (EditEntity.Deposit > 0 || EditEntity.ForeignDeposit > 0)
                    {
                        MessageBox.Show("全部预付时，不需要输入订金,系统默认总金额为支付金额。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }
            }


            if (chk平台单.Checked && !string.IsNullOrEmpty(txtPlatformOrderNo.Text))
            {
                //检测平台单号。重复性提示
                tb_SaleOrderController<tb_SaleOrder> ctr = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
                tb_SaleOrder saleOrder = ctr.ExistFieldValueWithReturn(c => c.PlatformOrderNo == txtPlatformOrderNo.Text.Trim());
                if (NeedValidated && saleOrder != null)
                {
                    string empName = UIHelper.ShowGridColumnsNameValue(typeof(tb_SaleOrder), "Employee_ID", saleOrder.Employee_ID);
                    if (MessageBox.Show($"系统检测到相同平台单号的订单，订单号是：{saleOrder.SOrderNo},业务员是{empName}\r\n确定继续保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                    {
                        return false;
                    }
                }

            }


            var eer = errorProviderForAllInput.GetError(txtTotalAmount);

            bindingSourceSub.EndEdit();

            List<tb_SaleOrderDetail> oldOjb = new List<tb_SaleOrderDetail>(details.ToArray());

            List<tb_SaleOrderDetail> detailentity = bindingSourceSub.DataSource as List<tb_SaleOrderDetail>;

            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.ProdDetailID > 0).ToList();

                details.ForEach(
                    c =>
                    {
                        if (c.ProdDetailID > 0)
                        {
                            //if (c.SubtotalCostAmount != (c.Cost + c.CustomizedCost) * c.Quantity)
                            //{
                            //    c.SubtotalCostAmount = (c.Cost + c.CustomizedCost) * c.Quantity;
                            //}
                            //c.SubtotalTransAmount = c.TransactionPrice * c.Quantity;
                            //c.SubtotalTaxAmount = c.SubtotalTransAmount / (1 + c.TaxRate) * c.TaxRate;
                            if (c.CustomizedCost > 0)
                            {
                                EditEntity.IsCustomizedOrder = true;
                            }
                        }
                    }
                );

                EditEntity.TotalQty = details.Sum(c => c.Quantity);
                EditEntity.TotalCost = details.Sum(c => (c.Cost + c.CustomizedCost) * c.Quantity);

                EditEntity.TotalTaxAmount = details.Sum(c => c.SubtotalTaxAmount);
                EditEntity.TotalCommissionAmount = details.Sum(c => c.CommissionAmount);
                EditEntity.TotalAmount = details.Sum(c => c.TransactionPrice * c.Quantity) + EditEntity.FreightIncome;

                //如果没有有效的明细。直接提示
                if (NeedValidated && details.Count == 0)
                {
                    MessageBox.Show("请录入有效明细记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (NeedValidated && (EditEntity.FreightIncome > 0 && EditEntity.TotalAmount != detailentity.Sum(c => c.SubtotalTransAmount) + EditEntity.FreightIncome))
                {
                    MessageBox.Show("销售总金额需要包含运费。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                if (NeedValidated && (EditEntity.TotalAmount + EditEntity.FreightIncome < detailentity.Sum(c => c.TransactionPrice * c.Quantity)))
                {
                    MessageBox.Show("单据总金额不能小于明细总金额！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }



                //订单只是警告。可以继续

                EditEntity.tb_SaleOrderDetails = details;
                //var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                //if (aa.Count > 1)
                //{
                //    System.Windows.Forms.MessageBox.Show("明细中，相同的产品不能多行录入,如有需要,请另建单据保存!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}
                //为什么这么做呢？
                //foreach (var item in details)
                //{
                //    item.tb_saleorder = EditEntity;
                //}

                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_SaleOrderDetail>(details))
                {
                    return false;
                }

                if (!MainForm.Instance.AppContext.SysConfig.CheckNegativeInventory)
                {
                    list = await dc.BaseGetQueryableAsync().ToListAsync();
                    foreach (var item in details)
                    {
                        var detail = list.FirstOrDefault(c => c.ProdDetailID == item.ProdDetailID);
                        if (detail != null)
                        {
                            if (NeedValidated && (detail.Quantity - item.Quantity) < 0)
                            {
                                if (MessageBox.Show($"产品{detail.SKU},{detail.CNName},{detail.prop}的库存不足\r\n实际数量为：{detail.Quantity} ，拟销数量为： {item.Quantity}，是否继续保存？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }

                if (EditEntity.ApprovalStatus == null)
                {
                    EditEntity.ApprovalStatus = (int)ApprovalStatus.未审核;
                }
                if (NeedValidated && EditEntity.TotalAmount == 0)
                {
                    if (MessageBox.Show(this, "订单总金额数据为零\r\n你确定吗? ", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                    {
                        return false;
                    }
                }

                if (NeedValidated)
                {
                    //1 2  4  8  大于等于 4 就是审核或结案了
                    //if (EditEntity.tb_SaleOuts != null && EditEntity.tb_SaleOuts.Where(c => c.DataStatus >= 4).ToList().Count > 0)
                    //{
                    //    MessageBox.Show("当前订单已有销售出库数据，无法修改保存。请联系仓库处理。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    return false;
                    //}
                    //简单来处理就是要先删除出库数据
                    if (EditEntity.tb_SaleOuts != null && EditEntity.tb_SaleOuts.Count > 0)
                    {
                        MessageBox.Show("当前订单已有销售出库数据，无法修改保存。请联系仓库处理。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
                if (NeedValidated)
                {
                    if (EditEntity.tb_ProductionPlans != null && EditEntity.tb_ProductionPlans.Count > 0)
                    {
                        MessageBox.Show("当前订单已有计划单数据，无法修改保存。请联系仓库处理。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }


                ReturnMainSubResults<tb_SaleOrder> SaveResult = new ReturnMainSubResults<tb_SaleOrder>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.SOrderNo}。");
                    }
                    else
                    {
                        MainForm.Instance.PrintInfoLog($"保存失败,{SaveResult.ErrorMsg}。", Color.Red);
                    }
                }
                return SaveResult.Succeeded;

                /*
                if (EditEntity.SOrder_ID > 0)
                {
                    //如果是超级管理员，提供一个保存方式 就是在基本明细数据行不变时。只更新部分字段
                    if (NeedValidated && MainForm.Instance.AppContext.IsSuperUser)
                    {
                        if (MessageBox.Show("确定是部分数据更新吗？\r\n如有删除增加明细！请点【否】", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            ReturnMainSubResults<tb_SaleOrder> UpdateResult = await base.UpdateSave(EditEntity);
                            if (UpdateResult.Succeeded)
                            {
                                MainForm.Instance.PrintInfoLog($"更新成功，{EditEntity.SOrderNo}。");
                                return true;
                            }
                            else
                            {
                                MainForm.Instance.PrintInfoLog($"更新失败，{UpdateResult.ErrorMsg}。", Color.Red);
                                return false;
                            }
                        }
                        else
                        {
                            //更新式  要先删除前面的数据相关的数据
                            var SaveResult1 = await base.Save(EditEntity);
                            if (SaveResult1.Succeeded)
                            {
                                MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.SOrderNo}。");
                            }
                            else
                            {
                                MainForm.Instance.PrintInfoLog($"保存失败,{SaveResult1.ErrorMsg}。", Color.Red);
                            }
                            return SaveResult1.Succeeded;
                        }
                    }
                    else
                    {
                        //更新式  要先删除前面的数据相关的数据
                        ReturnMainSubResults<tb_SaleOrder> SaveResult = new ReturnMainSubResults<tb_SaleOrder>();
                        if (NeedValidated)
                        {
                            SaveResult = await base.Save(EditEntity);
                            if (SaveResult.Succeeded)
                            {
                                MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.SOrderNo}。");
                            }
                            else
                            {
                                MainForm.Instance.PrintInfoLog($"保存失败,{SaveResult.ErrorMsg}。", Color.Red);
                            }
                        }
                        return SaveResult.Succeeded;
                    }
                }
                else
                {

                    ReturnMainSubResults<tb_SaleOrder> SaveResult = new ReturnMainSubResults<tb_SaleOrder>();
                    if (NeedValidated)
                    {
                        SaveResult = await base.Save(EditEntity);
                        if (SaveResult.Succeeded)
                        {
                            MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.SOrderNo}。");
                        }
                        else
                        {
                            MainForm.Instance.PrintInfoLog($"保存失败,{SaveResult.ErrorMsg}。", Color.Red);
                        }
                    }
                    return SaveResult.Succeeded;
                }
                */
            }
            return false;
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
                        EditEntity.Employee_ID = cv.Employee_ID.Value;
                        cmbCustomerVendor_ID.EndUpdate();
                        //cmbEmployee_ID.EndUpdate();
                    }
                }
            }
        }

        /*
        protected override void PrintDesigned()
        {
            string ReprotfileName = typeof(tb_SaleOrder).Name + ".frx";
            //RptDesignForm frm = new RptDesignForm();
            //frm.ReportTemplateFile = ReprotfileName;
            // frm.ShowDialog();
            //调用内置方法 给数据源 新编辑，后面的话，直接load 可以不用给数据源的格式
            //string ReprotfileName = "SOB.frx";
            //List<tb_SaleOrder> main = new List<tb_SaleOrder>();
            //_EditEntity.tb_sales_order_details = details;
            //main.Add(_EditEntity);
            FastReport.Report FReport;
            FReport = new FastReport.Report();
            List<tb_SaleOrder> rptSources = new List<tb_SaleOrder>();
            tb_SaleOrder saleOrder = EditEntity as tb_SaleOrder;
            saleOrder.tb_SaleOrderDetails = details;
            rptSources.Add(saleOrder);

            FReport.RegisterData(rptSources, "报表数据源");
            String reportFile = string.Format("ReportTemplate/{0}", ReprotfileName);
            FReport.Load(reportFile);  //载入报表文件
            //FReport.FileName = "销售订单单据";
            FReport.Design();

        }
        */

        /*
          protected override void Preview()
        {
            //RptMainForm PRT = new RptMainForm();
            //PRT.ShowDialog();
            //return;
            //RptPreviewForm pForm = new RptPreviewForm();
            //pForm.ReprotfileName = "SOB.frx";
            //List<tb_SaleOrder> main = new List<tb_SaleOrder>();
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
              {
                  return;
              }

              //反审，要审核过，并且通过了，才能反审。
              if (EditEntity.ApprovalStatus.Value == (int)ApprovalStatus.已审核 && !EditEntity.ApprovalResults.HasValue)
              {
                  MainForm.Instance.uclog.AddLog("已经审核,且【同意】的单据才能反审核。");
                  return;
              }


              if (EditEntity.tb_SaleOrderDetails == null || EditEntity.tb_SaleOrderDetails.Count == 0)
              {
                  MainForm.Instance.uclog.AddLog("单据中没有明细数据，请确认录入了完整数量和金额。", UILogType.警告);
                  return;
              }

              RevertCommand command = new RevertCommand();
              //缓存当前编辑的对象。如果撤销就回原来的值
              tb_SaleOrder oldobj = CloneHelper.DeepCloneObject<tb_SaleOrder>(EditEntity);
              command.UndoOperation = delegate ()
              {
                  //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                  CloneHelper.SetValues<tb_SaleOrder>(EditEntity, oldobj);
              };

              tb_SaleOrderController<tb_SaleOrder> ctr = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
              List<tb_SaleOrder> list = new List<tb_SaleOrder>();
              list.Add(EditEntity);
              ReturnResults<bool> rr = await ctr.AntiApprovalAsync(list);
              if (rr.Succeeded)
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
                  MainForm.Instance.PrintInfoLog($"{EditEntity.SOrderNo}反审失败{rr.ErrorMsg},请联系管理员！", Color.Red);
              }

          }

          */
        protected async override Task<bool> AntiCloseCaseAsync()
        {
            if (EditEntity == null)
            {
                return false;
            }
            BillConverterFactory bcf = Startup.GetFromFac<BillConverterFactory>();
            CommonUI.frmOpinion frm = new CommonUI.frmOpinion();
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<tb_SaleOrder>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
            ApprovalEntity ae = new ApprovalEntity();
            ae.BillID = pkid;
            CommBillData cbd = bcf.GetBillData<tb_SaleOrder>(EditEntity);
            ae.BillNo = cbd.BillNo;
            ae.bizType = cbd.BizType;
            ae.bizName = cbd.BizName;
            ae.Approver_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
            frm.BindData(ae);
            if (frm.ShowDialog() == DialogResult.OK)//审核了。不管是同意还是不同意
            {
                List<tb_SaleOrder> EditEntitys = new List<tb_SaleOrder>();
                EditEntity.CloseCaseOpinions = frm.txtOpinion.Text;
                EditEntitys.Add(EditEntity);
                //已经审核的,结案了的才能反结案
                List<tb_SaleOrder> needCloseCases = EditEntitys.Where(c => c.DataStatus == (int)DataStatus.完结 && c.ApprovalStatus == (int)ApprovalStatus.已审核 && c.ApprovalResults.HasValue).ToList();
                if (needCloseCases.Count == 0)
                {
                    MainForm.Instance.PrintInfoLog($"要反结案的数据为：{needCloseCases.Count}:请检查数据！");
                    return false;
                }

                tb_SaleOrderController<tb_SaleOrder> ctr = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
                ReturnResults<bool> rs = await ctr.AntiBatchCloseCaseAsync(needCloseCases);
                if (rs.Succeeded)
                {
                    //if (MainForm.Instance.WorkflowItemlist.ContainsKey(""))
                    //{

                    //}
                    //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                    //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                    //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                    //MainForm.Instance.ecs.AddSendData(od);
                    MainForm.Instance.AuditLogHelper.CreateAuditLog<tb_SaleOrder>("反结案", EditEntity, $"反结案意见:{ae.CloseCaseOpinions}");
                    Refreshs();
                }
                else
                {
                    MainForm.Instance.PrintInfoLog($"{EditEntity.SOrderNo}反结案操作失败,原因是{rs.ErrorMsg},如果无法解决，请联系管理员！", Color.Red);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        protected async override Task<bool> CloseCaseAsync()
        {
            if (EditEntity == null)
            {
                return false;
            }
            BillConverterFactory bcf = Startup.GetFromFac<BillConverterFactory>();
            CommonUI.frmOpinion frm = new CommonUI.frmOpinion();
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<tb_SaleOrder>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
            ApprovalEntity ae = new ApprovalEntity();
            ae.BillID = pkid;
            CommBillData cbd = bcf.GetBillData<tb_SaleOrder>(EditEntity);
            ae.BillNo = cbd.BillNo;
            ae.bizType = cbd.BizType;
            ae.bizName = cbd.BizName;
            ae.Approver_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
            frm.BindData(ae);
            if (frm.ShowDialog() == DialogResult.OK)//审核了。不管是同意还是不同意
            {
                List<tb_SaleOrder> EditEntitys = new List<tb_SaleOrder>();
                EditEntity.CloseCaseOpinions = frm.txtOpinion.Text;
                EditEntitys.Add(EditEntity);
                //已经审核的并且通过的情况才能结案
                List<tb_SaleOrder> needCloseCases = EditEntitys.Where(c => c.DataStatus == (int)DataStatus.确认 && c.ApprovalStatus == (int)ApprovalStatus.已审核 && c.ApprovalResults.HasValue && c.ApprovalResults.Value).ToList();
                if (needCloseCases.Count == 0)
                {
                    MainForm.Instance.PrintInfoLog($"要结案的数据为：{needCloseCases.Count}:请检查数据！");
                    return false;
                }

                tb_SaleOrderController<tb_SaleOrder> ctr = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
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
                    MainForm.Instance.AuditLogHelper.CreateAuditLog<tb_SaleOrder>("结案", EditEntity, $"结案意见:{ae.CloseCaseOpinions}");
                    Refreshs();
                }
                else
                {
                    MainForm.Instance.PrintInfoLog($"{EditEntity.SOrderNo}结案操作失败,原因是{rs.ErrorMsg},如果无法解决，请联系管理员！", Color.Red);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private void chk非平台单_CheckedChanged(object sender, EventArgs e)
        {

        }

        #region 付款调整
        ToolStripButton toolStripButton付款调整 = new System.Windows.Forms.ToolStripButton();
        ToolStripButton toolStripButton定制成本确认 = new System.Windows.Forms.ToolStripButton();

        /// <summary>
        /// 添加回收
        /// </summary>
        /// <returns></returns>
        public override ToolStripItem[] AddExtendButton(tb_MenuInfo menuInfo)
        {
            toolStripButton定制成本确认.Text = "定制成本确认";
            toolStripButton定制成本确认.Image = global::RUINORERP.UI.Properties.Resources.Assignment;
            toolStripButton定制成本确认.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton定制成本确认.Name = "定制成本确认";
            toolStripButton定制成本确认.Visible = false;//默认隐藏
            UIHelper.ControlButton<ToolStripButton>(CurMenuInfo, toolStripButton定制成本确认);
            toolStripButton定制成本确认.ToolTipText = "定制订单时，产品的成本有变化，额外增加或减少的成本要在明细中体现，使用本功能。";
            toolStripButton定制成本确认.Click += new System.EventHandler(this.toolStripButton定制成本确认_Click);


            toolStripButton付款调整.Text = "付款调整";
            toolStripButton付款调整.Image = global::RUINORERP.UI.Properties.Resources.Assignment;
            toolStripButton付款调整.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton付款调整.Name = "付款调整";
            toolStripButton付款调整.Visible = false;//默认隐藏
            UIHelper.ControlButton<ToolStripButton>(CurMenuInfo, toolStripButton付款调整);
            toolStripButton付款调整.ToolTipText = "客户付款情况变动时，使用本功能。";
            toolStripButton付款调整.Click += new System.EventHandler(this.toolStripButton付款调整_Click);


            toolStripButton反结案.Text = "反结案";
            toolStripButton反结案.Image = global::RUINORERP.UI.Properties.Resources.Assignment;
            toolStripButton反结案.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton反结案.Name = "反结案";
            toolStripButton反结案.Visible = false;//默认隐藏
            UIHelper.ControlButton<ToolStripButton>(CurMenuInfo, toolStripButton反结案);
            toolStripButton反结案.ToolTipText = "结案错误，要上级特殊处理时，使用本功能。";
            toolStripButton反结案.Click += new System.EventHandler(this.toolStripButton反结案_Click);

            System.Windows.Forms.ToolStripItem[] extendButtons = new System.Windows.Forms.ToolStripItem[]
            { toolStripButton付款调整,
                toolStripButton定制成本确认,
                toolStripButton反结案 };

            this.BaseToolStrip.Items.AddRange(extendButtons);
            return extendButtons;
        }

        private void toolStripButton定制成本确认_Click(object sender, EventArgs e)
        {
            UpdateCustomizedCost();
        }

        private async void UpdateCustomizedCost()
        {
            if (EditEntity == null)
            {
                return;
            }

            //反审，要审核过，并且通过了，才能反审。
            if (EditEntity.ApprovalStatus.Value == (int)ApprovalStatus.已审核 && !EditEntity.ApprovalResults.HasValue)
            {
                MainForm.Instance.uclog.AddLog("已经审核,且【同意】的单据才能更新付款状态。");
                return;
            }

            RevertCommand command = new RevertCommand();
            //缓存当前编辑的对象。如果撤销就回原来的值
            tb_SaleOrder oldobj = CloneHelper.DeepCloneObject_maxnew<tb_SaleOrder>(EditEntity);
            command.UndoOperation = delegate ()
            {
                //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                CloneHelper.SetValues<tb_SaleOrder>(EditEntity, oldobj);
            };

            tb_SaleOrderController<tb_SaleOrder> ctr = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();

            if (MessageBox.Show("你确定要调整当前订单的付款状态吗？", "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            {
                return;
            }

            if (AppContext.IsSuperUser)
            {
                await Save(true);
                await ctr.BaseSaveOrUpdate(EditEntity);
                return;
            }

            ReturnResults<bool> rr = await ctr.UpdatePaymentStatus(EditEntity);
            if (rr.Succeeded)
            {
                MainForm.Instance.AuditLogHelper.CreateAuditLog<tb_SaleOrder>("付款调整", EditEntity);
                //if (MainForm.Instance.WorkflowItemlist.ContainsKey(""))
                //{

                //}
                //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                //MainForm.Instance.ecs.AddSendData(od);
                base.ToolBarEnabledControl(MenuItemEnums.结案);
            }
            else
            {
                //付款调整失败 要恢复之前的值
                command.Undo();
                MainForm.Instance.PrintInfoLog($"{EditEntity.SOrderNo}付款调整失败{rr.ErrorMsg},请联系管理员！", Color.Red);
            }

        }

        private void toolStripButton付款调整_Click(object sender, EventArgs e)
        {
            UpdatePaymentStatus();
        }




        #endregion

        #region 反结案
        ToolStripButton toolStripButton反结案 = new System.Windows.Forms.ToolStripButton();



        private async void toolStripButton反结案_Click(object sender, EventArgs e)
        {
            if (EditEntity == null)
            {
                return;
            }
            if (EditEntity.DataStatus != (int)DataStatus.完结)
            {
                MessageBox.Show("只能对已【完结】的单据操作。\r\n请检查数据，刷新后重试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            await AntiCloseCaseAsync();
        }




        #endregion

        private void cmbPayStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void kryptonPanelMainInfo_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblFreightIncome_Click(object sender, EventArgs e)
        {

        }

        private void txtFreightIncome_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
