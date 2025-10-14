# 从 BizCacheHelper 迁移到 CacheInitializationService 指南

## 概述

本指南将帮助您从旧的 `BizCacheHelper` 类迁移到新的 `CacheInitializationService`。新的缓存初始化服务提供了更好的性能、更清晰的职责分离和更灵活的使用方式。

## 主要变化

1. **职责分离**：数据库查询逻辑从缓存管理类中分离出来
2. **异步操作**：所有操作都是异步的，提高应用程序响应性
3. **性能优化**：使用SqlSugar直接查询数据库，支持并行初始化
4. **简化设计**：减少了文件数量和复杂度

## 迁移步骤

### 1. 替换缓存初始化代码

#### 旧代码 (BizCacheHelper)
```csharp
// 初始化所有缓存
var helper = new BizCacheHelper(context, logger);
helper.InitCacheDict();

// 初始化特定表
helper.InitDict("tb_Prod");
```

#### 新代码 (CacheInitializationService)
```csharp
// 初始化所有缓存
await serviceProvider.InitializeAllCacheAsync();

// 初始化特定表
await serviceProvider.InitializeCacheForTableAsync("tb_Prod");
```

### 2. 更新依赖注入配置

#### 旧代码
```csharp
// 不需要显式注册，BizCacheHelper 通常直接实例化
```

#### 新代码
```csharp
// 在 BusinessDIConfig.cs 中已自动注册
// 如果使用 ServiceCollection，可以这样注册：
services.ConfigureCacheServices();
```

### 3. 更新应用程序启动代码

#### 旧代码
```csharp
public void ConfigureServices(IServiceCollection services)
{
    // 其他服务配置...
    
    // 初始化缓存
    var context = new ApplicationContext();
    var logger = loggerFactory.CreateLogger<BizCacheHelper>();
    var helper = new BizCacheHelper(context, logger);
    helper.InitCacheDict();
}
```

#### 新代码
```csharp
public async Task ConfigureServices(IServiceCollection services)
{
    // 配置缓存服务
    services.ConfigureCacheServices();
    
    // 其他服务配置...
}

public async Task Configure(IApplicationBuilder app, IServiceProvider serviceProvider)
{
    // 初始化缓存
    await serviceProvider.InitializeCacheOnStartupAsync();
    
    // 其他配置...
}
```

## 常见使用场景

### 场景1：应用程序启动时初始化所有缓存

```csharp
public static async Task Main(string[] args)
{
    var host = CreateHostBuilder(args).Build();
    
    // 初始化所有缓存
    await host.Services.InitializeCacheOnStartupAsync();
    
    await host.RunAsync();
}
```

### 场景2：初始化特定表的缓存

```csharp
public async Task InitializeProductCache(IServiceProvider serviceProvider)
{
    // 只初始化产品相关的表
    await serviceProvider.InitializeCacheForTableAsync("tb_Prod");
    await serviceProvider.InitializeCacheForTableAsync("tb_ProdCategory");
    await serviceProvider.InitializeCacheForTableAsync("tb_ProdUnit");
}
```

### 场景3：并行初始化多个表

```csharp
public async Task InitializeMultipleTables(IServiceProvider serviceProvider)
{
    // 并行初始化多个表
    await serviceProvider.InitializeCacheOnStartupAsync(
        "tb_Prod", 
        "tb_Customer", 
        "tb_Supplier", 
        "tb_Warehouse"
    );
}
```

## 性能对比

| 操作 | BizCacheHelper | CacheInitializationService | 改进 |
|------|---------------|---------------------------|------|
| 单表初始化 | 同步，阻塞 | 异步，非阻塞 | 响应性提高 |
| 多表初始化 | 顺序执行 | 并行执行 | 速度提升 |
| 数据库查询 | 反射调用 | 直接SqlSugar查询 | 性能提升 |
| 错误处理 | 基础 | 完善的日志记录 | 可维护性提高 |

## 注意事项

1. **异步操作**：所有新的API都是异步的，请确保调用方法也标记为 `async`
2. **依赖注入**：确保在应用程序启动时正确配置了依赖注入
3. **错误处理**：新服务提供了更完善的错误处理和日志记录
4. **BizCacheHelper将被删除**：请尽快迁移到新服务

## 故障排除

### 问题1：服务未注册
**错误**：`无法获取缓存初始化服务`

**解决方案**：确保在依赖注入配置中调用了 `services.ConfigureCacheServices()`

### 问题2：异步上下文问题
**错误**：`await 操作符只能在异步方法中使用`

**解决方案**：确保调用方法标记为 `async` 并返回 `Task`

### 问题3：表名不存在
**错误**：`表名 'xxx' 不在预定义的缓存表列表中`

**解决方案**：检查表名是否正确，或在 `CacheInitializationService` 中添加新的表名

## 示例项目

完整的示例代码请直接在项目中注入并使用 `CacheInitializationService` 服务。

## 总结

新的 `CacheInitializationService` 提供了更好的性能、更清晰的职责分离和更灵活的使用方式。通过遵循本指南，您可以轻松地从旧的 `BizCacheHelper` 迁移到新服务，并享受更好的性能和开发体验。