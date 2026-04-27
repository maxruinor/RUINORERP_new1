using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Server.Configuration;
using RUINORERP.Server.Workflow;
using WorkflowCore.Interface;

namespace RUINORERP.Server.Controls
{
    /// <summary>
    /// 库存快照管理控件
    /// 提供库存快照的配置、监控和手动触发功能
    /// </summary>
    public partial class InventorySnapshotManagementControl : UserControl
    {
        private IWorkflowHost _workflowHost;
        private ScheduledTaskConfiguration _taskConfig;
        private BindingList<ScheduledTask> _taskList;

        public InventorySnapshotManagementControl()
        {
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            try
            {
                // 获取工作流主机
                _workflowHost = Program.WorkflowHost;
                
                // 加载定时任务配置
                _taskConfig = ScheduledTaskConfiguration.GetInstance();
                
                // 初始化任务列表（只显示库存快照相关任务）
                _taskList = new BindingList<ScheduledTask>(
                    _taskConfig.Tasks.Where(t => t.Id == "InventorySnapshot").ToList()
                );
                
                dataGridViewTasks.DataSource = _taskList;
                
                // 设置DataGridView属性
                dataGridViewTasks.AutoGenerateColumns = false;
                dataGridViewTasks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridViewTasks.MultiSelect = false;
                dataGridViewTasks.ReadOnly = true;
                
                // 刷新显示
                RefreshDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化失败: {ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InventorySnapshotManagementControl_Load(object sender, EventArgs e)
        {
            RefreshDisplay();
        }

        /// <summary>
        /// 刷新显示
        /// </summary>
        private void RefreshDisplay()
        {
            try
            {
                if (_taskList.Count > 0)
                {
                    var task = _taskList[0];
                    
                    // 更新状态显示
                    lblStatus.Text = task.Enabled ? "已启用" : "已禁用";
                    lblStatus.ForeColor = task.Enabled ? Color.Green : Color.Red;
                    
                    // 更新时间显示
                    lblExecutionTime.Text = task.ExecutionTime;
                    
                    // 计算下次执行时间
                    var nextRunTime = ScheduledTaskHelper.CalculateNextExecutionTime(task.Id);
                    lblNextRunTime.Text = nextRunTime.ToString("yyyy-MM-dd HH:mm:ss");
                    
                    // 更新调试模式状态
                    lblDebugMode.Text = InventorySnapshotWorkflowConfig.DebugMode ? "是" : "否";
                    lblDebugInterval.Text = "由WorkflowCore管理";  // 【修复】不再使用自定义间隔
                    
                    // 更新间隔类型
                    lblIntervalType.Text = task.IntervalType == IntervalType.Daily ? "每日执行" : "循环执行";
                }
                
                // 刷新DataGridView
                if (dataGridViewTasks.InvokeRequired)
                {
                    dataGridViewTasks.Invoke(new MethodInvoker(() => dataGridViewTasks.Refresh()));
                }
                else
                {
                    dataGridViewTasks.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"刷新显示失败: {ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region 配置管理

        /// <summary>
        /// 启用/禁用任务
        /// </summary>
        private async void btnToggleEnabled_Click(object sender, EventArgs e)
        {
            try
            {
                if (_taskList.Count == 0) return;
                
                var task = _taskList[0];
                bool newState = !task.Enabled;
                
                var result = MessageBox.Show(
                    $"确定要{(newState ? "启用" : "禁用")}库存快照任务吗？",
                    "确认",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );
                
                if (result != DialogResult.Yes) return;
                
                // 更新配置
                _taskConfig.SetTaskEnabled(task.Id, newState);
                _taskConfig.Save();  // 【修复】持久化到磁盘
                
                // 重新加载配置
                _taskConfig.Reload();
                _taskList.Clear();
                foreach (var t in _taskConfig.Tasks.Where(t => t.Id == "InventorySnapshot"))
                {
                    _taskList.Add(t);
                }
                
                RefreshDisplay();
                
                // 【修复】记录操作审计日志
                LogConfigurationChange("启用/禁用任务", $"任务ID: {task.Id}, 新状态: {newState}");
                
                MessageBox.Show(
                    $"库存快照任务已{(newState ? "启用" : "禁用")}",
                    "成功",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"操作失败: {ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 修改执行时间
        /// </summary>
        private void btnEditTime_Click(object sender, EventArgs e)
        {
            try
            {
                if (_taskList.Count == 0) return;
                
                var task = _taskList[0];
                
                using (var dialog = new TimePickerDialog(task.ExecutionTime))
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        string newTime = dialog.SelectedTime;
                        
                        // 验证时间格式
                        if (!TimeSpan.TryParse(newTime, out _))
                        {
                            MessageBox.Show("时间格式不正确，请使用 HH:mm:ss 格式", "错误",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        
                        // 更新配置
                        _taskConfig.SetExecutionTime(task.Id, newTime);
                        _taskConfig.Save();  // 【修复】持久化到磁盘
                        
                        // 重新加载配置
                        _taskConfig.Reload();
                        _taskList.Clear();
                        foreach (var t in _taskConfig.Tasks.Where(t => t.Id == "InventorySnapshot"))
                        {
                            _taskList.Add(t);
                        }
                        
                        RefreshDisplay();
                        
                        MessageBox.Show(
                            $"执行时间已更新为: {newTime}\n重启服务后生效",
                            "成功",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"修改时间失败: {ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 切换调试模式
        /// </summary>
        private void btnToggleDebugMode_Click(object sender, EventArgs e)
        {
            try
            {
                bool newState = !InventorySnapshotWorkflowConfig.DebugMode;
                
                var result = MessageBox.Show(
                    $"确定要{(newState ? "启用" : "禁用")}调试模式吗？\n\n" +
                    $"注意：调试模式仅影响手动触发功能，定时调度由WorkflowCore管理",
                    "确认",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );
                
                if (result != DialogResult.Yes) return;
                
                InventorySnapshotWorkflowConfig.DebugMode = newState;
                
                RefreshDisplay();
                
                MessageBox.Show(
                    $"调试模式已{(newState ? "启用" : "禁用")}",
                    "成功",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"操作失败: {ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 设置调试间隔（已废弃，由WorkflowCore管理）
        /// </summary>
        private void btnSetDebugInterval_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "定时调度已由WorkflowCore工作流引擎管理，\n" +
                "请在配置文件scheduled_tasks.json中修改执行时间。\n\n" +
                "当前配置：" + ScheduledTaskHelper.GetTaskExecutionTime(ScheduledTaskHelper.InventorySnapshotTask),
                "提示",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        #endregion

        #region 手动触发

        /// <summary>
        /// 立即执行快照
        /// </summary>
        private async void btnTriggerNow_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MessageBox.Show(
                    "确定要立即执行库存快照吗？\n\n这将生成当前时间的库存快照数据。",
                    "确认",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );
                
                if (result != DialogResult.Yes) return;
                
                btnTriggerNow.Enabled = false;
                btnTriggerNow.Text = "执行中...";
                
                // 手动触发工作流
                bool success = await InventorySnapshotWorkflowConfig.TriggerManually(_workflowHost);
                
                if (success)
                {
                    MessageBox.Show(
                        "库存快照已成功触发！\n\n请查看日志了解执行情况。",
                        "成功",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    
                    // 更新下次执行时间
                    RefreshDisplay();
                }
                else
                {
                    MessageBox.Show(
                        "库存快照触发失败，请查看日志了解详情。",
                        "错误",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"触发失败: {ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                frmMainNew.Instance?.PrintErrorLog($"手动触发快照异常: {ex}");
            }
            finally
            {
                btnTriggerNow.Enabled = true;
                btnTriggerNow.Text = "立即执行";
            }
        }

        #endregion

        #region 快捷时间设置

        /// <summary>
        /// 设置为凌晨1点
        /// </summary>
        private void SetTimeTo01AM(object sender, EventArgs e)
        {
            SetExecutionTime("01:00:00");
        }

        /// <summary>
        /// 设置为凌晨2点
        /// </summary>
        private void SetTimeTo02AM(object sender, EventArgs e)
        {
            SetExecutionTime("02:00:00");
        }

        /// <summary>
        /// 设置为凌晨3点
        /// </summary>
        private void SetTimeTo03AM(object sender, EventArgs e)
        {
            SetExecutionTime("03:00:00");
        }

        /// <summary>
        /// 设置为凌晨4点
        /// </summary>
        private void SetTimeTo04AM(object sender, EventArgs e)
        {
            SetExecutionTime("04:00:00");
        }

        /// <summary>
        /// 设置执行时间
        /// </summary>
        private void SetExecutionTime(string time)
        {
            try
            {
                if (_taskList.Count == 0) return;
                
                // 【修复】验证时间格式
                if (!TimeSpan.TryParse(time, out TimeSpan executionTime))
                {
                    MessageBox.Show("时间格式不正确，请使用 HH:mm:ss 格式", "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                // 【修复】验证时间合理性
                if (executionTime.TotalHours < 0 || executionTime.TotalHours >= 24)
                {
                    MessageBox.Show("执行时间必须在00:00:00到23:59:59之间", "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                var task = _taskList[0];
                
                var result = MessageBox.Show(
                    $"确定要将执行时间设置为 {time} 吗？",
                    "确认",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );
                
                if (result != DialogResult.Yes) return;
                
                _taskConfig.SetExecutionTime(task.Id, time);
                _taskConfig.Save();  // 【修复】持久化到磁盘
                
                // 重新加载配置
                _taskConfig.Reload();
                _taskList.Clear();
                foreach (var t in _taskConfig.Tasks.Where(t => t.Id == "InventorySnapshot"))
                {
                    _taskList.Add(t);
                }
                
                RefreshDisplay();
                
                // 【修复】记录操作审计日志
                LogConfigurationChange("修改执行时间", $"任务ID: {task.Id}, 新时间: {time}");
                
                MessageBox.Show(
                    $"执行时间已更新为: {time}\n重启服务后生效",
                    "成功",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"设置失败: {ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 帮助信息

        /// <summary>
        /// 显示帮助信息
        /// </summary>
        private void btnHelp_Click(object sender, EventArgs e)
        {
            var helpText = new StringBuilder();
            helpText.AppendLine("库存快照功能说明：");
            helpText.AppendLine();
            helpText.AppendLine("1. 功能作用：");
            helpText.AppendLine("   - 每日自动生成库存快照，记录库存历史数据");
            helpText.AppendLine("   - 自动清理过期快照（默认保留12个月）");
            helpText.AppendLine("   - 采用差异化策略，仅对有变化的库存生成快照");
            helpText.AppendLine();
            helpText.AppendLine("2. 执行时间建议：");
            helpText.AppendLine("   - 推荐在夜间非工作时间执行（凌晨1-4点）");
            helpText.AppendLine("   - 避免在业务高峰期执行，影响系统性能");
            helpText.AppendLine();
            helpText.AppendLine("3. 调试模式：");
            helpText.AppendLine("   - 用于测试和验证功能");
            helpText.AppendLine("   - 按固定间隔执行（默认5分钟）");
            helpText.AppendLine("   - 生产环境请关闭调试模式");
            helpText.AppendLine();
            helpText.AppendLine("4. 注意事项：");
            helpText.AppendLine("   - 修改执行时间后需重启服务才生效");
            helpText.AppendLine("   - 首次执行会生成所有库存的初始快照");
            helpText.AppendLine("   - 后续只生成有变化的库存快照");
            
            MessageBox.Show(helpText.ToString(), "帮助", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 记录配置变更审计日志
        /// </summary>
        private void LogConfigurationChange(string operation, string detail)
        {
            try
            {
                var logEntry = $"[配置审计] 操作: {operation}, 详情: {detail}, " +
                    $"操作员: {Environment.UserName}, 时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                
                frmMainNew.Instance?.PrintInfoLog(logEntry);
            }
            catch (Exception ex)
            {
                // 审计日志失败不影响主流程
                System.Diagnostics.Debug.WriteLine($"审计日志记录失败: {ex.Message}");
            }
        }

        #endregion
    }

    /// <summary>
    /// 时间选择对话框
    /// </summary>
    public class TimePickerDialog : Form
    {
        private NumericUpDown nudHour;
        private NumericUpDown nudMinute;
        private NumericUpDown nudSecond;
        private Button btnOK;
        private Button btnCancel;
        
        public string SelectedTime { get; private set; }

        public TimePickerDialog(string currentTime)
        {
            InitializeComponents(currentTime);
        }

        private void InitializeComponents(string currentTime)
        {
            this.Text = "设置执行时间";
            this.Size = new Size(300, 200);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // 解析当前时间
            TimeSpan time = TimeSpan.Parse(currentTime);

            // 小时
            var lblHour = new Label { Text = "时:", Location = new Point(20, 30), AutoSize = true };
            nudHour = new NumericUpDown 
            { 
                Minimum = 0, 
                Maximum = 23, 
                Value = time.Hours,
                Location = new Point(60, 28),
                Width = 60
            };

            // 分钟
            var lblMinute = new Label { Text = "分:", Location = new Point(130, 30), AutoSize = true };
            nudMinute = new NumericUpDown 
            { 
                Minimum = 0, 
                Maximum = 59, 
                Value = time.Minutes,
                Location = new Point(170, 28),
                Width = 60
            };

            // 秒
            var lblSecond = new Label { Text = "秒:", Location = new Point(20, 70), AutoSize = true };
            nudSecond = new NumericUpDown 
            { 
                Minimum = 0, 
                Maximum = 59, 
                Value = time.Seconds,
                Location = new Point(60, 68),
                Width = 60
            };

            // 按钮
            btnOK = new Button 
            { 
                Text = "确定", 
                Location = new Point(80, 120),
                DialogResult = DialogResult.OK
            };
            btnOK.Click += (s, e) => 
            {
                SelectedTime = $"{nudHour.Value:D2}:{nudMinute.Value:D2}:{nudSecond.Value:D2}";
                this.Close();
            };

            btnCancel = new Button 
            { 
                Text = "取消", 
                Location = new Point(160, 120),
                DialogResult = DialogResult.Cancel
            };

            this.Controls.AddRange(new Control[] { 
                lblHour, nudHour, lblMinute, nudMinute, lblSecond, nudSecond, 
                btnOK, btnCancel 
            });
            
            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
        }
    }

    /// <summary>
    /// 输入对话框
    /// </summary>
    public class InputDialog : Form
    {
        private TextBox txtInput;
        private Button btnOK;
        private Button btnCancel;
        
        public string InputValue { get; private set; }

        public InputDialog(string prompt, string defaultValue = "")
        {
            InitializeComponents(prompt, defaultValue);
        }

        private void InitializeComponents(string prompt, string defaultValue)
        {
            this.Text = "输入";
            this.Size = new Size(350, 150);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // 提示标签
            var lblPrompt = new Label 
            { 
                Text = prompt, 
                Location = new Point(20, 20), 
                AutoSize = true,
                MaximumSize = new Size(300, 0)
            };

            // 输入框
            txtInput = new TextBox 
            { 
                Text = defaultValue,
                Location = new Point(20, 50),
                Width = 300
            };

            // 按钮
            btnOK = new Button 
            { 
                Text = "确定", 
                Location = new Point(120, 85),
                DialogResult = DialogResult.OK
            };
            btnOK.Click += (s, e) => 
            {
                InputValue = txtInput.Text.Trim();
                this.Close();
            };

            btnCancel = new Button 
            { 
                Text = "取消", 
                Location = new Point(210, 85),
                DialogResult = DialogResult.Cancel
            };

            this.Controls.AddRange(new Control[] { lblPrompt, txtInput, btnOK, btnCancel });
            
            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
            
            txtInput.SelectAll();
            txtInput.Focus();
        }
    }
}
