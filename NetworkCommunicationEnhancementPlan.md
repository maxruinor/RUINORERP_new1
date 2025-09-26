# RUINORERP 网络通信功能完善方案

## 一、概述

本方案旨在全面完善RUINORERP项目的网络通信功能，包括监控诊断、状态管理、响应超时统计、错误处理机制、请求重试逻辑以及API封装。通过这些改进，将提升系统的稳定性、可维护性和用户体验。

## 二、现状分析

### 现有架构组件

1. **命令处理系统**
   - `ICommand` 和 `ICommandHandler` 接口定义了命令和处理器模式
   - `CommandDispatcher` 负责命令分发和处理
   - `BaseCommandHandler` 提供了处理器基类实现

2. **连接管理**
   - `HeartbeatManager` 实现心跳检测和连接健康监控
   - `CommunicationManager` 管理连接状态和事件分发
   - `ClientCommunicationService` 提供客户端通信服务

3. **请求响应机制**
   - `RequestResponseManager` 管理请求-响应匹配和超时处理
   - `ApiResponse<T>` 提供统一的响应格式

4. **现有问题**
   - `BaseCommandHandler` 中的性能监控功能框架存在但未启用
   - 状态管理不够完善，缺少清晰的状态转换逻辑
   - 缺少统一的请求重试机制
   - 错误处理可以更精细化
   - API使用体验可以进一步优化

## 三、功能完善计划

### 1. 监控诊断系统

**目标**：建立全面的网络通信监控体系，收集关键性能指标和诊断信息。

**实现方案**：

```csharp
// 1. 完善HandlerStatistics类，增强性能数据收集
public class HandlerStatistics
{
    // 基本统计信息
    public int TotalRequests { get; set; } // 总请求数
    public int SuccessfulRequests { get; set; } // 成功请求数
    public int FailedRequests { get; set; } // 失败请求数
    public int TimeoutRequests { get; set; } // 超时请求数
    
    // 性能指标
    public long TotalProcessingTime { get; set; } // 总处理时间
    public long MinProcessingTime { get; set; } // 最小处理时间
    public long MaxProcessingTime { get; set; } // 最大处理时间
    public long AverageProcessingTime { get; set; } // 平均处理时间
    
    // 最近处理记录（用于诊断）
    public ConcurrentQueue<ProcessingRecord> RecentRecords { get; set; } = new ConcurrentQueue<ProcessingRecord>();
    
    // 状态信息
    public DateTime StartTime { get; set; } // 启动时间
    public DateTime LastRequestTime { get; set; } // 最后请求时间
    
    // 更新统计信息的方法
    public void UpdateStatistics(long processingTime, bool success, string commandId = null)
    {
        // 实现统计信息更新逻辑
    }
}

// 2. 为BaseCommandHandler添加性能监控实现
public abstract class BaseCommandHandler : ICommandHandler
{
    // 已有的字段
    protected HandlerStatistics _statistics = new HandlerStatistics();
    
    // 重写HandleAsync方法，添加性能监控
    public virtual async Task<ApiResponse> HandleAsync(ICommand command, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        bool isSuccess = false;
        
        try
        {
            // 现有的HandleAsync实现逻辑
            // ...
            isSuccess = true;
            return result;
        }
        catch (Exception ex)
        {
            // 异常处理
            isSuccess = false;
            throw;
        }
        finally
        {
            stopwatch.Stop();
            // 添加性能监控数据收集
            _statistics.UpdateStatistics(stopwatch.ElapsedMilliseconds, isSuccess, command.CommandId);
        }
    }
}

// 3. 新增NetworkMonitor类，集中管理网络监控
public class NetworkMonitor : IDisposable
{
    // 连接统计
    public int ActiveConnections { get; private set; }
    public int TotalConnections { get; private set; }
    public int DroppedConnections { get; private set; }
    
    // 吞吐量统计
    public int TotalPacketsSent { get; private set; }
    public int TotalPacketsReceived { get; private set; }
    public long TotalBytesSent { get; private set; }
    public long TotalBytesReceived { get; private set; }
    
    // 错误统计
    public Dictionary<string, int> ErrorCounts { get; private set; } = new Dictionary<string, int>();
    
    // 方法：记录连接状态变化
    public void RecordConnectionState(bool isConnected)
    {
        // 实现连接状态记录逻辑
    }
    
    // 方法：记录数据包传输
    public void RecordPacketTransfer(bool isSend, int packetSize)
    {
        // 实现数据包传输记录逻辑
    }
    
    // 方法：记录错误
    public void RecordError(string errorType)
    {
        // 实现错误记录逻辑
    }
    
    // 方法：生成监控报告
    public NetworkMonitorReport GenerateReport()
    {
        // 实现报告生成逻辑
    }
}
```

