# ERP 服务器连接顶级服务器说明

## 概述

ERP 服务器（RUINORERP.Server）通过 `TopServerClientService` 服务连接到顶级服务器（RUINORERP.TopServer），实现服务器注册、心跳维持、状态上报和用户信息同步等功能。

## 架构层次

```
┌─────────────────┐
│  RUINORERP.UI  │  (客户端)
└────────┬────────┘
         │ TCP连接
┌────────▼────────┐
│ RUINORERP.Server│  (业务服务器)
│                 │
│ TopServerClient │  ← 连接顶级服务器的客户端服务
│    Service      │
└────────┬────────┘
         │ TCP连接 + 注册协议
┌────────▼─────────────────┐
│ RUINORERP.TopServer     │  (顶级管理服务器)
│                        │
│ ServerManager           │  ← 管理所有业务服务器实例
│ UserManager            │  ← 管理所有用户状态
└────────────────────────┘
```

## 连接流程

### 1. 连接阶段

```csharp
// 创建 TopServer 配置
var topServerConfig = new TopServerConfig
{
    IpAddress = "192.168.0.254",  // 顶级服务器IP
    Port = 8090,                     // 顶级服务器端口
    AuthToken = "your-auth-token",     // 认证令牌
    ConnectTimeoutSeconds = 30          // 连接超时时间
};

// 连接到 TopServer
var connected = await topServerClientService.ConnectAsync(topServerConfig);
if (!connected)
{
    // 连接失败处理
    return;
}
```

**TopServerClientService.ConnectAsync() 内部实现**：
1. 通过 `ISessionService` 创建到 TopServer 的 TCP 连接
2. 获取会话 ID（SessionId）
3. 设置 `_isConnected = true`
4. 返回连接结果

### 2. 注册阶段

```csharp
// 准备服务器注册信息
var registrationInfo = new ServerRegistrationInfo
{
    ServerId = "ERP-SERVER-001",              // 服务器唯一ID
    ServerName = "深圳分公司ERP服务器",         // 服务器名称
    ServerType = "BusinessServer",              // 服务器类型
    IpAddress = "192.168.0.100",              // 本服务器IP
    Port = 3099,                              // 本服务器端口
    Version = "1.0.0",                        // 服务器版本
    Capabilities = new ServerCapabilities
    {
        SupportsMultiTenant = true,
        SupportsWorkflow = true,
        SupportsCache = true,
        SupportsRealtime = true
    },
    AuthToken = "your-auth-token"               // 认证令牌
};

// 注册到 TopServer
var registered = await topServerClientService.RegisterAsync(registrationInfo);
if (!registered)
{
    // 注册失败处理
    return;
}

// 获取分配的实例ID
var instanceId = topServerClientService.InstanceId;
```

**发送的注册请求**：
```json
{
  "CommandId": "RegisterServer",
  "Request": {
    "ServerId": "ERP-SERVER-001",
    "ServerName": "深圳分公司ERP服务器",
    "ServerType": "BusinessServer",
    "IpAddress": "192.168.0.100",
    "Port": 3099,
    "Version": "1.0.0",
    "Capabilities": {
      "SupportsMultiTenant": true,
      "SupportsWorkflow": true,
      "SupportsCache": true,
      "SupportsRealtime": true
    },
    "AuthToken": "your-auth-token"
  }
}
```

**TopServer 返回的注册响应**：
```json
{
  "CommandId": "RegisterServer",
  "Response": {
    "IsSuccess": true,
    "RegistrationSuccessful": true,
    "AssignedInstanceId": "guid-instance-id-123",
    "Message": "注册成功"
  }
}
```

### 3. 启动自动管理

```csharp
// 启动自动管理（心跳、状态上报、用户上报）
topServerClientService.StartAutoManagement(
    heartbeatIntervalSeconds: 30,      // 每30秒发送心跳
    statusReportIntervalSeconds: 300     // 每5分钟上报状态
);
```

**自动管理功能**：
- **心跳定时器**：每 30 秒发送一次心跳包
- **状态上报定时器**：每 5 分钟上报一次服务器状态
- **用户上报定时器**：定期上报在线用户信息

