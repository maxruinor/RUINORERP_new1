# CommandId 类型迁移指南

## 概述

本文档指导开发者从旧的 `uint` 类型命令标识符迁移到新的 `CommandId` 类型，以获得更好的类型安全性和代码可读性。

## 主要变更

### 1. 接口变更

**ICommandHandler 接口**
```csharp
// 旧版本
IReadOnlyList<uint> SupportedCommands { get; }

// 新版本
IReadOnlyList<CommandId> SupportedCommands { get; }
```

**新增方法**
```csharp
// 向后兼容的 CanHandle 重载
bool CanHandle(uint commandCode);
```

### 2. BaseCommandHandler 变更

**SetSupportedCommands 方法**
```csharp
// 现在有四个重载版本，支持不同的使用场景
SetSupportedCommands(params CommandId[] commands)           // 推荐
SetSupportedCommands(params uint[] commands)                // 向后兼容
SetSupportedCommands(IEnumerable<CommandId> commands)     // 推荐
SetSupportedCommands(IEnumerable<uint> commands)          // 向后兼容
```

**CanHandle 方法**
```csharp
// 主要方法（推荐）
bool CanHandle(QueuedCommand cmd);

// 向后兼容方法
bool CanHandle(uint commandCode);
```

## 迁移步骤

### 步骤1: 更新命令处理器

**旧代码：**
```csharp
public class MyCommandHandler : BaseCommandHandler
{
    public MyCommandHandler()
    {
        Name = "我的命令处理器";
        SetSupportedCommands(0x0001, 0x0002, 0x0003); // 使用uint
    }
    
    protected override async Task<BaseCommand<IResponse>> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
    {
        var commandCode = cmd.Command.CommandIdentifier; // uint类型
        // 处理逻辑
    }
}
```

**新代码（推荐）：**
```csharp
public class MyCommandHandler : BaseCommandHandler
{
    public MyCommandHandler()
    {
        Name = "我的命令处理器";
        SetSupportedCommands(
            new CommandId(CommandCategory.System, 0x01, "系统命令1"),
            new CommandId(CommandCategory.System, 0x02, "系统命令2"),
            new CommandId(CommandCategory.System, 0x03, "系统命令3")
        );
    }
    
    protected override async Task<BaseCommand<IResponse>> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
    {
        var commandId = cmd.Command.CommandIdentifier; // CommandId类型
        // 现在可以使用 commandId.Category 和 commandId.OperationCode
        // 处理逻辑
    }
}
```

**新代码（向后兼容）：**
```csharp
public class MyCommandHandler : BaseCommandHandler
{
    public MyCommandHandler()
    {
        Name = "我的命令处理器";
        // 仍然可以使用uint，系统会自动转换
        SetSupportedCommands(0x0001, 0x0002, 0x0003);
    }
}
```

### 步骤2: 更新命令创建

**旧代码：**
```csharp
// 创建命令时使用uint
var command = new BaseCommand<IResponse>
{
    CommandIdentifier = 0x0001 // uint类型
};
```

**新代码（推荐）：**
```csharp
// 创建命令时使用CommandId
var command = new BaseCommand<IResponse>
{
    CommandIdentifier = new CommandId(CommandCategory.System, 0x01, "系统初始化")
};
```

### 步骤3: 更新命令判断逻辑

**旧代码：**
```csharp
// 在处理器中判断命令类型
protected override async Task<BaseCommand<IResponse>> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
{
    var commandCode = cmd.Command.CommandIdentifier; // uint
    
    switch (commandCode)
    {
        case 0x0001:
            // 处理系统初始化
            break;
        case 0x0101:
            // 处理用户登录
            break;
    }
}
```

**新代码（推荐）：**
```csharp
// 在处理器中判断命令类型
protected override async Task<BaseCommand<IResponse>> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
{
    var commandId = cmd.Command.CommandIdentifier; // CommandId
    
    switch (commandId.Category)
    {
        case CommandCategory.System:
            switch (commandId.OperationCode)
            {
                case 0x01:
                    // 处理系统初始化
                    break;
            }
            break;
            
        case CommandCategory.Authentication:
            switch (commandId.OperationCode)
            {
                case 0x01:
                    // 处理用户登录
                    break;
            }
            break;
    }
}
```

