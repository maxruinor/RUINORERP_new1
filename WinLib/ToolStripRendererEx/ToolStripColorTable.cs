using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace WinLib
{
    /* 作者：Starts_2000
     * 日期：2009-12-20
     * 网站：http://www.WinLib.com CS 程序员之窗。
     * 你可以免费使用或修改以下代码，但请保留版权信息。
     * 具体请查看 CS程序员之窗开源协议（http://www.WinLib.com/csol.html）。
     */

    public class ToolStripColorTable
    {
        private static readonly Color _base = Color.FromArgb(105, 200, 254);
        private static readonly Color _border = Color.FromArgb(194, 169, 120);
        private static readonly Color _backNormal = Color.FromArgb(250, 250, 250);
        private static readonly Color _backHover = Color.FromArgb(255, 201, 15);
        private static readonly Color _backPressed = Color.FromArgb(226, 176, 0);
        private static readonly Color _fore = Color.FromArgb(21, 66, 139);
        private static readonly Color _dropDownImageBack = Color.FromArgb(233, 238, 238);
        private static readonly Color _dropDownImageSeparator = Color.FromArgb(197, 197, 197);

        public ToolStripColorTable() { }

        public virtual Color Base
        {
            get { return _base; }
        }

        public virtual Color Border
        {
            get { return _border; }
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

        public virtual Color Fore
        {
            get { return _fore; }
        }

        public virtual Color DropDownImageBack
        {
            get { return _dropDownImageBack; }
        }

        public virtual Color DropDownImageSeparator
        {
            get { return _dropDownImageSeparator; }
        }
    }
}
