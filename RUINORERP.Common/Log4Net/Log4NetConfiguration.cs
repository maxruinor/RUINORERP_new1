using System;
using System.IO;
using System.Xml;
using log4net.Config;
using RUINORERP.Common.Helper;

namespace RUINORERP.Common.Log4Net
{
    /// <summary>
    /// Log4Net 配置管理器,负责日志系统的初始化和配置
    /// </summary>
    public static class Log4NetConfiguration
    {
        /// <summary>
        /// 初始化 log4net 配置
        /// </summary>
        /// <param name="configFilePath">配置文件路径,相对或绝对路径</param>
        /// <param name="connectionString">数据库连接字符串,如果为null则从配置文件自动获取</param>
        public static void Initialize(string configFilePath, string connectionString = null)
        {
            if (string.IsNullOrEmpty(configFilePath))
            {
                throw new ArgumentException("配置文件路径不能为空", nameof(configFilePath));
            }

            System.Diagnostics.Debug.WriteLine($"========== 开始初始化 log4net 配置 ==========");
            System.Diagnostics.Debug.WriteLine($"配置文件路径: {configFilePath}");

            try
            {
                // 1. 加载 XML 配置文件
                var xmlDoc = LoadConfigFile(configFilePath);

                // 2. 替换连接字符串占位符
                ReplaceConnectionStringPlaceholder(xmlDoc, connectionString);

                // 3. 使用 XmlConfigurator 配置
                var log4netElement = xmlDoc.SelectSingleNode("//log4net") as XmlElement;
                if (log4netElement == null)
                {
                    throw new InvalidOperationException("配置文件中未找到 <log4net> 根元素");
                }

                XmlConfigurator.Configure(log4netElement);

                System.Diagnostics.Debug.WriteLine("========== log4net 配置初始化完成 ==========");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"初始化 log4net 配置失败: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"异常详情: {ex.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="configFilePath">配置文件路径</param>
        /// <returns>XmlDocument 对象</returns>
        private static XmlDocument LoadConfigFile(string configFilePath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            string actualFilePath = configFilePath;

            // 如果是相对路径,尝试相对于应用程序基础目录或当前目录
            if (!Path.IsPathRooted(configFilePath))
            {
                // 尝试相对于应用程序基础目录
                var baseDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFilePath);
                if (File.Exists(baseDirPath))
                {
                    actualFilePath = baseDirPath;
                    System.Diagnostics.Debug.WriteLine($"使用应用程序基础目录的配置文件: {actualFilePath}");
                }
                // 尝试相对于当前目录
                else if (File.Exists(configFilePath))
                {
                    actualFilePath = configFilePath;
                    System.Diagnostics.Debug.WriteLine($"使用当前目录的配置文件: {actualFilePath}");
                }
                else
                {
                    throw new FileNotFoundException($"未找到配置文件: {configFilePath}");
                }
            }
            else
            {
                // 绝对路径
                if (!File.Exists(actualFilePath))
                {
                    throw new FileNotFoundException($"未找到配置文件: {actualFilePath}");
                }
            }

            // 加载 XML 文件
            try
            {
                using (var stream = new FileStream(actualFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    xmlDoc.Load(stream);
                }
                System.Diagnostics.Debug.WriteLine($"配置文件加载成功: {actualFilePath}");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"加载配置文件失败: {actualFilePath}", ex);
            }

            return xmlDoc;
        }

        /// <summary>
        /// 替换配置文件中的连接字符串占位符
        /// </summary>
        /// <param name="xmlDoc">XmlDocument 对象</param>
        /// <param name="connectionString">数据库连接字符串,如果为null则使用加密连接字符串</param>
        private static void ReplaceConnectionStringPlaceholder(XmlDocument xmlDoc, string connectionString)
        {
            try
            {
                // 查找所有连接字符串节点
                XmlNodeList nodes = xmlDoc.SelectNodes("//connectionString[@value]");
                
                if (nodes == null || nodes.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("警告: 配置文件中未找到 connectionString 节点");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"找到 {nodes.Count} 个连接字符串节点");

                foreach (XmlNode node in nodes)
                {
                    XmlAttribute valueAttr = node.Attributes["value"];
                    if (valueAttr == null || string.IsNullOrEmpty(valueAttr.Value))
                    {
                        continue;
                    }

                    string originalValue = valueAttr.Value;
                    string newValue = originalValue;

                    // 如果没有传入连接字符串,自动获取加密连接字符串
                    if (string.IsNullOrEmpty(connectionString))
                    {
                        try
                        {
                            connectionString = CryptoHelper.GetDecryptedConnectionString();
                            System.Diagnostics.Debug.WriteLine($"已从配置文件获取加密连接字符串");
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"获取加密连接字符串失败: {ex.Message}");
                            throw;
                        }
                    }

                    // 替换占位符
                    if (newValue.Contains("${ConnectionString}"))
                    {
                        newValue = newValue.Replace("${ConnectionString}", connectionString);
                        System.Diagnostics.Debug.WriteLine("已替换连接字符串占位符 ${ConnectionString}");
                    }

                    // 更新值
                    if (newValue != originalValue)
                    {
                        valueAttr.Value = newValue;
                        System.Diagnostics.Debug.WriteLine($"连接字符串已更新,长度: {newValue?.Length ?? 0}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"替换连接字符串占位符失败: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"异常详情: {ex.StackTrace}");
                throw;
            }
        }
    }
}
