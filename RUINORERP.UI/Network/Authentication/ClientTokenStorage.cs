using System;
using System.Collections.Concurrent;

namespace RUINORERP.UI.Network.Authentication
{
    /// <summary>
    /// 客户端Token存储管理类
    /// 提供线程安全的Token存储、获取和过期检查功能
    /// </summary>
    public class ClientTokenStorage
    {
        // 使用ConcurrentDictionary存储Token信息，确保线程安全
        private static readonly ConcurrentDictionary<string, (string accessToken, string refreshToken, DateTime accessExpiryUtc, DateTime refreshExpiryUtc)> _store = 
            new ConcurrentDictionary<string, (string, string, DateTime, DateTime)>();

        private const string DEFAULT_KEY = "default_token";

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
            var accessExpiryUtc = now.AddSeconds(expiresInSeconds);
            // 刷新Token通常有效期更长，这里设置为访问Token的3倍
            var refreshExpiryUtc = now.AddSeconds(expiresInSeconds * 3);

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
    }
}