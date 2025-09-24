using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Protocol;

namespace RUINORERP.Server.Network.Commands
{
    /// <summary>
    /// 回显命令处理器 - 服务器端实现
    /// 处理EchoCommand命令的处理器实现
    /// </summary>
    [CommandHandler("EchoCommandHandler", priority: 10)]
    public class EchoCommandHandler : UnifiedCommandHandlerBase
    {
        /// <summary>
        /// 支持的命令类型列表
        /// </summary>
        public override IReadOnlyList<uint> SupportedCommands => new uint[]
        {
            // 使用Special类别和0x01操作码，与EchoCommand中定义的一致
            new CommandId(CommandCategory.Special, 0x01).FullCode
        };

        /// <summary>
        /// 处理器优先级
        /// </summary>
        public override int Priority => 10;

        /// <summary>
        /// 无参构造函数，以支持Activator.CreateInstance创建实例
        /// </summary>
        public EchoCommandHandler() : base()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public EchoCommandHandler(ILogger<EchoCommandHandler> logger) : base(logger)
        {
        }

        /// <summary>
        /// 具体的命令处理逻辑
        /// </summary>
        protected override async Task<CommandResult> ProcessCommandAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                LogInfo($"处理回显命令 [会话: {command.SessionID}]");

                // 解析回显数据
                var echoData = command.Packet.GetJsonData<EchoData>();
                
                // 创建响应数据
                var responseData = CreateEchoResponse(echoData);

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: new { 
                        OriginalMessage = echoData.Message,
                        EchoedMessage = echoData.Message,
                        Timestamp = DateTime.UtcNow,
                        SessionId = command.SessionID
                    },
                    message: "回显命令处理成功"
                );
            }
            catch (Exception ex)
            {
                LogError($"处理回显命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"回显命令处理异常: {ex.Message}", "ECHO_ERROR", ex);
            }
        }

        /// <summary>
        /// 解析回显数据
        /// </summary>
        private EchoData ParseEchoData(OriginalData originalData)
        {
            try
            {
                if (originalData.One == null || originalData.One.Length == 0)
                    return new EchoData { Message = string.Empty };

                var dataString = System.Text.Encoding.UTF8.GetString(originalData.One);
                return new EchoData { Message = dataString };
            }
            catch (Exception ex)
            {
                LogError($"解析回显数据异常: {ex.Message}", ex);
                return new EchoData { Message = string.Empty };
            }
        }

        /// <summary>
        /// 创建回显响应
        /// </summary>
        private OriginalData CreateEchoResponse(EchoData echoData)
        {
            var responseData = $"ECHO:{echoData.Message}";
            var data = System.Text.Encoding.UTF8.GetBytes(responseData);

            // 使用Special类别和0x01操作码，与EchoCommand中定义的一致
            uint commandId = new CommandId(CommandCategory.Special, 0x01).FullCode;
            byte category = (byte)(commandId & 0xFF); // 取低8位作为Category
            byte operationCode = (byte)((commandId >> 8) & 0xFF); // 取次低8位作为OperationCode

            return new OriginalData(
                category,
                new byte[] { operationCode },
                data
            );
        }
    }

    /// <summary>
    /// 回显数据
    /// </summary>
    public class EchoData
    {
        public string Message { get; set; }
    }
}