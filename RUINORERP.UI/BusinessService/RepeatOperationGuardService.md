# RepeatOperationGuardService 使用帮助

## 概述

RepeatOperationGuardService 是一个用于防止客户端重复操作的服务类，提供了统一的防重复操作判断机制。该服务支持多种操作类型，包括 MenuItemEnums 类型和自定义 string 类型，并支持方法级别的防重复调用。

### 核心功能

- ✅ 支持 MenuItemEnums 类型的操作防重复
- ✅ 支持 string 类型的自定义操作名称防重复
- ✅ 支持方法级别的防重复调用
- ✅ 支持实体级别的防重复检查
- ✅ 支持同步和异步方法的防重复执行
- ✅ 支持状态消息显示
- ✅ 自动清理过期操作记录
- ✅ 线程安全设计

## 快速开始

### 1. 获取服务实例

在类中通过依赖注入获取服务实例：

```csharp
// 方法1：在构造函数中注入
private readonly RepeatOperationGuardService _guardService;

public YourClass(RepeatOperationGuardService guardService)
{
    _guardService = guardService;
}

// 方法2：通过 Startup.GetFromFac 获取
private readonly RepeatOperationGuardService _guardService = Startup.GetFromFac<RepeatOperationGuardService>();

// 方法3：延迟初始化（适用于 UI 控件类，避免设计时错误）
private RepeatOperationGuardService _guardService;

private void EnsureGuardService()
{
    if (_guardService == null)
    {
        _guardService = Startup.GetFromFac<RepeatOperationGuardService>();
    }
}
```

### 2. 基本使用

在按钮点击或方法调用前进行防重复检查：

```csharp
// 检查是否应该阻止操作
if (_guardService.ShouldBlockOperation(MenuItemEnums.查询, this.GetType().Name))
{
    return;
}

// 记录操作
_guardService.RecordOperation(MenuItemEnums.查询, this.GetType().Name);

// 执行实际操作
```

## 核心功能

### 1. 操作防重复检查

#### 支持 MenuItemEnums 类型

```csharp
bool ShouldBlockOperation(MenuItemEnums operationType, string operationSource, long entityId = 0, int debounceIntervalMs = 1000, bool showStatusMessage = true)
```

#### 支持 string 类型

```csharp
bool ShouldBlockOperation(string operationName, string operationSource, long entityId = 0, int debounceIntervalMs = 1000, bool showStatusMessage = true)
```

### 2. 记录操作

#### 支持 MenuItemEnums 类型

```csharp
void RecordOperation(MenuItemEnums operationType, string operationSource, long entityId = 0)
```

#### 支持 string 类型

```csharp
void RecordOperation(string operationName, string operationSource, long entityId = 0)
```

### 3. 方法级防重复调用

#### 同步方法

```csharp
bool ExecuteWithGuard(string methodName, string operationSource, Action action, int debounceIntervalMs = 1000, bool showStatusMessage = false)
```

#### 异步方法

```csharp
Task<bool> ExecuteWithGuardAsync(string methodName, string operationSource, Func<Task> func, int debounceIntervalMs = 1000, bool showStatusMessage = false)
```

### 4. 辅助方法

```csharp
// 判断是否应该阻止方法调用
bool ShouldBlockMethod(string methodName, string operationSource, int debounceIntervalMs = 1000, bool showStatusMessage = false)

// 记录方法调用
void RecordMethodCall(string methodName, string operationSource)
```

### 5. 清理操作记录

```csharp
// 清理过期的操作记录
void CleanupOperationRecords(object state)

// 清理指定操作源的所有操作记录
void CleanupOperationRecords(string operationSource)

// 清理指定实体的所有操作记录
void CleanupEntityOperationRecords(string operationSource, long entityId)

// 清理所有操作记录
void CleanupAllOperationRecords()
```

## 使用示例

### 示例 1：在 DoButtonClick 方法中使用

```csharp
protected virtual async Task DoButtonClick(MenuItemEnums menuItem)
{
    // 确保服务实例已初始化
    if (_guardService == null)
    {
        _guardService = Startup.GetFromFac<RepeatOperationGuardService>();
    }

    // 防重复操作检查
    if (_guardService.ShouldBlockOperation(menuItem, this.GetType().Name, showStatusMessage: true))
    {
        return;
    }

    // 记录操作
    _guardService.RecordOperation(menuItem, this.GetType().Name);

    // 执行实际操作
    switch (menuItem)
    {
        case MenuItemEnums.查询:
            await QueryAsync();
            break;
        // 其他操作...
    }
}
```

