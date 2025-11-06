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
using RUINORERP.UI.Monitoring.Auditing;
using NPOI.SS.Formula.Functions;
using Netron.GraphLib;
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Business.Cache;
using RUINORERP.Business.BizMapperService;
using RUINORERP.UI.Network.Services;


namespace RUINORERP.UI.ASS
{


    /// <summary>
    /// 销售订单时：有运费外币，总金额外币，订单外币。反而出库时不用这么多。外币只是用于记账。出库时只要根据本币和外币及汇率。生成应收时自动算出来。
    /// </summary>
    [MenuAttrAssemblyInfo("售后申请单", ModuleMenuDefine.模块定义.售后管理, ModuleMenuDefine.售后管理.售后流程, BizType.售后申请单)]
    public partial class UCASAfterSaleApply : BaseBillEditGeneric<tb_AS_AfterSaleApply, tb_AS_AfterSaleApplyDetail>, IPublicEntityObject
    {
        public UCASAfterSaleApply()
        {
            InitializeComponent();
            //InitDataToCmbByEnumDynamicGeneratedDataSource<tb_AS_AfterSaleApply>(typeof(Priority), e => e.OrderPriority, cmbOrderPriority, false);
            AddPublicEntityObject(typeof(ProductSharePart));
            // 通过依赖注入获取缓存管理器
            _cacheManager = Startup.GetFromFac<IEntityCacheManager>();
            _tableSchemaManager = TableSchemaManager.Instance;
        }

        private readonly IEntityCacheManager _cacheManager;
        private readonly TableSchemaManager _tableSchemaManager;
        internal override void LoadDataToUI(object Entity)
        {
            ActionStatus actionStatus = ActionStatus.无操作;
            BindData(Entity as tb_AS_AfterSaleApply, actionStatus);
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            //BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_AS_AfterSaleApply).Name + "Processor");
            //QueryConditionFilter = baseProcessor.GetQueryFilter();
            base.QueryConditionBuilder();
            //创建表达式
            var lambda = Expressionable.Create<tb_AS_AfterSaleApply>()
                             .And(t => t.isdeleted == false)
                            //自己的只能查自己的
                            .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext), t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                            .ToExpression();//注意 这一句 不能少
            QueryConditionFilter.SetFieldLimitCondition(lambda);
        }




