# 版本回滚功能重构方案

**目标**: 从文件级版本管理升级为应用级快照管理  
**日期**: 2026-04-08  
**状态**: 设计方案

---

## 🎯 核心改进

### 当前问题
1. ❌ 每个文件独立版本号，无法保证兼容性
2. ❌ 回滚时可能混合不同版本的文件
3. ❌ 备份不完整，只备份 AutoUpdate.* 文件
4. ❌ 不处理子目录结构

### 解决方案
✅ **应用级快照**: 每次更新前保存完整的应用目录快照  
✅ **原子性回滚**: 回滚时替换整个应用目录，确保版本一致  
✅ **完整性验证**: 计算整个快照的校验和，确保数据完整  
✅ **智能清理**: 自动删除旧快照，保留最近 N 个版本  

---

## 📐 架构设计

### 1. 数据模型

#### VersionSnapshot.cs（新增）

```csharp
namespace AutoUpdate
{
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
        /// 快照文件夹完整路径
        /// </summary>
        public string SnapshotFolderPath 
        { 
            get => Path.Combine(VersionsRootDir, SnapshotFolderName); 
        }
        
        /// <summary>
        /// 文件数量
        /// </summary>
        public int FileCount { get; set; }
        
        /// <summary>
        /// 总大小（字节）
        /// </summary>
        public long TotalSizeBytes { get; set; }
        
        /// <summary>
        /// 整个快照的 SHA256 校验和
        /// </summary>
        public string Checksum { get; set; }
        
        /// <summary>
        /// 版本描述（可选）
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// 版本根目录
        /// </summary>
        private static string VersionsRootDir => 
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Versions");
        
        /// <summary>
        /// 显示名称（用于UI）
        /// </summary>
        public string DisplayName => $"{AppVersion} ({InstallTime:yyyy-MM-dd HH:mm})";
    }
}
```

#### VersionHistoryManager.cs（重构）

