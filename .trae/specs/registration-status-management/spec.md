# 注册状态管理与到期提醒功能 Spec

## Why
当前系统已有基础的注册信息管理功能（tb_sys_RegistrationInfo、IRegistrationService、RegistrationService），但缺乏用户登录时的注册状态验证、到期提醒和过期限制功能。需要在现有代码基础上完善这些功能，提升系统的商业价值和用户体验。

## What Changes
基于现有代码进行完善：
- 扩展 `LoginCommandHandler` 登录流程，集成注册状态验证
- 扩展 `LoginResponse` 模型，添加注册状态和到期提醒信息
- 扩展 `ServerMessageService`，支持注册到期提醒消息发送
- 创建注册到期提醒工作流，集成到 `ReminderWorkflowScheduler`
- 实现注册信息内存缓存机制，启动时加载，每日晚上更新
- 实现可配置的提醒参数

## Impact
- Affected specs: 认证系统、消息系统、定时任务系统
- Affected code:
  - RUINORERP.PacketSpec.Models.Authentication（扩展LoginResponse）
  - RUINORERP.Server.Network.CommandHandlers（扩展LoginCommandHandler）
  - RUINORERP.Server.Network.Services（扩展ServerMessageService）
  - RUINORERP.Server.Workflow（新增注册到期提醒工作流）
  - RUINORERP.Server.Services（扩展RegistrationService，添加内存缓存）

## ADDED Requirements

### Requirement: 注册状态数据扩展
基于现有 `tb_sys_RegistrationInfo` 模型，系统 SHALL 支持注册状态判断和到期提醒。

#### Scenario: 判断注册状态
- **WHEN** 系统需要判断注册状态
- **THEN** 系统根据 `ExpirationDate` 和 `IsRegistered` 字段判断状态（正常、即将到期、已过期）

### Requirement: 注册信息内存缓存
系统 SHALL 在启动时加载注册信息到内存，每日晚上更新。

#### Scenario: 启动时加载注册信息
- **WHEN** 服务器启动
- **THEN** 系统从数据库加载注册信息到内存
- **AND** 后续使用内存中的注册信息，不查询数据库

#### Scenario: 每日晚上更新注册信息
- **WHEN** 每日晚上指定时间到达
- **THEN** 系统从数据库重新加载注册信息到内存
- **AND** 更新内存中的注册状态和到期信息

### Requirement: 登录流程注册状态验证
系统 SHALL 在用户登录时验证注册状态。

#### Scenario: 正常用户登录
- **WHEN** 注册状态正常的用户登录
- **THEN** 系统允许登录，返回登录成功响应

#### Scenario: 即将到期用户登录
- **WHEN** 距离到期30天内的用户登录
- **THEN** 系统允许登录，并在 `LoginResponse` 中包含到期提醒信息

#### Scenario: 已过期用户登录
- **WHEN** 注册已过期的用户登录
- **THEN** 系统拒绝登录，返回过期错误响应，包含续费引导信息

### Requirement: 到期提醒机制
系统 SHALL 提供双重到期提醒策略。

#### Scenario: 登录触发提醒
- **WHEN** 用户登录且距离到期30天内
- **THEN** 系统在 `LoginResponse` 中包含到期提醒信息
- **AND** 提醒信息包含剩余天数和续费方式

#### Scenario: 定时广播提醒
- **WHEN** 每日上午10点到达
- **THEN** 系统自动向所有即将到期用户（30天内）发送广播提醒
- **AND** 使用内存中的注册信息判断，不查询数据库

### Requirement: 过期限制措施
系统 SHALL 对过期用户实施登录限制。

#### Scenario: 过期用户尝试登录
- **WHEN** 注册已过期的用户尝试登录
- **THEN** 系统拒绝登录请求
- **AND** 返回友好的过期提示信息
- **AND** 提供续费流程引导

### Requirement: 可配置的提醒参数
系统 SHALL 支持配置提醒相关参数。

#### Scenario: 配置提醒提前天数
- **WHEN** 管理员修改提醒提前天数
- **THEN** 系统使用新的提前天数进行提醒判断

#### Scenario: 配置广播时间
- **WHEN** 管理员修改广播提醒时间
- **THEN** 系统在新的时间点执行广播提醒

#### Scenario: 配置注册信息更新时间
- **WHEN** 管理员修改注册信息更新时间
- **THEN** 系统在新的时间点更新内存中的注册信息

## MODIFIED Requirements

### Requirement: 扩展LoginResponse模型
现有的 `LoginResponse` SHALL 扩展以支持注册状态和到期提醒信息。

#### Scenario: 添加注册状态属性
- **WHEN** 登录成功
- **THEN** `LoginResponse` 包含 `RegistrationStatus` 属性，表示注册状态
- **AND** `LoginResponse` 包含 `ExpirationReminder` 属性，包含到期提醒信息（如即将到期）

### Requirement: 扩展LoginCommandHandler
现有的 `LoginCommandHandler` SHALL 扩展以支持注册状态检查。

