using Castle.Core.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.Cache;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.Server.Network.Core;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.Network.Monitoring;
using RUINORERP.Server.Network.Services;
using RUINORERP.Server.SmartReminder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.Server.Controls
{
    public partial class ServerMonitorControl : UserControl
    {
        private NetworkServer _networkServer;
        private readonly ISessionService _sessionService;
        private readonly CommandDispatcher _commandDispatcher;
        private readonly DiagnosticsService _diagnosticsService;
        private readonly PerformanceMonitoringService _performanceMonitoringService;
        private readonly ErrorAnalysisService _errorAnalysisService;
        private readonly HeartbeatBatchProcessor _heartbeatBatchProcessor;
        private readonly HeartbeatPerformanceMonitor _heartbeatPerformanceMonitor;
        private readonly ILogger<ServerMonitorControl> _logger;
        // 不再使用独立的熔断器指标实例，改为使用CommandDispatcher.Metrics
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
            _heartbeatBatchProcessor = Startup.GetFromFac<HeartbeatBatchProcessor>();
            _heartbeatPerformanceMonitor = Startup.GetFromFac<HeartbeatPerformanceMonitor>();
            _logger = Startup.GetFromFac<ILogger<ServerMonitorControl>>();
            // 不再初始化独立的熔断器指标实例，改为使用CommandDispatcher.Metrics

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
            _heartbeatBatchProcessor = Startup.GetFromFac<HeartbeatBatchProcessor>();
            _heartbeatPerformanceMonitor = Startup.GetFromFac<HeartbeatPerformanceMonitor>();
            _logger = Startup.GetFromFac<ILogger<ServerMonitorControl>>();
            // 不再初始化独立的熔断器指标实例，改为使用CommandDispatcher.Metrics

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

            // 实现动态刷新频率：根据系统负载调整刷新间隔
            // 获取系统CPU使用率，如果使用率超过70%，降低刷新频率
            try
            {
                // 使用PerformanceCounter获取CPU使用率
                using (var cpuCounter = new System.Diagnostics.PerformanceCounter("Processor", "% Processor Time", "_Total"))
                {
                    // 首次采样返回0，需要再次采样
                    cpuCounter.NextValue();
                    System.Threading.Thread.Sleep(100);
                    float cpuUsage = cpuCounter.NextValue();

                    // 根据CPU使用率动态调整刷新频率
                    if (cpuUsage > 70 && _refreshTimer.Interval < SLOW_REFRESH_INTERVAL * 2)
                    {
                        // 高负载时，降低刷新频率到10秒
                        _refreshTimer.Interval = SLOW_REFRESH_INTERVAL * 2;
                        _logger.LogDebug($"系统负载较高 (CPU: {cpuUsage:F2}%)，降低刷新频率到 {_refreshTimer.Interval}ms");
                    }
                    else if (cpuUsage < 30 && _refreshCount >= FAST_REFRESH_COUNT && _refreshTimer.Interval > SLOW_REFRESH_INTERVAL)
                    {
                        // 低负载时，恢复正常刷新频率
                        _refreshTimer.Interval = SLOW_REFRESH_INTERVAL;
                        _logger.LogDebug($"系统负载较低 (CPU: {cpuUsage:F2}%)，恢复正常刷新频率 {_refreshTimer.Interval}ms");
                    }
                }
            }
            catch (Exception ex)
            {
                // 如果PerformanceCounter获取失败，忽略异常，使用默认刷新频率
                _logger.LogDebug($"获取CPU使用率失败，使用默认刷新频率: {ex.Message}");
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

                // 更新心跳性能监控数据
                UpdateHeartbeatPerformanceData();
            }
            catch (Exception ex)
            {
                // 记录错误但不中断程序
                System.Diagnostics.Debug.WriteLine($"刷新监控数据时出错: {ex.Message}");
                // 在UI上显示错误信息
                lblStatusValue.Text = "监控数据刷新错误";
                lblStatusValue.ForeColor = Color.Red;
            }
        }

        /// <summary>
        /// 获取最后一次会话活动时间
        /// </summary>
        /// <param name="sessionService">会话服务实例</param>
        /// <returns>最后活动时间，如果无法获取则返回null</returns>
        private DateTime? GetLastSessionActivityTime(SessionService sessionService)
        {
            try
            {
                // 尝试通过反射访问内部会话集合或方法
                // 这是一个后备机制，在SessionService没有公开方法时使用
                var type = sessionService.GetType();

                // 尝试查找可能包含会话信息的属性
                var sessionsProperty = type.GetProperty("Sessions", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (sessionsProperty != null)
                {
                    var sessions = sessionsProperty.GetValue(sessionService);
                    if (sessions is System.Collections.IEnumerable)
                    {
                        // 查找最近活动的会话
                        DateTime? lastActivity = null;
                        foreach (var session in (System.Collections.IEnumerable)sessions)
                        {
                            try
                            {
                                // 尝试获取会话的最后活动时间
                                var lastActiveProperty = session.GetType().GetProperty("LastActivityTime",
                                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                                if (lastActiveProperty != null)
                                {
                                    var lastActiveValue = lastActiveProperty.GetValue(session);
                                    if (lastActiveValue is DateTime sessionLastActive)
                                    {
                                        if (!lastActivity.HasValue || sessionLastActive > lastActivity.Value)
                                        {
                                            lastActivity = sessionLastActive;
                                        }
                                    }
                                }
                            }
                            catch { }
                        }
                        return lastActivity;
                    }
                }

                // 尝试查找GetLastActivityTime之类的方法
                var getLastActivityMethod = type.GetMethod("GetLastActivityTime",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                    null, System.Type.EmptyTypes, null);

                if (getLastActivityMethod != null)
                {
                    var result = getLastActivityMethod.Invoke(sessionService, null);
                    if (result is DateTime)
                    {
                        return (DateTime)result;
                    }
                    else if (result is DateTime?)
                    {
                        return (DateTime?)result;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取最后会话活动时间时出错: {ex.Message}");
            }

            return null;
        }

        private void UpdateServerStatus()
        {
            try
            {
                if (_networkServer == null)
                {
                    _networkServer = Startup.GetFromFac<NetworkServer>();
                }

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

                if (serverInfo == null)
                {
                    // 如果serverInfo为null，显示错误状态
                    lblStatusValue.Text = "获取信息失败";
                    lblPortValue.Text = "N/A";
                    lblServerIpValue.Text = "N/A";
                    lblMaxConnectionsValue.Text = "N/A";
                    lblCurrentConnectionsValue.Text = "N/A";
                    lblTotalConnectionsValue.Text = "N/A";
                    lblPeakConnectionsValue.Text = "N/A";
                    lblLastActivityValue.Text = "N/A";
                    return;
                }

                // 更新基本服务器信息
                lblStatusValue.Text = !string.IsNullOrEmpty(serverInfo.Status) ? serverInfo.Status : "未知";

                // 为端口号添加验证
                if (serverInfo.Port > 0)
                {
                    lblPortValue.Text = serverInfo.Port.ToString();
                }
                else
                {
                    // 尝试从配置中获取端口号作为备用
                    int defaultPort = 8080; // 默认端口
                    try
                    {
                        var config = Program.ServiceProvider.GetRequiredService<IConfiguration>();
                        string portStr = config.GetSection("serverOptions:listeners:0:port").Value;
                        if (!string.IsNullOrEmpty(portStr) && int.TryParse(portStr, out int port))
                        {
                            defaultPort = port;
                        }
                    }
                    catch { }
                    lblPortValue.Text = defaultPort.ToString();
                }

                // 为IP地址添加验证
                lblServerIpValue.Text = !string.IsNullOrEmpty(serverInfo.ServerIp) && serverInfo.ServerIp != "0.0.0.0"
                    ? serverInfo.ServerIp
                    : "127.0.0.1";

                // 更新连接限制信息
                if (serverInfo.MaxConnections > 0)
                {
                    lblMaxConnectionsValue.Text = serverInfo.MaxConnections.ToString();
                }
                else
                {
                    // 尝试从SessionService获取最大连接数
                    if (_sessionService != null && _sessionService is SessionService sessionService)
                    {
                        lblMaxConnectionsValue.Text = sessionService.MaxSessionCount.ToString();
                    }
                    else
                    {
                        lblMaxConnectionsValue.Text = "1000"; // 默认值
                    }
                }

                // 连接统计信息获取逻辑 - 确保数据准确性
                int currentConnections = 0;
                int totalConnections = 0;
                int peakConnections = 0;
                DateTime lastActivityTime = DateTime.Now;

                // 首选方法：从SessionService获取统计信息
                if (_sessionService != null)
                {
                    try
                    {
                        var sessionStats = _sessionService.GetStatistics();
                        if (sessionStats != null)
                        {
                            // 验证并使用SessionService的数据
                            currentConnections = sessionStats.CurrentConnections;
                            totalConnections = sessionStats.TotalConnections;
                            peakConnections = sessionStats.PeakConnections;

                            // 验证最后活动时间 - 使用LastHeartbeatCheck替代不存在的LastUpdateTime
                            if (sessionStats.LastHeartbeatCheck > DateTime.MinValue &&
                                sessionStats.LastHeartbeatCheck < DateTime.Now.AddDays(1)) // 确保时间合理
                            {
                                lastActivityTime = sessionStats.LastHeartbeatCheck;
                            }
                            else
                            {
                                // 如果SessionService的时间无效，尝试获取最后一次会话的时间
                                try
                                {
                                    // 假设SessionService有方法获取最近的会话
                                    if (_sessionService is SessionService sessionService)
                                    {
                                        var lastSessionTime = GetLastSessionActivityTime(sessionService);
                                        if (lastSessionTime.HasValue)
                                        {
                                            lastActivityTime = lastSessionTime.Value;
                                        }
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogDebug($"从SessionService获取统计信息时出错: {ex.Message}");
                    }
                }

                // 如果SessionService未能提供有效数据，回退到NetworkServer
                if (currentConnections <= 0 && _networkServer != null)
                {
                    currentConnections = serverInfo.CurrentConnections;
                    totalConnections = serverInfo.TotalConnections;
                    peakConnections = serverInfo.PeakConnections;

                    // 验证NetworkServer提供的时间
                    if (serverInfo.LastActivityTime > DateTime.MinValue &&
                        serverInfo.LastActivityTime < DateTime.Now.AddDays(1))
                    {
                        lastActivityTime = serverInfo.LastActivityTime;
                    }
                }

                // 确保所有数据有效
                if (currentConnections < 0) currentConnections = 0;
                if (totalConnections < 0) totalConnections = 0;
                if (peakConnections < 0) peakConnections = 0;
                if (peakConnections < currentConnections) peakConnections = currentConnections;

                // 更新UI标签
                lblCurrentConnectionsValue.Text = currentConnections.ToString();
                lblTotalConnectionsValue.Text = totalConnections.ToString();
                lblPeakConnectionsValue.Text = peakConnections.ToString();
                lblLastActivityValue.Text = lastActivityTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch (Exception ex)
            {
                // 显示错误信息但保持UI功能
                _logger.LogError(ex, "更新服务器状态时出错");
                lblStatusValue.Text = "数据更新错误";
                lblStatusValue.ForeColor = Color.Red;
            }
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
                System.Diagnostics.Debug.WriteLine($"更新性能指标时出错: {ex.Message}");
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
                lblUptimeValue.Text = $"{uptime.Days}天 {uptime.Hours}小时 {uptime.Minutes}分钟 {uptime.Seconds}秒";
            }
            else
            {
                lblUptimeValue.Text = "未启动";
            }

            // 更新内存使用情况
            var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            var workingSetMemory = currentProcess.WorkingSet64 / (1024 * 1024); // 转换为MB
            var managedMemory = GC.GetTotalMemory(false) / (1024 * 1024); // 转换为MB

            lblMemoryUsageValue.Text = $"{workingSetMemory} MB (托管: {managedMemory} MB)";

            // 根据内存使用情况设置颜色
            if (workingSetMemory > 2048) // 超过2GB
            {
                lblMemoryUsageValue.ForeColor = Color.Red;
            }
            else if (workingSetMemory > 1024) // 超过1GB
            {
                lblMemoryUsageValue.ForeColor = Color.Orange;
            }
            else
            {
                lblMemoryUsageValue.ForeColor = Color.Green;
            }

            // 添加CPU使用率监控
            try
            {
                // 使用PerformanceCounter获取CPU使用率
                using (var cpuCounter = new System.Diagnostics.PerformanceCounter("Processor", "% Processor Time", "_Total"))
                {
                    // 首次采样返回0，需要再次采样
                    cpuCounter.NextValue();
                    System.Threading.Thread.Sleep(50);
                    float cpuUsage = cpuCounter.NextValue();

                    // 动态添加CPU使用率标签（如果不存在）
                    Label lblCpuUsage = Controls.Find("lblCpuUsage", true).FirstOrDefault() as Label;
                    Label lblCpuUsageValue = Controls.Find("lblCpuUsageValue", true).FirstOrDefault() as Label;

                    if (lblCpuUsage != null && lblCpuUsageValue != null)
                    {
                        lblCpuUsageValue.Text = $"{cpuUsage:F2}%";
                        
                        // 根据CPU使用率设置颜色
                        if (cpuUsage > 80)
                        {
                            lblCpuUsageValue.ForeColor = Color.Red;
                        }
                        else if (cpuUsage > 50)
                        {
                            lblCpuUsageValue.ForeColor = Color.Orange;
                        }
                        else
                        {
                            lblCpuUsageValue.ForeColor = Color.Green;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"获取CPU使用率失败: {ex.Message}");
            }

            // 添加磁盘I/O监控
            try
            {
                // 使用PerformanceCounter获取磁盘读写速度
                using (var diskReadCounter = new System.Diagnostics.PerformanceCounter("PhysicalDisk", "Disk Read Bytes/sec", "_Total"))
                using (var diskWriteCounter = new System.Diagnostics.PerformanceCounter("PhysicalDisk", "Disk Write Bytes/sec", "_Total"))
                {
                    diskReadCounter.NextValue();
                    diskWriteCounter.NextValue();
                    System.Threading.Thread.Sleep(50);
                    float diskRead = diskReadCounter.NextValue();
                    float diskWrite = diskWriteCounter.NextValue();

                    // 动态添加磁盘I/O标签（如果不存在）
                    Label lblDiskReadValue = Controls.Find("lblDiskReadValue", true).FirstOrDefault() as Label;
                    Label lblDiskWriteValue = Controls.Find("lblDiskWriteValue", true).FirstOrDefault() as Label;

                    if (lblDiskReadValue != null)
                    {
                        lblDiskReadValue.Text = $"{FormatBytes((long)diskRead)}/秒";
                    }

                    if (lblDiskWriteValue != null)
                    {
                        lblDiskWriteValue.Text = $"{FormatBytes((long)diskWrite)}/秒";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"获取磁盘I/O信息失败: {ex.Message}");
            }

            // 更新库存缓存统计信息 (Phase 2.4 优化)
            UpdateStockCacheStatistics();
        }

        /// <summary>
        /// 更新库存缓存统计信息
        /// </summary>
        private void UpdateStockCacheStatistics()
        {
            try
            {
                var stockCacheService = Startup.GetFromFac<IStockCacheService>();
                if (stockCacheService == null)
                {
                    _logger.LogDebug("无法获取 IStockCacheService 服务实例");
                    return;
                }

                var stats = stockCacheService.GetCacheStatistics();

                // 查找现有的缓存统计标签，如果没有则跳过（避免UI错误）
                Label lblCacheHitRatio = Controls.Find("lblCacheHitRatioValue", true).FirstOrDefault() as Label;
                Label lblCacheSize = Controls.Find("lblCacheSizeValue", true).FirstOrDefault() as Label;
                Label lblCacheHitCount = Controls.Find("lblCacheHitCountValue", true).FirstOrDefault() as Label;
                Label lblCacheMissCount = Controls.Find("lblCacheMissCountValue", true).FirstOrDefault() as Label;
                Label lblCacheMissRatio = Controls.Find("lblCacheMissRatioValue", true).FirstOrDefault() as Label;

                if (lblCacheHitRatio != null)
                {
                    double hitRatio = stats.HitRatio * 100;
                    lblCacheHitRatio.Text = $"{hitRatio:F2}%";
                    lblCacheHitRatio.ForeColor = hitRatio >= 90 ? Color.Green : hitRatio >= 80 ? Color.Orange : Color.Red;
                }

                if (lblCacheSize != null)
                {
                    lblCacheSize.Text = stats.CurrentCacheSize.ToString("N0");
                }

                if (lblCacheHitCount != null)
                {
                    lblCacheHitCount.Text = stats.CacheHits.ToString("N0");
                }

                if (lblCacheMissCount != null)
                {
                    lblCacheMissCount.Text = stats.CacheMisses.ToString("N0");
                }

                if (lblCacheMissRatio != null)
                {
                    double missRatio = (1 - stats.HitRatio) * 100;
                    lblCacheMissRatio.Text = $"{missRatio:F2}%";
                }

                _logger.LogDebug("更新缓存统计: 命中率={HitRatio:P2}, 缓存大小={Size}, 命中={Hits}, 未命中={Misses}",
                    stats.HitRatio, stats.CurrentCacheSize, stats.CacheHits, stats.CacheMisses);
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"更新缓存统计时出错: {ex.Message}");
            }
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
                System.Diagnostics.Debug.WriteLine($"更新系统健康状态时出错: {ex.Message}");
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
                System.Diagnostics.Debug.WriteLine($"更新实时监控数据时出错: {ex.Message}");
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
            ILogger<ServerMonitorControl> logger = Startup.GetFromFac<ILogger<ServerMonitorControl>>();
            logger.LogInformation("刷新数据1");
            logger.Info("刷新数据2");
        }

        private void btnResetStats_Click(object sender, EventArgs e)
        {
            try
            {
                // 重置诊断服务的统计信息
                _diagnosticsService.ResetAllStatistics();

                // 重置熔断器指标
                if (_commandDispatcher != null && _commandDispatcher.Metrics != null)
                {
                    _commandDispatcher.Metrics.ResetAllMetrics();
                }

                // 重置性能监控服务的统计信息
                _performanceMonitoringService.ResetStatistics();

                // 重置错误分析服务的统计信息
                _errorAnalysisService.ResetStatistics();

                // 重置会话统计信息
                if (_sessionService != null)
                {
                    _sessionService.ResetStatistics();
                }

                //// 重置缓存统计信息
                //EntityCacheHelper.ResetStatistics();

                // 刷新数据显示
                RefreshData();

            }
            catch (Exception ex)
            {
                // 记录错误并显示错误消息
                ILogger<ServerMonitorControl> logger = Startup.GetFromFac<ILogger<ServerMonitorControl>>();
                logger.LogError(ex, "重置统计信息时发生错误");
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
                // 使用CommandDispatcher的Metrics属性获取熔断器指标
                var circuitBreakerMetrics = _commandDispatcher?.Metrics;

                if (circuitBreakerMetrics == null)
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
                var globalMetrics = circuitBreakerMetrics.GlobalMetrics;

                // 直接从circuitBreakerMetrics获取状态
                string circuitState = circuitBreakerMetrics.CurrentState.ToString();
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

                // 显示熔断器开关次数的详情
                if (globalMetrics.StateChanges > 0)
                {
                    // 可以在这里添加更多详细信息的显示，如开关历史记录等
                }

                // 更新新增的熔断器指标
                // 熔断器打开次数
                cbLblCircuitOpensValue.Text = globalMetrics.CircuitOpens.ToString();

                // 熔断器关闭次数
                cbLblCircuitClosesValue.Text = globalMetrics.CircuitCloses.ToString();

                // 熔断器半开次数
                cbLblCircuitHalfOpensValue.Text = globalMetrics.CircuitHalfOpens.ToString();

                // 当前活跃执行数
                cbLblActiveExecutionsValue.Text = globalMetrics.ActiveExecutions.ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"更新熔断器监控指标时出错: {ex.Message}");
                // 在UI上显示错误状态
                cbLblStatusValue.Text = "数据获取失败";
                cbLblStatusValue.ForeColor = Color.Red;
            }
        }

        /// <summary>
        /// 更新心跳性能监控数据
        /// </summary>
        private void UpdateHeartbeatPerformanceData()
        {
            try
            {
                // 更新心跳批量处理器统计信息
                if (_heartbeatBatchProcessor != null)
                {
                    var queueStats = _heartbeatBatchProcessor.GetQueueStats();
                    
                    // 查找并更新心跳队列长度标签
                    var lblHeartbeatQueueLength = Controls.Find("lblHeartbeatQueueLengthValue", true).FirstOrDefault() as Label;
                    if (lblHeartbeatQueueLength != null)
                    {
                        lblHeartbeatQueueLength.Text = queueStats.QueueLength.ToString();
                        // 根据队列长度设置颜色警告
                        if (queueStats.QueueLength > 20)
                        {
                            lblHeartbeatQueueLength.ForeColor = Color.Red;
                        }
                        else if (queueStats.QueueLength > 10)
                        {
                            lblHeartbeatQueueLength.ForeColor = Color.Orange;
                        }
                        else
                        {
                            lblHeartbeatQueueLength.ForeColor = Color.Green;
                        }
                    }
                }

                // 更新心跳性能监控统计信息
                if (_heartbeatPerformanceMonitor != null)
                {
                    var performanceStats = _heartbeatPerformanceMonitor.GetPerformanceStats();
                    
                    // 更新处理统计
                    var lblHeartbeatProcessed = Controls.Find("lblHeartbeatProcessedValue", true).FirstOrDefault() as Label;
                    if (lblHeartbeatProcessed != null)
                    {
                        lblHeartbeatProcessed.Text = performanceStats.TotalProcessed.ToString("N0");
                    }
                    
                    // 更新错误统计
                    var lblHeartbeatErrors = Controls.Find("lblHeartbeatErrorsValue", true).FirstOrDefault() as Label;
                    if (lblHeartbeatErrors != null)
                    {
                        lblHeartbeatErrors.Text = performanceStats.TotalErrors.ToString("N0");
                        lblHeartbeatErrors.ForeColor = performanceStats.ErrorRate > 5 ? Color.Red : Color.Green;
                    }
                    
                    // 更新错误率
                    var lblHeartbeatErrorRate = Controls.Find("lblHeartbeatErrorRateValue", true).FirstOrDefault() as Label;
                    if (lblHeartbeatErrorRate != null)
                    {
                        lblHeartbeatErrorRate.Text = $"{performanceStats.ErrorRate:F2}%";
                        lblHeartbeatErrorRate.ForeColor = performanceStats.ErrorRate > 5 ? Color.Red : 
                                                         performanceStats.ErrorRate > 1 ? Color.Orange : Color.Green;
                    }
                    
                    // 更新平均处理时间
                    var lblAvgHeartbeatTime = Controls.Find("lblAvgHeartbeatTimeValue", true).FirstOrDefault() as Label;
                    if (lblAvgHeartbeatTime != null)
                    {
                        lblAvgHeartbeatTime.Text = $"{performanceStats.AverageProcessingTime:F2}ms";
                        lblAvgHeartbeatTime.ForeColor = performanceStats.AverageProcessingTime > 100 ? Color.Red : 
                                                       performanceStats.AverageProcessingTime > 50 ? Color.Orange : Color.Green;
                    }
                    
                    // 更新优化策略状态
                    var lblBatchProcessing = Controls.Find("lblBatchProcessingValue", true).FirstOrDefault() as Label;
                    if (lblBatchProcessing != null)
                    {
                        lblBatchProcessing.Text = performanceStats.UseBatchProcessing ? "启用" : "禁用";
                        lblBatchProcessing.ForeColor = performanceStats.UseBatchProcessing ? Color.Green : Color.Gray;
                    }
                    
                    var lblMergeThreshold = Controls.Find("lblMergeThresholdValue", true).FirstOrDefault() as Label;
                    if (lblMergeThreshold != null)
                    {
                        lblMergeThreshold.Text = $"{performanceStats.MergeThresholdSeconds}秒";
                    }
                    
                    var lblBatchSize = Controls.Find("lblBatchSizeValue", true).FirstOrDefault() as Label;
                    if (lblBatchSize != null)
                    {
                        lblBatchSize.Text = performanceStats.BatchSize.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"更新心跳性能监控数据时出错: {ex.Message}");
            }
        }

    }
}