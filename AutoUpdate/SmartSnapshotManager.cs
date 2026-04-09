using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;
using Newtonsoft.Json;

namespace AutoUpdate
{
    /// <summary>
    /// 智能快照管理器
    /// 负责创建和管理应用版本的完整/增量快照，支持版本回滚
    /// </summary>
    public class SmartSnapshotManager
    {
        /// <summary>
        /// 每 N 个版本创建一次完整快照
        /// </summary>
        private const int FULL_SNAPSHOT_INTERVAL = 3;
        
        /// <summary>
        /// 最多保留的版本数量
        /// </summary>
        private const int MAX_VERSIONS_TO_KEEP = 10;
        
        /// <summary>
        /// 版本根目录
        /// </summary>
        private string VersionsRootDir => 
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Versions");
        
        /// <summary>
        /// 历史记录文件路径
        /// </summary>
        private string HistoryFilePath => 
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SnapshotHistory.json");
        
        /// <summary>
        /// 需要排除的目录
        /// </summary>
        private static readonly string[] ExcludedDirs = new[] 
        { 
            "Versions", "Backup", "Backup_*", "temp", "tmp", "Logs", "UpdaterData" 
        };
        
        /// <summary>
        /// 需要排除的文件
        /// </summary>
        private static readonly string[] ExcludedFiles = new[]
        {
            "AutoUpdaterList.xml", "*.lock", "*.tmp"
        };

        #region 快照创建

        /// <summary>
        /// 创建智能快照（自动判断完整或增量）
        /// </summary>
        /// <param name="appVersion">应用版本号</param>
        /// <param name="description">版本描述</param>
        /// <returns>创建的快照对象</returns>
        public VersionSnapshot CreateSmartSnapshot(string appVersion, string description = "")
        {
            try
            {
                var allSnapshots = GetAllSnapshots();
                
                // 判断是否需要创建完整快照
                bool shouldCreateFull = ShouldCreateFullSnapshot(allSnapshots);
                
                if (shouldCreateFull)
                {
                    Debug.WriteLine("[SmartSnapshot] 创建完整快照...");
                    return CreateFullSnapshot(appVersion, description);
                }
                else
                {
                    Debug.WriteLine("[SmartSnapshot] 创建增量快照...");
                    var latestSnapshot = allSnapshots.FirstOrDefault();
                    return CreateIncrementalSnapshot(appVersion, latestSnapshot, description);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[SmartSnapshot] 创建快照失败: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
                return null;
            }
        }
        
        /// <summary>
        /// 判断是否应该创建完整快照
        /// </summary>
        private bool ShouldCreateFullSnapshot(List<VersionSnapshot> allSnapshots)
        {
            if (allSnapshots == null || allSnapshots.Count == 0)
                return true; // 第一个版本必须完整
            
            // 计算自上次完整快照以来的增量版本数
            int incrementalCount = 0;
            foreach (var snapshot in allSnapshots)
            {
                if (snapshot.IsFullSnapshot)
                    break;
                incrementalCount++;
            }
            
            // 如果连续增量版本达到阈值，创建完整快照
            return incrementalCount >= FULL_SNAPSHOT_INTERVAL - 1;
        }
        
        /// <summary>
        /// 创建完整快照
        /// </summary>
        private VersionSnapshot CreateFullSnapshot(string appVersion, string description)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string folderName = $"v{appVersion}_{timestamp}";
            string snapshotPath = Path.Combine(VersionsRootDir, folderName);
            
            Debug.WriteLine($"[SmartSnapshot] 创建完整快照: {folderName}");
            
            // 复制整个应用目录
            CopyApplicationToSnapshot(snapshotPath);
            
            // 计算快照信息
            var files = Directory.GetFiles(snapshotPath, "*.*", SearchOption.AllDirectories);
            long totalSize = files.Sum(f => new FileInfo(f).Length);
            string checksum = CalculateSnapshotChecksum(snapshotPath);
            
            var snapshot = new VersionSnapshot
            {
                AppVersion = appVersion,
                InstallTime = DateTime.Now,
                SnapshotFolderName = folderName,
                Type = SnapshotType.Full,
                FileCount = files.Length,
                TotalSizeBytes = totalSize,
                IncrementalSizeBytes = 0,
                Checksum = checksum,
                Description = description,
                ChangedFiles = new List<FileChangeInfo>(),
                DeletedFiles = new List<string>()
            };
            
            // 保存到历史记录
            SaveToHistory(snapshot);
            
            // 清理旧版本
            CleanupOldSnapshots();
            
            Debug.WriteLine($"[SmartSnapshot] 完整快照创建成功: {files.Length} 个文件, {totalSize / 1024 / 1024} MB");
            
            return snapshot;
        }
        
