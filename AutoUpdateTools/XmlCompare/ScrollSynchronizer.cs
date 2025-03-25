using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoUpdateTools.XmlCompare;

namespace AutoUpdateTools.XmlCompare
{
    public class ScrollSynchronizer : IDisposable
    {
        public const int SB_VERT = 1;
        public const int SB_HORZ = 0;
        public const int WM_VSCROLL = 0x115;
        public const int WM_HSCROLL = 0x114;
        public const int SB_THUMBPOSITION = 4;

        private readonly RichTextBox _source;
        private readonly RichTextBox _target;
        private bool _isSyncing;
        private bool _disposed;

        public ScrollSynchronizer(RichTextBox source, RichTextBox target)
        {
            _source = source;
            _target = target;

            _source.VScroll += OnSourceVScroll;
            _source.HScroll += OnSourceHScroll;
        }

        private void OnSourceVScroll(object sender, EventArgs e)
        {
            if (_isSyncing || _disposed) return;

            try
            {
                _isSyncing = true;
                SyncScrollPosition(SB_VERT, WM_VSCROLL);
            }
            finally
            {
                _isSyncing = false;
            }
        }

        private void OnSourceHScroll(object sender, EventArgs e)
        {
            if (_isSyncing || _disposed) return;

            try
            {
                _isSyncing = true;
                SyncScrollPosition(SB_HORZ, WM_HSCROLL);
            }
            finally
            {
                _isSyncing = false;
            }
        }

        private void SyncScrollPosition(int scrollBarType, int message)
        {
            int position = Win32.GetScrollPos(_source.Handle, scrollBarType);

            // 使用PostMessage避免重入
            Win32.PostMessage(_target.Handle, message,
                Win32.SB_THUMBPOSITION + (position << 16), 0);

            // 确保位置同步
            Win32.SetScrollPos(_target.Handle, scrollBarType, position, true);
        }

        public void Dispose()
        {
            if (_disposed) return;

            _source.VScroll -= OnSourceVScroll;
            _source.HScroll -= OnSourceHScroll;
            _disposed = true;
        }
    }
}
