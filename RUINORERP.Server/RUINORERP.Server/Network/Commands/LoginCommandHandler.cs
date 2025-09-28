using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
    public class LoginCommandHandler : CommandHandlerBase
    {
        private const int MaxLoginAttempts = 5;
        private const int MaxConcurrentUsers = 1000;
        private static readonly ConcurrentDictionary<string, int> _loginAttempts = new ConcurrentDictionary<string, int>();
        private static readonly HashSet<string> _activeSessions = new HashSet<string>();
        private static readonly object _lock = new object();

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
        protected override async Task<ResponseBase> ProcessCommandAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
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

                    // 新类统一转泛型
                    //if (commandId.GetType().IsGenericType &&
                    //    commandId.GetType().GetGenericTypeDefinition() == typeof(GenericCommand<>))
                    //{
                    //    var payload = commandId.GetSerializableData();
                    //    return commandId.FullCode switch
                    //    {
                    //        var id when id == AuthenticationCommands.Login
                    //            => HandleLogin((LoginPayload)payload),
                    //        var id when id == CacheCommands.CacheUpdate
                    //            => HandleCacheUpdate((CacheUpdatePayload)payload),
                    //        _ => Task.FromResult(CommandResult.Failure("未实现"))
                    //    };
                    //}

                    // 统一转基类
                    //    var generic = (ICommand)cmd;
                    //var payload = generic.GetSerializableData();   // 就是 LoginPayLoad / LogoutPayLoad …

                    //return cmd.CommandIdentifier switch
                    //{
                    //    var id when id == AuthenticationCommands.Login =>
                    //        HandleLogin((LoginPayLoad)payload),

                    //    var id when id == AuthenticationCommands.Logout =>
                    //        HandleLogout((LogoutPayLoad)payload),

                    //    _ => Task.FromResult(CommandResult.Failure("未实现"))
                    //};


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
            catch (Exception ex)
            {
                LogError($"处理缓存同步命令异常: {ex.Message}", ex);
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
                    return ConvertToApiResponse(ResponseBase.CreateError("服务器达到最大用户数限制", 400)
                        .WithMetadata("ErrorCode", "MAX_USERS_EXCEEDED"));
                }

                // 验证登录请求数据
                if (command.LoginRequest == null)
                {
                    return ConvertToApiResponse(ResponseBase.CreateError("登录请求数据不能为空", 400)
                        .WithMetadata("ErrorCode", "EMPTY_LOGIN_REQUEST"));
                }

                if (!command.LoginRequest.IsValid())
                {
                    return ConvertToApiResponse(ResponseBase.CreateError("登录请求数据无效", 400)
                        .WithMetadata("ErrorCode", "INVALID_LOGIN_REQUEST"));
                }

                // 检查黑名单
                if (IsUserBlacklisted(command.LoginRequest.Username, command.LoginRequest.ClientIp))
                {
                    return ConvertToApiResponse(ResponseBase.CreateError("用户或IP在黑名单中", 403)
                        .WithMetadata("ErrorCode", "BLACKLISTED"));
                }

                // 检查登录尝试次数
                if (GetLoginAttempts(command.LoginRequest.Username) >= MaxLoginAttempts)
                {
                    return ConvertToApiResponse(ResponseBase.CreateError("登录尝试次数过多，请稍后再试", 429)
                        .WithMetadata("ErrorCode", "TOO_MANY_ATTEMPTS"));
                }

                // 验证用户凭据
                var validationResult = await ValidateUserCredentialsAsync(command.LoginRequest, cancellationToken);
                if (!validationResult.IsValid)
                {
                    IncrementLoginAttempts(command.LoginRequest.Username);
                    return ConvertToApiResponse(ResponseBase.CreateError(validationResult.ErrorMessage, 401)
                        .WithMetadata("ErrorCode", "LOGIN_FAILED"));
                }

                // 重置登录尝试次数
                ResetLoginAttempts(command.LoginRequest.Username);

                // 检查是否已经登录，如果是则发送重复登录确认请求
                var existingSessions = CheckExistingUserSessions(command.LoginRequest.Username);
                if (existingSessions.Any() && !IsDuplicateLoginConfirmed(command))
                {
                    // 发送重复登录确认请求给客户端
                    return await RequestDuplicateLoginConfirmation(command, existingSessions, cancellationToken);
                }

                // 处理重复登录确认后的逻辑
                if (IsDuplicateLoginConfirmed(command))
                {
                    // 踢掉之前的登录
                    KickExistingSessions(existingSessions, command.LoginRequest.Username);
                }

                // 获取或创建会话信息
                var sessionInfo = SessionService.GetSession(command.SessionID);
                if (sessionInfo == null)
                {
                    // 从命令或网络连接中获取客户端真实IP
                    string clientIp = GetClientIpAddress(command);
                    sessionInfo = SessionService.CreateSession(command.SessionID, clientIp);
                    if (sessionInfo == null)
                    {
                        return ConvertToApiResponse(ResponseBase.CreateError("创建会话失败", 500)
                            .WithMetadata("ErrorCode", "SESSION_CREATION_FAILED"));
                    }
                }

                // 更新会话信息
                UpdateSessionInfo(sessionInfo, validationResult.UserSessionInfo);
                SessionService.UpdateSession(sessionInfo);

                // 记录活跃会话
                AddActiveSession(command.SessionID);

                // 生成Token
                var tokenInfo = GenerateTokenInfo(validationResult.UserSessionInfo);

                // 创建响应数据
                var responseData = CreateLoginResponse(tokenInfo, validationResult.UserSessionInfo);

                // 返回成功结果
                return ConvertToApiResponse(ResponseBase.CreateSuccess(
                    message: "登录成功"
                ).WithMetadata("Data", new
                {
                    UserId = validationResult.UserSessionInfo.UserInfo.User_ID,
                    Username = validationResult.UserSessionInfo.UserInfo.UserName,
                    Token = tokenInfo.Token,
                    ExpiresIn = tokenInfo.ExpiresIn
                }));
            }
            catch (Exception ex)
            {
                LogError($"处理登录命令异常: {ex.Message}", ex);
                var errorResponse = ResponseBase.CreateError($"登录异常: {ex.Message}", 500)
                    .WithMetadata("ErrorCode", "LOGIN_EXCEPTION")
                    .WithMetadata("Exception", ex.Message)
                    .WithMetadata("StackTrace", ex.StackTrace);
                return ConvertToApiResponse(errorResponse);
            }
        }

        /// <summary>
        /// 处理登录验证命令
        /// </summary>
        private async Task<ResponseBase> HandleLoginValidationAsync(ICommand command, CancellationToken cancellationToken)
        {
            LogInfo($"处理登录验证 [会话: {command.SessionID}]");

            await Task.Delay(5, cancellationToken);

            // 使用SessionService验证会话有效性
            if (!SessionService.IsValidSession(command.SessionID))
            {
                return ConvertToApiResponse(ResponseBase.CreateError("会话无效", 401)
                    .WithMetadata("ErrorCode", "INVALID_SESSION"));
            }

            var sessionInfo = SessionService.GetSession(command.SessionID);
            if (sessionInfo == null)
            {
                return ConvertToApiResponse(ResponseBase.CreateError("获取会话信息失败", 404)
                    .WithMetadata("ErrorCode", "SESSION_NOT_FOUND"));
            }

            if (!sessionInfo.IsAuthenticated)
            {
                return ConvertToApiResponse(ResponseBase.CreateError("会话未认证", 401)
                    .WithMetadata("ErrorCode", "SESSION_NOT_AUTHENTICATED"));
            }

            // 检查会话是否仍然活跃
            if (!_activeSessions.Contains(command.SessionID))
            {
                return ConvertToApiResponse(ResponseBase.CreateError("会话已过期", 401)
                    .WithMetadata("ErrorCode", "SESSION_EXPIRED"));
            }

            var responseData = CreateValidationResponse(sessionInfo);

            return ConvertToApiResponse(ResponseBase.CreateSuccess(
                message: "验证成功"
            ).WithMetadata("Data", new { SessionId = command.SessionID, UserId = sessionInfo.UserId }));
        }

        /// <summary>
        /// 处理Token验证命令 - 整合了Token验证机制
        /// </summary>
        private async Task<ResponseBase> HandleTokenValidationAsync(ICommand command, CancellationToken cancellationToken)
        {
            LogInfo($"处理Token验证 [会话: {command.SessionID}]");

            try
            {
                var tokenData = command.Packet.GetJsonData<TokenData>();
                if (string.IsNullOrEmpty(tokenData.Token))
                {
                    return ConvertToApiResponse(ResponseBase.CreateError("Token不能为空", 400)
                        .WithMetadata("ErrorCode", "INVALID_TOKEN"));
                }

                // 验证Token
                var validationResult = await ValidateTokenAsync(tokenData.Token, command.SessionID);

                if (!validationResult.IsValid)
                {
                    return ConvertToApiResponse(ResponseBase.CreateError(validationResult.ErrorMessage, 401)
                        .WithMetadata("ErrorCode", "TOKEN_VALIDATION_FAILED"));
                }

                var responseData = CreateTokenValidationResponse(validationResult);

                return ConvertToApiResponse(ResponseBase.CreateSuccess(
                    message: "Token验证成功"
                ).WithMetadata("Data", new { UserId = validationResult.UserId, IsValid = true }));
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
            LogInfo($"处理Token刷新 [会话: {command.SessionID}]");

            try
            {
                var refreshData = command.Packet.GetJsonData<RefreshData>();
                if (string.IsNullOrEmpty(refreshData.RefreshToken) || string.IsNullOrEmpty(refreshData.CurrentToken))
                {
                    return ConvertToApiResponse(ResponseBase.CreateError("刷新Token和当前Token都不能为空", 400)
                        .WithMetadata("ErrorCode", "INVALID_REFRESH_DATA"));
                }

                // 刷新Token
                var refreshResult = await RefreshTokenAsync(refreshData.RefreshToken, refreshData.CurrentToken);

                if (!refreshResult.IsSuccess)
                {
                    return ConvertToApiResponse(ResponseBase.CreateError(refreshResult.ErrorMessage, 401)
                        .WithMetadata("ErrorCode", "REFRESH_FAILED"));
                }

                var responseData = CreateTokenRefreshResponse(refreshResult);

                return ConvertToApiResponse(ResponseBase.CreateSuccess(
                    message: "Token刷新成功"
                ).WithMetadata("Data", new
                {
                    AccessToken = refreshResult.AccessToken,
                    RefreshToken = refreshResult.RefreshToken
                }));
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
            LogInfo($"处理注销请求 [会话: {command.SessionID}]");

            try
            {
                // 使用SessionService验证会话有效性
                if (!SessionService.IsValidSession(command.SessionID))
                {
                    return ConvertToApiResponse(ResponseBase.CreateError("会话信息无效", 401)
                        .WithMetadata("ErrorCode", "INVALID_SESSION"));
                }

                var sessionInfo = SessionService.GetSession(command.SessionID);
                if (sessionInfo == null)
                {
                    return ConvertToApiResponse(ResponseBase.CreateError("获取会话信息失败", 404)
                        .WithMetadata("ErrorCode", "SESSION_NOT_FOUND"));
                }

                // 执行注销
                var logoutResult = await LogoutAsync(
                    command.SessionID,
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
                ).WithMetadata("Data", new { LoggedOut = true, SessionId = command.SessionID }));
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
            LogInfo($"处理准备登录命令 [会话: {command.SessionID}]");

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
        /// 解析登录数据
        /// </summary>
        private LoginData ParseLoginData(OriginalData originalData)
        {
            try
            {
                if (originalData.One == null || originalData.One.Length == 0)
                    return null;

                var dataString = System.Text.Encoding.UTF8.GetString(originalData.One);
                var parts = dataString.Split('|');

                if (parts.Length >= 2)
                {
                    return new LoginData
                    {
                        Username = parts[0],
                        Password = parts[1],
                        ClientInfo = parts.Length > 2 ? parts[2] : null
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                LogError($"解析登录数据异常: {ex.Message}", ex);
                return null;
            }
        }

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
        /// 请求重复登录确认
        /// </summary>
        private async Task<ResponseBase> RequestDuplicateLoginConfirmation(LoginCommand command,
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
                var confirmationRequest = new OriginalData(
                    (byte)CommandCategory.Authentication,
                    new byte[] { AuthenticationCommands.DuplicateLoginNotification.OperationCode }, // DuplicateLoginNotification命令码
                    System.Text.Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        Message = "您的账号已在其他地方登录",
                        ExistingSessions = sessionInfos,
                        ActionRequired = "ConfirmDuplicateLogin"
                    }))
                );

                // 发送确认请求给客户端
                var appSession = SessionService.GetAppSession(command.SessionID);
                if (appSession != null)
                {
                    await appSession.SendAsync(confirmationRequest.ToByteArray());
                }

                // 返回需要确认的响应
                return ConvertToApiResponse(ResponseBase.CreateSuccess("需要重复登录确认")
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
        private bool IsUserAlreadyLoggedIn(string username)
        {
            // 获取指定用户名的所有已认证会话
            var authenticatedSessions = SessionService.GetUserSessions(username);

            if (authenticatedSessions.Any())
            {
                // 主动踢掉之前的登录
                KickExistingSessions(authenticatedSessions, username);
                return true; // 表示之前有登录，但已被踢掉
            }

            return false; // 表示之前没有登录
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
            return new TokenInfo
            {
                AccessToken = $"access_token_{userSessionInfo.UserInfo.User_ID}_{Guid.NewGuid()}",
                RefreshToken = $"refresh_token_{userSessionInfo.UserInfo.User_ID}_{Guid.NewGuid()}",
                ExpiresIn = 3600,
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

            // 简化的Token验证逻辑
            if (string.IsNullOrEmpty(token) || !token.StartsWith("access_token_"))
            {
                return new TokenValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "无效的Token格式"
                };
            }

            // 检查Token是否在活跃会话中
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

            // 提取用户ID
            var parts = token.Split('_');
            if (parts.Length >= 3)
            {
                return new TokenValidationResult
                {
                    IsValid = true,
                    UserId = parts[2]
                };
            }

            return new TokenValidationResult
            {
                IsValid = false,
                ErrorMessage = "Token解析失败"
            };
        }

        /// <summary>
        /// 刷新Token
        /// </summary>
        private async Task<TokenRefreshResult> RefreshTokenAsync(string refreshToken, string currentToken)
        {
            await Task.Delay(30);

            if (string.IsNullOrEmpty(refreshToken) || !refreshToken.StartsWith("refresh_token_"))
            {
                return new TokenRefreshResult
                {
                    IsSuccess = false,
                    ErrorMessage = "无效的刷新Token"
                };
            }

            // 验证当前Token
            var validationResult = await ValidateTokenAsync(currentToken, null);
            if (!validationResult.IsValid)
            {
                return new TokenRefreshResult
                {
                    IsSuccess = false,
                    ErrorMessage = "当前Token无效"
                };
            }

            // 生成新的Token
            return new TokenRefreshResult
            {
                IsSuccess = true,
                AccessToken = $"access_token_{validationResult.UserId}_{Guid.NewGuid()}",
                RefreshToken = $"refresh_token_{validationResult.UserId}_{Guid.NewGuid()}",
                ExpiresIn = 3600
            };
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
            if (command != null && !string.IsNullOrEmpty(command.SessionID))
            {
                var session = SessionService.GetSession(command.SessionID);
                if (session != null && !string.IsNullOrEmpty(session.ClientIp))
                {
                    return session.ClientIp;
                }

                // 如果SessionInfo中没有IP，尝试从RemoteEndPoint获取
                var appSession = SessionService.GetAppSession(command.SessionID);
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

        private TokenData ParseTokenData(OriginalData originalData)
        {
            if (originalData.One == null || originalData.One.Length == 0)
                return new TokenData();

            try
            {
                var dataString = System.Text.Encoding.UTF8.GetString(originalData.One);
                return new TokenData { Token = dataString.Trim() };
            }
            catch
            {
                return new TokenData();
            }
        }

        private RefreshData ParseRefreshData(OriginalData originalData)
        {
            if (originalData.One == null || originalData.One.Length == 0)
                return new RefreshData();

            try
            {
                var dataString = System.Text.Encoding.UTF8.GetString(originalData.One);
                var parts = dataString.Split('|');

                if (parts.Length >= 2)
                {
                    return new RefreshData
                    {
                        RefreshToken = parts[0],
                        CurrentToken = parts[1]
                    };
                }

                return new RefreshData();
            }
            catch
            {
                return new RefreshData();
            }
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
                Timestamp = baseResponse.Timestamp,
                RequestId = baseResponse.RequestId,
                Metadata = baseResponse.Metadata,
                ExecutionTimeMs = baseResponse.ExecutionTimeMs
            };
            return response;
        }
    }


}