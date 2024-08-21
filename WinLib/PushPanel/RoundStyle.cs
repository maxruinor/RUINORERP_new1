using System;
using System.Collections.Generic;
using System.Text;

namespace WinLib.PushPanelEx
{
    /* 作者：Starts_2000
     * 日期：2009-07-31
     * 网站：http://www.WinLib.com CS 程序员之窗。
     * 你可以免费使用或修改以下代码，但请保留版权信息。
     * 具体请查看 CS程序员之窗开源协议（http://www.WinLib.com/csol.html）。
     */

    /// <summary>
    /// 建立圆角路径的样式。
    /// </summary>
    public enum RoundStyle
    {
        /// <summary>
        /// 四个角都不是圆角。
        /// </summary>
        None = 0,
        /// <summary>
        /// 左上角为圆角。
        /// </summary>
        TopLeft = 0x01,
        /// <summary>
        /// 右上角为圆角。
        /// </summary>
        TopRight = 0x02,
        /// <summary>
        /// 左下角为圆角。
        /// </summary>
        BottomLeft = 0x04,
        /// <summary>
        /// 右下角为圆角。
        /// </summary>
        BottomRight = 0x08,
        /// <summary>
        /// 左边两个角为圆角。
        /// </summary>
        Left = TopLeft | BottomLeft,
        /// <summary>
        /// 右边两个角为圆角。
        /// </summary>
        Right = TopRight | BottomRight,
        /// <summary>
        /// 上边两个角为圆角。
        /// </summary>
        Top = TopLeft | TopRight,
        /// <summary>
        /// 下边两个角为圆角。
        /// </summary>
        Bottom = BottomLeft | BottomRight,
        /// <summary>
        /// 四个角都为圆角。
        /// </summary>
        All = Left | Right,
    }
}
