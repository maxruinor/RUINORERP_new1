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
    /// CRM 跟进计划数据适配器
    /// </summary>
    public class FollowUpPlansAdapter : SqlDataProviderBase
    {
        public FollowUpPlansAdapter() { }
        
        public FollowUpPlansAdapter(ISqlSugarClient db) : base(db) { }

        protected override string PrimaryTableName => "tb_CRM_FollowUpPlans";

        public override IEnumerable<DimensionConfig> GetDimensions() => new[]
        {
            new DimensionConfig("Employee_ID", "业务员", DimensionType.String),
            new DimensionConfig("Created_at", "创建时间", DimensionType.DateTime),
            new DimensionConfig("PlanDate", "计划日期", DimensionType.DateTime),
            new DimensionConfig("FollowUpType", "跟进类型", DimensionType.String)
        };

        public override IEnumerable<MetricConfig> GetMetrics() => new[]
        {
            new MetricConfig("Count", "跟进计划数", MetricType.Count, MetricUnit.Count)
        };

        public async override Task<ChartData> GetDataAsync(DataRequest request)
        {
            var query = _db.Queryable<tb_CRM_FollowUpPlans>()
                .WhereIF(request.Employee_ID.HasValue, c => c.Employee_ID == request.Employee_ID.Value)
                .Where(c => c.Created_at >= request.StartTime && c.Created_at <= request.EndTime);

            // 构建分组字段 - 暂时使用固定字段
            var groupFields = new List<string>();
            groupFields.Add("Created_at"); // SqlTimeGroupBuilder.GetGroupByTimeField(request.TimeField, request.RangeType));
            groupFields.Add("Employee_ID");

            var groupByModels = groupFields.Select(f => new GroupByModel { FieldName = f }).ToList();

            var result = await query
                .GroupBy(groupByModels)
                .Select(g => (dynamic)new
                {
                    TimeGroup = SqlFunc.DateValue(g.Created_at.Value, DateType.Month),
                    EmployeeId = g.Employee_ID,
                    Count = SqlFunc.AggregateCount(1)
                })
                .ToListAsync();

            return TransformToChartData(result, request);
        }

        protected override ChartData TransformToChartData(List<dynamic> rawData, DataRequest request)
        {
            var chartData = new ChartData
            {
                Title = "跟进计划统计",
                ChartType = ChartType.Column,
                ValueType = RUINORERP.Model.ChartFramework.Models.ValueType.Absolute
            };

            // 处理时间维度作为 X 轴标签
            chartData.CategoryLabels = rawData
                .Select(x => ((int)x.TimeGroup).ToString() + "月")
                .Distinct()
                .OrderBy(x => x)
                .ToArray();

            // 按业务员分组创建系列
            if (request.Dimensions.Any(d => d.FieldName == "Employee_ID"))
            {
                var seriesGroups = rawData.GroupBy(x => x.EmployeeId);
                foreach (var group in seriesGroups)
                {
                    var series = new DataSeries
                    {
                        Name = GetEmployeeName(group.Key),
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

        private string GetEmployeeName(long? employeeId)
        {
            if (!employeeId.HasValue) return "未知";
            
            var employee = _db.Queryable<tb_Employee>()
                .First(e => e.Employee_ID == employeeId.Value);
            
            return employee?.Employee_Name ?? "未知";
        }
    }
}
