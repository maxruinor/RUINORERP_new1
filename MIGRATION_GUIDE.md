# RUINORERP 数据包模型迁移指南

## 概述

本文档指导如何从旧的数据包模型体系迁移到新的统一数据包模型体系。新体系提供了更好的扩展性、性能和开发体验。

## 主要变化

### 1. 模型结构重构
- **旧体系**: 多个重复的包信息类 (`PacketInfo`, `UnifiedPacketInfo`)
- **新体系**: 单一的统一数据包模型 (`PacketModel`)

### 2. 响应模型统一
- **旧体系**: 多个重复的响应类 (`BaseResponse`, `HeartbeatResponse` 等)
- **新体系**: 统一的 API 响应模型 (`ApiResponse<T>`)

### 3. 构建方式改进
- **旧体系**: 手动设置属性
- **新体系**: 流畅的构建器模式 (`PacketBuilder`)

### 4. 扩展性增强
- **旧体系**: 硬编码的属性
- **新体系**: 动态扩展属性 (`Extensions` 字典)

## 迁移步骤

### 步骤 1: 替换包信息类

**旧代码**:
```csharp
var packetInfo = new PacketInfo
{
    Key = "login_request",
    Body = "username=admin&password=123456",
    Command = "LOGIN",
    CreatedAt = DateTime.Now
};
```

**新代码**:
```csharp
var packet = PacketBuilder.Create()
    .WithCommand(PacketCommand.Login)
    .WithTextData("username=admin&password=123456")
    .Build();
```

### 步骤 2: 替换响应模型

**旧代码**:
```csharp
var response = new BaseResponse<string>
{
    Success = true,
    Message = "操作成功",
    Data = "result data"
};
```

**新代码**:
```csharp
var response = ApiResponse<string>.CreateSuccess("result data", "操作成功");
```

### 步骤 3: 处理文件操作

**旧代码**:
```csharp
var fileRequest = new FileDownloadRequest
{
    FileId = "file_123",
    Category = "documents"
};
```

**新代码**:
```csharp
var fileRequest = PacketBuilder.Create()
    .AsFileDownloadRequest("file_123", "documents")
    .Build();
```

### 步骤 4: 处理心跳请求

**旧代码**:
```csharp
var heartbeat = new HeartbeatRequest
{
    SessionToken = "session_123",
    UserId = "user_456"
};
```

**新代码**:
```csharp
var heartbeat = PacketBuilder.Create()
    .AsHeartbeatRequest("session_123", "user_456")
    .Build();
```

## 常用迁移示例

### 登录请求迁移

**旧代码**:
```csharp
var loginRequest = new PacketInfo
{
    Key = "login",
    Body = JsonConvert.SerializeObject(new 
    { 
        Username = "admin", 
        Password = "123456" 
    }),
    Command = "LOGIN",
    Priority = 1
};
```

**新代码**:
```csharp
var loginRequest = PacketBuilder.Create()
    .AsLoginRequest("admin", "123456", "1.0.0")
    .WithPriority(PacketPriority.High)
    .Build();
```

### 查询请求迁移

**旧代码**:
```csharp
var queryRequest = new PacketInfo
{
    Key = "query_users",
    Body = "SELECT * FROM Users WHERE Status = 1",
    Command = "QUERY",
    Direction = "C2S"
};
```

**新代码**:
```csharp
var queryRequest = PacketBuilder.Create()
    .WithCommand(PacketCommand.Query)
    .WithTextData("SELECT * FROM Users WHERE Status = 1")
    .WithDirection(PacketDirection.ClientToServer)
    .Build();
```

### 响应处理迁移

**旧代码**:
```csharp
var response = new BaseResponse<List<User>>
{
    Success = true,
    Message = "查询成功",
    Data = usersList,
    Code = 200
};
```

**新代码**:
```csharp
var response = ApiResponse<List<User>>.CreateSuccess(usersList, "查询成功");
```

## 性能优化建议

### 1. 使用数据切片
```csharp
// 处理大数据时避免复制
var largePacket = new PacketModel().WithDataDirect(largeData);
var slice = largePacket.GetDataSlice(0, 1024); // 只处理前1KB
```

### 2. 使用哈希比较
```csharp
// 快速比较数据包内容
var hash1 = packet1.ComputeHash();
var hash2 = packet2.ComputeHash();
if (hash1 == hash2)
{
    // 内容相同
}
```

### 3. 批量操作
```csharp
// 使用构建器批量创建
var packets = Enumerable.Range(0, 100)
    .Select(i => PacketBuilder.Create()
        .WithCommand(PacketCommand.Heartbeat)
        .WithRequestId($"req_{i}