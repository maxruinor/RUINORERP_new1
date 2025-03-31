using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using RUINORERP.UI.ChartFramework.Core.Rendering.Builders;
using RUINORERP.UI.ChartFramework.Extensions.Theming;
using RUINORERP.UI.ChartFramework.Models;
using RUINORERP.UI.ChartFramework.Rendering.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.Core.Contracts
{
    public interface IChartBuilder<TChart> where TChart : IChartView
    {
        /// <summary>
        /// 内置数据源
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<ChartControl> BuildChart(DataRequest request);

        public Task<IChartView> BuildChartControl();
        public void ApplyTheme(ChartTheme theme);
        public void UpdateData(ChartData newData);

        public event EventHandler<ChartInteractionEventArgs> OnInteraction;
    }
}
