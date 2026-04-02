# RUINORERP 移动端 APP 技术实施方案（优化版）

## 📋 文档信息

| 项目 | 内容 |
|------|------|
| **文档版本** | v2.0 (优化版) |
| **编制日期** | 2026-04-02 |
| **编制人** | AI Assistant |
| **审核状态** | 待审核 |
| **架构原则** | 复用优先、零服务器改动、最小化改造 |

---

## 一、方案概述

### 1.1 核心设计理念

**基于现有 SuperSocket 架构，直接复用，不重复建设。**

```
❌ 错误路线：Mobile → HTTP Gateway (新建) → Business → Database
✅ 正确路线：Mobile → SuperSocket Client → Server (现有) → Business → Database
```

### 1.2 技术决策要点

| 决策项 | 原方案问题 | 优化方案 | 理由 |
|--------|----------|---------|------|
| **通信协议** | 新建 HTTP API 网关 | 复用 SuperSocket TCP | 避免重复建设，降低运维成本 |
| **PacketSpec** | 升级到 .NET Standard 2.1 | 保持 .NET Standard 2.0 | 兼容 .NET Framework 4.8 项目 |
| **服务器端** | 开发 Web API Controllers | 零改动，复用现有 Commands | 最大化代码复用 |
| **认证机制** | 重新实现 JWT | 直接使用 PacketSpec TokenManager | 统一认证体系 |

### 1.3 整体架构图

```
┌─────────────────────────────────────────────────────────────┐
│              安卓移动端 (.NET MAUI App)                      │
│  ┌───────────────────────────────────────────────────────┐ │
│  │          表现层 (MAUI Views & ViewModels)             │ │
│  │  - LoginPage + LoginViewModel                         │ │
│  │  - BillListPage + BillListViewModel                   │ │
│  │  - BillDetailPage + BillDetailViewModel               │ │
│  │  - AuditDialog + AuditViewModel                       │ │
│  └───────────────────────────────────────────────────────┘ │
│  ┌───────────────────────────────────────────────────────┐ │
│  │          应用服务层 (App Services)                     │ │
│  │  - MobileAuthService (认证服务)                        │ │
│  │  - BillQueryAppService (查询服务)                      │ │
│  │  - BillAuditAppService (审核服务)                      │ │
│  └───────────────────────────────────────────────────────┘ │
│  ┌───────────────────────────────────────────────────────┐ │
│  │        基础设施层 (.NET Standard 2.0 ClassLib)        │ │
│  │  ├─ Network: MobileTcpClient (SuperSocket Client)    │ │
│  │  ├─ Security: JwtTokenManager (PacketSpec)           │ │
│  │  ├─ Storage: SQLiteDatabase + SecureStorage          │ │
│  │  └─ Models: 引用 RUINORERP.PacketSpec                │ │
│  └───────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
                        ↕ TCP/IP (加密协议，完全复用现有)
┌─────────────────────────────────────────────────────────────┐
│         RUINORERP.Server (现有服务器，零改动)                │
│  ┌───────────────────────────────────────────────────────┐ │
│  │  SuperSocket Server (net8.0-windows)                  │ │
│  │  ├─ EasyTcpServer                                     │ │
│  │  ├─ Command Handlers (复用现有)                        │ │
│  │  │  - LoginCommandHandler                            │ │
│  │  │  - QueryBillCommandHandler                        │ │
│  │  │  - AuditBillCommandHandler                        │ │
│  │  └─ Session Management                                │ │
│  └───────────────────────────────────────────────────────┘ │
│  ┌───────────────────────────────────────────────────────┐ │
│  │  RUINORERP.Business (net48, 完全复用)                 │ │
│  │  - tb_SaleOrderController                             │ │
│  │  - tb_PurOrderController                              │ │
│  │  - tb_MoOrderController                               │ │
│  └───────────────────────────────────────────────────────┘ │
│  ┌───────────────────────────────────────────────────────┐ │
│  │  Database (SQL Server / MySQL)                        │ │
│  └───────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
```

---

## 二、技术选型与可行性分析

### 2.1 跨平台框架选择

**推荐方案：.NET MAUI (.NET Multi-platform App UI)**

#### 选择理由：

| 维度 | .NET MAUI | Xamarin.Forms | Flutter | React Native |
|------|-----------|---------------|---------|--------------|
| **C#代码复用** | ✅ 90%+ | ✅ 90%+ | ❌ 0% | ❌ <50% |
| **业务逻辑复用** | ✅ 可直接引用 PacketSpec | ✅ 同左 | ❌ 需重写 | ❌ 需重写 |
| **学习曲线** | ✅ 低（团队熟悉 C#） | ✅ 同左 | ⚠️ 中（Dart） | ⚠️ 中（JS/TS） |
| **VS 支持** | ✅ VS2022 完整支持 | ✅ 同左 | ⚠️ 插件支持 | ⚠️ 插件支持 |
| **性能** | ✅ AOT 原生编译 | ✅ 同左 | ✅ 优秀 | ⚠️ 桥接损耗 |
| **未来前景** | ✅ 微软主力维护 | ❌ 停止维护 | ✅ Google 支持 | ✅ Meta 支持 |

**结论**：.NET MAUI 是唯一能直接复用 PacketSpec 且团队无学习成本的方案。

---

### 2.2 PacketSpec 框架兼容性

**关键决策：保持 .NET Standard 2.0，不升级**

#### 兼容性矩阵：

| 消费项目 | 目标框架 | 兼容 NS 2.0 | 兼容 NS 2.1 | 推荐方案 |
|---------|---------|------------|------------|---------|
| RUINORERP.Business | net48 | ✅ 支持 | ❌ **不支持** | ✅ NS 2.0 |
| RUINORERP.IServices | net48 | ✅ 支持 | ❌ **不支持** | ✅ NS 2.0 |
| RUINORERP.UI | net48 | ✅ 支持 | ❌ **不支持** | ✅ NS 2.0 |
| RUINORERP.Server | net8.0 | ✅ 支持 | ✅ 支持 | ✅ NS 2.0 |
| RUINORERP.Mobile (MAUI) | net8.0-android | ✅ 支持 | ✅ 支持 | ✅ NS 2.0 |

**NS = .NET Standard**

#### 为什么不能升级到 .NET Standard 2.1？

