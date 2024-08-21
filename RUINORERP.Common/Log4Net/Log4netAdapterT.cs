using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Common.Log4Net
{
    /*
      container.RegisterConditional(
    typeof(ILogger),
    c => typeof(Log4netAdapter<>).MakeGenericType(c.Consumer.ImplementationType),
    Lifestyle.Singleton,
    c => true);
     */

    public class Log4netAdapterT<T> : ILogger<T>
    {
        //private static readonly log4net.ILog logger = LogManager.GetLogger(typeof(T));
        private readonly ILog _log  = LogManager.GetLogger(typeof(T));
        public IDisposable BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                    return _log.IsFatalEnabled;
                case LogLevel.Debug:
                case LogLevel.Trace:
                    return _log.IsDebugEnabled;
                case LogLevel.Error:
                    return _log.IsErrorEnabled;
                case LogLevel.Information:
                    return _log.IsInfoEnabled;
                case LogLevel.Warning:
                    return _log.IsWarnEnabled;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel));
            }
        }

        //public void Log(LogEntry entry)
        //{
        //    if (entry.LoggingEventType == LoggingEventType.Information)
        //        logger.Info(entry.Message, entry.Exception);
        //    else if (entry.LoggingEventType == LoggingEventType.Warning)
        //        logger.Warn(entry.Message, entry.Exception);
        //    else if (entry.LoggingEventType == LoggingEventType.Error)
        //        logger.Error(entry.Message, entry.Exception);
        //    else
        //        logger.Fatal(entry.Message, entry.Exception);
        //}



        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,System.Exception exception, Func<TState, System.Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            string message = null;
            if (null != formatter)
            {
                message = formatter(state, exception);
            }

            if (!string.IsNullOrEmpty(message) || exception != null)
            {
                switch (logLevel)
                {
                    case LogLevel.Critical:
                        _log.Fatal(message);
                        break;
                    case LogLevel.Debug:
                    case LogLevel.Trace:
                        _log.Debug(message);
                        break;
                    case LogLevel.Error:
                        _log.Error(message);
                        break;
                    case LogLevel.Information:
                        _log.Info(message);
                        break;
                    case LogLevel.Warning:
                        _log.Warn(message);
                        break;
                    default:
                        _log.Warn($"Encountered unknown log level {logLevel}, writing out as Info.");
                        _log.Info(message, exception);
                        break;
                }
            }
        }

    }
}
