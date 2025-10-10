# CommandScanner 改进记录

## 概述
本次改进主要解决了扫描注册耦合问题，使处理器扫描方法支持缓存注册控制参数，与 `ScanCommands` 方法保持一致的设计模式。

## 主要改进

### 1. 扫描注册解耦
**问题**: `ScanCommandHandlers` 方法强制注册到映射表，缺乏灵活性
**解决方案**: 
- 添加 `registerToMapping` 参数控制是否注册到映射表
- 保持扫描和注册的独立性
- 支持只扫描不注册的使用场景

**代码变更**:
```csharp
// 原方法（强制注册）
public List<Type> ScanCommandHandlers(string namespaceFilter = null, params Assembly[] assemblies)

// 改进后方法（可选注册）
public List<Type> ScanCommandHandlers(string namespaceFilter = null, bool registerToMapping = false, params Assembly[] assemblies)
```

### 2. 明确注册意图的重载方法
**目的**: 提供语义更清晰的API
**实现**:
```csharp
public List<Type> ScanAndRegisterCommandHandlers(string namespaceFilter = null, params Assembly[] assemblies)
{
    return ScanCommandHandlers(namespaceFilter, true, assemblies);
}
```

### 3. 修复类型转换问题
**问题**: 直接传递 `Type` 给需要 `ICommandHandler` 实例的方法
**解决方案**: 
```csharp
// 创建处理器实例
if (Activator.CreateInstance(handlerType) is ICommandHandler handlerInstance)
{
    RegisterHandlerToMapping(handlerInstance);
    registeredCount++;
}
```

### 4. 修复重复注册问题 ⭐ 重要修复
**问题**: `AutoRegisterCommandsAndHandlersAsync` 方法存在严重的重复注册问题
- 第一次：扫描时 `ScanCommandHandlers(namespaceFilter, true, assemblies)` 已经注册到映射表
- 第二次：循环中又调用 `RegisterHandlerAsync` 再次注册相同类型

**解决方案**: 
```csharp
// 扫描时不注册，避免重复
var handlerTypes = ScanCommandHandlers(namespaceFilter, false, assemblies);
// 统一在 RegisterHandlerAsync 中完成注册
```

**影响**: 
- 消除重复注册导致的映射表污染
- 减少不必要的处理器实例创建
- 提高注册流程的可靠性和性能

## 优化命令类型注册逻辑 ⭐ 代码质量

**问题**: 在扫描命令类型时存在冗余的重复判断逻辑：
- 扫描阶段：`commandTypeMap.ContainsKey(commandId)` 检查重复
- 注册阶段：`RegisterCommandType` 方法中无重复检查，直接覆盖
- 两个不同的集合（`commandTypeMap` vs `_commandTypes`）但目的相同

**解决方案**: 
1. **将重复检查放到最底层**：修改 `RegisterCommandType` 方法，添加重复检查并返回是否为新注册
2. **简化扫描逻辑**：根据 `registerTypes` 参数决定使用哪个集合进行重复判断
3. **消除冗余代码**：移除 `ScanAndRegisterCommands` 中的重复注册逻辑

**核心改进**:
```csharp
// 优化前：
if (!commandTypeMap.ContainsKey(commandId))
{
    commandTypeMap[commandId] = commandType;
    totalCommandsFound++;
    if (registerTypes)
    {
        RegisterCommandType(commandId, commandType);
    }
}

// 优化后：
if (registerTypes)
{
    // 注册模式：使用RegisterCommandType的重复检查
    if (RegisterCommandType(commandId, commandType))
    {
        totalCommandsFound++;
    }
}
else
{
    // 扫描模式：只在临时map中记录
    if (!commandTypeMap.ContainsKey(commandId))
    {
        commandTypeMap[commandId] = commandType;
        totalCommandsFound++;
    }
}
```

**改进收益**:
- ✅ 消除重复判断逻辑
- ✅ 统一重复检查策略
- ✅ 提高代码可维护性
- ✅ 注册方法返回状态信息，便于统计

## 使用示例

### 只扫描不注册
```csharp
var handlerTypes = scanner.ScanCommandHandlers("MyApp.Handlers", registerToMapping: false);
```

### 扫描并注册（明确意图）
```csharp
var handlerTypes = scanner.ScanAndRegisterCommandHandlers("MyApp.Handlers");
```

### 扫描并注册（传统方式）
```csharp
var handlerTypes = scanner.ScanCommandHandlers("MyApp.Handlers", registerToMapping: true);
```

## 设计优势

1. **一致性**: 与 `ScanCommands` 方法的设计模式保持一致
2. **灵活性**: 支持多种使用场景（仅扫描、扫描+注册）
3. **明确性**: 通过重载方法提供语义清晰的API
4. **可维护性**: 扫描逻辑与注册逻辑解耦，便于独立修改

## 验证结果
- ✅ 编译通过（0错误，15警告）
- ✅ 方法重命名已完成
- ✅ 扫描注册解耦已实现
- ✅ 类型转换问题已修复