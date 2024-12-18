﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace WinLib
{
    /* 作者：Starts_2000
     * 日期：2009-09-20
     * 网站：http://www.WinLib.com CS 程序员之窗。
     * 你可以免费使用或修改以下代码，但请保留版权信息。
     * 具体请查看 CS程序员之窗开源协议（http://www.WinLib.com/csol.html）。
     */

     public delegate void SkinFormBackgroundRenderEventHandler(
        object sender,
        SkinFormBackgroundRenderEventArgs e);

    public class SkinFormBackgroundRenderEventArgs : PaintEventArgs
    {
        private SkinForm _skinForm;

        public SkinFormBackgroundRenderEventArgs(
            SkinForm skinForm,
            Graphics g,
            Rectangle clipRect)
            : base(g, clipRect)
        {
            _skinForm = skinForm;
        }

        public SkinForm SkinForm
        {
            get { return _skinForm; }
        }
    }
}
