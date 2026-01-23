# FileCleanupService 连接关闭问题修复方案

## 问题分析

### 异常信息
```
System.InvalidOperationException
  Message=Invalid operation. The connection is closed.
  Source=System.Data.SqlClient
  StackTrace:
   在 SqlSugar.AdoProvider.<GetDataReaderAsync>d__128.MoveNext()
   在 SqlSugar.QueryableProvider`1.<GetDataAsync>d__259`1.MoveNext()
   在 SqlSugar.QueryableProvider`1.<_ToListAsync>d__256`1.MoveNext()
   在 RUINORERP.Server.Network.Services.FileCleanupService.CleanupExpiredFilesAsync(int, bool)
   在 E:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.Server\Network\Services\FileCleanupService.cs 中: 第 97 行
```

### 根本原因

1. **FileCleanupService 生命周期问题**
   - FileCleanupService 被注册为 `Transient`(瞬时) 生命周期
   - FileStorageMonitorService (Singleton) 注入了 FileCleanupService
   - FileCleanupService 构造函数中启动了 Timer 定时器(每天凌晨2点执行)

2. **并发访问数据库**
   - 由于 FileCleanupService 是 Transient,每次注入都会创建新实例
   - 多个 FileCleanupService 实例共享同一个 UnitOfWorkManage 和 db 连接
   - 同一时间(凌晨2点)多个实例同时访问数据库,导致连接状态冲突

3. **SqlSugar 连接管理限制**
   - 虽然 SqlSugar 有自动连接管理,但在多线程并发场景下,如果同一个 db 实例被多个线程同时访问,会导致连接状态混乱
   - Timer 回调中的异步操作没有正确处理异常,可能导致连接泄漏

## 修复方案

### 1. 修改服务生命周期 (关键修复)

**文件**: `RUINORERP.Server/Network/DI/NetworkServicesDependencyInjection.cs`

```csharp
// 修改前
services.AddTransient<FileCleanupService>();

// 修改后
services.AddSingleton<FileCleanupService>();
```

```csharp
// Autofac 配置也要同步修改
// 修改前
builder.RegisterType<FileCleanupService>().AsSelf().InstancePerDependency();

// 修改后
builder.RegisterType<FileCleanupService>().AsSelf().SingleInstance();
```

**原因**:
- 将 FileCleanupService 改为 Singleton,确保系统中只有一个实例和 Timer
- 避免多个定时器同时执行导致的数据库连接冲突

### 2. 增强线程安全 (关键修复)

**文件**: `RUINORERP.Server/Network/Services/FileCleanupService.cs`

```csharp
public class FileCleanupService : IDisposable
{
    private readonly IUnitOfWorkManage _unitOfWorkManage;
    private readonly ILogger<FileCleanupService> _logger;
    private readonly ServerGlobalConfig _serverConfig;
    private readonly Timer _cleanupTimer;
    private readonly SemaphoreSlim _cleanupLock = new SemaphoreSlim(1, 1);
    private readonly SemaphoreSlim _dbLock = new SemaphoreSlim(1, 1); // 新增:数据库操作锁
    private bool _disposed = false;
    private int _activeOperations = 0; // 新增:活跃操作计数器
```

**新增的线程安全措施**:
- `_dbLock`: 确保所有数据库操作串行执行
- `_activeOperations`: 使用原子操作检查是否有任务在执行,避免重叠

### 3. 优化 Timer 回调处理

**修改前**:
```csharp
_cleanupTimer = new Timer(
    async _ => await ExecuteAutoCleanupAsync(),
    null,
    dueTime,
    TimeSpan.FromDays(1));
```

**修改后**:
```csharp
_cleanupTimer = new Timer(
    state =>
    {
        var task = ExecuteAutoCleanupAsync();
        // 不 await,让 Timer 继续运行
        // 异常在 ExecuteAutoCleanupAsync 内部处理
    },
    null,
    dueTime,
    TimeSpan.FromDays(1));
```

**原因**:
- 避免 Timer 回调中的异步操作异常导致定时器停止
- 使用 Fire-and-Forget 模式,异常在任务内部处理

