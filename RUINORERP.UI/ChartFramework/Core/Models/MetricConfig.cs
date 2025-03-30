using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.Core.Models
{
    /// <summary>
    /// 指标配置
    /// </summary>
    public class MetricConfig
    {
        public MetricConfig(string fieldName, string displayName, MetricType metricType, SKColor color)
        {
            DataFieldName = fieldName;
            DisplayName = displayName;
            MetricType = metricType;
            Color = color;
        }

        public ChartType ChartType { get; set; }
        public string DisplayName { get; set; }
        public string DataFieldName { get; set; }
        public MetricType MetricType { get; set; }
        public SKColor Color { get; set; }
    }

}