## 通信命令

### 1. 心跳命令 (Heartbeat)

**目的**：保持连接活跃，证明服务器在线

**发送周期**：默认每 30 秒

**请求格式**：
```csharp
var request = new ServerHeartbeatRequest
{
    ServerInstanceId = instanceId,
    Metrics = new ServerMetrics
    {
        CpuUsage = 45.5,                    // CPU使用率
        MemoryUsage = 65.2,                 // 内存使用率
        DiskUsage = 55.8,                   // 磁盘使用率
        CurrentConnections = 150,              // 当前连接数
        AverageResponseTime = 120.5,          // 平均响应时间(ms)
        TotalRequests = 50000,               // 总请求数
        ErrorCount = 23,                     // 错误数
        UptimeSeconds = 86400                // 运行时间(秒)
    }
};
```

**响应格式**：
```json
{
  "HeartbeatConfirmed": true,
  "LastHeartbeatTime": "2026-03-21T10:30:00",
  "Message": "心跳确认"
}
```

### 2. 状态上报命令 (ReportStatus)

**目的**：向 TopServer 上报服务器详细状态

**发送周期**：默认每 5 分钟

**请求格式**：
```csharp
var request = new StatusReportRequest
{
    ServerInstanceId = instanceId,
    Status = ServerStatus.Running,
    Metrics = serverMetrics,
    OnlineUserCount = 120,
    ActiveConnectionCount = 150
};
```

**响应格式**：
```json
{
  "IsSuccess": true,
  "LastReportTime": "2026-03-21T10:30:00",
  "Message": "状态已更新"
}
```

### 3. 用户信息上报命令 (ReportUsers)

**目的**：向 TopServer 上报在线用户信息

**发送周期**：由业务逻辑触发或定时触发

**请求格式**：
```csharp
var onlineUsers = new List<UserInfo>
{
    new UserInfo
    {
        UserId = 1001,
        UserName = "张三",
        Status = UserStatus.Online,
        LoginTime = DateTime.Now.AddHours(-2),
        LastActivityTime = DateTime.Now.AddMinutes(-5),
        IpAddress = "192.168.0.50"
    },
    // ... 更多用户
};

var request = new UsersReportRequest
{
    ServerInstanceId = instanceId,
    OnlineUsers = onlineUsers,
    TotalUserCount = onlineUsers.Count,
    ReportTimeRange = TimeSpan.FromMinutes(5)
};
```

**响应格式**：
```json
{
  "IsSuccess": true,
  "ReceivedUserCount": 120,
  "Message": "用户信息已接收"
}
```

## 服务器指标

### ServerMetrics 类结构

```csharp
public class ServerMetrics
{
    public double CpuUsage { get; set; }              // CPU使用率 (0-100)
    public double MemoryUsage { get; set; }           // 内存使用率 (0-100)
    public double DiskUsage { get; set; }              // 磁盘使用率 (0-100)
    public int CurrentConnections { get; set; }         // 当前连接数
    public double AverageResponseTime { get; set; }    // 平均响应时间(ms)
    public long TotalRequests { get; set; }           // 总请求数
    public long ErrorCount { get; set; }              // 错误数
    public long UptimeSeconds { get; set; }           // 运行时间(秒)
}
```

## 服务器能力

### ServerCapabilities 类结构

```csharp
public class ServerCapabilities
{
    public bool SupportsMultiTenant { get; set; }      // 是否支持多租户
    public bool SupportsWorkflow { get; set; }         // 是否支持工作流
    public bool SupportsCache { get; set; }            // 是否支持缓存
    public bool SupportsRealtime { get; set; }        // 是否支持实时通信
    public bool SupportsFileStorage { get; set; }      // 是否支持文件存储
    public bool SupportsMessageQueue { get; set; }     // 是否支持消息队列
    public int MaxConnections { get; set; }           // 最大连接数
    public string[] SupportedCommands { get; set; }     // 支持的命令列表
}
```

## 断开连接