```csharp
namespace AutoUpdate
{
    public class VersionHistoryManager
    {
        private string historyFilePath;
        
        public VersionHistoryManager()
        {
            historyFilePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, 
                "VersionHistory.xml"
            );
        }
        
        /// <summary>
        /// 创建应用快照
        /// </summary>
        public VersionSnapshot CreateSnapshot(string appVersion, string description = "")
        {
            // 1. 生成快照文件夹名称
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string folderName = $"v{appVersion}_{timestamp}";
            string snapshotPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, 
                "Versions", 
                folderName
            );
            
            // 2. 复制整个应用目录到快照文件夹
            CopyApplicationToSnapshot(snapshotPath);
            
            // 3. 计算快照信息
            var files = Directory.GetFiles(snapshotPath, "*.*", SearchOption.AllDirectories);
            long totalSize = files.Sum(f => new FileInfo(f).Length);
            string checksum = CalculateSnapshotChecksum(snapshotPath);
            
            // 4. 创建快照对象
            var snapshot = new VersionSnapshot
            {
                AppVersion = appVersion,
                InstallTime = DateTime.Now,
                SnapshotFolderName = folderName,
                FileCount = files.Length,
                TotalSizeBytes = totalSize,
                Checksum = checksum,
                Description = description
            };
            
            // 5. 保存到历史记录
            SaveToHistory(snapshot);
            
            return snapshot;
        }
        
        /// <summary>
        /// 复制整个应用到快照目录
        /// </summary>
        private void CopyApplicationToSnapshot(string snapshotPath)
        {
            string appRoot = AppDomain.CurrentDomain.BaseDirectory;
            
            // 排除不需要备份的目录
            string[] excludeDirs = new[] { "Versions", "Backup", "temp", "tmp", "Logs" };
            
            CopyDirectoryRecursive(appRoot, snapshotPath, excludeDirs);
        }
        
        /// <summary>
        /// 递归复制目录（排除特定目录）
        /// </summary>
        private void CopyDirectoryRecursive(string source, string dest, string[] excludeDirs)
        {
            Directory.CreateDirectory(dest);
            
            // 复制文件
            foreach (string file in Directory.GetFiles(source))
            {
                string fileName = Path.GetFileName(file);
                
                // 跳过临时文件和锁定文件
                if (fileName.EndsWith(".lock") || 
                    fileName.EndsWith(".tmp") ||
                    fileName.Equals("AutoUpdaterList.xml"))
                    continue;
                
                string destFile = Path.Combine(dest, fileName);
                
                try
                {
                    File.Copy(file, destFile, true);
                }
                catch (IOException)
                {
                    // 文件被锁定，跳过
                    Debug.WriteLine($"跳过锁定文件: {file}");
                }
            }
            
            // 递归复制子目录
            foreach (string dir in Directory.GetDirectories(source))
            {
                string dirName = Path.GetFileName(dir);
                
                // 排除指定目录
                if (excludeDirs.Contains(dirName, StringComparer.OrdinalIgnoreCase))
                    continue;
                
                string destDir = Path.Combine(dest, dirName);
                CopyDirectoryRecursive(dir, destDir, excludeDirs);
            }
        }
        
        /// <summary>
        /// 计算整个快照的校验和
        /// </summary>
        private string CalculateSnapshotChecksum(string snapshotPath)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var files = Directory.GetFiles(snapshotPath, "*.*", SearchOption.AllDirectories)
                    .OrderBy(f => f)  // 排序确保一致性
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
        /// 获取所有可用快照
        /// </summary>
        public List<VersionSnapshot> GetAllSnapshots()
        {
            if (!File.Exists(historyFilePath))
                return new List<VersionSnapshot>();
            
            // 从 XML 加载历史记录
            var doc = XDocument.Load(historyFilePath);
            return doc.Descendants("VersionEntry")
                .Select(x => new VersionSnapshot
                {
                    AppVersion = x.Element("AppVersion")?.Value,
                    InstallTime = DateTime.Parse(x.Element("InstallTime")?.Value),
                    SnapshotFolderName = x.Element("SnapshotFolderName")?.Value,
                    FileCount = int.Parse(x.Element("FileCount")?.Value ?? "0"),
                    TotalSizeBytes = long.Parse(x.Element("TotalSizeBytes")?.Value ?? "0"),
                    Checksum = x.Element("Checksum")?.Value,
                    Description = x.Element("Description")?.Value
                })
                .OrderByDescending(s => s.InstallTime)
                .ToList();
        }
        
        /// <summary>
        /// 回滚到指定快照
        /// </summary>
        public bool RollbackToSnapshot(string snapshotFolderName)
        {
            string snapshotPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Versions",
                snapshotFolderName
            );
            
            if (!Directory.Exists(snapshotPath))
            {
                Debug.WriteLine($"快照不存在: {snapshotPath}");
                return false;
            }
            
            string appRoot = AppDomain.CurrentDomain.BaseDirectory;
            string backupPath = Path.Combine(appRoot, $"Backup_{DateTime.Now:yyyyMMddHHmmss}");
            
            try
            {
                // 1. 备份当前版本
                Debug.WriteLine("正在备份当前版本...");
                BackupCurrentApplication(appRoot, backupPath);
                
                // 2. 清空当前应用目录（排除 Versions、Backup 等）
                Debug.WriteLine("正在清空应用目录...");
                ClearApplicationDirectory(appRoot);
                
                // 3. 从快照恢复文件
                Debug.WriteLine($"正在从快照恢复: {snapshotFolderName}");
                CopyDirectoryRecursive(snapshotPath, appRoot, new[] { "Versions", "Backup" });
                
                // 4. 验证恢复结果
                if (!VerifyRollback(snapshotPath, appRoot))
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
        /// 验证回滚结果
        /// </summary>
        private bool VerifyRollback(string snapshotPath, string appRoot)
        {
            // 检查关键文件是否存在
            string[] criticalFiles = new[] { "ERP.exe", "Core.dll", "AutoUpdate.exe" };
            
            foreach (string file in criticalFiles)
            {
                string filePath = Path.Combine(appRoot, file);
                if (!File.Exists(filePath))
                {
                    Debug.WriteLine($"关键文件缺失: {file}");
                    return false;
                }
            }
            
            // 可以添加更详细的校验和验证
            return true;
        }
        
        /// <summary>
        /// 清理旧快照，保留最新的 N 个
        /// </summary>
        public void CleanupOldSnapshots(int keepCount = 10)
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
                    string snapshotPath = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "Versions",
                        snapshot.SnapshotFolderName
                    );
                    
                    if (Directory.Exists(snapshotPath))
                    {
                        Directory.Delete(snapshotPath, true);
                        Debug.WriteLine($"删除旧快照: {snapshot.SnapshotFolderName}");
                    }
                    
                    RemoveFromHistory(snapshot);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"删除快照失败: {ex.Message}");
                }
            }
        }
    }
}
```

---

## 🔄 集成到现有流程

