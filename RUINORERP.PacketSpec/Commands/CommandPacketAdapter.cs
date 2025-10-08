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




        /// <summary>
        /// 创建命令对象
        /// 根据命令ID和数据包内容创建适当类型的命令对象
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>创建的命令对象</returns>
        public ICommand CreateCommand(PacketModel packet)
        {
            if (packet == null)
                throw new ArgumentNullException(nameof(packet));

            try
            {
                // 1. 首先尝试从命令工厂创建
                if (_commandFactory != null)
                {
                    var command = _commandFactory.CreateCommand(packet);
                    if (command != null)
                    {
                        InitializeCommandFromPacket(command, packet);
                        return command;
                    }
                }

                // 2. 尝试根据命令ID创建具体命令类型
                var commandType = _commandDispatcher?.GetCommandType(packet.CommandId.FullCode);
                if (commandType != null)
                {
                    var command = CreateCommandByType(commandType, packet);
                    if (command != null)
                    {
                        InitializeCommandFromPacket(command, packet);
                        return command;
                    }
                }
                
                // 增加泛型命令回退：即便客户端发了个未注册命令，也能用泛型命令接住，服务器不崩溃
                if (commandType == null && packet.CommandData != null && packet.CommandData.Length > 0)
                {
                    try
                    {
                        return new GenericCommand<Dictionary<string, object>>(packet.CommandId,
                            MessagePackSerializer.Deserialize<Dictionary<string, object>>(packet.CommandData));
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogWarning(ex, "尝试反序列化为字典类型的泛型命令失败");
                    }
                }

                // 3. 最后使用泛型命令作为后备
                return CreateGenericCommand(packet);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "创建命令对象时出错: CommandId={CommandId}", packet.CommandId);
                return CreateFallbackCommand(packet, ex);
            }
        }

        /// <summary>
        /// 根据类型创建命令实例
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <param name="packet">数据包</param>
        /// <returns>命令实例</returns>
        private ICommand CreateCommandByType(Type commandType, PacketModel packet)
        {
            try
            {
                var constructor = GetSuitableConstructor(commandType);
                if (constructor != null)
                {
                    var parameters = PrepareConstructorParameters(constructor, packet);
                    var command = Activator.CreateInstance(commandType, parameters) as ICommand;

                    _logger?.LogDebug("根据命令ID创建命令实例: CommandId={CommandId}, Type={TypeName}",
                        packet.CommandId, commandType.FullName);
                    return command;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "根据类型创建命令实例失败: Type={TypeName}", commandType.FullName);
            }
            return null;
        }

        /// <summary>
        /// 创建泛型命令对象
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>泛型命令实例</returns>
        private ICommand CreateGenericCommand(PacketModel packet)
        {
            // 根据数据内容自动判断使用哪种泛型命令
            if (packet.CommandData != null && packet.CommandData.Length > 0)
            {
                try
                {
                    // 尝试反序列化为字典，判断是否是键值对数据
                    var dict = MessagePack.MessagePackSerializer.Deserialize<Dictionary<string, object>>(packet.CommandData);
                    return new GenericCommand<Dictionary<string, object>>(packet.CommandId, dict);
                }
                catch
                {
                    // 如果无法反序列化为字典，使用byte[]作为Payload
                    return new GenericCommand<byte[]>(packet.CommandId, packet.CommandData);
                }
            }
            
            return new GenericCommand<object>(packet.CommandId, null);
        }

        /// <summary>
        /// 初始化命令对象的属性
        /// </summary>
        /// <param name="command">命令实例</param>
        /// <param name="packet">数据包</param>
        private void InitializeCommandFromPacket(ICommand command, PacketModel packet)
        {
            if (command is BaseCommand baseCommand)
            {
                baseCommand.ExecutionContext = CommandExecutionContext.CreateFromPacket(packet);
                baseCommand.TimestampUtc = packet.TimestampUtc;
                baseCommand.CreatedTimeUtc = packet.CreatedTimeUtc;
                
                // 自动提取Token
                if (!string.IsNullOrEmpty(packet.Token))
                {
                    baseCommand.AuthToken = packet.Token;
                    baseCommand.TokenType = "Bearer";
                }
                
                // 关键：将CommandData中的请求实体数据反序列化到命令的Request属性中
                if (packet.CommandData != null && packet.CommandData.Length > 0)
                {
                    DeserializeRequestData(command, packet.CommandData);
                }
            }
        }

        /// <summary>
        /// 将CommandData中的请求实体数据反序列化到命令的Request属性中
        /// </summary>
        /// <param name="command">命令实例</param>
        /// <param name="commandData">包含请求数据的字节数组</param>
        private void DeserializeRequestData(ICommand command, byte[] commandData)
        {
            try
            {
                // 获取命令的Request属性类型
                var commandType = command.GetType();
                var requestProperty = commandType.GetProperty("Request");
                
                if (requestProperty != null && requestProperty.CanWrite)
                {
                    var requestType = requestProperty.PropertyType;
                    
                    // 使用MessagePack反序列化请求数据
                    var requestObject = MessagePackSerializer.Deserialize(requestType, commandData);
                    requestProperty.SetValue(command, requestObject);
                    
                    _logger?.LogDebug("成功反序列化请求数据到命令的Request属性: CommandType={CommandType}, RequestType={RequestType}", 
                        commandType.Name, requestType.Name);
                }
                else
                {
                    _logger?.LogWarning("命令类型 {CommandType} 没有可写的Request属性，无法反序列化请求数据", commandType.Name);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "反序列化请求数据失败: CommandType={CommandType}", command.GetType().Name);
            }
        }

        /// <summary>
        /// 创建备用命令对象（当创建主命令失败时使用）
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <param name="ex">创建失败的异常</param>
        /// <returns>备用命令实例</returns>
        private ICommand CreateFallbackCommand(PacketModel packet, Exception ex)
        {
            try
            {
                var packetModelForError = PacketBuilder.Create()
                    .WithBinaryData(packet.CommandData)
                    .WithSession(packet.SessionId)
                    .WithExtension("PacketId", packet.PacketId)
                    .WithExtension("CreationError", ex.Message)
                    .Build();

                return new MessageCommand(
                    packet.CommandId,
                    packetModelForError,
                    packet.CommandData);
            }
            catch (Exception fallbackEx)
            {
                _logger?.LogCritical(fallbackEx, "创建备用命令对象也失败了");
                // 最后的防线，确保至少返回一个基本命令对象
                return new GenericCommand<object>(packet.CommandId, null);
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
                        var builder = PacketBuilder.Create()
                            .WithBinaryData(packet.CommandData)
                            .WithSession(packet.SessionId)
                            .WithExtension("PacketId", packet.PacketId);
                        
                        if (packet.Extensions != null)
                        {
                            foreach (var extension in packet.Extensions)
                            {
                                // 避免覆盖已经设置的扩展属性
                                if (extension.Key != "PacketId")
                                {
                                    builder.WithExtension(extension.Key, extension.Value);
                                }
                            }
                        }
                        
                        parameterValues[i] = builder.Build();

                    }
                    else if (parameters[i].ParameterType == typeof(byte[]))
                    {
                        parameterValues[i] = packet.CommandData;
                    }
                    else if (parameters[i].ParameterType == typeof(CommandId))
                    {
                        parameterValues[i] = packet.CommandId;
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

    /// <summary>
    /// CommandPacketAdapter的扩展方法类
    /// 提供便捷的JSON数据打包功能
    /// </summary>
    public static class CommandPacketAdapterExtensions
    {
        /// <summary>
        /// 将DTO对象打包为JSON格式的PacketModel
        /// </summary>
        /// <typeparam name="T">DTO类型</typeparam>
        /// <param name="dto">DTO对象</param>
        /// <param name="cmd">命令ID</param>
        /// <returns>包含JSON数据的PacketModel</returns>
        public static PacketModel PackJson<T>(T dto, CommandId cmd)
            => PacketBuilder.Create().WithJsonData(dto).WithCommand(cmd).Build();
    }
}