### 2. 状态管理系统

**目标**：完善状态机实现，确保组件状态的一致性和可追踪性。

**实现方案**：

```csharp
// 1. 增强HandlerStatus枚举和状态转换逻辑
public enum HandlerStatus
{
    Created = 0,      // 创建
    Initializing = 1, // 初始化中
    Ready = 2,        // 就绪
    Running = 3,      // 运行中
    Paused = 4,       // 暂停
    Stopping = 5,     // 停止中
    Stopped = 6,      // 已停止
    Error = 7         // 错误
}

// 2. 新增状态转换管理类
public class StateTransitionManager<TState, TEvent>
    where TState : struct, Enum
    where TEvent : struct, Enum
{
    // 状态转换表
    private readonly Dictionary<(TState CurrentState, TEvent Event), TState> _transitions = new();
    
    // 当前状态
    public TState CurrentState { get; private set; }
    
    // 构造函数
    public StateTransitionManager(TState initialState)
    {
        CurrentState = initialState;
    }
    
    // 配置状态转换
    public void ConfigureTransition(TState fromState, TEvent triggerEvent, TState toState)
    {
        _transitions[(fromState, triggerEvent)] = toState;
    }
    
    // 触发状态转换
    public bool Transition(TEvent triggerEvent, out TState previousState)
    {
        previousState = CurrentState;
        
        if (_transitions.TryGetValue((CurrentState, triggerEvent), out var newState))
        {
            CurrentState = newState;
            return true;
        }
        
        return false;
    }
}

// 3. 在BaseCommandHandler中集成状态管理
public abstract class BaseCommandHandler : ICommandHandler
{
    // 状态管理
    private readonly StateTransitionManager<HandlerStatus, HandlerEvent> _stateManager;
    
    // 状态事件
    public event EventHandler<StateChangedEventArgs> StateChanged;
    
    // 属性：当前状态
    public HandlerStatus Status => _stateManager.CurrentState;
    
    // 构造函数
    protected BaseCommandHandler()
    {
        _stateManager = new StateTransitionManager<HandlerStatus, HandlerEvent>(HandlerStatus.Created);
        ConfigureStateTransitions();
    }
    
    // 配置状态转换
    private void ConfigureStateTransitions()
    {
        _stateManager.ConfigureTransition(HandlerStatus.Created, HandlerEvent.Initialize, HandlerStatus.Initializing);
        _stateManager.ConfigureTransition(HandlerStatus.Initializing, HandlerEvent.InitializeSuccess, HandlerStatus.Ready);
        _stateManager.ConfigureTransition(HandlerStatus.Initializing, HandlerEvent.InitializeFailed, HandlerStatus.Error);
        _stateManager.ConfigureTransition(HandlerStatus.Ready, HandlerEvent.Start, HandlerStatus.Running);
        _stateManager.ConfigureTransition(HandlerStatus.Running, HandlerEvent.Stop, HandlerStatus.Stopping);
        // ... 其他状态转换配置
    }
    
    // 触发状态转换的方法示例
    public virtual async Task InitializeAsync(CancellationToken cancellationToken)
    {
        if (!_stateManager.Transition(HandlerEvent.Initialize, out var previousState))
        {
            throw new InvalidOperationException($"Cannot initialize from state: {previousState}");
        }
        
        try
        {
            // 初始化逻辑
            await OnInitializeAsync(cancellationToken);
            
            _stateManager.Transition(HandlerEvent.InitializeSuccess, out _);
            OnStateChanged(HandlerStatus.Ready);
        }
        catch (Exception ex)
        {
            _stateManager.Transition(HandlerEvent.InitializeFailed, out _);
            OnStateChanged(HandlerStatus.Error);
            throw;
        }
    }
}
```

