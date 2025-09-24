using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.System;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Protocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
// using RUINORERP.Server.Network.Services; // 暂时注释，缺少ISessionService接口定义
using Microsoft.Extensions.DependencyInjection;
using RUINORERP.Server.Network.Interfaces.Services;
// using RUINORERP.Server.Network.Interfaces.Services; // 暂时注释，缺少ISessionService接口定义

namespace RUINORERP.Server.Network.Commands
{
    /// <summary>
    /// 心跳命令处理器
    /// 处理客户端发送的心跳命令，维持连接活跃状态
    /// </summary>
    [CommandHandler("HeartbeatCommandHandler", priority: 50)]
    public class HeartbeatCommandHandler : UnifiedCommandHandlerBase
    {
         private readonly ISessionService _sessionService; 

        /// <summary>
        /// 无参构造函数，以支持Activator.CreateInstance创建实例
        /// </summary>
        public HeartbeatCommandHandler() : base()
        {
             _sessionService = Program.ServiceProvider.GetRequiredService<ISessionService>(); 
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public HeartbeatCommandHandler(
             ISessionService sessionService,
            ILogger<HeartbeatCommandHandler> logger = null) : base(logger)
        {
            _sessionService = sessionService; // 暂时注释，缺少ISessionService接口定义
        }

        /// <summary>
        /// 支持的命令类型
        /// </summary>
        public override IReadOnlyList<uint> SupportedCommands => new uint[]
        {
            (uint)SystemCommands.Heartbeat
        };

        /// <summary>
        /// 处理器优先级
        /// </summary>
        public override int Priority => 50;

        /// <summary>
        /// 具体的命令处理逻辑
        /// </summary>
        protected override async Task<CommandResult> ProcessCommandAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                var commandId = command.CommandIdentifier;

                if (commandId == SystemCommands.Heartbeat)
                {
                    return await HandleHeartbeatAsync(command, cancellationToken);
                }
                else
                {
                    return CommandResult.Failure($"不支持的命令类型: {command.CommandIdentifier}", "UNSUPPORTED_COMMAND");
                }
            }
            catch (Exception ex)
            {
                LogError($"处理心跳命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"处理异常: {ex.Message}", "HANDLER_ERROR", ex);
            }
        }

        /// <summary>
        /// 处理心跳命令
        /// </summary>
        private async Task<CommandResult> HandleHeartbeatAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                LogInfo($"处理心跳命令 [会话: {command.SessionID}]");

                 if (!string.IsNullOrEmpty(command.SessionID))
                 {
                     var session = _sessionService.GetSession(command.SessionID);
                     if (session != null)
                     {
                         session.UpdateActivity();
                         _sessionService.UpdateSession(session);
                     }
                 }

                // 创建心跳响应数据
                var responseData = CreateHeartbeatResponse();

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: new { 
                        Timestamp = DateTime.UtcNow,
                        SessionId = command.SessionID,
                        Status = "Alive"
                    },
                    message: "心跳响应成功"
                );
            }
            catch (Exception ex)
            {
                LogError($"处理心跳命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"心跳处理异常: {ex.Message}", "HEARTBEAT_ERROR", ex);
            }
        }

        /// <summary>
        /// 创建心跳响应
        /// </summary>
        private OriginalData CreateHeartbeatResponse()
        {
            var responseMessage = "ALIVE";
            var data = System.Text.Encoding.UTF8.GetBytes(responseMessage);

            // 将完整的CommandId正确分解为Category和OperationCode
            uint commandId = (uint)SystemCommands.HeartbeatResponse;
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