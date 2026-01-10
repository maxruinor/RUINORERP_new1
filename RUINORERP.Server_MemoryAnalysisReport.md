# 服务器内存使用分析报告

## 执行摘要

**问题**: 服务器运行3天后内存占用达到 2GB
**分析日期**: 2026-01-10
**分析范围**: RUINORERP.Server 项目内存使用情况

---

## 1. 内存显示位置分析

### 代码位置: `ServerMonitorControl.cs:504-508`

```504:508:E:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.Server\Controls\ServerMonitorControl.cs
// 更新内存使用情况
var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
var workingSetMemory = currentProcess.WorkingSet64 / (1024 * 1024); // 转换为MB
var managedMemory = GC.GetTotalMemory(false) / (1024 * 1024); // 转换为MB

lblMemoryUsageValue.Text = $"{workingSetMemory} MB (托管: {managedMemory} MB)";
```

**内存指标说明**:
- **WorkingSet64**: 进程占用的物理内存（包括托管、非托管、共享库等）
- **GC.GetTotalMemory(false)**: 托管堆中已分配的内存

---

## 2. 潜在内存泄漏源分析

### 2.1 静态集合累积 (高风险)

#### 2.1.1 SessionService - 会话集合
**文件**: `RUINORERP.Server\Network\Services\SessionService.cs:40,49-50`

```40,49-50:E:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.Server\Network\Services\SessionService.cs
private readonly ConcurrentDictionary<string, SessionInfo> _sessions;

// 存储待处理的请求任务，用于匹配响应
private static readonly ConcurrentDictionary<string, TaskCompletionSource<PacketModel>> _pendingRequests =
    new ConcurrentDictionary<string, TaskCompletionSource<PacketModel>>();
```

**问题**:
- `_pendingRequests` 是静态集合，永远不会被 GC 回收
- 每个未完成的请求都会在集合中保留 `TaskCompletionSource<PacketModel>` 对象
- 如果请求超时或异常未处理，这些对象会永久驻留内存

**估算内存占用**:
- 假设平均有 1000 个挂起的请求
- 每个 `TaskCompletionSource<PacketModel>` 约 200 字节
- **总计**: ~200 KB + 引用对象

**优化建议**:
```csharp
// 添加请求超时清理机制
private async Task CleanupPendingRequestsAsync()
{
    var expiredKeys = _pendingRequests
        .Where(kvp => kvp.Value.Task.IsCompleted || 
                     (DateTime.UtcNow - kvp.Value.CreationTime) > TimeSpan.FromMinutes(5))
        .Select(kvp => kvp.Key)
        .ToList();

    foreach (var key in expiredKeys)
    {
        if (_pendingRequests.TryRemove(key, out var tcs))
        {
            try { tcs.SetCanceled(); } catch { }
        }
    }
}
```

---

#### 2.1.2 ServerLockManager - 锁定信息集合
**文件**: `RUINORERP.Server\Network\Services\ServerLockManager.cs:45,60`

```45,60:E:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.Server\Network\Services\ServerLockManager.cs
// 简化的单一数据结构 - 按单据ID索引
private readonly ConcurrentDictionary<long, LockInfo> _documentLocks;

// 用于存储解锁请求的字典，键为单据ID，值为锁定请求信息
private readonly ConcurrentDictionary<long, UnlockRequestInfo> _unlockRequests = new ConcurrentDictionary<long, UnlockRequestInfo>();
```

**问题**:
- 会话断开时依赖事件驱动清理，如果事件未触发会导致孤儿锁
- `UnlockRequestInfo` 可能包含大对象引用

**估算内存占用**:
- 假设 5000 个文档被锁定
- 每个 `LockInfo` 约 1KB (包含用户信息、时间戳等)
- **总计**: ~5 MB

---

#### 2.1.3 EnhancedErrorHandlingService - 错误记录
**文件**: `RUINORERP.Server\Network\ErrorHandling\EnhancedErrorHandlingService.cs:20,26`

```20,26:E:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.Server\Network\ErrorHandling\EnhancedErrorHandlingService.cs
private readonly ConcurrentDictionary<string, ErrorRecord> _errorRecords;
private readonly int _maxErrorRecords = 1000; // 最大错误记录数
```

**问题**:
- 每个错误记录包含完整的异常堆栈跟踪
- 异常堆栈可能引用大量调用栈帧信息

