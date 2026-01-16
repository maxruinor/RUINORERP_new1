using System;
using System.IO;
using System.Xml;
using log4net;
using log4net.Config;
using log4net.Repository;
using RUINORERP.Common.Helper;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace RUINORERP.Common.Log4Net
{
    /// <summary>
    /// Log4Net 配置管理器,负责日志系统的初始化和配置
    /// </summary>
    public static class Log4NetConfiguration
    {
        /// <summary>
        /// 共享日志仓库名称,用于客户端和服务器统一日志配置
        /// </summary>
        public const string SHARED_REPOSITORY_NAME = "RUINORERP_Shared_LoggerRepository";

        /// <summary>
        /// 初始化 log4net 配置
        /// </summary>
        /// <param name="configFilePath">配置文件路径,相对或绝对路径</param>
        /// <param name="connectionString">数据库连接字符串,如果为null则从appsettings.json获取</param>
        public static void Initialize(string configFilePath, string connectionString = null)
        {
            if (string.IsNullOrEmpty(configFilePath))
            {
                throw new ArgumentException("配置文件路径不能为空", nameof(configFilePath));
            }

            System.Diagnostics.Debug.WriteLine($"========== 开始初始化 log4net 配置 ==========");
            System.Diagnostics.Debug.WriteLine($"配置文件路径: {configFilePath}");
            System.Diagnostics.Debug.WriteLine($"使用共享仓库: {SHARED_REPOSITORY_NAME}");

            try
            {
                // 如果连接字符串为空,从appsettings.json获取加密连接字符串
                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = GetConnectionStringFromAppSettings();
                }
                else
                {
                    // 传入的连接字符串需要解密
                    connectionString = DecryptConnectionString(connectionString);
                }

                //1. 加载 XML 配置文件
                var xmlDoc = LoadConfigFile(configFilePath);

                //2. 替换连接字符串占位符
                ReplaceConnectionStringPlaceholder(xmlDoc, connectionString);

                //3. 使用 XmlConfigurator 配置
                var log4netElement = xmlDoc.SelectSingleNode("//log4net") as XmlElement;
                if (log4netElement == null)
                {
                    throw new InvalidOperationException("配置文件中未找到 <log4net> 根元素");
                }

                // 获取或创建共享日志仓库
                ILoggerRepository repository;
                
                // 检查是否已存在该仓库
                repository = LogManager.GetAllRepositories().FirstOrDefault(r => r.Name == SHARED_REPOSITORY_NAME);
                
                if (repository != null)
                {
                    System.Diagnostics.Debug.WriteLine($"使用现有日志仓库: {SHARED_REPOSITORY_NAME}");
                }
                else
                {
                    repository = LogManager.CreateRepository(SHARED_REPOSITORY_NAME);
                    System.Diagnostics.Debug.WriteLine($"创建了新的日志仓库: {SHARED_REPOSITORY_NAME}");
                }

                XmlConfigurator.Configure(repository, log4netElement);

                System.Diagnostics.Debug.WriteLine("========== log4net 配置初始化完成 ==========");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"初始化 log4net 配置失败: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"异常详情: {ex.StackTrace}");
                throw;
            }
        }

        private static string GetConnectionStringFromAppSettings()
        {
            try
            {
                string appSettingsPath = FindAppSettingsFile();
                if (string.IsNullOrEmpty(appSettingsPath))
                {
                    throw new FileNotFoundException("未找到 appsettings.json 文件");
                }

                string jsonContent = File.ReadAllText(appSettingsPath);
                var jsonObject = JObject.Parse(jsonContent);

                string encryptedConnectionString = jsonObject["ConnectString"]?.ToString();
                if (string.IsNullOrEmpty(encryptedConnectionString))
                {
                    throw new InvalidOperationException("appsettings.json 中未找到 ConnectString 配置");
                }

                System.Diagnostics.Debug.WriteLine($"从 appsettings.json 获取到加密连接字符串, 长度: {encryptedConnectionString.Length}");
                return DecryptConnectionString(encryptedConnectionString);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"从 appsettings.json 获取连接字符串失败: {ex.Message}");
                throw;
            }
        }

        private static string FindAppSettingsFile()
        {
            string[] possiblePaths = new string[]
            {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.Development.json"),
                Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"),
                Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Development.json")
            };

            foreach (string path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    System.Diagnostics.Debug.WriteLine($"找到 appsettings.json: {path}");
                    return path;
                }
            }

            return null;
        }

        private static string DecryptConnectionString(string encryptedConnectionString)
        {
            try
            {
                string key = "ruinor1234567890";
                System.Diagnostics.Debug.WriteLine("开始解密连接字符串...");
                string decryptedConnectionString = HLH.Lib.Security.EncryptionHelper.AesDecrypt(encryptedConnectionString, key);
                System.Diagnostics.Debug.WriteLine("连接字符串解密成功");
                return decryptedConnectionString;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"解密连接字符串失败: {ex.Message}");
                throw;
            }
        }

        private static XmlDocument LoadConfigFile(string configFilePath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            string actualFilePath = configFilePath;

            if (!Path.IsPathRooted(configFilePath))
            {
                var baseDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFilePath);
                if (File.Exists(baseDirPath))
                {
                    actualFilePath = baseDirPath;
                    System.Diagnostics.Debug.WriteLine($"使用应用程序基础目录的配置文件: {actualFilePath}");
                }
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
                if (!File.Exists(actualFilePath))
                {
                    throw new FileNotFoundException($"未找到配置文件: {actualFilePath}");
                }
            }

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

        private static void ReplaceConnectionStringPlaceholder(XmlDocument xmlDoc, string connectionString)
        {
            try
            {
                // 验证解密后的连接字符串
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("解密后的连接字符串为空");
                }

                if (!connectionString.ToLower().Contains("server"))
                {
                    throw new InvalidOperationException("解密后的连接字符串无效，不包含 'server' 关键字");
                }

                XmlNodeList nodes = xmlDoc.SelectNodes("//connectionString[@value]");

                if (nodes == null || nodes.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("警告: 配置文件中未找到 connectionString 节点");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"找到 {nodes.Count} 个连接字符串节点");

                int replacedCount = 0;
                foreach (XmlNode node in nodes)
                {
                    XmlAttribute valueAttr = node.Attributes["value"];
                    if (valueAttr == null || string.IsNullOrEmpty(valueAttr.Value))
                    {
                        continue;
                    }

                    string originalValue = valueAttr.Value;
                    string newValue = originalValue;

                    if (newValue.Contains("${ConnectionString}"))
                    {
                        newValue = connectionString;
                        valueAttr.Value = newValue;
                        replacedCount++;
                        System.Diagnostics.Debug.WriteLine("已替换连接字符串占位符 ${ConnectionString}");
                        System.Diagnostics.Debug.WriteLine($"替换后连接字符串长度: {newValue?.Length ?? 0}");
                    }
                }

                if (replacedCount == 0)
                {
                    System.Diagnostics.Debug.WriteLine("警告: 没有替换任何连接字符串占位符");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"成功替换 {replacedCount} 个连接字符串占位符");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"替换连接字符串占位符失败: {ex.Message}");
                throw;
            }
        }
    }
}