        protected override async Task LoadRelatedDataToDropDownItemsAsync()
        {
            if (base.EditEntity is tb_AS_AfterSaleApply SaleApply)
            {
                if (SaleApply.tb_AS_AfterSaleDeliveries != null && SaleApply.tb_AS_AfterSaleDeliveries.Count > 0)
                {
                    foreach (var item in SaleApply.tb_AS_AfterSaleDeliveries)
                    {
                        RelatedQueryParameter rqp = new RelatedQueryParameter();
                        rqp.bizType = BizType.售后交付单;
                        rqp.billId = item.ASDeliveryID;
                        ToolStripMenuItem RelatedMenuItem = new ToolStripMenuItem();
                        RelatedMenuItem.Name = $"{rqp.billId}";
                        RelatedMenuItem.Tag = rqp;
                        RelatedMenuItem.Text = $"{rqp.bizType}:{item.ASDeliveryNo}";
                        RelatedMenuItem.Click += base.MenuItem_Click;
                        if (!toolStripbtnRelatedQuery.DropDownItems.ContainsKey(item.ASDeliveryID.ToString()))
                        {
                            toolStripbtnRelatedQuery.DropDownItems.Add(RelatedMenuItem);
                        }
                    }
                }

                if (SaleApply.tb_AS_RepairOrders != null && SaleApply.tb_AS_RepairOrders.Count > 0)
                {
                    foreach (var item in SaleApply.tb_AS_RepairOrders)
                    {
                        RelatedQueryParameter rqp = new RelatedQueryParameter();
                        rqp.bizType = BizType.维修工单;
                        rqp.billId = item.RepairOrderID;
                        ToolStripMenuItem RelatedMenuItem = new ToolStripMenuItem();
                        RelatedMenuItem.Name = $"{rqp.billId}";
                        RelatedMenuItem.Tag = rqp;
                        RelatedMenuItem.Text = $"{rqp.bizType}:{item.RepairOrderNo}";
                        RelatedMenuItem.Click += base.MenuItem_Click;
                        if (!toolStripbtnRelatedQuery.DropDownItems.ContainsKey(item.RepairOrderID.ToString()))
                        {
                            toolStripbtnRelatedQuery.DropDownItems.Add(RelatedMenuItem);
                        }
                    }
                }



            }
            await base.LoadRelatedDataToDropDownItemsAsync();
        }
        ToolStripButton toolStripButton维修评估 = new System.Windows.Forms.ToolStripButton();
        /// <summary>
        /// 添加回收
        /// </summary>
        /// <returns></returns>
        public override ToolStripItem[] AddExtendButton(tb_MenuInfo menuInfo)
        {

            toolStripButton维修评估.Text = "维修评估";
            toolStripButton维修评估.Image = global::RUINORERP.UI.Properties.Resources.Assignment;
            toolStripButton维修评估.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton维修评估.Name = "维修评估";
            toolStripButton维修评估.Visible = false;//默认隐藏
            UIHelper.ControlButton<ToolStripButton>(CurMenuInfo, toolStripButton维修评估);
            toolStripButton维修评估.ToolTipText = "对售后申请单的产品进行维修评估及报价，生成维修工单，使用本功能。";
            toolStripButton维修评估.Click += new System.EventHandler(this.toolStripButton维修评估_Click);


            System.Windows.Forms.ToolStripItem[] extendButtons = new System.Windows.Forms.ToolStripItem[]
            { toolStripButton维修评估};

            this.BaseToolStrip.Items.AddRange(extendButtons);
            return extendButtons;
        }
        private void toolStripButton维修评估_Click(object sender, EventArgs e)
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
                    if (EditEntity.tb_AS_AfterSaleDeliveries != null && EditEntity.tb_AS_AfterSaleDeliveries.Count > 0)
                    {
                        MessageBox.Show($"当前【售后申请单】{EditEntity.ASApplyNo}：已经生成过【售后交付单】，无法进行维修评估", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (EditEntity.tb_AS_RepairOrders != null && EditEntity.tb_AS_RepairOrders.Count > 0)
                    {
                        if (MessageBox.Show($"当前【售后申请单】{EditEntity.ASApplyNo}：已经生成过【维修工单】，\r\n确定再次生成吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                        {
                            return;
                        }
                    }

                    var ctr = Startup.GetFromFac<tb_AS_AfterSaleApplyController<tb_AS_AfterSaleApply>>();
                    tb_AS_RepairOrder RepairOrder = ctr.ToRepairOrder(EditEntity).Result;
                    MenuPowerHelper menuPowerHelper;
                    menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
                    tb_MenuInfo RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == nameof(tb_AS_RepairOrder) && m.BIBaseForm == "BaseBillEditGeneric`2").FirstOrDefault();
                    if (RelatedMenuInfo != null)
                    {
                        menuPowerHelper.ExecuteEvents(RelatedMenuInfo, RepairOrder);
                    }
                    return;
                }
                else
                {
                    MessageBox.Show($"当前【售后申请单】{EditEntity.ASApplyNo}：未审核，无法生成【维修工单】", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        }



        public override void BindData(tb_AS_AfterSaleApply entityPara, ActionStatus actionStatus)
        {
            tb_AS_AfterSaleApply entity = entityPara as tb_AS_AfterSaleApply;

            if (entity == null)
            {

                return;
            }
            cmbASProcessStatus.Enabled = false;
            if (entity != null)
            {
                if (entity.ASApplyID > 0)
                {
                    entity.PrimaryKeyID = entity.ASApplyID;
                    entity.ActionStatus = ActionStatus.加载;
                    // entity.DataStatus = (int)DataStatus.确认;
                    //如果审核了，审核要灰色
                }
                else
                {
                    entity.ActionStatus = ActionStatus.新增;
                    entity.DataStatus = (int)DataStatus.草稿;
                    entity.Priority = (int)Priority.正常;
                    if (string.IsNullOrEmpty(entity.ASApplyNo))
                    {
                        entity.ASApplyNo = BizCodeService.GetBizBillNo(BizType.售后申请单);
                    }
                    entity.ASProcessStatus = (int)ASProcessStatus.登记;
                    entity.RepairEvaluationOpinion = string.Empty;
                    entity.ApprovalOpinions = string.Empty;
                    entity.ApplyDate = System.DateTime.Now;
                    //entity.PreDeliveryDate
                    if (entity.tb_AS_AfterSaleApplyDetails != null && entity.tb_AS_AfterSaleApplyDetails.Count > 0)
                    {
                        entity.tb_AS_AfterSaleApplyDetails.ForEach(c => c.ASApplyID = 0);
                        entity.tb_AS_AfterSaleApplyDetails.ForEach(c => c.ASApplyDetailID = 0);
                    }


                    UIHelper.ControlForeignFieldInvisible<tb_AS_AfterSaleApply>(this, false);
                }
            }
            //StatusMachine.CurrentDataStatus = (DataStatus)entity.DataStatus;
            //StatusMachine.ApprovalStatus = (ApprovalStatus)entity.ApprovalStatus;
            if (entity.ApprovalStatus.HasValue)
            {
                lblReview.Text = ((ApprovalStatus)entity.ApprovalStatus).ToString();
            }
            EditEntity = entity;

            ////提交后才能复核数量  ，暂时不限制。只是审核时。判断 复核数量不能为0.如果两个不相等。则提示
            //if (EditEntity.DataStatus == (int)DataStatus.草稿)
            //{
            //    sgd.DefineColumns.SetCol_ReadOnly<tb_AS_AfterSaleApplyDetail>(c => c.ConfirmedQuantity);
            //}
            //else
            //{
            //    sgd.DefineColumns.SetCol_ReadOnly<tb_AS_AfterSaleApplyDetail>(c => c.ConfirmedQuantity, false);
            //}
            DataBindingHelper.BindData4CmbByEntity<tb_Location>(entity, k => k.Location_ID, cmbLocation_ID);
            DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.ASApplyNo, txtASApplyNo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID);
            DataBindingHelper.BindData4CmbByEnum<tb_AS_AfterSaleApply>(entity, k => k.Priority, typeof(Priority), cmbPriority, true);
            DataBindingHelper.BindData4CmbByEnum<tb_AS_AfterSaleApply>(entity, k => k.ASProcessStatus, typeof(ASProcessStatus), cmbASProcessStatus, true);
            DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.TotalInitialQuantity, txtTotalInitialQuantity, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.TotalConfirmedQuantity, txtTotalConfirmedQuantity, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4DataTime<tb_AS_AfterSaleApply>(entity, t => t.ApplyDate, dtpApplyDate, false);
            DataBindingHelper.BindData4DataTime<tb_AS_AfterSaleApply>(entity, t => t.Approver_at, dtpApprover_at, false);
            DataBindingHelper.BindData4DataTime<tb_AS_AfterSaleApply>(entity, t => t.PreDeliveryDate, dtpPreDeliveryDate, false);
            DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.ShippingAddress, txtShippingAddress, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.ShippingWay, txtshippingWay, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_AS_AfterSaleApply>(entity, t => t.InWarrantyPeriod, chkInWarrantyPeriod, false);

            DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.RepairEvaluationOpinion, txtRepairEvaluationOpinion, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CmbByEnum<tb_AS_AfterSaleApply>(entity, k => k.ExpenseAllocationMode, typeof(ExpenseAllocationMode), cmbExpenseAllocationMode, true);
            DataBindingHelper.BindData4CmbByEnum<tb_AS_AfterSaleApply>(entity, k => k.ExpenseBearerType, typeof(ExpenseBearerType), cmbExpenseBearerType, true);
            DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.TotalDeliveredQty, txtTotalDeliveredQty, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4CheckBox<tb_AS_AfterSaleApply>(entity, t => t.MaterialFeeConfirmed, chkMaterialFeeConfirmed, false);
            DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);
            //default  DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
            // DataBindingHelper.BindData4CheckBox<tb_AS_AfterSaleApply>(entity, t => t.ApprovalResults, chkApprovalResults, false);

            DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.CustomerSourceNo, txtCustomerSourceNo, BindDataType4TextBox.Text, false);
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