### 3. 响应超时统计

**目标**：实现详细的超时统计功能，帮助识别性能瓶颈。

**实现方案**：

```csharp
// 1. 增强RequestResponseManager的超时统计功能
public class RequestResponseManager : IDisposable
{
    // 超时统计信息
    public TimeoutStatistics TimeoutStats { get; } = new TimeoutStatistics();
    
    // 命令类型超时配置
    private readonly Dictionary<string, int> _commandTimeoutSettings = new();
    
    // 发送请求并等待响应的方法增强
    public async Task<TResponse> SendRequestAsync<TRequest, TResponse>(
        ISocketClient socketClient,
        CommandId commandId,
        TRequest requestData,
        CancellationToken ct = default,
        int? timeoutMs = null)
    {
        // 获取命令的超时配置
        int actualTimeout = timeoutMs ?? GetCommandTimeout(commandId.ToString()) ?? 30000;
        
        var startTime = DateTime.UtcNow;
        var requestId = Guid.NewGuid().ToString("N");
        
        try
        {
            // 现有发送请求逻辑
            // ...
            
            // 成功处理，记录处理时间
            var processingTime = (DateTime.UtcNow - startTime).TotalMilliseconds;
            TimeoutStats.RecordSuccess(commandId.ToString(), (long)processingTime);
            
            return response;
        }
        catch (TimeoutException)
        {
            // 记录超时
            TimeoutStats.RecordTimeout(commandId.ToString());
            throw;
        }
    }
    
    // 获取命令的超时配置
    private int? GetCommandTimeout(string commandId)
    {
        if (_commandTimeoutSettings.TryGetValue(commandId, out var timeout))
        {
            return timeout;
        }
        return null;
    }
}

// 2. 新增TimeoutStatistics类，用于统计超时信息
public class TimeoutStatistics
{
    // 总超时次数
    public int TotalTimeouts { get; private set; }
    
    // 命令类型超时统计
    public Dictionary<string, CommandTimeoutStats> CommandTimeoutDetails { get; } = new Dictionary<string, CommandTimeoutStats>();
    
    // 超时趋势数据（最近N分钟）
    public Queue<TimeoutTrendData> TimeoutTrends { get; } = new Queue<TimeoutTrendData>();
    
    // 记录成功请求
    public void RecordSuccess(string commandId, long processingTime)
    {
        // 实现成功请求记录逻辑
    }
    
    // 记录超时请求
    public void RecordTimeout(string commandId)
    {
        TotalTimeouts++;
        
        if (!CommandTimeoutDetails.TryGetValue(commandId, out var stats))
        {
            stats = new CommandTimeoutStats();
            CommandTimeoutDetails[commandId] = stats;
        }
        
        stats.TimeoutCount++;
        
        // 更新趋势数据
        UpdateTrendData(commandId, true);
    }
    
    // 生成超时报告
    public TimeoutReport GenerateReport()
    {
        // 实现报告生成逻辑
    }
}

// 3. 命令超时统计详情
public class CommandTimeoutStats
{
    // 超时次数
    public int TimeoutCount { get; set; }
    
    // 总请求次数
    public int TotalRequests { get; set; }
    
    // 超时率
    public double TimeoutRate => TotalRequests > 0 ? (double)TimeoutCount / TotalRequests * 100 : 0;
    
    // 平均处理时间（成功请求）
    public double AverageProcessingTime { get; set; }
    
    // 最长处理时间
    public long MaxProcessingTime { get; set; }
}
```

### 4. 完善错误处理机制

**目标**：建立更细粒度、更全面的错误处理和分类系统。

**实现方案**：

