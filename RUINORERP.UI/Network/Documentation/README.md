# RUINORERP 客户端网络通信模块

## 概述

本模块提供了RUINORERP客户端与服务器通信的完整解决方案，基于SuperSocket.ClientEngine构建，具有高性能、易用性和可扩展性等特点。

## 架构特点

- **轻量级设计**: 不依赖公共项目中的复杂调度器
- **高性能**: 基于SuperSocket.ClientEngine实现
- **安全性**: 集成数据加密和解密机制
- **易扩展**: 支持灵活的命令扩展机制
- **事件驱动**: 提供丰富的事件通知机制

## 核心组件

### 1. 网络层
- `ISocketClient`: Socket客户端接口
- `SuperSocketClient`: 基于SuperSocket的Socket客户端实现

### 2. 通信服务
- `IClientCommunicationService`: 客户端通信服务接口
- `ClientCommunicationService`: 客户端通信服务实现

### 3. 命令系统
- `ClientCommandDispatcher`: 客户端命令调度器
- `ClientCommandFactory`: 客户端命令工厂
- `BaseCommand`: 命令基类（来自PacketSpec）

### 4. 数据处理
- `RequestResponseManager`: 请求响应管理器
- `ClientEventManager`: 客户端事件管理器
- `BizPackageInfo`: 业务数据包信息
- `BizPipelineFilter`: 业务管道过滤器

### 5. 管理工具
- `HeartbeatManager`: 心跳管理器
- `CommunicationManager`: 通信管理器

## 使用方法

### 基本使用
```csharp
// 创建客户端组件
var socketClient = new SuperSocketClient();
var commandDispatcher = new ClientCommandDispatcher();
var communicationService = new ClientCommunicationService(socketClient, commandDispatcher);

// 连接服务器
await communicationService.ConnectAsync("127.0.0.1", 8080);

// 发送命令
var loginCommand = new LoginCommand("username", "password");
var response = await communicationService.SendCommandAsync<LoginResult>(loginCommand);
```

### 使用心跳管理
```csharp
var heartbeatManager = new HeartbeatManager(communicationService);
heartbeatManager.Start();
```

## 命令扩展

要添加新的命令，只需：

1. 创建继承自`BaseCommand`的新类
2. 添加`[Command]`特性标记
3. 实现具体的命令逻辑

示例：
```csharp
[Command(0x0201, "GetUserInfo", CommandCategory.User, Description = "获取用户信息")]
public class GetUserInfoCommand : BaseCommand
{
    public override CommandId CommandIdentifier => new CommandId(CommandCategory.User, 0x01);
    
    // 实现命令逻辑
    protected override async Task<CommandResult> OnExecuteAsync(CancellationToken cancellationToken)
    {
        // 命令执行逻辑
        return CommandResult.Success("用户信息");
    }
}
```

## 架构文档

详细的设计文档请参见：
- [CLIENT_ARCHITECTURE.md](./CLIENT_ARCHITECTURE.md)
- [ArchitectureDesign.md](../ArchitectureDesign.md)

## 示例代码

更多使用示例请查看[Examples](../Examples)目录：
- [ClientArchitectureExample.cs](../Examples/ClientArchitectureExample.cs)
- [CompleteUsageExample.cs](../Examples/CompleteUsageExample.cs)
- [LoginExample.cs](../Examples/LoginExample.cs)

## 依赖项

- SuperSocket.ClientEngine
- RUINORERP.PacketSpec（公共协议包）

一、三条决策路径
表格
复制
路径	适用场景	必须写的类	不写/复用	示例
① 最小路径	单向或一次性请求
只在一个窗体出现	指令类	服务类、处理器类复用 ApiResponse<T>	获取服务器时间
② 标准路径	两个以上窗体/模块调用	指令类 + 指令服务类	处理器类复用 ApiResponse<T>	查询用户档案
③ 完整路径	服务端需复杂校验、流程、事件	指令类 + 指令服务类 + 指令处理器类	复用 ApiResponse<T>	创建订单、审批流程
二、三步落地模板
指令类（任何路径都必须）
csharp
复制
[PacketCommand("Xxx", CommandCategory.Business)]
public class XxxCommand : BaseCommand
{
    public override CommandId CommandIdentifier 
        => new(CommandCategory.Business, 0x??);

    public XxxRequest Request { get; set; }   // 可选

    protected override Task<CommandResult> OnExecuteAsync(CancellationToken ct)
        => Task.FromResult(CommandResult.Success(Request)); // 仅打包
}
指令服务类（②③ 需要）
csharp
复制
public class XxxService
{
    private readonly IClientCommunicationService _comm;
    public XxxService(IClientCommunicationService comm) => _comm = comm;

    public async Task<ApiResponse<XxxResponse>> DoAsync(XxxRequest req)
    {
        var cmd = new XxxCommand { Request = req };
        return await _comm.SendCommandAsync<XxxResponse>(cmd);
    }
}
指令处理器类（仅③ 需要，放在服务端）
csharp
复制
public class XxxHandler : BaseCommandHandler
{
    public override IReadOnlyList<uint> SupportedCommands => 
        new[] { new CommandId(CommandCategory.Business, 0x??).FullCode };

    protected override Task<CommandResult> OnHandleAsync(ICommand cmd, CancellationToken ct)
    {
        // 复杂业务、校验、仓储、事件
        return Task.FromResult(CommandResult.Success(result));
    }
}
三、调用口诀
复制
// ① 最小路径
ApiResponse<Resp> rsp = await _comm.SendCommandAsync<Resp>(new XxxCommand());

// ②③ 标准路径
ApiResponse<Resp> rsp = await _xxxService.DoAsync(request);
四、前面对话一句话总结
“指令类是身份证，服务类是复用层，处理器是服务端专属；响应永远用 ApiResponse<T>，按场景选①②③即可。”