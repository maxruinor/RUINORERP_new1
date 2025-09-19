using System;
using Microsoft.Extensions.Logging;

namespace RUINORERP.PacketSpec.Utilities
{
    /// <summary>
    /// 简单的控制台日志器实现，用于在没有配置日志系统时提供基本的日志功能
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        private readonly string _name;

        public ConsoleLogger(string name = "Default")
        {
            _name = name;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var message = formatter(state, exception);
            var logMessage = $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] [{logLevel}] [{_name}] {message}";
            
            switch (logLevel)
            {
                case LogLevel.Critical:
                case LogLevel.Error:
                    Console.Error.WriteLine(logMessage);
                    if (exception != null)
                        Console.Error.WriteLine(exception);
                    break;
                default:
                    Console.WriteLine(logMessage);
                    break;
            }
        }
    }
}