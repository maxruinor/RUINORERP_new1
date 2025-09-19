# RUINORERP Network 模块架构文档 (深度分析版)

## 1. 核心组件详解

### 1.1 NetworkServer (核心服务器类)
```csharp
public class NetworkServer {
    // 关键字段
    private readonly ISessionManager _sessionManager;
    private IHost _host;

    // 核心方法
    public async Task<bool> StartAsync(int port = 8009) {
        // 配置SuperSocket服务器
        var hostBuilder = SuperSocketHostBuilder.Create<PacketModel, PacketPipelineFilter>()
            .ConfigureServices(services => {
                services.AddSingleton<ISessionManager>(_sessionManager);
                // 其他服务注册...
            });
    }
}
```

## 2. 关键设计模式

### 2.1 管道过滤器模式
```mermaid
classDiagram
    class PacketPipelineFilter{
        +FilterAsync()
    }
    class PacketHandler{
        +HandlePacket()
    }
    PacketPipelineFilter --> PacketHandler
```

## 3. 完整类关系图

```mermaid
classDiagram
    NetworkServer --> ISessionManager
    NetworkServer --> IUserService
    NetworkServer --> PacketHandler
    
    ISessionManager <|.. UnifiedSessionManager
    IUserService <|.. UnifiedUserService
    
    PacketHandler --> LoginHandler
    PacketHandler --> MessageHandler
```

## 4. 关键数据流

### 4.1 登录流程
```mermaid
sequenceDiagram
    Client->>+NetworkServer: TCP连接
    NetworkServer->>+UnifiedSessionManager: 创建会话
    UnifiedSessionManager-->>-Client: 会话ID
    Client->>+LoginHandler: 发送凭据
    LoginHandler->>UnifiedUserService: 验证用户
    UnifiedUserService-->>-LoginHandler: 验证结果
    LoginHandler-->>-Client: 认证响应
```

## 5. 性能关键点

1. **会话管理**：
   - 使用`ConcurrentDictionary`保证线程安全
   - 会话清理定时器配置

2. **网络配置**：
   ```csharp
   options.MaxPackageLength = 1024 * 1024; // 1MB
   options.ReceiveBufferSize = 4096;
   options.ReceiveTimeout = 120000; // 2分钟
   ```

## 6. 数据包处理子系统

### 6.1 PacketPipelineFilter (核心过滤器)
```csharp
public class PacketPipelineFilter : FixedHeaderPipelineFilter<PacketModel> {
    // 固定头长度18字节
    protected override int GetBodyLengthFromHeader(ref ReadOnlySequence<byte> buffer) {
        // 1. 跳过命令ID (4字节)
        // 2. 读取数据长度 (4字节)
        // 3. 计算预估包体长度
    }
}
```

**关键设计**：
- 使用SuperSocket原生管道过滤器
- 精确控制内存缓冲区
- 与PacketSpec序列化器深度集成

### 6.2 处理流程图
```mermaid
flowchart TD
    A[TCP字节流] --> B[解析包头]
    B --> C{长度有效?}
    C -->|是| D[读取完整包]
    C -->|否| E[丢弃数据]
    D --> F[反序列化]
    F --> G[业务处理器]
```

## 7. 用户认证服务 (UnifiedUserService)

### 7.1 核心功能
```csharp
public class UnifiedUserService : IUserService {
    // 依赖注入
    private readonly IUserRepository _userRepository;
    private readonly ICacheService _cacheService;
    
    // 核心方法
    public async Task<AuthenticationResult> AuthenticateAsync(string username, string password) {
        // 1. 验证用户状态
        // 2. 检查密码哈希
        // 3. 生成JWT令牌
        // 4. 返回用户角色和权限
    }
}
```

**功能矩阵**：
| 功能 | 方法 | 缓存策略 |
|------|------|----------|
| 用户认证 | AuthenticateAsync | 用户信息缓存15分钟 |
| 密码管理 | ResetPasswordAsync | 立即失效缓存 |
| 会话控制 | ForceUserOfflineAsync | 实时生效 |

