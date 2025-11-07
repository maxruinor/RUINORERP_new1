using Castle.Core.Resource;
using NPOI.SS.UserModel.Charts;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.Crmf;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Security;
using RUINORERP.Common.Extensions;
using RUINORERP.Model;
using RUINORERP.UI.ChartFramework.Adapters;
using RUINORERP.UI.ChartFramework.Core;
using RUINORERP.UI.ChartFramework.Core.Contracts;
using RUINORERP.UI.ChartFramework.Core.Models;
using RUINORERP.UI.ChartFramework.Core.Rendering.Builders;
using RUINORERP.UI.ChartFramework.DataProviders.SqlSugar;
using RUINORERP.UI.ChartFramework.Models;
using RUINORERP.UI.ChartFramework.Rendering.Controls;
using RUINORERP.UI.ChartFramework.Shared.Utilities;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ValueType = RUINORERP.UI.ChartFramework.Core.ValueType;
using RUINORERP.Extensions.Middlewares;

namespace RUINORERP.UI.ChartFramework.DataProviders.Adapters
{
    /// <summary>
    /// 提供数据源
    /// </summary>
    public class CRMDataAdapter : SqlDataProviderBase
    {
        public CRMDataAdapter(DataRequest request) : base(request) { }

        public CRMDataAdapter()
        {
        }
        public CRMDataAdapter(ISqlSugarClient db) : base(db) { }

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
            var query = await GetWeeklyStatisticsByEmployee();
            return CreateEmployeeComparisonChart(query);
            ////按月按员工
            //var query = await GetMonthlyStatisticsByEmployee();
            //return ConvertToMonthlyChartDataByEmployee(query);
        }

