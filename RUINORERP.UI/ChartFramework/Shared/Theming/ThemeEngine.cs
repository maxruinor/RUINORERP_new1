using LiveChartsCore.Kernel.Sketches;
using RUINORERP.Model.ChartFramework.Models;
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
            ["Default"] = new[] { 
                SKColor.Parse("#4285F4"), 
                SKColor.Parse("#EA4335"), 
                SKColor.Parse("#FBBC05"), 
                SKColor.Parse("#34A853") 
            },
            ["Pastel"] = new[] { 
                SKColor.Parse("#A7C7E7"), 
                SKColor.Parse("#FFD1DC"), 
                SKColor.Parse("#C1E1C1"), 
                SKColor.Parse("#FDFD96") 
            }
        };

        public static void ApplyTheme(this IChartView chart, ChartMetaData meta)
        {
            // TODO: 实现主题应用逻辑
            // LiveChartsCore 的 IChartView 没有 Series 属性，需要在图表级别处理
        }
    }
}

