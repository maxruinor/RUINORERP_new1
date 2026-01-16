using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Common.Log4Net
{
    /// <summary>
    /// 简化版日志提供者,实现 Microsoft.Extensions.Logging.ILoggerProvider 接口
    /// </summary>
    public class Log4NetProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, Log4NetLogger> _loggers;
        private bool _disposed;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Log4NetProvider()
        {
            _loggers = new ConcurrentDictionary<string, Log4NetLogger>();
        }

        /// <summary>
        /// 创建日志记录器
        /// </summary>
        /// <param name="categoryName">日志类别名称</param>
        /// <returns>日志记录器实例</returns>
        public ILogger CreateLogger(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                throw new ArgumentException("日志类别名称不能为空", nameof(categoryName));
            }

            return _loggers.GetOrAdd(categoryName, name => new Log4NetLogger(name));
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _loggers.Clear();
                _disposed = true;
            }
        }
    }
}
