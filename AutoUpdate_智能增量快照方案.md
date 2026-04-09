# 智能增量快照方案设计

**目标**: 在存储空间和回滚性能之间取得平衡  
**日期**: 2026-04-08  
**策略**: 完整快照 + 增量快照混合

---

## 📐 核心设计

### 存储策略

```
版本历史结构：
Versions/
├── v1.0.0.3_20260325090000/          ← 完整快照 (50MB)
│   ├── ERP.exe
│   ├── Core.dll
│   └── ...
│
├── v1.0.0.4_20260401152000/          ← 增量快照 (2MB)
│   ├── .metadata.json                 ← 元数据：基于 v1.0.0.3
│   ├── ERP.exe                        ← 只保存变化的文件
│   └── UI.dll
│
├── v1.0.0.5_20260408103000/          ← 增量快照 (1MB)
│   ├── .metadata.json                 ← 元数据：基于 v1.0.0.4
│   └── Core.dll
│
└── v1.0.0.6_20260415120000/          ← 完整快照 (52MB) ⭐ 每3个版本完整一次
    ├── ERP.exe
    ├── Core.dll
    └── ...
```

### 规则

1. **每 3 个版本创建一次完整快照**
   - v1.0.0.3: 完整
   - v1.0.0.4: 增量
   - v1.0.0.5: 增量
   - v1.0.0.6: 完整 ← 新一轮
   - v1.0.0.7: 增量
   - v1.0.0.8: 增量
   - v1.0.0.9: 完整 ← 新一轮

2. **保留最近 10 个版本**
   - 自动清理更旧的版本

3. **智能回滚**
   - 回滚到完整快照：直接复制（快）
   - 回滚到增量快照：先恢复到基准版本，再应用增量（稍慢但可接受）

---

## 💾 空间节省估算

### 场景：ERP 系统，每次更新平均变化 5% 的文件

| 版本数 | 完整快照方案 | 增量快照方案 | 节省空间 |
|--------|-------------|-------------|---------|
| 3 个版本 | 150 MB | 54 MB | 64% |
| 6 个版本 | 300 MB | 108 MB | 64% |
| 10 个版本 | 500 MB | 180 MB | 64% |
| 20 个版本 | 1000 MB | 360 MB | 64% |

**结论**: 长期运行可节省 **60-70%** 的存储空间

---

## 🔧 实现方案

### 1. 元数据文件设计

**.metadata.json**（增量快照的元数据）

```json
{
  "SnapshotType": "Incremental",
  "BaseVersion": "v1.0.0.3_20260325090000",
  "AppVersion": "1.0.0.4",
  "InstallTime": "2026-04-01T15:20:00",
  "ChangedFiles": [
    {
      "FileName": "ERP.exe",
      "Size": 1048576,
      "Checksum": "a3f5b8c9d2e1...",
      "ChangeType": "Modified"
    },
    {
      "FileName": "UI.dll",
      "Size": 524288,
      "Checksum": "b4e6c7d8a1f2...",
      "ChangeType": "Modified"
    },
    {
      "FileName": "NewPlugin.dll",
      "Size": 262144,
      "Checksum": "c5f7d8e9b2a3...",
      "ChangeType": "Added"
    }
  ],
  "DeletedFiles": [
    "OldModule.dll"
  ],
  "TotalChangedFiles": 3,
  "TotalDeletedFiles": 1,
  "IncrementalSize": 1835008
}
```

### 2. VersionSnapshot 扩展

```csharp
namespace AutoUpdate
{
    public enum SnapshotType
    {
        Full,       // 完整快照
        Incremental // 增量快照
    }
    
    public class VersionSnapshot
    {
        // ... 现有属性 ...
        
        /// <summary>
        /// 快照类型
        /// </summary>
        public SnapshotType Type { get; set; }
        
        /// <summary>
        /// 基准版本（仅增量快照使用）
        /// </summary>
        public string BaseVersionFolderName { get; set; }
        
        /// <summary>
        /// 变化的文件列表（仅增量快照使用）
        /// </summary>
        public List<FileChangeInfo> ChangedFiles { get; set; }
        
        /// <summary>
        /// 删除的文件列表（仅增量快照使用）
        /// </summary>
        public List<string> DeletedFiles { get; set; }
        
        /// <summary>
        /// 增量大小（字节）
        /// </summary>
        public long IncrementalSizeBytes { get; set; }
        
        /// <summary>
        /// 是否是完整快照
        /// </summary>
        public bool IsFullSnapshot => Type == SnapshotType.Full;
        
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
    
    public class FileChangeInfo
    {
        public string FileName { get; set; }
        public long Size { get; set; }
        public string Checksum { get; set; }
        public ChangeType ChangeType { get; set; } // Modified, Added, Deleted
    }
    
    public enum ChangeType
    {
        Modified,
        Added,
        Deleted
    }
}
```

