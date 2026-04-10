# Token 授权体系优化重构报告

## 一、重构概述

本次重构针对 RUINORERP 项目的 Token 授权体系进行了简化式优化，主要目标是：**提升安全性、减少冗余代码、优化性能**。

### 重构范围

- **认证核心**：`ITokenService.cs`, `JwtTokenService.cs`, `TokenServiceOptions.cs`, `TokenManager.cs`, `TokenValidationResult.cs`
- **服务端处理**：`LoginCommandHandler.cs`, `SessionService.cs`
- **依赖注入配置**：`PacketSpecServicesDependencyInjection.cs`

---

## 二、重构内容

### 2.1 安全性增强 (P0)

#### 问题：Token 撤销机制为空实现

**原代码** (`JwtTokenService.cs`):
```csharp
public void RevokeToken(string token)
{
    // 简化实现，此处仅作为接口实现占位
}
```

**优化后**:
```csharp
// 新增令牌黑名单
private static readonly ConcurrentDictionary<string, DateTime> _revokedTokens = new ConcurrentDictionary<string, DateTime>();

public async Task RevokeTokenAsync(string token)
{
    if (string.IsNullOrEmpty(token)) return;
    var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
    var jti = jwtToken.Id;
    var expiryTime = jwtToken.ValidTo > DateTime.Now ? jwtToken.ValidTo : DateTime.Now.AddHours(1);
    _revokedTokens.TryAdd(jti, expiryTime);
}

public bool IsTokenRevoked(string jti)
{
    if (_revokedTokens.TryGetValue(jti, out var expiryTime))
    {
        if (expiryTime < DateTime.Now)
        {
            _revokedTokens.TryRemove(jti, out _);
            return false;
        }
        return true;
    }
    return false;
}
```

**影响**: 登出后 Token 真正失效，支持强制下线功能。

---

#### 问题：硬编码默认密钥

**原代码** (`TokenServiceOptions.cs`):
```csharp
public string SecretKey { get; set; } = "your-default-secret-key";
```

**优化后**:
```csharp
public string SecretKey { get; set; }

public void Validate()
{
    if (string.IsNullOrWhiteSpace(SecretKey))
        throw new ArgumentException("Token服务配置错误：必须配置 SecretKey");
    if (SecretKey.Length < 32)
        throw new ArgumentException("Token服务配置错误：SecretKey 长度至少需要32位");
}
```

**影响**: 强制要求配置强密钥，提升安全性。

---

### 2.2 接口简化 (P1)

#### 问题：接口方法冗余

**原接口** (`ITokenService.cs`):
```csharp
string GenerateToken(...);           // 冗余
string GenerateAccessToken(...);     // 冗余
string GenerateRefreshToken(...);    // 冗余
(string, string) GenerateTokens(...);// 保留
TokenValidationResult ValidateToken(...);
(string, string) RefreshTokens(...); // 保留
string RefreshToken(...);            // 冗余，与 RefreshTokens 功能重叠
void RevokeToken(...);               // 空实现
```

**优化后**:
```csharp
(string AccessToken, string RefreshToken) GenerateTokens(...);
TokenValidationResult ValidateToken(...);
(string AccessToken, string RefreshToken) RefreshTokens(...);
Task RevokeTokenAsync(string token);
bool IsTokenRevoked(string jti);
```

**影响**: 接口从 8 个方法减少到 5 个，消除误用风险。

---

### 2.3 性能优化 (P2)

#### 问题：会话清理过于频繁

**原代码** (`SessionService.cs`):
```csharp
_cleanupTimer = new Timer(..., TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(2));
```

**优化后**:
```csharp
_cleanupTimer = new Timer(..., TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
```

**影响**: 清理频率降低 60%，减少 CPU 开销。

---

#### 问题：不必要的异步包装

**原代码** (`TokenManager.cs`):
```csharp
public async Task<bool> ValidateTokenAsync(string token)
{
    var result = await Task.Run(() => _tokenService.ValidateToken(token));
    return result?.IsValid == true;
}
```

**优化后**:
```csharp
public Task<bool> ValidateTokenAsync(string token)
{
    var result = _tokenService.ValidateToken(token);
    return Task.FromResult(result?.IsValid == true);
}
```

