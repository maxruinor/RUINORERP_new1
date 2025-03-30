using Castle.Core.Resource;
using RUINORERP.Common.Extensions;
using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Security;
using RUINORERP.UI.ChartAnalyzer;
using RUINORERP.Global.EnumExt.CRM;
using NPOI.SS.UserModel.Charts;
using Org.BouncyCastle.Tls;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using LiveChartsCore.Kernel.Sketches;
using RUINORERP.UI.ChartFramework.Models;
using RUINORERP.UI.ChartFramework.Models.ChartFramework.Core.Models;
using RUINORERP.UI.ChartFramework.Data.Abstract;

namespace RUINORERP.UI.ChartDataSource
{
    /// <summary>
    /// 客户资料数据源（单表结构）
    /// </summary>
    public class CRM_FollowUpPlansDataSource : ChartDataSourceBase
    {
        public async override Task<ChartDataSet> GetDataAsync(ChartRequest request)
        {
            var (startTime, endTime) = request.GetTimeRange();

            var query = MainForm.Instance.AppContext.Db.Queryable<tb_CRM_FollowUpPlans>()
                  .WhereIF(request.Employee_ID.HasValue, c => c.Employee_ID == request.Employee_ID.Value)
                 .WhereIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext), t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                .Where(c => c.Created_at >= startTime && c.Created_at <= endTime);


            // 构建分组字段（时间维度 + 其他维度）
            var groupFields = new List<string>(); //{ request.GetTimeGroupExpression() };
            groupFields.Add(request.GetGroupByTimeField(request.TimeField));
            groupFields.Add("Employee_ID");
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
            return base.TransformToChartDataSet(result, request);
        }


        public async Task<ChartDataSet> GetData跟进计划完成率Async(ChartRequest request)
        {
            var (startTime, endTime) = request.GetTimeRange();

            var query = MainForm.Instance.AppContext.Db.Queryable<tb_CRM_FollowUpPlans>()
                  .WhereIF(request.Employee_ID.HasValue, c => c.Employee_ID == request.Employee_ID.Value)
                 .WhereIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext), t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                .Where(c => c.Created_at >= startTime && c.Created_at <= endTime);


            // 构建分组字段（时间维度 + 其他维度）
            var groupFields = new List<string>(); //{ request.GetTimeGroupExpression() };
            groupFields.Add(request.GetGroupByTimeField(request.TimeField));
            groupFields.Add("Employee_ID");
            //groupFields.AddRange(request.Dimensions);


