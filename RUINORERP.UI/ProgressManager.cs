using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using RUINORERP.Common.CustomAttribute;
using System.Threading;
namespace RUINORERP.UI
{
    /*
    public sealed class ProgressManager : IDisposable
    {
        private static readonly Lazy<ProgressManager> instance =
            new Lazy<ProgressManager>(() => new ProgressManager());

        private ToolStripProgressBar _statusProgressBar;
        private ToolStripStatusLabel _statusLabel;
        private BackgroundWorker _worker;

        public static ProgressManager Instance => instance.Value;

        // 初始化时绑定到主窗体的statusStrip
        public void Initialize(ToolStripStatusLabel label, ToolStripProgressBar progressBar)
        {
            _statusLabel = label;
            _statusProgressBar = progressBar;
            ConfigureWorker();
        }

        private void ConfigureWorker()
        {
            _worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            _worker.DoWork += (s, e) =>
                ((Action<BackgroundWorker, DoWorkEventArgs>)e.Argument)(_worker, e);

            _worker.ProgressChanged += (s, e) =>
                UpdateUI(e.ProgressPercentage, e.UserState?.ToString());

            _worker.RunWorkerCompleted += (s, e) =>
                CompleteProgress(e.Error, e.Cancelled);
        }

        // 启动异步任务
        public void RunAsync(Action<BackgroundWorker> workAction,
                            Action<bool, Exception> completion = null)
        {
            if (_worker.IsBusy) return;

            _worker.RunWorkerAsync((Action<BackgroundWorker, DoWorkEventArgs>)((w, e) => {
                try
                {
                    workAction(w);
                    e.Result = true;
                }
                catch (Exception ex)
                {
                    e.Result = ex;
                }
            }));

            if (completion != null)
                _worker.RunWorkerCompleted += (s, e) =>
                    completion(e.Cancelled, e.Error ?? e.Result as Exception);
        }

        private void UpdateUI(int progress, string message)
        {
            if (_statusLabel.Owner.InvokeRequired)
            {
                _statusLabel.Owner.BeginInvoke((Action)(() =>
                    UpdateUI(progress, message)));
                return;
            }

            _statusProgressBar.Value = progress;
            _statusLabel.Text = message ?? $"Processing... {progress}%";
            _statusProgressBar.Visible = true;
        }

        private void CompleteProgress(Exception error = null, bool cancelled = false)
        {
            if (_statusLabel.Owner.InvokeRequired)
            {
                _statusLabel.Owner.BeginInvoke((Action)(() =>
                    CompleteProgress(error, cancelled)));
                return;
            }

            _statusProgressBar.Visible = false;
            _statusLabel.Text = cancelled ? "Cancelled" :
                (error != null ? $"Error: {error.Message}" : "Completed");
        }

        public void Dispose() => _worker?.Dispose();
    }
    */




    public sealed class ProgressManager : IDisposable
    {
        public ProgressManager()
        {
            
        }
        // 在 ProgressManager 类中添加状态标志
        private bool _isOperationCompleted;

        #region Singleton Implementation
        private static readonly Lazy<ProgressManager> _instance =
                new Lazy<ProgressManager>(() => new ProgressManager());
        public static ProgressManager Instance => _instance.Value;
        //private ProgressManager() { }
        #endregion

        #region Fields and Properties
        private ToolStripProgressBar _statusProgressBar;
        private ToolStripStatusLabel _statusLabel;
        private BackgroundWorker _worker;
        private bool _isInitialized;
        #endregion

        #region Public Methods
        public void Initialize(ToolStripStatusLabel label, ToolStripProgressBar progressBar)
        {
            if (label == null || progressBar == null)
                throw new ArgumentNullException("UI controls cannot be null");

            _statusLabel = label;
            _statusProgressBar = progressBar;
            _statusProgressBar.Visible = false;

            ConfigureWorker();
            _isInitialized = true;
        }

        public void RunAsync(
             Func<BackgroundWorker, Task> workAction, // 修改为 Func<Task>
            Action<bool, Exception> completion = null)
        {
            ValidateInitialization();

            if (_worker.IsBusy)
            {
                HandleBusyState(completion);
                return;
            }

            ConfigureWorkerCompletionHandler(completion);
            StartBackgroundWork(workAction); // 更新参数传递
        }
        #endregion

        #region Private Implementation
        private void ValidateInitialization()
        {
            if (!_isInitialized)
                throw new InvalidOperationException(
                    "ProgressManager must be initialized before use. " +
                    "Call Initialize() with valid UI controls first.");
        }

