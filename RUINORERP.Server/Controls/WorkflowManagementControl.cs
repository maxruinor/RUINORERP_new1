using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Model.TransModel;
using RUINORERP.Server.Workflow;
using RUINORERP.Server.Workflow.WFApproval;
using RUINORERP.Server.Workflow.WFPush;
using RUINORERP.Server.Workflow.WFReminder;
using RUINORERP.Server.Workflow.WFScheduled;
using RUINORERP.Server.SmartReminder.Strategies.SafetyStockStrategies;
using WorkflowCore.Interface;
using RUINORERP.Business.CommService;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Server.Comm;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RUINORERP.Business.Cache;
using RUINORERP.Server.BizService;

namespace RUINORERP.Server.Controls
{
    public partial class WorkflowManagementControl : UserControl
    {
        private IWorkflowHost _workflowHost;
        
        // 工作流列表数据源
        public BindingList<ReminderData> WorkflowList { get; set; } = new BindingList<ReminderData>();

        public WorkflowManagementControl()
        {
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            // 初始化工作流列表
            WorkflowList = new BindingList<ReminderData>(frmMainNew.Instance.ReminderBizDataList.Values.ToList());
            dataGridViewWorkflows.DataSource = WorkflowList;
            
            // 初始化工作流主机
            _workflowHost = Program.WorkflowHost;
        }

        private void WorkflowManagementControl_Load(object sender, EventArgs e)
        {
            // 设置数据网格视图属性
            dataGridViewWorkflows.AutoGenerateColumns = false;
            dataGridViewWorkflows.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewWorkflows.MultiSelect = false;
            
            // 刷新数据
            RefreshWorkflowData();
        }

