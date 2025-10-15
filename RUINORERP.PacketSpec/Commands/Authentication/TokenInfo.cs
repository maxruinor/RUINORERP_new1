using System;
using MessagePack;

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    /// <summary>
    /// Token信息类 - 最终简化版
    /// 简化过期时间计算，移除冗余方法
    /// </summary>
    [Serializable]
    [MessagePackObject]
    public class TokenInfo
    {
        /// <summary>
        /// 访问令牌
        /// </summary>
        [Key(29)]
        public string AccessToken { get; set; }

        /// <summary>
        /// 刷新令牌
        /// </summary>
        [Key(30)]
        public string RefreshToken { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        [Key(31)]
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// 令牌类型 - 默认Bearer
        /// </summary>
        [Key(32)]
        public string TokenType { get; set; } = "Bearer";

        /// <summary>
        /// 检查访问令牌是否已过期
        /// 【已废弃】请使用TokenManager.ValidateStoredTokenAsync()进行统一Token验证
        /// </summary>
        [Obsolete("请使用TokenManager.ValidateStoredTokenAsync()进行统一Token验证", false)]
        public bool IsAccessTokenExpired() => DateTime.Now >= ExpiresAt;

        /// <summary>
        /// 检查刷新令牌是否已过期
        /// </summary>
        public bool IsRefreshTokenExpired() => DateTime.Now >= ExpiresAt;
    }
}