        /// <summary>
        /// 创建增量快照
        /// </summary>
        private VersionSnapshot CreateIncrementalSnapshot(
            string appVersion, 
            VersionSnapshot baseSnapshot, 
            string description)
        {
            if (baseSnapshot == null)
            {
                Debug.WriteLine("[SmartSnapshot] 没有基准版本，创建完整快照");
                return CreateFullSnapshot(appVersion, description);
            }
            
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string folderName = $"v{appVersion}_{timestamp}";
            string snapshotPath = Path.Combine(VersionsRootDir, folderName);
            Directory.CreateDirectory(snapshotPath);
            
            Debug.WriteLine($"[SmartSnapshot] 创建增量快照: {folderName}, 基准: {baseSnapshot.SnapshotFolderName}");
            
            string appRoot = AppDomain.CurrentDomain.BaseDirectory;
            string baseSnapshotPath = baseSnapshot.SnapshotFolderPath;
            
            // 比较当前应用与基准版本，找出变化的文件
            var changedFiles = CompareAndCopyChangedFiles(appRoot, baseSnapshotPath, snapshotPath);
            
            // 创建元数据文件
            var metadata = new
            {
                SnapshotType = "Incremental",
                BaseVersion = baseSnapshot.SnapshotFolderName,
                AppVersion = appVersion,
                InstallTime = DateTime.Now.ToString("o"),
                ChangedFiles = changedFiles.Select(f => new
                {
                    f.FileName,
                    f.Size,
                    f.Checksum,
                    ChangeType = f.ChangeType.ToString()
                }).ToList(),
                DeletedFiles = changedFiles.Where(f => f.ChangeType == ChangeType.Deleted)
                                          .Select(f => f.FileName).ToList(),
                TotalChangedFiles = changedFiles.Count(f => f.ChangeType != ChangeType.Deleted),
                TotalDeletedFiles = changedFiles.Count(f => f.ChangeType == ChangeType.Deleted),
                IncrementalSize = changedFiles.Where(f => f.ChangeType != ChangeType.Deleted)
                                             .Sum(f => f.Size)
            };
            
            string metadataPath = Path.Combine(snapshotPath, ".metadata.json");
            File.WriteAllText(metadataPath, JsonConvert.SerializeObject(metadata, Formatting.Indented));
            
            long incrementalSize = changedFiles.Where(f => f.ChangeType != ChangeType.Deleted)
                                              .Sum(f => f.Size);
            
            var snapshot = new VersionSnapshot
            {
                AppVersion = appVersion,
                InstallTime = DateTime.Now,
                SnapshotFolderName = folderName,
                Type = SnapshotType.Incremental,
                BaseVersionFolderName = baseSnapshot.SnapshotFolderName,
                FileCount = changedFiles.Count,
                TotalSizeBytes = baseSnapshot.TotalSizeBytes,
                IncrementalSizeBytes = incrementalSize,
                Checksum = CalculateIncrementalChecksum(changedFiles),
                Description = description,
                ChangedFiles = changedFiles,
                DeletedFiles = changedFiles.Where(f => f.ChangeType == ChangeType.Deleted)
                                          .Select(f => f.FileName).ToList()
            };
            
            SaveToHistory(snapshot);
            CleanupOldSnapshots();
            
            Debug.WriteLine($"[SmartSnapshot] 增量快照创建成功: {changedFiles.Count} 个变更, {incrementalSize / 1024} KB");
            
            return snapshot;
        }
        
        /// <summary>
        /// 复制整个应用到快照目录
        /// </summary>
        private void CopyApplicationToSnapshot(string snapshotPath)
        {
            string appRoot = AppDomain.CurrentDomain.BaseDirectory;
            CopyDirectoryRecursive(appRoot, snapshotPath, ExcludedDirs);
        }
        
