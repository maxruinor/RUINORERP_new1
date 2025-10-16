using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MessagePack;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands.Message;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Serialization;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 优化后的统一命令创建服务实现 - 集中处理所有命令创建逻辑
    /// 精简了实现，减少了代码重复，提高了可维护性
    /// </summary>
    public class CommandCreationService : ICommandCreationService
    {
        private readonly ILogger<CommandCreationService> _logger;
        private readonly CommandScanner _commandScanner;

        // 使用并发字典提高性能和线程安全
        private readonly ConcurrentDictionary<CommandId, Func<PacketModel, ICommand>> _commandCreators;
        private readonly ConcurrentDictionary<CommandId, Type> _commandTypeCache;
        private readonly ConcurrentDictionary<Type, Func<ICommand>> _constructorCache;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="commandScanner">命令扫描器</param>
        public CommandCreationService(ILogger<CommandCreationService> logger = null, CommandScanner commandScanner = null)
        {
            _logger = logger;
            _commandScanner = commandScanner ?? new CommandScanner();
            _commandCreators = new ConcurrentDictionary<CommandId, Func<PacketModel, ICommand>>();
            _commandTypeCache = new ConcurrentDictionary<CommandId, Type>();
            _constructorCache = new ConcurrentDictionary<Type, Func<ICommand>>();
        }

        /// <summary>
        /// 从数据包创建命令 - 主入口方法
        /// 优化版本：仅从缓存获取命令类型，通过二进制数据创建命令实例，减少网络传输数据量
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>创建的命令对象，失败时返回null</returns>
        public ICommand CreateCommand(PacketModel packet)
        {
            if (packet == null)
            {
                _logger?.LogWarning("尝试从空的PacketModel创建命令");
                return null;
            }

            try
            {
                var commandId = packet.CommandId;
                // 3. 优先处理字节数组命令数据，通过二进制数据创建命令实例
                if (packet.CommandData != null && packet.CommandData.Length > 0)
                {
                    var command = CreateCommandFromBytes(packet.CommandData, packet.ExecutionContext.CommandType);
                    if (command != null)
                    {
                        InitializeCommandProperties(command, packet);
                        return command;
                    }
                }

                // 1. 检查是否有注册的自定义创建器
                if (_commandCreators.TryGetValue(commandId, out var creator))
                {
                    var command = creator(packet);
                    if (command != null)
                    {
                        InitializeCommandProperties(command, packet);
                        return command;
                    }
                }

                // 2. 仅从缓存获取命令类型，不获取完整的命令对象
                var commandType = GetCommandType(commandId);
                if (commandType == null)
                {
                    _logger?.LogWarning("未找到命令类型: {CommandId}", commandId);
                    return CreateFallbackCommand(packet);
                }

                // 4. 如果没有二进制数据，创建空命令实例
                var emptyCommand = CreateEmptyCommand(commandType);
                if (emptyCommand != null)
                {
                    InitializeCommandProperties(emptyCommand, packet);
                    return emptyCommand;
                }

                // 5. 最后使用泛型命令作为后备
                return CreateFallbackCommand(packet);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "创建命令对象时出错: CommandId={CommandId}", packet.CommandId);
                return CreateFallbackCommand(packet);
            }
        }

        /// <summary>
        /// 从字节数组和类型名称创建命令
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <param name="typeName">类型名称</param>
        /// <returns>创建的命令对象</returns>
        public ICommand CreateCommandFromBytes(byte[] data, string typeName)
        {
            if (data == null || data.Length == 0)
                throw new ArgumentException("数据不能为空", nameof(data));

            if (string.IsNullOrEmpty(typeName))
                throw new ArgumentException("类型名称不能为空", nameof(typeName));

            // 获取类型对象
            var commandType = GetCommandTypeByName(typeName);
            if (commandType == null)
                throw new ArgumentException($"未知的命令类型: {typeName}", nameof(typeName));

            // 调用基于类型的重载
            return CreateCommandFromBytes(data, commandType);
        }

        /// <summary>
        /// 从字节数组和类型对象创建命令
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <param name="type">类型对象</param>
        /// <returns>创建的命令对象</returns>
        public ICommand CreateCommandFromBytes(byte[] data, Type type)
        {
            if (data == null || data.Length == 0)
                throw new ArgumentException("数据不能为空", nameof(data));

            if (type == null)
                throw new ArgumentException("类型对象不能为空", nameof(type));

            try
            {
                // 验证类型是否实现了ICommand接口
                if (!typeof(ICommand).IsAssignableFrom(type))
                {
                    throw new ArgumentException($"类型 {type.FullName} 未实现 ICommand 接口", nameof(type));
                }

                // 使用MessagePack进行反序列化
                var command = MessagePackSerializer.Deserialize(type, data, UnifiedSerializationService.MessagePackOptions) as ICommand;
                return command;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "从字节数组创建命令失败: Type={TypeName}", type?.Name);
                throw new InvalidOperationException($"从字节数组创建命令失败: {type?.FullName}", ex);
            }
        }

        /// <summary>
        /// 从数据包中提取类型化命令
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>提取的命令对象，不是类型化命令时返回null</returns>
        public ICommand ExtractTypedCommand(PacketModel packet)
        {
            if (packet == null) throw new ArgumentNullException(nameof(packet));

            try
            {
                // 检查是否包含类型化指令
                if (packet.Extensions?.ContainsKey("IsTypedCommand") == true &&
                    packet.Extensions?.ContainsKey("CommandType") == true)
                {
                    var typeName = packet.Extensions["CommandType"] as string;
                    var commandData = packet.CommandData;

                    if (string.IsNullOrEmpty(typeName) || commandData == null || commandData.Length == 0)
                        return null;

                    // 使用CommandScanner获取类型
                    var commandType = Type.GetType(typeName);
                    if (commandType == null)
                    {
                        // 尝试从已注册的类型中查找
                        var allTypes = _commandScanner.GetAllCommandTypes();
                        foreach (var kvp in allTypes)
                        {
                            if (kvp.Value.FullName == typeName)
                            {
                                commandType = kvp.Value;
                                break;
                            }
                        }
                    }

                    if (commandType != null)
                    {
                        // 反序列化为具体指令类型
                        var command = MessagePackSerializer.Deserialize(commandType, commandData, UnifiedSerializationService.MessagePackOptions) as ICommand;
                        if (command != null)
                        {
                            return command;
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"从数据包提取指令失败: {packet.CommandId}", ex);
            }
        }

        /// <summary>
        /// 根据命令ID创建空命令实例
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <returns>空命令实例</returns>
        public ICommand CreateEmptyCommand(CommandId commandId)
        {
            var commandType = _commandScanner.GetCommandType(commandId);
            if (commandType == null)
                throw new ArgumentException($"未知的命令ID: {commandId}", nameof(commandId));

            return Activator.CreateInstance(commandType) as ICommand;
        }

        /// <summary>
        /// 创建空命令实例
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <returns>空命令实例</returns>
        public ICommand CreateEmptyCommand(Type commandType)
        {
            if (commandType == null)
                throw new ArgumentNullException(nameof(commandType));

            try
            {
                // 使用缓存提高性能
                if (_constructorCache.TryGetValue(commandType, out var constructor))
                {
                    var command = constructor();
                    if (command != null)
                    {
                        _logger?.LogDebug($"使用缓存构造函数创建空命令实例: {commandType.Name}");
                        return command;
                    }
                }

                // 使用Activator创建实例
                var commandInstance = Activator.CreateInstance(commandType) as ICommand;
                if (commandInstance != null)
                {
                    // 缓存构造函数
                    _constructorCache.TryAdd(commandType, () => commandInstance);
                    _logger?.LogDebug($"使用Activator创建空命令实例: {commandType.Name}");
                    return commandInstance;
                }

                throw new InvalidOperationException($"无法创建命令实例: {commandType.Name}");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"创建空命令实例失败: {commandType?.Name}");
                throw;
            }
        }

        /// <summary>
        /// 创建泛型命令作为后备方案
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>泛型命令对象</returns>
        public ICommand CreateFallbackCommand(PacketModel packet)
        {
            if (packet == null)
                return new GenericCommand<object>(CommandId.Empty, null);

            try
            {
                if (packet.CommandData != null && packet.CommandData.Length > 0)
                {
                    var executionContext = packet.ExecutionContext;

                    // 优先使用ExecutionContext中的RequestType
                    if (executionContext?.RequestType != null)
                    {
                        try
                        {
                            var requestObject = MessagePackSerializer.Deserialize(executionContext.RequestType, packet.CommandData, UnifiedSerializationService.MessagePackOptions);
                            var genericCommandType = typeof(GenericCommand<>).MakeGenericType(executionContext.RequestType);
                            var command = Activator.CreateInstance(genericCommandType, packet.CommandId, requestObject) as ICommand;
                            InitializeCommandProperties(command, packet);
                            _logger?.LogDebug("使用ExecutionContext.RequestType创建泛型命令: RequestType={RequestType}", executionContext.RequestType.Name);
                            return command;
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogWarning(ex, "使用ExecutionContext.RequestType创建泛型命令失败，RequestType={RequestType}", executionContext.RequestType.Name);
                        }
                    }

                    // 回退到字典类型
                    try
                    {
                        var dictData = MessagePackSerializer.Deserialize<Dictionary<string, object>>(packet.CommandData);
                        var command = new GenericCommand<Dictionary<string, object>>(packet.CommandId, dictData);
                        InitializeCommandProperties(command, packet);
                        return command;
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogWarning(ex, "尝试反序列化为字典类型的泛型命令失败");
                    }
                }

                // 最后使用空的泛型命令
                var emptyCommand = new GenericCommand<Dictionary<string, object>>(packet.CommandId, new Dictionary<string, object>());
                InitializeCommandProperties(emptyCommand, packet);
                return emptyCommand;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "创建后备命令失败，使用最基本的泛型命令");
                return new GenericCommand<object>(packet.CommandId, null);
            }
        }

        /// <summary>
        /// 初始化命令对象的基本属性
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="packet">源数据包</param>
        private void InitializeCommandProperties(ICommand command, PacketModel packet)
        {
            if (command == null || packet == null)
                return;
 
        }

        /// <summary>
        /// 注册命令创建器
        /// </summary>
        /// <param name="commandCode">命令代码</param>
        /// <param name="creator">创建器函数</param>
        public void RegisterCommandCreator(CommandId commandCode, Func<PacketModel, ICommand> creator)
        {
            if (creator == null)
            {
                throw new ArgumentNullException(nameof(creator), "命令创建器不能为null");
            }

            _commandCreators[commandCode] = creator;
            _logger?.LogDebug("已注册命令创建器: {CommandCode}", commandCode);
        }



        /// <summary>
        /// 根据命令ID获取命令类型
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <returns>命令类型</returns>
        private Type GetCommandType(CommandId commandId)
        {
            // 先从缓存中查找
            if (_commandTypeCache.TryGetValue(commandId, out var cachedType))
                return cachedType;

            // 从命令扫描器获取
            var commandType = _commandScanner.GetCommandType(commandId);
            if (commandType != null)
            {
                _commandTypeCache.TryAdd(commandId, commandType);
            }

            return commandType;
        }

        /// <summary>
        /// 根据类型名称获取命令类型
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <returns>命令类型</returns>
        private Type GetCommandTypeByName(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
                return null;

            // 先尝试使用Type.GetType
            var type = Type.GetType(typeName);
            if (type != null && typeof(ICommand).IsAssignableFrom(type))
                return type;

            // 从命令扫描器获取
            type = _commandScanner.GetCommandTypeByName(typeName);
            if (type != null)
                return type;

            // 从所有命令类型中查找
            var allTypes = _commandScanner.GetAllCommandTypes();
            foreach (var kvp in allTypes)
            {
                if (kvp.Value.FullName == typeName || kvp.Value.Name == typeName)
                {
                    return kvp.Value;
                }
            }

            return null;
        }



        /// <summary>
        /// 将命令对象转换为数据包模型
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <returns>数据包模型</returns>
        public PacketModel CreatePacket(ICommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            try
            {
                var packetBuilder = PacketBuilder.Create()
                    .WithCommand(command.CommandIdentifier)
                    .WithDirection(PacketDirection.Response);

                // 序列化命令数据
                if (command is BaseCommand baseCommand)
                {
                    var commandData = baseCommand.GetBinaryData();
                    if (commandData != null && commandData.Length > 0)
                    {
                        packetBuilder.WithBinaryData(commandData);
                    }
                }
 

                return packetBuilder.Build();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "创建数据包失败: CommandId={CommandId}", command.CommandIdentifier);
                throw;
            }
        }
    }
}
