using System;

namespace RUINORERP.PacketSpec.Models.Responses
{
    /// <summary>
    /// 登录响应数据模型
    /// </summary>
    [Serializable]
    public class LoginResponse
    {
        /// <summary>
        /// 登录是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 响应消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 用户会话令牌
        /// </summary>
        public string SessionToken { get; set; }

        /// <summary>
        /// 令牌过期时间
        /// </summary>
        public DateTime TokenExpiry { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 用户显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public string[] Roles { get; set; }

        /// <summary>
        /// 服务器时间
        /// </summary>
        public DateTime ServerTime { get; set; }

        /// <summary>
        /// 是否需要强制修改密码
        /// </summary>
        public bool RequirePasswordChange { get; set; }

        /// <summary>
        /// 创建成功登录响应
        /// </summary>
        public static LoginResponse CreateSuccess(int userId, string username, 
            string sessionToken, DateTime tokenExpiry, string[] roles = null)
        {
            return new LoginResponse
            {
                Success = true,
                Message = "登录成功",
                UserId = userId,
                Username = username,
                SessionToken = sessionToken,
                TokenExpiry = tokenExpiry,
                Roles = roles ?? Array.Empty<string>(),
                ServerTime = DateTime.UtcNow
            };
        }

        /// <summary>
        /// 创建失败登录响应
        /// </summary>
        public static LoginResponse CreateFailure(string message)
        {
            return new LoginResponse
            {
                Success = false,
                Message = message,
                ServerTime = DateTime.UtcNow
            };
        }

        /// <summary>
        /// 创建需要修改密码的响应
        /// </summary>
        public static LoginResponse CreatePasswordChangeRequired(int userId, string username)
        {
            return new LoginResponse
            {
                Success = true,
                Message = "需要修改密码",
                UserId = userId,
                Username = username,
                RequirePasswordChange = true,
                ServerTime = DateTime.UtcNow
            };
        }

        /// <summary>
        /// 验证响应有效性
        /// </summary>
        public bool IsValid()
        {
            if (!Success)
                return true; // 失败响应也是有效的

            return !string.IsNullOrEmpty(SessionToken) &&
                   UserId > 0 &&
                   !string.IsNullOrEmpty(Username) &&
                   TokenExpiry > DateTime.UtcNow;
        }

        /// <summary>
        /// 检查会话是否过期
        /// </summary>
        public bool IsSessionExpired()
        {
            return DateTime.UtcNow > TokenExpiry;
        }

        /// <summary>
        /// 安全清理敏感信息
        /// </summary>
        public void ClearSensitiveData()
        {
            SessionToken = null;
        }
    }
}