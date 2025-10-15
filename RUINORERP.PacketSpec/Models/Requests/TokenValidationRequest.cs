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
        [Key(10)]
        public TokenInfo Token { get; set; }


        /// <summary>
        /// 附加验证数据
        /// </summary>
        [Key(13)]
        [MessagePack.IgnoreMember]
        public Dictionary<string, object> ValidationData { get; set; }

        /// <summary>
        /// 创建Token验证请求
        /// </summary>
        public static TokenValidationRequest Create(TokenInfo token)
        {
            return new TokenValidationRequest
            {
                Token = token,
                ValidationData = new Dictionary<string, object>()
            };
        }

        /// <summary>
        /// 验证请求有效性
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Token.AccessToken) && Token.ExpiresAt > DateTime.Now;
        }
    }
}