        /// <summary>
        /// 递归复制目录（排除特定目录和文件）
        /// </summary>
        private void CopyDirectoryRecursive(string source, string dest, string[] excludeDirs)
        {
            Directory.CreateDirectory(dest);
            
            // 复制文件
            foreach (string file in Directory.GetFiles(source))
            {
                if (IsExcludedFile(file))
                    continue;
                
                string destFile = Path.Combine(dest, Path.GetFileName(file));
                
                try
                {
                    File.Copy(file, destFile, true);
                }
                catch (IOException ex)
                {
                    // 文件被锁定，跳过
                    Debug.WriteLine($"[SmartSnapshot] 跳过锁定文件: {file}, {ex.Message}");
                }
            }
            
            // 递归复制子目录
            foreach (string dir in Directory.GetDirectories(source))
            {
                string dirName = Path.GetFileName(dir);
                
                // 排除指定目录
                if (excludeDirs.Any(ex => dirName.Equals(ex, StringComparison.OrdinalIgnoreCase) ||
                                         System.Text.RegularExpressions.Regex.IsMatch(dirName, ex.Replace("*", ".*"))))
                    continue;
                
                string destDir = Path.Combine(dest, dirName);
                CopyDirectoryRecursive(dir, destDir, excludeDirs);
            }
        }
        
