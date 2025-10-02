using System;
using System.Collections.Concurrent;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Tokens;

namespace RUINORERP.UI.Network.Authentication
{
    /// <summary>
    /// 客户端Token存储管理类
    /// 提供线程安全的Token存储、获取和过期检查功能
    /// 实现ITokenStorage接口，使其可以与TokenManager集成
    /// </summary>
    public class ClientTokenStorage : ITokenStorage
    {
        // 使用ConcurrentDictionary存储Token信息，确保线程安全
        private static readonly ConcurrentDictionary<string, (string accessToken, string refreshToken, DateTime accessExpiryUtc, DateTime refreshExpiryUtc)> _store = 
            new ConcurrentDictionary<string, (string, string, DateTime, DateTime)>();

        private const string DEFAULT_KEY = "default_token";
        private static readonly ClientTokenStorage _instance = new ClientTokenStorage();
        
        /// <summary>
        /// 获取ClientTokenStorage的单例实例
        /// </summary>
        public static ClientTokenStorage Instance => _instance;
        
        /// <summary>
        /// 私有构造函数，防止外部实例化
        /// </summary>
        private ClientTokenStorage() { }
        
        /// <summary>
        /// 静态初始化方法 - 注册ClientTokenStorage到TokenManager
        /// 应该在应用程序启动时调用
        /// </summary>
        public static void Initialize()
        {
            TokenManager.Instance.SetTokenStorage(_instance);
        }

        #region ITokenStorage 接口实现

        /// <summary>
        /// 获取当前存储的Token信息（接口实现）
        /// </summary>
        /// <returns>返回一个元组，包含是否成功、访问Token和刷新Token</returns>
        (bool success, string atk, string rtk) ITokenStorage.GetTokens()
        {
            return GetTokens();
        }

        /// <summary>
        /// 设置Token信息（接口实现）
        /// </summary>
        /// <param name="atk">访问Token</param>
        /// <param name="rtk">刷新Token</param>
        /// <param name="expiresInSeconds">Token过期时间（秒）</param>
        void ITokenStorage.SetTokens(string atk, string rtk, int expiresInSeconds)
        {
            SetTokens(atk, rtk, expiresInSeconds);
        }

        /// <summary>
        /// 检查访问Token是否已过期或即将过期（接口实现）
        /// </summary>
        /// <returns>如果访问Token已过期或即将过期则返回true，否则返回false</returns>
        bool ITokenStorage.IsAccessTokenExpired()
        {
            return IsAccessTokenExpired();
        }

        /// <summary>
        /// 清除存储的Token信息（接口实现）
        /// </summary>
        void ITokenStorage.ClearTokens()
        {
            ClearTokens();
        }

        #endregion

        #region 静态方法（向后兼容）

        /// <summary>
        /// 获取当前存储的Token信息
        /// </summary>
        /// <returns>返回一个元组，包含是否成功、访问Token和刷新Token</returns>
        public static (bool success, string atk, string rtk) GetTokens()
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
        public static void SetTokens(string atk, string rtk, int expiresInSeconds)
        {
            if (string.IsNullOrEmpty(atk) || string.IsNullOrEmpty(rtk))
                return;

            var now = DateTime.UtcNow;
            var accessExpiryUtc = TokenInfo.CalcExpiry(now, expiresInSeconds);
            var refreshExpiryUtc = TokenInfo.CalcExpiry(now, expiresInSeconds, true);

            _store[DEFAULT_KEY] = (atk, rtk, accessExpiryUtc, refreshExpiryUtc);
        }

        /// <summary>
        /// 检查访问Token是否已过期或即将过期（5分钟内）
        /// </summary>
        /// <returns>如果访问Token已过期或即将过期则返回true，否则返回false</returns>
        public static bool IsAccessTokenExpired()
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
        /// 检查刷新Token是否已过期
        /// </summary>
        /// <returns>如果刷新Token已过期则返回true，否则返回false</returns>
        public static bool IsRefreshTokenExpired()
        {
            if (_store.TryGetValue(DEFAULT_KEY, out var tokenData))
            {
                return DateTime.UtcNow >= tokenData.refreshExpiryUtc;
            }
            return true; // 如果没有存储Token，则视为已过期
        }

        /// <summary>
        /// 清除存储的Token信息
        /// </summary>
        public static void ClearTokens()
        {
            _store.TryRemove(DEFAULT_KEY, out _);
        }

        /// <summary>
        /// 获取访问Token的过期时间
        /// </summary>
        /// <returns>访问Token的过期时间（UTC）</returns>
        public static DateTime? GetAccessTokenExpiryUtc()
        {
            if (_store.TryGetValue(DEFAULT_KEY, out var tokenData))
            {
                return tokenData.accessExpiryUtc;
            }
            return null;
        }

        /// <summary>
        /// 获取刷新Token的过期时间
        /// </summary>
        /// <returns>刷新Token的过期时间（UTC）</returns>
        public static DateTime? GetRefreshTokenExpiryUtc()
        {
            if (_store.TryGetValue(DEFAULT_KEY, out var tokenData))
            {
                return tokenData.refreshExpiryUtc;
            }
            return null;
        }

        #endregion
    }
}