using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.PacketSpec.Models
{
    #region 数据模型类

    /// <summary>
    /// 登录数据
    /// </summary>
    public class LoginData
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ClientInfo { get; set; }
    }

    /// <summary>
    /// 用户验证结果
    /// </summary>
    public class UserValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
        public UserSessionInfo UserSessionInfo { get; set; }
    }

    /// <summary>
    /// 用户会话信息
    /// </summary>
    public class UserSessionInfo
    {
        /// <summary>
        /// 数据库用户信息
        /// </summary>
        public tb_UserInfo UserInfo { get; set; }

        /// <summary>
        /// 会话ID
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// 客户端IP
        /// </summary>
        public string ClientIp { get; set; }

        /// <summary>
        /// 在线状态
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LastLoginTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 角色权限
        /// </summary>
        public List<tb_User_Role> Roles { get; set; }

        /// <summary>
        /// 部门信息
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 创建用户会话信息
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <param name="sessionId">会话ID</param>
        /// <param name="clientIp">客户端IP</param>
        /// <returns>用户会话信息对象</returns>
        public static UserSessionInfo Create(tb_UserInfo userInfo, string sessionId = null, string clientIp = null)
        {
            return new UserSessionInfo
            {
                UserInfo = userInfo,
                SessionId = sessionId,
                ClientIp = clientIp,
                IsOnline = true,
                Roles = new List<tb_User_Role>()
            };
        }
    }

    /// <summary>
    /// Token信息
    /// </summary>
    public class TokenInfo
    {
        /// <summary>
        /// 访问令牌
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 刷新令牌
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// 过期时间（秒）
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// 令牌类型
        /// </summary>
        public string TokenType { get; set; }

        public string Token { get; set; }
    }

    /// <summary>
    /// Token验证结果
    /// </summary>
    /// <summary>
    /// 令牌验证结果模型
    /// </summary>
    public class TokenValidationResult
    {
        /// <summary>
        /// 验证是否成功
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// 验证失败消息
        /// </summary>
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// Token刷新结果
    /// </summary>
    public class TokenRefreshResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
    }

    /// <summary>
    /// Token数据
    /// </summary>
    public class TokenData
    {
        public string Token { get; set; }
    }

    /// <summary>
    /// 刷新数据
    /// </summary>
    public class RefreshData
    {
        public string RefreshToken { get; set; }
        public string CurrentToken { get; set; }
    }

    #endregion
}