            // 构建 GroupByModel 列表
            var groupByModels = new List<GroupByModel>();
            var columnName = "PlanStatus";
            var groupByModel = new GroupByModel
            {
                FieldName = columnName
            };
            groupByModels.Add(groupByModel);

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
                   Count = g.PlanStatus,
               })

               .ToListAsync();

            // 转换为图表数据
            return base.TransformToChartDataSet(result, request);
        }


        public async Task<ChartDataSet> GetData跟进计划状态占比Async(ChartRequest request)
        {
            var query = MainForm.Instance.AppContext.Db.Queryable<tb_CRM_FollowUpPlans>();

            // 动态构建分组条件
            var groupByModels = new List<GroupByModel>
            {
                new GroupByModel { FieldName = "PlanStatus" } // 按状态分组是必须的
            };
            // 如果有业务员参数，则增加业务员分组
            if (request.Employee_ID.HasValue)
            {
                groupByModels.Add(new GroupByModel { FieldName = "Employee_ID" });
            }

            var querySelect = query.GroupBy(groupByModels);

            if (request.Employee_ID.HasValue)
            {
                var result = await query
                 .GroupBy(groupByModels)
                 .Select(g => (dynamic)new
                 {
                     Status = g.PlanStatus,
                     EmployeeId = request.Employee_ID.HasValue ? g.Employee_ID : 0, // 条件判断
                     Count = SqlFunc.AggregateCount(1)
                 })
                 .ToListAsync();
                // 转换为图表数据
                return TransformToChartDataSetByProportion(result, request.Employee_ID.HasValue);
            }
            else
            {
                var result = await query
                .GroupBy(groupByModels)
                .Select(g => (dynamic)new
                {
                    Status = g.PlanStatus,
                    Count = SqlFunc.AggregateCount(1)
                })
                .ToListAsync();
                // 转换为图表数据
                return TransformToChartDataSetByProportion(result, request.Employee_ID.HasValue);
            }
        }

        private ChartDataSet TransformToChartDataSetByProportion(List<dynamic> data, bool groupByEmployee)
        {
            var chartData = new ChartDataSet();

            // 1. 获取所有状态作为标签
            var allStatuses = Enum.GetValues(typeof(FollowUpPlanStatus))
                                 .Cast<FollowUpPlanStatus>()
                                 .Select(s => s.ToString())
                                 .ToList();
            chartData.Labels = allStatuses.ToArray();

            // 2. 按不同分组方式处理数据
            if (groupByEmployee)
            {
                // 按业务员分组的情况
                var employeeGroups = data.GroupBy(x => x.EmployeeId);

                foreach (var employeeGroup in employeeGroups)
                {
                    var employeeTotal = employeeGroup.Sum(x => (int)x.Count);
                    var seriesGroup = new ChartSeries
                    {
                        Name = $"业务员 {employeeGroup.Key}",
                        Values = new List<double>()
                    };

                    foreach (var status in allStatuses)
                    {
                        var statusValue = (int)Enum.Parse(typeof(FollowUpPlanStatus), status);
                        var count = employeeGroup.FirstOrDefault(x => (int)x.Status == statusValue)?.Count ?? 0;
                        var percentage = Math.Round((count / (double)employeeTotal) * 100, 2);
                        seriesGroup.Values.Add(percentage);
                    }

                    chartData.Series.Add(seriesGroup);
                }
            }
            else
            {
                // 不按业务员分组的情况
                var total = data.Sum(x => (int)x.Count);
                var series = new ChartSeries
                {
                    Name = "状态占比",
                    Values = new List<double>()
                };

                foreach (var status in allStatuses)
                {
                    var statusValue = (int)Enum.Parse(typeof(FollowUpPlanStatus), status);
                    var count = data.FirstOrDefault(x => (int)x.Status == statusValue)?.Count ?? 0;
                    var percentage = Math.Round((count / (double)total) * 100, 2);
                    series.Values.Add(percentage);
                }

                chartData.Series.Add(series);
            }

            return chartData;

            // 1. 处理状态维度作为X轴标签
            //chartData.Labels = data.Select(x => (string)x.Status.ToString()).Distinct().OrderBy(x => x).ToList().ToArray();

            var seriesTemp = new ChartSeries
            {
                Name = "状态占比",
                Values = new List<double>()
            };

            foreach (var item in data)
            {
                var status = Enum.GetName(typeof(FollowUpPlanStatus), item.Status);
                // 不按业务员分组的情况
                var total = data.Sum(x => (int)x.Count);
                //var percentage = (item.Count / (double)total) * 100;
                var percentage = Math.Round((item.Count / (double)total) * 100, 2);
                // 填充每个时间点的值
                foreach (var label in chartData.Labels)
                {
                    seriesTemp.Name = status;
                    //var value = group.FirstOrDefault(x => (string)(x.TimeGroup.ToString()) == label)?.Count ?? 0;
                    seriesTemp.Values.Add((double)percentage);
                }
            }
            chartData.Series.Add(seriesTemp);

            // 2. 处理其他维度作为系列
            //if (chartData.Series.Count > 0)
            //{
            //    chartData.Series[0].Name = "状态占比";
            //}
            return chartData;
        }


        private bool _isAllZero;    // 判断饼图中是否所有数据都是0

        /// <summary>
        /// 创建饼图
        /// </summary>
        /// <param name="titles">饼图标签</param>
        /// <param name="data">饼图数据</param>
        /// <returns></returns>
        private ISeries[] CreatePieChart(string[] titles, double[] data)
        {
            ISeries[] seriesCollection;
            _isAllZero = data.All(x => x == 0);

            // 如果所有数据不都为0
            if (!_isAllZero)
            {
                // 创建饼图数据
                List<PieSeries<double>> seriesList = new List<PieSeries<double>>();
                // 定义一个颜色数组
                var colors = new SKColor[] { SKColors.DodgerBlue, SKColors.OrangeRed, SKColors.YellowGreen };
                for (int i = 0; i < titles.Length; i++)
                {
                    if (data[i] != 0)
                    {
                        var series = new PieSeries<double>
                        {
                            // 设置扇形名称
                            Name = titles[i],
                            // 设置饼图扇形数据
                            Values = new double[] { data[i] },
                            // 设置饼图扇形填充颜色
                            Fill = new SolidColorPaint(colors[i]),
                            // 设置数据标签字体大小
                            DataLabelsSize = 15,
                            // 设置数据标签颜色
                            DataLabelsPaint = new SolidColorPaint(SKColors.White),
                            // 设置数据标签位置
                            DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                            // 设置数据标签格式化器
                            DataLabelsFormatter = point => point.Context.Series.Name + "：" + point.Coordinate.PrimaryValue.ToString(),
                            //  DataLabelsFormatter = _ => "", // 隐藏数据标签
                        };
                        seriesList.Add(series);
                    }
                }
                seriesCollection = seriesList.ToArray();
            }
            // 如果所有数据都为0，则添加一个虚拟的系列
            else
            {
                seriesCollection = new ISeries[1];
                // 添加一个虚拟的系列，确保总有一部分被显示
                var virtualSeries = new PieSeries<double>
                {
                    Name = "无",
                    Values = new double[] { 1 },
                    Fill = new SolidColorPaint(SKColor.Parse("#55B155")),
                    // 设置数据标签字体大小
                    DataLabelsSize = 15,
                    // 设置数据标签颜色
                    DataLabelsPaint = new SolidColorPaint(SKColors.White)
                    {
                        SKFontStyle = SKFontStyle.Italic,
                    },
                    // 设置数据标签位置
                    DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                    DataLabelsFormatter = point => point.Context.Series.Name ?? "无",
                    // 提示框文字格式
                    ToolTipLabelFormatter = _ => "",
                };
                seriesCollection[0] = virtualSeries;
            }

            return seriesCollection;

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


    }
}
