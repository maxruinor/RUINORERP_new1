namespace RUINORERP.Model.ChartFramework.Models
{
    /// <summary>
    /// 指标配置
    /// </summary>
    public class MetricConfig
    {
        public MetricConfig() { }

        public MetricConfig(string fieldName, string displayName, MetricUnit unit)
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

    /// <summary>
    /// 指标类型枚举
    /// </summary>
    public enum MetricType
    {
        Count,
        Sum,
        Avg,
        Max,
        Min
    }

    /// <summary>
    /// 指标单位枚举
    /// </summary>
    public enum MetricUnit
    {
        Count,      // 笔
        Amount,     // 元
        Quantity,   // 个
        Person      // 人
    }
}