```csharp
// 断开与 TopServer 的连接
await topServerClientService.DisconnectAsync();
```

**断开连接时执行的操作**：
1. 停止所有定时器（心跳、状态上报、用户上报）
2. 关闭与 TopServer 的会话
3. 清理连接状态
4. 记录日志

## 错误处理

### 连接失败处理

```csharp
var connected = await topServerClientService.ConnectAsync(config);
if (!connected)
{
    _logger.LogError("连接TopServer失败，将在30秒后重试");
    // 延迟后重试
    await Task.Delay(30000);
    // 重新连接...
}
```

### 注册失败处理

```csharp
var registered = await topServerClientService.RegisterAsync(registrationInfo);
if (!registered)
{
    _logger.LogError("注册TopServer失败: {Reason}", response?.Message);
    // 检查认证令牌
    // 检查网络连接
    // 联系管理员
}
```

### 心跳超时处理

```csharp
var heartbeatSuccess = await topServerClientService.SendHeartbeatAsync();
if (!heartbeatSuccess)
{
    _logger.LogWarning("心跳发送失败，可能连接已断开");
    // 尝试重新连接
    await ReconnectAsync();
}
```

## 配置建议

### ERP 服务器端配置 (appsettings.json)

需要在 Server 项目的配置文件中添加 TopServer 连接配置：

```json
{
  "TopServerConfiguration": {
    "IpAddress": "192.168.0.254",
    "Port": 8090,
    "AuthToken": "your-auth-token-here",
    "ConnectTimeoutSeconds": 30,
    "AutoReconnect": true,
    "ReconnectIntervalSeconds": 30,
    "EnableAutoManagement": true,
    "HeartbeatIntervalSeconds": 30,
    "StatusReportIntervalSeconds": 300,
    "UserReportIntervalSeconds": 60
  }
}
```

### 顶级服务器端配置 (TopServer appsettings.json)

```json
{
  "ServerOptions": {
    "Port": 8090,
    "MaxConnectionCount": 500
  },
  "HeartbeatConfiguration": {
    "HeartbeatInterval": 30000,
    "HeartbeatTimeout": 90000,
    "CheckInterval": 10000,
    "MaxMissedHeartbeats": 3
  }
}
```

## 安全考虑

1. **认证令牌**：
   - 使用强随机字符串作为 AuthToken
   - 定期更换 AuthToken
   - 不要在代码中硬编码 AuthToken

2. **加密通信**：
   - 建议使用 SSL/TLS 加密通信
   - 验证服务器证书

3. **访问控制**：
   - TopServer 应该只接受来自授权服务器的连接
   - 实施IP白名单机制

## 监控和日志

### 关键监控指标

1. **连接状态**：
   - 连接成功率
   - 平均连接时间
   - 连接失败次数

2. **心跳状态**：
   - 心跳成功率
   - 心跳丢失次数
   - 平均心跳延迟

3. **注册状态**：
   - 注册成功率
   - 实例ID分配情况

### 日志记录

```csharp
_logger.LogInformation("连接到TopServer - IP: {IpAddress}, Port: {Port}", 
    config.IpAddress, config.Port);

_logger.LogInformation("成功注册到TopServer - InstanceId: {InstanceId}", 
    instanceId);

_logger.LogWarning("心跳发送失败 - SessionId: {SessionId}", 
    _topServerSessionId);

_logger.LogError(ex, "注册到TopServer时出错");
```

## 总结

ERP 服务器通过 `TopServerClientService` 服务实现与顶级服务器的连接和管理，主要包括：

1. **连接建立**：TCP 连接到 TopServer
2. **服务器注册**：发送注册信息，获取实例ID
3. **心跳维持**：定期发送心跳包保持连接
4. **状态上报**：定期上报服务器状态和指标
5. **用户同步**：上报在线用户信息
6. **自动管理**：通过定时器自动化执行以上任务
7. **错误恢复**：处理连接断开和重连

这种三层架构确保了系统的可扩展性和可管理性，顶级服务器可以统一监控和管理所有业务服务器实例。
