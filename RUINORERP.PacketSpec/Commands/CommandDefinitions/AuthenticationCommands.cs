using System.ComponentModel;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 认证相关命令 - 小型系统简化版，直接进行登录认证
    /// </summary>
    public static class AuthenticationCommands
    {
 
        #region 核心认证命令 (0x01xx)
        /// <summary>
        /// 用户登录 - 客户端向服务器直接发起登录请求
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
        /// 强制用户下线 - 管理员强制用户退出系统
        /// </summary>
        public static readonly CommandId ForceLogout = new CommandId(CommandCategory.Authentication, (byte)(CommandCatalog.Authentication_ForceLogout & 0xFF));
        #endregion
    }
}
