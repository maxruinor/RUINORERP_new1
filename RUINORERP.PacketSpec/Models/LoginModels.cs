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
        public UserInfo UserInfo { get; set; }
    }


    /// <summary>
    /// 用户信息 网络会话层？
    /// </summary>
    public class UserInfo
    {
 
 
        public bool IsAdmin { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

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
        /// 创建用户信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="username">用户名</param>
        /// <param name="sessionId">会话ID</param>
        /// <param name="clientIp">客户端IP</param>
        /// <returns>用户信息对象</returns>
        public static UserInfo Create(long userId, string username, string sessionId = null, string clientIp = null)
        {
            return new UserInfo
            {
                UserId = userId,
                Username = username,
                SessionId = sessionId,
                ClientIp = clientIp,
                IsOnline = true,
                //Roles = Array.Empty<string>()
            };
        }


    }
   
    /// <summary>
    /// Token信息
    /// </summary>
    public class TokenInfo
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
        public string TokenType { get; set; }
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
