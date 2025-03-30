using RUINORERP.UI.ChartFramework.Core;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.Core.Models
{
    /// <summary>
    /// 图表数据系列（增强版）
    /// </summary>
    public class DataSeries
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

        public SKColor Color => ColorHex.ToSKColor();
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

    public class LineSeries : DataSeries
    { 
        private double _lineSmoothness = 0.5;
        private bool _showMarkers = true;
        private float _lineWidth = 2f;
        private LineType _lineType = LineType.Solid;

        /// <summary>线条平滑度（0-1）</summary>
        public double LineSmoothness
        {
            get => _lineSmoothness;
            set { _lineSmoothness = value; NotifyPropertyChanged(); }
        }

        /// <summary>是否显示数据点标记</summary>
        public bool ShowMarkers
        {
            get => _showMarkers;
            set { _showMarkers = value; NotifyPropertyChanged(); }
        }

        /// <summary>线条宽度</summary>
        public float LineWidth
        {
            get => _lineWidth;
            set { _lineWidth = value; NotifyPropertyChanged(); }
        }

        /// <summary>线条类型</summary>
        public LineType LineType
        {
            get => _lineType;
            set { _lineType = value; NotifyPropertyChanged(); }
        }
    }
    public class ColumnSeries : DataSeries
    {
        private double _columnWidth = 0.8;
        private double _cornerRadius = 4;

        /// <summary>柱宽比例（0-1）</summary>
        public double ColumnWidth
        {
            get => _columnWidth;
            set { _columnWidth = value; NotifyPropertyChanged(); }
        }

        /// <summary>柱角半径</summary>
        public double CornerRadius
        {
            get => _cornerRadius;
            set { _cornerRadius = value; NotifyPropertyChanged(); }
        }
        public bool IsStackedSeries { get; set; } // 是否参与堆叠
    }
    public class PieSeries : DataSeries
    {
        private double _innerRadius;
        private double _pushout;

        /// <summary>内半径（用于环形图）</summary>
        public double InnerRadius
        {
            get => _innerRadius;
            set { _innerRadius = value; NotifyPropertyChanged(); }
        }

        /// <summary>扇形突出距离</summary>
        public double Pushout
        {
            get => _pushout;
            set { _pushout = value; NotifyPropertyChanged(); }
        }
    }
    public class DataPoint
    {
        public object XValue { get; set; }
        public double YValue { get; set; }
        public string Label { get; set; }
        public object RawValue { get; set; }
    }
}

