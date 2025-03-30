using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using RUINORERP.UI.ChartFramework.Core;
using RUINORERP.UI.ChartFramework.Core.Contracts;
using LiveChartsCore.SkiaSharpView.WinForms;


namespace RUINORERP.UI.ChartFramework.Core.Rendering.Builders
{
    // 图表构建器工厂
    public static class ChartBuilderFactory
    {
        public static IChartBuilder<CartesianChart> CreateBuilder(ChartType chartType, IDataProvider dataSource)
        {
            return chartType switch
            {
                ChartType.Line => new LineChartBuilder(dataSource),
                ChartType.Column => new ColumnChartBuilder(dataSource),
                ChartType.Pie => new PieChartBuilder(dataSource),
                _ => throw new NotSupportedException($"不支持的图表类型: {chartType}")
            };
        }
    }







}
