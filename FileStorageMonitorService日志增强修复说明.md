# FileStorageMonitorService日志增强修复说明

## 修复内容

### 1. OnTimerElapsed方法增强

**文件**: `RUINORERP.Server\Network\Services\FileStorageMonitorService.cs`

**修改**:
- ✅ 添加执行时间统计
- ✅ 区分不同类型的异常（OperationCanceledException、TimeoutException、SqlSugarException）
- ✅ 记录完整的SQL语句（针对数据库异常）
- ✅ 添加详细的日志前缀 `[FileStorageMonitor]`

**效果**:
```csharp
// 修复前
_logger.LogError(ex, "定时检查存储空间失败: {ErrorType} - {ErrorMessage}", 
    ex.GetType().Name, ex.Message);

// 修复后
catch (SqlSugar.SqlSugarException ex)
{
    _logger.LogError(ex, "[FileStorageMonitor] 数据库操作失败: {ErrorMessage}\n{StackTrace}\nSQL: {Sql}", 
        ex.Message, ex.StackTrace, ex.Sql ?? "N/A");
}
```

### 2. CheckStorageSpaceAsync方法增强

**修改**:
- ✅ 添加2分钟超时保护（CancellationTokenSource）
- ✅ 所有日志添加 `[FileStorageMonitor]` 前缀
- ✅ 捕获OperationCanceledException并转换为TimeoutException
- ✅ 重新抛出异常，让上层能够正确处理

**新增方法**:
```csharp
/// <summary>
/// 带超时保护的统计数据获取
/// </summary>
private async Task<FileCleanupStatistics> GetCleanupStatisticsWithTimeoutAsync(CancellationToken ct)
{
    try
    {
        return await _fileCleanupService.GetCleanupStatisticsAsync();
    }
    catch (TaskCanceledException) when (ct.IsCancellationRequested)
    {
        throw new OperationCanceledException("获取清理统计数据超时", ct);
    }
}
```

## 预期效果

### 正常情况日志

```
2026-04-12 00:11:10 DEBUG [FileStorageMonitor] 开始定时检查存储空间
2026-04-12 00:11:10 DEBUG [FileStorageMonitor] 文件统计信息获取完成: TotalFiles=1234, TotalStorageSize=5678901234
2026-04-12 00:11:10 INFO  [FileStorageMonitor] 磁盘空间检查 - 总空间: 500.00GB, 已用: 250.00GB, 可用: 250.00GB, 使用率: 50.00%
2026-04-12 00:11:10 DEBUG [FileStorageMonitor] 定时检查完成，耗时: 123ms
```

### 异常情况日志

**场景1: 数据库超时**
```
2026-04-12 00:11:10 DEBUG [FileStorageMonitor] 开始定时检查存储空间
2026-04-12 00:13:10 WARN  [FileStorageMonitor] 检查存储空间超时（2分钟）
2026-04-12 00:13:10 ERROR [FileStorageMonitor] 定时检查超时: 检查存储空间操作超时
   at RUINORERP.Server.Network.Services.FileStorageMonitorService.<CheckStorageSpaceAsync>d__...
```

**场景2: SQL错误**
```
2026-04-12 00:11:10 DEBUG [FileStorageMonitor] 开始定时检查存储空间
2026-04-12 00:11:11 ERROR [FileStorageMonitor] 数据库操作失败: Invalid object name 'tb_FS_FileStorageInfo'
   at SqlSugar.AdoProvider.GetString(...)
SQL: SELECT COUNT(*) FROM tb_FS_FileStorageInfo WHERE isdeleted = 0
```

**场景3: 通用异常**
```
2026-04-12 00:11:10 DEBUG [FileStorageMonitor] 开始定时检查存储空间
2026-04-12 00:11:10 ERROR [FileStorageMonitor] 定时检查存储空间失败: IOException - 磁盘不可访问
   at System.IO.DriveInfo..ctor(String driveName)
   at RUINORERP.Server.Network.Services.FileStorageMonitorService.<CheckStorageSpaceAsync>d__...
```

## 下一步行动

### 立即验证

1. **重启服务器**，使修改生效
2. **等待30分钟**，观察是否还有ERROR日志
3. **查看日志文件** `Logs/SocketServer/yyyy-MM-DD/Error.log`，确认是否有详细的异常信息

### 如果仍然报错

根据新的日志输出，可以准确判断问题类型：

- **超时**: 需要优化数据库查询或增加超时时间
- **SQL错误**: 检查表是否存在、权限是否正确
- **连接错误**: 检查数据库服务是否正常运行
- **其他**: 根据具体异常信息进一步排查

### 后续优化

参考完整分析文档: `docs/FileStorageMonitorService和登录超时问题分析与修复.md`

建议按优先级实施：
1. P0 - 已完成：增强日志记录
2. P1 - 待实施：优化数据库查询性能
3. P2 - 待实施：实现健康检查和熔断器

## 相关文件

- 修复文件: `RUINORERP.Server\Network\Services\FileStorageMonitorService.cs`
- 分析文档: `docs/FileStorageMonitorService和登录超时问题分析与修复.md`
- 相关服务: `RUINORERP.Server\Network\Services\FileCleanupService.cs`
