using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveChartsCore.Drawing;
using RUINORERP.UI.ChartFramework.Models;
using RUINORERP.UI.ChartFramework.Core.Contracts;
using RUINORERP.UI.ChartFramework.Core.Rendering.Builders;
using LiveChartsCore.SkiaSharpView.WinForms;
using LiveChartsCore.Kernel.Sketches;
using RUINORERP.UI.ChartFramework.Rendering.Controls;

namespace RUINORERP.UI.ChartFramework.Core.Rendering.Builders
{
    public class CustomerChartBuilder : ChartBuilderBase<CartesianChart>
    {
        public CustomerChartBuilder(IDataProvider dataSource) : base(dataSource)
        {
        }

        // 覆盖X轴创建方法...
        protected override Axis[] CreateXAxes(ChartData data)
        {
            var baseAxes = base.CreateXAxes(data);
            baseAxes[0].Name = "区域分布";
            return baseAxes;
        }

        // 覆盖Y轴格式化方法...
        protected override string FormatYValue(double value)
        {
            return $"{base.FormatYValue(value)} 家"; // 添加单位
        }

        // 实现抽象方法...
        protected override IEnumerable<ISeries> CreateSeries(ChartData data)
        {  // 这里需要确保data中有正确的数据
            // 假设data有一个名为"Count"的系列数据
            var values = data.Series.FirstOrDefault()?.Values ?? new List<double>();
            yield return new ColumnSeries<double>
            {
                Name = "客户数量",
                Values = values,
                Fill = new SolidColorPaint(SKColor.Parse("#4CAF50")),
                XToolTipLabelFormatter = point => $"{point.StackedValue:N0} 家客户"
            };
        }

        public CartesianChart BuildChart1(ChartData data)
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

            return cartesianChart;
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
