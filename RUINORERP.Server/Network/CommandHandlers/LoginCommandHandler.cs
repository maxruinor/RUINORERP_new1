using Azure.Core;
using HLH.Lib.Security;
using log4net.Core;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualBasic.ApplicationServices;
using RUINORERP.Common.Extensions;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model;
 
using RUINORERP.PacketSpec;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Commands.Cache;
using RUINORERP.PacketSpec.Errors;
using RUINORERP.PacketSpec.Handlers;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Authentication;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Message;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.Server;
using RUINORERP.Server.Adapters;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.Network.Models;
using RUINORERP.Server.Network.Services;
using RUINORERP.Server.Services;
using SuperSocket.Channel;
using SuperSocket.Connection;
using SuperSocket.Server.Abstractions.Session;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO.Packaging;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ZXing.Common.ReedSolomon;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace RUINORERP.Server.Network.CommandHandlers
{
    /// <summary>
    /// 统一登录命令处理器 - 整合了命令模式和处理器模式的登录处理
    /// 包含重复登录检查、人数限制、黑名单验证、Token验证和刷新机制1
    /// </summary>
    [CommandHandler("LoginCommandHandler", priority: 100)]
    public class LoginCommandHandler : BaseCommandHandler
    {
        private const int MaxLoginAttempts = 5;
        private const int DefaultMaxConcurrentUsers = 1000;
        
        /// <summary>
        /// 最大并发用户数缓存
        /// </summary>
        private int? _cachedMaxConcurrentUsers;
        private DateTime _cacheExpiryTime;
        private readonly object _maxUsersCacheLock = new object();
        private readonly TimeSpan _maxUsersCacheDuration = TimeSpan.FromMinutes(5);
        
        /// <summary>
        /// 获取最大并发用户数（异步+缓存优化版）
        /// 优先从注册信息中获取，如果注册服务不可用则使用默认值
        /// </summary>
        private async Task<int> GetMaxConcurrentUsersAsync()
        {
            lock (_maxUsersCacheLock)
            {
                if (_cachedMaxConcurrentUsers.HasValue && DateTime.Now < _cacheExpiryTime)
                {
                    return _cachedMaxConcurrentUsers.Value;
                }
            }
            
            try
            {
                if (RegistrationService != null)
                {
                    var validationResult = await RegistrationService.ValidateSystemRegistrationAsync();
                    if (validationResult?.RegistrationInfo != null && validationResult.IsValid)
                    {
                        var concurrentUsers = validationResult.RegistrationInfo.ConcurrentUsers;
                        
                        lock (_maxUsersCacheLock)
                        {
                            _cachedMaxConcurrentUsers = concurrentUsers;
                            _cacheExpiryTime = DateTime.Now.Add(_maxUsersCacheDuration);
                        }
                        
                        return concurrentUsers;
                    }
                }
            }
            catch (Exception ex)
            {
                logger?.LogWarning(ex, "获取注册信息中的并发用户数失败，使用默认值");
            }
            
            return DefaultMaxConcurrentUsers;
        }
        private static readonly ConcurrentDictionary<string, int> _loginAttempts = new ConcurrentDictionary<string, int>();
        /// <summary>
        /// 记录每个用户名的首次登录尝试时间（用于保守式清理）
        /// </summary>
        private static readonly ConcurrentDictionary<string, DateTime> _loginAttemptTimes = new ConcurrentDictionary<string, DateTime>();
        private static readonly object _lock = new object();
        /// <summary>
        /// 登录尝试记录过期时间（30分钟）
        /// </summary>
        private static readonly TimeSpan LoginAttemptExpiryDuration = TimeSpan.FromMinutes(30);
        /// <summary>
        /// 保守清理阈值：失败次数少于该值才清理（避免误清理正在暴力破解的账号）
        /// </summary>
        private const int ConservativeCleanupThreshold = 3;
        /// <summary>
        /// 是否已启动清理定时器
        /// </summary>
        private static bool _cleanupTimerStarted = false;
        /// <summary>
        /// 日志记录器
        /// </summary>
        protected ILogger<LoginCommandHandler> logger { get; set; }
        protected ServerMessageService MessageService { get; set; }


        protected SystemManagementService managementService { get; set; }
        /// <summary>
        /// 会话管理服务
        /// </summary>
        protected ISessionService SessionService { get; set; }

        /// <summary>
        /// Token服务（新的统一Token服务）
        /// </summary>
        protected ITokenService TokenService { get; set; }

        /// <summary>
        /// Token管理器（简化版Token管理）
        /// </summary>
        protected TokenManager TokenManager { get; set; }

        /// <summary>
        /// 注册服务
        /// </summary>
        protected IRegistrationService RegistrationService { get; set; }

        /// <summary>
        /// Token服务配置选项
        /// </summary>
        protected TokenServiceOptions TokenOptions { get; set; }


        //public LoginCommandHandler(ILogger<LoginCommandHandler> _Logger) : base(_Logger)
        //{
        //    logger = _Logger;

        //    // 使用安全方法设置支持的命令
        //    SetSupportedCommands(
        //        AuthenticationCommands.Login,
        //        AuthenticationCommands.Logout,
        //        AuthenticationCommands.ValidateToken,
        //        AuthenticationCommands.DuplicateLogin
        //    );
        //}

        /// <summary>
        /// 完整构造函数，通过依赖注入获取服务
        /// </summary>
        public LoginCommandHandler(ILogger<LoginCommandHandler> _Logger, ISessionService sessionService,
                                  ITokenService tokenService, TokenManager tokenManager,
                                  ServerMessageService _MessageService,
                                  SystemManagementService _managementService,
                                  IRegistrationService _registrationService,
                                  TokenServiceOptions tokenOptions

            ) : base(_Logger)
        {
            logger = _Logger;
            SessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
            TokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            TokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
            MessageService = _MessageService;
            managementService = _managementService;
            RegistrationService = _registrationService ?? throw new ArgumentNullException(nameof(_registrationService));
            TokenOptions = tokenOptions ?? throw new ArgumentNullException(nameof(tokenOptions));
            // 使用安全方法设置支持的命令（单令牌机制，已移除RefreshToken）
            SetSupportedCommands(
                AuthenticationCommands.Login,
                AuthenticationCommands.Logout,
                AuthenticationCommands.DuplicateLogin
            );
        }



        /// <summary>
        /// 核心处理方法，根据命令类型分发到对应的处理函数（优化版：分层解析）
        /// 支持多种命令类型：LoginCommand、BaseCommand<LoginRequest, LoginResponse>、GenericCommand<LoginRequest>
        /// 采用延迟解析策略，具体业务数据解析在对应处理方法中完成
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令处理结果</returns>
        protected override async Task<IResponse> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                var commandId = cmd.Packet.CommandId;
                
                // 处理其他认证命令(使用LoginRequest)
                if (cmd.Packet.Request is LoginRequest loginRequest)
                {
                    if (commandId == AuthenticationCommands.Login)
                    {
                        //登录指令时上下文session是由服务器来指定的
                        if (loginRequest == null)
                        {
                            return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, "登录请求数据不能为空");
                        }
                        return await ProcessLoginAsync(loginRequest, cmd.Packet.ExecutionContext, cancellationToken);
                    }
                    else if (commandId == AuthenticationCommands.Logout)
                    {
                        return await ProcessLogoutAsync(loginRequest, cmd.Packet.ExecutionContext, cancellationToken);
                    }
                    else if (commandId == AuthenticationCommands.DuplicateLogin)
                    {
                        return await HandleDuplicateLoginAsync(loginRequest, cmd.Packet.ExecutionContext, cancellationToken);
                    }
                    else
                    {
                        // 确保所有代码路径都有返回值
                        return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, "不支持的认证命令");
                    }
                }
                else
                {
                    return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, $"不支持的命令类型: {commandId}");
                }
            }
            catch (Exception ex)
            {
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet.ExecutionContext, ex);
            }
        }






        /// <summary>
        /// 统一的登录业务逻辑处理方法
        /// 整合了所有登录相关的业务逻辑，被不同类型的登录命令处理器调用
        /// </summary>
        private async Task<IResponse> ProcessLoginAsync(
            LoginRequest loginRequest,
            CommandContext executionContext,
            CancellationToken cancellationToken)
        {
            // 【修复】在 try 块外声明 sessionInfo，确保 catch 块中可以访问
            SessionInfo sessionInfo = null;

            try
            {
                // 添加时间对比检测逻辑
                var serverTime = DateTime.Now;
                var clientTime = loginRequest.LoginTime;

                // 计算时间差（绝对值）
                var timeDifference = Math.Abs((serverTime - clientTime).TotalSeconds);

                // 如果时间差超过阈值（例如300秒=5分钟），则拒绝登录
                const double timeDifferenceThreshold = 300.0;
                if (timeDifference > timeDifferenceThreshold)
                {
                    // ✅ 详细记录时间不同步问题
                    logger?.LogWarning($"[登录被拒绝-时间不同步] ServerTime={serverTime:yyyy-MM-dd HH:mm:ss}, ClientTime={clientTime:yyyy-MM-dd HH:mm:ss}, Difference={timeDifference:F0}s, Threshold={timeDifferenceThreshold}s");
                    return ResponseFactory.CreateSpecificErrorResponse(executionContext, new Exception($"客户端时间与服务器时间差异过大 ({timeDifference:F0}秒)，请校准系统时间"));
                }

                // 验证请求参数
                if (!loginRequest.IsValid())
                {
                    return ResponseFactory.CreateSpecificErrorResponse(executionContext, new Exception("用户名和密码不能为空"));
                }

                // 获取或创建会话信息
                sessionInfo = SessionService.GetSession(executionContext.SessionId);
                
                // 如果从执行上下文获取失败，尝试从登录请求获取
                if (sessionInfo == null && loginRequest != null && !string.IsNullOrEmpty(loginRequest.SessionId))
                {
                    sessionInfo = SessionService.GetSession(loginRequest.SessionId);
                }
                
                // 如果仍然获取失败，创建新会话
                if (sessionInfo == null)
                {
                    sessionInfo = SessionService.CreateSession(executionContext.SessionId);
                    
                    if (sessionInfo == null)
                    {
                        return ResponseFactory.CreateSpecificErrorResponse(executionContext, "创建会话失败");
                    }
                }
                // ✅ 优化：统一使用服务器端从Socket获取的IP，防止客户端伪造
                string realIp = GetClientIp(sessionInfo);
                
                // 安全检查：确保能获取到真实IP
                if (string.IsNullOrEmpty(realIp) || realIp == "0.0.0.0")
                {
                    logger?.LogWarning($"[安全风险] 无法获取客户端真实IP, SessionID={sessionInfo.SessionID}, Username={loginRequest.Username}");
                    // 记录审计日志但不阻断登录（避免误杀正常用户）
                }
                
                sessionInfo.ClientIp = realIp;
                logger?.LogInformation($"[IP获取] SessionID={sessionInfo.SessionID}, ClientIp={sessionInfo.ClientIp}, Source=RemoteEndPoint");
                
                // ✅ 已移除IP黑名单检查 - 内网环境不需要IP限制，避免影响正常使用
                // if (IsUserBlacklisted(loginRequest.Username, sessionInfo.ClientIp))
                // {
                //     logger?.LogWarning($"[登录失败] 用户或IP在黑名单中: Username={loginRequest.Username}, ClientIp={sessionInfo.ClientIp}");
                //     return ResponseFactory.CreateSpecificErrorResponse(executionContext, "用户或IP在黑名单中");
                // }

                // 检查登录尝试次数
                int currentAttempts = GetLoginAttempts(loginRequest.Username);
                if (currentAttempts >= MaxLoginAttempts)
                {
                    // ✅ 详细记录登录限制原因
                    logger?.LogWarning($"[登录被拒绝-过度防护] Username={loginRequest.Username}, ClientIp={sessionInfo.ClientIp}, Attempts={currentAttempts}, MaxAttempts={MaxLoginAttempts}");
                    return ResponseFactory.CreateSpecificErrorResponse(executionContext, $"登录尝试次数过多({currentAttempts}/{MaxLoginAttempts})，请稍后再试");
                }

                // 验证用户凭据
                var userInfo = await ValidateUserCredentialsAsync(loginRequest, cancellationToken);
                if (userInfo == null)
                {
                    IncrementLoginAttempts(loginRequest.Username);
                    int newAttempts = GetLoginAttempts(loginRequest.Username);
                    
                    // 计算剩余尝试次数
                    int remainingAttempts = MaxLoginAttempts - newAttempts;
                    
                    // ✅ 优化：失败5次后不再封禁IP - 内网环境不需要IP封禁，避免影响正常使用
                    // if (newAttempts >= MaxLoginAttempts)
                    // {
                    //     BlacklistManager.BanIp(sessionInfo.ClientIp, TimeSpan.FromHours(1));
                    //     logger?.LogWarning($"[自动封禁] IP地址 {sessionInfo.ClientIp} 因登录失败次数过多被封禁1小时，Username={loginRequest.Username}, FailedAttempts={newAttempts}");
                    //     // 封禁后返回带封禁信息的错误（不暴露具体剩余次数以防暴力破解探测）
                    //     return ResponseFactory.CreateSpecificErrorResponse(executionContext, "登录尝试次数过多，账户已被临时锁定，请稍后再试或联系管理员");
                    // }
                    
                    // ✅ 优化：详细记录认证失败，并提供剩余尝试次数提示（帮助正常用户了解状态）
                    string errorMessage = remainingAttempts > 0 
                        ? $"用户名或密码错误，剩余尝试次数：{remainingAttempts}"
                        : "用户名或密码错误";
                    logger?.LogWarning($"[登录失败-认证错误] Username={loginRequest.Username}, ClientIp={sessionInfo.ClientIp}, Attempts={newAttempts}/{MaxLoginAttempts}, Remaining={remainingAttempts}, Reason=用户名或密码错误");
                    return ResponseFactory.CreateSpecificErrorResponse(executionContext, errorMessage);
                }

                // 重置登录尝试次数
                ResetLoginAttempts(loginRequest.Username);

                // 验证凭据成功后，再检查并发用户数限制（针对已认证用户）
                // 注意：这里检查的是已认证的唯一用户数，而不是总会话数
                var authenticatedUserCount = SessionService.GetAllUserSessions().Select(s => s.UserId).Distinct().Count();
                var maxUsers = await GetMaxConcurrentUsersAsync();
                if (authenticatedUserCount >= maxUsers)
                {
                    logger?.LogWarning($"[登录失败] 并发用户数已达上限: CurrentCount={authenticatedUserCount}, MaxCount={maxUsers}, Username={loginRequest.Username}");
                    return ResponseFactory.CreateSpecificErrorResponse(executionContext, $"当前系统用户数已达到上限({maxUsers}人)，请稍后再试");
                }
                else
                {
                    logger?.LogDebug($"[并发检查] 当前在线唯一用户数: {authenticatedUserCount}/{maxUsers}");
                }

                // 检查用户登录状态，分析重复登录情况
                var (hasExistingSessions, authorizedSessions, duplicateResult) = CheckUserLoginStatus(loginRequest.Username, executionContext.SessionId);

                // 处理重复登录情况
                if (duplicateResult.HasDuplicateLogin)
                {
                    // 根据重复登录类型记录日志
                    switch (duplicateResult.Type)
                    {
                        case DuplicateLoginType.LocalOnly:
                            // 本地重复登录 - 同一台机器
                            if (duplicateResult.AllowMultipleLocalSessions)
                            {
                                logger?.LogInformation($"[登录] 用户 {loginRequest.Username} 本地重复登录，允许多会话");
                            }
                            else
                            {
                                logger?.LogWarning($"[登录] 用户 {loginRequest.Username} 本地重复登录，但不允许多会话");
                            }
                            break;
                            
                        case DuplicateLoginType.RemoteOnly:
                        case DuplicateLoginType.Both:
                            // 远程重复登录 - 需要客户端决定是否强制下线
                            logger?.LogWarning($"[登录] 用户 {loginRequest.Username} 存在远程重复登录: 远程会话数={duplicateResult.ExistingSessions.Count(s => !s.IsLocal)}, 将会话信息返回给客户端处理");
                            break;
                            
                        default:
                            logger?.LogDebug($"[登录] 用户 {loginRequest.Username} 重复登录类型: {duplicateResult.Type}");
                            break;
                    }
                    
                    // 注意: 服务器不主动踢人，而是将重复登录信息返回给客户端
                    // 由客户端决定是否调用HandleDuplicateLoginAsync强制对方下线
                    // 这样设计可以避免服务器端误操作，给用户选择权
                }

                // 更新会话信息
                UpdateSessionInfo(sessionInfo, userInfo);

                // 设置会话授权状态
                sessionInfo.IsAuthenticated = true;

                SessionService.UpdateSession(sessionInfo);

                // 生成Token
                var tokenInfo = await GenerateTokenInfoAsync(userInfo, sessionInfo.SessionID, loginRequest.ClientIp);

                // 检查注册状态
                var registrationStatus = await RegistrationService.GetRegistrationStatusAsync();
                var expirationReminder = await RegistrationService.GetExpirationReminderInfoAsync();

                // 如果注册已过期，拒绝登录
                if (registrationStatus == RegistrationStatus.Expired)
                {
                    logger?.LogWarning($"[登录失败] 系统注册已过期: Username={loginRequest.Username}");
                    return ResponseFactory.CreateSpecificErrorResponse(executionContext, 
                        "系统注册已过期，请联系软件提供商续费。续费方式：请联系软件提供商");
                }

                // 返回成功结果 - 使用强类型 BaseCommand<LoginResponse>
                var loginResponse = new LoginResponse
                {
                    IsSuccess = true,
                    Message = "登录成功",
                    UserId = userInfo.User_ID,
                    Username = userInfo.UserName,
                    SessionId = sessionInfo.SessionID,
                    Token = tokenInfo,
                    // 新增：添加重复登录信息，让客户端决定后续处理
                    HasDuplicateLogin = duplicateResult.HasDuplicateLogin,
                    DuplicateLoginResult = duplicateResult,
                    // 新增：添加注册状态信息
                    RegistrationStatus = registrationStatus,
                    ExpirationReminder = expirationReminder
                };


                // 添加心跳间隔信息到元数据
                var maxUsersForMeta = await GetMaxConcurrentUsersAsync();
                loginResponse.WithMetadata("HeartbeatIntervalMs", "30000") // 默认30秒心跳间隔
                    .WithMetadata("ServerInfo", new Dictionary<string, object>
                    {
                        ["ServerTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        ["ServerVersion"] = "1.0.0",
                        ["MaxConcurrentUsers"] = maxUsersForMeta.ToString(),
                        ["CurrentActiveUsers"] = SessionService.ActiveSessionCount.ToString()
                    });

                // ✅ 详细记录登录成功信息
                logger?.LogInformation($"[登录成功] Username={loginRequest.Username}, UserId={userInfo.User_ID}, SessionId={sessionInfo.SessionID}, ClientIp={sessionInfo.ClientIp}, HasDuplicateLogin={duplicateResult.HasDuplicateLogin}, RegistrationStatus={registrationStatus}, ElapsedTime={(DateTime.Now - serverTime).TotalMilliseconds:F0}ms");

                return loginResponse;


            }
            catch (Exception ex)
            {
                // ✅ 详细记录登录异常，包含完整堆栈信息
                logger?.LogError(ex, $"[登录异常] Username={loginRequest.Username}, ClientIp={sessionInfo?.ClientIp}, ExceptionType={ex.GetType().Name}, Message={ex.Message}");
                return ResponseFactory.CreateSpecificErrorResponse(executionContext, ex, "处理登录异常");
            }
        }



 

        /// <summary>
        /// 处理重复用户下线命令
        /// 增强：添加会话有效性检查，避免对已断开的会话执行下线操作
        /// </summary>
        private async Task<IResponse> HandleDuplicateLoginAsync(LoginRequest request, CommandContext executionContext, CancellationToken cancellationToken)
        {
            try
            {
                string targetIdentifier = string.Empty;
                if (request.AdditionalData != null && request.AdditionalData.ContainsKey("TargetUserId"))
                {
                    targetIdentifier = request.AdditionalData["TargetUserId"].ToString();
                }
                        
                // 新增: 记录强制下线请求的详细信息
                logger?.LogInformation($"[强制下线请求] 来源SessionId={executionContext.SessionId}, 目标标识={targetIdentifier}");
                        
                // 查找目标用户会话 - 增强逻辑：支持 SessionId 或 UserId
                SessionInfo targetSession = null;
                
                // 1. 尝试直接作为 SessionId 查找
                targetSession = SessionService.GetSession(targetIdentifier);
                
                // 2. 如果没找到且看起来像数字 ID，尝试作为 UserId 查找
                if (targetSession == null && long.TryParse(targetIdentifier, out long userId))
                {
                    // 遍历所有已认证会话查找匹配的 UserId
                    var allSessions = SessionService.GetAllUserSessions();
                    targetSession = allSessions.FirstOrDefault(s => s.UserId.HasValue && s.UserId.Value == userId);
                }

                if (targetSession == null)
                {
                    // ✅ 修复：目标会话不存在可能意味着已经被断开，这实际上是成功的
                    logger?.LogInformation($"[强制下线] 目标会话不存在，可能已被断开: TargetIdentifier={targetIdentifier}，视为成功");
                    return ResponseFactory.CreateSpecificSuccessResponse(executionContext, "目标用户不在线或会话已失效");
                }
                        
                // 关键修复：检查目标会话是否仍然活跃连接
                if (!targetSession.IsConnected)
                {
                    logger?.LogInformation($"[强制下线] 目标会话已断开，无需执行下线操作: SessionId={targetSession.SessionID}, UserName={targetSession.UserName}, DisconnectTime={targetSession.DisconnectTime}，视为成功");
                    
                    // 会话已断开，清理残留的会话记录
                    await SessionService.RemoveSessionAsync(targetSession.SessionID);
                    
                    // ✅ 修复：返回成功而不是错误，因为目标已经下线
                    return ResponseFactory.CreateSpecificSuccessResponse(executionContext, "目标用户已断开连接");
                }
                        
                logger?.LogInformation($"[强制下线] 找到目标会话: SessionId={targetSession.SessionID}, UserName={targetSession.UserName}, ClientIp={targetSession.ClientIp}, IsConnected={targetSession.IsConnected}");
        
                // ✅ 修复：先发送提示消息
                try
                {
                    await MessageService.SendMessageToUserAsync(targetSession, 
                        message: "您的账号在另一地点登录,您已被强制下线。如非本人操作,请及时修改密码。", 
                        MessageType.System, 1500);
                    logger?.LogInformation($"[强制下线] 已向目标用户发送下线提示消息");
                }
                catch (Exception msgEx)
                {
                    // ✅ 修复：即使消息发送失败也继续执行断开连接，不视为致命错误
                    logger?.LogWarning(msgEx, $"[强制下线警告] 发送提示消息失败,但将继续执行断开连接: SessionId={targetSession.SessionID}");
                }
        
                // ✅ 修复：发送ForceLogout指令，等待A客户端响应并主动断开
                bool forceLogoutSuccess = false;
                string targetSessionId = targetSession.SessionID;
                
                try
                {
                    await managementService.ForceLogoutAsync(targetSession, 1000);
                    logger?.LogInformation($"[强制下线] 已发送ForceLogout指令给A客户端");
                    
                    // ✅ 关键修复：等待A客户端主动断开连接
                    // A客户端会：1.发送确认响应 2.主动断开连接 3.退出程序
                    // 我们等待最多3秒，直到检测到A的连接断开
                    int maxWaitMs = 3000;
                    int waitIntervalMs = 100;
                    int elapsedMs = 0;
                    
                    while (elapsedMs < maxWaitMs)
                    {
                        await Task.Delay(waitIntervalMs, cancellationToken);
                        elapsedMs += waitIntervalMs;
                        
                        // 检查A是否已断开
                        var checkSession = SessionService.GetSession(targetSessionId);
                        if (checkSession == null || !checkSession.IsConnected)
                        {
                            logger?.LogInformation($"[强制下线] 检测到A客户端已断开连接 (等待{elapsedMs}ms)");
                            forceLogoutSuccess = true;
                            break;
                        }
                        
                        if (elapsedMs % 1000 == 0)
                        {
                            logger?.LogDebug($"[强制下线] 等待A断开... ({elapsedMs}ms)");
                        }
                    }
                    
                    if (!forceLogoutSuccess)
                    {
                        logger?.LogWarning($"[强制下线] 等待超时({maxWaitMs}ms)，A仍未断开，将强制断开");
                    }
                }
                catch (Exception forceEx)
                {
                    // ✅ 修复：记录ForceLogout失败但不中断流程
                    logger?.LogWarning(forceEx, $"[强制下线警告] ForceLogoutAsync调用失败: SessionId={targetSession.SessionID}");
                }
                        
                // ✅ 修复：A已断开后，清理会话并返回成功给B
                try
                {
                    // 如果A还未完全断开，执行强制断开
                    var finalCheckSession = SessionService.GetSession(targetSessionId);
                    if (finalCheckSession != null && finalCheckSession.IsConnected)
                    {
                        logger?.LogWarning($"[强制下线] A仍未断开，执行强制断开: SessionId={targetSessionId}");
                        await SessionService.DisconnectSessionAsync(
                            targetSessionId, 
                            "您的账号在另一地点登录,您已被强制下线。");
                    }
                    else
                    {
                        logger?.LogInformation($"[强制下线] A已断开，清理会话记录: SessionId={targetSessionId}");
                    }
                    
                    // 清理会话记录
                    await SessionService.RemoveSessionAsync(targetSessionId);
                    logger?.LogInformation($"[强制下线] 已清理A的会话记录");
                }
                catch (Exception disconnectEx)
                {
                    // ✅ 修复：详细记录DisconnectSessionAsync的异常，但不一定返回失败
                    logger?.LogError(disconnectEx, $"[强制下线异常] 清理会话时发生异常: SessionId={targetSessionId}");
                    
                    // 再次检查会话状态，如果已断开则视为成功
                    var checkSession = SessionService.GetSession(targetSessionId);
                    if (checkSession == null || !checkSession.IsConnected)
                    {
                        logger?.LogInformation($"[强制下线成功] 尽管发生异常，但A的会话已断开");
                    }
                    else
                    {
                        // 如果仍然连接，才返回错误
                        return ResponseFactory.CreateSpecificErrorResponse(executionContext, $"强制下线异常: {disconnectEx.Message}");
                    }
                }
        
                // ✅ 修复：只要执行了断开操作，就返回成功
                return ResponseFactory.CreateSpecificSuccessResponse(executionContext, "用户已成功强制下线");
            }
            catch (Exception ex)
            {
                // 增强: 记录完整的异常堆栈和上下文
                logger?.LogError(ex, $"[强制下线严重错误] 处理强制用户下线命令时发生未预期异常: TargetUserId={request.AdditionalData?["TargetUserId"]}, SourceSessionId={executionContext.SessionId}, ExceptionType={ex.GetType().Name}, Message={ex.Message}, StackTrace={ex.StackTrace}");
                
                // ✅ 修复：即使发生异常，也尝试检查目标会话是否已断开
                try
                {
                    string targetId = request.AdditionalData?["TargetUserId"]?.ToString();
                    if (!string.IsNullOrEmpty(targetId))
                    {
                        var checkSession = SessionService.GetSession(targetId);
                        if (checkSession == null || !checkSession.IsConnected)
                        {
                            logger?.LogInformation($"[强制下线] 尽管发生异常，但目标会话已断开，视为成功: TargetId={targetId}");
                            return ResponseFactory.CreateSpecificSuccessResponse(executionContext, "用户已强制下线(异常恢复)");
                        }
                    }
                }
                catch { }
                
                return SystemCommandResponse.CreateForceLogoutFailure($"处理失败: {ex.Message}", "FORCE_LOGOUT_ERROR");
            }
        }


        /// <summary>
        /// 验证管理员权限
        /// </summary>
        private async Task<bool> ValidateAdminPermissionAsync(CommandContext context)
        {
            try
            {
                // 获取当前会话
                var session = SessionService.GetSession(context.SessionId);
                if (session == null)
                {
                    return false;
                }

                // 检查是否为管理员
                return session.IsAdmin;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "验证管理员权限时出错");
                return false;
            }
        }


        /// <summary>
        /// 处理登出操作
        /// </summary>
        private async Task<IResponse> ProcessLogoutAsync(
            LoginRequest loginRequest,
            CommandContext executionContext,
            CancellationToken cancellationToken)
        {
            try
            {
                // 从上下文中获取会话ID
                string sessionId = executionContext.SessionId;

                // 移除会话
                if (!string.IsNullOrEmpty(sessionId))
                {
                    await SessionService.RemoveSessionAsync(sessionId);
                }
                
                // 撤销 Token
                var token = executionContext.Token;
                if (token != null)
                {
                    if (!string.IsNullOrEmpty(token.AccessToken))
                    {
                        await TokenService.RevokeTokenAsync(token.AccessToken);
                    }
                }

                // 创建响应
                var response = new LoginResponse
                {
                    IsSuccess = true,
                    Message = "登出成功"
                };

                return response;
            }
            catch (Exception ex)
            {
                return ResponseFactory.CreateSpecificErrorResponse(executionContext, ex, "处理登出异常");
            }
        }
 

        #region 核心业务逻辑方法

        /// <summary>
        /// 验证用户凭据（不含防暴力延迟，仅执行核心验证）
        /// </summary>
        private async Task<tb_UserInfo> ValidateUserCredentialsCoreAsync(LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            try
            {
                logger?.LogDebug($"[凭据验证] 开始验证用户: Username={loginRequest.Username}, ClientIp={loginRequest.ClientIp}");
                
                string EnPassword = EncryptionHelper.AesEncryptByHashKey(loginRequest.Password, loginRequest.Username);

                var user = await Program.AppContextData.Db.CopyNew().Queryable<tb_UserInfo>()
                         .Where(u => u.UserName == loginRequest.Username && u.Password == EnPassword)
                   .Includes(x => x.tb_employee)
                         .Includes(x => x.tb_User_Roles)
                         .SingleAsync();
                
                if (user != null)
                {
                    logger?.LogInformation($"[凭据验证成功] UserId={user.User_ID}, UserName={user.UserName}, ClientIp={loginRequest.ClientIp}");
                }
                else
                {
                    logger?.LogWarning($"[凭据验证失败] 用户名或密码不匹配: Username={loginRequest.Username}, ClientIp={loginRequest.ClientIp}");
                }
                
                return user;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, $"[凭据验证异常] 验证用户凭据时发生异常: Username={loginRequest.Username}, ExceptionType={ex.GetType().Name}");
                throw;
            }
        }

        /// <summary>
        /// 验证用户凭据（完整版本，包含防暴力延迟）
        /// </summary>
        private async Task<tb_UserInfo> ValidateUserCredentialsAsync(LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            int currentAttempts = GetLoginAttempts(loginRequest.Username);
            
            var user = await ValidateUserCredentialsCoreAsync(loginRequest, cancellationToken);
            
            if (user == null)
            {
                int estimatedAttempts = currentAttempts + 1;
                if (estimatedAttempts >= 3)
                {
                    int delayMs = Math.Min(200 * (estimatedAttempts - 2), 2000);
                    logger?.LogWarning($"[防暴力破解] Username={loginRequest.Username}, CurrentAttempts={currentAttempts}, EstimatedAttempts={estimatedAttempts}, Delay={delayMs}ms");
                    await Task.Delay(delayMs, cancellationToken);
                }
            }
            
            return user;
        }

        /// <summary>
        /// 更新会话信息
        /// </summary>
        private void UpdateSessionInfo(SessionInfo sessionInfo, tb_UserInfo userInfo, bool IsAdmin = false)
        {
            // 使用SessionInfoAdapter来集中处理转换逻辑，保持代码一致性
            SessionInfoAdapter.UpdateSessionInfoFromUserEntity(sessionInfo, userInfo, IsAdmin);
        }

        /// <summary>
        /// 检查用户是否已登录（基于有效的Token和活跃连接）
        /// 修复：过滤掉已断开的会话，避免"幽灵会话"导致的误判
        /// </summary>
        private IEnumerable<SessionInfo> CheckExistingUserSessions(string username)
        {
            try
            {
                // 获取指定用户名的所有会话
                var allSessions = SessionService.GetUserSessions(username);

                // 过滤出有有效Token且连接活跃的会话
                var validSessions = new List<SessionInfo>();
                foreach (var session in allSessions)
                {
                    // 检查会话是否已认证且有有效的Token
                    if (session.IsAuthenticated && !string.IsNullOrEmpty(session.SessionID))
                    {
                        // 关键修复：检查会话是否仍在活跃连接状态
                        // IsConnected 表示物理连接是否仍然活跃
                        if (!session.IsConnected)
                        {
                            logger?.LogDebug($"[会话过滤] 跳过已断开的会话: SessionID={session.SessionID}, UserName={session.UserName}, DisconnectTime={session.DisconnectTime}");
                            continue;
                        }
                        
                        // 检查会话是否仍在SessionService中有效
                        if (SessionService.IsValidSession(session.SessionID))
                        {
                            validSessions.Add(session);
                        }
                        else
                        {
                            logger?.LogDebug($"[会话过滤] 跳过无效会话: SessionID={session.SessionID}, UserName={session.UserName}");
                        }
                    }
                }

                return validSessions;
            }
            catch (Exception ex)
            {
                LogError($"检查现有用户会话失败: {ex.Message}", ex);
                return Enumerable.Empty<SessionInfo>();
            }
        }



        /// <summary>
        /// 检查是否为本地登录（同一台机器）
        /// ✅ 修复：使用IP地址+计算机名组合判断，更准确识别同机登录
        /// </summary>
        /// <param name="currentSessionId">当前会话ID</param>
        /// <param name="existingSession">已存在的会话</param>
        /// <returns>是否为本地重复登录</returns>
        private bool IsLocalDuplicateLogin(string currentSessionId, SessionInfo existingSession)
        {
            try
            {
                // 获取当前会话的客户端信息
                var currentSession = SessionService.GetSession(currentSessionId);
                if (currentSession == null || existingSession == null)
                    return false;

                // ✅ 修复：使用IP地址 + 计算机名组合判断是否为同一台机器
                bool sameIp = string.Equals(currentSession.ClientIp, existingSession.ClientIp, StringComparison.OrdinalIgnoreCase);
                
                // 提取计算机名（从DeviceInfo中）
                string currentMachineName = ExtractMachineName(currentSession.DeviceInfo);
                string existingMachineName = ExtractMachineName(existingSession.DeviceInfo);
                bool sameMachine = !string.IsNullOrEmpty(currentMachineName) && 
                                   !string.IsNullOrEmpty(existingMachineName) &&
                                   string.Equals(currentMachineName, existingMachineName, StringComparison.OrdinalIgnoreCase);
                
                // IP相同且计算机名相同，才认为是同一台机器
                bool isSameMachine = sameIp && sameMachine;
                
                if (isSameMachine)
                {
                    logger?.LogDebug($"[同机判断] IP={currentSession.ClientIp}, MachineName={currentMachineName}，判定为同一台机器");
                }
                
                return isSameMachine;
            }
            catch (Exception ex)
            {
                LogError($"检查本地重复登录时发生异常: {ex.Message}", ex);
                return false;
            }
        }
        
        /// <summary>
        /// 从DeviceInfo中提取计算机名
        /// DeviceInfo格式可能为："DESKTOP-XXX | Windows 10" 或直接是计算机名
        /// </summary>
        private string ExtractMachineName(string deviceInfo)
        {
            if (string.IsNullOrEmpty(deviceInfo))
                return string.Empty;
            
            // 如果包含 "|" ，取第一部分作为计算机名
            if (deviceInfo.Contains("|"))
            {
                return deviceInfo.Split('|')[0].Trim();
            }
            
            // 否则直接返回
            return deviceInfo.Trim();
        }

        /// <summary>
        /// 检查会话是否已授权
        /// </summary>
        /// <param name="session">会话信息</param>
        /// <returns>是否已授权</returns>
        private bool IsSessionAuthorized(SessionInfo session)
        {
            return session != null && session.IsAuthenticated && session.UserId.HasValue;
        }



        /// <summary>
        /// 检查用户是否已登录，并处理重复登录情况
        /// 支持多种重复登录处理策略，提供详细的会话信息
        /// 修复：过滤掉已断开的会话，避免"幽灵会话"导致的误判
        /// </summary>
        private (bool hasExistingSessions, IEnumerable<SessionInfo> authorizedSessions, DuplicateLoginResult duplicateResult)
            CheckUserLoginStatus(string username, string currentSessionId)
        {
            try
            {
                // 获取指定用户名的所有已认证会话
                var userSessions = SessionService.GetUserSessions(username);

                // 关键修复：过滤掉已断开的会话，只保留活跃连接
                var activeSessions = userSessions.Where(s => s.IsConnected).ToList();
                
                if (activeSessions.Count != userSessions.Count())
                {
                    int filteredCount = userSessions.Count() - activeSessions.Count;
                    logger?.LogDebug($"[重复登录检测] 过滤了 {filteredCount} 个已断开的会话");
                }

                // 过滤出已授权的会话
                var authorizedSessions = activeSessions.Where(s => IsSessionAuthorized(s)).ToList();

                if (!authorizedSessions.Any())
                {
                    // 没有其他活跃会话，直接返回
                    return (false, authorizedSessions, new DuplicateLoginResult { HasDuplicateLogin = false });
                }

                // 获取当前会话的IP地址（用于判断是否为本地重复登录）
                var currentSession = SessionService.GetSession(currentSessionId);
                string currentClientIp = currentSession?.ClientIp ?? string.Empty;

                // 分类会话：本地会话和远程会话（一次性遍历完成）
                var localSessions = new List<SessionInfo>();
                var remoteSessions = new List<SessionInfo>();
                
                foreach (var session in authorizedSessions)
                {
                    // 如果IP地址相同，则认为是同一台机器的登录
                    bool isLocal = string.Equals(currentClientIp, session.ClientIp, StringComparison.OrdinalIgnoreCase);
                    
                    if (isLocal)
                    {
                        localSessions.Add(session);
                    }
                    else
                    {
                        remoteSessions.Add(session);
                    }
                }

                // 分析重复登录情况
                var duplicateResult = AnalyzeDuplicateLoginType(authorizedSessions, localSessions, remoteSessions, currentSessionId);

                // 记录诊断日志
                if (duplicateResult.HasDuplicateLogin)
                {
                    logger?.LogDebug($"[重复登录检测] Username={username}, LocalCount={localSessions.Count}, RemoteCount={remoteSessions.Count}, Type={duplicateResult.Type}");
                }

                // 返回分析结果
                return (remoteSessions.Any(), authorizedSessions, duplicateResult);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, $"[重复登录检测异常] 检查用户登录状态时发生异常: Username={username}");
                // 异常情况下返回无重复登录，避免阻断正常登录流程
                return (false, Enumerable.Empty<SessionInfo>(), new DuplicateLoginResult { HasDuplicateLogin = false });
            }
        }

        /// <summary>
        /// 分析重复登录类型和处理策略
        /// </summary>
        private DuplicateLoginResult AnalyzeDuplicateLoginType(
            List<SessionInfo> allSessions,
            List<SessionInfo> localSessions,
            List<SessionInfo> remoteSessions,
            string currentSessionId)
        {
            var result = new DuplicateLoginResult
            {
                HasDuplicateLogin = allSessions.Any(),
                ExistingSessions = allSessions.Select(s => new ExistingSessionInfo
                {
                    SessionId = s.SessionID,
                    LoginTime = s.LoginTime ?? DateTime.Now,
                    ClientIp = s.ClientIp ?? "未知",
                    DeviceInfo = s.DeviceInfo ?? "未知设备",
                    IsLocal = IsLocalDuplicateLogin(currentSessionId, s)
                }).ToList()
            };

            if (result.HasDuplicateLogin)
            {
                // 根据会话类型确定处理策略
                if (localSessions.Any() && !remoteSessions.Any())
                {
                    // 纯本地重复登录 - 同一台机器，直接允许登录，不需要用户确认
                    result.Message = "本机重复登录，允许直接登录";
                    result.RequireUserConfirmation = false; // 本地重复登录不需要用户确认
                    result.AllowMultipleLocalSessions = true;
                }
                else if (remoteSessions.Any())
                {
                    // 存在远程重复登录 - 需要用户确认
                    result.Message = $"您的账号已在其他地方登录（{remoteSessions.Count}个远程会话），是否强制对方下线，保持当前登录？";
                    result.RequireUserConfirmation = true;
                    result.AllowMultipleLocalSessions = false;
                }
            }

            return result;
        }


        /// <summary>
        /// 检查用户或IP是否在黑名单中
        /// ⚠️ 已禁用 - 内网环境不需要IP限制
        /// </summary>
        private bool IsUserBlacklisted(string username, string ipAddress)
        {
            // 内网业务系统：已移除IP封禁功能，避免影响正常用户使用
            // 仅保留登录次数限制作为基础防护
            return false;
        }

        /// <summary>
        /// 获取登录尝试次数
        /// </summary>
        private int GetLoginAttempts(string username)
        {
            return _loginAttempts.TryGetValue(username, out var attempts) ? attempts : 0;
        }

        /// <summary>
        /// 增加登录尝试次数
        /// </summary>
        private void IncrementLoginAttempts(string username)
        {
            _loginAttempts.AddOrUpdate(username, 1, (key, oldValue) => oldValue + 1);
            // 记录首次登录尝试时间
            _loginAttemptTimes.TryAdd(username, DateTime.Now);
            EnsureCleanupTimerStarted();
        }

        /// <summary>
        /// 重置登录尝试次数
        /// </summary>
        private void ResetLoginAttempts(string username)
        {
            _loginAttempts.TryRemove(username, out _);
            _loginAttemptTimes.TryRemove(username, out _);
        }

        /// <summary>
        /// 确保清理定时器已启动（延迟初始化，避免多线程竞争）
        /// </summary>
        private static void EnsureCleanupTimerStarted()
        {
            if (_cleanupTimerStarted) return;

            lock (_lock)
            {
                if (_cleanupTimerStarted) return;

                // 启动定时清理任务，每5分钟清理一次
                var timer = new System.Threading.Timer(
                    _ => CleanupExpiredLoginAttempts(),
                    null,
                    TimeSpan.FromMinutes(5),  // 首次延迟5分钟
                    TimeSpan.FromMinutes(5)   // 之后每5分钟执行一次
                );

                _cleanupTimerStarted = true;
            }
        }

        /// <summary>
        /// 保守式清理过期的登录尝试记录
        /// 注意：不能清理真正在线的用户，只能清理长时间未成功的登录尝试记录
        /// 保守策略：只清理超过过期时间且失败次数较少的记录
        /// </summary>
        private static void CleanupExpiredLoginAttempts()
        {
            try
            {
                var now = DateTime.Now;
                var keysToRemove = new List<string>();

                foreach (var kvp in _loginAttemptTimes)
                {
                    var username = kvp.Key;
                    var firstAttemptTime = kvp.Value;

                    // 检查是否超过过期时间
                    if (now - firstAttemptTime > LoginAttemptExpiryDuration)
                    {
                        // 保守策略：只有失败次数较少时才清理
                        // 如果失败次数很多（>=阈值），可能是暴力破解攻击，保留记录以便封锁
                        if (_loginAttempts.TryGetValue(username, out var attempts) && attempts < ConservativeCleanupThreshold)
                        {
                            keysToRemove.Add(username);
                        }
                    }
                }

                // 执行清理
                foreach (var key in keysToRemove)
                {
                    _loginAttempts.TryRemove(key, out _);
                    _loginAttemptTimes.TryRemove(key, out _);
                }

                // 注意：静态方法中无法访问实例Logger，如需日志请改为实例方法
                // if (keysToRemove.Any())
                // {
                //     Logger?.LogDebug("保守清理了 {Count} 个过期的登录尝试记录", keysToRemove.Count);
                // }
            }
            catch (Exception ex)
            {
                // 注意：静态方法中无法访问实例Logger
                // Logger?.LogError(ex, "清理登录尝试记录时发生错误");
            }
        }

        /// <summary>
        /// 生成Token信息 - 使用简化版TokenManager生成Token
        /// 生成Token：当用户登录时，服务器验证用户凭证（如用户名和密码）后，生成一个Token（通常是JWT，即JSON Web Token）并返回给客户端
        /// </summary>
        private async Task<TokenInfo> GenerateTokenInfoAsync(tb_UserInfo userInfo, string sessionId, string ClientIp)
        {
            var tokenManager = Program.ServiceProvider.GetRequiredService<TokenManager>();

            var additionalClaims = new Dictionary<string, object>
            {
                { "sessionId", sessionId },
                { "clientIp", ClientIp },
                { "userId", userInfo.User_ID }
            };

            var tokenInfo = await tokenManager.GenerateTokenAsync(
                userInfo.User_ID.ToString(),
                userInfo.UserName,
                additionalClaims
            );

            return tokenInfo;
        }


        /// <summary>
        /// 验证Token：客户端在后续的请求中携带此Token，服务器验证Token的合法性（如签名、有效期等）以确定用户身份
        /// </summary>
        private TokenValidationResult ValidateToken(string token, string sessionId)
        {
            // 使用新的ITokenService验证Token（TokenValidationService已合并到JwtTokenService）
            var validationResult = TokenService.ValidateToken(token);

            // 检查Token对应的会话是否仍然有效
            if (validationResult.IsValid && !string.IsNullOrEmpty(sessionId))
            {
                if (!SessionService.IsValidSession(sessionId))
                {
                    return new TokenValidationResult
                    {
                        IsValid = false,
                        ErrorMessage = "会话已过期"
                    };
                }
            }

            return validationResult;
        }

      
        /// <summary>
        /// 执行注销
        /// </summary>
        private async Task<bool> LogoutAsync(string sessionId, string userId)
        {
            await Task.Delay(10);

            ResetLoginAttempts(userId);

            // 通过SessionService移除会话
            return SessionService.RemoveSession(sessionId);
        }



        /// <summary>
        /// 获取客户端真实IP地址(从服务器端Socket获取，防止伪造)
        /// 增强版：添加详细日志和异常处理
        /// </summary>
        /// <param name="appSession">会话对象</param>
        /// <returns>客户端IP地址</returns>
        private string GetClientIp(IAppSession appSession)
        {
            try
            {
                if (appSession == null)
                {
                    logger?.LogWarning($"[IP获取] appSession为null");
                    return "0.0.0.0";
                }
                
                // 从RemoteEndPoint获取真实IP（服务器端视角）
                var remoteEndPoint = appSession.RemoteEndPoint;
                if (remoteEndPoint == null)
                {
                    logger?.LogWarning($"[IP获取] RemoteEndPoint为null, SessionID={appSession.SessionID}");
                    return "0.0.0.0";
                }
                
                var ipEndpoint = remoteEndPoint as System.Net.IPEndPoint;
                if (ipEndpoint == null)
                {
                    logger?.LogWarning($"[IP获取] RemoteEndPoint不是IPEndPoint类型, 实际类型={remoteEndPoint.GetType().Name}, SessionID={appSession.SessionID}");
                    return "0.0.0.0";
                }
                
                string ip = ipEndpoint.Address.ToString();
                
                // 记录IP类型用于调试
                if (ipEndpoint.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    logger?.LogDebug($"[IP获取] 检测到IPv6客户端: {ip}, SessionID={appSession.SessionID}");
                }
                else
                {
                    logger?.LogDebug($"[IP获取] IPv4客户端: {ip}, SessionID={appSession.SessionID}, Port={ipEndpoint.Port}");
                }
                
                return ip;
            }
            catch (Exception ex)
            {
                logger?.LogWarning(ex, $"[IP获取异常] 获取客户端IP地址失败, SessionID={appSession?.SessionID}");
                return "0.0.0.0";
            }
        }




        #endregion




    }


}

