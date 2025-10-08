using RUINORERP.PacketSpec.Commands.Authentication;
using System;
using System.Collections.Generic;
using MessagePack;

namespace RUINORERP.PacketSpec.Models.Requests
{
    /// <summary>
    /// Token验证请求数据模型
    /// </summary>
    [Serializable]
    [MessagePackObject]
    public class TokenValidationRequest : RequestBase
    {
        /// <summary>
        /// 待验证的Token
        /// </summary>
        [Key(0)]
        public TokenInfo Token { get; set; }

        /// <summary>
        /// 客户端IP地址
        /// </summary>
        [Key(1)]
        public string ClientIp { get; set; }

        /// <summary>
        /// 设备标识
        /// </summary>
        [Key(2)]
        public string DeviceId { get; set; }

        /// <summary>
        /// 附加验证数据
        /// </summary>
        [Key(3)]
        public Dictionary<string, object> ValidationData { get; set; }

        /// <summary>
        /// 创建Token验证请求
        /// </summary>
        public static TokenValidationRequest Create(TokenInfo token, string deviceId = null, string clientIp = null)
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
            return !string.IsNullOrEmpty(Token.AccessToken) && Token.AccessTokenExpiryUtc < System.DateTime.UtcNow;
        }
    }
}
