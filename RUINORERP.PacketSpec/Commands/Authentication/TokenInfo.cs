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
        /// 令牌的到期时间点
        /// </summary>
        [JsonProperty("expiresAt")]
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// 令牌类型 - 默认Bearer
        /// </summary>
        [JsonProperty("tokenType")]
        public string TokenType { get; set; } = "Bearer";

        /// <summary>
        /// 检查令牌是否已过期
        /// 注：建议使用TokenManager.ValidateStoredTokenAsync()进行更全面的Token验证
        /// </summary>
        public bool IsExpired() => DateTime.Now >= ExpiresAt;
    }
}