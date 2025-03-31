using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.SkiaSharpView.WinForms;
using RUINORERP.UI.ChartAnalyzer;
using RUINORERP.UI.ChartFramework.Adapters;
using RUINORERP.UI.ChartFramework.Core.Contracts;
using RUINORERP.UI.ChartFramework.Core.Models;
using RUINORERP.UI.ChartFramework.Extensions.Theming;
using RUINORERP.UI.ChartFramework.Models;
using RUINORERP.UI.ChartFramework.Rendering.Controls;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.Core.Rendering.Builders
{
    /// <summary>
    /// 图表构建器基类
    /// </summary>
    public abstract class ChartBuilderBase<TChart> : IChartBuilder<TChart>
    where TChart : IChartView
    {
        protected TChart Chart { get; private set; }
        protected ChartData CurrentData { get; private set; }

        protected DataRequest _currentRequest;
        protected CartesianChart _chart;
        protected readonly IDataProvider _dataSource;
        public ChartBuilderBase(DataRequest request, IDataProvider dataSource)
        {
            _dataSource = dataSource;
            _currentRequest = request;
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
                Tooltip = new DefaultTooltip
                {
                    TextSize = 16,
                    BackgroundColor = SKColors.DarkSlateGray
                    //BackgroundColor = SKColors.WhiteSmoke,
                },

                LegendPosition = LiveChartsCore.Measure.LegendPosition.Right,
                EasingFunction = EasingFunctions.QuadraticOut
            };
        }
        // 添加事件实现实现图表元素的交互功能（如高亮、工具提示等）
        public event EventHandler<ChartInteractionEventArgs> OnInteraction;

        // 触发事件的方法
        protected virtual void RaiseInteraction(ChartInteractionEventArgs e)
        {
            OnInteraction?.Invoke(this, e);
        }


        public abstract Task<ChartControl> BuildChart(DataRequest data);
        public abstract Task<IChartView> BuildChartControl();
        public virtual void UpdateData(ChartData newData)
        {
            CurrentData = newData;
            // 实现增量更新逻辑
        }


        // 在具体构建器中绑定图表事件
        protected void BindChartEvents(TChart chart)
        {
            if (chart is CartesianChart cartesianChart)
            {
                cartesianChart.MouseDown += (sender, e) =>
                {
                    //var interactionType = GetInteractionType(e);
                    //RaiseInteractionForChartPoint(e, interactionType);
                };
                cartesianChart.DataPointerDown += (sender, e) =>
                {
                    // var interactionType = GetInteractionType(e);
                    //RaiseInteractionForChartPoint(e, interactionType);
                };

            }
            else if (chart is PieChart pieChart)
            {
                pieChart.DataPointerDown += (sender, e) =>
                {
                    //RaiseInteractionForPiePoint(e);
                };
            }
        }

        private void RaiseInteractionForPiePoint(
   ChartPoint e)
        {
            var chartPoint = e;
            var dataPoint = new DataPoint
            {
                // XValue = chartPoint.SecondaryValue,
                // YValue = chartPoint.PrimaryValue,
                Label = chartPoint.Context.Series.Name ?? string.Empty,
                Tag = chartPoint.Context.Series.Tag
            };

            var series = chartPoint.Context.Series;

            //RaiseInteraction(new ChartInteractionEventArgs(
            //    dataPoint,
            //    series,
            //    InteractionType.Click));

        }
        // 获取交互类型（简化版，实际可根据需要扩展）
        private InteractionType GetInteractionType(ChartPoint e)
        {
            // 注意：LiveCharts2 2.0+版本中不直接提供按钮信息
            // 可通过其他方式判断右键点击（如结合MouseDown事件）

            return InteractionType.Click;
        }




        /// <summary>
        /// 可重写的坐标轴创建模板方法
        /// </summary>
        protected virtual Axis[] CreateXAxes(ChartData data)
        {
            return new[]
            {
            new Axis
            {
                Labels = data.CategoryLabels, // 使用MetaData中的分类标签
                LabelsRotation = 30,
                TextSize = 12,
                SeparatorsPaint = new SolidColorPaint(SKColors.LightGray.WithAlpha(100)),
                //NamePaint = new SolidColorPaint(SKColors.DarkGray)
                 NamePaint = CurrentTheme.AxisTextPaint
            }
        };
        }


        protected virtual Axis[] CreateYAxes(ChartData data)
        {
            return new[] { new Axis { Labeler = value => value.ToString("N0") } };
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
        protected abstract IEnumerable<ISeries> CreateSeries(ChartData data);

        protected virtual LabelVisual[] CreateVisualElements(ChartData data)
        {
            return new[]
            {
        new LabelVisual
        {
            Text = data.Title,
            TextSize = 16,
            Padding = new LiveChartsCore.Drawing.Padding(15),
            Paint = new SolidColorPaint(SKColors.DarkSlateGray),
            HorizontalAlignment = LiveChartsCore.Drawing.Align.Start,
            VerticalAlignment = LiveChartsCore.Drawing.Align.Start
        }
    };
        }



        protected ChartTheme CurrentTheme { get; private set; } = ChartTheme.DefaultLight;

        public virtual void ApplyTheme(ChartTheme theme)
        {
            CurrentTheme = theme;
        }



    }
}
