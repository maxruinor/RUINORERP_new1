# 缓存系统概述

## 概述

本文档提供了RUINORERP系统中缓存系统的完整概述，包括架构设计、组件说明和使用指南。

## 1. 架构设计

### 1.1 核心组件
- [IEntityCacheManager.cs](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/CommService/IEntityCacheManager.cs)：缓存管理器接口
- [EntityCacheManager.cs](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/CommService/EntityCacheManager.cs)：缓存管理器实现
- [TableSchemaManager.cs](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/CommService/TableSchemaManager.cs)：表结构信息管理器
- [TableSchemaInfo.cs](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/CommService/TableSchemaInfo.cs)：表结构信息实体类
- [EntityCacheInitializationService.cs](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/CommService/EntityCacheInitializationService.cs)：缓存初始化服务

### 1.2 设计原则
- 使用依赖注入替代单例模式
- 职责分离：TableSchemaManager管理表结构，EntityCacheManager管理缓存操作
- 性能优化：使用字典实现O(1)查找和删除
- 类型安全：强类型缓存操作
- 可扩展性：支持多种序列化方式

## 2. 核心功能

### 2.1 缓存查询
```csharp
// 获取指定类型的实体列表
List<T> list = cacheManager.GetEntityList<T>();

// 根据表名获取指定类型的实体列表
List<T> list = cacheManager.GetEntityList<T>(tableName);

// 根据ID获取实体
T entity = cacheManager.GetEntity<T>(idValue);

// 根据表名和主键值获取实体
object entity = cacheManager.GetEntity(tableName, primaryKeyValue);

// 获取指定表名的显示值
object displayValue = cacheManager.GetDisplayValue(tableName, idValue);
```

### 2.2 缓存更新
```csharp
// 更新实体列表缓存
cacheManager.UpdateEntityList(list);

// 更新单个实体缓存
cacheManager.UpdateEntity(entity);

// 根据表名更新缓存
cacheManager.UpdateEntityList(tableName, list);

// 根据表名更新单个实体缓存
cacheManager.UpdateEntity(tableName, entity);
```

### 2.3 缓存删除
```csharp
// 删除指定ID的实体缓存
cacheManager.DeleteEntity<T>(idValue);

// 删除实体列表缓存
cacheManager.DeleteEntityList(entities);

// 根据表名和主键删除实体缓存
cacheManager.DeleteEntity(tableName, primaryKeyValue);
```

### 2.4 表结构管理
```csharp
// 初始化表结构信息
cacheManager.InitializeTableSchema<tb_Company>(k => k.ID, v => v.CNName);

// 获取实体类型
Type entityType = cacheManager.GetEntityType(tableName);
```

### 2.5 序列化支持
```csharp
// 序列化缓存数据
byte[] serializedData = cacheManager.SerializeCacheData(data, CacheSerializationHelper.SerializationType.Json);

// 反序列化缓存数据
T deserializedData = cacheManager.DeserializeCacheData<T>(data, CacheSerializationHelper.SerializationType.Json);
```

## 3. 初始化服务

### 3.1 EntityCacheInitializationService
使用[EntityCacheInitializationService](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/CommService/EntityCacheInitializationService.cs)初始化所有缓存：

```csharp
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

## 4. 依赖注入配置

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

## 5. UI集成

在UI层中使用缓存系统：

```csharp
// 通过依赖注入获取缓存管理器
var cacheManager = Startup.GetFromFac<IEntityCacheManager>();

// 获取实体列表
List<T> list = cacheManager.GetEntityList<T>(tableName);
```

## 6. 最佳实践

### 6.1 性能优化
- 使用强类型缓存操作避免类型转换开销
- 合理设置缓存过期策略
- 避免缓存穿透和雪崩

### 6.2 错误处理
- 使用try-catch处理缓存操作异常
- 记录详细的错误日志便于问题排查
- 实现缓存降级策略

### 6.3 监控和维护
- 定期清理过期缓存
- 监控缓存命中率
- 实现缓存统计功能

## 7. 注意事项

1. 缓存管理器已配置为单例模式，确保全局唯一实例
2. 表结构信息在应用程序启动时初始化
3. 缓存数据在应用程序关闭时会丢失，需要重新初始化
4. 建议在开发过程中启用详细的日志记录以便调试