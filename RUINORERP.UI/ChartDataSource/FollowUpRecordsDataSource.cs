using Castle.Core.Resource;
using RUINORERP.Common.Extensions;
using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using RUINORERP.Business.Security;
using RUINORERP.Business.CommService;
using RUINORERP.UI.ChartAnalyzer;

namespace RUINORERP.UI.ChartDataSource
{
    /// <summary>
    /// 客户资料数据源（单表结构）
    /// </summary>
    public class FollowUpRecordsDataSource : IChartDataSource
    {

        public IEnumerable<DimensionConfig> GetDimensions()
        {
            return new List<DimensionConfig>
        {
            new DimensionConfig("Employee_ID", "业务员", DimensionType.String),
            new DimensionConfig("Region_ID", "区域", DimensionType.String),
            new DimensionConfig("Created_at", "创建时间", DimensionType.DateTime)
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
        public async Task<ChartDataSet> GetDataAsync(ChartRequest request)
        {
            var (startTime, endTime) = request.GetTimeRange();

            var query = MainForm.Instance.AppContext.Db.Queryable<tb_CRM_FollowUpRecords>()
                .WhereIF(request.Employee_ID.HasValue, c => c.Employee_ID == request.Employee_ID.Value)
                .WhereIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext), t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                .Where(c => c.Created_at >= startTime && c.Created_at <= endTime);


            // 构建分组字段（时间维度 + 其他维度）
            var groupFields = new List<string>(); //{ request.GetTimeGroupExpression() };
            groupFields.Add(request.GetGroupByTimeField(request.TimeField));
            groupFields.Add("Employee_ID");
            //groupFields.Add(request.TimeField);
            //groupFields.AddRange(request.Dimensions);


            // 构建 GroupByModel 列表
            var groupByModels = new List<GroupByModel>();
            //var columnName = request.GetGroupByTimeField(request.TimeField);
            //var groupByModel = new GroupByModel
            //{
            //    FieldName = columnName
            //};
            //groupByModels.Add(groupByModel);

            var groupByModel2 = new GroupByModel
            {
                FieldName = "Employee_ID"
            };

            groupByModels.Add(groupByModel2);

            var result = await query
                  .GroupBy(request.GetGroupByTimeField(request.TimeField))
                   .GroupBy(groupByModels)
               .Select(g => (dynamic)new
               {
                   TimeGroup = SqlFunc.DateValue(g.Created_at.Value, DateType.Month), // 时间分组键
                   业务员 = g.Employee_ID,
                   Count = SqlFunc.AggregateCount(1)
               })

               .ToListAsync();

            // 转换为图表数据
            return TransformToChartDataSet(result, request);
        }

        private ChartDataSet TransformToChartDataSet(List<dynamic> data, ChartRequest request)
        {
            var chartData = new ChartDataSet();
            //修改查询结果的值
            foreach (var item in data)
            {
                if (request.TimeGroupType == TimeRangeType.Monthly)
                {
                    item.TimeGroup = item.TimeGroup + "月";
                }
            }
            // 1. 处理时间维度作为X轴标签
            chartData.Labels = data.Select(x => (string)x.TimeGroup.ToString()).Distinct().OrderBy(x => x).ToList().ToArray();

            // 2. 处理其他维度作为系列
            if (request.Dimensions.Any())
            {
                var seriesGroups = data
                    .GroupBy(x => string.Join(",", x.业务员))
                    .ToList();

                foreach (var group in seriesGroups)
                {
                    string seriesName = group.Key.ToString();
                    tb_Employee Employee = MyCacheManager.Instance.GetEntity<tb_Employee>(seriesName.ToLong());
                    if (Employee != null)
                    {
                        seriesName = Employee.Employee_Name;
                    }

                    var series = new ChartSeries
                    {
                        Name = seriesName,
                        Values = new List<double>()
                    };

                    // 填充每个时间点的值
                    foreach (var label in chartData.Labels)
                    {
                        var value = group.FirstOrDefault(x => (string)(x.TimeGroup.ToString()) == label)?.Count ?? 0;
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
                    Name = "跟进数量",
                    Values = new List<double>()
                };

                foreach (var label in chartData.Labels)
                {
                    var value = data.FirstOrDefault(x => (string)(x.TimeGroup.ToString()) == label)?.Count ?? 0;
                    series.Values.Add((double)value);
                }

                chartData.Series.Add(series);
            }

            return chartData;
        }

        #endregion

        
       
        private void ApplyFilters(ref IQueryable<tb_CRM_FollowUpRecords> query, IEnumerable<QueryFilter> filters)
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
