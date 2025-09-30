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
using RUINORERP.PacketSpec.Protocol;
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
using RUINORERP.PacketSpec.Commands.Handlers;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands.Cache;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ZXing.Common.ReedSolomon;
using SuperSocket.Channel;
using SuperSocket.Connection;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Concurrent;

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
        private static readonly HashSet<string> _activeSessions = new HashSet<string>();
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
        }


        /// <summary>
        /// 会话管理服务
        /// </summary>
        private ISessionService SessionService => Program.ServiceProvider.GetRequiredService<ISessionService>();

        // 添加TokenValidationService依赖
        private TokenValidationService TokenValidationService => Program.ServiceProvider.GetRequiredService<TokenValidationService>();

        /// <summary>
        /// 支持的命令类型
        /// </summary>
        public override IReadOnlyList<uint> SupportedCommands => new uint[]
        {
            AuthenticationCommands.Login.FullCode,
            AuthenticationCommands.LoginRequest.FullCode,
            AuthenticationCommands.Logout.FullCode,
            AuthenticationCommands.ValidateToken.FullCode,
            AuthenticationCommands.RefreshToken.FullCode,
            AuthenticationCommands.PrepareLogin.FullCode
        };

        /// <summary>
        /// 处理器优先级
        /// </summary>
        public override int Priority => 100;

        /// <summary>
        /// 判断是否可以处理指定命令
        /// </summary>
        public override bool CanHandle(ICommand command)
        {
            return command is LoginCommand ||
                   command.CommandIdentifier == AuthenticationCommands.Login ||
                   command.CommandIdentifier == AuthenticationCommands.LoginRequest ||
                   command.CommandIdentifier == AuthenticationCommands.Logout ||
                   command.CommandIdentifier == AuthenticationCommands.ValidateToken ||
                   command.CommandIdentifier == AuthenticationCommands.RefreshToken;
        }





        /// <summary>
        /// 核心处理方法，根据命令类型分发到对应的处理函数
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令处理结果</returns>
        protected override async Task<ResponseBase> OnHandleAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                var commandId = command.CommandIdentifier;
                if (commandId == AuthenticationCommands.Login || commandId == AuthenticationCommands.LoginRequest)
                {
                    // LoginRequest和Login命令使用相同的处理逻辑
                    return await HandleLoginAsync(command as LoginCommand, cancellationToken);
                }
                else if (commandId == AuthenticationCommands.Logout)
                {
                    return await HandleLogoutAsync(command, cancellationToken);
                }
                else if (commandId == AuthenticationCommands.ValidateToken)
                {
                    return await HandleTokenValidationAsync(command, cancellationToken);
                }
                else if (commandId == AuthenticationCommands.RefreshToken)
                {
                    return await HandleTokenRefreshAsync(command, cancellationToken);
                }
                else if (commandId == AuthenticationCommands.PrepareLogin)
                {
                    return await HandlePrepareLoginAsync(command, cancellationToken);
                }
                else
                {
                    var errorResponse = ResponseBase.CreateError($"不支持的命令类型: {command.CommandIdentifier}", 400)
                        .WithMetadata("ErrorCode", "UNSUPPORTED_COMMAND");
                    return ConvertToApiResponse(errorResponse);
                }
            }
            catch (Exception ex)
            {
                LogError($"处理登录命令异常: {ex.Message}", ex);
                var errorResponse = ResponseBase.CreateError($"处理异常: {ex.Message}", 500)
                    .WithMetadata("ErrorCode", "HANDLER_ERROR")
                    .WithMetadata("Exception", ex.Message)
                    .WithMetadata("StackTrace", ex.StackTrace);
                return ConvertToApiResponse(errorResponse);
            }
        }



        /// <summary>
        /// 处理登录命令
        /// </summary>
        private async Task<ResponseBase> HandleLoginAsync(LoginCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // 检查并发用户数限制
                if (SessionService.ActiveSessionCount >= MaxConcurrentUsers)
                {
                    return ConvertToApiResponse(ResponseFactory.Fail(
                        UnifiedErrorCodes.System_InternalError,
                        "服务器达到最大用户数限制")
                        .WithMetadata("ErrorCode", "MAX_USERS_EXCEEDED"));
                }

                // 验证命令
                var commandValidationResult = await command.ValidateAsync(cancellationToken);
                if (!commandValidationResult.IsValid)
                {
                    logger.LogWarning($"登录命令验证失败: {commandValidationResult.Errors[0].ErrorMessage}");
                    return ResponseBase.CreateError($"登录命令验证失败: {commandValidationResult.Errors[0].ErrorMessage}", 400)
                        .WithMetadata("ErrorCode", "LOGIN_VALIDATION_FAILED");
                }

                // 检查登录请求数据
                if (command.LoginRequest == null)
                {
                    return ConvertToApiResponse(ResponseFactory.Fail(
                        UnifiedErrorCodes.Command_ValidationFailed,
                        "登录请求数据不能为空")
                        .WithMetadata("ErrorCode", "EMPTY_LOGIN_REQUEST"));
                }

                if (!command.LoginRequest.IsValid())
                {
                    return ConvertToApiResponse(ResponseFactory.Fail(
                        UnifiedErrorCodes.Command_ValidationFailed,
                        "登录请求数据无效")
                        .WithMetadata("ErrorCode", "INVALID_LOGIN_REQUEST"));
                }

                // 检查黑名单
                if (IsUserBlacklisted(command.LoginRequest.Username, command.LoginRequest.ClientIp))
                {
                    return ConvertToApiResponse(ResponseFactory.Fail(
                        UnifiedErrorCodes.Auth_UserNotFound,
                        "用户或IP在黑名单中")
                        .WithMetadata("ErrorCode", "BLACKLISTED"));
                }

                // 检查登录尝试次数
                if (GetLoginAttempts(command.LoginRequest.Username) >= MaxLoginAttempts)
                {
                    return ConvertToApiResponse(ResponseFactory.Fail(
                        UnifiedErrorCodes.Command_Timeout,
                        "登录尝试次数过多，请稍后再试")
                        .WithMetadata("ErrorCode", "TOO_MANY_ATTEMPTS"));
                }

                // 验证用户凭据
                var userValidationResult = await ValidateUserCredentialsAsync(command.LoginRequest, cancellationToken);
                if (!userValidationResult.IsValid)
                {
                    IncrementLoginAttempts(command.LoginRequest.Username);
                    return ConvertToApiResponse(ResponseFactory.Fail(
                        UnifiedErrorCodes.Auth_PasswordError,
                        userValidationResult.ErrorMessage)
                        .WithMetadata("ErrorCode", "INVALID_CREDENTIALS"));
                }

                // 重置登录尝试次数
                ResetLoginAttempts(command.LoginRequest.Username);

                // 检查是否已经登录，如果是则发送重复登录确认请求
                var (hasExistingSessions, authorizedSessions) = CheckUserLoginStatus(command.LoginRequest.Username, command.ExecutionContext.SessionId);
                if (hasExistingSessions && !IsDuplicateLoginConfirmed(command))
                {
                    // 发送重复登录确认请求给客户端
                    return RequestDuplicateLoginConfirmation(command, authorizedSessions, cancellationToken);
                }

                // 处理重复登录确认后的逻辑
                if (IsDuplicateLoginConfirmed(command))
                {
                    // 踢掉之前的登录
                    KickExistingSessions(authorizedSessions, command.LoginRequest.Username);
                }

                // 获取或创建会话信息
                var sessionInfo = SessionService.GetSession(command.ExecutionContext.SessionId);
                if (sessionInfo == null)
                {
                    // 从命令或网络连接中获取客户端真实IP
                    string clientIp = GetClientIpAddress(command);
                    if (string.IsNullOrEmpty(clientIp))
                    {
                        try
                        {
                            var loginRequest = command.Request;
                            clientIp = loginRequest?.ClientIp;
                        }
                        catch
                        {
                            // 如果无法从Packet获取LoginRequest，则跳过
                        }
                    }

                    sessionInfo = SessionService.CreateSession(GetSessionId(command), clientIp);
                    if (sessionInfo == null)
                    {
                        return ConvertToApiResponse(ResponseFactory.Fail(
                            UnifiedErrorCodes.System_InternalError,
                            "创建会话失败")
                            .WithMetadata("ErrorCode", "SESSION_CREATION_FAILED"));
                    }
                }

                // 更新会话信息
                UpdateSessionInfo(sessionInfo, userValidationResult.UserSessionInfo);
                SessionService.UpdateSession(sessionInfo);

                // 记录活跃会话
                AddActiveSession(GetSessionId(command));

                // 生成Token
                var tokenInfo = GenerateTokenInfo(userValidationResult.UserSessionInfo);

                // 创建响应数据
                var responseData = CreateLoginResponse(tokenInfo, userValidationResult.UserSessionInfo);

                // 返回成功结果
                return ConvertToApiResponse(ResponseFactory.Ok(
                    new
                    {
                        UserId = userValidationResult.UserSessionInfo.UserInfo.User_ID,
                        Username = userValidationResult.UserSessionInfo.UserInfo.UserName,
                        Token = tokenInfo,
                        ExpiresIn = tokenInfo.ExpiresIn
                    },
                    "登录成功")
                    .WithMetadata("Data", new
                    {
                        UserId = userValidationResult.UserSessionInfo.UserInfo.User_ID,
                        Username = userValidationResult.UserSessionInfo.UserInfo.UserName,
                        Token = tokenInfo,
                        ExpiresIn = tokenInfo.ExpiresIn
                    }));
            }
            catch (Exception ex)
            {
                LogError($"处理登录命令异常: {ex.Message}", ex);
                return ConvertToApiResponse(ResponseFactory.Except(ex, UnifiedErrorCodes.Auth_TokenInvalid)
                    .WithMetadata("ErrorCode", "TOKEN_ERROR")
                    .WithMetadata("Exception", ex.Message)
                    .WithMetadata("StackTrace", ex.StackTrace));
            }
        }

        /// <summary>
        /// 处理登录验证命令
        /// </summary>
        private async Task<ResponseBase> HandleLoginValidationAsync(ICommand command, CancellationToken cancellationToken)
        {

            await Task.Delay(5, cancellationToken);

            // 使用SessionService验证会话有效性
            if (!SessionService.IsValidSession(GetSessionId(command)))
            {
                return ConvertToApiResponse(ResponseFactory.Fail(
                    UnifiedErrorCodes.Auth_TokenInvalid,
                    "会话无效或已过期")
                    .WithMetadata("ErrorCode", "INVALID_SESSION"));
            }

            var sessionInfo = SessionService.GetSession(GetSessionId(command));
            if (sessionInfo == null)
            {
                return ConvertToApiResponse(ResponseFactory.Fail(
                            UnifiedErrorCodes.Auth_ValidationFailed,
                            "获取会话信息失败")
                            .WithMetadata("ErrorCode", "GET_SESSION_FAILED"));
            }

            if (!sessionInfo.IsAuthenticated)
            {
                return ConvertToApiResponse(ResponseFactory.Fail(
                            UnifiedErrorCodes.Auth_ValidationFailed,
                            "会话未认证")
                            .WithMetadata("ErrorCode", "SESSION_UNAUTHENTICATED"));
            }

            // 检查会话是否仍然活跃
            if (!_activeSessions.Contains(GetSessionId(command)))
            {
                return ConvertToApiResponse(ResponseFactory.Fail(
                            UnifiedErrorCodes.Auth_SessionExpired,
                            "会话已过期")
                            .WithMetadata("ErrorCode", "SESSION_EXPIRED"));
            }

            var responseData = CreateValidationResponse(sessionInfo);

            return ConvertToApiResponse(ResponseFactory.Ok(
                new { SessionId = GetSessionId(command), UserId = sessionInfo.UserId },
                "验证成功")
                .WithMetadata("Data", new { SessionId = GetSessionId(command), UserId = sessionInfo.UserId }));
        }

        /// <summary>
        /// 处理Token验证命令 - 整合了Token验证机制
        /// </summary>
        private async Task<ResponseBase> HandleTokenValidationAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {

                var context = GetCommandContext(command);

                // 使用新的TokenValidationService验证Token
                var validationResult = await TokenValidationService.ValidateTokenAsync(context.Token);

                if (!validationResult.IsValid)
                {
                    return ConvertToApiResponse(ResponseFactory.Fail(
                        UnifiedErrorCodes.Auth_TokenInvalid,
                    "Token验证失败")
                        .WithMetadata("ErrorCode", "TOKEN_VALIDATION_FAILED"));
                }

                var responseData = CreateTokenValidationResponse(validationResult);

                return ConvertToApiResponse(ResponseFactory.Ok(
                    new { UserId = validationResult.UserId, IsValid = true },
                    "Token验证成功")
                    .WithMetadata("Data", new { UserId = validationResult.UserId, IsValid = true }));
            }
            catch (Exception ex)
            {
                LogError($"Token验证异常: {ex.Message}", ex);
                return ConvertToApiResponse(ResponseBase.CreateError($"Token验证异常: {ex.Message}", 500)
                    .WithMetadata("ErrorCode", "TOKEN_ERROR")
                    .WithMetadata("Exception", ex.Message)
                    .WithMetadata("StackTrace", ex.StackTrace));
            }
        }

        /// <summary>
        /// 处理Token刷新命令 - 整合了Token刷新机制
        /// </summary>
        private async Task<ResponseBase> HandleTokenRefreshAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                // 确保命令是BaseCommand类型
                if (command is not BaseCommand baseCommand)
                {
                    return ConvertToApiResponse(ResponseBase.CreateError("无效的命令类型", 400)
                        .WithMetadata("ErrorCode", "INVALID_COMMAND_TYPE"));
                }
                
                // 从命令中获取刷新数据
                var refreshReq = baseCommand.GetObjectData<LoginRequest>();
                if (refreshReq == null || string.IsNullOrEmpty(refreshReq.RefreshToken) || string.IsNullOrEmpty(refreshReq.Token))
                {
                    return ConvertToApiResponse(ResponseBase.CreateError("刷新Token和当前Token都不能为空", 400)
                        .WithMetadata("ErrorCode", "INVALID_REFRESH_DATA"));
                }

                // 使用新的TokenValidationService刷新Token
                var refreshResult = await TokenValidationService.RefreshTokenAsync(refreshReq.RefreshToken, refreshReq.Token);

                if (!refreshResult.IsSuccess)
                {
                    return ConvertToApiResponse(ResponseBase.CreateError(refreshResult.ErrorMessage, 401)
                        .WithMetadata("ErrorCode", "REFRESH_FAILED"));
                }

                // 创建登录响应
                var loginResponse = new LoginResponse
                {
                    UserId = long.Parse(refreshResult.UserId),
                    Username = refreshResult.UserId, // 实际项目中应该从用户信息中获取
                    AccessToken = refreshResult.AccessToken,
                    RefreshToken = refreshResult.RefreshToken,
                    ExpiresIn = refreshResult.ExpiresIn,
                    TokenType = "Bearer"
                };

                return ConvertToApiResponse(ResponseFactory.Ok(loginResponse, "Token刷新成功")
                    .WithMetadata("Data", loginResponse));
            }
            catch (Exception ex)
            {
                LogError($"Token刷新异常: {ex.Message}", ex);
                return ConvertToApiResponse(ResponseBase.CreateError($"Token刷新异常: {ex.Message}", 500)
                    .WithMetadata("ErrorCode", "REFRESH_ERROR")
                    .WithMetadata("Exception", ex.Message)
                    .WithMetadata("StackTrace", ex.StackTrace));
            }
        }

        /// <summary>
        /// 处理注销命令
        /// </summary>
        private async Task<ResponseBase> HandleLogoutAsync(ICommand command, CancellationToken cancellationToken)
        {
            LogInfo($"处理注销请求 [会话: {GetSessionId(command)}]");

            try
            {
                // 使用SessionService验证会话有效性
                if (!SessionService.IsValidSession(GetSessionId(command)))
                {
                    return ConvertToApiResponse(ResponseBase.CreateError("会话信息无效", 401)
                        .WithMetadata("ErrorCode", "INVALID_SESSION"));
                }

                var sessionInfo = SessionService.GetSession(GetSessionId(command));
                if (sessionInfo == null)
                {
                    return ConvertToApiResponse(ResponseBase.CreateError("获取会话信息失败", 404)
                        .WithMetadata("ErrorCode", "SESSION_NOT_FOUND"));
                }

                // 执行注销
                var logoutResult = await LogoutAsync(
                    GetSessionId(command),
                    sessionInfo.UserId.ToString()
                );

                if (!logoutResult)
                {
                    return ConvertToApiResponse(ResponseBase.CreateError("注销失败", 500)
                        .WithMetadata("ErrorCode", "LOGOUT_FAILED"));
                }

                var responseData = CreateLogoutResponse();

                return ConvertToApiResponse(ResponseBase.CreateSuccess(
                    message: "注销成功"
                ).WithMetadata("Data", new { LoggedOut = true, SessionId = GetSessionId(command) }));
            }
            catch (Exception ex)
            {
                LogError($"注销异常: {ex.Message}", ex);
                return ConvertToApiResponse(ResponseBase.CreateError($"注销异常: {ex.Message}", 500)
                    .WithMetadata("ErrorCode", "LOGOUT_ERROR")
                    .WithMetadata("Exception", ex.Message)
                    .WithMetadata("StackTrace", ex.StackTrace));
            }
        }

        /// <summary>
        /// 处理准备登录命令
        /// </summary>
        private async Task<ResponseBase> HandlePrepareLoginAsync(ICommand command, CancellationToken cancellationToken)
        {
            LogInfo($"处理准备登录命令 [会话: {GetSessionId(command)}]");

            try
            {
                // 创建准备登录响应
                var responseData = CreatePrepareLoginResponse();

                return ConvertToApiResponse(ResponseBase.CreateSuccess(
                    message: "准备登录完成"
                ).WithMetadata("Data", new { Status = "Ready", MaxConcurrentUsers = MaxConcurrentUsers }));
            }
            catch (Exception ex)
            {
                LogError($"处理准备登录命令异常: {ex.Message}", ex);
                return ConvertToApiResponse(ResponseBase.CreateError($"准备登录异常: {ex.Message}", 500)
                    .WithMetadata("ErrorCode", "PREPARE_LOGIN_ERROR")
                    .WithMetadata("Exception", ex.Message)
                    .WithMetadata("StackTrace", ex.StackTrace));
            }
        }

        #region 核心业务逻辑方法

                /// <summary>
        /// 验证用户凭据
        /// </summary>
        private async Task<UserValidationResult> ValidateUserCredentialsAsync(LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            await Task.Delay(50, cancellationToken);

            if (string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return new UserValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "用户名或密码不能为空"
                };
            }
            //password = EncryptionHelper.AesDecryptByHashKey(enPwd, username);
            string EnPassword = EncryptionHelper.AesEncryptByHashKey(loginRequest.Password, loginRequest.Username);

            var user = await Program.AppContextData.Db.CopyNew().Queryable<tb_UserInfo>()
                     .Where(u => u.UserName == loginRequest.Username && u.Password == EnPassword)
               .Includes(x => x.tb_employee)
                     .Includes(x => x.tb_User_Roles)
                     .SingleAsync();
            if (user != null)
            {
                return new UserValidationResult
                {
                    IsValid = true,
                    UserSessionInfo = UserSessionInfo.Create(user)
                };
            }
            else
            {
                return new UserValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "用户名或密码错误"
                };
            }
        }

        /// <summary>
        /// 更新会话信息
        /// </summary>
        private void UpdateSessionInfo(SessionInfo sessionInfo, UserSessionInfo userSessionInfo)
        {
            if (sessionInfo != null && userSessionInfo != null)
            {
                sessionInfo.UserId = userSessionInfo.UserInfo.User_ID;
                sessionInfo.Username = userSessionInfo.UserInfo.UserName;
                sessionInfo.IsAuthenticated = true;
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
                        if (_activeSessions.Contains(session.SessionID))
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
        /// 检查是否已确认重复登录
        /// </summary>
        private bool IsDuplicateLoginConfirmed(LoginCommand command)
        {
            // 检查登录请求中是否包含重复登录确认标志
            if (command?.LoginRequest?.AdditionalData != null &&
                command.LoginRequest.AdditionalData.ContainsKey("DuplicateLoginConfirmed"))
            {
                return Convert.ToBoolean(command.LoginRequest.AdditionalData["DuplicateLoginConfirmed"]);
            }
            return false;
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
        private ResponseBase RequestDuplicateLoginConfirmation(LoginCommand command,
            IEnumerable<SessionInfo> existingSessions, CancellationToken cancellationToken)
        {
            try
            {
                // 收集现有会话信息
                var sessionInfos = existingSessions.Select(s => new
                {
                    SessionId = s.SessionID,
                    LoginTime = s.LoginTime,
                    ClientIp = s.ClientIp,
                    DeviceInfo = s.DeviceInfo
                }).ToList();

                // 创建重复登录确认请求
                //var confirmationRequest = new OriginalData(
                //    (byte)CommandCategory.Authentication,
                //    new byte[] { AuthenticationCommands.DuplicateLoginNotification.OperationCode }, // DuplicateLoginNotification命令码
                //    System.Text.Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(new
                //    {
                //        Message = "您的账号已在其他地方登录",
                //        ExistingSessions = sessionInfos,
                //        ActionRequired = "ConfirmDuplicateLogin"
                //    }))
                //);

                // 发送确认请求给客户端
                //var appSession = SessionService.GetAppSession(GetSessionId(command));
                //if (appSession != null)
                //{

                //    加密后发送 这里只在构建响应数据，发送已经框架处理好了。
                //    await appSession.SendAsync(confirmationRequest.ToByteArray());
                //}

                // 返回需要确认的响应
                return ConvertToApiResponse(ResponseBase.CreateSuccess("您的账号已在其他地方登录，确认强制对方下线，保持当前登陆吗？")
                    .WithMetadata("ActionRequired", "DuplicateLoginConfirmation")
                    .WithMetadata("ExistingSessions", sessionInfos));
            }
            catch (Exception ex)
            {
                LogError($"请求重复登录确认失败: {ex.Message}", ex);
                return ConvertToApiResponse(ResponseBase.CreateError("请求重复登录确认失败", 500)
                    .WithMetadata("ErrorCode", "DUPLICATE_LOGIN_CONFIRMATION_FAILED"));
            }
        }

        /// <summary>
        /// 踢掉现有会话
        /// </summary>
        private void KickExistingSessions(IEnumerable<SessionInfo> existingSessions, string username)
        {
            // 主动踢掉之前的登录
            foreach (var session in existingSessions)
            {
                // 发送重复登录通知
                if (!string.IsNullOrEmpty(session.SessionID))
                {
                    SendDuplicateLoginNotification(session.SessionID, username);

                    // 断开之前的会话
                    var appSession = SessionService.GetAppSession(session.SessionID);
                    if (appSession != null)
                    {
                        appSession.CloseAsync(CloseReason.ProtocolError);
                    }

                    // 从活跃会话列表中移除
                    RemoveActiveSession(session.SessionID);

                    // 从会话服务中移除
                    SessionService.RemoveSession(session.SessionID);
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
                        var notificationMessage = new OriginalData(
                            (byte)CommandCategory.Authentication,
                            new byte[] { AuthenticationCommands.DuplicateLoginNotification.OperationCode },
                            System.Text.Encoding.UTF8.GetBytes($"您的账号在其他地方登录，您已被强制下线。")
                        );

                        // 发送通知消息
                        appSession.SendAsync(notificationMessage.ToByteArray());
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
        /// 添加活跃会话
        /// </summary>
        private void AddActiveSession(string sessionId)
        {
            lock (_lock)
            {
                if (_activeSessions.Count < MaxConcurrentUsers)
                {
                    _activeSessions.Add(sessionId);
                }
            }
        }

        /// <summary>
        /// 移除活跃会话
        /// </summary>
        private void RemoveActiveSession(string sessionId)
        {
            lock (_lock)
            {
                _activeSessions.Remove(sessionId);
            }
        }

        /// <summary>
        /// 生成Token信息
        /// </summary>
        private TokenInfo GenerateTokenInfo(UserSessionInfo userSessionInfo)
        {
            // 使用JwtTokenService生成真实的Token
            var tokenService = Program.ServiceProvider.GetRequiredService<ITokenService>();

            var additionalClaims = new Dictionary<string, object>
            {
                { "sessionId", userSessionInfo.SessionId },
                { "clientIp", userSessionInfo.ClientIp }
            };

            var accessToken = tokenService.GenerateToken(
                userSessionInfo.UserInfo.User_ID.ToString(),
                userSessionInfo.UserInfo.UserName,
                additionalClaims
            );

            // 生成刷新Token
            var refreshToken = Guid.NewGuid().ToString(); // 实际项目中应该生成安全的刷新Token

            return new TokenInfo
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = 3600, // 1小时
                TokenType = "Bearer"
            };
        }

        /// <summary>
        /// 创建登录响应
        /// </summary>
        private OriginalData CreateLoginResponse(TokenInfo tokenInfo, UserSessionInfo userSessionInfo)
        {

            LoginResponse loginResponse = new LoginResponse();
            loginResponse.Username = userSessionInfo.UserInfo.UserName;
            loginResponse.UserId = userSessionInfo.UserInfo.User_ID;
            loginResponse.DisplayName = userSessionInfo.UserInfo.tb_employee?.Employee_Name;
            loginResponse.AccessToken = tokenInfo.AccessToken;
            loginResponse.RefreshToken = tokenInfo.RefreshToken;
            loginResponse.ExpiresIn = tokenInfo.ExpiresIn;
            loginResponse.TokenType = tokenInfo.TokenType;

            var data = RUINORERP.PacketSpec.Serialization.UnifiedSerializationService.SerializeToBinary<LoginResponse>(loginResponse);

            // 将完整的CommandId正确分解为Category和OperationCode
            uint commandId = (uint)AuthenticationCommands.LoginResponse;
            byte category = (byte)(commandId & 0xFF); // 取低8位作为Category
            byte operationCode = (byte)((commandId >> 8) & 0xFF); // 取次低8位作为OperationCode

            return new OriginalData(
                category,
                new byte[] { operationCode },
                data
            );
        }

        /// <summary>
        /// 验证Token
        /// </summary>
        private async Task<TokenValidationResult> ValidateTokenAsync(string token, string sessionId)
        {
            await Task.Delay(20);

            // 使用新的TokenValidationService验证Token
            var validationResult = await TokenValidationService.ValidateTokenAsync(token);

            // 检查Token是否在活跃会话中
            if (validationResult.IsValid)
            {
                lock (_lock)
                {
                    if (!string.IsNullOrEmpty(sessionId) && !_activeSessions.Contains(sessionId))
                    {
                        return new TokenValidationResult
                        {
                            IsValid = false,
                            ErrorMessage = "会话已过期"
                        };
                    }
                }
            }

            return validationResult;
        }

        /// <summary>
        /// 刷新Token
        /// </summary>
        private async Task<TokenRefreshResult> RefreshTokenAsync(string refreshToken, string currentToken)
        {
            await Task.Delay(30);

            // 使用新的TokenValidationService刷新Token
            var refreshResult = await TokenValidationService.RefreshTokenAsync(refreshToken, currentToken);
            return refreshResult;
        }

        /// <summary>
        /// 执行注销
        /// </summary>
        private async Task<bool> LogoutAsync(string sessionId, string userId)
        {
            await Task.Delay(10);

            RemoveActiveSession(sessionId);
            ResetLoginAttempts(userId);

            // 通过SessionService移除会话
            return SessionService.RemoveSession(sessionId);
        }

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <returns>客户端IP地址</returns>
        private string GetClientIpAddress(ICommand command)
        {
            // 首先尝试从命令的SessionInfo中获取IP
            if (command != null && !string.IsNullOrEmpty(GetSessionId(command)))
            {
                var session = SessionService.GetSession(GetSessionId(command));
                if (session != null && !string.IsNullOrEmpty(session.ClientIp))
                {
                    return session.ClientIp;
                }

                // 如果SessionInfo中没有IP，尝试从RemoteEndPoint获取
                var appSession = SessionService.GetAppSession(GetSessionId(command));
                if (appSession != null && appSession.RemoteEndPoint != null)
                {
                    var ipEndpoint = appSession.RemoteEndPoint as System.Net.IPEndPoint;
                    if (ipEndpoint != null)
                    {
                        return ipEndpoint.Address.ToString();
                    }
                }
            }

            // 如果无法从Session获取，则返回默认值
            return "0.0.0.0"; // 使用0.0.0.0表示未知IP
        }

        #endregion

        #region 响应创建方法

        private OriginalData CreateLogoutResponse()
        {
            var responseData = "LOGGED_OUT";
            var data = System.Text.Encoding.UTF8.GetBytes(responseData);

            // 将完整的CommandId正确分解为Category和OperationCode
            uint commandId = (uint)AuthenticationCommands.Logout;
            byte category = (byte)(commandId & 0xFF); // 取低8位作为Category
            byte operationCode = (byte)((commandId >> 8) & 0xFF); // 取次低8位作为OperationCode

            return new OriginalData(
                category,
                new byte[] { operationCode },
                data
            );
        }

        /// <summary>
        /// 创建准备登录响应
        /// </summary>
        private OriginalData CreatePrepareLoginResponse()
        {
            var responseMessage = $"READY|{MaxConcurrentUsers}";
            var data = System.Text.Encoding.UTF8.GetBytes(responseMessage);

            // 将完整的CommandId正确分解为Category和OperationCode
            uint commandId = (uint)AuthenticationCommands.PrepareLogin;
            byte category = (byte)(commandId & 0xFF); // 取低8位作为Category
            byte operationCode = (byte)((commandId >> 8) & 0xFF); // 取次低8位作为OperationCode

            return new OriginalData(
                category,
                new byte[] { operationCode },
                data
            );
        }

        /// <summary>
        /// 创建登录验证响应
        /// </summary>
        private OriginalData CreateValidationResponse(SessionInfo sessionInfo)
        {
            var responseData = $"VALID|{sessionInfo.SessionID}|{sessionInfo.UserId}";
            var data = System.Text.Encoding.UTF8.GetBytes(responseData);

            // 将完整的CommandId正确分解为Category和OperationCode
            uint commandId = (uint)AuthenticationCommands.LoginValidation;
            byte category = (byte)(commandId & 0xFF); // 取低8位作为Category
            byte operationCode = (byte)((commandId >> 8) & 0xFF); // 取次低8位作为OperationCode

            return new OriginalData(
                category,
                new byte[] { operationCode },
                data
            );
        }

        /// <summary>
        /// 创建Token验证响应
        /// </summary>
        private OriginalData CreateTokenValidationResponse(TokenValidationResult validationResult)
        {
            var responseData = $"TOKEN_VALID|{validationResult.UserId}";
            var data = System.Text.Encoding.UTF8.GetBytes(responseData);

            // 将完整的CommandId正确分解为Category和OperationCode
            uint commandId = (uint)AuthenticationCommands.ValidateToken;
            byte category = (byte)(commandId & 0xFF); // 取低8位作为Category
            byte operationCode = (byte)((commandId >> 8) & 0xFF); // 取次低8位作为OperationCode

            return new OriginalData(
                category,
                new byte[] { operationCode },
                data
            );
        }

        /// <summary>
        /// 创建Token刷新响应
        /// </summary>
        private OriginalData CreateTokenRefreshResponse(TokenRefreshResult refreshResult)
        {
            var responseData = $"TOKEN_REFRESHED|{refreshResult.AccessToken}|{refreshResult.RefreshToken}|{refreshResult.ExpiresIn}";
            var data = System.Text.Encoding.UTF8.GetBytes(responseData);

            // 将完整的CommandId正确分解为Category和OperationCode
            uint commandId = (uint)AuthenticationCommands.RefreshToken;
            byte category = (byte)(commandId & 0xFF); // 取低8位作为Category
            byte operationCode = (byte)((commandId >> 8) & 0xFF); // 取次低8位作为OperationCode

            return new OriginalData(
                category,
                new byte[] { operationCode },
                data
            );
        }

       
      
        #endregion

        /// <summary>
        /// 将ResponseBase转换为ApiResponse
        /// </summary>
        /// <param name="baseResponse">基础响应对象</param>
        /// <returns>ApiResponse对象</returns>
        private ResponseBase ConvertToApiResponse(ResponseBase baseResponse)
        {
            var response = new ResponseBase
            {
                IsSuccess = baseResponse.IsSuccess,
                Message = baseResponse.Message,
                Code = baseResponse.Code,
                TimestampUtc = baseResponse.TimestampUtc,
                RequestId = baseResponse.RequestId,
                Metadata = baseResponse.Metadata,
                ExecutionTimeMs = baseResponse.ExecutionTimeMs
            };
            return response;
        }
    }


}