/*****************************************************************************************
 * 文件名称：SessionManagementForm.cs
 * 创建人员：RUINOR ERP系统
 * 创建时间：2024年
 * 文件描述：增强版会话管理详情窗体，提供专业的客户端监控、控制和管理功能
 * 版权所有：RUINOR ERP系统
 *****************************************************************************************/

using RUINORERP.Server.Network.Models;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Commands;
 
using RUINORERP.Server.Network.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Diagnostics;
using RUINORERP.Server.Network.Interfaces.Services;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Models.Message;
using RUINORERP.Global.EnumExt;

namespace RUINORERP.Server.Controls
{
    public partial class SessionManagementForm : Form
    {
        private readonly SessionInfo _session;
        private readonly ISessionService _sessionService;
        private readonly Timer _updateTimer;
        private readonly Timer _performanceTimer;
        private PerformanceCounter _cpuCounter;
        private PerformanceCounter _memoryCounter;
        private PerformanceCounter _diskIOCounter;
        private DateTime _formOpenTime;
        private readonly ILogger<SessionManagementForm> _logger;
        private int _baseUpdateInterval = 2000; // 基础更新间隔，单位：毫秒
        private int _basePerformanceInterval = 1000; // 基础性能更新间隔，单位：毫秒
        public SessionManagementForm(SessionInfo session, ISessionService sessionService)
        {
            InitializeComponent();
            // 从依赖注入容器获取服务
            _logger = Startup.GetFromFac<ILogger<SessionManagementForm>>();
            _session = session;
            _sessionService = sessionService;
            _formOpenTime = DateTime.Now;

            Text = $"会话管理 - {session.UserName ?? "未登录用户"} ({session.SessionID})";

            // 设置定时器
            _updateTimer = new Timer { Interval = 2000 }; // 2秒更新一次基础信息
            _updateTimer.Tick += UpdateTimer_Tick;

            _performanceTimer = new Timer { Interval = 1000 }; // 1秒更新一次性能数据
            _performanceTimer.Tick += PerformanceTimer_Tick;

            // 初始化界面
            InitializeForm();
            LoadSessionData();

            // 启动定时器
            _updateTimer.Start();
            _performanceTimer.Start();
        }

        /// <summary>
        /// 初始化窗体设置
        /// </summary>
        private void InitializeForm()
        {
            // 设置图标和状态
            Icon = SystemIcons.Information;

            // 初始化性能计数器
            try
            {
                _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                _memoryCounter = new PerformanceCounter("Memory", "Available MBytes");
                _diskIOCounter = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");
            }
            catch (Exception ex)
            {
                LogError("初始化性能计数器失败", ex);
            }
        }

        /// <summary>
        /// 加载会话数据
        /// </summary>
        private void LoadSessionData()
        {
            UpdateBasicInfo();
            UpdateSystemInfo();
            UpdatePerformanceInfo();
            UpdateNetworkInfo();
            UpdateCacheInfo();
        }

        /// <summary>
        /// 更新基础信息
        /// </summary>
        private void UpdateBasicInfo()
        {
            // 使用try-catch保护关键操作，确保UI不会崩溃
            try
            {
                // 检查会话是否仍然有效
                if (_session == null)
                {
                    Close();
                    return;
                }

                // 会话基本信息
                lblSessionID.Text = _session.SessionID;
                lblUserName.Text = _session.UserName ?? "未登录用户";
                lblStatus.Text = _session.Status.ToString();
                lblClientIP.Text = _session.ClientIp;
                lblClientPort.Text = _session.ClientPort.ToString();
                lblConnectedTime.Text = _session.ConnectedTime.ToString("yyyy-MM-dd HH:mm:ss");
                lblClientVersion.Text = _session.UserInfo?.客户端版本 ?? "未知版本";

                // 计算连接时长
                TimeSpan duration = DateTime.Now - _session.ConnectedTime;
                lblConnectionDuration.Text = $"{duration.Days}天 {duration.Hours}小时 {duration.Minutes}分钟";

                // 更新状态指示器
                UpdateStatusIndicator(_session.Status);
            }
            catch (Exception ex)
            {
                LogError("更新基础信息失败", ex);
            }
        }