        /// <summary>
        /// 刷新工作流数据
        /// </summary>
        public void RefreshWorkflowData()
        {
            try
            {
                WorkflowList.Clear();
                foreach (var item in frmMainNew.Instance.ReminderBizDataList.Values)
                {
                    WorkflowList.Add(item);
                }
                
                // 刷新DataGridView
                if (dataGridViewWorkflows.InvokeRequired)
                {
                    dataGridViewWorkflows.Invoke(new System.Windows.Forms.MethodInvoker(() => dataGridViewWorkflows.Refresh()));
                }
                else
                {
                    dataGridViewWorkflows.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"刷新工作流数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region 工作流测试功能

        private async void btnStartApprovalWorkflow_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBoxBillId.Text))
                {
                    MessageBox.Show("请输入单据ID", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                //var wfController = Startup.GetFromFac<WFController>();
                //long billId = long.Parse(textBoxBillId.Text);
                //string workflowId = wfController.StartApprovalWorkflow(_workflowHost, billId, frmMainNew.Instance.workflowlist);
                
                //MessageBox.Show($"审批工作流已启动，工作流ID: {workflowId}", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //RefreshWorkflowData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"启动审批工作流时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnStartSafetyStockWorkflow_Click(object sender, EventArgs e)
        {
            try
            {
                // 启动安全库存动态计算工作流
                var data = new SafetyStockData();
                var workflowId = await _workflowHost.StartWorkflow("SafetyStockWorkflow", data);
                
                MessageBox.Show($"安全库存工作流已启动，工作流ID: {workflowId}", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"启动安全库存工作流时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnStartReminderWorkflow_Click(object sender, EventArgs e)
        {
            try
            {
                // 启动提醒工作流
                var data = new ReminderData();
                var workflowId = await _workflowHost.StartWorkflow("ReminderWorkflow", 1, data);
                
                MessageBox.Show($"提醒工作流已启动，工作流ID: {workflowId}", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"启动提醒工作流时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPublishEvent_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBoxEventName.Text) || string.IsNullOrEmpty(textBoxWorkflowId.Text))
                {
                    MessageBox.Show("请输入事件名称和工作流ID", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 发布外部事件
                var data = new ApprovalWFData();
                data.ApprovedDateTime = DateTime.Now;
                
                var approvalEntity = new ApprovalEntity();
                approvalEntity.ApprovalResults = true;
                //approvalEntity.BillID = textBoxBillId.Text;
                //data.approvalEntity = approvalEntity;
                
                //var wfController = Startup.GetFromFac<WFController>();
                //wfController.PublishEvent(_workflowHost, "ApprovalEvent", data, frmMainNew.Instance.workflowlist);
                
                MessageBox.Show("外部事件已发布", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshWorkflowData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发布外部事件时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPushTest_Click(object sender, EventArgs e)
        {
            try
            {
                var data = new PushData();
                data.InputData = "tb_UserInfo";
                //var workflowId = _workflowHost.StartWorkflow("PushBaseInfoWorkflow", data);
                
                MessageBox.Show("推送测试已启动", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"推送测试时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCacheTest_Click(object sender, EventArgs e)
        {
            try
            {
                IMemoryCache cache = Startup.GetFromFac<IMemoryCache>();
                // 使用新的缓存体系
                IEntityCacheManager entityCacheManager = Startup.GetFromFac<IEntityCacheManager>();
                object obj = entityCacheManager.GetEntity("tb_CustomerVendor", 1740971599693221888);
                
                if (obj != null)
                {
                    MessageBox.Show("缓存测试成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"缓存测试时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region DataGridView事件处理

        private void dataGridViewWorkflows_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < WorkflowList.Count)
            {
                var selectedWorkflow = WorkflowList[e.RowIndex];
                ShowWorkflowDetails(selectedWorkflow);
            }
        }

        /// <summary>
        /// 获取当前选中的工作流数据
        /// </summary>
        /// <returns>选中的工作流数据</returns>
        private ReminderData GetSelectedWorkflow()
        {
            if (dataGridViewWorkflows.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewWorkflows.SelectedRows[0];
                if (selectedRow.Index >= 0 && selectedRow.Index < WorkflowList.Count)
                {
                    return WorkflowList[selectedRow.Index];
                }
            }
            return null;
        }

        /// <summary>
        /// 立即触发提醒按钮点击事件
        /// </summary>
        private async void btnTriggerReminder_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedWorkflow = GetSelectedWorkflow();
                if (selectedWorkflow == null)
                {
                    MessageBox.Show("请先选择一个工作流", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                await TriggerReminderWorkflowAsync(selectedWorkflow);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"触发提醒时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 右键菜单-立即触发提醒
        /// </summary>
        private async void toolStripMenuItemTriggerReminder_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedWorkflow = GetSelectedWorkflow();
                if (selectedWorkflow == null)
                {
                    MessageBox.Show("请先选择一个工作流", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                await TriggerReminderWorkflowAsync(selectedWorkflow);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"触发提醒时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 右键菜单-查看详情
        /// </summary>
        private void toolStripMenuItemViewDetails_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedWorkflow = GetSelectedWorkflow();
                if (selectedWorkflow == null)
                {
                    MessageBox.Show("请先选择一个工作流", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ShowWorkflowDetails(selectedWorkflow);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"查看详情时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 直接执行提醒任务逻辑
        /// </summary>
        /// <param name="reminderData">提醒数据</param>
        private async Task TriggerReminderWorkflowAsync(ReminderData reminderData)
        {
            try
            {
                // 确认是否要直接触发提醒
                var result = MessageBox.Show($"是否立即触发此提醒？\n业务ID: {reminderData.BizKeyID}\n主题: {reminderData.RemindSubject}", 
                    "确认触发提醒", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result != DialogResult.Yes)
                {
                    return;
                }

                // 直接执行ReminderTask的提醒逻辑
                await ExecuteReminderTaskLogic(reminderData);
                
                MessageBox.Show("提醒已成功触发！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // 刷新显示
                RefreshWorkflowData();
            }
            catch (Exception ex)
            {
                throw new Exception($"触发提醒失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 执行ReminderTask的提醒逻辑
        /// </summary>
        /// <param name="reminderData">提醒数据</param>
        private async Task ExecuteReminderTaskLogic(ReminderData reminderData)
        {
            try
            {
                // 获取ISessionService实例
                var sessionService = Startup.GetFromFac<RUINORERP.Server.Network.Interfaces.Services.ISessionService>();
                
                // 获取DataServiceChannel实例
                var dataServiceChannel = Startup.GetFromFac<DataServiceChannel>();

                // 获取当前选中的提醒数据
                ReminderData exData = null;
                frmMainNew.Instance.ReminderBizDataList.TryGetValue(reminderData.BizPrimaryKey, out exData);
                
                if (exData == null)
                {
                    throw new Exception("未找到对应的提醒数据");
                }

                // 检查提醒是否已取消或到期
                if (exData.Status == Model.MessageStatus.Cancel)
                {
                    throw new Exception("该提醒已被取消");
                }

                if (exData.EndTime < DateTime.Now)
                {
                    // 提醒到期了
                    dataServiceChannel.ProcessCRMFollowUpPlansData(exData, true);
                    throw new Exception("该提醒已到期");
                }

                // 执行提醒逻辑 - 模拟ReminderTask.Run方法的核心逻辑
                var sessions = sessionService.GetAllUserSessions();
                bool hasSent = false;
                
                foreach (var session in sessions)
                {
                    if (exData.ReceiverUserIDs != null && exData.ReceiverUserIDs.Contains(session.UserInfo.Employee_ID))
                    {
                        try
                        {
                            // 增加提醒次数
                            exData.RemindTimes++;
                            var request = new RUINORERP.PacketSpec.Models.Messaging.MessageRequest(MessageType.Reminder, exData);
                            var success = await sessionService.SendCommandAsync(
                                session.SessionID, 
                                RUINORERP.PacketSpec.Commands.WorkflowCommands.WorkflowReminder, 
                                request);
                            
                            if (success)
                            {
                                // 更新数据
                                frmMainNew.Instance.ReminderBizDataList.TryUpdate(reminderData.BizPrimaryKey, exData, exData);
                                hasSent = true;
                                
                                if (frmMainNew.Instance.IsDebug)
                                {
                                    frmMainNew.Instance.PrintInfoLog($"手动触发提醒推送到{session.UserName}");
                                }
                            }
                            else
                            {
                                frmMainNew.Instance.PrintInfoLog($"发送提醒到用户 {session.UserName} 失败");
                            }
                        }
                        catch (Exception ex)
                        {
                            frmMainNew.Instance.PrintInfoLog($"手动触发提醒推送失败: {session.UserName} - {ex.Message}");
                        }
                    }
                }

                if (!hasSent)
                {
                    throw new Exception("未找到有效的接收用户或发送失败");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"执行提醒任务逻辑失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 显示工作流详细信息
        /// </summary>
        /// <param name="workflow">工作流数据</param>
        private void ShowWorkflowDetails(ReminderData workflow)
        {
            var details = new StringBuilder();
            details.AppendLine($"ID: {workflow.Id}");
            details.AppendLine($"业务类型: {workflow.BizType}");
            details.AppendLine($"业务主键: {workflow.BizPrimaryKey}");
            details.AppendLine($"业务ID: {workflow.BizKeyID}");
            details.AppendLine($"状态: {workflow.Status}");
            details.AppendLine($"优先级: {workflow.Priority}");
            details.AppendLine($"提醒主题: {workflow.RemindSubject}");
            details.AppendLine($"提醒内容: {workflow.ReminderContent}");
            details.AppendLine($"开始时间: {workflow.StartTime}");
            details.AppendLine($"结束时间: {workflow.EndTime}");
            details.AppendLine($"提醒间隔: {workflow.RemindInterval}分钟");
            details.AppendLine($"发送时间: {workflow.SendTime}");
            details.AppendLine($"工作流ID: {workflow.WorkflowId ?? "未启动"}");
            details.AppendLine($"是否已读: {workflow.IsRead}");
            
            if (workflow.ReceiverUserIDs != null && workflow.ReceiverUserIDs.Count > 0)
            {
                details.AppendLine($"接收用户ID: {string.Join(", ", workflow.ReceiverUserIDs)}");
            }

            MessageBox.Show(details.ToString(), "工作流详情", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dataGridViewWorkflows_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 处理数据错误
            System.Diagnostics.Debug.WriteLine($"DataGridView数据错误: {e.Exception?.Message}");
        }

        #endregion

        #region 刷新和清理

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshWorkflowData();
        }

        private void btnClearCompleted_Click(object sender, EventArgs e)
        {
            try
            {
                // 清理已完成的工作流（示例逻辑）
                var completedWorkflows = WorkflowList.Where(w => w.Status.ToString() == "Completed").ToList();
                foreach (var workflow in completedWorkflows)
                {
                    WorkflowList.Remove(workflow);
                }
                
                MessageBox.Show($"已清理 {completedWorkflows.Count} 个已完成的工作流", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"清理工作流时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}