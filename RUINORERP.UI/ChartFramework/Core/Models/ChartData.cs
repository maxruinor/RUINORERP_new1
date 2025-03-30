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

//    var metaData = new ChartMetaData();
//    metaData.SetExtension("XAxisName", "季度");
//metaData.SetExtension("YAxisName", "销售额");

    /// <summary>
    /// 图表数据集（支持多图表类型和多维数据）
    /// </summary>
    public  class ChartData : INotifyPropertyChanged
    {
        /// <summary>
        /// 主分类标签（用于X轴或主分类）
        /// 折线图/柱状图：X轴标签
        /// 饼图：通常不使用（每个扇区有自己的标签）
        /// </summary>
        public string[] PrimaryLabels { get; set; } = Array.Empty<string>();

        /// <summary>
        /// 次级分类标签（用于分组/堆叠）
        /// 折线图/柱状图：系列内的分组标签
        /// 饼图：通常不使用
        /// </summary>
        public string[] SecondaryLabels { get; set; } = Array.Empty<string>();

        /// <summary>
        /// 数据系列集合
        /// </summary>
        public IList<DataSeries> Series { get; set; } = new List<DataSeries>();

 

        // 元数据容器
        public ChartMetaData MetaData { get; set; } = new();



        private ChartType _chartType = ChartType.Line;
        /// <summary>图表类型</summary>
        public ChartType ChartType
        {
            get => _chartType;
            set { _chartType = value; NotifyPropertyChanged(); }
        }

        /// <summary>
        /// 是否为堆叠图表
        /// </summary>
        private bool _isStacked;
        /// <summary>是否为堆叠图表</summary>
        public bool IsStacked
        {
            get => _isStacked;
            set { _isStacked = value; NotifyPropertyChanged(); }
        }

        /// <summary>
        /// 是否显示数据标签
        /// </summary>
        public bool ShowDataLabels { get; set; } = true;

 
        private ValueType _valueType = ValueType.Absolute;

        /// <summary>数值类型（影响格式化）</summary>
        public ValueType ValueType
        {
            get => _valueType;
            set { _valueType = value; NotifyPropertyChanged(); }
        }

        /// <summary>X轴/分类轴标签</summary>
        public string[] Labels { get; set; } = Array.Empty<string>();


        /// <summary>原始数据（可选）</summary>
        public List<Dictionary<string, object>> RawData { get; set; } = new();



   


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 多维度数据支持（三维及以上数据）
        /// Key: 维度名称, Value: 维度值数组
        /// </summary>
        public Dictionary<string, string[]> MultiDimensions { get; set; } = new Dictionary<string, string[]>();


      
        private string _title = "图表标题";
        /// <summary>图表标题</summary>
        public string Title
        {
            get => _title;
            set { _title = value; NotifyPropertyChanged(); }
        }


        // 导出方法
        public DataTable ToDataTable()
        {

            return new DataTable();
        }
    }





    /*    
     *    public class VirtualChartDataSet : ChartDataSet
    {
        //private readonly Dictionary<int, DataChunk> _loadedChunks = new();

        public VirtualChartDataSet(int totalCount)
        {
            MetaData.MaxDataPoints = totalCount;
        }

        public void UpdateChunk(int chunkIndex, DataChunk chunk)
        {
            _loadedChunks[chunkIndex] = chunk;
            // 触发局部更新事件...
        }

        // 重写索引器实现按需加载
        //public override double GetValue(int seriesIndex, int pointIndex)
        //{
        //    var chunkIndex = pointIndex / _chunkSize;
        //    if (!_loadedChunks.TryGetValue(chunkIndex, out var chunk))
        //    {
        //        // 触发异步加载
        //        _ = LoadChunkAsync(chunkIndex);
        //        return double.NaN; // 临时返回空值
        //    }
        //    return chunk[seriesIndex, pointIndex % _chunkSize];
        //}
    }
    */

    public class ChartMetaData : INotifyPropertyChanged
    {
        private readonly Dictionary<string, object> _extensions = new();
        public T GetExtension<T>(string key)
        {
            return _extensions.TryGetValue(key, out var value) ? (T)value : default;
        }

        public void SetExtension<T>(string key, T value)
        {
            _extensions[key] = value;
        }
        public ChartType ChartType { get; set; } = ChartType.Line;
        public bool IsStacked { get; set; }
 
        public string Title { get; set; }
        public string[] Labels { get; set; } = Array.Empty<string>();
        // 数据特征
        public ValueType ValueType { get; set; }

        // 实现INotifyPropertyChanged...

        public event PropertyChangedEventHandler? PropertyChanged;

        public virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string[] _categoryLabels = Array.Empty<string>();
        /// <summary>
        /// 主分类标签（用于X轴或饼图扇形标签）
        /// </summary>
        public string[] CategoryLabels
        {
            get => _categoryLabels;
            set
            {
                _categoryLabels = value ?? Array.Empty<string>();
                NotifyPropertyChanged();
            }
        }
  
        /// <summary>
        /// 自动生成的标签类型（根据数据推断）
        /// </summary>
        public LabelType InferredLabelType { get; private set; } = LabelType.Text;

        // 智能推断标签类型
        public void InferLabelType(IEnumerable<object> sampleValues)
        {
            if (sampleValues == null || !sampleValues.Any())
            {
                InferredLabelType = LabelType.Text;
                return;
            }

            var firstValue = sampleValues.First();
            InferredLabelType = firstValue switch
            {
                DateTime _ => LabelType.DateTime,
                int _ => LabelType.Numeric,
                double _ => LabelType.Numeric,
                _ => LabelType.Text
            };
        }

        public enum LabelType { Text, DateTime, Numeric }
        // 显示相关
        public string[] PrimaryLabels { get; set; } = Array.Empty<string>();
        public string[] SecondaryLabels { get; set; } = Array.Empty<string>();

        // 图表配置
        public ChartType SuggestedChartType { get; set; }

        public bool Is100PercentStack { get; set; }

        public int MaxDataPoints { get; set; } = 1000;

        // 扩展槽
        public Dictionary<string, object> Extensions { get; } = new();
    }

}