```xml
<!-- ❌ 错误做法 -->
<TargetFramework>netstandard2.1</TargetFramework>
<!-- 后果：所有 .NET Framework 4.8 项目无法引用，编译失败 -->

<!-- ✅ 正确做法 -->
<TargetFramework>netstandard2.0</TargetFramework>
<!-- .NET MAUI (net8.0-android) 完全兼容 .NET Standard 2.0 -->
```

**技术依据**：
- .NET Framework 4.8 最高支持到 .NET Standard 2.0
- .NET Standard 2.1 仅被 .NET Core 3.0+ 和 .NET 5+ 支持
- .NET MAUI 基于 .NET 8.0，向下兼容 .NET Standard 2.0

---

### 2.3 通信协议选择

**推荐方案：复用 SuperSocket TCP 长连接**

#### 协议对比：

| 特性 | SuperSocket TCP | HTTP/HTTPS | WebSocket |
|------|----------------|------------|-----------|
| **连接方式** | ✅ 长连接 | ❌ 短连接 | ✅ 长连接 |
| **延迟** | ✅ 极低（毫秒级） | ⚠️ 高（握手开销） | ✅ 低 |
| **数据压缩** | ✅ 自定义协议 | ⚠️ gzip 固定格式 | ✅ 自定义 |
| **心跳检测** | ✅ 内置支持 | ❌ 需额外实现 | ✅ 需实现 |
| **断线重连** | ✅ 内置支持 | ❌ 需实现 | ✅ 需实现 |
| **防火墙穿透** | ⚠️ 需配置端口 | ✅ 80/443 默认开放 | ✅ 80/443 |
| **现有架构** | ✅ 已有完整体系 | ❌ 需重建 | ❌ 需重建 |
| **命令体系** | ✅ 复用现有 Commands | ❌ 需重写 | ❌ 需重写 |

**综合评估**：
- SuperSocket TCP 在 ERP 场景下性能最优
- 已有成熟的命令处理机制
- 零服务器端改造成本

---

## 三、项目结构与依赖关系

### 3.1 移动端项目结构

```
RUINORERP.Mobile.App/
├── RUINORERP.Mobile.App.csproj (.NET 8.0-android)
├── Platforms/
│   ├── Android/
│   │   ├── MainActivity.cs
│   │   ├── MainApplication.cs
│   │   └── AndroidManifest.xml
│   └── Windows/ (可选扩展)
├── Resources/
│   ├── Raw/
│   ├── Fonts/
│   └── Styles/
├── App.xaml
├── AppShell.xaml
├── MauiProgram.cs
├── Views/
│   ├── LoginPage.xaml + LoginPage.xaml.cs
│   ├── BillListPage.xaml + BillListPage.xaml.cs
│   ├── BillDetailPage.xaml + BillDetailPage.xaml.cs
│   └── AuditDialog.xaml + AuditDialog.xaml.cs
├── ViewModels/
│   ├── LoginViewModel.cs
│   ├── BillListViewModel.cs
│   ├── BillDetailViewModel.cs
│   └── AuditViewModel.cs
├── Converters/
│   ├── DateToStringConverter.cs
│   ├── BillStatusToColorConverter.cs
│   └── DecimalToCurrencyStringConverter.cs
├── Infrastructure/
│   ├── MobileTcpClient.cs (网络通信)
│   ├── CommandDispatcher.cs (命令分发)
│   └── ResponseHandler.cs (响应处理)
├── Services/
│   ├── IMobileAuthService.cs + MobileAuthService.cs
│   ├── IBillQueryAppService.cs + BillQueryAppService.cs
│   └── IBillAuditAppService.cs + BillAuditAppService.cs
├── Storage/
│   ├── IMobileDatabaseService.cs + MobileDatabaseService.cs
│   └── ISecureTokenStorage.cs + SecureTokenStorage.cs
└── Models/
    └── (使用 PacketSpec 中的模型，不自建)
```

### 3.2 共享类库项目

```
RUINORERP.Mobile.Shared/
├── RUINORERP.Mobile.Shared.csproj (.NET Standard 2.0)
├── Infrastructure/
│   ├── MobileTcpClient.cs (可复用的 TCP 客户端封装)
│   ├── PackageHandler/
│   │   └── CustomPackageHandler.cs (协议包处理)
│   └── Extensions/
│       └── CommandExtensions.cs (命令扩展方法)
└── Services/
    └── BaseMobileService.cs (移动服务基类)
```

### 3.3 项目引用关系

```xml
<!-- RUINORERP.Mobile.App.csproj -->
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFrameworks>net8.0-android</TargetFrameworks>
    <OutputType>Exe</OutputType>
    <UseMaui>true</UseMaui>
  </PropertyGroup>

  <ItemGroup>
    <!-- 直接引用现有 PacketSpec，无需修改 -->
    <ProjectReference Include="..\RUINORERP.PacketSpec\RUINORERP.PacketSpec.csproj" />
    
    <!-- 可选：共享工具类库 -->
    <ProjectReference Include="..\RUINORERP.Mobile.Shared\RUINORERP.Mobile.Shared.csproj" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Maui.Controls" Version="8.0.x" />
    <PackageReference Include="SuperSocket.ClientEngine" Version="0.10.0" />
    <PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="7.0.x" />
    <PackageReference Include="sqlite-net-pcl" Version="1.8.x" />
  </ItemGroup>
</Project>
```

---

## 四、核心技术实现

### 4.1 网络通信层实现

#### 4.11 MobileTcpClient 封装

