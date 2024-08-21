using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using log4net.Repository;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Logging;
using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
using RUINORERP.Model.Context;

namespace RUINORERP.Common.Log4Net
{

    /// <summary>
    /// 2023-11-22 添加了数据库和自定义字段,重写了自定义字段写入方法
    /// 生成仓库时。应该也可以用配置文件。只是字符串明码了。也可以字串重写。其他自定义应该是可以的 CustomAdoNetAppender
    /// </summary>
    public class Log4NetLoggerByDb : ILogger
    {
        private readonly string _name;
        private readonly XmlElement _xmlElement;
        private readonly ILog _log;
        private ILoggerRepository _loggerRepository;
        private ApplicationContext _appcontext;
        public Log4NetLoggerByDb(string name, XmlElement xmlElement, string ConnectionStr, ApplicationContext appcontext)
        {
            _appcontext = appcontext;
            _name = name;
            _xmlElement = xmlElement;
            // _loggerRepository = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(Hierarchy));
            // _log = CreateLogerByCustomeDb();
            _loggerRepository = CreateLoggerRepository(ConnectionStr);
            _log = LogManager.GetLogger(_loggerRepository.Name, name);
            // XmlConfigurator.Configure(_loggerRepository, xmlElement);//这句如果打开会覆盖 CreateLoggerRepository 这个方法里面的配置
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

        /// <summary>
        /// 两种方式，一种是外面提供全部信息。解析。再一次种是外部不变。里面补充行为信息
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel"></param>
        /// <param name="eventId"></param>
        /// <param name="state"></param>
        /// <param name="exception"></param>
        /// <param name="formatter"></param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            System.Exception exception, Func<TState, System.Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }
            Logs log = new Logs();
            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }
            string message = null;
            if (null != formatter)
            {
                try
                {
                    message = formatter(state, exception);
                }
                catch (Exception)
                { 
                   
                }

            }
            log = _appcontext.log;//公共部分一次给过去
            try
            {
                #region 如果这里异常可能会卡死所有db,因为使用了DB存日志 所以加上try
                log.Message = message;
                if (exception == null)
                {
                    log.Exception = null;
                }
                if (log.Exception == null && exception != null)
                {
                    log.Exception = exception.StackTrace;
                }

                //通过反射取出异常值
                FieldInfo[] fieldsInfo = state.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                FieldInfo props = fieldsInfo.FirstOrDefault(o => o.Name == "_values");
                object[] values = (object[])props?.GetValue(state);
                foreach (var item in values)
                {
                    if (item is Exception ex)
                    {
                        if (log.Message.Trim().Length > 0)
                        {
                            log.Message += "\r\n";
                        }
                        log.Message += ex.Message + "\r\n";
                        log.Exception += ex.StackTrace + "\r\n";
                        if (ex.InnerException != null)
                        {
                            log.Message += ex.InnerException.Message + "\r\n";
                            log.Exception += ex.InnerException.StackTrace + "\r\n";
                        }
                    }
                }
                if (log.Message != null)
                {
                    log.Message = log.Message.Trim();
                }
                if (log.Exception != null)
                {
                    log.Exception = log.Exception.Trim();
                }

                #endregion
            }
            catch (Exception exx)
            {

            }


            if (!string.IsNullOrEmpty(message) || exception != null)
            {
                switch (logLevel)
                {
                    case LogLevel.Critical:
                        //_log.Fatal(message);
                        _log.Fatal(log);
                        break;
                    case LogLevel.Debug:
                    case LogLevel.Trace:
                        // _log.Debug(message);
                        _log.Debug(log);
                        break;
                    case LogLevel.Error:
                        // _log.Error(message);
                        _log.Error(log);
                        break;
                    case LogLevel.Information:
                        //_log.Info(message);
                        _log.Info(log);
                        break;
                    case LogLevel.Warning:
                        //_log.Warn(message);
                        _log.Warn(log);
                        break;
                    default:
                        _log.Warn($"Encountered unknown log level {logLevel}, writing out as Info.");
                        _log.Info(message, exception);
                        break;
                }
            }
        }

