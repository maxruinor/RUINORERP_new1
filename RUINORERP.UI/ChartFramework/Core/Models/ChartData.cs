using OfficeOpenXml.Drawing.Chart;
using RUINORERP.UI.ChartAnalyzer;
using RUINORERP.UI.ChartFramework.Core;
using RUINORERP.UI.ChartFramework.Core.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ValueType = RUINORERP.UI.ChartFramework.Core.ValueType;

namespace RUINORERP.UI.ChartFramework.Models
{
    /// <summary>
    /// 图表数据容器（支持多种图表类型）
    /// </summary>
    public class ChartData : INotifyPropertyChanged
    {
        private string _title = "图表标题";
        private ChartType _chartType = ChartType.Line;
        private bool _isStacked;
        private ValueType _valueType = ValueType.Absolute;

        public string SubTitle { get; set; }

        /// <summary>图表标题</summary>
        public string Title
        {
            get => _title;
            set { _title = value; NotifyPropertyChanged(); }
        }

        /// <summary>图表类型</summary>
        public ChartType ChartType
        {
            get => _chartType;
            set { _chartType = value; NotifyPropertyChanged(); }
        }

        /// <summary>是否为堆叠图表</summary>
        public bool IsStacked
        {
            get => _isStacked;
            set { _isStacked = value; NotifyPropertyChanged(); }
        }

        /// <summary>数值类型（影响格式化）</summary>
        public ValueType ValueType
        {
            get => _valueType;
            set { _valueType = value; NotifyPropertyChanged(); }
        }

        /// <summary>X轴/分类轴标签</summary>
        public string[] CategoryLabels { get; set; } = Array.Empty<string>();

        /// <summary>数据系列集合</summary>
        public IList<DataSeries> Series { get; set; } = new List<DataSeries>();

        /// <summary>原始数据（可选）</summary>
        public List<Dictionary<string, object>> RawData { get; set; } = new();

        /// <summary>是否显示数据标签</summary>
        public bool ShowDataLabels { get; set; } = true;

        /// <summary>元数据容器</summary>
        public ChartMetaData MetaData { get; set; } = new();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>转换为DataTable</summary>
        public DataTable ToDataTable()
        {
            var dt = new DataTable();
            // 实现转换逻辑...
            return dt;
        }

        /// <summary>
        /// 图表注解集合（用于饼图中心标签、自定义标注等）
        /// </summary>
        public IList<ChartAnnotation> Annotations { get; set; } = new List<ChartAnnotation>();

        /// <summary>
        /// 饼图专用配置
        /// </summary>
        public PieChartOptions PieOptions { get; set; } = new PieChartOptions();

    

    }
    /// <summary>
    /// 图表注解定义
    /// </summary>
    public class ChartAnnotation
    {
        /// <summary>注解文本</summary>
        public string Text { get; set; }

        /// <summary>显示位置</summary>
        public AnnotationPosition Position { get; set; } = AnnotationPosition.Center;

        /// <summary>字体大小</summary>
        public double FontSize { get; set; } = 12;

        /// <summary>字体颜色（十六进制）</summary>
        public string ColorHex { get; set; } = "#333333";
    }

    /// <summary>
    /// 饼图专用配置项
    /// </summary>
    public class PieChartOptions
    {
        /// <summary>是否显示中心总数值</summary>
        public bool ShowTotalInCenter { get; set; } = true;

        /// <summary>中心标签格式（如"{Total}项\n{Percent}%"）</summary>
        public string CenterLabelFormat { get; set; } = "{Total}";

        /// <summary>饼图内半径（0-1，用于环形图）</summary>
        public double InnerRadius { get; set; } = 0;

        /// <summary>起始角度（0-360）</summary>
        public double StartAngle { get; set; } = 0;
    }

    /// <summary>
    /// 注解位置枚举
    /// </summary>
    public enum AnnotationPosition
    {
        Center,
        Top,
        Bottom,
        Left,
        Right,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }
    /// <summary>
    /// 图表元数据容器
    /// </summary>
    public class ChartMetaData : INotifyPropertyChanged
    {
        private readonly Dictionary<string, object> _extensions = new();

        /// <summary>获取扩展属性</summary>
        public T GetExtension<T>(string key) =>
            _extensions.TryGetValue(key, out var value) ? (T)value : default;

        /// <summary>设置扩展属性</summary>
        public void SetExtension<T>(string key, T value) =>
            _extensions[key] = value;

        /// <summary>推断的标签类型</summary>
        public LabelType InferredLabelType { get; private set; } = LabelType.Text;

        /// <summary>是否为100%堆叠</summary>
        public bool Is100PercentStack { get; set; }

        /// <summary>最大数据点数</summary>
        public int MaxDataPoints { get; set; } = 1000;

        /// <summary>X轴名称</summary>
        public string XAxisName { get; set; }

        /// <summary>Y轴名称</summary>
        public string YAxisName { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>智能推断标签类型</summary>
        public void InferLabelType(IEnumerable<object> sampleValues)
        {
            if (sampleValues == null || !sampleValues.Any())
            {
                InferredLabelType = LabelType.Text;
                return;
            }

            InferredLabelType = sampleValues.First() switch
            {
                DateTime _ => LabelType.DateTime,
                int _ => LabelType.Numeric,
                double _ => LabelType.Numeric,
                _ => LabelType.Text
            };
        }

        public enum LabelType { Text, DateTime, Numeric }
    }
}