        /// <summary>
        /// 更新系统信息
        /// </summary>
        private void UpdateSystemInfo()
        {
            try
            {
                // 检查会话是否仍然有效
                if (_session == null)
                {
                    Close();
                    return;
                }

                if (_session.ClientSystemInfo != null)
                {
                    var clientSystemInfo = _session.ClientSystemInfo;
                    var osInfo = clientSystemInfo.OperatingSystem;
                    var hwInfo = clientSystemInfo.Hardware;

                    // 操作系统信息
                    lblOSName.Text = $"{osInfo?.Platform} {osInfo?.Version}";
                    lblMachineName.Text = osInfo?.MachineName ?? "未知";
                    lblArchitecture.Text = osInfo?.Architecture ?? "未知";
                    lblIs64Bit.Text = osInfo?.Is64BitOperatingSystem == true ? "是" : "否";

                    // 硬件信息
                    if (hwInfo?.ProcessorInfo != null)
                    {
                        var cpuInfo = hwInfo.ProcessorInfo;
                        lblCPUName.Text = cpuInfo.Name ?? "未知";
                        lblCPUCores.Text = cpuInfo.NumberOfCores.ToString();
                        lblCPULogicalProcessors.Text = cpuInfo.NumberOfLogicalProcessors.ToString();
                        lblCPUMaxSpeed.Text = $"{cpuInfo.MaxClockSpeed} MHz";
                    }
                    else
                    {
                        lblCPUName.Text = "未采集";
                        lblCPUCores.Text = "未采集";
                        lblCPULogicalProcessors.Text = "未采集";
                        lblCPUMaxSpeed.Text = "未采集";
                    }

                    if (hwInfo?.MemoryInfo != null)
                    {
                        var memInfo = hwInfo.MemoryInfo;
                        lblTotalMemory.Text = $"{(memInfo.TotalPhysicalMemory / (1024.0 * 1024.0 * 1024.0)):F1} GB";
                        lblAvailableMemory.Text = $"{(memInfo.AvailablePhysicalMemory / (1024.0 * 1024.0)):F0} MB";
                    }
                    else
                    {
                        lblTotalMemory.Text = "未采集";
                        lblAvailableMemory.Text = "未采集";
                    }

                    // MAC地址和网络信息
                    lblMacAddress.Text = hwInfo?.MacAddress ?? "未知";
                }
                else
                {
                    // 清空显示
                    lblOSName.Text = "未采集";
                    lblMachineName.Text = "未采集";
                    lblArchitecture.Text = "未采集";
                    lblIs64Bit.Text = "未采集";
                    lblCPUName.Text = "未采集";
                    lblCPUCores.Text = "未采集";
                    lblCPULogicalProcessors.Text = "未采集";
                    lblCPUMaxSpeed.Text = "未采集";
                    lblTotalMemory.Text = "未采集";
                    lblAvailableMemory.Text = "未采集";
                    lblMacAddress.Text = "未采集";
                }
            }
            catch (Exception ex)
            {
                LogError("更新系统信息失败", ex);
            }
        }

