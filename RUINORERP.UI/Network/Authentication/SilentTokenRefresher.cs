using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Authentication
{
    /// <summary>
    /// 静默Token刷新管理器
    /// 只负责触发Token刷新操作，不自行维护定时器
    /// </summary>
    public sealed class SilentTokenRefresher : IDisposable
    {
        private readonly ITokenRefreshService _tokenRefreshService;
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
        /// <param name="tokenRefreshService">Token刷新服务实例</param>
        public SilentTokenRefresher(ITokenRefreshService tokenRefreshService)
        {
            _tokenRefreshService = tokenRefreshService ?? throw new ArgumentNullException(nameof(tokenRefreshService));
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
        /// 内部Token刷新方法 - 简化版
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>刷新是否成功</returns>
        private async Task<bool> RefreshTokenInternalAsync(CancellationToken cancellationToken)
        {
            await _refreshLock.WaitAsync(cancellationToken);
            try
            {
                TokenInfo tokenInfo = null;
                int retryCount = 0;
                Exception lastException = null;

                while (retryCount < MAX_RETRY_COUNT)
                {
                    try
                    {
                        /// 调用Token刷新服务
                        /// TODO  这里传入了空字符串 ，待完善
                        tokenInfo = await _tokenRefreshService.RefreshTokenAsync(string.Empty, cancellationToken);
                        if (tokenInfo != null && !string.IsNullOrEmpty(tokenInfo.AccessToken))
                        {
                            OnRefreshSucceeded(tokenInfo);
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        lastException = ex;
                        retryCount++;
                        if (retryCount < MAX_RETRY_COUNT)
                        {
                            await Task.Delay(RETRY_DELAY_MS, cancellationToken);
                        }
                    }
                }

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
        private void OnRefreshSucceeded(TokenInfo tokenInfo)
        {
            RefreshSucceeded?.Invoke(this, new RefreshSucceededEventArgs(tokenInfo));
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
            public TokenInfo tokenInfo { get; }

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="response">刷新响应</param>
            public RefreshSucceededEventArgs(TokenInfo _tokenInfo)
            {
                tokenInfo = _tokenInfo;
            }
        }
    }
}