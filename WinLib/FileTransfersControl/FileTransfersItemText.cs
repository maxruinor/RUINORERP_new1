using System;
using System.Collections.Generic;
using System.Text;

namespace WinLib
{
    /* 作者：Starts_2000
     * 日期：2009-08-04
     * 网站：http://www.WinLib.com CS 程序员之窗。
     * 你可以免费使用或修改以下代码，但请保留版权信息。
     * 具体请查看 CS程序员之窗开源协议（http://www.WinLib.com/csol.html）。
     */

    internal class FileTransfersItemText : IFileTransfersItemText
    {
        #region IFileTransfersItemText 成员

        public string Save
        {
            get { return "接收"; }
        }

        public string SaveTo
        {
            get { return "另存为..."; }
        }

        public string RefuseReceive
        {
            get { return "拒绝"; }
        }

        public string CancelTransfers
        {
            get { return "取消"; }
        }

        #endregion
    }
}
