using RUINORERP.Model;
using RUINORERP.Model.ChartFramework.Models;
using RUINORERP.UI.ChartFramework.Adapters;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RUINORERP.Common.Extensions;

namespace RUINORERP.UI.ChartFramework.DataProviders.Adapters.CRM
{
    /// <summary>
    /// 客户分析数据适配器 (试点示例)
    /// </summary>
    public class CustomerAnalysisAdapter : SqlDataProviderBase
    {
        public CustomerAnalysisAdapter() { }
        
        public CustomerAnalysisAdapter(ISqlSugarClient db) : base(db) { }

        /// <summary>
        /// 主表名：客户资料表
        /// </summary>
        protected override string PrimaryTableName => "tb_CRM_Customer";

        /// <summary>
        /// 可用的维度配置
        /// </summary>
        public override IEnumerable<DimensionConfig> GetDimensions() => new[]
        {
            new DimensionConfig("Employee_ID", "业务员", DimensionType.String),
            new DimensionConfig("Region_ID", "区域", DimensionType.String),
            new DimensionConfig("Customer_Level", "客户等级", DimensionType.String),
            new DimensionConfig("Created_at", "创建时间", DimensionType.DateTime)
        };

        /// <summary>
        /// 可用的指标配置
        /// </summary>
        public override IEnumerable<MetricConfig> GetMetrics() => new[]
        {
            new MetricConfig("Count", "客户数量", MetricType.Count, MetricUnit.Count),
            new MetricConfig("ActiveCount", "活跃客户数", MetricType.Sum, MetricUnit.Count)
        };

        /// <summary>
        /// 获取客户分析数据
        /// </summary>
        /// <param name="request">数据请求参数</param>
        /// <returns>图表数据</returns>
        public async override Task<ChartData> GetDataAsync(DataRequest request)
        {
            // Step 1: 构建基础查询
            var query = _db.Queryable<tb_CRM_Customer>()
                .WhereIF(request.Employee_ID.HasValue, x => x.Employee_ID == request.Employee_ID.Value)
                .WhereIF(request.StartTime.HasValue, x => x.Created_at >= request.StartTime)
                .WhereIF(request.EndTime.HasValue, x => x.Created_at <= request.EndTime);

            // Step 2: 构建分组字段
            var groupFields = new List<string>();
            
            // 时间维度 (必须) - 暂时使用固定字段
            groupFields.Add("Created_at"); // SqlTimeGroupBuilder.GetGroupByTimeField(request.TimeField, request.RangeType));
            
            // 其他维度 (可选)
            if (request.Dimensions.Any(d => d.FieldName != request.TimeField))
            {
                groupFields.AddRange(request.Dimensions
                    .Where(d => d.FieldName != request.TimeField)
                    .Select(d => d.FieldName));
            }

            var groupByModels = groupFields.Select(f => new GroupByModel { FieldName = f }).ToList();

            // Step 3: 执行分组查询
            var result = await query
                .GroupBy(groupByModels)
                .Select(g => (dynamic)new
                {
                    TimeGroup = SqlFunc.DateValue(g.Created_at.Value, DateType.Month),
                    EmployeeId = g.Employee_ID,
                    RegionId = g.Region_ID,
                    Count = SqlFunc.AggregateCount(1),
                    ActiveCount = 0 // g.Is_Active ?? false ? 1 : 0
                })
                .ToListAsync();

            // Step 4: 转换为图表数据
            return TransformToChartData(result, request);
        }

