# 缓存重构文档

## 概述

本文档描述了对缓存系统的重构，主要目标是：
1. 将数据库查询逻辑从缓存管理类中分离出来
2. 提供更灵活的缓存初始化方式
3. 优化性能，特别是多表查询的性能
4. 简化设计，减少文件数量和复杂度

## 重构后的架构

### 核心组件

1. **CacheInitializationService** - 缓存初始化服务
   - 负责从数据库加载数据并更新缓存
   - 使用SqlSugar直接查询数据库，提高性能
   - 支持并行初始化多个表的缓存

2. **CacheInitializationExtensions** - 扩展方法
   - 提供简洁的API来初始化缓存
   - 支持依赖注入

3. **CacheInitializationService** - 缓存初始化服务（已在BusinessDIConfig.cs中注册）
   - 提供应用程序启动时初始化缓存的便捷方法

### 使用方法

#### 在应用程序启动时初始化所有缓存

```csharp
public static async Task Main(string[] args)
{
    var host = CreateHostBuilder(args).Build();
    
    // 在应用程序启动时初始化所有缓存
    await host.Services.InitializeCacheOnStartupAsync();
    
    await host.RunAsync();
}
```

#### 初始化特定表的缓存

```csharp
// 初始化单个表
await serviceProvider.InitializeCacheForTableAsync("tb_Prod");

// 初始化多个表
await serviceProvider.InitializeCacheOnStartupAsync("tb_Prod", "tb_Customer", "tb_Supplier");
```

#### 在依赖注入配置中注册服务

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // 配置缓存相关服务
    services.ConfigureCacheServices();
    
    // 其他服务配置...
}
```

## 性能优化

1. **并行初始化**
   - 使用Task.WhenAll并行初始化多个表的缓存
   - 减少总体初始化时间

2. **直接使用SqlSugar查询**
   - 使用`_unitOfWorkManage.GetDbClient().Queryable<T>().AS(tableName).ToList()`
   - 避免反射调用，提高查询性能

3. **异步操作**
   - 所有缓存操作都是异步的，不会阻塞主线程
   - 提高应用程序响应性

## 迁移指南

### 从旧代码迁移

#### 替换旧的缓存初始化代码

**旧代码:**
```csharp
MyCacheManager.Instance.InitManager();
MyCacheManager.Instance.InitDict(tableName);
```

**新代码:**
```csharp
// 初始化所有缓存
await serviceProvider.InitializeAllCacheAsync();

// 初始化特定表
await serviceProvider.InitializeCacheForTableAsync(tableName);
```

#### 替换旧的依赖注入配置

**旧代码:**
```csharp
services.AddSingleton<MyCacheManager>(provider => MyCacheManager.Instance);
```

**新代码:**
```csharp
services.ConfigureCacheServices();
```

## 注意事项

1. **BizCacheHelper类将被删除**
   - 请使用新的CacheInitializationService替代
   - 新服务提供了相同的功能，但性能更好

2. **异步操作**
   - 所有新的API都是异步的，请使用await关键字
   - 确保调用方法也标记为async

3. **依赖注入**
   - 确保在应用程序启动时正确配置了依赖注入
   - 服务器端和客户端都需要配置

## 示例代码

### 完整的启动示例

```csharp
public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        
        try
        {
            // 初始化缓存
            await host.Services.InitializeCacheOnStartupAsync();
            
            // 启动应用程序
            await host.RunAsync();
        }
        catch (Exception ex)
        {
            // 处理异常
        }
    }
    
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // 配置缓存服务
                services.ConfigureCacheServices();
                
                // 配置其他服务...
            });
}
```

### 自定义缓存初始化

```csharp
public class CustomCacheInitializer
{
    private readonly IServiceProvider _serviceProvider;
    
    public CustomCacheInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task InitializeCustomCacheAsync()
    {
        // 初始化产品缓存
        await _serviceProvider.InitializeCacheForTableAsync("tb_Prod");
        
        // 初始化客户缓存
        await _serviceProvider.InitializeCacheForTableAsync("tb_Customer");
        
        // 可以根据业务需要添加更多逻辑
    }
}
```