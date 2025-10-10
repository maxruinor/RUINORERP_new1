using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 文件级别注释：
    /// 统一命令扫描器 - 企业级命令和处理器扫描管理器
    /// 
    /// 职责：
    /// 1. 扫描程序集中的命令类型（ICommand）和命令处理器（ICommandHandler）
    /// 2. 缓存扫描结果，提供高效的类型查找和创建功能
    /// 3. 管理命令类型与命令ID的映射关系
    /// 4. 支持处理器优先级排序和生命周期管理
    /// 5. 提供统一的API供客户端和服务器端使用
    /// 
    /// 设计目标：
    /// - 替代原有的CommandTypeHelper类，整合所有类型管理功能
    /// - 提供线程安全的缓存机制，避免重复扫描
    /// - 支持热重载和动态程序集扫描
    /// - 提供详细的扫描统计和性能监控
    /// 
    /// 工作流程：
    /// 1. 应用程序启动时创建CommandScanner实例
    /// 2. 扫描指定程序集中的命令和处理器类型
    /// 3. 建立类型缓存和映射关系
    /// 4. 提供类型查询、创建和注册服务
    /// 5. 支持运行时动态更新和扩展
    /// </summary>
    public class CommandScanner
    {
        private readonly ILogger<CommandScanner> _logger;
        private readonly ICommandHandlerFactory _handlerFactory;

        // 命令类型管理（原CommandTypeHelper功能）
        private readonly Dictionary<CommandId, Type> _commandTypes;
        private readonly object _lock = new object();
        private static readonly ConcurrentDictionary<CommandId, Func<ICommand>> _ctorCache = new();

        // 扫描统计和缓存
        private readonly Dictionary<string, ScanStatistics> _scanStatistics;
        private readonly HashSet<Assembly> _scannedAssemblies;
        private DateTime _lastScanTime;

        // 处理器映射管理（从CommandDispatcher迁移）
        private readonly ConcurrentDictionary<CommandId, List<ICommandHandler>> _commandHandlerMap;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器，可选参数</param>
        /// <param name="handlerFactory">处理器工厂，可选参数</param>
        public CommandScanner(ILogger<CommandScanner> logger = null, ICommandHandlerFactory handlerFactory = null)
        {
            _logger = logger;
            _handlerFactory = handlerFactory ?? new CommandHandlerFactory();

            // 初始化命令类型管理数据结构
            _commandTypes = new Dictionary<CommandId, Type>();
            _scanStatistics = new Dictionary<string, ScanStatistics>();
            _scannedAssemblies = new HashSet<Assembly>();
            _lastScanTime = DateTime.MinValue;

            // 初始化处理器映射管理
            _commandHandlerMap = new ConcurrentDictionary<CommandId, List<ICommandHandler>>();
        }

        #region 扫描统计类

        /// <summary>
        /// 扫描统计信息
        /// </summary>
        public class ScanStatistics
        {
            /// <summary>
            /// 扫描的程序集名称
            /// </summary>
            public string AssemblyName { get; set; }

            /// <summary>
            /// 扫描时间
            /// </summary>
            public DateTime ScanTime { get; set; }

            /// <summary>
            /// 发现的命令数量
            /// </summary>
            public int CommandsFound { get; set; }

            /// <summary>
            /// 发现的处理器数量
            /// </summary>
            public int HandlersFound { get; set; }

            /// <summary>
            /// 扫描耗时（毫秒）
            /// </summary>
            public long ScanDurationMs { get; set; }

            /// <summary>
            /// 是否扫描成功
            /// </summary>
            public bool Success { get; set; }

            /// <summary>
            /// 错误信息
            /// </summary>
            public string ErrorMessage { get; set; }
        }

        #endregion



        #region 命令类型管理（原CommandTypeHelper功能）

        /// <summary>
        /// 注册命令类型 ,添加到字典中
        /// </summary>
        /// <param name="commandCode">命令代码</param>
        /// <param name="commandType">命令类型</param>
        public void RegisterCommandType(CommandId commandCode, Type commandType)
        {
            if (commandType == null)
                throw new ArgumentNullException(nameof(commandType));

            if (!typeof(ICommand).IsAssignableFrom(commandType))
                throw new ArgumentException("命令类型必须实现ICommand接口", nameof(commandType));

            lock (_lock)
            {
                _commandTypes[commandCode] = commandType;
                _logger?.LogDebug("注册命令类型: {CommandCode} -> {TypeName}", commandCode, commandType.Name);
            }
        }

        /// <summary>
        /// 获取命令类型
        /// </summary>
        /// <param name="commandCode">命令代码</param>
        /// <returns>命令类型，如果找不到则返回null</returns>
        public Type GetCommandType(CommandId commandCode)
        {
            lock (_lock)
            {
                _commandTypes.TryGetValue(commandCode, out Type commandType);
                return commandType;
            }
        }

        /// <summary>
        /// 获取命令构造函数
        /// </summary>
        /// <param name="commandCode">命令代码</param>
        /// <returns>命令构造函数，如果找不到类型则返回null</returns>
        public Func<ICommand> GetCommandCtor(CommandId commandCode)
        {
            return _ctorCache.GetOrAdd(commandCode, code =>
            {
                var t = GetCommandType(code);
                if (t == null)
                {
                    _logger?.LogWarning("找不到命令类型: {CommandCode}", code);
                    return null;
                }
                return Expression.Lambda<Func<ICommand>>(
                           Expression.New(t.GetConstructor(Type.EmptyTypes)))
                       .Compile();
            });
        }

        /// <summary>
        /// 根据类型名称获取命令类型
        /// </summary>
        /// <param name="typeName">类型名称（完整名称或短名称）</param>
        /// <returns>命令类型，如果找不到则返回null</returns>
        public Type GetCommandTypeByName(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
                return null;

            lock (_lock)
            {
                // 首先尝试完整名称匹配
                foreach (var kvp in _commandTypes)
                {
                    var registeredType = kvp.Value;
                    if (registeredType.FullName == typeName || registeredType.Name == typeName)
                    {
                        return registeredType;
                    }
                }

                // 尝试从当前应用程序域中查找类型
                try
                {
                    var foundType = Type.GetType(typeName);
                    if (foundType != null && typeof(ICommand).IsAssignableFrom(foundType))
                    {
                        return foundType;
                    }
                }
                catch
                {
                    // 忽略类型查找异常
                }

                return null;
            }
        }

        /// <summary>
        /// 获取所有已注册的命令类型
        /// </summary>
        /// <returns>命令类型字典（命令代码 -> 类型）</returns>
        public IReadOnlyDictionary<CommandId, Type> GetAllCommandTypes()
        {
            lock (_lock)
            {
                return new Dictionary<CommandId, Type>(_commandTypes);
            }
        }
 
        /// <summary>
        /// 清理注册的命令类型
        /// </summary>
        public void Clear()
        {
            lock (_lock)
            {
                var count = _commandTypes.Count;
                _commandTypes.Clear();
                _ctorCache.Clear();
                _scanStatistics.Clear();
                _scannedAssemblies.Clear();
                _lastScanTime = DateTime.MinValue;
                _logger?.LogInformation("清理所有注册的命令类型，共清理 {Count} 个", count);
            }
        }

 

        /// <summary>
        /// 根据请求类型获取命令ID
        /// </summary>
        /// <typeparam name="TReq">请求类型</typeparam>
        /// <returns>对应的命令ID</returns>
        public CommandId GetCommandId<TReq>()
        {
            lock (_lock)
            {
                foreach (var kvp in _commandTypes)
                {
                    if (kvp.Value == typeof(TReq))
                    {
                        return kvp.Key;
                    }
                }

                throw new ArgumentException($"No command ID registered for type {typeof(TReq).FullName}");
            }
        }

        /// <summary>
        /// 获取扫描统计信息
        /// </summary>
        /// <returns>扫描统计信息字典</returns>
        public IReadOnlyDictionary<string, ScanStatistics> GetScanStatistics()
        {
            lock (_lock)
            {
                return new Dictionary<string, ScanStatistics>(_scanStatistics);
            }
        }

        /// <summary>
        /// 获取最后扫描时间
        /// </summary>
        /// <returns>最后扫描时间</returns>
        public DateTime GetLastScanTime()
        {
            return _lastScanTime;
        }

        /// <summary>
        /// 是否已扫描过指定程序集
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <returns>是否已扫描过</returns>
        public bool HasAssemblyBeenScanned(Assembly assembly)
        {
            if (assembly == null) return false;
            lock (_lock)
            {
                return _scannedAssemblies.Contains(assembly);
            }
        }

        #endregion

        /// <summary>
        /// 从类型中提取命令ID
        /// </summary>
        private CommandId ExtractCommandIdFromType(Type commandType)
        {
            try
            {
                // 尝试创建实例获取命令ID
                var commandInstance = Activator.CreateInstance(commandType) as ICommand;
                if (commandInstance != null)
                {
                    return commandInstance.CommandIdentifier;
                }
            }
            catch
            {
                // 忽略创建异常，使用备用方案
            }

            // 备用方案：使用类型名称哈希
            uint tempId = (uint)(commandType.FullName?.GetHashCode() ?? commandType.Name.GetHashCode());
            if (tempId == 0 || tempId == uint.MaxValue)
            {
                tempId = (uint)commandType.Name.GetHashCode();
            }
            return CommandId.FromUInt16((ushort)tempId);
        }

        /// <summary>
        /// 内部统一扫描方法 - 同时处理指令和处理器
        /// </summary>
        private (List<Type> CommandTypes, List<Type> HandlerTypes) ScanAssemblyInternal(Assembly assembly, string namespaceFilter)
        {
            var commandTypes = new List<Type>();
            var handlerTypes = new List<Type>();

            try
            {
                // 获取程序集类型（处理加载异常）
                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    types = ex.Types.Where(t => t != null).ToArray();
                    _logger?.LogWarning("程序集 {Assembly} 部分类型加载失败", assembly.GetName().Name);
                }

                foreach (var type in types)
                {
                    // 跳过无效类型
                    if (type == null || type.IsAbstract || type.IsInterface || type.IsGenericTypeDefinition)
                        continue;

                    // 命名空间过滤
                    if (!string.IsNullOrEmpty(namespaceFilter) && 
                        (type.Namespace == null || !type.Namespace.StartsWith(namespaceFilter)))
                        continue;

                    // 扫描指令类型
                    if (typeof(ICommand).IsAssignableFrom(type))
                    {
                        commandTypes.Add(type);
                    }
                    
                    // 扫描处理器类型
                    if (typeof(ICommandHandler).IsAssignableFrom(type) && 
                        type.GetCustomAttribute<CommandHandlerAttribute>() != null)
                    {
                        handlerTypes.Add(type);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "扫描程序集 {Assembly} 失败", assembly.GetName().Name);
            }

            return (commandTypes, handlerTypes);
        }

        /// <summary>
        /// 扫描并获取指定程序集中的所有命令类型，并可选注册到缓存
        /// </summary>
        /// <param name="assemblies">要扫描的程序集，可选参数</param>
        /// <param name="namespaceFilter">命名空间过滤器，可选参数，只扫描指定命名空间下的命令</param>
        /// <param name="registerTypes">是否立即注册发现的命令类型到缓存</param>
        /// <returns>命令类型和命令ID的映射字典</returns>
        public Dictionary<CommandId, Type> ScanCommands(string namespaceFilter = null, bool registerTypes = true, params Assembly[] assemblies)
        {
            var commandTypeMap = new Dictionary<CommandId, Type>();
            int totalCommandsFound = 0;

            if (assemblies == null || assemblies.Length == 0)
            {
                assemblies = new[] { Assembly.GetCallingAssembly() };
            }

            foreach (var assembly in assemblies)
            {
                var (commandTypes, _) = ScanAssemblyInternal(assembly, namespaceFilter);
                
                foreach (var commandType in commandTypes)
                {
                    try
                    {
                        // 获取命令ID
                        var commandId = ExtractCommandIdFromType(commandType);
                        var commandName = commandType.Name;
                        
                        // 获取命令特性中的名称
                        var commandAttribute = commandType.GetCustomAttribute<PacketCommandAttribute>();
                        if (commandAttribute != null && !string.IsNullOrEmpty(commandAttribute.Name))
                        {
                            commandName = commandAttribute.Name;
                        }

                        if (!commandTypeMap.ContainsKey(commandId))
                        {
                            commandTypeMap[commandId] = commandType;
                            totalCommandsFound++;

                            // 注册到缓存
                            if (registerTypes)
                            {
                                RegisterCommandType(commandId, commandType);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogWarning(ex, "处理命令类型 {TypeName} 失败", commandType.FullName);
                    }
                }
            }

            _logger?.LogInformation("扫描完成，发现 {Count} 个命令类型", totalCommandsFound);
            return commandTypeMap;
        }

        /// <summary>
        /// 扫描并注册命令到命令类型助手
        /// </summary>
        /// <param name="namespaceFilter">命名空间过滤器，可选参数</param>
        /// <param name="assemblies">要扫描的程序集，可选参数</param>
        public void ScanAndRegisterCommands(string namespaceFilter = null, params Assembly[] assemblies)
        {
            var commandTypes = ScanCommands(namespaceFilter, true, assemblies);
            foreach (var kvp in commandTypes)
            {
                try
                {
                    // 注册命令类型到缓存
                    RegisterCommandType(kvp.Key, kvp.Value);
                }
                catch (Exception ex)
                {
                    _logger?.LogWarning(ex, "注册命令 {CommandId} 到类型缓存时出错", kvp.Key);
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
        /// 扫描多个程序集中的所有命令类型
        /// </summary>
        /// <param name="assemblies">要扫描的程序集列表</param>
        /// <returns>命令类型映射</returns>
        public Dictionary<CommandId, Type> ScanAssemblies(params Assembly[] assemblies)
        {
            return ScanCommands(null, true, assemblies);
        }

        /// <summary>
        /// 扫描多个程序集中指定命名空间的命令类型
        /// </summary>
        /// <param name="namespaceFilter">命名空间过滤器</param>
        /// <param name="assemblies">要扫描的程序集列表</param>
        /// <returns>命令类型映射</returns>
        public Dictionary<CommandId, Type> ScanAssemblies(string namespaceFilter, params Assembly[] assemblies)
        {
            return ScanCommands(namespaceFilter, true, assemblies);
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

        #region 命令处理器扫描和注册功能

        /// <summary>
        /// 扫描并获取指定程序集中的所有命令处理器类型
        /// </summary>
        /// <param name="assemblies">要扫描的程序集，可选参数</param>
        /// <param name="namespaceFilter">命名空间过滤器，可选参数</param>
        /// <returns>命令处理器类型列表</returns>
        public List<Type> ScanCommandHandlers(string namespaceFilter = null, params Assembly[] assemblies)
        {
            var handlerTypes = new List<Type>();

            if (assemblies == null || assemblies.Length == 0)
            {
                assemblies = new[] { Assembly.GetCallingAssembly() };
            }

            foreach (var assembly in assemblies)
            {
                var (_, handlers) = ScanAssemblyInternal(assembly, namespaceFilter);
                handlerTypes.AddRange(handlers);
            }

            _logger?.LogInformation("扫描完成，发现 {Count} 个处理器类型", handlerTypes.Count);
            return handlerTypes;
        }

        /// <summary>
        /// 按优先级排序处理器类型
        /// </summary>
        /// <param name="handlerTypes">处理器类型列表</param>
        /// <returns>按优先级排序的处理器类型列表</returns>
        public List<Type> SortHandlersByPriority(List<Type> handlerTypes)
        {
            if (handlerTypes == null)
                return new List<Type>();

            return handlerTypes
                .Select(t => new
                {
                    Type = t,
                    Attribute = t.GetCustomAttribute<CommandHandlerAttribute>()
                })
                .OrderByDescending(h => h.Attribute?.Priority ?? 0)
                .Select(h => h.Type)
                .ToList();
        }

        /// <summary>
        /// 自动发现并注册处理器到命令扫描器
        /// </summary>
        /// <param name="namespaceFilter">命名空间过滤器，可选参数</param>
        /// <param name="assemblies">要扫描的程序集，可选参数</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>注册结果</returns>
        public async Task<bool> AutoDiscoverAndRegisterHandlersAsync(string namespaceFilter = null,
           CancellationToken cancellationToken = default, params Assembly[] assemblies)
        {
            try
            {
                _logger?.LogInformation("开始自动注册处理器");

                // 扫描处理器类型
                var handlerTypes = ScanCommandHandlers(namespaceFilter, assemblies);
                if (!handlerTypes.Any())
                {
                    _logger?.LogWarning("未发现处理器类型");
                    return false;
                }

                // 按优先级排序
                var sortedHandlers = SortHandlersByPriority(handlerTypes);
                _logger?.LogInformation("发现 {Count} 个处理器，开始注册...", sortedHandlers.Count);

                // 注册处理器
                var successCount = 0;
                foreach (var handlerType in sortedHandlers)
                {
                    try
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        var registered = await RegisterHandlerAsync(handlerType, cancellationToken);
                        if (registered)
                        {
                            successCount++;
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        _logger?.LogWarning("注册被取消");
                        throw;
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "注册 {TypeName} 失败", handlerType.FullName);
                    }
                }

                _logger?.LogInformation("注册完成: {SuccessCount}/{TotalCount}",
                    successCount, sortedHandlers.Count);

                return successCount > 0;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "自动注册失败");
                return false;
            }
        }

        /// <summary>
        /// 注册单个处理器并管理处理器映射
        /// </summary>
        /// <param name="handlerType">处理器类型</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>注册结果</returns>
        public async Task<bool> RegisterHandlerAsync(Type handlerType,
            CancellationToken cancellationToken = default)
        {
            if (handlerType == null)
            {
                _logger?.LogError("处理器类型不能为空");
                return false;
            }

            try
            {
                if (!typeof(ICommandHandler).IsAssignableFrom(handlerType))
                {
                    _logger?.LogError("类型 {TypeName} 不是有效的命令处理器", handlerType.Name);
                    return false;
                }

                // 创建处理器实例
                var handler = _handlerFactory.CreateHandler(handlerType);
                if (handler == null)
                {
                    _logger?.LogError("无法创建处理器实例: {TypeName}", handlerType.Name);
                    return false;
                }

                // 初始化处理器
                var initialized = await handler.InitializeAsync(cancellationToken);
                if (!initialized)
                {
                    _logger?.LogError("处理器初始化失败: {HandlerName}", handler.Name);
                    handler.Dispose();
                    return false;
                }

                // 启动处理器
                var started = await handler.StartAsync(cancellationToken);
                if (!started)
                {
                    _logger?.LogError("处理器启动失败: {HandlerName}", handler.Name);
                    handler.Dispose();
                    return false;
                }

                // 注册处理器到内部映射
                RegisterHandlerToMapping(handler);

                _logger?.LogInformation("注册成功: {HandlerName} [ID: {HandlerId}]", handler.Name, handler.HandlerId);
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "注册处理器 {TypeName} 异常", handlerType.Name);
                return false;
            }
        }



        #endregion

        #region 处理器映射管理（从CommandDispatcher迁移）

        /// <summary>
        /// 注册处理器到内部映射
        /// </summary>
        /// <param name="handler">处理器实例</param>
        public void RegisterHandlerToMapping(ICommandHandler handler)
        {
            if (handler == null)
            {
                _logger?.LogError("尝试为null处理器更新命令映射");
                return;
            }

            try
            {
                // 安全获取支持的命令列表，避免空引用
                var supportedCommands = handler.SupportedCommands ?? Array.Empty<CommandId>();

                if (!supportedCommands.Any())
                {
                    _logger?.LogWarning($"处理器 {handler.Name} [ID: {handler.HandlerId}] 未定义支持的命令类型");
                    return;
                }

                foreach (var commandCode in supportedCommands)
                {
                    _commandHandlerMap.AddOrUpdate(
                        commandCode,
                        new List<ICommandHandler> { handler },
                        (key, existingHandlers) =>
                        {
                            if (!existingHandlers.Contains(handler))
                            {
                                existingHandlers.Add(handler);
                            }
                            return existingHandlers;
                        });
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"更新处理器 {handler.Name} 命令映射时发生异常");
                throw;
            }
        }

        /// <summary>
        /// 从映射中移除处理器
        /// </summary>
        /// <param name="handler">处理器实例</param>
        public void RemoveHandlerFromMapping(ICommandHandler handler)
        {
            if (handler == null)
            {
                _logger?.LogError("尝试从映射中移除null处理器");
                return;
            }

            try
            {
                // 安全获取支持的命令列表，避免空引用
                var supportedCommands = handler.SupportedCommands ?? Array.Empty<CommandId>();

                if (!supportedCommands.Any())
                {
                    _logger?.LogWarning($"处理器 {handler.Name} [ID: {handler.HandlerId}] 未定义支持的命令类型，无法从映射中移除");
                    return;
                }

                foreach (var commandCode in supportedCommands)
                {
                    if (_commandHandlerMap.TryGetValue(commandCode, out var handlers))
                    {
                        handlers.Remove(handler);

                        if (!handlers.Any())
                        {
                            _commandHandlerMap.TryRemove(commandCode, out _);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"从映射中移除处理器 {handler.Name} 时发生异常");
                throw;
            }
        }

        /// <summary>
        /// 根据命令ID获取处理器列表
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <returns>处理器列表，如果没有找到则返回空列表</returns>
        public List<ICommandHandler> GetHandlers(CommandId commandId)
        {
            if (_commandHandlerMap.TryGetValue(commandId, out var handlers))
            {
                return handlers.ToList(); // 返回副本以避免外部修改
            }
            return new List<ICommandHandler>();
        }

        /// <summary>
        /// 获取所有处理器映射
        /// </summary>
        /// <returns>命令ID到处理器列表的映射字典</returns>
        public IReadOnlyDictionary<CommandId, List<ICommandHandler>> GetAllHandlerMappings()
        {
            return _commandHandlerMap;
        }

        /// <summary>
        /// 获取处理器映射数量
        /// </summary>
        /// <returns>映射数量</returns>
        public int GetHandlerMappingCount()
        {
            return _commandHandlerMap.Count;
        }

        /// <summary>
        /// 清理所有处理器映射
        /// </summary>
        public void ClearHandlerMappings()
        {
            _commandHandlerMap.Clear();
            _logger?.LogInformation("已清理所有处理器映射");
        }

        /// <summary>
        /// 异步移除处理器
        /// </summary>
        /// <param name="handlerId">处理器ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否成功移除</returns>
        public async Task<bool> RemoveHandlerAsync(string handlerId, CancellationToken cancellationToken = default)
        {
            try
            {
                // 查找处理器
                var handler = _commandHandlerMap.Values
                    .SelectMany(list => list)
                    .FirstOrDefault(h => h.HandlerId == handlerId);

                if (handler == null)
                {
                    return false;
                }

                // 停止处理器
                await handler.StopAsync(cancellationToken);
                handler.Dispose();

                // 从映射中移除处理器
                RemoveHandlerFromMapping(handler);

                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "移除处理器 {HandlerId} 时发生异常", handlerId);
                return false;
            }
        }

        #endregion
    }
}
