# RUINORERP.PacketSpec 架构文档

## 1. 项目概述
- **项目类型**：.NET Standard 2.0类库
- **核心职责**：统一数据包规范定义和命令调度
- **关键依赖**：
  - MessagePack (2.5.129)
  - StackExchange.Redis (2.8.24)
  - Newtonsoft.Json (13.0.3)

## 2. 核心数据结构

### 2.1 PacketModel
```csharp
public class PacketModel : IKeyedPackageInfo<uint> {
    public string PacketId { get; set; }
    public CommandId Command { get; set; }
    public uint Key => (uint)Command;
    public byte[] Data { get; set; }
}
```

## 3. 命令调度系统

### 3.1 CommandDispatcher
```csharp
public class CommandDispatcher {
    private ConcurrentDictionary<string, ICommandHandler> _handlers;
    
    public async Task<CommandResult> DispatchAsync(ICommand command) {
        // 1. 验证命令
        // 2. 查找处理器
        // 3. 执行处理
    }
}
```

## 4. 关键实现细节

### 4.1 数据包构造
```csharp
public static PacketModel FromOriginalData(byte[] data) {
    return new PacketModel {
        Data = data,
        Size = data.Length,
        Checksum = ComputeSHA256(data)
    };
}
```

### 4.2 处理器选择
```csharp
private ICommandHandler SelectBestHandler(List<ICommandHandler> handlers) {
    return handlers
        .OrderByDescending(h => h.Priority)
        .ThenBy(h => h.CurrentLoad)
        .FirstOrDefault();
}
```

## 5. 性能优化

1. **对象池**：重用PacketModel实例
2. **内存池**：使用ArrayPool
3. **并发控制**：命令级信号量

## 6. 设计决策

| 决策点 | 方案 | 原因 |
|--------|------|------|
| 数据包ID | ULID | 分布式唯一性 |
| 序列化 | MessagePack | 高性能 |