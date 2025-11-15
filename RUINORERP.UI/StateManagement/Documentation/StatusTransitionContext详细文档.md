# StatusTransitionContext 类详细文档

## 概述

`StatusTransitionContext` 类是 RUINORERP 项目中状态管理系统的核心实现，它实现了 `IStatusTransitionContext` 接口，提供了完整的状态转换功能。该类负责管理实体的状态转换过程，包括状态验证、转换执行、历史记录和事件触发。

## 类定义

```csharp
public class StatusTransitionContext : IStatusTransitionContext
```

**位置**: `RUINORERP.Model\Base\StatusManager\Core\StatusTransitionContext.cs`

## 依赖项

`StatusTransitionContext` 类依赖于以下组件：

- `ILogger<StatusTransitionContext>` - 用于日志记录
- `IUnifiedStateManager` - 状态管理器
- `IStatusTransitionEngine` - 状态转换引擎
- `IStatusTransitionLogger` - 状态转换日志记录器

## 构造函数

```csharp
public StatusTransitionContext(
    BaseEntity entity,
    Type statusType,
    object initialStatus,
    ILogger<StatusTransitionContext> logger,
    IUnifiedStateManager stateManager,
    IStatusTransitionEngine transitionEngine,
    IStatusTransitionLogger transitionLogger)
```

### 参数说明

- `entity` (BaseEntity): 状态转换的目标实体
- `statusType` (Type): 状态类型（如 DataStatus、ActionStatus 或业务状态枚举）
- `initialStatus` (object): 初始状态值
- `logger` (ILogger<StatusTransitionContext>): 日志记录器
- `stateManager` (IUnifiedStateManager): 状态管理器
- `transitionEngine` (IStatusTransitionEngine): 状态转换引擎
- `transitionLogger` (IStatusTransitionLogger): 状态转换日志记录器

## 属性

| 属性名 | 类型 | 访问修饰符 | 描述 |
|--------|------|------------|------|
| Entity | BaseEntity | get | 关联的实体对象 |
| StatusType | Type | get | 状态类型 |
| CurrentStatus | object | get | 当前状态值 |
| Reason | string | get/set | 转换原因 |
| UserId | string | get/set | 用户ID |
| TransitionTime | DateTime | get | 转换时间 |
| AdditionalData | Dictionary<string, object> | get | 附加数据 |

## 事件

| 事件名 | 委托类型 | 描述 |
|--------|----------|------|
| StatusChanged | EventHandler<StateTransitionEventArgs> | 状态变更事件 |

## 方法

### 状态获取方法

#### GetDataStatus
```csharp
public DataStatus GetDataStatus()
```
- **返回值**: `DataStatus` - 当前数据性状态
- **描述**: 获取实体的数据性状态

#### GetBusinessStatus<T>
```csharp
public T GetBusinessStatus<T>() where T : Enum
```
- **类型参数**: `T` - 业务状态枚举类型
- **返回值**: `T` - 当前业务性状态
- **描述**: 获取实体的业务性状态

#### GetActionStatus
```csharp
public ActionStatus GetActionStatus()
```
- **返回值**: `ActionStatus` - 当前操作状态
- **描述**: 获取实体的操作状态

### 状态设置方法

#### SetDataStatusAsync
```csharp
public async Task<bool> SetDataStatusAsync(DataStatus status, string reason = null)
```
- **参数**:
  - `status` (DataStatus): 要设置的数据性状态
  - `reason` (string, 可选): 状态变更原因
- **返回值**: `Task<bool>` - 设置是否成功
- **描述**: 异步设置实体的数据性状态

#### SetBusinessStatusAsync<T>
```csharp
public async Task<bool> SetBusinessStatusAsync<T>(T status, string reason = null) where T : Enum
```
- **类型参数**: `T` - 业务状态枚举类型
- **参数**:
  - `status` (T): 要设置的业务性状态
  - `reason` (string, 可选): 状态变更原因
- **返回值**: `Task<bool>` - 设置是否成功
- **描述**: 异步设置实体的业务性状态

#### SetActionStatusAsync
```csharp
public async Task<bool> SetActionStatusAsync(ActionStatus status, string reason = null)
```
- **参数**:
  - `status` (ActionStatus): 要设置的操作状态
  - `reason` (string, 可选): 状态变更原因