        /// <summary>
        /// 更新性能信息
        /// </summary>
        private void UpdatePerformanceInfo()
        {
            try
            {
                // 检查会话是否仍然有效
                if (_session == null || _session.Status == SessionStatus.Disconnected)
                {
                    Close();
                    return;
                }

                // 获取性能统计信息，使用安全调用
                var stats = _session.GetPerformanceStats();

                // 网络统计
                lblSentPackets.Text = stats.SentPackets.ToString("N0");
                lblReceivedPackets.Text = stats.ReceivedPackets.ToString("N0");
                lblTotalBytesSent.Text = FormatBytes(stats.TotalBytesSent);
                lblTotalBytesReceived.Text = FormatBytes(stats.TotalBytesReceived);

                // 心跳信息
                lblHeartbeatCount.Text = _session.HeartbeatCount.ToString();
                lblLastHeartbeat.Text = _session.LastActivityTime.ToString("yyyy-MM-dd HH:mm:ss");

                // 计算实时吞吐量
                TimeSpan duration = DateTime.Now - _session.ConnectedTime;
                if (duration.TotalSeconds > 0)
                {
                    long totalBytes = stats.TotalBytesSent + stats.TotalBytesReceived;
                    double bytesPerSecond = totalBytes / duration.TotalSeconds;
                    lblThroughput.Text = $"{FormatBytes((long)bytesPerSecond)}/秒";
                }
                else
                {
                    lblThroughput.Text = "0 B/秒";
                }

                // 更新活动状态
                TimeSpan idleTime = DateTime.Now - _session.LastActivityTime;
                lblIdleTime.Text = idleTime.TotalMinutes < 1 ? "刚刚" : $"{idleTime.TotalMinutes:F1} 分钟前";
            }
            catch (Exception ex)
            {
                LogError("更新性能信息失败", ex);
            }
        }

        /// <summary>
        /// 更新网络信息
        /// </summary>
        private void UpdateNetworkInfo()
        {
            try
            {
                // 检查会话是否仍然有效
                if (_session == null || _session.Status == SessionStatus.Disconnected)
                {
                    Close();
                    return;
                }

                if (_session.ClientSystemInfo?.Network != null)
                {
                    var networkInfo = _session.ClientSystemInfo.Network;

                    // 基础网络信息
                    lblLocalIP.Text = networkInfo.LocalIP ?? "未知";
                    lblLocalPort.Text = networkInfo.LocalPort > 0 ? networkInfo.LocalPort.ToString() : "未知";
                    lblRemoteIP.Text = networkInfo.RemoteIP ?? "未知";
                    lblRemotePort.Text = networkInfo.RemotePort > 0 ? networkInfo.RemotePort.ToString() : "未知";

                    // 网络质量指标
                    if (networkInfo.NetworkQuality != null)
                    {
                        double latency = networkInfo.NetworkQuality.Latency;
                        double packetLoss = networkInfo.NetworkQuality.PacketLossRate;
                        
                        lblLatency.Text = $"{latency:F0} ms";
                        lblPacketLoss.Text = $"{packetLoss:P2}";
                        lblBandwidth.Text = FormatBytes(networkInfo.NetworkQuality.AvailableBandwidth);

                        // 更新网络状态指示器
                        UpdateNetworkStatus(latency, packetLoss);
                    }
                    else
                    {
                        lblLatency.Text = "未检测";
                        lblPacketLoss.Text = "未检测";
                        lblBandwidth.Text = "未检测";
                        UpdateNetworkStatus(-1, 0);
                    }
                }
                else
                {
                    // 设置默认值，避免显示空白
                    lblLocalIP.Text = "未采集";
                    lblLocalPort.Text = "未采集";
                    lblRemoteIP.Text = "未采集";
                    lblRemotePort.Text = "未采集";
                    lblLatency.Text = "未采集";
                    lblPacketLoss.Text = "未采集";
                    lblBandwidth.Text = "未采集";
                    UpdateNetworkStatus(-1, 0);
                }
            }
            catch (Exception ex)
            {
                LogError("更新网络信息失败", ex);
                // 在异常情况下，确保UI显示合理的默认值
                UpdateNetworkStatus(-1, 0);
            }
        }