**影响**: 减少线程池调度开销。

---

### 2.4 代码清理

#### 移除 LoginCommandHandler 中的类型检查分支

**原代码**:
```csharp
if (TokenService is JwtTokenService jwtTokenService)
{
    var tokens = jwtTokenService.RefreshTokens(refreshToken);
    // ...
}
else
{
    // 兼容旧版本
    newAccessToken = TokenService.RefreshToken(refreshToken);
    // ...
}
```

**优化后**:
```csharp
var tokens = TokenService.RefreshTokens(refreshToken);
newAccessToken = tokens.AccessToken;
newRefreshToken = tokens.RefreshToken;
```

**影响**: 简化代码，消除死代码。

---

## 三、配置文件要求

### 服务器端配置 (`appsettings.json`)

```json
{
  "TokenService": {
    "SecretKey": "your-256-bit-secret-key-at-least-32-chars",
    "DefaultExpiryHours": 8
  }
}
```

**当前配置** (已满足要求):
```json
"TokenService": {
  "SecretKey": "kour-super-secret-key-at-least-32-chars",
  "DefaultExpiryHours": 8
}
```

---

## 四、修改文件清单

| 文件 | 修改类型 | 说明 |
|------|----------|------|
| `ITokenService.cs` | 重大修改 | 精简接口，移除冗余方法 |
| `JwtTokenService.cs` | 重大修改 | 实现 Token 黑名单 |
| `TokenServiceOptions.cs` | 重大修改 | 移除默认密钥，增加验证 |
| `TokenManager.cs` | 小幅修改 | 移除不必要的 async 包装 |
| `TokenValidationResult.cs` | 无修改 | - |
| `MemoryTokenStorage.cs` | 无修改 | - |
| `LoginCommandHandler.cs` | 小幅修改 | 简化 Token 刷新逻辑 |
| `SessionService.cs` | 小幅修改 | 优化清理频率 |
| `PacketSpecServicesDependencyInjection.cs` | 小幅修改 | 启动时强制验证配置 |

---

## 五、架构设计

### 5.1 优化后的 Token 流转

```
┌─────────────────────────────────────────────────────────────────────┐
│                           服务端                                     │
├─────────────────────────────────────────────────────────────────────┤
│  ITokenService (5个方法)                                            │
│  ├── GenerateTokens() → (AccessToken, RefreshToken)                 │
│  ├── ValidateToken() → TokenValidationResult                       │
│  ├── RefreshTokens() → (NewAccess, NewRefresh)                     │
│  ├── RevokeTokenAsync() → 加入黑名单                                │
│  └── IsTokenRevoked() → 检查黑名单                                  │
│                                                                     │
│  JwtTokenService                                                    │
│  ├── _revokedTokens (ConcurrentDictionary) → Token 黑名单          │
│  └── ValidateToken() → 自动检查黑名单                                │
└─────────────────────────────────────────────────────────────────────┘
```

### 5.2 与现有系统的兼容性

- ✅ Token 格式保持不变 (HS256)
- ✅ AccessToken 有效期 8 小时
- ✅ RefreshToken 有效期 7 天
- ✅ 服务器端配置无需修改 (已配置)

---

## 六、后续优化建议

### P3 优先级

1. **Token 黑名单升级为 Redis**
   - 当前使用内存存储
   - 多实例部署需改为 Redis 共享存储

2. **客户端 Token 持久化**
   - 当前 MemoryTokenStorage 重启后 Token 丢失
   - 可考虑使用加密文件存储实现"记住登录"

3. **Token 续期策略**
   - 可根据业务场景调整 AccessToken/RefreshToken 有效期
   - 可增加滑动窗口续期机制

---

## 七、总结

本次重构遵循**简化式优化**原则：

| 指标 | 重构前 | 重构后 | 改进 |
|------|--------|--------|------|
| ITokenService 接口方法数 | 8 | 5 | -37.5% |
| Token 撤销 | 空实现 | 黑名单机制 | ✅ |
| 默认密钥 | 有 | 无 | ✅ |
| 会话清理频率 | 2分钟 | 5分钟 | -60% |
| 冗余代码 | 多处 | 已清理 | ✅ |

重构已完成，系统可正常编译运行。
