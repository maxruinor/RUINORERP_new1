using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting; // ✅ 添加图表命名空间
using RUINORERP.Model.Base.StatusManager.PerformanceMonitoring;
using RUINORERP.Server.Network.Services;
using RUINORERP.Server.Network.Monitoring; // ✅ 添加监控命名空间

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
        private ServerLockManager _lockManager; // ✅ 添加锁管理器引用
        private Timer _refreshTimer;
        private int _memoryWarningThresholdMB = 500;
        private readonly List<MemoryDataPoint> _memoryHistory = new List<MemoryDataPoint>();
        private const int MaxHistoryPoints = 60;
        
        // ✅ 新增：图表控件
        private Chart _memoryChart;
        
        // ✅ 新增：性能指标采集器
        private BizCodeMetricsCollector _bizCodeCollector;
        private BroadcastMetricsCollector _broadcastCollector;
        private CacheMetricsCollector _cacheCollector;
        private DatabaseMetricsCollector _databaseCollector;
        
        // ✅ 新增：性能指标 UI 控件
        private FlowLayoutPanel _performanceMetricsPanel;
        private GroupBox _bizCodeGroupBox;
        private GroupBox _broadcastGroupBox;
        private GroupBox _cacheGroupBox;
        private GroupBox _databaseGroupBox;

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
        /// ✅ 设置锁管理器（用于获取单据锁定监控数据）
        /// </summary>
        public void SetLockManager(ServerLockManager lockManager)
        {
            _lockManager = lockManager;
        }

        /// <summary>
        /// 初始化 UI
        /// </summary>
        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;

            // 创建主布局面板
            var mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 5,
                ColumnCount = 1
            };
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 200)); // 性能指标面板
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 150)); // 图表区域
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 35));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 65));

            // 工具栏
            var toolbarPanel = CreateToolbarPanel();
            mainPanel.Controls.Add(toolbarPanel, 0, 0);

            // ✅ 新增：性能指标面板
            var metricsPanel = CreatePerformanceMetricsPanel();
            mainPanel.Controls.Add(metricsPanel, 0, 1);

            // 趋势图表
            var chartPanel = CreateChartPanel();
            mainPanel.Controls.Add(chartPanel, 0, 2);

            // 客户端列表
            var clientListPanel = CreateClientListPanel();
            mainPanel.Controls.Add(clientListPanel, 0, 3);

            // 统计详情
            var statisticsPanel = CreateStatisticsPanel();
            mainPanel.Controls.Add(statisticsPanel, 0, 4);

            this.Controls.Add(mainPanel);

            // 初始化定时刷新
            _refreshTimer = new Timer();
            _refreshTimer.Interval = 5000; // 5 秒刷新一次
            _refreshTimer.Tick += (s, e) => RefreshData();
            
            // ✅ 初始化性能指标采集器
            InitializeMetricsCollectors();
        }
        
        /// <summary>
        /// ✅ 初始化性能指标采集器
        /// </summary>
        private void InitializeMetricsCollectors()
        {
            var config = new PerformanceMonitoringConfig();
            _bizCodeCollector = new BizCodeMetricsCollector(config);
            _broadcastCollector = new BroadcastMetricsCollector(config);
            _cacheCollector = new CacheMetricsCollector(config);
            _databaseCollector = new DatabaseMetricsCollector(config);
        }
        
        /// <summary>
        /// ✅ 设置外部注入的性能监控采集器（由 ServerMonitorControl 注入）
        /// </summary>
        public void SetMetricsCollectors(IEnumerable<IPerformanceMetricsCollector> collectors)
        {
            foreach (var collector in collectors)
            {
                switch (collector.CollectorName)
                {
                    case "BizCodeMetrics":
                        _bizCodeCollector = collector as BizCodeMetricsCollector;
                        break;
                    case "BroadcastMetrics":
                        _broadcastCollector = collector as BroadcastMetricsCollector;
                        break;
                    case "CacheMetrics":
                        _cacheCollector = collector as CacheMetricsCollector;
                        break;
                    case "DatabaseMetrics":
                        _databaseCollector = collector as DatabaseMetricsCollector;
                        break;
                }
            }
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

            // ✅ 使用真正的Chart控件替代ListView
            _memoryChart = new Chart
            {
                Name = "chartMemoryTrend",
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderlineColor = Color.LightGray,
                BorderlineDashStyle = ChartDashStyle.Solid
            };

            // 创建图表区域
            var chartArea = new ChartArea("MemoryArea")
            {
                AxisX = { Title = "时间", LabelStyle = { Format = "HH:mm:ss" } },
                AxisY = { Title = "内存(MB)", Minimum = 0 }
            };
            
            // 添加警戒线
            var stripLine = new StripLine
            {
                Interval = 0,
                StripWidth = 0,
                BorderWidth = 2,
                BorderColor = Color.Red,
                Text = $"警告阈值: {_memoryWarningThresholdMB}MB",
                BackColor = Color.FromArgb(50, Color.Red)
            };
            chartArea.AxisY.StripLines.Add(stripLine);
            
            _memoryChart.ChartAreas.Add(chartArea);

            // 创建系列
            var series = new Series("MemoryUsage")
            {
                ChartType = SeriesChartType.Spline, // 平滑曲线
                Color = Color.Blue,
                BorderWidth = 2,
                XValueType = ChartValueType.DateTime,
                YValueType = ChartValueType.Double,
                IsValueShownAsLabel = false
            };
            
            _memoryChart.Series.Add(series);

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
                    
                    // ✅ 更新图表警戒线
                    if (_memoryChart.ChartAreas.Count > 0 && _memoryChart.ChartAreas[0].AxisY.StripLines.Count > 0)
                    {
                        var line = _memoryChart.ChartAreas[0].AxisY.StripLines[0];
                        line.Text = $"警告阈值: {threshold}MB";
                    }
                    
                    RefreshData();
                }
            };

            settingsPanel.Controls.Add(lblThreshold);
            settingsPanel.Controls.Add(txtThreshold);
            settingsPanel.Controls.Add(btnApplyThreshold);

            panel.Controls.Add(_memoryChart); // ✅ 添加Chart控件
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
        /// ✅ 创建性能指标面板
        /// </summary>
        private Panel CreatePerformanceMetricsPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(5)
            };

            var lblTitle = new Label
            {
                Text = "实时性能指标",
                Dock = DockStyle.Top,
                Height = 20,
                Font = new Font(this.Font, FontStyle.Bold)
            };

            // 创建 FlowLayoutPanel 用于放置各个指标组
            _performanceMetricsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoScroll = true
            };

            // 创建编号生成指标组
            _bizCodeGroupBox = CreateMetricGroupBox("编号生成性能", 
                new[] { "平均响应时间：-- ms", "成功率：--%", "累计成功：0 次", "累计失败：0 次" });
            _performanceMetricsPanel.Controls.Add(_bizCodeGroupBox);

            // 创建广播服务指标组
            _broadcastGroupBox = CreateMetricGroupBox("广播服务性能",
                new[] { "平均延迟：-- ms", "成功率：--%", "当前并发：0", "累计广播：0 次" });
            _performanceMetricsPanel.Controls.Add(_broadcastGroupBox);

            // 创建缓存指标组
            _cacheGroupBox = CreateMetricGroupBox("缓存服务性能",
                new[] { "缓存命中率：--%", "累计命中：0 次", "累计未命中：0 次" });
            _performanceMetricsPanel.Controls.Add(_cacheGroupBox);

            // 创建数据库指标组
            _databaseGroupBox = CreateMetricGroupBox("数据库性能",
                new[] { "平均查询耗时：-- ms", "慢查询数：0 次", "累计查询：0 次" });
            _performanceMetricsPanel.Controls.Add(_databaseGroupBox);

            panel.Controls.Add(_performanceMetricsPanel);
            panel.Controls.Add(lblTitle);

            return panel;
        }
        
        /// <summary>
        /// ✅ 创建指标组控件
        /// </summary>
        private GroupBox CreateMetricGroupBox(string title, string[] metrics)
        {
            var groupBox = new GroupBox
            {
                Text = title,
                Width = 280,
                Height = 120,
                Margin = new Padding(5),
                Font = new Font(this.Font.FontFamily, 8.5F)
            };

            var tableLayoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 4,
                ColumnCount = 2,
                Padding = new Padding(5)
            };

            for (int i = 0; i < 4; i++)
            {
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
                var label = new Label
                {
                    Text = i < metrics.Length ? metrics[i] : "--",
                    Dock = DockStyle.Fill,
                    AutoSize = true,
                    Tag = $"metric_{i}"
                };
                tableLayoutPanel.Controls.Add(label, i % 2, i / 2);
            }

            for (int i = 0; i < 2; i++)
            {
                tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            }

            groupBox.Controls.Add(tableLayoutPanel);
            return groupBox;
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
            // ✅ 添加单据锁定监控列
            listView.Columns.Add("单据锁定", 100);

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
                
                // ✅ 刷新性能指标
                UpdatePerformanceMetrics();

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
            // ✅ 使用真正的Chart控件
            if (_memoryChart == null || _memoryChart.Series.Count == 0)
                return;

            var series = _memoryChart.Series[0];
            series.Points.Clear();

            // 从所有客户端收集最新的内存数据
            foreach (var client in clientInfos.OrderByDescending(c => c.LastReportTime).Take(20))
            {
                // 估算内存使用（实际应从原始Metric数据获取）
                var memoryMB = client.TotalMetrics * 0.001; // 简化估算
                
                var point = new DataPoint
                {
                    XValue = client.LastReportTime.ToOADate(),
                    YValues = new[] { memoryMB }
                };
                
                // 根据阈值设置颜色
                if (memoryMB > _memoryWarningThresholdMB)
                {
                    point.Color = Color.Red;
                }
                else if (memoryMB > _memoryWarningThresholdMB * 0.8)
                {
                    point.Color = Color.Orange;
                }
                else
                {
                    point.Color = Color.Green;
                }
                
                series.Points.Add(point);
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
                
                // ✅ 如果是服务器端，显示单据锁定监控数据
                if (info.ClientId == "Server")
                {
                    var lockMetrics = GetDocumentLockMetrics();
                    item.SubItems.Add($"活跃:{lockMetrics.ActiveLockCount} 超时:{lockMetrics.LockTimeoutCount}");
                }
                else
                {
                    item.SubItems.Add("-");
                }

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
        /// ✅ 获取单据锁定监控指标
        /// </summary>
        private DocumentLockMetric GetDocumentLockMetrics()
        {
            if (_lockManager != null)
            {
                return _lockManager.GetDocumentLockMetrics();
            }
            
            // 如果没有锁管理器，返回默认值
            return new DocumentLockMetric
            {
                Timestamp = DateTime.Now,
                ClientId = "Server",
                MachineName = Environment.MachineName
            };
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

                    // ✅ 如果是服务器端，显示单据锁定监控数据
                    if (clientId == "Server" && _lockManager != null)
                    {
                        var lockMetrics = GetDocumentLockMetrics();
                        sb.AppendLine();
                        sb.AppendLine("=== 单据锁定监控（服务器端） ===");
                        sb.AppendLine($"  活跃锁数量: {lockMetrics.ActiveLockCount}");
                        sb.AppendLine($"  过期锁数量: {lockMetrics.ExpiredLockCount}");
                        sb.AppendLine($"  孤儿锁数量: {lockMetrics.OrphanedLockCount}");
                        sb.AppendLine($"  待处理解锁请求: {lockMetrics.PendingUnlockRequestCount}");
                        sb.AppendLine();
                        sb.AppendLine("  --- 锁操作统计 ---");
                        sb.AppendLine($"  锁获取成功次数: {lockMetrics.LockAcquireSuccessCount:N0}");
                        sb.AppendLine($"  锁冲突次数: {lockMetrics.LockConflictCount:N0}");
                        sb.AppendLine($"  锁超时次数: {lockMetrics.LockTimeoutCount:N0}");
                        sb.AppendLine($"  锁超时率: {lockMetrics.LockTimeoutRate:F2}%");
                        sb.AppendLine();
                        sb.AppendLine("  --- 广播统计 ---");
                        sb.AppendLine($"  广播总次数: {lockMetrics.BroadcastTotalCount:N0}");
                        sb.AppendLine($"  广播成功次数: {lockMetrics.BroadcastSuccessCount:N0}");
                        sb.AppendLine($"  广播失败次数: {lockMetrics.BroadcastFailedCount:N0}");
                        sb.AppendLine($"  广播成功率: {lockMetrics.BroadcastSuccessRate:F2}%");
                        sb.AppendLine();
                        sb.AppendLine("  --- 锁持有时间 ---");
                        sb.AppendLine($"  平均持有时间: {lockMetrics.AverageLockHoldTimeSeconds:F2}秒");
                        sb.AppendLine($"  最大持有时间: {lockMetrics.MaxLockHoldTimeSeconds:F2}秒");
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
        
        /// <summary>
        /// ✅ 刷新性能指标
        /// </summary>
        private void UpdatePerformanceMetrics()
        {
            try
            {
                // 更新编号生成指标
                if (_bizCodeCollector != null)
                {
                    var snapshots = _bizCodeCollector.GetCurrentSnapshots().ToList();
                    UpdateGroupBox(_bizCodeGroupBox, snapshots);
                }
                
                // 更新广播服务指标
                if (_broadcastCollector != null)
                {
                    var snapshots = _broadcastCollector.GetCurrentSnapshots().ToList();
                    UpdateGroupBox(_broadcastGroupBox, snapshots);
                }
                
                // 更新缓存指标
                if (_cacheCollector != null)
                {
                    var snapshots = _cacheCollector.GetCurrentSnapshots().ToList();
                    UpdateGroupBox(_cacheGroupBox, snapshots);
                }
                
                // 更新数据库指标
                if (_databaseCollector != null)
                {
                    var snapshots = _databaseCollector.GetCurrentSnapshots().ToList();
                    UpdateGroupBox(_databaseGroupBox, snapshots);
                }
            }
            catch (Exception ex)
            {
                // 静默处理，不影响其他功能
                System.Diagnostics.Debug.WriteLine($"更新性能指标失败：{ex.Message}");
            }
        }
        
        /// <summary>
        /// ✅ 更新组控件的指标显示
        /// </summary>
        private void UpdateGroupBox(GroupBox groupBox, List<PerformanceMetricSnapshot> snapshots)
        {
            if (groupBox == null || groupBox.Controls.Count == 0)
                return;
                
            var tableLayoutPanel = groupBox.Controls[0] as TableLayoutPanel;
            if (tableLayoutPanel == null)
                return;
                
            // 根据指标快照更新标签文本
            foreach (Control control in tableLayoutPanel.Controls)
            {
                if (control is Label label && label.Tag != null)
                {
                    var tag = label.Tag.ToString();
                    if (tag.StartsWith("metric_"))
                    {
                        int index = int.Parse(tag.Substring(7));
                        if (index < snapshots.Count)
                        {
                            var snapshot = snapshots[index];
                            var statusSymbol = snapshot.Status switch
                            {
                                MetricStatus.Normal => "✓",
                                MetricStatus.Warning => "⚠",
                                MetricStatus.Critical => "✗"
                            };
                            label.Text = $"{snapshot.MetricName}: {snapshot.CurrentValue:F2}{snapshot.Unit} {statusSymbol}";
                            
                            // 根据状态设置颜色
                            label.ForeColor = snapshot.Status switch
                            {
                                MetricStatus.Normal => Color.Black,
                                MetricStatus.Warning => Color.Orange,
                                MetricStatus.Critical => Color.Red,
                                _ => Color.Black
                            };
                        }
                    }
                }
            }
        }
    }
}