### 示例 2：保护查询方法

```csharp
public void Query()
{
    _guardService.ExecuteWithGuard(
        nameof(Query),
        this.GetType().Name,
        () =>
        {
            // 查询逻辑
            Console.WriteLine("执行查询操作...");
            // 模拟查询耗时
            Thread.Sleep(1000);
        },
        showStatusMessage: true
    );
}
```

### 示例 3：保护异步方法

```csharp
public async Task QueryAsync()
{
    await _guardService.ExecuteWithGuardAsync(
        nameof(QueryAsync),
        this.GetType().Name,
        async () =>
        {
            // 异步查询逻辑
            Console.WriteLine("执行异步查询操作...");
            // 模拟异步查询耗时
            await Task.Delay(1000);
        },
        showStatusMessage: true
    );
}
```

### 示例 4：实体级别的防重复检查

```csharp
public void EditEntity(long entityId)
{
    if (_guardService.ShouldBlockOperation("EditEntity", this.GetType().Name, entityId, showStatusMessage: true))
    {
        return;
    }

    _guardService.RecordOperation("EditEntity", this.GetType().Name, entityId);

    // 执行实体编辑操作
    Console.WriteLine($"编辑实体 {entityId}...");
}
```

## 最佳实践

### 1. 操作源标识的选择

- 使用当前类名作为操作源标识，便于区分不同类的相同操作
- 对于同一个类的不同实例，可以考虑在操作源标识中加入实例ID

```csharp
// 推荐：使用类名作为操作源标识
_guardService.ShouldBlockOperation(operationType, this.GetType().Name);

// 不推荐：使用固定字符串作为操作源标识
_guardService.ShouldBlockOperation(operationType, "MyOperation");
```

### 2. 防抖时间间隔的设置

- 根据操作的实际耗时设置合理的防抖时间间隔
- 对于快速操作，可以使用较短的防抖时间间隔（如 500ms）
- 对于耗时操作，可以使用较长的防抖时间间隔（如 2000ms）

```csharp
// 快速操作使用较短的防抖时间间隔
_guardService.ShouldBlockOperation(operationType, this.GetType().Name, debounceIntervalMs: 500);

// 耗时操作使用较长的防抖时间间隔
_guardService.ShouldBlockOperation(operationType, this.GetType().Name, debounceIntervalMs: 2000);
```

### 3. 状态消息的使用

- 对于用户交互操作，建议显示状态消息
- 对于后台操作，建议关闭状态消息

```csharp
// 用户交互操作显示状态消息
_guardService.ShouldBlockOperation(operationType, this.GetType().Name, showStatusMessage: true);

// 后台操作关闭状态消息
_guardService.ShouldBlockOperation(operationType, this.GetType().Name, showStatusMessage: false);
```

### 4. 实体级别的防重复检查

- 对于需要操作实体的方法，建议使用实体ID作为实体标识
- 这样可以确保不同实体的相同操作可以并行执行，同一实体的相同操作会被适当阻止

```csharp
// 使用实体ID进行实体级别的防重复检查
_guardService.ShouldBlockOperation(operationType, this.GetType().Name, entityId);
```

## 注意事项

1. **服务注册**：RepeatOperationGuardService 已在 Startup.cs 中注册为单例服务，无需手动注册
2. **线程安全**：服务内部使用 ConcurrentDictionary 确保线程安全，支持多线程并发调用
3. **过期清理**：服务会自动清理过期的操作记录，默认清理间隔为 1 分钟，过期时间为 30 秒
4. **设计时错误**：在 UI 控件类中使用时，建议使用延迟初始化，避免设计时错误
5. **异常处理**：ExecuteWithGuard 和 ExecuteWithGuardAsync 方法会捕获并记录异常，但会重新抛出，建议在调用处进行异常处理

## API 参考

### 公共属性

| 属性名 | 类型 | 说明 |
|--------|------|------|
| OperationRecordCount | int | 获取当前操作记录数量 |

### 公共方法

#### ShouldBlockOperation（MenuItemEnums 类型）

```csharp
bool ShouldBlockOperation(MenuItemEnums operationType, string operationSource, long entityId = 0, int debounceIntervalMs = 1000, bool showStatusMessage = true)
```

