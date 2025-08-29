using log4net.Appender;
using log4net.Core;
using RUINORERP.Common.Helper;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace RUINORERP.Common.Log4Net
{
    
    public class CustomADONetAppender : AdoNetAppender
    {
        protected override void SendBuffer(LoggingEvent[] events)
        {
            // 在发送日志前动态设置连接字符串
            if (string.IsNullOrEmpty(this.ConnectionString))
            {
               // this.ConnectionString = GetConnectionString();
                this.ConnectionString = CryptoHelper.GetDecryptedConnectionString();
            }

            base.SendBuffer(events);
        }

        private string GetConnectionString()
        {
            // 从应用程序配置文件中获取连接字符串
            // 假设连接字符串在app.config中的名字是"LogDatabase"
            var connectionString = ConfigurationManager.ConnectionStrings["LogDatabase"]?.ConnectionString;

            if (string.IsNullOrEmpty(connectionString))
            {
                // 如果配置文件中没有，尝试从应用程序设置中获取
                connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            }

            return connectionString;
        }
    }
}