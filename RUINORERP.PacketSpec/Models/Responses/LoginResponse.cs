﻿using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Core;
using System;
using System.Collections.Generic;
using System.Text;
namespace RUINORERP.PacketSpec.Models.Responses
{
    /// <summary>
    /// 登录响应模型
    /// </summary>
    [Serializable]
    
    public class LoginResponse : ResponseBase, IResponse
    {
        public LoginResponse()
        {
            
        }
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
        /// Token信息
        /// </summary>
        public TokenInfo Token { get; set; }

        /// <summary>
        /// 用户角色列表
        /// </summary>
        public List<string> Roles { get; set; } = new List<string>();

        /// <summary>
        /// 用户权限列表
        /// </summary>
        public List<string> Permissions { get; set; } = new List<string>();
    }
}
