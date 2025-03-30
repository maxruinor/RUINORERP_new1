using RUINORERP.UI.ChartAnalyzer;
using RUINORERP.UI.ChartFramework.Core;
using RUINORERP.UI.ChartFramework.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.Models
{
    /// <summary>
    /// 图表请求参数封装类
    /// </summary>
    public class DataRequest : ICloneable
    {

        public string Title { get; set; }
        public ChartType ChartType { get; set; }
 

 
        public ChartType chartType { get; set; }

        /// <summary>
        /// 时间字段名称（默认为创建时间）
        /// </summary>
        public string TimeField { get; set; } = "Created_at";

        /// <summary>
        /// 其他分组维度字段列表（如地区、类型等）
        /// 分组维度字段列表（最多支持3个维度）?
        /// </summary>
        public List<string> Dimensions { get; set; } = new List<string>();
        
        public long? Employee_ID { get; set; }

        /// <summary>
        /// 需要统计的指标列表
        /// </summary>
        public List<string> Metrics { get; set; } = new List<string>();
        
        /// <summary>
        /// 过滤条件集合
        /// </summary>
        public List<FieldFilter> Filters { get; set; } = new List<FieldFilter>();
        
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

            //if (TimeRange == TimeRangeType.Custom && (!CustomStart.HasValue || !CustomEnd.HasValue))
            //{
            //    errorMessage = "自定义时间范围必须指定开始和结束时间";
            //    return false;
            //}

            errorMessage = null;
            return true;
        }

        public object Clone() => MemberwiseClone();


        /// <summary>
        /// 自定义开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 自定义结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        public TimeRangeType RangeType { get; set; }

        /// <summary>
        /// 获取有效时间范围
        /// </summary>
        //public (DateTime Start, DateTime End) GetTimeRange()
        //{
        //    var now = DateTime.Now;
        //    if (StartTime.HasValue && EndTime.HasValue)
        //        return (StartTime.Value, EndTime.Value);
        //    return TimeRange switch
        //    {
        //        TimeRangeType.Daily => (now.Date, now.Date.AddDays(1).AddTicks(-1)),
        //        TimeRangeType.Weekly => (now.StartOfWeek(DayOfWeek.Monday), now.EndOfWeek(DayOfWeek.Sunday)),
        //        TimeRangeType.Monthly => (new DateTime(now.Year, now.Month, 1), now.LastDayOfMonth()),
        //        TimeRangeType.Quarterly => (now.FirstDayOfQuarter(), now.LastDayOfQuarter()),
        //        TimeRangeType.Yearly => (new DateTime(now.Year, 1, 1), new DateTime(now.Year, 12, 31)),
        //        TimeRangeType.Custom when CustomStart.HasValue && CustomEnd.HasValue
        //            => (CustomStart.Value, CustomEnd.Value),
        //        _ => throw new ArgumentException("Invalid time range configuration")
        //    };
        //}


        // 自动计算的属性
        public DateTimeRange TimeRange => CalculateTimeRange();

        ///// <summary>
        ///// 时间范围类型（默认周统计）
        ///// </summary>
        //public TimeGranularity TimeRange { get; set; } = TimeGranularity.Weekly;

        private DateTimeRange CalculateTimeRange()
        {
            var now = DateTime.Now;
            if (StartTime.HasValue && EndTime.HasValue)
                return new DateTimeRange(StartTime.Value, EndTime.Value);

            return RangeType switch
            {
                TimeRangeType.Daily => new DateTimeRange(now.Date, now.Date.AddDays(1).AddTicks(-1)),
                TimeRangeType.Weekly => new DateTimeRange(
                    now.StartOfWeek(DayOfWeek.Monday),
                    now.EndOfWeek(DayOfWeek.Sunday)),
                // 其他case...
                _ => throw new ArgumentException("Invalid time range type")
            };
        }

        // 新增辅助结构体
        public readonly struct DateTimeRange(DateTime start, DateTime end)
        {
            public DateTime Start { get; } = start;
            public DateTime End { get; } = end;
            public TimeSpan Duration => End - Start;
        }



    }

}

