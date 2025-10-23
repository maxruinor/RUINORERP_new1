using RUINORERP.PacketSpec.Commands.Authentication;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Authentication
{
    public class SilentTokenRefresher
    {
        private readonly ITokenRefreshService _tokenRefreshService;
        private readonly SemaphoreSlim _refreshSemaphore = new SemaphoreSlim(1, 1);
        private readonly int _maxRetries = 3;
        private readonly int _retryDelayMs = 1000; // 更合理的初始延迟
        private readonly double _backoffMultiplier = 1.5; // 指数退避乘数
        
        /// <summary>
        /// 当Token刷新成功时触发
        /// </summary>
        public event EventHandler<TokenInfo> OnRefreshSucceeded;
        
        /// <summary>
        /// 当Token刷新失败时触发
        /// </summary>
        public event EventHandler<Exception> OnRefreshFailed;

        public SilentTokenRefresher(ITokenRefreshService tokenRefreshService)
        {
            _tokenRefreshService = tokenRefreshService ?? throw new ArgumentNullException(nameof(tokenRefreshService));
        }

        /// <summary>
        /// 异步刷新Token，使用信号量确保并发安全
        /// </summary>
        /// <param name="ct">取消令牌</param>
        /// <returns>是否刷新成功</returns>
        public async Task<bool> TryRefreshTokenAsync(CancellationToken ct = default)
        {
            if (!await _refreshSemaphore.WaitAsync(0))
            {
                // 已有刷新操作正在进行，避免重复刷新
                return false;
            }

            try
            {
                int retryCount = 0;
                TokenInfo refreshedToken = null;
                Exception lastException = null;

                while (retryCount < _maxRetries)
                {
                    try
                    {
                        // 调用刷新服务获取新Token
                        refreshedToken = await _tokenRefreshService.RefreshTokenAsync(ct);
                        
                        // 验证是否成功获取到有效的TokenInfo对象
                        if (refreshedToken != null && !string.IsNullOrEmpty(refreshedToken.AccessToken))
                        {
                            // 触发刷新成功事件
                            OnRefreshSucceeded?.Invoke(this, refreshedToken);
                            return true;
                        }
                        else
                        {
                            throw new InvalidOperationException("刷新Token失败：未获取到有效的TokenInfo对象");
                        }
                    }
                    catch (Exception ex)
                    {
                        lastException = ex;
                        // 记录异常但继续尝试
                    }

                    retryCount++;
                    
                    if (retryCount < _maxRetries)
                    {
                        // 指数退避策略
                        int delayMs = (int)(_retryDelayMs * Math.Pow(_backoffMultiplier, retryCount - 1));
                        await Task.Delay(delayMs, ct);
                    }
                }

                // 所有重试都失败了
                if (lastException != null)
                {
                    OnRefreshFailed?.Invoke(this, lastException);
                }
                
                return false;
            }
            finally
            {
                _refreshSemaphore.Release();
            }
        }
    }
}