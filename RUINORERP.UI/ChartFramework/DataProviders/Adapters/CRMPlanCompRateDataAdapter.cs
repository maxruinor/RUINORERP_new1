using RUINORERP.Business.CommService;
using RUINORERP.Business.Security;
using RUINORERP.Common.Extensions;
using RUINORERP.Model;
using RUINORERP.UI.ChartFramework.Adapters;
using RUINORERP.UI.ChartFramework.Core.Models;
using RUINORERP.UI.ChartFramework.Core;
using RUINORERP.UI.ChartFramework.Models;
using RUINORERP.UI.ChartFramework.Shared.Utilities;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValueType = RUINORERP.UI.ChartFramework.Core.ValueType;
using RUINORERP.Global.EnumExt.CRM;
using FastReport.Data;
using NPOI.POIFS.Crypt.Dsig;
using NPOI.SS.Formula.Functions;
using System.Web.UI.WebControls;
using NSoup.Helper;

namespace RUINORERP.UI.ChartFramework.DataProviders.Adapters
{
    public class CRMPlanCompRateDataAdapter : SqlDataProviderBase
    {
        public CRMPlanCompRateDataAdapter(DataRequest request) : base(request) { }

        public CRMPlanCompRateDataAdapter()
        {
        }
        public CRMPlanCompRateDataAdapter(ISqlSugarClient db) : base(db) { }

        public override IEnumerable<DimensionConfig> GetDimensions() => new[]
        {
        new DimensionConfig("Employee_ID", "业务员", DimensionType.String),
        new DimensionConfig("Region_ID", "区域", DimensionType.String),
        new DimensionConfig("Created_at", "创建时间", DimensionType.DateTime)
    };
        // return new[] { "Region", "Employee", "ProductCategory" };
        public override IEnumerable<MetricConfig> GetMetrics() => new[]
        {
            new MetricConfig("Count", "客户数量", MetricType.Count,MetricUnit.人)
        };
        //  return new[] { "Count", "Sum", "Average" };
        protected override string PrimaryTableName => "tb_CRM_Customer";
        #region new function
        public async override Task<ChartData> GetDataAsync(DataRequest request)
        {
            //数据源决定了做一个完成率 饼形
            request.ChartType = ChartType.Pie;
            var query = await GetPlanStatusStatistics(request);
            return CreatePlanStatusPieChart(query, request);
        }

        public async Task<List<PlanStatusStatistic>> GetPlanStatusStatistics(DataRequest request)
        {
            var query = _db.Queryable<tb_CRM_FollowUpPlans>()
                .Where(p => !p.isdeleted.Value); // 排除已删除

            // 时间范围筛选
            if (request.StartTime.HasValue)
                query = query.Where(p => p.Created_at >= request.StartTime.Value);
            if (request.EndTime.HasValue)
                query = query.Where(p => p.Created_at <= request.EndTime.Value);

            if (_request.Employee_ID.HasValue && _request.Employee_ID.Value > 0)
                query = query.Where(p => p.Employee_ID == request.Employee_ID.Value);

            // 按状态分组统计
            var result = await query
                .GroupBy(p => p.PlanStatus)
                .Select(g => new
                {
                    Status = g.PlanStatus,
                    Count = SqlFunc.AggregateCount(1),
                })
                .ToListAsync();
            double totalRecords = result.Sum(r => r.Count);
            // 转换为强类型枚举
            return result.Select(r => new PlanStatusStatistic
            {
                Status = (FollowUpPlanStatus)r.Status,
                Count = r.Count,
                Percentage = Math.Round(r.Count * 100.0 / totalRecords, 2)
            }).ToList();
        }

        // 统计结果模型
        public class PlanStatusStatistic
        {
     
            public FollowUpPlanStatus Status { get; set; }
            public int Count { get; set; }
            public double Percentage { get; set; }

