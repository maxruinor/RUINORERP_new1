using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace AutoUpdate
{
    /// <summary>
    /// 增强版本管理器
    /// 提供更强大的版本管理功能，支持压缩包的版本管理
    /// </summary>
    public class EnhancedVersionManager
    {
        /// <summary>
        /// 版本历史管理器
        /// </summary>
        private VersionHistoryManager versionHistoryManager;

        /// <summary>
        /// 更新服务器URL
        /// </summary>
        public string UpdateServerUrl { get; set; }

        /// <summary>
        /// 应用程序ID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 应用程序主目录
        /// </summary>
        public string ApplicationDirectory { get; set; }

        /// <summary>
        /// 临时文件目录
        /// </summary>
        public string TempDirectory { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="applicationDirectory">应用程序主目录</param>
        /// <param name="updateServerUrl">更新服务器URL</param>
        /// <param name="appId">应用程序ID</param>
        public EnhancedVersionManager(string applicationDirectory = null, string updateServerUrl = null, string appId = null)
        {
            ApplicationDirectory = applicationDirectory ?? Environment.CurrentDirectory;
            TempDirectory = Path.Combine(ApplicationDirectory, "Temp");
            UpdateServerUrl = updateServerUrl;
            AppId = appId;
            
            versionHistoryManager = new VersionHistoryManager();
            
            // 确保临时目录存在
            if (!Directory.Exists(TempDirectory))
            {
                Directory.CreateDirectory(TempDirectory);
            }
        }

        /// <summary>
        /// 检查并处理压缩更新包
        /// </summary>
        /// <param name="packagePath">压缩包路径</param>
        /// <param name="version">版本信息</param>
        /// <returns>处理结果</returns>
        public bool ProcessCompressedUpdate(string packagePath, VersionEntry version)
        {
            return ProcessCompressedUpdateInternal(packagePath, version);
        }

        /// <summary>
        /// 静态方法：处理压缩更新包
        /// </summary>
        /// <param name="packagePath">压缩包路径</param>
        /// <param name="version">版本信息</param>
        /// <returns>处理结果</returns>
        public static bool ProcessCompressedUpdateStatic(string packagePath, VersionEntry version)
        {
            // 创建临时实例来处理更新
            EnhancedVersionManager manager = new EnhancedVersionManager();
            return manager.ProcessCompressedUpdateInternal(packagePath, version);
        }

        /// <summary>
        /// 内部方法：处理压缩更新包的核心逻辑
        /// </summary>
        /// <param name="packagePath">压缩包路径</param>
        /// <param name="version">版本信息</param>
        /// <returns>处理结果</returns>
        private bool ProcessCompressedUpdateInternal(string packagePath, VersionEntry version)
        {
            try
            {
                // 解压更新包
                string tempExtractionDir = Path.Combine(TempDirectory, $"Extract_{Guid.NewGuid()}");
                if (!Directory.Exists(tempExtractionDir))
                {
                    Directory.CreateDirectory(tempExtractionDir);
                }

                // 使用AutoUpdate中的GZip进行解压
                GZipResult decompressResult = GZip.Decompress(
                    Path.GetDirectoryName(packagePath),
                    tempExtractionDir,
                    Path.GetFileName(packagePath),
                    true,  // 删除临时文件
                    true,  // 写入文件
                    null,  // 不添加扩展名
                    null,  // 不使用文件哈希表过滤
                    4096   // 缓冲区大小
                );

                if (!decompressResult.Errors)
                {
                    // 检查是否包含版本信息文件
                    string versionInfoPath = Path.Combine(tempExtractionDir, "version_info.xml");
                    if (File.Exists(versionInfoPath))
                    {
                        // 解析版本信息
                        VersionInfo versionInfo = ParseVersionInfo(versionInfoPath);
                        if (versionInfo != null)
                        {
                            // 验证版本信息
                            if (string.IsNullOrEmpty(version.Version) || version.Version == versionInfo.Version)
                            {
                                version.Version = versionInfo.Version;
                                // AppId存储在EnhancedVersionManager对象中，而不是VersionEntry中
                                // version.InstallTime已经由调用者设置
                            }
                        }
                    }

                    // 复制解压后的文件到应用目录
                    CopyExtractedFiles(tempExtractionDir, ApplicationDirectory);

                    // 记录版本历史
                    if (!string.IsNullOrEmpty(version.Version))
                    {
                        string folderName = $"{version.Version}_{version.InstallTime.ToString("yyyyMMddHHmmss")}";
                        versionHistoryManager.RecordNewVersion(version.Version, folderName, null, null, version.InstallTime);
                    }

                    // 清理临时目录
                    if (Directory.Exists(tempExtractionDir))
                    {
                        Directory.Delete(tempExtractionDir, true);
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                WriteLog($"处理压缩更新包失败: {ex.Message}");
            }
            
            return false;
        }

        /// <summary>
        /// 从XML文件解析版本信息
        /// </summary>
        /// <param name="filePath">版本信息文件路径</param>
        /// <returns>版本信息对象</returns>
        private VersionInfo ParseVersionInfo(string filePath)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(VersionInfo));
                using (var reader = new StreamReader(filePath))
                {
                    return (VersionInfo)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                WriteLog($"解析版本信息失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 复制解压后的文件到应用目录
        /// </summary>
        /// <param name="sourceDir">源目录</param>
        /// <param name="targetDir">目标目录</param>
        private void CopyExtractedFiles(string sourceDir, string targetDir)
        {
            // 遍历源目录中的所有文件和子目录
            foreach (string dirPath in Directory.GetDirectories(sourceDir, "*", SearchOption.AllDirectories))
            {
                // 创建对应的目标目录结构
                Directory.CreateDirectory(dirPath.Replace(sourceDir, targetDir));
            }

            // 复制所有文件
            foreach (string filePath in Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories))
            {
                string destPath = filePath.Replace(sourceDir, targetDir);
                
                // 如果目标文件正在使用，尝试先备份
                if (File.Exists(destPath))
                {
                    try
                    {
                        // 尝试复制，如果失败则重命名备份
                        File.Copy(filePath, destPath, true);
                    }
                    catch (IOException)
                    {
                        // 目标文件可能被锁定，重命名后再复制
                        string backupPath = destPath + ".bak";
                        if (File.Exists(backupPath))
                        {
                            File.Delete(backupPath);
                        }
                        File.Move(destPath, backupPath);
                        File.Copy(filePath, destPath);
                    }
                }
                else
                {
                    File.Copy(filePath, destPath);
                }
            }
        }

        /// <summary>
        /// 获取所有版本历史（包括压缩包版本）
        /// </summary>
        /// <returns>版本历史列表</returns>
        public List<VersionEntry> GetAllVersions()
        {
            return versionHistoryManager.GetAllVersions();
        }

        /// <summary>
        /// 检查是否需要更新（支持压缩包）
        /// </summary>
        /// <param name="currentVersion">当前版本</param>
        /// <returns>是否需要更新</returns>
        public bool CheckForUpdates(string currentVersion)
        {
            // 这里可以实现与服务器通信，检查是否有新的压缩包版本
            // 目前返回一个示例逻辑
            return true;
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="message">日志消息</param>
        private void WriteLog(string message)
        {
            string logFile = Path.Combine(ApplicationDirectory, "update_log.txt");
            try
            {
                using (StreamWriter writer = new StreamWriter(logFile, true))
                {
                    writer.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}] {message}");
                }
            }
            catch { }
        }
    }

    /// <summary>
    /// 版本信息类
    /// 用于序列化和反序列化版本信息XML
    /// </summary>
    [Serializable]
    public class VersionInfo
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// 包含的文件列表
        /// </summary>
        public List<string> Files { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public VersionInfo()
        {
            Files = new List<string>();
        }
    }
}