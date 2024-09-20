using log4net;
using log4net.Config;

namespace RUINORERP.WebServerConsole.Comm
{
    public class LogHelper
    {
        private readonly static ILog Logger;

        static LogHelper()
        {
            if (Logger == null)
            {
                var LogRepository = LogManager.CreateRepository("YT");
                XmlConfigurator.Configure(LogRepository, new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));
                Logger = LogManager.GetLogger(LogRepository.Name, "logError");
            }
        }

        /// <summary>
        /// 普通日志
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            Task.Run(() => Logger.Info(message));
        }

        /// <summary>
        /// 告警日志
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(string message)
        {
            Task.Run(() => Logger.Warn(message));
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Error(string message, Exception ex)
        {
            if (ex == null)
                Task.Run(() => Logger.Error(message));
            else
                Task.Run(() => Logger.Error(message, ex));
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="ex"></param>
        public static void Error(Exception ex)
        {
            Task.Run(() => Logger.Error(ex));
        }

    }
}