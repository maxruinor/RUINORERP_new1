# SuperSocket 2.0 集成指南

## 项目概述

本指南介绍了如何在RUINORERP.PacketSpec项目中集成和使用SuperSocket 2.0的命令和命令过滤器功能。通过本次重构，项目现在支持SuperSocket的现代化命令处理系统，提供更高效、更灵活的网络通信能力。

## 重构内容概览

1. **项目依赖更新**：添加了SuperSocket.Server、SuperSocket.Command和SuperSocket.Channel包
2. **核心接口适配**：修改了PacketModel和CommandAttribute以兼容SuperSocket命令系统
3. **命令系统重构**：实现了SuperSocket命令适配器和会话适配器
4. **命令过滤器集成**：提供了可扩展的命令过滤机制
5. **配置与启动流程**：简化了SuperSocket服务器的配置和启动过程

## 项目结构

重构后，项目新增了以下关键目录和文件：

```
RUINORERP.PacketSpec/
├── Commands/
│   └── SuperSocket/            # SuperSocket相关命令处理器
│       ├── SuperSocketCommandAdapter.cs  # SuperSocket命令适配器
│       ├── CommandFilterBase.cs          # 命令过滤器基类
│       └── EchoCommandHandler.cs         # 示例命令处理器
├── Adapters/                   # 适配器目录
│   └── SuperSocketSessionAdapter.cs      # SuperSocket会话适配器
├── Configuration/              # 配置目录
│   └── SuperSocketServerConfig.cs        # SuperSocket服务器配置
├── Bootstrap/                  # 启动器目录
│   └── SuperSocketServerBootstrap.cs     # SuperSocket服务器启动器
├── SUPERSOCKET_REFACTORE_PLAN.md         # 重构计划文档
└── SUPERSOCKET_INTEGRATION_GUIDE.md      # 集成指南文档
```

## 核心组件说明

### 1. SuperSocket命令适配器

`SuperSocketCommandAdapter`负责将SuperSocket的命令调用转换为项目现有的命令处理系统：

- 实现了`IAsyncCommand`接口，处理SuperSocket命令执行
- 负责命令创建、调度和结果处理
- 处理命令执行过程中的异常情况

### 2. SuperSocket会话适配器

`SuperSocketSessionAdapter`将SuperSocket会话转换为项目的`ISessionContext`接口：

- 提供会话属性管理功能
- 支持IP地址获取和会话状态跟踪
- 实现最后活动时间更新机制

### 3. 命令过滤器

命令过滤器提供了命令执行前后的拦截能力：

- `CommandFilterBase`：过滤器基类，提供通用过滤逻辑
- `LoggingCommandFilter`：日志过滤器，记录命令执行信息
- `AuthenticationCommandFilter`：认证过滤器，验证命令执行权限

### 4. 服务器配置和启动

`SuperSocketServerConfig`和`SuperSocketServerBootstrap`提供了服务器配置和启动功能：

- 支持多种配置选项（端口、缓冲区大小、超时时间等）
- 提供便捷的初始化、启动、停止和重启方法
- 支持自定义服务注册和依赖注入

## 使用方法

### 1. 初始化和启动服务器

**基本启动方式**：

```csharp
// 创建并启动默认配置的服务器（端口4040）
var bootstrap = await SuperSocketServerBootstrap.StartDefaultAsync();

// 或者指定端口
var bootstrap = await SuperSocketServerBootstrap.StartDefaultAsync(8080);
```

**自定义配置启动**：

```csharp
// 创建服务器启动器
var bootstrap = new SuperSocketServerBootstrap();

// 配置服务器选项
bootstrap.ServerConfig.ServerName = "MyERPServer";
bootstrap.ServerConfig.ListeningPort = 5000;
bootstrap.ServerConfig.MaxConnections = 5000;
bootstrap.ServerConfig.IdleTimeout = 1800; // 30分钟

// 初始化并配置自定义服务
await bootstrap.InitializeAsync(services =>
{
    // 注册自定义服务
    services.AddSingleton<IMyService, MyServiceImpl>();
    
    // 注册自定义命令处理器
    services.AddSingleton(typeof(MyCommandHandler));
});

// 启动服务器
await bootstrap.StartAsync();
```

**使用选项对象配置**：

```csharp
// 创建选项对象
var options = new SuperSocketServerOptions
{
    ServerName = "AdvancedERPServer",
    ListeningPort = 6000,
    MaxConnections = 10000,
    AutoStart = true, // 初始化后自动启动
    ConfigureServices = services =>
    {
        // 配置服务
        services.AddSingleton<IMyService, MyServiceImpl>();
    }
};

// 使用选项初始化服务器
var bootstrap = await SuperSocketServerExtensions.CreateAndStartWithOptionsAsync(options);
```