        /// <summary>
        /// 检查文件是否应该排除
        /// </summary>
        private bool IsExcludedFile(string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            
            // 检查排除的文件名
            foreach (var pattern in ExcludedFiles)
            {
                if (pattern.Contains("*"))
                {
                    // 通配符匹配
                    var regex = new System.Text.RegularExpressions.Regex(
                        "^" + System.Text.RegularExpressions.Regex.Escape(pattern).Replace("\\*", ".*") + "$");
                    if (regex.IsMatch(fileName))
                        return true;
                }
                else if (fileName.Equals(pattern, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            
            return false;
        }
        
        /// <summary>
        /// 比较并复制变化的文件
        /// </summary>
        private List<FileChangeInfo> CompareAndCopyChangedFiles(
            string currentAppPath, 
            string baseSnapshotPath, 
            string incrementalPath)
        {
            var changes = new List<FileChangeInfo>();
            
            // 获取当前应用的所有文件
            var currentFiles = Directory.GetFiles(currentAppPath, "*.*", SearchOption.AllDirectories)
                .Where(f => !IsExcludedFile(f))
                .ToList();
            
            // 获取基准版本的所有文件
            var baseFiles = Directory.GetFiles(baseSnapshotPath, "*.*", SearchOption.AllDirectories)
                .Where(f => !Path.GetFileName(f).StartsWith("."))
                .ToDictionary(
                    f => GetRelativePath(f, baseSnapshotPath),
                    f => new FileInfo(f)
                );
            
            using (var sha256 = SHA256.Create())
            {
                foreach (string currentFile in currentFiles)
                {
                    string relativePath = GetRelativePath(currentFile, currentAppPath);
                    
                    // 检查文件是否在基准版本中存在
                    if (baseFiles.ContainsKey(relativePath))
                    {
                        // 文件存在，检查是否变化
                        var baseFileInfo = baseFiles[relativePath];
                        string currentChecksum = CalculateFileChecksum(currentFile, sha256);
                        string baseChecksum = CalculateFileChecksum(baseFileInfo.FullName, sha256);
                        
                        if (currentChecksum != baseChecksum)
                        {
                            // 文件已修改
                            var changeInfo = new FileChangeInfo
                            {
                                FileName = relativePath,
                                Size = new FileInfo(currentFile).Length,
                                Checksum = currentChecksum,
                                ChangeType = ChangeType.Modified
                            };
                            
                            changes.Add(changeInfo);
                            
                            // 复制变化的文件到增量目录
                            string destPath = Path.Combine(incrementalPath, relativePath);
                            Directory.CreateDirectory(Path.GetDirectoryName(destPath));
                            File.Copy(currentFile, destPath, true);
                        }
                    }
                    else
                    {
                        // 新文件
                        var changeInfo = new FileChangeInfo
                        {
                            FileName = relativePath,
                            Size = new FileInfo(currentFile).Length,
                            Checksum = CalculateFileChecksum(currentFile, sha256),
                            ChangeType = ChangeType.Added
                        };
                        
                        changes.Add(changeInfo);
                        
                        // 复制新文件
                        string destPath = Path.Combine(incrementalPath, relativePath);
                        Directory.CreateDirectory(Path.GetDirectoryName(destPath));
                        File.Copy(currentFile, destPath, true);
                    }
                }
            }
            
            // 检查删除的文件
            var currentRelativePaths = currentFiles
                .Select(f => GetRelativePath(f, currentAppPath))
                .ToHashSet();
            
            foreach (var baseFile in baseFiles)
            {
                if (!currentRelativePaths.Contains(baseFile.Key))
                {
                    changes.Add(new FileChangeInfo
                    {
                        FileName = baseFile.Key,
                        Size = 0,
                        Checksum = "",
                        ChangeType = ChangeType.Deleted
                    });
                }
            }
            
            return changes;
        }
        
        /// <summary>
        /// 获取相对路径
        /// </summary>
        private string GetRelativePath(string fullPath, string basePath)
        {
            if (fullPath.StartsWith(basePath))
            {
                return fullPath.Substring(basePath.Length + 1);
            }
            return fullPath;
        }
        
        /// <summary>
        /// 计算文件的 SHA256 校验和
        /// </summary>
        private string CalculateFileChecksum(string filePath, SHA256 sha256)
        {
            try
            {
                byte[] fileBytes = File.ReadAllBytes(filePath);
                byte[] hash = sha256.ComputeHash(fileBytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
            catch
            {
                return "";
            }
        }
        
        /// <summary>
        /// 计算整个快照的校验和
        /// </summary>
        private string CalculateSnapshotChecksum(string snapshotPath)
        {
            using (var sha256 = SHA256.Create())
            {
                var files = Directory.GetFiles(snapshotPath, "*.*", SearchOption.AllDirectories)
                    .OrderBy(f => f)
                    .ToList();
                
                foreach (string file in files)
                {
                    byte[] fileBytes = File.ReadAllBytes(file);
                    sha256.TransformBlock(fileBytes, 0, fileBytes.Length, null, 0);
                }
                
                sha256.TransformFinalBlock(new byte[0], 0, 0);
                return BitConverter.ToString(sha256.Hash).Replace("-", "").ToLower();
            }
        }
        
        /// <summary>
        /// 计算增量快照的校验和
        /// </summary>
        private string CalculateIncrementalChecksum(List<FileChangeInfo> changedFiles)
        {
            using (var sha256 = SHA256.Create())
            {
                foreach (var file in changedFiles.OrderBy(f => f.FileName))
                {
                    byte[] bytes = Encoding.UTF8.GetBytes($"{file.FileName}:{file.Checksum}");
                    sha256.TransformBlock(bytes, 0, bytes.Length, null, 0);
                }
                
                sha256.TransformFinalBlock(new byte[0], 0, 0);
                return BitConverter.ToString(sha256.Hash).Replace("-", "").ToLower();
            }
        }

        #endregion

        #region 快照回滚

        /// <summary>
        /// 回滚到指定快照
        /// </summary>
        /// <param name="snapshotFolderName">快照文件夹名称</param>
        /// <returns>回滚是否成功</returns>
        public bool RollbackToSnapshot(string snapshotFolderName)
        {
            var targetSnapshot = GetAllSnapshots()
                .FirstOrDefault(s => s.SnapshotFolderName == snapshotFolderName);
            
            if (targetSnapshot == null)
            {
                Debug.WriteLine($"[SmartSnapshot] 快照不存在: {snapshotFolderName}");
                return false;
            }
            
            string appRoot = AppDomain.CurrentDomain.BaseDirectory;
            string backupPath = Path.Combine(appRoot, $"Backup_{DateTime.Now:yyyyMMddHHmmss}");
            
            try
            {
                // 1. 备份当前版本
                Debug.WriteLine("[SmartSnapshot] 正在备份当前版本...");
                BackupCurrentApplication(appRoot, backupPath);
                
                // 2. 清空当前应用目录
                Debug.WriteLine("[SmartSnapshot] 正在清空应用目录...");
                ClearApplicationDirectory(appRoot);
                
                // 3. 恢复快照
                if (targetSnapshot.IsFullSnapshot)
                {
                    // 完整快照：直接复制
                    Debug.WriteLine($"[SmartSnapshot] 从完整快照恢复: {snapshotFolderName}");
                    CopyDirectoryRecursive(targetSnapshot.SnapshotFolderPath, appRoot, 
                        new[] { ".metadata.json" });
                }
                else
                {
                    // 增量快照：先恢复到基准版本，再应用增量
                    Debug.WriteLine($"[SmartSnapshot] 从增量快照恢复: {snapshotFolderName}");
                    RestoreFromIncrementalSnapshot(targetSnapshot, appRoot);
                }
                
                // 4. 验证
                if (!VerifyRollback(targetSnapshot, appRoot))
                {
                    Debug.WriteLine("[SmartSnapshot] 回滚验证失败，恢复备份");
                    RestoreFromBackup(backupPath, appRoot);
                    return false;
                }
                
                Debug.WriteLine("[SmartSnapshot] 回滚成功");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[SmartSnapshot] 回滚失败: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
                
                // 尝试恢复备份
                try 
                { 
                    RestoreFromBackup(backupPath, appRoot); 
                } 
                catch { }
                
                return false;
            }
            finally
            {
                // 清理备份
                if (Directory.Exists(backupPath))
                {
                    try { Directory.Delete(backupPath, true); } 
                    catch { }
                }
            }
        }
        
        /// <summary>
        /// 从增量快照恢复
        /// </summary>
        private void RestoreFromIncrementalSnapshot(VersionSnapshot snapshot, string appRoot)
        {
            // 1. 找到基准版本
            var baseSnapshot = GetAllSnapshots()
                .FirstOrDefault(s => s.SnapshotFolderName == snapshot.BaseVersionFolderName);
            
            if (baseSnapshot == null)
            {
                throw new InvalidOperationException($"找不到基准版本: {snapshot.BaseVersionFolderName}");
            }
            
            // 2. 如果基准版本也是增量，递归恢复
            if (!baseSnapshot.IsFullSnapshot)
            {
                Debug.WriteLine($"[SmartSnapshot] 递归恢复基准版本: {baseSnapshot.SnapshotFolderName}");
                RestoreFromIncrementalSnapshot(baseSnapshot, appRoot);
            }
            else
            {
                // 3. 从完整基准版本恢复
                Debug.WriteLine($"[SmartSnapshot] 从完整基准版本恢复: {baseSnapshot.SnapshotFolderName}");
                CopyDirectoryRecursive(baseSnapshot.SnapshotFolderPath, appRoot, 
                    new[] { ".metadata.json" });
            }
            
            // 4. 应用增量变化
            string incrementalPath = snapshot.SnapshotFolderPath;
            
            // 4.1 复制新增/修改的文件
            foreach (var change in snapshot.ChangedFiles)
            {
                if (change.ChangeType == ChangeType.Deleted)
                    continue;
                
                string sourceFile = Path.Combine(incrementalPath, change.FileName);
                string destFile = Path.Combine(appRoot, change.FileName);
                
                if (File.Exists(sourceFile))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(destFile));
                    File.Copy(sourceFile, destFile, true);
                    Debug.WriteLine($"[SmartSnapshot] 恢复文件: {change.FileName}");
                }
            }
            
            // 4.2 删除已删除的文件
            foreach (var deletedFile in snapshot.DeletedFiles)
            {
                string filePath = Path.Combine(appRoot, deletedFile);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Debug.WriteLine($"[SmartSnapshot] 删除文件: {deletedFile}");
                }
            }
        }
        
        /// <summary>
        /// 备份当前应用
        /// </summary>
        private void BackupCurrentApplication(string appRoot, string backupPath)
        {
            Directory.CreateDirectory(backupPath);
            CopyDirectoryRecursive(appRoot, backupPath, ExcludedDirs);
        }
        
        /// <summary>
        /// 清空应用目录（保留 Versions 和 Backup）
        /// </summary>
        private void ClearApplicationDirectory(string appRoot)
        {
            foreach (string file in Directory.GetFiles(appRoot))
            {
                if (IsExcludedFile(file))
                    continue;
                
                try
                {
                    File.Delete(file);
                }
                catch (IOException)
                {
                    // 文件被锁定，重命名
                    string tempPath = file + ".old";
                    if (File.Exists(tempPath))
                        File.Delete(tempPath);
                    File.Move(file, tempPath);
                }
            }
            
            foreach (string dir in Directory.GetDirectories(appRoot))
            {
                string dirName = Path.GetFileName(dir);
                if (ExcludedDirs.Any(ex => dirName.Equals(ex, StringComparison.OrdinalIgnoreCase) ||
                                          System.Text.RegularExpressions.Regex.IsMatch(dirName, ex.Replace("*", ".*"))))
                    continue;
                
                try
                {
                    Directory.Delete(dir, true);
                }
                catch
                {
                    // 目录被锁定，跳过
                }
            }
        }
        
        /// <summary>
        /// 从备份恢复
        /// </summary>
        private void RestoreFromBackup(string backupPath, string appRoot)
        {
            if (!Directory.Exists(backupPath))
                return;
            
            CopyDirectoryRecursive(backupPath, appRoot, new string[0]);
        }
        
        /// <summary>
        /// 验证回滚结果
        /// </summary>
        private bool VerifyRollback(VersionSnapshot snapshot, string appRoot)
        {
            // 检查关键文件是否存在
            string[] criticalFiles = new[] { "AutoUpdate.exe" };
            
            foreach (string file in criticalFiles)
            {
                string filePath = Path.Combine(appRoot, file);
                if (!File.Exists(filePath))
                {
                    Debug.WriteLine($"[SmartSnapshot] 关键文件缺失: {file}");
                    return false;
                }
            }
            
            return true;
        }

        #endregion

        #region 历史记录管理

        /// <summary>
        /// 获取所有可用快照
        /// </summary>
        public List<VersionSnapshot> GetAllSnapshots()
        {
            if (!File.Exists(HistoryFilePath))
                return new List<VersionSnapshot>();
            
            try
            {
                string json = File.ReadAllText(HistoryFilePath);
                var snapshots = JsonConvert.DeserializeObject<List<VersionSnapshot>>(json);
                return snapshots?.OrderByDescending(s => s.InstallTime).ToList() ?? new List<VersionSnapshot>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[SmartSnapshot] 加载历史记录失败: {ex.Message}");
                return new List<VersionSnapshot>();
            }
        }
        
        /// <summary>
        /// 保存快照到历史记录
        /// </summary>
        private void SaveToHistory(VersionSnapshot snapshot)
        {
            var snapshots = GetAllSnapshots();
            snapshots.Insert(0, snapshot);
            
            string json = JsonConvert.SerializeObject(snapshots, Formatting.Indented);
            File.WriteAllText(HistoryFilePath, json);
            
            Debug.WriteLine($"[SmartSnapshot] 快照已保存到历史记录: {snapshot.SnapshotFolderName}");
        }
        
        /// <summary>
        /// 清理旧快照，保留最新的 N 个
        /// </summary>
        public void CleanupOldSnapshots(int keepCount = MAX_VERSIONS_TO_KEEP)
        {
            var snapshots = GetAllSnapshots();
            
            if (snapshots.Count <= keepCount)
                return;
            
            // 删除旧的快照
            var snapshotsToDelete = snapshots.Skip(keepCount).ToList();
            
            foreach (var snapshot in snapshotsToDelete)
            {
                try
                {
                    string snapshotPath = snapshot.SnapshotFolderPath;
                    
                    if (Directory.Exists(snapshotPath))
                    {
                        Directory.Delete(snapshotPath, true);
                        Debug.WriteLine($"[SmartSnapshot] 删除旧快照: {snapshot.SnapshotFolderName}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[SmartSnapshot] 删除快照失败: {ex.Message}");
                }
            }
            
            // 更新历史记录
            var keptSnapshots = snapshots.Take(keepCount).ToList();
            string json = JsonConvert.SerializeObject(keptSnapshots, Formatting.Indented);
            File.WriteAllText(HistoryFilePath, json);
            
            Debug.WriteLine($"[SmartSnapshot] 清理完成，保留 {keepCount} 个版本");
        }

        #endregion
    }
}
