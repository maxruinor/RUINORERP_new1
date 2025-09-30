# Token管理功能说明

## 概述

本系统提供完整的Token管理功能，包括Token生成、验证、刷新和客户端管理。基于JWT实现，支持自动刷新和过期处理。

## 核心组件

### ITokenService

接口定义了Token生成、验证和刷新的核心功能。

### TokenValidationService

负责验证Token的有效性和处理过期检查，提供异步验证方法。在验证过程中会检查Token是否在5分钟内过期，如果是，则在Claims中添加NeedsRefresh标记和ExpiresIn秒数。

### TokenInfo

包含访问令牌和刷新令牌的完整信息，包括AccessToken、RefreshToken、ExpiresIn（访问令牌过期时间，秒）、RefreshTokenExpiresIn（刷新令牌过期时间，秒）、TokenType和GeneratedTime（生成时间，UTC）。

### LoginCommandHandler

登录命令处理器（模拟实现），负责生成完整的Token信息，包括访问令牌和刷新令牌。生成的访问令牌包含丰富的用户会话信息，并设置为1小时过期；刷新令牌包含特殊的类型标识，设置为7天过期。

## 使用流程

### 客户端使用

1. **登录**
```csharp
var loginService = new UserLoginService(commService, logger);
var loginResponse = await loginService.LoginAsync("username", "password");
// 保存Token信息
// 注意：客户端需要自行实现Token管理逻辑
```

2. **Token刷新**
```csharp
var refreshResponse = await loginService.RefreshTokenAsync(refreshToken, accessToken);
// 更新Token信息
// 注意：客户端需要自行实现Token管理逻辑
```

### 服务端处理

1. **Token验证**
```csharp
var tokenValidationService = new TokenValidationService(tokenService);
var validationResult = await tokenValidationService.ValidateTokenAsync(token);

// 检查验证结果
if (validationResult.IsValid) {
    // Token有效
    var userId = validationResult.UserId;
    var username = validationResult.Username;
    
    // 检查是否需要刷新Token（如果Claims中包含NeedsRefresh标记）
    if (validationResult.Claims != null && validationResult.Claims.ContainsKey("NeedsRefresh")) {
        // 提示客户端刷新Token
    }
} else {
    // Token无效
    Console.WriteLine($"Token验证失败: {validationResult.ErrorMessage}");
}
```

2. **Token刷新**
```csharp
var refreshResult = await tokenValidationService.RefreshTokenAsync(refreshToken, currentToken);
```

## 配置

在appsettings.json中添加以下配置：

```json
{
  "TokenService": {
    "SecretKey": "your-super-secret-key-at-least-32-chars",
    "DefaultExpiryHours": 8,
    "RefreshTokenExpiryHours": 24
  }
}
```

## 依赖注入

确保在Startup.cs或Program.cs中注册服务：

```csharp
services.AddPacketSpecServices(Configuration);
```

## 安全建议

1. 使用足够强度的SecretKey（至少32个字符）
2. 合理设置Token过期时间
3. 对敏感操作进行额外权限验证
4. 使用HTTPS传输Token
5. 定期轮换SecretKey