using System;
using System.Collections.Generic;
using MessagePack;
namespace RUINORERP.PacketSpec.Models.Requests
{
    /// <summary>
    /// Token刷新请求数据模型 - 简化版
    /// 服务端自动管理Token状态，客户端无需传递Token信息
    /// </summary>
    [Serializable]
    [MessagePackObject]
    public class TokenRefreshRequest : RequestBase
    {
        /// <summary>
        /// 当前Token（已废弃，服务端自动管理）
        /// </summary>
        [Obsolete("服务端自动管理Token状态，此字段不再使用")]
        [Key(10)]
        public string Token { get; set; }

        /// <summary>
        /// 刷新令牌
        /// </summary>
        [Key(11)]
        public string RefreshToken { get; set; }

        /// <summary>
        /// 客户端IP地址
        /// </summary>
        [Key(12)]
        public string ClientIp { get; set; }



        /// <summary>
        /// 创建Token刷新请求 - 简化版
        /// </summary>
        public static TokenRefreshRequest Create(string clientIp = null)
        {
            return new TokenRefreshRequest
            {
                ClientIp = clientIp
            };
        }

 
    }
}
