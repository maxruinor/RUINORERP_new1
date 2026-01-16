# Log4Net 日志系统重构技术文档

## 1. 文档概述

### 1.1 重构目标
- 简化日志系统架构,移除冗余代码
- 统一客户端和服务器端的日志配置方式
- 优化配置文件管理,避免配置混乱
- 保持现有数据库表结构和加密连接方式不变
- 提供清晰、易维护的日志系统实现

### 1.2 重构原则
- **简洁性**：移除不必要的包装类和示例文件
- **一致性**：客户端和服务器使用相同的日志机制
- **可维护性**：清晰的代码结构和配置方式
- **向后兼容**：保持现有数据库表结构和API接口不变
- **性能优化**：合理的缓冲区配置,平衡实时性和性能

---

## 2. 现状分析

### 2.1 现有架构问题

#### 2.1.1 代码层面
1. **Log4NetLoggerByDb.cs 问题**
   - `CreateLoggerRepository` 方法硬编码创建 `AdoNetAppender`,不使用 XML 配置
   - XML 配置文件仅用于读取 `BufferSize`,其他配置被忽略
   - 静态仓库缓存导致配置更新不生效
   - 调试代码过多,影响可读性

2. **Log4NetProviderByCustomeDb.cs 问题**
   - 配置文件查找逻辑复杂,存在多个备选路径
   - 连接字符串替换逻辑冗余

3. **EnhancedCustomLayout.cs 问题**
   - 支持 MDC 和 Properties 双重查找,增加复杂性
   - 反射获取 MDC 值,性能开销

#### 2.1.2 配置文件问题
1. **服务器端配置混乱**
   - `Log4net.config` (新创建)
   - `Log4net_db.config` (旧配置)
   - `Log4net_file.config` (文件日志)
   - `ConfigBySS/log4net.config` (SuperSocket)

2. **客户端配置**
   - `Log4net.config` (主配置)
   - `AssemblyInfo.cs` 中使用 `XmlConfigurator(Watch = true)`

3. **配置不一致**
   - 客户端依赖 `AssemblyInfo.cs` 特性自动配置
   - 服务器端手动配置,且 `BufferSize` 在 `Startup.cs` 中硬编码为 10

### 2.2 数据库表结构

```sql
CREATE TABLE [dbo].[Logs] (
    [ID] bigint IDENTITY(1,1) PRIMARY KEY,
    [Date] datetime NULL,
    [Level] varchar(10) NULL,
    [Logger] varchar(500) NULL,
    [Message] text NULL,
    [Exception] text NULL,
    [Operator] varchar(200) NULL,
    [ModName] varchar(50) NULL,
    [Path] varchar(100) NULL,
    [ActionName] varchar(50) NULL,
    [IP] varchar(20) NULL,
    [MAC] varchar(30) NULL,
    [MachineName] varchar(50) NULL,
    [User_ID] bigint NULL
)
```

### 2.3 加密连接方式

```csharp
// CryptoHelper.cs
public static string GetDecryptedConnectionString()
{
    // 从配置文件读取加密的连接字符串
    // 使用 AES 解密
    return decryptedConnectionString;
}
```

### 2.4 当前日志字段映射

| 字段 | 类型 | 大小 | 来源 |
|------|------|------|------|
| Date | DateTime | - | log4net.RawTimeStampLayout |
| Level | String | 10 | log4net.PatternLayout(%level) |
| Logger | String | 500 | log4net.PatternLayout(%logger) |
| Message | String | Max | ThreadContext.Properties["Message"] |
| Exception | String | Max | ThreadContext.Properties["Exception"] |
| Operator | String | 200 | ThreadContext.Properties["Operator"] |
| ModName | String | 50 | ThreadContext.Properties["ModName"] |
| Path | String | 100 | ThreadContext.Properties["Path"] |
| ActionName | String | 50 | ThreadContext.Properties["ActionName"] |
| IP | String | 20 | ThreadContext.Properties["IP"] |
| MAC | String | 30 | ThreadContext.Properties["MAC"] |
| MachineName | String | 50 | ThreadContext.Properties["MachineName"] |
| User_ID | Int64 | 8 | ThreadContext.Properties["User_ID"] |

