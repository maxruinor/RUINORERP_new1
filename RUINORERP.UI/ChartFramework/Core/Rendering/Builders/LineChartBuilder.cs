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

namespace RUINORERP.UI.ChartFramework.Core.Rendering.Builders
{
    // Rendering/Builders/LineChartBuilder.cs

    // 折线图构建器示例
    public class LineChartBuilder : ChartBuilderBase<CartesianChart>
    {
        public LineChartBuilder(IDataProvider dataSource) : base(dataSource) { }
        private ISeries CreateSeries(DataSeries series)
        {
            return new LineSeries<double>
            {
                Name = series.Name,
                Values = series.Values.ToArray(),
                Stroke = new SolidColorPaint(ChartHelper.HexToSKColor(series.Style.ColorHex), series.Style.StrokeWidth.ToLong()),
                Fill = new SolidColorPaint(SKColor.Parse("#4CAF50")),
            };
        }
        protected override IEnumerable<ISeries> CreateSeries(ChartData data)
        {
            return data.Series.Select(series => new LineSeries<double>
            {
                Name = series.Name,
                Values = series.Values.ToArray(),
                Stroke = new SolidColorPaint(series.Style.ColorHex.ToSKColor(), series.Style.StrokeWidth.ToLong()),
                Fill = new SolidColorPaint(series.Style.ColorHex.ToSKColor().WithAlpha((byte)(255 * series.Style.FillOpacity))),
                GeometryStroke = null,
                GeometryFill = null,
                LineSmoothness = 0.8
            }).ToArray();
        }

        public  ChartControl Build(ChartData data)
        {
            var chart = new CartesianChart
            {
                Series = data.Series.Select(CreateSeries).ToArray(),
                XAxes = new[] { new Axis { Labels = data.MetaData.PrimaryLabels } },
                YAxes = new[] { new Axis { Labeler = FormatLabel } }
            };

            return new ChartControl(chart, data);
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
     

        public override ChartControl BuildChart(ChartData data)
        {
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

                XAxes = new[] { new Axis { Labels = data.MetaData.CategoryLabels } },
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

            return new ChartControl(cartesianChart, data);

        }

        public override IChartView BuildChartControl(ChartData data)
        {
            throw new NotImplementedException();
        }
    }
}

