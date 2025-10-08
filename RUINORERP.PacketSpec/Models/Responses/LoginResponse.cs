using RUINORERP.PacketSpec.Commands.Authentication;
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
        /// Token信息
        /// </summary>
        [Key(14)]
        public TokenInfo Token { get; set; }

        /// <summary>
        /// 获取访问令牌（通过TokenInfo实例获取）
        /// </summary>
        [IgnoreMember]
        public string AccessToken => Token?.AccessToken;

        /// <summary>
        /// 获取刷新令牌（通过TokenInfo实例获取）
        /// </summary>
        [IgnoreMember]
        public string RefreshToken => Token?.RefreshToken;

        /// <summary>
        /// 获取过期时间（秒，通过TokenInfo实例获取）
        /// </summary>
        [IgnoreMember]
        public int ExpiresIn => Token?.ExpiresIn ?? 0;

        /// <summary>
        /// 获取令牌类型（通过TokenInfo实例获取）
        /// </summary>
        [IgnoreMember]
        public string TokenType => Token?.TokenType;

        /// <summary>
        /// 用户角色列表
        /// </summary>
        [Key(15)]
        public List<string> Roles { get; set; } = new List<string>();

        /// <summary>
        /// 用户权限列表
        /// </summary>
        [Key(16)]
        public List<string> Permissions { get; set; } = new List<string>();
    }
}