## CommandId 类型详解

### 结构
```csharp
public struct CommandId : IEquatable<CommandId>
{
    public CommandCategory Category { get; set; }      // 命令类别
    public byte OperationCode { get; set; }            // 操作码
    public string Name { get; set; }                   // 命令名称
    public ushort FullCode { get; }                    // 完整的命令码
}
```

### 创建方式
```csharp
// 方法1: 构造函数
var cmd1 = new CommandId(CommandCategory.System, 0x01, "系统初始化");

// 方法2: 从ushort转换
var cmd2 = CommandId.FromUInt16(0x0001);

// 方法3: 隐式转换
CommandId cmd3 = (ushort)0x0101;
```

### 转换操作
```csharp
var commandId = new CommandId(CommandCategory.System, 0x01);

// 转换为数值类型
ushort ushortValue = commandId;  // 隐式转换
uint uintValue = commandId;    // 隐式转换

// 从数值转换回CommandId
var converted = CommandId.FromUInt16(ushortValue);
```

## 最佳实践

### 1. 使用有意义的命令名称
```csharp
SetSupportedCommands(
    new CommandId(CommandCategory.System, 0x01, "心跳检测"),
    new CommandId(CommandCategory.System, 0x02, "系统状态"),
    new CommandId(CommandCategory.Authentication, 0x01, "用户登录"),
    new CommandId(CommandCategory.Authentication, 0x02, "用户注销")
);
```

### 2. 利用类别进行分组处理
```csharp
protected override async Task<BaseCommand<IResponse>> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
{
    var commandId = cmd.Command.CommandIdentifier;
    
    return commandId.Category switch
    {
        CommandCategory.System => await HandleSystemCommand(commandId.OperationCode),
        CommandCategory.Authentication => await HandleAuthCommand(commandId.OperationCode),
        CommandCategory.Cache => await HandleCacheCommand(commandId.OperationCode),
        _ => new BaseCommand<IResponse>().CreateError($"不支持的命令类别: {commandId.Category}")
    };
}
```

### 3. 使用命令常量
```csharp
public static class SystemCommands
{
    public static readonly CommandId Heartbeat = new CommandId(CommandCategory.System, 0x01, "心跳");
    public static readonly CommandId StatusCheck = new CommandId(CommandCategory.System, 0x02, "状态检查");
    public static readonly CommandId ConfigUpdate = new CommandId(CommandCategory.System, 0x03, "配置更新");
}

// 使用常量
SetSupportedCommands(SystemCommands.Heartbeat, SystemCommands.StatusCheck, SystemCommands.ConfigUpdate);
```

## 向后兼容性

### 现有代码无需修改
- 现有的 `SetSupportedCommands(uint[])` 调用仍然有效
- 现有的命令处理逻辑仍然可以工作
- 系统会自动进行uint到CommandId的转换

### 渐进式迁移
1. **第一阶段**: 保持现有代码不变，系统会自动处理转换
2. **第二阶段**: 逐步更新处理器，使用新的CommandId语法
3. **第三阶段**: 完全迁移到CommandId，获得更好的类型安全性

## 常见问题

### Q: 我必须立即迁移所有代码吗？
A: 不需要。系统提供了完全的向后兼容性，您可以逐步迁移。

### Q: CommandId会增加内存开销吗？
A: CommandId是结构体，性能开销很小。增加的名称字段有助于调试和日志记录。

### Q: 如何处理动态命令码？
A: 仍然可以使用FromUInt16方法从运行时值创建CommandId。

### Q: 序列化会受影响吗？
A: CommandId已添加MessagePack支持，序列化格式与ushort兼容。

## 总结

迁移到CommandId类型将为您带来：

1. **更好的类型安全性**: 编译时检查，减少运行时错误
2. **更好的可读性**: 命令名称让代码更易理解
3. **更好的可维护性**: 结构化数据便于扩展和修改
4. **向后兼容性**: 现有代码无需修改即可运行

建议在新开发的处理器中使用CommandId类型，并逐步迁移现有代码以获得更好的开发体验。