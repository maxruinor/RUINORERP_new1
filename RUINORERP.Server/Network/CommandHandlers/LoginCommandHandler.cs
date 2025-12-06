using Azure.Core;
using HLH.Lib.Security;
using log4net.Core;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualBasic.ApplicationServices;
using RUINORERP.Common.Extensions;
using RUINORERP.Model;
using RUINORERP.Model.TransModel;
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
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.Server;
using RUINORERP.Server.Adapters;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.Network.Models;
using RUINORERP.Server.Network.Services;
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
    /// 包含重复登录检查、人数限制、黑名单验证、Token验证和刷新机制
    /// </summary>
    [CommandHandler("LoginCommandHandler", priority: 100)]
    public class LoginCommandHandler : BaseCommandHandler
    {
        private const int MaxLoginAttempts = 5;
        // 最大并发用户数现在从注册信息中获取
        private int MaxConcurrentUsers => frmMainNew.Instance?.registrationInfo?.ConcurrentUsers ?? 1000;
        private static readonly ConcurrentDictionary<string, int> _loginAttempts = new ConcurrentDictionary<string, int>();
        private static readonly object _lock = new object();
        /// <summary>
        /// 日志记录器
        /// </summary>
        protected ILogger<LoginCommandHandler> logger { get; set; }
        protected ServerMessageService MessageService { get; set; }
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



        public LoginCommandHandler(ILogger<LoginCommandHandler> _Logger) : base(_Logger)
        {
            logger = _Logger;

            // 使用安全方法设置支持的命令
            SetSupportedCommands(
                AuthenticationCommands.Login,
                AuthenticationCommands.Logout,
                AuthenticationCommands.ValidateToken,
                AuthenticationCommands.RefreshToken,
                AuthenticationCommands.DuplicateLogin,
                AuthenticationCommands.ForceLogout
            );
        }

        /// <summary>
        /// 完整构造函数，通过依赖注入获取服务
        /// </summary>
        public LoginCommandHandler(ILogger<LoginCommandHandler> _Logger, ISessionService sessionService,
                                  ITokenService tokenService, TokenManager tokenManager, ServerMessageService _MessageService) : base(_Logger)
        {
            logger = _Logger;
            SessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
            TokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            TokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
            MessageService = _MessageService;
            // 使用安全方法设置支持的命令
            SetSupportedCommands(
                AuthenticationCommands.Login,
                AuthenticationCommands.Logout,
                AuthenticationCommands.ValidateToken,
                AuthenticationCommands.RefreshToken,
                AuthenticationCommands.DuplicateLogin,
                AuthenticationCommands.ForceLogout
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
                if (cmd.Packet.Request is LoginRequest loginRequest)
                {
                    if (commandId == AuthenticationCommands.Login)
                    {
                        //登陆指令时上下文session是由服务器来指定的
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
                    else if (commandId == AuthenticationCommands.ValidateToken)
                    {
                        return await ProcessTokenValidationAsync(cmd.Packet.ExecutionContext, cancellationToken);
                    }
                    else if (commandId == AuthenticationCommands.RefreshToken)
                    {
                        return await ProcessTokenRefreshAsync(loginRequest, cmd.Packet.ExecutionContext, cancellationToken);
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
                    return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, "不支持的命令类型");
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
                    return ResponseFactory.CreateSpecificErrorResponse(executionContext, new Exception($"客户端时间与服务器时间差异过大 ({timeDifference:F0}秒)，请校准系统时间"));
                }

                // 验证请求参数
                if (!loginRequest.IsValid())
                {
                    return ResponseFactory.CreateSpecificErrorResponse(executionContext, new Exception("用户名和密码不能为空"));
                }

                // 检查并发用户数限制
                if (SessionService.ActiveSessionCount >= MaxConcurrentUsers)
                {
                    return ResponseFactory.CreateSpecificErrorResponse(executionContext, "当前系统用户数已达到上限，请稍后再试");
                }

                // 获取或创建会话信息
                var sessionInfo = SessionService.GetSession(executionContext.SessionId);
                if (sessionInfo == null)
                {
                    sessionInfo = SessionService.CreateSession(executionContext.SessionId);

                    if (sessionInfo == null)
                    {
                        return ResponseFactory.CreateSpecificErrorResponse(executionContext, "创建会话失败");
                    }
                }
                sessionInfo.ClientIp = loginRequest.ClientIp;
                if (string.IsNullOrEmpty(sessionInfo.ClientIp))
                {
                    sessionInfo.ClientIp = GetClientIp(sessionInfo);
                }
                // 检查黑名单
                if (IsUserBlacklisted(loginRequest.Username, loginRequest.ClientIp))
                {
                    return ResponseFactory.CreateSpecificErrorResponse(executionContext, "用户或IP在黑名单中");
                }

                // 检查登录尝试次数
                if (GetLoginAttempts(loginRequest.Username) >= MaxLoginAttempts)
                {
                    return ResponseFactory.CreateSpecificErrorResponse(executionContext, "登录尝试次数过多，请稍后再试");
                }

                // 验证用户凭据
                var userInfo = await ValidateUserCredentialsAsync(loginRequest, cancellationToken);
                if (userInfo == null)
                {
                    IncrementLoginAttempts(loginRequest.Username);
                    return ResponseFactory.CreateSpecificErrorResponse(executionContext, "用户名或密码错误");
                }

                // 重置登录尝试次数
                ResetLoginAttempts(loginRequest.Username);

                // 检查用户登录状态，分析重复登录情况
                var (hasExistingSessions, authorizedSessions, duplicateResult) = CheckUserLoginStatus(loginRequest.Username, executionContext.SessionId);

                // 处理重复登录情况
                if (duplicateResult.HasDuplicateLogin)
                {
                    // 优化：无论是否有重复登录，都继续登录流程
                    // 服务器直接返回登录成功，重复登录信息由客户端后续处理
                    // 对于本地重复登录且允许多会话的情况，自动处理
                    if (duplicateResult.Type == DuplicateLoginType.LocalOnly && duplicateResult.AllowMultipleLocalSessions)
                    {
                        logger?.LogDebug($"用户 {loginRequest.Username} 本地重复登录，允许多会话，继续登录流程");
                    }


                }

                // 更新会话信息
                UpdateSessionInfo(sessionInfo, userInfo);
                SessionService.UpdateSession(sessionInfo);

                // 生成Token
                var tokenInfo = await GenerateTokenInfoAsync(userInfo, sessionInfo.SessionID, loginRequest.ClientIp);

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
                    DuplicateLoginResult = duplicateResult
                };


                // 添加心跳间隔信息到元数据
                loginResponse.WithMetadata("HeartbeatIntervalMs", "30000") // 默认30秒心跳间隔
                    .WithMetadata("ServerInfo", new Dictionary<string, object>
                    {
                        ["ServerTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        ["ServerVersion"] = "1.0.0",
                        ["MaxConcurrentUsers"] = MaxConcurrentUsers.ToString(),
                        ["CurrentActiveUsers"] = SessionService.ActiveSessionCount.ToString()
                    });

                return loginResponse;


            }
            catch (Exception ex)
            {
                return ResponseFactory.CreateSpecificErrorResponse(executionContext, ex, "处理登录异常");
            }
        }




        /// <summary>
        /// 处理Token刷新
        /// </summary>
        private async Task<IResponse> ProcessTokenRefreshAsync(
            LoginRequest loginRequest,
            CommandContext executionContext,
            CancellationToken cancellationToken)
        {
            try
            {
                // 从请求中提取刷新Token和当前Token
                string refreshToken = loginRequest.Token?.RefreshToken;
                string currentToken = loginRequest.Token?.AccessToken;

                // 验证Token
                if (string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(currentToken))
                {
                    return ResponseFactory.CreateSpecificErrorResponse(executionContext, "刷新Token和当前Token不能为空");
                }

                // 使用TokenService刷新Token
                string newAccessToken;
                string newRefreshToken;
                try
                {
                    // 使用新的RefreshTokens方法获取令牌对
                    if (TokenService is JwtTokenService jwtTokenService)
                    {
                        var tokens = jwtTokenService.RefreshTokens(refreshToken);
                        newAccessToken = tokens.AccessToken;
                        newRefreshToken = tokens.RefreshToken;
                    }
                    else
                    {
                        // 兼容旧版本，使用RefreshToken方法
                        newAccessToken = TokenService.RefreshToken(refreshToken);
                        newRefreshToken = newAccessToken; // 旧版本下刷新令牌与访问令牌相同
                    }
                }
                catch (Exception ex)
                {
                    return ResponseFactory.CreateSpecificErrorResponse(executionContext, ex);
                }

                // 创建响应
                var response = new TokenRefreshResponse
                {
                    IsSuccess = true,
                    Message = "Token刷新成功",
                    NewAccessToken = newAccessToken,
                    NewRefreshToken = newRefreshToken, // 现在使用独立的刷新令牌
                    ExpireTime = DateTime.Now.AddHours(8)
                };

                return response;
            }
            catch (Exception ex)
            {
                return ResponseFactory.CreateSpecificErrorResponse(executionContext, ex);
            }
        }



        /// <summary>
        /// 处理重复用户下线命令
        /// </summary>
        private async Task<IResponse> HandleDuplicateLoginAsync(LoginRequest request, CommandContext executionContext, CancellationToken cancellationToken)
        {
            try
            {
                string TargetUserId = string.Empty;
                if (request.AdditionalData != null && request.AdditionalData.ContainsKey("TargetUserId"))
                {
                    TargetUserId = request.AdditionalData["TargetUserId"].ToString();
                }
                // 查找目标用户会话
                var targetSession = SessionService.GetSession(TargetUserId);
                if (targetSession == null)
                {
                    return ResponseFactory.CreateSpecificErrorResponse(executionContext, "目标用户不在线");
                }

                await MessageService.SendMessageToUserAsync(targetSession, message: "您的账号在另一地点登录，您已被强制下线。如非本人操作，请及时修改密码。", MessageType.Notice, 1500);

                // 通知客户端强制下线
                await SessionService.DisconnectSessionAsync(targetSession.SessionID, $"您的账号在另一地点登录，您已被强制下线。如非本人操作，请及时修改密码。");

                return ResponseFactory.CreateSpecificSuccessResponse(executionContext, "用户已成功强制下线");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "处理强制用户下线命令时出错");
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
                // 撤销Token
                if (executionContext.Token?.AccessToken != null && !string.IsNullOrEmpty(executionContext.Token?.AccessToken))
                {
                    TokenService.RevokeToken(executionContext.Token?.AccessToken);
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

        /// <summary>
        /// 处理Token验证
        /// </summary>
        private async Task<IResponse> ProcessTokenValidationAsync(
            CommandContext executionContext,
            CancellationToken cancellationToken)
        {
            try
            {
                // 从上下文中获取Token
                string token = executionContext.Token?.AccessToken;
                // 验证Token
                if (string.IsNullOrEmpty(token))
                {
                    return ResponseFactory.CreateSpecificErrorResponse(executionContext, "Token不能为空");
                }

                // 使用TokenService验证Token
                var validationResult = TokenService.ValidateToken(token);

                // 创建响应
                var response = new LoginResponse
                {
                    IsSuccess = validationResult.IsValid,
                    Message = validationResult.IsValid ? "Token验证成功" : validationResult.ErrorMessage,
                    UserId = validationResult.UserId != null ? Convert.ToInt64(validationResult.UserId) : 0,
                    Username = validationResult.UserName
                };

                // 如果验证成功，设置Token信息
                if (validationResult.IsValid)
                {
                    response.Token = new TokenInfo { AccessToken = token };
                }

                return response;
            }
            catch (Exception ex)
            {
                LogError($"处理Token验证异常: {ex.Message}", ex);
                return ResponseFactory.CreateSpecificErrorResponse(executionContext, ex, "处理Token验证异常  ");
            }
        }

        #region 核心业务逻辑方法

        /// <summary>
        /// 验证用户凭据
        /// </summary>
        private async Task<tb_UserInfo> ValidateUserCredentialsAsync(LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            //防止暴力破解攻击,时间侧信道攻击防护,降低服务器负载 这是一个 安全最佳实践 ，在身份验证系统中很常见，目的是提高系统的安全性而不是处理超时情况。
            await Task.Delay(500, cancellationToken);
            //password = EncryptionHelper.AesDecryptByHashKey(enPwd, username);
            string EnPassword = EncryptionHelper.AesEncryptByHashKey(loginRequest.Password, loginRequest.Username);

            var user = await Program.AppContextData.Db.CopyNew().Queryable<tb_UserInfo>()
                     .Where(u => u.UserName == loginRequest.Username && u.Password == EnPassword)
               .Includes(x => x.tb_employee)
                     .Includes(x => x.tb_User_Roles)
                     .SingleAsync();
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
        /// 检查用户是否已登录（基于有效的Token）
        /// </summary>
        private IEnumerable<SessionInfo> CheckExistingUserSessions(string username)
        {
            try
            {
                // 获取指定用户名的所有会话
                var allSessions = SessionService.GetUserSessions(username);

                // 过滤出有有效Token的会话
                var validSessions = new List<SessionInfo>();
                foreach (var session in allSessions)
                {
                    // 检查会话是否已认证且有有效的Token
                    if (session.IsAuthenticated && !string.IsNullOrEmpty(session.SessionID))
                    {
                        // 检查会话是否仍在活跃状态
                        if (SessionService.IsValidSession(session.SessionID))
                        {
                            validSessions.Add(session);
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
        /// </summary>
        /// <param name="currentSessionId">当前会话ID</param>
        /// <param name="existingSession">已存在的会话</param>
        /// <returns>是否为本地重复登录</returns>
        private bool IsLocalDuplicateLogin(string currentSessionId, SessionInfo existingSession)
        {
            try
            {
                // 获取当前会话的客户端IP
                var currentSession = SessionService.GetSession(currentSessionId);
                if (currentSession == null || existingSession == null)
                    return false;

                // 如果IP地址相同，则认为是同一台机器的登录
                // 同一台机器允许多个会话同时存在
                return string.Equals(currentSession.ClientIp, existingSession.ClientIp, StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                LogError($"检查本地重复登录时发生异常: {ex.Message}", ex);
                return false;
            }
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
        /// </summary>
        private (bool hasExistingSessions, IEnumerable<SessionInfo> authorizedSessions, DuplicateLoginResult duplicateResult)
            CheckUserLoginStatus(string username, string currentSessionId)
        {
            // 获取指定用户名的所有已认证会话
            var userSessions = SessionService.GetUserSessions(username);

            // 过滤出已授权的会话
            var authorizedSessions = userSessions.Where(s => IsSessionAuthorized(s)).ToList();

            // 分类会话：本地会话和远程会话
            var localSessions = authorizedSessions.Where(s => IsLocalDuplicateLogin(currentSessionId, s)).ToList();
            var remoteSessions = authorizedSessions.Where(s => !IsLocalDuplicateLogin(currentSessionId, s)).ToList();

            // 分析重复登录情况
            var duplicateResult = AnalyzeDuplicateLoginType(authorizedSessions, localSessions, remoteSessions, currentSessionId);

            // 返回分析结果
            return (remoteSessions.Any(), authorizedSessions, duplicateResult);
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
        /// 发送重复登录通知
        /// </summary>
        [Obsolete("后面要优化通过消息模块发送")]
        private void SendDuplicateLoginNotification(string sessionId, string username)
        {
            try
            {
                if (!string.IsNullOrEmpty(sessionId))
                {
                    var appSession = SessionService.GetAppSession(sessionId);
                    if (appSession != null)
                    {
                        // 创建重复登录通知消息
                        //var notificationMessage = new OriginalData(
                        //    (byte)CommandCategory.Authentication,
                        //    new byte[] { AuthenticationCommands.DuplicateLoginNotification.OperationCode },
                        //    System.Text.Encoding.UTF8.GetBytes($"您的账号【{username}】在其他地方登录，您已被强制下线。")
                        //);

                        //  var encryptedData = PacketSpec.Security.EncryptedProtocol.EncryptionServerPackToClient(notificationMessage);

                        // 发送通知消息
                        //  appSession.SendAsync(encryptedData.ToByteArray());
                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"发送重复登录通知失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 检查用户或IP是否在黑名单中
        /// </summary>
        private bool IsUserBlacklisted(string username, string ipAddress)
        {
            // 检查IP是否被封禁
            return BlacklistManager.IsIpBanned(username) ||
                   !string.IsNullOrEmpty(ipAddress) && BlacklistManager.IsIpBanned(ipAddress);
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
        }

        /// <summary>
        /// 重置登录尝试次数
        /// </summary>
        private void ResetLoginAttempts(string username)
        {
            _loginAttempts.TryRemove(username, out _);
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
        /// 刷新Token - 使用TokenManager提供Token刷新机制
        /// 刷新Token：由于安全考虑，Token通常有有效期。当Token即将过期时，客户端可以使用刷新Token来获取新的访问Token，而无需用户重新登录
        /// </summary>
        private async Task<(bool Success, string AccessToken, string ErrorMessage)> RefreshTokenAsync(string refreshToken, string currentToken)
        {
            await Task.Delay(30);

            try
            {
                // 使用TokenService进行Token刷新
                if (TokenService is JwtTokenService jwtTokenService)
                {
                    // 使用新的RefreshTokens方法获取令牌对
                    var tokens = jwtTokenService.RefreshTokens(refreshToken);

                    if (!string.IsNullOrEmpty(tokens.AccessToken))
                    {
                        return (true, tokens.AccessToken, null);
                    }
                }
                else
                {
                    // 兼容旧版本，使用RefreshToken方法
                    var newToken = TokenService.RefreshToken(refreshToken);

                    if (!string.IsNullOrEmpty(newToken))
                    {
                        return (true, newToken, null);
                    }
                }

                return (false, null, "无法生成有效的刷新令牌");
            }
            catch (Microsoft.IdentityModel.Tokens.SecurityTokenException ex)
            {
                return (false, null, ex.Message);
            }
            catch (Exception ex)
            {
                return (false, null, $"Token刷新失败: {ex.Message}");
            }
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
        /// 统一的客户端IP获取方法，优先从请求数据中获取，其次从会话中获取
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="loginRequest">登录请求对象</param>
        /// <returns>客户端IP地址</returns>
        private string GetClientIp(IAppSession appSession)
        {
            // 4. 如果SessionInfo中没有IP，尝试从RemoteEndPoint获取
            if (appSession != null && appSession.RemoteEndPoint != null)
            {
                var ipEndpoint = appSession.RemoteEndPoint as System.Net.IPEndPoint;
                if (ipEndpoint != null)
                {
                    return ipEndpoint.Address.ToString();
                }
            }

            // 如果无法获取IP，则返回默认值
            return "0.0.0.0"; // 使用0.0.0.0表示未知IP
        }




        #endregion




    }


}

