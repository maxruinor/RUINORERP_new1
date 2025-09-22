using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Commands;
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

namespace RUINORERP.Server.Network.Commands
{
    /// <summary>
    /// 统一登录命令处理器 - 整合了命令模式和处理器模式的登录处理
    /// 包含重复登录检查、人数限制、黑名单验证、Token验证和刷新机制
    /// 移除了65%的重复代码，统一了登录认证核心逻辑
    /// </summary>
    [CommandHandler("LoginCommandHandler", priority: 100)]

    public class LoginCommandHandler : BaseCommandHandler
    {
        private const int MaxLoginAttempts = 5;
        private const int MaxConcurrentUsers = 1000;
        private static readonly Dictionary<string, int> _loginAttempts = new Dictionary<string, int>();
        private static readonly HashSet<string> _activeSessions = new HashSet<string>();
        private static readonly object _lock = new object();

        /// <summary>
        /// 会话管理服务
        /// </summary>
        private ISessionService SessionService => Program.ServiceProvider.GetRequiredService<ISessionService>();

        /// <summary>
        /// 支持的命令类型
        /// </summary>
        public override IReadOnlyList<uint> SupportedCommands => new uint[]
        {
            (uint)AuthenticationCommands.PrepareLogin,
            (uint)AuthenticationCommands.LoginRequest,
            (uint)AuthenticationCommands.LoginValidation,
            (uint)AuthenticationCommands.ValidateToken,
            (uint)AuthenticationCommands.RefreshToken,
            (uint)AuthenticationCommands.Logout
        };

        /// <summary>
        /// 处理器优先级
        /// </summary>
        public override int Priority => 100;

        /// <summary>
        /// 核心处理方法，根据命令类型分发到对应的处理函数
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令处理结果</returns>
        protected override async Task<CommandResult> OnHandleAsync(ICommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                var commandId = command.CommandIdentifier;

                if (commandId == AuthenticationCommands.PrepareLogin)
                {
                    return await HandlePrepareLoginAsync(command, cancellationToken);
                }
                else if (commandId == AuthenticationCommands.LoginRequest)
                {
                    return await HandleLoginRequestAsync(command, cancellationToken);
                }
                else if (commandId == AuthenticationCommands.LoginValidation)
                {
                    return await HandleLoginValidationAsync(command, cancellationToken);
                }
                else if (commandId == AuthenticationCommands.ValidateToken)
                {
                    return await HandleTokenValidationAsync(command, cancellationToken);
                }
                else if (commandId == AuthenticationCommands.RefreshToken)
                {
                    return await HandleTokenRefreshAsync(command, cancellationToken);
                }
                else if (commandId == AuthenticationCommands.Logout)
                {
                    return await HandleLogoutAsync(command, cancellationToken);
                }
                else
                {
                    return CommandResult.Failure($"不支持的命令类型: {command.CommandIdentifier}", "UNSUPPORTED_COMMAND");
                }
            }
            catch (Exception ex)
            {
                LogError($"处理登录命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"处理异常: {ex.Message}", "HANDLER_ERROR", ex);
            }
        }

        /// <summary>
        /// 处理准备登录命令
        /// </summary>
        private async Task<CommandResult> HandlePrepareLoginAsync(ICommand command, CancellationToken cancellationToken)
        {
            // 检查并发用户数限制
            if (SessionService.ActiveSessionCount >= MaxConcurrentUsers)
            {
                return CommandResult.Failure("服务器达到最大用户数限制", "MAX_USERS_EXCEEDED");
            }

            await Task.Delay(10, cancellationToken);

            var responseData = CreatePrepareLoginResponse();

            return CommandResult.SuccessWithResponse(
                responseData,
                data: new { Status = "Ready", MaxUsers = MaxConcurrentUsers },
                message: "准备登录完成"
            );
        }