```csharp
// RUINORERP.Mobile.App/Infrastructure/MobileTcpClient.cs
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SuperSocket.ClientEngine;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Serialization;

namespace RUINORERP.Mobile.App.Infrastructure
{
    /// <summary>
    /// 移动端 TCP 客户端封装
    /// 复用现有 SuperSocket 协议体系
    /// </summary>
    public class MobileTcpClient : IDisposable
    {
        private readonly EasyTcpClient _client;
        private readonly ILogger<MobileTcpClient> _logger;
        private readonly string _serverHost;
        private readonly int _serverPort;
        private bool _isConnected;
        private bool _disposed;

        // 命令响应字典：RequestId -> TaskCompletionSource
        private readonly ConcurrentDictionary<string, TaskCompletionSource<GenericCommand>> _pendingRequests
            = new ConcurrentDictionary<string, TaskCompletionSource<GenericCommand>>();

        public event EventHandler<bool> OnConnectionStateChanged;
        public event EventHandler<GenericCommand> OnServerNotification;

        public MobileTcpClient(ILogger<MobileTcpClient> logger, IConfiguration config)
        {
            _logger = logger;
            _serverHost = config["Server:Host"] ?? "your-server.com";
            _serverPort = int.Parse(config["Server:Port"] ?? "5000");

            _client = new EasyTcpClient();
            
            // 注册连接事件
            _client.Connected += Client_Connected;
            _client.Closed += Client_Closed;
            _client.Error += Client_Error;
            
            // 注册命令接收事件
            _client.CommandReceived += Client_CommandReceived;
        }

        /// <summary>
        /// 连接到服务器
        /// </summary>
        public async Task ConnectAsync()
        {
            try
            {
                _logger.LogInformation($"正在连接到 {_serverHost}:{_serverPort}...");
                await _client.ConnectAsync(_serverHost, _serverPort);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "连接服务器失败");
                throw new ConnectionException("无法连接到服务器", ex);
            }
        }

        /// <summary>
        /// 发送命令并等待响应
        /// </summary>
        public async Task<TResponse> SendCommandAsync<TRequest, TResponse>(
            string commandKey, 
            TRequest request,
            CancellationToken cancellationToken = default)
        {
            if (!_isConnected)
            {
                throw new ConnectionException("未连接到服务器");
            }

            // 生成请求 ID（用于匹配响应）
            var requestId = Guid.NewGuid().ToString("N");

            // 创建请求命令（使用 PacketSpec 的 GenericCommand）
            var command = new GenericCommand
            {
                Key = commandKey,
                Data = request,
                RequestId = requestId // 添加请求 ID
            };

            // 创建 TaskCompletionSource 用于等待响应
            var tcs = new TaskCompletionSource<GenericCommand>();
            _pendingRequests.TryAdd(requestId, tcs);

            try
            {
                // 发送命令
                _logger.LogDebug($"发送命令：{commandKey}, RequestId: {requestId}");
                await _client.SendAsync(command);

                // 等待响应（带超时和取消令牌）
                using (cancellationToken.Register(() => 
                    tcs.TrySetCanceled(cancellationToken)))
                {
                    var response = await tcs.Task.TimeoutAfter(TimeSpan.FromSeconds(30));
                    
                    // 解析响应数据
                    if (response.Data is TResponse responseData)
                    {
                        return responseData;
                    }
                    else
                    {
                        // 尝试 JSON 反序列化
                        var jsonData = response.Data.ToString();
                        return JsonConvert.DeserializeObject<TResponse>(jsonData);
                    }
                }
            }
            finally
            {
                // 清理请求
                _pendingRequests.TryRemove(requestId, out _);
            }
        }

        /// <summary>
        /// 发送单向命令（不需要响应）
        /// </summary>
        public async Task SendCommandAsync<TRequest>(string commandKey, TRequest request)
        {
            if (!_isConnected)
            {
                throw new ConnectionException("未连接到服务器");
            }

            var command = new GenericCommand
            {
                Key = commandKey,
                Data = request
            };

            await _client.SendAsync(command);
        }

        #region 事件处理

        private void Client_Connected(object sender, EventArgs e)
        {
            _isConnected = true;
            _logger.LogInformation("已连接到服务器");
            OnConnectionStateChanged?.Invoke(this, true);
        }

        private void Client_Closed(object sender, EventArgs e)
        {
            _isConnected = false;
            _logger.LogWarning("与服务器连接断开");
            OnConnectionStateChanged?.Invoke(this, false);
            
            // 取消所有待处理请求
            foreach (var kvp in _pendingRequests.ToArray())
            {
                kvp.Value.TrySetException(new ConnectionException("连接已断开"));
                _pendingRequests.TryRemove(kvp.Key, out _);
            }
        }

        private void Client_Error(object sender, ExceptionEventArgs e)
        {
            _logger.LogError(e.Exception, "通信异常");
        }

        private void Client_CommandReceived(object sender, CommandInfo e)
        {
            // 处理服务器推送的命令/通知
            if (e is GenericCommand command)
            {
                // 如果是响应命令（有 RequestId）
                if (!string.IsNullOrEmpty(command.RequestId))
                {
                    if (_pendingRequests.TryGetValue(command.RequestId, out var tcs))
                    {
                        tcs.TrySetResult(command);
                        return;
                    }
                }

                // 否则作为服务器通知处理
                OnServerNotification?.Invoke(this, command);
            }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            if (_disposed) return;

            _client.Dispose();
            _disposed = true;
            GC.SuppressFinalize(this);
        }

        #endregion
    }

    /// <summary>
    /// 连接异常
    /// </summary>
    public class ConnectionException : Exception
    {
        public ConnectionException(string message) : base(message) { }
        public ConnectionException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Task 超时扩展
    /// </summary>
    public static class TaskExtensions
    {
        public static async Task<T> TimeoutAfter<T>(this Task<T> task, TimeSpan timeout)
        {
            if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
            {
                return await task;
            }
            throw new TimeoutException($"操作超时 ({timeout.TotalSeconds}秒)");
        }
    }
}
```

---

#### 4.12 命令分发器

