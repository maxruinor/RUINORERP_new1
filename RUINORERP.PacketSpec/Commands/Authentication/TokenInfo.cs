using System;
using MessagePack;

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    /// <summary>
    /// Token信息类
    /// 包含访问令牌、刷新令牌和过期时间等信息
    /// </summary>
    [Serializable]
    [MessagePackObject]
    public class TokenInfo
    {
        /// <summary>
        /// 访问令牌
        /// 用于API访问的主要令牌
        /// </summary>
        [Key(29)]
        public string AccessToken { get; set; }

        /// <summary>
        /// 刷新令牌
        /// 用于在访问令牌过期后获取新的访问令牌
        /// </summary>
        [Key(30)]
        public string RefreshToken { get; set; }

        /// <summary>
        /// 过期时间
        /// 令牌的到期时间点
        /// </summary>
        [Key(31)]
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// 令牌类型 - 默认Bearer
        /// </summary>
        [Key(32)]
        public string TokenType { get; set; } = "Bearer";

        /// <summary>
        /// 检查令牌是否已过期
        /// 注：建议使用TokenManager.ValidateStoredTokenAsync()进行更全面的Token验证
        /// </summary>
        public bool IsExpired() => DateTime.Now >= ExpiresAt;
    }
}
