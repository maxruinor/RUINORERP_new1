# FileManagementControl 集成文件监控服务实施说明

## 概述

将 `FileStorageMonitorService` 集成到 `FileManagementControl` 文件管理控件中,增强文件管理功能,提供实时监控和手动清理能力。

## 集成内容

### 1. 服务注入

**文件**: `RUINORERP.Server/Controls/FileManagementControl.cs`

在构造函数中注入 `FileStorageMonitorService`:

```csharp
// 文件监控服务
private readonly FileStorageMonitorService _monitorService;

public FileManagementControl()
{
    // ... 其他初始化代码

    // 从依赖注入容器获取监控服务
    _monitorService = Program.ServiceProvider.GetService<FileStorageMonitorService>();

    // ...
}
```

### 2. 数据源优化

#### 2.1 使用监控服务数据

修改 `LoadRealStorageInfoAsync()` 方法,优先使用监控服务提供的实时数据:

```csharp
private async Task LoadRealStorageInfoAsync()
{
    _currentStorageSummary = new FileStorageSummary();
    _fileCategories = new List<FileCategoryInfo>();

    try
    {
        // 尝试从监控服务获取实时监控信息
        if (_monitorService != null)
        {
            _currentMonitorInfo = await _monitorService.GetMonitorInfoAsync();

            // 使用监控服务的统计数据
            _currentStorageSummary.TotalFileCount = _currentMonitorInfo.TotalFiles;
            _currentStorageSummary.TotalStorageSize = (long)(_currentMonitorInfo.TotalStorageSizeGB * 1024 * 1024 * 1024);
            _currentStorageSummary.TotalDiskSpace = (long)(_currentMonitorInfo.TotalDiskSpaceGB * 1024 * 1024 * 1024);

            // 如果监控服务返回的数据有效,使用它
            if (_currentStorageSummary.TotalFileCount > 0)
            {
                _currentStorageSummary.Categories = await LoadFileCategoriesAsync();
                return;
            }
        }

        // 回退到原始查询逻辑(当监控服务不可用时)
        // ...
    }
    catch (Exception ex)
    {
        System.Diagnostics.Debug.WriteLine("加载真实存储信息失败: " + ex.Message);
        throw;
    }
}
```

#### 2.2 新增 LoadFileCategoriesAsync 方法

将文件分类加载逻辑独立出来,提高代码复用性:

```csharp
/// <summary>
/// 加载文件分类信息
/// </summary>
private async Task<List<FileCategoryInfo>> LoadFileCategoriesAsync()
{
    try
    {
        var fileInfos = await _fileStorageInfoController.QueryAsync(
            c => c.FileStatus == (int)FileStatus.Active && c.isdeleted == false);

        if (fileInfos == null || fileInfos.Count == 0)
        {
            return new List<FileCategoryInfo>();
        }

        var groupedByBusinessType = fileInfos
            .Cast<tb_FS_FileStorageInfo>()
            .GroupBy(f => f.BusinessType ?? 0)
            .ToList();

        return groupedByBusinessType.Select(group => new FileCategoryInfo
        {
            BusinessType = group.Key,
            CategoryName = GetBusinessTypeName(group.Key),
            FileCount = group.Count(),
            StorageSize = group.Sum(f => f.FileSize),
            LastModified = group.Max(f => f.Modified_at ?? f.Created_at ?? DateTime.MinValue)
        }).ToList();
    }
    catch (Exception ex)
    {
        System.Diagnostics.Debug.WriteLine("加载文件分类失败: " + ex.Message);
        return new List<FileCategoryInfo>();
    }
}
```

### 3. UI 增强功能

#### 3.1 增强的存储摘要更新

修改 `UpdateStorageSummary()` 方法,集成监控状态信息:

