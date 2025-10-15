# ResponseBase<TEntity> 设计文档和使用指南

## 概述

ResponseBase<TEntity> 是RUINORERP系统中专门用于承载业务实体数据的响应基类，它继承自ResponseBase，提供了类型安全的数据传输机制，支持复杂的业务场景和网络通讯需求。

## 设计必要性

### 1. 类型安全的数据传输
```csharp
// ✅ 推荐：类型安全，编译时检查
public ResponseBase<User> GetUser(long userId)
{
    var user = _userService.GetById(userId);
    return ResponseBase<User>.CreateSuccess(user, "用户查询成功");
}

// ❌ 不推荐：运行时才能发现类型错误
public ResponseBase GetUserUnsafe(long userId)
{
    var user = _userService.GetById(userId);
    return ResponseBase.CreateSuccess("用户查询成功"); // 数据丢失
}
```

### 2. 统一的业务数据格式
```csharp
// 统一的响应格式，便于前端处理
{
    "isSuccess": true,
    "message": "用户查询成功",
    "code": 200,
    "data": { /* 具体的用户数据 */ },
    "totalCount": 100,
    "dataVersion": "v1.0",
    "extraData": { /* 额外的业务数据 */ }
}
```

### 3. 网络传输优化
- 支持MessagePack高效序列化
- 兼容字节数组传输机制
- 支持数据压缩和缓存

## 核心功能

### 1. 基本CRUD操作响应

```csharp
// 创建用户
public ResponseBase<User> CreateUser(User user)
{
    try
    {
        var createdUser = _userRepository.Insert(user);
        return ResponseBase<User>.CreateSuccess(createdUser, "用户创建成功");
    }
    catch (Exception ex)
    {
        return ResponseBase<User>.CreateError($"用户创建失败：{ex.Message}");
    }
}

// 更新用户
public ResponseBase<User> UpdateUser(long userId, User user)
{
    try
    {
        var existingUser = _userRepository.GetById(userId);
        if (existingUser == null)
        {
            return ResponseBase<User>.CreateError("用户不存在", 404);
        }
        
        // 更新用户属性
        existingUser.Name = user.Name;
        existingUser.Email = user.Email;
        
        var updatedUser = _userRepository.Update(existingUser);
        return ResponseBase<User>.CreateSuccess(updatedUser, "用户更新成功");
    }
    catch (Exception ex)
    {
        return ResponseBase<User>.CreateError($"用户更新失败：{ex.Message}");
    }
}

// 删除用户
public ResponseBase<bool> DeleteUser(long userId)
{
    try
    {
        var result = _userRepository.Delete(userId);
        if (result)
        {
            return ResponseBase<bool>.CreateSuccess(true, "用户删除成功");
        }
        return ResponseBase<bool>.CreateError("用户删除失败", 400);
    }
    catch (Exception ex)
    {
        return ResponseBase<bool>.CreateError($"用户删除失败：{ex.Message}");
    }
}
```

### 2. 分页查询响应

```csharp
// 使用PagedResponse进行分页查询
public PagedResponse<User> GetUsersPaged(int pageIndex, int pageSize, string searchKeyword = "")
{
    try
    {
        var query = _userRepository.GetQueryable();
        
        if (!string.IsNullOrEmpty(searchKeyword))
        {
            query = query.Where(u => u.Name.Contains(searchKeyword) || u.Email.Contains(searchKeyword));
        }
        
        var totalCount = query.Count();
        var users = query.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        
        return PagedResponse<User>.CreateSuccess(users, totalCount, pageIndex, pageSize, "分页查询成功");
    }
    catch (Exception ex)
    {
        return PagedResponse<User>.CreateError($"分页查询失败：{ex.Message}");
    }
}
```

### 3. 复杂业务场景

```csharp
// 用户登录响应，包含额外数据
public ResponseBase<User> Login(string username, string password)
{
    try
    {
        var user = _userRepository.GetByUsername(username);
        if (user == null)
        {
            return ResponseBase<User>.CreateError("用户名或密码错误", 401);
        }
        
        if (!_passwordHasher.VerifyPassword(password, user.PasswordHash))
        {
            return ResponseBase<User>.CreateError("用户名或密码错误", 401);
        }
        
        // 生成token
        var token = _tokenService.GenerateToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken(user);
        
        // 获取用户角色和权限
        var roles = _roleService.GetUserRoles(user.Id);
        var permissions = _permissionService.GetUserPermissions(user.Id);
        
        var response = ResponseBase<User>.CreateSuccess(user, "登录成功");
        
        // 添加额外的业务数据
        response.WithMetadata("Token", token)
                .WithMetadata("RefreshToken", refreshToken)
                .WithMetadata("Roles", roles)
                .WithMetadata("Permissions", permissions)
                .WithMetadata("LoginTime", DateTime.Now);
                
        return response;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "用户登录失败");
        return ResponseBase<User>.CreateError("登录失败，请稍后重试");
    }
}
```

## 网络通讯集成

### 1. 客户端发送请求

```csharp
// 使用ClientCommunicationService发送实体请求
public async Task<ResponseBase<User>> GetUserAsync(long userId)
{
    var request = EntityRequest<User>.CreateGetRequest(userId.ToString());
    
    return await _communicationService.SendCommandAsync<
        EntityRequest<User>, 
        ResponseBase<User>
    >("GetUserCommand", request);
}

// 分页查询
public async Task<PagedResponse<User>> GetUsersPagedAsync(int pageIndex, int pageSize)
{
    var pagedRequest = PagedRequest.Create(pageIndex, pageSize);
    
    return await _communicationService.SendCommandAsync<
        PagedRequest, 
        PagedResponse<User>
    >("GetUsersPagedCommand", pagedRequest);
}
```

