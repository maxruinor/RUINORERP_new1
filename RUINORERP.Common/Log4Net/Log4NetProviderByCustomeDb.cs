using System.Collections.Concurrent;
using System.IO;
using System.Xml;
using Microsoft.Extensions.Logging;
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
            return log4netConfig["log4net"];
        }
    }
}

