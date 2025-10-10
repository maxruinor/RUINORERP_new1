using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
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
    /// 命令与数据包适配器 - 用于在BaseCommand和PacketModel之间进行双向转换
    /// 实现网络传输层与业务命令层的分离
    /// </summary>
    public class CommandPacketAdapter
    {
        private readonly CommandDispatcher _commandDispatcher;
        private readonly ICommandFactory _commandFactory;
        private static readonly ConcurrentDictionary<uint, Func<ICommand>> _ctorCache = new();
        private readonly ILogger<CommandPacketAdapter> _logger;
        private readonly CommandScanner _commandScanner;
        public CommandPacketAdapter(CommandDispatcher commandDispatcher,
            ICommandFactory commandFactory,
            ILogger<CommandPacketAdapter> logger = null, CommandScanner commandScanner = null)
        {
            _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
            _commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
            _logger = logger;
            _commandScanner = commandScanner ?? new CommandScanner();
        }




        /// <summary>
        /// 创建命令对象
        /// 根据命令ID和数据包内容创建适当类型的命令对象
        /// 优化点：充分利用ExecutionContext中的类型信息，减少重复的类型查找
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>创建的命令对象</returns>
        public ICommand CreateCommand(PacketModel packet)
        {
            if (packet == null)
                throw new ArgumentNullException(nameof(packet));

            try
            {
                var executionContext = packet.ExecutionContext;
                
                // 1. 优先使用ExecutionContext中的类型信息（最高优先级）
                if (executionContext?.CommandType != null && packet.CommandData != null && packet.CommandData.Length > 0)
                {
                    try
                    {
                        var command = CreateCommandFromBytes(packet.CommandData, executionContext.CommandType.FullName);
                        if (command != null)
                        {
                            InitializeCommandProperties(command, packet);
                            _logger?.LogDebug("优先使用ExecutionContext.CommandType创建命令: {CommandType}, BytesLength={BytesLength}",
                                executionContext.CommandType.Name, packet.CommandData.Length);
                            return command;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogWarning(ex, "使用ExecutionContext.CommandType创建命令失败，CommandType={CommandType}", 
                            executionContext.CommandType.Name);
                    }
                }

                // 2. 预获取命令类型信息（避免重复查找）
                Type commandType = executionContext?.CommandType ?? _commandDispatcher?.GetCommandType(packet.CommandId);
                if (commandType != null)
                {
                    _logger?.LogDebug("获取命令类型: CommandId={CommandId}, TypeName={TypeName}", 
                        packet.CommandId, commandType.Name);
                }

                // 3. 处理字节数组命令数据
                if (packet.CommandData != null && packet.CommandData.Length > 0)
                {
                    // 如果有明确的命令类型信息，优先使用它
                    if (commandType != null)
                    {
                        try
                        {
                            var command = CreateCommandFromBytes(packet.CommandData, commandType.FullName);
                            if (command != null)
                            {
                                InitializeCommandProperties(command, packet);
                                _logger?.LogDebug("从命令字节数据创建命令: CommandType={CommandType}, BytesLength={BytesLength}",
                                    commandType.Name, packet.CommandData.Length);
                                return command;
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogWarning(ex, "从命令字节数据创建命令失败，CommandType={CommandType}", commandType.Name);
                        }
                    }
                    
                    // 如果没有明确的类型信息，尝试从扩展属性中获取（兼容性考虑）
                    if (packet.Extensions != null && 
                        packet.Extensions.TryGetValue("CommandType", out var extCommandType) &&
                        extCommandType is string typeName)
                    {
                        try
                        {
                            var command = CreateCommandFromBytes(packet.CommandData, typeName);
                            if (command != null)
                            {
                                InitializeCommandProperties(command, packet);
                                _logger?.LogDebug("从扩展属性创建命令: TypeName={TypeName}, BytesLength={BytesLength}",
                                    typeName, packet.CommandData.Length);
                                return command;
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogWarning(ex, "从扩展属性CommandType创建命令失败，TypeName={TypeName}", typeName);
                        }
                    }
                }

                // 4. 尝试从命令工厂创建
                if (_commandFactory != null)
                {
                    var command = _commandFactory.CreateCommand(packet);
                    if (command != null)
                    {
                        InitializeCommandFromPacket(command, packet);
                        return command;
                    }
                }

                // 5. 使用预获取的命令类型创建具体命令
                if (commandType != null)
                {
                    var command = CreateCommandByType(commandType, packet);
                    if (command != null)
                    {
                        InitializeCommandFromPacket(command, packet);
                        return command;
                    }
                }

                // 6. 泛型命令回退：使用ExecutionContext中的RequestType优先
                if (packet.CommandData != null && packet.CommandData.Length > 0)
                {
                    // 优先使用ExecutionContext中的RequestType进行反序列化
                    if (executionContext?.RequestType != null)
                    {
                        try
                        {
                            var requestObject = MessagePackSerializer.Deserialize(executionContext.RequestType, packet.CommandData);
                            var genericCommandType = typeof(GenericCommand<>).MakeGenericType(executionContext.RequestType);
                            var command = Activator.CreateInstance(genericCommandType, packet.CommandId, requestObject) as ICommand;
                            InitializeCommandProperties(command, packet);
                            _logger?.LogDebug("使用ExecutionContext.RequestType创建泛型命令: RequestType={RequestType}", 
                                executionContext.RequestType.Name);
                            return command;
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogWarning(ex, "使用ExecutionContext.RequestType创建泛型命令失败，RequestType={RequestType}", 
                                executionContext.RequestType.Name);
                        }
                    }

                    // 回退到字典类型
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

                // 7. 最后使用泛型命令作为后备
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
        /// 初始化命令属性（轻量级初始化，不处理反序列化）
        /// </summary>
        /// <param name="command">命令实例</param>
        /// <param name="packet">数据包</param>
        private void InitializeCommandProperties(ICommand command, PacketModel packet)
        {
            if (command is BaseCommand baseCommand)
            {
                baseCommand.TimestampUtc = packet.TimestampUtc;
                baseCommand.CreatedTimeUtc = packet.CreatedTimeUtc;
            }
        }

        /// <summary>
        /// 初始化命令对象的属性（完整初始化，包含反序列化）
        /// </summary>
        /// <param name="command">命令实例</param>
        /// <param name="packet">数据包</param>
        private void InitializeCommandFromPacket(ICommand command, PacketModel packet)
        {
            if (command is BaseCommand baseCommand)
            {
                // 从数据包创建执行上下文，命令不再持有PacketModel引用

                baseCommand.TimestampUtc = packet.TimestampUtc;
                baseCommand.CreatedTimeUtc = packet.CreatedTimeUtc;


                // 关键：将CommandData中的请求实体数据反序列化到命令的Request属性中
                if (packet.CommandData != null && packet.CommandData.Length > 0)
                {
                    DeserializeRequestData(command, packet.CommandData, packet.ExecutionContext);
                }
            }
        }

        /// <summary>
        /// 将CommandData中的请求实体数据反序列化到命令的Request属性中
        /// 使用CommandId查找命令类型，ExecutionContext中的类型信息作为备选
        /// </summary>
        /// <param name="command">命令实例</param>
        /// <param name="commandData">包含请求数据的字节数组</param>
        private void DeserializeRequestData(ICommand command, byte[] commandData, CommandExecutionContext executionContext)
        {
            try
            {
                var commandType = command.GetType();
                var requestProperty = commandType.GetProperty("Request");

                if (requestProperty != null && requestProperty.CanWrite)
                {
                    var requestType = requestProperty.PropertyType;

                    // 优先使用命令自身的Request类型进行反序列化
                    var requestObject = MessagePackSerializer.Deserialize(requestType, commandData);
                    requestProperty.SetValue(command, requestObject);

                    _logger?.LogDebug("成功反序列化请求数据: CommandType={CommandType}, RequestType={RequestType}",
                        commandType.Name, requestType.Name);
                }
                else
                {
                    // 如果没有Request属性，尝试反序列化到动态属性或字段
                    DeserializeToDynamicProperties(command, commandData);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "反序列化请求数据失败: CommandType={CommandType}", command.GetType().Name);

                // 尝试使用ExecutionContext中的类型信息作为备选方案
                if (command is BaseCommand baseCommand && executionContext?.RequestType != null)
                {
                    try
                    {
                        var requestObject = MessagePackSerializer.Deserialize(executionContext.RequestType, commandData);
                        // 设置到动态属性或备用存储
                        SetRequestDataToCommand(command, requestObject);
                        _logger?.LogInformation("使用ExecutionContext类型信息成功反序列化请求数据");
                    }
                    catch (Exception fallbackEx)
                    {
                        _logger?.LogError(fallbackEx, "使用ExecutionContext类型信息反序列化也失败");
                    }
                }
            }
        }

        /// <summary>
        /// 将请求数据设置到命令的动态属性或备用存储中
        /// </summary>
        private void SetRequestDataToCommand(ICommand command, object requestData)
        {
            var commandType = command.GetType();

            // 尝试设置到Request属性
            var requestProperty = commandType.GetProperty("Request");
            if (requestProperty?.CanWrite == true)
            {
                requestProperty.SetValue(command, requestData);
                return;
            }

            // 尝试设置到Data属性
            var dataProperty = commandType.GetProperty("Data");
            if (dataProperty?.CanWrite == true)
            {
                dataProperty.SetValue(command, requestData);
                return;
            }

            // 尝试设置到字段
            var requestField = commandType.GetField("_request", BindingFlags.NonPublic | BindingFlags.Instance);
            if (requestField != null)
            {
                requestField.SetValue(command, requestData);
                return;
            }

            _logger?.LogWarning("无法将请求数据设置到命令 {CommandType} 的任何属性或字段中", commandType.Name);
        }

        /// <summary>
        /// 反序列化到命令的动态属性
        /// </summary>
        private void DeserializeToDynamicProperties(ICommand command, byte[] commandData)
        {
            try
            {
                // 反序列化为动态对象
                var dynamicData = MessagePackSerializer.Deserialize<dynamic>(commandData);

                // 尝试设置到可写的属性
                var commandType = command.GetType();
                var properties = commandType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanWrite && p.Name != "ExecutionContext"); // 避免设置ExecutionContext

                if (dynamicData is Dictionary<string, object> dict)
                {
                    foreach (var property in properties)
                    {
                        if (dict.ContainsKey(property.Name))
                        {
                            var value = dict[property.Name];
                            if (value != null && property.PropertyType.IsAssignableFrom(value.GetType()))
                            {
                                property.SetValue(command, value);
                            }
                        }
                    }
                }

                _logger?.LogDebug("成功反序列化到动态属性: CommandType={CommandType}", commandType.Name);
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "反序列化到动态属性失败，将保留原始字节数据");
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
        /// 将命令对象转换为数据包模型
        /// 用于发送命令时构建网络传输数据包
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

                // 设置时间戳
                packetBuilder.WithExtension("CreatedTimeUtc", command.CreatedTimeUtc);
                packetBuilder.WithExtension("TimestampUtc", command.TimestampUtc);

                return packetBuilder.Build();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "创建数据包失败: CommandId={CommandId}", command.CommandIdentifier);
                throw;
            }
        }

        /// <summary>
        /// 将具体命令实例序列化为字节数组并创建数据包
        /// </summary>
        /// <param name="command">具体命令实例</param>
        /// <returns>包含序列化命令的数据包模型</returns>
        public PacketModel CreatePacketWithCommandBytes(ICommand command)
        {
            try
            {
                if (command == null)
                    throw new ArgumentNullException(nameof(command));

                // 将命令实例序列化为字节数组，使用配置的MessagePack选项
                var commandBytes = MessagePackSerializer.Serialize(command, UnifiedSerializationService.MessagePackOptions);

                var packetBuilder = PacketBuilder.Create()
                    .WithCommand(command.CommandIdentifier)
                    .WithDirection(PacketDirection.Request)
                    .WithBinaryData(commandBytes); // 将序列化的命令作为二进制数据


                // 添加命令类型信息到扩展属性
                packetBuilder.WithExtension("CommandType", command.GetType().FullName);
                packetBuilder.WithExtension("IsCommandBytes", "true"); // 标记这是命令字节数据

                // 设置时间戳
                packetBuilder.WithExtension("CreatedTimeUtc", command.CreatedTimeUtc);
                packetBuilder.WithExtension("TimestampUtc", command.TimestampUtc);

                var packet = packetBuilder.Build();
                _logger?.LogDebug("创建包含命令字节的数据包成功: CommandId={CommandId}, Type={TypeName}, BytesLength={BytesLength}",
                    command.CommandIdentifier, command.GetType().Name, commandBytes.Length);

                return packet;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "创建包含命令字节的数据包失败: CommandId={CommandId}", command?.CommandIdentifier);
                throw new InvalidOperationException($"创建包含命令字节的数据包失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 从JSON和类型名称创建命令实例
        /// </summary>
        /// <param name="json">JSON字符串</param>
        /// <param name="typeName">类型名称</param>
        /// <returns>命令实例</returns>
        public ICommand CreateCommandFromJson(string json, string typeName)
        {
            if (string.IsNullOrEmpty(json))
                throw new ArgumentException("JSON字符串不能为空", nameof(json));

            if (string.IsNullOrEmpty(typeName))
                throw new ArgumentException("类型名称不能为空", nameof(typeName));

            try
            {
                var commandType = _commandScanner.GetCommandTypeByName(typeName);
                if (commandType == null)
                {
                    // 尝试从已注册的类型中查找
                    var allTypes = _commandScanner.GetAllCommandTypes();
                    foreach (var kvp in allTypes)
                    {
                        if (kvp.Value.FullName == typeName || kvp.Value.Name == typeName)
                        {
                            commandType = kvp.Value;
                            break;
                        }
                    }
                }

                if (commandType == null)
                    throw new ArgumentException($"未知的命令类型: {typeName}", nameof(typeName));

                return MessagePackSerializer.Deserialize(commandType, Encoding.UTF8.GetBytes(json)) as ICommand;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "从JSON创建命令失败: TypeName={TypeName}", typeName);
                throw new InvalidOperationException($"从JSON创建命令失败: {typeName}", ex);
            }
        }

        /// <summary>
        /// 从字节数组和类型名称创建命令实例
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <param name="typeName">类型名称</param>
        /// <returns>命令实例</returns>
        public ICommand CreateCommandFromBytes(byte[] data, string typeName)
        {
            if (data == null || data.Length == 0)
                throw new ArgumentException("数据不能为空", nameof(data));

            if (string.IsNullOrEmpty(typeName))
                throw new ArgumentException("类型名称不能为空", nameof(typeName));

            try
            {
                var commandType = _commandScanner.GetCommandTypeByName(typeName);
                if (commandType == null)
                {
                    // 尝试从已注册的类型中查找
                    var allTypes = _commandScanner.GetAllCommandTypes();
                    foreach (var kvp in allTypes)
                    {
                        if (kvp.Value.FullName == typeName || kvp.Value.Name == typeName)
                        {
                            commandType = kvp.Value;
                            break;
                        }
                    }
                }

                if (commandType == null)
                    throw new ArgumentException($"未知的命令类型: {typeName}", nameof(typeName));

                // 使用配置的MessagePack选项进行反序列化，确保使用ContractlessStandardResolver
                return MessagePackSerializer.Deserialize(commandType, data, UnifiedSerializationService.MessagePackOptions) as ICommand;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "从字节数组创建命令失败: TypeName={TypeName}", typeName);
                throw new InvalidOperationException($"从字节数组创建命令失败: {typeName}", ex);
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

        /// <summary>
        /// 将具体指令实例序列化为字节数组并打包到数据包中
        /// </summary>
        /// <param name="command">具体指令实例（如LoginCommand）</param>
        /// <param name="cmd">命令ID</param>
        /// <returns>包含序列化指令数据的数据包</returns>
        public static PacketModel PackCommand(ICommand command, CommandId cmd)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            try
            {
                // 获取指令类型名称用于后续反序列化
                var typeName = command.GetType().FullName;

                // 序列化指令实例为字节数组，使用配置的MessagePack选项
                var commandData = MessagePackSerializer.Serialize(command, UnifiedSerializationService.MessagePackOptions);

                // 构建数据包，包含类型信息和序列化数据
                return PacketBuilder.Create()
                    .WithCommand(cmd)
                    .WithBinaryData(commandData)
                    .WithExtension("CommandType", typeName)
                    .WithExtension("IsTypedCommand", true)
                    .Build();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"序列化指令失败: {command.GetType().Name}", ex);
            }
        }

        /// <summary>
        /// 从数据包中提取并反序列化具体指令实例
        /// </summary>
        /// <param name="packet">包含指令数据的数据包</param>
        /// <param name="commandScanner">命令扫描器</param>
        /// <returns>反序列化后的具体指令实例</returns>
        public static ICommand ExtractCommand(PacketModel packet, CommandScanner commandScanner)
        {
            if (packet == null) throw new ArgumentNullException(nameof(packet));
            if (commandScanner == null) throw new ArgumentNullException(nameof(commandScanner));

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
                        var allTypes = commandScanner.GetAllCommandTypes();
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
                        // 反序列化为具体指令类型，使用配置的MessagePack选项
                        var command = MessagePackSerializer.Deserialize(commandType, commandData, UnifiedSerializationService.MessagePackOptions) as ICommand;
                        if (command != null)
                        {
                            // 设置基本属性
                            //command.CommandIdentifier = packet.CommandId;
                            //command.SessionId = packet.SessionId;
                            //command.CreatedTimeUtc = packet.CreatedTimeUtc;
                            command.TimestampUtc = packet.TimestampUtc;

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
    }

}
