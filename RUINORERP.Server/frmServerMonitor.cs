using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Server.Network.Core;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.Server.Network.Monitoring;
using System.Threading;

namespace RUINORERP.Server
{
    public partial class frmServerMonitor : Form
    {
        private readonly NetworkServer _networkServer;
        private readonly ISessionService _sessionService;
        private readonly CommandDispatcher _commandDispatcher;
        private readonly DiagnosticsService _diagnosticsService;
        private readonly System.Windows.Forms.Timer _refreshTimer;
        
        // 用于控制刷新频率的字段
        private int _refreshCount = 0;
        private const int FAST_REFRESH_COUNT = 10; // 前10次快速刷新
        private const int SLOW_REFRESH_INTERVAL = 5000; // 慢速刷新间隔(5秒)
        private const int FAST_REFRESH_INTERVAL = 1000; // 快速刷新间隔(1秒)

        public frmServerMonitor()
        {
            InitializeComponent();
            
            // 从全局服务提供者获取所需服务
            _networkServer = Startup.GetFromFac<NetworkServer>();
            _sessionService = Startup.GetFromFac<ISessionService>();
            _commandDispatcher = Startup.GetFromFac<CommandDispatcher>();
            _diagnosticsService = new DiagnosticsService(_commandDispatcher);
            
            // 初始化定时器
            _refreshTimer = new System.Windows.Forms.Timer();
            _refreshTimer.Interval = FAST_REFRESH_INTERVAL; // 初始快速刷新
            _refreshTimer.Tick += RefreshTimer_Tick;
            
            // 初始化UI
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Text = "服务器监控";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Normal;
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            
            // 刷新一次数据
            RefreshData();
        }

        private void frmServerMonitor_Load(object sender, EventArgs e)
        {
            // 启动定时器
            _refreshTimer.Start();
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            RefreshData();
            
            // 控制刷新频率
            _refreshCount++;
            if (_refreshCount >= FAST_REFRESH_COUNT && _refreshTimer.Interval == FAST_REFRESH_INTERVAL)
            {
                _refreshTimer.Interval = SLOW_REFRESH_INTERVAL;
            }
        }

        private void RefreshData()
        {
            try
            {
                // 更新服务器状态信息
                UpdateServerStatus();
                
                // 更新会话统计信息
                UpdateSessionStatistics();
                
                // 更新服务器运行信息
                UpdateServerRuntimeInfo();
                
                // 更新命令调度器信息
                UpdateCommandDispatcherInfo();
                
                // 更新处理器统计信息
                UpdateHandlerStatistics();
            }
            catch (Exception ex)
            {
                // 记录错误但不中断程序
                Console.WriteLine($"刷新监控数据时出错: {ex.Message}");
            }
        }

        private void UpdateServerStatus()
        {
            if (_networkServer == null) return;
            
            var serverInfo = _networkServer.GetServerInfo();
            
            // 更新基本服务器信息
            lblStatusValue.Text = serverInfo.Status;
            lblPortValue.Text = serverInfo.Port.ToString();
            lblServerIpValue.Text = serverInfo.ServerIp;
            lblMaxConnectionsValue.Text = serverInfo.MaxConnections.ToString();
            
            // 更新连接数信息
            lblCurrentConnectionsValue.Text = serverInfo.CurrentConnections.ToString();
            lblTotalConnectionsValue.Text = serverInfo.TotalConnections.ToString();
            lblPeakConnectionsValue.Text = serverInfo.PeakConnections.ToString();
            
            // 更新最后活动时间
            lblLastActivityValue.Text = serverInfo.LastActivityTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void UpdateSessionStatistics()
        {
            if (_sessionService == null) return;
            
            var stats = _sessionService.GetStatistics();
            
            // 更新会话统计信息
            lblActiveSessionsValue.Text = stats.CurrentConnections.ToString();
            lblTotalSessionsValue.Text = stats.TotalConnections.ToString();
            lblPeakSessionsValue.Text = stats.PeakConnections.ToString();
            lblTimeoutSessionsValue.Text = stats.TimeoutSessions.ToString();
            lblHeartbeatFailuresValue.Text = stats.HeartbeatFailures.ToString();
            
            // 更新时间信息
            lblLastCleanupValue.Text = stats.LastCleanupTime.ToString("yyyy-MM-dd HH:mm:ss");
            lblLastHeartbeatCheckValue.Text = stats.LastHeartbeatCheck.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void UpdateServerRuntimeInfo()
        {
            // 更新系统时间
            lblSystemTimeValue.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            
            // 更新运行时间（这里需要根据实际情况计算）
            lblUptimeValue.Text = "N/A"; // 需要从NetworkServer获取启动时间来计算
            
            // 更新内存使用情况（需要实现获取内存使用的方法）
            lblMemoryUsageValue.Text = "N/A"; // 需要实现获取内存使用情况的方法
        }

        private void UpdateCommandDispatcherInfo()
        {
            if (_commandDispatcher == null) return;
            
            // 更新命令调度器信息
            lblDispatcherInitializedValue.Text = _commandDispatcher.IsInitialized ? "是" : "否";
            lblHandlerCountValue.Text = _commandDispatcher.HandlerCount.ToString();
        }

        private void UpdateHandlerStatistics()
        {
            if (_commandDispatcher == null) return;
            
            // 获取所有处理器和统计信息
            var handlers = _commandDispatcher.GetAllHandlers();
            var statistics = _diagnosticsService.GetHandlerStatistics();
            
            // 清空现有数据
            dgvHandlerStatistics.Rows.Clear();
            
            // 添加数据
            foreach (var handler in handlers)
            {
                // 从处理器获取状态和优先级信息
                var status = handler.Status;
                var priority = handler.Priority;
                
                // 从统计信息字典中获取统计信息
                HandlerStatistics handlerStats = null;
                if (statistics.TryGetValue(handler.HandlerId, out var stats))
                {
                    handlerStats = stats;
                }
                
                var rowIndex = dgvHandlerStatistics.Rows.Add(
                    handler.Name,  // 使用处理器名称而不是HandlerId
                    status.ToString(),
                    priority.ToString(),
                    handlerStats?.TotalCommandsProcessed ?? 0,
                    handlerStats?.SuccessfulCommands ?? 0,
                    handlerStats?.FailedCommands ?? 0,
                    handlerStats?.TimeoutCount ?? 0,
                    handlerStats?.CurrentProcessingCount ?? 0,
                    handlerStats?.AverageProcessingTimeMs ?? 0,
                    handlerStats?.MaxProcessingTimeMs ?? 0
                );
                
                // 根据状态设置行颜色
                if (status != HandlerStatus.Running)
                {
                    dgvHandlerStatistics.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightCoral;
                }
                else if ((handlerStats?.FailedCommands ?? 0) > 0)
                {
                    dgvHandlerStatistics.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightYellow;
                }
            }
        }

        private void frmServerMonitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 停止定时器
            _refreshTimer.Stop();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void btnResetStats_Click(object sender, EventArgs e)
        {
            try
            {
                _diagnosticsService.ResetAllStatistics();
                RefreshData();
                MessageBox.Show("统计信息已重置", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"重置统计信息时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}