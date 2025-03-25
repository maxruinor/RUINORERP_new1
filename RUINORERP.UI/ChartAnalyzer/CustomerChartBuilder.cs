using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveChartsCore.Drawing;

namespace RUINORERP.UI.ChartAnalyzer
{
    public class CustomerChartBuilder : ChartBuilderBase
    {
        public CustomerChartBuilder(IChartDataSource dataSource) : base(dataSource)
        {
        }

        // 覆盖X轴创建方法...
        protected override Axis[] CreateXAxes(ChartDataSet data)
        {
            var baseAxes = base.CreateXAxes(data);
            baseAxes[0].Name = "区域分布";

        //    return new[] {
        //    new Axis {
        //        Labels = data.Labels,
        //        LabelsRotation = 45,
        //        TextSize = 12
        //    }
        //};


            return baseAxes;
        }

        // 覆盖Y轴格式化方法...
        protected override string FormatYValue(double value)
        {
            return $"{base.FormatYValue(value)} 家"; // 添加单位
        }

        // 实现抽象方法...
        protected override IEnumerable<ISeries> CreateSeries(ChartDataSet data)
        {
            yield return new ColumnSeries<double>
            {
                Name = "客户数量",
                Values = data.SeriesData["Count"],
                Fill = new SolidColorPaint(SKColor.Parse("#4CAF50")),
                XToolTipLabelFormatter = point => $"{point.StackedValue:N0} 家客户"
            };
        }

  

        // 使用后期处理钩子...
        protected override void PostBuildProcessing()
        {
            _chart.Title = new LabelVisual
            {
                Text = "客户分布统计",
                TextSize = 16,
                Padding = new Padding(15),
                Paint = new SolidColorPaint(SKColors.DarkSlateGray)
            };
        }
    }
}
