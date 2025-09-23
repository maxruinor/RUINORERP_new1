using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 文件级别注释：
    /// 命令扫描器 - 统一的命令类型扫描工具
    /// 负责扫描指定程序集中的所有命令类型并提供注册功能
    /// 用于解决客户端和服务器端命令扫描机制不一致的问题
    /// </summary>
    public class CommandScanner
    {
        private readonly ILogger _logger;
        private readonly CommandTypeHelper _commandTypeHelper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器，可选参数</param>
        /// <param name="commandTypeHelper">命令类型助手，可选参数</param>
        public CommandScanner(ILogger logger = null, CommandTypeHelper commandTypeHelper = null)
        {
            _logger = logger;
            _commandTypeHelper = commandTypeHelper ?? new CommandTypeHelper();
        }

        /// <summary>
        /// 扫描并获取指定程序集中的所有命令类型
        /// </summary>
        /// <param name="assemblies">要扫描的程序集，可选参数</param>
        /// <param name="namespaceFilter">命名空间过滤器，可选参数，只扫描指定命名空间下的命令</param>
        /// <returns>命令类型和命令ID的映射字典</returns>
        public Dictionary<uint, Type> ScanCommands(string namespaceFilter = null, params Assembly[] assemblies)
        {
            var commandTypeMap = new Dictionary<uint, Type>();
            int totalCommandsFound = 0;

            if (assemblies == null || assemblies.Length == 0)
            {
                assemblies = new[] { Assembly.GetCallingAssembly() };
            }

            foreach (var assembly in assemblies)
            {
                try
                {
                    IEnumerable<Type> typesQuery = assembly.GetTypes()
                        .Where(t => typeof(ICommand).IsAssignableFrom(t) && 
                                  !t.IsAbstract && 
                                  !t.IsInterface);

                    // 如果指定了命名空间过滤器，则只扫描该命名空间下的命令
                    if (!string.IsNullOrEmpty(namespaceFilter))
                    {
                        typesQuery = typesQuery.Where(t => t.Namespace != null && 
                                                         t.Namespace.StartsWith(namespaceFilter));
                    }

                    var commandTypes = typesQuery.ToList();

                    foreach (var commandType in commandTypes)
                    {
                        try
                        {
                            // 获取命令特性
                            var commandAttribute = commandType.GetCustomAttribute<PacketCommandAttribute>();
                            uint commandId = 0;
                            string commandName = commandType.Name;

                            if (commandAttribute != null)
                            {
                                commandName = commandAttribute.Name;
                                // 尝试通过CommandIdentifier属性获取命令ID
                                var commandInstance = Activator.CreateInstance(commandType) as ICommand;
                                if (commandInstance != null)
                                {
                                    commandId = commandInstance.CommandIdentifier.FullCode;
                                }
                            }

                            // 如果命令ID为0，则使用类型的哈希码作为后备方案
                            if (commandId == 0)
                            {
                                commandId = (uint)(commandType.FullName?.GetHashCode() ?? 0);
                                if (commandId == 0 || commandId == uint.MaxValue)
                                {
                                    commandId = (uint)commandType.Name.GetHashCode();
                                }
                            }

                            if (!commandTypeMap.ContainsKey(commandId))
                            {
                                commandTypeMap[commandId] = commandType;
                                totalCommandsFound++;
                                _logger?.LogDebug("扫描到命令: {CommandName} (ID: {CommandId})", commandName, commandId);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogWarning(ex, "处理命令类型 {TypeName} 时出错", commandType.FullName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "扫描程序集 {AssemblyName} 中的命令时出错", assembly.GetName().Name);
                }
            }

            _logger?.LogInformation("命令扫描完成，共发现 {Count} 个命令类型", totalCommandsFound);
            return commandTypeMap;
        }

        /// <summary>
        /// 扫描并注册命令到命令类型助手
        /// </summary>
        /// <param name="namespaceFilter">命名空间过滤器，可选参数</param>
        /// <param name="assemblies">要扫描的程序集，可选参数</param>
        public void ScanAndRegisterCommands(string namespaceFilter = null, params Assembly[] assemblies)
        {
            var commandTypes = ScanCommands(namespaceFilter, assemblies);
            foreach (var kvp in commandTypes)
            {
                try
                {
                    // 注册命令到命令类型助手
                    _commandTypeHelper.RegisterCommandType(kvp.Key, kvp.Value);
                }
                catch (Exception ex)
                {
                    _logger?.LogWarning(ex, "注册命令 {CommandId} 到命令类型助手时出错", kvp.Key);
                }
            }
        }

        /// <summary>
        /// 扫描并注册命令到命令调度器
        /// </summary>
        /// <param name="commandDispatcher">命令调度器实例</param>
        /// <param name="namespaceFilter">命名空间过滤器，可选参数</param>
        /// <param name="assemblies">要扫描的程序集，可选参数</param>
        /// <exception cref="ArgumentNullException">当commandDispatcher为null时抛出</exception>
        public void ScanAndRegisterCommands(ICommandDispatcher commandDispatcher, string namespaceFilter = null, params Assembly[] assemblies)
        {
            if (commandDispatcher == null)
                throw new ArgumentNullException(nameof(commandDispatcher));

            // 先扫描并注册到命令类型助手
            var commandTypes = ScanCommands(namespaceFilter, assemblies);
            
            // 也注册到命令调度器
            foreach (var kvp in commandTypes)
            {
                try
                {
                    commandDispatcher.RegisterCommandType(kvp.Key, kvp.Value);
                }
                catch (Exception ex)
                {
                    _logger?.LogWarning(ex, "注册命令 {CommandId} 到命令调度器时出错", kvp.Key);
                }
            }
        }

        /// <summary>
        /// 扫描当前程序集中的所有命令类型并注册
        /// </summary>
        public void ScanCurrentAssembly()
        {
            ScanAndRegisterCommands(null, new[] { Assembly.GetExecutingAssembly() });
        }

        /// <summary>
        /// 扫描当前程序集中的所有命令类型并注册到指定的命令调度器
        /// </summary>
        /// <param name="commandDispatcher">命令调度器</param>
        public void ScanCurrentAssembly(ICommandDispatcher commandDispatcher)
        {
            ScanAndRegisterCommands(commandDispatcher, null, Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// 扫描多个程序集中的所有命令类型
        /// </summary>
        /// <param name="assemblies">要扫描的程序集列表</param>
        /// <returns>命令类型映射</returns>
        public Dictionary<uint, Type> ScanAssemblies(params Assembly[] assemblies)
        {
            return ScanCommands(null, assemblies);
        }

        /// <summary>
        /// 扫描多个程序集中指定命名空间的命令类型
        /// </summary>
        /// <param name="namespaceFilter">命名空间过滤器</param>
        /// <param name="assemblies">要扫描的程序集列表</param>
        /// <returns>命令类型映射</returns>
        public Dictionary<uint, Type> ScanAssemblies(string namespaceFilter, params Assembly[] assemblies)
        {
            return ScanCommands(namespaceFilter, assemblies);
        }

        /// <summary>
        /// 扫描指定程序集中的所有命令类型并注册
        /// </summary>
        /// <param name="assembly">要扫描的程序集</param>
        public void ScanAssembly(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            ScanAndRegisterCommands(null, assembly);
        }

        /// <summary>
        /// 扫描指定命名空间下的所有命令类型并注册
        /// </summary>
        /// <param name="assembly">要扫描的程序集</param>
        /// <param name="namespacePrefix">命名空间前缀</param>
        public void ScanNamespace(Assembly assembly, string namespacePrefix)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            if (string.IsNullOrEmpty(namespacePrefix))
                throw new ArgumentNullException(nameof(namespacePrefix));

            ScanAndRegisterCommands(namespacePrefix, new[] { assembly });
        }
    }
}