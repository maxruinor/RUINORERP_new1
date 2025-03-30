using LiveChartsCore.Kernel.Sketches;
using NPOI.SS.UserModel.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.Core.Rendering
{
    // Core/Rendering/RenderingPipeline.cs
    public class RenderingPipeline
    {
        private readonly List<IRenderingMiddleware> _middlewares = new();

        public RenderingPipeline AddMiddleware(IRenderingMiddleware middleware)
        {
            _middlewares.Add(middleware);
            return this;
        }

        public Task RenderAsync(IChartView chart, ChartData data)
        {
            var context = new RenderingContext(chart, data);

            foreach (var middleware in _middlewares)
            {
                middleware.Invoke(context);
                if (context.IsAborted) break;
            }

            return Task.CompletedTask;
        }
    }

    public interface IRenderingMiddleware
    {
        void Invoke(RenderingContext context);
    }
}
