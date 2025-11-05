# 缓存系统扩展指南

## 1. 系统概述

RUINORERP系统采用了统一的缓存体系架构，基于 `IEntityCacheManager` 接口和 `EntityCacheManager` 实现类。该缓存系统提供了高效的内存管理、缓存一致性保障和完整的统计功能。

### 1.1 核心组件

- **IEntityCacheManager 接口**：定义了缓存系统的核心操作方法
- **EntityCacheManager 类**：提供了完整的缓存管理实现
- **ICacheStatistics 接口**：提供缓存统计功能
- **EntityCacheHelper 静态类**：提供便捷的缓存访问方式

### 1.2 现有缓存类型

当前系统支持四种缓存类型（通过 `CacheKeyType` 枚举）：

- **List**：实体列表缓存
- **Entity**：单个实体缓存
- **DisplayValue**：显示值缓存
- **QueryResult**：自定义查询结果缓存

## 2. 扩展缓存类型

要扩展新的缓存内容，最直接的方法是在 `CacheKeyType` 枚举中添加新的类型，并在 `GenerateCacheKey` 方法中处理新类型的键生成逻辑。

### 2.1 修改 CacheKeyType 枚举

```csharp
/// <summary>
/// 缓存键类型枚举，用于区分不同类型的缓存
/// </summary>
enum CacheKeyType
{
    /// <summary>
    /// 实体列表缓存
    /// </summary>
    List,
    /// <summary>
    /// 单个实体缓存
    /// </summary>
    Entity,
    /// <summary>
    /// 显示值缓存
    /// </summary>
    DisplayValue,
    /// <summary>
    /// 自定义查询缓存
    /// </summary>
    QueryResult,
    /// <summary>
    /// 工作流实例缓存
    /// </summary>
    WorkflowInstance,
    /// <summary>
    /// 处理中的单据缓存
    /// </summary>
    ProcessingDocument
}
```

### 2.2 更新 GenerateCacheKey 方法

修改 `EntityCacheManager.cs` 中的 `GenerateCacheKey` 方法，为新的缓存类型添加键生成逻辑：

```csharp
public string GenerateCacheKey(CacheKeyType keyType, string tableName, object primaryKeyValue = null)
{
    try
    {
        if (string.IsNullOrEmpty(tableName))
        {
            throw new ArgumentNullException(nameof(tableName), "表名不能为空");
        }

        // 根据不同的缓存类型生成不同格式的缓存键
        switch (keyType)
        {
            // 现有缓存类型...
            
            case CacheKeyType.WorkflowInstance:
                // 工作流实例缓存键格式：Workflow_{工作流类型}_Instance_{实例ID}
                return $"Workflow_{tableName}_Instance_{primaryKeyValue ?? string.Empty}";
                
            case CacheKeyType.ProcessingDocument:
                // 处理中的单据缓存键格式：Doc_{单据类型}_Processing_{单据ID}
                return $"Doc_{tableName}_Processing_{primaryKeyValue ?? string.Empty}";
                
            default:
                // 对于未明确支持的类型，记录警告并使用默认格式
                _logger?.LogWarning($"不支持的缓存键类型: {keyType}，使用默认格式");
                return $"Table_{tableName}_{keyType}_{primaryKeyValue ?? string.Empty}";
        }
    }
    catch (Exception ex)
    {
        _logger?.LogError(ex, $"生成缓存键时发生错误。类型: {keyType}, 表名: {tableName}");
        throw;
    }
}
```

## 3. 创建专用的缓存管理类

对于特定业务域，可以创建专用的缓存管理类，封装通用缓存操作。以下是工作流和单据缓存的示例实现。

### 3.1 工作流缓存管理器

