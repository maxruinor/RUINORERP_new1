# 缓存初始化服务使用说明

## 直接使用 CacheInitializationService

在项目的任何地方，只需通过依赖注入获取 `CacheInitializationService` 实例，然后调用相应方法：

```csharp
// 在控制器、服务或其他类中
public class YourService
{
    private readonly CacheInitializationService _cacheInitService;
    
    public YourService(CacheInitializationService cacheInitService)
    {
        _cacheInitService = cacheInitService;
    }
    
    // 初始化所有缓存
    public async Task InitializeAllCaches()
    {
        await _cacheInitService.InitializeAllCacheAsync();
    }
    
    // 初始化特定表的缓存
    public async Task InitializeSpecificCaches()
    {
        await _cacheInitService.InitializeCacheForTableAsync("tb_Prod");
        await _cacheInitService.InitializeCacheForTableAsync("tb_Customer");
    }
}
```

## 在应用程序启动时初始化缓存

在应用程序启动代码中（如 Program.cs 或 Startup.cs）：

```csharp
// 获取服务提供程序
var serviceProvider = services.BuildServiceProvider();

// 获取缓存初始化服务
var cacheInitService = serviceProvider.GetService<CacheInitializationService>();

if (cacheInitService != null)
{
    // 初始化所有缓存
    await cacheInitService.InitializeAllCacheAsync();
}
```

## 注意事项

1. `CacheInitializationService` 已在 `BusinessDIConfig.cs` 中注册为单例
2. 无需使用任何扩展方法或包装类
3. 直接注入服务并使用即可