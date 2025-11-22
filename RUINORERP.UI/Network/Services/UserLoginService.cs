using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Errors;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Authentication;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
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
        private readonly TokenManager _tokenManager;
        private readonly SilentTokenRefresher _silentTokenRefresher;
        private readonly TokenRefreshService _tokenRefreshService; // 添加TokenRefreshService字段
        private readonly CacheClientService _cacheClientService; // 缓存客户端服务，用于订阅表
        private bool _isLoggedIn = false; // 登录状态标志
        private UserSessionInfo _currentUserSession; // 当前用户会话信息
        private readonly SemaphoreSlim _loginLock = new SemaphoreSlim(1, 1); // 登录操作信号量防止并发登录请求
        private CancellationTokenSource _cancellationTokenSource; // 取消令牌源
        private readonly ILogger<UserLoginService> _logger; // 日志记录器
        private bool _isDisposed = false; // 防止重复释放标志
                                          // 客户端事件管理器，管理连接状态和命令接收事件
        private readonly ClientEventManager _eventManager;


        /// <summary>
        /// 构造函数 - 使用依赖注入的TokenManager
        /// </summary>
        /// <param name="communicationService">通信服务</param>
        /// <param name="tokenManager">Token管理器（依赖注入）</param>
        /// <param name="logger">日志记录器</param>
        /// <exception cref="ArgumentNullException">当必要参数为空时抛出</exception>
        public UserLoginService(
            ClientEventManager eventManager,
        ClientCommunicationService communicationService,
            TokenManager tokenManager,
            TokenRefreshService tokenRefreshService,
            CacheClientService cacheClientService,
            ILogger<UserLoginService> log = null)
        {
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
            _tokenRefreshService = tokenRefreshService ?? throw new ArgumentNullException(nameof(tokenRefreshService));
            _cacheClientService = cacheClientService ?? throw new ArgumentNullException(nameof(cacheClientService));
            _logger = log;
            _eventManager = eventManager;
            // 初始化SilentTokenRefresher
            _silentTokenRefresher = new SilentTokenRefresher(_tokenRefreshService);

            // 订阅刷新事件
            _silentTokenRefresher.OnRefreshSucceeded += OnRefreshSucceeded;
            _silentTokenRefresher.OnRefreshFailed += OnTokenRefreshFailed;

            // 通过事件管理器订阅连接状态变更事件
            _eventManager.ConnectionStatusChanged += OnConnectionStatusChanged;

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
        /// <summary>
        /// 异步登录方法，使用取消令牌支持超时取消
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="ct">取消令牌，用于支持超时取消</param>
        /// <returns>登录响应结果</returns>
        public async Task<LoginResponse> LoginAsync(string username, string password, CancellationToken ct = default)
        {
            // 验证参数
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("用户名不能为空", nameof(username));
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("密码不能为空", nameof(password));

            try
            {
                // 立即检查取消令牌状态，如果已取消，避免尝试获取锁
                ct.ThrowIfCancellationRequested();

                // 使用信号量确保同一时间只有一个登录请求
                await _loginLock.WaitAsync(ct);

                // 锁获取成功后，再次检查取消令牌状态
                if (ct.IsCancellationRequested)
                {
                    ct.ThrowIfCancellationRequested();
                }

                // 检查连接状态
                if (!_communicationService.IsConnected)
                {
                    _logger?.LogWarning("登录失败：未连接到服务器");
                    return ResponseFactory.CreateSpecificErrorResponse<LoginResponse>("未连接到服务器，请检查网络连接后重试");
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
                            AuthenticationCommands.Login, loginRequest, ct, 1000);
                        break; // 成功则跳出重试循环
                    }
                    catch (Exception ex) when (IsRetryableException(ex) && retryCount < maxRetries)
                    {
                        retryCount++;
                        _logger?.LogWarning(ex, "登录请求失败，正在重试 ({RetryCount}/{MaxRetries}) - 用户: {Username}",
                            retryCount, maxRetries, username);
                        // 指数退避策略
                        await Task.Delay(100 * (int)Math.Pow(2, retryCount), ct);
                    }
                }

                // 检查响应数据是否为空
                if (response == null)
                {
                    _logger?.LogError("登录失败：服务器返回了空的响应数据");
                    return ResponseFactory.CreateSpecificErrorResponse<LoginResponse>("服务器返回了空的响应数据，请联系系统管理员");
                }

                // 检查响应是否成功
                if (!response.IsSuccess)
                {
                    return response;
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
                        _logger?.LogWarning(heartbeatEx, "登录成功后启动心跳失败 - 用户: {Username}", username);
                        // 心跳启动失败不影响登录流程，只记录警告
                    }

                    // 登录成功后订阅基础业务表缓存
                    try
                    {
                        // 使用Task.Run避免阻塞登录流程
                        _ = Task.Run(async () =>
                        {
                            try
                            {
                                await _cacheClientService.SubscribeAllBaseTablesAsync();
                            }
                            catch (Exception cacheEx)
                            {
                                _logger?.LogError(cacheEx, "登录成功后订阅基础业务表缓存失败 - 用户: {Username}", username);
                                // 缓存订阅失败不影响登录流程
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "启动缓存订阅任务失败 - 用户: {Username}", username);
                    }
                }

                return response;
            }
            catch (OperationCanceledException ex)
            {
                // 记录取消异常
                _logger?.LogDebug(ex, "登录操作已被取消（可能是超时）- 用户: {Username}", username);
                throw; // 重新抛出异常，让调用者处理
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "登录过程中发生未预期的异常 - 用户: {Username}", username);
                return ResponseFactory.CreateSpecificErrorResponse<LoginResponse>("登录过程中发生错误，请稍后重试");
            }
            finally
            {
                // 确保锁总是被释放，不管是否发生异常
                try
                {
                    _loginLock.Release();
                }
                catch (Exception ex)
                {
                    // 记录锁释放错误，但不要让它影响主流程
                    _logger?.LogError(ex, "释放登录锁时发生异常");
                }
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
                    _logger?.LogWarning("登出失败：未找到有效的令牌信息，可能已处于登出状态");
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
                            _logger?.LogWarning("服务器登出请求失败，响应状态: {IsSuccess}, 错误: {ErrorMessage}",
                                response?.IsSuccess ?? false, response?.ErrorMessage);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogWarning(ex, "发送服务器登出请求时发生异常，但将继续本地登出流程");
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
                _logger?.LogError(ex, "登出过程中发生异常");
                // 即使发生异常，也尝试清理基本的登录状态
                try
                {
                    await CleanupLoginStateAsync();
                }
                catch (Exception cleanupEx)
                {
                    _logger?.LogError(cleanupEx, "清理登录状态时也发生异常");
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
        /// 尝试静默刷新Token - 使用新的无参数刷新方法
        /// </summary>
        /// <returns>是否刷新成功</returns>
        public async Task<bool> TrySilentRefreshAsync()
        {
            try
            {
                // 使用TokenRefreshService进行刷新
                var tokenRefreshResponse = await _tokenRefreshService.RefreshTokenAsync(CancellationToken.None);
                if (tokenRefreshResponse != null && !string.IsNullOrEmpty(tokenRefreshResponse.AccessToken))
                {
                    _logger?.LogDebug("Token刷新成功");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Token刷新失败");
                // 触发刷新失败事件
                OnTokenRefreshFailed(this, ex);
                return false;
            }
        }

        /// <summary>
        /// 处理连接状态变更事件
        /// </summary>
        /// <param name="connected">连接状态</param>
        private void OnConnectionStatusChanged(bool connected)
        {
            if (connected)
            {
                // 使用Task.Run避免阻塞事件处理线程
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await TryRestoreAuthenticationAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "在连接恢复后尝试恢复认证时发生异常");
                    }
                });
            }
            else
            {
                // 连接断开时，可以考虑清理一些状态
                _isLoggedIn = false;
            }
        }

        /// <summary>
        /// 处理接收到的命令
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <param name="data">数据对象</param>
        private void OnCommandReceived(PacketModel packet, object data)
        {
            // 根据需要处理特定的命令
            // 目前主要用于接收连接状态相关的通知
        }

        /// <summary>
        /// 当连接恢复后尝试恢复认证状态
        /// </summary>
        /// <returns>是否恢复成功</returns>
        public async Task<bool> TryRestoreAuthenticationAsync()
        {
            try
            {
                // 检查通信服务是否已连接
                if (!_communicationService.IsConnected)
                {
                    _logger?.LogWarning("尝试恢复认证失败：通信服务未连接");
                    return false;
                }

                // 获取当前存储的Token
                var storedToken = await _tokenManager.TokenStorage.GetTokenAsync();
                if (storedToken == null || string.IsNullOrEmpty(storedToken.AccessToken))
                {
                    _logger?.LogDebug("没有找到存储的Token，无法恢复认证");
                    return false;
                }

                // 验证Token是否有效
                bool isTokenValid = await ValidateTokenAsync(storedToken.AccessToken);
                if (!isTokenValid)
                {
                    // Token无效，尝试刷新
                    _logger?.LogDebug("存储的Token无效，尝试刷新");
                    bool refreshed = await TrySilentRefreshAsync();
                    if (!refreshed)
                    {
                        _logger?.LogWarning("Token刷新失败，无法恢复认证");
                        return false;
                    }
                    // 刷新成功后，获取新的Token
                    storedToken = await _tokenManager.TokenStorage.GetTokenAsync();
                    if (storedToken == null || string.IsNullOrEmpty(storedToken.AccessToken))
                    {
                        _logger?.LogWarning("刷新后未获取到有效Token");
                        return false;
                    }
                }

                // Token有效，向服务器发送认证恢复请求
                _logger?.LogDebug("Token有效，向服务器恢复认证状态");

                // 使用ValidateToken命令验证用户身份，作为认证恢复的方式
                var validateRequest = new TokenValidationRequest
                {
                    Token = storedToken
                };

                // 使用SendCommandWithResponseAsync获取更可靠的响应处理
                var response = await _communicationService.SendCommandWithResponseAsync<TokenValidationResponse>(
                    AuthenticationCommands.ValidateToken, validateRequest, CancellationToken.None);

                // 确保响应不为null并且成功
                if (response != null && response.IsSuccess)
                {
                    _logger?.LogDebug("通过Token验证成功恢复认证状态");
                    // 更新登录状态
                    _isLoggedIn = true;
                    // SilentTokenRefresher没有Start方法，这里不再需要额外操作
                    // 令牌刷新会在需要时自动触发
                    return true;
                }
                else
                {
                    _logger?.LogWarning("向服务器恢复认证失败: {Message}",
                        response?.ErrorMessage ?? "Token验证被拒绝");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "恢复认证过程中发生异常");
                return false;
            }
        }

        /// <summary>
        /// 验证Token是否有效
        /// </summary>
        /// <param name="token">要验证的Token</param>
        /// <returns>Token是否有效</returns>
        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                return await _tokenRefreshService.ValidateTokenAsync(token, CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Token验证失败");
                return false;
            }
        }

        /// <summary>
        /// 清理登录状态
        /// </summary>
        public async Task CleanupLoginStateAsync()
        {
            try
            {
                // 清除Token存储
                await _tokenManager.TokenStorage.ClearTokenAsync();
                // 重置登录状态
                _isLoggedIn = false;
                _currentUserSession = null;
                _logger?.LogDebug("登录状态已清理");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "清理登录状态失败");
            }
        }

        // 移除重复的事件处理方法，保留与SilentTokenRefresher事件匹配的方法

        /// <summary>
        /// 当Token刷新成功时调用 - 适配SilentTokenRefresher的事件参数
        /// </summary>
        private void OnRefreshSucceeded(object sender, TokenInfo e)
        {
            _logger?.LogDebug("Token refresh succeeded");
            // 更新登录状态
            _isLoggedIn = true;
        }

        /// <summary>
        /// 当Token刷新失败时调用 - 适配SilentTokenRefresher的事件参数
        /// </summary>
        private void OnTokenRefreshFailed(object sender, Exception ex)
        {
            _logger?.LogError(ex, "Token refresh failed during background refresh");
            // 不需要自动登出，让用户决定下一步操作
        }



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
                if (_silentTokenRefresher != null)
                {
                    _silentTokenRefresher.OnRefreshSucceeded -= OnRefreshSucceeded;
                    _silentTokenRefresher.OnRefreshFailed -= OnTokenRefreshFailed;
                }

                // 移除对不存在的CommandReceived事件的订阅
                // if (_communicationService != null)
                // {
                //     _communicationService.CommandReceived -= OnCommandReceived;
                // }
                if (_eventManager != null)
                {
                    _eventManager.ConnectionStatusChanged -= OnConnectionStatusChanged;
                }
                _loginLock.Dispose();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "释放UserLoginService资源时发生异常");
            }
        }
    }
}