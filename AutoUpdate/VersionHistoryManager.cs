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
        /// <param name="installTime">安装时间，默认为当前时间</param>
        public void RecordNewVersion(string version, DateTime? installTime = null)
        {
            try
            {
                // 确保版本号唯一
                var existingVersion = VersionHistory.FirstOrDefault(v => v.Version == version);
                if (existingVersion != null)
                {
                    // 更新现有版本的安装时间
                    existingVersion.InstallTime = installTime ?? DateTime.Now;
                }
                else
                {
                    // 添加新版本记录
                    VersionEntry newVersion = new VersionEntry
                    {
                        Version = version,
                        InstallTime = installTime ?? DateTime.Now
                    };
                    VersionHistory.Add(newVersion);
                }

                // 保存历史记录
                SaveVersionHistory();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"记录新版本失败: {ex.Message}");
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
    }
}