# FileStorageMonitorService日志问题修复说明

## 问题描述

`FileStorageMonitorService.cs`中有多处ERROR级别的日志记录，但在实际运行中没有记录到任何内容。

### 涉及的ERROR日志位置

1. **第77-78行**：启动时立即检查存储空间失败
   ```csharp
   _logger.LogError(ex, "启动时立即检查存储空间失败: {ErrorType} - {ErrorMessage}\n{StackTrace}", 
       ex.GetType().Name, ex.Message, ex.StackTrace);
   ```

2. **第105行**：定时检查存储空间失败
   ```csharp
   _logger.LogError(ex, "定时检查存储空间失败: {ErrorType} - {ErrorMessage}", ex.GetType().Name, ex.Message);
   ```

3. **第237-238行**：检查文件存储空间失败
   ```csharp
   _logger.LogError(ex, "检查文件存储空间失败: {ErrorType} - {ErrorMessage}\n{StackTrace}", 
       ex.GetType().Name, ex.Message, ex.StackTrace);
   ```

4. **第271-272行**：自动清理失败
   ```csharp
   _logger.LogError(ex, "自动清理失败: {ErrorType} - {ErrorMessage}\n{StackTrace}", 
       ex.GetType().Name, ex.Message, ex.StackTrace);
   ```

5. **第338-339行**：获取监控信息失败
   ```csharp
   _logger.LogError(ex, "获取监控信息失败: {ErrorType} - {ErrorMessage}\n{StackTrace}", 
       ex.GetType().Name, ex.Message, ex.StackTrace);
   ```

## 根本原因分析

### 1. 日志仓库不一致问题

**问题核心**：服务器端使用了两个不同的log4net日志仓库，导致日志配置不统一。

