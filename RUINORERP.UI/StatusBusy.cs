using System;
using System.Windows.Forms;

namespace RUINORERP.UI
{
    /// <summary>
    /// UI状态繁忙指示器，用于显示加载状态和等待光标
    /// 线程安全实现，避免在异步操作中引发UI线程异常
    /// </summary>
    public class StatusBusy : IDisposable
    {
        private string _oldStatus;
        private Cursor _oldCursor;
        private readonly System.Diagnostics.Stopwatch _stopwatch = new System.Diagnostics.Stopwatch();
        private bool _disposedValue = false;

        /// <summary>
        /// 构造函数，显示繁忙状态
        /// </summary>
        /// <param name="statusText">状态文本</param>
        public StatusBusy(string statusText)
        {
            try
            {
                _stopwatch.Start();

                // 确保在UI线程执行
                if (MainForm.Instance != null && !MainForm.Instance.IsDisposed)
                {
                    if (MainForm.Instance.InvokeRequired)
                    {
                        MainForm.Instance.Invoke(new Action(() =>
                        {
                            _oldStatus = MainForm.Instance.lblStatusGlobal.Text;
                            MainForm.Instance.lblStatusGlobal.Text = statusText + "...";
                            _oldCursor = MainForm.Instance.Cursor;
                            MainForm.Instance.Cursor = Cursors.WaitCursor;
                        }));
                    }
                    else
                    {
                        _oldStatus = MainForm.Instance.lblStatusGlobal.Text;
                        MainForm.Instance.lblStatusGlobal.Text = statusText + "...";
                        _oldCursor = MainForm.Instance.Cursor;
                        MainForm.Instance.Cursor = Cursors.WaitCursor;
                    }
                }
            }
            catch (Exception)
            {
                // 静默失败，避免影响主要流程
            }
        }

        /// <summary>
        /// 释放资源，恢复UI状态
        /// </summary>
        /// <param name="disposing">是否正在释放托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue && disposing)
            {
                try
                {
                    _stopwatch.Stop();

                    if (MainForm.Instance != null && !MainForm.Instance.IsDisposed)
                    {
                        if (MainForm.Instance.InvokeRequired)
                        {
                            MainForm.Instance.Invoke(new Action(() =>
                            {
                                // 可选：显示耗时信息
                                if (!string.IsNullOrEmpty(_oldStatus))
                                {
                                    var elapsed = _stopwatch.Elapsed.TotalMilliseconds;
                                    // MainForm.Instance.lblStatusGlobal.Text = $"完成，耗时: {elapsed:F2}ms";
                                    MainForm.Instance.lblStatusGlobal.Text = _oldStatus;
                                }
                                MainForm.Instance.Cursor = Cursors.Arrow;
                                MainForm.Instance.Refresh();
                            }));
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(_oldStatus))
                            {
                                var elapsed = _stopwatch.Elapsed.TotalMilliseconds;
                                // MainForm.Instance.lblStatusGlobal.Text = $"完成，耗时: {elapsed:F2}ms";
                                MainForm.Instance.lblStatusGlobal.Text = _oldStatus;
                            }
                            MainForm.Instance.Cursor = Cursors.Arrow;
                            MainForm.Instance.Refresh();
                        }
                    }
                }
                catch (Exception)
                {
                    // 静默失败
                }
            }
            _disposedValue = true;
        }

        /// <summary>
        /// 释放资源，恢复UI状态
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
