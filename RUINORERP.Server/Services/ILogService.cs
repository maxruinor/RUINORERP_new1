using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.Services
{
    /// <summary>
    /// 日志服务接口
    /// </summary>
    public interface ILogService
    {
        /// <summary>
        /// 打印信息日志
        /// </summary>
        /// <param name="message">日志消息</param>
        void PrintInfoLog(string message);

        /// <summary>
        /// 打印错误日志
        /// </summary>
        /// <param name="message">错误消息</param>
        void PrintErrorLog(string message);
    }
}