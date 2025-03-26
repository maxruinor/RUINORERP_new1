using Castle.Core.Resource;
using RUINORERP.Common.Extensions;
using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;


namespace RUINORERP.UI.ChartAnalyzer
{
    /// <summary>
    /// 客户资料数据源（单表结构）
    /// </summary>
    public class CustomerDataSource : IChartDataSource
    {

        public IEnumerable<DimensionConfig> GetDimensions()
        {
            return new List<DimensionConfig>
        {
            new DimensionConfig("SalesmanID", "业务员", DimensionType.String),
            new DimensionConfig("Region", "区域", DimensionType.String),
            new DimensionConfig("CreateTime", "创建时间", DimensionType.DateTime)
        };
        }

        public IEnumerable<MetricConfig> GetMetrics()
        {
            return new List<MetricConfig>
            {
                new MetricConfig("客户数量", MetricType.Count,ColorHelper.HexToSKColor("#4CAF50"))
            };
        }

        #region new function
        public async Task<ChartDataSet> GetCustomerStatsAsync(ChartRequest request)
        {
            var (startTime, endTime) = request.GetTimeRange();

            var query = MainForm.Instance.AppContext.Db.Queryable<tb_CRM_Customer>()
                .Where(c => c.Created_at >= startTime && c.Created_at <= endTime);

            // 构建分组字段（时间维度 + 其他维度）
            var groupFields = new List<string>(); //{ request.GetTimeGroupExpression() };
            groupFields.Add(request.GetGroupByTimeField(request.TimeField));
            //groupFields.Add(request.TimeField);
            //groupFields.AddRange(request.Dimensions);

            // 执行分组查询
            //var result = await query
            //    .GroupByDynamic(groupFields.ToArray())
            //    .Select(g => (dynamic)new
            //    {
            //        TimeGroup = request.GetQueryTimeFieldBySql(), // 时间分组键
            //        //OtherDimensions = request.Dimensions
            //        //    .Select(d => "未知")
            //        //    .ToList(),
            //        Count = SqlFunc.AggregateCount(1)
            //    })
            //    .ToListAsync();

            // 构建 GroupByModel 列表
            var groupByModels = new List<GroupByModel>();
            var columnName = request.GetGroupByTimeField(request.TimeField);
            var groupByModel = new GroupByModel
            {
                FieldName = columnName
            };
            groupByModels.Add(groupByModel);
            var result = await query
                  .GroupBy(groupByModels)
               .Select(g => (dynamic)new
               {
                   TimeGroup = request.GetGroupByTimeField(request.TimeField), // 时间分组键
                                                                 //OtherDimensions = request.Dimensions
                                                                 //    .Select(d => "未知")
                                                                 //    .ToList(),
                   Count = SqlFunc.AggregateCount(1)
               })
              
               .ToListAsync();

            // 转换为图表数据
            return TransformToChartDataSet1(result, request);
        }

        private ChartDataSet TransformToChartDataSet1(List<dynamic> data, ChartRequest request)
        {
            var chartData = new ChartDataSet();

            // 1. 处理时间维度作为X轴标签
            chartData.Labels = data.Select(x => (string)x.TimeGroup).Distinct().OrderBy(x => x).ToList().ToArray();

            // 2. 处理其他维度作为系列
            if (request.Dimensions.Any())
            {
                var seriesGroups = data
                    .GroupBy(x => string.Join("|", x.OtherDimensions))
                    .ToList();


                foreach (var group in seriesGroups)
                {
                    // 兼容方式构建系列名称
                    var dimensionPairs = new List<string>();
                    for (int i = 0; i < request.Dimensions.Count; i++)
                    {
                        var dimValue = i < group.First().OtherDimensions.Count
                            ? group.First().OtherDimensions[i]
                            : "未知";
                        dimensionPairs.Add($"{request.Dimensions[i]}:{dimValue}");
                    }

                    var seriesName = string.Join("+",
                    request.Dimensions.Select((dim, index) =>
                    $"{dim}:{(index < group.First().OtherDimensions.Count
                      ? group.First().OtherDimensions[index]
                      : "未知")}"));

                    //var seriesName = string.Join("+", dimensionPairs);

                    var series = new ChartSeries
                    {
                        Name = seriesName,
                        Values = new List<double>()
                    };

                    // 填充每个时间点的值
                    foreach (var label in chartData.Labels)
                    {
                        var value = group.FirstOrDefault(x => (string)x.TimeGroup == label)?.Count ?? 0;
                        series.Values.Add((double)value);
                    }

                    chartData.Series.Add(series);
                }
            }
            else
            {
                // 没有其他维度时，只显示一个系列
                var series = new ChartSeries
                {
                    Name = "客户数量",
                    Values = new List<double>()
                };

                foreach (var label in chartData.Labels)
                {
                    var value = data.FirstOrDefault(x => (string)x.TimeGroup == label)?.Count ?? 0;
                    series.Values.Add((double)value);
                }

                chartData.Series.Add(series);
            }

            return chartData;
        }

