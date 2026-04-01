using RUINORERP.Model.ChartFramework.Models;
using RUINORERP.UI.ChartFramework.Adapters;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RUINORERP.Common.Extensions;

namespace RUINORERP.UI.ChartFramework.DataProviders.Adapters.Inventory
{
    /// <summary>
    /// 库存周转适配器
    /// </summary>
    public class InventoryTurnoverAdapter : SqlDataProviderBase
    {
        public InventoryTurnoverAdapter() { }
        
        public InventoryTurnoverAdapter(ISqlSugarClient db) : base(db) { }

        protected override string PrimaryTableName => "tb_Inventory";

        public override IEnumerable<DimensionConfig> GetDimensions() => new[]
        {
            new DimensionConfig("Product_ID", "产品", DimensionType.String),
            new DimensionConfig("Warehouse_ID", "仓库", DimensionType.String),
            new DimensionConfig("StockDate", "入库日期", DimensionType.DateTime)
        };

        public override IEnumerable<MetricConfig> GetMetrics() => new[]
        {
            new MetricConfig("TurnoverDays", "周转天数", MetricType.Avg, MetricUnit.Quantity),
            new MetricConfig("StockQuantity", "库存数量", MetricType.Sum, MetricUnit.Quantity)
        };

        public async override Task<ChartData> GetDataAsync(DataRequest request)
        {
            // TODO: 需要定义 tb_Inventory 实体
            throw new NotImplementedException("需要先定义 tb_Inventory 实体类");
        }

        protected override ChartData TransformToChartData(List<dynamic> rawData, DataRequest request)
        {
            var chartData = new ChartData
            {
                Title = "库存周转分析",
                ChartType = ChartType.Column,
                ValueType = RUINORERP.Model.ChartFramework.Models.ValueType.Days
            };

            chartData.CategoryLabels = rawData
                .Select(x => ((int)x.TimeGroup).ToString() + "月")
                .Distinct()
                .OrderBy(x => x)
                .ToArray();

            var seriesGroups = rawData.GroupBy(x => x.ProductId);
            foreach (var group in seriesGroups)
            {
                var series = new DataSeries
                {
                    Name = GetProductName(group.Key),
                    Values = group.Select(x => (double)x.TurnoverDays).ToList()
                };
                chartData.Series.Add(series);
            }

            return chartData;
        }

        private string GetProductName(object productId)
        {
            // TODO: 需要定义 tb_Product_Info 实体
            return productId?.ToString() ?? "未知";
        }
    }
}


