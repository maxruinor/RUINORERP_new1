using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.WinForms;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using RUINORERP.UI.ChartAnalyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.UI.ChartFramework.Models;
using RUINORERP.Common.Extensions;
using RUINORERP.UI.ChartFramework.Extensions.Theming;
using SkiaSharp;
using RUINORERP.UI.ChartFramework.Core.Contracts;
using RUINORERP.UI.ChartFramework.Core.Rendering.Builders;
using RUINORERP.UI.ChartFramework.Rendering.Controls;
using RUINORERP.UI.ChartFramework.Core.Models;
using LiveChartsCore.SkiaSharpView.VisualElements;
using RUINORERP.UI.ChartFramework.DataProviders.Adapters;
using NPOI.POIFS.NIO;

namespace RUINORERP.UI.ChartFramework.Core.Rendering.Builders
{
    // 折线图构建器示例
    public class LineChartBuilder : ChartBuilderBase<CartesianChart>
    {
        public LineChartBuilder(DataRequest request, IDataProvider dataSource) : base(request, dataSource) { }

        private ISeries CreateSeries(DataSeries series)
        {
            return new LineSeries<double>
            {
                Name = series.Name,
                Values = series.Values.ToArray(),
                Fill = new SolidColorPaint(SKColor.Parse("#4CAF50")),
            };
        }
        protected override IEnumerable<ISeries> CreateSeries(ChartData data)
        {
            return data.Series.Select(series => new LineSeries<double>
            {
                Name = series.Name,
                Values = series.Values.ToArray(),
                GeometryStroke = null,
                GeometryFill = null,
                LineSmoothness = 0.8
            }).ToArray();
        }



        private static string FormatLabel(double value)
        {
            return value switch
            {
                >= 1000000 => $"{value / 1000000:F1}M",
                >= 1000 => $"{value / 1000:F1}K",
                _ => value.ToString("N0")
            };
        }


        public async override Task<IChartView> BuildChartControl()
        {
            var data = await _dataSource.GetDataAsync(_currentRequest);
            var cartesianChart = new CartesianChart
            {
                Series = data.Series.Select(series => new LineSeries<double>
                {
                    Name = series.Name,
                    Values = series.Values.ToArray(),
                    Stroke = new SolidColorPaint(series.ColorHex.ToSKColor(), 2),
                    Fill = null,
                    GeometrySize = 8,
                    GeometryStroke = new SolidColorPaint(series.ColorHex.ToSKColor(), 2),
                    GeometryFill = new SolidColorPaint(SKColors.White)
                }).ToArray(),

                XAxes = new[] { new Axis { Labels = data.CategoryLabels } },
                YAxes = new[] { new Axis() },

                Title = new LabelVisual
                {
                    Text = data.Title,
                    TextSize = 16,
                    Padding = new LiveChartsCore.Drawing.Padding(15),
                    Paint = new SolidColorPaint(SKColors.DarkSlateGray)
                },

                LegendPosition = LiveChartsCore.Measure.LegendPosition.Right
            };

            return cartesianChart;
        }

        public async override  Task<ChartControl> BuildChart(DataRequest request)
        {
            var data = await _dataSource.GetDataAsync(_currentRequest);
            var cartesianChart = new CartesianChart
            {
                Series = data.Series.Select(series => new LineSeries<double>
                {
                    Name = series.Name,
                    Values = series.Values.ToArray(),
                    Stroke = new SolidColorPaint(series.ColorHex.ToSKColor(), 2),
                    Fill = null,
                    GeometrySize = 8,
                    GeometryStroke = new SolidColorPaint(series.ColorHex.ToSKColor(), 2),
                    GeometryFill = new SolidColorPaint(SKColors.White)
                }).ToArray(),

                XAxes = new[] { new Axis { Labels = data.CategoryLabels } },
                YAxes = new[] { new Axis() },

                Title = new LabelVisual
                {
                    Text = data.Title,
                    TextSize = 16,
                    Padding = new LiveChartsCore.Drawing.Padding(15),
                    Paint = new SolidColorPaint(SKColors.DarkSlateGray)
                },

                LegendPosition = LiveChartsCore.Measure.LegendPosition.Right
            };

            // 绑定交互事件
            BindChartEvents(cartesianChart);
            return new ChartControl(cartesianChart, data);
        }
    }
}

