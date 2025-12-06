using RUINORERP.PacketSpec.Models.Authentication;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Collections.Generic;

namespace RUINORERP.PacketSpec.Models.Authentication
{
   

    /// <summary>
    /// 重复登录处理选项
    /// </summary>
    public enum DuplicateLoginAction
    {
        /// <summary>
        /// 取消登录
        /// </summary>
        Cancel,
        /// <summary>
        /// 强制对方下线
        /// </summary>
        ForceOfflineOthers,
    }

    /// <summary>
    /// 重复登录信息
    /// </summary>
    [Serializable]
    public class DuplicateLoginInfo
    {
        /// <summary>
        /// 是否存在重复登录
        /// </summary>
        public bool HasDuplicateLogin { get; set; }

        /// <summary>
        /// 现有会话信息
        /// </summary>
        public List<ExistingSessionInfo> ExistingSessions { get; set; } = new List<ExistingSessionInfo>();

        /// <summary>
        /// 重复登录消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 当前会话ID
        /// </summary>
        public string CurrentSessionId { get; set; }

        /// <summary>
        /// 是否为本地重复登录（同一台机器）
        /// </summary>
        public bool IsLocalDuplicate { get; set; }
    }

    /// <summary>
    /// 现有会话信息
    /// </summary>
    [Serializable]
    public class ExistingSessionInfo
    {
        /// <summary>
        /// 会话ID
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// 客户端IP
        /// </summary>
        public string ClientIp { get; set; }

        /// <summary>
        /// 设备信息
        /// </summary>
        public string DeviceInfo { get; set; }

        /// <summary>
        /// 是否为本地会话
        /// </summary>
        public bool IsLocal { get; set; }

        /// <summary>
        /// 会话状态描述
        /// </summary>
        public string StatusDescription => IsLocal ? "本机登录" : $"远程登录 ({ClientIp})";
    }

 

    
}
