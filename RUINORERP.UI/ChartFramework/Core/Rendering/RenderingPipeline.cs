using LiveChartsCore.Kernel.Sketches;
using RUINORERP.Model.ChartFramework.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.Core.Rendering
{
    /// <summary>
    /// 渲染管线 (简化版)
    /// </summary>
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
            // 简化实现，暂不支持中间件
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// 渲染中间件接口
    /// </summary>
    public interface IRenderingMiddleware
    {
        void Render(IChartView chart, ChartData data);
    }
}