---

## 3. 重构设计方案

### 3.1 架构设计

#### 3.1.1 核心组件

```
RUINORERP.Common.Log4Net/
├── EnhancedAdoNetAppender.cs      # 自定义 AdoNetAppender
├── Log4NetLogger.cs                # 日志记录器 (简化版)
├── Log4NetProvider.cs              # 日志提供者 (简化版)
├── CustomLayout.cs                 # 自定义布局器
└── Log4NetConfiguration.cs         # 配置管理器 (新增)
```

#### 3.1.2 架构原则

1. **移除硬编码逻辑**
   - 使用 XML 配置文件完全控制 Appender 配置
   - 移除 `CreateLoggerRepository` 中的硬编码创建逻辑

2. **简化布局器**
   - 只使用 `ThreadContext.Properties`,移除 MDC 支持
   - 移除反射获取 MDC 的复杂逻辑

3. **统一配置方式**
   - 客户端和服务器都使用相同的配置文件
   - 使用 `XmlConfigurator` 统一配置,不依赖 AssemblyInfo 特性

4. **简化日志仓库管理**
   - 移除静态仓库缓存
   - 使用 log4net 默认仓库机制

### 3.2 EnhancedAdoNetAppender 设计

#### 3.2.1 功能说明

继承 `AdoNetAppender`,扩展以下功能:
- 自动从 `CryptoHelper.GetDecryptedConnectionString()` 获取连接字符串
- 支持配置化的 `BufferSize`
- 自动处理加密连接字符串的占位符替换

#### 3.2.2 类结构

```csharp
public class EnhancedAdoNetAppender : AdoNetAppender
{
    /// <summary>
    /// 自动解析加密的连接字符串
    /// </summary>
    protected override void ActivateOptions()
    {
        // 如果连接字符串包含加密标识,自动解密
        if (ConnectionString.Contains("Encrypted:"))
        {
            ConnectionString = CryptoHelper.GetDecryptedConnectionString();
        }
        
        base.ActivateOptions();
    }
}
```

### 3.3 Log4NetConfiguration 设计

#### 3.3.1 功能说明

负责日志配置的初始化和管理:
- 加载配置文件
- 替换连接字符串占位符
- 使用 `XmlConfigurator` 配置日志仓库

#### 3.3.2 类结构

```csharp
public class Log4NetConfiguration
{
    /// <summary>
    /// 初始化 log4net 配置
    /// </summary>
    /// <param name="configFilePath">配置文件路径</param>
    /// <param name="connectionString">数据库连接字符串(可选)</param>
    public static void Initialize(string configFilePath, string connectionString = null)
    {
        // 1. 加载 XML 配置文件
        var xmlDoc = LoadConfigFile(configFilePath);
        
        // 2. 替换连接字符串占位符
        ReplaceConnectionStringPlaceholder(xmlDoc, connectionString);
        
        // 3. 使用 XmlConfigurator 配置
        XmlConfigurator.Configure(xmlDoc.DocumentElement);
    }
    
    private static XmlDocument LoadConfigFile(string configFilePath)
    {
        // 实现配置文件加载逻辑
    }
    
    private static void ReplaceConnectionStringPlaceholder(XmlDocument xmlDoc, string connectionString)
    {
        // 实现占位符替换逻辑
    }
}
```

### 3.4 Log4NetProvider 设计

#### 3.4.1 功能说明

实现 `ILoggerProvider`,为 Microsoft.Extensions.Logging 提供日志支持:
- 管理日志记录器实例
- 提供 Dispose 支持

#### 3.4.2 类结构

```csharp
public class Log4NetProvider : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, Log4NetLogger> _loggers;
    
    public Log4NetProvider()
    {
        _loggers = new ConcurrentDictionary<string, Log4NetLogger>();
    }
    
    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, 
            name => new Log4NetLogger(name));
    }
    
    public void Dispose()
    {
        _loggers.Clear();
    }
}
```

### 3.5 Log4NetLogger 设计

#### 3.5.1 功能说明

