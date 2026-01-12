# ERP系统请求-响应流程分析

## 核心组件

### 1. ClientCommunicationService.cs
- `SendRequestAsync`: 发送请求并等待响应的核心方法
- `OnReceived`: 处理服务器收到的数据
- `HandleResponsePacket`: 处理响应数据包
- `_pendingRequests`: 存储待处理请求的字典

## 请求-响应流程

### 1. 发送请求阶段

```csharp
// 1. 客户端调用发送请求方法
var response = await _communicationService.SendCommandWithResponseAsync<LoginResponse>(
    AuthenticationCommands.Login, loginRequest, cancellationToken);

// 2. 内部调用SendRequestAsync
// 3. 创建TaskCompletionSource用于等待响应
var tcs = new TaskCompletionSource<PacketModel>();

// 4. 创建PendingRequest对象
var pendingRequest = new PendingRequest
{
    Tcs = tcs,
    CreatedAt = DateTime.UtcNow,
    CommandId = commandId.ToString()
};

// 5. 将请求添加到待处理队列
_pendingRequests.TryAdd(request.RequestId, pendingRequest);

// 6. 发送请求到服务器
await SendPacketCoreAsync<TRequest>(_socketClient, commandId, request, timeoutMs, ct, ResponseTypeName);

// 7. 等待响应或超时
var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);
```

### 2. 接收响应阶段

```csharp
// 1. 服务器响应到达，调用OnReceived方法
private async void OnReceived(PacketModel packet)
{
    // 2. 验证数据包有效性
    if (!packet.IsValid())
    {
        _logger.LogWarning("接收到无效数据包");
        return;
    }

    // 3. 检查是否为响应包
    if (IsResponsePacket(packet))
    {
        // 4. 处理响应包
        if (HandleResponsePacket(packet))
        {
            _logger.LogDebug("数据包作为响应处理完成");
            return;
        }
    }
}

// 5. 处理响应包
private bool HandleResponsePacket(PacketModel packet)
{
    // 6. 获取请求ID
    var requestId = packet?.ExecutionContext?.RequestId;
    if (string.IsNullOrEmpty(requestId))
        return false;

    // 7. 从待处理队列中移除请求
    if (_pendingRequests.TryRemove(requestId, out var pendingRequest))
    {
        // 8. 设置任务结果，唤醒等待的请求
        return pendingRequest.Tcs.TrySetResult(packet);
    }

    return false;
}
```

### 3. 响应返回阶段

```csharp
// 1. tcs.TrySetResult(packet) 被调用
// 2. SendRequestAsync中的tcs.Task完成
var responsePacket = await tcs.Task;

// 3. 返回响应给调用者
return responsePacket as PacketModel;

// 4. finally块确保清理待处理请求
finally
{
    _pendingRequests.TryRemove(request.RequestId, out _);
}
```

## 关键点

1. **请求ID匹配**: 每个请求都有唯一的RequestId，用于匹配请求和响应
2. **TaskCompletionSource**: 用于异步等待响应
3. **PendingRequest队列**: 存储待处理请求的字典
4. **超时处理**: 每个请求都有超时时间，避免无限等待
5. **自动清理**: 无论成功还是失败，请求都会从队列中移除
6. **响应优先处理**: 收到数据后，优先尝试作为响应处理

## 如何处理来自服务器的请求

如果服务器主动向客户端发送请求（不是对客户端请求的响应），会：

1. `IsResponsePacket` 返回false
2. 检查是否为服务器主动推送命令 (`IsServerPushCommand`)
3. 如果是推送命令，调用 `HandleServerPushCommandAsync` 处理
4. 否则，作为通用命令调用 `HandleGeneralCommandAsync` 处理

## 代码优化建议

1. **添加更详细的日志**: 在关键步骤添加日志，便于调试
2. **增强异常处理**: 确保在各种异常情况下都能正确清理资源
3. **添加请求跟踪**: 可以考虑添加请求跟踪ID，便于跨系统调试
4. **优化超时机制**: 可以根据网络状况动态调整超时时间
5. **添加重试机制**: 对于网络不稳定的情况，可以添加自动重试机制