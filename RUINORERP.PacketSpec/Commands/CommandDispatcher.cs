using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polly;
using System.Threading.Channels;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Commands.Authentication;

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

        private readonly ConcurrentDictionary<CommandId, DateTime> _commandHistory;
        private ICommandHandlerFactory _handlerFactory;
        // 移除每个命令类型的信号量，改为使用Channel队列
        // private readonly ConcurrentDictionary<uint, SemaphoreSlim> _commandSemaphores;
        private int _maxConcurrencyPerCommand;
        private bool _disposed = false;
        private bool _isInitialized = false;
        private readonly SemaphoreSlim _dispatchSemaphore;
        // 命令类型管理功能已迁移到CommandScanner中
        // 使用Channel队列替代信号量
        private readonly Channel<QueuedCommand>[] _commandChannels;
        private readonly Task[] _channelProcessors;
        // 熔断器
        private readonly IAsyncPolicy<BaseCommand<IResponse>> _circuit;
        // 幂等过滤器
        private readonly IdempotencyFilter _idempotent = new IdempotencyFilter();
        // 回退处理器 - 用于处理没有专门处理器的命令
        private FallbackGenericCommandHandler _fallbackHandler;
        // 回退处理器初始化锁
        private readonly object _fallbackHandlerLock = new object();
        // 命令扫描器 - 用于扫描和注册命令处理器
        private readonly CommandScanner _commandScanner;
        
        // 缓存管理器 - 用于访问缓存的命令类型和构造函数
        private readonly CommandCacheManager _cacheManager;
        
        // 本地处理器映射缓存 - 避免重复从扫描器获取
        private readonly ConcurrentDictionary<CommandId, List<ICommandHandler>> _commandHandlerMap;


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
        public int HandlerCount => _commandScanner?.GetHandlerMappingCount() ?? 0;

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
        /// <param name="logger">日志记录器</param>
        /// <param name="commandScanner">命令扫描器（包含处理器映射管理功能）</param>
        /// <param name="handlerFactory">处理器工厂</param>
        /// <param name="maxConcurrencyPerCommand">每个命令的最大并发数</param>
        /// <param name="circuitBreakerPolicy">熔断器策略，默认为6次失败后熔断，30秒后恢复</param>
        public CommandDispatcher(ILogger<CommandDispatcher> logger, CommandScanner commandScanner, ICommandHandlerFactory handlerFactory = null,
            int maxConcurrencyPerCommand = 0,
            IAsyncPolicy<BaseCommand<IResponse>> circuitBreakerPolicy = null)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _commandScanner = commandScanner ?? throw new ArgumentNullException(nameof(commandScanner));
            _handlerFactory = handlerFactory;
            _commandHistory = new ConcurrentDictionary<CommandId, DateTime>();
            _dispatchSemaphore = new SemaphoreSlim(1, 1); // 添加缺失的信号量
            
            // 初始化缓存管理器和本地处理器映射缓存
            _cacheManager = commandScanner.GetCacheManager();
            _commandHandlerMap = new ConcurrentDictionary<CommandId, List<ICommandHandler>>();

            MaxConcurrencyPerCommand = maxConcurrencyPerCommand > 0 ? maxConcurrencyPerCommand : Environment.ProcessorCount;

            // 使用传入的熔断器策略，如果未提供则使用默认策略
            _circuit = circuitBreakerPolicy ?? Policy
                .HandleResult<BaseCommand<IResponse>>(r => !r.IsSuccess)
                .CircuitBreakerAsync(10, TimeSpan.FromMinutes(1)); // 增加到10次失败后熔断，持续1分钟

            // 创建三个优先级的Channel队列
            _commandChannels = new Channel<QueuedCommand>[3];
            _commandChannels[0] = Channel.CreateBounded<QueuedCommand>(new BoundedChannelOptions(1000)
            {
                FullMode = BoundedChannelFullMode.DropWrite   // 不再无限等待，直接丢弃
            });
            _commandChannels[1] = Channel.CreateBounded<QueuedCommand>(new BoundedChannelOptions(1000)
            {
                FullMode = BoundedChannelFullMode.DropWrite
            });
            _commandChannels[2] = Channel.CreateBounded<QueuedCommand>(new BoundedChannelOptions(1000)
            {
                FullMode = BoundedChannelFullMode.DropWrite
            });

            // 创建处理任务
            _channelProcessors = new Task[3];

        }

        /// <summary>
        /// 初始化调度器
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>初始化结果</returns>
        public async Task<bool> InitializeAsync(CancellationToken cancellationToken = default, params Assembly[] assemblies)
        {
            try
            {
                if (_isInitialized)
                    return true;

                // 确保处理器工厂已初始化
                if (_handlerFactory == null)
                {
                    _handlerFactory = new CommandHandlerFactory();
                }

                LogInfo("初始化命令调度器...");

                // 创建命令注册表 - 修复注册流程
                var commandRegistry = new CommandRegistry(_commandScanner.GetCacheManager());

                // 自动发现并注册处理器 - 真正注册到系统
                if (_commandScanner != null)
                {
                    if (assemblies.Length == 0)
                    {
                        assemblies = new[] { Assembly.GetExecutingAssembly() };
                    }

                    // 扫描并真正注册命令类型
                    await _commandScanner.ScanAndRegisterCommandsAsync(commandRegistry, null, assemblies);
                    
                    // 扫描并真正注册处理器类型
                    await _commandScanner.ScanAndRegisterHandlersAsync(commandRegistry, null, assemblies);

                    // 创建处理器实例并注册到映射 （实例级别）
                    var handlerTypes = _commandScanner.ScanCommandHandlers(null, assemblies);
                    foreach (var handlerType in handlerTypes)
                    {
                        try
                        {
                            // 跳过泛型类型定义，因为它们无法直接实例化
                            if (handlerType.IsGenericTypeDefinition)
                            {
                                LogInfo($"跳过泛型类型定义: {handlerType.FullName}");
                                continue;
                            }
                            
                            var handler = _handlerFactory.CreateHandler(handlerType);
                            await _commandScanner.RegisterHandlerAsync(handler, cancellationToken);
                            LogInfo($"处理器 {handler.Name} [ID: {handler.HandlerId}] 注册成功");
                        }
                        catch (Exception ex)
                        {
                            LogError($"创建处理器实例 {handlerType.Name} 失败", ex);
                        }
                    }
                }

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
                                    // 使用独立的取消令牌处理每个命令，避免一个命令的超时影响其他命令
                                    using (var commandCts = new CancellationTokenSource(TimeSpan.FromMinutes(5)))
                                    {
                                        var result = await ProcessAsync(queued, commandCts.Token);

                                        //这里之后会到具体的指令处理类OnHandleAsync去处理

                                        queued.Tcs.TrySetResult(result);
                                    }
                                }
                                catch (OperationCanceledException ex)
                                {
                                    LogWarning($"后台消费者线程处理命令超时或被取消: {queued.Command.CommandIdentifier.FullCode}");
                                    queued.Tcs.TrySetException(new TimeoutException($"命令处理超时: {ex.Message}"));
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
        public async Task<BaseCommand<IResponse>> DispatchAsync(PacketModel packet, ICommand command, CancellationToken cancellationToken = default)
        {


            if (!_isInitialized)
            {
                LogError("命令调度器未初始化");
                return BaseCommand<IResponse>.CreateError("命令调度器未初始化", 500);
            }

            // 确定命令优先级
            var channel = GetPriorityChannel(command.Priority);

            // 创建任务完成源
            var tcs = new TaskCompletionSource<BaseCommand<IResponse>>(TaskCreationOptions.RunContinuationsAsynchronously);

            // 创建队列项
            var queued = new QueuedCommand
            {
                Packet = packet,
                Command = command,
                Tcs = tcs,
            };

            // 将命令加入队列，非阻塞写入，满则立即返回 503
            if (!_commandChannels[channel].Writer.TryWrite(queued))
            {
                LogWarning($"Channel {channel} 已满，丢弃命令 {command.CommandIdentifier.FullCode}");
                return BaseCommand<IResponse>.CreateError("系统繁忙，请稍后重试", 503);
            }

            // 等待结果
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
                CommandPriority.High => 2,
                CommandPriority.Normal => 1,
                CommandPriority.Low => 0,
                _ => 1 // 默认使用普通优先级
            };
        }

        /// <summary>
        /// 处理异步命令（优化版：分层解析）
        /// </summary>
        /// <param name="cmd">命令对象</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<BaseCommand<IResponse>> ProcessAsync(QueuedCommand cmd, CancellationToken ct)
        {
            if (cmd == null)
            {
                return BaseCommand<IResponse>.CreateError("命令对象不能为空", 400);
            }

            if (!_isInitialized)
            {
                return BaseCommand<IResponse>.CreateError("调度器未初始化", 500);
            }

            // 创建链接的取消令牌，处理命令超时
            using (var linkedCts = CreateLinkedCancellationToken(ct, cmd.Command))
            {
                BaseCommand<IResponse> response = null;
#pragma warning disable CS0168 // 声明了变量，但从未使用过
                try
                {
                    // 验证命令对象
                    if (cmd?.Command == null)
                    {
                        LogError($"命令对象为空");
                        return BaseCommand<IResponse>.CreateError($"命令对象为空", 400);
                    }

                    // 第一层：命令基础验证（不解析具体数据）
                    var validationResult = await ValidateCommandBasicAsync(cmd.Command, ct);
                    if (!validationResult.IsValid)
                    {
                        LogWarning($"命令基础验证失败: {validationResult.ErrorMessage}");
                        return BaseCommand<IResponse>.CreateError($"命令验证失败: {validationResult.ErrorMessage}", 400);
                    }

                    // 第二层：命令预处理（获取指令信息，延迟具体解析）
                    var preprocessResult = await PreprocessCommandAsync(cmd.Command, ct);
                    if (!preprocessResult.Success)
                    {
                        LogWarning($"命令预处理失败: {preprocessResult.ErrorMessage}");
                        return BaseCommand<IResponse>.CreateError($"命令预处理失败: {preprocessResult.ErrorMessage}", 400);
                    }

                    // 幂等性检查 - 基于命令标识符和请求参数生成唯一键
                    if (cmd.Command.CommandIdentifier.FullCode != 0)
                    {
                        if (_idempotent.TryGetCached(cmd.Command, out var cached))
                        {
                            //return cached;
                        }
                    }

                    var startTime = DateTime.Now;
                    var commandIdentifier = cmd.Command.CommandIdentifier;

                    // 记录命令历史
                    _commandHistory.TryAdd(commandIdentifier, startTime);

                    // 查找合适的处理器和命令类型
                    var handlerCollection = FindHandlers(cmd);
                    var handlers = handlerCollection.Handlers;

                    if (handlers == null || !handlers.Any())
                    {

                        // 获取或初始化回退处理器
                        var fallbackHandler = GetFallbackHandler();
                        if (fallbackHandler != null)
                        {
                            try
                            {
                                // 使用熔断器执行回退处理器
                                response = await _circuit.ExecuteAsync(() => fallbackHandler.HandleAsync(cmd, linkedCts.Token));

                                // 设置执行时间
                                if (response != null)
                                {
                                    response.ExecutionTimeMs = (long)(DateTime.UtcNow - startTime).TotalMilliseconds;
                                }

                                return response;
                            }
                            catch (Exception ex)
                            {
                                LogError($"回退处理器处理命令时发生异常: {cmd.Command.CommandIdentifier.FullCode}", ex);
                                return BaseCommand<IResponse>.CreateError($"回退处理器异常: {ex.Message}", 500);
                            }
                        }

                        // 如果回退处理器也不可用，返回原始的404错误
                        return BaseCommand<IResponse>.CreateError(
                            $"没有找到适合的处理器处理命令: {cmd.Command.CommandIdentifier.FullCode}", 404);
                    }

                    // 选择最佳处理器
                    var bestHandler = SelectBestHandler(handlers, cmd.Command);
                    if (bestHandler == null)
                    {
                        return BaseCommand<IResponse>.CreateError(
                            $"无法选择合适的处理器处理命令: {cmd.Command.CommandIdentifier.FullCode}", 500);
                    }


                    // 使用熔断器执行处理逻辑
                    try
                    {
                        response = await _circuit.ExecuteAsync(() => bestHandler.HandleAsync(cmd, linkedCts.Token));
                    }
                    catch (Polly.CircuitBreaker.BrokenCircuitException ex)
                    {
                        // 熔断器已打开，记录详细信息并返回适当的错误
                        LogWarning($"命令 {cmd.Command.CommandIdentifier} [ID: {commandIdentifier.FullCode}] 的熔断器已打开，拒绝执行: {ex.Message}");
                        return BaseCommand<IResponse>.CreateError($"服务暂时不可用，熔断器已打开: {ex.Message}", 503);
                    }

                    // 设置执行时间
                    if (response != null)
                    {
                        response.ExecutionTimeMs = (long)(DateTime.UtcNow - startTime).TotalMilliseconds;
                    }


                    if (response == null)
                    {
                        response = BaseCommand<IResponse>.CreateError("处理器返回空结果", 500);
                    }

                    return response;
                }
                catch (OperationCanceledException ex)
                {
                    return BaseCommand<IResponse>.CreateError("命令处理超时或被取消", 504);
                }
                catch (Exception ex)
                {
                    LogError($"分发命令 {cmd.Command.CommandIdentifier.FullCode} 异常: {ex.Message}", ex);
                    return BaseCommand<IResponse>.CreateError($"命令分发异常: {ex.Message}", 500);
                }
                finally
                {
                    // 缓存结果以实现幂等性 - 基于命令标识符和请求参数生成唯一键
                    if (cmd.Command.CommandIdentifier != CommandId.Empty)
                    {
                        _idempotent.Cache(cmd.Command, response);
                    }

                    // 清理历史记录
                    _ = Task.Run(() => CleanupCommandHistory());
                }
#pragma warning restore CS0168 // 声明了变量，但从未使用过
            }
        }
 

        /// <summary>
        /// 获取所有处理器
        /// </summary>
        /// <returns>处理器列表</returns>
        /// <summary>
        /// 获取所有处理器（优化：使用本地缓存）
        /// </summary>
        /// <returns>所有处理器的只读列表</returns>
        public IReadOnlyList<ICommandHandler> GetAllHandlers()
        {
            // 优先使用本地缓存的处理器映射
            if (_commandHandlerMap != null && _commandHandlerMap.Count > 0)
            {
                return _commandHandlerMap
                    .SelectMany(kvp => kvp.Value)
                    .Distinct()
                    .ToList()
                    .AsReadOnly();
            }

            // 回退到从CommandScanner获取
            var handlerMappings = _commandScanner?.GetAllHandlerMappings();
            if (handlerMappings == null)
            {
                return new List<ICommandHandler>().AsReadOnly();
            }

            return handlerMappings
                .SelectMany(kvp => kvp.Value)
                .Distinct()
                .ToList()
                .AsReadOnly();
        }

        /// <summary>
        /// 获取处理器统计信息（优化：使用本地缓存）
        /// </summary>
        /// <returns>统计信息字典</returns>
        public Dictionary<string, HandlerStatistics> GetHandlerStatistics()
        {
            // 优先使用本地缓存的处理器映射
            if (_commandHandlerMap != null && _commandHandlerMap.Count > 0)
            {
                return _commandHandlerMap
                    .SelectMany(kvp => kvp.Value)
                    .Distinct()
                    .ToDictionary(
                        handler => handler.HandlerId,
                        handler => handler.GetStatistics()
                    );
            }

            // 回退到从CommandScanner获取
            var handlerMappings = _commandScanner?.GetAllHandlerMappings();
            if (handlerMappings == null)
            {
                return new Dictionary<string, HandlerStatistics>();
            }

            return handlerMappings
                .SelectMany(kvp => kvp.Value)
                .Distinct()
                .ToDictionary(
                    handler => handler.HandlerId,
                    handler => handler.GetStatistics()
                );
        }



        /// <summary>
        /// 查找处理器和命令类型
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <returns>包含处理器列表和命令类型的HandlerCollection</returns>
        private FallbackGenericCommandHandler GetFallbackHandler()
        {
            // 使用双重检查锁定模式确保线程安全
            if (_fallbackHandler == null)
            {
                lock (_fallbackHandlerLock)
                {
                    if (_fallbackHandler == null)
                    {
                        try
                        {
                            // 创建并初始化回退处理器
                            _fallbackHandler = new FallbackGenericCommandHandler();

                            // 初始化处理器
                            var initTask = _fallbackHandler.InitializeAsync(CancellationToken.None);
                            initTask.Wait(); // 同步等待初始化完成

                            if (!initTask.Result)
                            {
                                LogError("回退处理器初始化失败");
                                _fallbackHandler = null;
                                return null;
                            }

                            // 启动处理器
                            var startTask = _fallbackHandler.StartAsync(CancellationToken.None);
                            startTask.Wait(); // 同步等待启动完成

                            if (!startTask.Result)
                            {
                                LogError("回退处理器启动失败");
                                _fallbackHandler = null;
                                return null;
                            }

                            LogInfo("回退处理器初始化并启动成功");
                        }
                        catch (Exception ex)
                        {
                            LogError("创建回退处理器时发生异常", ex);
                            _fallbackHandler = null;
                        }
                    }
                }
            }

            return _fallbackHandler;
        }

        private HandlerCollection FindHandlers(QueuedCommand cmd)
        {
            var commandCode = cmd.Command.CommandIdentifier;

            // 从CommandScanner获取处理器映射信息（内存映射，最快路径）
            var handlers = _commandScanner?.GetHandlers(commandCode);
            var commandType = _commandScanner?.GetCommandType(commandCode);

            // 过滤可用的处理器
            List<ICommandHandler> availableHandlers = null;
            if (handlers != null && handlers.Any())
            {
                availableHandlers = handlers.Where(h => h.CanHandle(cmd)).ToList();
                if (availableHandlers.Any())
                {
                    // 内存映射中找到合适的处理器，直接返回
                    return new HandlerCollection(availableHandlers, commandType);
                }
            }

            // 内存映射中没有找到，尝试使用缓存管理器进行更全面的搜索
            if (_commandScanner?.GetCacheManager() != null)
            {
                var cacheManager = _commandScanner.GetCacheManager();
                
                // 如果命令类型未知，尝试从缓存中获取
                if (commandType == null)
                {
                    // 获取当前命令所在程序集
                    var commandAssembly = cmd.Command.GetType().Assembly;
                    
                    try
                    {
                        // 异步获取扫描缓存，但使用同步方式等待（避免在请求处理路径中引入异步）
                        var scanCache = cacheManager.GetOrCreateScanCacheAsync(commandAssembly, null, false).GetAwaiter().GetResult();
                        
                        if (scanCache?.ScanResults != null && scanCache.ScanResults.TryGetValue(commandCode, out var cachedCommandType))
                        {
                            commandType = cachedCommandType;
                            LogDebug($"从缓存中获取命令类型: {commandType?.Name} for command {commandCode.FullCode}");
                        }
                    }
                    catch (Exception ex)
                    {
                        LogWarning($"从缓存获取命令类型失败: {ex.Message}");
                    }
                }

                // 如果找到了命令类型，尝试从缓存中查找兼容的处理器类型
                if (commandType != null && _handlerFactory != null)
                {
                    try
                    {
                        // 获取所有已扫描程序集的处理器类型
                        var allHandlerTypes = GetAllCachedHandlerTypes(cacheManager);
                        
                        // 筛选兼容的处理器类型
                        var compatibleHandlerTypes = allHandlerTypes
                            .Where(ht => CanHandleCommandTypeByType(ht, commandType))
                            .ToList();

                        // 创建处理器实例并验证处理能力
                        var compatibleHandlers = new List<ICommandHandler>();
                        foreach (var handlerType in compatibleHandlerTypes)
                        {
                            try
                            {
                                // 跳过泛型类型定义，因为它们无法直接实例化
                                if (handlerType.IsGenericTypeDefinition)
                                {
                                    // 尝试根据命令类型动态创建泛型处理器的具体实例
                                    if (commandType != null && commandType.IsGenericType)
                                    {
                                        var genericArgs = commandType.GetGenericArguments();
                                        if (genericArgs.Length > 0)
                                        {
                                            try
                                            {
                                                // 使用新的CreateGenericHandler方法创建泛型处理器实例
                                                var genericHandler = _handlerFactory.CreateGenericHandler(handlerType, genericArgs);
                                                if (genericHandler != null && genericHandler.CanHandle(cmd))
                                                {
                                                    compatibleHandlers.Add(genericHandler);
                                                    LogDebug($"找到兼容处理器: {genericHandler.Name} for command type: {commandType.Name}");
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                LogWarning($"创建泛型处理器实例失败: {handlerType.Name}<{string.Join(", ", genericArgs.Select(t => t.Name))}>: {ex.Message}");
                                            }
                                        }
                                    }
                                    continue;
                                }
                                
                                var handler = _handlerFactory.CreateHandler(handlerType);
                                if (handler != null && handler.CanHandle(cmd))
                                {
                                    compatibleHandlers.Add(handler);
                                    LogDebug($"找到兼容处理器: {handler.Name} for command type: {commandType.Name}");
                                }
                            }
                            catch (Exception ex)
                            {
                                LogWarning($"创建处理器实例 {handlerType.Name} 失败: {ex.Message}");
                            }
                        }

                        if (compatibleHandlers.Any())
                        {
                            return new HandlerCollection(compatibleHandlers, commandType);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogWarning($"从缓存搜索处理器失败: {ex.Message}");
                    }
                }
            }

            // 缓存搜索失败，回退到传统的内存映射搜索
            return FindHandlersFromMemoryMapping(cmd, commandCode, commandType);
        }

        /// <summary>
        /// 从内存映射中查找处理器（传统方式）
        /// </summary>
        private HandlerCollection FindHandlersFromMemoryMapping(QueuedCommand cmd, CommandId commandCode, Type commandType)
        {
            var handlerMappings = _commandScanner?.GetAllHandlerMappings();
            if (handlerMappings == null)
            {
                return new HandlerCollection(null, commandType);
            }

            // 策略1：如果知道命令类型，尝试查找兼容处理器
            if (commandType != null)
            {
                var compatibleHandlers = handlerMappings
                    .SelectMany(kvp => kvp.Value)
                    .Where(h => CanHandleCommandType(h, commandType) && h.CanHandle(cmd))
                    .ToList();

                if (compatibleHandlers.Any())
                {
                    return new HandlerCollection(compatibleHandlers, commandType);
                }

                // 策略2：尝试创建动态处理器（如果支持）
                var dynamicHandler = TryCreateDynamicHandler(commandType, commandCode);
                if (dynamicHandler != null)
                {
                    return new HandlerCollection(new List<ICommandHandler> { dynamicHandler }, commandType);
                }
            }

            // 策略3：如果没有命令类型信息，遍历所有处理器
            var fallbackHandlers = handlerMappings
                .SelectMany(kvp => kvp.Value)
                .Where(h => h.CanHandle(cmd))
                .ToList();

            return new HandlerCollection(fallbackHandlers.Any() ? fallbackHandlers : null, commandType);
        }

        /// <summary>
        /// 获取所有缓存的处理器类型
        /// </summary>
        private List<Type> GetAllCachedHandlerTypes(CommandCacheManager cacheManager)
        {
            var handlerTypes = new List<Type>();
            
            try
            {
                // 获取当前应用程序域中的所有程序集（排除系统程序集）
                var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !a.IsDynamic && !a.FullName.StartsWith("System."))
                    .ToArray();

                foreach (var assembly in assemblies)
                {
                    try
                    {
                        // 从缓存获取扫描结果
                        var scanCache = cacheManager.GetOrCreateScanCacheAsync(assembly, null, false).GetAwaiter().GetResult();
                        
                        // 从处理器类型缓存中获取处理器类型
                        var handlerTypeEntries = cacheManager.GetAllCachedHandlerTypes();
                        if (handlerTypeEntries != null && handlerTypeEntries.Count > 0)
                        {
                            handlerTypes.AddRange(handlerTypeEntries.Where(t => t != null));
                        }
                    }
                    catch (Exception ex)
                    {
                        LogDebug($"获取程序集 {assembly.GetName().Name} 的处理器缓存失败: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                LogWarning($"获取所有缓存处理器类型失败: {ex.Message}");
            }

            return handlerTypes.Distinct().ToList();
        }

        /// <summary>
        /// 通过类型检查处理器是否兼容命令类型（高性能版本）
        /// </summary>
        private bool CanHandleCommandTypeByType(Type handlerType, Type commandType)
        {
            if (handlerType == null || commandType == null)
                return false;

            try
            {
                // 检查处理器是否实现了泛型接口
                var interfaces = handlerType.GetInterfaces();
                foreach (var iface in interfaces)
                {
                    if (iface.IsGenericType)
                    {
                        var genericType = iface.GetGenericTypeDefinition();
                        // 检查是否实现了泛型命令处理器接口
                        if (genericType.Name.StartsWith("ICommandHandler"))
                        {
                            var genericArgs = iface.GetGenericArguments();
                            if (genericArgs.Length > 0)
                            {
                                // 检查第一个泛型参数是否与命令类型兼容
                                var supportedCommandType = genericArgs[0];
                                if (supportedCommandType.IsAssignableFrom(commandType) ||
                                    commandType.IsAssignableFrom(supportedCommandType))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }

                // 检查基类是否支持
                var baseType = handlerType.BaseType;
                while (baseType != null && baseType != typeof(object))
                {
                    if (baseType.IsGenericType)
                    {
                        var genericArgs = baseType.GetGenericArguments();
                        if (genericArgs.Length > 0)
                        {
                            var supportedCommandType = genericArgs[0];
                            if (supportedCommandType.IsAssignableFrom(commandType) ||
                                commandType.IsAssignableFrom(supportedCommandType))
                            {
                                return true;
                            }
                        }
                    }
                    baseType = baseType.BaseType;
                }

                return false;
            }
            catch (Exception ex)
            {
                LogError($"检查处理器类型 {handlerType.Name} 兼容性时发生异常: {ex.Message}", ex);
                return false;
            }
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
        /// 命令基础验证结果
        /// </summary>
        private class CommandValidationResult
        {
            public bool IsValid { get; set; }
            public string ErrorMessage { get; set; }
            public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
        }

        /// <summary>
        /// 命令预处理结果
        /// </summary>
        private class CommandPreprocessResult
        {
            public bool Success { get; set; }
            public string ErrorMessage { get; set; }
            public CommandMetadata Metadata { get; set; }
        }

        /// <summary>
        /// 命令元数据
        /// </summary>
        private class CommandMetadata
        {
            public CommandId CommandId { get; set; }
            public string CommandName { get; set; }
            public bool RequiresAuthentication { get; set; }
            public CommandPriority Priority { get; set; }
            public Type TargetHandlerType { get; set; }
            public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
        }

        /// <summary>
        /// 基础验证（不解析具体数据）
        /// </summary>
        private async Task<CommandValidationResult> ValidateCommandBasicAsync(ICommand command, CancellationToken cancellationToken)
        {
            await Task.CompletedTask; // 异步占位符

            try
            {
                // 基础验证：命令ID有效性
                if (command.CommandIdentifier == CommandId.Empty)
                {
                    return new CommandValidationResult { IsValid = false, ErrorMessage = "命令ID无效" };
                }

                // 基础验证：会话ID存在性
                //if (string.IsNullOrEmpty(command.ExecutionContext.SessionId))
                //{
                //    return new CommandValidationResult { IsValid = false, ErrorMessage = "会话ID不能为空" };
                //}

                // 基础验证：命令类型存在性
                var commandType = command.GetType();
                if (commandType == null)
                {
                    return new CommandValidationResult { IsValid = false, ErrorMessage = "命令类型无效" };
                }

                return new CommandValidationResult { IsValid = true };
            }
            catch (Exception ex)
            {
                LogError($"基础验证异常: {ex.Message}", ex);
                return new CommandValidationResult { IsValid = false, ErrorMessage = $"验证异常: {ex.Message}" };
            }
        }

        /// <summary>
        /// 预处理命令（获取指令信息，延迟具体解析）
        /// </summary>
        private async Task<CommandPreprocessResult> PreprocessCommandAsync(ICommand command, CancellationToken cancellationToken)
        {
            await Task.CompletedTask; // 异步占位符

            try
            {
                var commandId = command.CommandIdentifier;
                var commandName = command.GetType().Name;

                // 获取处理器类型（不实例化）
                var handlerTypes = GetHandlerTypesForCommand(commandId);
                var targetHandlerType = handlerTypes.FirstOrDefault();

                var metadata = new CommandMetadata
                {
                    CommandId = commandId,
                    CommandName = commandName,
                    RequiresAuthentication = IsAuthenticationRequired(commandId),
                    Priority = command.Priority,
                    TargetHandlerType = targetHandlerType
                };

                return new CommandPreprocessResult
                {
                    Success = true,
                    Metadata = metadata
                };
            }
            catch (Exception ex)
            {
                LogError($"预处理异常: {ex.Message}", ex);
                return new CommandPreprocessResult { Success = false, ErrorMessage = $"预处理异常: {ex.Message}" };
            }
        }

        /// <summary>
        /// 判断是否需要认证
        /// </summary>
        private bool IsAuthenticationRequired(CommandId commandId)
        {
            // 登录相关命令不需要认证
            var authCommands = new CommandId[]
            {
                AuthenticationCommands.Login,
                AuthenticationCommands.LoginRequest,
                AuthenticationCommands.PrepareLogin,
                AuthenticationCommands.ValidateToken,
                AuthenticationCommands.RefreshToken
            };

            return !authCommands.Contains(commandId);
        }




        /// <summary>
        /// 检查处理器是否能处理指定的命令类型
        /// </summary>
        /// <param name="handler">处理器实例</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>是否能处理</returns>
        private bool CanHandleCommandType(ICommandHandler handler, Type commandType)
        {
            if (handler == null || commandType == null)
                return false;

            try
            {
                // 获取处理器的类型
                var handlerType = handler.GetType();

                // 检查处理器是否实现了泛型接口
                var interfaces = handlerType.GetInterfaces();
                foreach (var iface in interfaces)
                {
                    if (iface.IsGenericType)
                    {
                        var genericType = iface.GetGenericTypeDefinition();
                        // 检查是否实现了泛型命令处理器接口
                        if (genericType == typeof(ICommandHandler) ||
                            genericType.Name.StartsWith("ICommandHandler"))
                        {
                            var genericArgs = iface.GetGenericArguments();
                            if (genericArgs.Length > 0)
                            {
                                // 检查第一个泛型参数是否与命令类型兼容
                                var supportedCommandType = genericArgs[0];
                                if (supportedCommandType.IsAssignableFrom(commandType) ||
                                    commandType.IsAssignableFrom(supportedCommandType))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }

                // 检查基类是否支持
                var baseType = handlerType.BaseType;
                while (baseType != null && baseType != typeof(object))
                {
                    if (baseType.IsGenericType)
                    {
                        var genericArgs = baseType.GetGenericArguments();
                        if (genericArgs.Length > 0)
                        {
                            var supportedCommandType = genericArgs[0];
                            if (supportedCommandType.IsAssignableFrom(commandType) ||
                                commandType.IsAssignableFrom(supportedCommandType))
                            {
                                return true;
                            }
                        }
                    }
                    baseType = baseType.BaseType;
                }

                return false;
            }
            catch (Exception ex)
            {
                LogError($"检查处理器类型兼容性时发生异常: {ex.Message}", ex);
                return false;
            }
        }

        /// <summary>
        /// 尝试创建动态处理器
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <param name="commandCode">命令代码</param>
        /// <returns>动态处理器或null</returns>
        private ICommandHandler TryCreateDynamicHandler(Type commandType, CommandId commandCode)
        {
            try
            {
                // 这里可以实现动态处理器创建逻辑
                // 目前使用回退处理器作为动态处理器
                var fallbackHandler = GetFallbackHandler();

                if (fallbackHandler != null)
                {
                    LogInfo($"使用回退处理器作为动态处理器处理命令类型: {commandType?.Name}, 命令代码: {commandCode}");
                    return fallbackHandler;
                }

                return null;
            }
            catch (Exception ex)
            {
                LogError($"创建动态处理器失败，命令类型: {commandType?.Name}, 命令代码: {commandCode}", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取命令对应的处理器类型
        /// </summary>
        private Type[] GetHandlerTypesForCommand(CommandId commandId)
        {
            var handlers = _commandScanner?.GetHandlers(commandId);
            if (handlers != null)
            {
                return handlers.Select(h => h.GetType()).Distinct().ToArray();
            }
            return new Type[0];
        }

        /// <summary>
        /// 创建链接的取消令牌，考虑命令超时设置
        /// </summary>
        /// <param name="cancellationToken">外部取消令牌</param>
        /// <param name="command">命令对象</param>
        /// <returns>链接的取消令牌源</returns>
        private CancellationTokenSource CreateLinkedCancellationToken(CancellationToken cancellationToken, ICommand command)
        {
            // 如果命令有设置超时时间，创建带超时的取消令牌
            if (command.TimeoutMs > 0)
            {
                var timeoutCts = new CancellationTokenSource(TimeSpan.FromMilliseconds(command.TimeoutMs));

                // 如果外部取消令牌不为空且未取消，创建链接的取消令牌
                if (cancellationToken != CancellationToken.None && !cancellationToken.IsCancellationRequested)
                {
                    return CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);
                }

                LogDebug($"创建链接取消令牌，超时时间: {TimeSpan.FromMilliseconds(command.TimeoutMs).TotalSeconds}秒, 命令: {command.CommandIdentifier.FullCode}");
                return timeoutCts;
            }

            // 如果外部取消令牌不为空，返回链接的取消令牌
            if (cancellationToken != CancellationToken.None)
            {
                var cts = new CancellationTokenSource();
                return CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
            }

            // 默认返回新的取消令牌源，设置30秒默认超时
            var defaultCts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            LogDebug($"创建默认链接取消令牌，超时时间: 30秒, 命令: {command.CommandIdentifier.FullCode}");
            return defaultCts;
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
        /// 清理注册的命令类型
        /// </summary>
        public void ClearCommandTypes()
        {
            try
            {
                _commandScanner?.Clear();
                LogInfo("已清理所有注册的命令类型");
            }
            catch (Exception ex)
            {
                LogError("清理命令类型异常", ex);
            }
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

                    // 获取所有处理器并停止和释放（优化：优先使用本地缓存）
                    List<ICommandHandler> allHandlers;
                    if (_commandHandlerMap != null && _commandHandlerMap.Count > 0)
                    {
                        // 优先使用本地缓存的处理器映射
                        allHandlers = _commandHandlerMap.Values
                            .SelectMany(list => list)
                            .Distinct()
                            .ToList();
                    }
                    else
                    {
                        // 回退到从CommandScanner获取
                        allHandlers = _commandScanner?.GetAllHandlerMappings()
                            ?.SelectMany(kvp => kvp.Value)
                            .Distinct()
                            .ToList() ?? new List<ICommandHandler>();
                    }

                    // 停止所有处理器
                    var stopTasks = allHandlers.Select(h => h.StopAsync(CancellationToken.None));
                    Task.WhenAll(stopTasks).GetAwaiter().GetResult();

                    // 释放所有处理器
                    foreach (var handler in allHandlers)
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




}
