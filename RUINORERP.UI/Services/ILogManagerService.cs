using System;
using System.Drawing;
using Microsoft.Extensions.Logging;
using RUINORERP.UI.Log;
using RUINORERP.UI.UserCenter;

namespace RUINORERP.UI.Services
{
    /// <summary>
    /// 日志管理服务接口
    /// </summary>
    public interface ILogManagerService
    {
        /// <summary>
        /// 打印信息日志（默认）
        /// </summary>
        /// <param name="msg">日志消息</param>
        void PrintInfoLog(string msg);

        /// <summary>
        /// 打印信息日志（带颜色）
        /// </summary>
        /// <param name="msg">日志消息</param>
        /// <param name="color">字体颜色</param>
        void PrintInfoLog(string msg, Color color);

        /// <summary>
        /// 打印信息日志（带异常）
        /// </summary>
        /// <param name="msg">日志消息</param>
        /// <param name="ex">异常对象</param>
        void PrintInfoLog(string msg, Exception ex);
    }
}