            DataBindingHelper.BindData4DataTime<tb_AS_AfterSaleApply>(entity, t => t.PreDeliveryDate, dtpPreDeliveryDate, false);
            DataBindingHelper.BindData4DataTime<tb_AS_AfterSaleApply>(entity, t => t.ApplyDate, dtpApplyDate, false);
            DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.ShippingAddress, txtShippingAddress, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.ShippingWay, txtshippingWay, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text, false);


            base.errorProviderForAllInput.DataSource = entity;
            base.errorProviderForAllInput.ContainerControl = this;

            //this.ValidateChildren();
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            DataBindingHelper.BindData4ControlByEnum<tb_AS_AfterSaleApply>(entity, t => t.DataStatus, lblDataStatus, BindDataType4Enum.EnumName, typeof(Global.DataStatus));
            DataBindingHelper.BindData4ControlByEnum<tb_AS_AfterSaleApply>(entity, t => t.ApprovalStatus, lblReview, BindDataType4Enum.EnumName, typeof(Global.ApprovalStatus));

            if (entity.tb_AS_AfterSaleApplyDetails != null && entity.tb_AS_AfterSaleApplyDetails.Count > 0)
            {
                //LoadDataToGrid(entity.tb_AS_AfterSaleApplyDetails);
                sgh.LoadItemDataToGrid<tb_AS_AfterSaleApplyDetail>(grid1, sgd, entity.tb_AS_AfterSaleApplyDetails, c => c.ProdDetailID);
            }
            else
            {
                //LoadDataToGrid(new List<tb_AS_AfterSaleApplyDetail>());
                sgh.LoadItemDataToGrid<tb_AS_AfterSaleApplyDetail>(grid1, sgd, new List<tb_AS_AfterSaleApplyDetail>(), c => c.ProdDetailID);
            }

            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {
                if (EditEntity == null)
                {
                    return;
                }

                //权限允许
                if ((true && entity.DataStatus == (int)DataStatus.草稿) || (true && entity.DataStatus == (int)DataStatus.新建))
                {
                    if (s2.PropertyName == entity.GetPropertyName<tb_AS_AfterSaleApply>(c => c.Location_ID))
                    {
                        if (EditEntity.Location_ID > 0)
                        {

                            //明细仓库优先来自于主表，可以手动修改。
                            sgd.DefineColumns.SetCol_DefaultValue<tb_AS_AfterSaleApplyDetail>(c => c.Location_ID, EditEntity.Location_ID);
                            if (entity.tb_AS_AfterSaleApplyDetails != null)
                            {
                                entity.tb_AS_AfterSaleApplyDetails.ForEach(c => c.Location_ID = EditEntity.Location_ID);
                                sgh.SetCellValue<tb_AS_AfterSaleApplyDetail>(sgd, colNameExp => colNameExp.Location_ID, EditEntity.Location_ID);
                            }
                        }
                    }

                    if (s2.PropertyName == entity.GetPropertyName<tb_AS_AfterSaleApply>(c => c.ExpenseAllocationMode) && entity.ExpenseAllocationMode.HasValue && entity.ExpenseAllocationMode.Value > 0)
                    {
                        if (entity.ExpenseAllocationMode.Value == (int)ExpenseAllocationMode.单一承担)
                        {
                            //默认为客户
                            entity.ExpenseBearerType = (int)ExpenseBearerType.客户;
                        }
                    }

                    if (s2.PropertyName == entity.GetPropertyName<tb_AS_AfterSaleApply>(c => c.ProjectGroup_ID) && entity.ProjectGroup_ID.HasValue && entity.ProjectGroup_ID > 0)
                    {
                        if (cmbProjectGroup.SelectedItem is tb_ProjectGroup ProjectGroup)
                        {
                            if (ProjectGroup.tb_ProjectGroupAccountMappers != null && ProjectGroup.tb_ProjectGroupAccountMappers.Count > 0)
                            {
                                EditEntity.tb_projectgroup = ProjectGroup;
                            }
                        }
                    }
                }

                //数据状态变化会影响按钮变化
                if (s2.PropertyName == entity.GetPropertyName<tb_AS_AfterSaleApply>(c => c.DataStatus))
                {
                    ToolBarEnabledControl(entity);
                }

                //如果客户有变化，带出对应有业务员
                if (entity.CustomerVendor_ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_AS_AfterSaleApply>(c => c.CustomerVendor_ID))
                {
                    var obj = MyCacheManager.Instance.GetEntity<tb_CustomerVendor>(entity.CustomerVendor_ID);
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
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_AS_AfterSaleApplyValidator>(), kryptonPanelMainInfo.Controls);
                // base.InitEditItemToControl(entity, kryptonPanelMainInfo.Controls);
            }
            base.BindData(entity);
        }
        private void Grid1_Enter(object sender, EventArgs e)
        {
            if (EditEntity == null)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();
            Control firstInvalidControl = null;

            if (EditEntity.Location_ID == 0 || EditEntity.Location_ID == -1)
            {
                sb.AppendLine("请选择【售后暂存仓库】。");
                firstInvalidControl = cmbLocation_ID;
            }

            if (sb.Length > 0)
            {
                MessageBox.Show(sb.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (firstInvalidControl != null)
                {
                    firstInvalidControl.Focus();
                }
            }
        }



        // 在基类中定义静态属性

        SourceGridDefine sgd = null;
        //        SourceGridHelper<View_ProdDetail, tb_AS_AfterSaleApplyDetail> sgh = new SourceGridHelper<View_ProdDetail, tb_AS_AfterSaleApplyDetail>();
        SourceGridHelper sgh = new SourceGridHelper();
        //设计关联列和目标列
        View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
        List<View_ProdDetail> list = new List<View_ProdDetail>();
        private void UcSaleOrderEdit_Load(object sender, EventArgs e)
        {
            grid1.Enter += Grid1_Enter;

            var sw = new Stopwatch();
            sw.Start();

            grid1.BorderStyle = BorderStyle.FixedSingle;
            grid1.Selection.EnableMultiSelection = false;

            List<SGDefineColumnItem> listCols = new List<SGDefineColumnItem>();
            //指定了关键字段ProdDetailID
            listCols = sgh.GetGridColumns<ProductSharePart, tb_AS_AfterSaleApplyDetail>(c => c.ProdDetailID, false);

            listCols.SetCol_NeverVisible<tb_AS_AfterSaleApplyDetail>(c => c.ASApplyDetailID);
            listCols.SetCol_NeverVisible<tb_AS_AfterSaleApplyDetail>(c => c.ProdDetailID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Rack_ID);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.VendorModelCode);
            listCols.SetCol_NeverVisible<ProductSharePart>(c => c.Location_ID);

            if (!AppContext.SysConfig.UseBarCode)
            {
                listCols.SetCol_NeverVisible<ProductSharePart>(c => c.BarCode);
            }

            //listCols.SetCol_DefaultValue<tb_AS_AfterSaleApplyDetail>(a => a.TaxRate, 0.13m);//m =>decial d=>double

            //如果库位为只读  暂时只会显示 ID
            //listCols.SetCol_ReadOnly<ProductSharePart>(c => c.Location_ID);
            listCols.SetCol_ReadOnly<tb_AS_AfterSaleApplyDetail>(c => c.DeliveredQty);
            //主表指定了,明细 可以省略，不再明细可以随便更指定。不然太复杂
            listCols.SetCol_DefaultHide<tb_AS_AfterSaleApplyDetail>(c => c.Location_ID);

            sgd = new SourceGridDefine(grid1, listCols, true);
            sgd.GridMasterData = EditEntity;



            //设置总计列
            BaseProcessor baseProcessor = BusinessHelper._appContext.GetRequiredServiceByName<BaseProcessor>(typeof(tb_AS_AfterSaleApplyDetail).Name + "Processor");
            var summaryCols = baseProcessor.GetSummaryCols();
            foreach (var item in summaryCols)
            {
                foreach (var col in listCols)
                {
                    col.SetCol_Summary<tb_AS_AfterSaleApplyDetail>(item);
                }
            }
            listCols.SetCol_Summary<tb_AS_AfterSaleApplyDetail>(c => c.ConfirmedQuantity);
            listCols.SetCol_Summary<tb_AS_AfterSaleApplyDetail>(c => c.InitialQuantity);
            listCols.SetCol_Summary<tb_AS_AfterSaleApplyDetail>(c => c.DeliveredQty);


            //公共到明细的映射 源 ，左边会隐藏
            //sgh.SetPointToColumnPairs<ProductSharePart, tb_AS_AfterSaleApplyDetail>(sgd, f => f.Location_ID, t => t.Location_ID);
            //设置了默认值，上面的不能启用，不然会将产品的库位赋值过来
            listCols.SetCol_ReadOnly<tb_AS_AfterSaleApplyDetail>(c => c.Location_ID, true);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_AS_AfterSaleApplyDetail>(sgd, f => f.prop, t => t.property);
            sgh.SetPointToColumnPairs<ProductSharePart, tb_AS_AfterSaleApplyDetail>(sgd, f => f.Model, t => t.CustomerPartNo, false);

            //应该只提供一个结构
            List<tb_AS_AfterSaleApplyDetail> lines = new List<tb_AS_AfterSaleApplyDetail>();
            bindingSourceSub.DataSource = lines;
            sgd.BindingSourceLines = bindingSourceSub;

            //list = dc.BaseQueryByWhere(exp);
            list = MainForm.Instance.View_ProdDetailList;

            sgd.SetDependencyObject<ProductSharePart, tb_AS_AfterSaleApplyDetail>(list);
            sgd.HasRowHeader = true;
            UIHelper.ControlChildColumnsInvisible(CurMenuInfo, listCols);
            sgh.InitGrid(grid1, sgd, true, nameof(tb_AS_AfterSaleApplyDetail));
            sgh.OnCalculateColumnValue += Sgh_OnCalculateColumnValue;
            sgh.OnLoadMultiRowData += Sgh_OnLoadMultiRowData;

            sw.Stop();
            MainForm.Instance.uclog.AddLog("加载数据耗时：" + sw.ElapsedMilliseconds + "毫秒");

            UIHelper.ControlMasterColumnsInvisible(CurMenuInfo, this);
            UIHelper.ControlForeignFieldInvisible<tb_AS_AfterSaleApply>(this, false);

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
                List<tb_AS_AfterSaleApplyDetail> details = new List<tb_AS_AfterSaleApplyDetail>();

                foreach (var item in RowDetails)
                {
                    tb_AS_AfterSaleApplyDetail Detail = MainForm.Instance.mapper.Map<tb_AS_AfterSaleApplyDetail>(item);
                    details.Add(Detail);
                }
                sgh.InsertItemDataToGrid<tb_AS_AfterSaleApplyDetail>(grid1, sgd, details, c => c.ProdDetailID, position);
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
                List<tb_AS_AfterSaleApplyDetail> details = sgd.BindingSourceLines.DataSource as List<tb_AS_AfterSaleApplyDetail>;
                details = details.Where(c => c.ProdDetailID > 0).ToList();
                if (details.Count == 0)
                {
                    MainForm.Instance.uclog.AddLog("请先选择产品数据");
                    return;
                }
                EditEntity.TotalDeliveredQty = details.Sum(c => c.DeliveredQty);
                EditEntity.TotalInitialQuantity = details.Sum(c => c.InitialQuantity);
                EditEntity.TotalConfirmedQuantity = details.Sum(c => c.ConfirmedQuantity);

            }
            catch (Exception ex)
            {
                logger.LogError("计算出错", ex);
                MainForm.Instance.uclog.AddLog("Sgh_OnCalculateColumnValue" + ex.Message);
            }
        }

        List<tb_AS_AfterSaleApplyDetail> details = new List<tb_AS_AfterSaleApplyDetail>();
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



            //如果订单 选择了未付款，但是又选择了非账期的即实收账方式。则审核不通过。
            //如果订单选择了 非未付款，但又选择了账期也不能通过。
            if (NeedValidated)
            {

                if (EditEntity.ASApplyNo.Trim().Length == 0)
                {
                    MessageBox.Show("单据编号由系统自动生成，如果不小心清除，请重新生成单据的单据编号。");
                    return false;
                }

                if (EditEntity.ProjectGroup_ID.HasValue && EditEntity.ProjectGroup_ID.Value <= 0)
                {
                    EditEntity.ProjectGroup_ID = null;
                }


            }

            var eer = errorProviderForAllInput.GetError(txtTotalConfirmedQuantity);

            bindingSourceSub.EndEdit();

            List<tb_AS_AfterSaleApplyDetail> oldOjb = new List<tb_AS_AfterSaleApplyDetail>(details.ToArray());

            List<tb_AS_AfterSaleApplyDetail> detailentity = bindingSourceSub.DataSource as List<tb_AS_AfterSaleApplyDetail>;

            if (EditEntity.ActionStatus == ActionStatus.新增 || EditEntity.ActionStatus == ActionStatus.修改)
            {
                //产品ID有值才算有效值
                details = detailentity.Where(t => t.ProdDetailID > 0).ToList();


                EditEntity.TotalDeliveredQty = details.Sum(c => c.DeliveredQty);
                EditEntity.TotalInitialQuantity = details.Sum(c => c.InitialQuantity);
                EditEntity.TotalConfirmedQuantity = details.Sum(c => c.ConfirmedQuantity);



                //如果没有有效的明细。直接提示
                if (NeedValidated && details.Count == 0)
                {
                    MessageBox.Show("请录入有效明细记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (NeedValidated && (EditEntity.TotalConfirmedQuantity != detailentity.Sum(c => c.ConfirmedQuantity)))
                {
                    MessageBox.Show($"单据总复核数量{EditEntity.TotalConfirmedQuantity}和明细复核数量之和{detailentity.Sum(c => c.ConfirmedQuantity)}不相同，请检查后再试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (NeedValidated && EditEntity.TotalInitialQuantity != details.Sum(c => c.InitialQuantity))
                {
                    MessageBox.Show($"单据总登记数量{EditEntity.TotalInitialQuantity}和明细登记数量之和{detailentity.Sum(c => c.InitialQuantity)}不相同，请检查后再试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (NeedValidated && EditEntity.TotalDeliveredQty != details.Sum(c => c.DeliveredQty))
                {
                    MessageBox.Show($"单据总交付数量{EditEntity.TotalDeliveredQty}和明细交付数量之和{detailentity.Sum(c => c.DeliveredQty)}不相同，请检查后再试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }


                //订单只是警告。可以继续

                EditEntity.tb_AS_AfterSaleApplyDetails = details;


                //没有经验通过下面先不计算
                if (NeedValidated && !base.Validator(EditEntity))
                {
                    return false;
                }
                if (NeedValidated && !base.Validator<tb_AS_AfterSaleApplyDetail>(details))
                {
                    return false;
                }



                if (EditEntity.ApprovalStatus == null)
                {
                    EditEntity.ApprovalStatus = (int)ApprovalStatus.未审核;
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
                    if (EditEntity.tb_AS_AfterSaleDeliveries != null && EditEntity.tb_AS_AfterSaleDeliveries.Count > 0)
                    {
                        MessageBox.Show("当前【售后申请单】已有交付数据，无法修改保存。请检查数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
                if (NeedValidated)
                {
                    if (EditEntity.tb_AS_RepairOrders != null && EditEntity.tb_AS_RepairOrders.Count > 0)
                    {
                        MessageBox.Show("当前【售后申请单】已有维修单数据，无法修改保存。请检查数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }


                ReturnMainSubResults<tb_AS_AfterSaleApply> SaveResult = new ReturnMainSubResults<tb_AS_AfterSaleApply>();
                if (NeedValidated)
                {
                    SaveResult = await base.Save(EditEntity);
                    if (SaveResult.Succeeded)
                    {
                        MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.ASApplyNo}。");
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


        protected async override Task<ReviewResult> Review()
        {
            ReviewResult reviewResult = new ReviewResult();
            if (EditEntity.TotalConfirmedQuantity == 0)
            {
                MessageBox.Show("复核数量不能为零，请录入正确的数量。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return reviewResult;
            }

            if (EditEntity.TotalConfirmedQuantity != EditEntity.TotalInitialQuantity)
            {
                if (MessageBox.Show("登记数量和复核数量不相等，系统将以复核数量为准，确定要审核吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    reviewResult = await base.Review();
                }
                return reviewResult;
            }
            reviewResult = await base.Review();
            return reviewResult;
        }









        protected async override Task<bool> AntiCloseCaseAsync()
        {
            if (EditEntity == null)
            {
                return false;
            }
           
            CommonUI.frmOpinion frm = new CommonUI.frmOpinion();
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<tb_AS_AfterSaleApply>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
            ApprovalEntity ae = new ApprovalEntity();
            ae.BillID = pkid;
            CommBillData cbd = EntityMappingHelper.GetBillData<tb_AS_AfterSaleApply>(EditEntity);
            ae.BillNo = cbd.BillNo;
            ae.bizType = cbd.BizType;
            ae.bizName = cbd.BizName;
            ae.Approver_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
            frm.BindData(ae);
            if (frm.ShowDialog() == DialogResult.OK)//审核了。不管是同意还是不同意
            {
                List<tb_AS_AfterSaleApply> EditEntitys = new List<tb_AS_AfterSaleApply>();
                EditEntity.Notes = frm.txtOpinion.Text;
                EditEntitys.Add(EditEntity);
                //已经审核的,结案了的才能反结案
                List<tb_AS_AfterSaleApply> needCloseCases = EditEntitys.Where(c => c.DataStatus == (int)DataStatus.完结 && c.ApprovalStatus == (int)ApprovalStatus.已审核 && c.ApprovalResults.HasValue).ToList();
                if (needCloseCases.Count == 0)
                {
                    MainForm.Instance.PrintInfoLog($"要反结案的数据为：{needCloseCases.Count}:请检查数据！");
                    return false;
                }

                tb_AS_AfterSaleApplyController<tb_AS_AfterSaleApply> ctr = Startup.GetFromFac<tb_AS_AfterSaleApplyController<tb_AS_AfterSaleApply>>();
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
                    MainForm.Instance.AuditLogHelper.CreateAuditLog<tb_AS_AfterSaleApply>("反结案", EditEntity, $"反结案意见:{ae.CloseCaseOpinions}");
                    Refreshs();
                }
                else
                {
                    MainForm.Instance.PrintInfoLog($"{EditEntity.ASApplyNo}反结案操作失败,原因是{rs.ErrorMsg},如果无法解决，请联系管理员！", Color.Red);
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
      
            CommonUI.frmOpinion frm = new CommonUI.frmOpinion();
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<tb_AS_AfterSaleApply>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
            ApprovalEntity ae = new ApprovalEntity();
            ae.BillID = pkid;
            CommBillData cbd = EntityMappingHelper.GetBillData<tb_AS_AfterSaleApply>(EditEntity);
            ae.BillNo = cbd.BillNo;
            ae.bizType = cbd.BizType;
            ae.bizName = cbd.BizName;
            ae.Approver_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
            frm.BindData(ae);
            if (frm.ShowDialog() == DialogResult.OK)//审核了。不管是同意还是不同意
            {
                List<tb_AS_AfterSaleApply> EditEntitys = new List<tb_AS_AfterSaleApply>();
                EditEntity.Notes = frm.txtOpinion.Text;
                EditEntitys.Add(EditEntity);
                //已经审核的并且通过的情况才能结案
                List<tb_AS_AfterSaleApply> needCloseCases = EditEntitys.Where(c => c.DataStatus == (int)DataStatus.确认 && c.ApprovalStatus == (int)ApprovalStatus.已审核 && c.ApprovalResults.HasValue && c.ApprovalResults.Value).ToList();
                if (needCloseCases.Count == 0)
                {
                    MainForm.Instance.PrintInfoLog($"要结案的数据为：{needCloseCases.Count}:请检查数据！");
                    return false;
                }

                tb_AS_AfterSaleApplyController<tb_AS_AfterSaleApply> ctr = Startup.GetFromFac<tb_AS_AfterSaleApplyController<tb_AS_AfterSaleApply>>();
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
                    MainForm.Instance.AuditLogHelper.CreateAuditLog<tb_AS_AfterSaleApply>("结案", EditEntity, $"结案意见:{ae.CloseCaseOpinions}");
                    Refreshs();
                }
                else
                {
                    MainForm.Instance.PrintInfoLog($"{EditEntity.ASApplyNo}结案操作失败,原因是{rs.ErrorMsg},如果无法解决，请联系管理员！", Color.Red);
                }
                return true;
            }
            else
            {
                return false;
            }
        }





    }
}
