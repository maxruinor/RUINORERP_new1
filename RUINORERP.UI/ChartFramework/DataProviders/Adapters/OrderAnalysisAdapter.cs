using RUINORERP.Model;
using RUINORERP.Model.ChartFramework.Models;
using RUINORERP.UI.ChartFramework.Adapters;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.DataProviders.Adapters.Sales
{
    /// <summary>
    /// 订单分析适配器
    /// </summary>
    public class OrderAnalysisAdapter : SqlDataProviderBase
    {
        public OrderAnalysisAdapter() { }

        public OrderAnalysisAdapter(ISqlSugarClient db) : base(db) { }

        protected override string PrimaryTableName => "tb_SaleOrder";

        public override IEnumerable<DimensionConfig> GetDimensions() => new[]
        {
            new DimensionConfig("Customer_ID", "客户", DimensionType.String),
            new DimensionConfig("Employee_ID", "业务员", DimensionType.String),
            new DimensionConfig("OrderDate", "订单日期", DimensionType.DateTime)
        };

        public override IEnumerable<MetricConfig> GetMetrics() => new[]
        {
            new MetricConfig("Amount", "订单金额", MetricType.Sum, MetricUnit.Amount),
            new MetricConfig("Quantity", "订单数量", MetricType.Sum, MetricUnit.Quantity),
            new MetricConfig("Count", "订单数", MetricType.Count, MetricUnit.Count)
        };

        public async override Task<ChartData> GetDataAsync(DataRequest request)
        {
            // TODO: 需要修复动态操作问题
            var query = _db.Queryable<tb_SaleOrder>();

            // 暂时简化处理，不分组
            var result = await query
                .Select(g => new
                {
                    TimeGroup = SqlFunc.DateValue(g.SaleDate, DateType.Month),
                    CustomerId = g.CustomerVendor_ID,
                    Amount = SqlFunc.AggregateSum(g.TotalAmount),
                    Count = SqlFunc.AggregateCount(1)
                })
                .ToListAsync();

            return TransformToChartData(result.Cast<dynamic>().ToList(), request);
        }

        protected override ChartData TransformToChartData(List<dynamic> rawData, DataRequest request)
        {
            var chartData = new ChartData
            {
                Title = "订单分析",
                ChartType = ChartType.Column,
                ValueType = RUINORERP.Model.ChartFramework.Models.ValueType.Currency
            };

            chartData.CategoryLabels = rawData
                .Select(x => ((int)x.TimeGroup).ToString() + "月")
                .Distinct()
                .OrderBy(x => x)
                .ToArray();

            var seriesGroups = rawData.GroupBy(x => x.CustomerId);
            foreach (var group in seriesGroups)
            {
                var series = new DataSeries
                {
                    Name = GetCustomerName(group.Key),
                    Values = group.Select(x => (double)x.Amount).ToList()
                };
                chartData.Series.Add(series);
            }

            return chartData;
        }

        private string GetCustomerName(object customerId)
        {
            // TODO: 需要定义 tb_CRM_Customer 实体
            return customerId?.ToString() ?? "未知";
        }
    }
}

