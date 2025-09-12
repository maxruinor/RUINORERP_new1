using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Business.LogicaService;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using RUINORERP.Common;
using RUINORERP.Common.CollectionExtension;
using RUINOR.Core;
using RUINORERP.Common.Helper;
using RUINORERP.Business;
using System.Linq.Expressions;
using RUINORERP.Common.Extensions;
using RUINORERP.Business.Processor;
using SqlSugar;
using RUINORERP.Business.Security;
using RUINORERP.Global.EnumExt;
using RUINORERP.Global.EnumExt.CRM;
using RUINORERP.Global;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.SuperSocketClient;
using RUINORERP.UI.SysConfig;
using TransInstruction;
using AutoUpdateTools;
using RUINORERP.Model.TransModel;
using RUINORERP.Business.CommService;
using FastReport.DevComponents.DotNetBar;
using RUINORERP.UI.ClientCmdService;
using TransInstruction.CommandService;
using System.Threading;
using TransInstruction.Enums;

namespace RUINORERP.UI.CRM
{

    [MenuAttrAssemblyInfo("跟进计划", ModuleMenuDefine.模块定义.客户关系, ModuleMenuDefine.客户关系.跟进管理)]
    public partial class UCCRMFollowUpPlansList : BaseForm.BaseListGeneric<tb_CRM_FollowUpPlans>
    {
        public UCCRMFollowUpPlansList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCCRMFollowUpPlansEdit);
            System.Linq.Expressions.Expression<Func<tb_CRM_FollowUpPlans, int?>> expPlanStatus;
            expPlanStatus = (p) => p.PlanStatus;
            base.ColNameDataDictionary.TryAdd(expPlanStatus.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(FollowUpPlanStatus)));
            DisplayTextResolver.AddFixedDictionaryMappingByEnum(t => t.PlanStatus, typeof(FollowUpPlanStatus));
        }
        protected override async Task<bool> Delete()
        {
            bool rs = false;
            tb_CRM_FollowUpPlans rowInfo = (tb_CRM_FollowUpPlans)this.bindingSourceList.Current;
            if (rowInfo.Employee_ID != MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID && !MainForm.Instance.AppContext.IsSuperUser)
            {
                //只能删除自己的收款信息。
                MessageBox.Show("只能删除自己的计划信息。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return rs;
            }
            if (rowInfo.PlanStatus != (int)FollowUpPlanStatus.未开始)
            {
                //只能删除自己的收款信息。
                MessageBox.Show("只有【未开始】的计划才能删除。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return rs;
                //return Task.FromResult(false);
            }
            string PKColName = UIHelper.GetPrimaryKeyColName(typeof(tb_CRM_FollowUpPlans));
            object PKValue = this.bindingSourceList.Current.GetPropertyValue(PKColName);
            rs = await base.Delete();
            if (rs)
            {
                //如果删除了。服务器上的工作流就可以删除了。
                RequestReminderCommand request = new RequestReminderCommand();
                request.requestType = RequestReminderType.删除提醒;
                ReminderData reminderRequest = new ReminderData();
                reminderRequest.BizPrimaryKey = PKValue.ToLong();
                reminderRequest.BizType = BizType.CRM跟进计划;
                request.requestInfo = reminderRequest;
                MainForm.Instance.dispatcher.DispatchAsync(request, CancellationToken.None);
            }
            return rs;
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法，供应商和客户共用所有特殊处理
        /// </summary>
        public override void LimitQueryConditionsBuilder()
        {
            var lambda = Expressionable.Create<tb_CRM_FollowUpPlans>()
                               .AndIF(AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext),
                t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)   //限制了只看到自己的 
            .ToExpression();    //拥有权控制

            QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }

        public override void Query(bool UseAutoNavQuery = false)
        {
            base.Query(true);
        }
        private ToolStripMenuItem toolStripMenuItem1;



        private void UCCRMFollowUpPlansList_Load(object sender, EventArgs e)
        {
            ContextMenuStrip newContextMenuStrip = base.dataGridView1.GetContextMenu(contextMenuStrip1);

            // 初始化ContextMenuStrip 中的一项 如果未开始 可以设置为已取消
            toolStripMenuItem1 = new ToolStripMenuItem("取消跟进计划");
            toolStripMenuItem1.Click += new System.EventHandler(this.toolStripButton取消跟进计划_Click);
            //插到前面
            newContextMenuStrip.Items.Insert(0, toolStripMenuItem1);


            // 将ContextMenuStrip关联到DataGridView
            dataGridView1.ContextMenuStrip = contextMenuStrip1;

            base.dataGridView1.ContextMenuStrip = newContextMenuStrip;
        }

        private async void toolStripButton取消跟进计划_Click(object sender, EventArgs e)
        {
            if (base.bindingSourceList.Current != null)
            {
                if (bindingSourceList.Current is tb_CRM_FollowUpPlans plan)
                {
                    //如果计划已取消  或计划未执行 则不能添加记录了。
                    //如果未开始  进行中  延期中 则能添加记录

                    if (plan.PlanStatus == (int)FollowUpPlanStatus.未开始 || plan.PlanStatus == (int)FollowUpPlanStatus.延期中)
                    {
                        plan.PlanStatus = (int)FollowUpPlanStatus.已取消;

                        BaseController<tb_CRM_FollowUpPlans> ctrPlan = Startup.GetFromFacByName<BaseController<tb_CRM_FollowUpPlans>>(typeof(tb_CRM_FollowUpPlans).Name + "Controller");
                        ReturnResults<tb_CRM_FollowUpPlans> result = await ctrPlan.BaseSaveOrUpdate(plan);
                        if (result.Succeeded)
                        {
                            MainForm.Instance.ShowStatusText("操作成功");
                            Query();
                        }
                        else
                        {
                            MainForm.Instance.ShowStatusText("操作失败");
                        }
                    }
                }

            }
        }


        /// <summary>
        /// 重写基类方法，这个特殊少用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            // 确保点击的是单元格，而不是行的其他部分
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // 获取当前行的数据
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // 根据数据情况启用或禁用菜单项
                // 例如，如果某一列的值为特定值，启用或禁用某个菜单项
                if (row.DataBoundItem is tb_CRM_FollowUpPlans plan)
                {
                    if (plan.PlanStatus == (int)FollowUpPlanStatus.未开始 || plan.PlanStatus == (int)FollowUpPlanStatus.延期中)
                    {
                        toolStripMenuItem1.Enabled = true;
                    }
                    else
                    {
                        toolStripMenuItem1.Visible = false;
                    }
                    if (plan.PlanStatus == (int)FollowUpPlanStatus.已取消)//|| plan.PlanStatus == (int)FollowUpPlanStatus.已完成
                    {
                        添加跟进记录ToolStripMenuItem.Visible = false;
                    }
                    else
                    {
                        添加跟进记录ToolStripMenuItem.Visible = true;
                    }
                }
                else
                {
                    toolStripMenuItem1.Visible = false;
                }
                // 显示上下文菜单
                contextMenuStrip1.Show(dataGridView1, e.Location);
            }
        }
        public override async Task<List<tb_CRM_FollowUpPlans>> Save()
        {
            List<tb_CRM_FollowUpPlans> list = await base.Save();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    //向服务器推送工作流提醒的列表 typeof(T).Name
                    if (item.PlanStatus == (int)FollowUpPlanStatus.未开始)
                    {
                        ReminderData request = new ReminderData();
                        request.BizPrimaryKey = item.PlanID;
                        request.BizType = BizType.CRM跟进计划;
                        request.StartTime = item.PlanStartDate;
                        request.EndTime = item.PlanEndDate;
                        request.RemindSubject = item.PlanSubject;
                        request.ReminderContent = item.PlanContent;
                        request.ReceiverEmployeeIDs = new List<long>();
                        if (MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.HasValue)
                        {
                            request.ReceiverEmployeeIDs.Add(MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value);

                        }

                        //request.SenderEmployeeName = MainForm.Instance.AppContext.CurUserInfo.Name;

                        OriginalData beatDataDel = ClientDataBuilder.工作流提醒请求(request);
                        MainForm.Instance.ecs.AddSendData(beatDataDel);
                    }
                }

            }
            return list;
        }
        private async void 添加跟进记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (base.bindingSourceList.Current != null)
            {
                if (bindingSourceList.Current is tb_CRM_FollowUpPlans plan)
                {
                    //如果计划已取消  或计划未执行 则不能添加记录了。
                    //如果未开始  进行中  延期中 则能添加记录
                    if (plan.PlanStatus == (int)FollowUpPlanStatus.未开始 || plan.PlanStatus == (int)FollowUpPlanStatus.进行中 || plan.PlanStatus == (int)FollowUpPlanStatus.延期中)
                    {
                        //如果计划开始时间，还没有到，则提醒是否提前跟进。
                        if (plan.PlanStartDate > DateTime.Now)
                        {
                            if (MessageBox.Show("计划开始时间还没有到，是否提前跟进?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                            {
                                return;
                            }
                        }
                        object frm = Activator.CreateInstance(typeof(UCCRMFollowUpRecordsEdit));
                        if (frm.GetType().BaseType.Name.Contains("BaseEditGeneric"))
                        {
                            BaseEditGeneric<tb_CRM_FollowUpRecords> frmaddg = frm as BaseEditGeneric<tb_CRM_FollowUpRecords>;
                            frmaddg.CurMenuInfo = this.CurMenuInfo;
                            frmaddg.Text = "跟进记录编辑";
                            frmaddg.bindingSourceEdit.DataSource = new List<tb_CRM_FollowUpRecords>();
                            object obj = frmaddg.bindingSourceEdit.AddNew();
                            tb_CRM_FollowUpRecords EntityInfo = obj as tb_CRM_FollowUpRecords;
                            EntityInfo.Customer_id = plan.Customer_id;
                            EntityInfo.PlanID = plan.PlanID;
                            BusinessHelper.Instance.InitEntity(EntityInfo);
                            BaseEntity bty = EntityInfo as BaseEntity;
                            bty.ActionStatus = ActionStatus.加载;
                            frmaddg.BindData(bty, ActionStatus.新增);
                            if (frmaddg.ShowDialog() == DialogResult.OK)
                            {
                                BaseController<tb_CRM_FollowUpRecords> ctrRecords = Startup.GetFromFacByName<BaseController<tb_CRM_FollowUpRecords>>(typeof(tb_CRM_FollowUpRecords).Name + "Controller");
                                ReturnResults<tb_CRM_FollowUpRecords> result = await ctrRecords.BaseSaveOrUpdate(EntityInfo);
                                if (result.Succeeded)
                                {
                                    //记录添加成功后。客户如果是新客户 则转换为 潜在客户
                                    if (plan.tb_crm_customer != null)
                                    {
                                        if (plan.tb_crm_customer.CustomerStatus == (int)CustomerStatus.新增客户)
                                        {
                                            plan.tb_crm_customer.CustomerStatus = (int)CustomerStatus.潜在客户;
                                            BaseController<tb_CRM_Customer> ctrContactInfo = Startup.GetFromFacByName<BaseController<tb_CRM_Customer>>(typeof(tb_CRM_Customer).Name + "Controller");
                                            ReturnResults<tb_CRM_Customer> resultCustomer = await ctrContactInfo.BaseSaveOrUpdate(plan.tb_crm_customer);
                                            if (resultCustomer.Succeeded)
                                            {

                                            }
                                        }

                                        //根据要缓存的列表集合来判断是否需要上传到服务器。让服务器分发到其他客户端
                                        KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
                                        //只处理需要缓存的表
                                        if (BizCacheHelper.Manager.NewTableList.TryGetValue(typeof(tb_CRM_FollowUpRecords).Name, out pair))
                                        {
                                            //如果有更新变动就上传到服务器再分发到所有客户端
                                            OriginalData odforCache = ActionForClient.更新缓存<tb_CRM_FollowUpRecords>(result.ReturnObject);
                                            byte[] buffer = CryptoProtocol.EncryptClientPackToServer(odforCache);
                                            MainForm.Instance.ecs.client.Send(buffer);
                                        }
                                    }

                                    //
                                    if (plan.PlanStatus == (int)FollowUpPlanStatus.未开始)
                                    {
                                        //修改为进行中
                                        plan.PlanStatus = (int)FollowUpPlanStatus.进行中;
                                        BaseController<tb_CRM_FollowUpPlans> ctrplan = Startup.GetFromFacByName<BaseController<tb_CRM_FollowUpPlans>>(typeof(tb_CRM_FollowUpPlans).Name + "Controller");
                                        ReturnResults<tb_CRM_FollowUpPlans> rsPlan = await ctrplan.BaseSaveOrUpdate(plan);
                                        if (rsPlan.Succeeded)
                                        {

                                        }

                                    }



                                    MainForm.Instance.ShowStatusText("添加成功!");
                                }
                                else
                                {
                                    MainForm.Instance.ShowStatusText("添加失败!");
                                }
                            }
                        }
                    }
                    else
                    {
                        //提示出计划的状态。不能添加记录。
                        MessageBox.Show($"跟进计划状态为【{((FollowUpPlanStatus)plan.PlanStatus)}】,不能添加记录。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }


                }

            }
        }
    }
}
