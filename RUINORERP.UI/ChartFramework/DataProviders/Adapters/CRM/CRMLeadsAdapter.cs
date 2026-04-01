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
    /// CRM 线索数据适配器
    /// </summary>
    public class CRMLeadsAdapter : SqlDataProviderBase
    {
        public CRMLeadsAdapter() { }

        public CRMLeadsAdapter(ISqlSugarClient db) : base(db) { }

        protected override string PrimaryTableName => "tb_CRM_Leads";

        public override IEnumerable<DimensionConfig> GetDimensions() => new[]
        {
            new DimensionConfig("Employee_ID", "业务员", DimensionType.String),
            new DimensionConfig("LeadSource", "线索来源", DimensionType.String),
            new DimensionConfig("LeadStatus", "线索状态", DimensionType.String),
            new DimensionConfig("Created_at", "创建时间", DimensionType.DateTime)
        };

        public override IEnumerable<MetricConfig> GetMetrics() => new[]
        {
            new MetricConfig("Count", "线索数", MetricType.Count, MetricUnit.Count),
            new MetricConfig("ConvertedCount", "转化数", MetricType.Sum, MetricUnit.Count)
        };

        public async override Task<ChartData> GetDataAsync(DataRequest request)
        {
            var query = _db.Queryable<tb_CRM_Leads>()
                .WhereIF(request.Employee_ID.HasValue, c => c.Employee_ID == request.Employee_ID.Value)
                .Where(c => c.Created_at >= request.StartTime && c.Created_at <= request.EndTime);

            // 构建分组字段 - 暂时使用固定字段
            var groupFields = new List<string>();
            groupFields.Add("Created_at"); // SqlTimeGroupBuilder.GetGroupByTimeField(request.TimeField, request.RangeType));
            groupFields.Add("LeadSource");

            var groupByModels = groupFields.Select(f => new GroupByModel { FieldName = f }).ToList();

            var result = await query
                .GroupBy(groupByModels)
                .Select(g => (dynamic)new
                {
                    TimeGroup = SqlFunc.DateValue(g.Created_at.Value, DateType.Month),
                    LeadSource = "未知", // g.LeadSource ?? "未知",
                    Count = SqlFunc.AggregateCount(1),
                    ConvertedCount = SqlFunc.AggregateSum(g.Converted)
                })
                .ToListAsync();

            return TransformToChartData(result, request);
        }

        protected override ChartData TransformToChartData(List<dynamic> rawData, DataRequest request)
        {
            var chartData = new ChartData
            {
                Title = "线索统计",
                ChartType = ChartType.Column,
                ValueType = RUINORERP.Model.ChartFramework.Models.ValueType.Absolute
            };

            // 处理时间维度作为 X 轴标签
            chartData.CategoryLabels = rawData
                .Select(x => ((int)x.TimeGroup).ToString() + "月")
                .Distinct()
                .OrderBy(x => x)
                .ToArray();

            // 按线索来源分组创建系列
            if (request.Dimensions.Any(d => d.FieldName == "LeadSource"))
            {
                var seriesGroups = rawData.GroupBy(x => x.LeadSource);
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