```csharp
// 1. 定义详细的错误类型枚举
public enum NetworkErrorType
{
    ConnectionError,      // 连接错误
    AuthenticationError,  // 认证错误
    AuthorizationError,   // 授权错误
    TimeoutError,         // 超时错误
    SerializationError,   // 序列化错误
    DeserializationError, // 反序列化错误
    CommandError,         // 命令错误
    ServerError,          // 服务器错误
    ClientError,          // 客户端错误
    UnknownError          // 未知错误
}

// 2. 增强ApiResponse，添加更丰富的错误处理功能
public static class ApiResponseExtensions
{
    // 添加错误类型
    public static ApiResponse<T> WithErrorType<T>(this ApiResponse<T> response, NetworkErrorType errorType)
    {
        response.Metadata["ErrorType"] = errorType;
        return response;
    }
    
    // 添加错误代码
    public static ApiResponse<T> WithErrorCode<T>(this ApiResponse<T> response, string errorCode)
    {
        response.Metadata["ErrorCode"] = errorCode;
        return response;
    }
    
    // 添加错误详情
    public static ApiResponse<T> WithErrorDetails<T>(this ApiResponse<T> response, string details)
    {
        response.Metadata["ErrorDetails"] = details;
        return response;
    }
    
    // 添加可重试标记
    public static ApiResponse<T> WithRetryable<T>(this ApiResponse<T> response, bool isRetryable)
    {
        response.Metadata["IsRetryable"] = isRetryable;
        return response;
    }
}

// 3. 新增错误处理策略工厂
public class ErrorHandlingStrategyFactory
{
    // 创建错误处理策略
    public static IErrorHandlingStrategy CreateStrategy(NetworkErrorType errorType)
    {
        switch (errorType)
        {
            case NetworkErrorType.ConnectionError:
                return new RetryableErrorStrategy(3, 1000); // 重试3次，间隔1秒
            case NetworkErrorType.TimeoutError:
                return new RetryableErrorStrategy(2, 2000); // 重试2次，间隔2秒
            case NetworkErrorType.AuthenticationError:
            case NetworkErrorType.AuthorizationError:
                return new NoRetryErrorStrategy(); // 不重试
            default:
                return new DefaultErrorStrategy(); // 默认策略
        }
    }
}

// 4. 错误处理策略接口
public interface IErrorHandlingStrategy
{
    bool ShouldRetry(int attemptCount);
    Task WaitBeforeRetryAsync(int attemptCount, CancellationToken cancellationToken);
    bool IsFatalError(Exception ex);
}

// 5. 在BaseCommand中集成错误处理策略
public abstract class BaseCommand : ICommand
{
    public virtual async Task<ApiResponse> ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            // 命令执行逻辑
            // ...
        }
        catch (Exception ex)
        {
            // 识别错误类型
            var errorType = IdentifyErrorType(ex);
            
            // 记录错误
            LogError($"执行命令 {GetType().Name} [ID: {CommandId}] 异常: {ex.Message}", ex);
            
            // 创建错误响应
            var errorResponse = ApiResponse.CreateError($"执行异常: {ex.Message}", 500)
                    .WithMetadata("ErrorCode", "PROCESS_ERROR")
                    .WithMetadata("ErrorType", errorType)
                    .WithMetadata("Exception", ex.Message)
                    .WithMetadata("StackTrace", ex.StackTrace);
            
            return errorResponse;
        }
    }
    
    // 识别错误类型
    protected virtual NetworkErrorType IdentifyErrorType(Exception ex)
    {
        if (ex is TimeoutException)
            return NetworkErrorType.TimeoutError;
        if (ex is AuthenticationException)
            return NetworkErrorType.AuthenticationError;
        if (ex is AuthorizationException)
            return NetworkErrorType.AuthorizationError;
        // ... 其他错误类型判断
        return NetworkErrorType.UnknownError;
    }
}

### 5. 请求重试逻辑

**目标**：实现智能请求重试机制，提高系统的鲁棒性和可靠性。

**实现方案**：

```csharp
// 1. 新增重试策略接口和实现
public interface IRetryStrategy
{
    /// <summary>
    /// 获取下一次重试的等待时间
    /// </summary>
    /// <param name="attempt">当前重试次数</param>
    /// <returns>等待时间（毫秒）</returns>
    int GetNextDelay(int attempt);
    
