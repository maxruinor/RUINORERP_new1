using System;

namespace WinLib.PushPanelEx
{
    /* 作者：Starts_2000
     * 日期：2010-08-10
     * 网站：http://www.WinLib.com CS 程序员之窗。
     * 你可以免费使用或修改以下代码，但请保留版权信息。
     * 具体请查看 CS程序员之窗开源协议（http://www.WinLib.com/csol.html）。
     */
    public delegate void PushPanelItemCaptionClickEventHandler(
        object sender,
        PushPanelItemCaptionClickEventArgs e);

    public class PushPanelItemCaptionClickEventArgs : EventArgs
    {
        private PushPanelItem _item;

        public PushPanelItemCaptionClickEventArgs()
            : base()
        {
        }

        public PushPanelItemCaptionClickEventArgs(PushPanelItem item)
            : this()
        {
            _item = item;
        }

        public PushPanelItem Item
        {
            get { return _item; }
            set { _item = value; }
        }
    }
}
