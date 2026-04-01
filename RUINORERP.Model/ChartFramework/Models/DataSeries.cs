using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RUINORERP.Model.ChartFramework.Models
{
    /// <summary>
    /// 图表数据点
    /// </summary>
    public class DataPoint
    {
        /// <summary>
        /// X 轴值
        /// </summary>
        public object XValue { get; set; }

        /// <summary>
        /// Y 轴值
        /// </summary>
        public double YValue { get; set; }

        /// <summary>
        /// 数据标签
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 自定义标签
        /// </summary>
        public string Tag { get; set; }
    }

    /// <summary>
    /// 图表数据系列 (Model 层简化版)
    /// </summary>
    public class DataSeries : INotifyPropertyChanged
    {
        private string _name = "数据系列";
        private bool _isVisible = true;
        private string _colorHex = "#4285F4";

        public string Name
        {
            get => _name;
            set { _name = value; NotifyPropertyChanged(); }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set { _isVisible = value; NotifyPropertyChanged(); }
        }

        public string ColorHex
        {
            get => _colorHex;
            set { _colorHex = value; NotifyPropertyChanged(); }
        }

        public List<double> Values { get; set; } = new List<double>();
        public string[] PointLabels { get; set; }
        public string GroupName { get; set; }
        public bool IsDashed { get; set; }
        public string Id { get; } = Guid.NewGuid().ToString();
        public int AxisIndex { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
