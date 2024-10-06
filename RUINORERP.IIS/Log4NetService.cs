using log4net;
using RUINORERP.IIS.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.IIS
{
    public class Log4NetService : ILoggerService
    {

        public Log4NetService()
        {
              
        }

        private readonly ILog _log = LogManager.GetLogger(typeof(Log4NetService));

        public void LogInformation(string message)
        {
            LogHelper.Info(message);
        }

        public void LogError(string message, Exception exception)
        {
            LogHelper.Error(message, exception);
        }
        // 实现其他日志级别方法
    }
}
