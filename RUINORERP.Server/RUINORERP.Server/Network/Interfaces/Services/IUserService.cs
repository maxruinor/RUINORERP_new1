using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RUINORERP.Model;
using RUINORERP.PacketSpec.Handlers;
using RUINORERP.PacketSpec.Models;
using RUINORERP.Server.Network.Models;

namespace RUINORERP.Server.Network.Interfaces.Services
{
    /// <summary>
    /// 用户服务接口 - 定义用户认证和管理功能
    /// 提供统一的用户操作接口，支持认证、用户信息获取和用户管理
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 用户认证
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>认证结果，包含认证状态、用户信息和令牌</returns>
        /// <exception cref="ArgumentNullException">当用户名或密码为空时抛出</exception>
        /// <exception cref="SystemException">当系统错误导致认证失败时抛出</exception>
        Task<AuthenticationResult> AuthenticateAsync(string username, string password);

        /// <summary>
        /// 根据用户ID获取用户信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>用户信息，如果不存在则返回null</returns>
        /// <exception cref="ArgumentException">当用户ID为空或格式不正确时抛出</exception>
        Task<tb_UserInfo> GetUserByIdAsync(string userId);

        /// <summary>
        /// 根据用户名获取用户信息
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>用户信息，如果不存在则返回null</returns>
        /// <exception cref="ArgumentException">当用户名为空时抛出</exception>
        Task<tb_UserInfo> GetUserByUsernameAsync(string username);

        /// <summary>
        /// 创建新用户
        /// </summary>
        /// <param name="request">创建用户请求参数</param>
        /// <returns>创建的用户信息</returns>
        /// <exception cref="ArgumentNullException">当请求参数为空时抛出</exception>
        /// <exception cref="InvalidOperationException">当用户名已存在时抛出</exception>
        Task<tb_UserInfo> CreateUserAsync(CreateUserRequest request);

 

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>删除操作是否成功</returns>
        /// <exception cref="ArgumentException">当用户ID为空或格式不正确时抛出</exception>
        Task<bool> DeleteUserAsync(string userId);

        /// <summary>
        /// 获取用户角色列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>用户角色列表</returns>
        /// <exception cref="ArgumentException">当用户ID为空或格式不正确时抛出</exception>
        Task<List<string>> GetUserRolesAsync(string userId);

        /// <summary>
        /// 获取用户权限列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>用户权限列表</returns>
        /// <exception cref="ArgumentException">当用户ID为空或格式不正确时抛出</exception>
        Task<List<string>> GetUserPermissionsAsync(string userId);

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="newPassword">新密码</param>
        /// <returns>重置操作是否成功</returns>
        /// <exception cref="ArgumentException">当用户ID或新密码为空时抛出</exception>
        Task<bool> ResetPasswordAsync(string userId, string newPassword);

        /// <summary>
        /// 锁定用户账户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>锁定操作是否成功</returns>
        /// <exception cref="ArgumentException">当用户ID为空或格式不正确时抛出</exception>
        Task<bool> LockUserAsync(string userId);

        /// <summary>
        /// 解锁用户账户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>解锁操作是否成功</returns>
        /// <exception cref="ArgumentException">当用户ID为空或格式不正确时抛出</exception>
        Task<bool> UnlockUserAsync(string userId);

        /// <summary>
        /// 验证用户令牌
        /// </summary>
        /// <param name="token">认证令牌</param>
        /// <returns>令牌验证结果</returns>
        /// <exception cref="ArgumentException">当令牌为空时抛出</exception>
        Task<TokenValidationResult> ValidateTokenAsync(string token);

        /// <summary>
        /// 刷新用户令牌
        /// </summary>
        /// <param name="refreshToken">刷新令牌</param>
        /// <returns>新的认证令牌</returns>
        /// <exception cref="ArgumentException">当刷新令牌为空时抛出</exception>
        Task<string> RefreshTokenAsync(string refreshToken);

        /// <summary>
        /// 强制用户下线
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>强制下线操作是否成功</returns>
        /// <exception cref="ArgumentException">当用户ID为空或格式不正确时抛出</exception>
        Task<bool> ForceLogoutAsync(string userId);

        /// <summary>
        /// 获取在线用户列表
        /// </summary>
        /// <returns>在线用户信息列表</returns>
        Task<List<OnlineUserInfo>> GetOnlineUsersAsync();

        /// <summary>
        /// 发送消息给指定用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="message">消息内容</param>
        /// <returns>发送操作是否成功</returns>
        /// <exception cref="ArgumentException">当用户ID或消息内容为空时抛出</exception>
        Task<bool> SendMessageToUserAsync(string userId, string message);

        /// <summary>
        /// 广播消息给所有在线用户
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <returns>广播操作是否成功</returns>
        /// <exception cref="ArgumentException">当消息内容为空时抛出</exception>
        Task<bool> BroadcastMessageAsync(string message);
    }

    /// <summary>
    /// 认证结果模型
    /// </summary>
    public class AuthenticationResult
    {
        /// <summary>
        /// 认证是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 认证结果消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 认证成功的用户信息
        /// </summary>
        public tb_UserInfo User { get; set; }

        /// <summary>
        /// 认证令牌
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 用户角色列表
        /// </summary>
        public List<string> Roles { get; set; }

        /// <summary>
        /// 用户权限列表
        /// </summary>
        public List<string> Permissions { get; set; }
    }

    /// <summary>
    /// 创建用户请求模型
    /// </summary>
    public class CreateUserRequest
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public string DepartmentId { get; set; }

        /// <summary>
        /// 角色ID列表
        /// </summary>
        public List<string> RoleIds { get; set; }
    }

    

    /// <summary>
    /// 在线用户信息模型
    /// </summary>
    public class OnlineUserInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// 最后活动时间
        /// </summary>
        public DateTime LastActivityTime { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// 客户端信息
        /// </summary>
        public string ClientInfo { get; set; }

        /// <summary>
        /// 会话ID
        /// </summary>
        public string SessionId { get; set; }
    }
}