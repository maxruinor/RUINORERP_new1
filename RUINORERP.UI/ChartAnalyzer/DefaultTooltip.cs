using LiveChartsCore.SkiaSharpView.Drawing;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveChartsCore.SkiaSharpView.WinForms;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView.SKCharts;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Sketches;

namespace RUINORERP.UI.ChartAnalyzer
{
    /// <summary>
    /// 自定义默认工具提示（支持多指标显示）
    /// </summary>
    public class DefaultTooltip : SKDefaultTooltip
    {
        /// <summary>
        /// 字体大小
        /// </summary>
        public float TextSize { get; set; } = 14;

        /// <summary>
        /// 背景颜色
        /// </summary>
        public SKColor BackgroundColor { get; set; } = new SKColor(45, 45, 45);

        /*
        /// <summary>
        /// 虚方法：创建提示框内容
        /// </summary>
        protected virtual IEnumerable<ChartPointInfo> BuildContent(TooltipPoint points)
        {
            var list = new List<ChartPointInfo>();

            // 主指标（第一个指标）
            var primary = points.First();
            list.Add(new ChartPointInfo(
                $"分类: {primary.Model}",
                SKColors.White));

            // 各指标数值
            foreach (var point in points)
            {
                var value = FormatValue(point.Context.Series.Name, point.PrimaryValue);
                list.Add(new ChartPointInfo(
                    $"{point.Context.Series.Name}: {value}",
                    point.Context.Series.GetColor()));
            }

            return list;
        }
        */

        /// <summary>
        /// 创建提示框内容
        /// </summary>
        private List<ChartPointInfo> BuildContent(IEnumerable<ChartPoint> points)
        {
            var list = new List<ChartPointInfo>();

            // 主指标（第一个指标）
            //var primary = points.First();

            //list.Add(new ChartPointInfo(
            //    $"分类: {primary.Context.Series.Name}",
            //    SKColors.White));

            // 主分类（取第一个点的X轴值）
            var primaryPoint = points.First();
            list.Add(new ChartPointInfo(
                $"分类: {primaryPoint.StackedValue}", // 使用SecondaryValue获取X轴值
                SKColors.White));

            //  各指标数值
            //foreach (var point in points)
            //{
            //    var value = FormatValue(point.Context.Series.Name!, point.StackedValue.End);
            //    list.Add(new ChartPointInfo(
            //        $"{point.Context.Series.Name}: {value}",
            //        point.Context.Series.GetColor())); //要添加颜色。出错暂时注释掉了
            //}

            return list;
        }

        /// <summary>
        /// 计算最大文本宽度
        /// </summary>
        private float CalculateMaxWidth(IEnumerable<ChartPointInfo> content)
        {
            float maxWidth = 0;
            foreach (var item in content)
            {
                using var textPaint = new SKPaint
                {
                    TextSize = TextSize,
                    IsAntialias = true
                };
                var bounds = new SKRect();
                textPaint.MeasureText(item.Text, ref bounds);
                if (bounds.Width > maxWidth) maxWidth = bounds.Width;
            }
            return maxWidth + 20; // 添加 padding
        }

        /// <summary>
        /// 数值格式化方法
        /// </summary>
 


        /// <summary>
        /// 可重写的数值格式化方法
        /// </summary>
        protected virtual string FormatValue(string seriesName, double value)
        {
            return seriesName switch
            {
                string s when s.Contains("金额") => value.ToString("C2"),
                string s when s.Contains("率") => value.ToString("P2"),
                _ => value.ToString("N2")
            };
        }

        ///// <summary>
        ///// 渲染方法重写
        ///// </summary>
        //public override void Draw(SkiaSharpDrawingContext context, SKRect tooltipBounds, IEnumerable<ChartPoint> points)
        //{
        //    if (!points.Any()) return;

        //    var content = BuildContent(points);
        //   // var maxWidth = CalculateMaxWidth(content);

        //    // 绘制背景
        //    using var bgPaint = new SKPaint { Color = BackgroundColor };
        //    context.Canvas.DrawRoundRect(tooltipBounds, 5, 5, bgPaint);

        //    // 绘制文本内容
        //    float y = tooltipBounds.Top + 10;
        //    foreach (var item in content)
        //    {
        //        using var textPaint = new SKPaint
        //        {
        //            Color = item.Color,
        //            TextSize = TextSize,
        //            IsAntialias = true
        //        };
        //        context.Canvas.DrawText(item.Text, tooltipBounds.Left + 10, y, textPaint);
        //        y += TextSize + 5;
        //    }
        //}

         

        // 辅助类：提示项信息
        protected class ChartPointInfo
        {
            public string Text { get; }
            public SKColor Color { get; }

            public ChartPointInfo(string text, SKColor color)
            {
                Text = text;
                Color = color;
            }
        }
    }
}
