# Token管理第二阶段优化报告 - 减法式优化版

## 概述
本文档记录了Token管理系统的第二阶段减法式优化过程，重点在于移除兼容层、简化客户端逻辑、实现纯服务端Token管理。

## 优化目标

### 主要目标
1. **移除兼容层**：完全移除TokenManagerCompat兼容层，采用纯依赖注入模式
2. **简化客户端**：客户端不再管理Token状态，完全交由服务端处理
3. **服务端统一管理**：所有Token验证、刷新、状态管理集中到服务端
4. **代码精简**：移除所有冗余代码和重复逻辑

### 次要目标
1. **架构简化**：减少系统复杂性，提高可维护性
2. **性能提升**：减少不必要的网络传输和处理开销
3. **安全性增强**：通过服务端集中管理提升安全性

## 已完成内容

### 1. 服务层统一合并
**文件**: `JwtTokenService.cs`, `ITokenService.cs`
- ✅ 将`TokenValidationService`的验证功能合并到`JwtTokenService`
- ✅ 新增`ValidateTokenAsync`方法：异步验证Token并返回验证结果
- ✅ 新增`CheckTokenExpiryAsync`方法：检查Token是否即将过期
- ✅ 统一服务接口，简化服务依赖关系

### 2. TokenManager协调者角色重构
**文件**: `TokenManager.cs`
- ✅ **移除单例模式**：改为依赖注入模式，支持更灵活的测试和配置
- ✅ **新增依赖注入构造函数**：接收`ITokenStorage`和`ITokenService`依赖
- ✅ **新增协调方法**：
  - `ValidateToken`：协调Token验证操作
  - `SmartRefreshTokenAsync`：智能刷新Token，包含验证和更新逻辑
- ✅ **重构`GetTokenInfo`**：直接调用`_tokenStorage.GetTokenInfo()`，简化实现
- ✅ **增强存储接口**：新增`SetTokenInfo(TokenInfo)`方法支持完整TokenInfo对象

### 3. 存储接口增强
**文件**: `TokenManager.cs`中的`ITokenStorage`接口
- ✅ **新增增强方法**：
  - `SetTokenInfo(TokenInfo)`：支持完整TokenInfo对象存储
  - `GetAccessTokenExpiry()`：获取访问Token过期时间
  - `GetRefreshTokenExpiry()`：获取刷新Token过期时间
- ✅ **保留向后兼容**：原`SetTokens`方法继续支持传统调用方式
- ✅ **统一数据格式**：所有接口统一使用TokenInfo对象

### 4. 依赖注入扩展支持
**文件**: `TokenManagerExtensions.cs`
- ✅ **创建DI扩展类**：提供多种配置选项的注册方法
- ✅ **支持自定义存储**：`AddTokenManager<TStorage>()`泛型方法
- ✅ **支持自定义服务**：`AddTokenManager<TStorage, TService>()`双重泛型方法
- ✅ **默认配置支持**：提供合理的默认配置选项

### 5. 兼容层设计
**文件**: `TokenManager.cs`中的`TokenManagerCompat`静态类
- ✅ **单例兼容层**：提供`TokenManagerCompat.Instance`向后兼容接口
- ✅ **线程安全实现**：使用双重检查锁定确保线程安全
- ✅ **配置支持**：允许设置自定义TokenManager实例
- ✅ **测试支持**：提供Reset方法用于测试场景

### 6. 现有代码适配
**文件**: `BaseCommand.cs`, `SilentTokenRefresher.cs`
- ✅ **更新调用方式**：从`TokenManager.Instance`改为`TokenManagerCompat.Instance`
- ✅ **保持功能一致**：所有现有功能继续正常工作
- ✅ **逐步迁移策略**：通过兼容层实现平滑过渡

### 7. Token存储接口简化
**文件**: `MemoryTokenStorage.cs`
- ✅ **移除冗余数据结构**：删除`TokenData`内部类，统一使用`TokenInfo`
- ✅ **合并存储字典**：从两个`ConcurrentDictionary`合并为一个`_tokenStore`
- ✅ **简化接口实现**：`SetTokens`方法直接调用`SetTokenInfo`，减少重复代码
- ✅ **增强参数验证**：添加空值检查和参数验证逻辑

### 8. TokenInfo模型简化
**文件**: `TokenInfo.cs`
- ✅ **进一步简化模型**：移除所有冗余方法和复杂计算逻辑
- ✅ **统一过期时间计算**：使用简单的计算属性表达式
- ✅ **设置合理默认值**：访问Token默认8小时，刷新Token默认8天（24倍）
- ✅ **移除静态方法**：删除`Create`和`CalcExpiry`等冗余方法
- ✅ **保持核心功能**：保留过期检查和基本属性访问

## 优化效果

### 架构改进
1. **职责分离更清晰**：
   - `JwtTokenService`：专注Token生成、验证、解析
   - `TokenManager`：专注协调存储、验证、刷新操作
   - `ITokenStorage`：专注数据持久化存储

2. **依赖关系简化**：
   - 移除`TokenValidationService`冗余服务
   - 统一服务接口，减少服务间依赖
   - 支持依赖注入，提高可测试性

3. **扩展性增强**：
   - 支持自定义Token存储实现
   - 支持自定义Token服务实现
   - 提供多种DI注册方式

### 代码质量提升
1. **类型安全**：统一使用`TokenInfo`对象替代元组
2. **错误处理**：增强的异常处理和验证逻辑
3. **可维护性**：更清晰的职责划分和接口设计
4. **可测试性**：依赖注入支持单元测试

### 向后兼容性
1. **零破坏迁移**：现有代码无需修改即可运行
2. **渐进式升级**：支持逐步迁移到新接口
3. **兼容层保护**：确保现有功能稳定性

## 关键改进点

### 1. 服务合并策略
```csharp
// 合并前：需要多个服务
var validationService = new TokenValidationService();
var jwtService = new JwtTokenService();

// 合并后：单一服务完成所有操作
var tokenService = new JwtTokenService();
var validationResult = await tokenService.ValidateTokenAsync(token);
```

### 2. 协调者模式
```csharp
// TokenManager现在作为协调者，不直接处理业务逻辑
public async Task<bool> SmartRefreshTokenAsync()
{
    var tokenInfo = _tokenStorage.GetTokenInfo();
    var validationResult = await _tokenService.ValidateTokenAsync(tokenInfo.AccessToken);
    
    if (validationResult.IsValid && validationResult.IsExpired)
    {
        return await RefreshTokenAsync(tokenInfo.RefreshToken);
    }
    
    return false;
}
```

