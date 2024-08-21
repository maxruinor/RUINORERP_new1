using log4net.Config;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server
{

    //  首先创建一个仓储类
    public class Log4NetRepository
    {
        public static ILoggerRepository loggerRepository { get; set; }
    }

    /// <summary>
    /// 能不能按照info eooro分类建文件 是可以的，配置中实现的
    /// 应该比较完善? 下面先在Program中引用一下。识别配置文件 
    /// ILoggerRepository repository = LogManager.CreateRepository("kxrz");
    //XmlConfigurator.Configure(repository, new FileInfo("log4net.Config"));
    //      log4netHelper.loggerRepository = repository;

    /// 日志记录公共类 使用方法：引用log4net.dll 2.0版本  2020-9-13
    /// 引用HLH.LIb.dll 不需要单独引用log4.net.dll了。不需要其它配置
    /// </summary>
    public class log4netHelper
    {
        //public static ILoggerRepository loggerRepository { get; set; }
        //ILog log = LogManager.GetLogger(repository.Name, "kxlog");
        public static readonly log4net.ILog loggerError = log4net.LogManager.GetLogger(Log4NetRepository.loggerRepository.Name, "errorAppender");
        public static readonly log4net.ILog loggerInfo = log4net.LogManager.GetLogger(Log4NetRepository.loggerRepository.Name, "infoAppender");
        public static readonly log4net.ILog loggerDebug = log4net.LogManager.GetLogger(Log4NetRepository.loggerRepository.Name, "debugAppender");
        public static readonly log4net.ILog loggerPer = log4net.LogManager.GetLogger(Log4NetRepository.loggerRepository.Name, "perfAppender");

        /// <summary>
        /// 记录调试信息
        /// </summary>
        /// <param name="message"></param>
        public static void debug(string message)
        {
            
            if (loggerDebug.IsDebugEnabled)
            {
                loggerDebug.Debug(message);
            }
        }

        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="message"></param>
        public static void error(string message)
        {
            if (loggerError.IsErrorEnabled)
            {
                loggerError.Error(message);
            }
        }

        /// <summary>
        /// 记录错误信息,及异常信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void error(string message, Exception ex)
        {

            if (loggerError.IsErrorEnabled)
            {
                if (ex != null)
                {
                    if (ex.InnerException != null)
                    {
                        loggerError.Error(message, ex.InnerException);
                    }
                    else
                    {
                        loggerError.Error(message, ex);
                    }
                }
            }

        }


        /// <summary>
        /// 记录错误信息,及异常信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void error(Exception ex)
        {

            if (loggerError.IsErrorEnabled)
            {
                if (ex != null)
                {
                    if (ex.InnerException != null)
                    {
                        loggerError.Error(ex.Message, ex.InnerException);
                    }
                    else
                    {
                        loggerError.Error(ex.Message, ex);
                    }
                }
            }

        }

        /// <summary>
        /// 记录致命错误,及异常信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void fatal(string message, Exception ex)
        {


            if (loggerError.IsFatalEnabled)
            {
                if (ex != null)
                {
                    if (ex.InnerException != null)
                    {
                        loggerError.Fatal(message, ex.InnerException);
                    }
                    else
                    {
                        loggerError.Fatal(message, ex);
                    }
                }
            }

        }

        /// <summary>
        /// 记录一般信息
        /// </summary>
        /// <param name="message"></param>
        public static void info(string message)
        {

            if (loggerInfo.IsInfoEnabled)
            {
                loggerInfo.Info(message);
            }

        }

        /// <summary>
        /// 记录警告信息
        /// </summary>
        /// <param name="message"></param>
        public static void warn(string message)
        {
            if (loggerPer.IsWarnEnabled)
            {
                loggerPer.Warn(message);
            }

        }
    }
}
