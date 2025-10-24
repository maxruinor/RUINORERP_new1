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

namespace RUINORERP.Server.Controls
{
    public partial class ServerMonitorControl : UserControl
    {
        private readonly NetworkServer _networkServer;
        private readonly ISessionService _sessionService;
        private readonly CommandDispatcher _commandDispatcher;
        private readonly DiagnosticsService _diagnosticsService;
        private readonly PerformanceMonitoringService _performanceMonitoringService;
        private readonly ErrorAnalysisService _errorAnalysisService;
        private readonly CircuitBreakerMetrics _circuitBreakerMetrics;
        private readonly System.Windows.Forms.Timer _refreshTimer;
        
        // 用于控制刷新频率的字段
        private int _refreshCount = 0;
        private const int FAST_REFRESH_COUNT = 10; // 前10次快速刷新
        private const int SLOW_REFRESH_INTERVAL = 5000; // 慢速刷新间隔(5秒)
        private const int FAST_REFRESH_INTERVAL = 1000; // 快速刷新间隔(1秒)

        /// <summary>
        /// 创建服务器监控控件
        /// </summary>
        public ServerMonitorControl()
        {
            InitializeComponent();
            
            // 从全局服务提供者获取所需服务
            _networkServer = Startup.GetFromFac<NetworkServer>();
            _sessionService = Startup.GetFromFac<ISessionService>();
            _commandDispatcher = Startup.GetFromFac<CommandDispatcher>();
            _diagnosticsService = Startup.GetFromFac<DiagnosticsService>();
            _performanceMonitoringService = Startup.GetFromFac<PerformanceMonitoringService>();
            _errorAnalysisService = Startup.GetFromFac<ErrorAnalysisService>();
            _circuitBreakerMetrics = Startup.GetFromFac<CircuitBreakerMetrics>();
            
            // 初始化定时器
            _refreshTimer = new System.Windows.Forms.Timer();
            _refreshTimer.Interval = FAST_REFRESH_INTERVAL; // 初始快速刷新
            _refreshTimer.Tick += RefreshTimer_Tick;
            
            // 初始化UI
            InitializeUI();
            
            // 立即启动定时器刷新数据（不依赖Load事件）
            _refreshTimer.Start();
        }

        /// <summary>
        /// 创建服务器监控控件，使用指定的NetworkServer实例
        /// </summary>
        /// <param name="networkServer">正在运行的NetworkServer实例</param>
        public ServerMonitorControl(NetworkServer networkServer)
        {
            InitializeComponent();
            
            // 使用传入的NetworkServer实例
            _networkServer = networkServer;
            
            // 从全局服务提供者获取其他所需服务
            _sessionService = Startup.GetFromFac<ISessionService>();
            _commandDispatcher = Startup.GetFromFac<CommandDispatcher>();
            _diagnosticsService = Startup.GetFromFac<DiagnosticsService>();
            _performanceMonitoringService = Startup.GetFromFac<PerformanceMonitoringService>();
            _errorAnalysisService = Startup.GetFromFac<ErrorAnalysisService>();
            _circuitBreakerMetrics = Startup.GetFromFac<CircuitBreakerMetrics>();
            
            // 初始化定时器
            _refreshTimer = new System.Windows.Forms.Timer();
            _refreshTimer.Interval = FAST_REFRESH_INTERVAL; // 初始快速刷新
            _refreshTimer.Tick += RefreshTimer_Tick;
            
            // 初始化UI
            InitializeUI();
            
            // 立即启动定时器刷新数据（不依赖Load事件）
            _refreshTimer.Start();
        }

        private void InitializeUI()
        {
            // 刷新一次数据
            RefreshData();
        }

        private void ServerMonitorControl_Load(object sender, EventArgs e)
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
                
                // 更新系统健康状态
                UpdateSystemHealth();
                
                // 更新实时监控数据
            UpdateRealTimeData();
            
