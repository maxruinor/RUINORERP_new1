using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.SkiaSharpView.WinForms;
using RUINORERP.UI.ChartFramework.Models;
using SkiaSharp;
using RUINORERP.UI.ChartFramework.Core.Rendering.Builders;
using RUINORERP.UI.ChartFramework.Core.Contracts;
using RUINORERP.UI.ChartFramework.Rendering.Controls;
using RUINORERP.UI.ChartFramework.Shared.Extensions.UI;
using RUINORERP.UI.ChartFramework.Core.Models;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView.SKCharts;

namespace RUINORERP.UI.ChartFramework.Core.Rendering.Builders
{
    /*
     // 柱状图使用
var columnBuilder = new ColumnChartBuilder();
var columnChart = columnBuilder.Build(salesData);

// 饼图使用
var pieBuilder = new PieChartBuilder();
var pieChart = pieBuilder.Build(statusDistributionData)
.WithDonutOptions(true); // 启用环形图模式
     */
    public class ColumnChartBuilder : ChartBuilderBase<CartesianChart>
    {
        public ColumnChartBuilder(IDataProvider dataSource) : base(dataSource) { }

        public ChartControl Build(ChartData data)
        {
            var chart = new CartesianChart
            {
                Series = CreateSeries(data),
                XAxes = CreateXAxes(data),
                YAxes = CreateYAxes(data),
                Title = CreateTitle(data),
                VisualElements = CreateVisualElements(data)
            };

            return new ChartControl(chart, data)
                .AddExportMenu()
                .WithStackedOptions(data.MetaData.IsStacked);
        }

        protected override IEnumerable<ISeries> CreateSeries(ChartData data)
        {
            return data.Series.Select((series, index) => new ColumnSeries<double>
            {
                Name = series.Name,
                Values = series.Values.ToArray(),
                Fill = new SolidColorPaint(series.Style.ColorHex.ToSKColor()),
                Stroke = null,
                Rx = 3, // 圆角半径
                Ry = 3,
                MaxBarWidth = 30,
                DataLabelsPaint = new SolidColorPaint(SKColors.White),
                DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top,
                DataLabelsFormatter = point => FormatValue(point.Model, data)
            }).ToArray();
        }

        protected override Axis[] CreateXAxes(ChartData data)
        {
            return new[]
            {
            new Axis
            {
                Labels = data.MetaData.PrimaryLabels,
                LabelsRotation = 45,
                TextSize = 12
            }
        };
        }

        //protected override Axis[] CreateXAxes(ChartDataSet data)
        //{
        //    return AxisBuilder.BuildAxes(data.MetaData);
        //}


        protected override Axis[] CreateYAxes(ChartData data)
        {
            return new[]
            {
            new Axis
            {
                Labeler = value => FormatLabel(value, data),
                MinStep = data.MetaData.ValueType ==RUINORERP.UI.ChartFramework.Core.ValueType.Currency ? 100 : 1
            }
        };
        }
        private string FormatValue(double value, ChartData data)
        {
            return data.MetaData.ValueType switch
            {
                RUINORERP.UI.ChartFramework.Core.ValueType.Currency => value.ToString("C2"),
                RUINORERP.UI.ChartFramework.Core.ValueType.Percentage => value.ToString("P1"),
                _ => value.ToString("N0")
            };
        }

        private string FormatLabel(double value, ChartData data)
        {
            return data.MetaData.ValueType switch
            {
                Core.ValueType.Currency => (value / 1000).ToString("C0") + "K",
                Core.ValueType.Percentage => value.ToString("P0"),
                _ => value.ToString("N0")
            };
        }

        private LabelVisual CreateTitle(ChartData data)
        {
            return new LabelVisual
            {
                Text = data.Title,
                TextSize = 16,
                Padding = new LiveChartsCore.Drawing.Padding(15),
                Paint = new SolidColorPaint(SKColors.DarkSlateGray)
            };
        }

