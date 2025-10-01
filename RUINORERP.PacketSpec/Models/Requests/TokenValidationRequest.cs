using System;
using System.Collections.Generic;

namespace RUINORERP.PacketSpec.Models.Requests
{
    /// <summary>
    /// Token验证请求数据模型
    /// </summary>
    [Serializable]
    public class TokenValidationRequest : RequestBase
    {
        /// <summary>
        /// 待验证的Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 客户端IP地址
        /// </summary>
        public string ClientIp { get; set; }

        /// <summary>
        /// 设备标识
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 附加验证数据
        /// </summary>
        public Dictionary<string, object> ValidationData { get; set; }

        /// <summary>
        /// 创建Token验证请求
        /// </summary>
        public static TokenValidationRequest Create(string token, string deviceId = null, string clientIp = null)
        {
            return new TokenValidationRequest
            {
                Token = token,
                DeviceId = deviceId ?? Guid.NewGuid().ToString(),
                ClientIp = clientIp,
                ValidationData = new Dictionary<string, object>()
            };
        }

        /// <summary>
        /// 验证请求有效性
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Token);
        }
    }
}