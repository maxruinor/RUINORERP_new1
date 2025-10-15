using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Enums.Core;
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using RUINORERP.PacketSpec.Core;
using MessagePack;

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    /// <summary>
    /// 登录命令
    /// 提供用户身份验证功能，是获取访问令牌的入口点
    /// </summary>
    [PacketCommand("Login", CommandCategory.Authentication)]
    [MessagePackObject(AllowPrivate = true)]
    public class LoginCommand : BaseCommand<LoginRequest, LoginResponse>
    {
        /// <summary>
        /// 登录请求数据
        /// </summary>
        [Key(1000)]
        public LoginRequest LoginRequest
        {
            get => Request;
            set => Request = value;
        }
        [Key(1001)]
        public LoginResponse LoginResponse
        {
            get => Response;
            set => Response = value;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public LoginCommand() : base() // 移除 Direction 参数
        {
            CommandIdentifier = AuthenticationCommands.Login;
            // 注意：移除了 TimeoutMs 的设置，因为指令本身不应该关心超时
            // 超时应该是执行环境的问题，由网络层或业务处理层处理
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="clientInfo">客户端信息</param>
        public LoginCommand(string username, string password, string clientInfo = null)
            : this()
        {
            Request = LoginRequest.Create(username, password, clientInfo);
            // RequestId 由 BaseCommand 基类统一赋值，不再重复 
        }
        
        /// <summary>
        /// 构造函数（用于内部会话处理）
        /// </summary>
        /// <param name="session">会话对象</param>
        public LoginCommand(object session) : this()
        {
            // 仅用于兼容性，在内部处理会话相关逻辑
        }

        /// <summary>
        /// 验证命令数据
        /// 包含用户名和密码的特定验证逻辑
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>验证结果</returns>
        public override async Task<ValidationResult> ValidateAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.ValidateAsync(cancellationToken);
            
            // 添加登录特定的验证
            if (Request != null)
            {
                if (string.IsNullOrWhiteSpace(Request.Username))
                    result.Errors.Add(new ValidationFailure(nameof(Request.Username), "用户名不能为空"));
                     
                if (string.IsNullOrWhiteSpace(Request.Password))
                    result.Errors.Add(new ValidationFailure(nameof(Request.Password), "密码不能为空"));
            }
            
            return result;
        }
    }
}
