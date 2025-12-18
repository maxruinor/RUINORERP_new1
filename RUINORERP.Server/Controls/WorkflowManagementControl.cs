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
                // 可以在这里添加查看详细信息的逻辑
                MessageBox.Show($"工作流详细信息:\nID: {selectedWorkflow.Id}\n业务类型: {selectedWorkflow.BizType}\n状态: {selectedWorkflow.Status}", 
                    "工作流详情", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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