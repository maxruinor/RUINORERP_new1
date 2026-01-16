using System;
using log4net.Appender;

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
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"激活 AdoNetAppender 失败: {ex.Message}");
                throw;
            }

            base.ActivateOptions();
        }
    }
}