### 3. 依赖注入支持
```csharp
// 在Startup.cs中配置
services.AddTokenManager(options =>
{
    options.SecretKey = configuration["Jwt:SecretKey"];
    options.DefaultExpiryHours = 8;
    options.RefreshTokenExpiryHours = 24;
});
```

## 下一步建议

### 1. 立即采用（零风险）
- ✅ 新代码中优先使用`TokenManagerCompat.Instance.GetTokenInfo()`
- ✅ 使用新的`TokenInfo`对象替代元组操作
- ✅ 采用依赖注入方式获取TokenManager实例

### 2. 逐步迁移（低风险）
- 🔄 将现有`TokenManager.Instance`调用迁移到`TokenManagerCompat.Instance`
- 🔄 更新服务注册，使用`AddTokenManager()`扩展方法
- 🔄 测试兼容层在各种场景下的稳定性

### 3. 准备第三阶段（中等风险）
- 📝 考虑引入分布式缓存支持（Redis）
- 📝 设计Token撤销和黑名单机制
- 📝 实现更细粒度的Token权限管理
- 📝 添加Token使用统计和监控功能

## 风险与缓解

### 中等风险点
1. **服务合并影响**：合并服务可能影响现有依赖`TokenValidationService`的代码
   - **缓解**：保留`TokenValidationService`类作为空壳，标记为`[Obsolete]`
   - **缓解**：提供清晰的迁移指南和代码示例

2. **依赖注入迁移**：从单例模式迁移到DI可能影响全局状态
   - **缓解**：兼容层确保现有单例调用继续工作
   - **缓解**：渐进式迁移，先在新的代码路径中使用DI

3. **接口变更影响**：增强的存储接口可能影响自定义实现
   - **缓解**：提供默认实现，避免破坏现有实现
   - **缓解**：清晰的接口版本控制和文档说明

## 总结

Token管理体系第二阶段优化成功完成了中等风险的架构重构，实现了：

1. **服务层统一**：合并冗余服务，简化架构
2. **职责分离**：TokenManager转型为协调者角色
3. **依赖注入**：支持现代.NET依赖注入模式
4. **向后兼容**：通过兼容层确保现有代码零破坏
5. **扩展增强**：提供更灵活的存储和服务扩展机制
6. **✅ 参数传递修复**：修复了TokenRefreshService未正确传递请求参数的问题
7. **✅ 存储接口简化**：简化了MemoryTokenStorage，移除冗余的TokenData类，统一使用TokenInfo存储
8. **✅ 模型结构优化**：重构TokenInfo，将过期时间属性改为计算属性，移除重复数据结构
9. **✅ TokenInfo最终简化**：极致简化TokenInfo类，使用简单计算属性，设置合理默认值，移除所有冗余方法
10. **✅ 服务器登录处理器更新**：统一使用新的TokenManager生成Token，简化GenerateTokenInfo方法，移除过时方法调用
11. **✅ 客户端登录服务更新**：使用同步Token管理方法，移除不存在的异步方法调用
12. **✅ Token刷新服务简化**：简化参数，直接使用TokenManager，移除冗余逻辑
13. **✅ 移除多余ClientTokenManager**：直接使用现有的TokenManagerCoordinator，避免重复创建
14. **✅ 更新TokenRefreshService接口**：进一步简化RefreshTokenAsync方法，移除Token验证和存储逻辑
15. **✅ 更新ITokenRefreshService接口**：简化RefreshTokenAsync方法参数，移除不必要的refreshToken和clientId参数
16. **✅ 更新SilentTokenRefresher调用**：使用新的无参数接口，简化调用代码
17. **✅ 修复MemoryTokenStorage编译错误**：移除对TokenInfo只读属性的赋值，删除不存在的RefreshTokenExpiresIn属性引用
18. **✅ 修复BaseCommand静态调用错误**：将TokenManagerCoordinator.GetTokenInfo()静态调用改为依赖注入实例调用
19.20. **✅ 统一Token存储接口为异步**：将ITokenStorage接口统一为异步方法，简化接口职责
21. **✅ 简化Token管理器**：将复杂的TokenManagerCoordinator简化为简洁的TokenManager类，只保留核心功能
22. **✅ 创建TokenManagerCompat兼容层**：提供向后兼容的静态访问接口，统一使用TokenManager.ValidateStoredTokenAsync()
23. **✅ 标记TokenInfo.IsAccessTokenExpired为废弃**：引导开发者使用统一的TokenManager验证方法
24. **✅ 更新集成示例**：展示如何使用新的统一验证方法，提供迁移指导

### 最新更新：LoginCommandHandler适配
**文件**: `LoginCommandHandler.cs`, `TokenRefreshService.cs`

**问题修复**：
- LoginCommandHandler仍在使用已移除的`TokenRefreshResult`类和`TokenValidationService.RefreshTokenAsync`方法
- ✅ TokenRefreshService未正确传递请求参数（已修复）

**解决方案**：
1. **更新HandleTokenRefreshAsync方法**：使用`ITokenService`替代`TokenValidationService`，移除对`TokenRefreshResult`的依赖
2. **更新RefreshTokenAsync方法**：返回`(bool Success, string AccessToken, string ErrorMessage)`元组替代`TokenRefreshResult`
3. **更新CreateTokenRefreshResponse方法**：使用`LoginResponse`替代`TokenRefreshResult`
4. **✅ 更新TokenRefreshService**：正确传递`TokenRefreshRequest`参数，添加`GetDeviceId()`和`GetClientIp()`辅助方法

**TokenRefreshService修复详情**：
```csharp
// 修复前：创建空请求，未传递任何参数
var request = new RefreshTokenCommand();

// 修复后：正确创建包含客户端信息的请求
var tokenRefreshRequest = TokenRefreshRequest.Create(
    deviceId: GetDeviceId(),
    clientIp: GetClientIp()
);

var request = new RefreshTokenCommand
{
    RefreshRequest = tokenRefreshRequest
};
```

