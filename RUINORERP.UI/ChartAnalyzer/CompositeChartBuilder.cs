using LiveChartsCore.SkiaSharpView.WinForms;
using RUINORERP.Model.ChartFramework.Contracts;
using RUINORERP.Model.ChartFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartAnalyzer
{
    /// <summary>
    /// 复合图表构建器 (已废弃，建议使用新的架构)
    /// </summary>
    [Obsolete("请使用新的 ChartBuilderFactory 替代")]
    public class CompositeChartBuilder
    {
        private readonly List<IDataProvider> _dataProviders;

        public CompositeChartBuilder(params IDataProvider[] dataProviders)
        {
            _dataProviders = dataProviders.ToList();
        }

        public async Task<List<ChartData>> GetDataAsync(DataRequest request)
        {
            var results = new List<ChartData>();
            foreach (var provider in _dataProviders)
            {
                var data = await provider.GetDataAsync(request);
                results.Add(data);
            }
            return results;
        }
    }
}