        public ILoggerRepository CreateLoggerRepository(string _ConnectionString)
        {
            log4net.Repository.ILoggerRepository rep = log4net.LogManager.CreateRepository(Guid.NewGuid().ToString());
            AdoNetAppender adoNetAppender = new AdoNetAppender();
            adoNetAppender.BufferSize = -1;
            adoNetAppender.ConnectionType = "System.Data.SqlClient.SqlConnection, System.Data, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            if (string.IsNullOrEmpty(_ConnectionString))
            {
                adoNetAppender.ConnectionString = "data source=192.168.0.254;initial catalog=erpnew;persist security info=True;user id=sa;password=SA!@#123sa;MultipleActiveResultSets=True;";
            }
            else
            {
                adoNetAppender.ConnectionString = _ConnectionString;
            }

            adoNetAppender.CommandText = "INSERT INTO Logs ([User_ID],[Date],[Level],[Logger],[Message],[Exception],[Operator],[ModName],[MAC],[IP],[Path],[ActionName],[MachineName]) VALUES (@User_ID,@log_date, @log_level, @logger, @Message, @Exception,@Operator,@ModName,@MAC,@IP,@Path,@ActionName,@MachineName)";
            adoNetAppender.AddParameter(
                        new AdoNetAppenderParameter
                        {
                            ParameterName = "@log_date",
                            DbType = System.Data.DbType.DateTime,
                            Layout = new log4net.Layout.RawTimeStampLayout()
                        });
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@log_level", DbType = System.Data.DbType.String, Size = 50, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%level")) });
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@logger", DbType = System.Data.DbType.String, Size = 255, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%logger")) });
            // adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Exception", DbType = System.Data.DbType.String, Size = 214748364, Layout = new Layout2RawLayoutAdapter(new ExceptionLayout()) });
            // adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Exception", DbType = System.Data.DbType.String, Size = 214748364, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%Exception")) });

            log4net.Layout.PatternLayout layout = new CustomLayout() { ConversionPattern = "%property{Operator}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Operator", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new CustomLayout() { ConversionPattern = "%property{User_ID}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@User_ID", DbType = System.Data.DbType.Int64, Size = 40, Layout = new Layout2RawLayoutAdapter(layout) });



            layout = new CustomLayout() { ConversionPattern = "%property{ModName}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@ModName", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new CustomLayout() { ConversionPattern = "%property{MAC}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@MAC", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new CustomLayout() { ConversionPattern = "%property{IP}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@IP", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new CustomLayout() { ConversionPattern = "%property{Path}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Path", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new CustomLayout() { ConversionPattern = "%property{ActionName}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@ActionName", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new CustomLayout() { ConversionPattern = "%property{MachineName}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@MachineName", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new CustomLayout() { ConversionPattern = "%property{Message}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Message", DbType = System.Data.DbType.String, Size = 214748364, Layout = new Layout2RawLayoutAdapter(layout) });
            adoNetAppender.ActivateOptions();

            layout = new CustomLayout() { ConversionPattern = "%property{Exception}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Exception", DbType = System.Data.DbType.String, Size = 214748364, Layout = new Layout2RawLayoutAdapter(layout) });
            adoNetAppender.ActivateOptions();

            log4net.Config.BasicConfigurator.Configure(rep, adoNetAppender);
            return rep;
        }

        public static ILog CreateLogerByCustomeDb(ILoggerRepository loggerRepository)
        {

            ILog log = LogManager.GetLogger(loggerRepository.Name, "erp_log");

            //CustomLogContent message = new CustomLogContent();
            //message.Message = "审核成功111";
            //message.UserName = "金朝钱222";
            ////  message.Url = Request.Url.ToString();
            //// message.ModName = "其他入库";
            //// message.ActionName = "审核";
            //// message.Name = "Bill20150505";
            //log.Info(message);
            //// logger.Debug(message);

            return log;


        }



    }
}