```csharp
// RUINORERP.Mobile.App/Infrastructure/CommandDispatcher.cs
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Common;

namespace RUINORERP.Mobile.App.Infrastructure
{
    /// <summary>
    /// 命令分发器
    /// 封装常用命令的调用
    /// </summary>
    public class CommandDispatcher
    {
        private readonly MobileTcpClient _tcpClient;
        private readonly ILogger<CommandDispatcher> _logger;

        public CommandDispatcher(MobileTcpClient tcpClient, ILogger<CommandDispatcher> logger)
        {
            _tcpClient = tcpClient;
            _logger = logger;
        }

        /// <summary>
        /// 执行登录命令
        /// </summary>
        public async Task<ReturnResults> LoginAsync(string username, string passwordHash)
        {
            try
            {
                // 使用 PacketSpec 中的登录命令模型
                var loginCommand = new
                {
                    UserName = username,
                    PasswordHash = passwordHash,
                    ClientType = "Mobile",
                    ClientVersion = "1.0.0"
                };

                var result = await _tcpClient.SendCommandAsync<object, ReturnResults>(
                    "Login", 
                    loginCommand);

                _logger.LogInformation($"用户 {username} 登录 {(result.Succeeded ? "成功" : "失败")}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"登录过程异常：{username}");
                throw;
            }
        }

        /// <summary>
        /// 执行单据查询命令
        /// </summary>
        public async Task<PageResult<T>> QueryBillsAsync<T>(
            string billType,
            int pageIndex,
            int pageSize,
            object filterConditions = null)
        {
            try
            {
                var queryCommand = new
                {
                    BillType = billType,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    Filters = filterConditions ?? new { }
                };

                var result = await _tcpClient.SendCommandAsync<object, PageResult<T>>(
                    $"Query{billType}", 
                    queryCommand);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"查询{billType}失败");
                throw;
            }
        }

        /// <summary>
        /// 执行单据审核命令
        /// </summary>
        public async Task<ReturnResults> AuditBillAsync(
            string billType,
            long billId,
            bool pass,
            string comments)
        {
            try
            {
                var auditCommand = new
                {
                    BillType = billType,
                    BillId = billId,
                    Pass = pass,
                    Comments = comments,
                    AuditTime = DateTime.Now
                };

                var result = await _tcpClient.SendCommandAsync<object, ReturnResults>(
                    $"Audit{billType}", 
                    auditCommand);

                _logger.LogInformation($"{billType}-{billId} 审核 {(pass ? "通过" : "驳回")}: {comments}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{billType}-{billId} 审核失败");
                throw;
            }
        }

        /// <summary>
        /// 获取单据详情
        /// </summary>
        public async Task<T> GetBillDetailAsync<T>(string billType, long billId)
        {
            try
            {
                var detailCommand = new
                {
                    BillType = billType,
                    BillId = billId
                };

                var result = await _tcpClient.SendCommandAsync<object, T>(
                    $"Get{billType}Detail", 
                    detailCommand);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取{billType}-{billId}详情失败");
                throw;
            }
        }
    }
}
```

---

### 4.2 认证服务实现

#### 4.21 MobileAuthService

```csharp
// RUINORERP.Mobile.App/Services/MobileAuthService.cs
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Mobile.App.Infrastructure;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Security;

namespace RUINORERP.Mobile.App.Services
{
    /// <summary>
    /// 移动端认证服务
    /// </summary>
    public interface IMobileAuthService
    {
        Task<LoginResponse> LoginAsync(string username, string password);
        Task<TokenInfo> RefreshTokenAsync();
        Task<string> GetValidTokenAsync();
        Task LogoutAsync();
        bool IsLoggedIn { get; }
    }

    public class LoginResponse
    {
        public bool Success { get; set; }
        public TokenInfo Token { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class MobileAuthService : IMobileAuthService
    {
        private readonly CommandDispatcher _commandDispatcher;
        private readonly ISecureTokenStorage _tokenStorage;
        private readonly ILogger<MobileAuthService> _logger;
        private TokenInfo _currentToken;

        public MobileAuthService(
            CommandDispatcher commandDispatcher,
            ISecureTokenStorage tokenStorage,
            ILogger<MobileAuthService> logger)
        {
            _commandDispatcher = commandDispatcher;
            _tokenStorage = tokenStorage;
            _logger = logger;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        public async Task<LoginResponse> LoginAsync(string username, string password)
        {
            try
            {
                // 密码加密（MD5 + Salt，与现有系统一致）
                var passwordHash = HashPassword(password);

                // 调用登录命令
                var result = await _commandDispatcher.LoginAsync(username, passwordHash);

                if (result.Succeeded)
                {
                    // 从返回数据中提取 Token 信息
                    var tokenInfo = ExtractTokenFromResult(result);
                    
                    // 保存 Token
                    await _tokenStorage.SaveTokenAsync(tokenInfo);
                    _currentToken = tokenInfo;

                    _logger.LogInformation($"用户 {username} 登录成功");

                    return new LoginResponse
                    {
                        Success = true,
                        Token = tokenInfo,
                        UserId = result.Data?.ToString(),
                        UserName = username
                    };
                }
                else
                {
                    _logger.LogWarning($"用户 {username} 登录失败：{result.ErrorMessage}");

                    return new LoginResponse
                    {
                        Success = false,
                        ErrorMessage = result.ErrorMessage
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"登录过程异常：{username}");
                return new LoginResponse
                {
                    Success = false,
                    ErrorMessage = $"登录失败：{ex.Message}"
                };
            }
        }

        /// <summary>
        /// 刷新 Token
        /// </summary>
        public async Task<TokenInfo> RefreshTokenAsync()
        {
            try
            {
                var currentToken = await _tokenStorage.GetTokenAsync();
                if (currentToken == null)
                {
                    throw new InvalidOperationException("未找到有效的 Token");
                }

                // 调用刷新 Token 命令（需要服务器端支持）
                var refreshCommand = new
                {
                    RefreshToken = currentToken.RefreshToken,
                    UserId = currentToken.UserId
                };

                var result = await _commandDispatcher.ExecuteAsync<object, ReturnResults>(
                    "RefreshToken", 
                    refreshCommand);

                if (result.Succeeded)
                {
                    var newToken = ExtractTokenFromResult(result);
                    await _tokenStorage.SaveTokenAsync(newToken);
                    _currentToken = newToken;

                    _logger.LogInformation("Token 刷新成功");
                    return newToken;
                }
                else
                {
                    _logger.LogWarning("Token 刷新失败，需要重新登录");
                    await LogoutAsync();
                    throw new AuthenticationException("Token 已过期，请重新登录");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token 刷新异常");
                throw;
            }
        }

        /// <summary>
        /// 获取有效的 Token（自动刷新即将过期的 Token）
        /// </summary>
        public async Task<string> GetValidTokenAsync()
        {
            if (_currentToken == null)
            {
                _currentToken = await _tokenStorage.GetTokenAsync();
            }

            if (_currentToken == null)
            {
                return null;
            }

            // 如果 Token 即将过期（5 分钟内），自动刷新
            if (_currentToken.ExpiresAt.AddMinutes(-5) < DateTime.Now)
            {
                _logger.LogInformation("Token 即将过期，自动刷新");
                var newToken = await RefreshTokenAsync();
                return newToken.AccessToken;
            }

            return _currentToken.AccessToken;
        }

        /// <summary>
        /// 登出
        /// </summary>
        public async Task LogoutAsync()
        {
            try
            {
                // 通知服务器登出
                await _commandDispatcher.ExecuteAsync("Logout", new { });

                // 清除本地 Token
                await _tokenStorage.ClearAsync();
                _currentToken = null;

                _logger.LogInformation("用户已登出");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "登出异常");
            }
        }

        /// <summary>
        /// 检查是否已登录
        /// </summary>
        public bool IsLoggedIn
        {
            get
            {
                return _currentToken != null && 
                       !_currentToken.IsExpired();
            }
        }

        #region 辅助方法

        /// <summary>
        /// 密码哈希（与现有系统一致）
        /// </summary>
        private string HashPassword(string password)
        {
            using (var md5 = MD5.Create())
            {
                var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// 从结果中提取 Token 信息
        /// </summary>
        private TokenInfo ExtractTokenFromResult(ReturnResults result)
        {
            // 假设服务器返回的数据包含 Token 信息
            // 实际格式需要根据现有系统的返回结构调整
            var jsonData = result.Data?.ToString();
            return JsonConvert.DeserializeObject<TokenInfo>(jsonData);
        }

        #endregion
    }
}
```

