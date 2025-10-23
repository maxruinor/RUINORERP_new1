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
        
        public event EventHandler<TokenInfo> OnRefreshSucceeded;
        public event EventHandler<Exception> OnRefreshFailed;

        public SilentTokenRefresher(ITokenRefreshService tokenRefreshService)
        {
            _tokenRefreshService = tokenRefreshService ?? throw new ArgumentNullException(nameof(tokenRefreshService));
        }

        public async Task<bool> TryRefreshTokenAsync(CancellationToken ct = default)
        {
            if (!await _refreshSemaphore.WaitAsync(0, ct))
            {
                return false; // 已有刷新操作在进行中
            }

            try
            {
                var refreshedToken = await _tokenRefreshService.RefreshTokenAsync(ct);
                
                if (refreshedToken != null && !string.IsNullOrEmpty(refreshedToken.AccessToken))
                {
                    OnRefreshSucceeded?.Invoke(this, refreshedToken);
                    return true;
                }
                
                return false;
            }
            catch (Exception ex)
            {
                OnRefreshFailed?.Invoke(this, ex);
                return false;
            }
            finally
            {
                _refreshSemaphore.Release();
            }
        }
    }
}