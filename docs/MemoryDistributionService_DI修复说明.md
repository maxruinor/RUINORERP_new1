# MemoryDistributionService 依赖注入问题修复总结

## 问题现象

在 `ServerMonitorControl` 中通过 `Startup.GetFromFac<MemoryDistributionService>()` 获取服务实例时返回 **null**，导致内存分布统计功能无法使用。

## 根本原因分析

### 1. 注册方式不一致

原代码在 `Startup.cs` 中直接注册：
```csharp
builder.RegisterType<MemoryDistributionService>()
    .AsSelf()
    .SingleInstance()
    .PropertiesAutowired();
```

虽然语法正确，但与项目中其他模块化服务（如 SmartReminder）的注册模式不一致。

### 2. 属性注入依赖可能失败

`MemoryDistributionService` 依赖多个通过属性注入的服务：
- `ILogger<MemoryDistributionService>` ✅ (自动提供)
- `ISessionService` ✅ (NetworkServicesDependencyInjection)
- `ServerLockManager` ✅ (Startup.cs)
- `SmartReminderMonitor` ✅ (SmartReminderModule)
- `IEntityCacheManager` ✅ (BusinessDIConfig)
- `IStockCacheService` ✅ (Startup.cs)
- `IMemoryCache` ✅ (自动提供)
- `IRedisCacheService` ✅ (Startup.cs)
- `ImageCacheServiceBase` ✅ (Startup.cs)
- `FileStorageMonitorService` ✅ (NetworkServicesDependencyInjection)
- `PerformanceDataStorageService` ✅ (Startup.cs)
- **`CachedRuleEngineCenter` ❌ (未注册，只有 RuleEngineCenter)**

### 3. 类型不匹配问题

**关键发现**：SmartReminderModule 注册的是 `RuleEngineCenter`，但 MemoryDistributionService 需要的是 `CachedRuleEngineCenter`。

```csharp
// SmartReminderModule.cs 第89行
builder.RegisterType<RuleEngineCenter>().As<IRuleEngineCenter>().SingleInstance();

// MemoryDistributionService.cs 第45行（原代码）
public CachedRuleEngineCenter RuleEngineCenter { private get; set; }
```

由于容器中只有 `RuleEngineCenter` 而没有 `CachedRuleEngineCenter`，属性注入失败。

## 解决方案

### 方案1：创建 MemoryDistributionModule（已实施）✅

#### 步骤1：创建模块类

文件：`RUINORERP.Server/Services/MemoryDistributionModule.cs`

```csharp
public class MemoryDistributionModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MemoryDistributionService>()
            .AsSelf()
            .SingleInstance()
            .PropertiesAutowired();
    }
}
```

#### 步骤2：修改 Startup.cs 注册方式

```csharp
// 原来
builder.RegisterType<MemoryDistributionService>()
    .AsSelf()
    .SingleInstance()
    .PropertiesAutowired();

// 改为
builder.RegisterModule<MemoryDistributionModule>();
```

#### 步骤3：修正依赖类型

将 `CachedRuleEngineCenter` 改为基类 `RuleEngineCenter`：

```csharp
// 原来
public CachedRuleEngineCenter RuleEngineCenter { private get; set; }

// 改为
public RUINORERP.Server.SmartReminder.RuleEngineCenter RuleEngineCenter { private get; set; }
```

### 方案2：添加诊断日志（已实施）✅

在 `ServerMonitorControl` 构造函数中添加详细的诊断日志：

```csharp
_memoryDistributionService = Startup.GetFromFac<MemoryDistributionService>();

if (_memoryDistributionService == null)
{
    _logger.LogError("❌ MemoryDistributionService 获取失败，返回 null");
}
else
{
    _logger.LogInformation("✅ MemoryDistributionService 获取成功");
    var depStatus = _memoryDistributionService.GetDependencyStatus();
    _logger.LogDebug("MemoryDistributionService 依赖状态:\n{Status}", depStatus);
}
```

## 验证步骤

1. **重新编译项目**
   ```bash
   dotnet build RUINORERP.Server
   ```

2. **启动服务器并检查日志**

   查找以下日志输出：
   - ✅ `MemoryDistributionService 获取成功`
   - 📋 `MemoryDistributionService 依赖状态:` 后面列出所有依赖的注入状态

3. **如果仍然为 null，检查：**
   - Autofac 容器是否已初始化（`Startup.AutofacContainerScope != null`）
   - `MemoryDistributionModule` 是否在正确的时机被注册
   - 是否有异常阻止了服务注册

4. **查看依赖注入状态**

   调用 `_memoryDistributionService.GetDependencyStatus()` 会输出类似：
   ```
   Logger: 已注入
   SessionService: 已注入
   LockManager: 已注入
   SmartReminderMonitor: 已注入
   EntityCacheManager: 已注入
   StockCacheService: 已注入
   MemoryCache: 已注入
   RedisCacheService: 已注入
   ImageCacheService: 已注入
   FileStorageMonitorService: 已注入
   PerformanceDataStorageService: 已注入
   RuleEngineCenter: 已注入
   ```

## 相关文件

- ✅ `RUINORERP.Server/Services/MemoryDistributionModule.cs` - 新建模块类
- ✏️ `RUINORERP.Server/Startup.cs` - 修改注册方式（第405行）
- ✏️ `RUINORERP.Server/Services/MemoryDistributionService.cs` - 修正依赖类型（第45行）
- ✏️ `RUINORERP.Server/Controls/ServerMonitorControl.cs` - 添加诊断日志（第80行、第144行）

## 设计原则

本次修复遵循了项目的 **DI配置分层自治规范**：

1. **模块化组织**：使用 Autofac Module 统一管理相关服务的注册
2. **职责清晰**：每个 Module 负责特定功能域的服务注册
3. **一致性**：与 SmartReminderModule 等其他模块保持一致的注册模式
4. **可维护性**：便于后续扩展和维护

## 注意事项

⚠️ **属性注入的限制**：
- 属性注入依赖于 `.PropertiesAutowired()` 配置
- 如果某个依赖服务未注册，属性将为 null（不会抛出异常）
- 建议在服务中使用前检查 null

💡 **最佳实践**：
- 优先使用构造函数注入（更明确、更安全）
- 对于可选依赖，可以使用属性注入
- 始终添加 null 检查和适当的错误处理
