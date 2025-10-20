using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec;
using RUINORERP.PacketSpec.Errors;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.Server.Network.Models;
using RUINORERP.PacketSpec.Handlers;
using RUINORERP.PacketSpec.Models;
using HLH.Lib.Security;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Microsoft.VisualBasic.ApplicationServices;
using RUINORERP.Model;
using RUINORERP.Server.Network.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Sockets;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Models.Requests;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands.Cache;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ZXing.Common.ReedSolomon;
using SuperSocket.Channel;
using SuperSocket.Connection;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Concurrent;
using RUINORERP.PacketSpec.Models.Core;
using System.IdentityModel.Tokens.Jwt;
using System.IO.Packaging;
using Azure.Core;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace RUINORERP.Server.Network.Commands
{
    /// <summary>
    /// 统一登录命令处理器 - 整合了命令模式和处理器模式的登录处理
    /// 包含重复登录检查、人数限制、黑名单验证、Token验证和刷新机制
    /// </summary>
    [CommandHandler("LoginCommandHandler", priority: 100)]
    public class LoginCommandHandler : BaseCommandHandler
    {
        private const int MaxLoginAttempts = 5;
        private const int MaxConcurrentUsers = 1000;
        private static readonly ConcurrentDictionary<string, int> _loginAttempts = new ConcurrentDictionary<string, int>();
        private static readonly object _lock = new object();
        /// <summary>
        /// 日志记录器
        /// </summary>
        protected ILogger<LoginCommandHandler> logger { get; set; }
        /// <summary>
        /// 无参构造函数，用于动态创建实例
        /// </summary>
        public LoginCommandHandler() : base()
        {
        }

        public LoginCommandHandler(ILogger<LoginCommandHandler> _Logger) : base(_Logger)
        {
            logger = _Logger;

            // 使用安全方法设置支持的命令
            SetSupportedCommands(
                AuthenticationCommands.Login,
                AuthenticationCommands.Logout,
                AuthenticationCommands.ValidateToken,
                AuthenticationCommands.RefreshToken
            );
        }


        /// <summary>
        /// 会话管理服务
        /// </summary>
        private ISessionService SessionService => Program.ServiceProvider.GetRequiredService<ISessionService>();

        /// <summary>
        /// Token服务（新的统一Token服务）
        /// </summary>
        private ITokenService TokenService => Program.ServiceProvider.GetRequiredService<ITokenService>();

        /// <summary>
        /// Token服务实例（用于方法内部）
        /// </summary>
        private ITokenService _tokenService => TokenService;

        /// <summary>
        /// Token管理器（简化版Token管理）
        /// </summary>
        private TokenManager TokenManager => Program.ServiceProvider.GetRequiredService<TokenManager>();



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
                            return CreateErrorResponse("登录请求数据不能为空", UnifiedErrorCodes.Command_ValidationFailed, "EMPTY_LOGIN_REQUEST");
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
                    else
                    {
                        // 确保所有代码路径都有返回值
                        var errorResponse = ResponseBase.CreateError($"不支持的认证命令ID: {commandId.ToString()}")
                            .WithMetadata("ErrorCode", "UNSUPPORTED_AUTH_COMMAND");
                        return errorResponse;
                    }
                }
                else
                {
                    var errorResponse = ResponseBase.CreateError($"不支持的命令类型: {cmd.Packet.CommandId.ToString()}")
                        .WithMetadata("ErrorCode", "UNSUPPORTED_COMMAND");
                    return errorResponse;
                }
            }
            catch (Exception ex)
            {
                LogError($"处理命令 {cmd.Packet.CommandId.ToString()} 异常: {ex.Message}", ex);
                return CreateExceptionResponse(ex, "HANDLER_ERROR");
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
            LogInfo($"处理登录请求: {loginRequest.Username}");

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
                    return CreateErrorResponse($"客户端时间与服务器时间差异过大 ({timeDifference:F0}秒)，请校准系统时间",
                        UnifiedErrorCodes.Command_ValidationFailed, "TIME_MISMATCH");
                }

                // 验证请求参数
                if (!loginRequest.IsValid())
                {
                    return CreateErrorResponse("用户名和密码不能为空", UnifiedErrorCodes.Auth_PasswordError, "VALIDATION_FAILED");
                }

                // 检查并发用户数限制
                if (SessionService.ActiveSessionCount >= MaxConcurrentUsers)
                {
                    return CreateErrorResponse("当前系统用户数已达到上限，请稍后再试", UnifiedErrorCodes.Biz_LimitExceeded, "MAX_USERS_EXCEEDED");
                }

                // 获取或创建会话信息
                var sessionInfo = SessionService.GetSession(executionContext.SessionId);
                if (sessionInfo == null)
                {
                    sessionInfo = SessionService.CreateSession(executionContext.SessionId);

                    if (sessionInfo == null)
                    {
                        return CreateErrorResponse("创建会话失败", UnifiedErrorCodes.System_InternalError, "SESSION_CREATION_FAILED");
                    }
                }

                // 检查黑名单
                if (IsUserBlacklisted(loginRequest.Username, sessionInfo.ClientIp))
                {
                    return CreateErrorResponse("用户或IP在黑名单中", UnifiedErrorCodes.Auth_UserNotFound, "BLACKLISTED");
                }

                // 检查登录尝试次数
                if (GetLoginAttempts(loginRequest.Username) >= MaxLoginAttempts)
                {
                    return CreateErrorResponse("登录尝试次数过多，请稍后再试", UnifiedErrorCodes.Command_Timeout, "TOO_MANY_ATTEMPTS");
                }

                // 验证用户凭据
                var userSessionInfo = await ValidateUserCredentialsAsync(loginRequest, cancellationToken);
                if (userSessionInfo == null)
                {
                    IncrementLoginAttempts(loginRequest.Username);
                    return CreateErrorResponse("用户名或密码错误", UnifiedErrorCodes.Auth_PasswordError, "INVALID_CREDENTIALS");
                }

                // 重置登录尝试次数
                ResetLoginAttempts(loginRequest.Username);

                // 检查是否已经登录，如果是则发送重复登录确认请求
                var (hasExistingSessions, authorizedSessions) = CheckUserLoginStatus(loginRequest.Username, executionContext.SessionId);
                if (hasExistingSessions && !IsDuplicateLoginConfirmed(loginRequest))
                {
                    // 收集现有会话信息
                    var sessionInfos = authorizedSessions.Select(s => new Dictionary<string, object>
                {
                    { "SessionId", s.SessionID },
                    { "LoginTime", s.LoginTime },
                    { "ClientIp", s.ClientIp },
                    { "DeviceInfo", s.DeviceInfo }
                }).ToList();

                    var duplicateLoginResponse = new ResponseBase
                    {
                        Message = "您的账号已在其他地方登录，确认强制对方下线，保持当前登陆吗？",
                        IsSuccess = true
                    };
                    duplicateLoginResponse.WithMetadata("ExistingSessions", sessionInfos);

                    return duplicateLoginResponse;
                }

                // 处理重复登录确认后的逻辑
                if (IsDuplicateLoginConfirmed(loginRequest))
                {
                    // 踢掉之前的登录
                    await KickExistingSessions(authorizedSessions, loginRequest.Username);
                }

                // 更新会话信息
                UpdateSessionInfo(sessionInfo, userSessionInfo);
                SessionService.UpdateSession(sessionInfo);

                // 生成Token
                var tokenInfo = await GenerateTokenInfoAsync(userSessionInfo);

                // 返回成功结果 - 使用强类型 BaseCommand<LoginResponse>
                var loginResponse = new LoginResponse
                {
                    IsSuccess = true,
                    Message = "登录成功",
                    UserId = userSessionInfo.UserInfo.User_ID,
                    Username = userSessionInfo.UserInfo.UserName,
                    SessionId = sessionInfo.SessionID,
                    Token = tokenInfo
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
                LogError($"处理登录异常: {ex.Message}", ex);
                return CreateExceptionResponse(ex, "LOGIN_ERROR");
            }
        }

        /// <summary>
        /// 检查是否已确认重复登录（基于请求数据）
        /// </summary>
        private bool IsDuplicateLoginConfirmed(LoginRequest loginRequest)
        {
            if (loginRequest?.AdditionalData != null &&
                loginRequest.AdditionalData.ContainsKey("DuplicateLoginConfirmed"))
            {
                return Convert.ToBoolean(loginRequest.AdditionalData["DuplicateLoginConfirmed"]);
            }
            return false;
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
                // 从请求中提取Token
                string token = executionContext.Token?.AccessToken;
                // 验证Token
                if (string.IsNullOrEmpty(token))
                {
                    return CreateErrorResponse("Token不能为空", UnifiedErrorCodes.Auth_TokenInvalid, "EMPTY_TOKEN");
                }

                // 使用TokenService验证Token
                var validationResult = _tokenService.ValidateToken(token);

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
                return CreateExceptionResponse(ex, "VALIDATION_ERROR");
            }
        }

        /// <summary>
        /// 处理Token刷新命令 - 使用TokenManager提供智能刷新机制
        /// </summary>
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
                    return CreateErrorResponse("刷新Token和当前Token不能为空", UnifiedErrorCodes.Auth_TokenInvalid, "EMPTY_TOKEN");
                }

                // 使用TokenService刷新Token
                string newToken;
                try
                {
                    newToken = _tokenService.RefreshToken(refreshToken, currentToken);
                }
                catch (Exception ex)
                {
                    return CreateErrorResponse(ex.Message, UnifiedErrorCodes.Auth_TokenInvalid, "REFRESH_FAILED");
                }

                // 创建响应
                var response = new LoginResponse
                {
                    IsSuccess = true,
                    Message = "Token刷新成功",
                    Token = new TokenInfo { AccessToken = newToken }
                };

                return response;
            }
            catch (Exception ex)
            {
                LogError($"处理Token刷新异常: {ex.Message}", ex);
                return CreateExceptionResponse(ex, "REFRESH_ERROR");
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
                if (executionContext.Token != null && !string.IsNullOrEmpty(executionContext.Token.AccessToken))
                {
                    _tokenService.RevokeToken(executionContext.Token.AccessToken);
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
                LogError($"处理登出异常: {ex.Message}", ex);
                return CreateExceptionResponse(ex, "LOGOUT_ERROR");
            }
        }



        #region 核心业务逻辑方法

        /// <summary>
        /// 验证用户凭据
        /// </summary>
        private async Task<UserSessionInfo> ValidateUserCredentialsAsync(LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            //防止暴力破解攻击,时间侧信道攻击防护,降低服务器负载 这是一个 安全最佳实践 ，在身份验证系统中很常见，目的是提高系统的安全性而不是处理超时情况。
            await Task.Delay(50, cancellationToken);
            //password = EncryptionHelper.AesDecryptByHashKey(enPwd, username);
            string EnPassword = EncryptionHelper.AesEncryptByHashKey(loginRequest.Password, loginRequest.Username);

            var user = await Program.AppContextData.Db.CopyNew().Queryable<tb_UserInfo>()
                     .Where(u => u.UserName == loginRequest.Username && u.Password == EnPassword)
               .Includes(x => x.tb_employee)
                     .Includes(x => x.tb_User_Roles)
                     .SingleAsync();
            if (user != null)
            {
                return UserSessionInfo.Create(user);
            }
            return null;
        }

        /// <summary>
        /// 更新会话信息
        /// </summary>
        private void UpdateSessionInfo(SessionInfo sessionInfo, UserSessionInfo userSessionInfo, bool IsAdmin = false)
        {
            if (sessionInfo != null && userSessionInfo != null)
            {
                sessionInfo.UserId = userSessionInfo.UserInfo.User_ID;
                sessionInfo.UserName = userSessionInfo.UserInfo.UserName;
                sessionInfo.IsAuthenticated = true;
                sessionInfo.IsAdmin = IsAdmin;
                sessionInfo.UpdateActivity();
                // sessionInfo.AdditionalInfo["UserInfo"] = userInfo;
            }
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
                // 获取当前会话和已存在会话的客户端IP
                var currentSession = SessionService.GetSession(currentSessionId);
                if (currentSession == null || existingSession == null)
                    return false;

                // 如果IP地址相同，则认为是本地重复登录
                return currentSession.ClientIp == existingSession.ClientIp;
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
            return session != null && session.IsAuthenticated && !session.UserId.HasValue;
        }

        /// <summary>
        /// 请求重复登录确认
        /// </summary>
        private IResponse RequestDuplicateLoginConfirmation(IEnumerable<SessionInfo> existingSessions, CancellationToken cancellationToken)
        {
            try
            {
                // 收集现有会话信息
                var sessionInfos = existingSessions.Select(s => new Dictionary<string, object>
                {
                    { "SessionId", s.SessionID },
                    { "LoginTime", s.LoginTime },
                    { "ClientIp", s.ClientIp },
                    { "DeviceInfo", s.DeviceInfo }
                }).ToList();

                var duplicateLoginResponse = new ResponseBase
                {
                    Message = "检测到重复登录",
                    IsSuccess = true
                };
                duplicateLoginResponse.WithMetadata("ExistingSessions", sessionInfos);

                // 返回需要确认的响应
                return duplicateLoginResponse;
            }
            catch (Exception ex)
            {
                LogError($"请求重复登录确认失败: {ex.Message}", ex);
                return ResponseBase.CreateError("请求重复登录确认失败", UnifiedErrorCodes.System_InternalError.Code);
            }
        }

        /// <summary>
        /// 踢掉现有会话
        /// </summary>
        private async Task KickExistingSessions(IEnumerable<SessionInfo> existingSessions, string username)
        {
            // 主动踢掉之前的登录
            foreach (var session in existingSessions)
            {
                // 发送重复登录通知
                if (!string.IsNullOrEmpty(session.SessionID))
                {
                    SendDuplicateLoginNotification(session.SessionID, username);

                    // 使用SessionService的断开连接方法
                    await SessionService.DisconnectSessionAsync(session.SessionID, "重复登录，强制下线");
                }
            }
        }

        /// <summary>
        /// 检查用户是否已登录，并处理重复登录情况
        /// </summary>
        private (bool hasExistingSessions, IEnumerable<SessionInfo> authorizedSessions) CheckUserLoginStatus(string username, string currentSessionId)
        {
            // 获取指定用户名的所有已认证会话
            var userSessions = SessionService.GetUserSessions(username);

            // 过滤出已授权的会话
            var authorizedSessions = userSessions.Where(s => IsSessionAuthorized(s)).ToList();

            // 过滤出非本地的会话（排除本机登录）
            var remoteSessions = authorizedSessions.Where(s => !IsLocalDuplicateLogin(currentSessionId, s)).ToList();

            // 返回是否有远程会话存在以及已授权的会话列表
            return (remoteSessions.Any(), authorizedSessions);
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
                   (!string.IsNullOrEmpty(ipAddress) && BlacklistManager.IsIpBanned(ipAddress));
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
        private async Task<TokenInfo> GenerateTokenInfoAsync(UserSessionInfo userSessionInfo)
        {
            var tokenManager = Program.ServiceProvider.GetRequiredService<TokenManager>();

            var additionalClaims = new Dictionary<string, object>
            {
                { "sessionId", userSessionInfo.SessionId },
                { "clientIp", userSessionInfo.ClientIp },
                { "userId", userSessionInfo.UserInfo.User_ID }
            };

            var tokenInfo = await tokenManager.GenerateAndStoreTokenAsync(
                userSessionInfo.UserInfo.User_ID.ToString(),
                userSessionInfo.UserInfo.UserName,
                additionalClaims
            );

            return tokenInfo;
        }


        /// <summary>
        /// 验证Token
        /// 验证Token：客户端在后续的请求中携带此Token，服务器验证Token的合法性（如签名、有效期等）以确定用户身份
        /// </summary>
        private async Task<TokenValidationResult> ValidateTokenAsync(string token, string sessionId)
        {
            await Task.Delay(20);

            // 使用新的ITokenService验证Token（TokenValidationService已合并到JwtTokenService）
            var validationResult = await TokenService.ValidateTokenAsync(token);

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
                // 使用TokenManager进行Token刷新
                var refreshResult = await TokenManager.RefreshTokenAsync(refreshToken, currentToken);

                if (!refreshResult.Success)
                {
                    return (false, null, refreshResult.ErrorMessage);
                }

                return (true, refreshResult.AccessToken, null);
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
        private string GetClientIp(string SessionID)
        {
            // 4. 如果SessionInfo中没有IP，尝试从RemoteEndPoint获取
            var appSession = SessionService.GetAppSession(SessionID);
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

        #region 响应辅助方法

        /// <summary>
        /// 创建统一的错误响应
        /// </summary>
        private IResponse CreateErrorResponse(string message, ErrorCode errorCode, string customErrorCode)
        {
            return ResponseBase.CreateError($"{errorCode.Message}: {message}", errorCode.Code)
                .WithMetadata("ErrorCode", customErrorCode);
        }

        /// <summary>
        /// 创建统一的异常响应
        /// </summary>
        private IResponse CreateExceptionResponse(Exception ex, string errorCode)
        {
            return ResponseBase.CreateError($"[{ex.GetType().Name}] {ex.Message}", UnifiedErrorCodes.System_InternalError.Code)
                .WithMetadata("ErrorCode", errorCode)
                .WithMetadata("Exception", ex.Message)
                .WithMetadata("StackTrace", ex.StackTrace);
        }



        #endregion



        #endregion




    }


}