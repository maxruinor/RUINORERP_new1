# RUINORERP 统一缓存系统概述

本文档提供了RUINORERP系统中缓存系统的完整概述，包括架构设计、组件说明和使用指南。

## 1. 架构设计

### 1.1 核心组件
- [IEntityCacheManager.cs](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/CommService/IEntityCacheManager.cs)：缓存管理器接口
- [EntityCacheManager.cs](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/CommService/EntityCacheManager.cs)：缓存管理器实现
- [TableSchemaManager.cs](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/CommService/TableSchemaManager.cs)：表结构信息管理器
- [TableSchemaInfo.cs](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/CommService/TableSchemaInfo.cs)：表结构信息实体类
- [EntityCacheInitializationService.cs](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/CommService/EntityCacheInitializationService.cs)：缓存初始化服务
- [EventDrivenCacheManager.cs](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/CommService/EventDrivenCacheManager.cs)：事件驱动缓存管理器
- [CacheSubscriptionManager.cs](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/CommService/CacheSubscriptionManager.cs)：缓存订阅管理器

### 1.2 设计原则
- 使用依赖注入替代单例模式
- 职责分离：TableSchemaManager管理表结构，EntityCacheManager管理缓存操作
- 统一架构：CacheSubscriptionManager可以被服务器和客户端共同使用

## 2. 组件说明

### 2.1 IEntityCacheManager
缓存管理器接口，定义了缓存操作的标准方法，包括查询和更新操作。

### 2.2 EntityCacheManager
缓存管理器的具体实现，使用CacheManager.Core作为底层缓存框架。

### 2.3 TableSchemaManager
表结构信息管理器，负责管理数据库表的结构信息，包括主键字段、显示字段等。

### 2.4 EntityCacheInitializationService
缓存初始化服务，负责在系统启动时初始化缓存数据。

### 2.5 EventDrivenCacheManager
事件驱动缓存管理器，用于处理本地缓存变更事件，可以被服务器和客户端共同使用。

### 2.6 CacheSubscriptionManager
缓存订阅管理器，可以被服务器和客户端共同使用，通过IsServerMode属性区分服务器端和客户端模式。

## 3. 使用指南

在业务层中使用缓存系统：

```csharp
// 通过依赖注入获取缓存管理器
var cacheManager = Startup.GetFromFac<IEntityCacheManager>();

// 获取实体列表
List<T> list = cacheManager.GetEntityList<T>(tableName);
```

在UI层中使用缓存系统：

```csharp
// 通过依赖注入获取缓存管理器
var cacheManager = Startup.GetFromFac<IEntityCacheManager>();

// 获取实体列表
List<T> list = cacheManager.GetEntityList<T>(tableName);
```

组件已在[BusinessDIConfig.cs](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/DI/BusinessDIConfig.cs)中注册：

```csharp
// 注册缓存管理器
builder.RegisterType<EntityCacheManager>()
    .As<IEntityCacheManager>()
    .SingleInstance()
    .PropertiesAutowired();
    
// 注册缓存初始化服务
builder.RegisterType<EntityCacheInitializationService>()
    .AsSelf()
    .SingleInstance()
    .PropertiesAutowired();
```

## 4. 初始化服务

### 4.1 EntityCacheInitializationService
使用[EntityCacheInitializationService](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/CommService/EntityCacheInitializationService.cs)初始化所有缓存：

``csharp
public class SomeController
{
    private readonly EntityCacheInitializationService _cacheInitService;
    
    public SomeController(EntityCacheInitializationService cacheInitService)
    {
        _cacheInitService = cacheInitService;
    }
    
    public async Task InitializeCache()
    {
        await _cacheInitService.InitializeAllCacheAsync();
    }
}
```

## 5. 依赖注入配置

组件已在[BusinessDIConfig.cs](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/DI/BusinessDIConfig.cs)中注册：

``csharp
// 注册缓存管理器
builder.RegisterType<EntityCacheManager>()
    .As<IEntityCacheManager>()
    .SingleInstance()
    .PropertiesAutowired();
    
// 注册缓存初始化服务
builder.RegisterType<EntityCacheInitializationService>()
    .AsSelf()
    .SingleInstance()
    .PropertiesAutowired();
```

## 6. UI集成

在UI层中使用缓存系统：

``csharp
// 通过依赖注入获取缓存管理器
var cacheManager = Startup.GetFromFac<IEntityCacheManager>();

// 获取实体列表
List<T> list = cacheManager.GetEntityList<T>(tableName);
```

## 7. 最佳实践

### 7.1 性能优化
- 使用强类型缓存操作避免类型转换开销
- 合理设置缓存过期策略
- 避免缓存穿透和雪崩

### 7.2 错误处理
- 使用try-catch处理缓存操作异常
- 记录详细的错误日志便于问题排查
- 实现缓存降级策略

### 7.3 监控和维护
- 定期清理过期缓存
- 监控缓存命中率
- 实现缓存统计功能

## 8. 注意事项

1. 缓存管理器已配置为单例模式，确保全局唯一实例
2. 表结构信息在应用程序启动时初始化
3. 缓存数据在应用程序关闭时会丢失，需要重新初始化
4. 建议在开发过程中启用详细的日志记录以便调试