        private void ConfigureWorker()
        {
            if (_worker != null) return;

            _worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            _worker.DoWork += (s, e) => SafeExecuteWork(e);
            _worker.ProgressChanged += UpdateProgressSafely;
            _worker.RunWorkerCompleted += CompleteProgressSafely;
        }

        #region Private Implementation

        private void SafeExecuteWork(DoWorkEventArgs e)
        {
            try
            {
                var workAction = (Func<BackgroundWorker, Task>)e.Argument; // 修改为 Func<Task>
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

                // 同步执行异步操作，使用ConfigureAwait(false)避免上下文捕获
                workAction(_worker).ConfigureAwait(false).GetAwaiter().GetResult();

                e.Result = null;
            }
            catch (Exception ex)
            {
                e.Result = ex;
            }
        }

        #endregion

        private void ConfigureWorkerCompletionHandler(Action<bool, Exception> completion)
        {
            if (completion == null) return;

            _worker.RunWorkerCompleted += (s, e) =>
            {
                var error = e.Error ?? e.Result as Exception;
                completion(e.Cancelled, error);
            };
        }

        private void StartBackgroundWork(Func<BackgroundWorker, Task> workAction)
        {
            try
            {
                _statusProgressBar.Visible = true;
                _statusLabel.Text = "处理中...";
                _worker.RunWorkerAsync(workAction);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(
                    "Background worker failed to start. " +
                    "Ensure previous operations are completed.", ex);
            }
        }

        private void HandleBusyState(Action<bool, Exception> completion)
        {
            var error = new InvalidOperationException(
                "Previous operation is still in progress");

            completion?.Invoke(false, error);
            UpdateUI(0, "System busy - operation rejected");
        }
        #endregion

        #region UI Update Methods
        private void UpdateProgressSafely(object sender, ProgressChangedEventArgs e)
        {
            if (_isOperationCompleted) return; // 忽略已完成的进度更新

            if (_statusLabel.Owner.InvokeRequired)
            {
                _statusLabel.Owner.BeginInvoke((MethodInvoker)(() =>
                    UpdateProgressSafely(sender, e)));
                return;
            }

            _statusProgressBar.Value = Clamp(e.ProgressPercentage, 0, 100);
            _statusLabel.Text = e.UserState?.ToString() ??
                $"Processing... {e.ProgressPercentage}%";
            _statusProgressBar.Visible = true;
        }
        private static int Clamp(int value, int min, int max)
        {
            return value < min ? min : (value > max ? max : value);
        }
        private void CompleteProgressSafely(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_statusLabel.Owner.InvokeRequired)
            {
                _statusLabel.Owner.BeginInvoke((MethodInvoker)(() =>
                    CompleteProgressSafely(sender, e)));
                return;
            }

            _statusProgressBar.Visible = false;
            _isOperationCompleted = true; // 标记操作已完成
            var statusMessage = e.Cancelled ? "Operation cancelled" :
                e.Error != null ? $"Error: {e.Error.Message}" :
                e.Result is Exception ex ? $"Error: {ex.Message}" :
                "Operation completed";

            _statusLabel.Text = statusMessage;
        }

        private void UpdateUI(int progress, string message)
        {
            _statusProgressBar.Value = progress;
            _statusLabel.Text = message;
            _statusProgressBar.Visible = progress > 0;
        }
        #endregion

        #region Dispose Pattern
        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                if (_worker != null)
                {
                    if (_worker.IsBusy)
                    {
                        _worker.CancelAsync();
                        System.Threading.Thread.Sleep(100); // Allow cancellation
                    }
                    _worker.Dispose();
                }
            }

            _disposed = true;
        }

        ~ProgressManager() => Dispose(false);
        #endregion
    }
}


/*
 // 在状态栏添加控件
        var progressBar = new ToolStripProgressBar { 
            Style = ProgressBarStyle.Continuous,
            Visible = false
        };
        var statusLabel = new ToolStripStatusLabel();
        statusStrip1.Items.AddRange(new ToolStripItem[] { statusLabel, progressBar });
        
        ProgressManager.Instance.Initialize(statusLabel, progressBar);



// 在需要取消的地方调用
ProgressManager.Instance.Dispose(); // 安全取消并释放

worker.ReportSafeProgress(currentProgress);
if (!worker.ShouldContinue()) return;
 */