            // 更新熔断器监控数据
            UpdateCircuitBreakerMetrics();
            }
            catch (Exception ex)
            {
                // 记录错误但不中断程序
                Console.WriteLine($"刷新监控数据时出错: {ex.Message}");
                // 在UI上显示错误信息
                lblStatusValue.Text = "监控数据刷新错误";
                lblStatusValue.ForeColor = Color.Red;
            }
        }

        private void UpdateServerStatus()
        {
            if (_networkServer == null) 
            {
                // 如果_networkServer为null，显示未初始化状态
                lblStatusValue.Text = "未初始化";
                lblPortValue.Text = "N/A";
                lblServerIpValue.Text = "N/A";
                lblMaxConnectionsValue.Text = "N/A";
                lblCurrentConnectionsValue.Text = "N/A";
                lblTotalConnectionsValue.Text = "N/A";
                lblPeakConnectionsValue.Text = "N/A";
                lblLastActivityValue.Text = "N/A";
                return;
            }
            
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
            
            // 更新新增的性能指标
            try
            {
                // 更新连接利用率
                double utilization = stats.GetConnectionUtilization();
                lblConnectionUtilizationValue.Text = $"{utilization:F2}%";
                
                // 更新吞吐量
                double throughput = stats.GetAverageThroughput();
                lblThroughputValue.Text = $"{FormatBytes(throughput)}/秒";
                
                // 更新请求率
                double requestsPerSecond = stats.GetRequestsPerSecond();
                lblRequestsPerSecondValue.Text = $"{requestsPerSecond:F2}/秒";
                
                // 更新健康状态
                string healthStatus = stats.GetHealthStatus();
                lblSystemHealthValue.Text = healthStatus;
                lblSystemHealthValue.ForeColor = healthStatus == "正常" ? Color.Green : Color.OrangeRed;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"更新性能指标时出错: {ex.Message}");
            }
        }

        private void UpdateServerRuntimeInfo()
        {
            // 更新系统时间
            lblSystemTimeValue.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            
            // 更新运行时间（使用NetworkServer的StartTime属性）
            if (_networkServer != null && _networkServer.StartTime.HasValue)
            {
                var uptime = DateTime.Now - _networkServer.StartTime.Value;
                lblUptimeValue.Text = $"{uptime.Days}天 {uptime.Hours}小时 {uptime.Minutes}分钟";
            }
            else
            {
                lblUptimeValue.Text = "未启动";
            }
            
            // 更新内存使用情况
            var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            var memoryUsage = currentProcess.WorkingSet64 / (1024 * 1024); // 转换为MB
            lblMemoryUsageValue.Text = $"{memoryUsage} MB";
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

        /// <summary>
        /// 格式化字节数为可读字符串
        /// </summary>
        private string FormatBytes(double bytes)
        {
            string[] suffix = { "B", "KB", "MB", "GB", "TB" };
            int i;
            double doubleBytes = bytes;

            for (i = 0; i < suffix.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                doubleBytes = bytes / 1024.0;
            }

            return $"{doubleBytes:F2} {suffix[i]}";
        }
        
        /// <summary>
        /// 更新系统健康状态
        /// </summary>
        private void UpdateSystemHealth()
        {
            try
            {
                var healthStatus = _diagnosticsService.GetSystemHealth();
                
                // 更新健康状态显示
                lblHealthStatusValue.Text = healthStatus.IsHealthy ? "健康" : "存在问题";
                lblHealthStatusValue.ForeColor = healthStatus.IsHealthy ? Color.Green : Color.Red;
                
                // 更新成功率显示
                cbLblSuccessRateValue.Text = $"{healthStatus.SuccessRate:F2}%";
                
                // 更新命令统计显示
                lblTotalCommandsValue.Text = healthStatus.TotalCommands.ToString();
                lblFailedCommandsValue.Text = healthStatus.FailedCommands.ToString();
                lblTimeoutCommandsValue.Text = healthStatus.TimeoutCommands.ToString();
            }
            catch (Exception ex)
            {
                lblHealthStatusValue.Text = "获取失败";
                lblHealthStatusValue.ForeColor = Color.Red;
                Console.WriteLine($"更新系统健康状态时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 更新实时监控数据
        /// </summary>
        private void UpdateRealTimeData()
        {
            try
            {
                var realTimeData = _performanceMonitoringService.GetRealTimeData();
                
                // 更新实时数据
                lblTotalHandlersValue.Text = realTimeData.TotalHandlers.ToString();
                lblActiveHandlersValue.Text = realTimeData.ActiveHandlers.ToString();
                lblCurrentProcessingValue.Text = realTimeData.CurrentProcessing.ToString();
                lblRealTimeSuccessRateValue.Text = $"{realTimeData.SuccessRate:F2}%";
                lblAvgProcessingTimeValue.Text = $"{realTimeData.AverageProcessingTime:F2}ms";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"更新实时监控数据时出错: {ex.Message}");
            }
        }

        private void ServerMonitorControl_Disposed(object sender, EventArgs e)
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
        
        /// <summary>
        /// 显示诊断报告
        /// </summary>
        private void btnDiagnosticsReport_Click(object sender, EventArgs e)
        {
            try
            {
                var report = _diagnosticsService.GetDiagnosticsReport();
                ShowReport("诊断报告", report);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"获取诊断报告时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 显示性能报告
        /// </summary>
        private void btnPerformanceReport_Click(object sender, EventArgs e)
        {
            try
            {
                var report = _performanceMonitoringService.GetPerformanceReport();
                ShowReport("性能报告", report);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"获取性能报告时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 显示错误分析报告
        /// </summary>
        private void btnErrorReport_Click(object sender, EventArgs e)
        {
            try
            {
                var report = _errorAnalysisService.GetErrorAnalysisReport();
                ShowReport("错误分析报告", report);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"获取错误分析报告时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 显示报告的辅助方法
        /// </summary>
        private void ShowReport(string title, string content)
        {
            var reportForm = new Form();
            reportForm.Text = title;
            reportForm.Size = new Size(800, 600);
            reportForm.StartPosition = FormStartPosition.CenterParent;
            
            var textBox = new TextBox();
            textBox.Dock = DockStyle.Fill;
            textBox.Multiline = true;
            textBox.ScrollBars = ScrollBars.Both;
            textBox.ReadOnly = true;
            textBox.Text = content;
            textBox.Font = new Font("Consolas", 10);
            
            reportForm.Controls.Add(textBox);
            reportForm.ShowDialog(this);
        }
        
        /// <summary>
        /// 更新熔断器监控指标
        /// </summary>
        private void UpdateCircuitBreakerMetrics()
        {
            try
            {
                if (_circuitBreakerMetrics == null)
                {
                    // 如果熔断器指标对象为null，显示默认值
                cbLblStatusValue.Text = "未初始化";
                cbLblTotalRequestsValue.Text = "0";
                cbLblSuccessRateValue.Text = "N/A";
                cbLblFailedRequestsValue.Text = "0";
                cbLblStateChangesValue.Text = "0";
                cbLblAvgResponseTimeValue.Text = "N/A";
                    return;
                }
                
                // 获取全局熔断器指标
                var globalMetrics = _circuitBreakerMetrics.GlobalMetrics;
                
                // 直接从_circuitBreakerMetrics获取状态
                string circuitState = _circuitBreakerMetrics.CurrentState.ToString();
                cbLblStatusValue.Text = circuitState;
                
                // 根据状态设置颜色
                switch (circuitState)
                {
                    case "Closed":
                        cbLblStatusValue.Text = "关闭";
                        cbLblStatusValue.ForeColor = Color.Green;
                        break;
                    case "Open":
                        cbLblStatusValue.Text = "开启";
                        cbLblStatusValue.ForeColor = Color.Red;
                        break;
                    case "HalfOpen":
                        cbLblStatusValue.Text = "半开";
                        cbLblStatusValue.ForeColor = Color.Orange;
                        break;
                    default:
                        cbLblStatusValue.ForeColor = Color.Black;
                        break;
                }
                
                // 更新总请求数
                cbLblTotalRequestsValue.Text = globalMetrics.TotalRequests.ToString();
                
                // 更新成功率
                if (globalMetrics.TotalRequests > 0)
                {
                    double successRate = (double)globalMetrics.SuccessfulRequests / globalMetrics.TotalRequests * 100;
                    cbLblSuccessRateValue.Text = $"{successRate:F2}%";
                }
                else
                {
                    cbLblSuccessRateValue.Text = "100%";
                }
                
                // 更新失败请求数
                cbLblFailedRequestsValue.Text = globalMetrics.FailedRequests.ToString();
                
                // 更新状态变化次数
                cbLblStateChangesValue.Text = globalMetrics.StateChanges.ToString();
                
                // 更新平均响应时间
                if (globalMetrics.AverageExecutionTimeMs > 0)
                {
                    cbLblAvgResponseTimeValue.Text = $"{globalMetrics.AverageExecutionTimeMs:F2}ms";
                }
                else
                {
                    cbLblAvgResponseTimeValue.Text = "0ms";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"更新熔断器监控指标时出错: {ex.Message}");
                // 在UI上显示错误状态
                cbLblStatusValue.Text = "数据获取失败";
                cbLblStatusValue.ForeColor = Color.Red;
            }
        }
    }
}