#### Scenario: 登录流程集成注册状态检查
- **WHEN** 用户发起登录请求
- **THEN** 系统在验证用户凭据后检查注册状态
- **AND** 根据注册状态返回相应的响应（成功、提醒或拒绝）
- **AND** 使用内存中的注册信息，不查询数据库

### Requirement: 扩展ServerMessageService
现有的 `ServerMessageService` SHALL 支持注册到期通知消息。

#### Scenario: 发送注册到期消息
- **WHEN** 系统需要发送注册到期提醒
- **THEN** 使用 `ServerMessageService` 发送注册到期通知
- **AND** 客户端能够正确处理和显示该类型消息

### Requirement: 扩展ReminderWorkflowScheduler
现有的 `ReminderWorkflowScheduler` SHALL 支持注册到期提醒工作流和注册信息更新工作流。

#### Scenario: 添加注册到期提醒工作流
- **WHEN** 定时任务触发
- **THEN** 系统检查即将到期的注册信息
- **AND** 向相关用户发送到期提醒
- **AND** 使用内存中的注册信息，不查询数据库

#### Scenario: 添加注册信息更新工作流
- **WHEN** 每日晚上指定时间到达
- **THEN** 系统从数据库重新加载注册信息到内存
- **AND** 更新内存中的注册状态和到期信息

## REMOVED Requirements
无

## Technical Implementation Notes

### 架构设计
1. **数据模型层**：基于现有 `tb_sys_RegistrationInfo`，不修改数据库表结构
2. **命令层**：扩展 `LoginResponse` 模型，添加注册状态和提醒信息
3. **服务层**：
   - 扩展 `IRegistrationService` 接口，添加内存缓存相关方法
   - 扩展 `RegistrationService` 实现，添加内存缓存和到期检查逻辑
   - 扩展 `ServerMessageService`，支持注册到期提醒消息
4. **命令处理层**：扩展 `LoginCommandHandler`，集成注册状态检查
5. **定时任务层**：
   - 创建 `RegistrationExpirationReminderWorkflow`，集成到 `ReminderWorkflowScheduler`
   - 创建 `RegistrationInfoUpdateWorkflow`，每日晚上更新内存中的注册信息
6. **客户端层**：在 `RUINORERP.UI.Network` 中扩展登录响应处理逻辑

### 技术要点
1. **内存缓存**：启动时从数据库加载注册信息到内存，后续使用内存中的数据
2. **定时更新**：每日晚上指定时间从数据库重新加载注册信息到内存
3. **不修改数据库**：所有提醒相关逻辑基于内存中的注册信息，不保存到数据库
4. **使用现有的 SqlSugar ORM** 进行数据库操作（仅在启动和更新时）
5. **使用现有的 Autofac** 进行依赖注入
6. **使用现有的 WorkflowCore** 实现定时任务
7. **扩展现有的消息系统** 支持注册到期通知

### 性能考虑
1. 注册信息缓存到内存，避免每次登录都查询数据库
2. 定时任务使用批量处理，避免对服务器造成过大负载
3. 提醒发送使用异步方式，不阻塞主流程

### 安全考虑
1. 注册状态验证在服务器端进行，防止客户端绕过
2. 过期用户的 Token 立即失效
3. 续费操作需要管理员权限验证

### 与现有代码的集成
1. **LoginCommandHandler**：在 `ProcessLoginAsync` 方法中，验证用户凭据后，添加注册状态检查逻辑，使用内存中的注册信息
2. **RegistrationService**：
   - 添加内存缓存字段和方法
   - 扩展 `IsRegistrationExpired` 方法，支持即将到期判断
   - 添加 `GetRegistrationStatusAsync` 方法，从内存获取注册状态
   - 添加 `UpdateRegistrationInfoCacheAsync` 方法，从数据库重新加载注册信息到内存
3. **ServerMessageService**：添加 `SendExpirationReminderAsync` 方法，发送到期提醒消息
4. **ReminderWorkflowScheduler**：
   - 注册 `RegistrationExpirationReminderWorkflow` 工作流（每日上午10点）
   - 注册 `RegistrationInfoUpdateWorkflow` 工作流（每日晚上指定时间）
5. **LoginResponse**：添加 `RegistrationStatus` 和 `ExpirationReminder` 属性

### 数据库表设计
不修改数据库表结构，所有功能基于现有 `tb_sys_RegistrationInfo` 表。

### 内存缓存设计
```csharp
// 在RegistrationService中添加内存缓存
private tb_sys_RegistrationInfo _registrationInfoCache;
private DateTime _lastUpdateTime;
private readonly object _cacheLock = new object();
```

### 定时任务设计
1. **RegistrationExpirationReminderWorkflow**：每日上午10点触发，检查即将到期的注册信息，发送提醒
2. **RegistrationInfoUpdateWorkflow**：每日晚上指定时间触发，从数据库重新加载注册信息到内存
