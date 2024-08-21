using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using WinLib;
using System.Drawing;
using System.ComponentModel;

namespace WinLib
{
    /* 作者：Starts_2000
     * 日期：2009-09-20
     * 网站：http://www.WinLib.com CS 程序员之窗。
     * 你可以免费使用或修改以下代码，但请保留版权信息。
     * 具体请查看 CS程序员之窗开源协议（http://www.WinLib.com/csol.html）。
     */

    public class PanelEx : Panel
    {
        public PanelEx()
            : base()
        {
        }

        private Color _InnerBackColor = Color.White;

        [DefaultValue(typeof(Color), "White")]
        public Color InnerBackColor
        {
            get { return _InnerBackColor; }
            set
            {
                if (_InnerBackColor != value)
                {
                    _InnerBackColor = value;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath path = CreatePath(
                ClientRectangle,
                8,
                true))
            {
                g.FillPath(Brushes.White, path);
                using (Pen pen = new Pen(Color.FromArgb(51, 153, 204)))
                {
                    g.DrawPath(pen, path);
                }
            }
        }

        private GraphicsPath CreatePath(Rectangle rect, int radius, bool correction)
        {
            GraphicsPath path = new GraphicsPath();
            int radiusCorrection = correction ? 1 : 0;

            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(
                rect.Right - radius - radiusCorrection,
                rect.Y,
                radius,
                radius,
                270,
                90);
            path.AddArc(
                rect.Right - radius - radiusCorrection,
                rect.Bottom - radius - radiusCorrection,
                radius,
                radius, 0, 90);
            path.AddArc(
                rect.X,
                rect.Bottom - radius - radiusCorrection,
                radius,
                radius,
                90,
                90);
            path.CloseFigure();
            return path;
        }
    }
}