#### 仓库1：SuperSocket（旧配置）
- **位置**：[Log4NetExtensions.cs:27](file:///e:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Server/Log4NetExtensions.cs#L27)
- **创建方式**：`LogManager.CreateRepository("SuperSocket")`
- **配置文件**：`ConfigBySS/log4net.config`
- **用途**：SuperSocket网络框架的日志

#### 仓库2：RUINORERP_Shared_LoggerRepository（新配置）
- **位置**：[Log4NetConfiguration.cs:21](file:///e:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Common/Log4Net/Log4NetConfiguration.cs#L21)
- **常量定义**：`SHARED_REPOSITORY_NAME = "RUINORERP_Shared_LoggerRepository"`
- **用途**：通过Microsoft.Extensions.Logging注入的ILogger使用的仓库

### 2. 日志流转路径

```
FileStorageMonitorService
    ↓ 依赖注入
ILogger<FileStorageMonitorService>
    ↓ Microsoft.Extensions.Logging
Log4NetProvider.CreateLogger("RUINORERP.Server.Network.Services.FileStorageMonitorService")
    ↓ Log4NetLogger构造函数
LogManager.GetLogger(Log4NetConfiguration.SHARED_REPOSITORY_NAME, categoryName)
    ↓ 使用仓库
"RUINORERP_Shared_LoggerRepository"
    ↓ 问题
该仓库可能未正确配置或未加载log4net.config
```

### 3. 配置冲突

在[Startup.cs:196](file:///e:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Server/Startup.cs#L196)中：
```csharp
logBuilder.AddProvider(new RUINORERP.Common.Log4Net.Log4NetProvider());
```

这个Provider会使用`SHARED_REPOSITORY_NAME`仓库，但服务器启动时在`UseLog4Net()`中创建的是`"SuperSocket"`仓库并配置了它。

**结果**：
- `SuperSocket`仓库有正确的配置（从`ConfigBySS/log4net.config`加载）
- `RUINORERP_Shared_LoggerRepository`仓库没有配置或配置不完整
- FileStorageMonitorService的日志写入未配置的仓库，导致日志丢失

## 修复方案

### 方案：统一日志仓库（已实施）

修改[Log4NetExtensions.cs](file:///e:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Server/Log4NetExtensions.cs)，使用统一的共享日志仓库名称：

```csharp
public static IHostBuilder UseLog4Net(this IHostBuilder hostBuilder)
{
    // ✅ 使用统一的共享日志仓库名称，与客户端保持一致
    string repositoryName = RUINORERP.Common.Log4Net.Log4NetConfiguration.SHARED_REPOSITORY_NAME;
    
    // 检查是否已存在该仓库
    var existingRepository = LogManager.GetAllRepositories().FirstOrDefault(r => r.Name == repositoryName);
    ILoggerRepository log4netRepository;
    
    if (existingRepository != null)
    {
        // 使用现有仓库
        log4netRepository = existingRepository;
        System.Diagnostics.Debug.WriteLine($"使用现有日志仓库: {repositoryName}");
    }
    else
    {
        // 创建新仓库
        log4netRepository = LogManager.CreateRepository(repositoryName);
        System.Diagnostics.Debug.WriteLine($"创建新日志仓库: {repositoryName}");
    }
    
    // 配置log4net
    XmlConfigurator.Configure(log4netRepository, new FileInfo("ConfigBySS/log4net.config"));
    
    return hostBuilder;
}
```

### 修复效果

1. **统一仓库名称**：所有日志都使用`RUINORERP_Shared_LoggerRepository`仓库
2. **避免重复创建**：检查仓库是否已存在，避免重复配置
3. **正确加载配置**：确保仓库使用`ConfigBySS/log4net.config`配置
4. **日志正常输出**：ERROR级别的日志会正确写入`Logs/SocketServer/yyyy-MM-dd/Error.log`

## 验证方法

### 1. 检查日志文件

修复后，当FileStorageMonitorService发生错误时，应该在以下位置找到日志：

```
Logs/SocketServer/2026-04-11/Error.log
```

日志格式示例：
```
2026-04-11 13:05:55,123 [Thread-XX] ERROR RUINORERP.Server.Network.Services.FileStorageMonitorService - 启动时立即检查存储空间失败: IOException - 磁盘不可访问
类型: System.IO.IOException
消息: 磁盘不可访问
堆栈:    在 RUINORERP.Server.Network.Services.FileStorageMonitorService.<CheckStorageSpaceAsync>d__...
```

### 2. 测试场景

触发FileStorageMonitorService的错误场景：

1. **停止文件存储服务**：模拟存储路径不可用
2. **设置无效的存储路径**：配置一个不存在的路径
3. **权限不足**：设置存储目录为只读

观察是否在Error.log中记录了相应的错误信息。

### 3. 调试输出

在Visual Studio的输出窗口中应该看到：
```
使用现有日志仓库: RUINORERP_Shared_LoggerRepository
```
或
```
创建新日志仓库: RUINORERP_Shared_LoggerRepository
```

## 相关文件和配置

### 关键文件

1. **Log4NetExtensions.cs**：服务器日志初始化
   - 修复前：创建`"SuperSocket"`仓库
   - 修复后：使用`SHARED_REPOSITORY_NAME`常量

2. **Log4NetConfiguration.cs**：共享仓库名称定义
   - 常量：`SHARED_REPOSITORY_NAME = "RUINORERP_Shared_LoggerRepository"`

3. **Log4NetProvider.cs**：日志提供者实现
   - 创建Log4NetLogger实例

4. **Log4NetLogger.cs**：日志记录器实现
   - 使用`SHARED_REPOSITORY_NAME`获取logger

5. **log4net.config**：日志配置文件
   - 位置：`ConfigBySS/log4net.config`
   - 配置了ErrorLog、WarnLog、InfoLog、DebugLog四个appender

### 日志配置要点

在`log4net.config`中：
```xml
<root>
    <level value="DEBUG"/>  <!-- 最终生效的级别 -->
    <appender-ref ref="ErrorLog" />
    <appender-ref ref="WarnLog" />
    <appender-ref ref="InfoLog" />
    <appender-ref ref="DebugLog" />
</root>

<!-- ErrorLog appender 只接收ERROR级别 -->
<appender name="ErrorLog" type="log4net.Appender.RollingFileAppender">
    <param name="File" value="Logs/SocketServer"/>
    <param name="DatePattern" value="/yyyy-MM-dd/&quot;Error.log&quot;"/>
    <filter type="log4net.Filter.LevelRangeFilter">
        <param name="LevelMin" value="ERROR" />
        <param name="LevelMax" value="ERROR" />
    </filter>
</appender>
```

## 其他注意事项

### 1. Startup.cs中的过滤器

在[Startup.cs:186-193](file:///e:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Server/Startup.cs#L186-L193)中有一个临时过滤器：

```csharp
logBuilder.AddFilter((provider, category, logLevel) =>
{
    #if DEBUG
    System.Diagnostics.Debug.WriteLine($"日志过滤器检查: category={category}, level={logLevel}");
    #endif
    // 临时禁用所有过滤，让所有日志通过
    return true;
});
```

**注意**：这里的注释说"临时禁用所有过滤"，但实际上`return true`表示**启用**该级别的日志。如果需要真正禁用某些日志，应该返回`false`。

当前配置下，所有级别的日志都会被传递到Log4NetProvider，然后由log4net的配置文件决定最终是否输出。

### 2. 日志级别映射

Microsoft.Extensions.Logging → log4net：
- `LogLevel.Trace` → `_log.Debug()`
- `LogLevel.Debug` → `_log.Debug()`
- `LogLevel.Information` → `_log.Info()`
- `LogLevel.Warning` → `_log.Warn()`
- `LogLevel.Error` → `_log.Error()`
- `LogLevel.Critical` → `_log.Fatal()`

### 3. 性能优化

Log4NetLogger实现了分层缓存策略：
- **固定属性缓存**：UserName, MachineName, IP, MAC, Operator（5秒有效期）
- **半固定属性缓存**：ModName, ActionName, Path（1秒有效期）
- **变化属性**：Message, Exception（每次设置）

这减少了频繁读取ApplicationContext的性能开销。

## 总结

**问题根源**：服务器端使用了两个不同的log4net仓库，FileStorageMonitorService的日志写入了未正确配置的仓库。

**解决方案**：统一使用`RUINORERP_Shared_LoggerRepository`仓库，确保所有日志都使用相同的配置。

**预期效果**：ERROR级别的日志会正确记录到`Logs/SocketServer/yyyy-MM-dd/Error.log`文件中。