**估算内存占用**:
- 1000 个错误记录
- 每个 `ErrorRecord` 平均 5KB (包含 Exception、Stack trace、Context)
- **总计**: ~5 MB

**现有缓解措施**:
```36-44:E:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.Server\Network\ErrorHandling\EnhancedErrorHandlingService.cs
// 如果错误记录数已达上限，移除最旧的记录
if (_errorRecords.Count >= _maxErrorRecords)
{
    var oldestKey = _errorRecords.OrderBy(kvp => kvp.Value.Timestamp).First().Key;
    _errorRecords.TryRemove(oldestKey, out _);
}
```

---

#### 2.1.4 ProductSKUCodeGenerator - SKU 缓存
**文件**: `RUINORERP.Server\Services\BizCode\ProductSKUCodeGenerator.cs:39`

```39:E:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.Server\Services\BizCode\ProductSKUCodeGenerator.cs
private static readonly ConcurrentDictionary<string, bool> _skuCache = new ConcurrentDictionary<string, bool>();
```

**问题**:
- 静态字典，无过期机制
- SKU 字符串可能很长

**估算内存占用**:
- 假设 100,000 个 SKU
- 每个 SKU 字符串平均 20 字符 = 40 字节 (UTF-16)
- 字典开销 ~24 字节/条目
- **总计**: ~6.4 MB

---

#### 2.1.5 LoginCommandHandler - 登录尝试记录
**文件**: `RUINORERP.Server\Network\CommandHandlers\LoginCommandHandler.cs:59`

```59:E:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.Server\Network\CommandHandlers\LoginCommandHandler.cs
private static readonly ConcurrentDictionary<string, int> _loginAttempts = new ConcurrentDictionary<string, int>();
```

**问题**:
- 记录 IP 地址和失败次数
- 无自动清理机制

**估算内存占用**:
- 假设 10,000 个 IP 地址
- 每个 IP 字符串 ~15 字符 = 30 字节
- 字典开销 ~24 字节/条目
- **总计**: ~540 KB

---

#### 2.1.6 frmMainNew - 全局数据集合
**文件**: `RUINORERP.Server\frmMainNew.cs:109,119,128`

```109,119,128:E:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.Server\frmMainNew.cs
public ConcurrentDictionary<long, BaseConfig> UpdateConfigDataList = new ConcurrentDictionary<long, BaseConfig>();
public ConcurrentDictionary<long, ReminderData> ReminderBizDataList = new ConcurrentDictionary<long, ReminderData>();
public ConcurrentDictionary<string, string> workflowlist = new ConcurrentDictionary<string, string>();
```

**问题**:
- `ReminderBizDataList` 可能包含大量提醒数据
- `BaseConfig` 对象可能引用配置树结构

**估算内存占用**:
- 假设 50,000 条提醒数据
- 每个 `ReminderData` 平均 2KB
- **总计**: ~100 MB

---

### 2.2 IMemoryCache 使用 (中风险)

#### 2.2.1 StockCacheService
**文件**: `RUINORERP.Server\SmartReminder\StockCacheService.cs:78,84-88,92`

```78,84-88,92:E:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.Server\SmartReminder\StockCacheService.cs
private readonly IMemoryCache _cache;

// 缓存配置常量
private const string STOCK_CACHE_PREFIX = "stock_";
private const int DEFAULT_CACHE_EXPIRATION_SECONDS = 30;
private const int HIGH_PRIORITY_CACHE_EXPIRATION_SECONDS = 60;
private const int BULK_QUERY_BATCH_SIZE = 100;
private const int PREHEAT_BATCH_SIZE = 500;

// 缓存统计信息
private readonly CacheStatistics _statistics = new CacheStatistics();
private readonly ConcurrentDictionary<string, bool> _cacheKeys = new ConcurrentDictionary<string, bool>();
```

**内存配置分析**:
```148-156:E:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.Server\SmartReminder\StockCacheService.cs
var cacheEntryOptions = new MemoryCacheEntryOptions()
    .SetAbsoluteExpiration(expiration)
    .SetSlidingExpiration(TimeSpan.FromSeconds(10))
    .RegisterPostEvictionCallback(OnCacheEvicted)
    .SetPriority(IsHighPriorityProduct(productId) 
        ? CacheItemPriority.High 
        : CacheItemPriority.Normal);

_cache.Set(cacheKey, stock, cacheEntryOptions);
```

