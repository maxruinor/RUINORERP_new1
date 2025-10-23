using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    public class MemoryTokenStorage : ITokenStorage
    {
        private readonly ConcurrentDictionary<string, TokenInfo> _tokenStore = new ConcurrentDictionary<string, TokenInfo>();
        private const string DEFAULT_KEY = "default_token";

        public Task SetTokenAsync(TokenInfo tokenInfo)
        {
            if (tokenInfo == null) throw new ArgumentNullException(nameof(tokenInfo));
            if (string.IsNullOrEmpty(tokenInfo.AccessToken))
                throw new ArgumentException("AccessToken不能为空", nameof(tokenInfo));

            _tokenStore.AddOrUpdate(DEFAULT_KEY, tokenInfo, (key, oldValue) => tokenInfo);
            return Task.CompletedTask;
        }

        public Task<TokenInfo> GetTokenAsync()
        {
            var result = _tokenStore.TryGetValue(DEFAULT_KEY, out var tokenInfo) ? tokenInfo : null;
            return Task.FromResult(result);
        }

        public Task ClearTokenAsync()
        {
            _tokenStore.TryRemove(DEFAULT_KEY, out _);
            return Task.CompletedTask;
        }

        public Task<bool> IsTokenValidAsync()
        {
            var tokenInfo = GetTokenAsync().GetAwaiter().GetResult();
            var result = tokenInfo != null && !tokenInfo.IsExpired();
            return Task.FromResult(result);
        }
    }
}
