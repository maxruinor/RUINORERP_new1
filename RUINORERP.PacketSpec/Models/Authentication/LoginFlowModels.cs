using RUINORERP.PacketSpec.Models.Authentication;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Collections.Generic;

namespace RUINORERP.PacketSpec.Models.Authentication
{
    /// <summary>
    /// 登录状态枚举
    /// </summary>
    public enum LoginStatus
    {
        /// <summary>
        /// 未开始
        /// </summary>
        None,
        /// <summary>
        /// 连接中
        /// </summary>
        Connecting,
        /// <summary>
        /// 验证中
        /// </summary>
        Validating,
        /// <summary>
        /// 重复登录确认中
        /// </summary>
        DuplicateLoginConfirming,
        /// <summary>
        /// 登录成功
        /// </summary>
        Success,
        /// <summary>
        /// 登录失败
        /// </summary>
        Failed,
        /// <summary>
        /// 已取消
        /// </summary>
        Cancelled
    }

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
        /// <summary>
        /// 自己下线
        /// </summary>
        OfflineSelf
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

    /// <summary>
    /// 登录流程上下文
    /// </summary>
    public class LoginFlowContext
    {
        /// <summary>
        /// 登录状态
        /// </summary>
        public LoginStatus Status { get; set; } = LoginStatus.None;

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 服务器IP
        /// </summary>
        public string ServerIp { get; set; }

        /// <summary>
        /// 服务器端口
        /// </summary>
        public int ServerPort { get; set; }

        /// <summary>
        /// 重复登录信息
        /// </summary>
        public DuplicateLoginInfo DuplicateLoginInfo { get; set; }

        /// <summary>
        /// 用户选择的重复登录处理方式
        /// </summary>
        public DuplicateLoginAction? UserSelectedAction { get; set; }

        /// <summary>
        /// 登录开始时间
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 最后错误信息
        /// </summary>
        public string LastError { get; set; }

        /// <summary>
        /// 是否为快速登录（基于有效Token）
        /// </summary>
        public bool IsQuickLogin { get; set; }

        /// <summary>
        /// 取消令牌
        /// </summary>
        public System.Threading.CancellationToken CancellationToken { get; set; }
    }

    /// <summary>
    /// 登录流程事件参数
    /// </summary>
    public class LoginFlowEventArgs : EventArgs
    {
        public LoginFlowContext Context { get; }
        public LoginStatus PreviousStatus { get; }

        public LoginFlowEventArgs(LoginFlowContext context, LoginStatus previousStatus)
        {
            Context = context;
            PreviousStatus = previousStatus;
        }
    }
}