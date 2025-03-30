using LiveChartsCore.Kernel.Sketches;
using RUINORERP.UI.ChartFramework.Models.ChartFramework.Core.Models;
using RUINORERP.UI.ChartFramework.Models;
using SkiaSharp.Views.Desktop;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.Rendering
{
    // Rendering/GpuAcceleratedRenderer.cs
    public class GpuAcceleratedRenderer : IChartRenderer
    {
        private readonly SKGLControl _glControl;
        private GRContext _glContext;

        public GpuAcceleratedRenderer()
        {
            _glControl = new SKGLControl { VSync = true };
            _glContext = GRContext.CreateGl();
        }

        public void Render(IChartView chart, ChartDataSet data)
        {
            _glControl.PaintSurface += (sender, e) =>
            {
                var canvas = e.Surface.Canvas;
                using var paint = new SKPaint { IsAntialias = true };

                // 使用GPU加速绘制
                canvas.Clear(SKColors.Transparent);
                foreach (var series in data.Series)
                {
                    RenderSeries(_glContext, canvas, series, data.MetaData);
                }
            };
        }

        private void RenderSeries(GRContext context, SKCanvas canvas,
            ChartSeries series, ChartMetaData meta)
        {
            // 使用OpenGL着色器优化绘制
            using var shader = CreateOptimizedShader(series, meta);
            // ...GPU特定绘制逻辑
        }
    }
}