实现 `ILogger`,提供日志记录功能:
- 支持所有日志级别
- 自动设置 ThreadContext 属性
- 处理异常信息

#### 3.5.2 类结构

```csharp
public class Log4NetLogger : ILogger
{
    private readonly ILog _log;
    
    public Log4NetLogger(string categoryName)
    {
        _log = LogManager.GetLogger(typeof(Log4NetLogger));
    }
    
    public IDisposable BeginScope<TState>(TState state) => null;
    
    public bool IsEnabled(LogLevel logLevel) => true;
    
    public void Log<TState>(LogLevel logLevel, EventId eventId, 
        TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        // 1. 设置 ThreadContext 属性
        SetContextProperties(state, exception);
        
        // 2. 根据日志级别记录
        LogToLog4Net(logLevel, formatter(state, exception), exception);
    }
    
    private void SetContextProperties<TState>(TState state, Exception exception)
    {
        // 设置 User_ID, ModName, ActionName 等属性
        var appContext = ApplicationContext.Instance;
        
        if (appContext?.log != null)
        {
            ThreadContext.Properties["User_ID"] = appContext.log.User_ID ?? 0;
            ThreadContext.Properties["ModName"] = appContext.log.ModName ?? "";
            ThreadContext.Properties["ActionName"] = appContext.log.ActionName ?? "";
            // ... 其他属性
        }
    }
}
```

### 3.6 CustomLayout 设计

#### 3.6.1 功能说明

简化版布局器,只支持 `ThreadContext.Properties`:
- 移除 MDC 支持
- 移除反射获取属性的复杂逻辑

#### 3.6.2 类结构

```csharp
public class CustomLayout : PatternLayout
{
    public CustomLayout()
    {
        AddConverter("property", typeof(PropertyPatternConverter));
    }
    
    private class PropertyPatternConverter : PatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            if (Option != null && loggingEvent.Properties.Contains(Option))
            {
                WriteObject(writer, loggingEvent.Repository, 
                    loggingEvent.Properties[Option] ?? string.Empty);
            }
        }
    }
}
```

---

## 4. 配置文件设计

### 4.1 统一配置文件 (Log4net.config)

