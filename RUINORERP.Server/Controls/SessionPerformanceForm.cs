using RUINORERP.Server.Network.Models;
using System;
using System.Windows.Forms;
using System.Diagnostics;
using RUINORERP.Server.Network.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Server.Controls
{
    public partial class SessionPerformanceForm : Form
    {
        private readonly SessionInfo _session;
        private readonly Timer _updateTimer;
        private readonly ILogger<SessionPerformanceForm> _logger;
        private int _baseUpdateInterval = 1000; // 基础更新间隔，单位：毫秒
        private int _currentUpdateInterval;
        private PerformanceCounter _cpuCounter;
        private PerformanceCounter _memoryCounter;

        public SessionPerformanceForm(SessionInfo session)
        {
            InitializeComponent();
            _session = session;
            _logger = Startup.GetFromFac<ILogger<SessionPerformanceForm>>();
            _currentUpdateInterval = _baseUpdateInterval;
            
            Text = $"会话性能详情 - {session.UserName ?? "未登录用户"} ({session.SessionID})";
            _logger.LogInformation("打开会话性能详情窗体，会话ID: {SessionID}", session.SessionID);

            // 初始化性能计数器
            InitializePerformanceCounters();

            // 设置定时器用于实时更新性能数据
            _updateTimer = new Timer { Interval = _currentUpdateInterval };
            _updateTimer.Tick += UpdateTimer_Tick;
            _updateTimer.Start();

            // 初始化显示数据
            UpdatePerformanceData();
        }

        /// <summary>
        /// 初始化性能计数器
        /// </summary>
        private void InitializePerformanceCounters()
        {
            try
            {
                _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                _memoryCounter = new PerformanceCounter("Memory", "Available MBytes");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "初始化性能计数器失败");
            }
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdatePerformanceData();
            AdjustRefreshInterval();
        }

        private void UpdatePerformanceData()
        {
            try
            {
                // 检查会话是否仍然有效
                if (_session == null || _session.Status == SessionStatus.Disconnected)
                {
                    _logger.LogInformation("会话已断开，关闭性能详情窗体，会话ID: {SessionID}", _session?.SessionID);
                    Close();
                    return;
                }

                var stats = _session.GetPerformanceStats();

                // 基本信息更新
                lblSessionId.Text = stats.SessionId;
                lblUserName.Text = stats.UserName ?? "未登录用户";
                lblStatus.Text = stats.Status.ToString();
                lblClientInfo.Text = stats.ClientInfo;
                lblClientVersion.Text = stats.ClientVersion ?? "未知";

                // 连接时间信息
                lblConnectedTime.Text = _session.ConnectedTime.ToString("yyyy-MM-dd HH:mm:ss");
                lblLastActivity.Text = stats.LastActivity.ToString("yyyy-MM-dd HH:mm:ss");

                // 计算并显示连接时长
                TimeSpan duration = stats.ConnectedDuration;
                lblConnectedDuration.Text = $"{duration.Days}天 {duration.Hours}小时 {duration.Minutes}分钟 {duration.Seconds}秒";

                // 网络传输统计
                lblSentPackets.Text = stats.SentPackets.ToString("N0");
                lblReceivedPackets.Text = stats.ReceivedPackets.ToString("N0");

                // 数据量统计（转换为合适的单位）
                lblTotalBytesSent.Text = FormatBytes(stats.TotalBytesSent);
                lblTotalBytesReceived.Text = FormatBytes(stats.TotalBytesReceived);

                // 心跳信息
                lblHeartbeatCount.Text = _session.HeartbeatCount.ToString("N0");

                // 计算并显示实时性能指标
                UpdateRealTimeMetrics();

                // 更新状态指示器颜色
                SetStatusColor(stats.Status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新会话性能数据失败，会话ID: {SessionID}", _session?.SessionID);
            }
        }

        private void UpdateRealTimeMetrics()
        {
            try
            {
                // 计算总数据量
                long totalBytes = _session.TotalBytesSent + _session.TotalBytesReceived;
                TimeSpan duration = DateTime.Now - _session.ConnectedTime;
                
                if (duration.TotalSeconds > 0)
                {
                    // 计算平均吞吐量
                    double bytesPerSecond = totalBytes / duration.TotalSeconds;
                    lblThroughput.Text = $"{FormatBytes((long)bytesPerSecond)}/秒";
                }
                else
                {
                    lblThroughput.Text = "0 B/秒";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新实时性能指标失败，会话ID: {SessionID}", _session.SessionID);
            }
        }

        private void SetStatusColor(SessionStatus status)
        {
            switch (status)
            {
                case SessionStatus.Connected:
                    picStatusIndicator.BackColor = System.Drawing.Color.Green;
                    break;
                case SessionStatus.Disconnected:
                    picStatusIndicator.BackColor = System.Drawing.Color.Red;
                    break;
                case SessionStatus.Authenticated:
                    picStatusIndicator.BackColor = System.Drawing.Color.Blue;
                    break;
                default:
                    picStatusIndicator.BackColor = System.Drawing.Color.Gray;
                    break;
            }
        }

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
        /// 根据系统负载调整刷新间隔
        /// </summary>
        private void AdjustRefreshInterval()
        {
            try
            {
                if (_cpuCounter == null)
                {
                    return;
                }

                float cpuUsage = _cpuCounter.NextValue();
                
                // 根据CPU使用率调整刷新间隔
                // 负载越高，刷新间隔越长，减少系统压力
                int newInterval;
                if (cpuUsage < 30)
                {
                    newInterval = _baseUpdateInterval;
                }
                else if (cpuUsage < 70)
                {
                    newInterval = _baseUpdateInterval * 2;
                }
                else
                {
                    newInterval = _baseUpdateInterval * 4;
                }
                
                // 只有当间隔变化超过200ms时才更新，避免频繁调整
                if (Math.Abs(newInterval - _currentUpdateInterval) > 200)
                {
                    _currentUpdateInterval = newInterval;
                    _updateTimer.Interval = _currentUpdateInterval;
                    _logger.LogDebug("调整会话性能详情刷新间隔为 {Interval}ms，CPU使用率: {CPUUsage:F1}%", 
                        _currentUpdateInterval, cpuUsage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "调整刷新间隔失败");
            }
        }

        private void SessionPerformanceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _updateTimer.Stop();
            _updateTimer.Dispose();
            
            // 释放性能计数器资源
            _cpuCounter?.Dispose();
            _memoryCounter?.Dispose();
            
        }
    }
}