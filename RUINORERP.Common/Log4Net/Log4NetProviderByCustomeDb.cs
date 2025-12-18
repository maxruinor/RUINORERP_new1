using System;
using System.Collections.Concurrent;
using System.IO;
using System.Xml;
using Microsoft.Extensions.Logging;
using RUINORERP.Common.Helper;
using RUINORERP.Model.Context;

namespace RUINORERP.Common.Log4Net
{
    public class Log4NetProviderByCustomeDb : ILoggerProvider
    {
        private readonly string _log4NetConfigFile;

        private readonly ConcurrentDictionary<string, Log4NetLoggerByDb> _loggers =
            new ConcurrentDictionary<string, Log4NetLoggerByDb>();

        private string ConnectionStr;

        private ApplicationContext _appcontext;

        public Log4NetProviderByCustomeDb(string log4NetConfigFile, string _ConnectionStr ,ApplicationContext appcontext)
        {
            _log4NetConfigFile = log4NetConfigFile;
            _appcontext = appcontext;
            ConnectionStr = _ConnectionStr;
        }



        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, CreateLoggerImplementation);
        }

        public void Dispose()
        {
            foreach (var logger in _loggers.Values)
            {
                if (logger is IDisposable disposableLogger)
                {
                    disposableLogger.Dispose();
                }
            }
            _loggers.Clear();
        }

        private Log4NetLoggerByDb CreateLoggerImplementation(string name)
        {
            // 将连接字符串传递给Parselog4NetConfigFile方法，避免内部再次调用CryptoHelper.GetDecryptedConnectionString()
            return new Log4NetLoggerByDb(name, Parselog4NetConfigFile(_log4NetConfigFile, ConnectionStr), ConnectionStr, _appcontext);
        }

        private static XmlElement Parselog4NetConfigFile(string filename, string connectionString)
        {
            XmlDocument log4netConfig = new XmlDocument();
            
            // 检查文件是否存在，如果不存在则使用默认配置
            if (File.Exists(filename))
            {
                log4netConfig.Load(File.OpenRead(filename));
            }
            else
            {
                // 如果指定的配置文件不存在，尝试加载默认的log4net.config
                string defaultConfigFile = "log4net.config";
                if (File.Exists(defaultConfigFile))
                {
                    log4netConfig.Load(File.OpenRead(defaultConfigFile));
                    System.Diagnostics.Debug.WriteLine($"警告：未找到配置文件 {filename}，使用默认配置文件 {defaultConfigFile}");
                }
                else
                {
                    // 如果都不存在，创建一个基本的配置
                    System.Diagnostics.Debug.WriteLine($"警告：未找到配置文件 {filename} 和 {defaultConfigFile}，使用基本配置");
                    XmlElement log4netElement = log4netConfig.CreateElement("log4net");
                    log4netConfig.AppendChild(log4netElement);
                }
            }
            
            // 查找并替换连接字符串占位符
            try
            {
                // 直接使用传入的连接字符串，避免再次调用CryptoHelper.GetDecryptedConnectionString()
                // 这样可以确保连接字符串已经被正确解密并且配置文件已经完全加载
                if (!string.IsNullOrEmpty(connectionString))
                {
                    XmlNodeList nodes = log4netConfig.SelectNodes("//connectionString[@value]");
                    foreach (XmlNode node in nodes)
                    {
                        XmlAttribute valueAttr = node.Attributes["value"];
                        if (valueAttr != null)
                        {
                            // 检查是否包含占位符或者直接替换加密的连接字符串
                            if (valueAttr.Value.Contains("${ConnectionString}"))
                            {
                                valueAttr.Value = connectionString;
                                System.Diagnostics.Debug.WriteLine($"已在{filename}中替换连接字符串占位符");
                            }
                            else
                            {
                                // 如果是加密的连接字符串格式，也进行替换
                                //System.Diagnostics.Debug.WriteLine($"已在{filename}中使用传入的解密后连接字符串替换原始连接字符串");
                                valueAttr.Value = connectionString;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"警告：替换{filename}中的连接字符串占位符失败: " + ex.Message);
                System.Diagnostics.Debug.WriteLine($"异常详情: {ex.StackTrace}");
            }
            
            return log4netConfig["log4net"];
        }
    }
}
