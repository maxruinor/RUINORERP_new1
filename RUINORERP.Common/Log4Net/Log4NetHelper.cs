using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace RUINORERP.Common.Log4Net
{
    public static class Log4NetHelper
    {
        private static readonly object _configInitLock = new object();

        /// <summary>
        /// log4net init
        /// use the default log4net.config config file 这里是错的。
        /// </summary>
        /// <returns>1 config success,0 config has existed</returns>
        public static int LogInit() => LogInit(AppDomain.CurrentDomain.FriendlyName.Replace("","log4net.config"));

        /// <summary>
        /// log4net init
        /// </summary>
        /// <param name="configFilePath">log4net config file path</param>
        /// <returns>1 config success,0 config has existed</returns>
        public static int LogInit(string configFilePath)
        {
            if (null == LogManager.GetAllRepositories()?.FirstOrDefault(_ => _.Name == AppDomain.CurrentDomain.FriendlyName))
            {
                lock (_configInitLock)
                {
                    if (null == LogManager.GetAllRepositories()
                            ?.FirstOrDefault(_ => _.Name == AppDomain.CurrentDomain.FriendlyName))
                    {
                        XmlConfigurator.ConfigureAndWatch(LogManager.CreateRepository(AppDomain.CurrentDomain.FriendlyName), new System.IO.FileInfo(configFilePath));
                        return 1;
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// Get a log4net logger
        /// </summary>
        public static ILog GetLogger<TCategory>()
        {
            return LogManager.GetLogger(AppDomain.CurrentDomain.FriendlyName, typeof(TCategory));
        }

        /// <summary>
        /// Get a log4net logger
        /// </summary>
        public static ILog GetLogger(Type type)
        {
            return LogManager.GetLogger(AppDomain.CurrentDomain.FriendlyName, type);
        }

        /// <summary>
        /// Get a log4net logger
        /// </summary>
        public static ILog GetLogger(string loggerName)
        {
            return LogManager.GetLogger(AppDomain.CurrentDomain.FriendlyName, loggerName);
        }
    }
}
