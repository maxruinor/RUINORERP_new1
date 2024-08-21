namespace WinLib
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ArtTextLabel : Label
    {
        private WinLib.ArtTextStyle _artTextStyle = WinLib.ArtTextStyle.Border;
        private Color _borderColor = Color.White;
        private int _borderSize = 1;

        public ArtTextLabel()
        {
            this.SetStyles();
        }

        private PointF CalculateRenderTextStartPoint(Graphics g)
        {
            PointF empty = PointF.Empty;
            SizeF ef = g.MeasureString(base.Text, base.Font, PointF.Empty, StringFormat.GenericTypographic);
            if (this.AutoSize)
            {
                empty.X = base.Padding.Left;
                empty.Y = base.Padding.Top;
                return empty;
            }
            ContentAlignment textAlign = base.TextAlign;
            if (((textAlign == ContentAlignment.TopLeft) || (textAlign == ContentAlignment.MiddleLeft)) || (textAlign == ContentAlignment.BottomLeft))
            {
                empty.X = base.Padding.Left;
            }
            else if (((textAlign == ContentAlignment.TopCenter) || (textAlign == ContentAlignment.MiddleCenter)) || (textAlign == ContentAlignment.BottomCenter))
            {
                empty.X = (base.Width - ef.Width) / 2f;
            }
            else
            {
                empty.X = base.Width - (base.Padding.Right + ef.Width);
            }
            if (((textAlign == ContentAlignment.TopLeft) || (textAlign == ContentAlignment.TopCenter)) || (textAlign == ContentAlignment.TopRight))
            {
                empty.Y = base.Padding.Top;
                return empty;
            }
            if (((textAlign == ContentAlignment.MiddleLeft) || (textAlign == ContentAlignment.MiddleCenter)) || (textAlign == ContentAlignment.MiddleRight))
            {
                empty.Y = (base.Height - ef.Height) / 2f;
                return empty;
            }
            empty.Y = base.Height - (base.Padding.Bottom + ef.Height);
            return empty;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.ArtTextStyle == WinLib.ArtTextStyle.None)
            {
                base.OnPaint(e);
            }
            else if (base.Text.Length != 0)
            {
                this.RenderText(e.Graphics);
            }
        }

        private void RenderBordText(Graphics g, PointF point)
        {
            Brush brush;
            using (brush = new SolidBrush(this._borderColor))
            {
                for (int i = 1; i <= this._borderSize; i++)
                {
                    g.DrawString(base.Text, base.Font, brush, point.X - i, point.Y);
                    g.DrawString(base.Text, base.Font, brush, point.X, point.Y - i);
                    g.DrawString(base.Text, base.Font, brush, point.X + i, point.Y);
                    g.DrawString(base.Text, base.Font, brush, point.X, point.Y + i);
                }
            }
            using (brush = new SolidBrush(base.ForeColor))
            {
                g.DrawString(base.Text, base.Font, brush, point);
            }
        }

        private void RenderFormeText(Graphics g, PointF point)
        {
            Brush brush;
            using (brush = new SolidBrush(this._borderColor))
            {
                for (int i = 1; i <= this._borderSize; i++)
                {
                    g.DrawString(base.Text, base.Font, brush, (float) (point.X - i), (float) (point.Y + i));
                }
            }
            using (brush = new SolidBrush(base.ForeColor))
            {
                g.DrawString(base.Text, base.Font, brush, point);
            }
        }

        private void RenderRelievoText(Graphics g, PointF point)
        {
            Brush brush;
            using (brush = new SolidBrush(this._borderColor))
            {
                for (int i = 1; i <= this._borderSize; i++)
                {
                    g.DrawString(base.Text, base.Font, brush, point.X + i, point.Y);
                    g.DrawString(base.Text, base.Font, brush, point.X, point.Y + i);
                }
            }
            using (brush = new SolidBrush(base.ForeColor))
            {
                g.DrawString(base.Text, base.Font, brush, point);
            }
        }

        private void RenderText(Graphics g)
        {
            using (new TextRenderingHintGraphics(g))
            {
                PointF point = this.CalculateRenderTextStartPoint(g);
                switch (this._artTextStyle)
                {
                    case WinLib.ArtTextStyle.Border:
                        this.RenderBordText(g, point);
                        return;

                    case WinLib.ArtTextStyle.Relievo:
                        this.RenderRelievoText(g, point);
                        return;

                    case WinLib.ArtTextStyle.Forme:
                        this.RenderFormeText(g, point);
                        return;
                }
            }
        }

        private void SetStyles()
        {
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            base.UpdateStyles();
        }

        [Browsable(true), Category("Appearance"), DefaultValue(typeof(WinLib.ArtTextStyle), "1")]
        public WinLib.ArtTextStyle ArtTextStyle
        {
            get
            {
                return this._artTextStyle;
            }
            set
            {
                if (this._artTextStyle != value)
                {
                    this._artTextStyle = value;
                    base.Invalidate();
                }
            }
        }

        [Category("Appearance"), Browsable(true), DefaultValue(typeof(Color), "White")]
        public Color BorderColor
        {
            get
            {
                return this._borderColor;
            }
            set
            {
                if (this._borderColor != value)
                {
                    this._borderColor = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(1), Browsable(true), Category("Appearance")]
        public int BorderSize
        {
            get
            {
                return this._borderSize;
            }
            set
            {
                if (this._borderSize != value)
                {
                    this._borderSize = value;
                    base.Invalidate();
                }
            }
        }
    }
}

