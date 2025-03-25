using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace AutoUpdateTools.XmlCompare
{
    public class SafeScrollSynchronizer : IDisposable
    {
        private readonly RichTextBox _master;
        private readonly RichTextBox _slave;
        private bool _isSyncing;
        private bool _disposed;

        // 使用单向同步模式
        public SafeScrollSynchronizer(RichTextBox master, RichTextBox slave)
        {
            _master = master;
            _slave = slave;
            _master.VScroll += MasterScrolled;
        }

        private void MasterScrolled(object sender, EventArgs e)
        {
            if (_isSyncing || _disposed) return;

            _isSyncing = true;
            try
            {
                // 获取主控件的滚动位置
                var pt = new Win32.POINT();
                Win32.SendMessage(_master.Handle, Win32.EM_GETSCROLLPOS, 0, ref pt);

                // 设置从控件的滚动位置（使用EM_SETSCROLLPOS避免触发事件）
                Win32.SendMessage(_slave.Handle, Win32.EM_SETSCROLLPOS, 0, ref pt);
            }
            finally
            {
                _isSyncing = false;
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            _master.VScroll -= MasterScrolled;
            _disposed = true;
        }
    }
}