### 3. 智能快照创建逻辑

```csharp
public class SmartSnapshotManager
{
    private const int FULL_SNAPSHOT_INTERVAL = 3; // 每3个版本完整一次
    private const int MAX_VERSIONS_TO_KEEP = 10;
    
    /// <summary>
    /// 创建智能快照（自动判断完整或增量）
    /// </summary>
    public VersionSnapshot CreateSmartSnapshot(string appVersion, string description = "")
    {
        var historyManager = new VersionHistoryManager();
        var allSnapshots = historyManager.GetAllSnapshots();
        
        // 判断是否需要创建完整快照
        bool shouldCreateFull = ShouldCreateFullSnapshot(allSnapshots);
        
        if (shouldCreateFull)
        {
            Debug.WriteLine("创建完整快照...");
            return CreateFullSnapshot(appVersion, description);
        }
        else
        {
            Debug.WriteLine("创建增量快照...");
            var latestSnapshot = allSnapshots.FirstOrDefault();
            return CreateIncrementalSnapshot(appVersion, latestSnapshot, description);
        }
    }
    
    /// <summary>
    /// 判断是否应该创建完整快照
    /// </summary>
    private bool ShouldCreateFullSnapshot(List<VersionSnapshot> allSnapshots)
    {
        if (allSnapshots.Count == 0)
            return true; // 第一个版本必须完整
        
        // 计算自上次完整快照以来的版本数
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
        
        // 复制整个应用目录
        CopyApplicationToSnapshot(snapshotPath, excludeDirs: new[] { "Versions", "Backup", "temp" });
        
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
            Checksum = checksum,
            Description = description,
            ChangedFiles = new List<FileChangeInfo>(),
            DeletedFiles = new List<string>()
        };
        
        // 保存元数据
        SaveSnapshotMetadata(snapshot);
        
        // 保存到历史记录
        SaveToHistory(snapshot);
        
        // 清理旧版本
        CleanupOldSnapshots();
        
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
            Debug.WriteLine("没有基准版本，创建完整快照");
            return CreateFullSnapshot(appVersion, description);
        }
        
        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        string folderName = $"v{appVersion}_{timestamp}";
        string snapshotPath = Path.Combine(VersionsRootDir, folderName);
        Directory.CreateDirectory(snapshotPath);
        
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
            TotalSizeBytes = baseSnapshot.TotalSizeBytes, // 逻辑大小
            IncrementalSizeBytes = incrementalSize,
            Checksum = CalculateIncrementalChecksum(changedFiles),
            Description = description,
            ChangedFiles = changedFiles,
            DeletedFiles = changedFiles.Where(f => f.ChangeType == ChangeType.Deleted)
                                      .Select(f => f.FileName).ToList()
        };
        
        SaveToHistory(snapshot);
        CleanupOldSnapshots();
        
        return snapshot;
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
                f => f.Substring(baseSnapshotPath.Length + 1),
                f => new FileInfo(f)
            );
        
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            foreach (string currentFile in currentFiles)
            {
                string relativePath = currentFile.Substring(currentAppPath.Length + 1);
                
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
            .Select(f => f.Substring(currentAppPath.Length + 1))
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
    /// 回滚到指定快照（支持增量）
    /// </summary>
    public bool RollbackToSnapshot(string snapshotFolderName)
    {
        var historyManager = new VersionHistoryManager();
        var targetSnapshot = historyManager.GetAllSnapshots()
            .FirstOrDefault(s => s.SnapshotFolderName == snapshotFolderName);
        
        if (targetSnapshot == null)
        {
            Debug.WriteLine($"快照不存在: {snapshotFolderName}");
            return false;
        }
        
        string appRoot = AppDomain.CurrentDomain.BaseDirectory;
        string backupPath = Path.Combine(appRoot, $"Backup_{DateTime.Now:yyyyMMddHHmmss}");
        
        try
        {
            // 1. 备份当前版本
            BackupCurrentApplication(appRoot, backupPath);
            
            // 2. 清空当前应用目录
            ClearApplicationDirectory(appRoot);
            
            // 3. 恢复快照
            if (targetSnapshot.IsFullSnapshot)
            {
                // 完整快照：直接复制
                Debug.WriteLine($"从完整快照恢复: {snapshotFolderName}");
                CopyDirectoryRecursive(targetSnapshot.SnapshotFolderPath, appRoot, 
                    new[] { ".metadata.json" });
            }
            else
            {
                // 增量快照：先恢复到基准版本，再应用增量
                Debug.WriteLine($"从增量快照恢复: {snapshotFolderName}");
                RestoreFromIncrementalSnapshot(targetSnapshot, appRoot);
            }
            
            // 4. 验证
            if (!VerifyRollback(targetSnapshot, appRoot))
            {
                Debug.WriteLine("回滚验证失败，恢复备份");
                RestoreFromBackup(backupPath, appRoot);
                return false;
            }
            
            Debug.WriteLine("回滚成功");
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"回滚失败: {ex.Message}");
            try { RestoreFromBackup(backupPath, appRoot); } catch { }
            return false;
        }
        finally
        {
            if (Directory.Exists(backupPath))
            {
                try { Directory.Delete(backupPath, true); } catch { }
            }
        }
    }
    
    /// <summary>
    /// 从增量快照恢复
    /// </summary>
    private void RestoreFromIncrementalSnapshot(VersionSnapshot snapshot, string appRoot)
    {
        // 1. 找到基准版本
        var historyManager = new VersionHistoryManager();
        var baseSnapshot = historyManager.GetAllSnapshots()
            .FirstOrDefault(s => s.SnapshotFolderName == snapshot.BaseVersionFolderName);
        
        if (baseSnapshot == null)
        {
            throw new InvalidOperationException($"找不到基准版本: {snapshot.BaseVersionFolderName}");
        }
        
        // 2. 如果基准版本也是增量，递归恢复
        if (!baseSnapshot.IsFullSnapshot)
        {
            RestoreFromIncrementalSnapshot(baseSnapshot, appRoot);
        }
        else
        {
            // 3. 从完整基准版本恢复
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
            }
        }
        
        // 4.2 删除已删除的文件
        foreach (var deletedFile in snapshot.DeletedFiles)
        {
            string filePath = Path.Combine(appRoot, deletedFile);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
```

