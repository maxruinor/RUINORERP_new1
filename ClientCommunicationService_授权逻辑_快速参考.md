# 客户端通信授权逻辑 - 快速参考

## 🎯 核心规则

### 授权要求矩阵

| 命令类型 | Token 要求 | 说明 |
|---------|-----------|------|
| 🔑 **Login（登录）** | ❌ 不需要 | 首次认证，尚未获得 Token |
| 🚪 **Logout（登出）** | ✅ 需要 | 验证身份后安全登出 |
| 💼 **业务命令** | ✅ 需要 | 所有业务操作 |
| ⚙️ **系统命令-关键** | ✅ 需要 | 重要系统操作 |
| 📊 **系统命令-非关键** | ⚠️ 可选 | 心跳、性能监控等 |

---

## 🔍 代码位置

**文件**: `RUINORERP.UI\Network\ClientCommunicationService.cs`  
**方法**: `SendPacketCoreAsync` (第 2148 行)  
**授权检查**: 第 2227-2254 行

---

## 💡 关键代码

```csharp
packet.ExecutionContext.RequestId = request.RequestId;
packet.CommandId = commandId;

// 根据命令类型设置上下文信息
if (packet.CommandId == AuthenticationCommands.Login)
{
    // 登录命令：不需要SessionId、UserId和Token
    _logger?.LogDebug("登录命令，跳过SessionId/UserId/Token设置");
}
else
{
    // 非登录命令：需要设置SessionId、UserId和Token
    
    // 1. 设置SessionId和UserId
    if (MainForm.Instance?.AppContext != null)
    {
        packet.ExecutionContext.SessionId = MainForm.Instance.AppContext.SessionId;
        packet.ExecutionContext.UserId = MainForm.Instance.AppContext.CurUserInfo?.UserID ?? 0;
    }
    
    // 2. 自动附加Token
    await AutoAttachTokenAsync(packet.ExecutionContext);
    
    // 3. 授权检查
    if (packet.ExecutionContext.Token == null)
    {
        // 非关键命令：静默跳过
        bool isNonCriticalCommand = 
            packet.CommandId == SystemCommands.PerformanceDataUpload ||
            packet.CommandId == SystemCommands.Heartbeat ||
            packet.CommandId == SystemCommands.PerformanceMonitorStatus;
        
        if (isNonCriticalCommand)
        {
            _logger?.LogDebug("非关键命令 {CommandId} Token未就绪，静默跳过", commandId);
            return;  // 不发送请求
        }
        
        // 关键命令：抛出异常
        throw new Exception($"发送请求失败: 没有合法授权令牌, 指令：{commandId}");
    }
}

// 设置响应类型（所有命令都需要）
if (ResponseTypeName == null)
{
    packet.ExecutionContext.NeedResponse = false;
    packet.ExecutionContext.ExpectedResponseTypeName = nameof(ResponseBase);
}
else
{
    packet.ExecutionContext.ExpectedResponseTypeName = ResponseTypeName;
    packet.ExecutionContext.NeedResponse = true;
}
```

---

## 🛡️ Token 附加流程

```
AutoAttachTokenAsync()
    ↓
检查 TokenManager 是否初始化
    ↓
获取当前 Token
    ↓
Token 有效？
    ├─ ✅ 是 → 附加 Token
    └─ ❌ 否 → 尝试刷新
                ├─ ✅ 成功 → 附加新 Token
                └─ ❌ 失败 → Token 为空（由授权检查处理）
```

---

## ⚠️ 常见问题

### Q1: 为什么登出命令需要 Token？

**A**: 确保只有合法用户可以登出自己的会话，防止恶意调用。

### Q2: Token 过期了怎么办？

**A**: 
1. 系统会自动尝试刷新 Token（提前5分钟）
2. 如果刷新失败，会抛出异常
3. 用户需要重新登录

### Q3: 哪些命令可以没有 Token？

**A**: 
- Login（登录）
- Heartbeat（心跳）
- PerformanceDataUpload（性能数据上报）
- PerformanceMonitorStatus（性能监控状态）

---

## 🔧 调试技巧

### 查看 Token 状态

```csharp
// 在日志中搜索以下关键词
"Token附加成功"           // Token 正常附加
"Token已过期或即将过期"   // Token 需要刷新
"Token刷新成功"           // 刷新成功
"Token附加失败"           // Token 缺失
```

### 检查授权失败

```csharp
// 查找异常信息
"发送请求失败: 没有合法授权令牌"
```

---

## 📝 最佳实践

### ✅ 推荐做法

1. **登录成功后立即保存 Token**
   ```csharp
   await _tokenManager.TokenStorage.SetTokenAsync(response.Token);
   ```

2. **定期检查 Token 状态**
   ```csharp
   var token = await _tokenManager.TokenStorage.GetTokenAsync();
   if (token.IsExpired())
   {
       // 提示用户重新登录
   }
   ```

3. **优雅处理授权失败**
   ```csharp
   try
   {
       await SendCommandAsync(...);
   }
   catch (Exception ex) when (ex.Message.Contains("授权令牌"))
   {
       // 跳转到登录页面
       ShowLoginPage();
   }
   ```

### ❌ 避免做法

1. **不要手动设置 Token**
   ```csharp
   // ❌ 错误
   packet.ExecutionContext.Token = someToken;
   
   // ✅ 正确：让 AutoAttachTokenAsync 自动处理
   ```

2. **不要在未登录时执行业务操作**
   ```csharp
   // ❌ 错误
   if (CurUserInfo == null)
   {
       await SendBusinessCommandAsync(...);  // 会失败
   }
   ```

3. **不要忽略 Token 刷新失败**
   ```csharp
   // ❌ 错误：忽略异常
   try { await RefreshToken(); } catch {}
   
   // ✅ 正确：提示用户
   try { await RefreshToken(); }
   catch { ShowReloginPrompt(); }
   ```

---

## 🧪 测试场景

### 场景1: 正常登录和使用

```
1. 启动应用
2. 输入用户名密码
3. 点击登录 → 获得 Token
4. 执行业务操作 → Token 自动附加 ✅
```

### 场景2: Token 过期

```
1. 登录后等待 Token 过期（或修改系统时间）
2. 执行业务操作
3. 系统自动尝试刷新 Token
4. 刷新失败 → 提示重新登录 ⚠️
```

### 场景3: 未登录执行操作

```
1. 启动应用（未登录）
2. 执行业务操作
3. 抛出异常："没有合法授权令牌" ❌
```

### 场景4: 心跳命令（无 Token）

```
1. 启动应用（未登录）
2. 发送心跳命令
3. 静默跳过，不发送请求 ✅
```

---

## 📚 相关文档

- [ClientCommunicationService_授权逻辑审查报告.md](./ClientCommunicationService_授权逻辑审查报告.md)
- [NullReferenceException修复说明.md](./NullReferenceException修复说明.md)

---

**更新日期**: 2026-04-14  
**维护人员**: 开发团队
