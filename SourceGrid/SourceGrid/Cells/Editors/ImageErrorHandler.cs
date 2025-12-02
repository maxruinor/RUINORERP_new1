using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SourceGrid.Cells.Editors
{
    /// <summary>
    /// 图片处理错误处理和用户体验优化类
    /// 提供统一的错误处理、用户提示和体验优化功能
    /// </summary>
    public static class ImageErrorHandler
    {
        #region 配置属性

        /// <summary>
        /// 是否显示详细错误信息
        /// </summary>
        public static bool ShowDetailedErrors { get; set; } = false;

        /// <summary>
        /// 是否自动重试失败的加载
        /// </summary>
        public static bool EnableAutoRetry { get; set; } = true;

        /// <summary>
        /// 最大重试次数
        /// </summary>
        public static int MaxRetryCount { get; set; } = 3;

        /// <summary>
        /// 重试间隔（毫秒）
        /// </summary>
        public static int RetryIntervalMs { get; set; } = 1000;

        #endregion

        #region 错误处理方法

        /// <summary>
        /// 处理图片加载错误
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="context">错误上下文</param>
        /// <returns>是否已处理</returns>
        public static bool HandleImageLoadError(Exception error, string context = null)
        {
            try
            {
                var errorType = ClassifyError(error);
                var userMessage = GetUserFriendlyMessage(errorType, error);
                
                // 记录错误
                LogError(error, context);
                
                // 显示用户友好的错误信息
                if (ShouldShowUserMessage(errorType))
                {
                    ShowUserMessage(userMessage, errorType);
                }

                return true;
            }
            catch
            {
                // 防止错误处理器本身出错
                return false;
            }
        }

        /// <summary>
        /// 处理图片处理警告
        /// </summary>
        /// <param name="warning">警告信息</param>
        /// <param name="context">警告上下文</param>
        public static void HandleImageWarning(string warning, string context = null)
        {
            try
            {
                LogWarning(warning, context);
                
                // 非关键警告，仅记录日志
                if (IsUserVisibleWarning(warning))
                {
                    ShowUserWarning(warning);
                }
            }
            catch
            {
                // 防止警告处理器本身出错
            }
        }

        #endregion

        #region 自动重试机制

        /// <summary>
        /// 带自动重试的图片加载
        /// </summary>
        /// <param name="loadFunc">加载函数</param>
        /// <param name="context">上下文信息</param>
        /// <param name="onSuccess">成功回调</param>
        /// <param name="onFailure">失败回调</param>
        public static async Task<T> LoadWithRetryAsync<T>(
            Func<Task<T>> loadFunc, 
            string context = null,
            Action<T> onSuccess = null,
            Action<Exception> onFailure = null) where T : class
        {
            if (!EnableAutoRetry)
            {
                return await ExecuteSingleAttempt(loadFunc, context, onSuccess, onFailure);
            }

            Exception lastException = null;

            for (int attempt = 1; attempt <= MaxRetryCount; attempt++)
            {
                try
                {
                    var result = await loadFunc();
                    
                    // 成功时取消之前的错误状态
                    if (attempt > 1)
                    {
                        ShowSuccessMessage($"图片加载成功（重试 {attempt - 1} 次后）");
                    }

                    onSuccess?.Invoke(result);
                    return result;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    LogWarning($"图片加载失败，尝试 {attempt}/{MaxRetryCount}: {ex.Message}", context);

                    if (attempt < MaxRetryCount)
                    {
                        await Task.Delay(RetryIntervalMs * attempt); // 递增延迟
                    }
                }
            }

            // 所有尝试都失败
            HandleImageLoadError(lastException, context);
            onFailure?.Invoke(lastException);
            return null;
        }

        #endregion

        #region 用户体验优化

        /// <summary>
        /// 显示加载指示器
        /// </summary>
        /// <param name="control">要显示指示器的控件</param>
        /// <param name="message">加载消息</param>
        /// <returns>指示器控制器</returns>
        public static IDisposable ShowLoadingIndicator(Control control, string message = "正在加载图片...")
        {
            if (control == null || !control.InvokeRequired)
                return null;

            return new LoadingIndicator(control, message);
        }

        /// <summary>
        /// 显示进度指示器
        /// </summary>
        /// <param name="control">父控件</param>
        /// <param name="maxValue">最大值</param>
        /// <returns>进度控制器</returns>
        public static IProgressController ShowProgressIndicator(Control control, int maxValue = 100)
        {
            if (control == null)
                return null;

            return new ProgressIndicator(control, maxValue);
        }

        /// <summary>
        /// 显示图片预览加载状态
        /// </summary>
        /// <param name="cellContext">单元格上下文</param>
        /// <param name="status">加载状态</param>
        public static void ShowCellLoadingStatus(CellContext cellContext, ImageLoadingStatus status)
        {
            try
            {
                if (cellContext.Grid == null || cellContext.Grid.IsDisposed)
                    return;

                switch (status)
                {
                    case ImageLoadingStatus.Loading:
                        // 显示加载中状态
                        cellContext.Grid.Invoke((Action)(() =>
                        {
                            // 可以在这里设置加载中的显示样式
                            cellContext.Cell.BackColor = Color.LightGray;
                        }));
                        break;

                    case ImageLoadingStatus.Success:
                        // 恢复正常状态
                        cellContext.Grid.Invoke((Action)(() =>
                        {
                            cellContext.Cell.BackColor = Color.Empty;
                        }));
                        break;

                    case ImageLoadingStatus.Error:
                        // 显示错误状态
                        cellContext.Grid.Invoke((Action)(() =>
                        {
                            cellContext.Cell.BackColor = Color.LightPink;
                        }));
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"显示单元格加载状态失败: {ex.Message}");
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 错误分类
        /// </summary>
        private static ImageErrorType ClassifyError(Exception error)
        {
            if (error == null)
                return ImageErrorType.Unknown;

            if (error is FileNotFoundException || error is DirectoryNotFoundException)
                return ImageErrorType.FileNotFound;

            if (error is OutOfMemoryException)
                return ImageErrorType.OutOfMemory;

            if (error is UnauthorizedAccessException || error is System.Security.SecurityException)
                return ImageErrorType.AccessDenied;

            if (error is ArgumentException || error is InvalidOperationException)
                return ImageErrorType.InvalidData;

            if (error is System.Net.WebException || error is System.Net.Http.HttpRequestException)
                return ImageErrorType.NetworkError;

            return ImageErrorType.ProcessingError;
        }

        /// <summary>
        /// 获取用户友好的错误消息
        /// </summary>
        private static string GetUserFriendlyMessage(ImageErrorType errorType, Exception error)
        {
            var baseMessage = GetBaseErrorMessage(errorType);

            if (ShowDetailedErrors)
            {
                return $"{baseMessage}\n\n详细信息: {error.Message}";
            }

            return baseMessage;
        }

        /// <summary>
        /// 获取基础错误消息
        /// </summary>
        private static string GetBaseErrorMessage(ImageErrorType errorType)
        {
            switch (errorType)
            {
                case ImageErrorType.FileNotFound:
                    return "找不到图片文件，请检查文件路径是否正确。";

                case ImageErrorType.OutOfMemory:
                    return "内存不足，无法加载图片。请尝试关闭其他应用程序或选择较小的图片。";

                case ImageErrorType.AccessDenied:
                    return "没有权限访问图片文件，请检查文件权限设置。";

                case ImageErrorType.InvalidData:
                    return "图片文件格式不支持或已损坏，请选择有效的图片文件。";

                case ImageErrorType.NetworkError:
                    return "网络连接失败，无法下载图片。请检查网络连接。";

                case ImageErrorType.ProcessingError:
                    return "图片处理过程中发生错误，请重试或联系技术支持。";

                default:
                    return "加载图片时发生未知错误，请重试。";
            }
        }

        /// <summary>
        /// 判断是否应该显示用户消息
        /// </summary>
        private static bool ShouldShowUserMessage(ImageErrorType errorType)
        {
            // 某些错误类型可能不需要显示给用户
            switch (errorType)
            {
                case ImageErrorType.ProcessingError:
                case ImageErrorType.NetworkError:
                    return true;
                default:
                    return true;
            }
        }

        /// <summary>
        /// 显示用户消息
        /// </summary>
        private static void ShowUserMessage(string message, ImageErrorType errorType)
        {
            try
            {
                var icon = GetMessageBoxIcon(errorType);
                var title = GetMessageBoxTitle(errorType);
                
                MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"显示用户消息失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取消息框图标
        /// </summary>
        private static MessageBoxIcon GetMessageBoxIcon(ImageErrorType errorType)
        {
            switch (errorType)
            {
                case ImageErrorType.OutOfMemory:
                case ImageErrorType.AccessDenied:
                case ImageErrorType.NetworkError:
                    return MessageBoxIcon.Warning;

                case ImageErrorType.FileNotFound:
                case ImageErrorType.InvalidData:
                case ImageErrorType.ProcessingError:
                    return MessageBoxIcon.Error;

                default:
                    return MessageBoxIcon.Information;
            }
        }

        /// <summary>
        /// 获取消息框标题
        /// </summary>
        private static string GetMessageBoxTitle(ImageErrorType errorType)
        {
            switch (errorType)
            {
                case ImageErrorType.FileNotFound:
                    return "文件未找到";
                case ImageErrorType.OutOfMemory:
                    return "内存不足";
                case ImageErrorType.AccessDenied:
                    return "访问被拒绝";
                case ImageErrorType.InvalidData:
                    return "文件格式错误";
                case ImageErrorType.NetworkError:
                    return "网络错误";
                case ImageErrorType.ProcessingError:
                    return "处理错误";
                default:
                    return "错误";
            }
        }

        /// <summary>
        /// 显示用户警告
        /// </summary>
        private static void ShowUserWarning(string warning)
        {
            try
            {
                MessageBox.Show(warning, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"显示用户警告失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 显示成功消息
        /// </summary>
        private static void ShowSuccessMessage(string message)
        {
            try
            {
                // 可以使用更轻量级的通知方式，如ToolTip或状态栏
                System.Diagnostics.Debug.WriteLine($"[SUCCESS] {message}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"显示成功消息失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 判断是否为用户可见的警告
        /// </summary>
        private static bool IsUserVisibleWarning(string warning)
        {
            // 可以根据警告内容判断是否需要显示给用户
            return !warning.Contains("内部") && !warning.Contains("Debug");
        }

        /// <summary>
        /// 记录错误
        /// </summary>
        private static void LogError(Exception error, string context)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] [{context}] {error.GetType().Name}: {error.Message}");
                System.Diagnostics.Debug.WriteLine($"[ERROR] StackTrace: {error.StackTrace}");
            }
            catch
            {
                // 防止日志记录失败
            }
        }

        /// <summary>
        /// 记录警告
        /// </summary>
        private static void LogWarning(string warning, string context)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[WARNING] [{context}] {warning}");
            }
            catch
            {
                // 防止日志记录失败
            }
        }

        /// <summary>
        /// 执行单次尝试
        /// </summary>
        private static async Task<T> ExecuteSingleAttempt<T>(
            Func<Task<T>> loadFunc, 
            string context, 
            Action<T> onSuccess, 
            Action<Exception> onFailure) where T : class
        {
            try
            {
                var result = await loadFunc();
                onSuccess?.Invoke(result);
                return result;
            }
            catch (Exception ex)
            {
                HandleImageLoadError(ex, context);
                onFailure?.Invoke(ex);
                return null;
            }
        }

        #endregion
    }

    #region 枚举定义

    /// <summary>
    /// 图片错误类型
    /// </summary>
    public enum ImageErrorType
    {
        Unknown,
        FileNotFound,
        OutOfMemory,
        AccessDenied,
        InvalidData,
        NetworkError,
        ProcessingError
    }

    /// <summary>
    /// 图片加载状态
    /// </summary>
    public enum ImageLoadingStatus
    {
        None,
        Loading,
        Success,
        Error
    }

    #endregion

    #region 辅助类

    /// <summary>
    /// 加载指示器
    /// </summary>
    internal class LoadingIndicator : IDisposable
    {
        private readonly Control _control;
        private readonly Cursor _originalCursor;

        public LoadingIndicator(Control control, string message)
        {
            _control = control;
            _originalCursor = control.Cursor;
            
            if (control.InvokeRequired)
            {
                control.Invoke((Action)(() =>
                {
                    control.Cursor = Cursors.WaitCursor;
                    // 可以在这里添加更多的加载指示UI
                }));
            }
            else
            {
                control.Cursor = Cursors.WaitCursor;
            }
        }

        public void Dispose()
        {
            if (_control != null && !_control.IsDisposed)
            {
                if (_control.InvokeRequired)
                {
                    _control.Invoke((Action)(() =>
                    {
                        _control.Cursor = _originalCursor;
                    }));
                }
                else
                {
                    _control.Cursor = _originalCursor;
                }
            }
        }
    }

    /// <summary>
    /// 进度控制器接口
    /// </summary>
    public interface IProgressController
    {
        void UpdateProgress(int value);
        void SetMessage(string message);
        void Dispose();
    }

    /// <summary>
    /// 进度指示器
    /// </summary>
    internal class ProgressIndicator : IProgressController
    {
        private readonly Control _control;
        private readonly int _maxValue;
        private readonly Form _progressForm;
        private readonly ProgressBar _progressBar;
        private readonly Label _messageLabel;

        public ProgressIndicator(Control control, int maxValue)
        {
            _control = control;
            _maxValue = maxValue;
            
            // 创建简单的进度窗口
            _progressForm = new Form
            {
                Text = "处理中...",
                Size = new Size(300, 100),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                ShowInTaskbar = false
            };

            _progressBar = new ProgressBar
            {
                Location = new Point(20, 20),
                Size = new Size(260, 20),
                Maximum = maxValue
            };

            _messageLabel = new Label
            {
                Location = new Point(20, 50),
                Size = new Size(260, 20),
                Text = "正在处理..."
            };

            _progressForm.Controls.Add(_progressBar);
            _progressForm.Controls.Add(_messageLabel);
            
            _progressForm.Show(_control.FindForm());
        }

        public void UpdateProgress(int value)
        {
            if (_progressBar != null && !_progressBar.IsDisposed)
            {
                _progressBar.Invoke((Action)(() =>
                {
                    _progressBar.Value = Math.Min(value, _maxValue);
                }));
            }
        }

        public void SetMessage(string message)
        {
            if (_messageLabel != null && !_progressForm.IsDisposed)
            {
                _messageLabel.Invoke((Action)(() =>
                {
                    _messageLabel.Text = message;
                }));
            }
        }

        public void Dispose()
        {
            if (_progressForm != null && !_progressForm.IsDisposed)
            {
                _progressForm.Invoke((Action)(() =>
                {
                    _progressForm.Close();
                    _progressForm.Dispose();
                }));
            }
        }
    }

    #endregion
}