---

#### 4.22 安全 Token 存储

```csharp
// RUINORERP.Mobile.App/Storage/SecureTokenStorage.cs
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Security;

namespace RUINORERP.Mobile.App.Storage
{
    /// <summary>
    /// 安全 Token 存储接口
    /// </summary>
    public interface ISecureTokenStorage
    {
        Task SaveTokenAsync(TokenInfo token);
        Task<TokenInfo> GetTokenAsync();
        Task ClearAsync();
    }

    /// <summary>
    /// 使用 MAUI SecureStorage 实现
    /// </summary>
    public class SecureTokenStorage : ISecureTokenStorage
    {
        private const string TokenStorageKey = "auth_token_v1";
        private readonly ILogger<SecureTokenStorage> _logger;

        public SecureTokenStorage(ILogger<SecureTokenStorage> logger)
        {
            _logger = logger;
        }

        public async Task SaveTokenAsync(TokenInfo token)
        {
            try
            {
                var tokenJson = JsonConvert.SerializeObject(token);
                
                // 使用 SecureStorage 加密存储
                await SecureStorage.SetAsync(TokenStorageKey, tokenJson);
                
                _logger.LogDebug("Token 已安全存储");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token 存储失败");
                throw;
            }
        }

        public async Task<TokenInfo> GetTokenAsync()
        {
            try
            {
                var tokenJson = await SecureStorage.GetAsync(TokenStorageKey);
                
                if (string.IsNullOrEmpty(tokenJson))
                {
                    return null;
                }

                var token = JsonConvert.DeserializeObject<TokenInfo>(tokenJson);
                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token 读取失败");
                return null;
            }
        }

        public async Task ClearAsync()
        {
            try
            {
                SecureStorage.Remove(TokenStorageKey);
                await Task.CompletedTask;
                _logger.LogDebug("Token 已清除");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token 清除失败");
            }
        }
    }
}
```

---

### 4.3 业务服务层实现

#### 4.31 单据查询服务

```csharp
// RUINORERP.Mobile.App/Services/BillQueryAppService.cs
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Mobile.App.Infrastructure;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.Model;

namespace RUINORERP.Mobile.App.Services
{
    /// <summary>
    /// 单据查询应用服务接口
    /// </summary>
    public interface IBillQueryAppService
    {
        Task<PageResult<tb_SaleOrder>> GetSaleOrdersAsync(
            int pageIndex = 1,
            int pageSize = 20,
            BillStatus? status = null,
            DateTime? startDate = null,
            DateTime? endDate = null);

        Task<tb_SaleOrder> GetSaleOrderDetailAsync(long orderId);
        
        Task<PageResult<tb_PurOrder>> GetPurOrdersAsync(
            int pageIndex = 1,
            int pageSize = 20,
            BillStatus? status = null);

        Task<PageResult<tb_MoOrder>> GetMoOrdersAsync(
            int pageIndex = 1,
            int pageSize = 20,
            BillStatus? status = null);
    }

    /// <summary>
    /// 单据查询应用服务实现
    /// </summary>
    public class BillQueryAppService : IBillQueryAppService
    {
        private readonly CommandDispatcher _commandDispatcher;
        private readonly ILogger<BillQueryAppService> _logger;

        public BillQueryAppService(
            CommandDispatcher commandDispatcher,
            ILogger<BillQueryAppService> logger)
        {
            _commandDispatcher = commandDispatcher;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询销售订单
        /// </summary>
        public async Task<PageResult<tb_SaleOrder>> GetSaleOrdersAsync(
            int pageIndex = 1,
            int pageSize = 20,
            BillStatus? status = null,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            try
            {
                // 构建筛选条件
                var filters = new
                {
                    Status = status?.ToString(),
                    StartDate = startDate?.ToString("yyyy-MM-dd"),
                    EndDate = endDate?.ToString("yyyy-MM-dd")
                };

                // 调用查询命令（复用服务器端现有的 Query 命令）
                var result = await _commandDispatcher.QueryBillsAsync<tb_SaleOrder>(
                    "SaleOrder",
                    pageIndex,
                    pageSize,
                    filters);

                _logger.LogDebug($"查询销售订单成功，共 {result.TotalCount} 条记录");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "查询销售订单失败");
                throw;
            }
        }

        /// <summary>
        /// 获取销售订单详情
        /// </summary>
        public async Task<tb_SaleOrder> GetSaleOrderDetailAsync(long orderId)
        {
            try
            {
                var order = await _commandDispatcher.GetBillDetailAsync<tb_SaleOrder>(
                    "SaleOrder", 
                    orderId);

                _logger.LogDebug($"获取销售订单详情成功，ID: {orderId}");
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取销售订单详情失败，ID: {orderId}");
                throw;
            }
        }

        /// <summary>
        /// 查询采购订单（类似实现）
        /// </summary>
        public async Task<PageResult<tb_PurOrder>> GetPurOrdersAsync(
            int pageIndex = 1,
            int pageSize = 20,
            BillStatus? status = null)
        {
            try
            {
                var filters = new { Status = status?.ToString() };
                
                var result = await _commandDispatcher.QueryBillsAsync<tb_PurOrder>(
                    "PurOrder",
                    pageIndex,
                    pageSize,
                    filters);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "查询采购订单失败");
                throw;
            }
        }

        /// <summary>
        /// 查询生产订单（类似实现）
        /// </summary>
        public async Task<PageResult<tb_MoOrder>> GetMoOrdersAsync(
            int pageIndex = 1,
            int pageSize = 20,
            BillStatus? status = null)
        {
            try
            {
                var filters = new { Status = status?.ToString() };
                
                var result = await _commandDispatcher.QueryBillsAsync<tb_MoOrder>(
                    "MoOrder",
                    pageIndex,
                    pageSize,
                    filters);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "查询生产订单失败");
                throw;
            }
        }
    }
}
```

