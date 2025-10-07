using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Tokens;
using RUINORERP.UI.Network.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Authentication
{
    /// <summary>
    /// 静默Token刷新管理器
    /// 只负责触发Token刷新操作，不自行维护定时器
    /// 定时器统一由UserLoginService管理
    /// </summary>
    public sealed class SilentTokenRefresher : IDisposable
    {
        private readonly UserLoginService _loginService;
        private readonly SemaphoreSlim _refreshLock = new SemaphoreSlim(1, 1);
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
        /// 触发一次Token刷新
        /// 由UserLoginService的定时器调用
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
                    if (!TokenManager.Instance.IsAccessTokenExpired())
                    {
                        return true;
                    }

                    // 获取当前的Token信息
                    var (success, currentToken, refreshToken) = TokenManager.Instance.GetTokens();
                    if (!success || string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(currentToken))
                    {
                        return false;
                    }

                    // MemoryTokenStorage没有IsRefreshTokenExpired方法，直接返回false
                    if (false)
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
                            TokenManager.Instance.SetTokens(response.AccessToken, response.RefreshToken, response.ExpiresIn);
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