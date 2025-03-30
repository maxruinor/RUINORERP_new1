using LiveChartsCore.Kernel.Sketches;
using RUINORERP.UI.ChartFramework.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.Extensions.Theming
{
    // Extensions/Theming/ThemeEngine.cs
    public static class ThemeEngine
    {
        private static readonly Dictionary<string, SKColor[]> _palettes = new()
        {
            ["Default"] = new[] { "#4285F4", "#EA4335", "#FBBC05", "#34A853" },
            ["Pastel"] = new[] { "#A7C7E7", "#FFD1DC", "#C1E1C1", "#FDFD96" }
        };

        public static void ApplyTheme(this IChartView chart, ChartMetaData meta)
        {
            if (_palettes.TryGetValue(meta.ColorPalette, out var colors))
            {
                chart.Series.ForEach((series, index) =>
                    series.SetColor(colors[index % colors.Length]));
            }
        }
    }
}
