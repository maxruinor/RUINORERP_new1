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
    /// 
    /// 工作流程：
    /// 1. NetworkServer在启动时创建CommandScanner实例
    /// 2. CommandScanner扫描所有实现了ICommand接口的命令类型
    /// 3. 通过PacketCommandAttribute特性和CommandIdentifier属性获取命令ID
    /// 4. 将扫描到的命令类型注册到CommandDispatcher中
    /// 5. CommandDispatcher使用这些命令类型创建和处理命令实例
    /// </summary>
    public class CommandScanner
    {
        private readonly ILogger<CommandScanner> _logger;
        private readonly CommandTypeHelper _commandTypeHelper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器，可选参数</param>
        /// <param name="commandTypeHelper">命令类型助手，可选参数</param>
        public CommandScanner(ILogger<CommandScanner> logger = null, CommandTypeHelper commandTypeHelper = null)
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
        public Dictionary<CommandId, Type> ScanCommands(string namespaceFilter = null, params Assembly[] assemblies)
        {
            var commandTypeMap = new Dictionary<CommandId, Type>();
            int totalCommandsFound = 0;

            if (assemblies == null || assemblies.Length == 0)
            {
                assemblies = new[] { Assembly.GetCallingAssembly() };
            }

            foreach (var assembly in assemblies)
            {
                try
                {
                    // 处理 ReflectionTypeLoadException 异常
                    Type[] types;
                    try
                    {
                        types = assembly.GetTypes();
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        // 记录加载失败的类型信息
                        _logger?.LogWarning(ex, "加载程序集 {AssemblyName} 的类型时出错，将跳过无法加载的类型", assembly.GetName().Name);
                        
                        // 只使用成功加载的类型
                        types = ex.Types.Where(t => t != null).ToArray();
                        
                        // 记录无法加载的类型
                        foreach (var loaderException in ex.LoaderExceptions)
                        {
                            _logger?.LogWarning(loaderException, "类型加载异常");
                        }
                    }

                    IEnumerable<Type> typesQuery = types
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
                            // 跳过泛型定义类型（如 GenericCommand<>）
                            if (commandType.IsGenericTypeDefinition)
                            {
                                _logger?.LogDebug("跳过泛型定义类型: {TypeName}", commandType.FullName);
                                continue;
                            }

                            // 获取命令特性
                            var commandAttribute = commandType.GetCustomAttribute<PacketCommandAttribute>();
                            CommandId commandId = default(CommandId);
                            string commandName = commandType.Name;

                            if (commandAttribute != null)
                            {
                                commandName = commandAttribute.Name;
                                
                                // 对于非泛型类型，尝试创建实例获取命令ID
                                if (!commandType.IsGenericType)
                                {
                                    try
                                    {
                                        var commandInstance = Activator.CreateInstance(commandType) as ICommand;
                                        if (commandInstance != null)
                                        {
                                            commandId = commandInstance.CommandIdentifier;
                                          
                                        }
                                    }
                                    catch (Exception createEx)
                                    {
                                        _logger?.LogWarning(createEx, "创建命令实例 {TypeName} 时出错，将使用备用方案生成命令ID", commandType.FullName);
                                    }
                                }
                            }

                            // 如果命令ID为默认值，则使用类型的哈希码作为后备方案
                            CommandId finalCommandId;
                            if (commandId.Equals(default(CommandId)))
                            {
                                uint tempCommandId = (uint)(commandType.FullName?.GetHashCode() ?? 0);
                                if (tempCommandId == 0 || tempCommandId == uint.MaxValue)
                                {
                                    tempCommandId = (uint)commandType.Name.GetHashCode();
                                }
                                finalCommandId = CommandId.FromUInt16((ushort)tempCommandId);
                            }
                            else
                            {
                                finalCommandId = commandId;
                            }

                            if (!commandTypeMap.ContainsKey(finalCommandId))
                            {
                                commandTypeMap[finalCommandId] = commandType;
                                totalCommandsFound++;
                                _logger?.LogDebug("扫描到命令: {CommandName} (ID: {CommandId})", commandName, finalCommandId);
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


            // 2. 再把泛型定义当作"模板"塞进去（只塞一次） 新旧兼容，这里是用泛型定义来模拟泛型命令，而不是用泛型命令类
            commandDispatcher.RegisterCommandType(CommandId.FromUInt16(0xEEEE), typeof(GenericCommand<>));   // 用一个不可能冲突的伪码
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
        public Dictionary<CommandId, Type> ScanAssemblies(params Assembly[] assemblies)
        {
            return ScanCommands(null, assemblies);
        }

        /// <summary>
        /// 扫描多个程序集中指定命名空间的命令类型
        /// </summary>
        /// <param name="namespaceFilter">命名空间过滤器</param>
        /// <param name="assemblies">要扫描的程序集列表</param>
        /// <returns>命令类型映射</returns>
        public Dictionary<CommandId, Type> ScanAssemblies(string namespaceFilter, params Assembly[] assemblies)
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
