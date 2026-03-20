using System;
using Microsoft.Extensions.Logging;
using log4net;
using RUINORERP.Model.Context;
using System.Collections.Concurrent;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace RUINORERP.Common.Log4Net
{
    /// <summary>
    /// 简化版日志记录器 - 优化版1
    /// 实现 Microsoft.Extensions.Logging.ILogger 接口
    /// 添加日志缓冲机制，避免频繁调用log4net
    /// </summary>
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _log;
        private readonly string _categoryName;

        // 日志缓冲（仅用于高频日志，如Debug/Trace）
        private static readonly ConcurrentQueue<LogEntry> _logBuffer = new ConcurrentQueue<LogEntry>();
        private static readonly System.Threading.Timer _flushTimer;
        private static volatile bool _isFlushing = false;

        /// <summary>
        /// 缓冲区大小，默认1000
        /// </summary>
        private const int BufferSize = 1000;

        /// <summary>
        /// 刷新间隔（毫秒），默认100ms
        /// </summary>
        private const int FlushInterval = 100;

        static Log4NetLogger()
        {
            // 初始化定时刷新器
            _flushTimer = new System.Threading.Timer(
                FlushBufferCallback,
                null,
                FlushInterval,
                FlushInterval);
        }

        /// <summary>
        /// 日志条目
        /// </summary>
        private struct LogEntry
        {
            public LogLevel Level;
            public string Message;
            public Exception Exception;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="categoryName">日志类别名称</param>
        public Log4NetLogger(string categoryName)
        {
            _categoryName = categoryName;
            try
            {
                // 尝试使用共享日志仓库获取日志记录器
                _log = LogManager.GetLogger(Log4NetConfiguration.SHARED_REPOSITORY_NAME, categoryName);
            }
            catch (Exception ex)
            {
                // 仓库不存在时，使用默认仓库
                System.Diagnostics.Debug.WriteLine($"日志仓库初始化失败: {ex.Message}，使用默认仓库");
                _log = LogManager.GetLogger(categoryName);
            }
        }

        /// <summary>
        /// 开始作用域
        /// </summary>
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        /// <summary>
        /// 判断日志级别是否启用
        /// </summary>
        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    return _log.IsDebugEnabled;
                case LogLevel.Information:
                    return _log.IsInfoEnabled;
                case LogLevel.Warning:
                    return _log.IsWarnEnabled;
                case LogLevel.Error:
                    return _log.IsErrorEnabled;
                case LogLevel.Critical:
                    return _log.IsFatalEnabled;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 记录日志 - 优化版，添加缓冲机制
        /// </summary>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            try
            {
                // 格式化日志消息
                string message = formatter?.Invoke(state, exception) ?? string.Empty;

                // 高频日志（Debug/Trace）使用缓冲机制
                if (logLevel == LogLevel.Debug || logLevel == LogLevel.Trace)
                {
                    BufferLog(logLevel, message, exception);
                }
                else
                {
                    // 重要日志直接输出，不缓冲
                    WriteLogDirectly(logLevel, message, exception);
                }
            }
            catch (Exception ex)
            {
                // 记录日志失败时输出到调试窗口
                System.Diagnostics.Debug.WriteLine($"记录日志失败 [{logLevel}]: {ex.Message}");
            }
        }

        /// <summary>
        /// 缓冲日志（用于高频日志）
        /// </summary>
        private void BufferLog(LogLevel logLevel, string message, Exception exception)
        {
            // 设置日志上下文属性
            SetContextProperties<string>(null, exception);

            var entry = new LogEntry
            {
                Level = logLevel,
                Message = message,
                Exception = exception
            };

            // 如果缓冲区未满，加入队列
            if (_logBuffer.Count < BufferSize)
            {
                _logBuffer.Enqueue(entry);
            }
            else
            {
                // 缓冲区已满，直接输出
                WriteLogDirectly(logLevel, message, exception);
            }
        }

        /// <summary>
        /// 直接写入日志（用于重要日志）
        /// </summary>
        private void WriteLogDirectly(LogLevel logLevel, string message, Exception exception)
        {
            // 设置日志上下文属性
            SetContextProperties<string>(null, exception);

            // 根据日志级别记录
            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    _log.Debug(message, exception);
                    break;
                case LogLevel.Information:
                    _log.Info(message, exception);
                    break;
                case LogLevel.Warning:
                    _log.Warn(message, exception);
                    break;
                case LogLevel.Error:
                    _log.Error(message, exception);
                    break;
                case LogLevel.Critical:
                    _log.Fatal(message, exception);
                    break;
                default:
                    _log.Warn($"未知的日志级别: {logLevel}, 消息: {message}", exception);
                    break;
            }
        }

        /// <summary>
        /// 定时刷新缓冲区
        /// </summary>
        private static void FlushBufferCallback(object state)
        {
            // 防止重复刷新
            if (_isFlushing) return;

            try
            {
                _isFlushing = true;

                var tempBuffer = new System.Collections.Generic.List<LogEntry>();

                // 取出所有缓冲的日志
                while (_logBuffer.TryDequeue(out var entry))
                {
                    tempBuffer.Add(entry);
                }

                // 批量输出
                foreach (var entry in tempBuffer)
                {
                    try
                    {
                        var logger = LogManager.GetLogger(Log4NetConfiguration.SHARED_REPOSITORY_NAME, "BufferedLog");
                        switch (entry.Level)
                        {
                            case LogLevel.Debug:
                                logger.Debug(entry.Message, entry.Exception);
                                break;
                            case LogLevel.Trace:
                                logger.Debug(entry.Message, entry.Exception);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"刷新日志缓冲失败: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"日志缓冲刷新异常: {ex.Message}");
            }
            finally
            {
                _isFlushing = false;
            }
        }

        /// <summary>
        /// 设置日志上下文属性到 ThreadContext.Properties
        /// </summary>
        /// <param name="state">日志状态</param>
        /// <param name="exception">异常对象</param>
        private void SetContextProperties<TState>(TState state, Exception exception)
        {
            try
            {
                // 获取应用程序上下文
                var appContext = ApplicationContext.Current;

                // 设置默认值
                string userName = string.Empty;
                string modName = string.Empty;
                string actionName = string.Empty;
                string path = string.Empty;
                string ip = string.Empty;
                string mac = string.Empty;
                string machineName = Environment.MachineName ?? string.Empty;
                string operatorName = "系统服务";

                // 从应用上下文获取值
                if (appContext?.log != null)
                {
                    userName = appContext.log.UserName ?? string.Empty;
                    modName = appContext.log.ModName ?? string.Empty;
                    actionName = appContext.log.ActionName ?? string.Empty;
                    path = appContext.log.Path ?? string.Empty;
                    ip = appContext.log.IP ?? string.Empty;
                    mac = appContext.log.MAC ?? string.Empty;
                    machineName = appContext.log.MachineName ?? machineName;
                }

                if (appContext?.CurUserInfo != null)
                {
                    operatorName = appContext.CurUserInfo.客户端版本 ?? "已登录用户";
                }

                // 设置所有属性
                ThreadContext.Properties["UserName"] = userName;
                ThreadContext.Properties["ModName"] = modName;
                ThreadContext.Properties["ActionName"] = actionName;
                ThreadContext.Properties["Path"] = path;
                ThreadContext.Properties["IP"] = ip;
                ThreadContext.Properties["MAC"] = mac;
                ThreadContext.Properties["MachineName"] = machineName;
                ThreadContext.Properties["Operator"] = operatorName;

                // 设置消息和异常
                string message = state?.ToString() ?? string.Empty;
                ThreadContext.Properties["Message"] = message;

                // 如果有异常，递归处理 InnerException
                if (exception != null)
                {
                    ThreadContext.Properties["Exception"] = GetFullException(exception);
                }
                else
                {
                    ThreadContext.Properties["Exception"] = string.Empty;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"设置日志上下文属性失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取完整的异常信息(包括所有 InnerException)
        /// </summary>
        /// <param name="exception">异常对象</param>
        /// <returns>完整的异常信息字符串</returns>
        private string GetFullException(Exception exception)
        {
            if (exception == null)
            {
                return string.Empty;
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            Exception current = exception;

            int depth = 0;
            while (current != null && depth < 10) // 限制递归深度,防止无限循环
            {
                if (depth > 0)
                {
                    sb.AppendLine();
                    sb.AppendLine("--- InnerException ---");
                }

                sb.AppendLine($"类型: {current.GetType().FullName}");
                sb.AppendLine($"消息: {current.Message}");
                sb.AppendLine($"堆栈: {current.StackTrace}");

                current = current.InnerException;
                depth++;
            }

            return sb.ToString();
        }
    }
}
