using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using RUINORERP.UI.Common;

namespace RUINORERP.UI
{
    //    不要在using块外使用ProgressDialog

    //确保所有进度更新间隔至少50ms

    //对于立即取消的操作需要额外处理：

    //        // 在需要立即退出的场景：
    //_cts.CancelAfter(TimeSpan.Zero); 
    /*
    private void toolStripCopyRoleConfig_Click(object sender, EventArgs e)
    {
        using (var dialog = new ProgressDialog("角色授权", allowCancel: true, timeoutSeconds: 60))
        {
            dialog.RunAsync(async (progress, ct) =>
            {
                try
                {
                    progress.Report((0, "开始初始化权限..."));

                    // 模拟长时间操作
                    for (int i = 0; i <= 100; i++)
                    {
                        ct.ThrowIfCancellationRequested();
                        await Task.Delay(50, ct);
                        progress.Report((i, $"处理进度 {i}%"));
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    throw;
                }
            });

            var result = dialog.ShowDialog(this);
            switch (result)
            {
                case DialogResult.OK:
                    MessageBox.Show("操作成功");
                    break;
                case DialogResult.Cancel:
                    MessageBox.Show("用户取消");
                    break;
                case DialogResult.Abort:
                    MessageBox.Show("操作异常");
                    break;
            }
        }
        */
    public sealed class ProgressDialog : IDisposable
    {
        private readonly ProgressForm _form;
        private readonly CancellationTokenSource _cts = new();
        private Task _workerTask;
        private bool _isDisposed;
        private volatile bool _isOperationCompleted;
        private DateTime _lastCloseAttempt = DateTime.MinValue;
        // 新增状态标志
        private bool _userRequestedCancel;
        private readonly Stopwatch _sw = new Stopwatch();

        private CancellationTokenSource _updateTimerCts;
        private Task _updateTimerTask;



        public TimeSpan Elapsed => _sw.Elapsed;
        public ProgressDialog(string title = "处理中...", bool allowCancel = true)
        {
            _form = new ProgressForm(title, allowCancel);
            _form.CancelRequested += OnCancelRequested;
            _form.FormClosing += OnFormClosing;
        }



        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            // 排除非用户主动关闭的情况
            if (e.CloseReason != CloseReason.UserClosing)
                return;

            // 防抖检查（1秒内不重复提示）
            if ((DateTime.Now - _lastCloseAttempt).TotalSeconds < 1)
            {
                e.Cancel = true;
                return;
            }

            _lastCloseAttempt = DateTime.Now;

            // 精确判断是否需要确认
            bool ShouldConfirm()
            {
                // 已标记完成
                if (_isOperationCompleted) return false;

                // 任务未启动或已结束
                if (_workerTask == null) return false;
                if (_workerTask.IsCompleted) return false;

                // 任务处于运行状态
                return !_workerTask.IsCanceled && !_workerTask.IsFaulted;
            }
            if (!ShouldConfirm()) return;

            // 排除程序控制的关闭
            if (e.CloseReason == CloseReason.None ||
                e.CloseReason == CloseReason.FormOwnerClosing)
                return;

            // 精确判断任务状态
            var isRunning = !_workerTask.IsCompleted &&
                           !_workerTask.IsCanceled &&
                           !_workerTask.IsFaulted;

            if (!isRunning) return;

