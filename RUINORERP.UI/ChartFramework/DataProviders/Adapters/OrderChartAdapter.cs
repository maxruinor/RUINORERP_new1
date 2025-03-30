using RUINORERP.UI.ChartFramework.Core;
using RUINORERP.UI.ChartFramework.Data.Abstract.ChartFramework.Data.Abstract;
using RUINORERP.UI.ChartFramework.Models;
using RUINORERP.UI.ChartFramework.Models.ChartFramework.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.DataProviders.Adapters
{
    // 订单业务适配器
    public class OrderChartAdapter : ChartDataSourceBase
    {
        protected override ChartDataSet TransformData(List<dynamic> rawData, ChartRequest request)
        {
            var dataSet = new ChartDataSet { Title = "订单分析" };

            // 按时间维度处理
            if (request.TimeGroupType != TimeRangeType.None)
            {
                dataSet.MetaData.PrimaryLabels = rawData
                    .Select(x => FormatTimeLabel(x.TimeGroup, request))
                    .ToArray();
            }

            // 处理多指标
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

        private string FormatTimeLabel(object value, ChartRequest request)
        {
            // 实现时间标签格式化
        }
    }
}
