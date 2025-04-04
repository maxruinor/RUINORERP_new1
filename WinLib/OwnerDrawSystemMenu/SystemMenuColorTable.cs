﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace WinLib
{
    /* 作者：Starts_2000
     * 日期：2009-10-09
     * 网站：http://www.WinLib.com CS 程序员之窗。
     * 你可以免费使用或修改以下代码，但请保留版权信息。
     * 具体请查看 CS程序员之窗开源协议（http://www.WinLib.com/csol.html）。
     */

    public class SystemMenuColorTable
    {
        private static readonly Color _borderColor = 
            Color.FromArgb(65, 173, 236);
        private static readonly Color _innerBorderColor = 
            Color.FromArgb(200, 255, 255, 255);
        private static readonly Color _backColorNormal = 
            Color.FromArgb(229, 243, 251);
        private static readonly Color _backColorHover = 
            Color.FromArgb(65, 173, 236);
        private static readonly Color _backColorPressed = 
            Color.FromArgb(24, 142, 206);
        private static readonly Color _foreColor = 
            Color.FromArgb(12, 83, 124);

        public SystemMenuColorTable() { }

        public virtual Color BorderColor
        {
            get { return _borderColor; }
        }

        public virtual Color InnerBorderColor
        {
            get { return _innerBorderColor; }
        }

        public virtual Color BackColorNormal
        {
            get { return _backColorNormal; }
        }

        public virtual Color BackColorHover
        {
            get { return _backColorHover; }
        }

        public virtual Color BackColorPressed
        {
            get { return _backColorPressed; }
        }

        public virtual Color ForeColor
        {
            get { return _foreColor; }
        }
    }
}