        /// <summary>
        /// 转换为图表数据
        /// </summary>
        /// <param name="rawData">原始查询结果</param>
        /// <param name="request">请求参数</param>
        /// <returns>图表数据对象</returns>
        protected override ChartData TransformToChartData(List<dynamic> rawData, DataRequest request)
        {
            var chartData = new ChartData
            {
                Title = "客户增长分析",
                ChartType = ChartType.Column,
                ValueType = RUINORERP.Model.ChartFramework.Models.ValueType.Absolute,
                IsStacked = false
            };

            // Step 1: 处理时间维度作为 X 轴标签
            chartData.CategoryLabels = rawData
                .Select(x => FormatTimeLabel(x.TimeGroup, request))
                .Cast<string>()
                .Distinct()
                .OrderBy(x => x)
                .ToArray();

            // Step 2: 判断是否按维度分组
            var hasDimension = request.Dimensions.Any(d => 
                d.FieldName == "Employee_ID" || 
                d.FieldName == "Region_ID" ||
                d.FieldName == "Customer_Level");

            if (hasDimension)
            {
                // Step 3a: 按维度分组创建多个系列
                var dimensionField = request.Dimensions.FirstOrDefault(d => 
                    d.FieldName == "Employee_ID" || 
                    d.FieldName == "Region_ID" ||
                    d.FieldName == "Customer_Level")?.FieldName ?? "Employee_ID";

                var seriesGroups = rawData.GroupBy(x => GetDimensionValue(x, dimensionField));
                
                foreach (var group in seriesGroups)
                {
                    var series = new DataSeries
                    {
                        Name = GetDimensionName(group.Key, dimensionField),
                        Values = new List<double>()
                    };

                    // 填充每个时间点的值
                    foreach (var label in chartData.CategoryLabels)
                    {
                        var metricValue = request.Metrics.FirstOrDefault()?.FieldName ?? "Count";
                        var value = group.FirstOrDefault(x => FormatTimeLabel(x.TimeGroup, request) == label)?[metricValue] ?? 0;
                        series.Values.Add((double)value);
                    }

                    chartData.Series.Add(series);
                }
            }
            else
            {
                // Step 3b: 无分组维度，创建单个系列
                foreach (var metric in request.Metrics)
                {
                    var series = new DataSeries
                    {
                        Name = metric.DisplayName ?? metric.FieldName, // 使用中文显示名
                        Values = rawData.Select(x => (double)x[metric.FieldName]).ToList()
                    };
                    chartData.Series.Add(series);
                }
            }

            return chartData;
        }

        #region Helper Methods

        /// <summary>
        /// 格式化时间标签
        /// </summary>
        private string FormatTimeLabel(object value, DataRequest request)
        {
            if (value is DateTime dt)
            {
                return request.RangeType switch
                {
                    TimeRangeType.Daily => dt.ToString("yyyy-MM-dd"),
                    TimeRangeType.Monthly => dt.ToString("yyyy-MM"),
                    TimeRangeType.Quarterly => $"Q{(dt.Month - 1) / 3 + 1}-{dt.Year}",
                    TimeRangeType.Yearly => dt.ToString("yyyy"),
                    _ => dt.ToString("yyyy-MM-dd")
                };
            }
            return value?.ToString() ?? "未知";
        }

        /// <summary>
        /// 获取维度值
        /// </summary>
        private object GetDimensionValue(dynamic item, string dimensionField)
        {
            try
            {
                return item.GetType().GetProperty(dimensionField)?.GetValue(item);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取维度显示名称
        /// </summary>
        private string GetDimensionName(object key, string dimensionField)
        {
            if (key == null || key.ToString() == "")
                return "未知";

            // 业务员 ID 转姓名
            if (dimensionField == "Employee_ID")
            {
                if (long.TryParse(key.ToString(), out var employeeId))
                {
                    var employee = _db.Queryable<tb_Employee>()
                        .First(e => e.Employee_ID == employeeId);
                    
                    return employee?.Employee_Name ?? "未知";
                }
                return key?.ToString() ?? "未知";
            }

            // 区域 ID 转名称 - 暂时注释掉，等待 Sys_Region 实体定义
            if (dimensionField == "Region_ID")
            {
                // TODO: 添加 Sys_Region 实体后取消注释
                // if (long.TryParse(key.ToString(), out var regionId))
                // {
                //     var region = _db.Queryable<Sys_Region>()
                //         .First(r => r.Region_ID == regionId);
                //     return region?.Region_Name ?? "未知";
                // }
                return key?.ToString() ?? "未知";
            }

            return key?.ToString() ?? "未知";
        }

        #endregion
    }
}
