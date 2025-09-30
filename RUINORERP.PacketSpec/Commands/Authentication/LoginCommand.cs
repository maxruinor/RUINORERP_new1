using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Enums.Core;
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    /// <summary>
    /// 登录命令
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
        public LoginCommand()
        {
            Priority = CommandPriority.High; // 登录命令优先级高
            TimeoutMs = 30000; // 登录超时时间30秒
            Direction = PacketDirection.ClientToServer;
            CommandIdentifier = AuthenticationCommands.Login;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="clientInfo">客户端信息</param>
        public LoginCommand(string username, string password, string clientInfo = null)
        {
            Request = LoginRequest.Create(username, password, clientInfo);
            // ClientIp将在服务器端命令处理器中设置，这里不硬编码
            Priority = CommandPriority.High; // 登录命令优先级高
            TimeoutMs = 30000; // 登录超时时间30秒
            Direction = PacketDirection.ClientToServer;
            CommandIdentifier = AuthenticationCommands.Login;
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
                // 验证请求数据
                if (Request == null)
                {
                    return Task.FromResult(ResponseBase.CreateError("请求数据不能为空", 400)
                        .WithMetadata("ErrorCode", "INVALID_REQUEST"));
                }

                if (!Request.IsValid())
                {
                    return Task.FromResult(ResponseBase.CreateError("登录请求数据无效", 400).WithMetadata("ErrorCode", "INVALID_LOGIN_REQUEST"));
                }

                // 构建登录数据
                var loginData = GetSerializableData();

                // 返回成功结果，实际的网络请求由通信服务处理
                var result = new ResponseBase
                {
                    IsSuccess = true,
                    Message = "登录命令构建成功",
                    Code = 200,
                    TimestampUtc = DateTime.UtcNow
                };
                result.WithMetadata("Data", loginData);
                return Task.FromResult((ResponseBase)result);
            }
            catch (Exception ex)
            {
                return Task.FromResult((ResponseBase)ResponseBase.CreateError($"登录命令执行异常: {ex.Message}", 500)
                    .WithMetadata("ErrorCode", "LOGIN_EXCEPTION")
                    .WithMetadata("Exception", ex.Message)
                    .WithMetadata("StackTrace", ex.StackTrace));
            }
        }

        /// <summary>
        /// 验证命令数据
        /// 只做"参数级"校验，不做业务校验
        /// </summary>
        /// <returns>验证结果</returns>
        public override async Task<ValidationResult> ValidateAsync(CancellationToken cancellationToken = default)
        {
            // 调用基类验证方法，将使用独立的验证器类进行验证
            var result = await base.ValidateAsync(cancellationToken);
            return result;
        }
    }
}
