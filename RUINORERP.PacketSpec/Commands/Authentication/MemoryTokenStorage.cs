using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    /// <summary>
    /// 内存Token存储实现 - 异步版本
    /// 统一使用异步接口，简化Token生命周期管理
    /// </summary>
    public class MemoryTokenStorage : ITokenStorage
    {
        private readonly ConcurrentDictionary<string, TokenInfo> _tokenStore = new ConcurrentDictionary<string, TokenInfo>();
        private const string DEFAULT_KEY = "default_token";

        /// <summary>
        /// 异步设置Token信息
        /// </summary>
        /// <param name="tokenInfo">完整的Token信息对象</param>
        public async Task SetTokenAsync(TokenInfo tokenInfo)
        {
            if (tokenInfo == null) throw new ArgumentNullException(nameof(tokenInfo));
            if (string.IsNullOrEmpty(tokenInfo.AccessToken))
                throw new ArgumentException("AccessToken不能为空", nameof(tokenInfo));
 

            _tokenStore.AddOrUpdate(DEFAULT_KEY, tokenInfo, (key, oldValue) => tokenInfo);
            
            // 模拟异步操作
            await Task.CompletedTask;
        }

        /// <summary>
        /// 异步获取Token信息
        /// </summary>
        /// <returns>TokenInfo对象，如果不存在则返回null</returns>
        public async Task<TokenInfo> GetTokenAsync()
        {
            var result = _tokenStore.TryGetValue(DEFAULT_KEY, out var tokenInfo) ? tokenInfo : null;
            
            // 模拟异步操作
            await Task.CompletedTask;
            return result;
        }

        /// <summary>
        /// 异步清除存储的Token信息
        /// </summary>
        public async Task ClearTokenAsync()
        {
            _tokenStore.TryRemove(DEFAULT_KEY, out _);
            
            // 模拟异步操作
            await Task.CompletedTask;
        }

        /// <summary>
        /// 异步检查Token是否有效（未过期且存在）
        /// </summary>
        /// <returns>如果Token有效则返回true，否则返回false</returns>
        public async Task<bool> IsTokenValidAsync()
        {
            var tokenInfo = await GetTokenAsync();
            // 使用TokenInfo的过期检查方法 - 这是统一的标准实现
#pragma warning disable CS0618 // 类型或成员已过时
            var result = tokenInfo != null && !tokenInfo.IsAccessTokenExpired();
#pragma warning restore CS0618 // 类型或成员已过时
     
            // 模拟异步操作
            await Task.CompletedTask;
            return result;
        }
    }
}
