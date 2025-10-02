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

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    /// <summary>
    /// 登录命令
    /// 提供用户身份验证功能，是获取访问令牌的入口点
    /// </summary>
    [PacketCommand("Login", CommandCategory.Authentication)]
    public class LoginCommand : BaseCommand<LoginRequest, LoginResponse>
    {
        /// <summary>
        /// 登录请求数据
        /// </summary>
        public LoginRequest LoginRequest
        {
            get => Request;
            set => Request = value;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public LoginCommand() : base(PacketDirection.ClientToServer)
        {
            CommandIdentifier = AuthenticationCommands.Login;
            Priority = CommandPriority.High;
            TimeoutMs = 30000;
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
            Request.RequestId = IdGenerator.GenerateRequestId(CommandIdentifier.Name);
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
        /// 命令执行的具体逻辑
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>执行结果</returns>
        protected override Task<ResponseBase> OnExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (Request == null)
                {
                    return Task.FromResult(ResponseBase.CreateError("登录请求数据不能为空", 400)
                        .WithMetadata("ErrorCode", "INVALID_REQUEST"));
                }

                if (!Request.IsValid())
                {
                    return Task.FromResult(ResponseBase.CreateError("登录请求数据无效", 400)
                        .WithMetadata("ErrorCode", "INVALID_LOGIN_REQUEST"));
                }

                // 自动处理Token（通过基类的AutoAttachToken）
                var result = ResponseBase.CreateSuccess("登录命令构建成功");
                result.WithMetadata("Username", Request.Username);
                result.WithMetadata("ClientInfo", Request.ClientInfo);
                
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                return Task.FromResult(ResponseBase.CreateError($"登录命令执行异常: {ex.Message}", 500)
                    .WithMetadata("ErrorCode", "LOGIN_EXCEPTION"));
            }
        }

        /// <summary>
        /// 自动附加认证Token
        /// 登录命令不需要自动附加Token，因为这是获取Token的过程
        /// </summary>
        protected override void AutoAttachToken()
        {
            // 登录命令不需要自动附加Token，因为这是获取Token的过程
            // 重写基类方法，空实现
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
