using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using RUINORERP.Model.Base.StatusManager.PerformanceMonitoring;
using RUINORERP.Server.Network.Services;

namespace RUINORERP.Server.Controls
{
    /// <summary>
    /// 内存数据点
    /// </summary>
    public class MemoryDataPoint
    {
        public DateTime Time { get; set; }
        public long MemoryBytes { get; set; }
        public double MemoryMB => MemoryBytes / (1024.0 * 1024.0);
        public bool IsWarning => MemoryMB > 500;
    }

    /// <summary>
    /// 性能监控控件
    /// 服务器端用于显示和管理性能监控数据的UI控件
    /// </summary>
    public partial class PerformanceMonitorControl : UserControl
    {
        private PerformanceDataStorageService _storageService;
        private Timer _refreshTimer;
        private int _memoryWarningThresholdMB = 500;
        private readonly List<MemoryDataPoint> _memoryHistory = new List<MemoryDataPoint>();
        private const int MaxHistoryPoints = 60;

        /// <summary>
        /// 构造函数
        /// </summary>
        public PerformanceMonitorControl()
        {
            InitializeComponent();
            InitializeUI();
        }

        /// <summary>
        /// 设置数据存储服务
        /// </summary>
        public void SetStorageService(PerformanceDataStorageService storageService)
        {
            _storageService = storageService;
            RefreshData();
        }

        /// <summary>
        /// 初始化UI
        /// </summary>
        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;

