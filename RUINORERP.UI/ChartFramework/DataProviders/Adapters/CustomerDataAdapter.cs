using NPOI.SS.UserModel.Charts;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.Crmf;
using RUINORERP.Business.Cache;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Security;
using RUINORERP.Common.Extensions;
using RUINORERP.Model;
using RUINORERP.UI.ChartFramework.Adapters;
using RUINORERP.UI.ChartFramework.Core;
using RUINORERP.UI.ChartFramework.Core.Contracts;
using RUINORERP.UI.ChartFramework.Core.Models;
using RUINORERP.UI.ChartFramework.DataProviders.SqlSugar;
using RUINORERP.UI.ChartFramework.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RUINORERP.UI.ChartFramework.DataProviders.Adapters
{
    /// <summary>
    /// 提供数据源
    /// </summary>
    public class CustomerDataAdapter : SqlDataProviderBase
    {
        public CustomerDataAdapter()
        { 
        }
        public CustomerDataAdapter(ISqlSugarClient db) : base(db) { }

        public override IEnumerable<DimensionConfig> GetDimensions() => new[]
        {
        new DimensionConfig("Employee_ID", "业务员", DimensionType.String),
        new DimensionConfig("Region_ID", "区域", DimensionType.String),
        new DimensionConfig("Created_at", "创建时间", DimensionType.DateTime)
    };
        // return new[] { "Region", "Employee", "ProductCategory" };
        public override IEnumerable<MetricConfig> GetMetrics() => new[]
        {
            new MetricConfig("Count", "客户数量", MetricType.Count, MetricUnit.人)
        };
        //  return new[] { "Count", "Sum", "Average" };
        protected override string PrimaryTableName => "tb_CRM_Customer";
        #region new function
        public async override Task<ChartData> GetDataAsync(DataRequest request)
        {

            // 添加查询缓存

            //var cacheKey = $"ChartData_{request.GetHashCode()}";
            //if (_cache.TryGetValue(cacheKey, out ChartDataSet cachedData))
            //    return cachedData;

            //var data = await RealQueryAsync(request);
            //_cache.Set(cacheKey, data, TimeSpan.FromMinutes(5));
            //return data;


            var query = MainForm.Instance.AppContext.Db.Queryable<tb_CRM_Customer>()
                  .WhereIF(_request.Employee_ID.HasValue && _request.Employee_ID.Value > 0, c => c.Employee_ID == request.Employee_ID.Value)
                 .WhereIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext), t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                .Where(c => c.Created_at >= request.StartTime && c.Created_at <= request.EndTime);


            // 构建分组字段（时间维度 + 其他维度）
            var groupFields = new List<string>(); //{ request.GetTimeGroupExpression() };
            groupFields.Add(SqlTimeGroupBuilder.GetGroupByTimeField(request.TimeField, request.RangeType));
            groupFields.Add("Employee_ID");

            // 拼接分组字符串
            var groupByStr = string.Join(", ", groupFields);

            //groupFields.AddRange(request.Dimensions);
            // 构建 GroupByModel 列表
            var groupByModels = new List<GroupByModel>();
            groupFields.ForEach<string>(f => groupByModels.Add(new GroupByModel { FieldName = f }));
            var result = await query
                //                  .GroupBy(groupByModels)
                .GroupBy(groupByStr)
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
        // 在类开始处添加：
        private static IEntityCacheManager _cacheManager;
        private static IEntityCacheManager CacheManager => _cacheManager ?? (_cacheManager = Startup.GetFromFac<IEntityCacheManager>());
        private ChartData TransformToChartDataSet(List<dynamic> data, DataRequest request)
        {
            var chartData = new ChartData();

            //修改查询结果的值
            foreach (var item in data)
            {
                if (request.RangeType == TimeRangeType.Monthly)
                {
                    item.TimeGroup = item.TimeGroup + "月";
                }
            }

            // 1. 处理时间维度作为X轴标签
            chartData.CategoryLabels = data.Select(x => (string)x.TimeGroup.ToString()).Distinct().OrderBy(x => x).ToList().ToArray();

            // 2. 处理其他维度作为系列
            if (request.Dimensions.Any())
            {
                var seriesGroups = data
                    .GroupBy(x => string.Join(",", x.业务员))
                    .ToList();

                foreach (var group in seriesGroups)
                {
                    string seriesName = group.Key.ToString();
                    tb_Employee Employee = CacheManager.GetEntity<tb_Employee>(seriesName.ToLong());
                    if (Employee != null)
                    {
                        seriesName = Employee.Employee_Name;
                    }

                    var series = new DataSeries
                    {
                        Name = seriesName,
                        Values = new List<double>()
                    };

                    // 填充每个时间点的值
                    foreach (var label in chartData.CategoryLabels)
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
                var series = new DataSeries
                {
                    Name = "客户数量",
                    Values = new List<double>()
                };

                foreach (var label in chartData.CategoryLabels)
                {
                    var value = data.FirstOrDefault(x => (string)(x.TimeGroup.ToString()) == label)?.Count ?? 0;
                    series.Values.Add((double)value);
                }

                chartData.Series.Add(series);
            }


            // 模拟数据
            var chartData1 = new ChartData
            {
                Title = "客户统计",
                ChartType = ChartType.Column,
                CategoryLabels = new[] { "1月", "2月", "3月", "4月", "5月" },
                IsStacked = false
            };

            var series1 = new DataSeries
            {
                Name = "新增客户",
                Values = new List<double> { 120, 150, 180, 210, 240 },
                ColorHex = "#4285F4"
            };

            var series2 = new DataSeries
            {
                Name = "活跃客户",
                Values = new List<double> { 80, 90, 100, 110, 120 },
                ColorHex = "#34A853"
            };

            if (request.ChartType == ChartType.Column)
            {
                var columnChartData = new ChartData
                {
                    Title = "季度销售对比",
                    ChartType = ChartType.Column,
                    IsStacked = true,
                    CategoryLabels = new[] { "Q1", "Q2", "Q3", "Q4" },

                    Series = new List<DataSeries>
                {
                    new ColumnSeries
                    {
                        Name = "产品A",
                        Values = new List<double> { 120, 150, 180, 210 },
                       // ColorHex = "#4285F4",
                        ColumnWidth = 0.7
                    },
                    new ColumnSeries
                    {
                        Name = "产品B",
                        Values = new List<double> { 80, 90, 100, 110 },
                        //ColorHex = "#34A853"
                    }
                }
                };
                return columnChartData;
            }
       
            chartData1.Series.Add(series1);
            chartData1.Series.Add(series2);
            return chartData1;
            return chartData;
        }

        #endregion
        protected void ApplyGrouping(ref ISugarQueryable<dynamic> query, DataRequest request)
        {
            query.GroupBy(request.TimeField)
                 .GroupBy("Employee_ID");
        }

        //protected  ChartData TransformData(List<dynamic> rawData, DataRequest request)
        //{
        //    return Transform(rawData, new TransformationOptions
        //    {
        //        TimeField = "TimeGroup",
        //        ValueField = "Count",
        //        SeriesGroupField = "Employee_ID",
        //        TimeFormat = request.RangeType == TimeRangeType.Monthly ? "{0}月" : null
        //    });
        //}


        protected override ChartData TransformToChartData(List<dynamic> rawData, DataRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
