using NPOI.OpenXmlFormats.Dml.Chart;
using RUINORERP.Business.Processor;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.UI.ChartAnalyzer;
using RUINORERP.UI.ChartFramework.Core;
using RUINORERP.UI.ChartFramework.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.Models
{
    /// <summary>
    /// 图表请求参数封装类
    /// </summary>
    public class DataRequest : ICloneable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string Title { get; set; }

        public ChartType ChartType { get; set; }

        /// <summary>
        /// 时间字段名称（默认为创建时间）
        /// </summary>
        public string TimeField { get; set; } = "Created_at";

        /// <summary>
        /// 其他分组维度字段列表（如地区、类型等）
        /// 分组维度字段列表（最多支持3个维度）?
        /// </summary>
        public List<DimensionConfig> Dimensions { get; set; } = new List<DimensionConfig>();

        /// <summary>
        /// 需要统计的指标列表
        /// </summary>
        public List<MetricConfig> Metrics { get; set; } = new List<MetricConfig>();

        [AdvQueryAttribute(ColName = "Employee_ID", ColDesc = "业务员")]
        public long? Employee_ID { get; set; }

        /// <summary>
        /// 过滤条件集合
        /// </summary>
        public List<FieldFilter> Filters { get; set; } = new List<FieldFilter>();

        public List<LambdaExpression> FilterLimitExpressions { get; set; } = new List<LambdaExpression>();

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

        private int _selectedTimeRange = (int)TimeSelectType.CurrentWeek;
        public int SelectedTimeRange
        {
            get => _selectedTimeRange;
            set
            {
                if (_selectedTimeRange != value)
                {
                    _selectedTimeRange = value;
                    UpdateTimeRangeFromSelection();
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(TimeRange));
                }
            }
        }
        private void UpdateTimeRangeFromSelection()
        {
            if (SelectedTimeRange == (int)TimeSelectType.Custom)
                return;

            var now = DateTime.Now;
            var range = GetTimeRangeForSelection(now);

            _startTime = range.Start;
            _endTime = range.End;

            // Don't notify here to avoid recursive calls
            // The TimeRange property will handle notification
        }

        private DateTimeRange GetTimeRangeForSelection(DateTime now)
        {
            TimeSelectType SelectType = (TimeSelectType)SelectedTimeRange;
            return SelectType switch
            {
                TimeSelectType.Last7Days => new DateTimeRange(now.AddDays(-7).Date, now.Date.AddDays(1).AddTicks(-1)),
                TimeSelectType.Last15Days => new DateTimeRange(now.AddDays(-15).Date, now.Date.AddDays(1).AddTicks(-1)),
                TimeSelectType.Last30Days => new DateTimeRange(now.AddDays(-30).Date, now.Date.AddDays(1).AddTicks(-1)),
                TimeSelectType.CurrentWeek => new DateTimeRange(now.StartOfWeek(DayOfWeek.Monday), now.EndOfWeek(DayOfWeek.Sunday)),
                TimeSelectType.LastWeek => new DateTimeRange(now.AddDays(-7).StartOfWeek(DayOfWeek.Monday),
                                           now.AddDays(-7).EndOfWeek(DayOfWeek.Sunday)),
                TimeSelectType.CurrentMonth => new DateTimeRange(new DateTime(now.Year, now.Month, 1),
                                              new DateTime(now.Year, now.Month, 1).AddMonths(1).AddTicks(-1)),
                TimeSelectType.LastMonth => new DateTimeRange(new DateTime(now.Year, now.Month, 1).AddMonths(-1),
                                           new DateTime(now.Year, now.Month, 1).AddTicks(-1)),
                TimeSelectType.CurrentQuarter => new DateTimeRange(now.FirstDayOfQuarter(), now.LastDayOfQuarter()),
                TimeSelectType.LastQuarter => new DateTimeRange(now.FirstDayOfQuarter().AddMonths(-3),
                                              now.LastDayOfQuarter().AddMonths(-3)),
                TimeSelectType.CurrentYear => new DateTimeRange(new DateTime(now.Year, 1, 1),
                                                              new DateTime(now.Year, 12, 31, 23, 59, 59)),
                TimeSelectType.Last6Months => new DateTimeRange(now.AddMonths(-6).Date, now.Date.AddDays(1).AddTicks(-1)),
                TimeSelectType.Last12Months => new DateTimeRange(now.AddMonths(-12).Date, now.Date.AddDays(1).AddTicks(-1)),
                _ => new DateTimeRange(DateTime.MinValue, DateTime.MaxValue)
            };
        }
        private DateTime? _startTime;

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime
        {
            get => _startTime;
            set
            {
                if (_startTime != value)
                {
                    _startTime = value;
                    if (value.HasValue) SelectedTimeRange = (int)TimeSelectType.Custom;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(TimeRange));
                }
            }
        }

        private DateTime? _endTime;

        /// <summary>
        /// 自定义结束时间
        /// </summary>
        public DateTime? EndTime
        {
            get => _endTime;
            set
            {
                if (_endTime != value)
                {
                    _endTime = value;
                    if (value.HasValue) SelectedTimeRange = (int)TimeSelectType.Custom;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(TimeRange));
                }
            }
        }





        TimeRangeType _RangeType;
        public TimeRangeType RangeType
        {
            get => _RangeType;
            set { _RangeType = value; NotifyPropertyChanged(); }
        }

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
        // Automatic property that calculates based on current settings
        public DateTimeRange TimeRange
        {
            get
            {
                // If custom dates are set, use them
                if (StartTime.HasValue && EndTime.HasValue)
                {
                    return new DateTimeRange(
                        StartTime.Value,
                        EndTime.Value >= StartTime.Value ? EndTime.Value : StartTime.Value.AddDays(1));
                }

                // Otherwise calculate from selection
                return GetTimeRangeForSelection(DateTime.Now);
            }
        }

        ///// <summary>
        ///// 时间范围类型（默认周统计）
        ///// </summary>
        //public TimeGranularity TimeRange { get; set; } = TimeGranularity.Weekly;

        private DateTimeRange CalculateTimeRange()
        {
            var now = DateTime.Now;

            // 优先使用自定义时间范围
            if (StartTime.HasValue && EndTime.HasValue)
            {
                // 确保结束时间不小于开始时间
                var effectiveEnd = EndTime.Value >= StartTime.Value
                    ? EndTime.Value
                    : StartTime.Value.AddDays(1);
                return new DateTimeRange(StartTime.Value, effectiveEnd);
            }

            // 处理预设时间范围
            return RangeType switch
            {
                TimeRangeType.Daily => new DateTimeRange(
                    now.Date,
                    now.Date.AddDays(1).AddTicks(-1)), // 当天00:00:00到23:59:59

                TimeRangeType.Weekly => new DateTimeRange(
                    now.StartOfWeek(DayOfWeek.Monday),
                    now.EndOfWeek(DayOfWeek.Sunday)),

                TimeRangeType.Monthly => new DateTimeRange(
                    new DateTime(now.Year, now.Month, 1),
                    new DateTime(now.Year, now.Month, 1).AddMonths(1).AddTicks(-1)),

                TimeRangeType.Quarterly => new DateTimeRange(
                    now.FirstDayOfQuarter(),
                    now.LastDayOfQuarter()),

                TimeRangeType.Yearly => new DateTimeRange(
                    new DateTime(now.Year, 1, 1),
                    new DateTime(now.Year, 12, 31, 23, 59, 59)),

                TimeRangeType.None => new DateTimeRange(
                    DateTime.MinValue,
                    DateTime.MaxValue),

                _ => throw new ArgumentException($"不支持的时间范围类型: {RangeType}")
            };
        }

        // 新增辅助结构体
        public readonly struct DateTimeRange(DateTime start, DateTime end)
        {
            public DateTime Start { get; } = start;
            public DateTime End { get; } = end;
            public TimeSpan Duration => End - Start;
        }


        /// <summary>
        /// 添加指标
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryFieldIDExp">查询条件为这个字段时</param>
        /// <param name="queryFieldNameExp">查询出来的显示为这个字段，ID-》name,通常这个编号的字段在主表及引用的表中 字段名要相同，如果不同则用上面的方法T R</param>
        /// <param name="AddSubFilter">如果是关联字段时，是否添加子过滤条件</param>
        /// <param name="SubFieldLimitExp">子条件限制器</param>
        public MetricConfig SetMetricField<T>(Expression<Func<T, object>> queryFieldIDExp)
        {
            if (queryFieldIDExp == null)
                throw new ArgumentNullException(nameof(queryFieldIDExp));

            MetricConfig queryField = new MetricConfig();
            ////QueryTargetType = typeof(T);
            //queryField.QueryTargetType = QueryTargetType;
            string fieldID = RuinorExpressionHelper.ExpressionToString(queryFieldIDExp);

            queryField.FieldName = fieldID;
            // queryField.FieldPropertyInfo = typeof(T).GetProperties().FirstOrDefault(c => c.Name == fieldID);

            //上面没有调用其他方法来SET这里要添加
            if (!Metrics.Contains(queryField))
            {
                Metrics.Add(queryField);
            }

            return queryField;
        }

        /// <summary>
        /// 添加指标
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryFieldIDExp">查询条件为这个字段时</param>
        /// <param name="queryFieldNameExp">查询出来的显示为这个字段，ID-》name,通常这个编号的字段在主表及引用的表中 字段名要相同，如果不同则用上面的方法T R</param>
        /// <param name="AddSubFilter">如果是关联字段时，是否添加子过滤条件</param>
        /// <param name="SubFieldLimitExp">子条件限制器</param>
        public DimensionConfig SetDimensionField<T>(Expression<Func<T, object>> queryFieldIDExp)
        {
            if (queryFieldIDExp == null)
                throw new ArgumentNullException(nameof(queryFieldIDExp));

            DimensionConfig queryField = new DimensionConfig();
            ////QueryTargetType = typeof(T);
            //queryField.QueryTargetType = QueryTargetType;
            string fieldID = RuinorExpressionHelper.ExpressionToString(queryFieldIDExp);

            queryField.FieldName = fieldID;
            // queryField.FieldPropertyInfo = typeof(T).GetProperties().FirstOrDefault(c => c.Name == fieldID);

            //上面没有调用其他方法来SET这里要添加
            if (!Dimensions.Contains(queryField))
            {
                Dimensions.Add(queryField);
            }

            return queryField;
        }

    }

}

