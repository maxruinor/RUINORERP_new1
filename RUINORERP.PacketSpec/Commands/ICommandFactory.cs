﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Protocol;
using Microsoft.Extensions.Logging;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// ✅ [最新架构] 命令工厂接口 - 定义从各种数据包格式创建统一命令对象的契约
    /// 支持依赖注入和灵活的扩展机制
    /// </summary>
    public interface ICommandFactory
    {
        /// <summary>
        /// 从统一数据包创建命令
        /// </summary>
        /// <param name="packet">统一数据包</param>
        /// <returns>命令对象</returns>
        ICommand CreateCommand(PacketModel packet);

        /// <summary>
        /// 从OriginalData创建命令
        /// </summary>
        /// <param name="originalData">原始数据</param>
        /// <returns>命令对象</returns>
        ICommand CreateCommand(OriginalData originalData);

        /// <summary>
        /// 注册命令创建器
        /// </summary>
        /// <param name="commandCode">命令代码</param>
        /// <param name="creator">创建器函数</param>
        void RegisterCommandCreator(uint commandCode, Func<PacketModel, ICommand> creator);
    }

    /// <summary>
    /// 异步命令工厂接口
    /// </summary>
    public interface ICommandFactoryAsync : ICommandFactory
    {
        /// <summary>
        /// 异步从统一数据包创建命令
        /// </summary>
        /// <param name="packet">统一数据包</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令对象</returns>
        Task<ICommand> CreateCommandAsync(PacketModel packet, CancellationToken cancellationToken = default);


        /// <summary>
        /// 异步从OriginalData创建命令
        /// </summary>
        /// <param name="originalData">原始数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令对象</returns>
        Task<ICommand> CreateCommandAsync(OriginalData originalData, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// 默认命令工厂实现
    /// 负责根据命令ID创建对应的命令对象
    /// </summary>
    public class DefaultCommandFactory : ICommandFactoryAsync
    {
        private readonly ILogger<DefaultCommandFactory> _logger;
        private readonly Dictionary<uint, Func<PacketModel, ICommand>> _commandCreators;
        private readonly CommandTypeHelper _commandTypeHelper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="commandTypeHelper">命令类型助手</param>
        public DefaultCommandFactory(ILogger<DefaultCommandFactory> logger = null, CommandTypeHelper commandTypeHelper = null)
        {
            _logger = logger;
            _commandCreators = new Dictionary<uint, Func<PacketModel, ICommand>>();
            _commandTypeHelper = commandTypeHelper ?? new CommandTypeHelper();
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
                // 获取命令ID
                uint commandId = (uint)packet.Command;

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

                // 然后尝试从命令类型助手中获取命令类型并创建实例
                var commandType = _commandTypeHelper.GetCommandType(commandId);
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

                _logger?.LogWarning("未找到命令ID: {CommandId} 对应的命令类型", commandId);
                return null;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "创建命令时发生异常");
                return null;
            }
        }

        /// <summary>
        /// 从OriginalData创建命令
        /// </summary>
        /// <param name="originalData">原始数据</param>
        /// <returns>命令对象，如果无法创建则返回null</returns>
        public ICommand CreateCommand(OriginalData originalData)
        {
            if (!originalData.IsValid)
            {
                _logger?.LogWarning("尝试从无效的OriginalData创建命令");
                return null;
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
                _logger?.LogError(ex, "从OriginalData创建命令时发生异常");
                return null;
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
        /// 异步从OriginalData创建命令
        /// </summary>
        /// <param name="originalData">原始数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令对象</returns>
        public async Task<ICommand> CreateCommandAsync(OriginalData originalData, CancellationToken cancellationToken = default)
        {
            // 异步包装同步方法
            return await Task.Run(() => CreateCommand(originalData), cancellationToken);
        }

        /// <summary>
        /// 注册命令创建器
        /// </summary>
        /// <param name="commandCode">命令代码</param>
        /// <param name="creator">创建器函数</param>
        public void RegisterCommandCreator(uint commandCode, Func<PacketModel, ICommand> creator)
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
            try
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
            catch (Exception ex)
            {
                _logger?.LogError(ex, "从OriginalData创建PacketModel时发生异常");
                return null;
            }
        }
    }
}
