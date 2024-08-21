using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using WinLib.ToolTipExNS;

namespace WinLib
{
    /* 作者：Starts_2000
     * 日期：2009-10-08
     * 网站：http://www.WinLib.com CS 程序员之窗。
     * 你可以免费使用或修改以下代码，但请保留版权信息。
     * 具体请查看 CS程序员之窗开源协议（http://www.WinLib.com/csol.html）。
     */

    public class SystemMenuProfessionalRenderer : SystemMenuRenderer
    {
        private SystemMenuColorTable _colorTable;

        public SystemMenuProfessionalRenderer()
            : base()
        {
        }

        public SystemMenuProfessionalRenderer(
            SystemMenuColorTable colorTable)
            : base()
        {
            _colorTable = colorTable;
        }

        protected virtual SystemMenuColorTable ColorTable
        {
            get
            {
                if (_colorTable == null)
                {
                    _colorTable = new SystemMenuColorTable();
                }
                return _colorTable;
            }
        }

        protected override void OnRenderSystemMenuItem(
            DrawItemExEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rect = e.Bounds;
            string text = e.Text;
            DrawItemState state = e.State;

            using (Brush brush = new SolidBrush(ColorTable.BackColorNormal))
            {
                g.FillRectangle(brush, rect);
            }

            if (e.IsSeparator)
            {
              WinLib.ExToolTip.RenderHelper.RenderSeparatorLine(
                   g,
                   rect,
                   ColorTable.BackColorPressed,
                   ColorTable.BackColorNormal,
                   SystemColors.ControlLightLight,
                   false);
            }
            else
            {
                bool enabled = true;
                bool selected =
                    (state & DrawItemState.Selected) == DrawItemState.Selected;
                if ((state & DrawItemState.Disabled) == DrawItemState.Disabled ||
                    (state & DrawItemState.Grayed) == DrawItemState.Grayed)
                {
                    enabled = false;
                }

                rect.X += 1;
                rect.Width -= 1;
                rect.Height -= 2;
                if (enabled && selected)
                {
                    RenderHelper.RenderBackgroundInternal(
                      g,
                      rect,
                      ColorTable.BackColorHover,
                      ColorTable.BorderColor,
                      ColorTable.BackColorNormal,
                      RoundStyle.All,
                      true,
                      true,
                      LinearGradientMode.Vertical);
                }

                //还原(&R) 61728
                //移动(&M) 61456
                //大小(&S) 61440
                //最小化(&N) 61472
                //最大化(&X) 61488
                //关闭(&C)	Alt+F4 61536
                Rectangle iconRect = new Rectangle(
                    rect.X + 2,
                    rect.Y,
                    SystemInformation.MenuCheckSize.Width,
                    rect.Height);

                GraphicsPath path = null;
                bool bDrawIcon = true;
                switch (e.ItemId)
                {
                    case 61728:
                        path = GraphicsPathHelper.CreateMaximizeFlagPath(
                            iconRect, true);
                        break;
                    case 61472:
                        path = GraphicsPathHelper.CreateMinimizeFlagPath(iconRect);
                        break;
                    case 61488:
                        path = GraphicsPathHelper.CreateMaximizeFlagPath(
                           iconRect, false);
                        break;
                    case 61536:
                        path = GraphicsPathHelper.CreateCloseFlagPath(iconRect);
                        break;
                    default:
                        bDrawIcon = false;
                        break;
                }

                if (bDrawIcon)
                {
                    Color baseColor = enabled ?
                        ColorTable.ForeColor : SystemColors.GrayText;
                    using (SmoothingModeGraphics graphics =
                        new SmoothingModeGraphics(g))
                    {
                        using (Brush brush = new SolidBrush(baseColor))
                        {
                            g.FillPath(brush, path);
                        }
                    }
                }

                if (path != null)
                {
                    path.Dispose();
                }

                if (string.IsNullOrEmpty(text))
                {
                    return;
                }

                Rectangle textRect = new Rectangle(
                    iconRect.Right + 5,
                    rect.Y,
                    rect.Width - iconRect.Right - 5,
                    rect.Height);
                Color textColor = enabled ?
                    ColorTable.ForeColor : SystemColors.GrayText;
                TextFormatFlags textFlags =
                    TextFormatFlags.VerticalCenter |
                    TextFormatFlags.SingleLine;
                if (text.IndexOf('\t') == -1)
                {
                    TextRenderer.DrawText(
                        g,
                        text,
                        e.Font,
                        textRect,
                        textColor,
                        textFlags | TextFormatFlags.Left);
                }
                else
                {
                    string[] texts = text.Split('\t');
                    TextRenderer.DrawText(
                        g,
                        texts[0],
                        e.Font,
                        textRect,
                        textColor,
                        textFlags | TextFormatFlags.Left);
                    TextRenderer.DrawText(
                        g,
                        texts[1],
                        e.Font,
                        textRect,
                        textColor,
                        textFlags | TextFormatFlags.Right);
                }
            }
        }

        protected override void OnRenderSystemMenuNC(
            SystemMenuNCRenderEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle bounds = e.Bounds;

            Rectangle barRect = new Rectangle(
                bounds.X + 2,
                bounds.Y + 2,
                base.MenuBarWidth,
                bounds.Height - 4);

            using (Brush brush = new SolidBrush(ColorTable.BackColorNormal))
            {
                using (Region region = new Region(bounds))
                {
                    region.Exclude(barRect);
                    g.FillRegion(brush, region);
                }
            }

            using (LinearGradientBrush brush = new LinearGradientBrush(
                    barRect,
                    ColorTable.BackColorHover,
                    Color.White,
                    90f))
            {
                Blend blend = new Blend();
                blend.Positions = new float[] { 0f, .2f, 1f };
                blend.Factors = new float[] { 0f, 0.1f, .9f };
                brush.Blend = blend;
                g.FillRectangle(brush, barRect);
            }
            StringFormat sf = new StringFormat(StringFormatFlags.NoWrap);
            Font font = new Font(
                "Microsoft Sans Serif", 12, FontStyle.Regular);
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Near;
            sf.Trimming = StringTrimming.EllipsisCharacter;

            g.TranslateTransform(barRect.X, barRect.Bottom);
            g.RotateTransform(270f);

            Rectangle newRect = new Rectangle(
                barRect.X, barRect.Y, barRect.Height, barRect.Width);

            using (TextRenderingHintGraphics graphics =
                new TextRenderingHintGraphics(g))
            {
                using (SolidBrush brush = new SolidBrush(ColorTable.ForeColor))
                {
                    g.DrawString(
                       LegalCopyright.WebSite,
                       font,
                       brush,
                       newRect,
                       sf);
                }
            }

            g.ResetTransform();

            ControlPaint.DrawBorder(
                g,
                bounds,
                ColorTable.BorderColor,
                ButtonBorderStyle.Solid);
            bounds.Inflate(-1, -1);
            ControlPaint.DrawBorder(
                g,
                bounds,
                ColorTable.InnerBorderColor,
                ButtonBorderStyle.Solid);
        }
    }
}