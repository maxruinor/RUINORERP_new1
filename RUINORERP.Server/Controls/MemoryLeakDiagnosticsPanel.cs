using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RUINORERP.Server.Services;

namespace RUINORERP.Server.Controls
{
    /// <summary>
    /// 内存泄漏诊断面板
    /// 用于展示内存泄漏诊断结果和实时监控
    /// </summary>
    public partial class MemoryLeakDiagnosticsPanel : UserControl
    {
        private readonly MemoryLeakDiagnosticsService _diagnosticsService;
        private readonly System.Windows.Forms.Timer _refreshTimer;
        
        // UI 控件
        private Label lblStatus;
        private TextBox txtReport;
        private Button btnRunDiagnostics;
        private Button btnExportReport;
        private GroupBox grpCurrentStatus;
        private GroupBox grpTopConsumers;
        private GroupBox grpRecommendations;
        private Label lblWorkingSet;
        private Label lblManagedMemory;
        private Label lblGrowthRate;
        private Label lblTimeToCritical;
        private ListBox lstTopConsumers;
        private FlowLayoutPanel pnlRecommendations;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public MemoryLeakDiagnosticsPanel()
        {
            InitializeComponent();
            
            // 获取诊断服务
            _diagnosticsService = Startup.GetFromFac<MemoryLeakDiagnosticsService>();
            
            // 设置刷新定时器
            _refreshTimer = new System.Windows.Forms.Timer();
            _refreshTimer.Interval = 5000; // 5 秒刷新一次
            _refreshTimer.Tick += RefreshTimer_Tick;
            _refreshTimer.Start();
            
            // 初始加载
            LoadInitialData();
        }
        
        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // 主面板设置
            this.Name = "MemoryLeakDiagnosticsPanel";
            this.Size = new Size(1000, 700);
            this.Padding = new Padding(10);
            
            // 状态标签
            lblStatus = new Label
            {
                Name = "lblStatus",
                Text = "内存泄漏诊断状态",
                Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Bold),
                ForeColor = Color.Green,
                AutoSize = true,
                Location = new Point(10, 10)
            };
            this.Controls.Add(lblStatus);
            
            // 当前状态组
            grpCurrentStatus = new GroupBox
            {
                Name = "grpCurrentStatus",
                Text = "当前内存状态",
                Location = new Point(10, 50),
                Size = new Size(470, 120),
                Font = new Font("Microsoft YaHei UI", 9F)
            };
            
            lblWorkingSet = new Label
            {
                Name = "lblWorkingSet",
                Text = "工作集内存：-- MB",
                Location = new Point(20, 25),
                AutoSize = true,
                Font = new Font("Microsoft YaHei UI", 10F)
            };
            grpCurrentStatus.Controls.Add(lblWorkingSet);
            
            lblManagedMemory = new Label
            {
                Name = "lblManagedMemory",
                Text = "托管内存：-- MB",
                Location = new Point(20, 50),
                AutoSize = true,
                Font = new Font("Microsoft YaHei UI", 10F)
            };
            grpCurrentStatus.Controls.Add(lblManagedMemory);
            
            lblGrowthRate = new Label
            {
                Name = "lblGrowthRate",
                Text = "内存增长率：-- MB/分钟",
                Location = new Point(20, 75),
                AutoSize = true,
                Font = new Font("Microsoft YaHei UI", 10F)
            };
            grpCurrentStatus.Controls.Add(lblGrowthRate);
            
            lblTimeToCritical = new Label
            {
                Name = "lblTimeToCritical",
                Text = "预计达到临界值：-- 分钟",
                Location = new Point(20, 100),
                AutoSize = true,
                Font = new Font("Microsoft YaHei UI", 10F)
            };
            grpCurrentStatus.Controls.Add(lblTimeToCritical);
            
            this.Controls.Add(grpCurrentStatus);
            
            // Top 内存占用者组
            grpTopConsumers = new GroupBox
            {
                Name = "grpTopConsumers",
                Text = "Top 内存占用者",
                Location = new Point(490, 50),
                Size = new Size(490, 120),
                Font = new Font("Microsoft YaHei UI", 9F)
            };
            
            lstTopConsumers = new ListBox
            {
                Name = "lstTopConsumers",
                Location = new Point(15, 20),
                Size = new Size(460, 84),
                Font = new Font("Microsoft YaHei UI", 9F),
                HorizontalScrollbar = true
            };
            grpTopConsumers.Controls.Add(lstTopConsumers);
            
            this.Controls.Add(grpTopConsumers);
            
            // 诊断报告文本框
            txtReport = new TextBox
            {
                Name = "txtReport",
                Location = new Point(10, 180),
                Size = new Size(970, 300),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Consolas", 9F),
                ReadOnly = true
            };
            this.Controls.Add(txtReport);
            
            // 建议组
            grpRecommendations = new GroupBox
            {
                Name = "grpRecommendations",
                Text = "优化建议",
                Location = new Point(10, 490),
                Size = new Size(970, 150),
                Font = new Font("Microsoft YaHei UI", 9F)
            };
            
