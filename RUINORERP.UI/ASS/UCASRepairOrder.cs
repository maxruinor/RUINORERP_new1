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
using RUINORERP.UI.Network.Services;
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
using RUINORERP.UI.PSI.SAL;
using RUINORERP.Common.Extensions;
using NPOI.SS.Formula.Functions;
using ICSharpCode.SharpZipLib.Tar;
using RUINORERP.Extensions.Middlewares;


namespace RUINORERP.UI.ASS
{
    [MenuAttrAssemblyInfo("维修工单", ModuleMenuDefine.模块定义.售后管理, ModuleMenuDefine.售后管理.维修中心, BizType.维修工单)]
    public partial class UCASRepairOrder : BaseBillEditGeneric<tb_AS_RepairOrder, tb_AS_RepairOrderDetail>, IPublicEntityObject
    {
        public UCASRepairOrder()
        {
            InitializeComponent();
            //InitDataToCmbByEnumDynamicGeneratedDataSource<tb_AS_RepairOrder>(typeof(Priority), e => e.OrderPriority, cmbOrderPriority);
            AddPublicEntityObject(typeof(ProductSharePart));
        }

        protected override async Task LoadRelatedDataToDropDownItemsAsync()
        {
            if (base.EditEntity is tb_AS_RepairOrder RepairOrder)
            {


                if (RepairOrder.ASApplyID.HasValue && RepairOrder.ASApplyID.Value > 0)
                {
                    RelatedQueryParameter rqp = new RelatedQueryParameter();
                    rqp.bizType = BizType.售后申请单;
                    rqp.billId = RepairOrder.ASApplyID.Value;
                    rqp.billNo = RepairOrder.ASApplyNo;
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
                if (RepairOrder.tb_AS_RepairInStocks != null && RepairOrder.tb_AS_RepairInStocks.Count > 0)
                {
                    foreach (var item in RepairOrder.tb_AS_RepairInStocks)
                    {
                        RelatedQueryParameter rqp = new RelatedQueryParameter();
                        rqp.bizType = BizType.维修入库单;
                        rqp.billId = item.RepairInStockID;
                        ToolStripMenuItem RelatedMenuItem = new ToolStripMenuItem();
                        RelatedMenuItem.Name = $"{rqp.billId}";
                        RelatedMenuItem.Tag = rqp;
                        RelatedMenuItem.Text = $"{rqp.bizType}:{item.RepairInStockNo}";
                        RelatedMenuItem.Click += base.MenuItem_Click;
                        if (!toolStripbtnRelatedQuery.DropDownItems.ContainsKey(item.RepairInStockID.ToString()))
                        {
                            toolStripbtnRelatedQuery.DropDownItems.Add(RelatedMenuItem);
                        }
                    }
                }

                if (RepairOrder.tb_AS_RepairMaterialPickups != null && RepairOrder.tb_AS_RepairMaterialPickups.Count > 0)
                {
                    foreach (var item in RepairOrder.tb_AS_RepairMaterialPickups)
                    {
                        RelatedQueryParameter rqp = new RelatedQueryParameter();
                        rqp.bizType = BizType.维修领料单;
                        rqp.billId = item.RMRID;
                        ToolStripMenuItem RelatedMenuItem = new ToolStripMenuItem();
                        RelatedMenuItem.Name = $"{rqp.billId}";
                        RelatedMenuItem.Tag = rqp;
                        RelatedMenuItem.Text = $"{rqp.bizType}:{item.MaterialPickupNO}";
                        RelatedMenuItem.Click += base.MenuItem_Click;
                        if (!toolStripbtnRelatedQuery.DropDownItems.ContainsKey(item.RMRID.ToString()))
                        {
                            toolStripbtnRelatedQuery.DropDownItems.Add(RelatedMenuItem);
                        }
                    }
                }

                //如果有出库，则查应收
                if (RepairOrder.DataStatus >= (int)DataStatus.确认)
                {
                    var receivablePayables = await MainForm.Instance.AppContext.Db.Queryable<tb_FM_ReceivablePayable>()
                                                                    .Where(c => c.ARAPStatus >= (int)ARAPStatus.待审核
                                                                    && c.CustomerVendor_ID == RepairOrder.CustomerVendor_ID
                                                                    && c.SourceBillId == RepairOrder.RepairOrderID)
                                                                    .ToListAsync();
                    foreach (var item in receivablePayables)
                    {
                        var rqpara = new Model.CommonModel.RelatedQueryParameter();
                        rqpara.bizType = BizType.应付款单;
                        rqpara.billId = item.ARAPId;
                        ToolStripMenuItem RelatedMenuItemPara = new ToolStripMenuItem();
                        RelatedMenuItemPara.Name = $"{rqpara.billId}";
                        RelatedMenuItemPara.Tag = rqpara;
                        RelatedMenuItemPara.Text = $"{rqpara.bizType}:{item.ARAPNo}";
                        RelatedMenuItemPara.Click += base.MenuItem_Click;
                        if (!toolStripbtnRelatedQuery.DropDownItems.ContainsKey(item.ARAPId.ToString()))
                        {
                            toolStripbtnRelatedQuery.DropDownItems.Add(RelatedMenuItemPara);
                        }
                    }
                }

            }
            await base.LoadRelatedDataToDropDownItemsAsync();
        }


 


        #region 维修各种动作

        ToolStripButton toolStripButton维修处理 = new System.Windows.Forms.ToolStripButton();
        ToolStripButton toolStripButton维修领料 = new System.Windows.Forms.ToolStripButton();
        /// <summary>
        /// 添加回收
        /// </summary>
        /// <returns></returns>
        public override ToolStripItem[] AddExtendButton(tb_MenuInfo menuInfo)
        {

            toolStripButton维修处理.Text = "维修处理";
            toolStripButton维修处理.Image = global::RUINORERP.UI.Properties.Resources.Assignment;
            toolStripButton维修处理.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton维修处理.Name = "维修处理";
            toolStripButton维修处理.Visible = false;//默认隐藏
            UIHelper.ControlButton<ToolStripButton>(CurMenuInfo, toolStripButton维修处理);
            toolStripButton维修处理.ToolTipText = "对售后申请单的产品进行维修处理及报价，生成维修工单，使用本功能。";
            toolStripButton维修处理.Click += new System.EventHandler(this.toolStripButton维修处理_Click);


            toolStripButton维修领料.Text = "维修领料";
            toolStripButton维修领料.Image = global::RUINORERP.UI.Properties.Resources.Assignment;
            toolStripButton维修领料.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton维修领料.Name = "维修领料";
            toolStripButton维修领料.Visible = false;//默认隐藏
            UIHelper.ControlButton<ToolStripButton>(CurMenuInfo, toolStripButton维修领料);
            toolStripButton维修领料.ToolTipText = "对售后申请单的产品进行维修领料及报价，生成维修工单，使用本功能。";
            toolStripButton维修领料.Click += new System.EventHandler(this.toolStripButton维修领料_Click);


            System.Windows.Forms.ToolStripItem[] extendButtons = new System.Windows.Forms.ToolStripItem[]
            { toolStripButton维修处理,toolStripButton维修领料};

            this.BaseToolStrip.Items.AddRange(extendButtons);
            return extendButtons;
        }
        private  async void toolStripButton维修处理_Click(object sender, EventArgs e)
        {
            if (EditEntity == null)
            {
                return;
            }

            if (EditEntity != null)
            {
                //只有审核状态才可以转换
                if (EditEntity.DataStatus == (int)DataStatus.确认 && EditEntity.ApprovalStatus == (int)ApprovalStatus.已审核 && EditEntity.ApprovalResults.HasValue && EditEntity.ApprovalResults.Value)
                {
                    if (EditEntity.RepairStatus.HasValue && EditEntity.RepairStatus.Value != (int)RepairStatus.待维修)
                    {
                        MessageBox.Show($"当前【维修工单】的维修状态为:{(RepairStatus)EditEntity.RepairStatus.Value}，无法重复进行维修处理", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        toolStripButton维修处理.Enabled = false;
                        return;
                    }

                    if (EditEntity.PayStatus == (int)PayStatus.未付款)
                    {
                        if (MessageBox.Show($"当前【维修工单】的付款状态为:{(PayStatus)EditEntity.PayStatus}，你确定仍要进行【维修处理】吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No)
                        {
                            return;
                        }
                    }

                    var ctr = Startup.GetFromFac<tb_AS_RepairOrderController<tb_AS_RepairOrder>>();
                    ReturnResults<tb_AS_RepairOrder> rrs = await ctr.RepairProcessAsync(EditEntity);
                    if (rrs.Succeeded)
                    {
                        toolStripButton维修领料.Enabled = true;
                        toolStripButton维修处理.Enabled = false;
                        MainForm.Instance.AuditLogHelper.CreateAuditLog<tb_AS_RepairOrder>("维修处理", EditEntity);
                        MessageBox.Show($"当前【维修工单】的产品，将从【售后暂存仓】出库，【全部】交由维修人员处理", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    }
                    else
                    {
                        MessageBox.Show($"当前【维修工单】维修处理失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return;
                }
                else
                {
                    MessageBox.Show($"当前【维修工单】未审核，无法进行【维修处理】", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        }

        private void toolStripButton维修领料_Click(object sender, EventArgs e)
        {
            if (EditEntity == null)
            {
                return;
            }

            if (EditEntity != null)
            {
                //只有审核状态才可以转换
                if (EditEntity.DataStatus == (int)DataStatus.确认 && EditEntity.ApprovalStatus == (int)ApprovalStatus.已审核 && EditEntity.ApprovalResults.HasValue && EditEntity.ApprovalResults.Value)
                {
                    if (EditEntity.RepairStatus.HasValue && EditEntity.RepairStatus.Value != (int)RepairStatus.维修中)
                    {
                        MessageBox.Show($"【维修工单】{EditEntity.ASApplyNo}的维修状态为：{(RepairStatus)EditEntity.RepairStatus.Value}，请【维修处理】后，才能进行领料处理", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        toolStripButton维修领料.Enabled = false;
                        return;
                    }


                    if (EditEntity.tb_AS_RepairMaterialPickups != null && EditEntity.tb_AS_RepairMaterialPickups.Count > 0)
                    {
                        if (EditEntity.tb_AS_RepairMaterialPickups.Where(c => c.DataStatus >= (int)DataStatus.确认).Sum(c => c.TotalSendQty) == EditEntity.TotalQty)
                        {
                            MessageBox.Show($"当前【维修工单】的维修材料已全部领出，无法重复领取", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }

                    var ctr = Startup.GetFromFac<tb_AS_RepairMaterialPickupController<tb_AS_RepairMaterialPickup>>();
                    tb_AS_RepairMaterialPickup RepairOrder = ctr.ToRepairMaterialPickup(EditEntity);
                    MenuPowerHelper menuPowerHelper;
                    menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
                    tb_MenuInfo RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == nameof(tb_AS_RepairMaterialPickup) && m.BIBaseForm == "BaseBillEditGeneric`2").FirstOrDefault();
                    if (RelatedMenuInfo != null)
                    {
                        menuPowerHelper.ExecuteEvents(RelatedMenuInfo, RepairOrder);
                    }
                    return;
                }
                else
                {
                    MessageBox.Show($"当前【维修工单】{EditEntity.ASApplyNo}：状态为：{(DataStatus)EditEntity.DataStatus}，无法进行【维修领料】", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        }

        #endregion

        public override void QueryConditionBuilder()
        {
            base.QueryConditionBuilder();
            var lambda = Expressionable.Create<tb_AS_RepairOrder>()
            .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
            .ToExpression();
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);

        }


        internal override void LoadDataToUI(object Entity)
        {
            BindData(Entity as tb_AS_RepairOrder);
        }



        public override void BindData(tb_AS_RepairOrder entity, ActionStatus actionStatus = ActionStatus.无操作)
        {
            if (entity == null)
            {

                return;
            }
            cmbRepairStatus.Enabled = false;
            base.EditEntity = entity;
            if (entity != null)
            {
                if (entity.RepairOrderID > 0)
                {
                    entity.PrimaryKeyID = entity.RepairOrderID;
                    entity.ActionStatus = ActionStatus.加载;
                    //entity.DataStatus = (int)DataStatus.确认;
                    //如果审核了，审核要灰色
                    if (EditEntity.RepairStatus.HasValue && EditEntity.RepairStatus.Value == (int)RepairStatus.待维修)
                    {
                        toolStripButton维修处理.Enabled = true;
                    }
                    else
                    {
                        toolStripButton维修处理.Enabled = false;
                    }


                    if (EditEntity.RepairStatus.HasValue && EditEntity.RepairStatus.Value == (int)RepairStatus.维修中)
                    {
                        toolStripButton维修领料.Enabled = true;
                        toolStripBtnReverseReview.Enabled = false;//维修处理过了。不能反审核了
                        toolStripBtnReverseReview.ToolTipText = "【维修处理】后不能反审核。";
                    }

                }
                else
                {
                    entity.ActionStatus = ActionStatus.新增;
                    entity.DataStatus = (int)DataStatus.草稿;
                    if (string.IsNullOrEmpty(entity.RepairOrderNo))
                    {
                        entity.RepairOrderNo = BizCodeService.GetBizBillNo(BizType.维修工单);
                    }
                    entity.Employee_ID = AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                    if (entity.tb_AS_RepairOrderDetails != null && entity.tb_AS_RepairOrderDetails.Count > 0)
                    {
                        entity.tb_AS_RepairOrderDetails.ForEach(c => c.RepairOrderID = 0);
                        entity.tb_AS_RepairOrderDetails.ForEach(c => c.RepairOrderDetailID = 0);
                    }

                    //新建时  是未付款和账期
                    entity.PayStatus = (int)PayStatus.未付款;
                    entity.Paytype_ID = MainForm.Instance.AppContext.PaymentMethodOfPeriod.Paytype_ID;
                    entity.RepairStatus = (int)RepairStatus.评估报价;
                }
            }

            if (entity.DataStatus >= (int)DataStatus.确认)
            {
                DataBindingHelper.BindData4CmbByEnum<tb_AS_RepairOrder, PayStatus>(entity, k => k.PayStatus, cmbPayStatus, false);
                cmbRepairStatus.Enabled = false;
            }
            else
            {
                DataBindingHelper.BindData4CmbByEnum<tb_AS_RepairOrder, PayStatus>(entity, k => k.PayStatus, cmbPayStatus, false, PayStatus.全部付款, PayStatus.部分付款, PayStatus.部分预付);
                cmbRepairStatus.Enabled = true;
            }

            if (entity.ApprovalStatus.HasValue)
            {
                lblReview.Text = ((ApprovalStatus)entity.ApprovalStatus).ToString();
            }
            EditEntity = entity;
            //==
            DataBindingHelper.BindData4TextBox<tb_AS_RepairOrder>(entity, t => t.RepairOrderNo, txtRepairOrderNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v => v.ProjectGroupName, cmbProjectGroup_ID);
            DataBindingHelper.BindData4CmbByEnum<tb_AS_RepairOrder>(entity, k => k.RepairStatus, typeof(RepairStatus), cmbRepairStatus, true);


            DataBindingHelper.BindData4Cmb<tb_PaymentMethod>(entity, k => k.Paytype_ID, v => v.Paytype_Name, cmbPaytype_ID);
            DataBindingHelper.BindData4TextBox<tb_AS_RepairOrder>(entity, t => t.TotalQty, txtTotalQty, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_AS_RepairOrder>(entity, t => t.LaborCost.ToString(), txtLaborCost, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_AS_RepairOrder>(entity, t => t.TotalMaterialAmount.ToString(), txtTotalMaterialAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_AS_RepairOrder>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_AS_RepairOrder>(entity, t => t.CustomerPaidAmount.ToString(), txtCustomerPaidAmount, BindDataType4TextBox.Money, false);

            DataBindingHelper.BindData4CmbByEnum<tb_AS_RepairOrder>(entity, k => k.ExpenseAllocationMode, typeof(ExpenseAllocationMode), cmbExpenseAllocationMode, true);
            DataBindingHelper.BindData4CmbByEnum<tb_AS_RepairOrder>(entity, k => k.ExpenseBearerType, typeof(ExpenseBearerType), cmbExpenseBearerType, true);

            DataBindingHelper.BindData4DataTime<tb_AS_RepairOrder>(entity, t => t.RepairStartDate, dtpRepairStartDate, false);
            DataBindingHelper.BindData4DataTime<tb_AS_RepairOrder>(entity, t => t.PreDeliveryDate, dtpPreDeliveryDate, false);
            DataBindingHelper.BindData4TextBox<tb_AS_RepairOrder>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_AS_RepairOrder>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_AS_RepairOrder>(entity, t => t.TotalMaterialCost.ToString(), txtTotalMaterialCost, BindDataType4TextBox.Money, false);

            //==

            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, c => c.IsCustomer == true);
            base.errorProviderForAllInput.DataSource = entity;
            base.errorProviderForAllInput.ContainerControl = this;
            //this.ValidateChildren();
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            DataBindingHelper.BindData4ControlByEnum<tb_AS_RepairOrder>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_AS_RepairOrder>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));
            if (entity.tb_AS_RepairOrderDetails != null && entity.tb_AS_RepairOrderDetails.Count > 0)
            {
                //LoadDataToGrid(entity.tb_AS_RepairOrderDetails);
                sgh.LoadItemDataToGrid<tb_AS_RepairOrderDetail>(grid1, sgd, entity.tb_AS_RepairOrderDetails, c => c.ProdDetailID);
            }
            else
            {
                //LoadDataToGrid(new List<tb_AS_RepairOrderDetail>());
                sgh.LoadItemDataToGrid<tb_AS_RepairOrderDetail>(grid1, sgd, new List<tb_AS_RepairOrderDetail>(), c => c.ProdDetailID);
            }

            if (entity.tb_AS_RepairOrderMaterialDetails != null && entity.tb_AS_RepairOrderMaterialDetails.Count > 0)
            {
                //LoadDataToGrid(entity.tb_AS_RepairOrderDetails);
                sgh2.LoadItemDataToGrid<tb_AS_RepairOrderMaterialDetail>(grid2, sgd2, entity.tb_AS_RepairOrderMaterialDetails, c => c.ProdDetailID);
            }
            else
            {
                //LoadDataToGrid(new List<tb_AS_RepairOrderDetail>());
                sgh2.LoadItemDataToGrid<tb_AS_RepairOrderMaterialDetail>(grid2, sgd2, new List<tb_AS_RepairOrderMaterialDetail>(), c => c.ProdDetailID);
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
            DataBindingHelper.BindData4TextBox<tb_AS_RepairOrder>(entity, v => v.ASApplyNo, txtASApplyID, BindDataType4TextBox.Text, true);

            //创建表达式  草稿 结案 和没有提交的都不显示
            var lambdaSO = Expressionable.Create<tb_AS_AfterSaleApply>()
                            .And(t => t.DataStatus == (int)DataStatus.确认)
                             .And(t => t.isdeleted == false)
                             .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && !AppContext.IsSuperUser, t => t.Employee_ID == AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的
                            .ToExpression();//注意 这一句 不能少
            BaseProcessor baseProcessorSO = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_AS_AfterSaleApply).Name + "Processor");
            QueryFilter queryFilterSO = baseProcessorSO.GetQueryFilter();
            queryFilterSO.FilterLimitExpressions.Add(lambdaSO);

            ControlBindingHelper.ConfigureControlFilter<tb_AS_RepairOrder, tb_AS_AfterSaleApply>(entity, txtASApplyID, t => t.ASApplyNo,
              f => f.ASApplyNo, queryFilterSO, a => a.ASApplyID, b => b.ASApplyID, null, false);

            ToolBarEnabledControl(entity);

            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {                //数据状态变化会影响按钮变化
                if (s2.PropertyName == entity.GetPropertyName<tb_AS_RepairOrder>(c => c.DataStatus))
                {
                    ToolBarEnabledControl(entity);
                }
                //权限允许
                if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
                {
                    if (s2.PropertyName == entity.GetPropertyName<tb_AS_RepairOrder>(c => c.LaborCost) && entity.LaborCost > 0)
                    {
                        //默认为账期
                        entity.TotalAmount = entity.LaborCost + entity.TotalMaterialAmount;
                    }

                    if (s2.PropertyName == entity.GetPropertyName<tb_AS_RepairOrder>(c => c.PayStatus) && entity.PayStatus == (int)PayStatus.未付款)
                    {
                        //默认为账期
                        entity.Paytype_ID = MainForm.Instance.AppContext.PaymentMethodOfPeriod.Paytype_ID;
                    }
                    if (s2.PropertyName == entity.GetPropertyName<tb_AS_RepairOrder>(c => c.Paytype_ID) && entity.Paytype_ID == MainForm.Instance.AppContext.PaymentMethodOfPeriod.Paytype_ID)
                    {
                        //默认为未付款
                        entity.PayStatus = (int)PayStatus.未付款;
                    }
                    if (s2.PropertyName == entity.GetPropertyName<tb_AS_RepairOrder>(c => c.Paytype_ID) && entity.Paytype_ID > 0)
                    {
                        if (cmbPaytype_ID.SelectedItem is tb_PaymentMethod paymentMethod)
                        {
                            EditEntity.tb_paymentmethod = paymentMethod;
                        }
                    }

                    if (s2.PropertyName == entity.GetPropertyName<tb_AS_RepairOrder>(c => c.ProjectGroup_ID) && entity.ProjectGroup_ID.HasValue && entity.ProjectGroup_ID > 0)
                    {
                        if (cmbProjectGroup_ID.SelectedItem is tb_ProjectGroup ProjectGroup)
                        {
                            if (ProjectGroup.tb_ProjectGroupAccountMappers != null && ProjectGroup.tb_ProjectGroupAccountMappers.Count > 0)
                            {
                                EditEntity.tb_projectgroup = ProjectGroup;
                            }
                        }
                    }
                }
                //如果是销售订单引入变化则加载明细及相关数据
                if ((entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改) && entity.ASApplyID.HasValue && entity.ASApplyID.Value > 0 && s2.PropertyName == entity.GetPropertyName<tb_AS_RepairOrder>(c => c.ASApplyID))
                {
                    ToRepairOrder(entity.ASApplyID);
                }

                if (entity.CustomerVendor_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_AS_AfterSaleApply>(c => c.CustomerVendor_ID))
                {
                    var obj = MyCacheManager.Instance.GetEntity<tb_CustomerVendor>(entity.CustomerVendor_ID);
                    if (obj != null && obj.ToString() != "System.Object")
                    {
                        if (obj is tb_CustomerVendor cv)
                        {
                            EditEntity.Employee_ID = cv.Employee_ID.Value;
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

        /*
         重要思路提示，这个表格控件，数据源的思路是，
        产品共享表 ProductSharePart+tb_AS_RepairOrderDetail 有合体，SetDependencyObject 根据字段名相同的给值，值来自产品明细表
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
            //base.FieldNameList = UIHelper.GetFieldNameList<tb_AS_RepairOrderDetail>();

            grid1.BorderStyle = BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;

            List<SGDefineColumnItem> listCols = new List<SGDefineColumnItem>();
            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<ProductSharePart, tb_AS_RepairOrderDetail>(c => c.ProdDetailID, false);

            listCols.SetCol_NeverVisible<tb_AS_RepairOrderDetail>(c => c.RepairOrderID);
            listCols.SetCol_NeverVisible<tb_AS_RepairOrderDetail>(c => c.RepairOrderDetailID);
            listCols.SetCol_NeverVisible<tb_AS_RepairOrderDetail>(c => c.ProdDetailID);
            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);

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
                listCols.SetCol_Summary<tb_AS_RepairOrderDetail>(c => c.TotalCostAmount, true);
                listCols.SetCol_NeverVisible<tb_AS_RepairOrderDetail>(c => c.Cost);
                listCols.SetCol_NeverVisible<tb_AS_RepairOrderDetail>(c => c.TotalCostAmount);
            }*/
            listCols.SetCol_Summary<tb_AS_RepairOrderDetail>(c => c.Quantity);



            // listCols.SetCol_Formula<tb_AS_RepairOrderDetail>((a, b, c) => a.TransactionPrice * b.Quantity - c.TaxSubtotalAmount, d => d.UntaxedAmount);

            //应该是审核时要处理的逻辑暂时隐藏
            //将数量默认为已出库数量
            //listCols.SetCol_Formula<tb_AS_RepairOrderDetail>((a, b) => a.Quantity, c => c.TotalReturnedQty);
            //listCols.SetCol_NeverVisible<tb_AS_RepairOrderDetail>(c => c.TotalReturnedQty);

            //sgh.SetPointToColumnPairs<ProductSharePart, tb_AS_RepairOrderDetail>(sgd, f => f.Inv_Cost, t => t.MaterialCost);
            //sgh.SetPointToColumnPairs<ProductSharePart, tb_AS_RepairOrderDetail>(sgd, f => f.Standard_Price, t => t.MaterialCost);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_AS_RepairOrderDetail>(sgd, f => f.prop, t => t.property);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_AS_RepairOrderDetail>(sgd, f => f.Location_ID, t => t.Location_ID);



            //应该只提供一个结构
            List<tb_AS_RepairOrderDetail> lines = new List<tb_AS_RepairOrderDetail>();
            bindingSourceSub.DataSource = lines;
            sgd.BindingSourceLines = bindingSourceSub;
            //    Expression<Func<View_ProdDetail, bool>> exp = Expressionable.Create<View_ProdDetail>() //创建表达式
            // .AndIF(true, w => w.CNName.Length > 0)
            //// .AndIF(txtSpecifications.Text.Trim().Length > 0, w => w.Specifications.Contains(txtSpecifications.Text.Trim()))
            //.ToExpression();//注意 这一句 不能少
            //                // StringBuilder sb = new StringBuilder();
            //    /// sb.Append(string.Format("{0}='{1}'", item.ColName, valValue));
            //    list = dc.BaseQueryByWhere(exp);
            list = MainForm.Instance.View_ProdDetailList;
            sgd.SetDependencyObject<ProductSharePart, tb_AS_RepairOrderDetail>(list);
            sgd.HasRowHeader = true;
            sgh.InitGrid(grid1, sgd, true, nameof(tb_AS_RepairOrderDetail));
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
            listCols = sgh2.GetGridColumns<ProductSharePart, tb_AS_RepairOrderMaterialDetail>(c => c.ProdDetailID, false);

            listCols.SetCol_NeverVisible<tb_AS_RepairOrderMaterialDetail>(c => c.RepairMaterialDetailID);
            listCols.SetCol_NeverVisible<tb_AS_RepairOrderMaterialDetail>(c => c.RepairOrderID);
            listCols.SetCol_NeverVisible<tb_AS_RepairOrderMaterialDetail>(c => c.ProdDetailID);
            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);

            listCols.SetCol_Format<tb_AS_RepairOrderMaterialDetail>(c => c.TaxRate, CustomFormatType.PercentFormat);

            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Location_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Rack_ID);
            listCols.SetCol_ReadOnly<tb_AS_RepairOrderMaterialDetail>(c => c.ActualSentQty);
            if (!AppContext.SysConfig.UseBarCode)
            {
                listCols.SetCol_NeverVisible<ProductSharePart>(c => c.BarCode);
            }
            sgd2 = new SourceGridDefine(grid2, listCols, true);

            /*
            //具体审核权限的人才显示
            if (AppContext.CurUserInfo.UserButtonList.Where(c => c.BtnText == MenuItemEnums.审核.ToString()).Any())
            {
                listCols.SetCol_Summary<tb_AS_RepairOrderDetail>(c => c.TotalCostAmount, true);
                listCols.SetCol_NeverVisible<tb_AS_RepairOrderDetail>(c => c.Cost);
                listCols.SetCol_NeverVisible<tb_AS_RepairOrderDetail>(c => c.TotalCostAmount);
            }*/
            listCols.SetCol_Summary<tb_AS_RepairOrderMaterialDetail>(c => c.ShouldSendQty);
            listCols.SetCol_Summary<tb_AS_RepairOrderMaterialDetail>(c => c.ActualSentQty);
            listCols.SetCol_Summary<tb_AS_RepairOrderMaterialDetail>(c => c.SubtotalTransAmount);
            listCols.SetCol_Summary<tb_AS_RepairOrderMaterialDetail>(c => c.SubtotalCost);
            listCols.SetCol_Summary<tb_AS_RepairOrderMaterialDetail>(c => c.SubtotalTaxAmount);
            listCols.SetCol_Summary<tb_AS_RepairOrderMaterialDetail>(c => c.SubtotalUntaxedAmount);

            listCols.SetCol_Formula<tb_AS_RepairOrderMaterialDetail>((a, b) => a.Cost * b.ShouldSendQty, c => c.SubtotalCost);
            listCols.SetCol_Formula<tb_AS_RepairOrderMaterialDetail>((a, b) => a.UnitPrice * b.ShouldSendQty, c => c.SubtotalTransAmount);
            listCols.SetCol_Formula<tb_AS_RepairOrderMaterialDetail>((a, b) => a.SubtotalTransAmount - b.SubtotalTaxAmount, c => c.SubtotalUntaxedAmount);
            listCols.SetCol_Formula<tb_AS_RepairOrderMaterialDetail>((a, b, c) => a.SubtotalTransAmount / (1 + b.TaxRate) * c.TaxRate, d => d.SubtotalTaxAmount);

            //应该是审核时要处理的逻辑暂时隐藏
            //将数量默认为已出库数量
            //listCols.SetCol_Formula<tb_AS_RepairOrderDetail>((a, b) => a.Quantity, c => c.TotalReturnedQty);
            //listCols.SetCol_NeverVisible<tb_AS_RepairOrderDetail>(c => c.TotalReturnedQty);

            sgh2.SetPointToColumnPairs<ProductSharePart, tb_AS_RepairOrderMaterialDetail>(sgd2, f => f.Inv_Cost, t => t.Cost);
            sgh2.SetPointToColumnPairs<ProductSharePart, tb_AS_RepairOrderMaterialDetail>(sgd2, f => f.Standard_Price, t => t.UnitPrice);
            sgh2.SetPointToColumnPairs<ProductSharePart, tb_AS_RepairOrderMaterialDetail>(sgd2, f => f.prop, t => t.property);
            sgh2.SetPointToColumnPairs<ProductSharePart, tb_AS_RepairOrderMaterialDetail>(sgd2, f => f.Location_ID, t => t.Location_ID);



            //应该只提供一个结构
            List<tb_AS_RepairOrderMaterialDetail> lines = new List<tb_AS_RepairOrderMaterialDetail>();
            bindingSourceOtherSub.DataSource = lines;
            sgd2.BindingSourceLines = bindingSourceOtherSub;
            //    Expression<Func<View_ProdDetail, bool>> exp = Expressionable.Create<View_ProdDetail>() //创建表达式
            // .AndIF(true, w => w.CNName.Length > 0)
            //// .AndIF(txtSpecifications.Text.Trim().Length > 0, w => w.Specifications.Contains(txtSpecifications.Text.Trim()))
            //.ToExpression();//注意 这一句 不能少
            //                // StringBuilder sb = new StringBuilder();
            //    /// sb.Append(string.Format("{0}='{1}'", item.ColName, valValue));
            //    list = dc.BaseQueryByWhere(exp);
            list = MainForm.Instance.View_ProdDetailList;
            sgd2.SetDependencyObject<ProductSharePart, tb_AS_RepairOrderMaterialDetail>(list);
            sgd2.HasRowHeader = true;
            sgh2.InitGrid(grid2, sgd2, true, nameof(tb_AS_RepairOrderMaterialDetail));
            sgh2.OnLoadMultiRowData += Sgh2_OnLoadMultiRowData;
            sgh2.OnCalculateColumnValue += Sgh2_OnCalculateColumnValue;
        }

        private void Sgh2_OnCalculateColumnValue(object rowObj, SourceGridDefine griddefine, Position Position)
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
                List<tb_AS_RepairOrderMaterialDetail> details = sgd2.BindingSourceLines.DataSource as List<tb_AS_RepairOrderMaterialDetail>;
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
                EditEntity.TotalQty = details.Sum(c => c.ShouldSendQty.ToInt());


                EditEntity.tb_AS_RepairOrderMaterialDetails = details;
                EditEntity.TotalMaterialAmount = EditEntity.tb_AS_RepairOrderMaterialDetails.Sum(c => c.UnitPrice * c.ShouldSendQty.ToInt());
                EditEntity.TotalAmount = EditEntity.tb_AS_RepairOrderMaterialDetails.Sum(c => c.UnitPrice * c.ShouldSendQty.ToInt()) + EditEntity.LaborCost;
                EditEntity.TotalMaterialCost = EditEntity.tb_AS_RepairOrderMaterialDetails.Sum(c => c.Cost * c.ShouldSendQty.ToInt());
                Sgh_OnCalculateColumnValue(null, null, new Position());

            }
            catch (Exception ex)
            {
                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
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
                List<tb_AS_RepairOrderDetail> details = new List<tb_AS_RepairOrderDetail>();

                foreach (var item in RowDetails)
                {
                    tb_AS_RepairOrderDetail bOM_SDetail = MainForm.Instance.mapper.Map<tb_AS_RepairOrderDetail>(item);
                    bOM_SDetail.Quantity = 0;
                    details.Add(bOM_SDetail);
                }
                sgh.InsertItemDataToGrid<tb_AS_RepairOrderDetail>(grid1, sgd, details, c => c.ProdDetailID, position);
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
                List<tb_AS_RepairOrderMaterialDetail> details = new List<tb_AS_RepairOrderMaterialDetail>();

                foreach (var item in RowDetails)
                {
                    tb_AS_RepairOrderMaterialDetail bOM_SDetail = MainForm.Instance.mapper.Map<tb_AS_RepairOrderMaterialDetail>(item);
                    bOM_SDetail.ShouldSendQty = 0;
                    bOM_SDetail.ActualSentQty = 0;
                    details.Add(bOM_SDetail);
                }
                sgh2.InsertItemDataToGrid<tb_AS_RepairOrderMaterialDetail>(grid2, sgd2, details, c => c.ProdDetailID, position);
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
                List<tb_AS_RepairOrderDetail> details = sgd.BindingSourceLines.DataSource as List<tb_AS_RepairOrderDetail>;
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

                //EditEntity.tb_AS_RepairOrderMaterialDetails = LastRefurbishedMaterials;
                if (EditEntity.tb_AS_RepairOrderMaterialDetails != null)
                {
                    EditEntity.TotalMaterialAmount = EditEntity.tb_AS_RepairOrderMaterialDetails.Sum(c => c.UnitPrice * c.ShouldSendQty.ToInt());
                    EditEntity.TotalAmount = EditEntity.tb_AS_RepairOrderMaterialDetails.Sum(c => c.UnitPrice * c.ShouldSendQty.ToInt()) + EditEntity.LaborCost;
                    EditEntity.TotalMaterialCost = EditEntity.tb_AS_RepairOrderMaterialDetails.Sum(c => c.Cost * c.ShouldSendQty.ToInt());
                }

            }
            catch (Exception ex)
            {
                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }
        }


        //protected override async Task<bool> Submit()
        //{
        //    if (EditEntity != null)
        //    {
        //        bool rs = await base.Submit();
        //        EditEntity.RepairStatus = (int)RepairStatus.费用确认中;
        //        if (true)
        //        {

        //        }
        //        return rs;
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //}



        List<tb_AS_RepairOrderDetail> details = new List<tb_AS_RepairOrderDetail>();
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

            List<tb_AS_RepairOrderDetail> oldOjb = new List<tb_AS_RepairOrderDetail>(details.ToArray());

            List<tb_AS_RepairOrderDetail> detailentity = bindingSourceSub.DataSource as List<tb_AS_RepairOrderDetail>;
            List<tb_AS_RepairOrderMaterialDetail> RefurbishedMaterials = bindingSourceOtherSub.DataSource as List<tb_AS_RepairOrderMaterialDetail>;
            List<tb_AS_RepairOrderMaterialDetail> LastRefurbishedMaterials = bindingSourceOtherSub.DataSource as List<tb_AS_RepairOrderMaterialDetail>;


            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.ProdDetailID > 0).ToList();
                var aa = details.Select(c => c.ProdDetailID ).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                if (NeedValidated && aa.Count > 0)
                {
                    System.Windows.Forms.MessageBox.Show("维修明细中，相同的产品不能多行录入,如有需要,请另建单据保存!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }




                if (NeedValidated)
                {

                    if (EditEntity.RepairOrderNo.Trim().Length == 0)
                    {
                        MessageBox.Show("编号由系统自动生成，如果不小心清除，请重新生成单据的编号。");
                        return false;
                    }

                    if (EditEntity.ProjectGroup_ID.HasValue && EditEntity.ProjectGroup_ID.Value <= 0)
                    {
                        EditEntity.ProjectGroup_ID = null;
                    }

                    if (EditEntity.Paytype_ID > 0)
                    {
                        var paytype = EditEntity.Paytype_ID;
                        var paymethod = MyCacheManager.Instance.GetEntity<tb_PaymentMethod>(EditEntity.Paytype_ID);
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



                }


                //因为有一个特殊验证： RuleFor(tb_AS_RepairOrderDetail => tb_AS_RepairOrderDetail.Quantity).NotEqual(0).When(c => c.tb_AS_RepairOrder.RefundOnly == false).WithMessage("非仅退款时，退回数量不能为0为零。");
                foreach (var item in details)
                {
                    item.tb_as_repairorder = EditEntity;
                }


                EditEntity.tb_AS_RepairOrderDetails = details;

                EditEntity.TotalQty = details.Sum(c => c.Quantity);


                //产品ID有值才算有效值
                LastRefurbishedMaterials = RefurbishedMaterials.Where(t => t.ProdDetailID > 0).ToList();
                var bb = LastRefurbishedMaterials.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                if (NeedValidated && bb.Count > 0)
                {
                    System.Windows.Forms.MessageBox.Show("翻新物料明细中，相同的产品不能多行录入,如有需要,请另建单据保存!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                EditEntity.tb_AS_RepairOrderMaterialDetails = LastRefurbishedMaterials;
                EditEntity.TotalMaterialAmount = EditEntity.tb_AS_RepairOrderMaterialDetails.Sum(c => c.UnitPrice * c.ShouldSendQty.ToInt());
                EditEntity.TotalAmount = EditEntity.tb_AS_RepairOrderMaterialDetails.Sum(c => c.UnitPrice * c.ShouldSendQty.ToInt()) + EditEntity.LaborCost;
                EditEntity.TotalMaterialCost = EditEntity.tb_AS_RepairOrderMaterialDetails.Sum(c => c.Cost * c.ShouldSendQty.ToInt());
                if (NeedValidated && !EditEntity.PreDeliveryDate.HasValue)
                {
                    if (System.Windows.Forms.MessageBox.Show("预交日期为空，你确定无法确认预交日期吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                    {
                        return false;
                    }

                }

                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_AS_RepairOrderDetail>(details))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_AS_RepairOrderMaterialDetail>(LastRefurbishedMaterials))
                {
                    return false;
                }
                if (NeedValidated && EditEntity.TotalQty != details.Sum(c => c.Quantity))
                {
                    System.Windows.Forms.MessageBox.Show("单据总数量和明细数量的和不相等，请检查记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }



                ReturnMainSubResults<tb_AS_RepairOrder> SaveResult = new ReturnMainSubResults<tb_AS_RepairOrder>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.RepairOrderNo}。");
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

        //protected override async Task<bool> Submit()
        //{
        //    if (EditEntity != null)
        //    {
        //        if (EditEntity.RepairStartDate == null)
        //        {
        //            //退货日期不能为空。
        //            System.Windows.Forms.MessageBox.Show("退货日期不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            return false;
        //        }
        //        bool rs = await base.Submit();
        //        return rs;
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //}

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

        private async Task ToRepairOrder(long? ASApplyID)
        {
            //要加一个判断 值是否有变化
            //新增时才可以
            //转单
            ButtonSpecAny bsa = (txtASApplyID as KryptonTextBox).ButtonSpecs.FirstOrDefault(c => c.UniqueName == "btnQuery");
            if (bsa == null)
            {
                return;
            }
            var AfterSaleApply = bsa.Tag as tb_AS_AfterSaleApply;//这个tag值。赋值会比较当前方法晚，所以失效
            AfterSaleApply = await MainForm.Instance.AppContext.Db.Queryable<tb_AS_AfterSaleApply>().Where(c => c.ASApplyID == ASApplyID)
            .Includes(t => t.tb_AS_AfterSaleApplyDetails, d => d.tb_proddetail)
            .Includes(t => t.tb_AS_RepairOrders, a => a.tb_AS_RepairOrderDetails)
            .SingleAsync();
            if (AfterSaleApply != null)
            {
                var ctr = Startup.GetFromFac<tb_AS_AfterSaleApplyController<tb_AS_AfterSaleApply>>();
                tb_AS_RepairOrder RepairOrder = ctr.ToRepairOrder(AfterSaleApply);
                BindData(RepairOrder as tb_AS_RepairOrder);
            }
            else
            {
                MainForm.Instance.PrintInfoLog("选取的对象为空！");
            }
        }

        /*
        /// <summary>
        /// 将销售订单转换为销售退回单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async Task LoadSaleOutBillData(long? ASApplyID)
        {
            //要加一个判断 值是否有变化
            //新增时才可以
            //转单
            ButtonSpecAny bsa = (txtASApplyID as KryptonTextBox).ButtonSpecs.FirstOrDefault(c => c.UniqueName == "btnQuery");
            if (bsa == null)
            {
                return;
            }
            var AfterSaleApply = bsa.Tag as tb_AS_AfterSaleApply;//这个tag值。赋值会比较当前方法晚，所以失效
            AfterSaleApply = await MainForm.Instance.AppContext.Db.Queryable<tb_AS_AfterSaleApply>().Where(c => c.ASApplyID == ASApplyID)
            .Includes(t => t.tb_AS_AfterSaleApplyDetails, d => d.tb_proddetail)
            .Includes(t => t.tb_AS_RepairOrders, a => a.tb_AS_RepairOrderDetails)
            .SingleAsync();
            if (AfterSaleApply != null)
            {
                //如果这个销售出库单，已经有提交或审核过的。并且数量等于出库总数量则无法再次录入退回单。应该是不会显示出来了。
                if (AfterSaleApply.tb_AS_RepairOrders.Sum(c => c.TotalQty) == AfterSaleApply.TotalConfirmedQuantity)
                {
                    MainForm.Instance.uclog.AddLog("当前售后申请单已经全部维修回，无法再次维修。");

                    return;
                }

                ASApplyID = AfterSaleApply.ASApplyID;

                tb_AS_RepairOrder entity = MainForm.Instance.mapper.Map<tb_AS_RepairOrder>(AfterSaleApply);
                List<tb_AS_RepairOrderDetail> details = MainForm.Instance.mapper.Map<List<tb_AS_RepairOrderDetail>>(AfterSaleApply.tb_AS_AfterSaleApplyDetails);
                List<tb_AS_RepairOrderDetail> NewDetails = new List<tb_AS_RepairOrderDetail>();
                List<string> tipsMsg = new List<string>();
                for (global::System.Int32 i = 0; i < details.Count; i++)
                {
                    tb_AS_AfterSaleApplyDetail item = AfterSaleApply.tb_AS_AfterSaleApplyDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID);
                    details[i].Quantity = item.ConfirmedQuantity - item.DeliveredQty;
                    if (details[i].Quantity > 0)
                    {
                        NewDetails.Add(details[i]);
                    }
                    else
                    {
                        tipsMsg.Add($"当前行的SKU:{item.tb_proddetail.SKU}已维修数量为{details[i].Quantity}，当前行数据将不会加载到明细！");
                    }
                }

                if (NewDetails.Count == 0)
                {
                    tipsMsg.Add($"售后申请单:{entity.ASApplyNo}已全部维修处理，请检查是否正在重复操作！");
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


                entity.tb_AS_RepairOrderDetails = NewDetails;
                entity.RepairStartDate = System.DateTime.Now;

                dtpPreDeliveryDate.Checked = true;
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
                if (entity.ASApplyID.HasValue && entity.ASApplyID > 0)
                {
                    entity.CustomerVendor_ID = AfterSaleApply.CustomerVendor_ID;
                    entity.ASApplyNo = AfterSaleApply.ASApplyNo;
                    entity.ASApplyID = AfterSaleApply.ASApplyID;
                }
                BusinessHelper.Instance.InitEntity(entity);
                BindData(entity as tb_AS_RepairOrder);
            }
            else
            {
                MainForm.Instance.PrintInfoLog("选取的对象为空！");
            }
        }

        */
    }
}
