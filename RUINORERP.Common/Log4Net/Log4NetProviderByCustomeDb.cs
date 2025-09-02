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
            _loggers.Clear();
        }

        private Log4NetLoggerByDb CreateLoggerImplementation(string name)
        {
            return new Log4NetLoggerByDb(name, Parselog4NetConfigFile(_log4NetConfigFile), ConnectionStr, _appcontext);
        }

        private static XmlElement Parselog4NetConfigFile(string filename)
        {
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead(filename));
            
            // 查找并替换连接字符串占位符
            try
            {
                var connectionString = CryptoHelper.GetDecryptedConnectionString();
                XmlNodeList nodes = log4netConfig.SelectNodes("//connectionString[@value]");
                foreach (XmlNode node in nodes)
                {
                    XmlAttribute valueAttr = node.Attributes["value"];
                    if (valueAttr != null && valueAttr.Value.Contains("${ConnectionString}"))
                    {
                        valueAttr.Value = connectionString;
                        Console.WriteLine("已在log4net.config中替换连接字符串占位符");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("警告：替换log4net.config中的连接字符串占位符失败: " + ex.Message);
            }
            
            return log4netConfig["log4net"];
        }
    }
}