- **返回值**: `Task<bool>` - 设置是否成功
- **描述**: 异步设置实体的操作状态

### 状态转换方法

#### TransitionTo
```csharp
public async Task<StateTransitionResult> TransitionTo(object targetStatus, string reason = null)
```
- **参数**:
  - `targetStatus` (object): 目标状态
  - `reason` (string, 可选): 转换原因
- **返回值**: `Task<StateTransitionResult>` - 转换结果
- **描述**: 执行状态转换到指定状态

#### GetAvailableTransitions
```csharp
public IEnumerable<object> GetAvailableTransitions()
```
- **返回值**: `IEnumerable<object>` - 可转换状态列表
- **描述**: 获取当前状态下可转换的状态列表

#### CanTransitionTo
```csharp
public async Task<bool> CanTransitionTo(object targetStatus)
```
- **参数**:
  - `targetStatus` (object): 目标状态
- **返回值**: `Task<bool>` - 是否可以转换到目标状态
- **描述**: 检查是否可以转换到指定状态

### 历史记录方法

#### GetTransitionHistory
```csharp
public IEnumerable<IStatusTransitionRecord> GetTransitionHistory()
```
- **返回值**: `IEnumerable<IStatusTransitionRecord>` - 转换历史记录
- **描述**: 获取实体的状态转换历史记录

#### LogTransition
```csharp
public void LogTransition(object fromStatus, object toStatus, string reason = null)
```
- **参数**:
  - `fromStatus` (object): 源状态
  - `toStatus` (object): 目标状态
  - `reason` (string, 可选): 转换原因
- **描述**: 记录状态转换

## 内部类

### StatusTransitionRecord

```csharp
private class StatusTransitionRecord : IStatusTransitionRecord
```

实现了 `IStatusTransitionRecord` 接口，用于记录状态转换的详细信息。

## 使用示例

### 基本使用

```csharp
// 创建状态转换上下文
var context = new StatusTransitionContext(
    entity,
    typeof(DataStatus),
    DataStatus.草稿,
    logger,
    stateManager,
    transitionEngine,
    transitionLogger);

// 获取当前状态
var currentStatus = context.GetDataStatus();

// 设置新状态
var success = await context.SetDataStatusAsync(DataStatus.确认, "用户确认");

// 检查是否可以转换
var canTransition = await context.CanTransitionTo(DataStatus.完结);

// 执行转换
var result = await context.TransitionTo(DataStatus.完结, "流程完成");

// 获取转换历史
var history = context.GetTransitionHistory();
```

### 订阅状态变更事件

```csharp
context.StatusChanged += (sender, e) => {
    Console.WriteLine($"状态从 {e.OldStatus} 变更为 {e.NewStatus}");
    Console.WriteLine($"变更原因: {e.Reason}");
    Console.WriteLine($"变更时间: {e.TransitionTime}");
};
```

## 注意事项

1. **线程安全**: `StatusTransitionContext` 不是线程安全的，在多线程环境中使用时需要进行适当的同步。

2. **状态类型**: 支持三种状态类型：
   - `DataStatus`: 数据性状态
   - `ActionStatus`: 操作状态
   - 自定义枚举类型: 业务性状态

3. **状态转换规则**: 状态转换必须遵循预定义的转换规则，否则转换将失败。

4. **事件触发**: 状态变更事件仅在状态实际发生变更时触发。

5. **历史记录**: 所有的状态转换都会被记录，可以通过 `GetTransitionHistory` 方法获取。

## 相关类型

- `IStatusTransitionContext`: 状态转换上下文接口
- `StateTransitionResult`: 状态转换结果
- `StateTransitionEventArgs`: 状态转换事件参数
- `IStatusTransitionRecord`: 状态转换记录接口
- `BaseEntity`: 基础实体类

## 扩展点

1. **自定义状态转换规则**: 通过实现 `IStateTransitionValidator` 接口

2. **自定义状态转换处理**: 通过实现 `IStatusTransitionHandler` 接口

3. **自定义状态转换日志**: 通过实现 `IStatusTransitionLogger` 接口

## 版本信息

- **版本**: V3
- **创建日期**: 2024年
- **作者**: RUINOR ERP开发团队
- **命名空间**: `RUINORERP.Model.Base.StatusManager.Core`