        public async Task<List<CrmStatisticResult>> GetWeeklyStatisticsByEmployee()
        {
            var endDate = _request.EndTime;
            var startDate = _request.StartTime;

            // 获取员工列表
            var employees = await _db.Queryable<tb_Employee>()
                .Select(e => new { e.Employee_ID, e.Employee_Name })
                .WhereIF(_request.Employee_ID.HasValue && _request.Employee_ID.Value > 0, c => c.Employee_ID == _request.Employee_ID.Value)
                .WhereIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext), t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)
                .ToListAsync();

            var results = new List<CrmStatisticResult>();

            foreach (var emp in employees)
            {
                // 按周分组统计
                var weeklyStats = await _db.Queryable<tb_CRM_Customer>()
                    .Where(c => c.Employee_ID == emp.Employee_ID &&
                               c.Created_at >= startDate &&
                               c.Created_at <= endDate)
                    .GroupBy(c => new
                    {
                        Year = c.Created_at.Value.Year,
                        Week = SqlFunc.WeekOfYear(c.Created_at.Value)
                    })

                    .Select(g => new
                    {
                        // 添加周标识
                        Year = g.Created_at.Value.Year,
                        Week = SqlFunc.WeekOfYear(g.Created_at.Value),
                        NewCustomers = SqlFunc.AggregateCount(1),
                        Plans = SqlFunc.Subqueryable<tb_CRM_FollowUpPlans>()
                            .Where(p => p.Employee_ID == emp.Employee_ID &&
                                      p.Created_at >= startDate &&
                                      p.Created_at <= endDate &&
                                       p.Created_at.Value.Year == g.Created_at.Value.Year &&
                                     SqlFunc.WeekOfYear(p.Created_at.Value) == SqlFunc.WeekOfYear(g.Created_at.Value))
                            .Count(),
                        Leads = SqlFunc.Subqueryable<tb_CRM_Leads>()
                            .Where(l => l.Employee_ID == emp.Employee_ID &&
                                      l.Created_at >= startDate &&
                                      l.Created_at <= endDate &&
                                     l.Created_at.Value.Year == g.Created_at.Value.Year &&
                                      SqlFunc.WeekOfYear(l.Created_at.Value) == SqlFunc.WeekOfYear(g.Created_at.Value))
                            .Count(),
                        Followups = SqlFunc.Subqueryable<tb_CRM_FollowUpRecords>()
                            .Where(f => f.Employee_ID == emp.Employee_ID &&
                                      f.Created_at >= startDate &&
                                      f.Created_at <= endDate &&
                                      f.Created_at.Value.Year == g.Created_at.Value.Year &&
                                    SqlFunc.WeekOfYear(f.Created_at.Value) == SqlFunc.WeekOfYear(g.Created_at.Value))
                            .Count()
                    })

                    .ToListAsync();


                results.AddRange(weeklyStats.Select(s =>
                {
                    // 计算周的开始日期(周一)
                    var firstDayOfWeek = ISOWeekHelper.GetWeekRange(s.Year, s.Week).Start;
                    return new CrmStatisticResult
                    {
                        WeekLabel = s.Week.ToString("#周"), // 新增属性，存储周标识
                        DateRange = $"{firstDayOfWeek:yyyy-MM-dd} ~ {firstDayOfWeek.AddDays(6):yyyy-MM-dd}", // 周范围
                        Employee_ID = emp.Employee_ID,
                        EmployeeName = emp.Employee_Name,
                        Date = firstDayOfWeek, // 周的第一天(周一)
                        NewCustomers = s.NewCustomers,
                        Plans = s.Plans,
                        Leads = s.Leads,
                        Followups = s.Followups
                    };
                }));
            }


            return results;
        }


        public ChartData ConvertToWeeklyChartDataByEmployee(List<CrmStatisticResult> statistics)
        {
            var chartData = new ChartData
            {
                Title = "CRM系统周统计(按员工)",
                ChartType = ChartType.Column,
                IsStacked = false,
                ValueType = ValueType.Absolute
            };

            // 获取所有周并排序(格式：yyyy-Www)
            var weeks = statistics
                .Select(s => $"{s.Date.Year}-W{s.Date.GetWeeklyOfYear():D2}")
                .Distinct()
                .OrderBy(w => w)
                .ToArray();
            chartData.CategoryLabels = weeks;

            // 按员工和指标生成系列
            foreach (var empGroup in statistics.GroupBy(s => s.EmployeeName))
            {
                foreach (var metric in new[] { "新客户", "计划", "线索", "跟进" })
                {
                    var series = new ColumnSeries
                    {
                        Name = $"{empGroup.Key}-{metric}",
                        Values = weeks.Select(week =>
                        {
                            var yearWeek = week.Split('-');
                            var year = int.Parse(yearWeek[0]);
                            var weekNum = int.Parse(yearWeek[1].Substring(1));

                            var firstDayOfWeek = ISOWeekHelper.GetWeekRange(year, weekNum).Start;
                            var stat = empGroup.FirstOrDefault(s =>
                                s.Date.Year == firstDayOfWeek.Year &&
                                s.Date.GetWeekOfYear() == weekNum);

                            return metric switch
                            {
                                "新客户" => (double)(stat?.NewCustomers ?? 0),
                                "计划" => (double)(stat?.Plans ?? 0),
                                "线索" => (double)(stat?.Leads ?? 0),
                                "跟进" => (double)(stat?.Followups ?? 0),
                                _ => 0
                            };
                        }).ToList(),
                        ColorHex = GetMetricColor(metric)
                    };
                    chartData.Series.Add(series);
                }
            }

            return chartData;
        }


        #region 按员工+月分组统计
        public async Task<List<CrmStatisticResult>> GetMonthlyStatisticsByEmployee()
        {
            var endDate = _request.EndTime;
            var startDate = _request.StartTime;

            // 获取员工列表
            var employees = await _db.Queryable<tb_Employee>()
                .Select(e => new { e.Employee_ID, e.Employee_Name })
                .ToListAsync();

            var results = new List<CrmStatisticResult>();

            foreach (var emp in employees)
            {
                // 按月分组统计
                var monthlyStats = await _db.Queryable<tb_CRM_Customer>()
                    .Where(c => c.Employee_ID == emp.Employee_ID &&
                               c.Created_at >= startDate &&
                               c.Created_at <= endDate)
                    .GroupBy(c => new
                    {
                        Year = c.Created_at.Value.Year,
                        Month = c.Created_at.Value.Month
                    })
                    .Select(g => new
                    {
                        Year = g.Created_at.Value.Year,
                        Month = g.Created_at.Value.Month,
                        NewCustomers = SqlFunc.AggregateCount(1),
                        Plans = SqlFunc.Subqueryable<tb_CRM_FollowUpPlans>()
                            .Where(p => p.Employee_ID == emp.Employee_ID &&
                                      p.Created_at >= startDate &&
                                      p.Created_at <= endDate &&
                                      p.Created_at.Value.Year == g.Created_at.Value.Year &&
                                      p.Created_at.Value.Month == g.Created_at.Value.Month)
                            .Count(),
                        Leads = SqlFunc.Subqueryable<tb_CRM_Leads>()
                            .Where(l => l.Employee_ID == emp.Employee_ID &&
                                      l.Created_at >= startDate &&
                                      l.Created_at <= endDate &&
                                      l.Created_at.Value.Year == g.Created_at.Value.Year &&
                                      l.Created_at.Value.Month == g.Created_at.Value.Month)
                            .Count(),
                        Followups = SqlFunc.Subqueryable<tb_CRM_FollowUpRecords>()
                            .Where(f => f.Employee_ID == emp.Employee_ID &&
                                      f.Created_at >= startDate &&
                                      f.Created_at <= endDate &&
                                      f.Created_at.Value.Year == g.Created_at.Value.Year &&
                                      f.Created_at.Value.Month == g.Created_at.Value.Month)
                            .Count()
                    })
                    .ToListAsync();

                results.AddRange(monthlyStats.Select(s => new CrmStatisticResult
                {
                    Employee_ID = emp.Employee_ID,
                    EmployeeName = emp.Employee_Name,
                    Date = new DateTime(s.Year, s.Month, 1), // 月份的第一天
                    NewCustomers = s.NewCustomers,
                    Plans = s.Plans,
                    Leads = s.Leads,
                    Followups = s.Followups
                }));
            }

            return results;
        }
        public ChartData ConvertToMonthlyChartDataByEmployee(List<CrmStatisticResult> statistics)
        {
            var chartData = new ChartData
            {
                Title = "CRM系统月统计(按员工)",
                ChartType = ChartType.Column,
                IsStacked = false,
                ValueType = ValueType.Absolute
            };

            // 获取所有月份并排序（格式：yyyy-MM）
            var months = statistics
                .Select(s => s.Date.ToString("yyyy-MM"))
                .Distinct()
                .OrderBy(d => d)
                .ToArray();
            chartData.CategoryLabels = months;

            // 按员工和指标生成系列
            foreach (var empGroup in statistics.GroupBy(s => s.EmployeeName))
            {
                foreach (var metric in new[] { "新客户", "计划", "线索", "跟进" })
                {
                    var series = new ColumnSeries
                    {
                        Name = $"{empGroup.Key}-{metric}",
                        Values = months.Select(month =>
                        {
                            var stat = empGroup.FirstOrDefault(s => s.Date.ToString("yyyy-MM") == month);
                            return metric switch
                            {
                                "新客户" => (double)(stat?.NewCustomers ?? 0),
                                "计划" => (double)(stat?.Plans ?? 0),
                                "线索" => (double)(stat?.Leads ?? 0),
                                "跟进" => (double)(stat?.Followups ?? 0),
                                _ => 0
                            };
                        }).ToList(),
                        ColorHex = GetMetricColor(metric)
                    };
                    chartData.Series.Add(series);
                }
            }

            return chartData;
        }

        #endregion


        #region 按月不分员工
        public async Task<List<CrmStatisticResult>> GetMonthlyStatisticsTotal()
        {
            var endDate = _request.EndTime;
            var startDate = _request.StartTime;

            // 直接按月分组统计（不区分员工）
            var monthlyStats = await _db.Queryable<tb_CRM_Customer>()
                .Where(c => c.Created_at >= startDate && c.Created_at <= endDate)
                .GroupBy(c => new
                {
                    Year = c.Created_at.Value.Year,
                    Month = c.Created_at.Value.Month
                })
                .Select(g => new
                {
                    Year = g.Created_at.Value.Year,
                    Month = g.Created_at.Value.Month,
                    NewCustomers = SqlFunc.AggregateCount(1),
                    Plans = SqlFunc.Subqueryable<tb_CRM_FollowUpPlans>()
                        .Where(p => p.Created_at >= startDate &&
                                  p.Created_at <= endDate &&
                                  p.Created_at.Value.Year == g.Created_at.Value.Year &&
                                  p.Created_at.Value.Month == g.Created_at.Value.Month)
                        .Count(),
                    Leads = SqlFunc.Subqueryable<tb_CRM_Leads>()
                        .Where(l => l.Created_at >= startDate &&
                                  l.Created_at <= endDate &&
                                  l.Created_at.Value.Year == g.Created_at.Value.Year &&
                                  l.Created_at.Value.Month == g.Created_at.Value.Month)
                        .Count(),
                    Followups = SqlFunc.Subqueryable<tb_CRM_FollowUpRecords>()
                        .Where(f => f.Created_at >= startDate &&
                                  f.Created_at <= endDate &&
                                  f.Created_at.Value.Year == g.Created_at.Value.Year &&
                                  f.Created_at.Value.Month == g.Created_at.Value.Month)
                        .Count()
                })
                .ToListAsync();

            return monthlyStats.Select(s => new CrmStatisticResult
            {
                Date = new DateTime(s.Year, s.Month, 1),
                NewCustomers = s.NewCustomers,
                Plans = s.Plans,
                Leads = s.Leads,
                Followups = s.Followups
            }).ToList();
        }

        public ChartData ConvertToMonthlyChartDataTotal(List<CrmStatisticResult> statistics)
        {
            var chartData = new ChartData
            {
                Title = "CRM系统月统计(总计)",
                ChartType = ChartType.Column,
                IsStacked = true, // 堆叠显示总计
                ValueType = ValueType.Absolute
            };

            // 月份标签
            chartData.CategoryLabels = statistics
                .Select(s => s.Date.ToString("yyyy-MM"))
                .OrderBy(d => d)
                .ToArray();

            // 直接按指标生成系列
            foreach (var metric in new[] { "新客户", "计划", "线索", "跟进" })
            {
                var series = new ColumnSeries
                {
                    Name = metric,
                    Values = statistics
                        .OrderBy(s => s.Date)
                        .Select(s => metric switch
                        {
                            "新客户" => (double)s.NewCustomers,
                            "计划" => s.Plans,
                            "线索" => s.Leads,
                            "跟进" => s.Followups,
                            _ => 0
                        }).ToList(),
                    ColorHex = GetMetricColor(metric)
                };
                chartData.Series.Add(series);
            }

            return chartData;
        }

        #endregion


        public ChartData CreateTrendAnalysisChart(List<CrmStatisticResult> stats)
        {
            var chartData = new ChartData
            {
                Title = "CRM指标趋势分析",
                ChartType = ChartType.Line,
                ValueType = ValueType.Absolute
            };

            var periods = stats.Select(s => s.Date.ToString("yyyy-MM")).Distinct().OrderBy(p => p).ToList();
            chartData.CategoryLabels = periods.ToArray();

            var metrics = new[] { "新客户", "计划", "线索", "跟进" };
            foreach (var metric in metrics)
            {
                chartData.Series.Add(new LineSeries
                {
                    Name = metric,
                    Values = periods.Select(p =>
                        stats.Where(s => s.Date.ToString("yyyy-MM") == p)
                             .Sum(s => s.GetMetricValue(metric))).ToList(),
                    ColorHex = GetMetricColor(metric),
                    ShowMarkers = true
                });
            }

            return chartData;
        }
        public ChartData CreateEmployeeComparisonChart(List<CrmStatisticResult> stats)
        {
            if (!stats.Any()) return new ChartData();

            // 获取统计的时间范围
            var minDate = _request.StartTime;
            var maxDate = _request.EndTime;

            var chartData = new ChartData
            {
                Title = $"员工绩效对比 ({minDate:yyyy-MM-dd} 至 {maxDate:yyyy-MM-dd})", // 添加时间范围
                ChartType = ChartType.Bar, // 横向条形图
                ValueType = ValueType.Absolute,
                // 添加周标签作为副标题
                SubTitle = $"数据周期：{stats.First().DateRange} 等共 {stats.Select(s => s.WeekLabel).Distinct().Count()} 周"
            };
            // 按员工+周分组显示（更清晰）
            var employeeWeekGroups = stats
                .GroupBy(s => $"{s.EmployeeName}({s.WeekLabel})");

            chartData.CategoryLabels = employeeWeekGroups.Select(g => g.Key).ToArray();

            //var employees = stats.GroupBy(s => s.EmployeeName);
            //chartData.CategoryLabels = employees.Select(g => g.Key).ToArray();

            var metrics = new[] { "新客户", "计划", "线索", "跟进" };
            foreach (var metric in metrics)
            {
                chartData.Series.Add(new ColumnSeries
                {
                    Name = metric,
                    Values = employeeWeekGroups.Select(g =>
                        g.Sum(s => s.GetMetricValue(metric))).ToList(),
                    ColorHex = GetMetricColor(metric)
                });
            }

            return chartData;
        }


        private string GetMetricColor(string metric)
        {
            return metric switch
            {
                "新客户" => "#4285F4", // 蓝色
                "计划" => "#34A853",   // 绿色
                "线索" => "#FBBC05",   // 黄色
                "跟进" => "#EA4335",   // 红色
                _ => "#999999"
            };
        }



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
                    tb_Employee Employee = RUINORERP.Business.Cache.EntityCacheHelper.GetEntity<tb_Employee>(seriesName.ToLong());
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



        protected override ChartData TransformToChartData(List<dynamic> rawData, DataRequest request)
        {
            throw new NotImplementedException();
        }
    }

    public class CrmStatisticResult
    {
        public string WeekLabel { get; set; } // 如 "2023-W42"
        public string DateRange { get; set; } // 如 "2023-10-16 ~ 2023-10-22"
        public long Employee_ID { get; set; }
        public string EmployeeName { get; set; }
        public DateTime Date { get; set; }
        public int NewCustomers { get; set; }
        public int Plans { get; set; }
        public int Leads { get; set; }
        public int Followups { get; set; }
        public double GetMetricValue(string metric)
        {
            return metric switch
            {
                "新客户" => this.NewCustomers,
                "计划" => this.Plans,
                "线索" => this.Leads,
                "跟进" => this.Followups,
                _ => 0
            };
        }
    }




}
