# 锁管理功能说明

## 概述

锁管理功能用于处理业务单据的并发访问控制，确保在同一时间只有一个用户可以修改特定的业务单据。当用户正在修改一个销售订单时，其他用户只能查看不能修改，按时间顺序锁定。

## 功能特性

1. **单据锁定** - 用户可以锁定业务单据进行编辑
2. **单据解锁** - 用户完成编辑后可以解锁单据
3. **强制解锁** - 管理员可以强制解锁被锁定的单据
4. **锁状态查询** - 查询单据的锁定状态
5. **请求解锁** - 当单据被其他用户锁定时，可以请求解锁
6. **拒绝解锁** - 锁定用户可以拒绝其他用户的解锁请求
7. **状态广播** - 锁定状态变化会广播给所有在线用户

## 命令类型

### 服务器端命令

1. **DocumentLockApplyCommand** - 申请锁定单据命令
2. **DocumentUnlockCommand** - 解锁单据命令
3. **ForceUnlockCommand** - 强制解锁命令
4. **QueryLockStatusCommand** - 查询锁状态命令
5. **RequestUnlockCommand** - 请求解锁命令
6. **RefuseUnlockCommand** - 拒绝解锁命令
7. **BroadcastLockStatusCommand** - 广播锁状态命令

### 客户端命令

1. **RequestLockCommand** - 客户端请求锁命令
2. **ClientLockManagerCmd** - 客户端锁管理命令（旧版，仅作参考）

## 核心类

### 服务接口
- `ILockManagerService` - 锁管理服务接口

### 服务实现
- `LockManagerService` - 锁管理服务实现

### 命令处理器
- `LockCommandHandler` - 锁命令处理器

### 数据模型
- `LockInfo` - 锁定信息
- `LockRequest` - 锁定请求（包含解锁请求和拒绝解锁信息）
- `LockResponse` - 锁定响应

## 使用示例

### 锁定单据
```csharp
var lockCommand = new DocumentLockApplyCommand(orderId, billData, menuId);
var response = await commandFactory.ExecuteCommandAsync(lockCommand);
```

### 解锁单据
```csharp
var unlockCommand = new DocumentUnlockCommand(orderId, userId);
var response = await commandFactory.ExecuteCommandAsync(unlockCommand);
```

### 查询锁状态
```csharp
var queryCommand = new QueryLockStatusCommand(orderId);
var response = await commandFactory.ExecuteCommandAsync(queryCommand);
```

## 业务流程

1. 用户打开销售订单进行编辑时，系统自动发送锁定请求
2. 服务器检查单据是否已被锁定
3. 如果未被锁定，则锁定该单据并返回成功
4. 如果已被锁定，则返回失败，提示用户单据正在被其他用户编辑
5. 锁定状态变化会广播给所有在线用户
6. 用户完成编辑后，系统自动发送解锁请求
7. 其他用户可以通过请求解锁功能请求当前用户释放锁
8. 当前用户可以选择同意或拒绝解锁请求

## 注意事项

1. 锁定的单据会在用户关闭窗口或超时后自动解锁
2. 管理员具有强制解锁权限
3. 锁定状态会实时同步给所有在线用户
4. 系统会记录所有的锁操作日志