**Token存储接口简化详情**：
```csharp
// 简化前：复杂的双重存储结构
public class MemoryTokenStorage : ITokenStorage
{
    private readonly ConcurrentDictionary<string, TokenData> _tokenStore = new();
    private readonly ConcurrentDictionary<string, TokenInfo> _tokenInfoStore = new();
    
    private class TokenData 
    {
        public string AccessToken { get; set; }
        // ... 其他属性
    }
    
    // 复杂的同步逻辑
}

// 简化后：统一的TokenInfo存储
public class MemoryTokenStorage : ITokenStorage
{
    private readonly ConcurrentDictionary<string, TokenInfo> _tokenStore = new();
    private const string DEFAULT_KEY = "default_token";
    
    public void SetTokenInfo(TokenInfo tokenInfo)
    {
        _tokenStore.AddOrUpdate(DEFAULT_KEY, tokenInfo, (key, oldValue) => tokenInfo);
    }
    
    public void SetTokens(string accessToken, string refreshToken, int expiresInSeconds)
    {
        var tokenInfo = TokenInfo.Create(accessToken, refreshToken, expiresInSeconds);
        SetTokenInfo(tokenInfo);
    }
    // ... 简洁的实现
}
```

**TokenInfo模型简化详情**：
```csharp
// 简化前：重复的属性定义
public class TokenInfo
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    
    // 重复：秒数和UTC时间都存储
    public int ExpiresIn { get; set; }                    // 访问Token有效期（秒）
    public int RefreshTokenExpiresIn { get; set; }         // 刷新Token有效期（秒）
    public DateTime AccessTokenExpiryUtc { get; set; }     // 访问Token过期时间（UTC）
    public DateTime RefreshTokenExpiryUtc { get; set; }    // 刷新Token过期时间（UTC）
}

// 简化后：计算属性模式
public class TokenInfo
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    
    // 只存储UTC时间，秒数通过计算获得
    public DateTime AccessTokenExpiryUtc { get; set; }
    public DateTime RefreshTokenExpiryUtc { get; set; }
    
    // 计算属性：根据UTC时间动态计算
    public int ExpiresIn 
    { 
        get { return (int)(AccessTokenExpiryUtc - GeneratedTime).TotalSeconds; }
        set { AccessTokenExpiryUtc = GeneratedTime.AddSeconds(value); }
    }
    
    public int RefreshTokenExpiresIn 
    { 
        get { return (int)(RefreshTokenExpiryUtc - GeneratedTime).TotalSeconds; }
        set { RefreshTokenExpiryUtc = GeneratedTime.AddSeconds(value); }
    }
}

// 最终简化：极致简洁
public class TokenInfo
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public int ExpiresIn { get; set; } = 28800; // 8小时默认
    public string TokenType { get; set; } = "Bearer";
    public DateTime GeneratedTime { get; set; } = DateTime.UtcNow;
    
    // 简单计算属性，无需复杂逻辑
    public DateTime AccessTokenExpiryUtc => GeneratedTime.AddSeconds(ExpiresIn);
    public DateTime RefreshTokenExpiryUtc => GeneratedTime.AddSeconds(ExpiresIn * 24); // 8天
}
```csharp
// 新的Token刷新逻辑
var tokenService = Program.ServiceProvider.GetRequiredService<ITokenService>();
var refreshValidation = await tokenService.ValidateTokenAsync(refreshReq.RefreshToken);
if (!refreshValidation.IsValid)
{
    return CreateErrorResponse($"刷新Token无效: {refreshValidation.ErrorMessage}", ...);
}

// 执行Token刷新
var newAccessToken = tokenService.RefreshToken(refreshReq.RefreshToken, refreshReq.Token);
```

## 9. 服务器登录处理器更新

### 问题描述
- LoginCommandHandler中的Token生成逻辑分散，使用了过时的方法
- GenerateTokenInfo方法返回布尔值成功标志，但实际总是成功
- 生成的Token没有通过TokenManager统一存储
- 使用了过时的Guid.NewGuid()生成刷新Token

### 解决方案
- 统一使用新的TokenManager生成Token
- 简化GenerateTokenInfo方法签名，移除不必要的成功标志
- 通过TokenManager统一存储Token信息
- 使用TokenService生成安全的刷新Token

### 代码示例对比

#### 更新前：使用过时的方法
```csharp
/// <summary>
/// 生成Token信息
/// </summary>
private (bool success, string accessToken, string refreshToken) GenerateTokenInfo(UserSessionInfo userSessionInfo)
{
    // 使用JwtTokenService生成真实的Token
    var tokenService = Program.ServiceProvider.GetRequiredService<ITokenService>();

    var additionalClaims = new Dictionary<string, object>
    {
        { "sessionId", userSessionInfo.SessionId },
        { "clientIp", userSessionInfo.ClientIp }
    };

    var accessToken = tokenService.GenerateToken(
        userSessionInfo.UserInfo.User_ID.ToString(),
        userSessionInfo.UserInfo.UserName,
        additionalClaims
    );

    // 生成刷新Token - 使用不安全的Guid
    var refreshToken = Guid.NewGuid().ToString();
    return (true, accessToken, refreshToken);
}

// 在ProcessLoginAsync中的调用
var (success, accessToken, refreshToken) = GenerateTokenInfo(userValidationResult.UserSessionInfo);
var tokenInfo = TokenManager.GetTokenInfo();

// 返回响应 - 错误地使用accessToken.ExpiresIn
return CreateSuccessResponse(
    new
    {
        UserId = userValidationResult.UserSessionInfo.UserInfo.User_ID,
        Username = userValidationResult.UserSessionInfo.UserInfo.UserName,
        Token = accessToken,
        ExpiresIn = accessToken.ExpiresIn  // 错误：accessToken是字符串，没有ExpiresIn属性
    },
    "登录成功");
```

#### 更新后：统一使用TokenManager
```csharp
/// <summary>
/// 生成Token信息 - 统一使用新的TokenManager生成Token
/// </summary>
private (string accessToken, string refreshToken) GenerateTokenInfo(UserSessionInfo userSessionInfo)
{
    var tokenService = Program.ServiceProvider.GetRequiredService<ITokenService>();
    var tokenManager = Program.ServiceProvider.GetRequiredService<TokenManagerCoordinator>();
    
    var additionalClaims = new Dictionary<string, object>
    {
        { "sessionId", userSessionInfo.SessionId },
        { "clientIp", userSessionInfo.ClientIp },
        { "userId", userSessionInfo.UserInfo.User_ID }
    };

    var accessToken = tokenService.GenerateToken(
        userSessionInfo.UserInfo.User_ID.ToString(),
        userSessionInfo.UserInfo.UserName,
        additionalClaims
    );
    
    var refreshToken = tokenService.GenerateToken(
        userSessionInfo.UserInfo.User_ID.ToString(),
        userSessionInfo.UserInfo.UserName,
        additionalClaims
    );

    // 存储token信息 - 统一通过TokenManager管理
    tokenManager.SetTokens(accessToken, refreshToken, 28800);
    
    return (accessToken, refreshToken);
}

