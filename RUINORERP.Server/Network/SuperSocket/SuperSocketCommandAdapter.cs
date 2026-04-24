using Azure;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RUINORERP.PacketSpec;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Errors;
using RUINORERP.PacketSpec.Models.Authentication;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Lock;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.Server; // 添加对Program类所在命名空间的引用
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.Network.Models;
using RUINORERP.Server.Network.Services;
using RUINORERP.Server.Services; // 内存监控服务
using SuperSocket.Command;
using SuperSocket.Server.Abstractions.Session;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RUINORERP.Server.Network.SuperSocket
{
    /// <summary>
    /// 统一的SuperSocket命令适配器
    /// 整合了原有的SimplifiedSuperSocketAdapter、SocketCommand和SuperSocketCommandAdapter的功能
    /// 
    /// 工作流程：
    /// 1. SuperSocket接收到来自客户端的数据包
    /// 2. SuperSocketCommandAdapter.ExecuteAsync方法被调用
    /// 3. 从CommandDispatcher获取已注册的命令类型映射
    /// 4. 根据数据包中的命令ID创建对应的命令实例
    /// 5. 通过CommandDispatcher.DispatchAsync方法分发命令给相应的处理器
    /// 6. 处理结果通过网络返回给客户端
    /// </summary>
    [Command(Key = "SuperSocketCommandAdapter")]
    public class SuperSocketCommandAdapter<TAppSession> : IAsyncCommand<TAppSession, ServerPackageInfo>
        where TAppSession : IAppSession
    {
        private readonly CommandDispatcher _commandDispatcher;
        private readonly ILogger<SuperSocketCommandAdapter> _logger;
        private readonly ISessionService _sessionService;
        private readonly IServiceProvider _serviceProvider; // 添加服务提供者字段
        private readonly IClientResponseHandler _clientResponseHandler; // 客户端响应处理器
        private readonly MemoryMonitoringService _memoryMonitoringService; // 内存监控服务（用于熔断）

        /// <summary>
        /// Token验证缓存（TTL: 30秒）
        /// Key: AccessToken, Value: (验证结果, 缓存时间)
        /// </summary>
        private static readonly ConcurrentDictionary<string, (TokenValidationResult Result, DateTime CachedTime)> _tokenValidationCache 
            = new ConcurrentDictionary<string, (TokenValidationResult, DateTime)>();

        // P1-6修复: 按命令类别配置不同的超时时间(秒)
        // 根据CommandCategory枚举定义,不同类别的命令设置合理的超时时间
        private static readonly Dictionary<CommandCategory, int> CategoryTimeouts = new Dictionary<CommandCategory, int>
        {
            // 快速命令 (5-10秒): 心跳、Ping等保活命令
            { CommandCategory.System, 10 },
            { CommandCategory.Authentication, 10 },
            
            // 普通查询 (30秒): 缓存查询、消息查询等
            { CommandCategory.Cache, 30 },
            { CommandCategory.Message, 30 },
            
            // 数据操作 (60秒): 工作流、数据同步等
            { CommandCategory.Workflow, 60 },
            { CommandCategory.DataSync, 60 },
            { CommandCategory.Config, 60 },
            
            // 文件操作 (120秒): 文件上传下载
            { CommandCategory.File, 120 },
            
            // 锁管理 (30秒): 锁定解锁操作需要快速响应
            { CommandCategory.Lock, 30 },
            
            // 业务编码 (30秒): 编号生成
            { CommandCategory.BizCode, 30 },
            
            // 系统管理 (180秒): 服务器管理等复杂操作
            { CommandCategory.SystemManagement, 180 },
            { CommandCategory.Management, 180 },
            
            // 特殊功能 (180秒): 复合型命令等
            { CommandCategory.Composite, 180 },
            { CommandCategory.Special, 180 },
            
            // 连接管理 (10秒): 连接相关命令需要快速响应
            { CommandCategory.Connection, 10 },
            
            // 异常处理 (30秒)
            { CommandCategory.Exception, 30 }
        };

        /// <summary>
        /// 会话服务
        /// </summary>
        private ISessionService SessionService => _sessionService;

        /// <summary>
        /// 网络监控开关
        /// </summary>
        public static bool IsNetworkMonitorEnabled { get; private set; }

        /// <summary>
        /// 命令过滤器 - 存储需要监控的命令代码列表
        /// </summary>
        private static readonly ConcurrentBag<ushort> _commandFilters = new ConcurrentBag<ushort>();

        /// <summary>
        /// 设置网络监控开关状态
        /// </summary>
        /// <param name="enabled">是否启用</param>
        public static void SetNetworkMonitorEnabled(bool enabled)
        {
            IsNetworkMonitorEnabled = enabled;
        }

        /// <summary>
        /// 设置命令过滤器
        /// </summary>
        /// <param name="commandCodes">需要监控的命令代码列表</param>
        public static void SetCommandFilters(IEnumerable<ushort> commandCodes)
        {
            // 清空现有过滤器
            _commandFilters.Clear();

            // 添加新的过滤器
            if (commandCodes != null)
            {
                foreach (var code in commandCodes)
                {
                    _commandFilters.Add(code);
                }
            }
        }

        /// <summary>
        /// 检查命令是否应该被监控
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <returns>是否应该监控该命令</returns>
        private static bool ShouldMonitorCommand(CommandId commandId)
        {
            // 如果没有设置过滤器，则监控所有命令
            if (_commandFilters.IsEmpty)
            {
                return true;
            }

            // 检查命令代码是否在过滤器中
            return _commandFilters.Contains((ushort)commandId);
        }

        /// <summary>
        /// P1-6修复: 根据命令类别获取超时时间(秒)
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <returns>超时时间(秒)</returns>
        private static int GetCommandTimeout(CommandId? commandId)
        {
            if (!commandId.HasValue)
                return 300; // 默认超时300秒

            var category = commandId.Value.Category;
            
            // 尝试从类别配置中获取超时时间
            if (CategoryTimeouts.TryGetValue(category, out int timeout))
                return timeout;

            // 未配置的类别使用默认超时
            return 300;
        }

        /// <summary>
        /// P0优化：判断是否为核心指令（内存压力下仍需处理）
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <returns>是否为核心指令</returns>
        private static bool IsCriticalCommand(CommandId? commandId)
        {
            if (!commandId.HasValue)
                return false;

            // 核心指令白名单：保活、认证、登出
            return commandId.Value == SystemCommands.Heartbeat ||
                   commandId.Value == AuthenticationCommands.Login ||
                   commandId.Value == AuthenticationCommands.Logout ||
                   commandId.Value == SystemCommands.WelcomeAck;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commandDispatcher">命令调度器</param>
        /// <param name="sessionService">会话服务</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="clientResponseHandler">客户端响应处理器</param>
        /// <param name="serviceProvider">服务提供者</param>
        /// <param name="memoryMonitoringService">内存监控服务（可选，用于熔断）</param>
        public SuperSocketCommandAdapter(
            CommandDispatcher commandDispatcher,
            ISessionService sessionService,
            ILogger<SuperSocketCommandAdapter> logger = null,
            IClientResponseHandler clientResponseHandler = null,
            IServiceProvider serviceProvider = null,
            MemoryMonitoringService memoryMonitoringService = null)
        {
            _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
            _logger = logger;
            _clientResponseHandler = clientResponseHandler;

            // 优先使用注入的服务提供者，如果没有则使用全局服务提供者
            _serviceProvider = serviceProvider ?? Program.ServiceProvider;
            
            // 注入内存监控服务（用于熔断机制）
            _memoryMonitoringService = memoryMonitoringService;

            // 确保命令调度器使用相同的服务提供者
            if (_commandDispatcher != null && _serviceProvider != null)
            {
                _commandDispatcher.ServiceProvider = _serviceProvider;
                _logger?.LogDebug("SuperSocketCommandAdapter已配置为使用全局服务提供者");
            }
        }

        /// <summary>
        /// 执行命令
        /// 将 SuperSocket 的命令调用转换为现有的命令处理系统
        /// </summary>
        /// <param name="session">SuperSocket 会话</param>
        /// <param name="package">数据包</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>执行结果任务</returns>
        public async ValueTask ExecuteAsync(TAppSession session, ServerPackageInfo package, CancellationToken cancellationToken)
        {
            // 网络监控：接收数据包
            if (IsNetworkMonitorEnabled && package?.Packet != null && ShouldMonitorCommand(package.Packet.CommandId))
            {
                _logger?.LogDebug("[网络监控] 接收数据包: SessionId={SessionId}, CommandId={CommandId}, RequestId={RequestId}, PacketId={PacketId}",
                    session.SessionID, package.Packet.CommandId.ToString(), 
                    package.Packet.Request?.RequestId ?? package.Packet.Response?.RequestId,
                    package.Packet.PacketId);
            }
        
            if (package == null)
            {
                _logger?.LogWarning("接收到空的数据包");
                _logger?.LogWarning($"[主动断开连接] 接收到空数据包，准备关闭连接: SessionId={session.SessionID}");
                await SendErrorResponseAsync(session, package, UnifiedErrorCodes.System_InternalError, CancellationToken.None);
                return;
            }
        
            // P0优化：内存熔断检查 - 在内存压力下拒绝非核心请求
            if (_memoryMonitoringService != null && _memoryMonitoringService.IsUnderMemoryPressure)
            {
                var commandId = package.Packet?.CommandId;
                if (!IsCriticalCommand(commandId))
                {
                    _logger?.LogWarning("[内存熔断] 服务器处于内存压力状态，拒绝非核心请求: SessionId={SessionId}, CommandId={CommandId}",
                        session.SessionID, commandId?.ToString() ?? "Unknown");
                    await SendErrorResponseAsync(session, package, UnifiedErrorCodes.System_ResourceBusy, CancellationToken.None);
                    return; // 立即返回，不进入调度器，避免雪崩效应
                }
            }


            try
            {
                if (package.Packet.ExecutionContext.ExpectedResponseTypeName == "IResponse" && IsNetworkMonitorEnabled && ShouldMonitorCommand(package.Packet.CommandId))
                {
                    _logger?.LogDebug("[网络监控] 接收数据包的返回类型没有指定: SessionId={SessionId}, CommandId={CommandId}, PacketId={PacketId}",
                        session.SessionID, package.Packet.CommandId.ToString(), package.Packet.PacketId);
                }

                if (package.Packet.CommandId == LockCommands.Lock)
                {
                    // 锁命令特殊处理逻辑（如需要）
                }

                if (package.Packet.CommandId == SystemCommands.WelcomeAck)
                {
                    // 欢迎确认命令特殊处理逻辑（如需要）
                }

                // 检查是否是响应包（带有RequestId的响应包）
                //一般是客户端响应服务器请求的数据包才使用，否则通过调度器处理（客户端主动请求服务器的命令）
                if (IsResponsePacket(package.Packet))
                {
                    // 处理响应包
                    await HandleResponsePacketAsync(package.Packet);
                    return;
                }

                // 会话ID设置：适用于所有命令
                package.Packet.SessionId = session.SessionID;
                package.Packet.ExecutionContext.SessionId = session.SessionID;

                // 所有命令的数据大小检查：在调试模式下显示提示，不影响正常处理
                try
                {
                    // 获取请求数据
                    if (package.Packet.Request != null)
                    {
                        // 序列化请求数据，计算大小
                        var requestJson = Newtonsoft.Json.JsonConvert.SerializeObject(package.Packet.Request);
                        var requestSize = System.Text.Encoding.UTF8.GetByteCount(requestJson);

                        // 设置合理的阈值，比如1MB
                        const long DATA_SIZE_THRESHOLD = 1024 * 1024; // 1MB

                        if (requestSize > DATA_SIZE_THRESHOLD)
                        {
                            // 在调试模式下显示提示给管理员
                            _logger?.LogWarning("[数据过大] 会话 {SessionId} 的 {CommandId} 命令请求数据大小为 {Size} 字节，超过阈值 {Threshold} 字节",
                                session.SessionID, package.Packet.CommandId.ToString(), requestSize, DATA_SIZE_THRESHOLD);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // 捕获可能的异常，不影响正常处理
                    _logger?.LogDebug("[数据大小检查] 检查请求数据大小异常: {Exception}", ex.Message);
                }

                // 检查命令调度器初始化状态
                // 注意：CommandDispatcher应该在NetworkServer启动时已经初始化，这里只是验证
                if (!_commandDispatcher.IsInitialized)
                {
                    _logger?.LogCritical("严重错误：CommandDispatcher未初始化！这表明NetworkServer启动流程存在问题。");
                    await SendErrorResponseAsync(session, package, UnifiedErrorCodes.System_InternalError, CancellationToken.None);
                    return;
                }

                // 记录处理器数量，便于调试和监控
                if (_commandDispatcher.HandlerCount == 0)
                {
                    _logger?.LogWarning("CommandDispatcher已初始化但没有注册任何处理器！");
                }
                else
                {
                    _logger?.LogDebug("CommandDispatcher状态正常，已注册处理器数量: {HandlerCount}",
                        _commandDispatcher.HandlerCount);
                }

                // 获取现有会话信息
                var sessionId = session.SessionID;
                var sessionInfo = SessionService.GetSession(sessionId);
                
                // 检查会话是否存在
                if (sessionInfo == null)
                {
                    // 会话不存在，可能是连接已断开或会话已过期
                    _logger?.LogWarning("会话不存在或已过期: SessionId={SessionId}", sessionId);
                    await SendErrorResponseAsync(session, package, UnifiedErrorCodes.Auth_SessionExpired, CancellationToken.None);
                    return;
                }

                // 检查会话是否已验证（适用于非WelcomeAck和非Login命令）
                if (!sessionInfo.IsVerified &&
                    package.Packet.CommandId != SystemCommands.WelcomeAck &&
                    package.Packet.CommandId != AuthenticationCommands.Login)
                {
                    // 会话存在但未验证，返回相应错误
                    _logger?.LogWarning("[认证验证失败] SessionId={SessionId}, CommandId={CommandId}, IsVerified={IsVerified}, WelcomeAckReceived={WelcomeAckReceived}",
                        sessionId, package.Packet.CommandId.ToString(), sessionInfo.IsVerified, sessionInfo.WelcomeAckReceived);
                    await SendErrorResponseAsync(session, package, UnifiedErrorCodes.Auth_ValidationFailed, CancellationToken.None);
                    return;
                }

                // ✅ Token验证：除WelcomeAck和Login外，所有命令都需要有效的Token
                if (package.Packet.CommandId != SystemCommands.WelcomeAck &&
                    package.Packet.CommandId != AuthenticationCommands.Login)
                {
                    var validationResult = ValidateTokenWithCache(package.Packet.ExecutionContext?.Token);
                    if (!validationResult.IsValid)
                    {
                        _logger?.LogWarning("[Token验证失败] SessionId={SessionId}, CommandId={CommandId}, Error={ErrorMessage}, UserId={UserId}",
                            sessionId, 
                            package.Packet.CommandId.ToString(), 
                            validationResult.ErrorMessage,
                            sessionInfo.UserId);
                        
                        // 根据错误类型返回不同的错误码
                        var errorCode = validationResult.ErrorMessage.Contains("过期") 
                            ? UnifiedErrorCodes.Auth_TokenExpired 
                            : UnifiedErrorCodes.Auth_TokenInvalid;
                            
                        await SendErrorResponseAsync(session, package, errorCode, CancellationToken.None);
                        return;
                    }
                }

                // 更新会话的最后活动时间
                sessionInfo.UpdateActivity();
                SessionService.UpdateSession(sessionInfo);
                SessionService.UpdateSessionActivity(session.SessionID);
                IResponse result;
                try
                {
                    // 使用链接的取消令牌，考虑命令超时设置
                    // 修复：正确创建链接令牌源，避免修改原始cancellationToken
                    CancellationTokenSource linkedCts;
                    if (cancellationToken != CancellationToken.None)
                    {
                        // 如果传入了有效的取消令牌，则创建链接令牌源
                        linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                    }
                    else
                    {
                        // 否则创建新的令牌源
                        linkedCts = new CancellationTokenSource();
                    }

                    // P1-6修复: 根据命令类别配置不同的超时时间
                    var timeoutSeconds = GetCommandTimeout(package.Packet?.CommandId);
                    var timeout = TimeSpan.FromSeconds(timeoutSeconds);
                    linkedCts.CancelAfter(timeout);

                    // 确保命令执行上下文包含全局服务提供者
                    if (package.Packet?.ExecutionContext != null)
                    {
                        //package.Packet.ExecutionContext.ServiceProvider = _serviceProvider;
                    }

                    result = await _commandDispatcher.DispatchAsync(package.Packet, linkedCts.Token).ConfigureAwait(false);

                    if (result is LockResponse)
                    {

                    }
                    // 释放令牌源资源
                    linkedCts.Dispose();
                }
                catch (OperationCanceledException ex)
                {
                    _logger?.LogError(ex, "命令执行超时或被取消: CommandId={CommandId}", package.Packet.CommandId);
                    result = ResponseFactory.CreateSpecificErrorResponse(package.Packet.ExecutionContext, UnifiedErrorCodes.System_Timeout.Message);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "命令执行异常: CommandId={CommandId}", package.Packet.CommandId);
                    result = ResponseFactory.CreateSpecificErrorResponse(package.Packet.ExecutionContext, UnifiedErrorCodes.System_InternalError.Message);
                }
                if (result == null)
                {
                    result = ResponseFactory.CreateSpecificErrorResponse(package.Packet.ExecutionContext, UnifiedErrorCodes.System_InternalError.Message);
                }

                // 网络监控：命令处理完成
                if (IsNetworkMonitorEnabled && ShouldMonitorCommand(package.Packet.CommandId))
                {
                    _logger?.LogDebug("[网络监控] 命令处理完成: SessionId={SessionId}, CommandId={CommandId}, Success={Success}",
                        session.SessionID, package.Packet.CommandId.ToString(), result.IsSuccess);
                }

                // 使用新的 CancellationToken.None 来确保响应能够发送，即使命令处理超时
                await HandleCommandResultAsync(session, package, result, CancellationToken.None);

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理SuperSocket命令时发生异常: CommandId={CommandId}", package.Packet.CommandId);
                await SendErrorResponseAsync(session, package, UnifiedErrorCodes.System_InternalError, CancellationToken.None);
            }
        }

        /// <summary>
        /// 判断是否为响应数据包,这里意思是先优先处理：服务器请求，客户端响应服务器的数据。
        /// 如果是客户端请求的，则通过调度器处理
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>是否为响应数据包</returns>
        private bool IsResponsePacket(PacketModel packet)
        {
            // 响应包通常包含请求ID，并且是客户端对服务器请求的响应
            return !string.IsNullOrEmpty(packet?.ExecutionContext?.RequestId) &&
                   packet.Direction == PacketDirection.ClientResponse;
        }

        /// <summary>
        /// 处理响应数据包
        /// </summary>
        /// <param name="packet">响应数据包</param>
        private async Task HandleResponsePacketAsync(PacketModel packet)
        {
            try
            {
                var sessionId = packet?.ExecutionContext?.SessionId;
                if (string.IsNullOrEmpty(sessionId))
                {
                    _logger?.LogWarning("响应包缺少会话ID");
                    return;
                }

                // 获取会话信息
                var sessionInfo = SessionService.GetSession(sessionId);
                if (sessionInfo == null)
                {
                    _logger?.LogWarning("响应包对应的会话不存在: {SessionId}", sessionId);
                    return;
                }

                var requestId = packet?.ExecutionContext?.RequestId;

                // ✅ 优先匹配待处理请求（SendCommandAndWaitForResponseAsync 模式）
                if (!string.IsNullOrEmpty(requestId))
                {
                    if (SessionService is SessionService sessionServiceConcrete)
                    {
                        var matched = sessionServiceConcrete.TryCompletePendingRequest(requestId, packet);
                        if (matched)
                        {
                            _logger?.LogDebug("匹配到待处理请求，请求ID: {RequestId}", requestId);
                            return;
                        }
                    }
                }

                // 使用 ClientResponseHandler 处理响应
                if (_clientResponseHandler != null)
                {
                    var result = await _clientResponseHandler.HandleResponseAsync(packet, sessionInfo);
                    if (!result.IsSuccess)
                    {
                        _logger?.LogWarning("客户端响应处理失败: {ErrorMessage}", result.ErrorMessage);
                    }
                }

                _logger?.LogDebug("处理响应包完成，请求ID: {RequestId}", requestId);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理响应包时发生错误");
            }
        }

        /// <summary>
        /// 处理命令执行结果
        /// </summary>
        /// <param name="session">SuperSocket会话</param>
        /// <param name="requestPackage">请求数据包</param>
        /// <param name="result">命令执行结果</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果任务</returns>
        protected virtual async ValueTask HandleCommandResultAsync(
            TAppSession session,
            ServerPackageInfo requestPackage,
            IResponse response,
            CancellationToken cancellationToken)
        {
            if (response == null)
            {
                await SendErrorResponseAsync(session, requestPackage, UnifiedErrorCodes.System_InternalError, CancellationToken.None);
                return;
            }
            if (response is LockResponse)
            {

            }
            if (response is LoginResponse)
            {

            }
            if (response != null && response.IsSuccess)
            {
                // 命令执行成功，发送成功响应
                UpdatePacketWithResponse(requestPackage.Packet, response);
                await SendResponseAsync(session, requestPackage.Packet, CancellationToken.None);
            }
            else
            {
                // 命令执行失败，发送增强的错误响应
                // 从结果中提取所有错误信息，包括元数据中的详细信息
                await SendEnhancedErrorResponseAsync(session, requestPackage, response, UnifiedErrorCodes.Biz_DataNotFound, CancellationToken.None);
            }
        }

        /// <summary>
        /// 附加响应数据到数据包发回客户端
        /// </summary>
        /// <param name="package"></param>
        /// <param name="command"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual PacketModel UpdatePacketWithResponse(PacketModel package, IResponse result)
        {
            // 检查响应数据是否为空，避免空引用异常
            if (result == null)
            {
                _logger?.LogWarning("[UpdatePacketWithResponse] 响应对象为null - CommandId: {CommandId}, PacketId: {PacketId}",
                    package.CommandId, package.PacketId);
                package.Status = PacketStatus.Failed;
            }
            else
            {
                package.Status = result.IsSuccess ? PacketStatus.Completed : PacketStatus.Failed;
                
                // ✅ 详细日志记录响应类型
                _logger?.LogDebug("[UpdatePacketWithResponse] 设置响应 - CommandId: {CommandId}, ResponseType: {ResponseType}, IsSuccess: {IsSuccess}",
                    package.CommandId, result.GetType().Name, result.IsSuccess);
            }
            package.Response = result;
            package.Request = null;

            package.PacketId = IdGenerator.GenerateResponseId(package.PacketId);
            package.Direction = PacketDirection.ServerResponse; // 明确设置为响应方向
            package.SessionId = package.SessionId;

            // 设置请求ID 配对响应
            if (package.Request != null)
            {
                package.ExecutionContext.RequestId = package.Request.RequestId;
            }
            // 添加元数据
            if (result.Metadata != null && result.Metadata.Count > 0)
            {
                package.Extensions = new JObject();
                foreach (var metadata in result.Metadata)
                {
                    package.Extensions[metadata.Key] = JToken.FromObject(metadata.Value);
                }
            }

            return package;
        }

        /// <summary>
        /// 发送响应
        /// </summary>
        /// <param name="session">SuperSocket会话</param>
        /// <param name="package">数据包</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>发送结果任务</returns>
        protected virtual async ValueTask SendResponseAsync(TAppSession session, PacketModel package, CancellationToken cancellationToken)
        {
            try
            {
                // 检查会话是否有效
                if (session == null)
                {
                    _logger?.LogWarning("尝试发送响应到空会话: PacketId={PacketId}, CommandId={CommandId}",
                        package.PacketId, package.CommandId);
                    return;
                }
                package.SessionId = session.SessionID;
                package.Direction = PacketDirection.ServerResponse;
                var serializedData = JsonCompressionSerializationService.Serialize<PacketModel>(package);

                // 加密数据
                var originalData = new OriginalData((byte)package.CommandId.Category, new byte[] { package.CommandId.OperationCode },
                    serializedData
                );
                var encryptedData = PacketSpec.Security.UnifiedEncryptionProtocol.EncryptServerDataToClient(originalData);

                // 发送数据并捕获可能的异常
                try
                {
                    // 网络监控：发送响应
                    if (IsNetworkMonitorEnabled && ShouldMonitorCommand(package.CommandId))
                    {
                        _logger?.LogDebug("[网络监控] 发送响应: SessionId={SessionId}, CommandId={CommandId}, PacketId={PacketId}",
                            package.SessionId, package.CommandId.ToString(), package.PacketId);
                    }

                    await (session as SessionInfo).SendAsync(encryptedData.ToArray(), cancellationToken);
                }
                catch (TaskCanceledException ex) when (ex.InnerException is OperationCanceledException && cancellationToken.IsCancellationRequested)
                {
                    // 处理任务被取消的特定异常
                    _logger?.LogWarning(ex, "发送响应被取消: SessionId={SessionId}, PacketId={PacketId}",
                        package.SessionId, package.PacketId);
                    // 忽略此异常，因为可能是正常的超时或取消操作
                }
                catch (InvalidOperationException ex) when (ex.Message.Contains("Writing is not allowed after writer was completed"))
                {
                    // 处理管道写入器已完成的特定异常
                    _logger?.LogWarning(ex, "管道写入器已完成，无法发送响应: SessionId={SessionId}, PacketId={PacketId}",
                        package.SessionId, package.PacketId);
                    // 忽略此异常，因为会话可能已经关闭
                }
                catch (Exception ex)
                {
                    // 记录其他发送异常
                    _logger?.LogError(ex, "发送响应时发生异常: SessionId={SessionId}, PacketId={PacketId}",
                        package.SessionId, package.PacketId);
                    // 可以选择是否向上传播异常
                    // throw;
                }
            }
            catch (Exception ex)
            {
                // 捕获所有其他异常以确保方法不会失败
                _logger?.LogError(ex, "处理响应发送时发生未预期的异常");
            }
        }

        /// <summary>
        /// 发送错误响应（兼容旧版调用的方法）
        /// </summary>
        /// <param name="session">SuperSocket会话</param>
        /// <param name="requestPackage">请求数据包</param>
        /// <param name="errorCode">错误代码</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>发送结果任务</returns>
        protected virtual async ValueTask SendErrorResponseAsync(
            TAppSession session,
            ServerPackageInfo requestPackage,
            ErrorCode errorCode,
            CancellationToken cancellationToken)
        {
            // 调用增强版本的错误响应方法，传入null作为result参数
            await SendEnhancedErrorResponseAsync(session, requestPackage, null, errorCode, cancellationToken);
        }

        /// <summary>
        /// 从响应结果中提取错误代码信息
        /// </summary>
        /// <param name="result">响应结果</param>
        /// <returns>错误代码对象</returns>
        protected virtual ErrorCode ExtractErrorCodeFromResponse(IResponse result)
        {
            if (result == null)
            {
                return UnifiedErrorCodes.System_InternalError;
            }

            // 检查响应数据是否为空
            if (result == null)
            {
                _logger?.LogWarning("响应数据为空，使用默认错误信息");
                return UnifiedErrorCodes.System_InternalError;
            }

            // 优先使用响应中的元数据提取更详细的错误信息
            string detailedMessage = result.ErrorMessage;

            if (result.Metadata != null)
            {
                // 尝试获取更详细的错误信息
                if (result.Metadata.TryGetValue("Exception", out var exceptionObj))
                {
                    // 使用空值条件运算符避免空引用异常
                    string responseMessage = result?.Message ?? "未知错误";
                    detailedMessage = $"{responseMessage} | Exception: {exceptionObj}";
                }
            }

            // 直接使用响应中的错误代码创建错误代码对象
            return new ErrorCode(0, detailedMessage);
        }

        /// <summary>
        /// 发送增强的错误响应，包含命令处理结果中的所有错误信息
        /// </summary>
        /// <param name="session">SuperSocket会话</param>
        /// <param name="requestPackage">请求数据包</param>
        /// <param name="result">命令处理结果（可选）</param>
        /// <param name="errorCode">错误代码</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>发送结果任务</returns>
        protected virtual async ValueTask SendEnhancedErrorResponseAsync(
            TAppSession session,
            ServerPackageInfo requestPackage,
            IResponse result,
            ErrorCode errorCode,
            CancellationToken cancellationToken)
        {
            // 获取原始请求的RequestId
            string originalRequestId = requestPackage.Packet?.Request?.RequestId;

            // 创建错误响应包 - 使用对象池（优化GC）
            var errorResponse = PacketModelPool.Rent();
            errorResponse.PacketId = IdGenerator.GenerateResponseId(requestPackage.Packet?.PacketId ?? Guid.NewGuid().ToString());
            errorResponse.Direction = PacketDirection.ServerResponse;
            errorResponse.SessionId = requestPackage.Packet?.SessionId;
            errorResponse.Status = PacketStatus.Failed;
            errorResponse.ExecutionContext = new CommandContext
            {
                RequestId = originalRequestId ?? string.Empty
            };
            errorResponse.Extensions = new JObject
            {
                ["ErrorCode"] = errorCode.Code,
                ["ErrorMessage"] = errorCode.Message,
                ["Success"] = false,
                ["RequestId"] = originalRequestId ?? string.Empty
            };

            // 设置请求ID 配对响应
            if (requestPackage.Packet.Request != null)
            {
                requestPackage.Packet.ExecutionContext.RequestId = requestPackage.Packet.Request.RequestId;
            }

            // 如果提供了result参数，则添加增强的错误信息
            if (result != null)
            {
                // 添加原始错误信息
                errorResponse.Extensions["OriginalErrorMessage"] = result.ErrorMessage ?? "无错误消息";
                errorResponse.Extensions["OriginalMessage"] = result.Message ?? "无消息";

                // 设置响应对象 - 使用CreateCommandSpecificResponse确保返回正确类型的响应
                // 这样客户端在使用as TResponse转换时才能成功
                //if (result is ResponseBase responseBase)
                //{
                //    errorResponse.Response = responseBase;
                //}
                //else
                //{
                errorResponse.Response = result;
                //}

                // 添加元数据中的所有错误信息
                if (result.Metadata != null && result.Metadata.Count > 0)
                {
                    foreach (var metadata in result.Metadata)
                    {
                        // 避免重复添加已经存在的键
                        if (!errorResponse.Extensions.ContainsKey(metadata.Key))
                        {
                            errorResponse.Extensions[metadata.Key] = JToken.FromObject(metadata.Value);
                        }
                    }
                }
            }

            // 确保请求ID在响应中正确传递
            if (!string.IsNullOrEmpty(originalRequestId))
            {
                errorResponse.Extensions["RequestId"] = originalRequestId;
            }

            // 如果请求包中包含RequestId，则在响应包中保留它，以便客户端匹配请求和响应
            if (requestPackage.Packet?.Extensions?.TryGetValue("RequestId", out var requestIdFromExtensions) == true)
            {
                errorResponse.Extensions["RequestIdFromExtensions"] = JToken.FromObject(requestIdFromExtensions);
            }

            // 记录详细的错误信息用于调试
            _logger?.LogWarning("发送错误响应: ErrorCode={ErrorCode}, ErrorMessage={ErrorMessage}, MetadataKeys=[{MetadataKeys}]",
                errorCode.Code, errorCode.Message,
                result?.Metadata != null ? string.Join(", ", result.Metadata.Keys) : "none");




            // 发送响应
            await SendResponseAsync(session, errorResponse, cancellationToken);
            
            // 归还对象池（优化GC）
            PacketModelPool.Return(errorResponse);
        }

        /// <summary>
        /// 验证Token有效性（带缓存）
        /// </summary>
        /// <param name="tokenInfo">Token信息</param>
        /// <returns>验证结果</returns>
        private TokenValidationResult ValidateTokenWithCache(TokenInfo tokenInfo)
        {
            if (tokenInfo == null || string.IsNullOrEmpty(tokenInfo.AccessToken))
            {
                return new TokenValidationResult 
                { 
                    IsValid = false, 
                    ErrorMessage = "Token为空" 
                };
            }

            var cacheKey = tokenInfo.AccessToken;
            
            // 检查缓存（TTL: 30秒）
            if (_tokenValidationCache.TryGetValue(cacheKey, out var cached))
            {
                if ((DateTime.Now - cached.CachedTime).TotalSeconds < 30)
                {
                    return cached.Result;
                }
                else
                {
                    _tokenValidationCache.TryRemove(cacheKey, out _);
                }
            }

            // 执行验证
            var result = ValidateToken(tokenInfo);
            
            // 仅缓存有效Token
            if (result.IsValid)
            {
                _tokenValidationCache.TryAdd(cacheKey, (result, DateTime.Now));
            }

            return result;
        }

        /// <summary>
        /// 验证Token有效性（实际验证逻辑）
        /// </summary>
        /// <param name="tokenInfo">Token信息</param>
        /// <returns>验证结果</returns>
        private TokenValidationResult ValidateToken(TokenInfo tokenInfo)
        {
            try
            {
                var tokenService = Startup.GetFromFac<ITokenService>();
                if (tokenService == null)
                {
                    _logger?.LogError("无法获取ITokenService实例");
                    return new TokenValidationResult 
                    { 
                        IsValid = false, 
                        ErrorMessage = "Token服务不可用" 
                    };
                }

                return tokenService.ValidateToken(tokenInfo.AccessToken);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Token验证异常");
                return new TokenValidationResult 
                { 
                    IsValid = false, 
                    ErrorMessage = $"Token验证异常: {ex.Message}" 
                };
            }
        }
    }

    /// <summary>
    /// 非泛型版本的统一 SuperSocket 命令适配器，便于在不需要指定会话类型的场景中使用
    /// </summary>
    [Command(Key = "SuperSocketCommandAdapter")]
    public class SuperSocketCommandAdapter : SuperSocketCommandAdapter<IAppSession>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commandDispatcher">命令调度器</param>
        /// <param name="sessionService">会话服务</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="clientResponseHandler">客户端响应处理器</param>
        /// <param name="serviceProvider">服务提供者</param>
        /// <param name="memoryMonitoringService">内存监控服务（可选，用于熔断）</param>
        public SuperSocketCommandAdapter(
            CommandDispatcher commandDispatcher,
            ISessionService sessionService,
            ILogger<SuperSocketCommandAdapter> logger = null,
            IClientResponseHandler clientResponseHandler = null,
            IServiceProvider serviceProvider = null,
            MemoryMonitoringService memoryMonitoringService = null)
            : base(commandDispatcher, sessionService, logger, clientResponseHandler, serviceProvider, memoryMonitoringService)
        { }
    }
}