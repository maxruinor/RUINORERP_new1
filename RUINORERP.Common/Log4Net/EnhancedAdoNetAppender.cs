using System;
using log4net.Appender;
using RUINORERP.Common.Helper;

namespace RUINORERP.Common.Log4Net
{
    /// <summary>
    /// 增强版 AdoNetAppender,支持自动解密连接字符串
    /// </summary>
    public class EnhancedAdoNetAppender : AdoNetAppender
    {
        /// <summary>
        /// 激活 Appender 配置,自动处理加密连接字符串
        /// </summary>
        public override void ActivateOptions()
        {
            try
            {
                // 如果连接字符串包含加密标识,自动解密
                if (!string.IsNullOrEmpty(ConnectionString) && ConnectionString.Contains("Encrypted:"))
                {
                    System.Diagnostics.Debug.WriteLine("检测到加密连接字符串,开始解密...");
                    ConnectionString = CryptoHelper.GetDecryptedConnectionString();
                    System.Diagnostics.Debug.WriteLine($"连接字符串已解密,长度: {ConnectionString?.Length ?? 0}");
                }
                
                // 如果连接字符串包含占位符,尝试从配置文件获取
                if (!string.IsNullOrEmpty(ConnectionString) && ConnectionString.Contains("${ConnectionString}"))
                {
                    System.Diagnostics.Debug.WriteLine("检测到连接字符串占位符,开始替换...");
                    ConnectionString = CryptoHelper.GetDecryptedConnectionString();
                    System.Diagnostics.Debug.WriteLine($"连接字符串已替换,长度: {ConnectionString?.Length ?? 0}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"处理连接字符串失败: {ex.Message}");
                throw;
            }
            
            base.ActivateOptions();
        }
    }
}