---

#### 4.32 单据审核服务

```csharp
// RUINORERP.Mobile.App/Services/BillAuditAppService.cs
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Mobile.App.Infrastructure;
using RUINORERP.PacketSpec.Models.Common;

namespace RUINORERP.Mobile.App.Services
{
    /// <summary>
    /// 单据审核应用服务接口
    /// </summary>
    public interface IBillAuditAppService
    {
        Task<AuditResult> AuditSaleOrderAsync(
            long orderId, 
            bool pass, 
            string comments);

        Task<AuditResult> AuditPurOrderAsync(
            long orderId, 
            bool pass, 
            string comments);

        Task<AuditResult> AuditMoOrderAsync(
            long orderId, 
            bool pass, 
            string comments);
    }

    public class AuditResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string NewStatus { get; set; }
        public DateTime AuditTime { get; set; }
    }

    /// <summary>
    /// 单据审核应用服务实现
    /// </summary>
    public class BillAuditAppService : IBillAuditAppService
    {
        private readonly CommandDispatcher _commandDispatcher;
        private readonly ILogger<BillAuditAppService> _logger;

        public BillAuditAppService(
            CommandDispatcher commandDispatcher,
            ILogger<BillAuditAppService> logger)
        {
            _commandDispatcher = commandDispatcher;
            _logger = logger;
        }

        /// <summary>
        /// 审核销售订单
        /// </summary>
        public async Task<AuditResult> AuditSaleOrderAsync(
            long orderId, 
            bool pass, 
            string comments)
        {
            try
            {
                _logger.LogInformation($"开始审核销售订单，ID: {orderId}, 结果：{(pass ? "通过" : "驳回")}");

                // 调用审核命令（复用服务器端现有的 Audit 命令）
                var result = await _commandDispatcher.AuditBillAsync(
                    "SaleOrder",
                    orderId,
                    pass,
                    comments);

                if (result.Succeeded)
                {
                    _logger.LogInformation($"销售订单审核成功，ID: {orderId}");
                    
                    return new AuditResult
                    {
                        Success = true,
                        Message = "审核成功",
                        AuditTime = DateTime.Now
                    };
                }
                else
                {
                    _logger.LogWarning($"销售订单审核失败，ID: {orderId}, 原因：{result.ErrorMessage}");
                    
                    return new AuditResult
                    {
                        Success = false,
                        Message = result.ErrorMessage,
                        AuditTime = DateTime.Now
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"销售订单审核异常，ID: {orderId}");
                return new AuditResult
                {
                    Success = false,
                    Message = $"审核失败：{ex.Message}",
                    AuditTime = DateTime.Now
                };
            }
        }

        /// <summary>
        /// 审核采购订单（类似实现）
        /// </summary>
        public async Task<AuditResult> AuditPurOrderAsync(
            long orderId, 
            bool pass, 
            string comments)
        {
            try
            {
                var result = await _commandDispatcher.AuditBillAsync(
                    "PurOrder",
                    orderId,
                    pass,
                    comments);

                return new AuditResult
                {
                    Success = result.Succeeded,
                    Message = result.Succeeded ? "审核成功" : result.ErrorMessage,
                    AuditTime = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"采购订单审核异常，ID: {orderId}");
                return new AuditResult
                {
                    Success = false,
                    Message = $"审核失败：{ex.Message}",
                    AuditTime = DateTime.Now
                };
            }
        }

        /// <summary>
        /// 审核生产订单（类似实现）
        /// </summary>
        public async Task<AuditResult> AuditMoOrderAsync(
            long orderId, 
            bool pass, 
            string comments)
        {
            try
            {
                var result = await _commandDispatcher.AuditBillAsync(
                    "MoOrder",
                    orderId,
                    pass,
                    comments);

                return new AuditResult
                {
                    Success = result.Succeeded,
                    Message = result.Succeeded ? "审核成功" : result.ErrorMessage,
                    AuditTime = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"生产订单审核异常，ID: {orderId}");
                return new AuditResult
                {
                    Success = false,
                    Message = $"审核失败：{ex.Message}",
                    AuditTime = DateTime.Now
                };
            }
        }
    }
}
```

---

### 4.4 本地缓存与离线支持

#### 4.41 SQLite 数据库服务

