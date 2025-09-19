using System.ComponentModel;

namespace RUINORERP.PacketSpec.Enums.Auth
{
    /// <summary>
    /// 认证相关命令枚举
    /// </summary>
    public enum AuthenticationCommand : uint
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        [Description("用户登录")]
        Login = 0x0100,

        /// <summary>
        /// 用户登出
        /// </summary>
        [Description("用户登出")]
        Logout = 0x0101,

        /// <summary>
        /// 验证Token
        /// </summary>
        [Description("验证Token")]
        ValidateToken = 0x0102,

        /// <summary>
        /// 刷新Token
        /// </summary>
        [Description("刷新Token")]
        RefreshToken = 0x0103,

        /// <summary>
        /// 准备登录
        /// </summary>
        [Description("准备登录")]
        PrepareLogin = 0x0104,

        /// <summary>
        /// 用户登录请求
        /// </summary>
        [Description("用户登录请求")]
        LoginRequest = 0x0105,

        /// <summary>
        /// 登录回复
        /// </summary>
        [Description("登录回复")]
        LoginResponse = 0x0106,

        /// <summary>
        /// 登录验证
        /// </summary>
        [Description("登录验证")]
        LoginValidation = 0x0107,

        /// <summary>
        /// 重复登录通知
        /// </summary>
        [Description("重复登录通知")]
        DuplicateLoginNotification = 0x0108,

        /// <summary>
        /// 强制用户下线
        /// </summary>
        [Description("强制用户下线")]
        ForceLogout = 0x0109,

        /// <summary>
        /// 强制登录上线
        /// </summary>
        [Description("强制登录上线")]
        ForceLogin = 0x010A,

        /// <summary>
        /// 用户状态同步
        /// </summary>
        [Description("用户状态同步")]
        UserStatusSync = 0x010B,

        /// <summary>
        /// 在线用户列表
        /// </summary>
        [Description("在线用户列表")]
        OnlineUserList = 0x010C,

        /// <summary>
        /// 用户信息查询
        /// </summary>
        [Description("用户信息查询")]
        UserInfo = 0x010D,

        /// <summary>
        /// 用户列表
        /// </summary>
        [Description("用户列表")]
        UserList = 0x010E,

        /// <summary>
        /// 在线用户
        /// </summary>
        [Description("在线用户")]
        OnlineUsers = 0x010F
    }
}