using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.IIS
{
    public interface ILoggerService
    {
        void LogInformation(string message);
        void LogError(string message, Exception exception);
        // 可以添加更多的日志级别方法
    }

}
