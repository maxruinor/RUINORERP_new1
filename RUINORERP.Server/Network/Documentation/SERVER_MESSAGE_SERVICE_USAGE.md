# 服务器消息服务使用指南

## 概述

本文档介绍了如何在服务器端使用新添加的消息服务功能，包括向客户端发送消息并等待响应的完整流程。

## 核心组件

### 1. SessionService
- 提供会话管理功能
- 支持向客户端发送命令并等待响应
- 处理客户端的响应数据

### 2. ServerMessageService
- 封装了常用的消息发送功能
- 提供简化的API接口

### 3. SuperSocketCommandAdapter
- 处理来自客户端的数据包
- 区分请求包和响应包
- 将响应包路由到对应的等待任务

## 使用方法

### 1. 发送消息并等待响应（同步方式）

```csharp
// 获取会话服务
var sessionService = serviceProvider.GetRequiredService<SessionService>();

// 创建消息请求
var messageData = new {
    TargetUserId = "user123",
    Message = "这是一条测试消息",
    Title = "系统通知"
};

var request = new MessageRequest(MessageType.Unknown, messageData);

// 发送消息并等待响应
var responsePacket = await sessionService.SendCommandAndWaitForResponseAsync(
    "session123", 
    MessageCommands.SendPopupMessage, 
    request, 
    30000, // 30秒超时
    CancellationToken.None);

if (responsePacket?.Response is MessageResponse response)
{
    if (response.IsSuccess)
    {
        Console.WriteLine($"消息发送成功: {response.Data}");
    }
    else
    {
        Console.WriteLine($"消息发送失败: {response.ErrorMessage}");
    }
}
```

### 2. 发送单向消息（不等待响应）

```csharp
// 获取会话服务
var sessionService = serviceProvider.GetRequiredService<SessionService>();

// 创建消息请求
var messageData = new {
    TargetUserId = "user123",
    Message = "这是一条单向消息",
    MessageType = "Text"
};

var request = new MessageRequest(MessageType.Unknown, messageData);

// 发送单向消息
var success = await sessionService.SendCommandAsync(
    "session123", 
    MessageCommands.SendMessageToUser, 
    request, 
    CancellationToken.None);

if (success)
{
    Console.WriteLine("消息发送成功");
}
else
{
    Console.WriteLine("消息发送失败");
}
```

### 3. 使用ServerMessageService

```csharp
// 获取服务器消息服务
var messageService = serviceProvider.GetRequiredService<ServerMessageService>();

// 发送弹窗消息并等待响应
var response = await messageService.SendPopupMessageAsync(
    "user123", 
    "测试消息内容", 
    "测试标题",
    30000, // 30秒超时
    CancellationToken.None);

if (response.IsSuccess)
{
    Console.WriteLine($"弹窗消息发送成功: {response.Data}");
}
else
{
    Console.WriteLine($"弹窗消息发送失败: {response.ErrorMessage}");
}
```

### 4. 使用事件驱动方式处理响应（异步方式）

```csharp
// 获取会话服务
var sessionService = serviceProvider.GetRequiredService<SessionService>();

// 订阅消息响应事件（如果实现了事件机制）
// 注意：这需要在SessionService中实现事件机制
sessionService.MessageResponseReceived += (sender, e) => {
    // 异步处理响应
    Task.Run(() => ProcessMessageResponse(e));
};

// 发送消息（不等待响应，通过事件处理）
var messageData = new {
    TargetUserId = "user123",
    Message = "这是一条异步处理的消息",
    MessageType = "Text"
};

var request = new MessageRequest(MessageType.Unknown, messageData);

// 发送单向消息
await sessionService.SendCommandAsync(
    "session123", 
    MessageCommands.SendMessageToUser, 
    request, 
    CancellationToken.None);

// 响应处理方法
private async Task ProcessMessageResponse(MessageResponseEventArgs e)
{
    if (e.ResponseData is MessageResponse response)
    {
        if (response.IsSuccess)
        {
            Console.WriteLine($"异步处理消息成功: {response.Data}");
            // 执行业务逻辑，如更新数据库、记录日志等
        }
        else
        {
            Console.WriteLine($"异步处理消息失败: {response.ErrorMessage}");
            // 执行错误处理逻辑
        }
    }
}
```

## 客户端处理流程

### 1. 客户端接收消息
客户端通过ClientCommunicationService接收服务器发送的消息：

