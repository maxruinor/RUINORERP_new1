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
                System.Diagnostics.Debug.WriteLine("EnhancedAdoNetAppender 正在激活...");
                System.Diagnostics.Debug.WriteLine($"连接字符串长度: {ConnectionString?.Length ?? 0}");

                // 验证连接字符串是否有效（已被替换）
                if (string.IsNullOrEmpty(ConnectionString))
                {
                    throw new InvalidOperationException("连接字符串为空，请检查 Log4NetConfiguration 初始化");
                }

                if (ConnectionString.Contains("${ConnectionString}") || ConnectionString.Contains("Encrypted:"))
                {
                    System.Diagnostics.Debug.WriteLine("警告：连接字符串包含未替换的占位符或加密标识");
                }

                // 验证表结构
                if (!string.IsNullOrEmpty(CommandText))
                {
                    System.Diagnostics.Debug.WriteLine($"SQL命令: {CommandText}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"激活 AdoNetAppender 失败: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"堆栈: {ex.StackTrace}");
                throw;
            }

            // 先调用基类方法初始化
            base.ActivateOptions();

            
        }

        /// <summary>
        /// 重写 Append 方法，添加详细日志和错误处理
        /// </summary>
        protected override void Append(LoggingEvent loggingEvent)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"EnhancedAdoNetAppender 正在写入日志: {loggingEvent.Level} - {loggingEvent.RenderedMessage}");

                base.Append(loggingEvent);

                System.Diagnostics.Debug.WriteLine($"EnhancedAdoNetAppender 日志写入成功");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EnhancedAdoNetAppender 写入日志失败: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"日志内容: {loggingEvent.RenderedMessage}");
                System.Diagnostics.Debug.WriteLine($"堆栈: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"内部异常: {ex.InnerException.Message}");
                }
            }
        }

        /// <summary>
        /// 重写 SendBuffer 方法，捕获批量写入错误
        /// </summary>
        protected override void SendBuffer(IDbTransaction dbTran, LoggingEvent[] events)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"EnhancedAdoNetAppender 批量写入 {events.Length} 条日志...");
                base.SendBuffer(dbTran, events);
                System.Diagnostics.Debug.WriteLine($"EnhancedAdoNetAppender 批量写入成功");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EnhancedAdoNetAppender 批量写入失败: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"堆栈: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"内部异常: {ex.InnerException.Message}");
                }

                throw;
            }
        }
 
    }
}
