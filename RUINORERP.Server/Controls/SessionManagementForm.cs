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
using RUINORERP.PacketSpec.Models.Messaging;
using RUINORERP.Model.TransModel;
using RUINORERP.Server.Network.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using MessageRequest = RUINORERP.PacketSpec.Models.Messaging.MessageRequest;
using System.Diagnostics;
using RUINORERP.Server.Network.Interfaces.Services;
using Microsoft.Extensions.Logging;

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
        private DateTime _formOpenTime;
        private readonly ILogger<SessionManagementForm> _logger;
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
            try
            {
                // 会话基本信息
                lblSessionID.Text = _session.SessionID;
                lblUserName.Text = _session.UserName ?? "未登录用户";
                lblStatus.Text = _session.Status.ToString();
                lblClientIP.Text = _session.ClientIp;
                lblClientPort.Text = _session.ClientPort.ToString();
                lblConnectedTime.Text = _session.ConnectedTime.ToString("yyyy-MM-dd HH:mm:ss");
                lblClientVersion.Text = _session.UserInfo.客户端版本 ?? "未知版本";

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
                if (_session.ClientSystemInfo != null)
                {
                    var osInfo = _session.ClientSystemInfo.OperatingSystem;
                    var hwInfo = _session.ClientSystemInfo.Hardware;

                    // 操作系统信息
                    lblOSName.Text = $"{osInfo.Platform} {osInfo.Version}";
                    lblMachineName.Text = osInfo.MachineName;
                    lblArchitecture.Text = osInfo.Architecture;
                    lblIs64Bit.Text = osInfo.Is64BitOperatingSystem ? "是" : "否";

                    // 硬件信息
                    if (hwInfo.ProcessorInfo != null)
                    {
                        lblCPUName.Text = hwInfo.ProcessorInfo.Name;
                        lblCPUCores.Text = hwInfo.ProcessorInfo.NumberOfCores.ToString();
                        lblCPULogicalProcessors.Text = hwInfo.ProcessorInfo.NumberOfLogicalProcessors.ToString();
                        lblCPUMaxSpeed.Text = $"{hwInfo.ProcessorInfo.MaxClockSpeed} MHz";
                    }

                    if (hwInfo.MemoryInfo != null)
                    {
                        lblTotalMemory.Text = $"{hwInfo.MemoryInfo.TotalPhysicalMemory / (1024 * 1024 * 1024)} GB";
                        lblAvailableMemory.Text = $"{hwInfo.MemoryInfo.AvailablePhysicalMemory / (1024 * 1024)} MB";
                    }

                    // MAC地址和网络信息
                    lblMacAddress.Text = hwInfo.MacAddress ?? "未知";
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
                        lblLatency.Text = $"{networkInfo.NetworkQuality.Latency} ms";
                        lblPacketLoss.Text = $"{networkInfo.NetworkQuality.PacketLossRate:P2}";
                        lblBandwidth.Text = FormatBytes(networkInfo.NetworkQuality.AvailableBandwidth);

                        // 更新网络状态指示器
                        UpdateNetworkStatus(networkInfo.NetworkQuality.Latency, networkInfo.NetworkQuality.PacketLossRate);
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
            }
        }

        /// <summary>
        /// 更新缓存信息
        /// </summary>
        private void UpdateCacheInfo()
        {
            try
            {
                // 这里可以集成实际的缓存统计
                // 目前显示模拟数据
                lblCacheEntries.Text = "156";
                lblCacheSize.Text = "2.3 MB";
                lblCacheHitRate.Text = "87.5%";

                // 更新缓存状态
                var cacheStatus = GetCacheStatus();
                lblCacheStatus.Text = cacheStatus;
                lblCacheStatus.ForeColor = cacheStatus == "正常" ? Color.Green : Color.Red;
            }
            catch (Exception ex)
            {
                LogError("更新缓存信息失败", ex);
            }
        }

        /// <summary>
        /// 获取缓存状态
        /// </summary>
        private string GetCacheStatus()
        {
            // 这里应该实现实际的缓存状态检查逻辑
            // 返回 "正常", "警告", "错误" 等状态
            return "正常";
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
        }

        /// <summary>
        /// 更新性能图表
        /// </summary>
        private void UpdatePerformanceCharts()
        {
            // 这里可以添加实时图表更新逻辑
            // 例如CPU使用率、内存使用率的趋势图
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
                bool success = await _sessionService.SendCommandAsync(
                    _session.SessionID,
                    MessageCommands.SendMessageToUser,
                    request);

                if (success)
                {
                    // 添加到消息历史
                    AddMessageToHistory($"管理员: {txtMessage.Text}", Color.Blue);
                    txtMessage.Clear();
                }
                else
                {
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
                    MessageBox.Show("强制同步命令已发送", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AddMessageToHistory("系统: 强制同步缓存命令已发送", Color.Green);
                }
                else
                {
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

                AddMessageToHistory("[清除缓存] 已发送缓存清除命令", Color.YellowGreen);
            }
            catch (Exception ex)
            {
                _logger.LogError($"清除缓存失败: {ex.Message}");
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
                        MessageBox.Show("重启命令已发送", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        AddMessageToHistory("系统: 客户端重启命令已发送", Color.Orange);
                    }
                    else
                    {
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

                    AddMessageToHistory("[关机] 已发送客户端关机命令", Color.Yellow);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"关机失败: {ex.Message}");
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
            Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}: {ex.Message}");
            // 这里可以集成实际的日志系统
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