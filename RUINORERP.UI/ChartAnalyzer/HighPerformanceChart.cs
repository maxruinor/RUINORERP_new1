using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.SkiaSharpView.WinForms;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartAnalyzer
{
    public class HighPerformanceChart : CartesianChart
    {
        public HighPerformanceChart()
        {
            // 关闭非必要功能
            AnimationsSpeed = TimeSpan.Zero;
            EasingFunction = null;
            DrawMarginFrame = null;

            // 简化渲染
            Series = new ObservableCollection<ISeries>();
            VisualElements = new List<ChartElement<SkiaSharpDrawingContext>>();
        }
    }
}