        /// <summary>
        /// 更新缓存信息
        /// </summary>
        private void UpdateCacheInfo()
        {
            try
            {
                // 检查会话是否仍然有效
                if (_session == null || _session.Status != SessionStatus.Connected)
                {
                    return;
                }

                // 实际获取缓存统计信息
                var cacheStats = GetCacheStatistics();
                lblCacheEntries.Text = cacheStats.Entries.ToString("N0");
                lblCacheSize.Text = FormatBytes(cacheStats.SizeBytes);
                lblCacheHitRate.Text = $"{cacheStats.HitRate:P1}";

                // 更新缓存状态
                var cacheStatus = GetCacheStatus(cacheStats);
                lblCacheStatus.Text = cacheStatus;
                lblCacheStatus.ForeColor = cacheStatus == "正常" ? Color.Green : cacheStatus == "警告" ? Color.Orange : Color.Red;
            }
            catch (Exception ex)
            {
                LogError("更新缓存信息失败", ex);
                // 在异常情况下，设置合理的默认值
                lblCacheEntries.Text = "N/A";
                lblCacheSize.Text = "N/A";
                lblCacheHitRate.Text = "N/A";
                lblCacheStatus.Text = "未知";
                lblCacheStatus.ForeColor = Color.Gray;
            }
        }

        /// <summary>
        /// 缓存统计信息类
        /// </summary>
        private class CacheStatistics
        {
            public long Entries { get; set; }
            public long SizeBytes { get; set; }
            public double HitRate { get; set; }
        }

        /// <summary>
        /// 获取缓存统计信息
        /// </summary>
        private CacheStatistics GetCacheStatistics()
        {
            try
            {
                // 这里应该实现实际的缓存统计获取逻辑
                // 示例：从CacheManager获取缓存统计
                // 目前返回模拟数据，实际应用中应替换为真实实现
                return new CacheStatistics
                {
                    Entries = 156,
                    SizeBytes = 2401280, // 约2.3 MB
                    HitRate = 0.875
                };
            }
            catch (Exception ex)
            {
                LogError("获取缓存统计信息失败", ex);
                // 返回默认值
                return new CacheStatistics
                {
                    Entries = 0,
                    SizeBytes = 0,
                    HitRate = 0
                };
            }
        }

        /// <summary>
        /// 获取缓存状态
        /// </summary>
        private string GetCacheStatus(CacheStatistics cacheStats)
        {
            try
            {
                // 根据缓存统计信息判断缓存状态
                if (cacheStats.HitRate < 0.5)
                {
                    return "警告";
                }
                if (cacheStats.SizeBytes > 100 * 1024 * 1024) // 超过100MB
                {
                    return "警告";
                }
                return "正常";
            }
            catch (Exception ex)
            {
                LogError("获取缓存状态失败", ex);
                return "未知";
            }
        }

        /// <summary>
        /// 更新网络状态指示器
        /// </summary>
        private void UpdateNetworkStatus(double latency, double packetLoss)
        {
            if (latency < 0)
            {
                picNetworkStatus.BackColor = Color.Gray;
                lblNetworkStatus.Text = "未连接";
                return;
            }

            if (latency < 50 && packetLoss < 0.01)
            {
                picNetworkStatus.BackColor = Color.Green;
                lblNetworkStatus.Text = "优秀";
            }
            else if (latency < 100 && packetLoss < 0.05)
            {
                picNetworkStatus.BackColor = Color.Yellow;
                lblNetworkStatus.Text = "良好";
            }
            else if (latency < 200 && packetLoss < 0.1)
            {
                picNetworkStatus.BackColor = Color.Orange;
                lblNetworkStatus.Text = "一般";
            }
            else
            {
                picNetworkStatus.BackColor = Color.Red;
                lblNetworkStatus.Text = "较差";
            }
        }

        /// <summary>
        /// 更新状态指示器
        /// </summary>
        private void UpdateStatusIndicator(SessionStatus status)
        {
            switch (status)
            {
                case SessionStatus.Connected:
                    picSessionStatus.BackColor = Color.Green;
                    break;
                case SessionStatus.Disconnected:
                    picSessionStatus.BackColor = Color.Red;
                    break;
                case SessionStatus.Authenticated:
                    picSessionStatus.BackColor = Color.Blue;
                    break;
                default:
                    picSessionStatus.BackColor = Color.Gray;
                    break;
            }
        }