**参数说明**：
- `operationType`：操作类型
- `operationSource`：操作源标识（例如：表单名称、控件名称等）
- `entityId`：实体ID - 用于实体级别的防重复检查
- `debounceIntervalMs`：防抖时间间隔（毫秒）
- `showStatusMessage`：是否显示状态消息

**返回值**：
- `true`：应该阻止操作
- `false`：允许操作

#### ShouldBlockOperation（string 类型）

```csharp
bool ShouldBlockOperation(string operationName, string operationSource, long entityId = 0, int debounceIntervalMs = 1000, bool showStatusMessage = true)
```

**参数说明**：
- `operationName`：操作名称
- `operationSource`：操作源标识
- `entityId`：实体ID
- `debounceIntervalMs`：防抖时间间隔
- `showStatusMessage`：是否显示状态消息

**返回值**：
- `true`：应该阻止操作
- `false`：允许操作

#### RecordOperation（MenuItemEnums 类型）

```csharp
void RecordOperation(MenuItemEnums operationType, string operationSource, long entityId = 0)
```

**参数说明**：
- `operationType`：操作类型
- `operationSource`：操作源标识
- `entityId`：实体ID

#### RecordOperation（string 类型）

```csharp
void RecordOperation(string operationName, string operationSource, long entityId = 0)
```

**参数说明**：
- `operationName`：操作名称
- `operationSource`：操作源标识
- `entityId`：实体ID

#### ShouldBlockMethod

```csharp
bool ShouldBlockMethod(string methodName, string operationSource, int debounceIntervalMs = 1000, bool showStatusMessage = false)
```

**参数说明**：
- `methodName`：方法名称
- `operationSource`：操作源标识
- `debounceIntervalMs`：防抖时间间隔
- `showStatusMessage`：是否显示状态消息

**返回值**：
- `true`：应该阻止方法调用
- `false`：允许方法调用

#### RecordMethodCall

```csharp
void RecordMethodCall(string methodName, string operationSource)
```

**参数说明**：
- `methodName`：方法名称
- `operationSource`：操作源标识

#### ExecuteWithGuard

```csharp
bool ExecuteWithGuard(string methodName, string operationSource, Action action, int debounceIntervalMs = 1000, bool showStatusMessage = false)
```

**参数说明**：
- `methodName`：方法名称
- `operationSource`：操作源标识
- `action`：要执行的方法
- `debounceIntervalMs`：防抖时间间隔
- `showStatusMessage`：是否显示状态消息

**返回值**：
- `true`：成功执行了方法
- `false`：方法调用被阻止

#### ExecuteWithGuardAsync

```csharp
Task<bool> ExecuteWithGuardAsync(string methodName, string operationSource, Func<Task> func, int debounceIntervalMs = 1000, bool showStatusMessage = false)
```

**参数说明**：
- `methodName`：方法名称
- `operationSource`：操作源标识
- `func`：要执行的异步方法
- `debounceIntervalMs`：防抖时间间隔
- `showStatusMessage`：是否显示状态消息

**返回值**：
- `true`：成功执行了异步方法
- `false`：方法调用被阻止

#### CleanupOperationRecords（指定操作源）

```csharp
void CleanupOperationRecords(string operationSource)
```

**参数说明**：
- `operationSource`：操作源标识

#### CleanupEntityOperationRecords

```csharp
void CleanupEntityOperationRecords(string operationSource, long entityId)
```

**参数说明**：
- `operationSource`：操作源标识
- `entityId`：实体ID

#### CleanupAllOperationRecords

```csharp
void CleanupAllOperationRecords()
```

## 版本历史

### v1.0.0
- 初始版本
- 支持 MenuItemEnums 类型的操作防重复
- 支持 string 类型的自定义操作名称防重复
- 支持方法级别的防重复调用
- 支持实体级别的防重复检查
- 支持同步和异步方法的防重复执行
- 支持状态消息显示
- 自动清理过期操作记录

## 结语

RepeatOperationGuardService 提供了一个统一、灵活、易用的防重复操作机制，可以帮助您快速实现各种场景下的防重复操作保护。通过合理使用该服务，您可以提高用户体验，减少不必要的服务器负载，确保系统的稳定性和可靠性。

如果您在使用过程中遇到任何问题或有任何建议，欢迎随时反馈！