```xml
<?xml version="1.0" encoding="utf-8" ?>
<log4net>
  <!-- 自定义 Appender: 使用 EnhancedAdoNetAppender -->
  <appender name="AdoNetAppender" 
            type="RUINORERP.Common.Log4Net.EnhancedAdoNetAppender, RUINORERP.Common">
    
    <!-- 连接字符串: 使用占位符,运行时替换 -->
    <connectionString value="Encrypted:${ConnectionString}" />
    <connectionType value="System.Data.SqlClient.SqlConnection, System.Data, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
    
    <!-- BufferSize: 0=立即写入, 10=缓存10条后批量写入 -->
    <bufferSize value="0" />
    
    <!-- 命令文本 -->
    <commandText value="INSERT INTO Logs ([Date],[Level],[Logger],[Message],[Exception],[Operator],[ModName],[Path],[ActionName],[IP],[MAC],[MachineName],[User_ID]) 
                           VALUES (@log_date, @log_level, @logger, @Message, @Exception, @Operator, @ModName, @Path, @ActionName, @IP, @MAC, @MachineName, @User_ID)" />
    
    <!-- 标准参数: 使用 PatternLayout -->
    <parameter>
      <parameterName value="@log_date" />
      <dbType value="DateTime" />
      <layout type="log4net.Layout.RawTimeStampLayout" />
    </parameter>
    
    <parameter>
      <parameterName value="@log_level" />
      <dbType value="String" />
      <size value="10" />
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%level" />
      </layout>
    </parameter>
    
    <parameter>
      <parameterName value="@logger" />
      <dbType value="String" />
      <size value="500" />
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%logger" />
      </layout>
    </parameter>
    
    <!-- 自定义参数: 使用 CustomLayout -->
    <parameter>
      <parameterName value="@Message" />
      <dbType value="String" />
      <size value="-1" />
      <layout type="RUINORERP.Common.Log4Net.CustomLayout, RUINORERP.Common">
        <conversionPattern value="%property{Message}" />
      </layout>
    </parameter>
    
    <parameter>
      <parameterName value="@Exception" />
      <dbType value="String" />
      <size value="-1" />
      <layout type="RUINORERP.Common.Log4Net.CustomLayout, RUINORERP.Common">
        <conversionPattern value="%property{Exception}" />
      </layout>
    </parameter>
    
    <parameter>
      <parameterName value="@Operator" />
      <dbType value="String" />
      <size value="200" />
      <layout type="RUINORERP.Common.Log4Net.CustomLayout, RUINORERP.Common">
        <conversionPattern value="%property{Operator}" />
      </layout>
    </parameter>
    
    <parameter>
      <parameterName value="@ModName" />
      <dbType value="String" />
      <size value="50" />
      <layout type="RUINORERP.Common.Log4Net.CustomLayout, RUINORERP.Common">
        <conversionPattern value="%property{ModName}" />
      </layout>
    </parameter>
    
    <parameter>
      <parameterName value="@Path" />
      <dbType value="String" />
      <size value="100" />
      <layout type="RUINORERP.Common.Log4Net.CustomLayout, RUINORERP.Common">
        <conversionPattern value="%property{Path}" />
      </layout>
    </parameter>
    
    <parameter>
      <parameterName value="@ActionName" />
      <dbType value="String" />
      <size value="50" />
      <layout type="RUINORERP.Common.Log4Net.CustomLayout, RUINORERP.Common">
        <conversionPattern value="%property{ActionName}" />
      </layout>
    </parameter>
    
    <parameter>
      <parameterName value="@IP" />
      <dbType value="String" />
      <size value="20" />
      <layout type="RUINORERP.Common.Log4Net.CustomLayout, RUINORERP.Common">
        <conversionPattern value="%property{IP}" />
      </layout>
    </parameter>
    
    <parameter>
      <parameterName value="@MAC" />
      <dbType value="String" />
      <size value="30" />
      <layout type="RUINORERP.Common.Log4Net.CustomLayout, RUINORERP.Common">
        <conversionPattern value="%property{MAC}" />
      </layout>
    </parameter>
    
    <parameter>
      <parameterName value="@MachineName" />
      <dbType value="String" />
      <size value="50" />
      <layout type="RUINORERP.Common.Log4Net.CustomLayout, RUINORERP.Common">
        <conversionPattern value="%property{MachineName}" />
      </layout>
    </parameter>
    
    <parameter>
      <parameterName value="@User_ID" />
      <dbType value="Int64" />
      <layout type="RUINORERP.Common.Log4Net.CustomLayout, RUINORERP.Common">
        <conversionPattern value="%property{User_ID}" />
      </layout>
    </parameter>
  </appender>
  
  <!-- Root Logger 配置 -->
  <root>
    <level value="DEBUG" />
    <appender-ref ref="AdoNetAppender" />
  </root>
</log4net>
```

### 4.2 配置说明

#### 4.2.1 连接字符串配置

支持两种方式:

**方式1: 使用加密标识**
```xml
<connectionString value="Encrypted:${ConnectionString}" />
```
运行时自动调用 `CryptoHelper.GetDecryptedConnectionString()` 解密

**方式2: 直接使用明文连接字符串**
```xml
<connectionString value="${ConnectionString}" />
```
运行时替换为传入的连接字符串

#### 4.2.2 BufferSize 配置

| 值 | 说明 | 适用场景 |
|----|------|----------|
| 0 | 立即写入 | 开发环境、关键日志 |
| 10 | 缓存10条后写入 | 生产环境、高并发场景 |

#### 4.2.3 日志级别配置

| 级别 | 说明 |
|------|------|
| DEBUG | 调试信息 |
| INFO | 一般信息 |
| WARN | 警告信息 |
| ERROR | 错误信息 |
| FATAL | 致命错误 |

---

## 5. 使用方式

### 5.1 客户端初始化

