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
        /// 重写 Append 方法,添加详细日志和错误处理
        /// </summary>
        protected override void Append(LoggingEvent loggingEvent)
        {
            try
            {
                base.Append(loggingEvent);
            }
            catch (Exception ex)
            {
                // 记录详细的错误信息到调试输出
                System.Diagnostics.Debug.WriteLine($"[Log4Net] 写入日志失败:");
                System.Diagnostics.Debug.WriteLine($"  - 消息: {loggingEvent.RenderedMessage}");
                System.Diagnostics.Debug.WriteLine($"  - 级别: {loggingEvent.Level}");
                System.Diagnostics.Debug.WriteLine($"  - 记录器: {loggingEvent.LoggerName}");
                System.Diagnostics.Debug.WriteLine($"  - 错误: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"  - 内部异常: {ex.InnerException.Message}");
                }
                System.Diagnostics.Debug.WriteLine($"  - 堆栈: {ex.StackTrace}");
                        
                // 不抛出异常,避免影响主程序
            }
        }

        /// <summary>
        /// 重写 SendBuffer 方法,捕获批量写入错误
        /// </summary>
        protected override void SendBuffer(IDbTransaction dbTran, LoggingEvent[] events)
        {
            try
            {
                base.SendBuffer(dbTran, events);
            }
            catch (Exception ex)
            {
                // 记录详细的错误信息
                System.Diagnostics.Debug.WriteLine($"[Log4Net] 批量写入日志失败,共 {events?.Length ?? 0} 条:");
                System.Diagnostics.Debug.WriteLine($"  - 错误: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"  - 内部异常: {ex.InnerException.Message}");
                    System.Diagnostics.Debug.WriteLine($"  - 内部异常堆栈: {ex.InnerException.StackTrace}");
                }
                System.Diagnostics.Debug.WriteLine($"  - 堆栈: {ex.StackTrace}");
                        
                // 逐条记录失败的日志内容(仅前5条,避免过多输出)
                if (events != null)
                {
                    int count = Math.Min(events.Length, 5);
                    for (int i = 0; i < count; i++)
                    {
                        System.Diagnostics.Debug.WriteLine($"    [{i + 1}] 消息: {events[i].RenderedMessage}");
                    }
                    if (events.Length > 5)
                    {
                        System.Diagnostics.Debug.WriteLine($"    ... 还有 {events.Length - 5} 条");
                    }
                }
                        
                // 不抛出异常,避免影响主程序
            }
        }

    }
}
