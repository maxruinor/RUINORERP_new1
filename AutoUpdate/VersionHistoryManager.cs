using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Diagnostics;

namespace AutoUpdate
{
    /// <summary>
    /// 版本历史管理器
    /// 负责记录和管理应用程序的版本更新历史，支持版本回滚功能
    /// </summary>
    public class VersionHistoryManager
    {
        /// <summary>
        /// 版本历史记录文件路径
        /// </summary>
        private string HistoryFilePath { get; set; }

        /// <summary>
        /// 版本历史记录列表
        /// </summary>
        public List<VersionEntry> VersionHistory { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="historyFilePath">历史记录文件路径，默认为应用程序目录下的VersionHistory.xml</param>
        public VersionHistoryManager(string historyFilePath = null)
        {
            // 如果未指定历史文件路径，则使用默认路径
            HistoryFilePath = historyFilePath ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VersionHistory.xml");
            VersionHistory = new List<VersionEntry>();
            LoadVersionHistory();
        }

        /// <summary>
        /// 加载版本历史记录
        /// </summary>
        private void LoadVersionHistory()
        {
            try
            {
                if (File.Exists(HistoryFilePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<VersionEntry>));
                    using (StreamReader reader = new StreamReader(HistoryFilePath))
                    {
                        VersionHistory = (List<VersionEntry>)serializer.Deserialize(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"加载版本历史记录失败: {ex.Message}");
                // 如果加载失败，使用空列表
                VersionHistory = new List<VersionEntry>();
            }
        }

        /// <summary>
        /// 保存版本历史记录
        /// </summary>
        private void SaveVersionHistory()
        {
            try
            {
                // 确保目录存在
                Directory.CreateDirectory(Path.GetDirectoryName(HistoryFilePath));
                
                XmlSerializer serializer = new XmlSerializer(typeof(List<VersionEntry>));
                using (StreamWriter writer = new StreamWriter(HistoryFilePath))
                {
                    serializer.Serialize(writer, VersionHistory);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"保存版本历史记录失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 记录新版本安装
        /// </summary>
        /// <param name="version">版本号</param>
        /// <param name="folderName">版本文件夹名称</param>
        /// <param name="files">版本包含的文件列表</param>
        /// <param name="checksum">版本完整性校验和</param>
        /// <param name="installTime">安装时间，默认为当前时间</param>
        public void RecordNewVersion(string version, string folderName, List<string> files = null, string checksum = null, DateTime? installTime = null)
        {
            try
            {
                // 确保版本号唯一
                var existingVersion = VersionHistory.FirstOrDefault(v => v.Version == version);
                DateTime now = installTime ?? DateTime.Now;
                
                if (existingVersion != null)
                {
                    // 更新现有版本信息
                    existingVersion.InstallTime = now;
                    existingVersion.FolderName = folderName;
                    existingVersion.Files = files ?? new List<string>();
                    existingVersion.Checksum = checksum;
                }
                else
                {
                    // 添加新版本记录
                    VersionEntry newVersion = new VersionEntry
                    {
                        Version = version,
                        InstallTime = now,
                        FolderName = folderName,
                        Files = files ?? new List<string>(),
                        Checksum = checksum
                    };
                    VersionHistory.Add(newVersion);
                }

                // 保存历史记录
                SaveVersionHistory();
                
                // 清理旧版本，只保留最新的5个版本
                CleanupOldVersions();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"记录新版本失败: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 清理旧版本
        /// </summary>
        /// <param name="maxVersions">最多保留的版本数量，默认为5</param>
        public void CleanupOldVersions(int maxVersions = 5)
        {
            try
            {
                // 按安装时间排序，最新版本在前
                var sortedVersions = VersionHistory.OrderByDescending(v => v.InstallTime).ToList();
                
                // 如果版本数量超过最大值，删除最旧的版本
                if (sortedVersions.Count > maxVersions)
                {
                    // 需要删除的版本数量
                    int versionsToDelete = sortedVersions.Count - maxVersions;
                    
                    // 获取需要删除的版本（最旧的）
                    var versionsToRemove = sortedVersions.Skip(maxVersions).ToList();
                    
                    // 逐个删除旧版本
                    foreach (var versionToRemove in versionsToRemove)
                    {
                        // 删除版本文件夹
                        DeleteVersionFolder(versionToRemove);
                        
                        // 从历史记录中移除
                        VersionHistory.Remove(versionToRemove);
                    }
                    
                    // 保存更新后的历史记录
                    SaveVersionHistory();
                    Debug.WriteLine($"成功清理了 {versionsToDelete} 个旧版本，当前保留 {maxVersions} 个版本");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"清理旧版本失败: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 删除版本文件夹
        /// </summary>
        /// <param name="versionEntry">要删除的版本条目</param>
        private void DeleteVersionFolder(VersionEntry versionEntry)
        {
            try
            {
                string versionFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Versions", versionEntry.FolderName);
                if (Directory.Exists(versionFolderPath))
                {
                    Directory.Delete(versionFolderPath, true);
                    Debug.WriteLine($"成功删除版本文件夹: {versionFolderPath}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"删除版本文件夹失败: {versionEntry.FolderName}, 错误: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 获取版本文件夹路径
        /// </summary>
        /// <param name="version">版本号</param>
        /// <returns>版本文件夹路径，如果版本不存在则返回null</returns>
        public string GetVersionFolderPath(string version)
        {
            try
            {
                var versionEntry = VersionHistory.FirstOrDefault(v => v.Version == version);
                if (versionEntry != null && !string.IsNullOrEmpty(versionEntry.FolderName))
                {
                    return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Versions", versionEntry.FolderName);
                }
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"获取版本文件夹路径失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 获取所有历史版本（按安装时间倒序）
        /// </summary>
        /// <returns>版本历史记录列表</returns>
        public List<VersionEntry> GetAllVersions()
        {            
            return VersionHistory.OrderByDescending(v => v.InstallTime).ToList();
        }

        /// <summary>
        /// 获取当前安装的版本
        /// </summary>
        /// <returns>当前版本信息，如果没有记录则返回null</returns>
        public VersionEntry GetCurrentVersion()
        {            
            return VersionHistory.OrderByDescending(v => v.InstallTime).FirstOrDefault();
        }

        /// <summary>
        /// 检查是否有可回滚的版本
        /// </summary>
        /// <returns>如果有可回滚的版本则返回true，否则返回false</returns>
        public bool HasRollbackVersions()
        {            
            return VersionHistory.Count > 1;
        }
    }

    /// <summary>
    /// 版本条目类
    /// 用于存储单个版本的信息
    /// </summary>
    [Serializable]
    public class VersionEntry
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 安装时间
        /// </summary>
        public DateTime InstallTime { get; set; }
        
        /// <summary>
        /// 版本文件夹名称
        /// </summary>
        public string FolderName { get; set; }
        
        /// <summary>
        /// 版本包含的文件列表
        /// </summary>
        public List<string> Files { get; set; }
        
        /// <summary>
        /// 版本完整性校验和
        /// </summary>
        public string Checksum { get; set; }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public VersionEntry()
        {
            Files = new List<string>();
        }
    }
}