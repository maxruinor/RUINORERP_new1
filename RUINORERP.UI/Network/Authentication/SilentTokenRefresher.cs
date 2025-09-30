using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.UI.Network.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Authentication
{
    /// <summary>
    /// 静默Token刷新管理器
    /// 在后台线程中自动刷新即将过期的Token，避免阻塞用户操作
    /// </summary>
    public sealed class SilentTokenRefresher : IDisposable
    {
        private readonly UserLoginService _loginService;
        private CancellationTokenSource _cts;
        private Task _refreshTask;
        private readonly SemaphoreSlim _refreshLock = new SemaphoreSlim(1, 1);
        private const int CHECK_INTERVAL_MINUTES = 1; // 检查间隔（分钟）
        private const int MAX_RETRY_COUNT = 3; // 最大重试次数
        private const int RETRY_DELAY_MS = 2000; // 重试延迟（毫秒）

        /// <summary>
        /// 刷新失败事件
        /// </summary>
        public event EventHandler<RefreshFailedEventArgs> RefreshFailed;

        /// <summary>
        /// 刷新成功事件
        /// </summary>
        public event EventHandler<RefreshSucceededEventArgs> RefreshSucceeded;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="loginService">用户登录服务实例</param>
        public SilentTokenRefresher(UserLoginService loginService)
        {
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
        }

        /// <summary>
        /// 启动静默刷新服务
        /// </summary>
        public void Start()
        {
            if (_refreshTask != null && !_refreshTask.IsCompleted)
            {
                return; // 已经在运行中
            }

            _cts = new CancellationTokenSource();
            _refreshTask = Task.Run(() => RefreshLoopAsync(_cts.Token));
        }

        /// <summary>
        /// 停止静默刷新服务
        /// </summary>
        public void Stop()
        {
            try
            {
                _cts?.Cancel();
                _cts?.Dispose();
                _cts = null;
            }
            catch { }
        }

        /// <summary>
        /// 手动触发一次Token刷新（适用于需要立即刷新的场景）
        /// </summary>
        /// <returns>刷新是否成功</returns>
        public async Task<bool> TriggerRefreshAsync()
        {
            try
            {
                return await RefreshTokenInternalAsync(CancellationToken.None);
            }
            catch (Exception ex)
            {
                // 记录异常但不抛出，避免影响用户操作
                System.Diagnostics.Debug.WriteLine($"手动刷新Token异常: {ex.Message}");
                OnRefreshFailed(ex);
                return false;
            }
        }

        /// <summary>
        /// Token刷新循环
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>异步任务</returns>
        private async Task RefreshLoopAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        // 检查Token是否需要刷新
                        if (ClientTokenStorage.IsAccessTokenExpired())
                        {
                            await RefreshTokenInternalAsync(cancellationToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        // 记录异常但不中断循环，避免影响后续刷新
                        System.Diagnostics.Debug.WriteLine($"静默刷新Token异常: {ex.Message}");
                        OnRefreshFailed(ex);
                    }

                    // 等待下一次检查
                    await Task.Delay(TimeSpan.FromMinutes(CHECK_INTERVAL_MINUTES), cancellationToken);
                }
            }
            catch (TaskCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                // 任务被取消，正常退出
            }
        }

        /// <summary>
        /// 内部Token刷新方法
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>刷新是否成功</returns>
        private async Task<bool> RefreshTokenInternalAsync(CancellationToken cancellationToken)
        {
            // 使用信号量确保同一时间只有一个刷新操作在执行
            await _refreshLock.WaitAsync(cancellationToken);
            try
            {
                // 双重检查，避免多个线程同时进入刷新逻辑
                if (!ClientTokenStorage.IsAccessTokenExpired())
                {
                    return true;
                }

                // 获取当前的Token信息
                var (success, currentToken, refreshToken) = ClientTokenStorage.GetTokens();
                if (!success || string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(currentToken))
                {
                    return false;
                }

                // 检查刷新Token是否已过期
                if (ClientTokenStorage.IsRefreshTokenExpired())
                {
                    // 刷新Token已过期，触发刷新失败事件
                    OnRefreshFailed(new Exception("刷新Token已过期，需要重新登录"));
                    return false;
                }

                // 执行Token刷新（带重试机制）
                LoginResponse response = null;
                int retryCount = 0;
                Exception lastException = null;

                while (retryCount < MAX_RETRY_COUNT)
                {
                    try
                    {
                        response = await _loginService.RefreshTokenAsync(refreshToken, currentToken, cancellationToken);
                        if (response != null && !string.IsNullOrEmpty(response.AccessToken))
                        {
                            // 刷新成功，更新Token存储
                            ClientTokenStorage.SetTokens(response.AccessToken, response.RefreshToken, response.ExpiresIn);
                            OnRefreshSucceeded(response);
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        lastException = ex;
                        retryCount++;

                        // 如果是最后一次重试，则不等待直接抛出
                        if (retryCount < MAX_RETRY_COUNT)
                        {
                            await Task.Delay(RETRY_DELAY_MS, cancellationToken);
                        }
                    }
                }

                // 重试失败
                if (lastException != null)
                {
                    OnRefreshFailed(lastException);
                }
                return false;
            }
            finally
            {
                _refreshLock.Release();
            }
        }

        /// <summary>
        /// 触发刷新失败事件
        /// </summary>
        /// <param name="exception">导致刷新失败的异常</param>
        private void OnRefreshFailed(Exception exception)
        {
            RefreshFailed?.Invoke(this, new RefreshFailedEventArgs(exception));
        }

        /// <summary>
        /// 触发刷新成功事件
        /// </summary>
        /// <param name="response">刷新响应</param>
        private void OnRefreshSucceeded(LoginResponse response)
        {
            RefreshSucceeded?.Invoke(this, new RefreshSucceededEventArgs(response));
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Stop();
            _refreshLock.Dispose();
        }

        /// <summary>
        /// Token刷新失败事件参数
        /// </summary>
        public class RefreshFailedEventArgs : EventArgs
        {
            /// <summary>
            /// 获取导致刷新失败的异常
            /// </summary>
            public Exception Exception { get; }

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="exception">导致刷新失败的异常</param>
            public RefreshFailedEventArgs(Exception exception)
            {
                Exception = exception;
            }
        }

        /// <summary>
        /// Token刷新成功事件参数
        /// </summary>
        public class RefreshSucceededEventArgs : EventArgs
        {
            /// <summary>
            /// 获取刷新响应
            /// </summary>
            public LoginResponse Response { get; }

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="response">刷新响应</param>
            public RefreshSucceededEventArgs(LoginResponse response)
            {
                Response = response;
            }
        }
    }
}