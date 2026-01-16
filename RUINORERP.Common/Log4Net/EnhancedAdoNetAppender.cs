using System;
using log4net.Appender;
using log4net.Core;
using System.Data;

namespace RUINORERP.Common.Log4Net
{
    /// <summary>
    /// 增强版 AdoNetAppender,连接字符串已在 Log4NetConfiguration 中解密并替换
    /// </summary>
    public class EnhancedAdoNetAppender : AdoNetAppender
    {
        /// <summary>
        /// 激活 Appender 配置
        /// </summary>
        public override void ActivateOptions()
        {
            try
            {
          
                // 验证连接字符串是否有效（已被替换）
                if (string.IsNullOrEmpty(ConnectionString))
                {
                    throw new InvalidOperationException("连接字符串为空，请检查 Log4NetConfiguration 初始化");
                }

            
            }
            catch (Exception ex)
            {
                throw;
            }

            base.ActivateOptions();
        }

        /// <summary>
        /// 重写 Append 方法，添加详细日志和错误处理
        /// </summary>
        protected override void Append(LoggingEvent loggingEvent)
        {
            try
            {
                base.Append(loggingEvent);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 重写 SendBuffer 方法，捕获批量写入错误
        /// </summary>
        protected override void SendBuffer(IDbTransaction dbTran, LoggingEvent[] events)
        {
            try
            {
                base.SendBuffer(dbTran, events);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"内部异常: {ex.InnerException.Message}");
                }
                throw;
            }
        }

    }
}