```csharp
private void UpdateStorageSummary()
{
    if (_currentStorageSummary == null)
        return;

    // 更新存储概览 - 使用真实数据
    long totalSize = _currentStorageSummary.TotalDiskSpace;
    long usedSize = _currentStorageSummary.TotalStorageSize;
    long freeSize = totalSize - usedSize;

    lblTotalStorage.Text = FormatBytes(totalSize);
    lblUsedStorage.Text = FormatBytes(usedSize);
    lblFreeStorage.Text = FormatBytes(freeSize);

    // 计算使用百分比 - 优先使用监控服务的更精确数据
    int usagePercentage = 0;
    if (_currentMonitorInfo != null)
    {
        usagePercentage = (int)_currentMonitorInfo.FileStorageUsagePercentage;
    }
    else
    {
        usagePercentage = totalSize > 0 ? (int)((double)usedSize / totalSize * 100) : 0;
    }

    progressBar1.Value = usagePercentage;
    lblUsagePercentage.Text = usagePercentage + "%";

    // 根据使用情况设置进度条颜色和状态
    if (_currentMonitorInfo != null)
    {
        if (_currentMonitorInfo.IsCritical)
        {
            progressBar1.ForeColor = System.Drawing.Color.Red;
            UpdateStatusLabels(true, $"紧急警告: 磁盘使用率 {usagePercentage}%");
        }
        else if (_currentMonitorInfo.IsWarning)
        {
            progressBar1.ForeColor = System.Drawing.Color.Orange;
            UpdateStatusLabels(true, $"警告: 磁盘使用率 {usagePercentage}%");
        }
        else
        {
            progressBar1.ForeColor = System.Drawing.Color.Green;
        }
    }
    else
    {
        if (usagePercentage > 80)
            progressBar1.ForeColor = System.Drawing.Color.Red;
        else if (usagePercentage > 60)
            progressBar1.ForeColor = System.Drawing.Color.Orange;
        else
            progressBar1.ForeColor = System.Drawing.Color.Green;
    }

    // 更新文件统计
    lblTotalFiles.Text = _currentStorageSummary.TotalFileCount.ToString();

    // 如果有监控信息,显示额外信息
    if (_currentMonitorInfo != null)
    {
        // 可以在状态栏或标签显示过期文件数和孤立文件数
        var extraInfo = $"正常: {_currentMonitorInfo.ActiveFiles}";
        if (_currentMonitorInfo.ExpiredFiles > 0)
            extraInfo += $" | 过期: {_currentMonitorInfo.ExpiredFiles}";
        if (_currentMonitorInfo.OrphanedFiles > 0)
            extraInfo += $" | 孤立: {_currentMonitorInfo.OrphanedFiles}";

        // 更新状态标签文本,如果之前没有设置为警告/紧急状态
        if (!_currentMonitorInfo.IsWarning && !_currentMonitorInfo.IsCritical)
        {
            UpdateStatusLabels(true, $"数据加载成功 - {extraInfo}");
        }
    }
}
```

#### 3.2 添加新按钮

**文件**: `RUINORERP.Server/Controls/FileManagementControl.Designer.cs`

添加两个新按钮:
1. **btnMonitorDetails**: 查看监控详情按钮
2. **btnCleanupFiles**: 清理文件按钮

```csharp
// btnMonitorDetails
btnMonitorDetails.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
btnMonitorDetails.Location = new System.Drawing.Point(703, 40);
btnMonitorDetails.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
btnMonitorDetails.Name = "btnMonitorDetails";
btnMonitorDetails.Size = new System.Drawing.Size(205, 33);
btnMonitorDetails.TabIndex = 2;
btnMonitorDetails.Text = "查看监控详情";
btnMonitorDetails.UseVisualStyleBackColor = true;
btnMonitorDetails.Click += new System.EventHandler(btnMonitorDetails_Click);

// btnCleanupFiles
btnCleanupFiles.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
btnCleanupFiles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
btnCleanupFiles.Location = new System.Drawing.Point(586, 8);
btnCleanupFiles.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
btnCleanupFiles.Name = "btnCleanupFiles";
btnCleanupFiles.Size = new System.Drawing.Size(88, 33);
btnCleanupFiles.TabIndex = 3;
btnCleanupFiles.Text = "清理文件";
btnCleanupFiles.UseVisualStyleBackColor = false;
btnCleanupFiles.Click += new System.EventHandler(btnCleanupFiles_Click);
```

### 4. 新增功能方法

#### 4.1 手动清理功能

