using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace WinLib
{
    public delegate void DrawItemExEventHandler(
        object sender,
        DrawItemExEventArgs e);

    public class DrawItemExEventArgs : EventArgs
    {
        private Graphics _graphics;
        private string _text;
        private Font _font;
        private Rectangle _bounds;
        private int _itemId;
        private DrawItemState _state;
        private bool _isSeparator;

        public Graphics Graphics
        {
            get { return _graphics; }
        }

        public string Text
        {
            get { return _text; }
        }

        public Font Font
        {
            get { return _font; }
        }

        public Rectangle Bounds
        {
            get { return _bounds; }
        }

        public int ItemId
        {
            get { return _itemId; }
        }

        public DrawItemState State
        {
            get { return _state; }
        }

        public bool IsSeparator
        {
            get { return _isSeparator; }
        }

        public DrawItemExEventArgs(
            Graphics graphics,
            string text,
            Font font,
            Rectangle rect,
            int itemId,
            DrawItemState state,
            bool isSeparator)
            : base()
        {
            _graphics = graphics;
            _text = text;
            _font = font;
            _bounds = rect;
            _itemId = itemId;
            _state = state;
            _isSeparator = isSeparator;
        }
    }
}
