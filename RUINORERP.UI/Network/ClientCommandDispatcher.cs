using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Protocol;
using RUINORERP.PacketSpec.Serialization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 客户端命令调度器
    /// 负责客户端命令的创建、分发和处理
    /// 实现ICommandDispatcher接口以保持与服务器端的一致性
    /// </summary>
    public class ClientCommandDispatcher : ICommandDispatcher
    {
        private readonly CommandTypeHelper _commandTypeHelper;
        private readonly ConcurrentDictionary<string, ICommand> _commandInstances;
        private readonly object _lockObject = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        public ClientCommandDispatcher(CommandTypeHelper commandTypeHelper = null)
        {
            _commandTypeHelper = commandTypeHelper ?? new CommandTypeHelper();
            _commandInstances = new ConcurrentDictionary<string, ICommand>();
            
            // 自动注册客户端命令
            RegisterClientCommands();
        }

        /// <summary>
        /// 注册客户端命令类型
        /// </summary>
        /// <param name="commandCode">命令代码</param>
        /// <param name="commandType">命令类型</param>
        public void RegisterCommand(uint commandCode, Type commandType)
        {
            _commandTypeHelper.RegisterCommandType(commandCode, commandType);
        }

        /// <summary>
        /// 创建命令实例
        /// </summary>
        /// <param name="commandCode">命令代码</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>命令实例</returns>
        public ICommand CreateCommand(uint commandCode, params object[] parameters)
        {
            try
            {
                var command = _commandTypeHelper.CreateCommand(commandCode);
                _commandInstances.TryAdd(command.CommandId, command);
                return command;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"创建命令实例失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 创建命令实例（实现ICommandDispatcher接口）
        /// </summary>
        /// <param name="commandCode">命令代码</param>
        /// <returns>命令实例</returns>
        ICommand ICommandDispatcher.CreateCommand(uint commandCode)
        {
            try
            {
                var command = _commandTypeHelper.CreateCommand(commandCode);
                _commandInstances.TryAdd(command.CommandId, command);
                return command;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"创建命令实例失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取命令实例
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <returns>命令实例</returns>
        public ICommand GetCommand(string commandId)
        {
            _commandInstances.TryGetValue(commandId, out var command);
            return command;
        }

        /// <summary>
        /// 移除命令实例
        /// </summary>
        /// <param name="commandId">命令ID</param>
        public void RemoveCommand(string commandId)
        {
            _commandInstances.TryRemove(commandId, out _);
        }

        /// <summary>
        /// 清理过期的命令实例
        /// </summary>
        /// <param name="expirationMinutes">过期分钟数</param>
        public void CleanupExpiredCommands(int expirationMinutes = 30)
        {
            var cutoffTime = DateTime.UtcNow.AddMinutes(-expirationMinutes);
            var expiredCommands = _commandInstances
                .Where(kvp => kvp.Value.CreatedAt < cutoffTime)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var commandId in expiredCommands)
            {
                _commandInstances.TryRemove(commandId, out _);
            }
        }

        /// <summary>
        /// 获取所有已注册的命令类型
        /// </summary>
        /// <returns>命令类型字典</returns>
        public IReadOnlyDictionary<uint, Type> GetRegisteredCommandTypes()
        {
            return _commandTypeHelper.GetRegisteredCommandTypes();
        }

        /// <summary>
        /// 获取所有活动的命令实例
        /// </summary>
        /// <returns>命令实例字典</returns>
        public IReadOnlyDictionary<string, ICommand> GetActiveCommands()
        {
            return new ReadOnlyDictionary<string, ICommand>(_commandInstances);
        }

        /// <summary>
        /// 自动注册客户端命令
        /// </summary>
        private void RegisterClientCommands()
        {
            // 获取当前程序集中的所有命令类型
            var commandTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(ICommand).IsAssignableFrom(t) &&
                           !t.IsInterface &&
                           !t.IsAbstract &&
                           t.Namespace != null &&
                           t.Namespace.StartsWith("RUINORERP.UI.Network.Commands"));

            foreach (var commandType in commandTypes)
            {
                try
                {
                    // 尝试通过CommandIdentifier属性获取命令ID
                    var commandInstance = Activator.CreateInstance(commandType) as ICommand;
                    if (commandInstance != null)
                    {
                        var commandId = commandInstance.CommandIdentifier.FullCode;
                        RegisterCommand(commandId, commandType);
                    }
                }
                catch (Exception ex)
                {
                    // 在实际应用中应添加日志记录
                    Console.WriteLine($"注册命令类型 {commandType.Name} 失败: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 获取所有注册的命令类型（实现ICommandDispatcher接口）
        /// </summary>
        /// <returns>命令类型字典</returns>
        public Dictionary<uint, Type> GetAllCommandTypes()
        {
            return _commandTypeHelper.GetAllCommandTypes();
        }

        /// <summary>
        /// 清理注册的命令类型
        /// </summary>
        public void ClearCommandTypes()
        {
            _commandTypeHelper.Clear();
        }

        #region ICommandDispatcher 接口实现

        /// <summary>
        /// 初始化命令调度器
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>初始化结果</returns>
        public Task<bool> InitializeAsync(CancellationToken cancellationToken = default)
        {
            // 客户端命令调度器的初始化逻辑
            return Task.FromResult(true);
        }

        /// <summary>
        /// 分发命令（客户端实现）
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令结果</returns>
        public Task<CommandResult> DispatchAsync(ICommand command, CancellationToken cancellationToken = default)
        {
            // 客户端命令分发逻辑
            // 在实际应用中，这里应该将命令发送到服务器
            return Task.FromResult(CommandResult.Success());
        }

        /// <summary>
        /// 注册命令类型
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="commandType">命令类型</param>
        public void RegisterCommandType(uint commandId, Type commandType)
        {
            RegisterCommand(commandId, commandType);
        }

        /// <summary>
        /// 获取命令类型
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <returns>命令类型</returns>
        public Type GetCommandType(uint commandId)
        {
            return _commandTypeHelper.GetCommandType(commandId);
        }

        #endregion
    }
}