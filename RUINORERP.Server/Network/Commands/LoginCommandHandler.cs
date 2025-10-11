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
            
            // 使用安全方法设置支持的命令
            SetSupportedCommands(
                AuthenticationCommands.Login.FullCode,
                AuthenticationCommands.LoginRequest.FullCode,
                AuthenticationCommands.Logout.FullCode,
                AuthenticationCommands.ValidateToken.FullCode,
                AuthenticationCommands.RefreshToken.FullCode,
                AuthenticationCommands.PrepareLogin.FullCode
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
        /// Token管理器（简化版Token管理）
        /// </summary>
        private TokenManager TokenManager => Program.ServiceProvider.GetRequiredService<TokenManager>();

        /// <summary>
        /// 支持的命令类型
        /// </summary>
        public override IReadOnlyList<CommandId> SupportedCommands { get; protected set; } = Array.Empty<CommandId>();


        /// <summary>
        /// 核心处理方法，根据命令类型分发到对应的处理函数（优化版：分层解析）
        /// 支持多种命令类型：LoginCommand、BaseCommand<LoginRequest, LoginResponse>、GenericCommand<LoginRequest>
        /// 采用延迟解析策略，具体业务数据解析在对应处理方法中完成
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令处理结果</returns>
        protected override async Task<BaseCommand<IResponse>> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                var commandId = cmd.Command.CommandIdentifier;

                if (commandId == AuthenticationCommands.Login || commandId == AuthenticationCommands.LoginRequest)
                {
                    // 分层解析策略：先确定命令类型，具体数据解析延迟到处理方法中
                    return await HandleLoginCommandAsync(cmd, cancellationToken);
                }
                else if (commandId == AuthenticationCommands.Logout)
                {
                    return await HandleLogoutAsync(cmd, cancellationToken);
                }
                else if (commandId == AuthenticationCommands.ValidateToken)
                {
                    return await HandleTokenValidationAsync(cmd, cancellationToken);
                }
                else if (commandId == AuthenticationCommands.RefreshToken)
                {
                    return await HandleTokenRefreshAsync(cmd.Packet, cmd.Command, cancellationToken);
                }
                else if (commandId == AuthenticationCommands.PrepareLogin)
                {
                    return await HandlePrepareLoginAsync(cmd, cancellationToken);
                }
                else
                {
                    var errorResponse = BaseCommand<IResponse>.CreateError($"不支持的命令类型: {cmd.Command.CommandIdentifier}", 400)
                        .WithMetadata("ErrorCode", "UNSUPPORTED_COMMAND");
                    return errorResponse;
                }
            }
            catch (Exception ex)
            {
                LogError($"处理命令 {cmd.Command.CommandIdentifier} 异常: {ex.Message}", ex);
                return CreateExceptionResponse(ex, "HANDLER_ERROR");
            }
        }

        /// <summary>
        /// 分层处理登录命令（优化版：延迟解析策略）
        /// </summary>
        private async Task<BaseCommand<IResponse>> HandleLoginCommandAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                // 第一层：命令类型识别（不解析具体数据）
                var commandType = cmd.Command.GetType();

                // 第二层：根据命令类型选择对应的处理方法，具体数据解析延迟到对应方法中
                if (cmd.Command is LoginCommand loginCommand)
                {
                    // 延迟解析：在HandleLoginAsync中解析具体数据
                    return await HandleLoginAsync(cmd.Packet, loginCommand, cancellationToken);
                }
                else if (cmd.Command is BaseCommand<LoginRequest, LoginResponse> typedCommand)
                {
                    // 延迟解析：在HandleTypedLoginAsync中解析具体数据
                    return await HandleTypedLoginAsync(typedCommand, cmd.Packet, cancellationToken);
                }
                else if (cmd.Command is GenericCommand<LoginRequest> genericCommand)
                {
                    // 延迟解析：在HandleGenericLoginAsync中解析具体数据
                    return await HandleGenericLoginAsync(cmd.Packet, genericCommand, cancellationToken);
                }
                else
                {
                    return CreateErrorResponse("不支持的登录命令格式", UnifiedErrorCodes.Command_ValidationFailed, "UNSUPPORTED_LOGIN_FORMAT");
                }
            }
            catch (Exception ex)
            {
                LogError($"登录命令分层处理异常: {ex.Message}", ex);
                return CreateExceptionResponse(ex, "LAYERED_LOGIN_ERROR");
            }
        }

        /// <summary>
        /// 处理强类型登录命令（延迟解析策略）
        /// </summary>
        private async Task<BaseCommand<IResponse>> HandleTypedLoginAsync(
            BaseCommand<LoginRequest, LoginResponse> command, PacketModel Packet,
            CancellationToken cancellationToken)
        {
            var loginRequest = command.Request;
            if (loginRequest == null)
            {
                return BaseCommand<IResponse>.CreateError("登录请求数据不能为空", UnifiedErrorCodes.Command_ValidationFailed.Code);
            }

            // 使用统一的业务逻辑处理方法
            return await ProcessLoginAsync(loginRequest, Packet.ExecutionContext, cancellationToken);
        }

        /// <summary>
        /// 处理泛型登录命令
        /// </summary>
        private async Task<BaseCommand<IResponse>> HandleGenericLoginAsync(PacketModel Packet,
            GenericCommand<LoginRequest> command,
            CancellationToken cancellationToken)
        {
            var loginRequest = command.Payload;
            if (loginRequest == null)
            {
                return CreateErrorResponse("登录请求数据不能为空", UnifiedErrorCodes.Command_ValidationFailed, "EMPTY_LOGIN_REQUEST");
            }

            return await ProcessLoginAsync(loginRequest, Packet.ExecutionContext, cancellationToken);
        }

        /// <summary>
        /// 统一的登录业务逻辑处理方法
        /// 整合了所有登录相关的业务逻辑，被不同类型的登录命令处理器调用
        /// </summary>
        private async Task<BaseCommand<IResponse>> ProcessLoginAsync(
            LoginRequest loginRequest,
            CmdContext  executionContext,
            CancellationToken cancellationToken)
        {
            try
            {
                // 检查并发用户数限制
                if (SessionService.ActiveSessionCount >= MaxConcurrentUsers)
                {
                    return CreateErrorResponse("服务器达到最大用户数限制", UnifiedErrorCodes.System_InternalError, "MAX_USERS_EXCEEDED");
                }

                if (!loginRequest.IsValid())
                {
                    return CreateErrorResponse("登录请求数据无效", UnifiedErrorCodes.Command_ValidationFailed, "INVALID_LOGIN_REQUEST");
                }
                // 获取或创建会话信息
                var sessionInfo = SessionService.GetSession(executionContext.SessionId);
                if (sessionInfo == null)
                {
                    // 统一使用GetClientIp方法获取客户端IP


                    sessionInfo = SessionService.CreateSession(executionContext.SessionId, executionContext.ClientIp);
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
                var userValidationResult = await ValidateUserCredentialsAsync(loginRequest, cancellationToken);
                if (!userValidationResult.IsValid)
                {
                    IncrementLoginAttempts(loginRequest.Username);
                    return CreateErrorResponse(userValidationResult.ErrorMessage, UnifiedErrorCodes.Auth_PasswordError, "INVALID_CREDENTIALS");
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
                
                return BaseCommand<IResponse>.CreateSuccess(duplicateLoginResponse)
                    .WithMetadata("ActionRequired", "DuplicateLoginConfirmation");
                }

                // 处理重复登录确认后的逻辑
                if (IsDuplicateLoginConfirmed(loginRequest))
                {
                    // 踢掉之前的登录
                    KickExistingSessions(authorizedSessions, loginRequest.Username);
                }



                // 更新会话信息
                UpdateSessionInfo(sessionInfo, userValidationResult.UserSessionInfo);
                SessionService.UpdateSession(sessionInfo);

                // 记录活跃会话
                AddActiveSession(executionContext.SessionId);

                // 生成Token
                var tokenInfo = await GenerateTokenInfoAsync(userValidationResult.UserSessionInfo);

                // 返回成功结果 - 使用强类型 BaseCommand<LoginResponse>
                var loginResponse = new LoginResponse
                {
                    UserId = userValidationResult.UserSessionInfo.UserInfo.User_ID,
                    Username = userValidationResult.UserSessionInfo.UserInfo.UserName,
                    Token = tokenInfo,
                    IsSuccess = true
                    
                };
                return BaseCommand<IResponse>.CreateSuccess(loginResponse, "登录成功");
            }
            catch (Exception ex)
            {
                LogError($"处理登录逻辑异常: {ex.Message}", ex);
                return CreateExceptionResponse(ex, "LOGIN_PROCESS_ERROR");
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
        /// 检查是否已确认重复登录（基于命令对象）
        /// 为了保持向后兼容性而保留的重载版本
        /// </summary>
        private bool IsDuplicateLoginConfirmed(LoginCommand command)
        {
            if (command != null && command.Request != null)
            {
                return IsDuplicateLoginConfirmed(command.Request);
            }
            return false;
        }


        /// <summary>
        /// 处理登录命令 - 现在调用统一的ProcessLoginAsync方法
        /// </summary>
        private async Task<BaseCommand<IResponse>> HandleLoginAsync(PacketModel packet, ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (command == null)
                {
                    return CreateErrorResponse("命令对象为空", UnifiedErrorCodes.Command_ValidationFailed, "NULL_COMMAND");
                }

                // 验证命令
                var commandValidationResult = await command.ValidateAsync(cancellationToken);
                if (!commandValidationResult.IsValid)
                {
                    logger.LogWarning($"登录命令验证失败: {commandValidationResult.Errors[0].ErrorMessage}");
                    return CreateErrorResponse($"登录命令验证失败: {commandValidationResult.Errors[0].ErrorMessage}", UnifiedErrorCodes.Command_ValidationFailed, "LOGIN_VALIDATION_FAILED");
                }
                LoginCommand loginCommand = new LoginCommand();
                // 统一使用基类方法获取登录请求数据
                if (command is LoginCommand login)
                {
                    loginCommand = login;
                    if (login.Request == null)
                    {
                        return CreateErrorResponse("登录请求数据不能为空", UnifiedErrorCodes.Command_ValidationFailed, "EMPTY_LOGIN_REQUEST");
                    }

                }

                // 调用统一的登录业务逻辑处理方法
                return await ProcessLoginAsync(loginCommand.Request, packet.ExecutionContext, cancellationToken);
            }
            catch (Exception ex)
            {
                LogError($"处理登录命令异常: {ex.Message}", ex);
                return CreateExceptionResponse(ex, "TOKEN_ERROR");
            }
        }

        /// <summary>
        /// 处理登录验证命令
        /// </summary>
        private async Task<BaseCommand<IResponse>> HandleLoginValidationAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {

            await Task.Delay(5, cancellationToken);

            // 使用SessionService验证会话有效性
            if (!SessionService.IsValidSession(cmd.Packet.ExecutionContext.SessionId))
            {
                return BaseCommand<IResponse>.CreateError(
                    $"{UnifiedErrorCodes.Auth_TokenInvalid.Message}: 会话无效或已过期",
                    UnifiedErrorCodes.Auth_TokenInvalid.Code);
            }

            var sessionInfo = SessionService.GetSession(cmd.Packet.ExecutionContext.SessionId);
            if (sessionInfo == null)
            {
                return BaseCommand<IResponse>.CreateError(
                            $"{UnifiedErrorCodes.Auth_ValidationFailed.Message}: 获取会话信息失败",
                            UnifiedErrorCodes.Auth_ValidationFailed.Code);
            }

            if (!sessionInfo.IsAuthenticated)
            {
                return BaseCommand<IResponse>.CreateError(
                            $"{UnifiedErrorCodes.Auth_ValidationFailed.Message}: 会话未认证",
                            UnifiedErrorCodes.Auth_ValidationFailed.Code);
            }

            // 检查会话是否仍然活跃
            if (!_activeSessions.Contains(cmd.Packet.ExecutionContext.SessionId))
            {
                return BaseCommand<IResponse>.CreateError(
                            $"{UnifiedErrorCodes.Auth_SessionExpired.Message}: 会话已过期",
                            UnifiedErrorCodes.Auth_SessionExpired.Code);
            }

            var responseData = CreateValidationResponse(sessionInfo);

            var loginResponse = new LoginResponse
            {
                SessionId = cmd.Packet.ExecutionContext.SessionId,
                UserId = sessionInfo.UserId.Value,
                Message = "登录验证成功"
            };
            return BaseCommand<IResponse>.CreateSuccess(loginResponse);
        }

        /// <summary>
        /// 处理Token验证命令 - 使用TokenManager进行Token验证
        /// </summary>
        private async Task<BaseCommand<IResponse>> HandleTokenValidationAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                // 统一使用基类方法获取Token验证请求数据
                var validationRequest = ParseBusinessData<TokenValidationRequest>(cmd.Command);
                TokenInfo token = validationRequest?.Token;

                // 如果未从请求数据中获取到Token，则尝试从上下文中获取
                if (string.IsNullOrEmpty(token.AccessToken))
                {
                    var context = cmd.Packet.ExecutionContext;
                    token = context.Token;
                }

                if (string.IsNullOrEmpty(token.AccessToken))
                {
                    return BaseCommand<IResponse>.CreateError("Token不能为空", UnifiedErrorCodes.Auth_TokenInvalid.Code);
                }

                // 使用TokenManager验证Token
                var validationResult = await TokenManager.ValidateStoredTokenAsync();

                if (!validationResult.IsValid)
                {
                    return BaseCommand<IResponse>.CreateError("Token验证失败", UnifiedErrorCodes.Auth_TokenInvalid.Code);
                }

                var responseData = CreateTokenValidationResponse(validationResult);

                var validationResponse = new ResponseBase
                {
                    Message = "Token验证成功",
                    IsSuccess = true
                };
                validationResponse.WithMetadata("UserId", validationResult.UserId.ToString());
                validationResponse.WithMetadata("IsValid", "true");
                return BaseCommand<IResponse>.CreateSuccess(validationResponse);
            }
            catch (Exception ex)
            {
                LogError($"Token验证异常: {ex.Message}", ex);
                return BaseCommand<IResponse>.CreateError($"Token验证异常: {ex.Message}", UnifiedErrorCodes.System_InternalError.Code);
            }
        }

        /// <summary>
        /// 处理Token刷新命令 - 使用TokenManager提供智能刷新机制
        /// </summary>
        /// <summary>
        /// 处理Token刷新命令 - 使用最新的Token体系
        /// </summary>
        private async Task<BaseCommand<IResponse>> HandleTokenRefreshAsync(PacketModel Packet, ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                // 统一使用基类方法获取Token刷新请求数据
                var refreshReq = ParseBusinessData<TokenRefreshRequest>(command);
                if (refreshReq == null || !refreshReq.IsValid())
                {
                    return BaseCommand<IResponse>.CreateError("Token刷新请求数据无效", UnifiedErrorCodes.Command_ValidationFailed.Code);
                }

                // 使用TokenManager进行Token刷新
                try
                {
                    // 使用TokenManager的Token刷新功能
                    var refreshResult = await TokenManager.RefreshTokenAsync(refreshReq.RefreshToken, refreshReq.Token);

                    if (!refreshResult.Success)
                    {
                        return BaseCommand<IResponse>.CreateError($"Token刷新失败: {refreshResult.ErrorMessage}", UnifiedErrorCodes.Auth_TokenInvalid.Code);
                    }
                    // TODO: 要处理的逻辑
                    // 创建登录响应（使用TokenManager返回的用户信息）
                    var loginResponse = new LoginResponse
                    {
                        // UserId = refreshResult.UserId,
                        // Username = refreshResult.UserName ?? refreshResult.UserId.ToString(), // 优先使用用户名，如果没有则使用用户ID
                        // ExpiresIn = refreshResult.ex, // 从TokenManager获取实际的过期时间
                    };

                    return BaseCommand<IResponse>.CreateSuccess(loginResponse);
                }
                catch (Microsoft.IdentityModel.Tokens.SecurityTokenException ex)
                {
                    return BaseCommand<IResponse>.CreateError(ex.Message, UnifiedErrorCodes.Auth_TokenInvalid.Code);
                }
                catch (Exception ex)
                {
                    return BaseCommand<IResponse>.CreateError($"Token刷新失败: {ex.Message}", UnifiedErrorCodes.Auth_TokenInvalid.Code);
                }
            }
            catch (Exception ex)
            {
                LogError($"Token刷新异常: {ex.Message}", ex);
                return BaseCommand<IResponse>.CreateError($"Token刷新异常: {ex.Message}", UnifiedErrorCodes.System_InternalError.Code);
            }
        }

        /// <summary>
        /// 处理注销命令
        /// </summary>
        private async Task<BaseCommand<IResponse>> HandleLogoutAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            LogInfo($"处理注销请求 [会话: {cmd.Packet.ExecutionContext.SessionId}]");

            try
            {
                // 使用SessionService验证会话有效性
                if (!SessionService.IsValidSession(cmd.Packet.ExecutionContext.SessionId))
                {
                    return BaseCommand<IResponse>.CreateError("会话信息无效", UnifiedErrorCodes.Auth_SessionExpired.Code);
                }

                var sessionInfo = SessionService.GetSession(cmd.Packet.ExecutionContext.SessionId);
                if (sessionInfo == null)
                {
                    return BaseCommand<IResponse>.CreateError("获取会话信息失败", UnifiedErrorCodes.Biz_DataNotFound.Code);
                }

                // 执行注销
                var logoutResult = await LogoutAsync(
                    cmd.Packet.ExecutionContext.SessionId,
                    sessionInfo.UserId.ToString()
                );

                if (!logoutResult)
                {
                    return BaseCommand<IResponse>.CreateError("注销失败", UnifiedErrorCodes.System_InternalError.Code);
                }

                var responseData = CreateLogoutResponse();

                var logoutResponse = new ResponseBase
                {
                    Message = "注销成功",
                    IsSuccess = true
                };
                logoutResponse.WithMetadata("LoggedOut", "true");
                logoutResponse.WithMetadata("SessionId", cmd.Packet.ExecutionContext.SessionId);
                return BaseCommand<IResponse>.CreateSuccess(logoutResponse);
            }
            catch (Exception ex)
            {
                LogError($"注销异常: {ex.Message}", ex);
                return BaseCommand<IResponse>.CreateError($"注销异常: {ex.Message}", UnifiedErrorCodes.System_InternalError.Code);
            }
        }

        /// <summary>
        /// 处理准备登录命令
        /// </summary>
        private async Task<BaseCommand<IResponse>> HandlePrepareLoginAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            LogInfo($"处理准备登录命令 [会话: {cmd.Packet.ExecutionContext.SessionId}");

            try
            {
                // 创建准备登录响应
                var responseData = CreatePrepareLoginResponse();

                var prepareResponse = new ResponseBase
                {
                    Message = "准备登录成功",
                    IsSuccess = true
                };
                prepareResponse.WithMetadata("Status", "Ready");
                prepareResponse.WithMetadata("MaxConcurrentUsers", MaxConcurrentUsers.ToString());
                return BaseCommand<IResponse>.CreateSuccess(prepareResponse);
            }
            catch (Exception ex)
            {
                LogError($"处理准备登录命令异常: {ex.Message}", ex);
                return BaseCommand<IResponse>.CreateError($"处理准备登录命令异常: {ex.Message}", UnifiedErrorCodes.System_InternalError.Code);
            }
        }

        #region 核心业务逻辑方法

        /// <summary>
        /// 验证用户凭据
        /// </summary>
        private async Task<UserValidationResult> ValidateUserCredentialsAsync(LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            //防止暴力破解攻击,时间侧信道攻击防护,降低服务器负载 这是一个 安全最佳实践 ，在身份验证系统中很常见，目的是提高系统的安全性而不是处理超时情况。
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
        private BaseCommand<IResponse> RequestDuplicateLoginConfirmation(LoginCommand command,
            IEnumerable<SessionInfo> existingSessions, CancellationToken cancellationToken)
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
                return BaseCommand<IResponse>.CreateSuccess(duplicateLoginResponse);
            }
            catch (Exception ex)
            {
                LogError($"请求重复登录确认失败: {ex.Message}", ex);
                return BaseCommand<IResponse>.CreateError("请求重复登录确认失败", UnifiedErrorCodes.System_InternalError.Code);
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
                        var notificationMessage = new OriginalData(
                            (byte)CommandCategory.Authentication,
                            new byte[] { AuthenticationCommands.DuplicateLoginNotification.OperationCode },
                            System.Text.Encoding.UTF8.GetBytes($"您的账号【{username}】在其他地方登录，您已被强制下线。")
                        );

                        var encryptedData = PacketSpec.Security.EncryptedProtocol.EncryptionServerPackToClient(notificationMessage);

                        // 发送通知消息
                        appSession.SendAsync(encryptedData.ToByteArray());
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

            RemoveActiveSession(sessionId);
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
        private BaseCommand<IResponse> CreateErrorResponse(string message, ErrorCode errorCode, string customErrorCode)
        {
            return BaseCommand<IResponse>.CreateError($"{errorCode.Message}: {message}", errorCode.Code)
                .WithMetadata("ErrorCode", customErrorCode);
        }

        /// <summary>
        /// 创建统一的异常响应
        /// </summary>
        private BaseCommand<IResponse> CreateExceptionResponse(Exception ex, string errorCode)
        {
            return BaseCommand<IResponse>.CreateError($"[{ex.GetType().Name}] {ex.Message}", UnifiedErrorCodes.System_InternalError.Code)
                .WithMetadata("ErrorCode", errorCode)
                .WithMetadata("Exception", ex.Message)
                .WithMetadata("StackTrace", ex.StackTrace);
        }

        /// <summary>
        /// 创建统一的成功响应
        /// </summary>
        private BaseCommand<IResponse> CreateSuccessResponse(IResponse data, string message)
        {
            return BaseCommand<IResponse>.CreateSuccess(data, message);
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
        private OriginalData CreateTokenRefreshResponse(LoginResponse refreshResponse)
        {
            var responseData = $"TOKEN_REFRESHED|{refreshResponse.Token.AccessToken}|{refreshResponse.Token.RefreshToken}|{refreshResponse.Token.ExpiresIn}";
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

        #endregion


    }


}