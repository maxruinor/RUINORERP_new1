# 缓存同步机制使用说明

## 概述

本缓存同步机制提供了服务器端和客户端之间的双向缓存同步功能，支持内存缓存和Redis缓存的无缝切换，具有良好的性能、维护性、扩展性和实时性。

## 核心组件

### 1. UnifiedCacheManager (统一缓存管理器)
- 支持内存缓存和Redis缓存的无缝切换
- 提供缓存变更通知机制
- 支持单个和批量缓存操作

### 2. CacheSyncService (缓存同步服务)
- 处理服务器端和客户端之间的缓存同步
- 管理客户端订阅关系
- 广播缓存变更消息

### 3. ClientCacheManager (客户端缓存管理器)
- 管理客户端本地缓存
- 处理来自服务器的缓存同步消息
- 支持缓存变更事件

### 4. CacheEventPublisher (缓存事件发布器)
- 实现观察者模式
- 发布缓存变更事件

### 5. CacheSyncProtocol (缓存同步协议)
- 定义缓存同步相关的命令和数据结构

## 使用方法

### 服务器端使用

1. **配置缓存管理器**
```csharp
// 在Startup.cs中配置
services.AddUnifiedCacheManager(options =>
{
    options.CacheType = CacheType.Hybrid; // 使用混合缓存
    options.RedisConfiguration = "localhost:6379";
    options.DefaultExpirationMinutes = 30;
});
```

2. **使用缓存管理器**
```csharp
// 注入并使用UnifiedCacheManager
public class MyService
{
    private readonly UnifiedCacheManager _cacheManager;
    
    public MyService(UnifiedCacheManager cacheManager)
    {
        _cacheManager = cacheManager;
    }
    
    public void SetUserData(User user)
    {
        _cacheManager.Set($"user:{user.Id}", user);
    }
    
    public User GetUserData(int userId)
    {
        return _cacheManager.Get<User>($"user:{userId}");
    }
}
```

### 客户端使用

1. **配置客户端缓存管理器**
```csharp
// 在Startup.cs中配置
services.AddClientCacheManager(options =>
{
    options.CacheType = CacheType.Memory; // 客户端使用内存缓存
    options.DefaultExpirationMinutes = 30;
});
```

2. **使用客户端缓存管理器**
```csharp
// 注入并使用ClientCacheManager
public class MyClientService
{
    private readonly ClientCacheManager _clientCacheManager;
    
    public MyClientService(ClientCacheManager clientCacheManager)
    {
        _clientCacheManager = clientCacheManager;
    }
    
    public void SetLocalData(string key, object data)
    {
        _clientCacheManager.Set(key, data);
    }
    
    public T GetLocalData<T>(string key)
    {
        return _clientCacheManager.Get<T>(key);
    }
}
```

## 缓存同步流程

1. **客户端订阅缓存变更**
   - 客户端向服务器发送订阅请求
   - 服务器记录客户端的订阅关系

2. **服务器缓存变更**
   - 服务器端缓存发生变更时，触发变更事件
   - CacheSyncService收集变更信息

3. **广播变更消息**
   - CacheSyncService向所有订阅者广播变更消息
   - 客户端接收并处理变更消息

4. **客户端处理变更**
   - 客户端更新本地缓存
   - 客户端可选择是否将变更同步回服务器

## 配置选项

### CacheManagerOptions
- `CacheType`: 缓存类型（Memory、Redis、Hybrid）
- `RedisConfiguration`: Redis配置字符串
- `DefaultExpirationMinutes`: 默认过期时间
- `EnablePerformanceCounters`: 是否启用性能计数器
- `EnableStatistics`: 是否启用统计信息

## 最佳实践

1. **合理设置过期时间**
   - 基础数据可以设置较长的过期时间
   - 业务数据建议设置较短的过期时间

2. **使用批量操作**
   - 对于大量数据操作，使用批量方法提高性能

3. **监控缓存状态**
   - 定期检查缓存统计信息
   - 监控缓存命中率和性能指标

4. **异常处理**
   - 妥善处理缓存操作异常
   - 实现降级策略

## 扩展性

1. **自定义缓存策略**
   - 可以通过继承CacheManagerFactory实现自定义缓存策略

2. **添加新的缓存类型**
   - 支持添加其他缓存提供程序

3. **事件监听**
   - 通过ICacheEventListener接口扩展事件处理逻辑

## 性能优化

1. **使用混合缓存**
   - 本地内存缓存作为一级缓存
   - Redis作为二级缓存和同步机制

2. **合理的缓存键设计**
   - 使用有意义的缓存键前缀
   - 避免过长的缓存键

3. **批量操作**
   - 尽量使用批量方法减少网络往返

## 故障排除

1. **缓存未同步**
   - 检查订阅关系是否正确建立
   - 确认网络连接是否正常

2. **性能问题**
   - 检查Redis连接配置
   - 监控缓存统计信息

3. **数据不一致**
   - 检查缓存过期时间设置
   - 确认缓存更新逻辑是否正确