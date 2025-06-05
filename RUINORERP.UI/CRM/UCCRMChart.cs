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
using Netron.GraphLib;
using RUINORERP.Business.Security;
using ICSharpCode.SharpZipLib.Zip;
using static RUINORERP.UI.ChartFramework.Models.DataRequest;
using RUINORERP.Global;


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

        private void UCCRMChart_Load(object sender, EventArgs e)
        {
            //LoadOK();
            request = new DataRequest
            {
                TimeField = "Created_at",
                RangeType = TimeRangeType.Custom,
                StartTime = DtpStart.Value,
                EndTime = dtpEnd.Value,
                ChartType = ChartType.Column
            };

            if (AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser)
            {
                request.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID;
            }
            else
            {
                lblEmployee.Visible = true;
                cmbEmployee_ID.Visible = true;
                DataBindingHelper.BindData4CmbByEntity<tb_Employee>(request, k => k.Employee_ID, cmbEmployee_ID);
            }

            //var timeRangeOptions = DataBindingHelper.GetTimeSelectTypeItems(typeof(TimeSelectType));
            //cmbTimeType.DataSource = timeRangeOptions;
            //cmbTimeType.DisplayMember = "Description";
            //cmbTimeType.ValueMember = "Value";

            DataBindingHelper.BindData4CmbByEnum<DataRequest>(request, t => (int)t.SelectedTimeRange, typeof(TimeSelectType), cmbTimeType, false);
            DataBindingHelper.BindData4DataTime<DataRequest>(request, t => t.StartTime, DtpStart, false);
            DataBindingHelper.BindData4DataTime<DataRequest>(request, t => t.EndTime, dtpEnd, false);

        }

        DataRequest request;
        private async void LoadOK()
        {
            request = new DataRequest
            {
                TimeField = "Created_at",
                RangeType = TimeRangeType.Monthly,
                StartTime = DateTime.Now.AddMonths(-6),
                EndTime = DateTime.Now,
                ChartType = ChartType.Column
            };

            // 构建图表
            var builder = ChartBuilderFactory.CreateBuilder(request, new CustomerDataAdapter());
            builder.OnInteraction += (sender, args) =>
            {
                if (args.InteractionType == InteractionType.Click && args.DataPoint != null)
                {
                    Console.WriteLine($"点击了数据点: {args.DataPoint.Label}, 值: {args.DataPoint.YValue}");
                }
                switch (args.InteractionType)
                {
                    case InteractionType.Click:
                        Console.WriteLine($"点击了 {args.Series.Name} 系列的 {args.DataPoint.Label}");
                        break;
                    case InteractionType.RightClick:
                        Console.WriteLine($"右键点击了 {args.Series.Name} 系列");
                        break;
                    case InteractionType.Hover:
                        Console.WriteLine($"悬停在 {args.DataPoint.YValue} 值上");
                        break;
                }
            };
            var chartControl = await builder.BuildChartControl();
            UserControl userControl = chartControl as UserControl;
            userControl.Width = 400;
            userControl.Height = 400;
            flowLayoutPanel1.Controls.Add(userControl);

            var chartControl1 = await builder.BuildChart(request);
            UserControl userControl1 = chartControl1 as UserControl;
            userControl1.Width = 400;
            userControl1.Height = 400;
            flowLayoutPanel1.Controls.Add(userControl1);
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

            if (data == null) throw new ArgumentNullException(nameof(data));
            if (data.Series == null || !data.Series.Any()) return;
            if (data.CategoryLabels == null)
                data.CategoryLabels = new List<string>().ToArray();

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
                            Values = series.Values.ToArray(),
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
                            Values = series.Values.ToArray(),
                            IsVisibleAtLegend = true,
                            XToolTipLabelFormatter = point => $"{point.Context.Series.Name}: {point.Model:N2}"//如保留两位小数
                        });
                        break;

                    case ChartType.Pie:
                        seriesCollection.Add(new PieSeries<double>
                        {
                            Name = series.Name,
                            Values = series.Values.ToArray(),
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
                cartesianChart.XAxes = new[] { new Axis { Labels = data.CategoryLabels.ToArray() } };
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
                cartesianChart.XAxes = new[] { new Axis { Labels = data.CategoryLabels.ToArray() } };
                cartesianChart.YAxes = new[] { new Axis() };
                chartControl = cartesianChart;
            }
            chartControl.Width = 500;
            chartControl.Height = 500;
            // 添加标题和图表到flowLayoutPanel
            flowLayoutPanel1.Controls.Add(chartControl);

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
            var builder = ChartBuilderFactory.CreateBuilder(request, new CustomerDataAdapter());
            builder.OnInteraction += (sender, args) =>
            {
                if (args.InteractionType == InteractionType.Click && args.DataPoint != null)
                {
                    Console.WriteLine($"点击了数据点: {args.DataPoint.Label}, 值: {args.DataPoint.YValue}");
                }
            };
            var chartControl = await builder.BuildChartControl();
            flowLayoutPanel1.Controls.Add(chartControl as UserControl);
        }

        private async void btnQuery_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();

            request.ChartType = ChartType.Line;
            // 构建图表
            var builder = ChartBuilderFactory.CreateBuilder(request, new CRMDataAdapter(request));

            builder.OnInteraction += (sender, args) =>
            {
                if (args.InteractionType == InteractionType.Click && args.DataPoint != null)
                {
                    Console.WriteLine($"点击了数据点: {args.DataPoint.Label}, 值: {args.DataPoint.YValue}");
                }
                switch (args.InteractionType)
                {
                    case InteractionType.Click:
                        Console.WriteLine($"点击了 {args.Series.Name} 系列的 {args.DataPoint.Label}");
                        break;
                    case InteractionType.RightClick:
                        Console.WriteLine($"右键点击了 {args.Series.Name} 系列");
                        break;
                    case InteractionType.Hover:
                        Console.WriteLine($"悬停在 {args.DataPoint.YValue} 值上");
                        break;
                }
            };

            //var chartControl = await builder.BuildChartControl();
            //UserControl userControl = chartControl as UserControl;
            //userControl.Width = 900;
            //userControl.Height = 900;
            ////userControl.Dock = DockStyle.Fill;
            //flowLayoutPanel1.Controls.Add(userControl);
        
            var chartControl1 = await builder.BuildChart(request);
            UserControl userControl1 = chartControl1 as UserControl;
            userControl1.Width = 624;
            userControl1.Height = 500;
            flowLayoutPanel1.Controls.Add(userControl1);


            request.ChartType = ChartType.Pie;
            // 构建图表
            var PlanCompRatebuilder = ChartBuilderFactory.CreateBuilder(request, new CRMPlanCompRateDataAdapter(request));

            var PlanCompRatechartControl = await PlanCompRatebuilder.BuildChart(request);
            UserControl userControl2 = PlanCompRatechartControl as UserControl;
            userControl2.Width = 624;
            userControl2.Height = 500;
            flowLayoutPanel1.Controls.Add(userControl2);

        }
    }
}
