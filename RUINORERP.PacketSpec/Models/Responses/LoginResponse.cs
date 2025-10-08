using RUINORERP.PacketSpec.Core;
using System;
using System.Collections.Generic;
using System.Text;
using MessagePack;
namespace RUINORERP.PacketSpec.Models.Responses
{
    /// <summary>
    /// 登录响应模型
    /// </summary>
    [Serializable]
    [MessagePackObject]
    public class LoginResponse : ResponseBase
    {
        public LoginResponse()
        {
            
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        [Key(10)]
        public long UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Key(11)]
        public string Username { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        [Key(12)]
        public string DisplayName { get; set; }

        /// <summary>
        /// 会话ID
        /// </summary>
        [Key(13)]
        public string SessionId { get; set; }

        /// <summary>
        /// 访问令牌
        /// </summary>
        [Key(14)]
        public string AccessToken { get; set; }

        /// <summary>
        /// 刷新令牌
        /// </summary>
        [Key(15)]
        public string RefreshToken { get; set; }

        /// <summary>
        /// 过期时间（秒）
        /// </summary>
        [Key(16)]
        public int ExpiresIn { get; set; }

        /// <summary>
        /// 令牌类型
        /// </summary>
        [Key(17)]
        public string TokenType { get; set; }

        /// <summary>
        /// 用户角色列表
        /// </summary>
        [Key(18)]
        public List<string> Roles { get; set; } = new List<string>();

        /// <summary>
        /// 用户权限列表
        /// </summary>
        [Key(19)]
        public List<string> Permissions { get; set; } = new List<string>();
    }
}
