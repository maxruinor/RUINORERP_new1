using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace WinLib
{
    /* 
    * 作者：Starts_2000
    * 日期：2009-08-07
    * 网站：http://www.WinLib.com CS 程序员之窗。
    * 你可以免费使用或修改以下代码，但请保留版权信息。
    * 具体请查看 CS程序员之窗开源协议（http://www.WinLib.com/csol.html）。
    */
    public class QQLoadingProgress : Control
    {
        private Color _baseColor = Color.FromArgb(9, 170, 254);
        private Color _glassColor = Color.FromArgb(191, 241, 255);
        private Color _borderColor = Color.FromArgb(165, 233, 255);
        private int _spokeNumber = 15;
        private int _rotationSpeed = 40;
        private Timer _timer;
        private int _value;
        private bool _active;

        #region Constructors

        public QQLoadingProgress()
            : base()
        {
            SetStyles();
        }

        #endregion

        #region Properties

        [DefaultValue(typeof(Color), "9, 170, 254")]
        public Color BaseColor
        {
            get { return _baseColor; }
            set
            {
                _baseColor = value;
                base.Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "191, 241, 255")]
        public Color GlassColor
        {
            get { return _glassColor; }
            set
            {
                _glassColor = value;
                base.Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "165, 233, 255")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                base.Invalidate();
            }
        }

        [DefaultValue(15)]
        public int SpokeNumber
        {
            get { return _spokeNumber; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(
                        "SpokeNumber",
                        "不能小于等于0。");
                }
                if (_spokeNumber != value)
                {
                    _spokeNumber = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(40)]
        public int RotationSpeed
        {
            get { return _rotationSpeed; }
            set
            {
                if (value != _rotationSpeed)
                {
                    _rotationSpeed = value <= 10 ? 10 : value;
                    Timer.Interval = _rotationSpeed;
                }
            }
        }

        [DefaultValue(false)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Active
        {
            get { return _active; }
            set
            {
                if (_active != value)
                {
                    if (value)
                    {
                        Start();
                    }
                    else
                    {
                        Stop();
                    }
                }
            }
        }

        protected override Size DefaultSize
        {
            get { return new Size(120, 23); }
        }

        private float SpokeWidth
        {
            get { return (Width - Height / 2.0f - 4) / (float)_spokeNumber; }
        }

        private Timer Timer
        {
            get
            {
                if (_timer == null)
                {
                    _timer = new Timer();
                    _timer.Interval = _rotationSpeed;
                    _timer.Tick += new EventHandler(TimerTick);
                }
                return _timer;
            }
        }

        #endregion

        #region Public Methods

        public void Start()
        {
            if (!_active && !DesignMode)
            {
                _active = true;
                Timer.Start();
            }
        }

        public void Stop()
        {
            if (_active)
            {
                Timer.Stop();
                _value = 0;
                _active = false;
                base.Invalidate();
            }
        }

        #endregion

        #region Protected Methods

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = ClientRectangle;
            using (GraphicsPath path = CreatePath(rect))
            {
                using (SolidBrush brush = new SolidBrush(_baseColor))
                {
                    g.FillPath(brush, path);
                }

                RectangleF glassRect = rect;
                glassRect.Y = rect.Y + rect.Height * 0.2f;
                glassRect.Height = (rect.Height - rect.Height * 0.2f) * 2;
                DrawGlass(
                    g,
                    glassRect,
                    _glassColor,
                    255,
                    0);

                using (GraphicsPath innerPath = new GraphicsPath())
                {
                    innerPath.AddBezier(
                        0f,
                        Height / 6.0f,
                        Width / 4.0f,
                        Height / 3.0f,
                        Width - Width / 4.0f,
                        Height / 3.0f,
                        Width,
                        Height / 6.0f);
                    innerPath.AddLine(Width, 0, 0, 0);
                    innerPath.CloseFigure();
                    g.SetClip(path);
                    using (SolidBrush brush =
                        new SolidBrush(Color.FromArgb(180, Color.White)))
                    {
                        g.FillPath(brush, innerPath);
                    }

                    using (Region region = new Region(innerPath))
                    {
                        g.ExcludeClip(region);
                    }

                    float spokeWidth = SpokeWidth;
                    float halfSpokeWidth = spokeWidth / 2;
                    int value = _value;
                    PointF pointStart = new PointF(
                        Height / 4.0f,
                        Height - 8);
                    PointF pointEnd = new PointF(pointStart.X + 4, 2);
                    PointF point1;
                    PointF point2;

                    for (int i = 0; i < _spokeNumber; i++)
                    {
                        point1 = new PointF(
                            pointStart.X + value, pointStart.Y);
                        point2 = new PointF(
                            pointEnd.X + value, pointEnd.Y);

                        Color color = CalculateColor(i);
                        using (Pen pen =
                            new Pen(color, halfSpokeWidth))
                        {
                            pen.StartCap = LineCap.Round;
                            pen.EndCap = LineCap.Round;
                            g.DrawLine(pen, point1, point2);
                        }
                        pointStart.X += spokeWidth;
                        pointEnd.X += spokeWidth;
                    }

                    g.ResetClip();
                }

                Rectangle bottomRect = new Rectangle(
                    Height / 3,
                    Height - Height / 3 - 4,
                    Width,
                    Height / 3);

                using (GraphicsPath innerPath = CreatePath(bottomRect))
                {
                    using (SolidBrush brush = new SolidBrush(
                        Color.FromArgb(50, 255, 255, 255)))
                    {
                        g.FillPath(brush, innerPath);
                    }
                }

                using (Pen pen = new Pen(_borderColor))
                {
                    g.DrawPath(pen, path);
                }
            }

            rect.Inflate(-1, -1);
            rect.Width ++;
            using (GraphicsPath path = CreatePath(rect))
            {
                using (Pen pen = new Pen(Color.FromArgb(200, 255, 255, 255)))
                {
                    g.DrawPath(pen, path);
                }
            }
        }

        #endregion

        #region Help Methods

        private void SetStyles()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.CacheText, true);
            SetStyle(ControlStyles.Opaque, false);
            UpdateStyles();
        }

        private Color CalculateColor(int index)
        {
            int alpha = 30;
            int halfSpokeNumber = _spokeNumber / 2;
            float alphaStep = 70.0f / halfSpokeNumber;

            if (index <= halfSpokeNumber)
            {
                alpha += (int)(alphaStep * index);
            }
            else
            {
                alpha += (int)((_spokeNumber - index) * alphaStep);
            }
            return Color.FromArgb(alpha, 255, 255, 255);
        }

        private GraphicsPath CreatePath(Rectangle rect)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(
                rect.X, 
                rect.Y, 
                rect.Height - 1, 
                rect.Height - 1, 
                90, 
                180);
            path.AddArc(
                rect.Right - Height - 1,
                rect.Y,
                rect.Height - 1,
                rect.Height - 1, 
                270, 
                180);
            path.CloseFigure();
            return path;
        }

        private void DrawGlass(
            Graphics g, RectangleF glassRect, int alphaCenter, int alphaSurround)
        {
            DrawGlass(g, glassRect, Color.White, alphaCenter, alphaSurround);
        }

        private void DrawGlass(
            Graphics g,
            RectangleF glassRect,
            Color glassColor,
            int alphaCenter,
            int alphaSurround)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(glassRect);
                using (PathGradientBrush brush = new PathGradientBrush(path))
                {
                    brush.CenterColor = Color.FromArgb(alphaCenter, glassColor);
                    brush.SurroundColors = new Color[] { 
                        Color.FromArgb(alphaSurround, glassColor) };
                    brush.CenterPoint = new PointF(
                        glassRect.X + glassRect.Width / 2,
                        glassRect.Y + glassRect.Height / 2);
                    g.FillPath(brush, path);
                }
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            float spokeWidth = SpokeWidth;
            _value = ++_value % (int)spokeWidth;
            base.Invalidate();
        }

        #endregion
    }
}