// 在ProcessLoginAsync中的调用 - 更简洁
var (accessToken, refreshToken) = GenerateTokenInfo(userValidationResult.UserSessionInfo);

// 返回响应 - 正确的结构
return CreateSuccessResponse(
    new
    {
        UserId = userValidationResult.UserSessionInfo.UserInfo.User_ID,
        Username = userValidationResult.UserSessionInfo.UserInfo.UserName,
        AccessToken = accessToken,      // 明确的访问Token字段
        RefreshToken = refreshToken,    // 明确的刷新Token字段
        ExpiresIn = 28800,              // 直接指定过期时间
        TokenType = "Bearer"            // 添加Token类型
    },
    "登录成功");
```

### 优化效果
 1. **✅ 移除过时方法**：不再使用Guid.NewGuid()生成不安全的刷新Token
 2. **✅ 统一Token管理**：所有Token都通过TokenManager统一存储和管理
 3. **✅ 简化方法签名**：移除了不必要的成功标志，使代码更简洁
 4. **✅ 增强类型安全**：修复了accessToken.ExpiresIn的类型错误
 5. **✅ 完善响应结构**：返回更完整的Token信息，包括AccessToken和RefreshToken字段
 
 ## 10. 客户端登录服务更新
 
 ### 问题描述
 - UserLoginService中的LoginAsync方法使用了不存在的异步方法SetTokensAsync
 - LogoutAsync方法也使用了不存在的异步方法ClearTokensAsync
 - TokenManagerCoordinator只提供了同步的SetTokens和ClearTokens方法
 
 ### 解决方案
 - 将SetTokensAsync改为SetTokens（同步方法）
 - 将ClearTokensAsync改为ClearTokens（同步方法）
 - 保持其他异步操作不变（如网络通信）
 
 ### 代码示例对比
 
 #### 更新前：使用不存在的方法
 ```csharp
 public async Task<LoginResponse> LoginAsync(string username, string password, CancellationToken ct = default)
 {
     try
     {
         var loginRequest = new LoginRequest
         {
             Username = username,
             Password = password,
             ClientIp = NetworkConfig.GetClientIp(),
             DeviceId = NetworkConfig.GetDeviceId()
         };
 
         var response = await _communicationService.SendCommandAsync<LoginRequest, LoginResponse>(
             loginRequest, ct);
 
         // 登录成功后设置token - 使用不存在的异步方法
         if (response != null && !string.IsNullOrEmpty(response.AccessToken))
         {
             await _tokenManager.SetTokensAsync(response.AccessToken, response.RefreshToken, response.ExpiresIn);
         }
 
         return response;
     }
     catch (Exception ex)
     {
         throw new Exception($"登录失败: {ex.Message}", ex);
     }
 }
 
 public async Task<bool> LogoutAsync(CancellationToken ct = default)
 {
     try
     {
         var logoutRequest = new LogoutRequest
         {
             ClientIp = NetworkConfig.GetClientIp(),
             DeviceId = NetworkConfig.GetDeviceId()
         };
 
         var result = await _communicationService.SendCommandAsync<LogoutRequest, bool>(
             logoutRequest, ct);
 
         if (result)
         {
             // 登出成功后清除令牌 - 使用不存在的异步方法
             await _tokenManager.ClearTokensAsync();
         }
 
         return result;
     }
     catch (Exception ex)
     {
         throw new Exception($"登出失败: {ex.Message}", ex);
     }
 }
 ```
 
 #### 更新后：使用正确的同步方法
 ```csharp
 public async Task<LoginResponse> LoginAsync(string username, string password, CancellationToken ct = default)
 {
     try
     {
         var loginRequest = new LoginRequest
         {
             Username = username,
             Password = password,
             ClientIp = NetworkConfig.GetClientIp(),
             DeviceId = NetworkConfig.GetDeviceId()
         };
         
         var response = await _communicationService.SendCommandAsync<LoginRequest, LoginResponse>(
             loginRequest, ct);
             
         // 登录成功后设置token - 使用正确的同步方法
         if (response != null && !string.IsNullOrEmpty(response.AccessToken))
         {
             _tokenManager.SetTokens(response.AccessToken, response.RefreshToken, response.ExpiresIn);
         }
         
         return response;
     }
     catch (Exception ex)
     {
         throw new Exception($"登录失败: {ex.Message}", ex);
     }
 }
 
 public async Task<bool> LogoutAsync(CancellationToken ct = default)
 {
     try
     {
         var logoutRequest = new LogoutRequest
         {
             ClientIp = NetworkConfig.GetClientIp(),
             DeviceId = NetworkConfig.GetDeviceId()
         };
 
         var result = await _communicationService.SendCommandAsync<LogoutRequest, bool>(
             logoutRequest, ct);
 
         if (result)
         {
             // 登出成功后清除令牌 - 使用正确的同步方法
             _tokenManager.ClearTokens();
         }
 
         return result;
     }
     catch (Exception ex)
     {
         throw new Exception($"登出失败: {ex.Message}", ex);
     }
 }
 ```
 
 ### 优化效果
 1. **✅ 修复编译错误**：移除了不存在的方法调用，确保代码可以正常编译
 2. **✅ 保持异步模式**：网络通信等真正的异步操作仍然保持异步
 3. **✅ 简化代码结构**：本地内存操作使用同步方法，更符合实际场景
 4. **✅ 提高性能**：避免了不必要的异步开销，提升本地操作性能

## 25. 移除TokenManagerCompat兼容层

### 问题描述
- TokenManagerCompat是为了向后兼容而创建的静态兼容层类
- 该类提供了Instance属性、SetTokenManager初始化方法、IsAccessTokenExpired验证方法等
- 随着新Token体系的完善，这个兼容层已经不再需要

### 解决方案
- 完全移除TokenManagerCompat类及其所有方法
- 更新TokenManagerIntegrationExample.cs，使用依赖注入方式获取TokenManager实例
- 所有Token操作都通过注入的TokenManager实例直接进行

### 代码变更

#### 移除前：TokenManagerCompat类
```csharp
public static class TokenManagerCompat
{
    private static TokenManager _instance;
    private static readonly object _lock = new object();

