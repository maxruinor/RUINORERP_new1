using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using System.Threading.Channels;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Models.Responses.Authentication;

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
    /// 
    /// 熔断机制改进（2024年更新）：
    /// 1. 区分系统错误和业务错误 - 熔断器仅在系统错误时触发
    /// 2. 添加会话级熔断器 - 避免单个会话影响全局服务
    /// 3. 实现高频请求检测 - 防止恶意请求或循环调用
    /// 4. 自适应熔断阈值 - 根据系统负载动态调整熔断参数
    /// 5. 完善错误日志记录 - 提高可观测性和问题排查效率
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

        //当熔断器触发打开后，默认的恢复时间是1分钟。在这段时间内，熔断器会拒绝所有请求，直接返回503错误。1分钟后，
        //熔断器会进入半开状态，尝试处理少量请求以检测服务是否已恢复正常。
        private readonly IAsyncPolicy<IResponse> _defaultCircuitBreakerPolicy;
        private readonly CircuitBreakerPolicyManager _circuitBreakerPolicyManager;
        private readonly CircuitBreakerMetrics _metrics;
        private readonly IdempotencyFilter _idempotent = new IdempotencyFilter();
        private FallbackGenericCommandHandler _fallbackHandler;
        private readonly object _fallbackHandlerLock = new object();
        private readonly CommandHandlerRegistry _handlerRegistry;

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
        public int HandlerCount => _handlerRegistry?.HandlerCount ?? 0;

        /// <summary>
        /// 是否已初始化
        /// </summary>
        public bool IsInitialized => _isInitialized;

        /// <summary>
        /// 全局服务提供者
        /// 用于在命令处理过程中访问系统中注册的所有服务
        /// </summary>
        public IServiceProvider ServiceProvider
        {
            get => _serviceProvider;
            set
            {
                _serviceProvider = value;
                // 当设置服务提供者时，将其传递给命令处理器工厂
                if (_serviceProvider != null && _handlerFactory != null)
                {
                    //_handlerFactory.UpdateServiceProvider(_serviceProvider);
                }
            }
        }

        private IServiceProvider _serviceProvider;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="handlerFactory">处理器工厂</param>
        /// <param name="maxConcurrencyPerCommand">每个命令的最大并发数</param>
        /// <param name="circuitBreakerPolicy">熔断器策略，默认为6次失败后熔断，30秒后恢复</param>
        public CommandDispatcher(ILogger<CommandDispatcher> logger, CommandHandlerRegistry handlerRegistry, ICommandHandlerFactory handlerFactory = null,
              int maxConcurrencyPerCommand = 0,
            IAsyncPolicy<IResponse> circuitBreakerPolicy = null,
            CircuitBreakerPolicyManager circuitBreakerPolicyManager = null,
            CircuitBreakerMetrics metrics = null
            )
        {
            Logger = logger;
            _handlerFactory = handlerFactory;
            _commandHistory = new ConcurrentDictionary<CommandId, DateTime>();
            _dispatchSemaphore = new SemaphoreSlim(1, 1);
            // 使用传入的handlerRegistry或创建新实例
            // 注意：CommandHandlerRegistry构造函数需要handlerFactory作为第一个参数
            _handlerRegistry = handlerRegistry;
            MaxConcurrencyPerCommand = maxConcurrencyPerCommand > 0 ? maxConcurrencyPerCommand : Environment.ProcessorCount;

            // 使用传入的熔断器策略，如果未提供则使用默认策略
            // 熔断器策略实际上是按命令类别分组应用的，同一类别的命令共享一个熔断器，而不是所有命令或单个命令ID独立使用一个熔断器。这种设计既能有效保护系统，又避免了过度熔断的问题。
            // 熔断器的核心参数
            /*
            - 触发条件 ：当响应为空，或发生异常时，认为是系统故障
            - 失败阈值 ：连续10次失败后触发熔断
            - 熔断持续时间 ：熔断器打开后，持续1分钟
            - 错误码 ：熔断器打开时返回503错误
            */
            // 注意：不将业务层面的失败(IsSuccess=false)作为熔断触发条件，因为这是正常的业务逻辑
            // 熔断器应该只对系统级错误(如连接超时、服务器内部错误等)做出反应
            _defaultCircuitBreakerPolicy = circuitBreakerPolicy ?? Policy
                //.HandleResult<IResponse>(r => r != null && !r.IsSuccess)
                //.OrResult(r => r == null) // 处理空响应情况
                .Handle<Exception>()
                .OrResult<IResponse>(r => r == null) // 只处理空响应情况
                .CircuitBreakerAsync<IResponse>(
                    handledEventsAllowedBeforeBreaking: 10,
                    durationOfBreak: TimeSpan.FromMinutes(1),
                    onBreak: OnCircuitBreak,
                    onReset: OnCircuitReset,
                    onHalfOpen: OnCircuitHalfOpen);

            // 初始化差异化熔断器策略管理器
            _circuitBreakerPolicyManager = circuitBreakerPolicyManager ?? new CircuitBreakerPolicyManager();

            // 初始化熔断器指标监控
            _metrics = metrics ?? new CircuitBreakerMetrics();

            // 初始化自适应熔断管理器，传入熔断器指标实例
            _adaptiveCircuitBreakerManager = new AdaptiveCircuitBreakerManager(_metrics);

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
                // 如果没有指定程序集，则扫描当前执行程序集
                if (assemblies == null || assemblies.Length == 0)
                {
                    assemblies = new[] { Assembly.GetExecutingAssembly() };
                    Logger?.LogDebug("未指定程序集，使用当前执行程序集: {AssemblyName}", assemblies[0].FullName);
                }

                // 检查是否已经初始化
                if (_isInitialized)
                {
                    // 如果已经初始化且提供了新的程序集参数，扫描新程序集并添加新处理器
                    Logger?.LogDebug("命令调度器已初始化，现在将扫描新程序集并添加新的处理器: {AssemblyCount} 个程序集", assemblies.Length);
                }
                else
                {
                    // 首次初始化时记录信息
                    Logger?.LogDebug("首次初始化命令调度器，开始扫描并注册命令处理器: {AssemblyCount} 个程序集", assemblies.Length);
                }

                // 使用CommandHandlerRegistry进行扫描和注册处理器
                await _handlerRegistry.InitializeAsync(cancellationToken, assemblies);

                // 将命令处理器注册表设置到响应工厂，用于响应类型缓存
                ResponseFactory.SetDefaultCommandHandlerRegistry(_handlerRegistry);

                // 如果是首次初始化，设置初始化标志
                if (!_isInitialized)
                {
                    _isInitialized = true;
                    Logger?.LogDebug("命令调度器初始化成功，处理器数量: {HandlerCount}", HandlerCount);
                }
                else
                {
                    Logger?.LogDebug("新程序集扫描完成，更新后的处理器数量: {HandlerCount}", HandlerCount);
                }

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

                LogDebug($"命令调度器初始化完成，已注册 {HandlerCount} 个处理器");
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
        // ScanAndRegisterHandlersAsync方法已被CommandHandlerRegistry替代
        // 处理器的扫描、注册和缓存现在由CommandHandlerRegistry类统一管理
        /// <summary>
        /// 异步分发数据包
        /// </summary>
        /// <param name="packet">数据包对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        public async Task<IResponse> DispatchAsync(PacketModel packet, CancellationToken cancellationToken = default)
        {
            if (!_isInitialized)
            {
                LogError("命令调度器未初始化");
                return ResponseFactory.CreateSpecificErrorResponse(packet, errorMessage: "命令调度器未初始化");

            }

            // 确定优先级
            var channel = GetPriorityChannel(packet.PacketPriority);

            // 创建任务完成源
            var tcs = new TaskCompletionSource<IResponse>(TaskCreationOptions.RunContinuationsAsynchronously);

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
                return ResponseFactory.CreateSpecificErrorResponse(packet, errorMessage: "系统繁忙，请稍后重试");
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
        /// 自适应熔断管理器实例
        /// </summary>
        private readonly AdaptiveCircuitBreakerManager _adaptiveCircuitBreakerManager;

        /// <summary>
        /// 会话级熔断器指标字典 - 为每个会话ID维护独立的熔断器状态
        /// </summary>
        private readonly ConcurrentDictionary<string, CircuitBreakerMetrics> _sessionCircuitBreakers = new();

        /// <summary>
        /// 请求频率跟踪器字典 - 用于检测高频相同参数请求
        /// </summary>
        private readonly ConcurrentDictionary<string, RequestRateTracker> _requestRateTrackers = new();

        private async Task<IResponse> ProcessAsync(QueuedCommand cmd, CancellationToken ct)
        {
            if (cmd == null || cmd.Packet == null)
            {
                // 创建特定错误响应，并设置错误类型为系统错误
                var errorResponse = ResponseFactory.CreateSpecificErrorResponse(cmd?.Packet, errorMessage: "命令对象不能为空");
                if (errorResponse is ResponseBase errorResponseBase)
                {
                    errorResponseBase.ResponseErrorType = ResponseBase.ErrorType.SystemError;
                }
                return errorResponse;
            }

            // 创建链接的取消令牌，处理命令超时
            using (var linkedCts = CreateLinkedCancellationToken(ct, cmd.Packet.CommandId))
            {
                IResponse response = null;
                var startTime = DateTime.Now;
                var commandIdentifier = cmd.Packet.CommandId;
                string commandCategory = "Default";
                DateTime processingStartTime = DateTime.MinValue;

                try
                {
                    // 获取会话ID
                    var sessionId = cmd.Packet.ExecutionContext?.SessionId;

                    // 幂等性检查 - 基于命令标识符和请求参数生成唯一键
                    if (cmd.Packet.CommandId.FullCode != 0)
                    {
                        if (_idempotent.TryGetCached(cmd.Packet.Request, cmd.Packet.CommandId, out var cached))
                        {
                            return cached;
                        }
                    }

                    // 检查高频请求（基于会话ID和命令参数）
                    if (!string.IsNullOrEmpty(sessionId))
                    {
                        var cacheKey = _idempotent.GetType().GetMethod("GenerateCacheKey", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
                            .Invoke(_idempotent, new object[] { cmd.Packet.Request, cmd.Packet.CommandId }) as string;

                        if (!string.IsNullOrEmpty(cacheKey))
                        {
                            var tracker = _requestRateTrackers.GetOrAdd(cacheKey, _ => new RequestRateTracker());
                            if (tracker.IsFrequentRequest(sessionId))
                            {
                                // 记录高频请求警告
                                Logger.LogWarning("检测到来自会话 {SessionId} 的高频请求: {CommandId}", sessionId, cmd.Packet.CommandId);

                                // 获取或创建会话级熔断器
                                var sessionCircuitBreaker = _sessionCircuitBreakers.GetOrAdd(sessionId, _ => new CircuitBreakerMetrics());
                                sessionCircuitBreaker.RecordCircuitOpen(cmd.Packet.CommandId.ToString());

                                // 返回限流响应，设置为业务错误，避免触发全局熔断器
                                var rateLimitResponse = ResponseFactory.CreateSpecificErrorResponse(cmd.Packet.ExecutionContext,
                                    errorMessage: "请求过于频繁，请稍后再试");
                                rateLimitResponse.ResponseErrorType = ResponseBase.ErrorType.BusinessError;
                                rateLimitResponse.ErrorCode = 429; // 429 - Too Many Requests

                                return rateLimitResponse;
                            }
                        }
                    }

                    // 记录命令历史
                    _commandHistory.TryAdd(commandIdentifier, startTime);

                    // 获取适合当前命令的熔断器策略和分类
                    var policy = _circuitBreakerPolicyManager.GetPolicyForCommand(cmd.Packet) ?? _defaultCircuitBreakerPolicy;
                    var categoryObj = _circuitBreakerPolicyManager.Classifier?.GetCommandCategory(cmd.Packet);
                    commandCategory = categoryObj?.ToString() ?? "Default";

                    // 查找合适的处理器和命令类型
                    var handlers = FindHandlers(cmd);

                    if (handlers == null || !handlers.Any())
                    {
                        // 使用回退处理器
                        var fallbackHandler = GetFallbackHandler();
                        if (fallbackHandler != null)
                        {
                            try
                            {
                                processingStartTime = DateTime.UtcNow;
                                response = await ExecuteWithPolicy(cmd, fallbackHandler.HandleAsync, policy, linkedCts.Token, processingStartTime, false);
                            }
                            catch (Exception ex)
                            {
                                LogError($"回退处理器处理命令时发生异常: {cmd.Packet.CommandId.ToString()}", ex);
                                var fallbackErrorResponse = ResponseFactory.CreateSpecificErrorResponse(cmd.Packet.ExecutionContext, ex, errorMessage: "回退处理器异常");
                                if (fallbackErrorResponse is ResponseBase fallbackResponseBase)
                                {
                                    fallbackResponseBase.ResponseErrorType = ResponseBase.ErrorType.SystemError;
                                }
                                return fallbackErrorResponse;
                            }
                        }
                        else
                        {
                            // 无可用处理器时返回错误
                            return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, errorMessage: "无法找到合适的处理器处理命令");
                        }
                    }
                    else
                    {
                        // 选择最佳处理器
                        var bestHandler = SelectBestHandler(handlers, cmd.Packet.CommandId);
                        if (bestHandler == null)
                        {
                            return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, errorMessage: "无法选择合适的处理器处理命令");
                        }

                        processingStartTime = DateTime.UtcNow;
                        response = await ExecuteWithPolicy(cmd, bestHandler.HandleAsync, policy, linkedCts.Token, processingStartTime, true);
                    }

                    // 设置执行时间
                    if (response != null)
                    {
                        response.ExecutionTimeMs = (long)(DateTime.Now - startTime).TotalMilliseconds;
                    }

                    // 处理空响应
                    if (response == null)
                    {
                        // 创建错误响应
                        var nullResponse = ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, errorMessage: "处理器返回空结果");
                        nullResponse.ResponseErrorType = ResponseBase.ErrorType.SystemError;
                        nullResponse.ErrorCode = 500;
                        // 更新自适应熔断指标（空响应视为失败）
                        // 更新自适应熔断指标（视为失败）
                        // 记录熔断器打开事件到指标收集器
                        _adaptiveCircuitBreakerManager.RecordCircuitOpen(cmd.Packet.CommandId.ToString());
                        // 计算执行时间并传递给指标收集方法
                        long executionTimeMs = (long)(DateTime.Now - processingStartTime).TotalMilliseconds;
                        _adaptiveCircuitBreakerManager.UpdateMetricsWithCommandInfo(cmd.Packet.CommandId.ToString(), false, executionTimeMs);

                        return nullResponse;
                    }
                    return response;
                }
                finally
                {
                    // 记录命令执行指标（如果有开始时间）
                    if (processingStartTime != DateTime.MinValue && response != null)
                    {
                        // 正确区分业务错误和系统错误：只有系统错误才视为熔断器的失败
                        bool isSuccess = response.IsSuccess ||
                                       (response is ResponseBase responseBase && responseBase.ResponseErrorType != ResponseBase.ErrorType.SystemError);
                        _metrics.RecordCommandExecution(
                            (cmd.Packet != null && cmd.Packet.CommandId != null) ? cmd.Packet.CommandId.Name : "Unknown",
                            commandCategory,
                            isSuccess,
                            DateTime.UtcNow - processingStartTime);
                    }

                    // 缓存结果以实现幂等性 - 基于命令标识符和请求参数生成唯一键
                    if (cmd != null && cmd.Packet != null && cmd.Packet.CommandId != CommandId.Empty)
                    {
                        _idempotent.Cache(cmd.Packet, response);
                    }

                    // 清理历史记录
                    _ = Task.Run(() => CleanupCommandHistory());
                }
            }
        }

        /// <summary>
        /// 使用熔断器策略执行命令处理逻辑并处理异常
        /// </summary>
        private async Task<IResponse> ExecuteWithPolicy(QueuedCommand cmd, Func<QueuedCommand, CancellationToken, Task<IResponse>> handlerFunc,
            IAsyncPolicy<IResponse> policy, CancellationToken token, DateTime processingStartTime, bool withDetailedExceptions = false)
        {
            IResponse response = null;

            try
            {
                // 使用熔断器执行处理逻辑
                response = await policy.ExecuteAsync(() => handlerFunc(cmd, token));

                // 正确区分业务错误和系统错误：只有系统错误才视为熔断器的失败
                bool isSuccess = response != null &&
                                (response.IsSuccess ||
                                (response is ResponseBase responseBase && responseBase.ResponseErrorType != ResponseBase.ErrorType.SystemError));

                // 计算执行时间并更新自适应熔断指标
                long executionTimeMs = (long)(DateTime.Now - processingStartTime).TotalMilliseconds;
                _adaptiveCircuitBreakerManager.UpdateMetricsWithCommandInfo(cmd.Packet.CommandId.ToString(), isSuccess, executionTimeMs);
            }
            catch (Polly.CircuitBreaker.BrokenCircuitException ex)
            {
                if (withDetailedExceptions)
                {
                    // 熔断器已打开，记录详细信息并返回适当的错误
                    Logger.LogWarning("命令 {CommandId}[ID: {CommandFullCode}] 的熔断器已打开，拒绝执行: {ExceptionMessage}",
                        cmd.Packet.CommandId.ToString(), cmd.Packet.CommandId.FullCode, ex.Message);
                    var circuitBreakerResponse = ResponseFactory.CreateSpecificErrorResponse<ResponseBase>(errorMessage: "服务暂时不可用，熔断器已打开");
                    circuitBreakerResponse.ResponseErrorType = ResponseBase.ErrorType.SystemError;
                    circuitBreakerResponse.ErrorCode = 503; // Service Unavailable
                    // 计算执行时间并传递给指标收集方法
                    long executionTimeMs = (long)(DateTime.Now - processingStartTime).TotalMilliseconds;
                    _adaptiveCircuitBreakerManager.UpdateMetricsWithCommandInfo(cmd.Packet.CommandId.ToString(), false, executionTimeMs);
                    // 直接记录熔断器打开状态到指标中
                    _metrics.RecordCircuitOpen(cmd.Packet.CommandId.Name);
                    return circuitBreakerResponse;
                }
                throw;
            }
            catch (OperationCanceledException ex) when (ex.CancellationToken == token)
            {
                if (withDetailedExceptions)
                {
                    // 区分超时/取消异常与外部取消请求
                    Logger.LogInformation("命令处理超时:{CommandId}", cmd.Packet.CommandId.ToString());

                    var timeoutResponse = ResponseFactory.CreateSpecificErrorResponse(cmd.Packet.ExecutionContext, ex, errorMessage: "命令处理超时");
                    timeoutResponse.ResponseErrorType = ResponseBase.ErrorType.SystemError;
                    timeoutResponse.ErrorCode = 504; // Gateway Timeout

                    // 计算执行时间并传递给指标收集方法
                    long executionTimeMs = (long)(DateTime.Now - processingStartTime).TotalMilliseconds;
                    _adaptiveCircuitBreakerManager.UpdateMetricsWithCommandInfo(cmd.Packet.CommandId.ToString(), false, executionTimeMs);
                    return timeoutResponse;
                }
                throw;
            }
            catch (OperationCanceledException ex)
            {
                if (withDetailedExceptions)
                {
                    // 外部取消请求
                    Logger.LogInformation("命令处理被外部取消: {CommandId}", cmd.Packet.CommandId.ToString());

                    var canceledResponse = ResponseFactory.CreateSpecificErrorResponse(cmd.Packet.ExecutionContext, ex, errorMessage: "命令处理被取消");
                    canceledResponse.ResponseErrorType = ResponseBase.ErrorType.BusinessError; // 取消请求视为业务错误，不触发熔断

                    return canceledResponse;
                }
                throw;
            }
            catch (Exception ex)
            {
                // 记录异常
                Logger.LogError(ex, "处理器处理命令时发生系统异常: {CommandId}", cmd.Packet.CommandId.ToString());

                if (withDetailedExceptions)
                {
                    var errorResponse = ResponseFactory.CreateSpecificErrorResponse(cmd.Packet.ExecutionContext, ex, errorMessage: "命令分发异常");
                    errorResponse.ResponseErrorType = ResponseBase.ErrorType.SystemError;

                    // 计算执行时间并传递给指标收集方法
                    long executionTimeMs = (long)(DateTime.Now - processingStartTime).TotalMilliseconds;
                    _adaptiveCircuitBreakerManager.UpdateMetricsWithCommandInfo(cmd.Packet.CommandId.ToString(), false, executionTimeMs);
                    return errorResponse;
                }
                throw;
            }

            return response;
        }



        /// <summary>
        /// 获取所有处理器
        /// </summary>
        /// <returns>处理器列表</returns>
        public IReadOnlyList<ICommandHandler> GetAllHandlers()
        {
            return _handlerRegistry?.GetAllHandlers().ToList().AsReadOnly() ?? new List<ICommandHandler>().AsReadOnly();
        }

        /// <summary>
        /// 获取处理器统计信息
        /// </summary>
        /// <returns>统计信息字典</returns>
        public Dictionary<string, HandlerStatistics> GetHandlerStatistics()
        {
            var handlers = _handlerRegistry?.GetAllHandlers();
            if (handlers != null)
            {
                return handlers
                    .Distinct()
                    .ToDictionary(
                        handler => handler.HandlerId,
                        handler => handler.GetStatistics()
                    );
            }
            return new Dictionary<string, HandlerStatistics>();
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
                // 使用CommandHandlerRegistry查找处理器
                var handlers = _handlerRegistry.GetHandlersByCommandId(commandCode);
                if (handlers != null && handlers.Any())
                {
                    // 过滤出能处理当前命令的处理器
                    var compatibleHandlers = handlers
                        .Where(h => h.CanHandle(cmd))
                        .ToList();

                    if (compatibleHandlers.Any())
                    {
                        return compatibleHandlers;
                    }
                }

                // 如果没有找到特定命令ID的处理器，尝试查找所有兼容的处理器
                var allCompatibleHandlers = _handlerRegistry.GetAllHandlers()
                    .Where(h => h.CanHandle(cmd))
                    .ToList();

                if (allCompatibleHandlers.Any())
                {
                    return allCompatibleHandlers;
                }

                // 3. 尝试创建动态处理器（保持原有逻辑）
                var dynamicHandler = TryCreateDynamicHandler(commandType, commandCode);
                if (dynamicHandler != null)
                {
                    // 将动态创建的处理器也添加到registry中，以便后续复用
                    _handlerRegistry.RegisterHandler(dynamicHandler);
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
            // 使用处理器注册表获取处理器
            var handlers = _handlerRegistry?.GetHandlersByCommandId(commandId);
            if (handlers != null && handlers.Any())
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
        /// 熔断器打开时的回调
        /// </summary>
        private void OnCircuitBreak(Polly.DelegateResult<IResponse> result, TimeSpan breakDuration)
        {
            string message = result?.Exception?.Message ?? "unknown reason";
            LogWarning($"Circuit broken for {breakDuration.TotalSeconds} seconds due to {message}");

            // 记录熔断器状态变化到自适应熔断管理器
            _adaptiveCircuitBreakerManager.RecordCircuitOpen("Global");

            // 直接调用RecordCircuitOpen方法确保状态变化被正确记录
            _metrics.RecordCircuitOpen("Global");
            
            // 同时记录详细的状态变化信息
            _metrics.RecordCircuitStateChange("Break", breakDuration, message);
        }

        /// <summary>
        /// 熔断器重置时的回调
        /// </summary>
        private void OnCircuitReset()
        {
            LogInfo("Circuit reset to closed state");

            // 记录熔断器状态变化到自适应熔断管理器
            _adaptiveCircuitBreakerManager.RecordCircuitClosed();

            // 直接调用RecordCircuitClosed方法确保状态变化被正确记录
            _metrics.RecordCircuitClosed();
            
            // 同时记录详细的状态变化信息
            _metrics.RecordCircuitStateChange("Reset", TimeSpan.Zero, "Circuit reset");
        }

        /// <summary>
        /// 熔断器半开时的回调
        /// </summary>
        private void OnCircuitHalfOpen()
        {
            LogInfo("Circuit transitioned to half-open state");

            // 记录熔断器状态变化到自适应熔断管理器
            _adaptiveCircuitBreakerManager.RecordCircuitHalfOpen();

            // 直接调用RecordCircuitHalfOpen方法确保状态变化被正确记录
            _metrics.RecordCircuitHalfOpen();
            
            // 同时记录详细的状态变化信息
            _metrics.RecordCircuitStateChange("HalfOpen", TimeSpan.Zero, "Circuit half-open");
        }

        /// <summary>
        /// 获取熔断器指标
        /// </summary>
        public CircuitBreakerMetrics Metrics => _metrics;

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
                // 使用处理器注册表清理所有处理器
                _handlerRegistry?.Clear();
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

                    // 等待所有处理任务完成，过滤掉可能的null任务
                    var validChannelProcessors = _channelProcessors.Where(p => p != null).ToList();
                    if (validChannelProcessors.Any())
                    {
                        Task.WhenAll(validChannelProcessors).GetAwaiter().GetResult();
                    }

                    // 使用处理器注册表获取所有处理器
                    List<ICommandHandler> allHandlers = _handlerRegistry?.GetAllHandlers().ToList() ?? new List<ICommandHandler>();

                    // 停止所有处理器，过滤掉可能的null任务
                    var stopTasks = allHandlers.Select(h => h.StopAsync(CancellationToken.None));
                    var validStopTasks = stopTasks.Where(t => t != null).ToList();
                    if (validStopTasks.Any())
                    {
                        Task.WhenAll(validStopTasks).GetAwaiter().GetResult();
                    }

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

