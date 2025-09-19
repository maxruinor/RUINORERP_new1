# ApiResponse 统一响应模型使用指南

## 📖 概述

`ApiResponse<T>` 是 RUINORERP 项目的统一API响应模型，用于标准化所有接口的响应格式。它替代了原有的 `BaseResponse` 和各种特定响应类，提供一致的成功/失败响应结构，便于前后端协作和错误处理。

## 🏗️ 核心架构

### 类定义位置
```csharp
// 文件位置：RUINORERP.PacketSpec/Models/Responses/ApiResponse.cs
namespace RUINORERP.PacketSpec.Models.Responses
{
    [Serializable]
    public class ApiResponse<T>
    {
        // 核心属性...
    }
    
    [Serializable] 
    public class ApiResponse : ApiResponse<object>
    {
        // 无数据版本...
    }
}
```

### 核心属性
| 属性 | 类型 | 描述 | 示例 |
|------|------|------|------|
| `Success` | `bool` | 操作是否成功 | `true` |
| `Message` | `string` | 响应消息 | `"操作成功"` |
| `Data` | `T` | 响应数据 | `User` 对象 |
| `Timestamp` | `DateTime` | 响应时间戳(UTC) | `2025-09-17T11:30:45.123Z` |
| `Code` | `int` | HTTP状态码 | `200` |
| `RequestId` | `string` | 请求追踪标识 | `"req_abc123"` |

## 🚀 快速开始

### 基本用法示例
```csharp
// 1. 创建成功响应（带数据）
var successResponse = ApiResponse<UserDto>.CreateSuccess(userData, "用户获取成功");

// 2. 创建成功响应（无数据）
var simpleSuccess = ApiResponse.CreateSuccess("操作成功");

// 3. 创建失败响应
var errorResponse = ApiResponse<object>.Failure("操作失败", 500);

// 4. 特定错误类型
var unauthorized = ApiResponse<object>.Unauthorized("请先登录");
var notFound = ApiResponse<object>.NotFound("资源不存在");
```

### 链式配置
```csharp
var response = ApiResponse<UserProfile>
    .CreateSuccess(userProfile, "用户信息获取成功")
    .WithRequestId(Guid.NewGuid().ToString())  // 设置请求ID
    .WithCode(200);                           // 设置状态码
```

## 🎯 实际应用场景

### 在控制器中使用
```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    [HttpGet("{id}")]
    public async Task<ApiResponse<UserDto>> GetUser(string id)
    {
        try
        {
            var user = await _userService.GetUserAsync(id);
            if (user == null)
                return ApiResponse<UserDto>.NotFound("用户不存在");
                
            return ApiResponse<UserDto>.CreateSuccess(user, "用户信息获取成功");
        }
        catch (Exception ex)
        {
            return ApiResponse<UserDto>.Failure($"系统错误: {ex.Message}", 500);
        }
    }

    [HttpPost]
    public async Task<ApiResponse> CreateUser([FromBody] CreateUserRequest request)
    {
        // 参数验证
        if (!ModelState.IsValid)
            return ApiResponse.ValidationFailed("参数验证失败");
        
        var result = await _userService.CreateUserAsync(request);
        return result 
            ? ApiResponse.CreateSuccess("用户创建成功")
            : ApiResponse.Failure("用户创建失败");
    }
}
```

### 在服务层中使用
```csharp
public class UserService : IUserService
{
    public async Task<ApiResponse<bool>> UpdateUserAsync(UpdateUserRequest request)
    {
        // 业务逻辑验证
        if (request.UserName.Length < 3)
            return ApiResponse<bool>.ValidationFailed("用户名至少3个字符");
        
        // 执行更新操作
        var success = await _repository.UpdateAsync(request);
        
        return success
            ? ApiResponse<bool>.CreateSuccess(true, "用户更新成功")
            : ApiResponse<bool>.Failure("用户更新失败");
    }
}
```

## 📊 响应格式规范

### 成功响应示例
```json
{
  "success": true,
  "message": "用户信息获取成功",
  "data": {
    "id": "12345",
    "username": "zhangsan",
    "email": "zhangsan@example.com",
    "roles": ["admin", "user"],
    "createdAt": "2025-01-15T08:00:00Z"
  },
  "timestamp": "2025-09-17T11:30:45.123Z",
  "code": 200,
  "requestId": "req_a1b2c3d4e5f6"
}
```

### 失败响应示例
```json
{
  "success": false,
  "message": "用户不存在",
  "data": null,
  "timestamp": "2025-09-17T11:30:45.123Z",
  "code": 404,
  "requestId": "req_a1b2c3d4e5f6"
}
```

## 🔧 HTTP状态码规范

| 状态码 | 含义 | 使用场景 | 示例方法 |
|--------|------|----------|----------|
| 200 | 成功 | 正常操作成功 | `CreateSuccess` |
| 201 | 创建成功 | 资源创建成功 | `CreateSuccess` + 设置201 |
| 400 | 请求错误 | 参数验证失败 | `ValidationFailed` |
| 401 | 未授权 | 需要登录认证 | `Unauthorized` |
| 403 | 禁止访问 | 权限不足 | `Failure("无权限", 403)` |
| 404 | 未找到 | 资源不存在 | `NotFound` |
| 500 | 服务器错误 | 系统内部错误 | `Failure` |
| 503 | 服务不可用 | 系统维护中 | `Failure("服务维护中", 503)` |

## 💡 最佳实践指南

### 1. 统一响应格式
所有API接口必须使用 `ApiResponse<T>` 作为返回类型，保持前后端约定的一致性。

