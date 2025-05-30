using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using WinLib.PushPanelEx.Win32;
using WinLib.PushPanelEx.Win32.Const;
using WinLib.PushPanelEx.Win32.Struct;
using System.Runtime.InteropServices;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace WinLib.PushPanelEx
{
    /* 作者：Starts_2000
     * 日期：2010-08-10
     * 网站：http://www.WinLib.com CS 程序员之窗。
     * 你可以免费使用或修改以下代码，但请保留版权信息。
     * 具体请查看 CS程序员之窗开源协议（http://www.WinLib.com/csol.html）。
     */
    public abstract class PanelBase : Panel
    {
        #region Fields

        private PanelColorTable _colorTable;
        private int _radius = 8;
        private RoundStyle _roundStyle = RoundStyle.All;

        #endregion

        #region Constructors

        public PanelBase()
            : base()
        {
            base.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw, true);
        }

        #endregion

        #region Properties

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PanelColorTable ColorTable
        {
            get
            {
                if (_colorTable == null)
                {
                    _colorTable = new PanelColorTable();
                }
                return _colorTable;
            }
            set
            {
                _colorTable = value;
                base.Invalidate();
            }
        }

        [DefaultValue(8)]
        public int Radius
        {
            get { return _radius; }
            set
            {
                if (value != _radius)
                {
                    _radius = value < 2 ? 2 : value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(typeof(RoundStyle), "15")]
        public RoundStyle RoundStyle
        {
            get { return _roundStyle; }
            set
            {
                if (value != _roundStyle)
                {
                    _roundStyle = value;
                    base.Invalidate();
                }
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new BorderStyle BorderStyle
        {
            get { return base.BorderStyle; }
            set { }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool AutoScroll
        {
            get { return base.AutoScroll; }
            set { base.AutoScroll = value; }
        }

        protected int BorderWidth
        {
            get
            {
                return _roundStyle == RoundStyle.None ?
                    1 : (int)(_radius * (1 - Math.Sin(45D / 180 * Math.PI)));
            }
        }

        #endregion

        #region Protected Methods

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if ((base.BackColor == Color.Transparent &&
                base.BackgroundImage == null) ||
                _roundStyle == RoundStyle.None)
            {
                base.OnPaintBackground(e);
            }
            else
            {
                Graphics g = e.Graphics;

                using (GraphicsPath path = GraphicsPathHelper.CreatePath(
                    base.ClientRectangle, _radius, _roundStyle, true))
                {
                    GraphicsState gState = g.Save();
                    g.SetClip(path);
                    base.OnPaintBackground(e);
                    g.Restore(gState);

                    g.ExcludeClip(new Region(path));
                    ButtonRenderer.DrawParentBackground(
                        g,
                        base.ClientRectangle,
                        this);
                    g.ResetClip();
                }
            }
        }

        #endregion
    }
}
