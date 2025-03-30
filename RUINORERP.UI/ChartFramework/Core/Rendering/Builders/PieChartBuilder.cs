using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using global::RUINORERP.UI.ChartFramework.Models;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.SkiaSharpView.WinForms;
using RUINORERP.UI.ChartFramework.Core.Contracts;
using RUINORERP.UI.ChartFramework.Core.Rendering.Builders;
using RUINORERP.UI.ChartFramework.Rendering.Controls;
using SkiaSharp;


namespace RUINORERP.UI.ChartFramework.Core.Rendering.Builders
{
    public class PieChartBuilder : ChartBuilderBase<CartesianChart>
    {
        public PieChartBuilder(IDataProvider dataSource) : base(dataSource) { }
        //public override ChartControl Build(ChartDataSet data)
        //{
        //    var chart = new PieChart
        //    {
        //        Series = CreateSeries(data),
        //        Title = data.Title,
        //        InitialRotation = -15,
        //        AnimationsSpeed = TimeSpan.FromSeconds(1),
        //        VisualElements = CreateVisualElements(data)
        //    };

        //    return new ChartControl(chart, data)
        //        .WithPieContextMenu()
        //        .WithDonutOptions(data.MetaData.Is100PercentStack);
        //}
        public override ChartControl BuildChart(ChartData data)
        {
            //CurrentData = data;

            // 创建饼图实例
            var pieChart = new PieChart
            {
                Series = CreateSeries(data).ToArray(),
                Title = new LabelVisual
                {
                    Text = data.Title,
                    TextSize = 16,
                    Padding = new LiveChartsCore.Drawing.Padding(15),
                    Paint = new SolidColorPaint(SKColors.DarkSlateGray),
                    HorizontalAlignment = LiveChartsCore.Drawing.Align.Start,
                    VerticalAlignment = LiveChartsCore.Drawing.Align.Start
                },
                InitialRotation = -15,
                AnimationsSpeed = TimeSpan.FromSeconds(1),
                VisualElements = CreateVisualElements(data)
            };

            // 应用当前主题
            ApplyTheme(CurrentTheme);

            return new ChartControl(pieChart, data);
        }

        public ChartControl BuildChart1(ChartData data)
        {
            var pieChart = new PieChart
            {
                Series = data.Series.Select(series => new PieSeries<double>
                {
                    Name = series.Name,
                    Values = series.Values.ToArray(),
                    Fill = new SolidColorPaint(series.ColorHex.ToSKColor()),
                    DataLabelsPaint = new SolidColorPaint(SKColors.White),
                    DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                    DataLabelsFormatter = point => $"{point.Context.Series.Name}: {point.StackedValue}"
                }).ToArray(),

                Title = new LabelVisual
                {
                    Text = data.Title,
                    TextSize = 16,
                    Padding = new LiveChartsCore.Drawing.Padding(15),
                    Paint = new SolidColorPaint(SKColors.DarkSlateGray)
                },

                InitialRotation = -15,
                AnimationsSpeed = TimeSpan.FromSeconds(1)
            };

            return new ChartControl(pieChart,data);
        }
        protected override IEnumerable<ISeries> CreateSeries(ChartData data)
        {
            // 饼图通常只显示第一个系列
          
            if (data?.Series == null || !data.Series.Any())
                yield break;
            // 饼图通常只显示第一个系列
            var primarySeries = data.Series.First();

            yield return new PieSeries<double>
            {
                Name = primarySeries.Name,
                Values = primarySeries.Values.ToArray(),
                DataLabelsPaint = new SolidColorPaint(SKColors.White),
                DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                DataLabelsFormatter = point =>
                    $"{point.Context.Series.Name}: {FormatValue(point.StackedValue.Share, data)}",
                ToolTipLabelFormatter = point =>
                    $"{point.Context.Series.Name}\n" +
                    $"数值: {point.StackedValue.Total:N0}\n" +
                    $"占比: {point.StackedValue.Share:P1}",
                Fill = new SolidColorPaint(GetColorForIndex(0)),
                InnerRadius = data.MetaData.Is100PercentStack ? 60 : 0 // 环形图内半径
            };

            // 如果有多个系列，可以作为附加环添加
            if (data.Series.Count > 1)
            {
                for (int i = 1; i < data.Series.Count; i++)
                {
                    var series = data.Series[i];
                    yield return new PieSeries<double>
                    {
                        Name = series.Name,
                        Values = series.Values.ToArray(),
                        Fill = new SolidColorPaint(GetColorForIndex(i)),
                        InnerRadius = 60 + (i * 20) // 每增加一个系列，内半径增大
                    };
                }
            }
        }




        private string FormatValue(double value, ChartData data)
        {
            return data.MetaData.ValueType switch
            {
                Core.ValueType.Percentage => value.ToString("P1"),
                Core.ValueType.Currency => value.ToString("C2"),
                _ => value.ToString("N2")
            };
        }
      
        public override void UpdateData(ChartData newData)
        {
            base.UpdateData(newData);

            // 更新饼图数据
            if (Chart != null)
            {
                Chart.Series = CreateSeries(newData).ToArray();
            }
        }


        private SKColor GetColorForIndex(int index)
        {
            // 使用预设颜色轮转
            var colors = new[]
            {
                SKColor.Parse("#4285F4"), // Blue
                SKColor.Parse("#EA4335"), // Red
                SKColor.Parse("#FBBC05"), // Yellow
                SKColor.Parse("#34A853"), // Green
                SKColor.Parse("#673AB7")  // Purple
            };
            return colors[index % colors.Length];
        }

        public override IChartView BuildChartControl(ChartData data)
        {
            throw new NotImplementedException();
        }
    }

    // 扩展方法
    public static class PieChartExtensions
    {
        public static ChartControl WithDonutOptions(this ChartControl chart, bool isDonut)
        {
            if (isDonut && chart.ChartView is PieChart pieChart)
            {
                foreach (var series in pieChart.Series.Cast<PieSeries<double>>())
                {
                    series.InnerRadius = 60; // 设置内半径创建环形图
                    series.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Outer;
                }
            }
            return chart;
        }

        public static ChartControl WithPieContextMenu(this ChartControl chart)
        {
            chart.AddMenuItem("高亮扇区", ds =>
            {
                // 实现扇区高亮逻辑
            });
            chart.AddMenuItem("分离扇区", ds =>
            {
                // 实现扇区分离效果
            });
            return chart;
        }
    }
}

