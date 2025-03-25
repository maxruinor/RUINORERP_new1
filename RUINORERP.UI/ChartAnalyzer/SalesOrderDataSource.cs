using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartAnalyzer
{
    /// <summary>
    /// 销售订单数据源（主子表结构）
    /// </summary>
    public class SalesOrderDataSource : IChartDataSource
    {
        private readonly ErpDbContext _context;

        public SalesOrderDataSource(ErpDbContext context)
        {
            _context = context;
        }

        public IEnumerable<DimensionConfig> GetDimensions()
        {
            return new List<DimensionConfig>
        {
            new DimensionConfig("SalesmanID", "业务员", DimensionType.String),
            new DimensionConfig("CustomerID", "客户", DimensionType.String),
            new DimensionConfig("OrderDate", "日期", DimensionType.DateTime),
            new DimensionConfig("ProductID", "产品", DimensionType.String)
        };
        }

        public IEnumerable<MetricConfig> GetMetrics()
        {
            return new List<MetricConfig>
        {
            new MetricConfig("Count", "订单数", MetricType.Count, "#2196F3"),
            new MetricConfig("Quantity", "销售数量", MetricType.Sum, "#FF9800"),
            new MetricConfig("Amount", "销售额", MetricType.Sum, "#4CAF50")
        };
        }

        public async Task<ChartDataSet> GetDataAsync(ChartRequest request)
        {
            var query = _context.Orders
                .Join(_context.OrderDetails,
                    o => o.OrderID,
                    d => d.OrderID,
                    (o, d) => new { Order = o, Detail = d });

            ApplyFilters(ref query, request.Filters);

            var groupQuery = query.GroupByDynamic(request.Dimensions);

            var result = await groupQuery
                .Select(g => new {
                    Keys = g.Keys,
                    Count = g.Count(),
                    Quantity = g.Sum(x => x.Detail.Quantity),
                    Amount = g.Sum(x => x.Detail.UnitPrice * x.Detail.Quantity)
                })
                .ToListAsync();

            return TransformToDataSet(result, request);
        }

        private void ApplyFilters(ref IQueryable<dynamic> query, IEnumerable<QueryFilter> filters)
        {
            foreach (var filter in filters)
            {
                query = filter.Field switch
                {
                    "OrderDate" => ApplyDateFilter(query, filter),
                    "ProductID" => query.Where(x => x.Detail.ProductID == filter.Value),
                    "SalesmanID" => query.Where(x => x.Order.SalesmanID == filter.Value),
                    _ => query
                };
            }
        }

        private ChartDataSet TransformToDataSet(List<dynamic> result, ChartRequest request)
        {
            var dataSet = new ChartDataSet();
            dataSet.Labels = result.Select(r => string.Join("-", r.Keys)).ToList();

            foreach (var metric in request.Metrics)
            {
                dataSet.SeriesData[metric] = result.Select(r => (double)GetProperty(r, metric)).ToArray();
            }

            return dataSet;
        }

        private object GetProperty(dynamic obj, string propertyName)
        {
            return propertyName switch
            {
                "Count" => obj.Count,
                "Quantity" => obj.Quantity,
                "Amount" => obj.Amount,
                _ => 0
            };
        }
    }
}