            pnlRecommendations = new FlowLayoutPanel
            {
                Name = "pnlRecommendations",
                Location = new Point(15, 20),
                Size = new Size(940, 115),
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };
            grpRecommendations.Controls.Add(pnlRecommendations);
            
            this.Controls.Add(grpRecommendations);
            
            // 运行诊断按钮
            btnRunDiagnostics = new Button
            {
                Name = "btnRunDiagnostics",
                Text = "立即运行诊断",
                Location = new Point(10, 650),
                Size = new Size(120, 30),
                Font = new Font("Microsoft YaHei UI", 9F),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRunDiagnostics.Click += BtnRunDiagnostics_Click;
            this.Controls.Add(btnRunDiagnostics);
            
            // 导出报告按钮
            btnExportReport = new Button
            {
                Name = "btnExportReport",
                Text = "导出诊断报告",
                Location = new Point(140, 650),
                Size = new Size(120, 30),
                Font = new Font("Microsoft YaHei UI", 9F),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnExportReport.Click += BtnExportReport_Click;
            this.Controls.Add(btnExportReport);
            
            this.ResumeLayout(false);
        }
        
        private void LoadInitialData()
        {
            if (_diagnosticsService != null)
            {
                var report = _diagnosticsService.GetLastReport();
                if (report != null)
                {
                    UpdateUI(report);
                }
                else
                {
                    txtReport.Text = "等待诊断数据...";
                }
            }
            else
            {
                txtReport.Text = "内存泄漏诊断服务未初始化";
                lblStatus.Text = "服务未可用";
                lblStatus.ForeColor = Color.Red;
            }
        }
        
        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            if (_diagnosticsService != null)
            {
                var report = _diagnosticsService.GetLastReport();
                if (report != null)
                {
                    UpdateUI(report);
                }
            }
        }
        
