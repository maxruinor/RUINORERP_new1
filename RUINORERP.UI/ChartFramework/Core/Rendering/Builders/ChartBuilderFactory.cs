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
using RUINORERP.UI.ChartFramework.Adapters;
using RUINORERP.UI.ChartFramework.Models;


namespace RUINORERP.UI.ChartFramework.Core.Rendering.Builders
{
    // 图表构建器工厂
    public static class ChartBuilderFactory
    {
        public static IChartBuilder<CartesianChart> CreateBuilder(DataRequest request, SqlDataProviderBase sqlDataProvider)
        {
            return request.ChartType switch
            {
                ChartType.Line => new LineChartBuilder(request,sqlDataProvider),
                ChartType.Column => new ColumnChartBuilder(request, sqlDataProvider),
                ChartType.Pie => new PieChartBuilder(request, sqlDataProvider),
                _ => throw new NotSupportedException($"不支持的图表类型: {request.ChartType}")
            };
        }
    }







}