```csharp
// RUINORERP.Mobile.App/Storage/MobileDatabaseService.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SQLite;
using RUINORERP.Model;

namespace RUINORERP.Mobile.App.Storage
{
    /// <summary>
    /// 移动端本地数据库服务
    /// </summary>
    public interface IMobileDatabaseService
    {
        Task InitializeAsync();
        Task CacheBillAsync<T>(string billType, T billData);
        Task<T> GetCachedBillAsync<T>(string billType, string billNo);
        Task<List<CachedBill>> GetCachedBillsAsync(string billType = null);
        Task ClearExpiredCacheAsync(TimeSpan maxAge);
        Task ClearAllCacheAsync();
    }

    /// <summary>
    /// 缓存的单据数据模型
    /// </summary>
    [Table("CachedBills")]
    public class CachedBill
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(50)]
        public string BillType { get; set; } // SaleOrder, PurOrder, etc.

        [MaxLength(100)]
        public string BillNo { get; set; }

        public long BillId { get; set; }

        public string JsonData { get; set; } // 序列化后的完整数据

        public DateTime CacheTime { get; set; }

        public string Checksum { get; set; } // 用于判断数据是否变更

        public string UserId { get; set; } // 缓存所属用户
    }

    /// <summary>
    /// 数据库服务实现
    /// </summary>
    public class MobileDatabaseService : IMobileDatabaseService
    {
        private readonly ILogger<MobileDatabaseService> _logger;
        private SQLiteAsyncConnection _database;
        private readonly string _dbPath;

        public MobileDatabaseService(ILogger<MobileDatabaseService> logger)
        {
            _logger = logger;
            _dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "ruinorerp_mobile.db3");
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        public async Task InitializeAsync()
        {
            try
            {
                _database = new SQLiteAsyncConnection(_dbPath);
                
                // 创建表
                await _database.CreateTableAsync<CachedBill>();
                
                _logger.LogInformation($"数据库初始化完成，路径：{_dbPath}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "数据库初始化失败");
                throw;
            }
        }

        /// <summary>
        /// 缓存单据数据
        /// </summary>
        public async Task CacheBillAsync<T>(string billType, T billData)
        {
            try
            {
                var json = JsonConvert.SerializeObject(billData);
                var checksum = CalculateChecksum(json);
                
                // 提取 BillNo 和 BillId（需要 T 类型有这些属性）
                var billNo = ExtractProperty(billData, "BillNo")?.ToString();
                var billId = ExtractProperty(billData, "Id")?.ToString()?.ToLong() ?? 0;

                var cachedBill = new CachedBill
                {
                    BillType = billType,
                    BillNo = billNo,
                    BillId = billId,
                    JsonData = json,
                    CacheTime = DateTime.Now,
                    Checksum = checksum,
                    UserId = GetCurrentUserId()
                };

                // 插入或替换
                var existing = await _database.Table<CachedBill>()
                    .Where(x => x.BillType == billType && x.BillId == billId)
                    .FirstOrDefaultAsync();

                if (existing != null)
                {
                    cachedBill.Id = existing.Id;
                    await _database.UpdateAsync(cachedBill);
                }
                else
                {
                    await _database.InsertAsync(cachedBill);
                }

                _logger.LogDebug($"缓存{billType}-{billNo}成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "缓存单据失败");
                throw;
            }
        }

        /// <summary>
        /// 获取缓存的单据数据
        /// </summary>
        public async Task<T> GetCachedBillAsync<T>(string billType, string billNo)
        {
            try
            {
                var cachedBill = await _database.Table<CachedBill>()
                    .Where(x => x.BillType == billType && x.BillNo == billNo)
                    .FirstOrDefaultAsync();

                if (cachedBill == null)
                {
                    return default;
                }

                // 检查缓存是否过期（超过 24 小时）
                if ((DateTime.Now - cachedBill.CacheTime).TotalHours > 24)
                {
                    _logger.LogDebug($"缓存{billType}-{billNo}已过期");
                    return default;
                }

                // 验证 checksum（可选）
                var currentChecksum = CalculateChecksum(cachedBill.JsonData);
                if (cachedBill.Checksum != currentChecksum)
                {
                    _logger.LogWarning($"缓存{billType}-{billNo}校验和不匹配，可能已损坏");
                    return default;
                }

                var result = JsonConvert.DeserializeObject<T>(cachedBill.JsonData);
                _logger.LogDebug($"读取缓存{billType}-{billNo}成功");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "读取缓存失败");
                return default;
            }
        }

        /// <summary>
        /// 获取缓存的单据列表
        /// </summary>
        public async Task<List<CachedBill>> GetCachedBillsAsync(string billType = null)
        {
            try
            {
                var query = _database.Table<CachedBill>();
                
                if (!string.IsNullOrEmpty(billType))
                {
                    query = query.Where(x => x.BillType == billType);
                }

                var bills = await query.OrderByDescending(x => x.CacheTime).ToListAsync();
                return bills;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取缓存列表失败");
                return new List<CachedBill>();
            }
        }

        /// <summary>
        /// 清除过期缓存
        /// </summary>
        public async Task ClearExpiredCacheAsync(TimeSpan maxAge)
        {
            try
            {
                var expireTime = DateTime.Now - maxAge;
                
                var deleted = await _database.DeleteAsync(
                    _database.Table<CachedBill>().Where(x => x.CacheTime < expireTime));

                _logger.LogInformation($"清除 {deleted} 条过期缓存记录");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清除过期缓存失败");
            }
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public async Task ClearAllCacheAsync()
        {
            try
            {
                await _database.DeleteAllAsync<CachedBill>();
                _logger.LogInformation("已清除所有缓存");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清除所有缓存失败");
            }
        }

        #region 辅助方法

        private string CalculateChecksum(string data)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        private object ExtractProperty<T>(T obj, string propertyName)
        {
            var prop = typeof(T).GetProperty(propertyName);
            return prop?.GetValue(obj);
        }

        private string GetCurrentUserId()
        {
            // 从认证服务获取当前用户 ID
            // 这里简化处理
            return "mobile_user";
        }

        #endregion
    }

    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class StringExtensions
    {
        public static long? ToLong(this string str)
        {
            if (long.TryParse(str, out var result))
            {
                return result;
            }
            return null;
        }
    }
}
```

---

## 五、UI 层实现示例

### 5.1 登录页面 ViewModel

```csharp
// RUINORERP.Mobile.App/ViewModels/LoginViewModel.cs
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RUINORERP.Mobile.App.Services;

namespace RUINORERP.Mobile.App.ViewModels
{
    [ObservableObject]
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly IMobileAuthService _authService;

        [ObservableProperty]
        private string _userName;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private bool _rememberPassword;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _errorMessage;

        public LoginViewModel(IMobileAuthService authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            if (IsBusy) return;

            if (string.IsNullOrWhiteSpace(UserName))
            {
                ErrorMessage = "请输入用户名";
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "请输入密码";
                return;
            }

            try
            {
                IsBusy = true;
                ErrorMessage = null;

                var result = await _authService.LoginAsync(UserName, Password);

                if (result.Success)
                {
                    // 导航到主页
                    await Shell.Current.GoToAsync("//MainPage");
                }
                else
                {
                    ErrorMessage = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"登录失败：{ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task ForgotPasswordAsync()
        {
            await Shell.Current.GoToAsync("ForgotPasswordPage");
        }
    }
}
```

---

### 5.2 单据列表 ViewModel