            // 创建主布局面板
            var mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 4,
                ColumnCount = 1
            };
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 150)); // 图表区域
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 35));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 65));

            // 工具栏
            var toolbarPanel = CreateToolbarPanel();
            mainPanel.Controls.Add(toolbarPanel, 0, 0);

            // 趋势图表
            var chartPanel = CreateChartPanel();
            mainPanel.Controls.Add(chartPanel, 0, 1);

            // 客户端列表
            var clientListPanel = CreateClientListPanel();
            mainPanel.Controls.Add(clientListPanel, 0, 2);

            // 统计详情
            var statisticsPanel = CreateStatisticsPanel();
            mainPanel.Controls.Add(statisticsPanel, 0, 3);

            this.Controls.Add(mainPanel);

            // 初始化定时刷新
            _refreshTimer = new Timer();
            _refreshTimer.Interval = 5000; // 5秒刷新一次
            _refreshTimer.Tick += (s, e) => RefreshData();
        }

        /// <summary>
        /// 创建图表面板（内存使用趋势）
        /// </summary>
        private Panel CreateChartPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(5)
            };

            var lblTitle = new Label
            {
                Text = "内存使用趋势 (最近10分钟)",
                Dock = DockStyle.Top,
                Height = 20,
                Font = new Font(this.Font, FontStyle.Bold)
            };

            // 使用ListView模拟简单图表
            var chartListView = new ListView
            {
                Name = "lvMemoryChart",
                Dock = DockStyle.Fill,
                View = View.Details,
                GridLines = true,
                BackColor = Color.WhiteSmoke
            };
            chartListView.Columns.Add("时间", 80);
            chartListView.Columns.Add("内存(MB)", 80);
            chartListView.Columns.Add("趋势", 200);
            chartListView.Columns.Add("状态", 60);

            // 阈值设置
            var settingsPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 30
            };

            var lblThreshold = new Label
            {
                Text = "警告阈值(MB):",
                Location = new Point(10, 5),
                AutoSize = true
            };

            var txtThreshold = new TextBox
            {
                Name = "txtMemoryThreshold",
                Location = new Point(110, 5),
                Width = 60,
                Text = "500"
            };

            var btnApplyThreshold = new Button
            {
                Text = "应用",
                Location = new Point(180, 3),
                Size = new Size(50, 22)
            };
            btnApplyThreshold.Click += (s, e) =>
            {
                if (int.TryParse(txtThreshold.Text, out int threshold))
                {
                    _memoryWarningThresholdMB = threshold;
                    RefreshData();
                }
            };

            settingsPanel.Controls.Add(lblThreshold);
            settingsPanel.Controls.Add(txtThreshold);
            settingsPanel.Controls.Add(btnApplyThreshold);

            panel.Controls.Add(chartListView);
            panel.Controls.Add(settingsPanel);
            panel.Controls.Add(lblTitle);

            return panel;
        }

        /// <summary>
        /// 创建工具栏面板
        /// </summary>
        private Panel CreateToolbarPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle
            };

            var btnRefresh = new Button
            {
                Text = "刷新数据",
                Location = new Point(10, 10),
                Size = new Size(80, 30)
            };
            btnRefresh.Click += (s, e) => RefreshData();

            var btnClear = new Button
            {
                Text = "清除数据",
                Location = new Point(100, 10),
                Size = new Size(80, 30)
            };
            btnClear.Click += (s, e) => ClearAllData();

            var btnAutoRefresh = new CheckBox
            {
                Text = "自动刷新",
                Location = new Point(190, 15),
                Size = new Size(80, 20),
                Checked = true
            };
            btnAutoRefresh.CheckedChanged += (s, e) =>
            {
                _refreshTimer.Enabled = btnAutoRefresh.Checked;
            };

            var lblStatus = new Label
            {
                Name = "lblStatus",
                Text = "就绪",
                Location = new Point(280, 15),
                Size = new Size(200, 20),
                AutoSize = true
            };

            panel.Controls.Add(btnRefresh);
            panel.Controls.Add(btnClear);
            panel.Controls.Add(btnAutoRefresh);
            panel.Controls.Add(lblStatus);

            return panel;
        }

        /// <summary>
        /// 创建客户端列表面板
        /// </summary>
        private Panel CreateClientListPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(5)
            };

            var lblTitle = new Label
            {
                Text = "客户端列表",
                Dock = DockStyle.Top,
                Height = 20,
                Font = new Font(this.Font, FontStyle.Bold)
            };

            var listView = new ListView
            {
                Name = "lvClients",
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true
            };
            listView.Columns.Add("客户端ID", 200);
            listView.Columns.Add("总指标数", 100);
            listView.Columns.Add("首次上报", 150);
            listView.Columns.Add("最后上报", 150);
            listView.Columns.Add("方法执行", 80);
            listView.Columns.Add("数据库", 80);
            listView.Columns.Add("网络", 80);
            listView.Columns.Add("内存", 80);
            listView.Columns.Add("事务", 80);
            listView.Columns.Add("死锁", 80);

            listView.SelectedIndexChanged += (s, e) =>
            {
                if (listView.SelectedItems.Count > 0)
                {
                    var clientId = listView.SelectedItems[0].Text;
                    ShowClientDetails(clientId);
                }
            };

            panel.Controls.Add(listView);
            panel.Controls.Add(lblTitle);

            return panel;
        }

        /// <summary>
        /// 创建统计详情面板
        /// </summary>
        private Panel CreateStatisticsPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(5)
            };

            var lblTitle = new Label
            {
                Text = "统计详情",
                Dock = DockStyle.Top,
                Height = 20,
                Font = new Font(this.Font, FontStyle.Bold)
            };

            var textBox = new TextBox
            {
                Name = "txtStatistics",
                Dock = DockStyle.Fill,
                Multiline = true,
                ScrollBars = ScrollBars.Both,
                Font = new Font("Consolas", 9),
                ReadOnly = true,
                BackColor = Color.White
            };

            panel.Controls.Add(textBox);
            panel.Controls.Add(lblTitle);

            return panel;
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        private void RefreshData()
        {
            if (_storageService == null)
                return;

            try
            {
                var clientInfos = _storageService.GetAllClientInfos();
                UpdateClientList(clientInfos);
                UpdateMemoryChart(clientInfos);

                var lblStatus = this.Controls.Find("lblStatus", true).FirstOrDefault() as Label;
                if (lblStatus != null)
                {
                    lblStatus.Text = $"最后更新: {DateTime.Now:HH:mm:ss}, 客户端数: {clientInfos.Count}";
                }
            }
            catch (Exception ex)
            {
                ShowError($"刷新数据失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 更新内存趋势图表
        /// </summary>
        private void UpdateMemoryChart(List<ClientPerformanceInfo> clientInfos)
        {
            var chartListView = this.Controls.Find("lvMemoryChart", true).FirstOrDefault() as ListView;
            if (chartListView == null)
                return;

            // 从所有客户端收集最新的内存数据
            foreach (var client in clientInfos)
            {
                var dataPoint = new MemoryDataPoint
                {
                    Time = client.LastReportTime,
                    MemoryBytes = client.TotalMetrics * 1024 // 估算值，实际应从原始数据获取
                };

                // 添加到历史记录
                _memoryHistory.Add(dataPoint);
            }

            // 限制历史记录数量
            while (_memoryHistory.Count > MaxHistoryPoints)
            {
                _memoryHistory.RemoveAt(0);
            }

            // 更新图表显示
            chartListView.Items.Clear();
            foreach (var point in _memoryHistory.OrderByDescending(p => p.Time).Take(20))
            {
                var item = new ListViewItem(point.Time.ToString("HH:mm:ss"));
                item.SubItems.Add(point.MemoryMB.ToString("F1"));

                // 生成简单的趋势条
                var barLength = (int)(point.MemoryMB / 10);
                barLength = Math.Min(barLength, 20);
                var bar = new string('█', barLength);
                item.SubItems.Add(bar);

                // 状态标记
                if (point.MemoryMB > _memoryWarningThresholdMB)
                {
                    item.BackColor = Color.LightPink;
                    item.SubItems.Add("⚠️");
                }
                else if (point.MemoryMB > _memoryWarningThresholdMB * 0.8)
                {
                    item.BackColor = Color.LightYellow;
                    item.SubItems.Add("⚡");
                }
                else
                {
                    item.SubItems.Add("✓");
                }

                chartListView.Items.Add(item);
            }
        }

        /// <summary>
        /// 更新客户端列表
        /// </summary>
        private void UpdateClientList(List<ClientPerformanceInfo> clientInfos)
        {
            var listView = this.Controls.Find("lvClients", true).FirstOrDefault() as ListView;
            if (listView == null)
                return;

            listView.Items.Clear();

            foreach (var info in clientInfos.OrderByDescending(c => c.LastReportTime))
            {
                var item = new ListViewItem(info.ClientId);
                item.SubItems.Add(info.TotalMetrics.ToString("N0"));
                item.SubItems.Add(info.FirstReportTime.ToString("yyyy-MM-dd HH:mm:ss"));
                item.SubItems.Add(info.LastReportTime.ToString("yyyy-MM-dd HH:mm:ss"));

                // 各类型指标数
                item.SubItems.Add(GetMetricTypeCount(info, PerformanceMonitorType.MethodExecution).ToString("N0"));
                item.SubItems.Add(GetMetricTypeCount(info, PerformanceMonitorType.Database).ToString("N0"));
                item.SubItems.Add(GetMetricTypeCount(info, PerformanceMonitorType.Network).ToString("N0"));
                item.SubItems.Add(GetMetricTypeCount(info, PerformanceMonitorType.Memory).ToString("N0"));
                item.SubItems.Add(GetMetricTypeCount(info, PerformanceMonitorType.Transaction).ToString("N0"));
                item.SubItems.Add(GetMetricTypeCount(info, PerformanceMonitorType.Deadlock).ToString("N0"));

                listView.Items.Add(item);
            }
        }

        /// <summary>
        /// 获取指定类型的指标数量
        /// </summary>
        private long GetMetricTypeCount(ClientPerformanceInfo info, PerformanceMonitorType type)
        {
            if (info.MetricTypeCounts.TryGetValue(type, out var count))
                return count;
            return 0;
        }

        /// <summary>
        /// 显示客户端详情
        /// </summary>
        private void ShowClientDetails(string clientId)
        {
            if (_storageService == null)
                return;

            try
            {
                var clientInfo = _storageService.GetClientInfo(clientId);
                if (clientInfo == null)
                    return;

                // 生成统计摘要
                var request = new RUINORERP.PacketSpec.Commands.CommandDefinitions.PerformanceStatisticsRequest
                {
                    TargetClientId = clientId,
                    TimeRangeHours = 1
                };

                var response = _storageService.GetStatisticsSummary(request);

                var textBox = this.Controls.Find("txtStatistics", true).FirstOrDefault() as TextBox;
                if (textBox != null)
                {
                    var sb = new System.Text.StringBuilder();
                    sb.AppendLine($"=== 客户端性能统计: {clientId} ===");
                    sb.AppendLine($"生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    sb.AppendLine();

                    sb.AppendLine($"总指标数: {clientInfo.TotalMetrics:N0}");
                    sb.AppendLine($"首次上报: {clientInfo.FirstReportTime:yyyy-MM-dd HH:mm:ss}");
                    sb.AppendLine($"最后上报: {clientInfo.LastReportTime:yyyy-MM-dd HH:mm:ss}");
                    sb.AppendLine();

                    if (!string.IsNullOrEmpty(response.StatisticsJson))
                    {
                        var summary = Newtonsoft.Json.JsonConvert.DeserializeObject<PerformanceStatisticsSummary>(response.StatisticsJson);
                        if (summary != null)
                        {
                            sb.AppendLine("=== 方法执行统计 ===");
                            sb.AppendLine($"  总调用次数: {summary.MethodSummary.TotalCalls:N0}");
                            sb.AppendLine($"  成功: {summary.MethodSummary.SuccessfulCalls:N0}, 失败: {summary.MethodSummary.FailedCalls:N0}");
                            sb.AppendLine($"  平均耗时: {summary.MethodSummary.AverageExecutionTimeMs:F2}ms");
                            sb.AppendLine($"  最大耗时: {summary.MethodSummary.MaxExecutionTimeMs}ms");
                            sb.AppendLine();

                            sb.AppendLine("=== 数据库统计 ===");
                            sb.AppendLine($"  总查询数: {summary.DatabaseSummary.TotalQueries:N0}");
                            sb.AppendLine($"  平均耗时: {summary.DatabaseSummary.AverageQueryTimeMs:F2}ms");
                            sb.AppendLine($"  慢查询数: {summary.DatabaseSummary.SlowQueryCount:N0}");
                            sb.AppendLine($"  死锁次数: {summary.DatabaseSummary.DeadlockCount:N0}");
                            sb.AppendLine();

                            sb.AppendLine("=== 网络统计 ===");
                            sb.AppendLine($"  总请求数: {summary.NetworkSummary.TotalRequests:N0}");
                            sb.AppendLine($"  成功: {summary.NetworkSummary.SuccessfulRequests:N0}, 失败: {summary.NetworkSummary.FailedRequests:N0}");
                            sb.AppendLine($"  平均响应时间: {summary.NetworkSummary.AverageResponseTimeMs:F2}ms");
                            sb.AppendLine($"  发送数据: {summary.NetworkSummary.TotalBytesSent / 1024:N0} KB");
                            sb.AppendLine($"  接收数据: {summary.NetworkSummary.TotalBytesReceived / 1024:N0} KB");
                            sb.AppendLine();

                            sb.AppendLine("=== 内存统计 ===");
                            sb.AppendLine($"  平均工作集: {summary.MemorySummary.AverageWorkingSetMB} MB");
                            sb.AppendLine($"  最大工作集: {summary.MemorySummary.MaxWorkingSetMB} MB");
                            sb.AppendLine($"  GC触发次数: {summary.MemorySummary.GCTriggerCount:N0}");
                            sb.AppendLine();

                            sb.AppendLine("=== 事务统计 ===");
                            sb.AppendLine($"  总事务数: {summary.TransactionSummary.TotalTransactions:N0}");
                            sb.AppendLine($"  已提交: {summary.TransactionSummary.CommittedCount:N0}, 已回滚: {summary.TransactionSummary.RolledBackCount:N0}");
                            sb.AppendLine($"  死锁次数: {summary.TransactionSummary.DeadlockCount:N0}");
                            sb.AppendLine($"  长事务数: {summary.TransactionSummary.LongRunningTransactionCount:N0}");
                            sb.AppendLine();

                            sb.AppendLine("=== 死锁统计 ===");
                            sb.AppendLine($"  总死锁数: {summary.DeadlockSummary.TotalDeadlocks:N0}");
                            if (summary.DeadlockSummary.FirstDeadlockTime.HasValue)
                            {
                                sb.AppendLine($"  首次死锁: {summary.DeadlockSummary.FirstDeadlockTime.Value:yyyy-MM-dd HH:mm:ss}");
                                sb.AppendLine($"  最后死锁: {summary.DeadlockSummary.LastDeadlockTime.Value:yyyy-MM-dd HH:mm:ss}");
                            }
                        }
                    }

                    textBox.Text = sb.ToString();
                }
            }
            catch (Exception ex)
            {
                ShowError($"显示客户端详情失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 清除所有数据
        /// </summary>
        private void ClearAllData()
        {
            if (_storageService == null)
                return;

            var result = MessageBox.Show("确定要清除所有性能监控数据吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                _storageService.ClearAllData();
                RefreshData();
            }
        }

        /// <summary>
        /// 显示错误信息
        /// </summary>
        private void ShowError(string message)
        {
            var textBox = this.Controls.Find("txtStatistics", true).FirstOrDefault() as TextBox;
            if (textBox != null)
            {
                textBox.Text = $"[错误] {message}";
                textBox.ForeColor = Color.Red;
            }
        }

        /// <summary>
        /// 启动自动刷新
        /// </summary>
        public void StartAutoRefresh()
        {
            _refreshTimer?.Start();
        }

        /// <summary>
        /// 停止自动刷新
        /// </summary>
        public void StopAutoRefresh()
        {
            _refreshTimer?.Stop();
        }
    }
}
