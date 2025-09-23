# 客户端网络架构设计

## 1. 概述

本文档描述了RUINORERP.UI客户端网络通信模块的架构设计。该架构旨在提供一个轻量级、高效且易于维护的客户端通信解决方案，不依赖于公共项目中的复杂调度器。

## 2. 设计原则

- **轻量级**: 避免使用公共项目中的重型调度器，减少依赖和复杂性
- **高性能**: 基于SuperSocket.ClientEngine实现高效的Socket通信
- **易用性**: 提供简洁的API接口，方便业务层调用
- **可扩展**: 支持灵活的命令扩展机制
- **安全性**: 集成数据加密和解密机制
- **稳定性**: 具备完善的错误处理和重连机制

## 3. 核心组件

### 3.1 ISocketClient接口
定义了Socket客户端的基本功能：
- 连接和断开服务器
- 发送数据
- 接收数据事件
- 连接状态管理

### 3.2 SuperSocketClient实现
基于SuperSocket.ClientEngine的具体实现：
- 使用EasyClient<BizPackageInfo>进行通信
- 实现BizPipelineFilter进行数据包解析
- 处理连接、断开、错误等事件

### 3.3 ClientCommunicationService
客户端通信服务的核心实现：
- 协调Socket客户端和命令系统
- 提供统一的命令发送和接收接口
- 处理数据加密和解密
- 管理请求响应机制

### 3.4 ClientCommandDispatcher
客户端专用的命令调度器：
- 负责命令类型的注册和实例创建
- 管理命令实例的生命周期
- 自动扫描和注册客户端命令

### 3.5 ClientCommandFactory
客户端命令工厂：
- 专门用于创建客户端命令实例
- 与ClientCommandDispatcher配合使用
- 提供从数据包创建命令的功能

### 3.6 RequestResponseManager
请求响应管理器：
- 管理客户端的请求和对应的响应
- 支持超时控制和异步等待
- 处理服务器推送的命令

### 3.7 ClientEventManager
客户端事件管理器：
- 统一管理各种客户端事件
- 提供事件订阅和触发机制

## 4. 数据流设计

### 4.1 发送数据流程
1. 业务层调用ClientCommunicationService的SendCommandAsync方法
2. ClientCommunicationService构建PacketModel数据包
3. 使用UnifiedSerializationService序列化数据包
4. 通过EncryptedProtocol加密数据
5. 通过SuperSocketClient发送加密数据到服务器

### 4.2 接收数据流程
1. SuperSocketClient接收到服务器数据
2. BizPipelineFilter解析数据包
3. 触发Received事件到ClientCommunicationService
4. ClientCommunicationService解密数据
5. RequestResponseManager处理响应数据或触发命令接收事件

## 5. 命令系统设计

### 5.1 命令基类
所有客户端命令都继承自PacketSpec.Commands.BaseCommand，确保统一的接口和行为。

### 5.2 命令注册
ClientCommandDispatcher自动扫描RUINORERP.UI.Network.Commands命名空间下的所有命令类，并根据Command特性或CommandIdentifier进行注册。

### 5.3 命令创建
通过ClientCommandFactory可以从PacketModel或OriginalData创建对应的命令实例。

## 6. 安全设计

### 6.1 数据加密
集成PacketSpec.Security.EncryptedProtocol进行数据加密和解密，确保传输安全。

### 6.2 身份验证
通过LoginCommand实现用户身份验证，建立安全会话。

## 7. 错误处理

### 7.1 网络异常
SuperSocketClient处理底层网络异常，并通过事件通知上层。

### 7.2 命令执行异常
ClientCommunicationService捕获命令执行过程中的异常，并通过ErrorOccurred事件通知。

### 7.3 超时处理
RequestResponseManager提供请求超时控制机制。

## 8. 性能优化

### 8.1 连接复用
使用单例模式管理客户端连接，避免频繁创建和销毁。

### 8.2 异步处理
所有网络操作均采用异步模式，避免阻塞UI线程。

### 8.3 内存管理
ClientCommandDispatcher定期清理过期的命令实例，减少内存占用。

## 9. 扩展性设计

### 9.1 命令扩展
通过继承BaseCommand可以轻松添加新的命令类型。

### 9.2 事件扩展
ClientEventManager提供可扩展的事件机制。

### 9.3 配置扩展
支持通过构造函数参数配置各种行为。

## 10. 使用示例

### 10.1 基本使用
```csharp
// 创建客户端组件
var socketClient = new SuperSocketClient();
var commandDispatcher = new ClientCommandDispatcher();
var communicationService = new ClientCommunicationService(socketClient, commandDispatcher);

// 连接服务器
await communicationService.ConnectAsync("127.0.0.1", 8080);

// 发送登录命令
var loginCommand = new LoginCommand("username", "password");
var response = await communicationService.SendCommandAsync<LoginResult>(loginCommand);
```

### 10.2 使用心跳管理器
```csharp
var heartbeatManager = new HeartbeatManager(communicationService);
heartbeatManager.Start();
```