### 7.2 认证流程
```mermaid
sequenceDiagram
    participant Client
    participant LoginHandler
    participant UnifiedUserService
    participant CacheService
    
    Client->>LoginHandler: 发送凭据
    LoginHandler->>UnifiedUserService: AuthenticateAsync
    UnifiedUserService->>CacheService: 检查用户缓存
    alt 缓存命中
        UnifiedUserService-->>LoginHandler: 返回缓存数据
    else 缓存未命中
        UnifiedUserService->>UserRepository: 查询数据库
        UnifiedUserService->>CacheService: 写入缓存
    end
    LoginHandler-->>Client: 返回认证结果
```

### 7.3 安全设计
1. **密码哈希**：
   ```csharp
   private string HashPassword(string password, string salt) {
       using (var sha256 = SHA256.Create()) {
           var saltedPassword = password + salt;
           var bytes = Encoding.UTF8.GetBytes(saltedPassword);
           var hash = sha256.ComputeHash(bytes);
           return Convert.ToBase64String(hash);
       }
   }
   ```

2. **账户保护**：
   - 失败尝试限制（默认5次）
   - 自动账户锁定
   - 密码强制重置

## 8. 文件存储服务 (FileStorageManager)

### 8.1 核心架构
```csharp
public class FileStorageManager {
    private readonly string _storageRoot; // 文件系统存储
    private readonly IDatabase _redisDb;  // 元数据存储
    
    // 关键方法
    public async Task CheckConsistencyAsync() {
        // 1. 检查文件系统与Redis记录的一致性
        // 2. 自动修复不一致记录
    }
}
```

**存储设计**：
| 存储类型 | 用途 | 技术实现 |
|----------|------|----------|
| 文件系统 | 存储实际文件 | 本地磁盘/NAS |
| Redis | 存储文件元数据 | 哈希表+集合 |

### 8.2 文件分类管理
```mermaid
flowchart TD
    A[上传文件] --> B{文件类型}
    B -->|图片| C[Images/分类]
    B -->|文档| D[Documents/分类]
    B -->|其他| E[Temp]
    
    C --> F[记录到Redis]
    D --> F
    E --> G[定期清理]
```

### 8.3 关键操作流程

1. **文件上传**：
   - 写入临时目录
   - 验证后移动到正式目录
   - 记录元数据到Redis

2. **文件检索**：
   ```csharp
   // 从Redis获取文件信息
   var fileInfo = await _redisDb.StringGetAsync($"file:{fileId}");
   // 组合物理路径
   var filePath = Path.Combine(_storageRoot, categoryPath, fileId);
   ```

3. **备份恢复**：
   - 全量备份文件系统
   - 导出Redis快照
   - 支持时间点恢复

## 9. 依赖注入配置

### 9.1 核心服务注册
```csharp
public static void AddNetworkServices(this IServiceCollection services) {
    // 核心服务
    services.AddSingleton<ISessionManager, UnifiedSessionManager>();
    services.AddSingleton<IUserService, UserService>();
    
    // 基础设施
    services.AddSingleton<FileStorageManager>();
    services.AddSingleton<CacheService>();
}
```

**注册策略**：
- 所有服务均为单例模式
- 接口与实现分离注册
- 支持Autofac和原生DI容器

### 9.2 自动注册机制
```csharp
// 自动注册所有命令处理器
builder.RegisterAssemblyTypes(assembly)
    .Where(t => t.GetInterfaces().Any(i => 
        i.IsGenericType && 
        i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)))
    .AsImplementedInterfaces();
```

### 9.3 依赖关系图
```mermaid
classDiagram
    NetworkServer --> ISessionManager
    NetworkServer --> IUserService
    NetworkServer --> FileStorageManager
    
    ISessionManager <|.. UnifiedSessionManager
    IUserService <|.. UnifiedUserService
    
    class NetworkServicesDependencyInjection{
        +AddNetworkServices()
        +ConfigureNetworkServicesContainer()
    }
```

## 10. 扩展接口

### 10.1 ISessionManager 完整定义
```csharp
public interface ISessionManager : IDisposable {
    Task AddSessionAsync(IAppSession session);
    Task RemoveSessionAsync(string sessionId);
    Task<int> BroadcastPacketAsync(PacketModel packet);
    SessionStatistics GetStats();
    // ...其他方法
}