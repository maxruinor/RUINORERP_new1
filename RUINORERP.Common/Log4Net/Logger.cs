using log4net;
using log4net.Config;
using RUINORERP.Common.Helper;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

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
                // 首先配置基本日志器，以便后续错误可以记录
                BasicConfigurator.Configure();

                // 预解密连接字符串
                var connectionString = CryptoHelper.GetDecryptedConnectionString();

                // 如果有配置文件，使用详细配置
                if (File.Exists("log4net.config"))
                {
                    // 加载配置文件并替换连接字符串
                    XmlDocument log4netConfig = new XmlDocument();
                    log4netConfig.Load(File.OpenRead("log4net.config"));

                    // 查找并替换连接字符串
                    XmlNodeList nodes = log4netConfig.SelectNodes("//connectionString[@value]");
                    foreach (XmlNode node in nodes)
                    {
                        XmlAttribute valueAttr = node.Attributes["value"];
                        if (valueAttr != null && valueAttr.Value == "${ConnectionString}")
                        {
                            valueAttr.Value = connectionString;
                        }
                    }

                    // 使用修改后的配置
                    var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
                    XmlConfigurator.Configure(logRepository, log4netConfig.DocumentElement);

                    log.Info("日志系统初始化完成（使用配置文件）");
                }
                else
                {
                    log.Warn("未找到log4net.config文件，使用基本日志配置");
                }

                initialized = true;
            }
            catch (Exception ex)
            {
                // 尝试记录错误到基本日志器
                try
                {
                    log.Error("日志初始化失败", ex);
                }
                catch
                {
                    // 如果基本日志也失败，显示消息框
                    MessageBox.Show($"日志初始化失败: {ex.Message}\n\n应用程序无法正常记录日志。",
                                    "严重错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
      
        public static void Debug(string message) => log.Debug(message);
        public static void Info(string message) => log.Info(message);
        public static void Warn(string message) => log.Warn(message);
        public static void Error(string message, Exception ex = null) => log.Error(message, ex);
    }
}