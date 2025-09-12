using System;

namespace RUINORERP.PacketSpec.Models.Requests
{
    /// <summary>
    /// 登录请求数据模型
    /// </summary>
    [Serializable]
    public class LoginRequest
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 密码（建议传输加密后的密码）
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 客户端版本号
        /// </summary>
        public string ClientVersion { get; set; }

        /// <summary>
        /// 设备标识
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 登录时间戳
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// 客户端IP地址
        /// </summary>
        public string ClientIp { get; set; }

        /// <summary>
        /// 客户端类型（Web、Desktop、Mobile等）
        /// </summary>
        public string ClientType { get; set; }

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
                ClientVersion = Protocol.ProtocolVersion.Current,
                DeviceId = deviceId ?? Guid.NewGuid().ToString(),
                LoginTime = DateTime.UtcNow,
                ClientType = clientType
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