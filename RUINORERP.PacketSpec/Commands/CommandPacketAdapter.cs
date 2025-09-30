using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using MessagePack;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands.Message;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 命令与数据包适配器 - 用于在BaseCommand和PacketModel之间进行双向转换
    /// 实现网络传输层与业务命令层的分离
    /// </summary>
    public class CommandPacketAdapter
    {
        private readonly CommandDispatcher _commandDispatcher;
        private readonly ICommandFactory _commandFactory;
        private static readonly ConcurrentDictionary<uint, Func<ICommand>> _ctorCache = new();
        private readonly ILogger<CommandPacketAdapter> _logger;
        private readonly CommandTypeHelper _commandTypeHelper;
        public CommandPacketAdapter(CommandDispatcher commandDispatcher,
            ICommandFactory commandFactory,
            ILogger<CommandPacketAdapter> logger = null, CommandTypeHelper commandTypeHelper = null)
        {
            _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
            _commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
            _logger = logger;
            _commandTypeHelper = commandTypeHelper ?? new CommandTypeHelper();
        }

        // 命令 -> 网络包
        public static PacketModel ToPacket<TCommand>(TCommand command)
            where TCommand : ICommand
            => PacketModel.Create(command);

        // 网络包 -> 命令
        public  TCommand ToCommand<TCommand>(PacketModel packet)
            where TCommand : ICommand
            => MessagePackSerializer.Deserialize<TCommand>(packet.Data);

        // 自动类型推断版本
        public ICommand ToCommand(PacketModel packet)
        {
            if (packet == null)
                throw new ArgumentNullException(nameof(packet));

            var commandType = _commandTypeHelper.GetCommandType(packet.Command.FullCode);

            if (commandType == null)
                throw new InvalidOperationException($"未找到命令类型: {packet.Command}");
            
            var command = (ICommand)MessagePackSerializer.Deserialize(commandType, packet.Data);

            // 新增：如果命令有二进制数据，设置到容器中
            if (command is BaseCommand baseCmd && packet.Data != null)
            {
                // 对于泛型命令，尝试设置请求数据
                var genericBaseType = command.GetType().BaseType;
                if (genericBaseType != null && genericBaseType.IsGenericType &&
                    genericBaseType.GetGenericTypeDefinition() == typeof(BaseCommand<,>))
                {
                    var setMethod = command.GetType().GetMethod("SetRequestFromBinary");
                    setMethod?.Invoke(command, new object[] { packet.Data });
                }
            }

            return command;
        }


        /// <summary>
        /// 将命令转换为数据包
        /// </summary>
        /// <param name="command">命令实例</param>
        /// <returns>数据包实例</returns>
        public PacketModel ToPacket(ICommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var packet = new PacketModel(command.Serialize())
            {
                Direction = PacketDirection.Unknown,
                CreatedTimeUtc = command.CreatedTimeUtc,
                TimestampUtc = command.CreatedTimeUtc
            };

            // 添加命令标识符到扩展属性中
            packet.Extensions["CommandIdentifier"] = command.CommandIdentifier;

            return packet;
        }

        /// <summary>
        /// 从数据包创建命令
        /// </summary>
        /// <param name="packet">数据包实例</param>
        /// <returns>命令实例</returns>
        public ICommand FromPacket(PacketModel packet)
        {
            if (packet == null)
                throw new ArgumentNullException(nameof(packet));

            if (!packet.Extensions.TryGetValue("CommandIdentifier", out var commandIdObj) || !(commandIdObj is CommandId))
                throw new ArgumentException("数据包中缺少有效的命令标识符", nameof(packet));

            var commandId = (CommandId)commandIdObj;

            // 使用命令类型助手创建命令实例
            var command = CreateCommand(packet);
            if (command == null)
                throw new InvalidOperationException($"无法创建命令类型: {commandId}");

            // 反序列化命令数据
            //if (packet.Data != null && packet.Data.Length > 0)
            //{
            //    command.Deserialize(packet.Data);
            //}

            return command;
        }




        /// <summary>
        /// 创建命令对象
        /// 根据命令ID和数据包内容创建适当类型的命令对象
        /// </summary>
        /// <param name="package">数据包</param>
        /// <param name="sessionContext">会话上下文</param>
        /// <returns>创建的命令对象</returns>
        public ICommand CreateCommand(PacketModel packet)
        {
            if (packet == null)
                throw new ArgumentNullException(nameof(packet));

            try
            {
                // 优先使用命令工厂创建命令
                if (_commandFactory != null)
                {
                    var command = _commandFactory.CreateCommand(packet);
                    if (command != null)
                    {
                        // 新增：设置执行上下文
                        if (command is BaseCommand baseCommand)
                        {
                            baseCommand.ExecutionContext = CommandExecutionContext.CreateFromPacket(packet);
                        }
                        //command.BizData=packet.GetDataAsText
                        // 尝试从数据包中获取业务数据 这里晚再再看
                        // businessDataCommand.BusinessData = packetModel.GetJsonData<object>();

                        return command;
                    }
                }

                // 如果命令工厂无法创建命令，尝试根据命令ID查找对应的命令类型
                // 直接使用CommandDispatcher中的方法获取命令类型
                var commandType = _commandDispatcher?.GetCommandType(packet.Command);
                if (commandType != null)
                {
                    // 尝试使用构造函数创建命令实例
                    var constructor = GetSuitableConstructor(commandType);
                    if (constructor != null)
                    {
                        var parameters = PrepareConstructorParameters(constructor, packet);
                        var command = Activator.CreateInstance(commandType, parameters) as ICommand;

                        _logger?.LogDebug("根据命令ID创建命令实例: CommandId={CommandId}, Type={TypeName}",
                            packet.Command, commandType.FullName);
                        return command;
                    }
                }

                // 如果没有找到对应的命令类型或无法创建实例
                _logger?.LogWarning("无法创建命令对象: CommandId={CommandId}", packet.Command);
                return null;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "创建命令对象时出错: CommandId={CommandId}", packet.Command);
                // 如果创建失败，返回一个默认的命令对象
                var packetModelForError = new PacketModel(packet.Data)
                {
                    SessionId = packet.SessionId,
                    PacketId = packet.PacketId
                };

                return new MessageCommand(
                     packet.Command,
                    packetModelForError,
                    packet.Data)
                {
                } as ICommand;
            }
        }


        /// <summary>
        /// 获取适合的构造函数
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <returns>构造函数信息</returns>
        protected virtual ConstructorInfo GetSuitableConstructor(Type commandType)
        {
            try
            {
                // 查找包含CommandId、SessionInfo和Data参数的构造函数
                var constructors = commandType.GetConstructors();
                foreach (var constructor in constructors)
                {
                    var parameters = constructor.GetParameters();
                    if (parameters.Length >= 1 && parameters[0].ParameterType == typeof(PacketModel))
                    {
                        return constructor;
                    }
                }

                // 如果没有找到理想的构造函数，返回第一个可用的构造函数
                return constructors.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取命令类型 {CommandType} 的构造函数时出错", commandType?.FullName ?? "null");
                return null;
            }
        }



        /// <summary>
        /// 准备构造函数参数
        /// </summary>
        /// <param name="constructor">构造函数信息</param>
        /// <param name="package">数据包</param>
        /// <param name="sessionContext">会话上下文</param>
        /// <returns>参数数组</returns>
        protected virtual object[] PrepareConstructorParameters(ConstructorInfo constructor, PacketModel packet)
        {
            try
            {
                var parameters = constructor.GetParameters();
                var parameterValues = new object[parameters.Length];

                for (int i = 0; i < parameters.Length; i++)
                {
                    if (parameters[i].ParameterType == typeof(PacketModel))
                    {
                        // 创建PacketModel对象
                        parameterValues[i] = new PacketModel()
                        {
                            Data = packet.Data,
                            SessionId = packet.SessionId,
                            PacketId = packet.PacketId,
                            Extensions = packet.Extensions ?? new Dictionary<string, object>()
                        };

                    }
                    else if (parameters[i].ParameterType == typeof(byte[]))
                    {
                        parameterValues[i] = packet.Data;
                    }
                    else if (parameters[i].ParameterType == typeof(CommandId))
                    {
                        parameterValues[i] = packet.Command;
                    }
                    else if (parameters[i].ParameterType == typeof(string))
                    {
                        parameterValues[i] = packet.SessionId;
                    }
                    else
                    {
                        // 对于其他类型的参数，尝试使用默认值或null
                        parameterValues[i] = parameters[i].HasDefaultValue ? parameters[i].DefaultValue : null;
                    }
                }

                return parameterValues;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "准备构造函数参数时出错");
                return new object[0];
            }
        }


    }
}
