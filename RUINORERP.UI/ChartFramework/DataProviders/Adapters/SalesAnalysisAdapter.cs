using RUINORERP.UI.ChartAnalyzer;
using RUINORERP.UI.ChartFramework.Core;
using RUINORERP.UI.ChartFramework.Models;
using RUINORERP.UI.ChartFramework.Models.ChartFramework.Core.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.DataProviders.Adapters
{
    // Adapters/Sales/SalesAnalysisAdapter.cs
    public class SalesAnalysisAdapter : BaseChartAdapter
    {
        public SalesAnalysisAdapter(ISqlSugarClient db) : base(db) { }

        protected override ChartDataSet TransformToChartDataSet(List<dynamic> rawData, ChartRequest request)
        {
            var dataSet = new ChartDataSet
            {
                Title = "销售分析",
                MetaData = new ChartMetaData
                {
                    SuggestedChartType = ChartType.Column,
                    PrimaryLabels = rawData.Select(x => FormatLabel(x, request)).Distinct().ToArray()
                }
            };

            // 按指标分组
            foreach (var metric in request.Metrics)
            {
                dataSet.Series.Add(new ChartSeries
                {
                    Name = GetMetricName(metric),
                    Values = rawData.Select(x => (double)x[metric]).ToList()
                });
            }

            return dataSet;
        }
        public ChartDataSet ConvertSalesData(List<SalesRecord> records)
        {
            var dataSet = new ChartDataSet
            {
                MetaData = new ChartMetaData()
            };

            // 从销售日期生成标签
            dataSet.MetaData.CategoryLabels = LabelGenerator.CreateCategoryLabels(
                records,
                r => r.SaleDate,
                new ShortDateLabelFormat()
            );

            // 自动推断标签类型
            dataSet.MetaData.InferLabelType(records.Select(r => r.SaleDate));

            return dataSet;
        }

        // 自定义日期格式
        public class ShortDateLabelFormat : LabelFormat
        {
            public override string FormatDateTime(DateTime value)
                => value.ToString("MM/dd");

            public override string FormatNumber(double value)
                => value.ToString("N0");
        }
        private string FormatLabel(dynamic item, ChartRequest request)
        {
            if (request.Dimensions.Contains("Created_at"))
            {
                DateTime dt = item.Created_at;
                return request.TimeGroupType switch
                {
                    TimeRangeType.Daily => dt.ToString("yyyy-MM-dd"),
                    TimeRangeType.Monthly => dt.ToString("yyyy-MM"),
                    _ => dt.ToString()
                };
            }
            return item.ToString();
        }
    }
}
