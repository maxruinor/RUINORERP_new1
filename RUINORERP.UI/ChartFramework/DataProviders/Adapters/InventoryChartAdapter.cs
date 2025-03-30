using RUINORERP.UI.ChartFramework.Core;
using RUINORERP.UI.ChartFramework.Data.Abstract.ChartFramework.Data.Abstract;
using RUINORERP.UI.ChartFramework.Models;
using RUINORERP.UI.ChartFramework.Models.ChartFramework.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.DataProviders.Adapters
{
    // Adapters/Inventory/InventoryChartAdapter.cs

    /// <summary>
    /// 库存分析适配器
    /// </summary>
    public class InventoryChartAdapter : ChartDataSourceBase
    {
        protected override ChartDataSet TransformData(List<dynamic> rawData, ChartRequest request)
        {
            return new ChartDataSet
            {
                Title = "库存周转分析",
                MetaData = new ChartMetaData
                {
                    SuggestedChartType = ChartType.Column,
                    ValueType = ValueType.Days
                },
                Series = {
                new ChartSeries
                {
                    Name = "平均周转天数",
                    Values = rawData.Select(x => (double)x.TurnoverDays).ToList(),
                    DataLabels = rawData.Select(x => x.ProductName.ToString()).ToList()
                }
            }
            };
        }

        public override async Task<ChartDataSet> GetDataAsync(ChartRequest request)
        {
            var data = await _db.Queryable<InventoryRecord>()
                .GroupBy(request.GetGroupByFields())
                .Select<InventorySummary>()
                .ToListAsync();

            return new InventoryDataTransformer().Transform(data);
        }
    }

    public class InventorySummary
    {
        public string ProductCode { get; set; }
        public double AvgTurnoverDays { get; set; }
        public int StockCount { get; set; }
    }
}