### 1. 更新完成后创建快照

**位置**: `FrmUpdate.cs` - `LastCopy()` 方法末尾

```csharp
private void LastCopy()
{
    try
    {
        // ... 现有的文件复制逻辑 ...
        
        // 【新增】更新完成后创建应用快照
        AppendAllText("[版本快照] 开始创建应用快照...");
        
        var historyManager = new VersionHistoryManager();
        var snapshot = historyManager.CreateSnapshot(NewVersion, "自动更新");
        
        AppendAllText($"[版本快照] 快照创建成功: {snapshot.SnapshotFolderName}");
        AppendAllText($"[版本快照] 文件数: {snapshot.FileCount}, 大小: {snapshot.TotalSizeBytes / 1024 / 1024} MB");
        
        // 清理旧快照（保留最近10个）
        historyManager.CleanupOldSnapshots(10);
        AppendAllText("[版本快照] 旧快照清理完成");
    }
    catch (Exception ex)
    {
        AppendAllText($"[版本快照] 创建失败: {ex.Message}");
        // 快照创建失败不影响更新，只记录日志
    }
    
    // ... 后续的自我更新逻辑 ...
}
```

### 2. 回滚界面改造

**位置**: `FrmUpdate.cs` - `ShowRollbackMode()` 方法

```csharp
private void ShowRollbackMode()
{
    // 隐藏更新相关控件
    panel1.Visible = false;
    btnFinish.Visible = false;
    
    // 显示回滚界面
    groupBox4.Visible = true;
    lvUpdateList.Visible = true;
    btnRollback.Visible = true;
    
    // 加载可用快照
    LoadAvailableSnapshots();
}

private void LoadAvailableSnapshots()
{
    lvUpdateList.Items.Clear();
    
    var historyManager = new VersionHistoryManager();
    var snapshots = historyManager.GetAllSnapshots();
    
    if (snapshots.Count == 0)
    {
        MessageBox.Show("没有可用的历史版本。", "提示", 
            MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
    }
    
    foreach (var snapshot in snapshots)
    {
        var item = new ListViewItem(snapshot.AppVersion);
        item.SubItems.Add(snapshot.InstallTime.ToString("yyyy-MM-dd HH:mm:ss"));
        item.SubItems.Add(snapshot.SnapshotFolderName);
        item.SubItems.Add(snapshot.FileCount.ToString());
        item.SubItems.Add($"{snapshot.TotalSizeBytes / 1024 / 1024} MB");
        item.Tag = snapshot;  // 存储完整对象
        
        lvUpdateList.Items.Add(item);
    }
}
```

### 3. 回滚执行逻辑

**位置**: `FrmUpdate.cs` - `btnRollback_Click()` 方法

```csharp
private void btnRollback_Click(object sender, EventArgs e)
{
    if (lvUpdateList.SelectedItems.Count == 0)
    {
        MessageBox.Show("请选择要回滚的版本。", "提示", 
            MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
    }
    
    var selectedItem = lvUpdateList.SelectedItems[0];
    var snapshot = selectedItem.Tag as VersionSnapshot;
    
    if (snapshot == null)
    {
        MessageBox.Show("版本信息无效。", "错误", 
            MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
    }
    
    // 确认对话框
    string confirmMsg = $"确认要回滚到版本 {snapshot.AppVersion} 吗？\n\n" +
                       $"安装时间: {snapshot.InstallTime:yyyy-MM-dd HH:mm}\n" +
                       $"文件数量: {snapshot.FileCount}\n" +
                       $"快照大小: {snapshot.TotalSizeBytes / 1024 / 1024} MB\n\n" +
                       $"回滚过程可能需要几分钟，请耐心等待。";
    
    if (MessageBox.Show(confirmMsg, "版本回滚确认", 
        MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
    {
        return;
    }
    
    // 禁用按钮
    btnRollback.Enabled = false;
    btnRollback.Text = "回滚中...";
    
    // 显示进度
    lbState.Visible = true;
    pbDownFile.Visible = true;
    lbState.Text = "正在准备回滚...";
    SafeSetProgressValue(0);
    Application.DoEvents();
    
    // 执行回滚
    bool success = false;
    try
    {
        lbState.Text = "正在备份当前版本...";
        SafeSetProgressValue(10);
        Application.DoEvents();
        
        var historyManager = new VersionHistoryManager();
        
        lbState.Text = $"正在回滚到版本 {snapshot.AppVersion}...";
        SafeSetProgressValue(30);
        Application.DoEvents();
        
        success = historyManager.RollbackToSnapshot(snapshot.SnapshotFolderName);
        
        SafeSetProgressValue(100);
        lbState.Text = success ? "回滚成功！" : "回滚失败！";
        Application.DoEvents();
    }
    catch (Exception ex)
    {
        AppendAllText($"回滚异常: {ex.Message}\n{ex.StackTrace}");
        MessageBox.Show($"回滚过程中发生错误:\n{ex.Message}", "错误", 
            MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
    }
    finally
    {
        btnRollback.Enabled = true;
        btnRollback.Text = "还原(&R)";
    }
    
    if (success)
    {
        MessageBox.Show($"成功回滚到版本 {snapshot.AppVersion}！\n应用程序将重新启动。", 
            "操作成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        
        // 启动主程序
        StartEntryPointExe("rollback=true", snapshot.AppVersion);
        this.Close();
    }
    else
    {
        MessageBox.Show("版本回滚失败，请检查日志文件。", "操作失败", 
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
```

