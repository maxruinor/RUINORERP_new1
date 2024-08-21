using System;

namespace HLH.Lib.Helper
{

    /// <summary>
    /// ��־��¼������ ʹ�÷���������log4net.dll 2.0�汾  2020-9-13
    /// ����HLH.LIb.dll ����Ҫ��������log4.net.dll�ˡ�����Ҫ��������
    /// </summary>
    public class log4netHelper
    {
        //private static string loggerName = ConfigHelper.getWebConfigAttribute("log4netLogger");
        private static string loggerName = "Watson";

        /// <summary>
        /// ��¼������Ϣ
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
        /// ��¼������Ϣ
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
        /// ��¼������Ϣ,���쳣��Ϣ
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
        /// ��¼������Ϣ,���쳣��Ϣ
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
        /// ��¼��������,���쳣��Ϣ
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
        /// ��¼һ����Ϣ
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
        /// ��¼������Ϣ
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

