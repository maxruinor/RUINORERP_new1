# 重构 CacheCommandHandler 中的网络发送操作

## 1. 重构目标

将 CacheCommandHandler.cs 中三处直接调用 SendAsync 方法的代码修改为使用 ISessionService.SendPacketCoreAsync 方法，统一处理加密和构建过程，确保网络通信安全和一致性。

## 2. 重构内容

### 2.1 第一处修改（行号 419）

**原代码**：

```csharp
await sessionInfo.SendAsync(encryptedData.ToArray(), cancellationToken);
```

**重构后**：

```csharp
// 使用 ISessionService.SendPacketCoreAsync 统一处理发送
// 注：此处原代码已经完成加密，需要调整为使用 SendPacketCoreAsync 的标准流程
// 由于原代码上下文是推送缓存更新，应使用 CacheSync 命令
await _sessionService.SendPacketCoreAsync(sessionInfo, CacheCommands.CacheSync, request, 30000, cancellationToken, PacketDirection.ServerResponse);
```

### 2.2 第二处修改（行号 894）

**原代码**：

```csharp
await (session).SendAsync(dataBytes);
```

**重构后**：

```csharp
// 使用 ISessionService.SendPacketCoreAsync 统一处理发送
// 创建标准的缓存同步请求对象
var cacheSyncRequest = new CacheRequest
{
    TableName = tableName,
    Operation = "Update",
    Data = data,
    Timestamp = DateTime.Now
};
await _sessionService.SendPacketCoreAsync(session, CacheCommands.CacheSync, cacheSyncRequest, 30000, CancellationToken.None, PacketDirection.ServerResponse);
```

### 2.3 第三处修改（行号 1053）

**原代码**：

```csharp
await (session).SendAsync(dataBytes);
```

**重构后**：

```csharp
// 使用 ISessionService.SendPacketCoreAsync 统一处理发送
// 复用原有的 notification 对象作为请求数据
await _sessionService.SendPacketCoreAsync(session, CacheCommands.CacheSync, notification, 30000, CancellationToken.None, PacketDirection.ServerResponse);
```

## 3. 重构原则

1. **统一加密处理**：移除各处理代码中单独实现的加密和构建过程，统一通过 ISessionService.SendPacketCoreAsync 处理
2. **保留原有功能**：确保重构后的代码保留原有的功能逻辑和参数传递（如 cancellationToken 等）
3. **遵循项目规范**：验证所有修改符合项目的加密解密处理规范，确保网络通信安全
4. **使用标准 CommandId**：根据不同的业务场景使用合适的 CommandId（CacheSync 或其他）
5. **简化代码结构**：通过统一调用减少重复代码，提高可维护性

## 4. 重构步骤

1. 修改第一处代码（行号 419），替换为使用 ISessionService.SendPacketCoreAsync
2. 修改第二处代码（行号 894），替换为使用 ISessionService.SendPacketCoreAsync
3. 修改第三处代码（行号 1053），替换为使用 ISessionService.SendPacketCoreAsync
4. 移除各修改点周围单独实现的加密和构建过程
5. 确保所有修改符合项目规范和安全要求

## 5. 预期效果

* 统一的网络发送处理流程

* 一致的加密解密规范

* 简化的代码结构

* 提高代码可维护性和安全性

* 确保所有网络通信遵循统一标准

## 6. 验证要点

* 重构后的代码能够正确编译

* 保留原有的功能逻辑

* 网络通信安全可靠

* 符合项目的加密解密规范

* 没有引入新的bug或性能问题

