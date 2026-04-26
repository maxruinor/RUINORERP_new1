# Redis 缓存统一配置说明

## 📋 概述

RUINORERP.Server 现已支持通过**统一的配置开关**控制所有模块的 Redis 缓存服务，包括：
- ✅ 主缓存服务（Startup.cs）
- ✅ 智能提醒模块（SmartReminder）

**默认状态：Redis 已禁用**，系统自动使用内存缓存作为替代方案。

---

## ⚙️ 配置方法

### 1. 配置文件位置

编辑 `RUINORERP.Server/appsettings.json` 文件

### 2. 配置项说明

```json
{
  "RedisEnabled": false,              // 🔑 Redis 启用开关（true=启用，false=禁用）
  "RedisServer": "192.168.0.254:6379", // Redis 服务器地址
  "RedisServerPWD": "your_password"    // Redis 密码（可选）
}
```

**注意**：所有模块（主缓存服务、智能提醒等）都使用上述统一配置，无需为各模块单独配置。

---

## 🎯 使用场景

### 场景 1：禁用 Redis（默认，推荐开发环境）

```json
{
  "RedisEnabled": false,
  "RedisServer": "192.168.0.254:6379",
  "RedisServerPWD": ""
}
```

**效果**：
- ✅ 不会尝试连接 Redis 服务器
- ✅ 所有模块自动使用内存缓存
- ✅ 避免 SocketException 异常日志
- ✅ 启动速度更快

### 场景 2：启用 Redis（生产环境）

```json
{
  "RedisEnabled": true,
  "RedisServer": "192.168.0.254:6379",
  "RedisServerPWD": "H[E]PC6Oz9H4bBYfz03B1UHj"
}
```

**效果**：
- ✅ 主缓存服务和智能提醒模块都会连接 Redis
- ✅ 支持分布式缓存
- ✅ 多服务器实例共享缓存数据

### 场景 3：Redis 服务器不可用时的降级

即使 `RedisEnabled: true`，如果 Redis 服务器无法连接，系统会**自动降级**到内存缓存，不会导致应用启动失败。

---

## 🔍 调试信息

在 Debug 模式下，您可以在输出窗口看到以下日志：

### Redis 禁用时
```
[Redis] Redis 已禁用，使用内存缓存适配器
[SmartReminder] Redis 已禁用，跳过 Redis 连接
```

### Redis 启用并连接成功
```
[Redis] 正在连接到 Redis 服务器: 192.168.0.254:6379
[Redis] 成功连接到 Redis 服务器
[SmartReminder] 正在连接到 Redis 服务器: 192.168.0.254:6379
[SmartReminder] Redis连接成功: 192.168.0.254:6379
[SmartReminder] Redis 服务已成功注册
```

### Redis 连接失败（自动降级）
```
[Redis] 连接失败: It was not possible to connect to the redis server(s).，降级到内存缓存
[SmartReminder] Redis连接错误: 类型=UnableToConnect, 消息=It was not possible to connect...
```

---

## 📝 修改历史

| 日期 | 版本 | 说明 |
|------|------|------|
| 2026-04-26 | 1.0 | 初始版本，添加统一 Redis 开关 |

---

## ❓ 常见问题

### Q1: 为什么默认禁用 Redis？
**A**: 大多数开发环境不需要 Redis，禁用后可以：
- 避免不必要的 SocketException 日志
- 加快应用启动速度
- 简化开发环境配置

### Q2: 如何确认当前使用的是 Redis 还是内存缓存？
**A**: 查看 Debug 输出窗口的日志，会明确显示使用的是哪种缓存。

### Q3: 如何为不同模块配置不同的 Redis 服务器？
**A**: 当前设计不支持多 Redis 配置。所有模块共享同一个 Redis 服务器，通过 `RedisEnabled` 开关统一控制。

### Q4: 切换 Redis 开关后需要重启应用吗？
**A**: 是的，`RedisEnabled` 配置在应用启动时读取，修改后需要重启才能生效。

---

## 🔗 相关文件

- 配置文件：`RUINORERP.Server/appsettings.json`
- 主服务注册：`RUINORERP.Server/Startup.cs` (第 276-350 行)
- 智能提醒模块：`RUINORERP.Server/SmartReminder/SmartReminderModule.cs` (第 92-207 行)
