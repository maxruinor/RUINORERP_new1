using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinLib.ScrollBarEx
{
    /* 作者：Starts_2000
     * 日期：2010-07-30
     * 网站：http://www.WinLib.com CS 程序员之窗。
     * 你可以免费使用或修改以下代码，但请保留版权信息。
     * 具体请查看 CS程序员之窗开源协议（http://www.WinLib.com/csol.html）。
     */

    public class PaintScrollBarThumbEventArgs : IDisposable
    {
        private Graphics _graphics;
        private Rectangle _thumbRect;
        private ControlState _controlState;
        private Orientation _orientation;
        private bool _enabled;

        public PaintScrollBarThumbEventArgs(
           Graphics graphics,
           Rectangle thumbRect,
           ControlState controlState,
           Orientation orientation)
            : this(graphics, thumbRect, controlState, orientation, true)
        {
        }

        public PaintScrollBarThumbEventArgs(
            Graphics graphics,
            Rectangle thumbRect,
            ControlState controlState,
            Orientation orientation,
            bool enabled)
        {
            _graphics = graphics;
            _thumbRect = thumbRect;
            _controlState = controlState;
            _orientation = orientation;
            _enabled = enabled;
        }

        public Graphics Graphics
        {
            get { return _graphics; }
            set { _graphics = value; }
        }

        public Rectangle ThumbRectangle
        {
            get { return _thumbRect; }
            set { _thumbRect = value; }
        }

        public ControlState ControlState
        {
            get { return _controlState; }
            set { _controlState = value; }
        }

        public Orientation Orientation
        {
            get { return _orientation; }
            set { _orientation = value; }
        }

        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public void Dispose()
        {
            _graphics = null;
        }
    }
}
