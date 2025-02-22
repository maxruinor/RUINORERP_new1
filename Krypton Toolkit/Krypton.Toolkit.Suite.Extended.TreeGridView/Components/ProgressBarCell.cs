using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krypton.Toolkit.Suite.Extended.TreeGridView.Components
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class ProgressBarCell : DataGridViewTextBoxCell
    {
        /// <summary>
        /// 进度条的值
        /// 优先按这个来画
        /// </summary>
        public double? ProcessBarValue { get; set; }

        private const int ProgressBarHeight = 20; // 进度条高度

        public ProgressBarCell()
        {
            this.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        public override object Clone()
        {
            return new ProgressBarCell();
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
            double percentage = 0.0;
            //
            if (ProcessBarValue.HasValue)
            {
                percentage = ProcessBarValue.Value;
            }
            else if (value is double)
            {
                percentage = (double)value;
            }
            // 计算进度条的宽度
            int progressBarWidth = (int)(cellBounds.Width * percentage / 100);

            // 绘制进度条背景
            using (Brush backgroundBrush = new SolidBrush(Color.LightGray))
            {
                graphics.FillRectangle(backgroundBrush, cellBounds.X, cellBounds.Y + (cellBounds.Height - ProgressBarHeight) / 2, cellBounds.Width, ProgressBarHeight);
            }

            // 绘制渐变色进度条前景
            if (progressBarWidth > 0)
            {
                using (LinearGradientBrush progressBrush = new LinearGradientBrush(
                    new Rectangle(cellBounds.X, cellBounds.Y + (cellBounds.Height - ProgressBarHeight) / 2, progressBarWidth, ProgressBarHeight),
                    GetGradientColor(percentage, Color.LightSalmon, Color.LightGreen),
                    GetGradientColor(percentage, Color.LightSalmon, Color.LightGreen),
                    LinearGradientMode.Horizontal))
                {
                    graphics.FillRectangle(progressBrush, cellBounds.X, cellBounds.Y + (cellBounds.Height - ProgressBarHeight) / 2, progressBarWidth, ProgressBarHeight);
                }
            }

            // 绘制百分比文本
            string percentageText = $"{percentage:F2}%";
            SizeF textSize = graphics.MeasureString(percentageText, cellStyle.Font);
            PointF textPoint = new PointF(cellBounds.X + (cellBounds.Width - textSize.Width) / 2, cellBounds.Y + (cellBounds.Height - textSize.Height) / 2);
            graphics.DrawString(percentageText, cellStyle.Font, Brushes.Black, textPoint);

        }

        private Color GetGradientColor(double percentage, Color startColor, Color endColor)
        {
            // 计算颜色插值
            float red = startColor.R + (endColor.R - startColor.R) * (float)(percentage / 100);
            float green = startColor.G + (endColor.G - startColor.G) * (float)(percentage / 100);
            float blue = startColor.B + (endColor.B - startColor.B) * (float)(percentage / 100);

            return Color.FromArgb((int)red, (int)green, (int)blue);
        }
    }
}