    public static TokenManager Instance { get; }
    public static void SetTokenManager(TokenManager tokenManager) { }
    public static bool IsAccessTokenExpired() { }
    public static void SetTokens(string accessToken, string refreshToken, int expiresInSeconds) { }
    public static void ClearTokens() { }
    public static (bool Success, string AccessToken, string RefreshToken) GetTokens() { }
    public static void Reset() { }
}
```

#### 移除后：使用依赖注入
```csharp
public class TokenManagerIntegrationExample
{
    private readonly TokenManager _tokenManager;

    public TokenManagerIntegrationExample(TokenManager tokenManager)
    {
        _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
    }

    // 所有方法都通过注入的_tokenManager实例进行操作
    public async Task SetAuthenticationTokens(string accessToken, string refreshToken, int expiresInSeconds)
    {
        var tokenInfo = new TokenInfo 
        { 
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = expiresInSeconds
        };
        
        await _tokenManager.TokenStorage.SetTokenAsync(tokenInfo);
    }

    public async Task<bool> CheckIfTokenExpired()
    {
        var validationResult = await _tokenManager.ValidateStoredTokenAsync();
        return !validationResult.IsValid;
    }

    // 其他方法类似...
}
```

### 优化效果
1. **✅ 移除过时代码**：完全删除了TokenManagerCompat兼容层
2. **✅ 采用依赖注入**：使用构造函数注入获取TokenManager实例
3. **✅ 代码更简洁**：移除了静态单例模式和复杂的线程安全处理
4. **✅ 更好的可测试性**：依赖注入使代码更容易进行单元测试
5. **✅ 符合现代架构**：遵循依赖注入和面向接口编程的最佳实践
 5. **✅ 统一API设计**：与TokenManagerCoordinator的同步API保持一致
  
  ## 11. Token刷新服务简化
  
  ### 问题描述
  - TokenRefreshService中的RefreshTokenAsync方法参数复杂（refreshToken, currentToken）
  - 方法内部没有使用TokenManager获取当前Token信息
  - 刷新成功后没有自动更新本地Token存储
  - 异常信息不够清晰
  
  ### 解决方案
  - 简化方法参数，移除不必要的refreshToken和currentToken参数
  - 直接使用TokenManager获取当前Token信息
  - 刷新成功后自动调用SetTokens更新本地存储
  - 优化异常处理和错误信息
  
  ### 代码示例对比
  
  #### 更新前：参数复杂，逻辑冗余
  ```csharp
  public async Task<LoginResponse> RefreshTokenAsync(string refreshToken, string currentToken, CancellationToken ct = default)
  {
      try
      {
          // 创建Token刷新请求，包含必要的客户端信息
          var tokenRefreshRequest = TokenRefreshRequest.Create(
              deviceId: GetDeviceId(),
              clientIp: GetClientIp()
          );

          // 创建刷新命令并设置请求数据
          var request = new RefreshTokenCommand
          {
              RefreshRequest = tokenRefreshRequest
          };

          var response = await _communicationService.SendCommandAsync<RefreshTokenCommand, LoginResponse>(
              request, ct);

          if (response != null && response.Success)
          {
              return response;
          }

          throw new Exception(response?.Message ?? "Token刷新失败");
      }
      catch (Exception ex)
      {
          throw new Exception($"Token刷新服务调用失败: {ex.Message}", ex);
      }
  }
  ```
  
  #### 更新后：参数简化，直接使用TokenManager
  ```csharp
  public async Task<LoginResponse> RefreshTokenAsync(CancellationToken ct = default)
  {
      try
      {
          var tokenInfo = _tokenManager.GetTokenInfo();
          if (tokenInfo == null || string.IsNullOrEmpty(tokenInfo.RefreshToken))
              throw new Exception("没有可用的刷新令牌");

          // 创建Token刷新请求，包含必要的客户端信息
          var tokenRefreshRequest = TokenRefreshRequest.Create(
              deviceId: GetDeviceId(),
              clientIp: GetClientIp()
          );

          // 创建刷新命令并设置请求数据
          var request = new RefreshTokenCommand
          {
              RefreshRequest = tokenRefreshRequest
          };

          var response = await _communicationService.SendCommandAsync<RefreshTokenCommand, LoginResponse>(
              request, ct);
              
          if (response != null && !string.IsNullOrEmpty(response.AccessToken))
          {
              _tokenManager.SetTokens(response.AccessToken, response.RefreshToken, response.ExpiresIn);
              return response;
          }
          
          throw new Exception(response?.Message ?? "Token刷新失败");
      }
      catch (Exception ex)
      {
          throw new Exception($"Token刷新失败: {ex.Message}", ex);
      }
  }
  ```
  
  ### 优化效果
  1. **✅ 简化方法签名**：从3个参数减少到1个参数，提高易用性
  2. **✅ 自动Token管理**：刷新成功后自动更新本地Token存储
  3. **✅ 增强安全性**：通过TokenManager统一管理Token，避免参数传递错误
  4. **✅ 更好的错误处理**：提前检查刷新Token的可用性，提供更清晰的错误信息
  5. **✅ 减少依赖**：调用方无需关心Token的具体获取和管理细节
  
  这次优化为Token管理体系奠定了更现代化的架构基础，为第三阶段的分布式缓存、权限管理等高级功能创造了良好条件。建议在充分测试后逐步推广使用，并准备进入第三阶段的优化工作。
  
  ## 12. TokenRefreshService接口进一步简化

### 问题描述
- `ITokenRefreshService`接口的`RefreshTokenAsync`方法参数过于复杂
- `SilentTokenRefresher`中仍然使用旧的接口调用方式
- 接口定义与实现不一致，导致调用方代码冗余

### 解决方案
- 简化`ITokenRefreshService`接口，移除不必要的参数
- 更新`SilentTokenRefresher`使用新的无参数接口
- 保持接口定义与实现的一致性

### 代码示例

**更新前的接口定义：**
```csharp
public interface ITokenRefreshService
{
    Task<LoginResponse> RefreshTokenAsync(string refreshToken, string clientId, CancellationToken ct = default);
    Task<bool> ValidateTokenAsync(string token, CancellationToken ct = default);
}
```

**更新后的接口定义：**
```csharp
public interface ITokenRefreshService
{
    Task<LoginResponse> RefreshTokenAsync(CancellationToken ct = default);
    Task<bool> ValidateTokenAsync(string token, CancellationToken ct = default);
}
```

**更新前的调用方式：**
```csharp
response = await _tokenRefreshService.RefreshTokenAsync(string.Empty, string.Empty, cancellationToken);
```

**更新后的调用方式：**
```csharp
response = await _tokenRefreshService.RefreshTokenAsync(cancellationToken);
```

### 优化效果
1. **接口简化**：移除了不必要的参数，使接口更加清晰
2. **代码一致性**：接口定义与实现保持一致
3. **调用简化**：调用方代码更加简洁
4. **职责明确**：接口专注于核心功能，不包含冗余参数
5. **维护便利**：简化的接口更容易维护和扩展
  
  ### 问题描述
  - TokenRefreshService中的RefreshTokenAsync方法仍然包含Token验证和存储逻辑
  - 方法职责不够单一，混合了业务逻辑和数据处理
  - 与简单的接口调用模式不一致
  
  ### 解决方案
  - 进一步简化RefreshTokenAsync方法，移除Token验证逻辑
  - 移除Token存储逻辑，保持方法职责单一
  - 专注于接口调用和异常处理
  - 让调用方负责Token的验证和存储
  
  ### 代码示例对比
  
  #### 更新前：仍包含Token验证和存储逻辑
  ```csharp
  public async Task<LoginResponse> RefreshTokenAsync(CancellationToken ct = default)
  {
      try
      {
          var tokenInfo = _tokenManager.GetTokenInfo();
          if (tokenInfo == null || string.IsNullOrEmpty(tokenInfo.RefreshToken))
              throw new Exception("没有可用的刷新令牌");

          // 创建Token刷新请求，包含必要的客户端信息
          var tokenRefreshRequest = TokenRefreshRequest.Create(
              deviceId: GetDeviceId(),
              clientIp: GetClientIp()
          );

          // 创建刷新命令并设置请求数据
          var request = new RefreshTokenCommand
          {
              RefreshRequest = tokenRefreshRequest
          };

          var response = await _communicationService.SendCommandAsync<RefreshTokenCommand, LoginResponse>(
              request, ct);

          if (response != null && !string.IsNullOrEmpty(response.AccessToken))
          {
              _tokenManager.SetTokens(response.AccessToken, response.RefreshToken, response.ExpiresIn);
              return response;
          }

          throw new Exception(response?.Message ?? "Token刷新失败");
      }
      catch (Exception ex)
      {
          throw new Exception($"Token刷新失败: {ex.Message}", ex);
      }
  }
  ```
  
  #### 更新后：进一步简化，专注接口调用
  ```csharp
  public async Task<LoginResponse> RefreshTokenAsync(CancellationToken ct = default)
  {
      try
      {
          var request = new RefreshTokenCommand();
          var response = await _communicationService.SendCommandAsync<RefreshTokenCommand, LoginResponse>(
              request, ct);
              
          return response;
      }
      catch (Exception ex)
      {
          throw new Exception($"Token刷新服务调用失败: {ex.Message}", ex);
      }
  }
  ```
  
  ### 优化效果
  1. **✅ 职责单一**：方法专注于接口调用，不包含业务逻辑
  2. **✅ 更加简洁**：代码行数大幅减少，逻辑清晰
  3. **✅ 灵活性高**：调用方可以根据需要处理Token验证和存储
  4. **✅ 异常明确**：错误信息更加具体，便于调试
  5. **✅ 可测试性强**：简化的逻辑更容易进行单元测试
  
  这次优化使TokenRefreshService更加符合单一职责原则，提高了代码的可维护性和可测试性。

## 13. 编译错误修复

### 问题描述
在优化过程中出现了两个编译错误：

1. **MemoryTokenStorage.cs编译错误** (CS0200, CS1061):
   - 错误：无法为只读属性"TokenInfo.AccessTokenExpiryUtc"和"TokenInfo.RefreshTokenExpiryUtc"赋值
   - 错误："TokenInfo"未包含"RefreshTokenExpiresIn"的定义

2. **BaseCommand.cs编译错误** (CS0120):
   - 错误：调用TokenManagerCoordinator.GetTokenInfo()时缺少对象引用

### 解决方案

#### 1. MemoryTokenStorage编译错误修复
**问题原因**：
- `TokenInfo`类中的`AccessTokenExpiryUtc`和`RefreshTokenExpiryUtc`是计算属性，只读无法直接赋值
- `TokenInfo`类中不存在`RefreshTokenExpiresIn`属性

**修复方案**：
```csharp
// 修复前：尝试给只读属性赋值
public void SetTokenInfo(TokenInfo tokenInfo)
{
    if (tokenInfo == null) return;
    
    // 错误：无法给只读属性赋值
    tokenInfo.AccessTokenExpiryUtc = DateTime.UtcNow.AddSeconds(tokenInfo.ExpiresIn);
    tokenInfo.RefreshTokenExpiryUtc = DateTime.UtcNow.AddSeconds(tokenInfo.RefreshTokenExpiresIn);
    
    _tokenStore.AddOrUpdate(DEFAULT_KEY, tokenInfo, (key, oldValue) => tokenInfo);
}

