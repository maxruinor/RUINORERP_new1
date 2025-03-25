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

        //public async Task<ChartDataSet> GetDataAsync(ChartRequest request)
        //{
        //    var query = MainForm.Instance.AppContext.Db.Queryable<tb_CRM_Customer>();
             
        //    query.GroupBy(x => x.Region_ID);
        //    // 应用过滤条件
        //    //ApplyFilters(ref query, request.Filters);
           
        //    // 动态分组
        //    var groupQuery = query.GroupByDynamic(request.Dimensions);

        //    // 执行查询
        //    var result = await groupQuery
        //        .Select(g => new
        //        {
        //            Keys = g.Customer_id,
        //            Count = g.Count()
        //        })
        //        .ToListAsync();

        //    return TransformToDataSet(result, request);
        //}

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
                    Category =g.CustomerName,
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
