using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Protocol;
using RUINORERP.PacketSpec.Serialization;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 客户端命令工厂
    /// 专门用于创建客户端命令实例，实现ICommandFactory接口
    /// 负责将网络数据包转换为具体的命令对象，是网络层与业务层之间的重要桥梁
    /// 处理从底层网络数据到应用层命令对象的转换过程
    /// </summary>
    public class ClientCommandFactory : ICommandFactory
    {
        private readonly ClientCommandDispatcher _dispatcher;
        private readonly ILogger<ClientCommandFactory> _logger;
        private readonly object _lock = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dispatcher">客户端命令调度器，负责根据命令ID创建具体命令实例</param>
        /// <param name="logger">日志记录器，用于记录命令创建过程中的信息和异常</param>
        /// <exception cref="ArgumentNullException">当调度器为空时抛出</exception>
        public ClientCommandFactory(ClientCommandDispatcher dispatcher, ILogger<ClientCommandFactory> logger = null)
        {
            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
            _logger = logger;
            _logger?.LogDebug("ClientCommandFactory已初始化");
        }

        /// <summary>
        /// 从统一数据包创建命令
        /// 将PacketModel对象转换为具体的ICommand实现
        /// </summary>
        /// <param name="packet">统一数据包，包含命令ID和相关数据</param>
        /// <returns>创建的命令对象，如果无法创建则返回null</returns>
        /// <exception cref="ArgumentNullException">当数据包为空时抛出</exception>
        /// <exception cref="InvalidOperationException">当创建命令过程中发生异常时抛出</exception>
        public ICommand CreateCommand(PacketModel packet)
        {
            if (packet == null)
            {
                throw new ArgumentNullException(nameof(packet), "数据包不能为空");
            }

            try
            {
                // 获取命令ID
                uint commandId = packet.Command.FullCode;
                _logger?.LogDebug("创建命令，命令ID: {CommandId}", commandId);

                // 通过调度器创建命令
                var command = _dispatcher.CreateCommand(commandId);
                
                // 初始化命令属性
                if (command != null)
                {
                    InitializeCommand(command, packet);
                    _logger?.LogDebug("成功创建命令: {CommandType}", command.GetType().Name);
                }
                else
                {
                    _logger?.LogWarning("无法创建命令，命令ID: {CommandId}，调度器未找到对应的命令类型", commandId);
                }

                return command;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "创建命令时发生异常，命令ID: {CommandId}", packet?.Command.OperationCode);
                throw new InvalidOperationException($"创建命令时发生异常: {ex.Message}", ex);
            }
        }

        

        /// <summary>
        /// 注册命令创建器
        /// 客户端命令工厂不支持此功能，使用调度器注册机制
        /// </summary>
        /// <param name="commandCode">命令代码</param>
        /// <param name="creator">创建器函数</param>
        /// <exception cref="NotSupportedException">总是抛出，表示不支持此功能</exception>
        public void RegisterCommandCreator(uint commandCode, Func<PacketModel, ICommand> creator)
        {
            _logger?.LogWarning("尝试调用不支持的方法RegisterCommandCreator");
            throw new NotSupportedException("客户端命令工厂不支持自定义命令创建器，请使用调度器注册命令类型");
        }

        /// <summary>
        /// 初始化命令对象的属性
        /// 设置命令的会话ID、数据包信息、时间戳等关键属性
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="packet">数据包</param>
        private void InitializeCommand(ICommand command, PacketModel packet)
        {
            if (command == null || packet == null)
                return;

            try
            {
                lock (_lock)
                {
                    // 设置会话ID
                    command.SessionID = packet.ClientId;

                    // 如果命令是BaseCommand类型，设置更多属性
                    if (command is BaseCommand baseCommand)
                    {
                        baseCommand.Packet = packet;
                        baseCommand.Timestamp = packet.Timestamp;
                        baseCommand.CreatedTimeUtc = packet.CreatedTimeUtc;
                       // baseCommand.CommandId = packet.Command.FullCode;
                    }
                }
                
                _logger?.LogTrace("已初始化命令属性，会话ID: {SessionId}", command.SessionID);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "初始化命令属性时发生异常");
                // 记录异常但不中断流程，确保命令创建过程不被完全阻断
            }
        }

        /// <summary>
        /// 从原始数据创建PacketModel对象
        /// 将底层网络数据转换为应用层可识别的数据包模型
        /// </summary>
        /// <param name="originalData">原始数据</param>
        /// <returns>创建的PacketModel对象</returns>
        /// <exception cref="InvalidOperationException">当创建PacketModel过程中发生异常时抛出</exception>
        private PacketModel CreatePacketModelFromOriginalData(OriginalData originalData)
        {
            try
            {
                lock (_lock)
                {
                    var packet = new PacketModel
                    {
                        PacketId = Guid.NewGuid().ToString(),
                        CreatedTimeUtc = DateTime.UtcNow,
                        Timestamp = DateTime.UtcNow,
                        Size = originalData.Length,
                        IsEncrypted = false,
                        IsCompressed = false,
                        Extensions = new Dictionary<string, object>(),
                        ClientId = string.Empty // 初始化为空字符串，稍后可能会设置
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
                    
                    // 添加原始数据到扩展字段中，以便后续处理
                    packet.Extensions["OriginalData"] = originalData;

                    _logger?.LogDebug("从原始数据创建PacketModel成功，命令类别: {Category}, 操作码: {OperationCode}", 
                        category, operationCode);
                    
                    return packet;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "从原始数据创建PacketModel时发生异常");
                throw new InvalidOperationException($"从原始数据创建PacketModel时发生异常: {ex.Message}", ex);
            }
        }
    }
}