### 4. 增强自动清理任务的并发控制

```csharp
private async Task ExecuteAutoCleanupAsync()
{
    // 检查是否已有活跃任务在执行
    if (Interlocked.CompareExchange(ref _activeOperations, 1, 0) != 0)
    {
        _logger.LogWarning("文件清理任务已在执行中,跳过本次执行");
        return;
    }

    try
    {
        _logger.LogInformation("开始执行自动文件清理任务");

        // 检查清理锁,避免重叠执行
        if (!await _cleanupLock.WaitAsync(TimeSpan.Zero))
        {
            _logger.LogWarning("清理任务正在进行中,跳过本次执行");
            return;
        }

        try
        {
            await CleanupExpiredFilesAsync();
            await CleanupOrphanedFilesAsync();

            _logger.LogInformation("自动文件清理任务执行成功");
        }
        finally
        {
            _cleanupLock.Release();
        }
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "自动文件清理任务执行失败");
    }
    finally
    {
        Interlocked.Exchange(ref _activeOperations, 0);
    }
}
```

### 5. 所有数据库操作使用数据库锁

**CleanupExpiredFilesAsync**:
```csharp
public async Task<int> CleanupExpiredFilesAsync(int daysThreshold = 30, bool physicalDelete = false)
{
    // 使用数据库操作锁,避免并发访问导致的连接冲突
    if (!await _dbLock.WaitAsync(TimeSpan.FromMinutes(10)))
    {
        _logger.LogWarning("数据库操作繁忙,跳过本次清理");
        return 0;
    }

    try
    {
        var db = _unitOfWorkManage.GetDbClient();
        // ... 数据库操作 ...
        _logger.LogInformation("过期文件清理完成,共清理 {Count} 个文件", cleanedCount);
        return cleanedCount;
    }
    finally
    {
        _dbLock.Release();
    }
}
```

**CleanupOrphanedFilesAsync** 和 **CleanupPhysicalOrphanedFilesAsync** 也采用相同的模式。

### 6. 完善资源释放

```csharp
public void Dispose()
{
    if (_disposed)
        return;

    _disposed = true;

    // 释放定时器
    _cleanupTimer?.Dispose();
    _cleanupLock?.Dispose();
    _dbLock?.Dispose(); // 新增:释放数据库锁
}
```

## 修复效果

### 解决的问题
1. ✅ **消除了多个定时器并发执行** - Singleton 生命周期确保只有一个 Timer
2. ✅ **避免了数据库连接冲突** - 使用 `_dbLock` 串行化所有数据库操作
3. ✅ **防止任务重叠执行** - 使用 `_activeOperations` 和 `_cleanupLock` 双重检查
4. ✅ **增强了异常处理** - Timer 回调异常不会导致定时器停止
5. ✅ **提升了系统稳定性** - 详细的日志记录便于问题追踪

### 性能影响
- **性能提升**: 减少了不必要的并发尝试,避免了重复执行
- **轻微延迟**: 数据库操作串行化可能导致清理任务执行时间略微增加(但影响极小)
- **总体评价**: 牺牲微不足道的性能换取系统稳定性和可靠性

## 验证建议

1. **测试定时任务执行**
   - 手动触发清理任务,验证是否正常执行
   - 检查日志中是否有并发执行的警告

2. **监控数据库连接**
   - 观察凌晨2点定时任务执行时的数据库连接数
   - 确认没有连接泄漏或连接冲突

3. **验证清理功能**
   - 测试过期文件清理功能
   - 测试孤立文件清理功能
   - 测试物理文件清理功能

4. **压力测试**
   - 在高负载下同时触发多个清理任务
   - 验证是否有异常或性能问题

## 总结

本次修复从根本上解决了 FileCleanupService 的数据库连接关闭问题:
- 通过修改生命周期为 Singleton,消除了多个定时器并发执行的根源
- 通过引入数据库操作锁,确保了所有数据库操作的线程安全
- 通过增强异常处理和日志记录,提升了系统的可观测性和稳定性

这是一个典型的**并发访问导致资源竞争**问题的解决方案,适用于类似的定时任务场景。