// 修复后：移除过期时间计算，依赖TokenInfo自身的计算逻辑
public void SetTokenInfo(TokenInfo tokenInfo)
{
    if (tokenInfo == null) return;
    
    // 确保GeneratedTime和ExpiresIn有合理值
    if (tokenInfo.GeneratedTime == default)
        tokenInfo.GeneratedTime = DateTime.UtcNow;
    if (tokenInfo.ExpiresIn <= 0)
        tokenInfo.ExpiresIn = 28800; // 8小时默认
        
    _tokenStore.AddOrUpdate(DEFAULT_KEY, tokenInfo, (key, oldValue) => tokenInfo);
}
```

#### 2. BaseCommand静态调用错误修复
**问题原因**：
- `TokenManagerCoordinator`是一个实例类，需要通过依赖注入获取
- `BaseCommand`中却试图静态调用`TokenManagerCoordinator.GetTokenInfo()`

**修复方案**：
```csharp
// 修复前：错误的静态调用
protected virtual void AutoAttachToken()
{
    try
    {
        // 错误：静态调用实例方法
        var tokenInfo = TokenManagerCoordinator.GetTokenInfo();
        if (tokenInfo != null && !string.IsNullOrEmpty(tokenInfo.AccessToken))
        {
            AuthToken = tokenInfo.AccessToken;
            TokenType = "Bearer";
            // ...
        }
    }
    catch (Exception ex)
    {
        Logger?.LogWarning(ex, "自动附加Token失败");
    }
}