```csharp
/// <summary>
/// 工作流缓存管理器
/// 负责管理工作流相关的缓存操作
/// </summary>
public class WorkflowCacheManager
{
    private readonly IEntityCacheManager _cacheManager;
    private readonly ILogger<WorkflowCacheManager> _logger;
    
    public WorkflowCacheManager(IEntityCacheManager cacheManager, ILogger<WorkflowCacheManager> logger)
    {
        _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    /// <summary>
    /// 缓存工作流实例
    /// </summary>
    /// <param name="workflowType">工作流类型名称</param>
    /// <param name="instanceId">工作流实例ID</param>
    /// <param name="workflowData">工作流数据</param>
    public void CacheWorkflowInstance(string workflowType, string instanceId, object workflowData)
    {
        try
        {
            string cacheKey = _cacheManager.GenerateCacheKey(
                IEntityCacheManager.CacheKeyType.WorkflowInstance, 
                workflowType, 
                instanceId);
            
            _cacheManager.PutToCache(cacheKey, workflowData);
            _logger.LogDebug("已缓存工作流实例: {WorkflowType}-{InstanceId}", workflowType, instanceId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "缓存工作流实例时发生错误: {WorkflowType}-{InstanceId}", workflowType, instanceId);
            throw;
        }
    }
    
    /// <summary>
    /// 获取缓存的工作流实例
    /// </summary>
    /// <typeparam name="T">工作流数据类型</typeparam>
    /// <param name="workflowType">工作流类型名称</param>
    /// <param name="instanceId">工作流实例ID</param>
    /// <returns>缓存的工作流数据，如果不存在则返回null</returns>
    public T GetWorkflowInstance<T>(string workflowType, string instanceId) where T : class
    {
        try
        {
            string cacheKey = _cacheManager.GenerateCacheKey(
                IEntityCacheManager.CacheKeyType.WorkflowInstance, 
                workflowType, 
                instanceId);
            
            return _cacheManager.GetFromCache<T>(cacheKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取缓存工作流实例时发生错误: {WorkflowType}-{InstanceId}", workflowType, instanceId);
            return null;
        }
    }
    
    /// <summary>
    /// 清理工作流实例缓存
    /// </summary>
    /// <param name="workflowType">工作流类型名称</param>
    /// <param name="instanceId">工作流实例ID</param>
    public void ClearWorkflowInstanceCache(string workflowType, string instanceId)
    {
        try
        {
            string cacheKey = _cacheManager.GenerateCacheKey(
                IEntityCacheManager.CacheKeyType.WorkflowInstance, 
                workflowType, 
                instanceId);
            
            _cacheManager.DeleteFromCache(cacheKey);
            _logger.LogDebug("已清理工作流实例缓存: {WorkflowType}-{InstanceId}", workflowType, instanceId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "清理工作流实例缓存时发生错误: {WorkflowType}-{InstanceId}", workflowType, instanceId);
            throw;
        }
    }
    
    /// <summary>
    /// 缓存工作流状态
    /// </summary>
    /// <param name="workflowType">工作流类型名称</param>
    /// <param name="instanceId">工作流实例ID</param>
    /// <param name="status">状态数据</param>
    public void CacheWorkflowStatus(string workflowType, string instanceId, string status)
    {
        try
        {
            // 使用QueryResult类型缓存工作流状态
            string cacheKey = _cacheManager.GenerateCacheKey(
                IEntityCacheManager.CacheKeyType.QueryResult, 
                $"{workflowType}_Status", 
                instanceId);
            
            _cacheManager.PutToCache(cacheKey, status);
            _logger.LogDebug("已缓存工作流状态: {WorkflowType}-{InstanceId}-{Status}", workflowType, instanceId, status);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "缓存工作流状态时发生错误: {WorkflowType}-{InstanceId}", workflowType, instanceId);
            throw;
        }
    }
}
```

### 3.2 单据缓存管理器

