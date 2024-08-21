using System;
using System.IO;
using System.Reflection;
using System.Resources;
namespace HLH.Lib.Helper
{
    /// <summary>
    /// 自定义日志记录类
    /// <para>与日志类冲突，只最后一个生效，小心使用</para>
    /// </summary>
    public class log4netCustomer
    {

        //思路，动态写入配置文件

        //private static string loggerName = ConfigHelper.getWebConfigAttribute("log4netLogger");



        private static string _loggerName = string.Empty;


        private static readonly object mylock = new object();


        public static string loggerName
        {
            get
            {
                if (!string.IsNullOrEmpty(_loggerName))
                {
                    return _loggerName;
                }
                else
                {
                    throw new Exception("请先定义日志标签");
                };
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    //如果属性值有变化。换一个名字则
                    if (!_loggerName.Equals(value))
                    {
                        _loggerName = value;
                        CreatConfig(value);
                    }
                }
                else
                {
                    throw new Exception("日志标签不能为空");
                }
                ;
            }
        }



        /// <summary>
        /// 动态生成配置文件，并加载监视
        /// </summary>
        /// <param name="ploggerName"></param>
        private static void CreatConfig(string ploggerName)
        {

            #region 动态生成配置文件，并加载监视
            lock (mylock)
            {
                Assembly ab = Assembly.GetExecutingAssembly();
                string logconfigPath = System.IO.Path.Combine(ab.Location.Replace(ab.ManifestModule.Name, ""), ploggerName + ".xml");
                if (!System.IO.File.Exists(logconfigPath))
                {
                    //创建
                    ResourceManager rm = new ResourceManager("HLH.Lib.Properties.resources", ab);

                    System.IO.FileStream f = System.IO.File.Create(logconfigPath);
                    f.Close();

                    System.IO.StreamWriter f2 = new System.IO.StreamWriter(logconfigPath, false, System.Text.Encoding.UTF8);
                    string temp = rm.GetString("log4net.config");
                    temp = temp.Replace("#Watson#", ploggerName);
                    f2.Write(temp);
                    f2.Close();
                    f2.Dispose();
                }

                //程序启动时就需要并加载监视
                FileInfo finfo = new FileInfo(logconfigPath);
                log4net.Config.XmlConfigurator.Configure(finfo);
                log4net.Config.XmlConfigurator.ConfigureAndWatch(finfo);
            }
            #endregion

        }










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