        public override IChartView BuildChartControl(ChartData data)
        {
            // 参数校验
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (data.Series == null || !data.Series.Any())
                throw new ArgumentException("图表数据必须包含至少一个数据系列", nameof(data.Series));

            // 创建基础图表
            var cartesianChart = new CartesianChart
            {
                // 设置图表标题
                Title = CreateChartTitle(data),

                // 设置图例位置
                LegendPosition = LiveChartsCore.Measure.LegendPosition.Right,

                // 设置动画效果
                AnimationsSpeed = TimeSpan.FromMilliseconds(800),
                EasingFunction = LiveChartsCore.EasingFunctions.BounceOut,

                // 设置边距
                //                Margin = new LiveChartsCore.Drawing.g .Margin(40, 20, 40, 40)
            };

            // 配置X轴
            var xAxis = new Axis
            {
                Labels = data.MetaData?.CategoryLabels ?? data.Labels,
                LabelsRotation = CalculateLabelRotation(data.MetaData?.CategoryLabels ?? data.Labels),
                TextSize = 12,
                SeparatorsPaint = new SolidColorPaint(SKColors.LightGray.WithAlpha(100)),
                Name = data.MetaData?.GetExtension<string>("XAxisName") ?? "分类"
            };

            // 配置Y轴
            var yAxis = new Axis
            {
                Labeler = value => FormatYValue(value, data.ValueType),
                MinStep = GetMinStep(data.ValueType),
                SeparatorsPaint = new SolidColorPaint(SKColors.LightGray.WithAlpha(100)),
                Name = data.MetaData?.GetExtension<string>("YAxisName") ?? "数值"
            };

            // 设置坐标轴
            cartesianChart.XAxes = new[] { xAxis };
            cartesianChart.YAxes = new[] { yAxis };

            // 添加数据系列
            cartesianChart.Series = CreateColumnSeries(data);

            // 配置工具提示
            cartesianChart.Tooltip = new DefaultTooltip
            {
                BackgroundColor = SKColors.WhiteSmoke,
                TextSize = 14
            };

            // 配置数据标签
            if (data.ShowDataLabels)
            {
                foreach (var series in cartesianChart.Series.Cast<ColumnSeries<double>>())
                {
                    series.DataLabelsPaint = new SolidColorPaint(SKColors.White);
                    series.DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top;
                    series.DataLabelsFormatter = point => FormatDataLabel(point, data.ValueType);
                }
            }

            return cartesianChart;
        }

        // 辅助方法：创建柱状图系列
        private ISeries[] CreateColumnSeries(ChartData data)
        {
            return data.Series.Select(series =>
            {
                var columnSeries = new ColumnSeries<double>
                {
                    Name = series.Name,
                    Values = series.Values.ToArray(),
                    Fill = new SolidColorPaint(series.ColorHex.ToSKColor()),
                    Stroke = null,
                    Rx = 4, // 圆角半径X
                    Ry = 4, // 圆角半径Y
                    MaxBarWidth = 30
                };

                // 设置堆叠属性
                //if (data.IsStacked)
                //{
                //    columnSeries.se = true; // 使用相同的StackGroup实现堆叠
                //}

                // 如果是ColumnSeries类型，应用特定配置
                if (series is ColumnSeries columnSeriesData)
                {
                    columnSeries.Rx = columnSeries.Ry = (float)columnSeriesData.CornerRadius;
                    columnSeries.MaxBarWidth = (float)(columnSeriesData.ColumnWidth * 50); // 转换为像素值
                }

                return columnSeries;
            }).ToArray();
        }

        // 辅助方法：创建图表标题
        private LabelVisual CreateChartTitle(ChartData data)
        {
            return new LabelVisual
            {
                Text = data.Title,
                TextSize = 18,
                Padding = new LiveChartsCore.Drawing.Padding(15),
                Paint = new SolidColorPaint(SKColors.DarkSlateGray),
                HorizontalAlignment = LiveChartsCore.Drawing.Align.Middle,
                VerticalAlignment = LiveChartsCore.Drawing.Align.Start
            };
        }

        // 辅助方法：计算标签旋转角度
        private double CalculateLabelRotation(string[] labels)
        {
            if (labels == null || !labels.Any()) return 0;

            var maxLength = labels.Max(l => l?.Length ?? 0);

            return maxLength switch
            {
                > 15 => 45,
                > 10 => 30,
                _ => 0
            };
        }

        // 辅助方法：格式化Y轴数值
        private string FormatYValue(double value, ValueType valueType)
        {
            return valueType switch
            {
                ValueType.Currency => (value / 1000).ToString("C0") + "K",
                ValueType.Percentage => value.ToString("P1"),
                ValueType.Scientific => value.ToString("E2"),
                _ => value.ToString("N0")
            };
        }

        // 辅助方法：格式化数据标签
        // 修正后的格式化方法
        private string FormatDataLabel(LiveChartsCore.Kernel.ChartPoint point, ValueType valueType)
        {
            // 获取实际数值
            var value = point.Coordinate.PrimaryValue;
            return valueType switch
            {
                ValueType.Currency => value.ToString("C2"),
                ValueType.Percentage => value.ToString("P1"),
                ValueType.Scientific => value.ToString("E2"),
                _ => value.ToString("N2")
            };
        }

        // 辅助方法：获取最小步长
        private double GetMinStep(ValueType valueType)
        {
            return valueType switch
            {
                ValueType.Currency => 100,
                ValueType.Percentage => 0.1,
                _ => 0
            };
        }

        public override ChartControl BuildChart(ChartData data)
        {
            throw new NotImplementedException();
        }
    }


}