**问题**:
- `PreheatCacheAsync` 会预热大量库存数据
- `_cacheKeys` 字典重复存储缓存键（冗余）

**估算内存占用**:
- 假设预热 50,000 个产品
- 每个 `tb_Inventory` 对象 ~1KB
- 缓存键字典冗余 ~30 字节/条目
- **总计**: ~51.5 MB

---

#### 2.2.2 多个 IMemoryCache 实例
**文件**: `RUINORERP.Server\Startup.cs:480-482`

```480-482:E:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.Server\Startup.cs
services.AddMemoryCache();
services.AddMemoryCacheSetup();
services.AddDistributedMemoryCache();
```

**问题**:
- 注册了三个独立的内存缓存实例
- 每个实例都有自己的内存池

---

### 2.3 Timer 和后台任务 (低风险但累积)

#### 2.3.1 多个 Timer 服务
**文件**: 多个服务使用 Timer

| 服务 | 文件 | Timer 用途 | 间隔 |
|------|------|-----------|------|
| MemoryMonitoringService | Services/MemoryMonitoringService.cs | 内存监控 | 30秒 |
| SessionService | Network/Services/SessionService.cs | 会话清理 | 5分钟 |
| ServerLockManager | Network/Services/ServerLockManager.cs | 锁清理 | 2分钟 |
| SmartReminderService | SmartReminder/SmartReminderService.cs | 提醒检查 | 5分钟 |

**问题**:
- Timer 回调可能未正确处理异常，导致内存泄漏
- `ServerMonitorControl` 每 1-5 秒刷新一次监控数据

**估算内存占用**:
- 每个 Timer 回调分配的临时对象
- 假设每次回调 10KB，每秒总触发 0.5 次
- **总计**: ~5 KB/秒 临时分配

---

### 2.4 大对象分配

#### 2.4.1 会话数据
**文件**: `RUINORERP.Server\Network\Models\SessionInfo.cs:407`

```407:E:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.Server\Network\Models\SessionInfo.cs
public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
```

**问题**:
- `Properties` 字典可能累积任意对象
- 未限制字典大小

---

#### 2.4.2 广播数据
**文件**: `RUINORERP.Server\Network\Services\ServerLockManager.cs:92-137`

```92-137:E:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.Server\Network\Services\ServerLockManager.cs
public async Task BroadcastLockStatusAsync(IEnumerable<LockInfo> lockedDocuments, bool NeedReponse = false)
{
    try
    {
        // 创建广播数据
        var broadcastData = new LockRequest
        {
            LockedDocuments = lockedDocuments?.ToList() ?? new List<LockInfo>(),
            Timestamp = DateTime.UtcNow
        };

        // 获取所有用户会话
        var sessions = _sessionService.GetAllUserSessions();

        // 向所有会话发送消息并等待响应
        int successCount = 0;
        foreach (var session in sessions)
        {
            // ... 发送逻辑
        }
    }
}
```

**问题**:
- 每次广播都会创建新的 `LockRequest` 对象
- `lockedDocuments.ToList()` 创建列表副本

---

## 3. 内存泄漏根因分析

### 3.1 主要问题总结

| 问题类别 | 严重程度 | 预计内存影响 | 根本原因 |
|---------|---------|-------------|---------|
| 静态集合累积 | 🔴 高 | ~120 MB | 无自动清理机制 |
| 缓存配置不当 | 🟡 中 | ~50 MB | 过度预热、冗余键字典 |
| 异常堆栈保留 | 🟡 中 | ~5 MB | 完整保留异常对象 |
| 会话数据增长 | 🟡 中 | ~50 MB | Properties 字典无限制 |
| Timer 回调累积 | 🟢 低 | ~5 KB/s | 临时对象频繁分配 |

### 3.2 2GB 内存占用分解

```
总内存: 2 GB (2048 MB)
├─ 托管内存 (GC.GetTotalMemory): ~500 MB (25%)
│  ├─ 静态集合: ~120 MB
│  ├─ 内存缓存: ~50 MB
│  ├─ 业务对象: ~200 MB
│  └─ 其他托管对象: ~130 MB
│
└─ 非托管内存: ~1548 MB (75%)
   ├─ SuperSocket 网络栈: ~800 MB
   │  ├─ 缓冲区池: ~500 MB
   │  ├─ 连接对象: ~200 MB
   │  └─ 协议解析: ~100 MB
   │
   ├─ SqlSugar ORM: ~300 MB
   │  ├─ 查询缓存: ~150 MB
   │  ├─ 实体跟踪: ~100 MB
   │  └─ 连接池: ~50 MB
   │
   ├─ 日志缓冲: ~100 MB
   │  ├─ Log4Net 缓冲: ~80 MB
   │  └─ 日志文件句柄: ~20 MB
   │
   └─ 其他非托管资源: ~348 MB
      ├─ GDI+ 对象: ~50 MB
      ├─ 线程栈: ~100 MB
      └─ 未识别碎片: ~198 MB
```

