using MessagePack;
using RUINORERP.PacketSpec.Models.Responses;
using System;

namespace RUINORERP.PacketSpec.Models.Responses.Authentication
{
    /// <summary>
    /// 令牌刷新响应 - 表示令牌刷新的结果
    /// </summary>
    [MessagePackObject]
    public class TokenRefreshResponse : ResponseBase
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Key(10)]
        public long UserId { get; set; }

        /// <summary>
        /// 新的访问令牌
        /// </summary>
        [Key(11)]
        public string NewAccessToken { get; set; }

        /// <summary>
        /// 新的刷新令牌
        /// </summary>
        [Key(12)]
        public string NewRefreshToken { get; set; }

        /// <summary>
        /// 令牌过期时间
        /// </summary>
        [Key(13)]
        public DateTime ExpireTime { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public TokenRefreshResponse()
        {}

        /// <summary>
        /// 创建成功响应
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="newAccessToken">新的访问令牌</param>
        /// <param name="newRefreshToken">新的刷新令牌</param>
        /// <param name="expireTime">过期时间</param>
        /// <returns>成功的响应实例</returns>
        public static TokenRefreshResponse Success(long userId, string newAccessToken, string newRefreshToken, DateTime expireTime)
        {
            return new TokenRefreshResponse
            {
                IsSuccess = true,
                ErrorCode = 0,
                Message = "令牌刷新成功",
                UserId = userId,
                NewAccessToken = newAccessToken,
                NewRefreshToken = newRefreshToken,
                ExpireTime = expireTime
            };
        }

        /// <summary>
        /// 创建失败响应
        /// </summary>
        /// <param name="errorCode">错误码</param>
        /// <param name="errorMessage">错误消息</param>
        /// <returns>失败的响应实例</returns>
        public static TokenRefreshResponse Fail(int errorCode, string errorMessage)
        {
            return new TokenRefreshResponse
            {
                IsSuccess = false,
                ErrorCode = errorCode,
                ErrorMessage = errorMessage,
                Message = "令牌刷新失败"
            };
        }
    }
}