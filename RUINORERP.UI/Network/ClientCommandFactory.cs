using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Protocol;
using RUINORERP.PacketSpec.Serialization;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 客户端命令工厂
    /// 专门用于创建客户端命令实例
    /// </summary>
    public class ClientCommandFactory : ICommandFactory
    {
        private readonly ClientCommandDispatcher _dispatcher;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dispatcher">客户端命令调度器</param>
        public ClientCommandFactory(ClientCommandDispatcher dispatcher)
        {
            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        }

        /// <summary>
        /// 从统一数据包创建命令
        /// </summary>
        /// <param name="packet">统一数据包</param>
        /// <returns>命令对象</returns>
        public ICommand CreateCommand(PacketModel packet)
        {
            if (packet == null)
            {
                throw new ArgumentNullException(nameof(packet));
            }

            // 获取命令ID
            uint commandId = packet.Command.FullCode;

            // 通过调度器创建命令
            var command = _dispatcher.CreateCommand(commandId);
            
            // 初始化命令属性
            if (command != null)
            {
                InitializeCommand(command, packet);
            }

            return command;
        }

        /// <summary>
        /// 从OriginalData创建命令
        /// </summary>
        /// <param name="originalData">原始数据</param>
        /// <returns>命令对象</returns>
        public ICommand CreateCommand(OriginalData originalData)
        {
            if (!originalData.IsValid)
            {
                throw new ArgumentException("原始数据无效", nameof(originalData));
            }

            try
            {
                // 从原始数据创建PacketModel
                var packet = CreatePacketModelFromOriginalData(originalData);
                if (packet == null)
                {
                    return null;
                }

                // 使用PacketModel创建命令
                return CreateCommand(packet);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"从原始数据创建命令时发生异常: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 注册命令创建器
        /// </summary>
        /// <param name="commandCode">命令代码</param>
        /// <param name="creator">创建器函数</param>
        public void RegisterCommandCreator(uint commandCode, Func<PacketModel, ICommand> creator)
        {
            // 客户端命令工厂不支持自定义创建器，使用调度器注册机制
            throw new NotSupportedException("客户端命令工厂不支持自定义命令创建器，请使用调度器注册命令类型");
        }

        /// <summary>
        /// 初始化命令对象
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="packet">数据包</param>
        private void InitializeCommand(ICommand command, PacketModel packet)
        {
            if (command == null || packet == null)
                return;

            // 设置会话ID
            command.SessionID = packet.ClientId;

            // 如果命令是BaseCommand类型，设置更多属性
            if (command is BaseCommand baseCommand)
            {
                baseCommand.packetModel = packet;
                baseCommand.Timestamp = packet.Timestamp;
                baseCommand.CreatedTime = packet.CreatedTime;
            }
        }

        /// <summary>
        /// 从OriginalData创建PacketModel
        /// </summary>
        /// <param name="originalData">原始数据</param>
        /// <returns>创建的PacketModel对象</returns>
        private PacketModel CreatePacketModelFromOriginalData(OriginalData originalData)
        {
            var packet = new PacketModel
            {
                PacketId = Guid.NewGuid().ToString(),
                CreatedTime = DateTime.UtcNow,
                Timestamp = DateTime.UtcNow,
                Size = originalData.Length,
                IsEncrypted = false,
                IsCompressed = false,
                Extensions = new Dictionary<string, object>()
            };

            // 从OriginalData构建CommandId
            // Cmd表示CommandCategory，One表示OperationCode子指令
            CommandCategory category = (CommandCategory)originalData.Cmd;
            byte operationCode = 0; // 默认OperationCode
            
            // 如果One不为空，则使用第一个字节作为OperationCode
            if (originalData.One != null && originalData.One.Length > 0)
            {
                operationCode = originalData.One[0];
            }
            
            packet.Command = new CommandId(category, operationCode);

            return packet;
        }
    }
}