﻿//--------------------------------------------------------------------------------
// Copyright (C) 2013-2021 JDH Software - <support@jdhsoftware.com>
//
// This program is provided to you under the terms of the Microsoft Public
// License (Ms-PL) as published at https://github.com/Cocotteseb/Krypton-OutlookGrid/blob/master/LICENSE.md
//
// Visit https://www.jdhsoftware.com and follow @jdhsoftware on Twitter
//
//--------------------------------------------------------------------------------

using ComponentFactory.Krypton.Toolkit;
using JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.Formatting;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.CustomColumns
{
    /// <summary>
    /// Class for a KryptonDataGridViewFormattingColumn : KryptonDataGridViewTextBoxColumn with conditionnal formatting abilities
    /// </summary>
    /// <seealso cref="ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn" />
    public class KryptonDataGridViewFormattingColumn : KryptonDataGridViewTextBoxColumn
    {
        private bool _contrastTextColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="KryptonDataGridViewFormattingColumn"/> class.
        /// </summary>
        public KryptonDataGridViewFormattingColumn()
            : base()
        {
            this.CellTemplate = new FormattingCell();
            this.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.ValueType = typeof(FormattingCell);
            ContrastTextColor = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [contrast text color].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [contrast text color]; otherwise, <c>false</c>.
        /// </value>
        public bool ContrastTextColor
        {
            get
            {
                return _contrastTextColor;
            }

            set
            {
                _contrastTextColor = value;
            }
        }
    }

    /// <summary>
    /// Formatting cell
    /// </summary>
    /// <seealso cref="ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxCell" />
    public class FormattingCell : KryptonDataGridViewTextBoxCell
    {

        /// <summary>
        /// Gets or sets the type of the format.
        /// </summary>
        /// <value>
        /// The type of the format.
        /// </value>
        public EnumConditionalFormatType FormatType { get; set; }
        /// <summary>
        /// Gets or sets the format parameters.
        /// </summary>
        /// <value>
        /// The format parameters.
        /// </value>
        public IFormatParams FormatParams { get; set; }


        /// <summary>
        /// Contrasts the color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        private Color ContrastColor(Color color)
        {
            int d = 0;
            //  Counting the perceptive luminance - human eye favors green color... 
            double a = (1
                        - (((0.299 * color.R)
                        + ((0.587 * color.G) + (0.114 * color.B)))
                        / 255));
            if ((a < 0.5))
            {
                d = 0;
            }
            else
            {
                //  bright colors - black font
                d = 255;
            }

            //  dark colors - white font
            return Color.FromArgb(d, d, d);
        }

        /// <summary>
        /// Paints the specified graphics.
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        /// <param name="clipBounds">The clip bounds.</param>
        /// <param name="cellBounds">The cell bounds.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="cellState">State of the cell.</param>
        /// <param name="value">The value.</param>
        /// <param name="formattedValue">The formatted value.</param>
        /// <param name="errorText">The error text.</param>
        /// <param name="cellStyle">The cell style.</param>
        /// <param name="advancedBorderStyle">The advanced border style.</param>
        /// <param name="paintParts">The paint parts.</param>
        protected override void Paint(System.Drawing.Graphics graphics, System.Drawing.Rectangle clipBounds, System.Drawing.Rectangle cellBounds, int rowIndex, System.Windows.Forms.DataGridViewElementStates cellState, object value, object formattedValue, string errorText, System.Windows.Forms.DataGridViewCellStyle cellStyle, System.Windows.Forms.DataGridViewAdvancedBorderStyle advancedBorderStyle,
        System.Windows.Forms.DataGridViewPaintParts paintParts)
        {
            if (FormatParams != null)  // null can happen when cell set to Formatting but no condition has been set !
            {
                switch (FormatType)
                {
                    case EnumConditionalFormatType.Bar:
                        int barWidth;
                        BarParams par = (BarParams)FormatParams;
                        barWidth = (int)((cellBounds.Width - 10) * par.ProportionValue);
                        Style.BackColor = this.DataGridView.DefaultCellStyle.BackColor;
                        Style.ForeColor = this.DataGridView.DefaultCellStyle.ForeColor;

                        if (barWidth > 0) //(double)value > 0 &&
                        {
                            Rectangle r = new Rectangle(cellBounds.X + 3, cellBounds.Y + 3, barWidth, cellBounds.Height - 8);
                            if (par.GradientFill)
                            {
                                using (LinearGradientBrush linearBrush = new LinearGradientBrush(r, par.BarColor, Color.White, LinearGradientMode.Horizontal)) //Color.FromArgb(255, 247, 251, 242)
                                {
                                    graphics.FillRectangle(linearBrush, r);
                                }
                            }
                            else
                            {
                                using (SolidBrush solidBrush = new SolidBrush(par.BarColor)) //Color.FromArgb(255, 247, 251, 242)
                                {
                                    graphics.FillRectangle(solidBrush, r);
                                }
                            }

                            using (Pen pen = new Pen(par.BarColor)) //Color.FromArgb(255, 140, 197, 66)))
                            {
                                graphics.DrawRectangle(pen, r);
                            }
                        }

                        break;
                    case EnumConditionalFormatType.TwoColorsRange:
                        TwoColorsParams TWCpar = (TwoColorsParams)FormatParams;
                        Style.BackColor = TWCpar.ValueColor;
                      //  if (ContrastTextColor)
                            Style.ForeColor = ContrastColor(TWCpar.ValueColor);
                        break;
                    case EnumConditionalFormatType.ThreeColorsRange:
                        ThreeColorsParams THCpar = (ThreeColorsParams)FormatParams;
                        Style.BackColor = THCpar.ValueColor;
                        Style.ForeColor = ContrastColor(THCpar.ValueColor);
                        break;
                    default:
                        Style.BackColor = this.DataGridView.DefaultCellStyle.BackColor;
                        Style.ForeColor = this.DataGridView.DefaultCellStyle.ForeColor;
                        break;
                }
            }
            else
            {
                Style.BackColor = this.DataGridView.DefaultCellStyle.BackColor;
                Style.ForeColor = this.DataGridView.DefaultCellStyle.ForeColor;
            }

            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle,
            DataGridViewPaintParts.None | DataGridViewPaintParts.ContentForeground);
        }
    }
}