        /// <summary>
        /// 格式化字节数
        /// </summary>
        private string FormatBytes(long bytes)
        {
            if (bytes < 1024)
                return $"{bytes} B";
            else if (bytes < 1024 * 1024)
                return $"{bytes / 1024.0:F2} KB";
            else if (bytes < 1024 * 1024 * 1024)
                return $"{bytes / (1024.0 * 1024.0):F2} MB";
            else
                return $"{bytes / (1024.0 * 1024.0 * 1024.0):F2} GB";
        }

        /// <summary>
        /// 定时器事件 - 更新基础信息
        /// </summary>
        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdateBasicInfo();
            UpdateSystemInfo();
            UpdateCacheInfo();
        }

        /// <summary>
        /// 定时器事件 - 更新性能数据
        /// </summary>
        private void PerformanceTimer_Tick(object sender, EventArgs e)
        {
            UpdatePerformanceInfo();
            UpdateNetworkInfo();
            UpdatePerformanceCharts();
            AdjustRefreshIntervals();
        }

        /// <summary>
        /// 根据系统负载调整刷新间隔
        /// </summary>
        private void AdjustRefreshIntervals()
        {
            try
            {
                float cpuUsage = _cpuCounter?.NextValue() ?? 0;
                float diskUsage = _diskIOCounter?.NextValue() ?? 0;
                
                // 根据CPU和磁盘使用率调整刷新间隔
                // 负载越高，刷新间隔越长，减少系统压力
                int cpuBasedInterval = cpuUsage < 30 ? _baseUpdateInterval : cpuUsage < 70 ? _baseUpdateInterval * 2 : _baseUpdateInterval * 4;
                int diskBasedInterval = diskUsage < 30 ? _basePerformanceInterval : diskUsage < 70 ? _basePerformanceInterval * 2 : _basePerformanceInterval * 4;
                
                // 取最大值作为新的间隔，确保系统负载高时减少更新频率
                int newUpdateInterval = Math.Max(cpuBasedInterval, diskBasedInterval);
                int newPerformanceInterval = Math.Max(cpuBasedInterval / 2, diskBasedInterval / 2);
                
                // 应用新的间隔，避免频繁调整
                if (Math.Abs(_updateTimer.Interval - newUpdateInterval) > 500)
                {
                    _updateTimer.Interval = newUpdateInterval;
                }
                
                if (Math.Abs(_performanceTimer.Interval - newPerformanceInterval) > 250)
                {
                    _performanceTimer.Interval = newPerformanceInterval;
                }
                
                // 更新状态栏显示当前刷新间隔 - 移除对不存在控件的引用
            }
            catch (Exception ex)
            {
                LogError("调整刷新间隔失败", ex);
            }
        }

        /// <summary>
        /// 更新性能图表
        /// </summary>
        private void UpdatePerformanceCharts()
        {
            try
            {
                // 示例：获取并显示系统CPU和内存使用率
                // 实际应用中应替换为客户端实际性能数据
                float cpuUsage = _cpuCounter?.NextValue() ?? 0;
                float availableMemory = _memoryCounter?.NextValue() ?? 0;
                
                // 客户端性能数据已在UpdatePerformanceInfo方法中更新
                // 这里可以添加图表控件的更新逻辑
                // 例如：
                // chartCPU.Series["CPU"].Points.AddY(cpuUsage);
                // chartMemory.Series["Memory"].Points.AddY(availableMemory);
            }
            catch (Exception ex)
            {
                LogError("更新性能图表失败", ex);
            }
        }