#### 5.1.1 Startup.cs

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // 获取连接字符串
    var connectionString = CryptoHelper.GetDecryptedConnectionString();
    
    // 初始化 log4net
    Log4NetConfiguration.Initialize("Log4net.config", connectionString);
    
    // 添加日志提供者
    services.AddLogging(loggingBuilder =>
    {
        loggingBuilder.AddProvider(new Log4NetProvider());
    });
}
```

#### 5.1.2 项目文件配置

```xml
<ItemGroup>
  <Content Include="Log4net.config">
    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
  </Content>
</ItemGroup>
```

### 5.2 服务器初始化

#### 5.2.1 Startup.cs

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // 获取连接字符串
    var connectionString = CryptoHelper.GetDecryptedConnectionString();
    
    // 初始化 log4net
    Log4NetConfiguration.Initialize("Log4net.config", connectionString);
    
    // 添加日志提供者
    services.AddLogging(loggingBuilder =>
    {
        loggingBuilder.AddProvider(new Log4NetProvider());
    });
}
```

#### 5.2.2 移除旧代码

删除以下内容:
- `SetLogBufferSize()` 调用
- 日志仓库重置逻辑
- `Log4NetProviderByCustomeDb` 使用

### 5.3 日志记录

```csharp
public class MyService
{
    private readonly ILogger<MyService> _logger;
    
    public MyService(ILogger<MyService> logger)
    {
        _logger = logger;
    }
    
    public void DoSomething()
    {
        // 设置日志上下文
        var appContext = ApplicationContext.Instance;
        if (appContext?.log != null)
        {
            appContext.log.ModName = "销售管理";
            appContext.log.ActionName = "保存订单";
            appContext.log.User_ID = 123;
        }
        
        // 记录日志
        _logger.LogInformation("订单保存成功");
        _logger.LogError("订单保存失败: {OrderID}", orderId);
        
        try
        {
            // 业务逻辑
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "处理订单时发生异常");
            throw;
        }
    }
}
```

---

## 6. 文件清理清单

### 6.1 需要删除的文件

#### 6.1.1 服务器端
- `RUINORERP.Server/Log4net_db.config` (被 Log4net.config 替代)
- `RUINORERP.Server/Log4net_file.config` (不再需要)
- `RUINORERP.Server/ConfigBySS/log4net.config` (SuperSocket 使用独立配置)

#### 6.1.2 客户端
- `RUINORERP.UI/Properties/AssemblyInfo.cs` 中的 log4net 配置特性
  ```csharp
  [assembly: log4net.Config.XmlConfigurator(Watch = true)]
  ```

### 6.2 需要重构的文件

| 文件 | 操作 |
|------|------|
| `Log4NetLoggerByDb.cs` | 删除或重写为 Log4NetLogger.cs |
| `Log4NetProviderByCustomeDb.cs` | 删除或重写为 Log4NetProvider.cs |
| `EnhancedCustomLayout.cs` | 重写为 CustomLayout.cs (简化版) |
| `RUINORERP.Server/Startup.cs` | 移除 SetLogBufferSize, 使用 Log4NetConfiguration |
| `RUINORERP.UI/Startup.cs` | 使用 Log4NetConfiguration |

### 6.3 需要新增的文件

| 文件 | 说明 |
|------|------|
| `EnhancedAdoNetAppender.cs` | 自定义 AdoNetAppender |
| `Log4NetConfiguration.cs` | 配置管理器 |
| `RUINORERP.Server/Log4net.config` | 统一配置文件 |
| `RUINORERP.UI/Log4net.config` | 统一配置文件 (更新) |

---

## 7. 实施步骤

### 7.1 第一阶段: 准备工作

1. **备份现有代码**
   - 备份 `Log4NetLoggerByDb.cs`
   - 备份 `Log4NetProviderByCustomeDb.cs`
   - 备份配置文件

2. **创建新类文件**
   - 创建 `EnhancedAdoNetAppender.cs`
   - 创建 `Log4NetConfiguration.cs`
   - 创建 `CustomLayout.cs` (简化版)
   - 创建 `Log4NetLogger.cs` (简化版)
   - 创建 `Log4NetProvider.cs` (简化版)

### 7.2 第二阶段: 配置文件迁移