### 2. 错误处理规范
```csharp
// 使用预定义的错误类型
return ApiResponse<object>.Unauthorized("Token已过期，请重新登录");
return ApiResponse<object>.NotFound($"订单 {orderId} 不存在");

// 自定义业务错误
return ApiResponse<object>.Failure("库存不足，无法下单", 400);
```

### 3. 请求追踪配置
```csharp
// 在ASP.NET Core中设置请求ID
public async Task<ApiResponse<User>> GetUser(string id)
{
    var response = ApiResponse<User>.CreateSuccess(user, "成功");
    response.WithRequestId(HttpContext.TraceIdentifier);
    return response;
}
```

### 4. 时间戳处理
- 所有时间戳使用UTC时间
- 客户端负责时区转换
- 便于日志分析和调试

### 5. 数据序列化
```csharp
// 转换为JSON字符串
string json = response.ToJson(Formatting.Indented);

// 从JSON反序列化
var response = ApiResponse<User>.FromJson(jsonString);

// 验证响应有效性
if (response.IsValid())
{
    // 处理有效响应
}
```

## 🛠️ 高级功能

### 分页数据响应
```csharp
public class PagedResult<T>
{
    public List<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
}

// 使用示例
var pagedData = new PagedResult<UserDto>
{
    Items = users,
    TotalCount = totalCount,
    PageSize = pageSize,
    CurrentPage = currentPage
};

return ApiResponse<PagedResult<UserDto>>.CreateSuccess(pagedData, "分页数据获取成功");
```

### 批量操作响应
```csharp
public class BatchResult
{
    public int SuccessCount { get; set; }
    public int FailureCount { get; set; }
    public List<string> Errors { get; set; }
}

// 使用示例
var batchResult = new BatchResult
{
    SuccessCount = successCount,
    FailureCount = failureCount,
    Errors = errorMessages
};

return ApiResponse<BatchResult>.CreateSuccess(batchResult, "批量操作完成");
```

## 🔄 迁移指南

### 从旧响应类迁移
```csharp
// 旧代码（BaseResponse）
return new BaseResponse 
{ 
    Success = true, 
    Message = "成功", 
    Data = result,
    Code = 200
};

// 新代码（ApiResponse）
return ApiResponse<ResultType>.CreateSuccess(result, "成功")
    .WithCode(200);
```

### 处理集合数据迁移
```csharp
// 旧代码返回列表
return new ListResponse<User> { Data = users, Success = true };

// 新代码
return ApiResponse<List<User>>.CreateSuccess(users, "用户列表获取成功");
```

### 处理空数据迁移
```csharp
// 旧代码
return new SimpleResponse { Success = true, Message = "成功" };

// 新代码  
return ApiResponse.CreateSuccess("成功");
```

## 📝 注意事项

### 1. 泛型参数使用
```csharp
// 正确：即使返回空数据也要指定具体类型
return ApiResponse<object>.Failure("错误");

// 错误：不要使用非泛型版本返回数据
return ApiResponse.Failure("错误"); // 这是无数据版本
```

### 2. 时间戳一致性
- 所有响应时间戳自动设置为UTC时间
- 避免手动设置时间戳
- 客户端应正确处理时区转换

### 3. 错误消息规范
- 错误消息应清晰明确，便于前端显示给用户
- 避免暴露系统内部错误信息
- 生产环境应使用友好的错误消息

### 4. 性能考虑
- 响应数据不宜过大
- 避免在响应中包含敏感信息
- 使用分页处理大量数据

## 🎪 扩展建议

### 自定义响应扩展
```csharp
public static class ApiResponseExtensions
{
    public static ApiResponse<T> WithPagination<T>(this ApiResponse<T> response, 
        int page, int pageSize, long totalCount)
    {
        // 添加分页元数据
        return response;
    }
    
    public static ApiResponse<T> WithMetadata<T>(this ApiResponse<T> response, 
        object metadata)
    {
        // 添加自定义元数据
        return response;
    }
}
```

### 全局异常处理
```csharp
// 在Startup.cs中配置全局异常处理
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>();
        var response = ApiResponse<object>.Failure("系统内部错误", 500);
        await context.Response.WriteAsJsonAsync(response);
    });
});
```

### 响应压缩
```csharp
// 在Program.cs中配置响应压缩
services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<GzipCompressionProvider>();
});
```

## 🔍 调试技巧

### 1. 日志记录
```csharp
// 记录响应信息
_logger.LogInformation("API响应: {Response}", response.ToJson());

// 记录请求ID便于追踪
_logger.LogInformation("请求ID: {RequestId}", response.RequestId);
```

### 2. 单元测试
```csharp
[Test]
public void Test_SuccessResponse()
{
    //  Arrange
    var user = new User { Id = 1, Name = "Test" };
    
    // Act
    var response = ApiResponse<User>.CreateSuccess(user, "成功");
    
    // Assert
    Assert.IsTrue(response.Success);
    Assert.AreEqual("成功", response.Message);
    Assert.AreEqual(200, response.Code);
}
```

### 3. 性能监控
```csharp
// 监控响应时间
var stopwatch = Stopwatch.StartNew();
var response = await SomeOperation();
stopwatch.Stop();

_logger.LogInformation("操作耗时: {ElapsedMs}ms", stopwatch.ElapsedMilliseconds);
```

---

**文档版本**: 2.0  
**最后更新**: 2025-09-17  
**适用项目**: RUINORERP  
**技术栈**: .NET 6.0+, ASP.NET Core, SuperSocket

> 💡 提示：本文档应随项目代码一起维护，任何对ApiResponse的修改都应同步更新此文档。