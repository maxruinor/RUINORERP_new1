# 统一请求响应处理框架

## 概述

统一请求响应处理框架是一个通用的请求处理架构，旨在为服务器端和客户端提供一致的请求处理方式。该框架基于以下核心概念：

1. **请求处理器模式**：定义了标准的请求处理接口和基类
2. **类型安全**：使用泛型确保请求和响应类型的正确性
3. **异步处理**：所有操作都支持异步处理
4. **统一错误处理**：提供标准的错误响应格式
5. **可扩展性**：易于扩展和自定义

## 核心组件

### 1. IRequestHandler<TRequest, TResponse>

定义请求处理的通用接口：

```csharp
public interface IRequestHandler<TRequest, TResponse>
{
    Task<ApiResponse<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}
```

### 2. RequestHandlerBase<TRequest, TResponse>

请求处理器基类，提供通用的处理逻辑：

```csharp
public abstract class RequestHandlerBase<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
{
    public async Task<ApiResponse<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        // 验证请求
        var validationResult = await ValidateRequestAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ApiResponse<TResponse>.Failure(validationResult.ErrorMessage, 400);
        }

        // 执行业务逻辑
        var result = await ProcessRequestAsync(request, cancellationToken);
        
        // 返回成功响应
        return ApiResponse<TResponse>.CreateSuccess(result.Data, result.Message);
    }

    protected virtual async Task<RequestValidationResult> ValidateRequestAsync(TRequest request, CancellationToken cancellationToken)
    {
        // 默认实现
        await Task.CompletedTask;
        return RequestValidationResult.Success();
    }

    protected abstract Task<RequestProcessResult<TResponse>> ProcessRequestAsync(TRequest request, CancellationToken cancellationToken);
}
```

### 3. ApiResponse<T>

统一的API响应模型：

```csharp
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
    public int Code { get; set; }
    // ... 其他属性和方法
}
```

## 使用示例

### 服务器端实现

#### 1. 创建请求处理器

```csharp
public class ServerLoginRequestHandler : RequestHandlerBase<LoginRequest, LoginResult>
{
    private readonly IServiceProvider _serviceProvider;

    public ServerLoginRequestHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task<RequestValidationResult> ValidateRequestAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        // 验证登录请求
        if (string.IsNullOrEmpty(request.Username))
        {
            return RequestValidationResult.Failure("用户名不能为空");
        }

        // ... 其他验证逻辑

        return RequestValidationResult.Success();
    }

    protected override async Task<RequestProcessResult<LoginResult>> ProcessRequestAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        // 处理登录逻辑
        var userInfo = await ValidateUserCredentialsAsync(request.Username, request.Password, cancellationToken);
        
        // 生成登录结果
        var loginResult = new LoginResult
        {
            UserId = userInfo.UserId,
            Username = userInfo.Username,
            // ... 其他属性
        };

        return RequestProcessResult<LoginResult>.Create(loginResult, "登录成功");
    }
}
```

#### 2. 创建命令处理器

```csharp
[CommandHandler("UnifiedLoginCommandHandler", priority: 100)]
public class UnifiedLoginCommandHandler : UnifiedCommandHandlerBase
{
    private readonly ServerLoginRequestHandler _loginRequestHandler;

    public UnifiedLoginCommandHandler(ServerLoginRequestHandler loginRequestHandler, ILogger logger = null) 
        : base(logger)
    {
        _loginRequestHandler = loginRequestHandler;
    }

    public override IReadOnlyList<uint> SupportedCommands => new uint[]
    {
        (uint)AuthenticationCommands.Login,
        (uint)AuthenticationCommands.LoginRequest
    };

    protected override async Task<CommandResult> ProcessCommandAsync(ICommand command, CancellationToken cancellationToken)
    {
        var loginRequest = ParseLoginRequest(command.OriginalData);
        
        // 使用通用请求处理器处理登录请求
        var response = await _loginRequestHandler.HandleAsync(loginRequest, cancellationToken);

        if (response.IsSuccess())
        {
            var responseData = CreateLoginSuccessResponse(response.Data);
            return CommandResult.SuccessWithResponse(responseData, response.Data, response.Message);
        }
        else
        {
            return CommandResult.Failure(response.Message, "LOGIN_FAILED");
        }
    }
}
```

#### 3. 注册依赖注入

在 `NetworkServicesDependencyInjection.cs` 中注册服务：

```csharp
// 注册服务器端登录请求处理器
services.AddTransient<ServerLoginRequestHandler>();

// 在Autofac配置中
builder.RegisterType<ServerLoginRequestHandler>().AsSelf().InstancePerDependency();
```

### 客户端实现

#### 1. 创建命令类

```csharp
[Command(0x0100, "Login", CommandCategory.Authentication, Description = "用户登录命令")]
public class LoginCommand : BaseCommand
{
    public override CommandId CommandIdentifier => AuthenticationCommands.Login;
    
    public LoginRequest LoginRequest { get; set; }

    public LoginCommand(string username, string password, string clientInfo = null)
    {
        LoginRequest = LoginRequest.Create(username, password, clientInfo);
        Priority = CommandPriority.High;
        TimeoutMs = 30000;
    }

    protected override object GetSerializableData()
    {
        return LoginRequest;
    }
}
```

#### 2. 使用通信管理器发送请求

```csharp
public class UnifiedLoginExample
{
    private readonly CommunicationManager _communicationManager;

    public async Task<bool> LoginAsync(string username, string password)
    {
        // 创建登录请求数据
        var loginRequest = LoginRequest.Create(username, password, "Windows Client");

        // 发送登录请求并等待响应
        var response = await _communicationManager.SendCommandAsync<LoginRequest, LoginResult>(
            AuthenticationCommands.Login,
            loginRequest,
            CancellationToken.None,
            30000);

        if (response.IsSuccess())
        {
            Console.WriteLine("用户登录成功");
            return true;
        }
        else
        {
            Console.WriteLine($"用户登录失败: {response.Message}");
            return false;
        }
    }
}
```

## 优势

1. **代码复用**：服务器端和客户端可以共享相同的请求/响应模型
2. **类型安全**：编译时检查请求和响应类型
3. **统一错误处理**：标准化的错误响应格式
4. **易于测试**：请求处理器可以独立测试
5. **可扩展性**：易于添加新的请求类型和处理器
6. **异步支持**：完全支持异步操作

## 最佳实践

1. **请求验证**：在 `ValidateRequestAsync` 方法中进行请求数据验证
2. **异常处理**：在 `HandleException` 方法中处理异常并返回适当的错误响应
3. **日志记录**：在适当的位置记录处理日志
4. **性能优化**：使用异步操作避免阻塞
5. **安全性**：确保敏感数据得到适当处理

## 扩展

可以通过继承 `RequestHandlerBase` 类来创建自定义的请求处理器，实现特定业务逻辑的处理。