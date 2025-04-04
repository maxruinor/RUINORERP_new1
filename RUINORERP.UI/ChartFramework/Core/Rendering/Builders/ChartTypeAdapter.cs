﻿using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.UI.ChartFramework.Core;
using RUINORERP.UI.ChartFramework.Core.Contracts;
using RUINORERP.UI.ChartFramework.Core.Models;

namespace RUINORERP.UI.ChartFramework.Core.Rendering.Builders
{
    public class ChartTypeAdapter
    {
        public ISeries Adapt(MetricConfig config, double[] values)
        {
            return config.ChartType switch
            {
                ChartType.Line => new LineSeries<double>
                {
                    Values = values,
                    Name = config.DisplayName,
                    Stroke = new SolidColorPaint(config.Color)
                },
               ChartType.Column => new ColumnSeries<double>
                {
                    Values = values,
                    Name = config.DisplayName,
                    Fill = new SolidColorPaint(config.Color)
                },
                ChartType.Pie => new PieSeries<double>
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