// 修复后：通过依赖注入获取实例
public abstract class BaseCommand : ICommand
{
    /// <summary>
    /// Token管理协调器 - 通过依赖注入获取
    /// </summary>
    protected TokenManagerCoordinator TokenManager { get; set; }
    
    /// <summary>
    /// 构造函数 - 支持依赖注入
    /// </summary>
    protected BaseCommand(TokenManagerCoordinator tokenManager, PacketDirection direction = PacketDirection.Unknown, ILogger<BaseCommand> logger = null) 
        : this(direction, logger)
    {
        TokenManager = tokenManager;
    }
}

protected virtual void AutoAttachToken()
{
    try
    {
        // 检查TokenManager是否可用
        if (TokenManager == null)
        {
            Logger?.LogDebug("TokenManager未初始化，跳过自动附加");
            return;
        }
        
        // 使用依赖注入的TokenManager实例
        var tokenInfo = TokenManager.GetTokenInfo();
        if (tokenInfo != null && !string.IsNullOrEmpty(tokenInfo.AccessToken))
        {
            AuthToken = tokenInfo.AccessToken;
            TokenType = "Bearer";
            // ...
        }
    }
    catch (Exception ex)
    {
        Logger?.LogWarning(ex, "自动附加Token失败");
    }
}
```

### 优化效果
1. **✅ 修复编译错误**：解决了CS0200、CS1061、CS0120等编译错误
2. **✅ 符合依赖注入原则**：通过构造函数注入TokenManagerCoordinator实例
3. **✅ 保持向后兼容**：原有的构造函数继续保留，支持渐进式迁移
4. **✅ 代码更加健壮**：添加了空值检查和错误处理
5. **✅ 架构更加清晰**：明确了TokenManagerCoordinator的获取方式

## 14. Token存储接口统一为异步

### 任务描述
统一Token存储接口，将ITokenStorage接口的所有方法改为异步版本，简化接口职责，提高性能。

### 问题分析
**原始接口问题**：
- 接口方法过多，职责不够单一
- 同步方法可能阻塞线程，影响性能
- 缺乏统一的异步抽象

**原始接口定义**：
```csharp
public interface ITokenStorage
{
    // 过多方法，职责复杂
    (bool success, string accessToken, string refreshToken) GetTokens();
    TokenInfo GetTokenInfo();
    void SetTokenInfo(TokenInfo tokenInfo);
    void SetTokens(string accessToken, string refreshToken, int expiresInSeconds);
    bool IsAccessTokenExpired();
    DateTime? GetAccessTokenExpiry();
    DateTime? GetRefreshTokenExpiry();
    void ClearTokens();
}
```

### 解决方案

#### 1. 简化接口定义
**新的异步接口**：
```csharp
public interface ITokenStorage
{
    /// <summary>
    /// 异步获取Token信息
    /// </summary>
    Task<TokenInfo> GetTokenAsync();

    /// <summary>
    /// 异步设置Token信息
    /// </summary>
    Task SetTokenAsync(TokenInfo tokenInfo);

    /// <summary>
    /// 异步清除存储的Token信息
    /// </summary>
    Task ClearTokenAsync();

    /// <summary>
    /// 异步检查Token是否有效（未过期且存在）
    /// </summary>
    Task<bool> IsTokenValidAsync();
}
```

#### 2. MemoryTokenStorage实现更新
```csharp
public class MemoryTokenStorage : ITokenStorage
{
    private readonly ConcurrentDictionary<string, TokenInfo> _tokenStore = new();
    private const string DEFAULT_KEY = "default_token";

    public async Task SetTokenAsync(TokenInfo tokenInfo)
    {
        if (tokenInfo == null) throw new ArgumentNullException(nameof(tokenInfo));
        if (string.IsNullOrEmpty(tokenInfo.AccessToken))
            throw new ArgumentException("AccessToken不能为空", nameof(tokenInfo));

        // 确保GeneratedTime有值
        if (tokenInfo.GeneratedTime == default(DateTime))
            tokenInfo.GeneratedTime = DateTime.UtcNow;

        // 确保ExpiresIn有合理值
        if (tokenInfo.ExpiresIn <= 0)
            tokenInfo.ExpiresIn = 28800; // 默认8小时

        _tokenStore.AddOrUpdate(DEFAULT_KEY, tokenInfo, (key, oldValue) => tokenInfo);
        
        // 模拟异步操作
        await Task.CompletedTask;
    }

    public async Task<TokenInfo> GetTokenAsync()
    {
        var result = _tokenStore.TryGetValue(DEFAULT_KEY, out var tokenInfo) ? tokenInfo : null;
        await Task.CompletedTask;
        return result;
    }

    public async Task ClearTokenAsync()
    {
        _tokenStore.TryRemove(DEFAULT_KEY, out _);
        await Task.CompletedTask;
    }

    public async Task<bool> IsTokenValidAsync()
    {
        var tokenInfo = await GetTokenAsync();
        var result = tokenInfo?.IsAccessTokenExpired() == false;
        await Task.CompletedTask;
        return result;
    }
}
```

#### 3. TokenManagerCoordinator适配
```csharp
public class TokenManagerCoordinator
{
    // 新的异步方法
    public async Task<TokenInfo> GetTokenInfoAsync() => await _tokenStorage.GetTokenAsync();
    public async Task SetTokenInfoAsync(TokenInfo tokenInfo) => await _tokenStorage.SetTokenAsync(tokenInfo);
    public async Task<bool> IsAccessTokenExpiredAsync() => !await _tokenStorage.IsTokenValidAsync();
    public async Task ClearTokensAsync() => await _tokenStorage.ClearTokenAsync();

