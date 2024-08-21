using System;

namespace HLH.Lib.Helper
{

    /// <summary>
    /// 日志记录公共类 使用方法：引用log4net.dll 2.0版本  2020-9-13
    /// 引用HLH.LIb.dll 不需要单独引用log4.net.dll了。不需要其他配置
    /// </summary>
    public class log4netHelper
    {
        //private static string loggerName = ConfigHelper.getWebConfigAttribute("log4netLogger");
        private static string loggerName = "Watson";

        /// <summary>
        /// 记录调试信息
        /// </summary>
        /// <param name="message"></param>
        public static void debug(string message)
        {

            log4net.ILog log = log4net.LogManager.GetLogger(loggerName);
            if (log.IsDebugEnabled)
            {
                log.Debug(message);
            }
            log = null;
        }

        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="message"></param>
        public static void error(string message)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(loggerName);
            if (log.IsErrorEnabled)
            {
                log.Error(message);
            }
            log = null;
        }

        /// <summary>
        /// 记录错误信息,及异常信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void error(string message, Exception ex)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(loggerName);
            if (log.IsErrorEnabled)
            {
                if (ex != null)
                {
                    if (ex.InnerException != null)
                    {
                        log.Error(message, ex.InnerException);
                    }
                    else
                    {
                        log.Error(message, ex);
                    }
                }
            }
            log = null;
        }


        /// <summary>
        /// 记录错误信息,及异常信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void error(Exception ex)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(loggerName);
            if (log.IsErrorEnabled)
            {
                if (ex != null)
                {
                    if (ex.InnerException != null)
                    {
                        log.Error(ex.Message, ex.InnerException);
                    }
                    else
                    {
                        log.Error(ex.Message, ex);
                    }
                }
            }
            log = null;
        }

        /// <summary>
        /// 记录致命错误,及异常信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void fatal(string message, Exception ex)
        {

            log4net.ILog log = log4net.LogManager.GetLogger(loggerName);
            if (log.IsFatalEnabled)
            {
                if (ex != null)
                {
                    if (ex.InnerException != null)
                    {
                        log.Fatal(message, ex.InnerException);
                    }
                    else
                    {
                        log.Fatal(message, ex);
                    }
                }
            }
            log = null;
        }

        /// <summary>
        /// 记录一般信息
        /// </summary>
        /// <param name="message"></param>
        public static void info(string message)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(loggerName);
            if (log.IsInfoEnabled)
            {
                log.Info(message);
            }
            log = null;
        }

        /// <summary>
        /// 记录警告信息
        /// </summary>
        /// <param name="message"></param>
        public static void warn(string message)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(loggerName);
            if (log.IsWarnEnabled)
            {
                log.Warn(message);
            }
            log = null;
        }
    }
}

