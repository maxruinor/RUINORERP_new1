using System;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Logging;


namespace RUINORERP.Common.Log4Net
{
    // 泛型 logger 配置
    public sealed class GenericLoggerOptions
    {
        // 返回 true 则使用带泛型参数的 typeName，否则使用默认的行为
        public Func<Type, bool>? FullNamePredict { get; set; }
    }

    public class GenericLogger<T> : ILogger<T>
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Creates a new <see cref="GenericLogger{T}"/>.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="options">GenericLoggerOptions</param>
        public GenericLogger(Microsoft.Extensions.Logging.ILoggerFactory factory, Microsoft.Extensions.Options.IOptions<GenericLoggerOptions> options)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            // 通过配置的委托来判断是否要包含泛型参数
            var includeGenericParameters = options.Value.FullNamePredict?.Invoke(typeof(T)) == true;
            
                    _logger = factory.CreateLogger(Helper.TypeHelper.GetTypeDisplayName(typeof(T), includeGenericParameters: includeGenericParameters, nestedTypeDelimiter: '.'));
        }

        /// <inheritdoc />
        System.IDisposable ILogger.BeginScope<TState>(TState state)
        {
            return _logger.BeginScope(state);
        }

        /// <inheritdoc />
        bool ILogger.IsEnabled(LogLevel logLevel)
        {
            return _logger.IsEnabled(logLevel);
        }

        /// <inheritdoc />
        void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            _logger.Log(logLevel, eventId, state, exception, formatter);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) => throw new NotImplementedException();

  
            public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}
