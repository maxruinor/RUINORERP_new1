# 客户端网络架构设计文档

## 1. 概述

本文档描述了RUINORERP.UI客户端网络通信模块的架构设计。该架构旨在提供一个轻量级、高效且易于维护的客户端通信解决方案，不依赖于公共项目中的复杂调度器。

## 2. 设计目标

- **轻量级**: 避免使用公共项目中的重型调度器，减少依赖和复杂性
- **高性能**: 基于SuperSocket.ClientEngine实现高效的Socket通信
- **易用性**: 提供简洁的API接口，方便业务层调用
- **可扩展**: 支持灵活的命令扩展机制
- **安全性**: 集成数据加密和解密机制
- **稳定性**: 具备完善的错误处理和重连机制

## 3. 架构组件

### 3.1 核心组件

#### ISocketClient
- 定义Socket客户端的基本接口
- 提供连接、断开、发送数据等基本功能
- 支持事件驱动的接收机制

#### SuperSocketClient
- ISocketClient的具体实现
- 基于SuperSocket.ClientEngine构建
- 处理底层网络通信细节

#### ClientCommunicationService
- 客户端通信服务的核心实现
- 协调各个组件的工作
- 提供统一的命令发送和接收接口

#### ClientCommandDispatcher
- 客户端专用的命令调度器
- 负责命令类型的注册和实例创建
- 管理命令实例的生命周期

#### ClientCommandFactory
- 客户端命令工厂
- 专门用于创建客户端命令实例
- 与ClientCommandDispatcher配合使用

#### RequestResponseManager
- 请求响应管理器
- 管理客户端的请求和对应的响应
- 支持超时控制和异步等待

#### ClientEventManager
- 客户端事件管理器
- 统一管理各种客户端事件
- 提供事件订阅和触发机制

### 3.2 数据处理组件

#### BizPackageInfo
- 业务数据包信息类
- 封装接收到的原始数据包

#### BizPipelineFilter
- 业务管道过滤器
- 处理数据包的解析和过滤

### 3.3 命令组件

#### 基础命令类
- LoginCommand: 登录命令
- HeartbeatCommand: 心跳命令
- GetUserDataCommand: 获取用户数据命令

## 4. 工作流程

### 4.1 连接流程
1. 创建SuperSocketClient实例
2. 创建ClientCommandDispatcher实例
3. 创建ClientCommunicationService实例
4. 调用ConnectAsync方法连接服务器

### 4.2 命令发送流程
1. 业务层创建命令实例或调用SendCommandAsync方法
2. ClientCommunicationService构建数据包
3. 数据包通过UnifiedSerializationService序列化
4. 数据包通过EncryptedProtocol加密
5. 加密数据通过SuperSocketClient发送到服务器

### 4.3 命令接收流程
1. SuperSocketClient接收到服务器数据
2. BizPipelineFilter解析数据包
3. ClientCommunicationService解密数据
4. RequestResponseManager处理响应数据或触发事件

## 5. 特性说明

### 5.1 命令系统
- 基于PacketSpec.Commands的ICommand接口
- 支持命令的自动注册和创建
- 提供命令验证和执行机制

### 5.2 加密机制
- 集成PacketSpec.Security.EncryptedProtocol
- 支持客户端到服务器和服务器到客户端的数据加密

### 5.3 序列化机制
- 使用UnifiedSerializationService进行数据序列化
- 支持JSON和MessagePack两种格式

### 5.4 心跳机制
- HeartbeatManager负责定期发送心跳
- 支持自动重连机制

### 5.5 事件系统
- 提供丰富的事件通知机制
- 支持连接状态变化、命令接收、错误处理等事件

## 6. 使用示例

### 6.1 基本使用
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

### 6.2 使用心跳管理器
```csharp
var heartbeatManager = new HeartbeatManager(communicationService);
heartbeatManager.Start();
```

## 7. 扩展机制

### 7.1 添加新命令
1. 创建继承自BaseCommand的新命令类
2. 添加Command特性标记
3. 实现具体的命令逻辑

### 7.2 自定义数据处理
1. 继承BizPipelineFilter类
2. 重写数据包解析逻辑
3. 注册自定义过滤器

## 8. 性能优化

### 8.1 连接复用
- 使用单例模式管理客户端连接
- 避免频繁创建和销毁连接

### 8.2 内存管理
- 及时清理过期的命令实例
- 使用对象池减少GC压力

### 8.3 异步处理
- 所有网络操作均采用异步模式
- 避免阻塞UI线程

## 9. 错误处理

### 9.1 网络异常
- 自动重连机制
- 连接状态监控

### 9.2 命令执行异常
- 详细的错误信息记录
- 支持自定义异常处理

### 9.3 超时处理
- 请求超时控制
- 取消令牌支持

## 10. 安全性

### 10.1 数据加密
- 集成成熟的加密协议
- 支持端到端加密

### 10.2 身份验证
- 支持多种认证方式
- 会话管理机制

## 11. 测试策略

### 11.1 单元测试
- 对核心组件进行单元测试
- 模拟网络环境进行测试

### 11.2 集成测试
- 测试完整的通信流程
- 验证与服务器的兼容性

## 12. 部署和维护

### 12.1 配置管理
- 支持灵活的配置选项
- 环境相关的配置管理

### 12.2 监控和日志
- 集成日志记录系统
- 性能监控指标

## 13. 未来扩展

### 13.1 协议升级
- 支持协议版本管理
- 向后兼容性保证

### 13.2 功能扩展
- 支持更多的命令类型
- 插件化架构设计