            public string StatusName => GetStatusDisplayName(Status);
            public string DisplayValue => $"{StatusName} ({Percentage}%)";
            private string GetStatusDisplayName(FollowUpPlanStatus status)
            {
                return status switch
                {
                    FollowUpPlanStatus.未开始 => "未开始",
                    FollowUpPlanStatus.延期中 => "延期",
                    FollowUpPlanStatus.进行中 => "进行中",
                    FollowUpPlanStatus.已完成 => "已完成",
                    FollowUpPlanStatus.已取消 => "已取消",
                    FollowUpPlanStatus.未执行 => "未执行",
                    _ => status.ToString()
                };
            }
        }

        public ChartData CreatePlanStatusPieChart(List<PlanStatusStatistic> statistics, DataRequest request)
        {
             
            // 获取统计的时间范围
            var minDate = statistics.Min(s => request.StartTime);
            var maxDate = statistics.Max(s => request.EndTime);

            var chartData = new ChartData
            {
                Title = $"跟进计划状态分布 ({minDate:yyyy-MM-dd} 至 {maxDate:yyyy-MM-dd})", // 添加时间范围
                ChartType = ChartType.Pie,
                ValueType = ValueType.Percentage,
                // 启用饼图特殊配置
                PieOptions = new PieChartOptions
                {
                    ShowTotalInCenter = true,
                    CenterLabelFormat = "总计: {Total}\n完成率: {Percent}%"
                }
            };

            // 按百分比排序（从大到小）
            var sortedStats = statistics.OrderByDescending(s => s.Percentage).ToList();

            // 设置分类标签和数据
            chartData.CategoryLabels = sortedStats.Select(s => s.DisplayValue).ToArray();

            foreach (var metric in chartData.CategoryLabels)
            {
                chartData.Series.Add(new PieSeries
                {
                    Name = metric,
                    Values = sortedStats.Where(c => c.DisplayValue == metric).Select(s => (double)s.Count).ToList(),
                    PointLabels = sortedStats.Where(c => c.DisplayValue == metric).Select(s => s.DisplayValue).ToArray(),
                    //  DataLabels = true,
                    // Colors = GetStatusColors(sortedStats.Select(s => s.Status).ToList())
                }); ;

            }



            //chartData.Series.Add(new LineSeries
            //{
            //    Name = metric,
            //    Values = periods.Select(p =>
            //        stats.Where(s => s.Date.ToString("yyyy-MM") == p)
            //             .Sum(s => s.GetMetricValue(metric))).ToList(),
            //    ColorHex = GetMetricColor(metric),
            //    ShowMarkers = true
            //});




            // 添加完成率指标
            var completionRate = statistics
                .Where(s => s.Status == FollowUpPlanStatus.已完成)
                .Sum(s => s.Percentage);

            chartData.SubTitle = $"计划完成率: {completionRate}%";
            chartData.Annotations.Add(new ChartAnnotation
            {
                Text = $"总计划数: {statistics.Sum(s => s.Count)}",
                Position = AnnotationPosition.Center,
                FontSize = 14,
            });


            chartData.Annotations.Add(new ChartAnnotation
            {
                Text = $"完成率: {completionRate:F1}%",
                Position = AnnotationPosition.Bottom,
                FontSize = 12,
            });

            return chartData;
        }

        // 状态对应的颜色
        private List<string> GetStatusColors(List<FollowUpPlanStatus> statuses)
        {
            var colorMap = new Dictionary<FollowUpPlanStatus, string>
            {
                [FollowUpPlanStatus.已完成] = "#4CAF50", // 绿色
                [FollowUpPlanStatus.进行中] = "#2196F3", // 蓝色
                [FollowUpPlanStatus.延期中] = "#FFC107", // 黄色
                [FollowUpPlanStatus.未开始] = "#9E9E9E", // 灰色
                [FollowUpPlanStatus.已取消] = "#F44336", // 红色
                [FollowUpPlanStatus.未执行] = "#607D8B"  // 蓝灰色
            };

            return statuses.Select(s => colorMap[s]).ToList();
        }


        #endregion
        protected void ApplyGrouping(ref ISugarQueryable<dynamic> query, DataRequest request)
        {
            query.GroupBy(request.TimeField)
                 .GroupBy("Employee_ID");
        }



        protected override ChartData TransformToChartData(List<dynamic> rawData, DataRequest request)
        {
            throw new NotImplementedException();
        }
    }

}
