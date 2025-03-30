using RUINORERP.UI.ChartFramework.Adapters;
using RUINORERP.UI.ChartFramework.Core;
using RUINORERP.UI.ChartFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValueType = RUINORERP.UI.ChartFramework.Core.ValueType;

namespace RUINORERP.UI.ChartFramework.DataProviders.Adapters
{
    // Adapters/OrderAnalysisAdapter.cs
    public class OrderAnalysisAdapter : SqlDataProviderBase
    {
        public override async Task<ChartData> GetDataAsync(DataRequest request)
        {
            var query = BuildQuery(request);
            var rawData = await ExecuteQuery(query);

            return new ChartData
            {
                Title = "订单分析报告",
                MetaData = new ChartMetaData
                {
                    PreferredVisualType = DetectChartType(request),
                    ValueType = ValueType.Currency,
                    StackMode = request.Dimensions.Count > 1 ? StackMode.Normal : StackMode.None,
                    CategoryLabels = await GetCategoryLabels(query, request)
                },
                Series = await BuildSeries(query, request),
                RawRecords = rawData.Select(r => new DynamicRecord(r)).ToList()
            };
        }

        private ChartType DetectChartType(DataRequest request)
        {
            return request.Metrics.Count > 3 ? ChartType.Line : ChartType.Column;
        }
    }
}
