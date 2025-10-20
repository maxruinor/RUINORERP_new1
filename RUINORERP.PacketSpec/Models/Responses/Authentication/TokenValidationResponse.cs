using MessagePack;
using RUINORERP.PacketSpec.Models.Responses;
using System;

namespace RUINORERP.PacketSpec.Models.Responses.Authentication
{
    /// <summary>
    /// 令牌验证响应 - 表示令牌验证的结果
    /// </summary>
    [MessagePackObject]
    public class TokenValidationResponse : ResponseBase
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Key(10)]
        public long UserId { get; set; }

        /// <summary>
        /// 令牌是否有效
        /// </summary>
        [Key(11)]
        public bool IsTokenValid { get; set; }

        /// <summary>
        /// 令牌过期时间
        /// </summary>
        [Key(12)]
        public DateTime? ExpireTime { get; set; }

        /// <summary>
        /// 会话ID
        /// </summary>
        [Key(13)]
        public string SessionId { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public TokenValidationResponse()
        {}

        /// <summary>
        /// 创建成功响应（令牌有效）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="sessionId">会话ID</param>
        /// <param name="expireTime">过期时间</param>
        /// <returns>成功的响应实例</returns>
        public static TokenValidationResponse Success(long userId, string sessionId, DateTime? expireTime)
        {
            return new TokenValidationResponse
            {
                IsSuccess = true,
                ErrorCode = 0,
                Message = "令牌验证成功",
                UserId = userId,
                SessionId = sessionId,
                ExpireTime = expireTime,
                IsTokenValid = true
            };
        }

        /// <summary>
        /// 创建失败响应（令牌无效）
        /// </summary>
        /// <param name="errorCode">错误码</param>
        /// <param name="errorMessage">错误消息</param>
        /// <returns>失败的响应实例</returns>
        public static TokenValidationResponse Fail(int errorCode, string errorMessage)
        {
            return new TokenValidationResponse
            {
                IsSuccess = false,
                ErrorCode = errorCode,
                ErrorMessage = errorMessage,
                Message = "令牌验证失败",
                IsTokenValid = false
            };
        }
    }
}