### 2. 服务器端处理

```csharp
// 服务器端命令处理器
public class GetUserCommandHandler : CommandHandlerBase<EntityRequest<User>, ResponseBase<User>>
{
    private readonly IUserService _userService;
    
    public override async Task<ResponseBase<User>> HandleAsync(EntityRequest<User> request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.EntityId))
            {
                return ResponseBase<User>.CreateError("用户ID不能为空", 400);
            }
            
            if (!long.TryParse(request.EntityId, out long userId))
            {
                return ResponseBase<User>.CreateError("用户ID格式错误", 400);
            }
            
            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
            {
                return ResponseBase<User>.CreateError("用户不存在", 404);
            }
            
            return ResponseBase<User>.CreateSuccess(user, "用户查询成功");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "查询用户失败");
            return ResponseBase<User>.CreateError($"查询失败：{ex.Message}");
        }
    }
}
```

## 数据版本控制和乐观锁

```csharp
// 更新操作时使用数据版本控制
public ResponseBase<User> UpdateUserWithVersion(long userId, User user, string dataVersion)
{
    try
    {
        var existingUser = _userRepository.GetById(userId);
        if (existingUser == null)
        {
            return ResponseBase<User>.CreateError("用户不存在", 404);
        }
        
        // 检查数据版本
        var currentVersion = _userRepository.GetDataVersion(userId);
        if (currentVersion != dataVersion)
        {
            return ResponseBase<User>.CreateError("数据已被其他用户修改，请刷新后重试", 409)
                .WithMetadata("CurrentVersion", currentVersion)
                .WithMetadata("YourVersion", dataVersion);
        }
        
        // 更新用户数据
        existingUser.Name = user.Name;
        existingUser.Email = user.Email;
        
        var updatedUser = _userRepository.Update(existingUser);
        var newVersion = _userRepository.GetDataVersion(userId);
        
        return ResponseBase<User>.CreateSuccess(
            updatedUser, 
            "用户更新成功", 
            dataVersion: newVersion
        );
    }
    catch (Exception ex)
    {
        return ResponseBase<User>.CreateError($"用户更新失败：{ex.Message}");
    }
}
```

## 批量操作支持

```csharp
// 批量创建用户
public async Task<ResponseBase<List<User>>> BatchCreateUsersAsync(List<User> users)
{
    try
    {
        var batchRequest = BatchEntityRequest<User>.CreateBatchCreateRequest(users);
        
        return await _communicationService.SendCommandAsync<
            BatchEntityRequest<User>, 
            ResponseBase<List<User>>
        >("BatchCreateUsersCommand", batchRequest);
    }
    catch (Exception ex)
    {
        return ResponseBase<List<User>>.CreateError($"批量创建失败：{ex.Message}");
    }
}
```

## 错误处理和日志记录

```csharp
// 统一的错误处理
public ResponseBase<TEntity> HandleBusinessException<TEntity>(Exception ex, string operation) 
    where TEntity : class
{
    _logger.LogError(ex, $"{operation}失败");
    
    var errorData = new Dictionary<string, object>
    {
        ["Operation"] = operation,
        ["ExceptionType"] = ex.GetType().Name,
        ["StackTrace"] = ex.StackTrace,
        ["Timestamp"] = DateTime.Now
    };
    
    return ResponseBase<TEntity>.CreateError(
        $"{operation}失败：{ex.Message}", 
        500, 
        errorData
    );
}
```

## 性能优化建议

### 1. 缓存机制
```csharp
// 对频繁查询的数据使用缓存
public ResponseBase<User> GetCachedUser(long userId)
{
    var cacheKey = $"user_{userId}";
    
    if (_cache.TryGetValue(cacheKey, out User cachedUser))
    {
        return ResponseBase<User>.CreateSuccess(cachedUser, "从缓存获取用户成功");
    }
    
    var user = _userRepository.GetById(userId);
    if (user != null)
    {
        _cache.Set(cacheKey, user, TimeSpan.FromMinutes(5));
        return ResponseBase<User>.CreateSuccess(user, "用户查询成功");
    }
    
    return ResponseBase<User>.CreateError("用户不存在", 404);
}
```

### 2. 异步处理
```csharp
// 使用异步方法提高性能
public async Task<ResponseBase<User>> GetUserAsync(long userId)
{
    try
    {
        var user = await _userRepository.GetByIdAsync(userId);
        return ResponseBase<User>.CreateSuccess(user, "用户查询成功");
    }
    catch (Exception ex)
    {
        return ResponseBase<User>.CreateError($"查询失败：{ex.Message}");
    }
}
```

## 总结

ResponseBase<TEntity>的设计是项目架构中非常重要的一环，它提供了：

1. **类型安全**：编译时类型检查，避免运行时错误
2. **统一格式**：前后端统一的响应格式，便于处理
3. **业务适配**：支持CRUD、分页、批量操作等各种业务场景
4. **网络优化**：支持高效序列化和传输
5. **扩展性**：支持数据版本控制、缓存、乐观锁等高级功能

结合项目的网络通讯架构，ResponseBase<TEntity>能够很好地支持字节数组传输、JSON序列化、MessagePack高效序列化等多种传输方式，是企业级ERP系统的理想选择。