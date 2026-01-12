# ERP系统网络连接问题修复计划

## 问题分析
经过对代码的全面分析，发现以下几个关键问题导致网络连接出现异常：

1. **客户端未发送WelcomeAck响应**：客户端收到服务器Welcome消息后，未发送WelcomeAck确认，导致服务器无法将会话标记为已验证和已连接状态
2. **锁定状态下心跳处理不当**：当主窗体锁定时，心跳返回失败，导致连接被错误判定为断开
3. **连接状态同步问题**：部分组件在连接断开时未正确更新状态
4. **会话清理机制不明确**：需要确保过期会话被及时清理

## 修复方案

### 1. 修复客户端WelcomeAck未发送问题
**文件**：`RUINORERP.UI\Network\ClientCommandHandlers\WelcomeCommandHandler.cs`
**问题**：第99-102行发送WelcomeAck的代码被注释
**修复**：取消注释发送WelcomeAck的代码，确保服务器收到确认

### 2. 修复锁定状态下的心跳处理
**文件**：`RUINORERP.UI\Network\ClientCommunicationService.cs`
**问题**：第812行当主窗体锁定时，心跳返回false，导致失败计数增加
**修复**：修改`SendHeartbeatAsync`方法，在锁定状态下跳过心跳发送，不增加失败计数

### 3. 优化心跳机制
**文件**：`RUINORERP.UI\Network\ClientCommunicationService.cs`
**问题**：心跳失败处理逻辑不够健壮
**修复**：
- 改进`UpdateHeartbeatState`方法，区分真正的连接失败和锁定状态
- 确保心跳取消令牌正确管理

### 4. 增强连接状态同步
**文件**：`RUINORERP.UI\Network\ConnectionManager.cs`
**问题**：部分组件状态同步不及时
**修复**：确保所有组件在连接状态变化时正确更新

### 5. 完善会话清理机制
**文件**：`RUINORERP.Server\Network\Services\SessionService.cs`
**问题**：需要确保过期会话被及时清理
**修复**：检查并完善`CleanupAndHeartbeatCallback`方法

## 修复步骤
1. 首先修复WelcomeAck未发送问题（最关键）
2. 然后修复锁定状态下的心跳处理
3. 优化心跳机制和连接状态同步
4. 完善会话清理机制
5. 测试连接稳定性

## 预期效果
- 客户端与服务器能够正常建立和维持连接
- 连接断开后能够自动重连
- 心跳机制正常工作，不会因锁定状态误判连接断开
- 会话管理更加稳定

## 注意事项
- 保持现有架构和功能不变
- 避免过度设计和复杂的日志记录
- 确保修复后的代码简洁可靠
- 优先修复最关键的连接问题