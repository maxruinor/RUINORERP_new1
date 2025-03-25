using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartAnalyzer
{
    /// <summary>
    /// 销售订单图表构建器
    /// </summary>
    public class SalesChartBuilder : ChartBuilderBase
    {
        public SalesChartBuilder(IChartDataSource dataSource)
            : base(dataSource)
        {
        }

        protected override IEnumerable<ISeries> CreateSeries(ChartDataSet data)
        {
            var series = new List<ISeries>();

            foreach (var metric in data.SeriesData.Keys)
            {
                series.Add(CreateSeriesByMetric(metric, data.SeriesData[metric]));
            }

            return series;
        }

        private ISeries CreateSeriesByMetric(string metric, double[] values)
        {
            return metric switch
            {
                "Amount" => new LineSeries<double>
                {
                    Name = "销售额",
                    Values = values,
                    Stroke = new SolidColorPaint(SKColor.Parse("#4CAF50"), 3),
                    Fill = null,
                    GeometryStroke = null,
                    TooltipLabelFormatter = point => $"{point.PrimaryValue:C2}"
                },
                "Quantity" => new ColumnSeries<double>
                {
                    Name = "销售数量",
                    Values = values,
                    Fill = new SolidColorPaint(SKColor.Parse("#FF9800")),
                    TooltipLabelFormatter = point => $"{point.PrimaryValue:N0} 件"
                },
                _ => new ColumnSeries<double>
                {
                    Name = "订单数",
                    Values = values,
                    Fill = new SolidColorPaint(SKColor.Parse("#2196F3")),
                    TooltipLabelFormatter = point => $"{point.PrimaryValue:N0} 单"
                }
            };
        }

        protected override Axis[] CreateYAxes(ChartDataSet data)
        {
            return new[] {
            new Axis { Labeler = value => value.ToString("N0") }, // 左轴数量
            new Axis {
                Labeler = value => value.ToString("C2"),
                Position = LiveChartsCore.Measure.AxisPosition.End // 右轴金额
            }
        };
        }
    }
}
