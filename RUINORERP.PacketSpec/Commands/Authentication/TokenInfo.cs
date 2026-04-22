using System;
using Newtonsoft.Json; // 新增

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    /// <summary>
    /// Token信息类（单令牌机制）
    /// 包含访问令牌和过期时间等信息
    /// </summary>
    [Serializable]
    public class TokenInfo
    {
        /// <summary>
        /// 访问令牌
        /// 用于API访问的唯一令牌
        /// </summary>
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        /// <summary>
        /// 过期时间
        /// 访问令牌的到期时间点
        /// </summary>
        [JsonProperty("expiresAt")]
        public DateTime ExpiresAt { get; set; }

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
            // 添加5分钟的缓冲时间，避免短期重连时Token被误判为失效
            // 使用UtcNow确保时区一致性
            return ExpiresAt.AddMinutes(-5) < DateTime.UtcNow;
        }

        /// <summary>
        /// 检查Token是否即将过期
        /// </summary>
        /// <param name="threshold">提前预警时间</param>
        /// <returns>是否即将过期</returns>
        public bool WillExpireSoon(TimeSpan threshold)
        {
            // 使用UtcNow确保时区一致性
            return ExpiresAt > DateTime.UtcNow && (ExpiresAt - DateTime.UtcNow) < threshold;
        }
    }
}