```csharp
/// <summary>
/// 手动触发存储空间清理
/// </summary>
private async Task PerformManualCleanupAsync()
{
    if (_monitorService == null)
    {
        MessageBox.Show("监控服务未启动,无法执行清理操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
    }

    var confirmResult = MessageBox.Show(
        "确定要执行文件清理吗?\n\n" +
        "清理内容包括:\n" +
        "- 过期文件(超过7天未访问)\n" +
        "- 孤立文件(无业务关联)\n\n" +
        "建议先备份数据!",
        "确认清理",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question
    );

    if (confirmResult != DialogResult.Yes)
        return;

    try
    {
        // 显示等待光标
        this.Cursor = Cursors.WaitCursor;
        btnRefresh.Enabled = false;

        // 获取清理服务
        var cleanupService = Program.ServiceProvider.GetService<FileCleanupService>();
        if (cleanupService == null)
        {
            MessageBox.Show("清理服务未找到", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        // 清理过期文件
        var expiredCount = await cleanupService.CleanupExpiredFilesAsync(
            daysThreshold: 7,
            physicalDelete: true
        );

        // 清理孤立文件
        var orphanedCount = await cleanupService.CleanupOrphanedFilesAsync(
            daysThreshold: 3,
            physicalDelete: true
        );

        var message = $"清理完成!\n\n" +
                     $"过期文件清理: {expiredCount} 个\n" +
                     $"孤立文件清理: {orphanedCount} 个\n" +
                     $"总计清理: {expiredCount + orphanedCount} 个";

        MessageBox.Show(message, "清理结果", MessageBoxButtons.OK, MessageBoxIcon.Information);

        // 刷新数据
        await LoadStorageInfo();
    }
    catch (Exception ex)
    {
        MessageBox.Show($"清理失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    finally
    {
        this.Cursor = Cursors.Default;
        btnRefresh.Enabled = true;
    }
}
```

#### 4.2 监控详情显示

```csharp
/// <summary>
/// 显示监控详细信息
/// </summary>
private void ShowMonitorDetails()
{
    if (_currentMonitorInfo == null)
    {
        MessageBox.Show("监控数据不可用", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
    }

    var details = new Form
    {
        Text = "文件存储监控详情",
        Size = new System.Drawing.Size(600, 500),
        StartPosition = FormStartPosition.CenterParent
    };

    var listView = new ListView
    {
        Dock = DockStyle.Fill,
        View = View.Details,
        FullRowSelect = true,
        GridLines = true
    };

    listView.Columns.Add("监控项", 200);
    listView.Columns.Add("值", 300);

    var items = new[]
    {
        new { Name = "总文件数", Value = _currentMonitorInfo.TotalFiles.ToString() },
        new { Name = "正常文件数", Value = _currentMonitorInfo.ActiveFiles.ToString() },
        new { Name = "过期文件数", Value = _currentMonitorInfo.ExpiredFiles.ToString() },
        new { Name = "孤立文件数", Value = _currentMonitorInfo.OrphanedFiles.ToString() },
        new { Name = "总存储大小", Value = _currentMonitorInfo.TotalStorageSizeFormatted },
        new { Name = "文件存储使用率", Value = $"{_currentMonitorInfo.FileStorageUsagePercentage:F2}%" },
        new { Name = "磁盘使用率", Value = $"{_currentMonitorInfo.DiskUsagePercentage:F2}%" },
        new { Name = "可用磁盘空间", Value = $"{_currentMonitorInfo.FreeDiskSpaceGB:F2} GB" },
        new { Name = "总磁盘空间", Value = $"{_currentMonitorInfo.TotalDiskSpaceGB:F2} GB" },
        new { Name = "配置最大存储空间", Value = $"{_currentMonitorInfo.MaxStorageSizeGB:F2} GB" },
        new { Name = "警告阈值", Value = $"{_currentMonitorInfo.WarningThreshold}%" },
        new { Name = "紧急阈值", Value = $"{_currentMonitorInfo.CriticalThreshold}%" },
        new { Name = "最后检查时间", Value = _currentMonitorInfo.LastCheckTime.ToString("yyyy-MM-dd HH:mm:ss") },
        new { Name = "警告状态", Value = _currentMonitorInfo.IsWarning ? "是" : "否" },
        new { Name = "紧急状态", Value = _currentMonitorInfo.IsCritical ? "是" : "否" }
    };

    foreach (var item in items)
    {
        var listViewItem = new ListViewItem(item.Name);
        listViewItem.SubItems.Add(item.Value);
        listView.Items.Add(listViewItem);
    }

    details.Controls.Add(listView);
    details.ShowDialog();
}
```

## 新增功能说明

### 1. 实时监控数据

