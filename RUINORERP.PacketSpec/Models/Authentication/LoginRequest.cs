using System;
using System.Collections.Generic;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Models.Common;
using System.Linq;
using RUINORERP.PacketSpec.Models.Core;
namespace RUINORERP.PacketSpec.Models.Requests
{
    /// <summary>
    /// 登录请求数据模型
    /// </summary>
    [Serializable]
    
    public class LoginRequest : RequestBase
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
        /// 附加数据，用于传递额外信息，如重复登录确认等
        /// </summary>
        public Dictionary<string, object> AdditionalData { get; set; }

        /// <summary>
        /// 客户端Token信息（用于Token刷新场景）
        /// </summary>
        public TokenInfo Token { get; set; }

        /// <summary>
        /// 会话ID
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// 创建登录请求
        /// </summary>
        public static LoginRequest Create(string username, string password, 
            string clientType = "Desktop")
        {
            return new LoginRequest
            {
                Username = username,
                Password = password,
                DeviceId= GetDeviceId(),
                ClientVersion = ProtocolVersion.Current,
                LoginTime = DateTime.Now,
                // ✅ 优化：移除客户端自报IP，由服务器端从Socket获取真实IP
                // ClientIp字段保留用于兼容性，但不再主动赋值
            };
        }
        
        /// <summary>
        /// 创建登出请求
        /// </summary>
        public static LoginRequest CreateLogoutRequest()
        {
            return new LoginRequest
            {
                LoginTime = DateTime.Now,
                DeviceId = GetDeviceId()
            };
        }
        
 

        /// <summary>
        /// 安全清理敏感信息
        /// </summary>
        public void ClearSensitiveData()
        {
            Password = null;
        }

        /// <summary>
        /// 获取设备标识
        /// </summary>
        /// <returns>设备标识</returns>
        private static string GetDeviceId()
        {
            try
            {
                // 使用机器名和用户名组合生成设备标识
                var machineName = Environment.MachineName;
                var userName = Environment.UserName;
                return $"{machineName}_{userName}";
            }
            catch
            {
                // 如果获取失败，使用GUID
                return Guid.NewGuid().ToString();
            }
        }

        /// <summary>
        /// 客户端IP地址（仅用于接收服务器端设置的值，不再由客户端主动上报）
        /// </summary>
        public string ClientIp { get; set; }

        // ✅ 优化：删除GetClientIp()方法，避免客户端自报不可靠的IP地址
        // 原方法通过Dns.GetHostEntry()获取的是客户端本地网卡IP，容易被伪造且在NAT环境下错误
        // 现在统一由服务器端从Socket RemoteEndPoint获取真实IP

        public bool IsValid()
        {
            return true;
        }
    }
}
