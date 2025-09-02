using RUINORERP.Common.Helper;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace RUINORERP.Common.Log4Net {
    /// <summary>
    /// 日志系统诊断助手类
    /// 用于测试和诊断日志系统的问题
    /// </summary>
    public static class LogDiagnosticHelper
    {
        /// <summary>
        /// 测试日志系统是否正常工作
        /// </summary>
        /// <returns>诊断结果信息</returns>
        public static string TestLoggingSystem()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine("===== 日志系统诊断报告 =====");
            result.AppendLine($"诊断时间: {DateTime.Now}");
            result.AppendLine();

            // 1. 测试基本日志记录
            try
            {
                result.AppendLine("1. 测试基本日志记录:");
                Logger.Debug("日志系统诊断 - Debug测试消息");
                Logger.Info("日志系统诊断 - Info测试消息");
                Logger.Warn("日志系统诊断 - Warning测试消息");
                Logger.Error("日志系统诊断 - Error测试消息", new Exception("测试异常"));
                result.AppendLine("   ✓ 基本日志记录成功");
            }
            catch (Exception ex)
            {
                result.AppendLine($"   ✗ 基本日志记录失败: {ex.Message}");
                result.AppendLine($"   堆栈: {ex.StackTrace}");
            }

            result.AppendLine();

            // 2. 测试数据库连接和配置
            result.AppendLine("2. 测试数据库连接和配置:");
            try
            {
                // 获取连接字符串
                string connectionString = CryptoHelper.GetDecryptedConnectionString();
                if (!string.IsNullOrEmpty(connectionString))
                {
                    result.AppendLine("   ✓ 获取连接字符串成功");
                    
                    // 测试数据库连接
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        result.AppendLine("   ✓ 数据库连接成功");
                        conn.Close();
                        
                        // 测试表是否存在
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM sys.tables WHERE name = 'Logs'", conn))
                            {
                                conn.Open();
                                int count = (int)cmd.ExecuteScalar();
                                conn.Close();
                                if (count > 0)
                                {
                                    result.AppendLine("   ✓ Logs表存在");
                                }
                                else
                                {
                                    result.AppendLine("   ✗ Logs表不存在");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            result.AppendLine($"   ✗ 检查Logs表时出错: {ex.Message}");
                        }
                    }
                }
                else
                {
                    result.AppendLine("   ✗ 连接字符串为空或无效");
                }
            }
            catch (Exception ex)
            {
                result.AppendLine($"   ✗ 数据库测试失败: {ex.Message}");
                result.AppendLine($"   堆栈: {ex.StackTrace}");
            }

            result.AppendLine();
            result.AppendLine("===== 诊断报告结束 =====");
            return result.ToString();
        }

        /// <summary>
        /// 检查日志配置文件是否存在
        /// </summary>
        /// <returns>配置文件检查结果</returns>
        public static string CheckLogConfiguration()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine("===== 日志配置检查报告 =====");
            
            // 检查log4net.config文件
            if (File.Exists("log4net.config"))
            {
                result.AppendLine("✓ log4net.config文件存在");
                
                try
                {
                    // 检查文件内容是否有效
                    string content = File.ReadAllText("log4net.config");
                    if (!string.IsNullOrEmpty(content) && content.Contains("<log4net>"))
                    {
                        result.AppendLine("✓ log4net.config文件内容有效");
                    }
                    else
                    {
                        result.AppendLine("✗ log4net.config文件内容无效或不完整");
                    }
                }
                catch (Exception ex)
                {
                    result.AppendLine($"✗ 读取log4net.config文件时出错: {ex.Message}");
                }
            }
            else
            {
                result.AppendLine("✗ log4net.config文件不存在");
            }
            
            // 检查app.config文件
            if (File.Exists(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile))
            {
                result.AppendLine("✓ app.config文件存在");
            }
            else
            {
                result.AppendLine("✗ app.config文件不存在");
            }
            
            result.AppendLine("===== 配置检查结束 =====");
            return result.ToString();
        }
    }
}