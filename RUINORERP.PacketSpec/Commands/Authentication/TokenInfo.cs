using System;
using Newtonsoft.Json; // 新增

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    /// <summary>
    /// Token信息类
    /// 包含访问令牌、刷新令牌和过期时间等信息
    /// </summary>
    [Serializable]
    public class TokenInfo
    {
        /// <summary>
        /// 访问令牌
        /// 用于API访问的主要令牌
        /// </summary>
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        /// <summary>
        /// 刷新令牌
        /// 用于在访问令牌过期后获取新的访问令牌
        /// </summary>
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// 过期时间
        /// 访问令牌的到期时间点
        /// </summary>
        [JsonProperty("expiresAt")]
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// 刷新令牌过期时间
        /// 刷新令牌的到期时间点（通常比访问令牌更长）
        /// </summary>
        [JsonProperty("refreshTokenExpiresAt")]
        public DateTime RefreshTokenExpiresAt { get; set; }

        /// <summary>
        /// 令牌类型 - 默认Bearer
        /// </summary>
        [JsonProperty("tokenType")]
        public string TokenType { get; set; } = "Bearer";

        /// <summary>
        /// 检查访问令牌是否已过期
        /// </summary>
        public bool IsExpired()
        {
            // 添加1分钟的缓冲时间，避免临界情况
            return ExpiresAt.AddMinutes(-1) < DateTime.Now;
        }

        /// <summary>
        /// 检查刷新令牌是否已过期
        /// </summary>
        public bool IsRefreshTokenExpired()
        {
            // 添加1分钟的缓冲时间，避免临界情况
            return RefreshTokenExpiresAt.AddMinutes(-1) < DateTime.Now;
        }
    }
}