    /// <summary>
    /// 是否应该继续重试
    /// </summary>
    /// <param name="attempt">当前重试次数</param>
    /// <returns>是否继续重试</returns>
    bool ShouldContinue(int attempt);
}

// 2. 固定间隔重试策略
public class FixedIntervalRetryStrategy : IRetryStrategy
{
    private readonly int _maxAttempts;
    private readonly int _delayMs;
    
    public FixedIntervalRetryStrategy(int maxAttempts, int delayMs)
    {
        _maxAttempts = maxAttempts;
        _delayMs = delayMs;
    }
    
    public int GetNextDelay(int attempt)
    {
        return _delayMs;
    }
    
    public bool ShouldContinue(int attempt)
    {
        return attempt < _maxAttempts;
    }
}

// 3. 指数退避重试策略
public class ExponentialBackoffRetryStrategy : IRetryStrategy
{
    private readonly int _maxAttempts;
    private readonly int _initialDelayMs;
    private readonly double _multiplier;
    private readonly int _maxDelayMs;
    
    public ExponentialBackoffRetryStrategy(int maxAttempts, int initialDelayMs = 1000, double multiplier = 2.0, int maxDelayMs = 30000)
    {
        _maxAttempts = maxAttempts;
        _initialDelayMs = initialDelayMs;
        _multiplier = multiplier;
        _maxDelayMs = maxDelayMs;
    }
    
    public int GetNextDelay(int attempt)
    {
        // 计算指数退避延迟，并限制最大值
        double delay = _initialDelayMs * Math.Pow(_multiplier, attempt);
        return Math.Min((int)delay, _maxDelayMs);
    }
    
    public bool ShouldContinue(int attempt)
    {
        return attempt < _maxAttempts;
    }
}

// 4. 增强RequestResponseManager，添加重试功能
public class RequestResponseManager : IDisposable
{
    // 带重试逻辑的发送请求方法
    public async Task<TResponse> SendRequestWithRetryAsync<TRequest, TResponse>(
        ISocketClient socketClient,
        CommandId commandId,
        TRequest requestData,
        IRetryStrategy retryStrategy = null,
        CancellationToken ct = default,
        int timeoutMs = 30000)
    {
        // 默认使用指数退避策略
        retryStrategy ??= new ExponentialBackoffRetryStrategy(3, 1000, 2.0);
        
        int attempt = 0;
        Exception lastException = null;
        
        while (retryStrategy.ShouldContinue(attempt))
        {
            if (attempt > 0)
            {
                // 等待重试延迟
                var delay = retryStrategy.GetNextDelay(attempt - 1);
                _logger?.LogDebug("请求重试，命令ID: {commandId}, 尝试次数: {Attempt}, 等待延迟: {Delay}ms",
                    commandId, attempt, delay);
                
                try
                {
                    await Task.Delay(delay, ct);
                }
                catch (TaskCanceledException)
                {
                    // 任务被取消，向上抛出
                    throw;
                }
            }
            
            try
            {
                // 尝试发送请求
                return await SendRequestAsync<TRequest, TResponse>(
                    socketClient, commandId, requestData, ct, timeoutMs);
            }
            catch (Exception ex)
            {
                lastException = ex;
                
                // 只对特定类型的异常进行重试
                if (!IsRetryableException(ex))
                {
                    _logger?.LogDebug("不可重试的异常，命令ID: {CommandId}, 异常类型: {ExceptionType}",
                        commandId, ex.GetType().Name);
                    throw;
                }
                
                _logger?.LogWarning(ex, "请求失败，将重试，命令ID: {CommandId}, 尝试次数: {Attempt}",
                    commandId, attempt + 1);
            }
            
            attempt++;
        }
        
        // 达到最大重试次数，抛出最后一个异常
        _logger?.LogError(lastException, "请求重试失败，已达到最大重试次数，命令ID: {CommandId}", commandId);
        
        if (lastException is TimeoutException)
        {
            throw new TimeoutException($"请求超时，已重试{attempt}次");
        }