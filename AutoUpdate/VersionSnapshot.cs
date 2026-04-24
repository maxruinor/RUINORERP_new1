using System;
using System.Collections.Generic;
using System.IO;

namespace AutoUpdate
{
    /// <summary>
    /// 快照类型枚举
    /// </summary>
    public enum SnapshotType
    {
        /// <summary>
        /// 完整快照 - 包含所有文件
        /// </summary>
        Full,
        
        /// <summary>
        /// 增量快照 - 只包含变化的文件
        /// </summary>
        Incremental
    }
    
    /// <summary>
    /// 文件变更类型
    /// </summary>
    public enum ChangeType
    {
        /// <summary>
        /// 文件被修改
        /// </summary>
        Modified,
        
        /// <summary>
        /// 新增文件
        /// </summary>
        Added,
        
        /// <summary>
        /// 删除文件
        /// </summary>
        Deleted
    }
    
    /// <summary>
    /// 文件变更信息
    /// </summary>
    public class FileChangeInfo
    {
        /// <summary>
        /// 文件相对路径
        /// </summary>
        public string FileName { get; set; }
        
        /// <summary>
        /// 文件大小（字节）
        /// </summary>
        public long Size { get; set; }
        
        /// <summary>
        /// 文件校验和
        /// </summary>
        public string Checksum { get; set; }
        
        /// <summary>
        /// 变更类型
        /// </summary>
        public ChangeType ChangeType { get; set; }
    }
    
    /// <summary>
    /// 应用版本快照
    /// 代表某个时间点整个应用的完整状态
    /// </summary>
    public class VersionSnapshot
    {
        /// <summary>
        /// 应用版本号（如 1.0.0.5）
        /// </summary>
        public string AppVersion { get; set; }
        
        /// <summary>
        /// 安装时间
        /// </summary>
        public DateTime InstallTime { get; set; }
        
        /// <summary>
        /// 快照文件夹名称（如 v1.0.0.5_20260408103000）
        /// </summary>
        public string SnapshotFolderName { get; set; }
        
        /// <summary>
        /// 快照类型
        /// </summary>
        public SnapshotType Type { get; set; }
        
        /// <summary>
        /// 基准版本文件夹名称（仅增量快照使用）
        /// </summary>
        public string BaseVersionFolderName { get; set; }
        
        /// <summary>
        /// 文件数量
        /// </summary>
        public int FileCount { get; set; }
        
        /// <summary>
        /// 总大小（字节）- 完整快照为实际大小，增量快照为基准版本大小
        /// </summary>
        public long TotalSizeBytes { get; set; }
        
        /// <summary>
        /// 增量大小（字节）- 仅增量快照使用
        /// </summary>
        public long IncrementalSizeBytes { get; set; }
        
        /// <summary>
        /// 整个快照的 SHA256 校验和
        /// </summary>
        public string Checksum { get; set; }
        
        /// <summary>
        /// 版本描述（可选）
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// 变化的文件列表（仅增量快照使用）
        /// </summary>
        public List<FileChangeInfo> ChangedFiles { get; set; } = new List<FileChangeInfo>();
        
        /// <summary>
        /// 删除的文件列表（仅增量快照使用）
        /// </summary>
        public List<string> DeletedFiles { get; set; } = new List<string>();
        
        /// <summary>
        /// 版本根目录
        /// </summary>
        private static string VersionsRootDir => 
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Versions");
        
        /// <summary>
        /// 快照文件夹完整路径
        /// </summary>
        public string SnapshotFolderPath 
        { 
            get => Path.Combine(VersionsRootDir, SnapshotFolderName); 
        }
        
        /// <summary>
        /// 是否是完整快照
        /// </summary>
        public bool IsFullSnapshot => Type == SnapshotType.Full;
        
        /// <summary>
        /// 显示名称（用于UI）
        /// </summary>
        public string DisplayName => $"{AppVersion} ({InstallTime:yyyy-MM-dd HH:mm})";
        
        /// <summary>
        /// 显示大小
        /// </summary>
        public string DisplaySize
        {
            get
            {
                if (IsFullSnapshot)
                    return $"{TotalSizeBytes / 1024 / 1024} MB (完整)";
                else
                    return $"{IncrementalSizeBytes / 1024} KB (增量)";
            }
        }
    }
    
    /// <summary>
    /// 版本历史统计信息
    /// </summary>
    public class VersionHistoryStats
    {
        /// <summary>
        /// 总版本数量
        /// </summary>
        public int TotalVersionCount { get; set; }
        
        /// <summary>
        /// 完整快照数量
        /// </summary>
        public int FullSnapshotCount { get; set; }
        
        /// <summary>
        /// 增量快照数量
        /// </summary>
        public int IncrementalSnapshotCount { get; set; }
        
        /// <summary>
        /// 当前总大小（字节）
        /// </summary>
        public long TotalSizeBytes { get; set; }
        
        /// <summary>
        /// 最大限制（字节）
        /// </summary>
        public long MaxSizeBytes { get; set; }
        
        /// <summary>
        /// 使用百分比
        /// </summary>
        public double UsagePercentage { get; set; }
        
        /// <summary>
        /// 显示大小
        /// </summary>
        public string DisplayTotalSize => $"{TotalSizeBytes / 1024 / 1024} MB";
        
        /// <summary>
        /// 显示最大限制
        /// </summary>
        public string DisplayMaxSize => $"{MaxSizeBytes / 1024 / 1024} MB";
        
        /// <summary>
        /// 显示使用百分比
        /// </summary>
        public string DisplayUsage => $"{UsagePercentage:F1}%";
    }
}
