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
        public MetricConfig()
        {
                
        }

        public MetricConfig(string fieldName, string displayName,  MetricUnit unit)
        {
            FieldName = fieldName;
            DisplayName = displayName;
            Unit = unit;

        }
        public MetricConfig(string fieldName, string displayName, MetricType metricType, MetricUnit unit)
        {
            FieldName = fieldName;
            DisplayName = displayName;
            MetricType = metricType;
            Unit = unit;
        }

        public ChartType ChartType { get; set; }
        public string DisplayName { get; set; }
        public string FieldName { get; set; }
        public MetricType MetricType { get; set; }
        public MetricUnit Unit { get; set; }
    }

}
