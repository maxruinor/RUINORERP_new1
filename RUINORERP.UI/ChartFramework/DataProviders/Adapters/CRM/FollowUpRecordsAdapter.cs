using RUINORERP.Model;
using RUINORERP.Model.ChartFramework.Models;
using RUINORERP.UI.ChartFramework.Adapters;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.DataProviders.Adapters.CRM
{
    /// <summary>
    /// CRM 跟进记录数据适配器
    /// </summary>
    public class FollowUpRecordsAdapter : SqlDataProviderBase
    {
        public FollowUpRecordsAdapter() { }
        
        public FollowUpRecordsAdapter(ISqlSugarClient db) : base(db) { }

        protected override string PrimaryTableName => "tb_CRM_FollowUpRecords";

        public override IEnumerable<DimensionConfig> GetDimensions() => new[]
        {
            new DimensionConfig("Employee_ID", "业务员", DimensionType.String),
            new DimensionConfig("Customer_ID", "客户", DimensionType.String),
            new DimensionConfig("FollowUpType", "跟进类型", DimensionType.String),
            new DimensionConfig("FollowUpDate", "跟进日期", DimensionType.DateTime)
        };

        public override IEnumerable<MetricConfig> GetMetrics() => new[]
        {
            new MetricConfig("Count", "跟进次数", MetricType.Count, MetricUnit.Count),
            new MetricConfig("Duration", "平均时长", MetricType.Avg, MetricUnit.Quantity)
        };

        public async override Task<ChartData> GetDataAsync(DataRequest request)
        {
            var query = _db.Queryable<tb_CRM_FollowUpRecords>()
                .WhereIF(request.Employee_ID.HasValue, c => c.Employee_ID == request.Employee_ID.Value)
                .Where(c => c.FollowUpDate >= request.StartTime && c.FollowUpDate <= request.EndTime);

            // 构建分组字段 - 暂时使用固定字段
            var groupFields = new List<string>();
            groupFields.Add("FollowUpDate"); // SqlTimeGroupBuilder.GetGroupByTimeField(request.TimeField, request.RangeType));
            groupFields.Add("FollowUpType");

            var groupByModels = groupFields.Select(f => new GroupByModel { FieldName = f }).ToList();

            var result = await query
                .GroupBy(groupByModels)
                .Select(g => (dynamic)new
                {
                    TimeGroup = SqlFunc.DateValue(g.FollowUpDate, DateType.Month), // g.FollowUpDate.Value → g.FollowUpDate
                    FollowUpType = "未知", // g.FollowUpType ?? "未知",
                    Count = SqlFunc.AggregateCount(1),
                    Duration = 0 // SqlFunc.AggregateSum(g.DurationMinutes ?? 0) / SqlFunc.AggregateCount(1)
                })
                .ToListAsync();

            return TransformToChartData(result, request);
        }

        protected override ChartData TransformToChartData(List<dynamic> rawData, DataRequest request)
        {
            var chartData = new ChartData
            {
                Title = "跟进记录统计",
                ChartType = ChartType.Column,
                ValueType = RUINORERP.Model.ChartFramework.Models.ValueType.Absolute
            };

            // 处理时间维度作为 X 轴标签
            chartData.CategoryLabels = rawData
                .Select(x => ((int)x.TimeGroup).ToString() + "月")
                .Distinct()
                .OrderBy(x => x)
                .ToArray();

            // 按跟进类型分组创建系列
            if (request.Dimensions.Any(d => d.FieldName == "FollowUpType"))
            {
                var seriesGroups = rawData.GroupBy(x => x.FollowUpType);
                foreach (var group in seriesGroups)
                {
                    var series = new DataSeries
                    {
                        Name = group.Key?.ToString() ?? "未知",
                        Values = new List<double>()
                    };

                    foreach (var label in chartData.CategoryLabels)
                    {
                        var value = group.FirstOrDefault(x => ((int)x.TimeGroup).ToString() + "月" == label)?.Count ?? 0;
                        series.Values.Add((double)value);
                    }

                    chartData.Series.Add(series);
                }
            }

            return chartData;
        }
    }
}
