using LiveChartsCore.SkiaSharpView.WinForms;
using LiveChartsCore.SkiaSharpView;
using RUINORERP.UI.BaseForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveChartsCore;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using RUINORERP.UI.ChartAnalyzer;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;
using LiveChartsCore.Kernel.Sketches;
using RUINORERP.UI.ChartFramework.Core;
using RUINORERP.UI.ChartFramework.Models;
using RUINORERP.UI.ChartFramework.DataProviders.Adapters;
using RUINORERP.UI.ChartFramework.Core.Rendering.Builders;
using Org.BouncyCastle.Asn1.Cms;
using OfficeOpenXml.Drawing.Chart;
using RUINORERP.UI.ChartFramework.Core.Contracts;

namespace RUINORERP.UI.CRM
{
    [MenuAttrAssemblyInfo("商机总览", ModuleMenuDefine.模块定义.客户关系, ModuleMenuDefine.客户关系.商机总览)]
    public partial class UCCRMChart : BaseUControl
    {
        public UCCRMChart()
        {
            InitializeComponent();
            ConfigureChineseFont();
        }

        private void ConfigureChineseFont()
        {
            // 方法1：使用系统已安装字体（推荐）
            var chineseFont = SKTypeface.FromFamilyName("Microsoft YaHei");
            LiveCharts.Configure(config =>
            config
                .HasGlobalSKTypeface(chineseFont)
        // 可选：深色主题适配
        // .AddDarkTheme()


        );


        }

        private async void UCCRMChart_Load(object sender, EventArgs e)
        {




            var db = MainForm.Instance.AppContext.Db; // 获取SqlSugar实例
            var request = new DataRequest
            {
                TimeField = "Created_at",
                RangeType = TimeRangeType.Monthly,
                StartTime = DateTime.Now.AddMonths(-6),
                EndTime = DateTime.Now,
                chartType = ChartType.Column
            };

            CustomerDataAdapter customerAdapter = new CustomerDataAdapter(MainForm.Instance.AppContext.Db);
            //var dataSource = await customerAdapter.GetDataAsync(request);
            // 构建图表
            var builder = ChartBuilderFactory.CreateBuilder(ChartType.Column, customerAdapter);
            builder.OnInteraction += (sender, args) =>
            {
                if (args.InteractionType == InteractionType.Click && args.DataPoint != null)
                {
                    Console.WriteLine($"点击了数据点: {args.DataPoint.Label}, 值: {args.DataPoint.YValue}");
                }
            };
            var dataSource = await customerAdapter.GetDataAsync(request);
            var chartControl = builder.BuildChartControl(dataSource);
         
            flowLayoutPanel1.Controls.Add(chartControl as CartesianChart);
           // Load1();
           // Load2();
        }

        private async void Load2()
        {
            // 示例1：按月统计
            var monthlyRequest = new DataRequest
            {
                Dimensions = new List<string> { "Created_at", "Employee_ID" },
                Metrics = new List<string> { MetricType.Count.ToString() },
                //TimeRange = TimeGranularity.Yearly,
                //TimeGroupType = TimeGranularity.Monthly,
                StartTime = new DateTime(2025, 1, 1),
                EndTime = new DateTime(2025, 12, 31)
            };

            // 创建数据源和构建器
            //var dataSource = new CustomerDataSource();
            //var monthlyData = await dataSource.GetDataAsync(monthlyRequest);
            //DisplayChartCreate(monthlyData, "新建目标客户数统计");

            //// 创建数据源和构建器
            //var RecordsDataSource = new FollowUpRecordsDataSource();
            //var monthlyRecordsData = await RecordsDataSource.GetDataAsync(monthlyRequest);
            //DisplayChartCreate(monthlyRecordsData, "跟踪记录统计");

            //// 创建数据源和构建器
            //var FollowUpPlansDataSource = new CRM_FollowUpPlansDataSource();
            //var monthlyFollowUpPlansData = await FollowUpPlansDataSource.GetDataAsync(monthlyRequest);
            //DisplayChartCreate(monthlyFollowUpPlansData, "跟踪计划统计");

            //var monthlyCompletionRateData = await FollowUpPlansDataSource.GetData跟进计划完成率Async(monthlyRequest);
            //DisplayChartCreate(monthlyCompletionRateData, "跟踪计划完成率", ChartType.Pie);

            // var monthlyCompletionRateData = await FollowUpPlansDataSource.GetData跟进计划状态占比Async(monthlyRequest);
            //DisplayChartCreate(monthlyCompletionRateData, "跟进计划状态占比", ChartType.Pie);

            // 创建数据源和构建器
            //var LeadsDataSource = new CRM_LeadsDataSource();
            //var monthlyLeadsData = await LeadsDataSource.GetDataAsync(monthlyRequest);
            //DisplayChartCreate(monthlyLeadsData, "线索记录统计");


            // 示例2：按季度+地区统计
            //var quarterRequest = new ChartRequest
            //{
            //    TimeGroupType = TimeRangeType.Quarterly,
            //    Dimensions = new List<string> { "Region_ID" },
            //    StartTime = new DateTime(2025, 1, 1),
            //    EndTime = new DateTime(2025, 12, 31)
            //};

            //var quarterData = await dataSource.GetCustomerStatsAsync(quarterRequest);
            //DisplayChart(quarterData, "按季度+地区客户统计");
        }