```csharp
// 客户端订阅消息
messageService.PopupMessageReceived += (args) => {
    // 处理接收到的弹窗消息
    Console.WriteLine($"收到弹窗消息: {args.Data}");
    
    // 可以在这里执行相应的业务逻辑
};
```

### 2. 客户端发送响应
客户端处理完消息后，可以发送响应给服务器：

```csharp
// 客户端发送响应
var response = MessageResponse.Success(MessageType.Unknown, new { Status = "Processed" });
// 通过ClientCommunicationService发送响应
```

## 错误处理

### 超时处理
```csharp
try
{
    var responsePacket = await sessionService.SendCommandAndWaitForResponseAsync(
        sessionId, commandId, request, 5000, ct);
}
catch (TimeoutException ex)
{
    Console.WriteLine($"请求超时: {ex.Message}");
}
```

### 异常处理
```csharp
try
{
    var responsePacket = await sessionService.SendCommandAndWaitForResponseAsync(
        sessionId, commandId, request, timeoutMs, ct);
}
catch (Exception ex)
{
    Console.WriteLine($"发送消息时发生异常: {ex.Message}");
}
```

## 两种使用方式的对比

### 同步等待方式（推荐用于需要立即处理响应的场景）
- 优点：代码逻辑清晰，响应处理直接
- 缺点：阻塞线程，直到收到响应
- 适用场景：需要根据响应结果决定后续操作的业务逻辑

```csharp
public async Task<bool> SendImportantNotificationAsync(string userId, string message)
{
    var request = new MessageRequest(MessageType.Unknown, new { 
        TargetUserId = userId, 
        Message = message 
    });
    
    // 发送并等待响应
    var responsePacket = await _sessionService.SendCommandAndWaitForResponseAsync(
        sessionId, MessageCommands.SendPopupMessage, request);
    
    // 根据响应结果决定返回值
    if (responsePacket?.Response is MessageResponse response && response.IsSuccess)
    {
        // 处理成功逻辑
        return true;
    }
    else
    {
        // 处理失败逻辑
        return false;
    }
}
```

### 事件驱动方式（推荐用于异步处理响应的场景）
- 优点：不阻塞线程，适合高并发场景
- 缺点：代码逻辑分散，需要额外的事件处理机制
- 适用场景：发送后不需要立即处理响应，可以异步处理的业务逻辑

```csharp
public class NotificationService
{
    public NotificationService(SessionService sessionService)
    {
        // 订阅响应事件
        sessionService.MessageResponseReceived += OnMessageResponse;
    }
    
    public async Task SendNotificationAsync(string userId, string message)
    {
        var request = new MessageRequest(MessageType.Unknown, new { 
            TargetUserId = userId, 
            Message = message 
        });
        
        // 发送消息（不等待响应）
        await _sessionService.SendCommandAsync(
            sessionId, MessageCommands.SendPopupMessage, request);
        
        // 方法立即返回，响应通过事件处理
    }
    
    private async void OnMessageResponse(object sender, MessageResponseEventArgs e)
    {
        // 异步处理响应
        await ProcessResponseAsync(e);
    }
    
    private async Task ProcessResponseAsync(MessageResponseEventArgs e)
    {
        // 处理响应的业务逻辑
        if (e.ResponseData is MessageResponse response && response.IsSuccess)
        {
            // 处理成功响应
            Console.WriteLine("通知发送成功");
        }
        else
        {
            // 处理失败响应
            Console.WriteLine("通知发送失败");
        }
    }
}
```

## 最佳实践

### 1. 合理设置超时时间
根据业务需求设置合适的超时时间，避免过短或过长。

### 2. 异常处理
始终使用try-catch块处理可能的异常情况。

### 3. 资源管理
确保正确释放资源，特别是在长时间运行的服务中。

### 4. 日志记录
使用ILogger记录关键操作和错误信息，便于问题排查。

### 5. 选择合适的处理方式
- 对于需要立即处理响应结果的业务，使用同步等待方式
- 对于发送后不需要立即处理响应的业务，使用事件驱动方式

## 注意事项

1. 确保会话ID有效且客户端在线
2. 处理网络异常和超时情况
3. 避免在主线程中执行长时间等待操作
4. 合理使用单向消息和双向消息
5. 在使用事件驱动方式时，注意事件订阅和取消订阅，避免内存泄漏