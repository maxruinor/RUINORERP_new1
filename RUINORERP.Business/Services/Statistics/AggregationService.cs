using RUINORERP.Model;
using RUINORERP.Model.ChartFramework.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.Services.Statistics
{
    /// <summary>
    /// 通用聚合服务实现
    /// </summary>
    public class AggregationService : IStatisticsService
    {
        private readonly ISqlSugarClient _db;
        private readonly IReportCacheService _cache;

        public AggregationService(ISqlSugarClient db, IReportCacheService cache)
        {
            _db = db;
            _cache = cache;
        }

        public async Task<ChartData> ExecuteQueryAsync(StatisticsQuery query)
        {
            // 1. 验证参数
            if (!query.Validate(out var errorMsg))
                throw new ArgumentException(errorMsg);

            // 2. 检查缓存
            var cacheKey = BuildCacheKey(query);
            if (_cache.TryGet<ChartData>(cacheKey, out var cachedData))
                return cachedData;

            try
            {
                // 3. 动态构建 SQL
                var sqlBuilder = new DynamicSqlBuilder(_db);
                var selectable = sqlBuilder
                    .From(query.EntityType)
                    .SelectDimensions(query.Dimensions)
                    .SelectMetrics(query.Metrics)
                    .WhereTimeRange(query.TimeField, query.StartTime, query.EndTime)
                    .ApplyFilters(query.Filters)
                    .GroupByDimensions(query.Dimensions)
                    .Page(query.PageIndex, query.PageSize)
                    .Build();

                // 4. 执行查询
                var rawData = await selectable.ToListAsync();

                // 5. 转换为 ChartData
                var chartData = TransformToChartData(rawData, query);

                // 6. 写入缓存 (TTL=30 分钟)
                await _cache.SetAsync(cacheKey, chartData, TimeSpan.FromMinutes(30));

                return chartData;
            }
            catch (Exception ex)
            {
                throw new Exception($"统计查询执行失败：{ex.Message}", ex);
            }
        }

        public IEnumerable<DimensionInfo> GetDimensions(string entityType)
        {
            // TODO: 从数据库元数据或配置中获取维度信息
            // 这里返回示例数据
            return new List<DimensionInfo>
            {
                new DimensionInfo { FieldName = "Created_at", DisplayName = "创建时间", FieldType = typeof(DateTime) },
                new DimensionInfo { FieldName = "Employee_ID", DisplayName = "业务员", FieldType = typeof(long) },
                new DimensionInfo { FieldName = "Customer_ID", DisplayName = "客户", FieldType = typeof(long) }
            };
        }

        public IEnumerable<MetricInfo> GetMetrics(string entityType)
        {
            // TODO: 从数据库元数据或配置中获取指标信息
            // 这里返回示例数据
            return new List<MetricInfo>
            {
                new MetricInfo { FieldName = "OrderID", DisplayName = "订单数", AggregationType = MetricType.Count, Unit = "笔" },
                new MetricInfo { FieldName = "TotalAmount", DisplayName = "金额", AggregationType = MetricType.Sum, Unit = "元" },
                new MetricInfo { FieldName = "Quantity", DisplayName = "数量", AggregationType = MetricType.Sum, Unit = "个" }
            };
        }

        /// <summary>
        /// 构建缓存键
        /// </summary>
        private string BuildCacheKey(StatisticsQuery query)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"Stats:{query.EntityType}");

            if (query.Dimensions.Any())
                keyBuilder.Append($":D={string.Join(",", query.Dimensions.OrderBy(d => d))}");

            if (query.Metrics.Any())
                keyBuilder.Append($":M={string.Join(",", query.Metrics.OrderBy(m => m))}");

            if (query.StartTime.HasValue)
                keyBuilder.Append($":ST={query.StartTime.Value:yyyyMMddHHmmss}");

            if (query.EndTime.HasValue)
                keyBuilder.Append($":ET={query.EndTime.Value:yyyyMMddHHmmss}");

            return keyBuilder.ToString();
        }

        /// <summary>
        /// 转换原始数据为 ChartData
        /// </summary>
        private ChartData TransformToChartData(List<dynamic> rawData, StatisticsQuery query)
        {
            var chartData = new ChartData
            {
                Title = $"{query.EntityType}统计分析",
                CategoryLabels = rawData.Select(x =>
                {
                    // 提取维度值作为标签
                    if (query.Dimensions.Any())
                    {
                        var dimValues = query.Dimensions.Select(d =>
                        {
                            var prop = x.GetType().GetProperty(d);
                            return prop?.GetValue(x)?.ToString() ?? "";
                        });
                        return string.Join("-", dimValues);
                    }
                    return "未知";
                }).ToArray(),
                Series = new List<DataSeries>()
            };

            // 添加系列数据
            foreach (var metric in query.Metrics)
            {
                var values = rawData.Select(x =>
                {
                    var prop = x.GetType().GetProperty(metric);
                    var value = prop?.GetValue(x);
                    return value != null ? Convert.ToDouble(value) : 0.0;
                }).Cast<double>().ToList();

                chartData.Series.Add(new DataSeries
                {
                    Name = metric,
                    Values = values
                });
            }

            return chartData;
        }
    }
}
