using log4net;
using log4net.Config;
using RUINORERP.Common.Helper;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Data.SqlClient;

namespace RUINORERP.Common.Log4Net
{
    public static class Logger
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Logger));
        private static bool initialized = false;

        /// <summary>
        /// 初始化日志系统
        /// </summary>
        public static void Initialize()
        {
            if (initialized) return;

            try
            {
                Console.WriteLine("开始初始化日志系统...");
                
                // 首先配置基本日志器，以便后续错误可以记录
                BasicConfigurator.Configure();
                Console.WriteLine("基本日志器配置完成");

                // 尝试获取解密后的连接字符串，但不中断初始化流程
                string connectionString = null;
                try
                {
                    connectionString = CryptoHelper.GetDecryptedConnectionString();
                    Console.WriteLine("成功获取解密后的连接字符串");
                    
                    // 测试数据库连接是否有效
                    if (TestDatabaseConnection(connectionString))
                    {
                        Console.WriteLine("数据库连接测试成功");
                    }
                    else
                    {
                        Console.WriteLine("警告：数据库连接测试失败");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("警告：获取连接字符串失败: " + ex.Message);
                    // 继续初始化流程，即使没有连接字符串
                }

                // 如果有配置文件，使用详细配置
                if (File.Exists("log4net.config"))
                {
                    // 加载配置文件并替换连接字符串
                    try
                    {
                        XmlDocument log4netConfig = new XmlDocument();
                        log4netConfig.Load(File.OpenRead("log4net.config"));
                        Console.WriteLine("成功加载log4net.config配置文件");

                        // 查找并替换连接字符串
                        if (!string.IsNullOrEmpty(connectionString))
                        {
                            XmlNodeList nodes = log4netConfig.SelectNodes("//connectionString[@value]");
                            foreach (XmlNode node in nodes)
                            {
                                XmlAttribute valueAttr = node.Attributes["value"];
                                if (valueAttr != null && valueAttr.Value.Contains("${ConnectionString}"))
                                {
                                    valueAttr.Value = connectionString;
                                    Console.WriteLine("已在配置文件中替换连接字符串占位符");
                                }
                            }
                        }

                        // 使用修改后的配置
                        var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
                        XmlConfigurator.Configure(logRepository, log4netConfig.DocumentElement);
                        Console.WriteLine("日志系统初始化完成（使用配置文件）");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("警告：配置文件处理失败: " + ex.Message);
                        log.Warn("配置文件处理失败，继续使用基本日志配置", ex);
                    }
                }
                else
                {
                    Console.WriteLine("未找到log4net.config文件，使用基本日志配置");
                    log.Warn("未找到log4net.config文件，使用基本日志配置");
                }

                initialized = true;
                Console.WriteLine("日志系统初始化成功标记已设置");
            }
            catch (Exception ex)
            {
                Console.WriteLine("严重错误：日志系统初始化失败: " + ex.Message);
                // 尝试记录错误到基本日志器
                try
                {
                    log.Error("日志初始化失败", ex);
                }
                catch
                {
                    // 如果基本日志也失败，显示消息框
                    MessageBox.Show($"日志初始化失败: {ex.Message}\n\n应用程序无法正常记录详细日志，但仍可以使用基本日志功能。",
                                    "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        /// 测试数据库连接是否有效
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <returns>连接是否有效</returns>
        private static bool TestDatabaseConnection(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                return false;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("数据库连接已成功打开");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("数据库连接测试失败: " + ex.Message);
                return false;
            }
        }
      
        /// <summary>
        /// 记录调试信息
        /// </summary>
        /// <param name="message">日志消息</param>
        public static void Debug(string message)
        {
            try
            {
                if (log != null)
                {
                    log.Debug(message);
                }
                else
                {
                    Console.WriteLine($"[DEBUG] {message}");
                }
            }
            catch (Exception logEx)
            {
                Console.WriteLine($"记录调试日志失败: {logEx.Message}\n原消息: {message}");
            }
        }

        /// <summary>
        /// 记录一般信息
        /// </summary>
        /// <param name="message">日志消息</param>
        public static void Info(string message)
        {
            try
            {
                if (log != null)
                {
                    log.Info(message);
                }
                else
                {
                    Console.WriteLine($"[INFO] {message}");
                }
            }
            catch (Exception logEx)
            {
                Console.WriteLine($"记录信息日志失败: {logEx.Message}\n原消息: {message}");
            }
        }

        /// <summary>
        /// 记录警告信息
        /// </summary>
        /// <param name="message">日志消息</param>
        public static void Warn(string message)
        {
            try
            {
                if (log != null)
                {
                    log.Warn(message);
                }
                else
                {
                    Console.WriteLine($"[WARN] {message}");
                }
            }
            catch (Exception logEx)
            {
                Console.WriteLine($"记录警告日志失败: {logEx.Message}\n原消息: {message}");
            }
        }

        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="ex">异常对象（可选）</param>
        public static void Error(string message, Exception ex = null)
        {
            try
            {
                if (log != null)
                {
                    log.Error(message, ex);
                }
                else
                {
                    if (ex != null)
                    {
                        Console.WriteLine($"[ERROR] {message}\n异常详情: {ex.Message}\n堆栈跟踪: {ex.StackTrace}");
                    }
                    else
                    {
                        Console.WriteLine($"[ERROR] {message}");
                    }
                }
            }
            catch (Exception logEx)
            {
                Console.WriteLine($"记录错误日志失败: {logEx.Message}\n原消息: {message}" +
                                  (ex != null ? $"\n原始异常: {ex.Message}" : ""));
            }
        }
    }
}