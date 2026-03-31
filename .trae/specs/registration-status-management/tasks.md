# Tasks

- [x] Task 1: 扩展数据模型
  - [x] SubTask 1.1: 在RUINORERP.PacketSpec.Models.Authentication中扩展LoginResponse，添加RegistrationStatus属性
  - [x] SubTask 1.2: 在LoginResponse中添加ExpirationReminder属性
  - [x] SubTask 1.3: 创建RegistrationStatus枚举（正常、即将到期、已过期）
  - [x] SubTask 1.4: 创建ExpirationReminder数据模型

- [x] Task 2: 扩展IRegistrationService接口
  - [x] SubTask 2.1: 添加GetRegistrationStatusAsync方法（从内存获取）
  - [x] SubTask 2.2: 添加CheckExpirationReminderAsync方法（从内存判断）
  - [x] SubTask 2.3: 添加GetExpirationReminderInfoAsync方法（从内存获取）
  - [x] SubTask 2.4: 添加UpdateRegistrationInfoCacheAsync方法（从数据库重新加载到内存）

- [x] Task 3: 扩展RegistrationService实现
  - [x] SubTask 3.1: 添加内存缓存字段和锁对象
  - [x] SubTask 3.2: 实现启动时加载注册信息到内存的方法
  - [x] SubTask 3.3: 实现GetRegistrationStatusAsync方法（从内存获取）
  - [x] SubTask 3.4: 实现CheckExpirationReminderAsync方法（从内存判断）
  - [x] SubTask 3.5: 实现GetExpirationReminderInfoAsync方法（从内存获取）
  - [x] SubTask 3.6: 实现UpdateRegistrationInfoCacheAsync方法（从数据库重新加载到内存）
  - [x] SubTask 3.7: 优化IsRegistrationExpired方法，支持即将到期判断

- [x] Task 4: 扩展LoginCommandHandler
  - [x] SubTask 4.1: 在ProcessLoginAsync方法中注入IRegistrationService
  - [x] SubTask 4.2: 在验证用户凭据后添加注册状态检查逻辑（使用内存中的注册信息）
  - [x] SubTask 4.3: 实现正常用户登录逻辑（允许登录）
  - [x] SubTask 4.4: 实现即将到期用户登录逻辑（允许登录+提醒）
  - [x] SubTask 4.5: 实现过期用户登录拒绝逻辑（拒绝登录+续费引导）

- [x] Task 5: 扩展ServerMessageService
  - [x] SubTask 5.1: 添加SendExpirationReminderAsync方法
  - [x] SubTask 5.2: 实现向指定用户发送到期提醒
  - [x] SubTask 5.3: 实现批量发送到期提醒

- [x] Task 6: 创建注册到期提醒工作流
  - [x] SubTask 6.1: 创建RegistrationExpirationReminderWorkflow类
  - [x] SubTask 6.2: 实现检查即将到期注册信息的逻辑（使用内存中的注册信息）
  - [x] SubTask 6.3: 实现批量发送提醒消息的逻辑

- [x] Task 7: 创建注册信息更新工作流
  - [x] SubTask 7.1: 创建RegistrationInfoUpdateWorkflow类
  - [x] SubTask 7.2: 实现从数据库重新加载注册信息到内存的逻辑
  - [x] SubTask 7.3: 实现更新内存中注册状态和到期信息的逻辑

- [x] Task 8: 扩展ReminderWorkflowScheduler
  - [x] SubTask 8.1: 在ReminderWorkflowScheduler中注册RegistrationExpirationReminderWorkflow（每日上午10点）
  - [x] SubTask 8.2: 在ReminderWorkflowScheduler中注册RegistrationInfoUpdateWorkflow（每日晚上指定时间）
  - [x] SubTask 8.3: 实现工作流启动和错误处理

- [x] Task 9: 实现可配置的提醒参数
  - [x] SubTask 9.1: 创建RegistrationReminderConfig配置类
  - [x] SubTask 9.2: 实现配置读取和保存逻辑
  - [x] SubTask 9.3: 实现配置更新通知机制
  - [x] SubTask 9.4: 集成到appsettings.json

- [x] Task 10: 实现客户端注册状态处理
  - [x] SubTask 10.1: 在RUINORERP.UI.Network中扩展登录响应处理
  - [x] SubTask 10.2: 实现到期提醒显示逻辑
  - [x] SubTask 10.3: 实现过期提示界面
  - [x] SubTask 10.4: 实现续费引导流程

- [x] Task 11: 注册依赖注入服务
  - [x] SubTask 11.1: 在NetworkServicesDependencyInjection中注册扩展的服务
  - [x] SubTask 11.2: 注册RegistrationExpirationReminderWorkflow工作流
  - [x] SubTask 11.3: 注册RegistrationInfoUpdateWorkflow工作流
  - [x] SubTask 11.4: 注册配置管理服务

- [x] Task 12: 单元测试
  - [x] SubTask 12.1: 编写RegistrationService内存缓存方法单元测试
  - [x] SubTask 12.2: 编写LoginCommandHandler注册状态检查集成测试
  - [x] SubTask 12.3: 编写ServerMessageService到期提醒测试
  - [x] SubTask 12.4: 编写RegistrationExpirationReminderWorkflow测试
  - [x] SubTask 12.5: 编写RegistrationInfoUpdateWorkflow测试

# Task Dependencies
- [Task 2] depends on [Task 1]
- [Task 3] depends on [Task 1, Task 2]
- [Task 4] depends on [Task 1, Task 3]
- [Task 5] depends on [Task 1]
- [Task 6] depends on [Task 1, Task 3, Task 5]
- [Task 7] depends on [Task 1, Task 3]
- [Task 8] depends on [Task 6, Task 7]
- [Task 9] depends on [Task 1]
- [Task 10] depends on [Task 1]
- [Task 11] depends on [Task 3, Task 5, Task 6, Task 7, Task 9]
- [Task 12] depends on [Task 3, Task 4, Task 5, Task 6, Task 7]
