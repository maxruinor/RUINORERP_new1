using System;
using System.Collections.Generic;
using MessagePack;

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    /// <summary>
    /// Token验证结果类
    /// </summary>
    [MessagePackObject]
    public class TokenValidationResult
    {
        /// <summary>
        /// 验证是否成功
        /// </summary>
        [Key(0)]
        public bool IsValid { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Key(1)]
        public string UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Key(2)]
        public string UserName { get; set; }

        /// <summary>
        /// 过期时间（可空，验证失败时为null）
        /// </summary>
        [Key(3)]
        public DateTime? ExpiryTime { get; set; }

        /// <summary>
        /// 验证失败消息
        /// </summary>
        [Key(4)]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 附加声明
        /// </summary>
        [MessagePack.IgnoreMember]
        public IDictionary<string, object> Claims { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Token信息
        /// </summary>
        [Key(5)]
        public TokenInfo Token { get; set; }

        /// <summary>
        /// 获取刷新令牌（通过TokenInfo实例获取）
        /// </summary>
        [IgnoreMember]
        public string RefreshToken => Token?.RefreshToken;
    }
}