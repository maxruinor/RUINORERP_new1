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

        // ========== 性能优化: 分层缓存策略 ==========
        
        /// <summary>
        /// 线程局部的固定属性缓存(UserName, MachineName, IP, MAC, Operator)
        /// 这些属性在用户会话期间几乎不变,缓存以避免重复读取 ApplicationContext
        /// </summary>
        private static readonly System.Threading.AsyncLocal<FixedPropertiesCache> _fixedPropsCache = 
            new System.Threading.AsyncLocal<FixedPropertiesCache>();
        
        /// <summary>
        /// 固定属性缓存结构
        /// </summary>
        private class FixedPropertiesCache
        {
            public string UserName { get; set; } = string.Empty;
            public string MachineName { get; set; } = string.Empty;
            public string IP { get; set; } = string.Empty;
            public string MAC { get; set; } = string.Empty;
            public string Operator { get; set; } = string.Empty;
            public int LastUpdateTick { get; set; }  // 最后更新时间戳(用于检测用户切换)
            
            /// <summary>
            /// 缓存有效期(毫秒),默认 5 秒
            /// 超过此时间后重新检查用户是否切换
            /// </summary>
            public const int CacheValidityMs = 5000;
            
            public bool IsExpired => Math.Abs(Environment.TickCount - LastUpdateTick) > CacheValidityMs;
        }
        
        /// <summary>
        /// 线程局部的半固定属性缓存(ModName, ActionName, Path)
        /// 这些属性在一个业务请求/操作期间不变
        /// 使用 AsyncLocal 确保异步上下文中的正确性
        /// </summary>
        private static readonly System.Threading.AsyncLocal<SemiFixedPropertiesCache> _semiFixedPropsCache = 
            new System.Threading.AsyncLocal<SemiFixedPropertiesCache>();
        
        /// <summary>
        /// 半固定属性缓存结构
        /// </summary>
        private class SemiFixedPropertiesCache
        {
            public string ModName { get; set; } = string.Empty;
            public string ActionName { get; set; } = string.Empty;
            public string Path { get; set; } = string.Empty;
            public int LastUpdateTick { get; set; }
            
            /// <summary>
            /// 缓存有效期(毫秒),默认 1 秒
            /// 业务操作通常在几百毫秒内完成,1秒足够覆盖
            /// </summary>
            public const int CacheValidityMs = 1000;
            
            public bool IsExpired => Math.Abs(Environment.TickCount - LastUpdateTick) > CacheValidityMs;
        }

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
        /// 释放静态资源(在应用程序关闭时调用)
        /// </summary>
        public static void Dispose()
        {
            _flushTimer?.Dispose();
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
        /// 缓冲日志(用于高频日志)
        /// </summary>
        private void BufferLog(LogLevel logLevel, string message, Exception exception)
        {
            // 设置日志上下文属性 - 传递格式化后的消息
            SetContextProperties(message, exception);
        
            var entry = new LogEntry
            {
                Level = logLevel,
                Message = message,
                Exception = exception
            };
        
            // 如果缓冲区未满,加入队列
            if (_logBuffer.Count < BufferSize)
            {
                _logBuffer.Enqueue(entry);
            }
            else
            {
                // 缓冲区已满,直接输出
                WriteLogDirectly(logLevel, message, exception);
            }
        }

        /// <summary>
        /// 直接写入日志(用于重要日志)
        /// </summary>
        private void WriteLogDirectly(LogLevel logLevel, string message, Exception exception)
        {
            // 设置日志上下文属性 - 传递格式化后的消息
            SetContextProperties(message, exception);

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

                // 批量输出 - 性能优化: 在循环外设置固定和半固定属性
                var fixedCache = RefreshFixedPropertiesCacheStatic();
                var semiFixedCache = RefreshSemiFixedPropertiesCacheStatic();
                
                foreach (var entry in tempBuffer)
                {
                    try
                    {
                        // 创建临时日志记录器并设置上下文属性
                        var logger = LogManager.GetLogger(Log4NetConfiguration.SHARED_REPOSITORY_NAME, "BufferedLog");
                        
                        // 从缓存快速读取固定属性
                        ThreadContext.Properties["UserName"] = fixedCache.UserName;
                        ThreadContext.Properties["MachineName"] = fixedCache.MachineName;
                        ThreadContext.Properties["IP"] = fixedCache.IP;
                        ThreadContext.Properties["MAC"] = fixedCache.MAC;
                        ThreadContext.Properties["Operator"] = fixedCache.Operator;
                        
                        // 从缓存快速读取半固定属性
                        ThreadContext.Properties["ModName"] = semiFixedCache.ModName;
                        ThreadContext.Properties["ActionName"] = semiFixedCache.ActionName;
                        ThreadContext.Properties["Path"] = semiFixedCache.Path;
                        
                        // 设置变化属性(每条日志不同)
                        ThreadContext.Properties["Message"] = entry.Message ?? string.Empty;
                        ThreadContext.Properties["Exception"] = entry.Exception != null ? GetFullExceptionStatic(entry.Exception) : string.Empty;
                        
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
        /// 设置日志上下文属性到 ThreadContext.Properties (优化版: 分层缓存)
        /// </summary>
        /// <param name="message">日志消息(已格式化)</param>
        /// <param name="exception">异常对象</param>
        private void SetContextProperties(string message, Exception exception)
        {
            try
            {
                // ========== 第1层: 固定属性缓存(UserName, MachineName, IP, MAC, Operator) ==========
                var fixedCache = _fixedPropsCache.Value;
                if (fixedCache == null || fixedCache.IsExpired)
                {
                    fixedCache = RefreshFixedPropertiesCache();
                    _fixedPropsCache.Value = fixedCache;
                }
                
                // 从缓存快速读取固定属性
                ThreadContext.Properties["UserName"] = fixedCache.UserName;
                ThreadContext.Properties["MachineName"] = fixedCache.MachineName;
                ThreadContext.Properties["IP"] = fixedCache.IP;
                ThreadContext.Properties["MAC"] = fixedCache.MAC;
                ThreadContext.Properties["Operator"] = fixedCache.Operator;
                
                // ========== 第2层: 半固定属性缓存(ModName, ActionName, Path) ==========
                var semiFixedCache = _semiFixedPropsCache.Value;
                if (semiFixedCache == null || semiFixedCache.IsExpired)
                {
                    semiFixedCache = RefreshSemiFixedPropertiesCache();
                    _semiFixedPropsCache.Value = semiFixedCache;
                }
                
                // 从缓存快速读取半固定属性
                ThreadContext.Properties["ModName"] = semiFixedCache.ModName;
                ThreadContext.Properties["ActionName"] = semiFixedCache.ActionName;
                ThreadContext.Properties["Path"] = semiFixedCache.Path;

                // ========== 第3层: 变化属性(每次都设置) ==========
                ThreadContext.Properties["Message"] = message ?? string.Empty;
                ThreadContext.Properties["Exception"] = exception != null ? GetFullException(exception) : string.Empty;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"设置日志上下文属性失败: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 刷新固定属性缓存(慢速路径,只在缓存过期时调用)
        /// </summary>
        private FixedPropertiesCache RefreshFixedPropertiesCache()
        {
            var cache = new FixedPropertiesCache();
            var appContext = ApplicationContext.Current;
            
            // 从应用上下文获取固定属性
            if (appContext?.log != null)
            {
                cache.UserName = appContext.log.UserName ?? string.Empty;
                cache.IP = appContext.log.IP ?? string.Empty;
                cache.MAC = appContext.log.MAC ?? string.Empty;
                cache.MachineName = appContext.log.MachineName ?? Environment.MachineName;
            }
            else
            {
                cache.UserName = string.Empty;
                cache.IP = string.Empty;
                cache.MAC = string.Empty;
                cache.MachineName = Environment.MachineName ?? string.Empty;
            }

            if (appContext?.CurUserInfo != null)
            {
                cache.Operator = appContext.CurUserInfo.客户端版本;
            }
            else
            {
                cache.Operator = "系统服务";
            }
            
            cache.LastUpdateTick = Environment.TickCount;
            return cache;
        }
        
        /// <summary>
        /// 刷新半固定属性缓存(慢速路径,只在缓存过期时调用)
        /// </summary>
        private SemiFixedPropertiesCache RefreshSemiFixedPropertiesCache()
        {
            var cache = new SemiFixedPropertiesCache();
            var appContext = ApplicationContext.Current;
            
            // 从应用上下文获取半固定属性
            if (appContext?.log != null)
            {
                cache.ModName = appContext.log.ModName ?? string.Empty;
                cache.ActionName = appContext.log.ActionName ?? string.Empty;
                cache.Path = appContext.log.Path ?? string.Empty;
            }
            else
            {
                cache.ModName = string.Empty;
                cache.ActionName = string.Empty;
                cache.Path = string.Empty;
            }
            
            cache.LastUpdateTick = Environment.TickCount;
            return cache;
        }
        
        /// <summary>
        /// 刷新固定属性缓存(静态版本,用于 FlushBufferCallback)
        /// </summary>
        private static FixedPropertiesCache RefreshFixedPropertiesCacheStatic()
        {
            var cache = new FixedPropertiesCache();
            var appContext = RUINORERP.Model.Context.ApplicationContext.Current;
            
            if (appContext?.log != null)
            {
                cache.UserName = appContext.log.UserName ?? string.Empty;
                cache.IP = appContext.log.IP ?? string.Empty;
                cache.MAC = appContext.log.MAC ?? string.Empty;
                cache.MachineName = appContext.log.MachineName ?? Environment.MachineName;
            }
            else
            {
                cache.UserName = string.Empty;
                cache.IP = string.Empty;
                cache.MAC = string.Empty;
                cache.MachineName = Environment.MachineName ?? string.Empty;
            }

            if (appContext?.CurUserInfo != null)
            {
                cache.Operator = appContext.CurUserInfo.客户端版本;
            }
            else
            {
                cache.Operator = "系统服务";
            }
            
            cache.LastUpdateTick = Environment.TickCount;
            return cache;
        }
        
        /// <summary>
        /// 刷新半固定属性缓存(静态版本,用于 FlushBufferCallback)
        /// </summary>
        private static SemiFixedPropertiesCache RefreshSemiFixedPropertiesCacheStatic()
        {
            var cache = new SemiFixedPropertiesCache();
            var appContext = RUINORERP.Model.Context.ApplicationContext.Current;
            
            if (appContext?.log != null)
            {
                cache.ModName = appContext.log.ModName ?? string.Empty;
                cache.ActionName = appContext.log.ActionName ?? string.Empty;
                cache.Path = appContext.log.Path ?? string.Empty;
            }
            else
            {
                cache.ModName = string.Empty;
                cache.ActionName = string.Empty;
                cache.Path = string.Empty;
            }
            
            cache.LastUpdateTick = Environment.TickCount;
            return cache;
        }

        /// <summary>
        /// 获取完整的异常信息(包括所有 InnerException)
        /// </summary>
        /// <param name="exception">异常对象</param>
        /// <returns>完整的异常信息字符串</returns>
        private string GetFullException(Exception exception)
        {
            return GetFullExceptionStatic(exception);
        }

        /// <summary>
        /// 获取完整的异常信息(静态版本,用于静态方法调用)
        /// </summary>
        /// <param name="exception">异常对象</param>
        /// <returns>完整的异常信息字符串</returns>
        private static string GetFullExceptionStatic(Exception exception)
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