```csharp
// RUINORERP.Mobile.App/ViewModels/BillListViewModel.cs
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RUINORERP.Mobile.App.Services;
using RUINORERP.Model;

namespace RUINORERP.Mobile.App.ViewModels
{
    [ObservableObject]
    public partial class BillListViewModel : BaseViewModel
    {
        private readonly IBillQueryAppService _queryService;
        private readonly IBillAuditAppService _auditService;

        [ObservableProperty]
        private ObservableCollection<tb_SaleOrder> _orders;

        [ObservableProperty]
        private bool _isRefreshing;

        [ObservableProperty]
        private int _currentPage = 1;

        [ObservableProperty]
        private bool _hasMoreItems = true;

        [ObservableProperty]
        private tb_SaleOrder _selectedOrder;

        public BillListViewModel(
            IBillQueryAppService queryService,
            IBillAuditAppService auditService)
        {
            _queryService = queryService;
            _auditService = auditService;
            Orders = new ObservableCollection<tb_SaleOrder>();
        }

        [RelayCommand]
        private async Task LoadOrdersAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var result = await _queryService.GetSaleOrdersAsync(
                    CurrentPage, 
                    20);

                foreach (var order in result.Items)
                {
                    Orders.Add(order);
                }

                HasMoreItems = Orders.Count < result.TotalCount;
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("错误", $"加载失败：{ex.Message}");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        private async Task SelectOrderAsync(tb_SaleOrder order)
        {
            if (order == null) return;

            // 导航到详情页
            await Shell.Current.GoToAsync($"BillDetailPage?id={order.Id}");
        }

        [RelayCommand]
        private async Task AuditOrderAsync(tb_SaleOrder order)
        {
            if (order == null) return;

            var confirm = await DisplayAlertAsync(
                "确认审核",
                "确定要通过审核吗？此操作不可撤销。",
                "确定",
                "取消");

            if (!confirm) return;

            try
            {
                IsBusy = true;

                var result = await _auditService.AuditSaleOrderAsync(
                    order.Id, 
                    true, 
                    "移动端审核通过");

                if (result.Success)
                {
                    await DisplayAlertAsync("成功", "审核成功");
                    // 刷新列表
                    await LoadOrdersAsync();
                }
                else
                {
                    await DisplayAlertAsync("失败", result.Message);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("错误", $"审核失败：{ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task DisplayAlertAsync(string title, string message, string accept = "确定", string cancel = null)
        {
            // MAUI 的显示对话框方法
            await Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }
    }
}
```

---

## 六、实施路线图（修正版）

### Phase 1：基础框架搭建（3 周）

#### Week 1: 项目创建 + PacketSpec 集成
- [ ] 创建 .NET MAUI 项目
- [ ] 添加 NuGet 包引用
- [ ] 配置项目结构
- [ ] **直接引用 PacketSpec，无需修改其目标框架**

#### Week 2-3: 网络层开发
- [ ] 实现 MobileTcpClient
- [ ] 实现 CommandDispatcher
- [ ] 实现 MobileAuthService
- [ ] 单元测试

**交付物**：可运行的 MAUI 项目骨架，能连接服务器

---

### Phase 2：核心功能开发（4 周）

#### Week 4-5: 单据查询
- [ ] 实现 BillQueryAppService
- [ ] 复用服务器端现有 Query 命令
- [ ] 实现 BillListPage + ViewModel

#### Week 6-7: 单据详情 + 审核
- [ ] 实现 BillDetailPage + ViewModel
- [ ] 实现 BillAuditAppService
- [ ] 复用服务器端现有 Audit 命令
- [ ] 实现 AuditDialog

**交付物**：完整的查询 + 审核功能 APK

---

### Phase 3：优化与测试（3 周）

#### Week 8-9: 缓存 + 离线
- [ ] 实现 MobileDatabaseService
- [ ] 实现缓存策略
- [ ] 弱网模式优化

#### Week 10: 全面测试
- [ ] 功能测试
- [ ] 性能测试
- [ ] 兼容性测试

**交付物**：Release 候选版 APK

---

### Phase 4：试点部署（2 周）

#### Week 11-12: 试点运行
- [ ] 小范围试点（10-20 用户）
- [ ] 收集反馈
- [ ] Bug 修复

**交付物**：正式版 APK

---

## 七、关键技术风险与应对

### 风险 1：PacketSpec 命令模型不兼容
**可能性**：低  
**影响**：中  
**应对**：
- 提前验证 GenericCommand 在 MAUI 中的序列化/反序列化
- 准备适配器模式封装

### 风险 2：SuperSocket 客户端在 MAUI 中不稳定
**可能性**：中  
**影响**：高  
**应对**：
- 使用官方支持的 SuperSocket.ClientEngine
- 实现完善的断线重连机制
- 准备 HTTP 备用方案（仅在紧急情况下使用）

### 风险 3：服务器端 Commands 不支持移动端参数
**可能性**：中  
**影响**：中  
**应对**：
- 提前审查现有 Command Handlers
- 必要时增加参数适配层
- 保持向后兼容

---

## 八、成功标准

### 技术指标：
- ✅ 登录响应时间 < 2 秒
- ✅ 列表加载时间 < 3 秒
- ✅ 审核操作响应时间 < 3 秒
- ✅ 支持 Android 8.0~13

### 质量指标：
- ✅ 严重 Bug 数为 0
- ✅ 单元测试覆盖率 > 70%
- ✅ 代码审查通过率 100%

### 业务指标：
- ✅ 支持销售订单、采购订单、生产订单查询
- ✅ 支持单据审核（通过/驳回）
- ✅ 用户满意度 > 4.5/5

---

## 九、总结

### 核心优势：
1. **零服务器改动**：完全复用现有 SuperSocket 架构
2. **最大代码复用**：90%+ 业务逻辑直接复用
3. **最低风险**：技术栈成熟，无实验性方案
4. **最快速度**：12 周完成一期开发

### 关键决策：
- ✅ 保持 PacketSpec 为 .NET Standard 2.0
- ✅ 复用 SuperSocket TCP，不建 HTTP 网关
- ✅ 直接使用现有 Command Handlers
- ✅ 聚焦移动端 UI 定制开发

---

**文档版本**: v2.0  
**编制日期**: 2026-04-02  
**编制人**: AI Assistant  
**审核状态**: 待审核
