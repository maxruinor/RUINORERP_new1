using System;

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    /// <summary>
    /// Token信息类，包含访问令牌和刷新令牌的完整信息
    /// </summary>
    public class TokenInfo
    {
        /// <summary>
        /// 访问令牌
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 刷新令牌
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// 访问令牌过期时间（秒）
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// 刷新令牌过期时间（秒）
        /// </summary>
        public int RefreshTokenExpiresIn { get; set; }

        /// <summary>
        /// 令牌类型
        /// </summary>
        public string TokenType { get; set; }

        /// <summary>
        /// 生成时间（UTC）
        /// </summary>
        public DateTime GeneratedTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 访问令牌过期时间（UTC）
        /// </summary>
        public DateTime AccessTokenExpiryUtc { get; set; }

        /// <summary>
        /// 刷新令牌过期时间（UTC）
        /// </summary>
        public DateTime RefreshTokenExpiryUtc { get; set; }

        /// <summary>
        /// 检查访问令牌是否已过期
        /// </summary>
        public bool IsAccessTokenExpired() => DateTime.UtcNow >= AccessTokenExpiryUtc;

        /// <summary>
    /// 检查刷新令牌是否已过期
    /// </summary>
    public bool IsRefreshTokenExpired() => DateTime.UtcNow >= RefreshTokenExpiryUtc;

    /// <summary>
    /// 计算令牌过期时间
    /// </summary>
    /// <param name="currentTime">当前时间（UTC）</param>
    /// <param name="expiresInSeconds">过期时间（秒）</param>
    /// <param name="isRefreshToken">是否为刷新令牌</param>
    /// <returns>计算后的过期时间（UTC）</returns>
    public static DateTime CalcExpiry(DateTime currentTime, int expiresInSeconds, bool isRefreshToken = false)
    {
        if (isRefreshToken)
        {
            // 刷新令牌通常有效期更长，设置为访问令牌的3倍
            return currentTime.AddSeconds(expiresInSeconds * 3);
        }
        return currentTime.AddSeconds(expiresInSeconds);
    }
}
}
