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
using System.Data;
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
                _commandScanner.ScanCommands(null, true, assembliesToScan.ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"自动注册命令类型时发生异常: {ex.Message}");
            }
        }

 
 
    }
}