using LiveChartsCore.SkiaSharpView;
using RUINORERP.UI.ChartFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RUINORERP.UI.ChartFramework.Models.ChartMetaData;

namespace RUINORERP.UI.ChartFramework.Rendering
{
    // Rendering/AxisBuilder.cs
    public static class AxisBuilder
    {
        public static Axis[] BuildAxes(ChartMetaData meta)
        {
            var axis = new Axis
            {
                Labels = meta.CategoryLabels,
                LabelsRotation = CalculateRotation(meta.CategoryLabels),
                Labeler = GetLabelFormatter(meta.InferredLabelType)
            };

            return new[] { axis };
        }

        private static double CalculateRotation(string[] labels)
        {
            if (labels.Length > 15)
                return 45;
            if (labels.Any(l => l?.Length > 10))
                return 30;
            return 0;
        }

        private static Func<double, string> GetLabelFormatter(LabelType type)
        {
            return type switch
            {
                LabelType.DateTime => val => new DateTime((long)val).ToString("d"),
                LabelType.Numeric => val => val.ToString("N0"),
                _ => val => val.ToString()
            };
        }
    }
}
