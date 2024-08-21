using System;
using System.ComponentModel;
using System.Windows.Forms;
using WinLib.PushPanelEx.Win32.Struct;
using System.Runtime.InteropServices;
using WinLib.PushPanelEx.Win32;
using System.Drawing;
using System.Drawing.Drawing2D;
using WinLib.PushPanelEx.Win32.Const;
using System.Drawing.Imaging;

namespace WinLib.PushPanelEx
{
    public class BorderPanel : PanelBase
    {
        public BorderPanel()
            : base()
        {
        }

        public override Rectangle DisplayRectangle
        {
            get
            {
                Rectangle rect = base.DisplayRectangle;
                int borderWidth = base.BorderWidth;
                rect.Inflate(-borderWidth, -borderWidth);
                return rect;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            OnPaintBorder(e);
        }

        protected virtual void OnPaintBorder(PaintEventArgs e)
        {
            RenderBorder(e.Graphics, base.ClientRectangle);
        }

        private void RenderBorder(Graphics g, Rectangle bounds)
        {
            if (RoundStyle == RoundStyle.None)
            {
                ControlPaint.DrawBorder(
                    g,
                    bounds,
                    ColorTable.Border,
                    ButtonBorderStyle.Solid);
            }
            else
            {
                using (SmoothingModeGraphics sg = new SmoothingModeGraphics(g))
                {
                    using (GraphicsPath path = GraphicsPathHelper.CreatePath(
                        bounds, Radius, RoundStyle, true))
                    {
                        using (Pen pen = new Pen(ColorTable.Border))
                        {
                            g.DrawPath(pen, path);
                        }
                    }
                }
            }
        }
    }
}