        /// <summary>
        /// 创建并显示图表
        /// </summary>
        /// <param name="data">图表数据集</param>
        /// <param name="title">图表标题</param>
        /// <param name="chartType">指定生成的图形类型，默认为折线图</param>
        /// <param name="width">图表宽度，默认400</param>
        /// <param name="height">图表高度，默认400</param>
        private void DisplayChartCreate(ChartData data, string title,
            ChartType chartType = ChartType.Line, int width = 400, int height = 400)
        {
            // 根据chartType和data中的配置创建相应图表
            switch (chartType)
            {
                case ChartType.Line:
                    CreateLineChart(data, title);
                    break;
                case ChartType.Column:
                    CreateColumnChart(data, title);
                    break;
                case ChartType.Pie:
                    CreatePieChart(data, title);
                    break;
            }


            if (data == null) throw new ArgumentNullException(nameof(data));
            if (data.Series == null || !data.Series.Any()) return;
            if (data.MetaData.CategoryLabels == null)
                data.MetaData.CategoryLabels = new List<string>().ToArray();

            // 确保在UI线程上执行
            if (flowLayoutPanel1.InvokeRequired)
            {
                flowLayoutPanel1.Invoke(new Action(() =>
                    DisplayChartCreate(data, title, chartType, width, height)));
                return;
            }

            // 清除现有图表
            flowLayoutPanel1.Controls.Clear();

            var seriesCollection = new List<ISeries>();

            foreach (var series in data.Series)
            {
                //  if (series.Values == null || !series.Values.Any()) continue;

                switch (chartType)
                {
                    case ChartType.Line:
                        seriesCollection.Add(new LineSeries<double>
                        {
                            Name = series.Name,
                            // Values = series.Values.ToArray(),
                            Fill = null,
                            IsVisibleAtLegend = true,
                            //XToolTipLabelFormatter = point => $"{point.Context.Series.Name}: {point.Coordinate.PrimaryValue}"
                            XToolTipLabelFormatter = point => $"{point.Context.Series.Name}: {point.Model}"
                        });
                        break;

                    case ChartType.Column:
                        seriesCollection.Add(new ColumnSeries<double>
                        {
                            Name = series.Name,
                            //Values = series.Values.ToArray(),
                            IsVisibleAtLegend = true,
                            XToolTipLabelFormatter = point => $"{point.Context.Series.Name}: {point.Model:N2}"//如保留两位小数
                        });
                        break;

                    case ChartType.Pie:
                        seriesCollection.Add(new PieSeries<double>
                        {
                            Name = series.Name,
                            //Values = series.Values.ToArray(),
                            IsHoverable = true,
                            ToolTipLabelFormatter = point => $"{point.Context.Series.Name}: {point.StackedValue.Share}"
                        });
                        break;
                }
            }

            if (!seriesCollection.Any()) return;

            // 创建适当的图表类型
            IChartView chart;
            if (chartType == ChartType.Pie)
            {
                var pieChart = new PieChart();
                pieChart.Series = seriesCollection.ToArray();
                pieChart.InitialRotation = 0;
                pieChart.MaxAngle = 360;
                //pieChart.Total = seriesCollection.Sum(s => ((PieSeries<double>)s).Values.Sum());
                //在 LiveCharts2 中，PieChart 没有 Total 属性。饼图会自动计算各部分的总和。如果您需要自定义总和，可以通过 InitialRotation 和 MaxAngle 来控制显示范围，但不需要手动设置总和。

                chart = pieChart;
            }
            else
            {
                var cartesianChart = new CartesianChart();
                cartesianChart.Series = seriesCollection.ToArray();
                cartesianChart.XAxes = new[] { new Axis { Labels = data.MetaData.CategoryLabels.ToArray() } };
                cartesianChart.YAxes = new[] { new Axis() };
                chart = cartesianChart;
            }

            // 创建适当的图表控件
            UserControl chartControl;
            if (chartType == ChartType.Pie)
            {
                var pieChart = new PieChart();
                pieChart.Series = seriesCollection.ToArray();
                pieChart.InitialRotation = 0;
                pieChart.MaxAngle = 360;
                chartControl = pieChart;
            }
            else
            {
                var cartesianChart = new CartesianChart();
                cartesianChart.Series = seriesCollection.ToArray();
                cartesianChart.XAxes = new[] { new Axis { Labels = data.MetaData.CategoryLabels.ToArray() } };
                cartesianChart.YAxes = new[] { new Axis() };
                chartControl = cartesianChart;
            }

            // 设置图表标题
            var titleLabel = new Label
            {
                Text = title,
                Font = new Font("Arial", 12, FontStyle.Bold),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // 创建容器控制图表大小
            var container = new Panel
            {
                Width = width,
                Height = height,
                BorderStyle = BorderStyle.FixedSingle
            };

            // 添加控件到容器
            container.Controls.Add(chartControl);
            chartControl.Dock = DockStyle.Fill;

            // 添加标题和图表到flowLayoutPanel
            flowLayoutPanel1.Controls.Add(titleLabel);
            flowLayoutPanel1.Controls.Add(container);

        }

        private void CreateLineChart(ChartData data, string title)
        {
            // 实现折线图创建逻辑
            // 使用data.PrimaryLabels作为X轴
            // 使用data.Series中的IsDashed等属性
        }

        private void CreateColumnChart(ChartData data, string title)
        {
            // 实现柱状图创建逻辑
            // 处理堆叠情况(data.IsStacked)
            // 使用data.SecondaryLabels进行分组
        }

        private void CreatePieChart(ChartData data, string title)
        {
            // 实现饼图创建逻辑
            // 使用series.DataPointLabels作为扇形标签
        }



        private async void Load1()
        {
            // 在WinForms中使用
            var db = MainForm.Instance.AppContext.Db; // 获取SqlSugar实例
            var request = new DataRequest
            {
                TimeField = "Created_at",
                // TimeRange = TimeGranularity.Monthly,
                StartTime = DateTime.Now.AddMonths(-6),
                EndTime = DateTime.Now
            };

            CustomerDataAdapter customerAdapter = new CustomerDataAdapter(MainForm.Instance.AppContext.Db);
            var dataSource = await customerAdapter.GetDataAsync(request);
            var builder = new ColumnChartBuilder(customerAdapter);
            builder.OnInteraction += (sender, args) =>
            {
                if (args.InteractionType == InteractionType.Click && args.DataPoint != null)
                {
                    Console.WriteLine($"点击了数据点: {args.DataPoint.Label}, 值: {args.DataPoint.YValue}");
                }
            };

            //var chart = await builder.BuildChartAsync(dataSource);

            //var chartControl = new CartesianChart
            //{
            //    Dock = DockStyle.Fill,
            //    Series = chart.Series,
            //    XAxes = chart.XAxes,
            //    YAxes = chart.YAxes
            //};
            var chartControl = builder.Build(dataSource);
            flowLayoutPanel1.Controls.Add(chartControl);
        }
    }
}
