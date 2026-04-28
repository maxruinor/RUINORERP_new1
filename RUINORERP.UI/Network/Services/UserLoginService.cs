using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Models.Authentication;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.UI.Network;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        private readonly CacheClientService _cacheClientService;
        private bool _isLoggedIn = false; // 登录状态标志
        private readonly SemaphoreSlim _loginLock = new SemaphoreSlim(1, 1);

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
            CacheClientService cacheClientService,
            ILogger<UserLoginService> log = null)
        {
            _eventManager = eventManager ?? throw new ArgumentNullException(nameof(eventManager));
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
            _cacheClientService = cacheClientService ?? throw new ArgumentNullException(nameof(cacheClientService));
            _logger = log;

            // 订阅连接状态变更事件
            _eventManager.ConnectionStatusChanged += OnConnectionStatusChanged;
        }

        /// <summary>
        /// 异步登录方法，使用简化的连接管理
        /// 增强版：添加详细的日志记录和状态管理，确保登录流程健壮性
        /// ✅ 修复：优化超时重试逻辑，避免立即失败
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="ct">取消令牌</param>
        /// <param name="isAutoLogin">是否为自动登录（自动登录时不显示弹窗）</param>
        /// <returns>包含指令信息的登录响应</returns>
        public async Task<LoginResponse> LoginAsync(string username, string password, CancellationToken ct = default, bool isAutoLogin = false)
        {
            // 验证参数
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("用户名不能为空", nameof(username));
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("密码不能为空", nameof(password));

            _logger?.LogInformation("========== 开始登录流程 ==========");
            _logger?.LogInformation("[登录] 用户名: {Username}, 自动登录: {IsAutoLogin}, 时间: {Time}", 
                username, isAutoLogin, DateTime.Now);

            try
            {
                // 立即检查取消令牌状态
                if (ct.IsCancellationRequested)
                {
                    _logger?.LogDebug("[登录] 登录前已被取消 - Username: {Username}", username);
                    ct.ThrowIfCancellationRequested();
                }

                // ✅ 修复：增加锁等待超时前的连接状态检查
                _logger?.LogDebug("[登录] 尝试获取登录锁...");
                var lockAcquired = await _loginLock.WaitAsync(TimeSpan.FromSeconds(20), ct); // 从10秒增加到20秒

                if (!lockAcquired)
                {
                    _logger?.LogWarning("[登录] 获取锁超时，可能已有登录在进行 - Username: {Username}", username);
                    
                    // ✅ 修复：检查是否是因为前一次登录超时导致的锁占用，如果是则等待一小段时间后重试
                    _logger?.LogInformation("[登录] 等待500ms后重新尝试获取锁...");
                    await Task.Delay(500, ct);
                    
                    // 再次尝试获取锁
                    lockAcquired = await _loginLock.WaitAsync(TimeSpan.FromSeconds(10), ct);
                    if (!lockAcquired)
                    {
                        _logger?.LogError("[登录] 第二次获取锁仍然超时，返回错误 - Username: {Username}", username);
                        return ResponseFactory.CreateSpecificErrorResponse<LoginResponse>("正在登录中，请稍候...");
                    }
                    
                    _logger?.LogDebug("[登录] 第二次成功获取登录锁");
                }
                else
                {
                    _logger?.LogDebug("[登录] 成功获取登录锁");
                }

                // 锁获取成功后，再次检查取消令牌状态
                if (ct.IsCancellationRequested)
                {
                    _logger?.LogDebug("[登录] 获取锁后被取消 - Username: {Username}", username);
                    ct.ThrowIfCancellationRequested();
                }

                // ✅ 修复：检查并重置连接状态，确保使用最新状态
                if (!_communicationService.ConnectionManager.IsConnected)
                {
                    _logger?.LogWarning("[登录] 未连接到服务器，尝试重新连接 - Username: {Username}", username);
                    
                    // ✅ 优化：外网环境下增加重连等待时间，确保网络稳定
                    try
                    {
                        var reconnectResult = await _communicationService.ConnectionManager.ManualReconnectAsync();
                        if (!reconnectResult)
                        {
                            _logger?.LogError("[登录] 重新连接失败 - Username: {Username}", username);
                            return ResponseFactory.CreateSpecificErrorResponse<LoginResponse>("未连接到服务器，请检查网络连接后重试");
                        }
                        
                        // ✅ 关键修复：重连成功后等待3秒，确保Socket完全建立连接（外网环境）
                        _logger?.LogInformation("[登录] 重新连接成功，等待3秒确保连接稳定...");
                        await Task.Delay(3000, ct);
                        
                        // 再次验证连接状态
                        if (!_communicationService.ConnectionManager.IsConnected)
                        {
                            _logger?.LogError("[登录] 重连后连接仍未建立 - Username: {Username}", username);
                            return ResponseFactory.CreateSpecificErrorResponse<LoginResponse>("连接不稳定，请稍后重试");
                        }
                        
                        _logger?.LogInformation("[登录] 连接已稳定，继续登录流程");
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "[登录] 重新连接异常 - Username: {Username}", username);
                        return ResponseFactory.CreateSpecificErrorResponse<LoginResponse>("连接服务器失败，请稍后重试");
                    }
                }

                _logger?.LogDebug("[登录] 连接状态正常，准备发送登录请求");

                var loginRequest = LoginRequest.Create(username, password);
                // 附加session ID到登录请求
                if (MainForm.Instance != null && MainForm.Instance.AppContext != null)
                {
                    loginRequest.SessionId = MainForm.Instance.AppContext.SessionId;
                }
                _logger?.LogDebug("[登录请求] 用户名: {Username}, SessionId: {SessionId}, RequestId: {RequestId}", 
                    username, loginRequest.SessionId, loginRequest.RequestId);

                // 发送登录命令并获取响应
                // ✅ 修复：针对网络卡顿情况，优化超时和重试策略
                _logger?.LogDebug("[登录] 正在发送登录命令到服务器，超时时间: 15秒...");
                
                const int maxLoginRetries = 2; // 最多重试2次（总共3次尝试）
                LoginResponse response = null;
                Exception lastException = null;
                
                for (int retryCount = 0; retryCount <= maxLoginRetries; retryCount++)
                {
                    try
                    {
                        // ✅ 修复：每次重试前检查连接状态
                        if (retryCount > 0 && !_communicationService.ConnectionManager.IsConnected)
                        {
                            _logger?.LogWarning("[登录重试] 第{RetryCount}次重试前检测到连接断开，尝试重连...", retryCount + 1);
                            try
                            {
                                await _communicationService.ConnectionManager.ManualReconnectAsync();
                                
                                // ✅ 关键修复：重连后等待3秒（比首次多1秒），确保连接稳定
                                _logger?.LogInformation("[登录重试] 重连成功，等待3秒确保连接稳定...");
                                await Task.Delay(3000, ct);
                                
                                // 验证连接状态
                                if (!_communicationService.ConnectionManager.IsConnected)
                                {
                                    _logger?.LogError("[登录重试] 重连后连接仍未建立");
                                    lastException = new Exception("重连后连接未建立");
                                    continue;
                                }
                                
                                _logger?.LogInformation("[登录重试] 连接已稳定，继续重试");
                            }
                            catch (OperationCanceledException cancelEx) when (ct.IsCancellationRequested)
                            {
                                // ✅ 修复：如果是用户取消，直接抛出
                                _logger?.LogWarning(cancelEx, "[登录重试] 操作被用户取消");
                                throw;
                            }
                            catch (Exception reconnectEx)
                            {
                                _logger?.LogError(reconnectEx, "[登录重试] 重连失败");
                                lastException = new Exception($"第{retryCount + 1}次重试时连接失败: {reconnectEx.Message}");
                                continue;
                            }
                        }
                        else if (retryCount > 0)
                        {
                            // ✅ 优化：外网环境下重试前增加随机抖动等待，避免与服务端重连风暴冲突
                            var jitterDelay = new Random().Next(1500, 3000);
                            _logger?.LogDebug("[登录重试] 第{RetryCount}次重试，等待 {DelayMs} 毫秒后发送请求...", retryCount + 1, jitterDelay);
                            await Task.Delay(jitterDelay, ct);
                        }
                        
                        int timeoutMs = 15000; // 15秒超时
                        response = await _communicationService.SendCommandWithResponseAsync<LoginResponse>(
                            AuthenticationCommands.Login, loginRequest, ct, timeoutMs);
                        
                        if (response != null)
                        {
                            _logger?.LogInformation("[登录] 第{Attempt}次尝试收到响应 - IsSuccess: {IsSuccess}", 
                                retryCount + 1, response.IsSuccess);
                            break; // 成功收到响应，退出重试循环
                        }
                        
                        if (retryCount < maxLoginRetries)
                        {
                            _logger?.LogWarning("[登录重试] 第 {RetryCount} 次尝试返回空响应，{DelayMs} 毫秒后重试...", 
                                retryCount + 1, 2000);
                            await Task.Delay(2000, ct); // 等待2秒后重试
                        }
                    }
                    catch (TimeoutException ex) when (retryCount < maxLoginRetries)
                    {
                        lastException = ex;
                        _logger?.LogWarning(ex, "[登录超时] 第 {RetryCount} 次尝试超时，{DelayMs} 毫秒后重试...", 
                            retryCount + 1, 3000);  // ✅ 修复：增加重试间隔至3秒
                        await Task.Delay(3000, ct); // ✅ 修复：从2秒增加到3秒
                    }
                    catch (OperationCanceledException ex) when (ct.IsCancellationRequested)
                    {
                        // ✅ 修复：用户主动取消，直接抛出
                        _logger?.LogWarning(ex, "[登录取消] 用户取消了登录操作 - Username: {Username}", username);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        lastException = ex;
                        
                        // ✅ 修复：区分TaskCanceledException（超时导致）和其他异常
                        if (ex is System.Threading.Tasks.TaskCanceledException && retryCount < maxLoginRetries)
                        {
                            _logger?.LogWarning(ex, "[登录重试] 第 {RetryCount} 次尝试因超时被取消，{DelayMs} 毫秒后重试...", 
                                retryCount + 1, 3000);
                            await Task.Delay(3000, ct); // ✅ 修复：从2秒增加到3秒
                        }
                        else if (retryCount == maxLoginRetries)
                        {
                            throw; // 最后一次尝试失败，抛出异常
                        }
                        else
                        {
                            _logger?.LogWarning(ex, "[登录重试] 第 {RetryCount} 次尝试失败，{DelayMs} 毫秒后重试...", 
                                retryCount + 1, 3000);
                            await Task.Delay(3000, ct); // ✅ 修复：从2秒增加到3秒
                        }
                    }
                }
                
                // ✅ 修复：如果所有重试都失败但有异常记录，返回详细错误信息
                if (response == null && lastException != null)
                {
                    _logger?.LogError(lastException, "[登录最终失败] 所有重试均失败 - Username: {Username}", username);
                    return ResponseFactory.CreateSpecificErrorResponse<LoginResponse>(
                        $"登录失败: {lastException.Message}");
                }

                _logger?.LogDebug("[登录] 收到服务器响应 - IsNull: {IsNull}", response == null);

                // 检查响应数据
                if (response == null)
                {
                    _logger?.LogDebug("[登录] 服务器返回了空的响应数据 - Username: {Username}, RequestId: {RequestId}", 
                        username, loginRequest.RequestId);
                    return ResponseFactory.CreateSpecificErrorResponse<LoginResponse>("服务器返回了空的响应数据，请联系系统管理员");
                }

                _logger?.LogDebug("[登录] 响应数据 - IsSuccess: {IsSuccess}, ErrorMessage: {ErrorMessage}", 
                    response.IsSuccess, response.ErrorMessage);

                // 检查响应是否成功
                if (!response.IsSuccess)
                {
                    _logger?.LogDebug("[登录失败] 服务器返回失败 - Username: {Username}, Error: {ErrorMessage}", 
                        username, response.ErrorMessage ?? "未知错误");
                    return response;
                }

                _logger?.LogInformation("[登录成功] 服务器验证通过 - Username: {Username}, SessionId: {SessionId}", 
                    username, response.SessionId);

                // 登录成功后处理Token
                if (response.Token != null && !string.IsNullOrEmpty(response.Token.AccessToken))
                {
                    _logger?.LogDebug("[登录] 正在保存Token...");
                    //接收来自服务器的Token并保存
                    await _tokenManager.TokenStorage.SetTokenAsync(response.Token);
                    _logger?.LogDebug("[登录] Token保存成功");

                    // 使用服务器返回的SessionId更新客户端上下文
                    if (MainForm.Instance?.AppContext != null && !string.IsNullOrEmpty(response.SessionId))
                    {
                        MainForm.Instance.AppContext.SessionId = response.SessionId;
                        _logger?.LogInformation("[登录成功] 更新SessionId: {SessionId}", response.SessionId);
                    }

                    // ✅ 如果不是自动登录，保存凭据供下次自动登录使用
                    if (!isAutoLogin)
                    {
                        try
                        {
                            // 获取通信服务实例并保存凭据
                            var commService = RUINORERP.UI.Startup.GetFromFac<ClientCommunicationService>();
                            commService?.SetAutoReloginCredentials(username, password);
                            _logger?.LogDebug("[登录] 已保存自动登录凭据");
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogWarning(ex, "[登录] 保存自动登录凭据失败");
                        }
                    }
                }
                else
                {
                    _logger?.LogWarning($"[登录警告] 登录成功但未收到Token: Username={username}, SessionId={response.SessionId}");
                }

                // 处理注册状态和到期提醒
                _logger?.LogDebug("[登录] 正在处理注册状态...");
                await HandleRegistrationStatusAsync(response, isAutoLogin);

                // ✅ 关键修改：登录成功后启用自动重连机制
                _logger?.LogInformation("[登录] 登录成功，启用自动重连机制");
                _communicationService.ConnectionManager.AutoReconnect = true;
                _communicationService.StartBackgroundTasks();

                _logger?.LogInformation("========== 登录流程完成 ==========");
                return response;
            }
            catch (OperationCanceledException ex)
            {
                _logger?.LogDebug(ex, "[登录取消] 登录操作被取消 - Username: {Username}, IsUserCanceled: {IsUserCanceled}", 
                    username, ct.IsCancellationRequested);
                throw; // 重新抛出，让调用方知道是被取消的
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "[登录异常] 登录过程中发生异常 - Username: {Username}, ExceptionType: {ExceptionType}, Message: {Message}", 
                    username, ex.GetType().Name, ex.Message);
                
                // 根据异常类型提供更具体的错误信息
                string errorMessage = ex switch
                {
                    TimeoutException => "登录超时，请检查网络连接",
                    OperationCanceledException => "登录已取消",
                    System.Net.Sockets.SocketException => "网络连接失败，请检查网络",
                    _ => $"登录失败: {ex.Message}"
                };
                
                return ResponseFactory.CreateSpecificErrorResponse<LoginResponse>(errorMessage);
            }
            finally
            {
                // 确保锁总是被释放
                try
                {
                    _loginLock.Release();
                    _logger?.LogDebug("[登录] 登录锁已释放 - Username: {Username}", username);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "[登录] 释放登录锁时发生异常");
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
                if (_communicationService.ConnectionManager.IsConnected)
                {
                    try
                    {
                        var request = LoginRequest.CreateLogoutRequest();
                        var response = await _communicationService.SendCommandWithResponseAsync<LogoutResponse>(
                            AuthenticationCommands.Logout, request, ct);

                        serverLogoutSuccess = response != null && response.IsSuccess;

                        if (!serverLogoutSuccess)
                        {

                        }
                    }
                    catch (Exception ex)
                    {

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

                // 即使发生异常，也尝试清理基本的登录状态
                try
                {
                    await CleanupLoginStateAsync();
                }
                catch (Exception cleanupEx)
                {

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
            // 优化：使用5分钟缓冲时间，避免短期重连时Token被误判为失效
            if (tokenInfo != null && !tokenInfo.IsExpired())
            {
                return tokenInfo.AccessToken;
            }
            return null;
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
                // ✅ 简化：直接使用TokenManager验证，不再依赖TokenRefreshService
                return await _tokenManager.ValidateTokenAsync(token);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 清理登录状态
        /// ✅ 修复：外网环境下，如果是因为超时导致的登录失败，不清理连接状态
        /// </summary>
        public async Task CleanupLoginStateAsync()
        {
            try
            {
                // ✅ 关键修复：先清理SessionId，避免重新登录时使用旧状态
                if (MainForm.Instance?.AppContext != null)
                {
                    MainForm.Instance.AppContext.SessionId = null;
                }
                
                // 清除Token存储
                await _tokenManager.TokenStorage.ClearTokenAsync();
                // 重置登录状态
                _isLoggedIn = false;
                //_currentUserSession = null;

                _logger?.LogDebug("[清理登录状态] 已清理SessionId和Token");
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "[清理登录状态] 发生异常");
            }
        }

        /// <summary>
        /// 处理注册状态和到期提醒
        /// </summary>
        /// <param name="loginResponse">登录响应</param>
        /// <param name="isAutoLogin">是否为自动登录（自动登录时不显示弹窗）</param>
        private async Task HandleRegistrationStatusAsync(LoginResponse loginResponse, bool isAutoLogin = false)
        {
            try
            {
                // 检查是否需要显示到期提醒
                if (loginResponse.ExpirationReminder != null && loginResponse.ExpirationReminder.NeedsReminder)
                {
                    _logger?.LogInformation($"收到注册到期提醒：{loginResponse.ExpirationReminder.ReminderMessage}");

                    // ✅ 自动登录时不显示弹窗，仅记录日志
                    if (!isAutoLogin)
                    {
                        // 在主线程上显示提醒
                        if (MainForm.Instance != null && !MainForm.Instance.IsDisposed)
                        {
                            MainForm.Instance.Invoke(new Action(() =>
                            {
                                MessageBox.Show(
                                    loginResponse.ExpirationReminder.ReminderMessage + "\n\n" +
                                    $"续费方式：{loginResponse.ExpirationReminder.RenewalMethod}\n" +
                                    $"联系方式：{loginResponse.ExpirationReminder.ContactInfo}",
                                    "注册到期提醒",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                            }));
                        }
                    }
                    else
                    {
                        _logger?.LogDebug("自动登录模式，跳过注册到期提醒弹窗");
                    }
                }

                // 检查注册状态
                if (loginResponse.RegistrationStatus == RegistrationStatus.ExpiringSoon)
                {
                    _logger?.LogInformation("系统注册即将到期");
                }
                else if (loginResponse.RegistrationStatus == RegistrationStatus.Normal)
                {
                    _logger?.LogInformation("系统注册状态正常");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理注册状态时发生异常");
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
                if (!_communicationService.ConnectionManager.IsConnected || string.IsNullOrEmpty(sessionId))
                {

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
                if (!_communicationService.ConnectionManager.IsConnected ||
                    _duplicateLoginResult == null ||
                    _duplicateLoginResult.ExistingSessions == null ||
                    _duplicateLoginResult.ExistingSessions.Count == 0)
                {
                    _logger?.LogWarning("[强制下线] 前置条件不满足: Connected={IsConnected}, HasSessions={HasSessions}",
                        _communicationService.ConnectionManager.IsConnected,
                        _duplicateLoginResult?.ExistingSessions?.Count > 0);
                    return false;
                }

                var targetSessionId = _duplicateLoginResult.ExistingSessions[0].SessionId.ToString();
                _logger?.LogInformation($"[强制下线请求] 准备发送DuplicateLogin命令: TargetSessionId={targetSessionId}");

                var request = new LoginRequest()
                {
                    Username = "",
                    Password = "",
                    AdditionalData = new System.Collections.Generic.Dictionary<string, object>
                    {
                        ["Action"] = "你的账号在其它地方登录。当前连接即将断开。请保存数据。",
                        // ✅ 修复：确保传递的是 SessionId 而不是 UserId，以便服务器能准确定位会话
                        ["TargetUserId"] = targetSessionId
                    }
                };

                // ✅ 修复：增加重试机制，最多重试2次
                int maxRetries = 2;
                Exception lastException = null;

                for (int retry = 0; retry <= maxRetries; retry++)
                {
                    try
                    {
                        if (retry > 0)
                        {
                            _logger?.LogInformation($"[强制下线] 第{retry}次重试...");
                            await Task.Delay(500, ct); // 等待500ms后重试
                        }

                        var response = await _communicationService.SendCommandWithResponseAsync<ResponseBase>(
                            AuthenticationCommands.DuplicateLogin, request, ct, 20000);

                        if (response != null)
                        {
                            if (response.IsSuccess)
                            {
                                _logger?.LogInformation($"[强制下线成功] 服务器返回成功: TargetSessionId={targetSessionId}, Retry={retry}");

                                // 等待短暂时间，确保服务器端会话清理完成
                                await Task.Delay(500, ct);
                                return true;
                            }
                            else
                            {
                                _logger?.LogWarning($"[强制下线失败] 服务器返回失败: TargetSessionId={targetSessionId}, Message={response.ErrorMessage ?? "未知错误"}, Retry={retry}");
                                lastException = new Exception(response.ErrorMessage ?? "未知错误");
                                // 继续重试
                            }
                        }
                        else
                        {
                            _logger?.LogWarning($"[强制下线失败] 服务器返回空响应: TargetSessionId={targetSessionId}, Retry={retry}");
                            lastException = new Exception("服务器返回空响应");
                            // 继续重试
                        }
                    }
                    catch (TimeoutException ex)
                    {
                        _logger?.LogWarning(ex, $"[强制下线超时] 第{retry}次尝试超时");
                        lastException = ex;
                        // 继续重试
                    }
                    catch (OperationCanceledException ex)
                    {
                        _logger?.LogWarning(ex, $"[强制下线取消] 操作被取消");
                        return false;
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, $"[强制下线异常] 第{retry}次尝试发生异常: Type={ex.GetType().Name}, Message={ex.Message}");
                        lastException = ex;
                        // 继续重试
                    }
                }

                // 所有重试都失败了
                _logger?.LogError(lastException, $"[强制下线最终失败] 已重试{maxRetries}次，仍然失败: TargetSessionId={targetSessionId}");
                return false;
            }
            catch
            {
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


                // 使用通信服务的取消重连和断开连接方法
                return await _communicationService.CancelReconnectAndForceDisconnectAsync();
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        /// <summary>
        /// 手动触发重连（用于服务器恢复后解除锁定状态）
        /// 重置心跳失败计数和锁定状态，然后尝试重连
        /// </summary>
        /// <returns>重连是否成功</returns>
        public async Task<bool> ManualReconnectAsync()
        {
            try
            {

                return await _communicationService.ManualReconnectAsync();
            }
            catch (Exception ex)
            {

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

            }
        }
    }
}

