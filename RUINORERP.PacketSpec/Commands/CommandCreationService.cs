using System;
using System.Collections.Generic;
using MessagePack;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Serialization;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 统一命令创建服务实现 - 集中处理所有命令创建逻辑
    /// 避免 CommandPacketAdapter 和 DefaultCommandFactory 之间的代码重复
    /// </summary>
    public class CommandCreationService : ICommandCreationService
    {
        private readonly ILogger<CommandCreationService> _logger;
        private readonly CommandScanner _commandScanner;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="commandScanner">命令扫描器</param>
        public CommandCreationService(ILogger<CommandCreationService> logger = null, CommandScanner commandScanner = null)
        {
            _logger = logger;
            _commandScanner = commandScanner ?? new CommandScanner();
        }

        /// <summary>
        /// 从数据包创建命令 - 主入口方法
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
                var executionContext = packet.ExecutionContext;

                // 1. 优先使用ExecutionContext中的类型信息
                if (executionContext?.CommandType != null && packet.CommandData != null && packet.CommandData.Length > 0)
                {
                    try
                    {
                        var command = CreateCommandFromBytes(packet.CommandData, executionContext.CommandType.FullName);
                        if (command != null)
                        {
                            InitializeCommandProperties(command, packet);
                            _logger?.LogDebug("使用ExecutionContext.CommandType创建命令: {CommandType}, BytesLength={BytesLength}",
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

                // 2. 获取命令类型
                Type commandType = executionContext?.CommandType ?? _commandScanner.GetCommandType(packet.CommandId);

                // 3. 处理字节数组命令数据
                if (packet.CommandData != null && packet.CommandData.Length > 0)
                {
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

                    // 尝试从扩展属性中获取类型信息
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

                // 4. 使用命令类型创建具体命令
                if (commandType != null)
                {
                    var command = CreateEmptyCommand(commandType);
                    if (command != null)
                    {
                        InitializeCommandProperties(command, packet);
                        return command;
                    }
                }

                // 5. 最后使用泛型命令作为后备
                return CreateGenericCommand(packet);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "创建命令对象时出错: CommandId={CommandId}", packet.CommandId);
                return CreateFallbackCommand(packet, ex);
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

                // 使用配置的MessagePack选项进行反序列化
                return MessagePackSerializer.Deserialize(commandType, data, UnifiedSerializationService.MessagePackOptions) as ICommand;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "从字节数组创建命令失败: TypeName={TypeName}", typeName);
                throw new InvalidOperationException($"从字节数组创建命令失败: {typeName}", ex);
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
                            // 设置时间戳
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
        /// 根据类型创建空命令实例
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <returns>空命令实例</returns>
        private ICommand CreateEmptyCommand(Type commandType)
        {
            if (commandType == null) return null;
            return Activator.CreateInstance(commandType) as ICommand;
        }

        /// <summary>
        /// 创建泛型命令作为后备方案
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>泛型命令对象</returns>
        public ICommand CreateGenericCommand(PacketModel packet)
        {
            if (packet.CommandData != null && packet.CommandData.Length > 0)
            {
                var executionContext = packet.ExecutionContext;

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

            // 最后使用空的泛型命令
            return new GenericCommand<Dictionary<string, object>>(packet.CommandId, new Dictionary<string, object>());
        }

        /// <summary>
        /// 根据类型创建命令实例
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <param name="packet">数据包</param>
        /// <returns>创建的命令对象</returns>
        public ICommand CreateCommandByType(Type commandType, PacketModel packet)
        {
            if (commandType == null) throw new ArgumentNullException(nameof(commandType));
            if (packet == null) throw new ArgumentNullException(nameof(packet));

            try
            {
                var command = CreateEmptyCommand(commandType);
                if (command != null)
                {
                    InitializeCommandProperties(command, packet);
                    _logger?.LogDebug("使用类型创建命令: {CommandType}", commandType.Name);
                }
                return command;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "根据类型创建命令失败: {CommandType}", commandType.Name);
                throw new InvalidOperationException($"根据类型创建命令失败: {commandType.Name}", ex);
            }
        }

        /// <summary>
        /// 从JSON字符串创建命令
        /// </summary>
        /// <param name="json">JSON字符串</param>
        /// <param name="typeName">类型名称</param>
        /// <returns>创建的命令对象</returns>
        public ICommand CreateCommandFromJson(string json, string typeName)
        {
            if (string.IsNullOrEmpty(json)) throw new ArgumentException("JSON字符串不能为空", nameof(json));
            if (string.IsNullOrEmpty(typeName)) throw new ArgumentException("类型名称不能为空", nameof(typeName));

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

                // 使用System.Text.Json进行反序列化
                return global::System.Text.Json.JsonSerializer.Deserialize(json, commandType, new global::System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) as ICommand;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "从JSON创建命令失败: TypeName={TypeName}", typeName);
                throw new InvalidOperationException($"从JSON创建命令失败: {typeName}", ex);
            }
        }

        /// <summary>
        /// 初始化命令对象的基本属性
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="packet">源数据包</param>
        public void InitializeCommandProperties(ICommand command, PacketModel packet)
        {
            if (command == null || packet == null)
                return;

            // 设置基本属性
            command.TimestampUtc = packet.TimestampUtc;
            command.CreatedTimeUtc = packet.CreatedTimeUtc;

            // 如果命令是BaseCommand类型，设置更多属性
            if (command is BaseCommand baseCommand)
            {
                // 可以在这里添加更多BaseCommand特有的属性初始化
                baseCommand.TimestampUtc = packet.TimestampUtc;
                baseCommand.CreatedTimeUtc = packet.CreatedTimeUtc;
            }
        }

        /// <summary>
        /// 创建后备命令（当所有方法都失败时）
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <param name="exception">异常信息</param>
        /// <returns>后备命令对象</returns>
        private ICommand CreateFallbackCommand(PacketModel packet, Exception exception)
        {
            _logger?.LogWarning("使用后备命令，原命令创建失败: CommandId={CommandId}, Error={Error}",
                packet.CommandId, exception.Message);

            // 创建一个包含错误信息的泛型命令
            var fallbackData = new Dictionary<string, object>
            {
                ["Error"] = exception.Message,
                ["CommandId"] = packet.CommandId.ToString(),
                ["Fallback"] = true
            };

            return new GenericCommand<Dictionary<string, object>>(packet.CommandId, fallbackData);
        }
    }
}