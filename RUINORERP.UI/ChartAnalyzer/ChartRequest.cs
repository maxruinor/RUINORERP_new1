using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartAnalyzer
{
    /// <summary>
    /// 图表请求参数封装类
    /// </summary>
    public class ChartRequest : ICloneable
    {
        /// <summary>
        /// 时间维度分组类型
        /// </summary>
        public TimeRangeType TimeGroupType { get; set; } = TimeRangeType.Monthly;

        /// <summary>
        /// 时间字段名称（默认为创建时间）
        /// </summary>
        public string TimeField { get; set; } = "Created_at";


        /// <summary>
        /// 自定义结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 其他分组维度字段列表（如地区、类型等）
        /// 分组维度字段列表（最多支持3个维度）?
        /// </summary>
        public List<string> Dimensions { get; set; } = new List<string>();


        /// <summary>
        /// 时间范围类型（默认周统计）
        /// </summary>
        public TimeRangeType TimeRange { get; set; } = TimeRangeType.Weekly;

        /// <summary>
        /// 自定义开始时间（当TimeRange=Custom时生效）
        /// </summary>
        public DateTime? CustomStart { get; set; }

        /// <summary>
        /// 自定义结束时间（当TimeRange=Custom时生效）
        /// </summary>
        public DateTime? CustomEnd { get; set; }



        /// <summary>
        /// 需要统计的指标列表
        /// </summary>
        public List<string> Metrics { get; set; } = new List<string>();

        /// <summary>
        /// 过滤条件集合
        /// </summary>
        public List<QueryFilter> Filters { get; set; } = new List<QueryFilter>();



        /// <summary>
        /// 验证请求参数有效性
        /// </summary>
        public virtual bool Validate(out string errorMessage)
        {
            if (Dimensions.Count > 3)
            {
                errorMessage = "最多支持3个分组维度";
                return false;
            }

            if (Metrics.Count == 0)
            {
                errorMessage = "必须至少选择一个统计指标";
                return false;
            }

            if (TimeRange == TimeRangeType.Custom && (!CustomStart.HasValue || !CustomEnd.HasValue))
            {
                errorMessage = "自定义时间范围必须指定开始和结束时间";
                return false;
            }

            errorMessage = null;
            return true;
        }

        public object Clone() => MemberwiseClone();
        /// <summary>
        /// 自定义开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 获取有效时间范围
        /// </summary>
        public (DateTime Start, DateTime End) GetTimeRange()
        {
            var now = DateTime.Now;
            if (StartTime.HasValue && EndTime.HasValue)
                return (StartTime.Value, EndTime.Value);
            return TimeRange switch
            {
                TimeRangeType.Daily => (now.Date, now.Date.AddDays(1).AddTicks(-1)),
                TimeRangeType.Weekly => (now.StartOfWeek(DayOfWeek.Monday), now.EndOfWeek(DayOfWeek.Sunday)),
                TimeRangeType.Monthly => (new DateTime(now.Year, now.Month, 1), now.LastDayOfMonth()),
                TimeRangeType.Quarterly => (now.FirstDayOfQuarter(), now.LastDayOfQuarter()),
                TimeRangeType.Yearly => (new DateTime(now.Year, 1, 1), new DateTime(now.Year, 12, 31)),
                TimeRangeType.Custom when CustomStart.HasValue && CustomEnd.HasValue
                    => (CustomStart.Value, CustomEnd.Value),
                _ => throw new ArgumentException("Invalid time range configuration")
            };
        }

        /// <summary>
        /// 获取时间分组的SQL表达式
        /// </summary>
        public string GetTimeGroupExpression()
        {
            return TimeGroupType switch
            {
                TimeRangeType.Daily => $"CONVERT(varchar(10), {TimeField}, 120)", // YYYY-MM-DD
                TimeRangeType.Weekly => $"CONCAT(YEAR({TimeField}), '-', DATEPART(week, {TimeField}))",
                TimeRangeType.Monthly => $"CONCAT(YEAR({TimeField}), '-', MONTH({TimeField}))",
                TimeRangeType.Quarterly => $"CONCAT(YEAR({TimeField}), '-Q', DATEPART(quarter, {TimeField}))",
                TimeRangeType.Yearly => $"YEAR({TimeField})",
                _ => throw new ArgumentException("Unsupported time group type")
            };
        }


        /// <summary>
        /// 获取时间分组的SQL表达式
        /// GROUP BY   YEAR(Created_at), MONTH(Created_at)
        /// </summary>
        public string GetGroupByTimeField(string TimeField)
        {
            return TimeGroupType switch
            {
                TimeRangeType.Daily => $"CONVERT(varchar(10), {TimeField}, 120)", // YYYY-MM-DD
                TimeRangeType.Weekly => $"CONCAT(YEAR({TimeField}), '-', DATEPART(week, {TimeField}))",
                TimeRangeType.Monthly => $"MONTH({TimeField})",
                TimeRangeType.YearlyMonthly => $"YEAR({TimeField}), MONTH({TimeField})",
                TimeRangeType.Quarterly => $"CONCAT(YEAR({TimeField}), '-Q', DATEPART(quarter, {TimeField}))",
                TimeRangeType.Yearly => $"YEAR({TimeField})",
                _ => throw new ArgumentException("Unsupported time group type")
            };
        }

        /// <summary>
        /// 获取时间分组的SQL表达式
        /// </summary>
        public string GetQueryTimeFieldBySql()
        {
            return TimeGroupType switch
            {
                TimeRangeType.Daily => $"CONVERT(varchar(10), {TimeField}, 120)", // YYYY-MM-DD
                TimeRangeType.Weekly => $"CONCAT(YEAR({TimeField}), '-', DATEPART(week, {TimeField}))",
                TimeRangeType.Monthly => $"CONVERT(varchar, YEAR({TimeField})) + '-' + RIGHT('0' + CONVERT(varchar, MONTH({TimeField})), 2)",
                TimeRangeType.Quarterly => $"CONCAT(YEAR({TimeField}), '-Q', DATEPART(quarter, {TimeField}))",
                TimeRangeType.Yearly => $"YEAR({TimeField})",
                _ => throw new ArgumentException("Unsupported time group type")
            };
        }

    }




    /// <summary>
    /// 时间范围类型枚举
    /// 按周 应该就是最近7天
    /// </summary>
    public enum TimeRangeType
    {
        Daily,      // 按日
        Weekly,     // 按周
        Monthly,    // 按月
        YearlyMonthly,     // 按年月
        Quarterly,  // 按季度
        Yearly,     // 按年
        Custom      // 自定义
    }


}
