using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace WinLib
{
    /* 作者：Starts_2000
     * 日期：2009-10-08
     * 网站：http://www.WinLib.com CS 程序员之窗。
     * 你可以免费使用或修改以下代码，但请保留版权信息。
     * 具体请查看 CS程序员之窗开源协议（http://www.WinLib.com/csol.html）。
     */

    public delegate void SystemMenuNCRenderEventHandler(
        object sender,
        SystemMenuNCRenderEventArgs e);

    public class SystemMenuNCRenderEventArgs : EventArgs
    {
        private Graphics _graphics;
        private Rectangle _bounds;

        public SystemMenuNCRenderEventArgs(
            Graphics graphics,
            Rectangle bounds) : base()
        {
            _graphics = graphics;
            _bounds = bounds;
        }

        public Graphics Graphics
        {
            get { return _graphics; }
        }

        public Rectangle Bounds
        {
            get { return _bounds; }
        }
    }
}