```csharp
/// <summary>
/// 单据缓存管理器
/// 负责管理处理中的单据缓存操作
/// </summary>
public class DocumentCacheManager
{
    private readonly IEntityCacheManager _cacheManager;
    private readonly ILogger<DocumentCacheManager> _logger;
    
    public DocumentCacheManager(IEntityCacheManager cacheManager, ILogger<DocumentCacheManager> logger)
    {
        _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    /// <summary>
    /// 缓存处理中的单据
    /// </summary>
    /// <param name="docType">单据类型</param>
    /// <param name="docId">单据ID</param>
    /// <param name="docData">单据数据</param>
    public void CacheProcessingDocument(string docType, long docId, object docData)
    {
        try
        {
            string cacheKey = _cacheManager.GenerateCacheKey(
                IEntityCacheManager.CacheKeyType.ProcessingDocument, 
                docType, 
                docId);
            
            _cacheManager.PutToCache(cacheKey, docData);
            _logger.LogDebug("已缓存处理中的单据: {DocType}-{DocId}", docType, docId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "缓存处理中的单据时发生错误: {DocType}-{DocId}", docType, docId);
            throw;
        }
    }
    
    /// <summary>
    /// 获取缓存的处理中单据
    /// </summary>
    /// <typeparam name="T">单据数据类型</typeparam>
    /// <param name="docType">单据类型</param>
    /// <param name="docId">单据ID</param>
    /// <returns>缓存的单据数据，如果不存在则返回null</returns>
    public T GetProcessingDocument<T>(string docType, long docId) where T : class
    {
        try
        {
            string cacheKey = _cacheManager.GenerateCacheKey(
                IEntityCacheManager.CacheKeyType.ProcessingDocument, 
                docType, 
                docId);
            
            return _cacheManager.GetFromCache<T>(cacheKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取缓存的处理中单据时发生错误: {DocType}-{DocId}", docType, docId);
            return null;
        }
    }
    
    /// <summary>
    /// 清理处理中单据缓存
    /// </summary>
    /// <param name="docType">单据类型</param>
    /// <param name="docId">单据ID</param>
    public void ClearProcessingDocumentCache(string docType, long docId)
    {
        try
        {
            string cacheKey = _cacheManager.GenerateCacheKey(
                IEntityCacheManager.CacheKeyType.ProcessingDocument, 
                docType, 
                docId);
            
            _cacheManager.DeleteFromCache(cacheKey);
            _logger.LogDebug("已清理处理中单据缓存: {DocType}-{DocId}", docType, docId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "清理处理中单据缓存时发生错误: {DocType}-{DocId}", docType, docId);
            throw;
        }
    }
    
    /// <summary>
    /// 批量缓存处理中的单据
    /// </summary>
    /// <typeparam name="T">单据数据类型</typeparam>
    /// <param name="docType">单据类型</param>
    /// <param name="documents">单据数据集合</param>
    /// <param name="idSelector">ID选择器函数</param>
    public void BatchCacheProcessingDocuments<T>(string docType, IEnumerable<T> documents, Func<T, long> idSelector)
    {
        if (documents == null || !documents.Any())
            return;
            
        try
        {
            foreach (var doc in documents)
            {
                long docId = idSelector(doc);
                CacheProcessingDocument(docType, docId, doc);
            }
            _logger.LogDebug("已批量缓存处理中的单据: {DocType}, 数量: {Count}", docType, documents.Count());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "批量缓存处理中的单据时发生错误: {DocType}", docType);
            throw;
        }
    }
}
```

## 4. 利用现有内存管理机制

新添加的缓存类型会自动利用现有的内存管理机制，包括：

### 4.1 缓存大小监控和LRU清理

系统会自动监控缓存大小，并在达到阈值时使用LRU（最少最近使用）策略清理缓存：

```csharp
// 系统已有的内存管理机制（在EntityCacheManager中）
private void CheckAndCleanCacheSize()
{
    if (EstimatedCacheSize > _cacheSizeThreshold)
    {
        _logger.LogInformation("缓存大小达到阈值({Threshold}MB)，开始清理缓存。当前大小：{CurrentSize}MB",
            _cacheSizeThreshold / (1024 * 1024), EstimatedCacheSize / (1024 * 1024));
        CleanCacheByLeastRecentlyUsed();
    }
}

private void CleanCacheByLeastRecentlyUsed()
{
    // 按最少使用原则排序并移除10%缓存项
    // ...现有实现...
}
```

### 4.2 缓存统计

新的缓存类型也会自动纳入缓存统计系统，可以通过 `ICacheStatistics` 接口获取统计信息：

```csharp
// 在专用缓存管理器中添加获取统计信息的方法
public ICacheStatistics GetCacheStatistics()
{
    return _cacheManager as ICacheStatistics;
}

// 使用示例
public void LogWorkflowCacheStats()
{
    var stats = GetCacheStatistics();
    if (stats != null)
    {
        _logger.LogInformation("工作流缓存统计 - 总项数: {ItemCount}, 命中率: {HitRatio:P2}, 估计大小: {Size}MB",
            stats.CacheItemCount, stats.HitRatio, stats.EstimatedCacheSize / (1024 * 1024));
    }
}
```

## 5. 扩展方法列表

