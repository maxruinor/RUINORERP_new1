using System;
using Microsoft.Extensions.Logging;
using log4net;
using RUINORERP.Model.Context;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace RUINORERP.Common.Log4Net
{
    /// <summary>
    /// 简化版日志记录器,实现 Microsoft.Extensions.Logging.ILogger 接口
    /// </summary>
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _log;
        private readonly string _categoryName;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="categoryName">日志类别名称</param>
        public Log4NetLogger(string categoryName)
        {
            _categoryName = categoryName;
            // 使用共享日志仓库获取日志记录器
            _log = LogManager.GetLogger(Log4NetConfiguration.SHARED_REPOSITORY_NAME, categoryName);
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
        /// 记录日志
        /// </summary>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            try
            {
                // 设置日志上下文属性
                SetContextProperties(state, exception);

                // 格式化日志消息
                string message = formatter?.Invoke(state, exception) ?? string.Empty;

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
            catch (Exception ex)
            {
                // 记录日志失败时输出到调试窗口
                System.Diagnostics.Debug.WriteLine($"记录日志失败 [{logLevel}]: {ex.Message}");
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
                long? userId = null;
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
                    userId = appContext.log.User_ID;
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
                // 对于 User_ID，直接使用 DBNull.Value 而不是 null，避免类型转换错误
                ThreadContext.Properties["User_ID"] = userId.HasValue ? (object)userId.Value : DBNull.Value;
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
                ThreadContext.Properties["User_ID"] = null;
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