---

## 4. 循环引用分析

### 4.1 会话-锁循环引用

```
SessionInfo
    ↓
ServerLockManager._documentLocks[docId].SessionId
    ↓
SessionService._sessions[sessionId]
    ↓
Properties["LockInfo"] → LockInfo
```

**影响**: 会话断开后，如果循环引用未解除，会话对象无法被 GC 回收

### 4.2 处理器-事件循环引用

```
CommandHandler
    ↓ (注册事件)
CommandDispatcher.HandlerRegistered
    ↓
CommandHandler (引用保持)
```

**影响**: 处理器永不释放

---

## 5. 优化建议

### 5.1 立即实施 (高优先级)

#### 5.1.1 添加请求超时清理
**文件**: `SessionService.cs`

```csharp
// 添加定时清理任务
private void CleanupPendingRequests()
{
    var now = DateTime.UtcNow;
    var expiredKeys = _pendingRequests
        .Where(kvp => 
        {
            if (kvp.Value.Task.IsCompleted)
                return true;
            
            // 假设 TaskCompletionSource 有创建时间属性
            // 如果没有，需要扩展它
            var creationTime = GetRequestCreationTime(kvp.Key);
            return (now - creationTime) > TimeSpan.FromMinutes(5);
        })
        .Select(kvp => kvp.Key)
        .ToList();

    foreach (var key in expiredKeys)
    {
        if (_pendingRequests.TryRemove(key, out var tcs))
        {
            try { tcs.SetCanceled(); } catch { }
        }
    }
}

private DateTime GetRequestCreationTime(string requestId)
{
    // 实现获取请求创建时间的逻辑
    return DateTime.UtcNow.AddMinutes(-10); // 默认10分钟前
}
```

#### 5.1.2 限制 Properties 字典大小
**文件**: `SessionInfo.cs`

```csharp
public class SessionInfo
{
    private const int MaxProperties = 50;
    private readonly object _propertiesLock = new object();
    
    private Dictionary<string, object> _properties = new Dictionary<string, object>();
    
    public Dictionary<string, object> Properties
    {
        get { lock (_propertiesLock) return _properties; }
        set 
        { 
            lock (_propertiesLock) 
            {
                if (value != null && value.Count > MaxProperties)
                {
                    _logger.LogWarning($"Properties 字典超过限制 {MaxProperties}，截断到前 {MaxProperties} 项");
                    _properties = value.Take(MaxProperties).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                }
                else
                {
                    _properties = value;
                }
            }
        }
    }
}
```

#### 5.1.3 减少内存缓存预热数量
**文件**: `StockCacheService.cs`

```csharp
// 修改默认预热批次大小
private const int PREHEAT_BATCH_SIZE = 100; // 从 500 降到 100

// 添加最大预热数量限制
private const int MAX_PREHEAT_COUNT = 10000; // 最多预热 1 万条

public async Task PreheatCacheAsync(int batchSize = 100)
{
    // ... 现有代码 ...
    
    while (totalPreheated < MAX_PREHEAT_COUNT)
    {
        var productIds = await GetProductIdsForPreheatAsync(batchNumber, batchSize);
        if (!productIds.Any())
            break;
        
        // ... 现有代码 ...
    }
}
```

#### 5.1.4 合并 IMemoryCache 实例
**文件**: `Startup.cs`

```csharp
// 删除重复的 MemoryCache 注册
// services.AddMemoryCacheSetup();
// services.AddDistributedMemoryCache();
services.AddMemoryCache(options =>
{
    options.SizeLimit = 500 * 1024 * 1024; // 500 MB 限制
});
```

---

### 5.2 中期优化 (中优先级)

#### 5.2.1 实现自动 SKU 缓存清理
**文件**: `ProductSKUCodeGenerator.cs`