        #endregion

        public async Task<ChartDataSet> GetDataAsync(ChartRequest request)
        {
            var query = MainForm.Instance.AppContext.Db.Queryable<tb_CRM_Customer>();

            // 应用过滤条件（如果有）
            // ApplyFilters(ref query, request.Filters);

            // 动态分组并执行查询
            var groupedData = await query
                .GroupByDynamic(request.Dimensions)
                .Select(g => (dynamic)new
                {
                    // 假设第一个维度作为分类轴
                    // Category = g.PurchaseCount.Key[request.Dimensions[0]]?.ToString() ?? "未知",
                    Category = g.CustomerName,
                    // 其他维度可以作为标签或分组
                    //SecondaryDimension = request.Dimensions.Count > 1
                    //    ? g.Key[request.Dimensions[1]]?.ToString() ?? "未知"
                    //    : null,
                    Count = SqlFunc.AggregateCount(1) // 使用SqlSugar的聚合函数
                })
                .ToListAsync();

            // 转换为LiveCharts2需要的数据结构
            return TransformToChartDataSet(groupedData, request);
        }

        private ChartDataSet TransformToChartDataSet(List<dynamic> data, ChartRequest request)
        {
            var chartDataSet = new ChartDataSet();

            // 1. 处理单维度情况
            if (request.Dimensions.Count == 1)
            {
                chartDataSet.Labels = data.Select(x => (string)x.Category).ToList().ToArray();
                chartDataSet.Series.Add(new ChartSeries
                {
                    Name = "客户数量",
                    Values = data.Select(x => (double)x.Count).ToList()
                });
            }
            // 2. 处理多维度情况（分组柱状图）
            else if (request.Dimensions.Count >= 2)
            {
                // 获取所有主分类
                var mainCategories = data.Select(x => (string)x.Category).Distinct().ToList();
                chartDataSet.Labels = mainCategories.ToArray();

                // 按第二维度分组
                var secondaryGroups = data.GroupBy(x => (string)x.SecondaryDimension);

                foreach (var group in secondaryGroups)
                {
                    var series = new ChartSeries
                    {
                        Name = group.Key,
                        Values = new List<double>()
                    };

                    // 填充每个主分类的值（如果没有则为0）
                    foreach (var category in mainCategories)
                    {
                        var value = group.FirstOrDefault(x => (string)x.Category == category)?.Count ?? 0;
                        series.Values.Add((double)value);
                    }

                    chartDataSet.Series.Add(series);
                }
            }

            return chartDataSet;
        }

        private void ApplyFilters(ref IQueryable<tb_CRM_Customer> query, IEnumerable<QueryFilter> filters)
        {
            //foreach (var filter in filters)
            //{
            //    query = filter.Field switch
            //    {
            //        "SalesmanID" => query.Where(c => c.SalesmanID == filter.Value.ToString()),
            //        "Region" => query.Where(c => c.Region_ID == filter.Value.ToLong()),
            //        "CreateTime" => ApplyDateFilter(query, filter),
            //        _ => query
            //    };
            //}
        }

        //private ChartDataSet TransformToDataSet(List<dynamic> result, ChartRequest request)
        //{
        //    var dataSet = new ChartDataSet();
        //    dataSet.Labels = result.Select(r => string.Join("-", r.Keys)).ToList().ToArray();
        //    dataSet.SeriesData["Count"] = result.Select(r => (double)r.Count).ToArray();
        //    return dataSet;
        //}
    }
}