        /// <summary>
        /// 发送消息按钮点击事件
        /// </summary>
        private async void btnSendMessage_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMessage.Text))
            {
                MessageBox.Show("请输入要发送的消息内容", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var messageData = new MessageData
                {
                    MessageType = MessageType.Business,
                    Content = txtMessage.Text,
                    SendTime = DateTime.Now
                };

                var request = new MessageRequest(MessageType.Business, messageData);
                _logger.LogInformation("正在向会话 {SessionID} 发送消息", _session.SessionID);
                bool success = await _sessionService.SendCommandAsync(
                    _session.SessionID,
                    MessageCommands.SendMessageToUser,
                    request);

                if (success)
                {
                    // 添加到消息历史
                    AddMessageToHistory($"管理员: {txtMessage.Text}", Color.Blue);
                    txtMessage.Clear();
                    _logger.LogInformation("消息发送成功，会话 {SessionID}", _session.SessionID);
                }
                else
                {
                    _logger.LogWarning("消息发送失败，会话 {SessionID}", _session.SessionID);
                    MessageBox.Show("发送消息失败，请检查连接状态", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                LogError("发送消息失败", ex);
                MessageBox.Show($"发送消息时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 强制同步缓存按钮点击事件
        /// </summary>
        private async void btnForceSync_Click(object sender, EventArgs e)
        {
            try
            {
                _logger.LogInformation("正在向会话 {SessionID} 发送强制同步缓存命令", _session.SessionID);
                var messageData = new MessageData
                {
                    MessageType = MessageType.System,
                    Content = "FORCE_CACHE_SYNC",
                    SendTime = DateTime.Now
                };
                messageData.ExtendedData["Scope"] = "ALL";

                var request = new MessageRequest(MessageType.System, messageData);
                bool success = await _sessionService.SendCommandAsync(
                    _session.SessionID,
                    MessageCommands.SendMessageToUser,
                    request);

                if (success)
                {
                    _logger.LogInformation("强制同步缓存命令发送成功，会话 {SessionID}", _session.SessionID);
                    MessageBox.Show("强制同步命令已发送", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AddMessageToHistory("系统: 强制同步缓存命令已发送", Color.Green);
                }
                else
                {
                    _logger.LogWarning("强制同步缓存命令发送失败，会话 {SessionID}", _session.SessionID);
                    MessageBox.Show("发送同步命令失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                LogError("发送同步命令失败", ex);
                MessageBox.Show($"发送同步命令时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 清理缓存按钮点击事件
        /// </summary>
        private async void btnClearCache_Click(object sender, EventArgs e)
        {
            try
            {
                _logger.LogInformation("正在向会话 {SessionID} 发送清理缓存命令", _session.SessionID);
                var messageData = new MessageData
                {
                    MessageType = MessageType.System,
                    Content = "CacheClear",
                    SendTime = DateTime.Now
                };
                messageData.ExtendedData["Scope"] = "All";

                var request = new MessageRequest(MessageType.System, messageData);

                await _sessionService.SendCommandAsync(
                    _session.SessionID,
                    MessageCommands.SendMessageToUser,
                    request);

                _logger.LogInformation("清理缓存命令发送成功，会话 {SessionID}", _session.SessionID);
                AddMessageToHistory("[清除缓存] 已发送缓存清除命令", Color.YellowGreen);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清除缓存失败，会话 {SessionID}", _session.SessionID);
                MessageBox.Show($"清除缓存失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 重启客户端按钮点击事件
        /// </summary>
        private async void btnRestartClient_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "确定要重启客户端吗？这将断开当前连接并重新启动客户端应用程序。",
                "确认",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    _logger.LogInformation("正在向会话 {SessionID} 发送重启客户端命令", _session.SessionID);
                    var messageData = new MessageData
                    {
                        MessageType = MessageType.System,
                        Content = "RESTART_CLIENT",
                        SendTime = DateTime.Now
                    };
                    messageData.ExtendedData["Reason"] = "管理员命令";
                    messageData.ExtendedData["Delay"] = 5;

                    var request = new MessageRequest(MessageType.System, messageData);
                    bool success = await _sessionService.SendCommandAsync(
                        _session.SessionID,
                        MessageCommands.SendMessageToUser,
                        request);

                    if (success)
                    {
                        _logger.LogInformation("重启客户端命令发送成功，会话 {SessionID}", _session.SessionID);
                        MessageBox.Show("重启命令已发送", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        AddMessageToHistory("系统: 客户端重启命令已发送", Color.Orange);
                    }
                    else
                    {
                        _logger.LogWarning("重启客户端命令发送失败，会话 {SessionID}", _session.SessionID);
                        MessageBox.Show("发送重启命令失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    LogError("发送重启命令失败", ex);
                    MessageBox.Show($"发送重启命令时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// 关机按钮点击事件
        /// </summary>
        private async void btnShutdown_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "确定要关闭客户端吗？这将断开当前连接并退出客户端应用程序。",
                "确认",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    _logger.LogInformation("正在向会话 {SessionID} 发送关闭客户端命令", _session.SessionID);
                    var messageData = new MessageData
                    {
                        MessageType = MessageType.System,
                        Content = "SHUTDOWN_CLIENT",
                        SendTime = DateTime.Now
                    };
                    messageData.ExtendedData["Reason"] = "管理员命令";
                    messageData.ExtendedData["Timeout"] = 30;

                    var request = new MessageRequest(MessageType.System, messageData);

                    await _sessionService.SendCommandAsync(
                        _session.SessionID,
                        MessageCommands.SendMessageToUser,
                        request);

                    _logger.LogInformation("关闭客户端命令发送成功，会话 {SessionID}", _session.SessionID);
                    AddMessageToHistory("[关机] 已发送客户端关机命令", Color.Yellow);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "关闭客户端失败，会话 {SessionID}", _session.SessionID);
                    MessageBox.Show($"关机失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// 添加消息到历史记录
        /// </summary>
        private void AddMessageToHistory(string message, Color color)
        {
            if (txtMessageHistory.InvokeRequired)
            {
                txtMessageHistory.Invoke(new Action<string, Color>(AddMessageToHistory), message, color);
                return;
            }

            var timestamp = DateTime.Now.ToString("HH:mm:ss");
            txtMessageHistory.SelectionColor = Color.Gray;
            txtMessageHistory.AppendText($"[{timestamp}] ");
            txtMessageHistory.SelectionColor = color;
            txtMessageHistory.AppendText(message + Environment.NewLine);
            txtMessageHistory.ScrollToCaret();
        }

        /// <summary>
        /// 日志错误
        /// </summary>
        private void LogError(string message, Exception ex)
        {
            _logger.LogError(ex, message);
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        private void SessionManagementForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _updateTimer?.Stop();
            _updateTimer?.Dispose();

            _performanceTimer?.Stop();
            _performanceTimer?.Dispose();

            _cpuCounter?.Dispose();
            _memoryCounter?.Dispose();
        }

        /// <summary>
        /// 导出会话信息按钮点击事件
        /// </summary>
        private void btnExportSessionInfo_Click(object sender, EventArgs e)
        {
            try
            {
                using (var saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "JSON文件|*.json|文本文件|*.txt";
                    saveDialog.FileName = $"SessionInfo_{_session.SessionID}_{DateTime.Now:yyyyMMdd_HHmmss}.json";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        var sessionData = new
                        {
                            SessionID = _session.SessionID,
                            UserName = _session.UserName,
                            ConnectedTime = _session.ConnectedTime,
                            ClientIP = _session.ClientIp,
                            ClientVersion = _session.UserInfo.客户端版本,
                            Status = _session.Status.ToString(),
                            PerformanceStats = _session.GetPerformanceStats(),
                            ClientSystemInfo = _session.ClientSystemInfo,
                            ExportTime = DateTime.Now
                        };

                        string json = System.Text.Json.JsonSerializer.Serialize(sessionData, new System.Text.Json.JsonSerializerOptions
                        {
                            WriteIndented = true
                        });

                        System.IO.File.WriteAllText(saveDialog.FileName, json);
                        MessageBox.Show("会话信息导出成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("导出会话信息失败", ex);
                MessageBox.Show($"导出会话信息时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}