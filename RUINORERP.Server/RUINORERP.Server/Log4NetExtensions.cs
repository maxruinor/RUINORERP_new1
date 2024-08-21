using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace RUINORERP.Server
{
    /// <summary>
    ///.net core3.1使用 log4net  https://www.cnblogs.com/shy1766IT/p/16917227.html
    /// </summary>
    public static class Log4NetExtensions
    {
        /// <summary>
        /// 这里只能用到socket,日志这个东西 比方在工作注或其它都是线程级。感觉是的。混用一个实例会死锁。可以查询调试中的控制台信息
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <returns></returns>
        public static IHostBuilder UseLog4Net(this IHostBuilder hostBuilder)
        {
           //没有起到作用？
            //只是初始化而已
            ILoggerRepository repository = LogManager.CreateRepository("SuperSocket");
            var log4netRepository = LogManager.GetRepository("SuperSocket");
            XmlConfigurator.Configure(log4netRepository, new FileInfo("ConfigBySS/log4net.config"));
            return hostBuilder;
        }

        public static IHostBuilder ConfigureLogging(this IHostBuilder hostBuilder, ILoggerFactory loggerFactory)
        {
            //depending on what you use for logging 
            var log4netRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(log4netRepository, new FileInfo("log4net.config"));
            return hostBuilder;
        }
    }
}
