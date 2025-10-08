using System.ComponentModel;

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    /// <summary>
    /// 认证相关命令
    /// </summary>
    public static class AuthenticationCommands
    {
        #region 认证命令 (0x01xx)
        /// <summary>
        /// 用户登录 - 客户端向服务器发起登录请求
        /// </summary>
        public static readonly CommandId Login = new CommandId(CommandCategory.Authentication, (byte)(CommandCatalog.Authentication_Login & 0xFF));
        
        /// <summary>
        /// 用户登出 - 用户主动或被动退出系统
        /// </summary>
        public static readonly CommandId Logout = new CommandId(CommandCategory.Authentication, (byte)(CommandCatalog.Authentication_Logout & 0xFF));

        /// <summary>
        /// 验证Token - 验证用户身份令牌的有效性
        /// </summary>
        public static readonly CommandId ValidateToken = new CommandId(CommandCategory.Authentication, (byte)(CommandCatalog.Authentication_ValidateToken & 0xFF));

        /// <summary>
        /// 刷新Token - 更新用户身份令牌
        /// </summary>
        public static readonly CommandId RefreshToken = new CommandId(CommandCategory.Authentication, (byte)(CommandCatalog.Authentication_RefreshToken & 0xFF));

        /// <summary>
        /// 准备登录 - 登录前的准备工作
        /// </summary>
        public static readonly CommandId PrepareLogin = new CommandId(CommandCategory.Authentication, (byte)(CommandCatalog.Authentication_PrepareLogin & 0xFF));

        /// <summary>
        /// 用户登录请求 - 客户端发起的登录请求
        /// </summary>
        public static readonly CommandId LoginRequest = new CommandId(CommandCategory.Authentication, (byte)(CommandCatalog.Authentication_LoginRequest & 0xFF));

        /// <summary>
        /// 登录回复 - 服务器对登录请求的响应
        /// </summary>
        public static readonly CommandId LoginResponse = new CommandId(CommandCategory.Authentication, (byte)(CommandCatalog.Authentication_LoginResponse & 0xFF));

        /// <summary>
        /// 登录验证 - 服务器验证用户登录信息
        /// </summary>
        public static readonly CommandId LoginValidation = new CommandId(CommandCategory.Authentication, (byte)(CommandCatalog.Authentication_LoginValidation & 0xFF));

        /// <summary>
        /// 重复登录通知 - 通知用户账号在其他地方登录
        /// </summary>
        public static readonly CommandId DuplicateLoginNotification = new CommandId(CommandCategory.Authentication, (byte)(CommandCatalog.Authentication_DuplicateLoginNotification & 0xFF));

        /// <summary>
        /// 强制用户下线 - 管理员强制用户退出系统
        /// </summary>
        public static readonly CommandId ForceLogout = new CommandId(CommandCategory.Authentication, (byte)(CommandCatalog.Authentication_ForceLogout & 0xFF));

        /// <summary>
        /// 强制登录上线 - 强制用户登录到系统
        /// </summary>
        public static readonly CommandId ForceLogin = new CommandId(CommandCategory.Authentication, (byte)(CommandCatalog.Authentication_ForceLogin & 0xFF));

        /// <summary>
        /// 用户状态同步 - 同步用户在线状态信息
        /// </summary>
        public static readonly CommandId UserStatusSync = new CommandId(CommandCategory.Authentication, (byte)(CommandCatalog.Authentication_UserStatusSync & 0xFF));

        /// <summary>
        /// 在线用户列表 - 获取当前在线用户列表
        /// </summary>
        public static readonly CommandId OnlineUserList = new CommandId(CommandCategory.Authentication, (byte)(CommandCatalog.Authentication_OnlineUserList & 0xFF));

        /// <summary>
        /// 用户信息查询 - 查询特定用户信息
        /// </summary>
        public static readonly CommandId UserInfo = new CommandId(CommandCategory.Authentication, (byte)(CommandCatalog.Authentication_UserInfo & 0xFF));

        /// <summary>
        /// 用户列表 - 获取系统用户列表
        /// </summary>
        public static readonly CommandId UserList = new CommandId(CommandCategory.Authentication, (byte)(CommandCatalog.Authentication_UserList & 0xFF));

        /// <summary>
        /// 在线用户 - 获取在线用户信息
        /// </summary>
        public static readonly CommandId OnlineUsers = new CommandId(CommandCategory.Authentication, (byte)(CommandCatalog.Authentication_OnlineUsers & 0xFF));

        public static readonly CommandId Connected = new CommandId(CommandCategory.Authentication, (byte)(CommandCatalog.Authentication_Connected & 0xFF));

        #endregion
    }
}
