using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RUINORERP.Model.ChartFramework.Models
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
        public string TimeField { get; set; } = "Created_at";
        public List<DimensionConfig> Dimensions { get; set; } = new List<DimensionConfig>();
        public List<MetricConfig> Metrics { get; set; } = new List<MetricConfig>();
        public long? Employee_ID { get; set; }
        public List<FieldFilter> Filters { get; set; } = new List<FieldFilter>();

        private DateTime? _startTime;
        public DateTime? StartTime
        {
            get => _startTime;
            set
            {
                if (_startTime != value)
                {
                    _startTime = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private DateTime? _endTime;
        public DateTime? EndTime
        {
            get => _endTime;
            set
            {
                if (_endTime != value)
                {
                    _endTime = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public TimeRangeType RangeType { get; set; } = TimeRangeType.None;

        public object Clone() => MemberwiseClone();
    }

    /// <summary>
    /// 字段过滤器
    /// </summary>
    public class FieldFilter
    {
        public string Field { get; set; }
        public object Value { get; set; }
        public string Operator { get; set; } = "=";
    }
}
