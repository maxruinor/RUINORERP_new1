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

    public class PaintScrollBarArrowEventArgs : IDisposable
    {
        private Graphics _graphics;
        private Rectangle _arrowRect;
        private ControlState _controlState;
        private ArrowDirection _arrowDirection;
        private Orientation _orientation;
        private bool _enabled;

        public PaintScrollBarArrowEventArgs(
            Graphics graphics,
            Rectangle arrowRect,
            ControlState controlState,
            ArrowDirection arrowDirection,
            Orientation orientation)
            : this(
            graphics,
            arrowRect,
            controlState,
            arrowDirection,
            orientation,
            true)
        {
        }

        public PaintScrollBarArrowEventArgs(
            Graphics graphics,
            Rectangle arrowRect,
            ControlState controlState,
            ArrowDirection arrowDirection,
            Orientation orientation,
            bool enabled)
        {
            _graphics = graphics;
            _arrowRect = arrowRect;
            _controlState = controlState;
            _arrowDirection = arrowDirection;
            _orientation = orientation;
            _enabled = enabled;
        }

        public Graphics Graphics
        {
            get { return _graphics; }
            set { _graphics = value; }
        }

        public Rectangle ArrowRectangle
        {
            get { return _arrowRect; }
            set { _arrowRect = value; }
        }

        public ControlState ControlState
        {
            get { return _controlState; }
            set { _controlState = value; }
        }

        public ArrowDirection ArrowDirection
        {
            get { return _arrowDirection; }
            set { _arrowDirection = value; }
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
