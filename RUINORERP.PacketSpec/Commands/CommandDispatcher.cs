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
    /// 1. 直接扫描程序集中的命令处理器
    /// 2. 根据数据包中的CommandId查找对应的处理器
    /// 3. 执行命令处理逻辑并返回结果
    /// </summary>
    public class CommandDispatcher : IDisposable, ICommandDispatcher
    {
        private readonly ConcurrentDictionary<CommandId, DateTime> _commandHistory;
        private ICommandHandlerFactory _handlerFactory;
        private int _maxConcurrencyPerCommand;
        private bool _disposed = false;
        private bool _isInitialized = false;
        private readonly SemaphoreSlim _dispatchSemaphore;
        private readonly Channel<QueuedCommand>[] _commandChannels;
        private readonly Task[] _channelProcessors;
        private readonly IAsyncPolicy<ResponseBase> _circuit;
        private readonly IdempotencyFilter _idempotent = new IdempotencyFilter();
        private FallbackGenericCommandHandler _fallbackHandler;
        private readonly object _fallbackHandlerLock = new object();

        // 处理器映射 - 直接映射CommandId到处理器
        private readonly ConcurrentDictionary<CommandId, List<ICommandHandler>> _commandHandlerMap;

        // 所有已注册的处理器
        private readonly List<ICommandHandler> _allHandlers;

        // 日志记录器
        protected ILogger<CommandDispatcher> Logger { get; set; }


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
        public int HandlerCount => _allHandlers.Count;

        /// <summary>
        /// 是否已初始化
        /// </summary>
        public bool IsInitialized => _isInitialized;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="handlerFactory">处理器工厂</param>
        /// <param name="maxConcurrencyPerCommand">每个命令的最大并发数</param>
        /// <param name="circuitBreakerPolicy">熔断器策略，默认为6次失败后熔断，30秒后恢复</param>
        public CommandDispatcher(ILogger<CommandDispatcher> logger, ICommandHandlerFactory handlerFactory = null,
            int maxConcurrencyPerCommand = 0,
            IAsyncPolicy<ResponseBase> circuitBreakerPolicy = null)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _handlerFactory = handlerFactory;
            _commandHistory = new ConcurrentDictionary<CommandId, DateTime>();
            _dispatchSemaphore = new SemaphoreSlim(1, 1);
            _commandHandlerMap = new ConcurrentDictionary<CommandId, List<ICommandHandler>>();
            _allHandlers = new List<ICommandHandler>();

            MaxConcurrencyPerCommand = maxConcurrencyPerCommand > 0 ? maxConcurrencyPerCommand : Environment.ProcessorCount;

            // 使用传入的熔断器策略，如果未提供则使用默认策略
            _circuit = circuitBreakerPolicy ?? Policy
                .HandleResult<ResponseBase>(r => r != null && !r.IsSuccess)
                .CircuitBreakerAsync(10, TimeSpan.FromMinutes(1));

            // 创建三个优先级的Channel队列
            _commandChannels = new Channel<QueuedCommand>[3];
            _commandChannels[0] = Channel.CreateBounded<QueuedCommand>(new BoundedChannelOptions(1000)
            {
                FullMode = BoundedChannelFullMode.DropWrite
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

                // 如果未提供程序集，默认扫描当前程序集和引用的RUINORERP.PacketSpec程序集
                if (assemblies.Length == 0)
                {
                    var currentAssembly = Assembly.GetExecutingAssembly();
                    var packetSpecAssembly = Assembly.GetAssembly(typeof(PacketModel));
                    assemblies = new[] { currentAssembly };

                    if (packetSpecAssembly != null && packetSpecAssembly != currentAssembly)
                    {
                        assemblies = assemblies.Concat(new[] { packetSpecAssembly }).ToArray();
                    }
                }

                // 扫描并注册处理器
                await ScanAndRegisterHandlersAsync(assemblies, cancellationToken);

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
                                    LogWarning($"后台消费者线程处理命令超时或被取消: {queued.Packet.CommandId.FullCode}");
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
        /// 扫描并注册处理器
        /// </summary>
        /// <param name="assemblies">要扫描的程序集</param>
        /// <param name="cancellationToken">取消令牌</param>
        private async Task ScanAndRegisterHandlersAsync(Assembly[] assemblies, CancellationToken cancellationToken)
        {
            try
            {
                LogInfo($"开始扫描程序集，共 {assemblies.Length} 个程序集");

                var handlerTypes = new List<Type>();

                // 扫描所有程序集中的处理器类型
                foreach (var assembly in assemblies)
                {
                    try
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        // 获取所有实现了ICommandHandler接口的非抽象类型
                        var assemblyHandlerTypes = assembly.GetTypes()
                            .Where(t => !t.IsAbstract && !t.IsInterface && typeof(ICommandHandler).IsAssignableFrom(t))
                            .ToList();

                        handlerTypes.AddRange(assemblyHandlerTypes);
                        LogInfo($"从程序集 {assembly.GetName().Name} 中发现 {assemblyHandlerTypes.Count} 个处理器类型");
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        LogWarning($"扫描程序集 {assembly.GetName().Name} 时发生类型加载异常: {ex.Message}");
                        // 记录无法加载的类型信息
                        if (ex.LoaderExceptions != null)
                        {
                            foreach (var loaderEx in ex.LoaderExceptions)
                            {
                                LogDebug($"加载器异常: {loaderEx.Message}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogError($"扫描程序集 {assembly.GetName().Name} 时发生异常", ex);
                    }
                }

                LogInfo($"总共发现 {handlerTypes.Count} 个处理器类型");

                // 创建并注册处理器
                foreach (var handlerType in handlerTypes)
                {
                    try
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        // 使用处理器工厂创建处理器实例
                        ICommandHandler handler = null;

                        if (_handlerFactory != null)
                        {
                            handler = _handlerFactory.CreateHandler(handlerType);
                        }
                        else
                        {
                            // 如果没有处理器工厂，直接创建实例
                            handler = (ICommandHandler)Activator.CreateInstance(handlerType);
                        }

                        if (handler != null)
                        {
                            // 初始化处理器
                            await handler.InitializeAsync(cancellationToken);

                            // 启动处理器
                            await handler.StartAsync(cancellationToken);

                            // 获取处理器支持的命令ID列表
                            var supportedCommands = handler.SupportedCommands;

                            // 注册处理器到命令映射
                            foreach (var commandId in supportedCommands)
                            {
                                _commandHandlerMap.AddOrUpdate(
                                    commandId,
                                    new List<ICommandHandler> { handler },
                                    (key, existingHandlers) =>
                                    {
                                        existingHandlers.Add(handler);
                                        return existingHandlers;
                                    });
                            }

                            // 添加到所有处理器列表
                            lock (_allHandlers)
                            {
                                _allHandlers.Add(handler);
                            }

                            LogInfo($"已注册处理器: {handler.Name}, 支持命令数: {supportedCommands.Count}");
                        }
                    }
                    catch (Exception ex)
                    {
                        LogError($"创建或初始化处理器 {handlerType.Name} 时发生异常", ex);
                    }
                }

                LogInfo($"处理器注册完成，共注册 {_allHandlers.Count} 个处理器实例");
            }
            catch (OperationCanceledException)
            {
                LogInfo("处理器扫描和注册已被取消");
                throw;
            }
            catch (Exception ex)
            {
                LogError("扫描和注册处理器时发生异常", ex);
                throw;
            }
        }

        /// <summary>
        /// 异步分发数据包
        /// </summary>
        /// <param name="packet">数据包对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        public async Task<ResponseBase> DispatchAsync(PacketModel packet, CancellationToken cancellationToken = default)
        {


            if (!_isInitialized)
            {
                LogError("命令调度器未初始化");
                return ResponseBase.CreateError("命令调度器未初始化", 500);
            }

            // 确定优先级
            var channel = GetPriorityChannel(packet.PacketPriority);

            // 创建任务完成源
            var tcs = new TaskCompletionSource<ResponseBase>(TaskCreationOptions.RunContinuationsAsynchronously);

            // 创建队列项
            var queued = new QueuedCommand
            {
                Packet = packet,
                Tcs = tcs,
            };

            // 将命令加入队列，非阻塞写入，满则立即返回 503
            if (!_commandChannels[channel].Writer.TryWrite(queued))
            {
                LogWarning($"Channel {channel} 已满，丢弃命令 {packet.CommandId.FullCode},{packet.CommandId.Name}");
                return ResponseBase.CreateError("系统繁忙，请稍后重试", 503);
            }

            // 等待结果
            return await tcs.Task;
        }



        /// <summary>
        /// 根据命令优先级确定应该使用的Channel队列
        /// </summary>
        /// <param name="priority">命令优先级</param>
        /// <returns>Channel队列索引 (0=高优先级, 1=普通优先级, 2=低优先级)</returns>
        private int GetPriorityChannel(PacketPriority priority)
        {
            return priority switch
            {
                PacketPriority.High => 2,
                PacketPriority.Normal => 1,
                PacketPriority.Low => 0,
                _ => 1 // 默认使用普通优先级
            };
        }

        /// <summary>
        /// 处理异步命令（优化版：分层解析）
        /// </summary>
        /// <param name="cmd">命令对象</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<ResponseBase> ProcessAsync(QueuedCommand cmd, CancellationToken ct)
        {
            if (cmd == null || cmd.Packet == null)
            {
                return ResponseBase.CreateError("命令对象不能为空", 400);
            }

            if (!_isInitialized)
            {
                return ResponseBase.CreateError("调度器未初始化", 500);
            }

            // 创建链接的取消令牌，处理命令超时
            using (var linkedCts = CreateLinkedCancellationToken(ct, cmd.Packet.CommandId))
            {
                ResponseBase response = null;
#pragma warning disable CS0168 // 声明了变量，但从未使用过
                try
                {

                    // 幂等性检查 - 基于命令标识符和请求参数生成唯一键
                    if (cmd.Packet.CommandId.FullCode != 0)
                    {
                        if (_idempotent.TryGetCached(cmd.Packet.Request, cmd.Packet.CommandId, out var cached))
                        {
                            return cached;
                        }
                    }

                    var startTime = DateTime.Now;
                    var commandIdentifier = cmd.Packet.CommandId;

                    // 记录命令历史
                    _commandHistory.TryAdd(commandIdentifier, startTime);

                    // 查找合适的处理器和命令类型
                    var handlerCollection = FindHandlers(cmd);
                    var handlers = handlerCollection;

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

                                return response;
                            }
                            catch (Exception ex)
                            {
                                LogError($"回退处理器处理命令时发生异常: {cmd.Packet.CommandId.ToString()}", ex);
                                return ResponseBase.CreateError($"回退处理器异常: {ex.Message}", 500);
                            }
                        }

                        // 如果回退处理器也不可用，返回原始的404错误
                        return ResponseBase.CreateError(
                            $"没有找到适合的处理器处理命令: {cmd.Packet.CommandId.ToString()}", 404);
                    }

                    // 选择最佳处理器
                    var bestHandler = SelectBestHandler(handlers, cmd.Packet.CommandId);
                    if (bestHandler == null)
                    {
                        return ResponseBase.CreateError($"无法选择合适的处理器处理命令: {cmd.Packet.CommandId.ToString()}");
                    }


                    // 使用熔断器执行处理逻辑
                    try
                    {
                        response = await _circuit.ExecuteAsync(() => bestHandler.HandleAsync(cmd, linkedCts.Token));
                    }
                    catch (Polly.CircuitBreaker.BrokenCircuitException ex)
                    {
                        // 熔断器已打开，记录详细信息并返回适当的错误
                        LogWarning($"命令 {cmd.Packet.CommandId.ToString()}[ID: {commandIdentifier.FullCode}] 的熔断器已打开，拒绝执行: {ex.Message}");
                        return ResponseBase.CreateError($"服务暂时不可用，熔断器已打开: {ex.Message}", 503);
                    }

                    //// 设置执行时间
                    if (response != null && response != null)
                    {
                        response.ExecutionTimeMs = (long)(DateTime.Now - startTime).TotalMilliseconds;
                    }

                    if (response == null)
                    {
                        response = ResponseBase.CreateError("处理器返回空结果", 500);
                    }

                    return response;
                }
                catch (OperationCanceledException ex) when (ex.CancellationToken == linkedCts.Token)
                {
                    // 区分超时/取消异常与外部取消请求
                    LogInfo($"命令处理超时:{cmd.Packet.CommandId.ToString()}");
                    return ResponseBase.CreateError("命令处理超时", 504);
                }
                catch (OperationCanceledException ex)
                {
                    // 外部取消请求
                    LogInfo($"命令处理被外部取消: {cmd.Packet.CommandId.ToString()}");
                    return ResponseBase.CreateError("命令处理被取消", 504);
                }
                catch (Exception ex)
                {
                    LogError($"分发命令 {cmd.Packet.CommandId.ToString()} 异常: {ex.Message}", ex);
                    return ResponseBase.CreateError($"命令分发异常: {ex.Message}", 500);
                }
                finally
                {
                    // 缓存结果以实现幂等性 - 基于命令标识符和请求参数生成唯一键
                    if (cmd.Packet.CommandId != CommandId.Empty)
                    {
                        _idempotent.Cache(cmd.Packet, response);
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
        public IReadOnlyList<ICommandHandler> GetAllHandlers()
        {
            return _allHandlers.ToList().AsReadOnly();
        }

        /// <summary>
        /// 获取处理器统计信息
        /// </summary>
        /// <returns>统计信息字典</returns>
        public Dictionary<string, HandlerStatistics> GetHandlerStatistics()
        {
            return _allHandlers
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

                            // 减少信息日志，在生产环境中不记录
#if DEBUG
                            LogInfo("回退处理器初始化并启动成功");
#endif
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

        /// <summary>
        /// 查找能处理指定命令的处理器
        /// </summary>
        /// <param name="cmd">命令对象</param>
        /// <returns>处理器集合</returns>
        private List<ICommandHandler> FindHandlers(QueuedCommand cmd)
        {
            var commandCode = cmd.Packet.CommandId;
            var commandType = cmd.Packet.GetType();

            try
            {
                // 1. 首先从处理器映射中查找（按命令ID）
                if (_commandHandlerMap.TryGetValue(commandCode, out var handlersByCommandId))
                {
                    var compatibleHandlers = handlersByCommandId
                        .Where(h => h.CanHandle(cmd))
                        .ToList();

                    if (compatibleHandlers.Any())
                    {
                        return compatibleHandlers;
                    }
                }

                // 2. 如果按命令ID没有找到，尝试在所有处理器中查找兼容的处理器
                var allCompatibleHandlers = _allHandlers
                    .Where(h => h.CanHandle(cmd))
                    .ToList();

                if (allCompatibleHandlers.Any())
                {
                    return allCompatibleHandlers;
                }

                // 3. 尝试创建动态处理器
                var dynamicHandler = TryCreateDynamicHandler(commandType, commandCode);
                if (dynamicHandler != null)
                {
                    return new List<ICommandHandler> { dynamicHandler };
                }
            }
            catch (Exception ex)
            {
                LogWarning($"查找处理器失败: {ex.Message}");
            }

            // 如果没有找到兼容的处理器，返回空集合
            return new List<ICommandHandler>();
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
        /// <param name="commandId">命令ID</param>
        /// <returns>最佳处理器</returns>
        private ICommandHandler SelectBestHandler(List<ICommandHandler> handlers, CommandId commandId)
        {
            if (handlers == null || !handlers.Any())
                return null;

            // 按优先级和当前处理负载选择
            // 允许已初始化或运行中的处理器
            return handlers
                .Where(h => h.Status == HandlerStatus.Initialized || h.Status == HandlerStatus.Running)
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
            public PacketPriority Priority { get; set; }
            public Type TargetHandlerType { get; set; }
            public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
        }






        /// <summary>
        /// 尝试创建动态处理器
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <param name="commandCode">命令代码</param>
        /// <returns>动态处理器实例</returns>
        private ICommandHandler TryCreateDynamicHandler(Type commandType, CommandId commandCode)
        {
            try
            {
                // 创建回退处理器
                if (_fallbackHandler == null)
                {
                    lock (_fallbackHandlerLock)
                    {
                        if (_fallbackHandler == null)
                        {
                            _fallbackHandler = new FallbackGenericCommandHandler();
                        }
                    }
                }

                // 返回回退处理器
                return _fallbackHandler;
            }
            catch (Exception ex)
            {
                LogError($"创建动态处理器失败: {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取命令对应的处理器类型
        /// </summary>
        private Type[] GetHandlerTypesForCommand(CommandId commandId)
        {
            // 从处理器映射中查找
            if (_commandHandlerMap.TryGetValue(commandId, out var handlers))
            {
                return handlers.Select(h => h.GetType()).Distinct().ToArray();
            }
            return new Type[0];
        }

        /// <summary>
        /// 创建链接的取消令牌 - 移除了对命令超时的依赖
        /// </summary>
        /// <param name="cancellationToken">外部取消令牌</param>
        /// <param name="command">命令对象</param>
        /// <returns>链接的取消令牌源</returns>
        private CancellationTokenSource CreateLinkedCancellationToken(CancellationToken cancellationToken, CommandId CommandIdentifier)
        {
            // 如果外部取消令牌不为空且未取消，创建链接的取消令牌
            if (cancellationToken != CancellationToken.None && !cancellationToken.IsCancellationRequested)
            {
                // 正确创建链接令牌源，避免修改原始令牌
                return CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            }

            // 默认返回新的取消令牌源，设置30秒默认超时
            var defaultCts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            LogDebug($"创建默认链接取消令牌，超时时间: 30秒, 命令: {CommandIdentifier.FullCode}{CommandIdentifier.Name}");
            return defaultCts;
        }



        /// <summary>
        /// 清理命令历史
        /// </summary>
        private void CleanupCommandHistory()
        {
            try
            {
                var cutoff = DateTime.Now.AddMinutes(-30); // 保留30分钟的历史
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
            // 在生产环境中减少调试日志
#if DEBUG
            Logger.LogDebug(message);
#endif
        }

        /// <summary>
        /// 记录信息日志
        /// </summary>
        public void LogInfo(string message)
        {
            // 仅记录关键信息日志
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
                // 清理处理器映射和所有处理器列表
                _commandHandlerMap.Clear();
                _allHandlers.Clear();
                LogInfo("已清理所有注册的命令类型和处理器");
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
                        // 使用本地存储的所有处理器
                        allHandlers = _allHandlers.ToList();
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