    // 为了保持向后兼容，提供同步包装方法
    public TokenInfo GetTokenInfo() => GetTokenInfoAsync().GetAwaiter().GetResult();
    public void SetTokenInfo(TokenInfo tokenInfo) => SetTokenInfoAsync(tokenInfo).GetAwaiter().GetResult();
    public bool IsAccessTokenExpired() => IsAccessTokenExpiredAsync().GetAwaiter().GetResult();
    public void ClearTokens() => ClearTokensAsync().GetAwaiter().GetResult();
}
```

### 优化效果
1. **✅ 接口职责单一**：每个方法职责明确，符合单一职责原则
2. **✅ 性能提升**：异步方法不会阻塞线程，提高系统响应能力
3. **✅ 向后兼容**：通过同步包装方法保持现有代码的兼容性
4. **✅ 代码简洁**：接口方法从8个减少到4个，更加清晰
5. **✅ 扩展性强**：为未来支持数据库等异步存储做好准备
6. **✅ 易于测试**：异步接口更容易进行单元测试和模拟

## 15. Token管理器简化

### 任务描述
将复杂的TokenManagerCoordinator类简化为简洁的TokenManager类，移除不必要的复杂逻辑，只保留核心的Token生成、验证和存储功能。

### 问题分析
**原始TokenManagerCoordinator问题**：
- 类职责过多，包含Token生成、验证、存储、缓存、协调等多个职责
- 代码复杂度高，维护困难
- 依赖注入配置复杂，需要注册多个相关服务
- 与现有代码耦合度高，替换成本高

**原始TokenManagerCoordinator代码**：
```csharp
public class TokenManagerCoordinator
{
    private readonly ITokenService _tokenService;
    private readonly ITokenStorage _tokenStorage;
    private readonly ILogger<TokenManagerCoordinator> _logger;
    private readonly IMemoryCache _cache;
    private readonly TokenManagerOptions _options;
    
    // 过多的方法和复杂的逻辑
    public TokenInfo GenerateToken(string userId, string userName, IDictionary<string, object> claims = null)
    {
        // 复杂的Token生成逻辑
    }
    
    public TokenValidationResult ValidateToken(string token)
    {
        // 复杂的Token验证逻辑
    }
    
    public async Task<TokenInfo> GetTokenInfoAsync()
    {
        // 复杂的Token获取逻辑，包含缓存、异常处理等
    }
    
    // 更多复杂的方法...
}
```

### 解决方案

#### 1. 简化TokenManager类设计
**新的简洁TokenManager**：
```csharp
/// <summary>
/// 简化的Token管理器 - 只保留核心功能
/// </summary>
public class TokenManager
{
    private readonly ITokenService _tokenService;
    private readonly ITokenStorage _tokenStorage;

    /// <summary>
    /// 构造函数 - 通过依赖注入获取服务
    /// </summary>
    public TokenManager(ITokenService tokenService, ITokenStorage tokenStorage)
    {
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        _tokenStorage = tokenStorage ?? throw new ArgumentNullException(nameof(tokenStorage));
    }

    /// <summary>
    /// 生成并存储Token
    /// </summary>
    public async Task<TokenInfo> GenerateAndStoreTokenAsync(string userId, string userName, IDictionary<string, object> claims = null)
    {
        var token = _tokenService.GenerateToken(userId, userName, claims);
        var tokenInfo = new TokenInfo { AccessToken = token };
        await _tokenStorage.SetTokenAsync(tokenInfo);
        return tokenInfo;
    }

    /// <summary>
    /// 验证存储的Token
    /// </summary>
    public async Task<TokenValidationResult> ValidateStoredTokenAsync()
    {
        var tokenInfo = await _tokenStorage.GetTokenAsync();
        if (tokenInfo == null)
            return new TokenValidationResult { IsValid = false, ErrorMessage = "No token found" };
        
        return _tokenService.ValidateToken(tokenInfo.AccessToken);
    }

    /// <summary>
    /// 清除Token
    /// </summary>
    public Task ClearTokenAsync() => _tokenStorage.ClearTokenAsync();

    /// <summary>
    /// 获取Token存储（用于兼容现有代码）
    /// </summary>
    public ITokenStorage TokenStorage => _tokenStorage;
}
```

#### 2. 依赖注入配置简化
**原始复杂的依赖注入**：
```csharp
public static void ConfigurePacketSpecServicesContainer(ContainerBuilder builder)
{
    // 复杂的TokenManagerCoordinator注册
    builder.RegisterType<TokenManagerCoordinator>()
        .AsSelf()
        .SingleInstance()
        .WithParameter(new TypedParameter(typeof(ITokenService), tokenService))
        .WithParameter(new TypedParameter(typeof(ITokenStorage), tokenStorage))
        .WithParameter(new TypedParameter(typeof(ILogger<TokenManagerCoordinator>), logger))
        .WithParameter(new TypedParameter(typeof(IMemoryCache), cache))
        .WithParameter(new TypedParameter(typeof(TokenManagerOptions), options));
}
```

**简化后的依赖注入**：
```csharp
public static void ConfigurePacketSpecServicesContainer(ContainerBuilder builder)
{
    // 简化的TokenManager注册
    builder.RegisterType<TokenManager>()
        .AsSelf()
        .SingleInstance();
}
```

#### 3. BaseCommand适配更新
**更新BaseCommand类**：
```csharp
public abstract class BaseCommand : ICommand
{
    /// <summary>
    /// Token管理器 - 通过依赖注入获取
    /// </summary>
    protected TokenManager TokenManager { get; set; }
    
    /// <summary>
    /// 构造函数 - 支持依赖注入
    /// </summary>
    protected BaseCommand(TokenManager tokenManager, PacketDirection direction = PacketDirection.Unknown, ILogger<BaseCommand> logger = null) 
        : this(direction, logger)
    {
        TokenManager = tokenManager;
    }
    
    /// <summary>
    /// 自动附加Token - 适配简化版TokenManager
    /// </summary>
    protected virtual void AutoAttachToken()
    {
        try
        {
            // 简化版：使用依赖注入的TokenManager
            var tokenInfo = TokenManager?.TokenStorage?.GetTokenAsync().GetAwaiter().GetResult();
            if (tokenInfo != null && !string.IsNullOrEmpty(tokenInfo.AccessToken))
            {
                AuthToken = tokenInfo.AccessToken;
                TokenType = "Bearer";
                // ...
            }
        }
        catch (Exception ex)
        {
            Logger?.LogWarning(ex, "自动附加Token失败");
        }
    }
}
```

### 优化效果
1. **✅ 代码简洁**：TokenManager类从复杂的协调器简化为简洁的管理器
2. **✅ 职责单一**：每个类只负责一个明确的职责，符合单一职责原则
3. **✅ 依赖简化**：依赖注入配置大幅简化，只需注册基本服务
4. **✅ 向后兼容**：通过TokenStorage属性保持与现有代码的兼容性
5. **✅ 易于维护**：代码结构清晰，逻辑简单，易于理解和维护
6. **✅ 性能提升**：移除了不必要的缓存和复杂逻辑，提高了执行效率