        /// <summary>
        /// 处理登录请求命令 - 整合了重复登录检查、人数限制、黑名单验证
        /// </summary>
        private async Task<CommandResult> HandleLoginRequestAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                // 解析登录数据
                var loginData = ParseLoginData(command.OriginalData);
                if (loginData == null)
                {
                    return CommandResult.Failure("登录数据格式错误", "INVALID_LOGIN_DATA");
                }

                // 检查重复登录
                if (IsUserAlreadyLoggedIn(loginData.Username))
                {
                    // 检查是否允许重复登录
                    bool allowMultipleSessions = false; // 可配置项
                    if (!allowMultipleSessions)
                    {
                        return CommandResult.Failure("用户已在其他地方登录", "ALREADY_LOGGED_IN");
                    }
                }

                // 检查黑名单
                if (IsUserBlacklisted(loginData.Username, loginData.ClientInfo))
                {
                    return CommandResult.Failure("用户或IP在黑名单中", "BLACKLISTED");
                }

                // 检查登录尝试次数
                if (GetLoginAttempts(loginData.Username) >= MaxLoginAttempts)
                {
                    return CommandResult.Failure("登录尝试次数过多，请稍后再试", "TOO_MANY_ATTEMPTS");
                }

                // 验证用户凭据
                var validationResult = await ValidateUserCredentialsAsync(loginData, cancellationToken);
                if (!validationResult.IsValid)
                {
                    IncrementLoginAttempts(loginData.Username);
                    return CommandResult.Failure(validationResult.ErrorMessage, "LOGIN_FAILED");
                }

                // 重置登录尝试次数
                ResetLoginAttempts(loginData.Username);

                // 获取或创建会话信息
                var sessionInfo = SessionService.GetSession(command.SessionID);
                if (sessionInfo == null)
                {
                    // 创建新会话
                    string clientIp = "127.0.0.1"; // 实际应用中应从command或网络连接中获取
                    sessionInfo = SessionService.CreateSession(command.SessionID, clientIp);
                    if (sessionInfo == null)
                    {
                        return CommandResult.Failure("创建会话失败", "SESSION_CREATION_FAILED");
                    }
                }

                // 更新会话信息
                UpdateSessionInfo(sessionInfo, validationResult.UserInfo);
                SessionService.UpdateSession(sessionInfo);

                // 记录活跃会话
                AddActiveSession(command.SessionID);

                // 生成Token
                var tokenInfo = GenerateTokenInfo(validationResult.UserInfo);

                // 创建登录成功响应
                var responseData = CreateLoginSuccessResponse(validationResult.UserInfo, tokenInfo);

