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
                ClientIp=GetClientIp(),
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
        
 

        ///// <summary>
        ///// 验证请求有效性
        ///// </summary>
        //public bool IsValid()
        //{
        //    switch (Action)
        //        {
        //            case AuthenticationCommands.LoginAction.Login:
        //                return !string.IsNullOrEmpty(Username) && 
        //                       !string.IsNullOrEmpty(Password) &&
        //                       Username.Length >= 3 &&
        //                       Password.Length >= 6;
        //            case AuthenticationCommands.LoginAction.RefreshToken:
        //            case AuthenticationCommands.LoginAction.ValidateToken:
        //                return Token != null && !string.IsNullOrEmpty(Token.AccessToken);
        //            case AuthenticationCommands.LoginAction.Logout:
        //            case AuthenticationCommands.LoginAction.PrepareLogin:
        //                return true; // 这些操作不需要额外验证
        //            default:
        //                return false;
        //        }
        //}

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
        /// 客户端IP地址
        /// </summary>
        public string ClientIp { get; set; }


        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns>客户端IP地址</returns>
        private static string GetClientIp()
        {
            try
            {
                // 获取本地机器的IP地址
                var hostName = System.Net.Dns.GetHostName();
                var hostEntry = System.Net.Dns.GetHostEntry(hostName);
                var ipAddress = hostEntry.AddressList
                    .FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                return ipAddress?.ToString() ?? "127.0.0.1";
            }
            catch
            {
                return "127.0.0.1";
            }
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