            var result = MessageBox.Show(
                "操作仍在进行中，确定要取消吗？",
                "确认取消",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                // 标记为用户主动取消
                _userRequestedCancel = true;
                _cts.Cancel();
                e.Cancel = false;


            }
        }
        public void RunAsync(Func<IProgress<(int Percentage, string Message)>, CancellationToken, Task> operation)
        {
            _sw.Restart();
            _updateTimerCts = new CancellationTokenSource();
            // 定时更新耗时
            // 定时更新耗时
            _updateTimerTask = Task.Run(async () =>
            {
                try
                {
                    while (!_updateTimerCts.Token.IsCancellationRequested &&
                          (_workerTask == null || !_workerTask.IsCompleted))
                    {
                        _form.InvokeIfRequired(() =>
                        {
                            if (!_form.IsDisposed && _form.IsHandleCreated)
                            {
                                _form.UpdateTimeInfo(_sw.Elapsed);
                            }
                        });
                        await Task.Delay(1000, _updateTimerCts.Token);
                    }
                }
                catch (OperationCanceledException)
                {
                    // 正常取消，无需处理
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"更新耗时显示出错: {ex.Message}");
                }
            }, _updateTimerCts.Token);


            _isOperationCompleted = false;
            _workerTask = Task.Run(async () =>
            {
                try
                {
                    var progress = new Progress<(int Percentage, string Message)>(v =>
                        SafeUpdateProgress(v.Percentage, v.Message));

                    await operation(progress, _cts.Token);


                    SafeClose(DialogResult.OK);
                }
                catch (OperationCanceledException)
                {
                    SafeClose(DialogResult.Cancel);
                }
                catch
                {
                    SafeClose(DialogResult.Abort);
                }
                finally
                {
                    _isOperationCompleted = true;
                    _updateTimerCts.Cancel();

                    
                        _cts.Cancel();
                    
                    _workerTask = null;
                }
            }, _cts.Token);



        }

        private void SafeUpdateProgress(int percentage, string message)
        {
            if (_form.IsDisposed || _isDisposed || !_form.IsHandleCreated)
                return;

            if (_form.InvokeRequired)
            {
                try
                {
                    _form.BeginInvoke(new Action(() =>
                        SafeUpdateProgress(percentage, message)));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"更新进度出错: {ex.Message}");
                }
            }
            else
            {
                _form.UpdateProgress(percentage, message);
            }
        }

        private void SafeClose(DialogResult result)
        {
            // 如果用户已主动取消，不再修改状态
            if (_userRequestedCancel && result == DialogResult.Cancel)
                return;

            if (_form.IsDisposed || _isDisposed || !_form.IsHandleCreated)
                return;

            if (_form.InvokeRequired)
            {
                try
                {
                    _form.Invoke(new Action(() => SafeClose(result)));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"关闭对话框出错: {ex.Message}");
                }
            }
            else
            {
                _isOperationCompleted = result == DialogResult.OK;
                _form.DialogResult = result;
                _form.Close();
            }
        }

        public DialogResult ShowDialog(IWin32Window owner)
        {
            if (_form.Visible)
                throw new InvalidOperationException("对话框已经显示");

            // 防止外部直接关闭对话框时资源未清理
            var result = _form.ShowDialog(owner);
            Dispose(); // 自动清理资源
            return result;
        }

        public void Dispose()
        {
            if (_isDisposed) return;

            try
            {
                _cts.Cancel();
                // 取消并等待更新计时器任务完成
                try
                {
                    _updateTimerCts?.Cancel();
                    if (_updateTimerTask != null && !_updateTimerTask.IsCompleted)
                    {
                        _updateTimerTask.Wait(1000); // 给计时器任务更多时间完成
                    }
                }
                catch (AggregateException ex)
                {
                    // 忽略操作已取消的异常
                    ex.Handle(e => e is OperationCanceledException);
                }
                catch { }
                // 释放资源
                _updateTimerCts?.Dispose();
            

                if (!_form.IsDisposed)
                {
                    if (_form.IsHandleCreated)
                    {
                        _form.InvokeIfRequired(() => _form.Close());
                    }
                    _form.Dispose();
                }
            }
            finally
            {
                _isDisposed = true;
                _cts.Dispose();
            }
        }

        private void OnCancelRequested(object sender, EventArgs e)
        {
            _cts.Cancel();
            _form.UpdateProgress(-1, "正在取消操作...");
        }
    }

    
}
