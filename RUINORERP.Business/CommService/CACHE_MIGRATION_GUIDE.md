# 缓存系统迁移指南

## 概述

本文档提供了从旧缓存架构迁移到新缓存架构的详细指南。

## 1. 新旧架构对比

### 1.1 旧架构组件（已过时）
- [MyCacheManager.cs](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Extensions/Middlewares/MyCacheManager.cs)：旧的缓存管理器实现
- [BaseCacheDataList.cs](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Extensions/Middlewares/BaseCacheDataList.cs)：旧的表结构信息管理
- [CacheInitializationService.cs](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/CommService/CacheInitializationService.cs)：旧的缓存初始化服务

**注意：以上组件已被标记为过时，请使用新的缓存架构。**

### 1.2 新架构组件
- [ICacheManager.cs](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/CommService/ICacheManager.cs)：缓存管理器接口
- [CacheManager.cs](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/CommService/CacheManager.cs)：优化的缓存管理器实现
- [TableSchemaManager.cs](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/CommService/TableSchemaManager.cs)：表结构信息管理器
- [TableSchemaInfo.cs](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/CommService/TableSchemaInfo.cs)：表结构信息和外键关系实体类
- [OptimizedCacheInitializationService.cs](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/CommService/OptimizedCacheInitializationService.cs)：优化的缓存初始化服务

## 2. 迁移步骤

### 2.1 依赖注入配置
新架构组件已在[BusinessDIConfig.cs](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/DI/BusinessDIConfig.cs)中注册：

```csharp
// 注册新的优化缓存管理器
builder.RegisterType<OptimizedCacheManager>()
    .As<ICacheManager>()
    .SingleInstance()
    .PropertiesAutowired();
    
// 注册新的优化缓存初始化服务
builder.RegisterType<OptimizedCacheInitializationService>()
    .AsSelf()
    .SingleInstance()
    .PropertiesAutowired();
```

### 2.2 控制器/服务修改
将旧的缓存管理器依赖替换为新的接口：

**旧代码：**
```csharp
public class SomeController
{
    private readonly MyCacheManager _cacheManager;
    
    public SomeController()
    {
        _cacheManager = MyCacheManager.Instance;
    }
}
```

**新代码：**
```csharp
public class SomeController
{
    private readonly ICacheManager _cacheManager;
    
    public SomeController(ICacheManager cacheManager)
    {
        _cacheManager = cacheManager;
    }
}
```

### 2.3 缓存操作修改
将旧的缓存操作方法替换为新的方法：

**旧代码：**
```csharp
// 获取实体列表
var list = _cacheManager.GetEntityList<tb_Company>();

// 更新实体列表
_cacheManager.UpdateEntityList(list);
```

**新代码：**
```csharp
// 获取实体列表
var list = _cacheManager.GetEntityList<tb_Company>();

// 更新实体列表
_cacheManager.UpdateEntityList(list);
```

### 2.4 表结构信息管理修改
将旧的表结构信息管理替换为新的管理器：

**旧代码：**
```csharp
// 在BaseCacheDataList中注册表结构信息
baseCacheDataList.RegistInformation<tb_Company>(k => k.ID, v => v.CNName);
```

**新代码：**
```csharp
// 在TableSchemaManager中注册表结构信息
_tableSchemaManager.RegisterTableSchema<tb_Company>(k => k.ID, v => v.CNName);
```

## 3. 缓存初始化服务迁移

### 3.1 旧服务
[CacheInitializationService](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/CommService/CacheInitializationService.cs)已标记为过时。

### 3.2 新服务
使用[OptimizedCacheInitializationService](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/CommService/OptimizedCacheInitializationService.cs)：

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

## 4. 序列化支持

新架构提供了更好的序列化支持：

```csharp
// 序列化数据以便在网络中传输
var serializedData = _cacheManager.SerializeCacheData(someData, CacheSerializationHelper.SerializationType.MessagePack);

// 反序列化从网络接收到的数据
var deserializedData = _cacheManager.DeserializeCacheData<SomeType>(receivedData, CacheSerializationHelper.SerializationType.Json);
```

## 5. 向后兼容性

旧架构仍然可用以确保向后兼容性：
- [MyCacheManager](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Extensions/Middlewares/MyCacheManager.cs#L17-L982)类仍然可以使用
- [BaseCacheDataList](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Extensions/Middlewares/BaseCacheDataList.cs#L19-L439)类仍然可以使用
- [CacheInitializationService](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/CommService/CacheInitializationService.cs#L13-L568)类仍然可以使用

但建议新开发的功能使用新架构。

## 6. 测试和验证

在迁移完成后，进行以下测试：
1. 功能测试：确保所有缓存操作正常工作
2. 性能测试：验证新架构的性能提升
3. 兼容性测试：确保旧代码仍然可以正常运行
4. 集成测试：验证与其他系统的集成是否正常

## 7. 注意事项

1. 旧架构组件已被标记为过时，编译时会显示警告
2. 在迁移过程中，可以同时使用新旧两种架构
3. 新架构提供了更好的性能和可维护性
4. 请参考相关文档进行迁移和开发
5. 建议逐步迁移，避免一次性大规模修改