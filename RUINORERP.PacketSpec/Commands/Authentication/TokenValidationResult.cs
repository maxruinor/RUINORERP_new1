using System;
using System.Collections.Generic;

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    /// <summary>
    /// Token验证结果类
    /// </summary>
    public class TokenValidationResult
    {
        /// <summary>
        /// 验证是否成功
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 过期时间（可空，验证失败时为null）
        /// </summary>
        public DateTime? ExpiryTime { get; set; }

        /// <summary>
        /// 验证失败消息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 附加声明
        /// </summary>
        public IDictionary<string, object> Claims { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// 刷新令牌（刷新接口需要原样带回）
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
