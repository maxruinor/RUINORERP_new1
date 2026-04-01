using RUINORERP.Model.ChartFramework.Models;
using RUINORERP.UI.ChartFramework.Adapters;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RUINORERP.UI.ChartFramework.DataProviders.Adapters.Sales
{
    /// <summary>
    /// 销售分析适配器
    /// </summary>
    public class SalesAnalysisAdapter : BaseChartAdapter
    {
        public SalesAnalysisAdapter(ISqlSugarClient db) : base(db) { }

        protected override string PrimaryTableName => "tb_Sales";

        public override IEnumerable<DimensionConfig> GetDimensions() => new[]
        {
            new DimensionConfig("Employee_ID", "业务员", DimensionType.String),
            new DimensionConfig("Customer_ID", "客户", DimensionType.String),
            new DimensionConfig("SaleDate", "销售日期", DimensionType.DateTime)
        };

        public override IEnumerable<MetricConfig> GetMetrics() => new[]
        {
            new MetricConfig("Amount", "销售额", MetricType.Sum, MetricUnit.Amount),
            new MetricConfig("Quantity", "销售数量", MetricType.Sum, MetricUnit.Quantity),
            new MetricConfig("Count", "订单数", MetricType.Count, MetricUnit.Count)
        };

        protected override ChartData TransformToChartData(List<dynamic> rawData, DataRequest request)
        {
            var chartData = new ChartData
            {
                Title = "销售分析",
                ChartType = ChartType.Column,
                ValueType = RUINORERP.Model.ChartFramework.Models.ValueType.Absolute
            };

            // 处理时间维度作为 X 轴标签
            if (request.Dimensions.Any(d => d.Type == DimensionType.DateTime))
            {
                chartData.CategoryLabels = rawData
                    .Select(x => FormatTimeLabel(x.TimeGroup, request))
                    .Distinct()
                    .OrderBy(x => x)
                    .Cast<string>()
                    .ToArray();
            }

            // 按指标创建系列
            foreach (var metric in request.Metrics)
            {
                var series = new DataSeries
                {
                    Name = GetMetricName(metric.FieldName),
                    Values = rawData.Select(x => (double)x[metric.FieldName]).ToList()
                };
                chartData.Series.Add(series);
            }

            return chartData;
        }

        private string FormatTimeLabel(object value, DataRequest request)
        {
            if (value is DateTime dt)
            {
                return request.RangeType switch
                {
                    TimeRangeType.Daily => dt.ToString("yyyy-MM-dd"),
                    TimeRangeType.Monthly => dt.ToString("yyyy-MM"),
                    TimeRangeType.Quarterly => $"Q{(dt.Month - 1) / 3 + 1}-{dt.Year}",
                    TimeRangeType.Yearly => dt.ToString("yyyy"),
                    _ => dt.ToString("yyyy-MM-dd")
                };
            }
            return value?.ToString() ?? "未知";
        }

        private string GetMetricName(string metricKey)
        {
            return metricKey switch
            {
                "Amount" => "销售额",
                "Quantity" => "销售数量",
                "Count" => "订单数",
                _ => metricKey
            };
        }
    }
}

