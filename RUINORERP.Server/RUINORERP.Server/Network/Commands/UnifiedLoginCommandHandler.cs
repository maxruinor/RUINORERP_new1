using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Communication;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.Server.Network.Communication.Handlers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Server.Network.Commands;
using System.Text.Json;
using RUINORERP.PacketSpec.Communication.Handlers;
using RUINORERP.PacketSpec.Protocol;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Commands.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace RUINORERP.Server.Network.Commands
{
    /// <summary>
    /// 统一登录命令处理器
    /// 使用通用请求处理框架处理登录命令
    /// </summary>
    [CommandHandler("UnifiedLoginCommandHandler", priority: 100)]
    public class UnifiedLoginCommandHandler : UnifiedCommandHandlerBase
    {
        private readonly ServerLoginRequestHandler _loginRequestHandler;

        /// <summary>
        /// 无参构造函数，以支持Activator.CreateInstance创建实例
        /// </summary>
        public UnifiedLoginCommandHandler() : base(new LoggerFactory().CreateLogger<UnifiedCommandHandlerBase>())
        {
            _loginRequestHandler = Program.ServiceProvider.GetRequiredService<ServerLoginRequestHandler>();
        }

        public UnifiedLoginCommandHandler(
            ServerLoginRequestHandler loginRequestHandler,
            ILogger<UnifiedLoginCommandHandler> logger = null) : base(logger)
        {
            _loginRequestHandler = loginRequestHandler;
        }

        /// <summary>
        /// 支持的命令类型
        /// </summary>
        public override IReadOnlyList<uint> SupportedCommands => new uint[]
        {
            (uint)AuthenticationCommands.Login,
            (uint)AuthenticationCommands.LoginRequest,
            (uint)AuthenticationCommands.PrepareLogin
        };

        /// <summary>
        /// 处理器优先级
        /// </summary>
        public override int Priority => 100;

        /// <summary>
        /// 具体的命令处理逻辑
        /// </summary>
        protected override async Task<CommandResult> ProcessCommandAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                var commandId = command.CommandIdentifier;

                if (commandId == AuthenticationCommands.Login || commandId == AuthenticationCommands.LoginRequest)
                {
                    return await HandleLoginAsync(command, cancellationToken);
                }
                else if (commandId == AuthenticationCommands.PrepareLogin)
                {
                    return await HandlePrepareLoginAsync(command, cancellationToken);
                }
                else
                {
                    return CommandResult.Failure($"不支持的命令类型: {command.CommandIdentifier}", "UNSUPPORTED_COMMAND");
                }
            }
            catch (Exception ex)
            {
                LogError($"处理登录命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"处理异常: {ex.Message}", "HANDLER_ERROR", ex);
            }
        }

        /// <summary>
        /// 处理登录命令
        /// </summary>
        private async Task<CommandResult> HandleLoginAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                // 从命令数据中解析登录请求
                var loginRequest = ParseLoginRequest(command.OriginalData);
                if (loginRequest == null)
                {
                    return CommandResult.Failure("登录数据格式错误", "INVALID_LOGIN_DATA");
                }

                // 使用通用请求处理器处理登录请求
                var response = await _loginRequestHandler.HandleAsync(loginRequest, cancellationToken);

                if (response.IsSuccess())
                {
                    // 创建成功响应数据
                    var responseData = CreateLoginSuccessResponse(response.Data);
                    
                    return CommandResult.SuccessWithResponse(
                        responseData,
                        data: response.Data,
                        message: response.Message
                    );
                }
                else
                {
                    return CommandResult.Failure(response.Message, "LOGIN_FAILED");
                }
            }
            catch (Exception ex)
            {
                LogError($"处理登录请求异常: {ex.Message}", ex);
                return CommandResult.Failure($"登录处理异常: {ex.Message}", "LOGIN_ERROR", ex);
            }
        }

        /// <summary>
        /// 处理准备登录命令
        /// </summary>
        private async Task<CommandResult> HandlePrepareLoginAsync(ICommand command, CancellationToken cancellationToken)
        {
            await Task.Delay(10, cancellationToken);

            var responseData = CreatePrepareLoginResponse();

            return CommandResult.SuccessWithResponse(
                responseData,
                data: new { Status = "Ready" },
                message: "准备登录完成"
            );
        }

        /// <summary>
        /// 解析登录请求数据
        /// </summary>
        private LoginRequest ParseLoginRequest(OriginalData originalData)
        {
            try
            {
                if (originalData.One == null || originalData.One.Length == 0)
                    return null;

                var dataString = System.Text.Encoding.UTF8.GetString(originalData.One);
                return JsonSerializer.Deserialize<LoginRequest>(dataString);
            }
            catch (Exception ex)
            {
                LogError($"解析登录数据异常: {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// 创建登录成功响应
        /// </summary>
        private OriginalData CreateLoginSuccessResponse(LoginResult loginResult)
        {
            var responseData = JsonSerializer.Serialize(loginResult);
            var data = System.Text.Encoding.UTF8.GetBytes(responseData);

            // 将完整的CommandId正确分解为Category和OperationCode
            uint commandId = (uint)AuthenticationCommands.LoginResponse;
            byte category = (byte)(commandId & 0xFF); // 取低8位作为Category
            byte operationCode = (byte)((commandId >> 8) & 0xFF); // 取次低8位作为OperationCode

            return new OriginalData(
                category,
                new byte[] { operationCode },
                data
            );
        }

        /// <summary>
        /// 创建准备登录响应
        /// </summary>
        private OriginalData CreatePrepareLoginResponse()
        {
            var responseMessage = "READY";
            var data = System.Text.Encoding.UTF8.GetBytes(responseMessage);

            // 将完整的CommandId正确分解为Category和OperationCode
            uint commandId = (uint)AuthenticationCommands.PrepareLogin;
            byte category = (byte)(commandId & 0xFF); // 取低8位作为Category
            byte operationCode = (byte)((commandId >> 8) & 0xFF); // 取次低8位作为OperationCode

            return new OriginalData(
                category,
                new byte[] { operationCode },
                data
            );
        }
    }
}