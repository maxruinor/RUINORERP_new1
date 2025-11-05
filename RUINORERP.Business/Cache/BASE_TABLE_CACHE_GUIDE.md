# 基础表缓存管理器使用指南

## 1. 功能概述

基础表缓存管理器 (BaseTableCacheManager) 是对现有缓存系统的扩展，专门用于管理和监控基础表的缓存信息。它提供以下核心功能：

- **缓存信息监控**：获取所有基础表的缓存状态，包括表名、行数、更新时间等
- **缓存完整性验证**：验证缓存数据是否完整，特别是在网络问题后
- **自动检测不完整缓存**：自动识别缓存数据缺失或不匹配的表
- **缓存修复机制**：提供工具帮助重新加载和修复不完整的缓存
- **健康状态报告**：生成详细的缓存健康状态报告

## 2. 解决的问题

这个功能主要解决以下场景中的问题：

- **网络波动导致的缓存不完整**：当客户端因网络问题未能完整获取基础表数据时，可以通过对比预期行数和实际缓存行数来检测
- **缓存数据一致性验证**：确保客户端缓存的数据与服务器端预期的数据量一致
- **缓存健康监控**：提供机制监控系统中所有基础表的缓存状态
- **自动恢复机制**：当发现缓存不完整时，可以触发重新加载操作

## 3. 核心组件

### 3.1 接口与实现

- **`IBaseTableCacheManager`**：定义基础表缓存管理的核心功能接口
- **`BaseTableCacheManager`**：接口的具体实现类
- **`BaseTableCacheInfo`**：存储单个表的缓存信息数据结构
- **`BaseTableCacheExtensions`**：扩展方法集合，提供便捷功能

### 3.2 关键数据结构

**`BaseTableCacheInfo`** 类包含以下主要属性：

| 属性名 | 类型 | 描述 |
|--------|------|------|
| `TableName` | string | 表名 |
| `DataCount` | int | 数据行数 |
| `LastUpdateTime` | DateTime | 最后更新时间 |
| `ExpirationTime` | DateTime | 过期时间 |
| `HasExpiration` | bool | 是否有过期设置 |
| `EstimatedSize` | long | 估计内存大小（字节） |
| `SourceInfo` | string | 源信息 |
| `IsExpired` | bool | 缓存是否已过期（只读） |
| `IsEmpty` | bool | 缓存是否为空（只读） |
| `ReadableSize` | string | 可读的大小描述（只读） |
| `StatusDescription` | string | 状态描述（只读） |

## 4. 使用方法

### 4.1 依赖注入配置

在应用程序的启动配置中，将 `IBaseTableCacheManager` 注册为依赖项：

```csharp
// 在 Startup.cs 或 Program.cs 中
public void ConfigureServices(IServiceCollection services)
{
    // ... 现有配置 ...
    
    // 注册基础表缓存管理器
    services.AddScoped<IBaseTableCacheManager, BaseTableCacheManager>();
    
    // ... 其他配置 ...
}
```

### 4.2 基本使用示例

#### 获取所有基础表的缓存信息

```csharp
public class SomeService
{
    private readonly IBaseTableCacheManager _baseTableCacheManager;
    
    public SomeService(IBaseTableCacheManager baseTableCacheManager)
    {
        _baseTableCacheManager = baseTableCacheManager;
    }
    
    public void CheckCacheStatus()
    {
        // 获取所有基础表的缓存信息
        var allTablesInfo = _baseTableCacheManager.GetAllBaseTablesCacheInfo();
        
        foreach (var tableInfo in allTablesInfo)
        {
            Console.WriteLine($"表: {tableInfo.TableName}, 行数: {tableInfo.DataCount}, 状态: {tableInfo.StatusDescription}");
        }
    }
}
```

#### 验证特定表的缓存完整性

```csharp
public bool ValidateEmployeeTableCache()
{
    // 验证员工表缓存是否完整
    bool isIntegrity = _baseTableCacheManager.ValidateTableCacheIntegrity("Employees");
    
    if (!isIntegrity)
    {
        Console.WriteLine("员工表缓存不完整，需要重新加载");
        // 触发重新加载操作
    }
    
    return isIntegrity;
}
```

#### 获取并修复不完整的缓存

```csharp
public void FixIncompleteCaches()
{
    // 获取所有缓存不完整的表
    var incompleteTables = _baseTableCacheManager.GetTablesWithIncompleteCache();
    
    Console.WriteLine($"发现 {incompleteTables.Count} 个表的缓存不完整");
    
    // 刷新不完整的表
    int fixedCount = _baseTableCacheManager.RefreshIncompleteTables(tableName => {
        Console.WriteLine($"正在刷新表: {tableName}");
        // 这里调用实际的数据加载逻辑
        // 例如: _dataService.ReloadTableData(tableName);
    });
    
    Console.WriteLine($"成功修复了 {fixedCount} 个表的缓存");
}
```

### 4.3 高级使用示例

#### 使用扩展方法生成缓存状态报告

```csharp
public string GenerateCacheReport()
{
    // 生成详细的缓存状态报告
    string report = _baseTableCacheManager.GetCacheStatusReport();
    
    // 可以将报告保存到日志或返回给前端
    return report;
}
```

#### 检查并修复特定表的缓存

```csharp
public class CacheService
{
    private readonly IEntityCacheManager _cacheManager;
    private readonly ICacheSyncMetadata _cacheSyncMetadata;
    private readonly IDataService _dataService;
    
    public CacheService(
        IEntityCacheManager cacheManager,
        ICacheSyncMetadata cacheSyncMetadata,
        IDataService dataService)
    {
        _cacheManager = cacheManager;
        _cacheSyncMetadata = cacheSyncMetadata;
        _dataService = dataService;
    }
    
    public bool EnsureDepartmentTableCache()
    {
        // 使用扩展方法检查并修复缓存
        return _cacheManager.CheckAndFixCacheIntegrity(
            _cacheSyncMetadata,
            "Departments",
            () => _dataService.GetAllDepartments());
    }
}
```

