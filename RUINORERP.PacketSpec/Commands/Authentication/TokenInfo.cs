using System;

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    /// <summary>
    /// Token信息类 - 最终简化版
    /// 简化过期时间计算，移除冗余方法
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
        /// 访问令牌有效期（秒）- 默认8小时
        /// </summary>
        public int ExpiresIn { get; set; } = 28800;

        /// <summary>
        /// 令牌类型 - 默认Bearer
        /// </summary>
        public string TokenType { get; set; } = "Bearer";

        /// <summary>
        /// 生成时间（UTC）
        /// </summary>
        public DateTime GeneratedTime { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// 访问令牌过期时间（UTC）- 计算属性
        /// </summary>
        public DateTime AccessTokenExpiryUtc => GeneratedTime.AddSeconds(ExpiresIn);
        
        /// <summary>
        /// 刷新令牌过期时间（UTC）- 计算属性（默认24倍，8天）
        /// </summary>
        public DateTime RefreshTokenExpiryUtc => GeneratedTime.AddSeconds(ExpiresIn * 24);

        /// <summary>
        /// 检查访问令牌是否已过期
        /// 【已废弃】请使用TokenManager.ValidateStoredTokenAsync()进行统一Token验证
        /// </summary>
        [Obsolete("请使用TokenManager.ValidateStoredTokenAsync()进行统一Token验证", false)]
        public bool IsAccessTokenExpired() => DateTime.UtcNow >= AccessTokenExpiryUtc;

        /// <summary>
        /// 检查刷新令牌是否已过期
        /// </summary>
        public bool IsRefreshTokenExpired() => DateTime.UtcNow >= RefreshTokenExpiryUtc;
    }
}