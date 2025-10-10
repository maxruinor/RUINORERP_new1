using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.System;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
// using RUINORERP.Server.Network.Services; // 暂时注释，缺少ISessionService接口定义
using Microsoft.Extensions.DependencyInjection;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.PacketSpec.Models.Core;
// using RUINORERP.Server.Network.Interfaces.Services; // 暂时注释，缺少ISessionService接口定义

namespace RUINORERP.Server.Network.Commands
{
    /// <summary>
    /// 心跳命令处理器
    /// 处理客户端发送的心跳命令，维持连接活跃状态
    /// </summary>
    [CommandHandler("HeartbeatCommandHandler", priority: 0)]
    public class HeartbeatCommandHandler : BaseCommandHandler
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
            
            // 使用安全方法设置支持的命令
            SetSupportedCommands(SystemCommands.Heartbeat.FullCode);
        }

        /// <summary>
        /// 支持的命令类型
        /// </summary>
        public override IReadOnlyList<CommandId> SupportedCommands { get; protected set; }
   
        /// <summary>
        /// 具体的命令处理逻辑
        /// </summary>
        protected override async Task<BaseCommand<IResponse>> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                var commandId = cmd.Command.CommandIdentifier;

                if (commandId == SystemCommands.Heartbeat)
                {
                    return await HandleHeartbeatAsync(cmd.Command, cancellationToken);
                }
                else
                {
                    return BaseCommand<IResponse>.CreateError($"不支持的命令类型: {cmd.Command.CommandIdentifier}", 400)
                        .WithMetadata("ErrorCode", "UNSUPPORTED_COMMAND");
                }
            }
            catch (Exception ex)
            {
                LogError($"处理心跳命令异常: {ex.Message}", ex);
                return BaseCommand<IResponse>.CreateError($"处理异常: {ex.Message}", 500)
                    .WithMetadata("ErrorCode", "HANDLER_ERROR")
                    .WithMetadata("Exception", ex.Message)
                    .WithMetadata("StackTrace", ex.StackTrace);
            }
        }

        /// <summary>
        /// 处理心跳命令
        /// </summary>
        private async Task<BaseCommand<IResponse>> HandleHeartbeatAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                // 获取SessionId，通过反射方式访问
                string sessionId = null;
                try
                {
                    var commandWithSession = command as dynamic;
                    sessionId = commandWithSession.SessionId;
                }
                catch
                {
                    // 如果无法获取SessionId，则设置为null
                    sessionId = null;
                }

                LogInfo($"处理心跳命令 [会话: {sessionId}]");

                 if (!string.IsNullOrEmpty(sessionId))
                 {
                     var session = _sessionService.GetSession(sessionId);
                     if (session != null)
                     {
                         session.UpdateActivity();
                         _sessionService.UpdateSession(session);
                     }
                 }

                // 创建心跳响应数据
                var responseData = CreateHeartbeatResponse();

                var response = new ResponseBase
                {
                    Message = "心跳响应成功",
                    IsSuccess = true
                };
                response.WithMetadata("Data", new { 
                        Timestamp = DateTime.UtcNow,
                        SessionId = sessionId,
                        Status = "Alive"
                    });
                return BaseCommand<IResponse>.CreateSuccess(response);
            }
            catch (Exception ex)
            {
                LogError($"处理心跳命令异常: {ex.Message}", ex);
                return BaseCommand<IResponse>.CreateError($"心跳处理异常: {ex.Message}", 500)
                    .WithMetadata("ErrorCode", "HEARTBEAT_ERROR")
                    .WithMetadata("Exception", ex.Message)
                    .WithMetadata("StackTrace", ex.StackTrace);
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