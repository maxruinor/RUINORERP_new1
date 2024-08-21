using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace WinLib
{
    /* 作者：Starts_2000
     * 日期：2009-09-08
     * 网站：http://www.WinLib.com CS 程序员之窗。
     * 你可以免费使用或修改以下代码，但请保留版权信息。
     * 具体请查看 CS程序员之窗开源协议（http://www.WinLib.com/csol.html）。
     */

    public delegate void MeasureItemExEventHandler(
        object sender,
        MeasureItemExEventArgs e);

    public class MeasureItemExEventArgs : EventArgs
    {
        private Graphics _graphics;
        private string _text;
        private Font _font;
        private int _itenWidth;
        private int _itemHeight;

        public MeasureItemExEventArgs(
            Graphics graphics,
            string text,
            Font font)
            : this(graphics, text, font, 0)
        {
        }

        public MeasureItemExEventArgs(
            Graphics graphics,
            string text,
            Font font,
            int itemHeight)
            : base()
        {
            _graphics = graphics;
            _text = text;
            _font = font;
            _itemHeight = itemHeight;
        }

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

        public int ItemWidth
        {
            get { return _itenWidth; }
            set { _itenWidth = value; }
        }

        public int ItemHeight
        {
            get { return _itemHeight; }
            set { _itemHeight = value; }
        }
    }
}
