using Microsoft.Extensions.Logging;
using NPOI.SS.Formula.Functions;
using RUINORERP.Business;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;

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

    /// </summary>
    public class ClientCommandDispatcher
    {
        private readonly CommandScanner _commandScanner;
        private readonly ConcurrentDictionary<ushort, ICommand> _commandInstances;
        private readonly object _lockObject = new object();
        public readonly ILogger<ClientCommandDispatcher> _logger;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_commandScanner">命令类型助手，可选参数，用于管理命令类型映射关系</param>
        public ClientCommandDispatcher(ILogger<ClientCommandDispatcher> logger, CommandScanner _commandScanner = null)
        {
            this._commandScanner = _commandScanner ?? new CommandScanner();
            _commandInstances = new ConcurrentDictionary<ushort, ICommand>();
            _logger = logger;
            // 自动注册客户端命令
            RegisterClientCommands();
        }

        /// <summary>
        /// 注册客户端命令类型
        /// </summary>
        /// <param name="commandCode">命令代码，唯一标识命令的数值</param>
        /// <param name="commandType">命令类型，命令类的Type对象</param>
        /// <exception cref="ArgumentNullException">当命令类型为空时抛出</exception>
        public void RegisterCommand(CommandId commandCode, Type commandType)
        {
            if (commandType == null)
            {
                throw new ArgumentNullException(nameof(commandType));
            }

            _commandScanner.RegisterCommandType(commandCode, commandType);
        }

        /// <summary>
        /// 创建命令实例
        /// </summary>
        /// <param name="commandCode">命令代码</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>命令实例</returns>
        /// <exception cref="InvalidOperationException">当创建命令实例失败时抛出</exception>
        public ICommand CreateCommand(uint commandCode, params object[] parameters)
        {
            try
            {
                //var command = _commandTypeHelper.CreateCommand(commandCode, parameters);
                var command = CreateCommand(commandCode);
                if (command != null)
                {
                    _commandInstances.TryAdd(command.CommandIdentifier, command);
                }
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
        /// <exception cref="InvalidOperationException">当创建命令实例失败时抛出</exception>
        ICommand CreateCommand(uint commandCode)
        {
            try
            {
                var command = CreateCommand(commandCode);
                if (command != null)
                {
                    _commandInstances.TryAdd(command.CommandIdentifier, command);
                }
                return command;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"创建命令实例失败: {ex.Message}", ex);
            }
        }




        /// <summary>
        /// 清理过期的命令实例
        /// 自动清理超过指定时间的命令实例，释放内存资源
        /// </summary>
        /// <param name="expirationMinutes">过期分钟数，默认30分钟</param>
        public void CleanupExpiredCommands(int expirationMinutes = 30)
        {
            if (expirationMinutes <= 0)
            {
                expirationMinutes = 30; // 确保最小值为30分钟
            }

            var cutoffTime = DateTime.UtcNow.AddMinutes(-expirationMinutes);
            var expiredCommands = _commandInstances
                .Where(kvp => kvp.Value.CreatedTimeUtc < cutoffTime)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var commandId in expiredCommands)
            {
                _commandInstances.TryRemove(commandId, out _);
            }
        }




        /// <summary>
        /// 获取所有活动的命令实例
        /// </summary>
        /// <returns>命令实例的只读字典</returns>
        public IReadOnlyDictionary<ushort, ICommand> GetActiveCommands()
        {
            return new ReadOnlyDictionary<ushort, ICommand>(_commandInstances);
        }

        /// <summary>
        /// 自动注册客户端命令
        /// 扫描程序集中所有实现了ICommand接口的类，并自动注册
        /// </summary>
        private void RegisterClientCommands()
        {
            try
            {
                // 获取当前程序集中的所有命令类型
                var assembliesToScan = new List<Assembly>
                {
                    Assembly.GetExecutingAssembly(), // 客户端程序集
                    Assembly.GetAssembly(typeof(PacketSpec.Commands.ICommand)) // PacketSpec程序集
                };

                foreach (var assembly in assembliesToScan)
                {
                    if (assembly == null) continue;

                    var commandTypes = assembly
                        .GetTypes()
                        .Where(t => typeof(ICommand).IsAssignableFrom(t) &&
                                   !t.IsInterface &&
                                   !t.IsAbstract);

                    foreach (var commandType in commandTypes)
                    {
                        try
                        {
                            // 检查命令是否使用了PacketCommandAttribute特性
                            var commandAttribute = commandType.GetCustomAttribute<PacketCommandAttribute>();
                            if (commandAttribute != null)
                            {
                                // 尝试通过CommandIdentifier属性获取命令ID
                                var commandInstance = Activator.CreateInstance(commandType) as ICommand;
                                if (commandInstance != null)
                                {
                                    var commandId = commandInstance.CommandIdentifier;
                                    RegisterCommand(commandId, commandType);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // 在实际应用中应添加日志记录
                            Console.WriteLine($"注册命令类型 {commandType.Name} 失败: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"自动注册命令类型时发生异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 清理注册的命令类型
        /// </summary>
        public void ClearCommandTypes()
        {
            _commandScanner.Clear();
        }

        #region ICommandDispatcher 接口实现



         /// <summary>
        /// 获取命令类型
        /// </summary>
        /// <param name="commandCode">命令代码</param>
        /// <returns>命令类型，如果找不到则返回null</returns>
        public Type GetCommandType(CommandId commandCode)
        {
            return _commandScanner.GetCommandType(commandCode);
        }
     

        public ICommand CreateCommand(CommandId commandCode)
        {
            try
            {
                // 使用预编译的构造函数创建命令实例
                var ctor = _commandScanner.GetCommandCtor(commandCode);
                var command = ctor();
                if (command != null)
                {
                    _logger.Debug($"创建命令实例成功: {commandCode}");
                }
                else
                {
                    _logger.Warn($"创建命令实例失败: {commandCode}");
                }
                return command;
            }
            catch (Exception ex)
            {
                _logger.Error($"创建命令实例异常: {commandCode}", ex);
                return null;
            }
        }

        #endregion
    }
}