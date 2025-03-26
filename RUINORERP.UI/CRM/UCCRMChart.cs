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

namespace RUINORERP.UI.CRM
{
    [MenuAttrAssemblyInfo("商机总览", ModuleMenuDefine.模块定义.客户关系, ModuleMenuDefine.客户关系.商机总览)]
    public partial class UCCRMChart : BaseUControl
    {
        public UCCRMChart()
        {
            InitializeComponent();
        }

        private  void UCCRMChart_Load(object sender, EventArgs e)
        {
            Load2();
        }

        private async void Load2()
        {

            // 示例1：按月统计
            var monthlyRequest = new ChartRequest
            {
                TimeGroupType = TimeRangeType.Monthly,
                StartTime = new DateTime(2025, 1, 1),
                EndTime = new DateTime(2025, 12, 31)
            };

            // 创建数据源和构建器
            var dataSource = new CustomerDataSource();
          


            var monthlyData = await dataSource.GetCustomerStatsAsync(monthlyRequest);
            DisplayChart(monthlyData, "按月客户统计");

            // 示例2：按季度+地区统计
            var quarterRequest = new ChartRequest
            {
                TimeGroupType = TimeRangeType.Quarterly,
                Dimensions = new List<string> { "Region_ID" },
                StartTime = new DateTime(2025, 1, 1),
                EndTime = new DateTime(2025, 12, 31)
            };

            var quarterData = await dataSource.GetCustomerStatsAsync(quarterRequest);
            DisplayChart(quarterData, "按季度+地区客户统计");
        }


        private void DisplayChart(ChartDataSet data, string title)
        {
            var seriesCollection = new List<ISeries>();

            foreach (var series in data.Series)
            {
                seriesCollection.Add(new ColumnSeries<double>
                {
                    Name = series.Name,
                    Values = series.Values.ToArray(),
                    XToolTipLabelFormatter = point => $"{point.Context.Series.Name}: {point.AsDataLabel}"
                });
            }

            cartesianChart1.Series = seriesCollection.ToArray();
            cartesianChart1.XAxes = new[] { new Axis { Labels = data.Labels.ToArray() } };
            cartesianChart1.Title = new LabelVisual { Text = title, TextSize = 16 };
        }

        private async void Load1()
        {
            // 创建请求参数
            var request = new ChartRequest
            {
                Dimensions = new List<string> { "Created_at" },
                Metrics = new List<string> { "Count" },
                TimeRange = TimeRangeType.Yearly
            };

            // 创建数据源和构建器
            var dataSource = new CustomerDataSource();
            var builder = new CustomerChartBuilder(dataSource);

            // 生成图表
            var chart = await builder.BuildChartAsync(request);

            // WinForms中显示
            var chartControl = new CartesianChart
            {
                Dock = DockStyle.Fill,
                Series = chart.Series,
                XAxes = chart.XAxes,
                YAxes = chart.YAxes
            };
            tabControlEx1.Controls.Add(chartControl);



            //   cartesianChart1.Series = new ISeries[]
            //{
            //       new LineSeries<double>
            //       {
            //           Values = new double[] { 2, 1, 3, 5, 3, 4, 6 },
            //           Fill = null
            //       }
            //};


            //        // 使用TabControl实现多维度查看
            //        var tabControl = new TabControl();

            //        // 第一页：趋势分析
            //        var trendPage = new TabPage("趋势分析")
            //        {
            //            Controls = {
            //    new DateTimePicker(),  // 自定义日期选择组件
            //    new CartesianChart {
            //        Series = new[] {
            //            new LineSeries<double> { Name = "客户线索" },
            //            new LineSeries<double> { Name = "目标客户" }
            //        },
            //        XAxes = new[] { new Axis { Labels = weeks } }
            //    }
            //}
            //        };

            //        // 第二页：个人对比
            //        var comparePage = new TabPage("个人对比")
            //        {
            //            Controls = {
            //            new ComboBox { DataSource = GetDepartments() },
            //            new CartesianChart {
            //                Series = new ColumnSeries<double>[] {
            //                    new() { Name = "张三", Values = [20,15,8,12] },
            //                    new() { Name = "李四", Values = [18,20,10,15] }
            //                }
            //            }
            //        }
            //        };
        }
    }
}