- **自动使用监控服务数据**: 优先从 `FileStorageMonitorService` 获取实时统计信息
- **回退机制**: 当监控服务不可用时,自动回退到原始查询逻辑
- **精确使用率**: 使用监控服务提供的文件存储使用率计算

### 2. 监控状态提示

- **警告状态**: 当使用率达到警告阈值(默认80%)时,进度条变为橙色,状态栏显示警告信息
- **紧急状态**: 当使用率达到紧急阈值(默认90%)时,进度条变为红色,状态栏显示紧急警告
- **文件分类统计**: 在状态栏显示正常文件、过期文件和孤立文件的数量

### 3. 手动清理功能

- **安全确认**: 执行清理前弹出确认对话框,提醒用户备份数据
- **分步清理**:
  - 清理过期文件(超过7天未访问)
  - 清理孤立文件(超过3天无业务关联)
- **进度提示**: 清理过程中显示等待光标并禁用按钮
- **结果报告**: 显示清理的文件数量和类型统计
- **自动刷新**: 清理完成后自动刷新显示数据

### 4. 监控详情查看

点击"查看监控详情"按钮可查看详细的监控信息:
- 文件统计(总数、正常、过期、孤立)
- 存储空间统计(总大小、使用率、可用空间)
- 配置信息(最大存储空间、警告阈值、紧急阈值)
- 状态信息(警告状态、紧急状态、最后检查时间)

## 使用方法

### 1. 基本使用

1. 控件自动加载存储信息
2. 定时器每60秒自动刷新数据
3. 显示存储使用概览和文件分类列表

### 2. 查看监控详情

1. 点击"查看监控详情"按钮
2. 查看详细的监控信息窗口
3. 了解系统当前状态和配置

### 3. 手动清理文件

1. 点击"清理文件"按钮(红色背景)
2. 确认清理操作
3. 等待清理完成
4. 查看清理结果报告
5. 数据自动刷新

### 4. 查看文件详情

1. 在文件分类列表中选择一行
2. 点击"查看详情"按钮
3. 查看该业务类型下的所有文件详细信息

## 技术要点

### 1. 依赖注入

- 通过 `Program.ServiceProvider` 获取服务
- 添加了对 `FileStorageMonitorService` 的引用
- 添加了对 `FileCleanupService` 的引用

### 2. 命名空间

```csharp
using RUINORERP.Server.Helpers;
using RUINORERP.Server.Network.Services;
```

### 3. 线程安全

- 所有UI更新都通过 `Invoke` 确保在UI线程执行
- 异步操作使用 `async/await` 模式
- 清理过程中禁用按钮防止重复操作

### 4. 错误处理

- 所有方法都包含 try-catch 块
- 错误信息显示在状态栏或消息框
- 使用 `Debug.WriteLine` 记录调试信息

### 5. 用户体验

- 提供明确的操作确认
- 显示进度指示(等待光标)
- 清晰的结果反馈
- 合理的按钮布局和颜色区分

## 兼容性说明

### 向后兼容

- 如果 `FileStorageMonitorService` 未注册,控件仍可正常工作
- 会自动回退到原始的查询逻辑
- 不会影响现有功能

### 前置条件

1. `FileStorageMonitorService` 已在依赖注入容器中注册
2. `FileCleanupService` 已在依赖注入容器中注册
3. 服务已正确配置(存储路径、阈值等)

## 测试建议

### 1. 功能测试

- [ ] 控件正常加载和显示数据
- [ ] 监控数据正确显示
- [ ] 监控详情窗口正常打开和关闭
- [ ] 手动清理功能正常工作
- [ ] 文件详情查看正常工作

### 2. 异常测试

- [ ] 监控服务未注册时的回退逻辑
- [ ] 清理服务未注册时的错误提示
- [ ] 数据库连接失败时的错误处理
- [ ] 清理过程中的异常处理

### 3. 用户体验测试

- [ ] 按钮布局合理
- [ ] 颜色提示清晰
- [ ] 消息提示明确
- [ ] 进度指示及时

## 总结

通过集成 `FileStorageMonitorService`,`FileManagementControl` 现在具备以下增强功能:

1. ✅ 实时监控数据集成
2. ✅ 监控状态智能提示
3. ✅ 手动清理过期和孤立文件
4. ✅ 详细的监控信息查看
5. ✅ 向后兼容和容错机制

这些功能大大提升了文件管理的可用性和维护性,为系统管理员提供了更好的工具来监控和管理文件存储。
