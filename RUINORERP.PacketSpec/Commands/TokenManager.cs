using System;
using System.Collections.Concurrent;

namespace RUINORERP.PacketSpec.Tokens
{
    /// <summary>
    /// Token存储接口 - 定义Token的存储和获取抽象方法
    /// </summary>
    public interface ITokenStorage
    {
        /// <summary>
        /// 获取当前存储的Token信息
        /// </summary>
        /// <returns>返回一个元组，包含是否成功、访问Token和刷新Token</returns>
        (bool success, string atk, string rtk) GetTokens();
        
        /// <summary>
        /// 设置Token信息
        /// </summary>
        /// <param name="atk">访问Token</param>
        /// <param name="rtk">刷新Token</param>
        /// <param name="expiresInSeconds">Token过期时间（秒）</param>
        void SetTokens(string atk, string rtk, int expiresInSeconds);
        
        /// <summary>
        /// 检查访问Token是否已过期或即将过期（5分钟内）
        /// </summary>
        /// <returns>如果访问Token已过期或即将过期则返回true，否则返回false</returns>
        bool IsAccessTokenExpired();
        
        /// <summary>
        /// 清除存储的Token信息
        /// </summary>
        void ClearTokens();
    }
    
    /// <summary>
    /// 默认内存Token存储实现
    /// </summary>
    public class MemoryTokenStorage : ITokenStorage
    {
        // 使用ConcurrentDictionary存储Token信息，确保线程安全
        private static readonly ConcurrentDictionary<string, (string accessToken, string refreshToken, DateTime accessExpiryUtc, DateTime refreshExpiryUtc)> _store = 
            new ConcurrentDictionary<string, (string, string, DateTime, DateTime)>();

        private const string DEFAULT_KEY = "default_token";

        /// <summary>
        /// 获取当前存储的Token信息
        /// </summary>
        /// <returns>返回一个元组，包含是否成功、访问Token和刷新Token</returns>
        public (bool success, string atk, string rtk) GetTokens()
        {
            if (_store.TryGetValue(DEFAULT_KEY, out var tokenData))
            {
                return (true, tokenData.accessToken, tokenData.refreshToken);
            }
            return (false, null, null);
        }
        
        /// <summary>
        /// 设置Token信息
        /// </summary>
        /// <param name="atk">访问Token</param>
        /// <param name="rtk">刷新Token</param>
        /// <param name="expiresInSeconds">Token过期时间（秒）</param>
        public void SetTokens(string atk, string rtk, int expiresInSeconds)
        {
            if (string.IsNullOrEmpty(atk) || string.IsNullOrEmpty(rtk))
                return;

            var now = DateTime.UtcNow;
            // 这里假设TokenInfo.CalcExpiry是一个公共方法，或者我们可以在这里直接计算
            var accessExpiryUtc = now.AddSeconds(expiresInSeconds);
            var refreshExpiryUtc = now.AddSeconds(expiresInSeconds * 2); // 刷新Token通常比访问Token有效期长

            _store[DEFAULT_KEY] = (atk, rtk, accessExpiryUtc, refreshExpiryUtc);
        }
        
        /// <summary>
        /// 检查访问Token是否已过期或即将过期（5分钟内）
        /// </summary>
        /// <returns>如果访问Token已过期或即将过期则返回true，否则返回false</returns>
        public bool IsAccessTokenExpired()
        {
            if (_store.TryGetValue(DEFAULT_KEY, out var tokenData))
            {
                // 检查Token是否已过期或将在5分钟内过期
                return DateTime.UtcNow >= tokenData.accessExpiryUtc || 
                       DateTime.UtcNow.AddMinutes(5) >= tokenData.accessExpiryUtc;
            }
            return true; // 如果没有存储Token，则视为已过期
        }
        
        /// <summary>
        /// 清除存储的Token信息
        /// </summary>
        public void ClearTokens()
        {
            _store.TryRemove(DEFAULT_KEY, out _);
        }
    }
    
    /// <summary>
    /// Token管理器 - 提供全局访问点来管理认证Token
    /// 采用单例模式设计，确保全局只有一个Token管理器实例
    /// </summary>
    public class TokenManager
    {
        // 单例实例
        private static readonly Lazy<TokenManager> _instance = new Lazy<TokenManager>(() => new TokenManager());
        
        // 当前使用的Token存储实现
        private ITokenStorage _tokenStorage = new MemoryTokenStorage();
        
        /// <summary>
        /// 获取Token管理器的单例实例
        /// </summary>
        public static TokenManager Instance => _instance.Value;
        
        /// <summary>
        /// 私有构造函数，防止外部实例化
        /// </summary>
        private TokenManager() { }
        
        /// <summary>
        /// 设置Token存储实现
        /// 允许在应用程序启动时自定义Token的存储方式
        /// </summary>
        /// <param name="storage">Token存储实现</param>
        public void SetTokenStorage(ITokenStorage storage)
        {
            if (storage != null)
            {
                _tokenStorage = storage;
            }
        }
        
        /// <summary>
        /// 获取当前存储的Token信息
        /// </summary>
        /// <returns>返回一个元组，包含是否成功、访问Token和刷新Token</returns>
        public (bool success, string atk, string rtk) GetTokens()
        {
            return _tokenStorage.GetTokens();
        }
        
        /// <summary>
        /// 设置Token信息
        /// </summary>
        /// <param name="atk">访问Token</param>
        /// <param name="rtk">刷新Token</param>
        /// <param name="expiresInSeconds">Token过期时间（秒）</param>
        public void SetTokens(string atk, string rtk, int expiresInSeconds)
        {
            _tokenStorage.SetTokens(atk, rtk, expiresInSeconds);
        }
        
        /// <summary>
        /// 检查访问Token是否已过期或即将过期（5分钟内）
        /// </summary>
        /// <returns>如果访问Token已过期或即将过期则返回true，否则返回false</returns>
        public bool IsAccessTokenExpired()
        {
            return _tokenStorage.IsAccessTokenExpired();
        }
        
        /// <summary>
        /// 清除存储的Token信息
        /// </summary>
        public void ClearTokens()
        {
            _tokenStorage.ClearTokens();
        }
    }
}