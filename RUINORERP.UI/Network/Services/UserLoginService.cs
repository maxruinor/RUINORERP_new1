using MessagePack;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Errors;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Models.Responses.Authentication;
using RUINORERP.UI.Network;
using RUINORERP.UI.Network.Authentication;
using RUINORERP.UI.Network.Exceptions;
using SourceLibrary.Security;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 用户登录服务类
    /// 提供用户认证、Token管理、登录状态控制等核心功能
    /// 优化版：增强连接状态管理、异常处理和Token生命周期控制
    /// </summary>
    public sealed class UserLoginService : IDisposable
    {
        private readonly ClientCommunicationService _communicationService;
        private readonly ILogger<UserLoginService> _log;
        private readonly SilentTokenRefresher _silentTokenRefresher;
        private readonly TokenManager _tokenManager;
        private readonly SemaphoreSlim _loginLock = new SemaphoreSlim(1, 1); // 防止并发登录请求
        private bool _isDisposed = false;

        /// <summary>
        /// 构造函数 - 使用依赖注入的TokenManager
        /// </summary>
        /// <param name="communicationService">通信服务</param>
        /// <param name="tokenManager">Token管理器（依赖注入）</param>
        /// <param name="logger">日志记录器</param>
        /// <exception cref="ArgumentNullException">当必要参数为空时抛出</exception>
        public UserLoginService(
            ClientCommunicationService communicationService,
            TokenManager tokenManager,
            ILogger<UserLoginService> logger = null)
        {
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
            _log = logger;

            _silentTokenRefresher = new SilentTokenRefresher(new TokenRefreshService(communicationService, _tokenManager));

            // 订阅静默刷新事件
            _silentTokenRefresher.RefreshSucceeded += OnRefreshSucceeded;
            _silentTokenRefresher.RefreshFailed += OnTokenRefreshFailed;
        }

        /// <summary>
        /// 用户登录 - 增强版实现
        /// 返回包含指令信息的响应数据，并提供完善的连接状态管理和错误处理
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>包含指令信息的登录响应</returns>
        /// <exception cref="NetworkException">网络连接相关异常</exception>
        /// <exception cref="AuthenticationException">认证相关异常</exception>
        public async Task<LoginResponse> LoginAsync(string username, string password, CancellationToken ct = default)
        {
            // 验证参数
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("用户名不能为空", nameof(username));
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("密码不能为空", nameof(password));

            // 使用信号量确保同一时间只有一个登录请求
            await _loginLock.WaitAsync(ct);
            try
            {
                // 检查连接状态
                if (!_communicationService.IsConnected)
                {
                    _log?.LogWarning("登录失败：未连接到服务器");
                    return ResponseBase.CreateError("未连接到服务器，请检查网络连接后重试") as LoginResponse;
                }

                var loginRequest = LoginRequest.Create(username, password);

                // 使用重试机制发送登录请求
                LoginResponse response = null;
                int maxRetries = 2;
                int retryCount = 0;

                while (retryCount <= maxRetries)
                {
                    try
                    {
                        // 发送登录命令并获取响应
                        response = await _communicationService.SendCommandWithResponseAsync<LoginResponse>(
                            AuthenticationCommands.Login, loginRequest, ct);
                        break; // 成功则跳出重试循环
                    }
                    catch (Exception ex) when (IsRetryableException(ex) && retryCount < maxRetries)
                    {
                        retryCount++;
                        _log?.LogWarning(ex, "登录请求失败，正在重试 ({RetryCount}/{MaxRetries}) - 用户: {Username}", 
                            retryCount, maxRetries, username);
                        // 指数退避策略
                        await Task.Delay(100 * (int)Math.Pow(2, retryCount), ct);
                    }
                }

                // 检查响应数据是否为空
                if (response == null)
                {
                    _log?.LogError("登录失败：服务器返回了空的响应数据");
                    return ResponseBase.CreateError("服务器返回了空的响应数据，请联系系统管理员") as LoginResponse;
                }

                // 检查响应是否成功
                if (!response.IsSuccess)
                {
                    // 检查特定错误类型并提供更详细的错误信息
                    string errorMessage = response.ErrorMessage;
                    string detailedMessage = "登录失败";
                    var errorCode = UnifiedErrorCodes.Command_ValidationFailed;

                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        if (errorMessage.IndexOf("TIME_MISMATCH", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            detailedMessage = "客户端时间与服务器时间差异过大，请校准系统时间后重试";
                        }
                        else if (errorMessage.IndexOf("invalid", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            detailedMessage = "用户名或密码错误，请重新输入";
                        }
                        else if (errorMessage.IndexOf("lock", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            detailedMessage = "账户已被锁定，请联系系统管理员";
                        }
                        else if (errorMessage.IndexOf("expire", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            detailedMessage = "账户已过期，请联系系统管理员";
                        }
                        else
                        {
                            detailedMessage = $"登录失败: {errorMessage}";
                        }
                    }

                    _log?.LogWarning("登录失败 - 用户: {Username}, 错误: {ErrorMessage}", username, errorMessage);
                    return ResponseBase.CreateError(detailedMessage, errorCode)
                        .WithMetadata("ErrorCode", errorMessage) as LoginResponse;
                }

                // 登录成功后处理Token
                if (response.Token != null && !string.IsNullOrEmpty(response.Token.AccessToken))
                {
                    await _tokenManager.TokenStorage.SetTokenAsync(response.Token);
                    MainForm.Instance.AppContext.SessionId = response.SessionId;

                    // 登录成功后启动心跳
                    try
                    {
                        await _communicationService.StartHeartbeatAfterLoginAsync(ct);
                    }
                    catch (Exception heartbeatEx)
                    {
                        _log?.LogWarning(heartbeatEx, "登录成功后启动心跳失败 - 用户: {Username}", username);
                        // 心跳启动失败不影响登录流程，只记录警告
                    }
                }
                else
                {
                    _log?.LogWarning("登录响应中未包含有效的Token信息 - 用户: {Username}, 请求ID: {RequestId}",
                        username, loginRequest.RequestId);
                    return ResponseBase.CreateError("登录响应中未包含有效的Token信息") as LoginResponse;
                }

                return response;
            }
            catch (OperationCanceledException ex)
            {
                _log?.LogInformation(ex, "登录操作已被用户取消 - 用户: {Username}", username);
                return ResponseBase.CreateError("登录操作已取消") as LoginResponse;
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "登录过程中发生未预期的异常 - 用户: {Username}", username);
                return ResponseBase.CreateError("登录过程中发生错误，请稍后重试") as LoginResponse;
            }
            finally
            {
                _loginLock.Release();
            }
        }

        /// <summary>
        /// 用户登出 - 增强版实现
        /// 完善响应检查，确保令牌正确清除，并提供优雅的资源释放
        /// </summary>
        /// <param name="ct">取消令牌</param>
        /// <returns>登出结果</returns>
        public async Task<bool> LogoutAsync(CancellationToken ct = default)
        {
            try
            {
                // 获取当前令牌信息
                var tokenInfo = await _tokenManager.TokenStorage.GetTokenAsync();
                if (tokenInfo == null)
                {
                    _log?.LogWarning("登出失败：未找到有效的令牌信息，可能已处于登出状态");
                    // 即使没有令牌，我们也应该清除可能存在的会话信息
                    await CleanupLoginStateAsync();
                    return true; // 认为登出成功，因为没有什么需要登出的
                }

                bool serverLogoutSuccess = true;
                
                // 只有当连接有效时，才发送服务器登出请求
                if (_communicationService.IsConnected)
                {
                    try
                    {
                        // 创建登出请求
                        var request = LoginRequest.CreateLogoutRequest();
                        
                        // 使用SendCommandWithResponseAsync获取更详细的响应信息
                        var response = await _communicationService.SendCommandWithResponseAsync<LogoutResponse>(
                            AuthenticationCommands.Logout, request, ct);
                        
                        // 完善响应检查
                        serverLogoutSuccess = response != null && response.IsSuccess;
                        
                        if (!serverLogoutSuccess)
                        {
                            _log?.LogWarning("服务器登出请求失败，响应状态: {IsSuccess}, 错误: {ErrorMessage}", 
                                response?.IsSuccess ?? false, response?.ErrorMessage);
                        }
                    }
                    catch (Exception ex)
                    {
                        _log?.LogWarning(ex, "发送服务器登出请求时发生异常，但将继续本地登出流程");
                        // 即使发送登出请求失败，我们仍然执行本地登出清理
                        serverLogoutSuccess = false;
                    }
                }
                else
                {
                    //未连接到服务器，仅执行本地登出操作;
                }

                // 执行本地登出清理
                await CleanupLoginStateAsync();
                
                return serverLogoutSuccess; // 返回服务器登出是否成功
            }
            catch (OperationCanceledException)
            {
                return false;
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "登出过程中发生异常");
                // 即使发生异常，也尝试清理基本的登录状态
                try
                {
                    await CleanupLoginStateAsync();
                }
                catch (Exception cleanupEx)
                {
                    _log?.LogError(cleanupEx, "清理登录状态时也发生异常");
                }
                throw new Exception($"登出失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取当前访问令牌 - 使用简化版TokenManager
        /// </summary>
        /// <returns>访问令牌，如果不存在或已过期则返回null</returns>
        public async Task<string> GetCurrentAccessToken()
        {
            var tokenInfo = await _tokenManager.TokenStorage.GetTokenAsync();
            // 检查Token是否有效（未过期）
            if (tokenInfo != null && tokenInfo.ExpiresAt > DateTime.UtcNow.AddMinutes(1)) // 提前1分钟视为过期
            {
                return tokenInfo.AccessToken;
            }
            return null;
        }

        /// <summary>
        /// 手动触发Token刷新（通常由静默刷新器调用）
        /// </summary>
        /// <returns>刷新是否成功</returns>
        public async Task<bool> TrySilentRefreshAsync()
        {
            try
            {
                return await _silentTokenRefresher.TriggerRefreshAsync();
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "手动触发Token刷新失败");
                return false;
            }
        }

        /// <summary>
        /// 验证Token有效性
        /// </summary>
        /// <returns>Token是否有效</returns>
        public async Task<bool> ValidateTokenAsync()
        {
            try
            {
                var currentToken = await _tokenManager.TokenStorage.GetTokenAsync();
                if (currentToken == null)
                {
                    _log?.LogDebug("Token验证失败：未找到Token信息");
                    return false;
                }

                // 先进行本地验证
                if (currentToken.ExpiresAt <= DateTime.UtcNow)
                {
                    _log?.LogDebug("Token验证失败：Token已过期");
                    return false;
                }

                // 如果连接有效，发送到服务器验证
                if (_communicationService.IsConnected)
                {
                    var validateRequest = SimpleRequest.CreateString(currentToken.AccessToken);
                    var response = await _communicationService.SendCommandWithResponseAsync<TokenValidationResponse>(
                        AuthenticationCommands.ValidateToken, validateRequest, CancellationToken.None);
                    
                    bool isValid = response != null && response.IsSuccess;
                    _log?.LogDebug("服务器Token验证结果: {IsValid}", isValid);
                    return isValid;
                }
                else
                {
                    _log?.LogDebug("未连接到服务器，无法进行服务器Token验证");
                    // 未连接到服务器时，仅依赖本地验证
                    return true;
                }
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "Token验证过程中发生异常");
                return false;
            }
        }

        /// <summary>
        /// 清理登录状态
        /// </summary>
        /// <returns>异步任务</returns>
        private async Task CleanupLoginStateAsync()
        {
            try
            {
                // 清除令牌信息
                await _tokenManager.TokenStorage.ClearTokenAsync();
                
                // 清除会话信息
                if (MainForm.Instance != null && MainForm.Instance.AppContext != null)
                {
                    MainForm.Instance.AppContext.SessionId = null;
                }
                
                // 释放静默刷新器资源
                _silentTokenRefresher.Dispose();
                
                _log?.LogDebug("登录状态已清理完毕");
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "清理登录状态时发生异常");
                throw;
            }
        }

        /// <summary>
        /// 判断异常是否可以重试
        /// </summary>
        /// <param name="ex">异常</param>
        /// <returns>是否可以重试</returns>
        private bool IsRetryableException(Exception ex)
        {
            return ex is TimeoutException ||
                   ex is System.Net.Sockets.SocketException ||
                   ex is System.IO.IOException ||
                   (ex is AggregateException && ex.InnerException != null && IsRetryableException(ex.InnerException)) ||
                   (ex.Message != null && (
                       ex.Message.IndexOf("timeout", StringComparison.OrdinalIgnoreCase) >= 0 ||
                       ex.Message.IndexOf("connection", StringComparison.OrdinalIgnoreCase) >= 0 ||
                       ex.Message.IndexOf("网络", StringComparison.OrdinalIgnoreCase) >= 0));
        }

        /// <summary>
        /// 处理Token刷新成功事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void OnRefreshSucceeded(object sender, SilentTokenRefresher.RefreshSucceededEventArgs e)
        {
            _log?.LogDebug("静默刷新Token成功");
        }

        /// <summary>
        /// 处理Token刷新失败事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void OnTokenRefreshFailed(object sender, SilentTokenRefresher.RefreshFailedEventArgs e)
        {
            _log?.LogWarning("静默刷新Token失败: {Message}", e.Exception.Message);
            
            // 如果是Token过期或无效的错误，尝试清理登录状态
            if (e.Exception.Message.IndexOf("expired", StringComparison.OrdinalIgnoreCase) >= 0 ||
                e.Exception.Message.IndexOf("invalid", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                // 注意：这里不能直接调用LogoutAsync，因为这可能在非UI线程中执行
                // 应该通过事件通知UI线程处理登出
                // 实际实现中需要添加相应的事件或通知机制
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;
            
            try
            {
                // 取消订阅事件
                _silentTokenRefresher.RefreshFailed -= OnTokenRefreshFailed;
                _silentTokenRefresher.RefreshSucceeded -= OnRefreshSucceeded;
                
                // 释放静默刷新器资源
                _silentTokenRefresher.Dispose();
                
                // 释放资源
                _silentTokenRefresher.Dispose();
                _loginLock.Dispose();
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "释放UserLoginService资源时发生异常");
            }
        }
    }
}