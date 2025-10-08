using System;
using System.Collections.Generic;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Models.Core;
using MessagePack;
namespace RUINORERP.PacketSpec.Models.Requests
{
    /// <summary>
    /// 登录请求数据模型
    /// </summary>
    [Serializable]
    [MessagePackObject]
    public class LoginRequest : RequestBase
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Key(0)]
        public string Username { get; set; }

        /// <summary>
        /// 密码（建议传输加密后的密码）
        /// </summary>
        [Key(1)]
        public string Password { get; set; }

        /// <summary>
        /// 客户端版本号
        /// </summary>
        [Key(2)]
        public string ClientVersion { get; set; }

        /// <summary>
        /// 设备标识
        /// </summary>
        [Key(3)]
        public string DeviceId { get; set; }

        /// <summary>
        /// 登录时间戳
        /// </summary>
        [Key(4)]
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// 附加数据，用于传递额外信息，如重复登录确认等
        /// </summary>
        [Key(5)]
        public Dictionary<string, object> AdditionalData { get; set; }

        /// <summary>
        /// 客户端Token（用于Token刷新场景）
        /// </summary>
        [Key(6)]
        public TokenInfo Token { get; set; }

        /// <summary>
        /// 刷新Token（用于Token刷新场景）
        /// </summary>
        [Key(7)]
        public string RefreshToken { get; set; }

        /// <summary>
        /// 创建登录请求
        /// </summary>
        public static LoginRequest Create(string username, string password, 
            string deviceId = null, string clientType = "Desktop")
        {
            return new LoginRequest
            {
                Username = username,
                Password = password,
                ClientVersion = ProtocolVersion.Current,
                LoginTime = DateTime.UtcNow,
            };
        }

        /// <summary>
        /// 验证请求有效性
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Username) && 
                   !string.IsNullOrEmpty(Password) &&
                   Username.Length >= 3 &&
                   Password.Length >= 6;
        }

        /// <summary>
        /// 安全清理敏感信息
        /// </summary>
        public void ClearSensitiveData()
        {
            Password = null;
        }
    }
}
