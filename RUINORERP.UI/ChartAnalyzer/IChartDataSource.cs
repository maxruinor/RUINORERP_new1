using LiveChartsCore.SkiaSharpView.SKCharts;
using LiveChartsCore.SkiaSharpView.WinForms;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace RUINORERP.UI.ChartAnalyzer
{
    /// <summary>
    /// 图表数据源通用接口
    /// </summary>
    public interface IChartDataSource
    {
        /// <summary>
        /// 获取可用维度配置
        /// </summary>
        IEnumerable<DimensionConfig> GetDimensions();

        /// <summary>
        /// 获取可用指标配置
        /// </summary>
        IEnumerable<MetricConfig> GetMetrics();

        /// <summary>
        /// 获取图表数据
        /// </summary>
        /// <param name="request">图表请求参数</param>
        Task<ChartDataSet> GetDataAsync(ChartRequest request);
    }

    /// <summary>
    /// 维度类型
    /// </summary>
    public enum DimensionType
    {
        String,
        DateTime
    }

    public enum ChartType
    {
        Line,
        Column,
        Pie
    }

    public enum MetricType
    {
        Count //个数
    }

    /// <summary>
    /// 可用维度配置
    /// </summary>
    public class DimensionConfig
    {
        public DimensionConfig(string v1, string v2, object @string)
        {
            V1 = v1;
            V2 = v2;
            String = @string;
        }

        public string V1 { get; }
        public string V2 { get; }
        public object String { get; }
    }


    /// <summary>
    /// 可用指标配置
    /// </summary>
    public class MetricConfig
    {
        
     

        public MetricConfig(string _DisplayName, MetricType _MetricType, SKColor _Color )
        {
            this.DisplayName = _DisplayName;
            this.MetricType = _MetricType;
            this.Color = _Color;
        }

        /// <summary>
        ///  Line Column Pie
        /// </summary>
        public string ChartType { get; set; }
        public string DisplayName { get; internal set; }

        public MetricType MetricType { get; set; }
        public SKColor Color { get; internal set; }
    }

}
