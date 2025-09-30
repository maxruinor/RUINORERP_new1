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
using RUINORERP.PacketSpec.Models.Responses;
using Polly;
using System.Threading.Channels;
using RUINORERP.PacketSpec.Enums.Core;

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
        // 定义HandlerCollection结构体，用于同时返回处理器列表和命令类型
        private readonly struct HandlerCollection
        {
            public readonly List<ICommandHandler> Handlers;
            public readonly Type CommandType;

            public HandlerCollection(List<ICommandHandler> handlers, Type commandType)
            {
                Handlers = handlers ?? new List<ICommandHandler>();
                CommandType = commandType;
            }
        }

        private static readonly List<ICommandHandler> EmptyList = new List<ICommandHandler>();

        private readonly ConcurrentDictionary<uint, List<ICommandHandler>> _commandHandlerMap;
        private readonly ConcurrentDictionary<ushort, DateTime> _commandHistory;
        private ICommandHandlerFactory _handlerFactory;
        // 移除每个命令类型的信号量，改为使用Channel队列
        // private readonly ConcurrentDictionary<uint, SemaphoreSlim> _commandSemaphores;
        private int _maxConcurrencyPerCommand;
        private bool _disposed = false;
        private bool _isInitialized = false;
        private readonly SemaphoreSlim _dispatchSemaphore;
        // 命令类型辅助类，用于管理命令类型
        private readonly CommandTypeHelper _commandTypeHelper;
        // 使用Channel队列替代信号量
        private readonly Channel<QueuedCommand>[] _commandChannels;
        private readonly Task[] _channelProcessors;
        // 熔断器
        private readonly IAsyncPolicy<ResponseBase> _circuit;
        // 幂等过滤器
        private readonly IdempotencyFilter _idempotent = new IdempotencyFilter();

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
        public int HandlerCount => _commandHandlerMap.Values.Sum(list => list.Count);

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
        /// <param name="circuitBreakerPolicy">熔断器策略，默认为6次失败后熔断，30秒后恢复</param>
        public CommandDispatcher(ILogger<CommandDispatcher> logger, ICommandHandlerFactory handlerFactory = null,
            CommandTypeHelper commandTypeHelper = null, int maxConcurrencyPerCommand = 0,
            IAsyncPolicy<ResponseBase> circuitBreakerPolicy = null)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _handlerFactory = handlerFactory;
            _commandTypeHelper = commandTypeHelper ?? new CommandTypeHelper();
            _commandHandlerMap = new ConcurrentDictionary<uint, List<ICommandHandler>>();
            _commandHistory = new ConcurrentDictionary<ushort, DateTime>();
            _dispatchSemaphore = new SemaphoreSlim(1, 1); // 添加缺失的信号量

            MaxConcurrencyPerCommand = maxConcurrencyPerCommand > 0 ? maxConcurrencyPerCommand : Environment.ProcessorCount;

            // 使用传入的熔断器策略，如果未提供则使用默认策略
            _circuit = circuitBreakerPolicy ?? Policy
                .HandleResult<ResponseBase>(r => !r.IsSuccess)
                .CircuitBreakerAsync(10, TimeSpan.FromMinutes(1)); // 增加到10次失败后熔断，持续1分钟

            // 创建三个优先级的Channel队列
            _commandChannels = new Channel<QueuedCommand>[3];
            _commandChannels[0] = Channel.CreateBounded<QueuedCommand>(new BoundedChannelOptions(1000)
            {
                FullMode = BoundedChannelFullMode.Wait
            });
            _commandChannels[1] = Channel.CreateBounded<QueuedCommand>(new BoundedChannelOptions(1000)
            {
                FullMode = BoundedChannelFullMode.Wait
            });
            _commandChannels[2] = Channel.CreateBounded<QueuedCommand>(new BoundedChannelOptions(1000)
            {
                FullMode = BoundedChannelFullMode.Wait
            });

            // 创建处理任务
            _channelProcessors = new Task[3];

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

                // 确保处理器工厂已初始化
                if (_handlerFactory == null)
                {
                    _handlerFactory = new DefaultCommandHandlerFactory();
                }

                LogInfo("初始化命令调度器...");

                // 自动发现并注册处理器
                await AutoDiscoverAndRegisterHandlersAsync(cancellationToken);

                _isInitialized = true;

                // 启动后台消费者线程
                for (int i = 0; i < 3; i++)
                {
                    int priority = i; // 保存循环变量的副本
                    _channelProcessors[i] = Task.Run(async () =>
                    {
                        var reader = _commandChannels[priority].Reader;
                        while (await reader.WaitToReadAsync(cancellationToken))
                        {
                            while (reader.TryRead(out var queued))
                            {
                                try
                                {
                                    var result = await ProcessAsync(queued.Command, cancellationToken);
                                    queued.Tcs.TrySetResult(result);
                                }
                                catch (Exception ex)
                                {
                                    LogError($"后台消费者线程处理命令时发生异常", ex);
                                    queued.Tcs.TrySetException(ex);
                                }
                            }
                        }
                    }, cancellationToken);
                }

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
        public async Task<ResponseBase> DispatchAsync(ICommand command, CancellationToken cancellationToken = default)
        {
            if (command == null)
            {
                return ResponseBase.CreateError("命令对象不能为空", 400);
            }

            if (!_isInitialized)
            {
                return ResponseBase.CreateError("调度器未初始化", 500);
            }

            // 确定命令优先级
            int priorityChannel = GetPriorityChannel(command.Priority);

            // 创建一个TaskCompletionSource来等待处理结果
            var tcs = new TaskCompletionSource<ResponseBase>();

            // 创建排队命令对象
            var queuedCommand = new QueuedCommand
            {
                Command = command,
                Tcs = tcs
            };

            // 将命令加入对应优先级的Channel队列
            await _commandChannels[priorityChannel].Writer.WriteAsync(queuedCommand, cancellationToken);

            return await tcs.Task;
        }

        /// <summary>
        /// 根据命令优先级确定应该使用的Channel队列
        /// </summary>
        /// <param name="priority">命令优先级</param>
        /// <returns>Channel队列索引 (0=高优先级, 1=普通优先级, 2=低优先级)</returns>
        private int GetPriorityChannel(CommandPriority priority)
        {
            return priority switch
            {
                CommandPriority.High => 0,
                CommandPriority.Normal => 1,
                CommandPriority.Low => 2,
                _ => 1 // 默认使用普通优先级
            };
        }

        /// <summary>
        /// 处理命令的核心方法
        /// </summary>
        /// <param name="cmd">命令对象</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<ResponseBase> ProcessAsync(ICommand cmd, CancellationToken ct)
        {
            // 检查幂等性

            if (cmd.CommandIdentifier != 0)
            {
                if (_idempotent.TryGetCached(cmd.CommandIdentifier.ToString(), out var cached))
                {
                    return cached;
                }
            }

            var startTime = DateTime.Now;
            var commandIdentifier = cmd.CommandIdentifier;

            ResponseBase result = null;
            try
            {
                // 记录命令历史
                _commandHistory.TryAdd(commandIdentifier.FullCode, startTime);

                LogDebug($"开始分发命令: {cmd.CommandIdentifier} [ID: {commandIdentifier.FullCode}]");

                // 查找合适的处理器和命令类型
                var handlerCollection = FindHandlers(cmd);
                var handlers = handlerCollection.Handlers;

                if (handlers == null || !handlers.Any())
                {
                    result = ResponseBase.CreateError(
                        $"没有找到适合的处理器处理命令: {cmd.CommandIdentifier}", 404);
                    return result;
                }

                // 选择最佳处理器
                var bestHandler = SelectBestHandler(handlers, cmd);
                if (bestHandler == null)
                {
                    result = ResponseBase.CreateError(
                        $"无法选择合适的处理器处理命令: {cmd.CommandIdentifier}", 500);
                    return result;
                }

                LogDebug($"选择处理器: {bestHandler.Name} 处理命令: {cmd.CommandIdentifier}");

                // 使用熔断器执行处理逻辑
                try
                {
                    result = await _circuit.ExecuteAsync(() => bestHandler.HandleAsync(cmd, ct));
                }
                catch (Polly.CircuitBreaker.BrokenCircuitException ex)
                {
                    // 熔断器已打开，记录详细信息并返回适当的错误
                    LogWarning($"命令 {cmd.CommandIdentifier} [ID: {commandIdentifier.FullCode}] 的熔断器已打开，拒绝执行: {ex.Message}");
                    result = ResponseBase.CreateError($"服务暂时不可用，熔断器已打开: {ex.Message}", 503);
                    return result;
                }

                // 设置执行时间
                if (result != null)
                {
                    result.ExecutionTimeMs = (long)(DateTime.UtcNow - startTime).TotalMilliseconds;
                }

                LogDebug($"命令分发完成: {cmd.CommandIdentifier} [ID: {commandIdentifier.FullCode}] - {result?.ExecutionTimeMs}ms");

                if (result == null)
                {
                    result = ResponseBase.CreateError("处理器返回空结果", 500);
                }

                return result;
            }
            catch (OperationCanceledException)
            {
                LogWarning($"命令分发被取消: {cmd.CommandIdentifier} [ID: {commandIdentifier.FullCode}]");
                result = ResponseBase.CreateError("命令分发被取消", 503);
                return result;
            }
            catch (Exception ex)
            {
                var executionTime = (long)(DateTime.UtcNow - startTime).TotalMilliseconds;
                LogError($"分发命令 {cmd.CommandIdentifier} [ID: {commandIdentifier.FullCode}] 异常: {ex.Message}", ex);
                result = ResponseBase.CreateError($"命令分发异常: {ex.Message}", 500);
                return result;
            }
            finally
            {
                // 缓存结果以实现幂等性
                if (!string.IsNullOrEmpty(cmd.CommandIdentifier.ToString()) && result != null)
                {
                    _idempotent.Cache(cmd.CommandIdentifier.ToString(), result);
                }

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
            if (_handlerFactory == null)
            {
                LogError("处理器工厂未初始化");
                return false;
            }

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
                // 从_commandHandlerMap中查找处理器
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
            return _commandHandlerMap.Values.SelectMany(list => list).Distinct().ToList().AsReadOnly();
        }

        /// <summary>
        /// 获取处理器统计信息
        /// </summary>
        /// <returns>统计信息字典</returns>
        public Dictionary<string, HandlerStatistics> GetHandlerStatistics()
        {
            return _commandHandlerMap.Values
                .SelectMany(list => list)
                .Distinct()
                .ToDictionary(
                    handler => handler.HandlerId,
                    handler => handler.GetStatistics()
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

                        // 处理 ReflectionTypeLoadException 异常
                        Type[] types;
                        try
                        {
                            types = assembly.GetTypes();
                        }
                        catch (ReflectionTypeLoadException ex)
                        {
                            // 记录加载失败的类型信息
                            LogWarning($"加载程序集 {assembly.GetName().Name} 的类型时出错，将跳过无法加载的类型: {ex.Message}");

                            // 只使用成功加载的类型
                            types = ex.Types.Where(t => t != null).ToArray();

                            // 记录无法加载的类型
                            foreach (var loaderException in ex.LoaderExceptions)
                            {
                                LogWarning($"类型加载异常: {loaderException?.Message}");
                            }
                        }

                        var typesQuery = types
                            .Where(t => typeof(ICommandHandler).IsAssignableFrom(t) &&
                                      !t.IsInterface &&
                                      !t.IsAbstract &&
                                      t.GetCustomAttribute<CommandHandlerAttribute>() != null)
                            .ToList();

                        LogInfo($"在程序集 {assembly.GetName().Name} 中发现 {typesQuery.Count} 个命令处理器");
                        handlerTypes.AddRange(typesQuery);

                        // 记录发现的处理器类型
                        foreach (var type in typesQuery)
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
        /// 查找处理器和命令类型
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <returns>包含处理器列表和命令类型的HandlerCollection</returns>
        private HandlerCollection FindHandlers(ICommand command)
        {
            var commandCode = command.CommandIdentifier.FullCode;

            // 同时从命令映射和命令类型辅助类中获取信息
            _commandHandlerMap.TryGetValue(commandCode, out var handlers);
            var commandType = _commandTypeHelper.GetCommandType(commandCode);

            // 过滤可用的处理器
            List<ICommandHandler> availableHandlers = null;
            if (handlers != null)
            {
                availableHandlers = handlers.Where(h => h.CanHandle(command)).ToList();
                if (!availableHandlers.Any())
                {
                    availableHandlers = null;
                }
            }

            // 如果映射中没有找到，则遍历所有处理器
            if (availableHandlers == null)
            {
                availableHandlers = _commandHandlerMap.Values
                    .SelectMany(list => list)
                    .Where(h => h.CanHandle(command))
                    .ToList();

                if (!availableHandlers.Any())
                {
                    availableHandlers = null;
                }
            }

            return new HandlerCollection(availableHandlers, commandType);
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
                // 使用预编译的构造函数创建命令实例
                var ctor = _commandTypeHelper.GetCommandCtor(commandCode);
                var command = ctor();
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
                    // 关闭所有Channel
                    foreach (var channel in _commandChannels)
                    {
                        channel?.Writer.TryComplete();
                    }

                    // 等待所有处理任务完成
                    Task.WhenAll(_channelProcessors).GetAwaiter().GetResult();

                    // 停止所有处理器
                    var stopTasks = _commandHandlerMap.Values
                        .SelectMany(list => list)
                        .Distinct()
                        .Select(h => h.StopAsync(CancellationToken.None));
                    Task.WhenAll(stopTasks).GetAwaiter().GetResult();

                    // 释放所有处理器
                    foreach (var handler in _commandHandlerMap.Values.SelectMany(list => list).Distinct())
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

        /// <summary>
        /// 将ResponseBase转换为ApiResponse
        /// </summary>
        /// <param name="baseResponse">基础响应对象</param>
        /// <returns>ApiResponse对象</returns>
        private ResponseBase ConvertToApiResponse(ResponseBase baseResponse)
        {
            var response = new ResponseBase
            {
                IsSuccess = baseResponse.IsSuccess,
                Message = baseResponse.Message,
                Code = baseResponse.Code,
                TimestampUtc = baseResponse.TimestampUtc,
                RequestId = baseResponse.RequestId,
                Metadata = baseResponse.Metadata?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                ExecutionTimeMs = baseResponse.ExecutionTimeMs
            };
            return response;
        }
    }




}
