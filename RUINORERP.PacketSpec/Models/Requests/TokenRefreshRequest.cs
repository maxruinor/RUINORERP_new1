using System;
using System.Collections.Generic;

namespace RUINORERP.PacketSpec.Models.Requests
{
    /// <summary>
    /// Token刷新请求数据模型
    /// </summary>
    [Serializable]
    public class TokenRefreshRequest : RequestBase
    {
        /// <summary>
        /// 当前Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 刷新Token
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// 客户端IP地址
        /// </summary>
        public string ClientIp { get; set; }

        /// <summary>
        /// 设备标识
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 创建Token刷新请求
        /// </summary>
        public static TokenRefreshRequest Create(string token, string refreshToken, string deviceId = null, string clientIp = null)
        {
            return new TokenRefreshRequest
            {
                Token = token,
                RefreshToken = refreshToken,
                DeviceId = deviceId ?? Guid.NewGuid().ToString(),
                ClientIp = clientIp
            };
        }

        /// <summary>
        /// 验证请求有效性
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Token) && !string.IsNullOrEmpty(RefreshToken);
        }
    }
}