        private void BtnRunDiagnostics_Click(object sender, EventArgs e)
        {
            if (_diagnosticsService != null)
            {
                btnRunDiagnostics.Enabled = false;
                btnRunDiagnostics.Text = "诊断中...";
                
                try
                {
                    var report = _diagnosticsService.RunDiagnosticsNow();
                    UpdateUI(report);
                    
                    MessageBox.Show("诊断完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"诊断失败：{ex.Message}", "错误", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    btnRunDiagnostics.Enabled = true;
                    btnRunDiagnostics.Text = "立即运行诊断";
                }
            }
        }
        
        private void BtnExportReport_Click(object sender, EventArgs e)
        {
            if (_diagnosticsService != null)
            {
                var report = _diagnosticsService.GetLastReport();
                if (report != null)
                {
                    try
                    {
                        var sb = new StringBuilder();
                        sb.AppendLine("========== 内存泄漏诊断报告 ==========");
                        sb.AppendLine($"时间：{report.Timestamp:yyyy-MM-dd HH:mm:ss}");
                        sb.AppendLine();
                        sb.AppendLine($"工作集内存：{report.CurrentSnapshot.WorkingSetMB} MB");
                        sb.AppendLine($"托管内存：{report.CurrentSnapshot.ManagedMemoryMB} MB");
                        sb.AppendLine($"内存增长率：{report.MemoryGrowthRatePerMinute:F2} MB/分钟");
                        sb.AppendLine($"预计达到临界值：{(report.MinutesToCriticalThreshold < int.MaxValue ? report.MinutesToCriticalThreshold.ToString() : "N/A")} 分钟");
                        sb.AppendLine();
                        sb.AppendLine("Top 内存占用者:");
                        foreach (var consumer in report.TopMemoryConsumers)
                        {
                            sb.AppendLine($"  - {consumer.Name}: {consumer.EstimatedMemoryMB} MB");
                        }
                        sb.AppendLine();
                        sb.AppendLine("建议措施:");
                        foreach (var rec in report.Recommendations)
                        {
                            sb.AppendLine($"  {rec}");
                        }
                        sb.AppendLine("=====================================");
                        
                        // 保存到文件
                        var filePath = $"D:\\Dumps\\MemoryLeakReport_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                        System.IO.File.WriteAllText(filePath, sb.ToString());
                        
                        MessageBox.Show($"报告已导出到：{filePath}", "成功", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"导出失败：{ex.Message}", "错误", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        
        private void UpdateUI(MemoryLeakReport report)
        {
            // ✅ 确保在UI线程执行
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateUI(report)));
                return;
            }
            
            // 更新状态
            if (report.IsMemoryLeakDetected)
            {
                lblStatus.Text = "⚠️ 检测到内存泄漏！";
                lblStatus.ForeColor = Color.Red;
            }
            else
            {
                lblStatus.Text = "✅ 内存使用正常";
                lblStatus.ForeColor = Color.Green;
            }
            
            // 更新当前状态
            lblWorkingSet.Text = $"工作集内存：{report.CurrentSnapshot.WorkingSetMB} MB";
            lblManagedMemory.Text = $"托管内存：{report.CurrentSnapshot.ManagedMemoryMB} MB";
            
            var growthColor = report.MemoryGrowthRatePerMinute > 10 ? Color.Red : 
                             report.MemoryGrowthRatePerMinute > 5 ? Color.Orange : Color.Green;
            lblGrowthRate.ForeColor = growthColor;
            lblGrowthRate.Text = $"内存增长率：{report.MemoryGrowthRatePerMinute:F2} MB/分钟";
            
            if (report.MinutesToCriticalThreshold < int.MaxValue)
            {
                var timeColor = report.MinutesToCriticalThreshold < 60 ? Color.Red : 
                               report.MinutesToCriticalThreshold < 180 ? Color.Orange : Color.Green;
                lblTimeToCritical.ForeColor = timeColor;
                lblTimeToCritical.Text = $"预计达到临界值：{report.MinutesToCriticalThreshold} 分钟";
            }
            else
            {
                lblTimeToCritical.ForeColor = Color.Green;
                lblTimeToCritical.Text = "预计达到临界值：N/A (增长率低或为负)";
            }
            
            // 更新 Top 占用者列表
            lstTopConsumers.Items.Clear();
            foreach (var consumer in report.TopMemoryConsumers.Take(5))
            {
                var leakMark = consumer.IsPotentialLeak ? " ⚠️" : "";
                lstTopConsumers.Items.Add($"{consumer.Name}: {consumer.EstimatedMemoryMB} MB{leakMark}");
            }
            
            // 更新建议
            pnlRecommendations.Controls.Clear();
            foreach (var rec in report.Recommendations)
            {
                var lbl = new Label
                {
                    Text = rec,
                    AutoSize = true,
                    Font = new Font("Microsoft YaHei UI", 9F),
                    ForeColor = rec.StartsWith("⚠️") || rec.StartsWith("🔴") ? Color.Red : 
                               rec.StartsWith("💡") ? Color.Blue : Color.Black,
                    Margin = new Padding(5)
                };
                pnlRecommendations.Controls.Add(lbl);
            }
            
            // 更新报告文本
            txtReport.Text = GenerateReportText(report);
        }
        
        private string GenerateReportText(MemoryLeakReport report)
        {
            var sb = new StringBuilder();
            sb.AppendLine("========== 内存泄漏诊断报告 ==========");
            sb.AppendLine($"时间：{report.Timestamp:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine();
            sb.AppendLine($"【内存状态】");
            sb.AppendLine($"  工作集内存：{report.CurrentSnapshot.WorkingSetMB} MB");
            sb.AppendLine($"  托管内存：{report.CurrentSnapshot.ManagedMemoryMB} MB");
            sb.AppendLine($"  私有内存：{report.CurrentSnapshot.PrivateMemoryMB} MB");
            sb.AppendLine($"  虚拟内存：{report.CurrentSnapshot.VirtualMemoryMB} MB");
            sb.AppendLine($"  句柄数：{report.CurrentSnapshot.HandleCount}");
            sb.AppendLine($"  线程数：{report.CurrentSnapshot.ThreadCount}");
            sb.AppendLine();
            sb.AppendLine($"【泄漏检测】");
            sb.AppendLine($"  是否检测到泄漏：{(report.IsMemoryLeakDetected ? "是 ⚠️" : "否 ✅")}");
            sb.AppendLine($"  内存增长率：{report.MemoryGrowthRatePerMinute:F2} MB/分钟");
            sb.AppendLine($"  预计达到临界值：{(report.MinutesToCriticalThreshold < int.MaxValue ? report.MinutesToCriticalThreshold.ToString() : "N/A")} 分钟");
            sb.AppendLine();
            sb.AppendLine($"【Top 内存占用者】");
            foreach (var consumer in report.TopMemoryConsumers)
            {
                var leakMark = consumer.IsPotentialLeak ? " ⚠️ 可能是泄漏源" : "";
                sb.AppendLine($"  - {consumer.Name}: {consumer.EstimatedMemoryMB} MB{leakMark}");
                sb.AppendLine($"    {consumer.Description}");
            }
            sb.AppendLine();
            sb.AppendLine($"【GC 分布】");
            sb.AppendLine($"  Gen0 收集次数：{report.CurrentSnapshot.GCGeneration0}");
            sb.AppendLine($"  Gen1 收集次数：{report.CurrentSnapshot.GCGeneration1}");
            sb.AppendLine($"  Gen2 收集次数：{report.CurrentSnapshot.GCGeneration2}");
            sb.AppendLine($"  LOH 大小：{report.CurrentSnapshot.LOHSizeMB} MB");
            sb.AppendLine();
            sb.AppendLine($"【线程分析】");
            sb.AppendLine($"  总线程数：{report.ThreadAnalysis.TotalThreads}");
            sb.AppendLine($"  线程池线程：{report.ThreadAnalysis.ThreadPoolThreads}");
            sb.AppendLine($"  后台线程：{report.ThreadAnalysis.BackgroundThreads}");
            sb.AppendLine($"  前台线程：{report.ThreadAnalysis.ForegroundThreads}");
            sb.AppendLine();
            sb.AppendLine($"【建议措施】");
            foreach (var rec in report.Recommendations)
            {
                sb.AppendLine($"  {rec}");
            }
            sb.AppendLine("=====================================");
            
            return sb.ToString();
        }
    }
}
