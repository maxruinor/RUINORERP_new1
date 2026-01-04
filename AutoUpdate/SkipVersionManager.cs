using System;
using System.IO;
using System.Xml;

namespace AutoUpdate
{
    /// <summary>
    /// 跳过版本管理器
    /// 负责管理用户跳过的版本信息，包括存储和检查功能
    /// </summary>
    public class SkipVersionManager
    {
        private readonly string skipVersionFilePath;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SkipVersionManager()
        {
            try
            {
                // 使用程序当前目录存储跳过的版本信息，避免权限问题
                string currentDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                // 如果获取当前目录失败，使用当前工作目录
                if (string.IsNullOrEmpty(currentDir))
                {
                    currentDir = Environment.CurrentDirectory;
                }
                skipVersionFilePath = Path.Combine(currentDir, "SkippedVersions.xml");
            }
            catch (Exception ex)
            {
                // 记录异常但不抛出，确保功能不会中断
                System.Diagnostics.Debug.WriteLine("初始化SkipVersionManager失败: " + ex.Message);
                // 使用一个安全的默认值，确保程序能继续运行
                skipVersionFilePath = "SkippedVersions.xml";
            }
        }

        /// <summary>
        /// 记录跳过的版本
        /// </summary>
        /// <param name="version">要跳过的版本号</param>
        /// <param name="applicationId">应用程序ID，用于区分不同应用</param>
        public void SkipVersion(string version, string applicationId)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                // 如果文件存在，加载现有文件；否则创建新文档
                if (File.Exists(skipVersionFilePath))
                {
                    xmlDoc.Load(skipVersionFilePath);
                }
                else
                {
                    XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                    xmlDoc.AppendChild(xmlDeclaration);
                    XmlElement rootElement = xmlDoc.CreateElement("SkippedVersions");
                    xmlDoc.AppendChild(rootElement);
                }

                XmlNode rootNode = xmlDoc.DocumentElement;

                // 检查是否已存在相同应用ID和版本的记录
                XmlNode existingNode = FindSkipVersionNode(xmlDoc, applicationId, version);
                if (existingNode != null)
                {
                    // 更新现有记录的时间戳
                    existingNode.Attributes["skippedDate"].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    // 创建新记录
                    XmlElement skipVersionElement = xmlDoc.CreateElement("SkipVersion");
                    skipVersionElement.SetAttribute("applicationId", applicationId);
                    skipVersionElement.SetAttribute("version", version);
                    skipVersionElement.SetAttribute("skippedDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    rootNode.AppendChild(skipVersionElement);
                }

                // 保存到文件
                xmlDoc.Save(skipVersionFilePath);
            }
            catch (Exception ex)
            {
                // 记录异常但不抛出，确保功能不会中断
                System.Diagnostics.Debug.WriteLine("保存跳过版本信息失败: " + ex.Message);
            }
        }

        /// <summary>
        /// 检查指定版本是否被跳过
        /// </summary>
        /// <param name="version">要检查的版本号</param>
        /// <param name="applicationId">应用程序ID</param>
        /// <returns>如果版本被跳过返回true，否则返回false</returns>
        public bool IsVersionSkipped(string version, string applicationId)
        {
            try
            {
                if (!File.Exists(skipVersionFilePath))
                {
                    return false;
                }

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(skipVersionFilePath);

                // 查找匹配的跳过版本记录
                XmlNode skipNode = FindSkipVersionNode(xmlDoc, applicationId, version);
                return skipNode != null;
            }
            catch (Exception ex)
            {
                // 出错时默认返回false，避免影响正常的更新流程
                System.Diagnostics.Debug.WriteLine("检查跳过版本信息失败: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 移除跳过的版本记录
        /// 当用户选择不再跳过某个版本时使用
        /// </summary>
        /// <param name="version">要移除跳过状态的版本号</param>
        /// <param name="applicationId">应用程序ID</param>
        public void RemoveSkippedVersion(string version, string applicationId)
        {
            try
            {
                if (!File.Exists(skipVersionFilePath))
                {
                    return;
                }

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(skipVersionFilePath);

                XmlNode skipNode = FindSkipVersionNode(xmlDoc, applicationId, version);
                if (skipNode != null)
                {
                    skipNode.ParentNode.RemoveChild(skipNode);
                    xmlDoc.Save(skipVersionFilePath);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("移除跳过版本信息失败: " + ex.Message);
            }
        }

        /// <summary>
        /// 在XML文档中查找指定的跳过版本记录
        /// </summary>
        /// <param name="xmlDoc">XML文档对象</param>
        /// <param name="applicationId">应用程序ID</param>
        /// <param name="version">版本号</param>
        /// <returns>找到的节点，如果不存在返回null</returns>
        private XmlNode FindSkipVersionNode(XmlDocument xmlDoc, string applicationId, string version)
        {
            XmlNodeList nodes = xmlDoc.SelectNodes("//SkipVersion");
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    if (node.Attributes["applicationId"]?.Value == applicationId && 
                        node.Attributes["version"]?.Value == version)
                    {
                        return node;
                    }
                }
            }
            return null;
        }
    }
}
