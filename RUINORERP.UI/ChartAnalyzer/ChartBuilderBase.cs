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
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace RUINORERP.UI.ChartAnalyzer
{
    /// <summary>
    /// 图表构建器基类
    /// </summary>
    public abstract class ChartBuilderBase
    {
        protected readonly IChartDataSource _dataSource;
        protected ChartRequest _currentRequest;
        protected CartesianChart _chart;

        protected ChartBuilderBase(IChartDataSource dataSource)
        {
            _dataSource = dataSource;
            InitializeChart();
        }
 

        /// <summary>
        /// 可重写的初始化方法
        /// </summary>
        protected virtual void InitializeChart()
        {
            _chart = new CartesianChart
            {
                AnimationsSpeed = TimeSpan.FromMilliseconds(300),
                //Tooltip = new DefaultTooltip(),
                Tooltip = new DefaultTooltip
                {
                    TextSize = 16,
                    BackgroundColor = SKColors.DarkSlateGray
                },

                LegendPosition = LiveChartsCore.Measure.LegendPosition.Right,
                EasingFunction = EasingFunctions.QuadraticOut
            };
        }

        /// <summary>
        /// 构建图表主方法
        /// </summary>
        public async Task<CartesianChart> BuildChartAsync(ChartRequest request)
        {
            _currentRequest = request;
            var dataSet = await _dataSource.GetDataAsync(request);

            _chart.XAxes = CreateXAxes(dataSet);
            _chart.YAxes = CreateYAxes(dataSet);
            _chart.Series = CreateSeries(dataSet);

            return _chart;
        }

        //protected virtual Axis[] CreateXAxes(ChartDataSet data)
        //{
        //    return new[] { new Axis { Labels = data.Labels } };
        //}

        /// <summary>
        /// 可重写的坐标轴创建模板方法
        /// </summary>
        protected virtual Axis[] CreateXAxes(ChartDataSet data)
        {
            return new[]
            {
            new Axis
            {
                Labels = data.Labels,
                LabelsRotation = 30,
                TextSize = 12,
                SeparatorsPaint = new SolidColorPaint(SKColors.LightGray.WithAlpha(100)),
                NamePaint = new SolidColorPaint(SKColors.DarkGray)
            }
        };
        }


        //protected virtual Axis[] CreateYAxes(ChartDataSet data)
        //{
        //    return new[] { new Axis { Labeler = value => value.ToString("N0") } };
        //}

        /// <summary>
        /// 可重写的Y轴创建方法
        /// </summary>
        protected virtual Axis[] CreateYAxes(ChartDataSet data)
        {
            return new[]
            {
            new Axis
            {
                Labeler = value => FormatYValue(value),
                ShowSeparatorLines = true,
                SeparatorsPaint = new SolidColorPaint(SKColors.LightGray.WithAlpha(100)),
                NamePaint = new SolidColorPaint(SKColors.DarkGray)
            }
        };
        }

        /// <summary>
        /// 可扩展的Y轴数值格式化方法
        /// </summary>
        protected virtual string FormatYValue(double value)
        {
            return value switch
            {
                > 1000000 => $"{value / 1000000:N1}M",
                > 1000 => $"{value / 1000:N1}K",
                _ => value.ToString("N0")
            };
        }

        /// <summary>
        /// 可重写的系列生成策略
        /// </summary>
        protected abstract IEnumerable<ISeries> CreateSeries(ChartDataSet data);

        /// <summary>
        /// 可选的后期处理钩子方法
        /// </summary>
        protected virtual void PostBuildProcessing()
        {
            // 默认空实现，子类可按需覆盖
        }
        


    }


    /// <summary>
    /// 图表请求参数
    /// </summary>
    //public class ChartRequest
    //{
    //    public string[] Dimensions { get; set; }
    //    public string[] Metrics { get; set; }
    //    public Filter[] Filters { get; set; }
    //    public TimeRangeType TimeRange { get; set; }
    //    public string ChartType { get; set; }
    //    public Dictionary<string, object> Options { get; set; }
    //}
   
}

 

