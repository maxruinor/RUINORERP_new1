# 业务处理器框架使用指南

## 概述

本框架提供了四种不同类型的业务处理器，用于处理不同复杂度的业务场景：

1. **泛型命令处理器** - 提供通用的请求处理框架
2. **通用CRUD处理器** - 标准化的增删改查操作
3. **动态路由处理器** - 基于配置的动态命令路由
4. **配置式业务处理器** - 通过配置定义业务逻辑

## 处理器类型对比

| 处理器类型 | 适用场景 | 优点 | 缺点 |
|-----------|---------|------|------|
| 简单业务处理器 | 字符串转换、数据验证等基础操作 | 简单易用、性能高 | 功能有限 |
| 泛型CRUD处理器 | 标准化的增删改查操作 | 代码复用、标准化 | 不够灵活 |
| 动态路由处理器 | 需要运行时配置的场景 | 灵活、可配置 | 配置复杂 |
| 配置式业务处理器 | 业务逻辑经常变化的场景 | 无需重新编译 | 性能开销 |

## 使用场景建议

### 简单业务场景

适用于字符串转换、数据验证、状态检查等简单操作：

```csharp
// 使用SimpleRequest/SimpleResponse
public class SimpleBusinessHandler : BaseCommandHandler
{
    public async Task<ResponseBase> HandleSimpleOperation(SimpleRequest request)
    {
        // 直接处理，无需创建复杂实体
        var result = request.GetStringValue();
        return SimpleResponse.CreateSuccessString(result.ToUpper(), "处理成功");
    }
}
```

### 中等复杂度业务

适用于标准的CRUD操作：

```csharp
// 使用泛型处理器
public class UserCrudHandler : CrudCommandHandler<User>
{
    protected override async Task<ResponseBase<User>> CreateAsync(User user, CancellationToken ct)
    {
        // 具体的用户创建逻辑
        return ResponseBase<User>.CreateSuccess(user);
    }
}
```

### 复杂业务

适用于登录认证、支付处理等复杂业务：

```csharp
// 传统方式，保持四个部分
// 但可以通过代码生成器自动生成
public class LoginCommandHandler : BaseCommandHandler
{
    // 复杂的登录认证逻辑
}
```

## 快速开始

### 1. 注册服务

在Startup.cs或Program.cs中注册处理器服务：

```csharp
services.AddTransient<SimpleBusinessHandler>();
services.AddTransient<UserCrudHandler>();
services.AddTransient<LoginCommandHandler>();
services.AddSingleton<BusinessProcessorFactory>();
```

### 2. 使用处理器工厂

```csharp
// 获取处理器工厂
var factory = serviceProvider.GetService<BusinessProcessorFactory>();

// 根据请求类型获取合适的处理器
var handler = factory.CreateProcessor("UserRequest");

// 执行处理
var result = await handler.HandleAsync(request);
```

### 3. 配置业务复杂度

```csharp
// 配置新的业务复杂度
factory.ConfigureComplexity("MyRequest", BusinessComplexity.Medium);

// 批量配置
var configurations = new Dictionary<string, BusinessComplexity>
{
    { "Request1", BusinessComplexity.Simple },
    { "Request2", BusinessComplexity.Medium },
    { "Request3", BusinessComplexity.Complex }
};
factory.ConfigureComplexities(configurations);
```

## 高级用法

### 动态路由配置

```csharp
// 创建动态路由配置
var config = new DynamicRouterConfig();
config.Routes.Add(new DynamicRouterConfig.RouteConfig
{
    CommandType = "CustomRequest",
    HandlerTypeName = typeof(MyHandler).FullName,
    HandlerType = typeof(MyHandler),
    Priority = 1
});

// 创建动态路由处理器
var router = new DynamicCommandRouter(serviceProvider, config, logger);
```

### 配置式业务处理

```csharp
// 创建业务配置
var businessConfig = new BusinessConfig();
var configInfo = new BusinessConfigInfo
{
    BusinessType = BusinessType.Create,
    EntityType = typeof(User),
    ValidationRules = new List<ValidationRule>
    {
        new ValidationRule
        {
            RuleName = "RequiredValidation",
            Expression = "Username != null",
            ErrorMessage = "用户名不能为空"
        }
    },
    DataMapping = new Dictionary<string, string>
    {
        { "Name", "Username" }
    }
};

businessConfig.AddBusinessConfig("CreateUserRequest", configInfo);

// 创建配置式处理器
var processor = new ConfigurableBusinessProcessor(businessConfig, logger);
```

## 最佳实践

### 1. 选择合适的处理器类型

- **简单业务**：使用`SimpleBusinessHandler`
- **标准CRUD**：使用`CrudCommandHandler<TEntity>`
- **复杂业务**：使用专门的强类型处理器
- **动态需求**：使用`DynamicCommandRouter`
- **配置驱动**：使用`ConfigurableBusinessProcessor`

