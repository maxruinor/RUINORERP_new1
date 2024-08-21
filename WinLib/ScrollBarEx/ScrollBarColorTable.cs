using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace WinLib.ScrollBarEx
{
    /* 作者：Starts_2000
     * 日期：2010-01-25
     * 网站：http://www.WinLib.com CS 程序员之窗。
     * 你可以免费使用或修改以下代码，但请保留版权信息。
     * 具体请查看 CS程序员之窗开源协议（http://www.WinLib.com/csol.html）。
     */

    public class ScrollBarColorTable
    {
        private static readonly Color _base = Color.FromArgb(171, 230, 247);
        private static readonly Color _backNormal = Color.FromArgb(235, 249, 253);
        private static readonly Color _backHover = Color.FromArgb(121, 216, 243);
        private static readonly Color _backPressed = Color.FromArgb(70, 202, 239);
        private static readonly Color _border = Color.FromArgb(89, 210, 249);
        private static readonly Color _innerBorder = Color.FromArgb(200, 250, 250, 250);
        private static readonly Color _fore = Color.FromArgb(48, 135, 192);

        public ScrollBarColorTable() { }

        public virtual Color Base
        {
            get { return _base; }
        }

        public virtual Color BackNormal
        {
            get { return _backNormal; }
        }

        public virtual Color BackHover
        {
            get { return _backHover; }
        }

        public virtual Color BackPressed
        {
            get { return _backPressed; }
        }

        public virtual Color Border
        {
            get { return _border; }
        }

        public virtual Color InnerBorder
        {
            get { return _innerBorder; }
        }

        public virtual Color Fore
        {
            get { return _fore; }
        }
    }
}