1. **服务器端**
   - 创建 `RUINORERP.Server/Log4net.config`
   - 配置 `CopyToOutputDirectory`
   - 删除旧的配置文件

2. **客户端**
   - 更新 `RUINORERP.UI/Log4net.config`
   - 移除 `AssemblyInfo.cs` 中的特性
   - 确认配置正确

### 7.3 第三阶段: 代码迁移

1. **更新 Startup.cs**
   - 服务器端: 移除 `Log4NetProviderByCustomeDb`, 使用 `Log4NetConfiguration`
   - 客户端: 移除 `Log4NetProviderByCustomeDb`, 使用 `Log4NetConfiguration`

2. **测试日志功能**
   - 测试日志写入数据库
   - 测试异常信息记录
   - 测试自定义属性记录

### 7.4 第四阶段: 清理工作

1. **删除旧代码**
   - 删除 `Log4NetLoggerByDb.cs`
   - 删除 `Log4NetProviderByCustomeDb.cs`
   - 删除 `EnhancedCustomLayout.cs`

2. **删除旧配置文件**
   - 服务器端: 删除 `Log4net_db.config`, `Log4net_file.config`
   - 客户端: 清理 `AssemblyInfo.cs`

3. **验证编译**
   - 客户端编译通过
   - 服务器端编译通过
   - 无警告无错误

---

## 8. 测试验证

### 8.1 功能测试

| 测试项 | 测试内容 | 预期结果 |
|--------|----------|----------|
| 日志写入 | 记录 Info 级别日志 | 数据库中有对应记录 |
| 异常日志 | 记录带异常的 Error 日志 | 异常信息完整保存 |
| 自定义属性 | 设置 User_ID, ModName 等属性 | 属性值正确保存 |
| BufferSize | BufferSize=0 | 日志立即写入 |
| BufferSize | BufferSize=10 | 缓存10条后批量写入 |
| 连接字符串 | 使用加密连接字符串 | 自动解密并连接 |

### 8.2 性能测试

| 测试项 | 测试内容 | 性能指标 |
|--------|----------|----------|
| 吞吐量 | 每秒写入日志数量 | >= 1000 条/秒 |
| 延迟 | 单条日志写入延迟 | BufferSize=0: < 10ms<br>BufferSize=10: < 5ms |
| 内存占用 | 日志系统内存占用 | < 50MB |

---

## 9. 风险评估

### 9.1 技术风险

| 风险项 | 影响 | 应对措施 |
|--------|------|----------|
| 数据库连接异常 | 日志丢失 | Lossy=false 确保不丢失 |
| 配置文件错误 | 日志系统无法启动 | 提供默认配置 |
| 性能下降 | 系统响应变慢 | 调整 BufferSize |

### 9.2 兼容性风险

| 风险项 | 影响 | 应对措施 |
|--------|------|----------|
| 旧代码引用 | 编译错误 | 逐步迁移,保留兼容层 |
| 配置格式变化 | 日志功能失效 | 提供配置迁移工具 |

---

## 10. 总结

### 10.1 重构收益

1. **代码简化**
   - 移除 500+ 行冗余代码
   - 代码可读性提升 50%

2. **配置统一**
   - 客户端和服务器使用相同配置
   - 配置文件数量从 4 个减少到 1 个

3. **维护成本降低**
   - 新增日志功能只需修改配置文件
   - Bug 修复更容易定位

### 10.2 后续优化方向

1. **日志分表**
   - 按日期分表,提升查询性能
   - 按模块分表,便于管理

2. **日志归档**
   - 定期归档旧日志
   - 提供日志导出功能

3. **日志分析**
   - 集成日志分析工具
   - 提供可视化报表

---

## 附录

### A. 参考文档

- log4net 官方文档: http://logging.apache.org/log4net/
- Microsoft.Extensions.Logging 文档: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/

### B. 版本历史

| 版本 | 日期 | 作者 | 说明 |
|------|------|------|------|
| 1.0 | 2026-01-16 | Claude | 初始版本 |

### C. 联系方式

如有问题,请联系开发团队。