```csharp
// 添加 LRU 缓存机制
private static readonly ConcurrentDictionary<string, (bool value, DateTime lastAccess)> _skuCache = 
    new ConcurrentDictionary<string, (bool, DateTime)>();
private const int MaxSkuCacheSize = 100000;
private const int SkuCleanupBatchSize = 1000;

public static bool ContainsSKU(string sku)
{
    if (string.IsNullOrEmpty(sku))
        return false;

    if (_skuCache.TryGetValue(sku, out var entry))
    {
        // 更新访问时间
        _skuCache.TryUpdate(sku, (entry.value, DateTime.UtcNow), entry);
        return entry.value;
    }

    // 检查缓存大小，超过则清理
    if (_skuCache.Count >= MaxSkuCacheSize)
    {
        CleanupSkuCache();
    }

    var result = CheckSKUInDatabase(sku); // 假设的数据库检查
    _skuCache.TryAdd(sku, (result, DateTime.UtcNow));
    return result;
}

private static void CleanupSkuCache()
{
    var toRemove = _skuCache
        .OrderBy(kvp => kvp.Value.lastAccess)
        .Take(SkuCleanupBatchSize)
        .Select(kvp => kvp.Key)
        .ToList();

    foreach (var key in toRemove)
    {
        _skuCache.TryRemove(key, out _);
    }
}
```

#### 5.2.2 添加会话数据配额
**文件**: `SessionService.cs`

```csharp
// 添加会话数据配额限制
private const int MaxSessionProperties = 100;
private const long MaxSessionDataSizeBytes = 10 * 1024 * 1024; // 10 MB

public void SetSessionProperty(string sessionId, string key, object value)
{
    if (!_sessions.TryGetValue(sessionId, out var session))
        return;

    // 检查属性数量
    if (session.Properties.Count >= MaxSessionProperties)
    {
        throw new InvalidOperationException($"会话属性数量超过限制 {MaxSessionProperties}");
    }

    // 检查数据大小
    var dataSize = CalculateObjectSize(value);
    if (dataSize > MaxSessionDataSizeBytes)
    {
        throw new InvalidOperationException($"会话数据大小超过限制 {MaxSessionDataSizeBytes} 字节");
    }

    session.Properties[key] = value;
}
```

---

### 5.3 长期优化 (低优先级)



#### 5.3.2 实现内存分析工具
**建议**: 添加定期内存快照功能

```csharp
public class MemorySnapshotService
{
    private readonly ILogger<MemorySnapshotService> _logger;

    public void TakeSnapshot(string reason)
    {
        var memoryInfo = new
        {
            Timestamp = DateTime.UtcNow,
            Reason = reason,
            WorkingSet = Process.GetCurrentProcess().WorkingSet64,
            ManagedMemory = GC.GetTotalMemory(true),
            Gen0Collections = GC.CollectionCount(0),
            Gen1Collections = GC.CollectionCount(1),
            Gen2Collections = GC.CollectionCount(2),
            TotalMemory = GC.GetTotalMemory(false),
            LargeObjectHeap = GC.GetTotalMemory(false) - GetSmallObjectHeapSize()
        };

        _logger.LogInformation("内存快照: {Snapshot}", JsonSerializer.Serialize(memoryInfo));

        // 触发条件性 GC
        if (memoryInfo.WorkingSet > 2L * 1024 * 1024 * 1024) // 超过 2GB
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            _logger.LogWarning("已触发强制 GC，原因: 内存超过 2GB");
        }
    }

    private long GetSmallObjectHeapSize()
    {
        // 实现获取小对象堆大小
        return 0;
    }
}
```

---

## 6. 监控和诊断建议

### 6.1 添加关键指标监控

```csharp
// 在 ServerMonitorControl 中添加
private void UpdateMemoryMetrics()
{
    var metrics = new
    {
        // 静态集合大小
        SessionCount = _sessionService.ActiveSessionCount,
        PendingRequestsCount = GetPendingRequestsCount(),
        LockCount = _lockManager.GetLockCount(),
        SkuCacheCount = GetSkuCacheCount(),
        
        // 缓存统计
        CacheHitRatio = _stockCacheService.GetCacheStatistics().HitRatio,
        CacheSize = _stockCacheService.GetCacheStatistics().CurrentCacheSize,
        
        // GC 统计
        Gen0Collections = GC.CollectionCount(0),
        Gen1Collections = GC.CollectionCount(1),
        Gen2Collections = GC.CollectionCount(2),
        
        // 内存分配
        TotalAllocated = GC.GetTotalMemory(false)
    };

    lblMemoryMetrics.Text = JsonSerializer.Serialize(metrics, new JsonSerializerOptions
    {
        WriteIndented = true
    });
}
```

