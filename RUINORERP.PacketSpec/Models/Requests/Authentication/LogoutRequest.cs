using RUINORERP.PacketSpec.Models.Requests;
using System;

namespace RUINORERP.PacketSpec.Models.Requests.Authentication
{
    /// <summary>
    /// 登出请求 - 用于用户退出系统
    /// </summary>
    
    public class LogoutRequest : RequestBase
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 登出原因
        /// </summary>
        public string LogoutReason { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public LogoutRequest()
        {}

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="deviceId">设备ID</param>
        /// <param name="logoutReason">登出原因</param>
        public LogoutRequest(long userId, string deviceId, string logoutReason = "用户主动登出")
        {
            UserId = userId;
            DeviceId = deviceId;
            LogoutReason = logoutReason;
        }
    }
}