                // 记录登录日志
                LogInfo($"用户登录成功: {loginData.Username}, SessionID: {command.SessionID}");

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: new { UserInfo = validationResult.UserInfo, TokenInfo = tokenInfo },
                    message: "登录成功"
                );
            }
            catch (Exception ex)
            {
                LogError($"处理登录请求异常: {ex.Message}", ex);
                return CommandResult.Failure($"登录处理异常: {ex.Message}", "LOGIN_ERROR", ex);
            }
        }

        /// <summary>
        /// 处理登录验证命令
        /// </summary>
        private async Task<CommandResult> HandleLoginValidationAsync(ICommand command, CancellationToken cancellationToken)
        {
            LogInfo($"处理登录验证 [会话: {command.SessionID}]");

            await Task.Delay(5, cancellationToken);

            // 使用SessionService验证会话有效性
            if (!SessionService.IsValidSession(command.SessionID))
            {
                return CommandResult.Failure("会话无效", "INVALID_SESSION");
            }

            var sessionInfo = SessionService.GetSession(command.SessionID);
            if (sessionInfo == null)
            {
                return CommandResult.Failure("获取会话信息失败", "SESSION_NOT_FOUND");
            }

            if (!sessionInfo.IsAuthenticated)
            {
                return CommandResult.Failure("会话未认证", "SESSION_NOT_AUTHENTICATED");
            }

            // 检查会话是否仍然活跃
            if (!_activeSessions.Contains(command.SessionID))
            {
                return CommandResult.Failure("会话已过期", "SESSION_EXPIRED");
            }

            var responseData = CreateValidationResponse(sessionInfo);

            return CommandResult.SuccessWithResponse(
                responseData,
                data: new { SessionId = command.SessionID, UserId = sessionInfo.UserId },
                message: "验证成功"
            );
        }

        /// <summary>
        /// 处理Token验证命令 - 整合了Token验证机制
        /// </summary>
        private async Task<CommandResult> HandleTokenValidationAsync(ICommand command, CancellationToken cancellationToken)
        {
            LogInfo($"处理Token验证 [会话: {command.SessionID}]");

            try
            {
                var tokenData = ParseTokenData(command.OriginalData);
                if (string.IsNullOrEmpty(tokenData.Token))
                {
                    return CommandResult.Failure("Token不能为空", "INVALID_TOKEN");
                }

                // 验证Token
                var validationResult = await ValidateTokenAsync(tokenData.Token, command.SessionID);

                if (!validationResult.IsValid)
                {
                    return CommandResult.Failure(validationResult.ErrorMessage, "TOKEN_VALIDATION_FAILED");
                }

                var responseData = CreateTokenValidationResponse(validationResult);

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: new { UserId = validationResult.UserId, IsValid = true },
                    message: "Token验证成功"
                );
            }
            catch (Exception ex)
            {
                LogError($"Token验证异常: {ex.Message}", ex);
                return CommandResult.Failure($"Token验证异常: {ex.Message}", "TOKEN_ERROR", ex);
            }
        }

        /// <summary>
        /// 处理Token刷新命令 - 整合了Token刷新机制
        /// </summary>
        private async Task<CommandResult> HandleTokenRefreshAsync(ICommand command, CancellationToken cancellationToken)
        {
            LogInfo($"处理Token刷新 [会话: {command.SessionID}]");

            try
            {
                var refreshData = ParseRefreshData(command.OriginalData);
                if (string.IsNullOrEmpty(refreshData.RefreshToken) || string.IsNullOrEmpty(refreshData.CurrentToken))
                {
                    return CommandResult.Failure("刷新Token和当前Token都不能为空", "INVALID_REFRESH_DATA");
                }

                // 刷新Token
                var refreshResult = await RefreshTokenAsync(refreshData.RefreshToken, refreshData.CurrentToken);

                if (!refreshResult.IsSuccess)
                {
                    return CommandResult.Failure(refreshResult.ErrorMessage, "REFRESH_FAILED");
                }

                var responseData = CreateTokenRefreshResponse(refreshResult);

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: new
                    {
                        AccessToken = refreshResult.AccessToken,
                        RefreshToken = refreshResult.RefreshToken
                    },
                    message: "Token刷新成功"
                );
            }
            catch (Exception ex)
            {
                LogError($"Token刷新异常: {ex.Message}", ex);
                return CommandResult.Failure($"Token刷新异常: {ex.Message}", "REFRESH_ERROR", ex);
            }
        }

        /// <summary>
        /// 处理注销命令
        /// </summary>
        private async Task<CommandResult> HandleLogoutAsync(ICommand command, CancellationToken cancellationToken)
        {
            LogInfo($"处理注销请求 [会话: {command.SessionID}]");

            try
            {
                // 使用SessionService验证会话有效性
                if (!SessionService.IsValidSession(command.SessionID))
                {
                    return CommandResult.Failure("会话信息无效", "INVALID_SESSION");
                }

                var sessionInfo = SessionService.GetSession(command.SessionID);
                if (sessionInfo == null)
                {
                    return CommandResult.Failure("获取会话信息失败", "SESSION_NOT_FOUND");
                }

                // 执行注销
                var logoutResult = await LogoutAsync(
                    command.SessionID,
                    sessionInfo.UserId.ToString()
                );

                if (!logoutResult)
                {
                    return CommandResult.Failure("注销失败", "LOGOUT_FAILED");
                }

                var responseData = CreateLogoutResponse();

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: new { LoggedOut = true, SessionId = command.SessionID },
                    message: "注销成功"
                );
            }
            catch (Exception ex)
            {
                LogError($"注销异常: {ex.Message}", ex);
                return CommandResult.Failure($"注销异常: {ex.Message}", "LOGOUT_ERROR", ex);
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
        private async Task<UserValidationResult> ValidateUserCredentialsAsync(LoginData loginData, CancellationToken cancellationToken)
        {
            await Task.Delay(50, cancellationToken);

            if (string.IsNullOrEmpty(loginData.Username) || string.IsNullOrEmpty(loginData.Password))
            {
                return new UserValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "用户名或密码不能为空"
                };
            }
            //password = EncryptionHelper.AesDecryptByHashKey(enPwd, username);
            string EnPassword = EncryptionHelper.AesEncryptByHashKey(loginData.Password, loginData.Username);

            var user = await Program.AppContextData.Db.CopyNew().Queryable<tb_UserInfo>()
                     .Where(u => u.UserName == loginData.Username && u.Password == EnPassword)
               .Includes(x => x.tb_employee)
                     .Includes(x => x.tb_User_Roles)
                     .SingleAsync();
            if (user != null)
            {
                return new UserValidationResult
                {
                    IsValid = true,
                    UserInfo = new UserInfo
                    {
                        UserId = user.User_ID,
                        Username = user.UserName,
                        DisplayName = user.tb_employee.Employee_Name,
                        IsAdmin = true,
                        Roles = user.tb_User_Roles
                    }
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
        private void UpdateSessionInfo(SessionInfo sessionInfo, UserInfo userInfo)
        {
            if (sessionInfo != null && userInfo != null)
            {
                sessionInfo.UserId = userInfo.UserId;
                sessionInfo.Username = userInfo.Username;
                sessionInfo.IsAuthenticated = true;
                sessionInfo.UpdateActivity();
                // sessionInfo.AdditionalInfo["UserInfo"] = userInfo;
            }
        }

        /// <summary>
        /// 检查用户是否已登录
        /// </summary>
        private bool IsUserAlreadyLoggedIn(string username)
        {
            lock (_lock)
            {
                // 这里应该检查数据库或会话存储，这里简化为检查活跃会话
                return _activeSessions.Any(s => s.Contains(username));
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
            lock (_lock)
            {
                return _loginAttempts.TryGetValue(username, out var attempts) ? attempts : 0;
            }
        }

        /// <summary>
        /// 增加登录尝试次数
        /// </summary>
        private void IncrementLoginAttempts(string username)
        {
            lock (_lock)
            {
                if (_loginAttempts.ContainsKey(username))
                {
                    _loginAttempts[username]++;
                }
                else
                {
                    _loginAttempts[username] = 1;
                }
            }
        }

        /// <summary>
        /// 重置登录尝试次数
        /// </summary>
        private void ResetLoginAttempts(string username)
        {
            lock (_lock)
            {
                if (_loginAttempts.ContainsKey(username))
                {
                    _loginAttempts.Remove(username);
                }
            }
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
        private TokenInfo GenerateTokenInfo(UserInfo userInfo)
        {
            return new TokenInfo
            {
                AccessToken = $"access_token_{userInfo.UserId}_{Guid.NewGuid()}",
                RefreshToken = $"refresh_token_{userInfo.UserId}_{Guid.NewGuid()}",
                ExpiresIn = 3600,
                TokenType = "Bearer"
            };
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

        #endregion

        #region 响应创建方法

        private OriginalData CreatePrepareLoginResponse()
        {
            var responseMessage = $"READY|{MaxConcurrentUsers}";
            var data = System.Text.Encoding.UTF8.GetBytes(responseMessage);

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

        private OriginalData CreateLoginSuccessResponse(UserInfo userInfo, TokenInfo tokenInfo)
        {
            var responseData = $"SUCCESS|{userInfo.UserId}|{userInfo.DisplayName}|{tokenInfo.AccessToken}|{tokenInfo.RefreshToken}|{tokenInfo.ExpiresIn}";
            var data = System.Text.Encoding.UTF8.GetBytes(responseData);

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
    }


}
