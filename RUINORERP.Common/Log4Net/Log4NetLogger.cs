using System;
using System.Reflection;
using System.Xml;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using log4net.Repository;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Logging;
using RUINORERP.Common.Helper;

namespace RUINORERP.Common.Log4Net
{

    public class Log4NetLogger : ILogger
    {
        private readonly string _name;
        private readonly XmlElement _xmlElement;
        private readonly ILog _log;
        private ILoggerRepository _loggerRepository;

        public Log4NetLogger(string name, XmlElement xmlElement)
        {
            _name = name;
            _xmlElement = xmlElement;
            _loggerRepository = LogManager.CreateRepository(
                Assembly.GetEntryAssembly(), typeof(Hierarchy));
            _log = LogManager.GetLogger(_loggerRepository.Name, name);
            
            // 处理加密的连接字符串
            UpdateConnectionStringInXmlConfig(xmlElement);
            XmlConfigurator.Configure(_loggerRepository, xmlElement);
        }

        public IDisposable BeginScope<TState>(TState state)
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

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            System.Exception exception, Func<TState, System.Exception, string> formatter)
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
                        if (exception!=null)
                        {
                            _log.Debug(message);
                        }
                
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

        /// <summary>
        /// 更新XML配置中的连接字符串，处理加密的连接字符串
        /// </summary>
        /// <param name="xmlElement">XML配置元素</param>
        private void UpdateConnectionStringInXmlConfig(XmlElement xmlElement)
        {
            try
            {
                // 获取解密后的连接字符串
                string decryptedConnectionString = CryptoHelper.GetDecryptedConnectionStringOrDefault("LogDatabase");
                
                if (!string.IsNullOrEmpty(decryptedConnectionString))
                {
                    // 查找所有connectionString属性并更新
                    XmlNodeList connectionStringNodes = xmlElement.SelectNodes("//*[@connectionString]");
                    foreach (XmlNode node in connectionStringNodes)
                    {
                        XmlAttribute connectionStringAttr = node.Attributes["connectionString"];
                        if (connectionStringAttr != null)
                        {
                            connectionStringAttr.Value = decryptedConnectionString;
                        }
                    }
                    
                    // 查找所有ConnectionString属性并更新
                    XmlNodeList ConnectionStringNodes = xmlElement.SelectNodes("//*[@ConnectionString]");
                    foreach (XmlNode node in ConnectionStringNodes)
                    {
                        XmlAttribute ConnectionStringAttr = node.Attributes["ConnectionString"];
                        if (ConnectionStringAttr != null)
                        {
                            ConnectionStringAttr.Value = decryptedConnectionString;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"更新连接字符串配置失败: {ex.Message}");
                // 继续使用原始配置，不抛出异常
            }
        }
    }




}