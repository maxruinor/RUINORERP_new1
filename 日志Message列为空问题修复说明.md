# 日志 Message 列为空问题修复说明

## 问题描述

日志记录成功写入数据库 `Logs` 表,但 `Message` 列始终为空字符串。

## 根本原因

在 `Log4NetLogger.cs` 中,调用 `SetContextProperties` 方法时传递了错误的参数:

```csharp
// ❌ 错误代码 (修复前)
SetContextProperties<string>(null, exception);  // 第一个参数是 null!
```

导致在 `SetContextProperties` 方法中:
```csharp
string message = state?.ToString() ?? string.Empty;  // state 是 null,所以 message 永远是空字符串
ThreadContext.Properties["Message"] = message;        // 设置的是空字符串
```

而 `Log4net.config` 中使用 `%property{Message}` 从 `ThreadContext.Properties["Message"]` 读取,自然得到空字符串。

## 修复内容

### 1. Log4NetLogger.cs - 核心修复

#### 修改点 1: BufferLog 方法 (第 142-164 行)
```csharp
// ✅ 修复后
private void BufferLog(LogLevel logLevel, string message, Exception exception)
{
    // 设置日志上下文属性 - 传递格式化后的消息
    SetContextProperties(message, exception);  // 传递已格式化的 message
    // ...
}
```

#### 修改点 2: WriteLogDirectly 方法 (第 169-197 行)
```csharp
// ✅ 修复后
private void WriteLogDirectly(LogLevel logLevel, string message, Exception exception)
{
    // 设置日志上下文属性 - 传递格式化后的消息
    SetContextProperties(message, exception);  // 传递已格式化的 message
    // ...
}
```

#### 修改点 3: SetContextProperties 方法签名 (第 256 行)
```csharp
// ❌ 修复前
private void SetContextProperties<TState>(TState state, Exception exception)

// ✅ 修复后
private void SetContextProperties(string message, Exception exception)
```

#### 修改点 4: SetContextProperties 内部逻辑 (第 328-329 行)
```csharp
// ❌ 修复前
string message = state?.ToString() ?? string.Empty;
ThreadContext.Properties["Message"] = message;

// ✅ 修复后
// 设置消息(直接使用已格式化的消息)
ThreadContext.Properties["Message"] = message ?? string.Empty;
```

#### 修改点 5: FlushBufferCallback 静态方法 (第 219-267 行)
由于 `FlushBufferCallback` 是静态方法,无法调用实例方法 `SetContextProperties`,需要手动设置 ThreadContext 属性:

```csharp
// ✅ 修复后 - 在刷新缓冲时手动设置所有属性
var appContext = RUINORERP.Model.Context.ApplicationContext.Current;
if (appContext?.log != null)
{
    ThreadContext.Properties["UserName"] = appContext.log.UserName ?? string.Empty;
    ThreadContext.Properties["ModName"] = appContext.log.ModName ?? string.Empty;
    // ... 其他属性
}

// 设置消息和异常
ThreadContext.Properties["Message"] = entry.Message ?? string.Empty;
ThreadContext.Properties["Exception"] = entry.Exception != null ? GetFullExceptionStatic(entry.Exception) : string.Empty;
```

#### 修改点 6: 添加静态版本的 GetFullExceptionStatic 方法 (第 360-389 行)
```csharp
/// <summary>
/// 获取完整的异常信息(静态版本,用于静态方法调用)
/// </summary>
private static string GetFullExceptionStatic(Exception exception)
{
    // ... 原有逻辑
}
```

### 2. EnhancedAdoNetAppender.cs - 增强错误处理

增强了 `Append` 和 `SendBuffer` 方法的错误日志输出,确保任何写入失败都能被详细记录到调试输出:

```csharp
protected override void Append(LoggingEvent loggingEvent)
{
    try
    {
        base.Append(loggingEvent);
    }
    catch (Exception ex)
    {
        // 记录详细的错误信息到调试输出
        System.Diagnostics.Debug.WriteLine($"[Log4Net] 写入日志失败:");
        System.Diagnostics.Debug.WriteLine($"  - 消息: {loggingEvent.RenderedMessage}");
        System.Diagnostics.Debug.WriteLine($"  - 级别: {loggingEvent.Level}");
        System.Diagnostics.Debug.WriteLine($"  - 记录器: {loggingEvent.LoggerName}");
        System.Diagnostics.Debug.WriteLine($"  - 错误: {ex.Message}");
        // ...
        
        // 不抛出异常,避免影响主程序
    }
}
```

