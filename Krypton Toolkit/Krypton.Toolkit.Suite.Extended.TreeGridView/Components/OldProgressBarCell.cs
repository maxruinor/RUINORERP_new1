using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Krypton.Toolkit.Suite.Extended.TreeGridView.Components
{


    public class OldProgressBarCell : DataGridViewTextBoxCell
    {
        private const int ProgressBarHeight = 20; // 进度条高度

        public OldProgressBarCell()
        {
            this.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        public override object Clone()
        {
            return new OldProgressBarCell();
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);

            if (value is double percentage)
            {
                // 计算进度条的宽度
                int progressBarWidth = (int)(cellBounds.Width * percentage / 100);

                // 绘制进度条背景
                using (Brush backgroundBrush = new SolidBrush(Color.LightGray))
                {
                    graphics.FillRectangle(backgroundBrush, cellBounds.X, cellBounds.Y + (cellBounds.Height - ProgressBarHeight) / 2, cellBounds.Width, ProgressBarHeight);
                }

                // 绘制进度条前景
                using (Brush progressBrush = new SolidBrush(Color.Blue))
                {
                    graphics.FillRectangle(progressBrush, cellBounds.X, cellBounds.Y + (cellBounds.Height - ProgressBarHeight) / 2, progressBarWidth, ProgressBarHeight);
                }

                // 绘制百分比文本
                string percentageText = $"{percentage:F2}%";
                SizeF textSize = graphics.MeasureString(percentageText, cellStyle.Font);
                PointF textPoint = new PointF(cellBounds.X + (cellBounds.Width - textSize.Width) / 2, cellBounds.Y + (cellBounds.Height - textSize.Height) / 2);
                graphics.DrawString(percentageText, cellStyle.Font, Brushes.Black, textPoint);
            }
        }
    }
}
