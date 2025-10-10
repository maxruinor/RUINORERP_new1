using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Models.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;
using System.ComponentModel;
using Newtonsoft.Json;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 默认命令工厂实现 - 负责根据命令ID创建对应的命令对象（重构版：提取类型扫描逻辑）
    /// </summary>
    public class DefaultCommandFactory : ICommandFactoryAsync
    {
        private readonly ILogger<DefaultCommandFactory> _logger;
        private readonly Dictionary<CommandId, Func<PacketModel, ICommand>> _commandCreators;
        private readonly CommandScanner _commandScanner;
        private readonly ICommandCreationService _commandCreationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="commandScanner">命令扫描器</param>
        /// <param name="commandCreationService">命令创建服务</param>
        public DefaultCommandFactory(ILogger<DefaultCommandFactory> logger = null, CommandScanner commandScanner = null, ICommandCreationService commandCreationService = null)
        {
            _logger = logger;
            _commandCreators = new Dictionary<CommandId, Func<PacketModel, ICommand>>();
            _commandScanner = commandScanner ?? new CommandScanner();
            _commandCreationService = commandCreationService ?? new CommandCreationService(null, _commandScanner);
        }




        /// <summary>
        /// 从统一数据包创建命令
        /// </summary>
        /// <param name="packet">统一数据包</param>
        /// <returns>命令对象，如果无法创建则返回null</returns>
        public ICommand CreateCommand(PacketModel packet)
        {
            if (packet == null)
            {
                _logger?.LogWarning("尝试从空的PacketModel创建命令");
                return null;
            }

            try
            {

                // 首先尝试从数据包中提取类型化命令
                var typedCommand = CommandPacketAdapterExtensions.ExtractCommand(packet, _commandCreationService);
                if (typedCommand != null)
                {
                    _logger?.LogDebug("成功从数据包提取类型化命令: {CommandType}", typedCommand.GetType().Name);
                    return typedCommand;
                }

                var comm = packet.GetJsonData<BaseCommand>();

                // 获取命令ID
                CommandId commandId = packet.CommandId;

                // 首先检查是否有注册的自定义创建器
                if (_commandCreators.TryGetValue(commandId, out var creator))
                {
                    var command = creator(packet);
                    if (command != null)
                    {
                        InitializeCommand(command, packet);
                        return command;
                    }
                }

                // 然后尝试从命令扫描器中获取命令类型并创建实例
                var commandType = _commandScanner.GetCommandType(commandId);
                if (commandType != null)
                {
                    try
                    {
                        var command = Activator.CreateInstance(commandType) as ICommand;
                        if (command != null)
                        {
                            InitializeCommand(command, packet);
                            return command;
                        }
                        else
                        {
                            _logger?.LogWarning("无法将类型 {CommandType} 转换为 ICommand", commandType.FullName);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "创建命令实例失败: {CommandType}", commandType.FullName);
                    }
                }

                //新的简化版本测试 
                // 如果无法创建命令，返回null 
                // 2. 使用CommandScanner获取有效载荷类型
                var payloadType = _commandScanner.GetCommandType(commandId);
                if (payloadType != null)
                {
                    //var closedType = typeof(GenericCommand<>).MakeGenericType(payloadType);
                    //return (ICommand)Activator.CreateInstance(closedType, commandId, null);

                    // 2.1 先从缓存里拿"开放泛型定义"
                    if (!_commandScanner.GetAllCommandTypes().TryGetValue(new CommandId(CommandCategory.System, 0xEE, "GenericCommandTemplate"), out var openGeneric))
                        throw new InvalidOperationException("GenericCommand<> 模板未注册");

                    // 2.2 MakeGenericType 产生封闭类型
                    var closedType = openGeneric.MakeGenericType(payloadType);
                    return (ICommand)Activator.CreateInstance(closedType, commandId, null);

                }


                _logger?.LogWarning("未找到命令ID: {CommandId} 对应的命令类型", commandId.ToString());
                return null;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "创建命令时发生异常");
                return null;
            }
        }


        /// <summary>
        /// 创建命令实例（重构版：提取参数设置逻辑）
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>命令实例</returns>
        public async Task<ICommand> CreateCommandAsync(string commandId, Dictionary<string, object> parameters = null)
        {
            if (string.IsNullOrEmpty(commandId))
            {
                _logger?.LogError("命令ID为空");
                return null;
            }

            try
            {
                var command = await CreateCommandInstanceAsync(commandId);
                if (command == null)
                {
                    return null;
                }

                // 设置命令参数
                if (parameters != null && parameters.Count > 0)
                {
                    await SetCommandParametersAsync(command, parameters);
                }

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
        /// 创建命令实例 - 提取为独立方法
        /// </summary>
        private async Task<ICommand> CreateCommandInstanceAsync(string commandId)
        {
            // 尝试从缓存中获取命令类型
            if (!CommandId.TryParse(commandId, out var cmdId) || !_commandScanner.GetAllCommandTypes().TryGetValue(cmdId, out var commandType))
            {
                _logger?.LogWarning($"未找到命令类型: {commandId}");
                return null;
            }

            // 创建命令实例
            var command = Activator.CreateInstance(commandType) as ICommand;
            if (command == null)
            {
                _logger?.LogError($"创建命令实例失败: {commandId}");
                return null;
            }

            return command;
        }

        /// <summary>
        /// 设置命令参数（重构版：提取参数转换逻辑）
        /// </summary>
        private async Task SetCommandParametersAsync(ICommand command, Dictionary<string, object> parameters)
        {
            var commandType = command.GetType();
            
            foreach (var param in parameters)
            {
                try
                {
                    await SetCommandPropertyAsync(command, commandType, param.Key, param.Value);
                }
                catch (Exception ex)
                {
                    _logger?.LogWarning(ex, $"设置命令参数 {param.Key} 时发生异常");
                }
            }
        }

        /// <summary>
        /// 设置命令属性 - 提取为独立方法
        /// </summary>
        private async Task SetCommandPropertyAsync(ICommand command, Type commandType, string propertyName, object value)
        {
            var property = commandType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (property == null || !property.CanWrite)
            {
                _logger?.LogWarning($"命令 {commandType.Name} 不存在属性 {propertyName} 或属性不可写");
                return;
            }

            var convertedValue = ConvertParameterValue(value, property.PropertyType);
            property.SetValue(command, convertedValue);
        }

        /// <summary>
        /// 转换参数值类型
        /// </summary>
        private object ConvertParameterValue(object value, Type targetType)
        {
            if (value == null)
            {
                return null;
            }

            var valueType = value.GetType();
            if (targetType.IsAssignableFrom(valueType))
            {
                return value;
            }

            try
            {
                // 处理可空类型
                var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;
                
                // 特殊处理枚举类型
                if (underlyingType.IsEnum)
                {
                    if (value is string stringValue)
                    {
                        return Enum.Parse(underlyingType, stringValue, true);
                    }
                    return Enum.ToObject(underlyingType, value);
                }

                // 使用类型转换器
                var converter = TypeDescriptor.GetConverter(underlyingType);
                if (converter.CanConvertFrom(valueType))
                {
                    return converter.ConvertFrom(value);
                }

                // 尝试强制转换
                return Convert.ChangeType(value, underlyingType);
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, $"参数值类型转换失败: {valueType.Name} -> {targetType.Name}");
                throw;
            }
        }

        /// <summary>
        /// 异步从统一数据包创建命令
        /// </summary>
        /// <param name="packet">统一数据包</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令对象</returns>
        public async Task<ICommand> CreateCommandAsync(PacketModel packet, CancellationToken cancellationToken = default)
        {
            // 异步包装同步方法
            return await Task.Run(() => CreateCommand(packet), cancellationToken);
        }

        /// <summary>
        /// 从JSON和类型名称创建命令实例
        /// </summary>
        /// <param name="json">JSON字符串</param>
        /// <param name="typeName">类型名称</param>
        /// <returns>命令实例</returns>
        public ICommand CreateCommandFromJson(string json, string typeName)
        {
            return _commandCreationService.CreateCommandFromJson(json, typeName);
        }

        /// <summary>
        /// 从byte[]和类型名称创建命令实例
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <param name="typeName">类型名称</param>
        /// <returns>命令实例</returns>
        public ICommand CreateCommandFromBytes(byte[] data, string typeName)
        {
            return _commandCreationService.CreateCommandFromBytes(data, typeName);
        }

        /// <summary>
        /// 从CommandId创建空命令实例
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <returns>空命令实例</returns>
        public ICommand CreateEmptyCommand(CommandId commandId)
        {
            return _commandCreationService.CreateEmptyCommand(commandId);
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
        /// 初始化命令对象
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="packet">数据包</param>
        private void InitializeCommand(ICommand command, PacketModel packet)
        {
            if (command == null || packet == null)
                return;
            // 如果命令是BaseCommand类型，设置更多属性
            if (command is BaseCommand baseCommand)
            {
                packet.GetJsonData<BaseCommand>();
                baseCommand.TimestampUtc = packet.TimestampUtc;
                baseCommand.CreatedTimeUtc = packet.CreatedTimeUtc;
            }
        }

        /// <summary>
        /// 从OriginalData创建PacketModel
        /// </summary>
        /// <param name="originalData">原始数据</param>
        /// <returns>创建的PacketModel对象</returns>
        [Obsolete("暂时作废")]
        private PacketModel CreatePacketModelFromOriginalData(OriginalData originalData)
        {
            try
            {
                var packet = new PacketModel
                {
                    PacketId = Guid.NewGuid().ToString(),
                    CreatedTimeUtc = DateTime.UtcNow,
                    TimestampUtc = DateTime.UtcNow,
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

                packet.CommandId = new CommandId(category, operationCode);

                return packet;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "从OriginalData创建PacketModel时发生异常");
                return null;
            }
        }
    }
}
