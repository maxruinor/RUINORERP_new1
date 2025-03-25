using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartAnalyzer
{
    public class ChartTypeAdapter
    {
        public ISeries Adapt(MetricConfig config, double[] values)
        {
            return config.ChartType switch
            {
                "Line" => new LineSeries<double>
                {
                    Values = values,
                    Name = config.DisplayName,
                    Stroke = new SolidColorPaint(config.Color)
                },
                "Column" => new ColumnSeries<double>
                {
                    Values = values,
                    Name = config.DisplayName,
                    Fill = new SolidColorPaint(config.Color)
                },
                "Pie" => new PieSeries<double>
                {
                    Values = values,
                    Name = config.DisplayName,
                    Fill = new SolidColorPaint(config.Color)
                },
                _ => throw new NotSupportedException()
            };
        }
    }
}
