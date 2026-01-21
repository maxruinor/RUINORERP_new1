using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Models.Authentication;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.UI.Network;
using RUINORERP.UI.Network.Authentication;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 用户登录服务类
    /// 提供用户认证、Token管理、登录状态控制等核心功能
    /// 使用简化的连接管理
    /// </summary>
    public sealed class UserLoginService : IDisposable
    {
        private readonly IClientCommunicationService _communicationService;
        private readonly TokenManager _tokenManager;
        private readonly TokenRefreshService _tokenRefreshService;
        private readonly CacheClientService _cacheClientService;
        private bool _isLoggedIn = false; // 登录状态标志
                                          //private readonly SemaphoreSlim _loginLock = new SemaphoreSlim(1, 1); // 登录操作信号量防止并发登录请求
                                          // 可以增加并发数，比如允许5个并发登录请求
        private readonly SemaphoreSlim _loginLock = new SemaphoreSlim(5, 5);

        private readonly ILogger<UserLoginService> _logger;
        private bool _isDisposed = false;
        private readonly ClientEventManager _eventManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UserLoginService(
            ClientEventManager eventManager,
            IClientCommunicationService communicationService,
            TokenManager tokenManager,
            TokenRefreshService tokenRefreshService,
            CacheClientService cacheClientService,
            ILogger<UserLoginService> log = null)
        {
            _eventManager = eventManager ?? throw new ArgumentNullException(nameof(eventManager));
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
            _tokenRefreshService = tokenRefreshService ?? throw new ArgumentNullException(nameof(tokenRefreshService));
            _cacheClientService = cacheClientService ?? throw new ArgumentNullException(nameof(cacheClientService));
            _logger = log;

            // 订阅连接状态变更事件
            _eventManager.ConnectionStatusChanged += OnConnectionStatusChanged;
        }

        /// <summary>
        /// 异步登录方法，使用简化的连接管理
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>包含指令信息的登录响应</returns>
        public async Task<LoginResponse> LoginAsync(string username, string password, CancellationToken ct = default)
        {
            // 验证参数
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("用户名不能为空", nameof(username));
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("密码不能为空", nameof(password));

            try
            {
                // 立即检查取消令牌状态
                ct.ThrowIfCancellationRequested();

                // 使用信号量确保同一时间只有一个登录请求
                await _loginLock.WaitAsync(TimeSpan.FromSeconds(20), ct);

                // 锁获取成功后，再次检查取消令牌状态
                if (ct.IsCancellationRequested)
                {
                    ct.ThrowIfCancellationRequested();
                }

                // 检查连接状态 - 直接依赖ConnectionManager的连接状态1111
                if (!_communicationService.IsConnected)
                {
                    _logger?.LogWarning("登录失败：未连接到服务器");
                    return ResponseFactory.CreateSpecificErrorResponse<LoginResponse>("未连接到服务器，请检查网络连接后重试");
                }

                var loginRequest = LoginRequest.Create(username, password);

                // 发送登录命令并获取响应 - 移除复杂重试逻辑，依赖ClientCommunicationService的可靠性
                var response = await _communicationService.SendCommandWithResponseAsync<LoginResponse>(
                    AuthenticationCommands.Login, loginRequest, ct, 20000);

                // 检查响应数据
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
                    //接收来自服务器的Token并保存
                    await _tokenManager.TokenStorage.SetTokenAsync(response.Token);
                    MainForm.Instance.AppContext.SessionId = response.SessionId;
                    //_logger?.LogDebug("登录成功，Token已保存 - 用户: {Username}", username);
                }

                return response;
            }
            catch (OperationCanceledException ex)
            {
                _logger?.LogDebug(ex, "登录操作已被取消（可能是超时）- 用户: {Username}", username);
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "登录过程中发生未预期的异常 - 用户: {Username}", username);
                return ResponseFactory.CreateSpecificErrorResponse<LoginResponse>("登录过程中发生错误，请稍后重试");
            }
            finally
            {
                // 确保锁总是被释放
                try
                {
                    _loginLock.Release();
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "释放登录锁时发生异常");
                }
            }
        }

        /// <summary>
        /// 用户登出方法
        /// 简化版：移除复杂逻辑，专注于本地状态清理和服务器通知
        /// </summary>
        public async Task<bool> LogoutAsync(CancellationToken ct = default)
        {
            try
            {
                bool serverLogoutSuccess = true;

                // 只有当连接有效时，才发送服务器登出请求
                if (_communicationService.IsConnected)
                {
                    try
                    {
                        var request = LoginRequest.CreateLogoutRequest();
                        var response = await _communicationService.SendCommandWithResponseAsync<LogoutResponse>(
                            AuthenticationCommands.Logout, request, ct);

                        serverLogoutSuccess = response != null && response.IsSuccess;

                        if (!serverLogoutSuccess)
                        {
                            _logger?.LogWarning("服务器登出请求失败，响应状态: {IsSuccess}, 错误: {ErrorMessage}",
                                response?.IsSuccess ?? false, response?.ErrorMessage);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogWarning(ex, "发送服务器登出请求时发生异常");
                        serverLogoutSuccess = false;
                    }
                }

                // 执行本地登出清理
                await CleanupLoginStateAsync();

                return serverLogoutSuccess;
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
        /// 尝试静默刷新Token
        /// 简化版：移除事件触发，专注于核心功能
        /// </summary>
        public async Task<bool> TrySilentRefreshAsync()
        {
            try
            {
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
                return false;
            }
        }

        /// <summary>
        /// 处理连接状态变更事件
        /// 使用简化的连接管理，连接状态由ConnectionManager负责
        /// </summary>
        private void OnConnectionStatusChanged(bool connected)
        {
            if (!connected)
            {
                // 连接断开时，更新登录状态
                _isLoggedIn = false;
                _logger?.LogDebug("连接断开，重置登录状态");
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
                //_currentUserSession = null;
                _logger?.LogDebug("登录状态已清理");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "清理登录状态失败");
            }
        }

        /// <summary>
        /// 取消登录操作
        /// </summary>
        /// <param name="sessionId">当前会话ID</param>
        /// <returns>操作是否成功</returns>
        public async Task<bool> CancelLoginAsync(string sessionId)
        {
            try
            {
                if (!_communicationService.IsConnected || string.IsNullOrEmpty(sessionId))
                {
                    _logger?.LogWarning("取消登录失败：未连接到服务器或会话ID为空");
                    return false;
                }

                var request = new LoginRequest
                {
                    Username = "",
                    Password = "",
                    AdditionalData = new System.Collections.Generic.Dictionary<string, object>
                    {
                        ["Action"] = "CancelLogin"
                    }
                };

                //var response = await _communicationService.SendCommandWithResponseAsync<ResponseBase>(AuthenticationCommands.Logout, request, CancellationToken.None);

                // 清理本地登录状态
                await CleanupLoginStateAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "取消登录操作失败");
                // 即使发送失败，也清理本地状态
                await CleanupLoginStateAsync();
                return false;
            }
        }

        /// <summary>
        /// 处理重复登录操作
        /// </summary>
        /// <param name="sessionId">当前会话ID</param>
        /// <param name="username">用户名</param>
        /// <param name="action">用户选择的操作</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>操作是否成功</returns>
        public async Task<bool> HandleDuplicateLoginAsync(DuplicateLoginResult _duplicateLoginResult, DuplicateLoginAction action, CancellationToken ct = default)
        {
            try
            {
                if (!_communicationService.IsConnected || _duplicateLoginResult == null || _duplicateLoginResult.ExistingSessions == null || _duplicateLoginResult.ExistingSessions.Count == 0)
                {
                    _logger?.LogWarning("处理重复登录失败：未连接到服务器或参数不完整");
                    return false;
                }


                var request = new LoginRequest()
                {
                    Username = "",
                    Password = "",

                    AdditionalData = new System.Collections.Generic.Dictionary<string, object>
                    {
                        ["Action"] = "你的账号在其它地方登录。当前连接即将断开。请保存数据。",
                        ["TargetUserId"] = _duplicateLoginResult.ExistingSessions[0].SessionId.ToString()
                    }
                };


                var response = await _communicationService.SendCommandWithResponseAsync<LoginResponse>(
                    AuthenticationCommands.DuplicateLogin, request, ct, 20000);
                if (response != null)
                {
                    return response.IsSuccess;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理重复登录操作失败");
                return false;
            }
        }

        /// <summary>
        /// 服务器切换时取消重连并断开连接
        /// </summary>
        /// <returns>操作是否成功</returns>
        public async Task<bool> CancelReconnectAndDisconnectForServerSwitchAsync()
        {
            try
            {
                _logger?.LogDebug("服务器切换：取消重连并断开连接");
                
                // 使用通信服务的取消重连和断开连接方法
                return await _communicationService.CancelReconnectAndForceDisconnectAsync();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "服务器切换时取消重连并断开连接失败");
                return false;
            }
        }

        /// <summary>
        /// 释放资源
        /// 简化版：移除不必要的事件取消订阅
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            try
            {
                // 取消订阅事件
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

