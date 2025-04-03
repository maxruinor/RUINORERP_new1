using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using global::RUINORERP.UI.ChartFramework.Models;
using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.SkiaSharpView.WinForms;
using NPOI.POIFS.Crypt.Dsig;
using RUINORERP.UI.ChartFramework.Core.Contracts;
using RUINORERP.UI.ChartFramework.Core.Rendering.Builders;
using RUINORERP.UI.ChartFramework.Rendering.Controls;
using SkiaSharp;


namespace RUINORERP.UI.ChartFramework.Core.Rendering.Builders
{
    public class PieChartBuilder : ChartBuilderBase<CartesianChart>
    {
        public PieChartBuilder(DataRequest request, IDataProvider dataSource) : base(request, dataSource) { }

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


        public async  Task<ChartControl> BuildChart11(DataRequest request)
        {
            var data = await _dataSource.GetDataAsync(request);
            if (data?.Series == null || !data.Series.Any() ||
            data.Series[0].Values.Count != data.CategoryLabels.Length)
            {
                throw new ArgumentException("图表数据不完整");
            }
            // 创建饼图实例
            var pieChart = new PieChart();

            var Series = data.Series.Select(series => new PieSeries<double>
            {
                Name = series.Name,
                Values = series.Values.ToArray(),
                Fill = new SolidColorPaint(series.ColorHex.ToSKColor()),
                Stroke = null,
                DataLabelsPaint = new SolidColorPaint(SKColors.White),
                DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                DataLabelsFormatter = point => $"{point.StackedValue} ({point.StackedValue.Share:P1})",
                InnerRadius = data.PieOptions?.InnerRadius ?? 0,
                //CornerRadius = 0.9,
                Pushout = 5
            }).ToArray();

            pieChart.Series = Series;

            pieChart.Title = new LabelVisual
            {
                Text = data.Title,
                TextSize = 16,
                Padding = new LiveChartsCore.Drawing.Padding(15),
                Paint = new SolidColorPaint(SKColors.DarkSlateGray)
            };

            //Subtitle = string.IsNullOrEmpty(data.SubTitle) ? null : new LabelVisual
            //{
            //    Text = data.SubTitle,
            //    TextSize = 12,
            //    Padding = new LiveChartsCore.Drawing.Padding(0, 0, 0, 5),
            //    Paint = new SolidColorPaint(SKColors.Gray)
            //},

            pieChart.LegendPosition = LiveChartsCore.Measure.LegendPosition.Right;

            // 饼图专用配置
            pieChart.InitialRotation = data.PieOptions?.StartAngle ?? 0;
            pieChart.MaxAngle = 360;


            //// 添加注解（VisualElements）
            //foreach (var annotation in data.Annotations)
            //{
            //    pieChart.VisualElements.Add(new LabelVisual
            //    {
            //        Text = annotation.Text,
            //        X = GetXPosition(annotation.Position),
            //        Y = GetYPosition(annotation.Position),
            //        TextSize = annotation.FontSize,
            //        Paint = new SolidColorPaint(annotation.ColorHex.ToSKColor()),
            //        HorizontalAlignment = annotation.Position == AnnotationPosition.Center ? Align.Center : Align.Start,
            //        VerticalAlignment = annotation.Position == AnnotationPosition.Center ? Align.Middle : Align.Start
            //    });
            //}
            List<LabelVisual> labelVisuals = new List<LabelVisual>();

            foreach (var annotation in data.Annotations)
            {
                labelVisuals.Add(new LabelVisual
                {

                    Text = annotation.Text,
                    X = GetXPosition(annotation.Position),
                    Y = GetYPosition(annotation.Position),
                    TextSize = annotation.FontSize,
                    Paint = new SolidColorPaint(annotation.ColorHex.ToSKColor()),
                    HorizontalAlignment = annotation.Position == AnnotationPosition.Center ? Align.Middle : Align.Start,
                    VerticalAlignment = annotation.Position == AnnotationPosition.Center ? Align.Middle : Align.Start
                });
            }

            pieChart.VisualElements = labelVisuals;

            // 绑定交互事件
            BindChartEvents(pieChart as IChartView);

            return new ChartControl(pieChart, data);

        }


        public async override Task<ChartControl> BuildChart(DataRequest request)
        {
            var data = await _dataSource.GetDataAsync(request);

            var pieChart = new PieChart
            {
                Series = data.Series.Select(series => new PieSeries<double>
                {
                    Name = series.Name,
                    Values = series.Values,
                  //  Fill = new SolidColorPaint(series.ColorHex.ToSKColor()),
                    Stroke = null,
                    DataLabelsPaint = new SolidColorPaint(SKColors.Black),
                    DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Outer,
                    // DataLabelsFormatter = point =>                        $"{data.CategoryLabels[point.Context.Index]}\n" +                        $"{point.PrimaryValue} ({point.StackedValue.Share:P0})",
                    InnerRadius = data.PieOptions?.InnerRadius ?? 0,
                    Pushout = 2,
                    HoverPushout = 8
                }).ToArray(),

                Title = new LabelVisual
                {
                    Text = data.Title,
                    TextSize = 16,
                    Padding = new LiveChartsCore.Drawing.Padding(15),
                    Paint = new SolidColorPaint(SKColors.DarkSlateGray)
                },

                LegendPosition = LiveChartsCore.Measure.LegendPosition.Right,
                InitialRotation = data.PieOptions?.StartAngle ?? 0,
                MaxAngle = 360
            };

            //// 添加中心总数值
            //if (data.Annotations?.Any(a => a.Position == AnnotationPosition.Center) ?? false)
            //{
            //    pieChart.VisualElements.Add(new LabelVisual
            //    {
            //        Text = data.Annotations
            //            .First(a => a.Position == AnnotationPosition.Center).Text,
            //        TextSize = 14,
            //        Paint = new SolidColorPaint(SKColors.DarkSlateGray),
            //        X = 0.5,
            //        Y = 0.5,
            //        HorizontalAlignment = Align.Middle,
            //        VerticalAlignment = Align.Middle
            //    });
            //}

            // 绑定交互事件
            BindChartEvents(pieChart);

            return new ChartControl(pieChart, data);
        }

        // 位置转换辅助方法
        private static double GetXPosition(AnnotationPosition position) => position switch
        {
            AnnotationPosition.Left => 0.1,
            AnnotationPosition.Right => 0.9,
            AnnotationPosition.TopLeft => 0.1,
            AnnotationPosition.TopRight => 0.9,
            AnnotationPosition.BottomLeft => 0.1,
            AnnotationPosition.BottomRight => 0.9,
            _ => 0.5 // Center/Top/Bottom
        };



        private static double GetYPosition(AnnotationPosition position) => position switch
        {
            AnnotationPosition.Top => 0.1,
            AnnotationPosition.Bottom => 0.9,
            AnnotationPosition.TopLeft => 0.1,
            AnnotationPosition.TopRight => 0.1,
            AnnotationPosition.BottomLeft => 0.9,
            AnnotationPosition.BottomRight => 0.9,
            _ => 0.5 // Center/Left/Right
        };

        private string FormatCenterLabel(ChartData data)
        {
            return data.PieOptions.CenterLabelFormat
                .Replace("{Total}", data.Series.Sum(s => s.Values.Sum()).ToString())
                .Replace("{Percent}", data.Series[0].Values.Average().ToString("F1"));
        }

        // 位置转换方法示例



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

            return new ChartControl(pieChart, data);
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
            return data.ValueType switch
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

        public override Task<IChartView> BuildChartControl()
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

