using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.Extensions.Theming
{
    // Extensions/Theming/ChartTheme.cs
 
        public class ChartTheme
        {
            public static ChartTheme DefaultLight => new()
            {
                Background = SKColors.White,
                AxisTextPaint = new SolidColorPaint(SKColors.Black, 12),
                SeriesPalette = new[]
                {
                SKColor.Parse("#4285F4"),
                SKColor.Parse("#EA4335"),
                SKColor.Parse("#FBBC05"),
                SKColor.Parse("#34A853")
            }
            };

            public SKColor Background { get; set; }
            public SolidColorPaint AxisTextPaint { get; set; }
            public SKColor[] SeriesPalette { get; set; }
        }
    }
 