| 方法名 | 说明 | 参数 | 返回值 |
|--------|------|------|--------|
| `UpdateEntityListWithSync` | 更新实体列表并记录同步信息 | tableName, entityList, cacheSyncMetadata | void |
| `DefaultSyncMetadata` | 获取默认的同步元数据管理器 | 无 | ICacheSyncMetadata |
| `CreateBaseTableCacheManager` | 创建基础表缓存管理器实例 | cacheManager, cacheSyncMetadata, logger | IBaseTableCacheManager |
| `UpdateEntityListWithIntegrityCheck` | 更新实体列表并验证完整性 | tableName, entityList, cacheSyncMetadata, logger | bool |
| `CheckAndFixCacheIntegrity` | 检查并修复缓存完整性 | tableName, reloadFunction, logger | bool |
| `GetCacheStatusReport` | 获取缓存状态报告 | baseTableCacheManager | string |
| `BatchCheckAndFixCacheIntegrity` | 批量检查并修复缓存完整性 | tableNames, reloadFunction | int |
| `GetCacheHealthScore` | 获取缓存健康评分 | baseTableCacheManager | int |

## 6. 依赖注入配置

为新创建的缓存管理器配置依赖注入：

```csharp
// 在Startup.cs或依赖注入配置类中
public void ConfigureServices(IServiceCollection services)
{
    // 现有缓存服务配置
    services.AddSingleton<IEntityCacheManager, EntityCacheManager>();
    
    // 添加新的专用缓存管理器
    services.AddSingleton<WorkflowCacheManager>();
    services.AddSingleton<DocumentCacheManager>();
    
    // 添加基础表缓存管理器
    services.AddScoped<IBaseTableCacheManager, BaseTableCacheManager>();
    
    // 其他服务配置...
}
```

## 6. 使用示例

### 6.1 工作流缓存使用示例

```csharp
public class WorkflowService
{
    private readonly WorkflowCacheManager _workflowCacheManager;
    
    public WorkflowService(WorkflowCacheManager workflowCacheManager)
    {
        _workflowCacheManager = workflowCacheManager;
    }
    
    public void ProcessWorkflowStep(string workflowType, string instanceId)
    {
        // 先尝试从缓存获取工作流实例
        var workflowInstance = _workflowCacheManager.GetWorkflowInstance<WorkflowData>(workflowType, instanceId);
        
        if (workflowInstance == null)
        {
            // 缓存未命中，从数据库加载
            workflowInstance = LoadWorkflowFromDatabase(workflowType, instanceId);
            
            // 缓存工作流实例
            _workflowCacheManager.CacheWorkflowInstance(workflowType, instanceId, workflowInstance);
        }
        
        // 处理工作流步骤
        ExecuteWorkflowStep(workflowInstance);
        
        // 更新缓存中的工作流状态
        _workflowCacheManager.CacheWorkflowStatus(workflowType, instanceId, workflowInstance.Status);
        
        // 如果工作流已完成，清理缓存
        if (workflowInstance.IsCompleted)
        {
            _workflowCacheManager.ClearWorkflowInstanceCache(workflowType, instanceId);
        }
    }
    
    private WorkflowData LoadWorkflowFromDatabase(string workflowType, string instanceId)
    {
        // 数据库查询实现
        // ...
    }
    
    private void ExecuteWorkflowStep(WorkflowData workflowData)
    {
        // 工作流步骤执行逻辑
        // ...
    }
}
```

### 6.2 单据缓存使用示例

```csharp
public class DocumentProcessingService
{
    private readonly DocumentCacheManager _documentCacheManager;
    
    public DocumentProcessingService(DocumentCacheManager documentCacheManager)
    {
        _documentCacheManager = documentCacheManager;
    }
    
    public void StartDocumentProcessing(string docType, long docId)
    {
        // 加载单据数据
        var docData = LoadDocumentFromDatabase(docType, docId);
        
        // 缓存处理中的单据
        _documentCacheManager.CacheProcessingDocument(docType, docId, docData);
        
        // 开始处理单据
        BeginProcessing(docData);
    }
    
    public void UpdateDocumentStatus(string docType, long docId, string newStatus)
    {
        // 从缓存获取单据
        var docData = _documentCacheManager.GetProcessingDocument<DocumentData>(docType, docId);
        
        if (docData != null)
        {
            // 更新状态
            docData.Status = newStatus;
            
            // 更新缓存
            _documentCacheManager.CacheProcessingDocument(docType, docId, docData);
            
            // 如果处理完成，清理缓存
            if (newStatus == "Completed" || newStatus == "Rejected")
            {
                _documentCacheManager.ClearProcessingDocumentCache(docType, docId);
                
                // 保存到数据库
                SaveDocumentToDatabase(docData);
            }
        }
    }
    
    private DocumentData LoadDocumentFromDatabase(string docType, long docId)
    {
        // 数据库查询实现
        // ...
    }
    
    private void BeginProcessing(DocumentData docData)
    {
        // 开始处理逻辑
        // ...
    }
    
    private void SaveDocumentToDatabase(DocumentData docData)
    {
        // 数据库保存实现
        // ...
    }
}
```