### 3. CustomLogContent.cs - 增加调试输出

在 `LogInfoPatternConverter.LookupProperty` 方法中增加了 DEBUG 模式的调试输出,帮助排查属性映射问题:

```csharp
#if DEBUG
System.Diagnostics.Debug.WriteLine($"[Log4Net] 从 ThreadContext 找到属性 '{property}': {(propertyValue == null ? "null" : $"'{propertyValue}'")}");
#endif
```

## 验证步骤

### 1. 编译项目
```bash
# 在 Visual Studio 中重新生成 RUINORERP.Common 项目
# 或运行: dotnet build RUINORERP.Common/RUINORERP.Common.csproj
```

### 2. 运行测试
执行任意会触发日志记录的操作,例如:
- 登录系统
- 执行单据锁定/解锁操作
- 查看调试输出窗口

### 3. 检查调试输出
在 Visual Studio 的"输出"窗口中,应该能看到类似以下的调试信息:
```
[Log4Net] 从 ThreadContext 找到属性 'Message': '解锁请求参数无效: 单据ID=0'
[Log4Net] 从 ThreadContext 找到属性 'UserName': 'admin'
[Log4Net] 从 ThreadContext 找到属性 'ModName': '锁管理'
...
```

### 4. 检查数据库
查询 `Logs` 表,确认 `Message` 列不再为空:
```sql
SELECT TOP 10 ID, Date, Level, Message, UserName, ModName 
FROM Logs 
ORDER BY Date DESC
```

预期结果:
- `Message` 列应该有实际内容
- `UserName`、`ModName` 等其他字段也应该有值

## 技术要点

### 为什么之前会出错?

1. **参数传递错误**: `SetContextProperties<string>(null, exception)` 传入了 `null`
2. **泛型误导**: 使用泛型 `<TState>` 让人误以为可以处理任意类型,但实际上只使用了 `ToString()`
3. **消息格式化时机**: 在 `Log` 方法中已经通过 `formatter` 格式化了消息,但没有传递给 `SetContextProperties`

### 修复的关键

1. **直接传递格式化后的消息**: 将 `formatter.Invoke(state, exception)` 的结果直接传递给 `SetContextProperties`
2. **简化方法签名**: 去掉不必要的泛型,直接使用 `string` 类型
3. **静态方法特殊处理**: `FlushBufferCallback` 是静态方法,需要手动设置 ThreadContext 属性

### 为什么不抛出异常?

在 `EnhancedAdoNetAppender` 中捕获异常但不抛出,是为了:
1. **避免影响主程序**: 日志记录失败不应该导致业务逻辑中断
2. **便于排查问题**: 通过调试输出可以看到详细的错误信息
3. **符合日志系统设计原则**: 日志系统应该是"尽力而为"的,不应成为系统的单点故障

## 相关文件

- `RUINORERP.Common/Log4Net/Log4NetLogger.cs` - 核心修复
- `RUINORERP.Common/Log4Net/EnhancedAdoNetAppender.cs` - 错误处理增强
- `RUINORERP.Common/Log4Net/CustomLogContent.cs` - 调试输出增强
- `RUINORERP.UI/Log4net.config` - Log4Net 配置文件(无需修改)
- `RUINORERP.Model/Logs.cs` - 数据库实体模型(无需修改)

## 注意事项

1. **DEBUG 模式**: `CustomLogContent.cs` 中的调试输出仅在 DEBUG 模式下生效,Release 模式不会影响性能
2. **线程安全**: `ThreadContext.Properties` 是线程局部的,每个线程有自己的属性副本,无需担心并发问题
3. **缓冲区**: Debug/Trace 级别的日志会使用缓冲机制,其他级别直接写入
4. **连接字符串**: 确保 `Log4NetConfiguration.Initialize` 已正确初始化,连接字符串已解密

## 后续优化建议

1. **移除缓冲机制**: 如果性能不是瓶颈,可以考虑移除 Debug/Trace 的缓冲机制,简化代码
2. **统一日志接口**: 考虑完全迁移到 Microsoft.Extensions.Logging,减少对 log4net 的直接依赖
3. **结构化日志**: 考虑使用结构化日志框架(如 Serilog),便于日志分析和查询
4. **日志采样**: 对于高频日志,可以实现采样策略,只记录部分日志以减少数据库压力
