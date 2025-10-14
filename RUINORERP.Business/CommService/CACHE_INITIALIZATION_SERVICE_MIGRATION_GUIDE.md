# 缓存初始化服务迁移指南

## 概述

本文档说明如何从旧的缓存初始化服务迁移到新的优化缓存初始化服务。

## 1. 旧缓存初始化服务（已过时）

### 1.1 类名
[CacheInitializationService](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/CommService/CacheInitializationService.cs#L13-L568)

### 1.2 特点
- 依赖于旧的[MyCacheManager](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Extensions/Middlewares/MyCacheManager.cs#L17-L982)类
- 使用[BaseCacheDataList](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Extensions/Middlewares/BaseCacheDataList.cs#L19-L439)管理表结构信息
- 与旧架构紧密耦合
- 保持向后兼容性

**注意：此类已被标记为过时，请使用新的缓存初始化服务。**

### 1.3 使用场景
- 现有代码仍在使用旧缓存架构的部分
- 需要保持向后兼容性的场景

## 2. 新优化缓存初始化服务

### 2.1 类名
[OptimizedCacheInitializationService](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/CommService/OptimizedCacheInitializationService.cs#L11-L290)

### 2.2 特点
- 专门为新的优化缓存管理器设计
- 依赖于新的[ICacheManager](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/CommService/ICacheManager.cs#L12-L99)接口
- 使用[TableSchemaManager](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/CommService/TableSchemaManager.cs#L12-L427)管理表结构信息
- 与新架构紧密集成
- 提供更好的性能和可维护性

### 2.3 使用场景
- 新开发的功能模块
- 需要更高性能的缓存操作
- 可以接受不向后兼容的场景

## 3. 迁移步骤

### 3.1 依赖注入配置
新服务已在[BusinessDIConfig.cs](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/DI/BusinessDIConfig.cs)中注册：

```csharp
// 注册新的优化缓存初始化服务
builder.RegisterType<OptimizedCacheInitializationService>()
    .AsSelf()
    .SingleInstance()
    .PropertiesAutowired();
```

### 3.2 使用新服务
在控制器或服务中注入并使用新服务：

```csharp
public class SomeController
{
    private readonly OptimizedCacheInitializationService _cacheInitService;
    
    public SomeController(OptimizedCacheInitializationService cacheInitService)
    {
        _cacheInitService = cacheInitService;
    }
    
    public async Task InitializeCache()
    {
        await _cacheInitService.InitializeAllCacheAsync();
    }
}
```

### 3.3 序列化支持
新服务提供了更好的序列化支持：

```csharp
// 序列化数据以便在网络中传输
var serializedData = _cacheInitService.SerializeCacheDataForTransmission(someData, CacheSerializationHelper.SerializationType.MessagePack);

// 反序列化从网络接收到的数据
var deserializedData = _cacheInitService.DeserializeCacheDataFromTransmission<SomeType>(receivedData, CacheSerializationHelper.SerializationType.Json);
```

## 4. 注意事项

1. **向后兼容性**：旧的[CacheInitializationService](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/CommService/CacheInitializationService.cs#L13-L568)仍然可用，以确保现有代码不受影响
2. **并行使用**：在迁移过程中，可以同时使用新旧两种服务
3. **性能提升**：新服务提供了更好的性能，特别是在高并发场景下
4. **维护性**：新服务具有更清晰的代码结构和更好的可维护性

## 5. 迁移建议

1. **新功能开发**：所有新功能应使用[OptimizedCacheInitializationService](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/CommService/OptimizedCacheInitializationService.cs#L11-L290)
2. **逐步迁移**：对于现有功能，可以逐步迁移到新服务
3. **测试验证**：在迁移后进行充分的测试，确保功能正常
4. **监控性能**：监控新服务的性能表现，确保达到预期效果