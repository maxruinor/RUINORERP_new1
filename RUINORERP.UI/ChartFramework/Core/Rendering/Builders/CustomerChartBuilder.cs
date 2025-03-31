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
using NPOI.POIFS.Crypt.Dsig;

namespace RUINORERP.UI.ChartFramework.Core.Rendering.Builders
{
    public class CustomerChartBuilder : ChartBuilderBase<CartesianChart>
    {
        public CustomerChartBuilder(DataRequest request, IDataProvider dataSource) : base(request, dataSource) { }
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

        public async override Task<ChartControl> BuildChart(DataRequest request)
        {
            var data = await _dataSource.GetDataAsync(request);
            var cartesianChart = new CartesianChart
            {
                Series = data.Series.Select(series => new LineSeries<double>
                {
                    Name = series.Name,
                    Values = series.Values.ToArray(),
                    Stroke = new SolidColorPaint(series.ColorHex.ToSKColor(), 2),
                    Fill = null,
                    GeometrySize = 8,
                    //GeometryStroke = new SolidColorPaint(series.ColorHex.ToSKColor(), 2),
                   // GeometryFill = new SolidColorPaint(SKColors.White)
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

            return new ChartControl(cartesianChart, data);
        }

        public async override Task<IChartView> BuildChartControl( )
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
                    //GeometryStroke = new SolidColorPaint(series.ColorHex.ToSKColor(), 2),
                    //GeometryFill = new SolidColorPaint(SKColors.White)
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
    }
}
