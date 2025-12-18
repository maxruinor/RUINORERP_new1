using RUINORERP.Server.Network.Models;
using System;
using System.Windows.Forms;

namespace RUINORERP.Server.Controls
{
    public partial class SessionPerformanceForm : Form
    {
        private readonly SessionInfo _session;
        private readonly Timer _updateTimer;

        public SessionPerformanceForm(SessionInfo session)
        {
            InitializeComponent();
            _session = session;
            Text = $"会话性能详情 - {session.UserName ?? "未登录用户"} ({session.SessionID})";

            // 设置定时器用于实时更新性能数据
            _updateTimer = new Timer { Interval = 1000 };
            _updateTimer.Tick += UpdateTimer_Tick;
            _updateTimer.Start();

            // 初始化显示数据
            UpdatePerformanceData();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdatePerformanceData();
        }

        private void UpdatePerformanceData()
        {
            try
            {
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
                lblSentPackets.Text = stats.SentPackets.ToString();
                lblReceivedPackets.Text = stats.ReceivedPackets.ToString();

                // 数据量统计（转换为合适的单位）
                lblTotalBytesSent.Text = FormatBytes(stats.TotalBytesSent);
                lblTotalBytesReceived.Text = FormatBytes(stats.TotalBytesReceived);

                // 心跳信息
                lblHeartbeatCount.Text = _session.HeartbeatCount.ToString();

                // 状态指示器颜色
                SetStatusColor(stats.Status);

                // 计算并显示实时性能指标
                UpdateRealTimeMetrics();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("更新性能数据时出错: " + ex.Message);
            }
        }

        private void UpdateRealTimeMetrics()
        {
            // 这里可以添加更多实时性能指标的计算和显示
            // 例如：连接利用率、吞吐量、请求率等

            // 示例：计算大致的吞吐量（如果有相关数据）
            long totalBytes = _session.TotalBytesSent + _session.TotalBytesReceived;
            TimeSpan duration = DateTime.Now - _session.ConnectedTime;
            if (duration.TotalSeconds > 0)
            {
                double bytesPerSecond = totalBytes / duration.TotalSeconds;
                lblThroughput.Text = $"{FormatBytes((long)bytesPerSecond)}/秒";
            }
            else
            {
                lblThroughput.Text = "计算中...";
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

        private void SessionPerformanceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _updateTimer.Stop();
            _updateTimer.Dispose();
        }
    }
}