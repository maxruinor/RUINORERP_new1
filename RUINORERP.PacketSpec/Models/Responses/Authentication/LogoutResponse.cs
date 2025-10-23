using Newtonsoft.Json;
using RUINORERP.PacketSpec.Models.Responses;
using System;

namespace RUINORERP.PacketSpec.Models.Responses.Authentication
{
    /// <summary>
    /// 登出响应 - 表示用户退出系统的结果
    /// </summary>
    [JsonObject]
    public class LogoutResponse : ResponseBase
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [JsonProperty(Order=10)]
        public long UserId { get; set; }

        /// <summary>
        /// 登出时间
        /// </summary>
        [JsonProperty(Order=11)]
        public DateTime LogoutTime { get; set; }

        /// <summary>
        /// 会话ID
        /// </summary>
        [JsonProperty(Order=12)]
        public string SessionId { get; set; }

        /// <summary>
        /// 构造函�?        /// </summary>
        public LogoutResponse()
        {
            LogoutTime = DateTime.Now;
        }

        /// <summary>
        /// 创建成功响应
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="sessionId">会话ID</param>
        /// <returns>成功的响应实�?/returns>
        public static LogoutResponse Success(long userId, string sessionId)
        {
            return new LogoutResponse
            {
                IsSuccess = true,
                ErrorCode = 0,
                Message = "登出成功",
                UserId = userId,
                SessionId = sessionId,
                LogoutTime = DateTime.Now
            };
        }

        /// <summary>
        /// 创建失败响应
        /// </summary>
        /// <param name="errorCode">错误�?/param>
        /// <param name="errorMessage">错误消息</param>
        /// <returns>失败的响应实�?/returns>
        public static LogoutResponse Fail(int errorCode, string errorMessage)
        {
            return new LogoutResponse
            {
                IsSuccess = false,
                ErrorCode = errorCode,
                ErrorMessage = errorMessage,
                Message = "登出失败"
            };
        }
    }
}


