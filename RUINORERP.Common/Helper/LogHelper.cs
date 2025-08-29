using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace RUINORERP.Common
{
    public static class Logger
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Logger));

        static Logger()
        {
            InitializeLogging();
        }

        /// <summary>
        /// 初始化日志系统
        /// </summary>
        private static void InitializeLogging()
        {
            try
            {
                // 加载log4net配置
                var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
                XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

                log.Info("日志系统初始化完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"日志初始化失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 记录信息日志
        /// </summary>
        public static void Info(string message, string operatorName = null, string modName = null,
                               string path = null, string actionName = null, string ip = null)
        {
            SetCustomProperties(operatorName, modName, path, actionName, ip);
            log.Info(message);
            ClearCustomProperties();
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        public static void Error(string message, Exception ex = null, string operatorName = null,
                                string modName = null, string path = null, string actionName = null,
                                string ip = null)
        {
            SetCustomProperties(operatorName, modName, path, actionName, ip);
            if (ex != null)
                log.Error(message, ex);
            else
                log.Error(message);
            ClearCustomProperties();
        }

        /// <summary>
        /// 记录警告日志
        /// </summary>
        public static void Warn(string message, string operatorName = null, string modName = null,
                               string path = null, string actionName = null, string ip = null)
        {
            SetCustomProperties(operatorName, modName, path, actionName, ip);
            log.Warn(message);
            ClearCustomProperties();
        }

        /// <summary>
        /// 设置自定义属性
        /// </summary>
        private static void SetCustomProperties(string operatorName, string modName, string path,
                                              string actionName, string ip)
        {
            if (!string.IsNullOrEmpty(operatorName))
                log4net.ThreadContext.Properties["Operator"] = operatorName;

            if (!string.IsNullOrEmpty(modName))
                log4net.ThreadContext.Properties["ModName"] = modName;

            if (!string.IsNullOrEmpty(path))
                log4net.ThreadContext.Properties["Path"] = path;

            if (!string.IsNullOrEmpty(actionName))
                log4net.ThreadContext.Properties["ActionName"] = actionName;

            if (!string.IsNullOrEmpty(ip))
                log4net.ThreadContext.Properties["IP"] = ip;

            // 自动添加机器名
            log4net.ThreadContext.Properties["MachineName"] = Environment.MachineName;

            // 如果需要MAC地址，可以在这里添加获取MAC地址的代码
        }

        /// <summary>
        /// 清除自定义属性
        /// </summary>
        private static void ClearCustomProperties()
        {
            log4net.ThreadContext.Properties.Remove("Operator");
            log4net.ThreadContext.Properties.Remove("ModName");
            log4net.ThreadContext.Properties.Remove("Path");
            log4net.ThreadContext.Properties.Remove("ActionName");
            log4net.ThreadContext.Properties.Remove("IP");
            log4net.ThreadContext.Properties.Remove("MachineName");
        }
    }
}