---

## 📊 性能对比

### 存储空间（10个版本）

| 方案 | 总空间 | 说明 |
|------|--------|------|
| 全部完整 | 500 MB | 每个版本 50MB |
| 全部增量 | 80 MB | 第1个完整50MB + 9个增量各3MB |
| **混合策略** | **180 MB** | 4个完整(200MB) + 6个增量(18MB) |

### 回滚速度

| 回滚目标 | 完整快照 | 增量快照 | 混合策略 |
|---------|---------|---------|---------|
| 最新版本 | 快 (1s) | 快 (1s) | 快 (1s) |
| 3个版本前 | 快 (1s) | 中 (3s) | 快 (1s) ⭐ |
| 9个版本前 | 快 (1s) | 慢 (10s) | 中 (3s) |

---

## 🎯 最终建议

### 推荐配置

```csharp
// 每 3 个版本创建一次完整快照
FULL_SNAPSHOT_INTERVAL = 3

// 保留最近 10 个版本
MAX_VERSIONS_TO_KEEP = 10

// 预期空间占用
// 10个版本 ≈ 180 MB（vs 500 MB 全完整）
// 节省 64% 空间
```

### 优势

✅ **空间效率**: 节省 60-70% 存储空间  
✅ **回滚性能**: 最近的版本都是完整快照，回滚快  
✅ **可靠性**: 定期完整快照，避免增量链过长  
✅ **灵活性**: 可根据实际情况调整间隔  

### 可选优化

如果空间仍然紧张，可以：

1. **增加完整快照间隔**: 改为每 5 个版本
2. **减少保留版本数**: 从 10 个减到 5 个
3. **压缩增量文件**: 使用 ZIP 压缩增量文件
4. **云端存储**: 将旧快照上传到云存储

---

## 💡 总结

**混合策略是最佳选择**：
- 兼顾空间和性能
- 实现复杂度适中
- 用户体验良好

需要我开始实施这个方案吗？