## 5. 在客户端应用中的应用

在客户端应用程序中，可以通过API调用获取基础表缓存信息，然后进行验证：

```csharp
public class ClientCacheValidator
{
    private readonly IApiClient _apiClient;
    private readonly LocalCacheManager _localCache;
    
    public async Task ValidateAllCaches()
    {
        try
        {
            // 从服务器获取基础表缓存信息
            var serverCacheInfo = await _apiClient.GetBaseTablesCacheInfo();
            
            // 检查每个表的缓存完整性
            foreach (var tableInfo in serverCacheInfo)
            {
                // 获取本地缓存中的数据行数
                int localCount = _localCache.GetEntityCount(tableInfo.TableName);
                
                // 验证行数是否匹配
                if (tableInfo.DataCount > 0 && localCount != tableInfo.DataCount)
                {
                    Console.WriteLine($"表 {tableInfo.TableName} 缓存不完整: 服务器 {tableInfo.DataCount} 行, 本地 {localCount} 行");
                    // 触发重新下载
                    await ReloadTableData(tableInfo.TableName);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"验证缓存时发生错误: {ex.Message}");
        }
    }
    
    private async Task ReloadTableData(string tableName)
    {
        try
        {
            Console.WriteLine($"正在重新加载表 {tableName} 的数据");
            var data = await _apiClient.GetTableData(tableName);
            _localCache.UpdateTableData(tableName, data);
            Console.WriteLine($"表 {tableName} 数据重新加载成功");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"重新加载表 {tableName} 时发生错误: {ex.Message}");
        }
    }
}
```

## 6. 集成最佳实践

### 6.1 定期验证缓存

在应用程序启动时或定期（如每天）运行缓存验证，确保缓存数据的完整性：

```csharp
public class CacheValidationHostedService : BackgroundService
{
    private readonly IBaseTableCacheManager _baseTableCacheManager;
    private readonly ILogger<CacheValidationHostedService> _logger;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("开始执行定期缓存验证");
                
                // 生成缓存健康报告
                string report = _baseTableCacheManager.GetCacheStatusReport();
                _logger.LogInformation("缓存验证报告: {Report}", report);
                
                // 检查并修复不完整的缓存
                var incompleteTables = _baseTableCacheManager.GetTablesWithIncompleteCache();
                if (incompleteTables.Any())
                {
                    _logger.LogWarning("发现 {Count} 个表的缓存不完整，开始修复", incompleteTables.Count);
                    // 执行修复逻辑...
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "定期缓存验证过程中发生错误");
            }
            
            // 每24小时运行一次
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}
```

### 6.2 与网络状态监听结合

将缓存验证与网络状态监听结合，在网络恢复后自动验证和修复缓存：

```csharp
public class NetworkAwareCacheManager
{
    private readonly IBaseTableCacheManager _baseTableCacheManager;
    private readonly INetworkMonitor _networkMonitor;
    
    public NetworkAwareCacheManager(
        IBaseTableCacheManager baseTableCacheManager,
        INetworkMonitor networkMonitor)
    {
        _baseTableCacheManager = baseTableCacheManager;
        _networkMonitor = networkMonitor;
        
        // 订阅网络状态变化事件
        _networkMonitor.NetworkReconnected += OnNetworkReconnected;
    }
    
    private void OnNetworkReconnected(object sender, EventArgs e)
    {
        // 网络恢复后验证所有缓存
        Task.Run(async () => {
            try
            {
                // 等待网络完全稳定
                await Task.Delay(5000);
                
                // 验证并修复不完整的缓存
                var incompleteTables = _baseTableCacheManager.GetTablesWithIncompleteCache();
                if (incompleteTables.Any())
                {
                    await ReloadIncompleteTables(incompleteTables);
                }
            }
            catch (Exception)
            {
                // 处理异常
            }
        });
    }
    
    private async Task ReloadIncompleteTables(List<string> tableNames)
    {
        // 实现表数据重新加载逻辑
    }
}
```

## 7. 性能考虑

- **内存占用**：`BaseTableCacheInfo` 对象本身很小，不会显著增加内存占用
- **验证开销**：验证操作主要涉及读取缓存和计数，开销较小
- **并发访问**：使用了线程安全的设计，可以在多线程环境下安全使用
- **批处理优化**：对于大量表的验证，可以使用批处理方式提高效率

## 8. 常见问题与解决方案

### 8.1 缓存信息不准确

**症状**：报告的缓存行数与实际不符

**解决方案**：
- 确保在每次更新缓存后正确更新同步元数据
- 使用 `UpdateEntityListWithSync` 方法代替直接更新缓存

### 8.2 验证性能问题

**症状**：验证大量表时性能较差

**解决方案**：
- 采用异步验证方式
- 只验证关键表或分批验证
- 缓存验证结果，避免频繁验证

### 8.3 修复操作失败

**症状**：尝试修复不完整缓存时失败

**解决方案**：
- 检查数据加载逻辑是否正确
- 增加重试机制
- 记录详细日志以便排查问题

## 9. 版本兼容性

- 此功能与现有的缓存系统完全兼容
- 需要 .NET Core 3.1 或更高版本
- 依赖于现有的 `IEntityCacheManager` 和 `ICacheSyncMetadata` 接口

## 10. 后续改进方向

- 添加更细粒度的缓存监控指标
- 实现缓存预热机制
- 添加缓存使用统计分析
- 支持自定义验证规则
- 提供缓存可视化界面