using LiveChartsCore.SkiaSharpView.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartAnalyzer
{
    public class CompositeChartBuilder : ChartBuilderBase
    {
        private readonly List<ChartBuilderBase> _builders;

        public CompositeChartBuilder(params ChartBuilderBase[] builders)
            : base(null) // 不直接使用数据源
        {
            _builders = builders.ToList();
        }

        public override async Task<CartesianChart> BuildChartAsync(ChartRequest request)
        {
            var chart = new CartesianChart();

            foreach (var builder in _builders)
            {
                var subChart = await builder.BuildChartAsync(request);
                MergeCharts(chart, subChart);
            }

            return chart;
        }

        private void MergeCharts(CartesianChart main, CartesianChart sub)
        {
            main.Series = main.Series.Concat(sub.Series);
            main.XAxes = main.XAxes.Concat(sub.XAxes);
            main.YAxes = main.YAxes.Concat(sub.YAxes);
        }
    }
}