### 2. 创建自定义命令处理器

要创建自定义命令处理器，需要实现`IAsyncCommand<PacketModel>`接口，并使用`Command`特性标记：

```csharp
[Command(1001)] // 命令ID
[LoggingCommandFilter] // 添加日志过滤器
public class LoginCommandHandler : IAsyncCommand<PacketModel>
{
    private readonly IUserService _userService;
    
    // 构造函数注入依赖
    public LoginCommandHandler(IUserService userService)
    {
        _userService = userService;
    }
    
    public async ValueTask ExecuteAsync(IAppSession session, PacketModel package)
    {
        try
        {
            // 处理登录逻辑
            var username = package.Data?["username"]?.ToString();
            var password = package.Data?["password"]?.ToString();
            
            // 验证用户
            var user = await _userService.ValidateUserAsync(username, password);
            
            if (user != null)
            {
                // 登录成功
                session.Items["UserId"] = user.UserId;
                session.Items["IsAuthenticated"] = true;
                
                // 发送成功响应
                var response = new PacketModel
                {
                    PacketId = package.PacketId,
                    Command = package.Command,
                    ResponseType = ResponseType.Success,
                    Data = new { userId = user.UserId, userName = user.UserName }
                };
                await session.SendAsync(response);
            }
            else
            {
                // 登录失败
                var response = new PacketModel
                {
                    PacketId = package.PacketId,
                    Command = package.Command,
                    ResponseType = ResponseType.Error,
                    Data = new { errorMessage = "用户名或密码错误" }
                };
                await session.SendAsync(response);
            }
        }
        catch (Exception ex)
        {
            // 处理异常
            var response = new PacketModel
            {
                PacketId = package.PacketId,
                Command = package.Command,
                ResponseType = ResponseType.Error,
                Data = new { errorMessage = ex.Message }
            };
            await session.SendAsync(response);
        }
    }
}
```

### 3. 创建自定义命令过滤器

要创建自定义命令过滤器，需要继承`CommandFilterBase`类：

```csharp
public class PerformanceMonitorFilter : CommandFilterBase
{
    public PerformanceMonitorFilter() : base(50) { }
    
    protected override async ValueTask<bool> OnExecutingAsync(IAppSession session, PacketModel packet, CommandExecutingContext context)
    {
        // 记录开始时间
        context.Items["StartTime"] = DateTime.UtcNow;
        
        await Task.CompletedTask;
        return true; // 允许命令执行
    }
    
    protected override async ValueTask OnExecutedAsync(IAppSession session, PacketModel packet, CommandExecutingContext context)
    {
        // 计算执行时间
        if (context.Items.TryGetValue("StartTime", out var startTimeObj) && startTimeObj is DateTime startTime)
        {
            var executionTime = DateTime.UtcNow - startTime;
            
            // 如果执行时间超过阈值，记录警告
            if (executionTime.TotalMilliseconds > 100)
            {
                LogWarning($"命令执行时间过长: {packet.Command}, 耗时: {executionTime.TotalMilliseconds}ms, 会话ID: {session.SessionID}");
            }
        }
        
        await Task.CompletedTask;
    }
    
    protected override void LogWarning(string message)
    {
        // 实现日志记录逻辑
        Console.WriteLine($"[PERFORMANCE WARNING] {message}");
    }
}
```

### 4. 发送消息和广播

```csharp
// 发送消息到指定会话
var message = new PacketModel
{
    PacketId = Guid.NewGuid().ToString(),
    Command = 2000, // 自定义命令ID
    ResponseType = ResponseType.Notification,
    Data = new { notificationType = "message", content = "您有一条新消息" }
};

await bootstrap.SendToSessionAsync("session-id-123", message);

// 广播消息到所有会话
var broadcastMessage = new PacketModel
{
    PacketId = Guid.NewGuid().ToString(),
    Command = 2001, // 广播命令ID
    ResponseType = ResponseType.Broadcast,
    Data = new { announcement = "系统将在10分钟后进行维护" }
};

await bootstrap.BroadcastAsync(broadcastMessage);
```

### 5. 监控服务器状态

```csharp
// 检查服务器是否运行
if (bootstrap.IsRunning)
{
    Console.WriteLine("服务器正在运行中");
    
    // 获取当前连接数
    var sessionCount = bootstrap.GetSessionCount();
    Console.WriteLine($"当前连接数: {sessionCount}