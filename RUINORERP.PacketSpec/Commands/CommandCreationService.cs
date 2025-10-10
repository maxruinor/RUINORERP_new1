using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
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
    /// 合并了 DefaultCommandFactory 的功能，提供统一的命令创建入口
    /// </summary>
    public class CommandCreationService : ICommandCreationService
    {
        private readonly ILogger<CommandCreationService> _logger;
        private readonly CommandScanner _commandScanner;
        private readonly CommandCacheManager _cacheManager;
        private readonly Dictionary<CommandId, Func<PacketModel, ICommand>> _commandCreators;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="commandScanner">命令扫描器</param>
        /// <param name="cacheManager">缓存管理器</param>
        public CommandCreationService(ILogger<CommandCreationService> logger = null, CommandScanner commandScanner = null, CommandCacheManager cacheManager = null)
        {
            _logger = logger;
            _commandScanner = commandScanner ?? new CommandScanner();
            _cacheManager = cacheManager ?? new CommandCacheManager(true);
            _commandCreators = new Dictionary<CommandId, Func<PacketModel, ICommand>>();
        }

        /// <summary>
        /// 从数据包创建命令 - 主入口方法（合并了 DefaultCommandFactory 的功能）
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

                // 1. 首先尝试从缓存获取命令创建器
                var cachedCreator = _cacheManager.GetCachedCommandCreator(commandId);
                if (cachedCreator != null)
                {
                    var command = cachedCreator(packet);
                    if (command != null)
                    {
                        InitializeCommandProperties(command, packet);
                        _logger?.LogDebug("从缓存创建器获取命令: {CommandId}", commandId);
                        return command;
                    }
                }

                // 2. 从数据包提取类型化命令（来自 DefaultCommandFactory 的优化）
                var typedCommand = ExtractTypedCommand(packet);
                if (typedCommand != null)
                {
                    _logger?.LogDebug("成功从数据包提取类型化命令: {CommandType}", typedCommand.GetType().Name);
                    // 缓存命令创建器以供后续使用
                    _cacheManager.CacheCommandCreator(commandId, packet => typedCommand);
                    return typedCommand;
                }

                var executionContext = packet.ExecutionContext;

                // 3. 检查是否有注册的自定义创建器（来自 DefaultCommandFactory）
                if (_commandCreators.TryGetValue(commandId, out var creator))
                {
                    var command = creator(packet);
                    if (command != null)
                    {
                        InitializeCommandProperties(command, packet);
                        // 缓存命令创建器以供后续使用
                        _cacheManager.CacheCommandCreator(commandId, packet => command);
                        return command;
                    }
                }

                // 4. 优先使用ExecutionContext中的类型信息
                if (executionContext?.CommandType != null && packet.CommandData != null && packet.CommandData.Length > 0)
                {
                    try
                    {
                        var command = CreateCommandFromBytes(packet.CommandData, executionContext.CommandType.FullName);
                        if (command != null)
                        {
                            InitializeCommandProperties(command, packet);
                            // 缓存命令创建器以供后续使用
                            _cacheManager.CacheCommandCreator(commandId, packet => command);
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

                // 5. 获取命令类型（优先缓存）
                Type commandType = executionContext?.CommandType ?? null;
                if (commandType == null)
                {
                    commandType = _commandScanner.GetCommandType(commandId);
                    if (commandType != null)
                    {
                        _cacheManager.CacheCommandType(commandId, commandType);
                    }
                }

                // 6. 处理字节数组命令数据
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
                                // 缓存命令创建器以供后续使用
                                _cacheManager.CacheCommandCreator(commandId, packet => command);
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
                                // 缓存命令创建器以供后续使用
                                _cacheManager.CacheCommandCreator(commandId, packet => command);
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

                // 7. 使用命令类型创建具体命令
                if (commandType != null)
                {
                    var command = CreateEmptyCommand(commandType);
                    if (command != null)
                    {
                        InitializeCommandProperties(command, packet);
                        // 缓存命令创建器以供后续使用
                        _cacheManager.CacheCommandCreator(commandId, packet => command);
                        return command;
                    }
                }

                // 8. 最后使用泛型命令作为后备
                var fallbackCommand = CreateGenericCommand(packet);
                if (fallbackCommand != null)
                {
                    // 缓存泛型命令创建器
                    _cacheManager.CacheCommandCreator(commandId, packet => fallbackCommand);
                }
                return fallbackCommand;
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
                // 1. 优先从缓存获取类型 - 注意：GetCachedCommandType需要CommandId参数，这里使用字符串作为后备方案
                var commandType = (Type)null; // 暂时设置为null，后续从扫描器获取
                if (commandType == null)
                {
                    // 2. 缓存未命中，从扫描器获取
                    commandType = _commandScanner.GetCommandTypeByName(typeName);
                    if (commandType == null)
                    {
                        // 3. 尝试从已注册的类型中查找
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

                    if (commandType != null)
                    {
                        // 缓存类型 - 注意：CacheCommandType需要CommandId参数，这里无法提供，跳过缓存
                    }
                }

                if (commandType == null)
                    throw new ArgumentException($"未知的命令类型: {typeName}", nameof(typeName));

                var ss = MessagePackSerializer.Deserialize(commandType, data, UnifiedSerializationService.MessagePackOptions);

                // 4. 使用配置的MessagePack选项进行反序列化
                var command = MessagePackSerializer.Deserialize(commandType, data, UnifiedSerializationService.MessagePackOptions) as ICommand;
                if (command != null)
                {
                    // 注意：这里无法获取commandId，只能缓存构造函数
                    _cacheManager.CacheConstructor(commandType, () => command);
                }
                return command;
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
        /// 创建空命令实例
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <returns>空命令实例</returns>
        public ICommand CreateEmptyCommand(Type commandType)
        {
            try
            {
                if (commandType == null)
                {
                    _logger?.LogWarning("命令类型为空");
                    return CreateFallbackCommand(null, new ArgumentNullException(nameof(commandType)));
                }

                // 1. 优先使用缓存管理器获取构造函数
                var constructor = _cacheManager.GetOrCreateConstructor(commandType);
                var command = constructor?.Invoke();
                if (command != null)
                {
                    _logger?.LogDebug($"使用缓存管理器创建空命令实例: {commandType.Name}");
                    return command;
                }

                // 2. 如果没有无参构造函数，尝试使用Activator
                try
                {
                    command = Activator.CreateInstance(commandType) as ICommand;
                    if (command != null)
                    {
                        // 缓存到缓存管理器
                        _cacheManager.CacheConstructor(commandType, () => command);
                        _logger?.LogDebug($"使用Activator创建空命令实例: {commandType.Name}");
                        return command;
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogWarning(ex, $"使用Activator创建实例失败: {commandType.Name}");
                }

                _logger?.LogWarning($"无法创建命令实例: {commandType.Name}");
                return CreateFallbackCommand(null, new InvalidOperationException($"无法创建命令实例: {commandType.Name}"));
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"创建空命令实例时发生异常: {commandType?.Name}");
                return CreateFallbackCommand(null, ex);
            }
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

        /// <summary>
        /// 异步创建命令实例（从命令ID和参数字典）- 来自 DefaultCommandFactory
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>创建的命令对象</returns>
        public async Task<ICommand> CreateCommandAsync(string commandId, Dictionary<string, object> parameters = null)
        {
            if (string.IsNullOrEmpty(commandId))
            {
                _logger?.LogError("命令ID为空");
                return null;
            }

            try
            {
                if (!CommandId.TryParse(commandId, out var cmdId))
                {
                    _logger?.LogError($"命令ID解析失败: {commandId}");
                    return null;
                }

                // 1. 首先尝试从缓存获取命令实例
                // 注意：CommandCacheManager 没有 GetCommand 方法，跳过缓存检查步骤

                // 2. 创建命令实例
                var command = await CreateCommandInstanceAsync(commandId);
                if (command == null)
                {
                    return null;
                }

                // 3. 设置命令参数
                if (parameters != null && parameters.Count > 0)
                {
                    await SetCommandParametersAsync(command, parameters);
                }

                // 4. 缓存最终命令实例
                // 注意：CommandCacheManager 没有 CacheCommand 方法，使用命令创建器缓存代替
                _cacheManager.CacheCommandCreator(cmdId, packet => command);

                _logger?.LogDebug($"成功创建命令实例: {commandId}");
                return command;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"创建命令 {commandId} 时发生异常");
                return null;
            }
        }



        /// <summary>
        /// 创建命令实例 - 提取为独立方法（来自 DefaultCommandFactory）
        /// </summary>
        private async Task<ICommand> CreateCommandInstanceAsync(string commandId)
        {
            // 1. 解析命令ID
            CommandId cmdId;
            if (!CommandId.TryParse(commandId, out cmdId))
            {
                _logger?.LogWarning($"命令ID解析失败: {commandId}");
                return null;
            }

            // 2. 优先从缓存获取命令类型
            var commandType = _cacheManager.GetCachedCommandType(cmdId);
            if (commandType == null)
            {
                // 3. 缓存未命中，从扫描器获取
                var allTypes = _commandScanner.GetAllCommandTypes();
                if (!allTypes.TryGetValue(cmdId, out commandType))
                {
                    _logger?.LogWarning($"未找到命令类型: {commandId}");
                    return null;
                }

                // 4. 缓存命令类型
                _cacheManager.CacheCommandType(cmdId, commandType);
            }

            // 5. 优先使用缓存管理器创建命令实例
            var constructor = _cacheManager.GetOrCreateConstructor(commandType);
            var command = constructor?.Invoke();
            if (command == null)
            {
                // 6. 缓存创建失败，使用传统方式创建
                command = Activator.CreateInstance(commandType) as ICommand;
                if (command != null)
                {
                    // 缓存构造函数以供后续使用
                    _cacheManager.CacheConstructor(commandType, () => command);
                }
            }

            if (command == null)
            {
                _logger?.LogError($"创建命令实例失败: {commandId}");
                return null;
            }

            return command;
        }

        /// <summary>
        /// 设置命令参数 - 来自 DefaultCommandFactory
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="parameters">参数字典</param>
        private async Task SetCommandParametersAsync(ICommand command, Dictionary<string, object> parameters)
        {
            if (command == null || parameters == null || parameters.Count == 0)
                return;

            var commandType = command.GetType();
            var properties = commandType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var param in parameters)
            {
                var property = properties.FirstOrDefault(p =>
                    string.Equals(p.Name, param.Key, StringComparison.OrdinalIgnoreCase));

                if (property != null && property.CanWrite)
                {
                    try
                    {
                        var value = param.Value;
                        if (value != null && !property.PropertyType.IsAssignableFrom(value.GetType()))
                        {
                            // 尝试类型转换
                            var converter = TypeDescriptor.GetConverter(property.PropertyType);
                            if (converter != null && converter.CanConvertFrom(value.GetType()))
                            {
                                value = converter.ConvertFrom(value);
                            }
                            else
                            {
                                _logger?.LogWarning($"参数 {param.Key} 类型不匹配: 期望 {property.PropertyType.Name}, 实际 {value.GetType().Name}");
                                continue;
                            }
                        }

                        property.SetValue(command, value);
                        _logger?.LogDebug($"设置命令参数: {param.Key} = {value}");
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, $"设置命令参数失败: {param.Key}");
                    }
                }
                else
                {
                    _logger?.LogWarning($"命令类型 {commandType.Name} 不包含属性: {param.Key}");
                }
            }
        }

        /// <summary>
        /// 注册命令创建器 - 来自 DefaultCommandFactory
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
        /// 获取缓存统计信息
        /// </summary>
        /// <returns>缓存统计信息</returns>
        public Dictionary<string, object> GetCacheStatistics()
        {
            var stats = _cacheManager.GetCacheStatistics();
            return new Dictionary<string, object>
            {
                ["CommandTypeCacheCount"] = stats.CommandTypeCacheCount,
                ["CommandTypeByNameCacheCount"] = stats.CommandTypeByNameCacheCount,
                ["ConstructorCacheCount"] = stats.ConstructorCacheCount,
                ["CommandCreatorCacheCount"] = stats.CommandCreatorCacheCount,
                ["ScanResultCacheCount"] = stats.ScanResultCacheCount,
                ["HandlerTypeCacheCount"] = stats.HandlerTypeCacheCount,
                ["TotalCacheEntries"] = stats.CommandTypeCacheCount + stats.CommandTypeByNameCacheCount +
                                     stats.ConstructorCacheCount + stats.CommandCreatorCacheCount +
                                     stats.HandlerTypeCacheCount + stats.ScanResultCacheCount +
                                     stats.AssemblyMetadataCacheCount
            };
        }

        /// <summary>
        /// 清理缓存
        /// </summary>
        public void ClearCache()
        {
            _cacheManager.ClearAllCaches();
            _logger?.LogInformation("命令创建服务缓存已清理");
        }

    }
}