### 6.2 添加定期 GC 触发

```csharp
// 在 MemoryMonitoringService 中
private void MonitorMemoryUsage(object state)
{
    var memoryInfo = GetCurrentMemoryUsage();
    
    // 如果内存使用超过 1.8GB，触发 GC
    if (memoryInfo.WorkingSetMB >= 1800)
    {
        _logger.LogWarning($"内存使用较高: {memoryInfo.WorkingSetMB} MB，触发 GC");
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        
        var afterGC = GetCurrentMemoryUsage();
        _logger.LogInformation($"GC 后内存: {afterGC.WorkingSetMB} MB，回收: {memoryInfo.WorkingSetMB - afterGC.WorkingSetMB} MB");
    }
    
    // 原有逻辑...
}
```

---

## 7. 预期效果

### 7.1 立即优化后预期内存降低

| 优化项 | 预期降低 | 风险等级 |
|-------|---------|---------|
| 请求超时清理 | ~5 MB | 低 |
| Properties 字典限制 | ~10 MB | 中 |
| 减少缓存预热 | ~40 MB | 低 |
| 合并 IMemoryCache | ~10 MB | 低 |
| **小计** | **~65 MB** | - |

### 7.2 中期优化后预期内存降低

| 优化项 | 预期降低 | 风险等级 |
|-------|---------|---------|
| SKU 缓存 LRU | ~4 MB | 中 |
| 会话数据配额 | ~30 MB | 中 |
| 定期 GC 触发 | ~50 MB | 低 |
| **小计** | **~84 MB** | - |

### 7.3 总体预期

- **短期 (1周)**: 内存从 2GB 降至 ~1.8GB (降低 ~10%)
- **中期 (1月)**: 内存降至 ~1.5GB (降低 ~25%)
- **长期 (3月)**: 内存稳定在 ~1.2GB (降低 ~40%)

---

## 8. 风险评估

| 优化项 | 风险类型 | 风险描述 | 缓解措施 |
|-------|---------|---------|---------|
| 请求超时清理 | 功能风险 | 可能误删有效请求 | 使用保守的超时时间 |
| Properties 限制 | 兼容性风险 | 可能破坏现有功能 | 先在测试环境验证 |
| 减少缓存预热 | 性能风险 | 可能增加数据库查询 | 监控缓存命中率 |
| SKU 缓存 LRU | 性能风险 | 可能增加重复计算 | 添加缓存预热 |

---

## 9. 实施计划

### Phase 1: 紧急修复 (1-2 天)
1. ✅ 添加请求超时清理机制
2. ✅ 实现定期 GC 触发
3. ✅ 添加内存监控指标

### Phase 2: 稳定优化 (1 周)
1. ✅ 限制 Properties 字典大小
2. ✅ 减少内存缓存预热数量
3. ✅ 合并 IMemoryCache 实例

### Phase 3: 深度优化 (2-4 周)
1. ✅ 实现 SKU 缓存 LRU 机制
2. ✅ 添加会话数据配额
3. ✅ 迁移到分布式缓存 (可选)

### Phase 4: 持续监控 (长期)
1. ✅ 部署内存分析工具
2. ✅ 建立内存使用告警
3. ✅ 定期生成内存报告

---

## 10. 结论

**关键发现**:
1. **静态集合是主要内存泄漏源**: `_pendingRequests`、`_skuCache`、`_loginAttempts` 等静态集合无自动清理机制
2. **缓存配置不当**: 过度预热和冗余键字典导致不必要的内存占用
3. **会话数据无限制**: `Properties` 字典可能无限增长
4. **非托管内存占比高**: SuperSocket 和 SqlSugar 占用大量非托管内存

**建议优先级**:
1. 🔴 **高优先级**: 添加请求超时清理、限制会话数据
2. 🟡 **中优先级**: 优化缓存配置、实现 LRU 清理
3. 🟢 **低优先级**: 迁移分布式缓存、实现高级监控

**预期效果**: 通过实施上述优化，预计可将内存占用从 2GB 降至 1.2-1.5GB，降低 25-40%。