### 2. 业务复杂度配置

```csharp
// 在应用启动时配置业务复杂度
var factory = serviceProvider.GetService<BusinessProcessorFactory>();

// 简单业务
factory.ConfigureComplexity("StringValidation", BusinessComplexity.Simple);
factory.ConfigureComplexity("StatusCheck", BusinessComplexity.Simple);

// 中等复杂度
factory.ConfigureComplexity("UserManagement", BusinessComplexity.Medium);
factory.ConfigureComplexity("ProductManagement", BusinessComplexity.Medium);

// 复杂业务
factory.ConfigureComplexity("Login", BusinessComplexity.Complex);
factory.ConfigureComplexity("Payment", BusinessComplexity.Complex);
```

### 3. 错误处理

```csharp
public async Task<ResponseBase> HandleBusinessOperation(RequestBase request)
{
    try
    {
        var handler = _processorFactory.CreateProcessor(request.GetType().Name);
        if (handler == null)
        {
            return ResponseBase.CreateError("未找到合适的处理器");
        }

        var result = await handler.HandleAsync(request);
        return result;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "业务处理失败");
        return ResponseBase.CreateError($"处理失败: {ex.Message}");
    }
}
```

### 4. 性能优化

```csharp
// 缓存处理器实例
private readonly Dictionary<string, ICommandHandler> _handlerCache = new Dictionary<string, ICommandHandler>();

public async Task<ResponseBase> ProcessRequest(RequestBase request)
{
    var requestType = request.GetType().Name;
    
    if (!_handlerCache.ContainsKey(requestType))
    {
        _handlerCache[requestType] = _processorFactory.CreateProcessor(requestType);
    }

    var handler = _handlerCache[requestType];
    return await handler.HandleAsync(request);
}
```

## 文件结构

```
RUINORERP/
├── RUINORERP.PacketSpec/
│   └── Commands/
│       ├── GenericCommandHandler.cs          # 泛型命令处理器基类
│       └── CrudCommandHandler.cs             # 通用CRUD处理器
├── RUINORERP.Server/
│   └── Network/
│       └── Commands/
│           ├── DynamicCommandRouter.cs       # 动态路由处理器
│           ├── ConfigurableBusinessProcessor.cs  # 配置式业务处理器
│           ├── SimpleBusinessHandler.cs      # 简单业务处理器
│           ├── UserCrudHandler.cs            # 用户CRUD处理器示例
│           └── BusinessProcessorFactory.cs   # 业务处理器工厂
└── Examples/
    └── BusinessProcessorUsageExamples.cs     # 使用示例
```

## 迁移指南

### 从传统处理器迁移

1. **识别业务复杂度**：分析现有业务逻辑的复杂度
2. **选择合适的处理器**：根据复杂度选择对应的处理器类型
3. **逐步迁移**：先迁移简单业务，再迁移复杂业务
4. **测试验证**：确保迁移后的功能与原功能一致

### 迁移示例

```csharp
// 传统方式
public class OldUserHandler : BaseCommandHandler
{
    protected override async Task<ResponseBase> HandleRequestAsync(object request, CancellationToken cancellationToken)
    {
        // 复杂的处理逻辑
    }
}

// 迁移到CRUD处理器
public class NewUserHandler : CrudCommandHandler<User>
{
    protected override async Task<ResponseBase<User>> CreateAsync(User user, CancellationToken cancellationToken)
    {
        // 专注于创建逻辑
    }

    protected override async Task<ResponseBase<User>> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        // 专注于更新逻辑
    }
}
```

## 常见问题

### Q: 如何选择合适的处理器类型？

A: 根据业务复杂度选择：
- 简单数据转换 → 简单业务处理器
- 标准CRUD操作 → 通用CRUD处理器
- 复杂业务逻辑 → 专用强类型处理器
- 需要动态配置 → 动态路由处理器
- 业务规则经常变化 → 配置式业务处理器

### Q: 性能如何？

A: 不同处理器的性能特点：
- 简单业务处理器：性能最高，适合高频简单操作
- 通用CRUD处理器：性能良好，适合标准CRUD操作
- 动态路由处理器：有轻微性能开销，但灵活性高
- 配置式业务处理器：性能开销较大，但可配置性强

### Q: 如何扩展？

A: 框架支持多种扩展方式：
- 继承现有处理器基类
- 实现自定义处理器接口
- 配置动态路由规则
- 添加自定义业务配置

## 总结

本业务处理器框架提供了灵活且可扩展的解决方案，能够适应不同复杂度的业务场景。通过合理选择处理器类型，可以在保持代码简洁的同时，满足各种业务需求。建议根据实际业务情况，选择合适的处理器类型，并逐步迁移现有代码以获得更好的维护性和扩展性。