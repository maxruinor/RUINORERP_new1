using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Core;

using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 命令调度器 - 统一的命令分发和处理中心
    /// 实现ICommandDispatcher接口
    /// 
    /// 工作流程：
    /// 1. NetworkServer启动时通过CommandScanner扫描并注册所有命令类型
    /// 2. CommandDispatcher自动发现并注册所有带有CommandHandlerAttribute特性的命令处理器
    /// 3. 当SuperSocketCommandAdapter接收到命令时，通过DispatchAsync方法分发命令
    /// 4. 根据命令ID查找对应的处理器并执行命令处理逻辑
    /// 5. 返回处理结果给SuperSocketCommandAdapter
    /// </summary>
    public class CommandDispatcher : IDisposable, ICommandDispatcher
    {
        private readonly ConcurrentDictionary<string, ICommandHandler> _handlers;
        private readonly ConcurrentDictionary<uint, List<ICommandHandler>> _commandHandlerMap;
        private readonly ConcurrentDictionary<string, DateTime> _commandHistory;
        private readonly ICommandHandlerFactory _handlerFactory;
        // 为每个命令类型维护一个信号量，实现更细粒度的并发控制
        private readonly ConcurrentDictionary<uint, SemaphoreSlim> _commandSemaphores;
        private int _maxConcurrencyPerCommand;
        private bool _disposed = false;
        private bool _isInitialized = false;
        private readonly SemaphoreSlim _dispatchSemaphore;
        // 命令类型辅助类，用于管理命令类型
        private readonly CommandTypeHelper _commandTypeHelper;

        /// <summary>
        /// 每个命令的最大并发数
        /// </summary>
        public int MaxConcurrencyPerCommand
        {
            get => _maxConcurrencyPerCommand;
            set => _maxConcurrencyPerCommand = value > 0 ? value : Environment.ProcessorCount;
        }

        /// <summary>
        /// 处理器数量
        /// </summary>
        public int HandlerCount => _handlers.Count;

        /// <summary>
        /// 是否已初始化
        /// </summary>
        public bool IsInitialized => _isInitialized;

        /// <summary>
        /// 日志记录器
        /// </summary>
        protected ILogger<CommandDispatcher> Logger { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="handlerFactory">处理器工厂</param>
        /// <param name="commandTypeHelper">命令类型辅助类</param>
        /// <param name="maxConcurrencyPerCommand">每个命令的最大并发数</param>
        public CommandDispatcher(ILogger<CommandDispatcher> _Logger, ICommandHandlerFactory handlerFactory = null,
            CommandTypeHelper commandTypeHelper = null, int maxConcurrencyPerCommand = 0)
        {
            Logger = _Logger;
            _handlerFactory = handlerFactory ?? new DefaultCommandHandlerFactory();
            _commandTypeHelper = commandTypeHelper ?? new CommandTypeHelper();
            _handlers = new ConcurrentDictionary<string, ICommandHandler>();
            _commandHandlerMap = new ConcurrentDictionary<uint, List<ICommandHandler>>();
            _commandHistory = new ConcurrentDictionary<string, DateTime>();
            _commandSemaphores = new ConcurrentDictionary<uint, SemaphoreSlim>();
            _dispatchSemaphore = new SemaphoreSlim(1, 1); // 添加缺失的信号量

            MaxConcurrencyPerCommand = maxConcurrencyPerCommand > 0 ? maxConcurrencyPerCommand : Environment.ProcessorCount;

            // 不再初始化默认的日志记录器，而是延迟初始化
        }

        /// <summary>
        /// 初始化调度器
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>初始化结果</returns>
        public async Task<bool> InitializeAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (_isInitialized)
                    return true;


                LogInfo("初始化命令调度器...");

                // 自动发现并注册处理器
                await AutoDiscoverAndRegisterHandlersAsync(cancellationToken);

                _isInitialized = true;
                LogInfo($"命令调度器初始化完成，已注册 {HandlerCount} 个处理器");
                return true;
            }
            catch (Exception ex)
            {
                LogError("初始化命令调度器失败", ex);
                return false;
            }
        }

        /// <summary>
        /// 异步分发命令
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        public async Task<CommandResult> DispatchAsync(ICommand command, CancellationToken cancellationToken = default)
        {
            if (command == null)
            {
                return CommandResult.Failure("命令对象不能为空", ErrorCodes.NullCommand);
            }

            if (!_isInitialized)
            {
                return CommandResult.Failure("调度器未初始化", ErrorCodes.DispatcherNotInitialized);
            }

            var startTime = DateTime.Now;
            var commandId = command.CommandId;
            var commandIdentifier = command.CommandIdentifier;

            // 为每个命令获取或创建信号量，实现更细粒度的并发控制
            var semaphore = _commandSemaphores.GetOrAdd(commandIdentifier,
                _ => new SemaphoreSlim(MaxConcurrencyPerCommand, MaxConcurrencyPerCommand));

            // 限制同一类型命令的并发处理数
            if (!await semaphore.WaitAsync(5000, cancellationToken))
            {
                return CommandResult.Failure($"命令类型 {commandIdentifier} 处理繁忙，请稍后重试", ErrorCodes.DispatcherBusy);
            }

            try
            {
                // 记录命令历史
                _commandHistory.TryAdd(commandId, startTime);

                LogDebug($"开始分发命令: {command.CommandIdentifier} [ID: {commandId}]");

                // 查找合适的处理器
                var handlers = FindHandlers(command);
                if (handlers == null || !handlers.Any())
                {
                    return CommandResult.Failure(
                        $"没有找到适合的处理器处理命令: {command.CommandIdentifier}",
                        ErrorCodes.NoHandlerFound);
                }

                // 选择最佳处理器
                var bestHandler = SelectBestHandler(handlers, command);
                if (bestHandler == null)
                {
                    return CommandResult.Failure(
                        $"无法选择合适的处理器处理命令: {command.CommandIdentifier}",
                        ErrorCodes.HandlerSelectionFailed);
                }

                LogDebug($"选择处理器: {bestHandler.Name} 处理命令: {command.CommandIdentifier}");

                // 分发给处理器
                var result = await bestHandler.HandleAsync(command, cancellationToken);

                // 设置执行时间
                if (result != null)
                {
                    result.ExecutionTimeMs = (long)(DateTime.UtcNow - startTime).TotalMilliseconds;
                }

                LogDebug($"命令分发完成: {command.CommandIdentifier} [ID: {commandId}] - {result?.ExecutionTimeMs}ms");
                return result ?? CommandResult.Failure("处理器返回空结果", ErrorCodes.NullResult);
            }
            catch (OperationCanceledException)
            {
                LogWarning($"命令分发被取消: {command.CommandIdentifier} [ID: {commandId}]");
                return CommandResult.Failure("命令分发被取消", ErrorCodes.DispatchCancelled);
            }
            catch (Exception ex)
            {
                var executionTime = (long)(DateTime.UtcNow - startTime).TotalMilliseconds;
                LogError($"分发命令 {command.CommandIdentifier} [ID: {commandId}] 异常: {ex.Message}", ex);
                return CommandResult.Failure($"命令分发异常: {ex.Message}", ErrorCodes.DispatchError, ex);
            }
            finally
            {
                semaphore.Release();

                // 清理历史记录
                _ = Task.Run(() => CleanupCommandHistory());
            }
        }

        /// <summary>
        /// 注册处理器
        /// </summary>
        /// <param name="handlerType">处理器类型</param>
        public async Task<bool> RegisterHandlerAsync(Type handlerType, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!typeof(ICommandHandler).IsAssignableFrom(handlerType))
                {
                    LogError($"类型 {handlerType.Name} 不是有效的命令处理器");
                    return false;
                }

                var handler = _handlerFactory.CreateHandler(handlerType);
                if (handler == null)
                {
                    LogError($"无法创建处理器实例: {handlerType.Name}");
                    return false;
                }

                // 初始化并启动处理器
                if (!await handler.InitializeAsync(cancellationToken))
                {
                    LogError($"处理器初始化失败: {handler.Name}");
                    handler.Dispose();
                    return false;
                }

                if (!await handler.StartAsync(cancellationToken))
                {
                    LogError($"处理器启动失败: {handler.Name}");
                    handler.Dispose();
                    return false;
                }

                // 注册处理器
                _handlers.TryAdd(handler.HandlerId, handler);

                // 更新命令映射
                UpdateCommandHandlerMapping(handler);

                LogInfo($"注册处理器成功: {handler.Name} [ID: {handler.HandlerId}]");
                return true;
            }
            catch (Exception ex)
            {
                LogError($"注册处理器 {handlerType.Name} 异常", ex);
                return false;
            }
        }

        /// <summary>
        /// 注册处理器
        /// </summary>
        /// <typeparam name="T">处理器类型</typeparam>
        public async Task<bool> RegisterHandlerAsync<T>(CancellationToken cancellationToken = default)
            where T : class, ICommandHandler
        {
            return await RegisterHandlerAsync(typeof(T), cancellationToken);
        }

        /// <summary>
        /// 取消注册处理器
        /// </summary>
        /// <param name="handlerId">处理器ID</param>
        public async Task<bool> UnregisterHandlerAsync(string handlerId, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!_handlers.TryRemove(handlerId, out var handler))
                {
                    return false;
                }

                // 停止处理器
                await handler.StopAsync(cancellationToken);
                handler.Dispose();

                // 更新命令映射
                RemoveHandlerFromMapping(handler);

                LogInfo($"取消注册处理器成功: {handler.Name} [ID: {handlerId}]");
                return true;
            }
            catch (Exception ex)
            {
                LogError($"取消注册处理器 {handlerId} 异常", ex);
                return false;
            }
        }

        /// <summary>
        /// 获取所有处理器
        /// </summary>
        /// <returns>处理器列表</returns>
        public IReadOnlyList<ICommandHandler> GetAllHandlers()
        {
            return _handlers.Values.ToList().AsReadOnly();
        }

        /// <summary>
        /// 获取处理器统计信息
        /// </summary>
        /// <returns>统计信息字典</returns>
        public Dictionary<string, HandlerStatistics> GetHandlerStatistics()
        {
            return _handlers.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.GetStatistics()
            );
        }

        /// <summary>
        /// 自动发现并注册处理器
        /// </summary>
        private async Task AutoDiscoverAndRegisterHandlersAsync(CancellationToken cancellationToken)
        {
            // 默认只扫描当前程序集
            await AutoDiscoverAndRegisterHandlersAsync(cancellationToken, Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// 自动发现并注册处理器（支持多程序集）
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <param name="assemblies">要扫描的程序集</param>
        public async Task AutoDiscoverAndRegisterHandlersAsync(CancellationToken cancellationToken, params Assembly[] assemblies)
        {
            try
            {
                if (assemblies == null || assemblies.Length == 0)
                {
                    assemblies = new[] { Assembly.GetExecutingAssembly() };
                }

                LogInfo($"开始自动发现并注册命令处理器，扫描程序集数量: {assemblies.Length}");
                var handlerTypes = new List<Type>();

                foreach (var assembly in assemblies)
                {
                    try
                    {
                        LogInfo($"正在扫描程序集: {assembly.GetName().Name}");
                        var types = assembly.GetTypes()
                            .Where(t => typeof(ICommandHandler).IsAssignableFrom(t) &&
                                      !t.IsInterface &&
                                      !t.IsAbstract &&
                                      t.GetCustomAttribute<CommandHandlerAttribute>() != null)
                            .ToList();
                        
                        LogInfo($"在程序集 {assembly.GetName().Name} 中发现 {types.Count} 个命令处理器");
                        handlerTypes.AddRange(types);
                        
                        // 记录发现的处理器类型
                        foreach (var type in types)
                        {
                            LogInfo($"发现命令处理器: {type.FullName}");
                            
                            // 记录处理器支持的命令类型
                            var attr = type.GetCustomAttribute<CommandHandlerAttribute>();
                            if (attr != null)
                            {
                                var supportedCommands = string.Join(", ", attr.SupportedCommands);
                                LogInfo($"  处理器 {type.Name} 支持的命令类型: [{supportedCommands}]");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogError($"扫描程序集 {assembly.GetName().Name} 中的命令处理器时出错", ex);
                    }
                }

                // 按优先级排序处理器
                var sortedHandlers = handlerTypes
                    .Select(t => new
                    {
                        Type = t,
                        Attribute = t.GetCustomAttribute<CommandHandlerAttribute>()
                    })
                    .OrderByDescending(h => h.Attribute?.Priority ?? 0)
                    .ToList();

                LogInfo($"总共发现 {sortedHandlers.Count} 个命令处理器，开始注册...");

                // 注册处理器
                var registrationTasks = sortedHandlers.Select(handlerInfo =>
                {
                    LogInfo($"正在注册处理器: {handlerInfo.Type.FullName}");
                    return RegisterHandlerAsync(handlerInfo.Type, cancellationToken);
                });

                var results = await Task.WhenAll(registrationTasks);
                var successCount = results.Count(r => r);

                LogInfo($"命令处理器自动注册完成，共注册 {successCount}/{results.Length} 个处理器");
                
                // 记录注册后的处理器映射信息
                LogCommandHandlerMapping();
            }
            catch (Exception ex)
            {
                LogError("自动发现处理器异常", ex);
            }
        }
        
        /// <summary>
        /// 记录命令处理器映射信息（用于调试）
        /// </summary>
        private void LogCommandHandlerMapping()
        {
            try
            {
                LogInfo($"当前命令处理器映射数量: {_commandHandlerMap.Count}");
                
                foreach (var kvp in _commandHandlerMap)
                {
                    var handlerNames = string.Join(", ", kvp.Value.Select(h => h.Name));
                    LogInfo($"命令代码 {kvp.Key} 映射到处理器: [{handlerNames}]");
                }
            }
            catch (Exception ex)
            {
                LogError("记录命令处理器映射信息时出错", ex);
            }
        }

        /// <summary>
        /// 查找处理器
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <returns>处理器列表</returns>
        private List<ICommandHandler> FindHandlers(ICommand command)
        {
            // 首先尝试从命令映射中查找
            if (_commandHandlerMap.TryGetValue(command.CommandIdentifier.FullCode, out var mappedHandlers))
            {
                var availableHandlers = mappedHandlers.Where(h => h.CanHandle(command)).ToList();
                if (availableHandlers.Any())
                {
                    return availableHandlers;
                }
            }

            // 如果映射中没有找到，则遍历所有处理器
            return _handlers.Values.Where(h => h.CanHandle(command)).ToList();
        }

        /// <summary>
        /// 选择最佳处理器
        /// </summary>
        /// <param name="handlers">候选处理器列表</param>
        /// <param name="command">命令对象</param>
        /// <returns>最佳处理器</returns>
        private ICommandHandler SelectBestHandler(List<ICommandHandler> handlers, ICommand command)
        {
            if (!handlers.Any())
                return null;

            // 按优先级和当前处理负载选择
            return handlers
                .Where(h => h.Status == HandlerStatus.Running)
                .OrderByDescending(h => h.Priority)
                .ThenBy(h => h.GetStatistics().CurrentProcessingCount)
                .ThenBy(h => h.GetStatistics().AverageProcessingTimeMs)
                .FirstOrDefault();
        }

        /// <summary>
        /// 更新命令处理器映射
        /// </summary>
        /// <param name="handler">处理器</param>
        private void UpdateCommandHandlerMapping(ICommandHandler handler)
        {
            foreach (var commandCode in handler.SupportedCommands)
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

        /// <summary>
        /// 从映射中移除处理器
        /// </summary>
        /// <param name="handler">处理器</param>
        private void RemoveHandlerFromMapping(ICommandHandler handler)
        {
            foreach (var commandCode in handler.SupportedCommands)
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

        /// <summary>
        /// 清理命令历史
        /// </summary>
        private void CleanupCommandHistory()
        {
            try
            {
                var cutoff = DateTime.UtcNow.AddMinutes(-30); // 保留30分钟的历史
                var keysToRemove = _commandHistory
                    .Where(kvp => kvp.Value < cutoff)
                    .Select(kvp => kvp.Key)
                    .Take(1000) // 每次清理最多1000条记录
                    .ToList();

                foreach (var key in keysToRemove)
                {
                    _commandHistory.TryRemove(key, out _);
                }
            }
            catch (Exception ex)
            {
                LogError("清理命令历史异常", ex);
            }
        }

        /// <summary>
        /// 记录调试日志
        /// </summary>
        private void LogDebug(string message)
        {
            Logger.LogDebug(message);
        }

        /// <summary>
        /// 记录信息日志
        /// </summary>
        public void LogInfo(string message)
        {
            Logger.LogInformation(message);
        }

        /// <summary>
        /// 记录警告日志
        /// </summary>
        private void LogWarning(string message)
        {
            Logger.LogWarning(message);
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        private void LogError(string message, Exception ex = null)
        {
            if (ex != null)
            {
                Logger.LogError(ex, message);
            }
            else
            {
                Logger.LogError(message);
            }
        }

        /// <summary>
        /// 注册命令类型
        /// </summary>
        /// <param name="commandCode">命令代码</param>
        /// <param name="commandType">命令类型</param>
        public void RegisterCommandType(uint commandCode, Type commandType)
        {
            try
            {
                _commandTypeHelper.RegisterCommandType(commandCode, commandType);
                LogDebug($"注册命令类型成功: {commandCode} -> {commandType.Name}");
            }
            catch (Exception ex)
            {
                LogError($"注册命令类型异常: {commandCode}", ex);
            }
        }

        /// <summary>
        /// 获取命令类型
        /// </summary>
        /// <param name="commandCode">命令代码</param>
        /// <returns>命令类型，如果找不到则返回null</returns>
        public Type GetCommandType(uint commandCode)
        {
            try
            {
                return _commandTypeHelper.GetCommandType(commandCode);
            }
            catch (Exception ex)
            {
                LogError($"获取命令类型异常: {commandCode}", ex);
                return null;
            }
        }

        /// <summary>
        /// 创建命令实例
        /// </summary>
        /// <param name="commandCode">命令代码</param>
        /// <returns>命令实例，如果找不到类型或创建失败则返回null</returns>
        public ICommand CreateCommand(uint commandCode)
        {
            try
            {
                var command = _commandTypeHelper.CreateCommand(commandCode);
                if (command != null)
                {
                    LogDebug($"创建命令实例成功: {commandCode}");
                }
                else
                {
                    LogWarning($"创建命令实例失败: {commandCode}");
                }
                return command;
            }
            catch (Exception ex)
            {
                LogError($"创建命令实例异常: {commandCode}", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取所有注册的命令类型
        /// </summary>
        /// <returns>命令代码和类型的映射</returns>
        public Dictionary<uint, Type> GetAllCommandTypes()
        {
            return _commandTypeHelper.GetAllCommandTypes();
        }

        /// <summary>
        /// 清理注册的命令类型
        /// </summary>
        public void ClearCommandTypes()
        {
            try
            {
                _commandTypeHelper.Clear();
                LogInfo("已清理所有注册的命令类型");
            }
            catch (Exception ex)
            {
                LogError("清理命令类型异常", ex);
            }
        }

        /// <summary>
        /// 获取命令处理器映射信息（用于调试）
        /// </summary>
        /// <returns>命令代码到处理器列表的映射</returns>
        public Dictionary<uint, List<string>> GetCommandHandlerMappingInfo()
        {
            var mappingInfo = new Dictionary<uint, List<string>>();
            
            foreach (var kvp in _commandHandlerMap)
            {
                var handlerNames = kvp.Value.Select(h => h.Name).ToList();
                mappingInfo[kvp.Key] = handlerNames;
            }
            
            return mappingInfo;
        }
        
        /// <summary>
        /// 检查特定命令代码是否已映射到处理器
        /// </summary>
        /// <param name="commandCode">命令代码</param>
        /// <returns>是否已映射</returns>
        public bool IsCommandMapped(uint commandCode)
        {
            return _commandHandlerMap.ContainsKey(commandCode);
        }
        
        /// <summary>
        /// 获取映射到特定命令代码的处理器数量
        /// </summary>
        /// <param name="commandCode">命令代码</param>
        /// <returns>处理器数量</returns>
        public int GetMappedHandlerCount(uint commandCode)
        {
            if (_commandHandlerMap.TryGetValue(commandCode, out var handlers))
            {
                return handlers.Count;
            }
            return 0;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                try
                {
                    // 停止所有处理器
                    var stopTasks = _handlers.Values.Select(h => h.StopAsync(CancellationToken.None));
                    Task.WhenAll(stopTasks).GetAwaiter().GetResult();

                    // 释放所有处理器
                    foreach (var handler in _handlers.Values)
                    {
                        try
                        {
                            handler.Dispose();
                        }
                        catch (Exception ex)
                        {
                            LogError($"释放处理器 {handler.Name} 异常", ex);
                        }
                    }

                    _handlers.Clear();
                    _commandHandlerMap.Clear();
                    _commandHistory.Clear();
                    _dispatchSemaphore?.Dispose();
                }
                catch (Exception ex)
                {
                    LogError("释放调度器资源异常", ex);
                }

                _disposed = true;
            }

            GC.SuppressFinalize(this);
        }

    }

    /// <summary>
    /// 默认命令处理器工厂
    /// </summary>
    public class DefaultCommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly ConcurrentDictionary<Type, Func<ICommandHandler>> _handlerFactories;

        public DefaultCommandHandlerFactory()
        {
            _handlerFactories = new ConcurrentDictionary<Type, Func<ICommandHandler>>();
        }

        /// <summary>
        /// 创建命令处理器
        /// </summary>
        public ICommandHandler CreateHandler(Type handlerType)
        {
            if (_handlerFactories.TryGetValue(handlerType, out var factory))
            {
                return factory();
            }

            // 使用反射创建实例
            return (ICommandHandler)Activator.CreateInstance(handlerType);
        }

        /// <summary>
        /// 创建命令处理器
        /// </summary>
        public T CreateHandler<T>() where T : class, ICommandHandler
        {
            return CreateHandler(typeof(T)) as T;
        }

        /// <summary>
        /// 注册处理器工厂方法
        /// </summary>
        public void RegisterHandler<T>(Func<T> factory) where T : class, ICommandHandler
        {
            _handlerFactories.TryAdd(typeof(T), () => factory());
        }

        /// <summary>
        /// 注册处理器
        /// </summary>
        public void RegisterHandler(Type handlerType)
        {
            if (typeof(ICommandHandler).IsAssignableFrom(handlerType))
            {
                _handlerFactories.TryAdd(handlerType, () => (ICommandHandler)Activator.CreateInstance(handlerType));
            }
        }
    }

    /// <summary>
    /// 命令处理器工厂接口
    /// </summary>
    public interface ICommandHandlerFactory
    {
        /// <summary>
        /// 创建命令处理器
        /// </summary>
        /// <param name="handlerType">处理器类型</param>
        /// <returns>处理器实例</returns>
        ICommandHandler CreateHandler(Type handlerType);

        /// <summary>
        /// 创建命令处理器
        /// </summary>
        /// <typeparam name="T">处理器类型</typeparam>
        /// <returns>处理器实例</returns>
        T CreateHandler<T>() where T : class, ICommandHandler;

        /// <summary>
        /// 注册处理器类型
        /// </summary>
        /// <param name="handlerType">处理器类型</param>
        void RegisterHandler(Type handlerType);
    }
}
