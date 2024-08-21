using log4net;
using log4net.Appender;
using log4net.Layout;
using Microsoft.Extensions.Logging;
using RUINORERP.Common.Log4Net;
using RUINORERP.UI.Log;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RUINORERP.UI
{
    public class JimMessage
    {
        public JimMessage()
        {
            IP = "127.0.0.1";// Util.getLocalIP();
            Mac = "abc";// Util.getLocalMac();
            Url = "666";// System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
        }
        public string IP { get; set; }
        public string Mac { get; set; }

        public string Url { get; set; }

        public string ModName { get; set; }

        public string ActionName { get; set; }

        public string Message { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }
    }
    public class Log4netExt
    {
        public Log4netExt()
        {

        }

        public static ILogger CreateLogerByCustomeDb()
        {
            log4net.Repository.ILoggerRepository rep = log4net.LogManager.CreateRepository(Guid.NewGuid().ToString());
            AdoNetAppender adoNetAppender = new AdoNetAppender();
            adoNetAppender.BufferSize = -1;
            adoNetAppender.ConnectionType = "System.Data.SqlClient.SqlConnection, System.Data, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            adoNetAppender.ConnectionString = "data source=.;initial catalog=test;persist security info=True;user id=sa;password=Jiehui@2016;MultipleActiveResultSets=True;";
            adoNetAppender.CommandText = "INSERT INTO Logs ([Date],[Level],[Logger],[Message],[Exception],[UserName],[ModName],[Mac],[IP],[Url],[ActionName],[Name]) VALUES (@log_date, @log_level, @logger, @Message, @exception,@UserName,@ModName,@Mac,@IP,@Url,@ActionName,@Name)";
            adoNetAppender.AddParameter(
                        new AdoNetAppenderParameter
                        {
                            ParameterName = "@log_date",
                            DbType = System.Data.DbType.DateTime,
                            Layout = new log4net.Layout.RawTimeStampLayout()
                        });
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@log_level", DbType = System.Data.DbType.String, Size = 50, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%level")) });
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@logger", DbType = System.Data.DbType.String, Size = 255, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%logger")) });
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@exception", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(new ExceptionLayout()) });

            log4net.Layout.PatternLayout layout = new MyLayout() { ConversionPattern = "%property{UserName}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@UserName", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new MyLayout() { ConversionPattern = "%property{ModName}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@ModName", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new MyLayout() { ConversionPattern = "%property{Mac}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Mac", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new MyLayout() { ConversionPattern = "%property{IP}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@IP", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new MyLayout() { ConversionPattern = "%property{Url}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Url", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new MyLayout() { ConversionPattern = "%property{ActionName}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@ActionName", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new MyLayout() { ConversionPattern = "%property{Name}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Name", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new MyLayout() { ConversionPattern = "%property{Message}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Message", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(layout) });

            adoNetAppender.ActivateOptions();

            log4net.Config.BasicConfigurator.Configure(rep, adoNetAppender);
          
            var loggerFactory = (ILoggerFactory)new LoggerFactory();
            loggerFactory.AddProvider(new Log4NetProviderByDb("Log4net_db.config"));

            //loggerFactory.AddProvider(new Log4NetProvider("log4net.config"));
            var logger = loggerFactory.CreateLogger("555");

           // ILog log = LogManager.GetLogger(rep.Name, "NoBufferingTest");
            return logger;

           
        }
    }

    public class Log4NetProviderByDb : ILoggerProvider
    {
        private readonly string _log4NetConfigFile;

        private readonly ConcurrentDictionary<string, Log4NetLogger> _loggers =
            new ConcurrentDictionary<string, Log4NetLogger>();

        public Log4NetProviderByDb(string log4NetConfigFile)
        {
            _log4NetConfigFile = log4NetConfigFile;
        }

        //public ILogger CreateLogger(string categoryName)
        //{
        //    return _loggers.GetOrAdd(categoryName, CreateLoggerImplementation);
        //}

        public ILogger CreateLogger(string categoryName)
        {
            return Log4netExt.CreateLogerByCustomeDb();
        }

        public void Dispose()
        {
            _loggers.Clear();
        }

        private Log4NetLogger CreateLoggerImplementation(string name)
        {
            return new Log4NetLogger(name, Parselog4NetConfigFile(_log4NetConfigFile));
        }

        private static XmlElement Parselog4NetConfigFile(string filename)
        {
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead(filename));
            return log4netConfig["log4net"];
        }
    }


     

}