## 7. 扩展缓存统计功能

为了更好地监控新添加的缓存类型，可以扩展 `TableCacheStatistics` 类，添加对新缓存类型的统计：

```csharp
/// <summary>
/// 扩展的表缓存统计信息
/// </summary>
public class ExtendedTableCacheStatistics : TableCacheStatistics
{
    /// <summary>
    /// 工作流实例缓存数量
    /// </summary>
    public int WorkflowInstanceCount { get; set; }
    
    /// <summary>
    /// 处理中的单据缓存数量
    /// </summary>
    public int ProcessingDocumentCount { get; set; }
    
    /// <summary>
    /// 扩展的总缓存项数量
    /// </summary>
    public new int TotalItemCount => 
        base.TotalItemCount + WorkflowInstanceCount + ProcessingDocumentCount;
}
```

## 8. 最佳实践

### 8.1 缓存键设计

- 使用有意义的表名/类型名称作为缓存键的一部分
- 为不同业务域设计统一且可识别的缓存键格式
- 确保缓存键具有唯一性，避免冲突
- 使用 `GenerateCacheKey` 方法生成缓存键，避免手动构造

### 8.2 缓存生命周期管理

- 为不同类型的数据设置合理的缓存过期时间
- 在数据变更后及时更新或清理相关缓存
- 对于临时性数据（如处理中的单据），确保在完成处理后清理缓存
- 利用系统的LRU机制自动管理缓存大小

### 8.3 错误处理

- 添加适当的异常处理和日志记录
- 在缓存操作失败时提供回退机制
- 避免缓存操作错误影响核心业务流程

### 8.4 性能优化

- 对频繁访问的数据优先考虑缓存
- 批量操作优于单条操作
- 避免缓存过大的数据对象，考虑只缓存必要字段
- 使用异步操作处理缓存（如果系统支持）

## 10. 版本历史

| 版本 | 日期 | 说明 |
|------|------|------|
| 1.1 | 当前版本 | 添加基础表缓存管理器功能 |
| 1.0 | 初始版本 | 基础缓存扩展功能 |

## 11. 基础表缓存管理器集成

基础表缓存管理器是对现有缓存系统的重要扩展，专门用于管理和监控基础表的缓存状态。详细使用说明请参考单独的文档：

- [BASE_TABLE_CACHE_GUIDE.md](BASE_TABLE_CACHE_GUIDE.md) - 基础表缓存管理器完整使用指南

### 11.1 快速集成步骤

1. 注册依赖项：
```csharp
services.AddScoped<IBaseTableCacheManager, BaseTableCacheManager>();
```

2. 在需要的地方注入并使用：
```csharp
private readonly IBaseTableCacheManager _baseTableCacheManager;

public MyService(IBaseTableCacheManager baseTableCacheManager)
{
    _baseTableCacheManager = baseTableCacheManager;
}
```

3. 验证缓存完整性：
```csharp
if (!_baseTableCacheManager.ValidateTableCacheIntegrity("Employees"))
{
    // 缓存不完整，需要重新加载
}
```

## 12. 注意事项

- 缓存系统设计为内存缓存，应用重启后缓存会丢失
- 确保缓存操作的线程安全性，特别是在并发场景下
- 缓存大小监控和LRU清理会自动应用于所有缓存类型
- 对于自定义的复杂缓存需求，可能需要扩展核心缓存接口
- 基础表缓存管理器主要用于管理系统基础数据，建议与业务数据缓存分开管理

通过遵循本指南，您可以有效地利用现有的缓存体系架构，扩展支持新的业务需求，同时保持系统的一致性和可维护性。