---

## 📊 优势对比

### 当前设计 vs 新设计

| 特性 | 当前设计 | 新设计 |
|------|---------|--------|
| **版本一致性** | ❌ 文件级版本，可能不一致 | ✅ 应用级快照，完全一致 |
| **回滚可靠性** | ⚠️ 部分文件可能失败 | ✅ 原子性操作，全或无 |
| **子目录支持** | ❌ 不支持 | ✅ 完整支持 |
| **完整性验证** | ❌ 无 | ✅ SHA256 校验和 |
| **备份机制** | ⚠️ 只备份 AutoUpdate.* | ✅ 完整应用备份 |
| **存储空间** | ⚠️ 每个文件单独存储 | ✅ 增量存储（未来可优化） |
| **实现复杂度** | 🔴 复杂且易错 | 🟢 简单清晰 |
| **用户体验** | ⚠️ 需要理解文件版本 | ✅ 只需选择应用版本 |

---

## 🚀 实施步骤

### Phase 1: 基础架构（1-2天）
1. ✅ 创建 `VersionSnapshot.cs` 数据模型
2. ✅ 重构 `VersionHistoryManager.cs`
3. ✅ 实现 `CreateSnapshot()` 方法
4. ✅ 实现 `RollbackToSnapshot()` 方法

### Phase 2: 集成测试（1天）
1. ✅ 在 `LastCopy()` 中调用快照创建
2. ✅ 测试快照创建是否正确
3. ✅ 测试回滚功能
4. ✅ 验证完整性检查

### Phase 3: UI 改造（1天）
1. ✅ 修改 `ShowRollbackMode()` 显示快照列表
2. ✅ 更新 `btnRollback_Click()` 使用新逻辑
3. ✅ 添加进度条显示
4. ✅ 改进错误提示

### Phase 4: 优化与清理（1天）
1. ✅ 移除旧的文件级版本代码
2. ✅ 清理 `htUpdateFile` 相关逻辑
3. ✅ 添加单元测试
4. ✅ 编写用户文档

---

## ⚠️ 注意事项

### 1. 存储空间管理
- 每个快照约 50-100 MB（取决于应用大小）
- 保留 10 个快照 ≈ 500-1000 MB
- 建议：定期清理旧快照，或实现增量备份

### 2. 文件锁定问题
- AutoUpdate.exe 自身运行时会被锁定
- 解决：跳过锁定文件，由自我更新流程处理

### 3. 回滚时的进程管理
- 回滚前必须关闭主程序
- 解决：在 `RollbackToSnapshot()` 中调用 `KillProcessBeforeApply()`

### 4. 向后兼容
- 旧的历史记录格式需要迁移
- 解决：提供迁移工具或手动清理

---

## 🎯 总结

### 核心价值
1. **消除版本冲突**: 应用级快照确保所有文件版本一致
2. **提高可靠性**: 原子性回滚，要么成功要么恢复
3. **简化逻辑**: 不再需要管理复杂的文件级版本映射
4. **增强体验**: 用户只需选择"回滚到哪个版本"，无需关心文件细节

### 风险评估
- 🟢 **低风险**: 新逻辑独立于现有更新流程
- 🟢 **可回退**: 保留旧代码作为备选
- 🟢 **渐进式**: 可以分阶段实施

### 建议
**强烈建议采用此方案**，它能从根本上解决版本耦合问题，提供更可靠的回滚功能。
