﻿using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Enums.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    /// <summary>
    /// 登录命令 - 客户端向服务器发起登录请求
    /// </summary>
    [PacketCommand("Login", CommandCategory.Authentication)]
    public class LoginCommand : BaseCommand
    {
        /// <summary>
        /// 命令标识符
        /// </summary>
        public override CommandId CommandIdentifier => AuthenticationCommands.Login;

        /// <summary>
        /// 登录请求数据
        /// </summary>
        public LoginRequest LoginRequest { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public LoginCommand()
        {
            Priority = PacketPriority .High; // 登录命令优先级高
            TimeoutMs = 30000; // 登录超时时间30秒
            Direction = PacketDirection.ClientToServer;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="clientInfo">客户端信息</param>
        public LoginCommand(string username, string password, string clientInfo = null)
        {
            LoginRequest = LoginRequest.Create(username, password, clientInfo);
            // ClientIp将在服务器端命令处理器中设置，这里不硬编码

            Priority = PacketPriority .High; // 登录命令优先级高
            TimeoutMs = 30000; // 登录超时时间30秒
            Direction = PacketDirection.ClientToServer;
        }

        /// <summary>
        /// 获取可序列化的数据
        /// </summary>
        /// <returns>可序列化的登录数据</returns>
        public override object GetSerializableData()
        {
            return LoginRequest;
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
                // 登录命令契约只定义数据结构，实际的业务逻辑在Handler中实现
                // 这里只做基本的数据验证

                // 验证登录请求数据
                if (LoginRequest == null)
                {
                    return Task.FromResult(ResponseBase.CreateError("登录请求数据不能为空", 400).WithMetadata("ErrorCode", "EMPTY_LOGIN_REQUEST"));
                }

                if (!LoginRequest.IsValid())
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
        /// 只做“参数级”校验，不做业务校验
        /// </summary>
        /// <returns>验证结果</returns>
        public override CommandValidationResult Validate()
        {
            // 调用基类验证
            var baseResult = base.Validate();
            if (!baseResult.IsValid)
            {
                return baseResult;
            }
            if (this.Packet != null)
            {
                LoginRequest = this.Packet.GetJsonData<LoginRequest>();
            }
            // 验证登录请求数据
            if (LoginRequest == null)
            {
                return CommandValidationResult.Failure("登录请求数据不能为空");
            }

            if (!LoginRequest.IsValid())
            {
                return CommandValidationResult.Failure("登录请求数据无效");
            }

            return CommandValidationResult.Success();
        }
    }
}
