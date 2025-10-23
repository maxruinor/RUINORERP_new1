using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 熔断器指标收集器
    /// 用于收集和监控熔断器的详细运行指标
    /// </summary>
    public class CircuitBreakerMetrics
    {
        // 熔断器状态枚举
        public enum CircuitState
        { 
            Closed = 0,    // 关闭状态 - 正常工作
            Open = 1,      // 打开状态 - 拒绝请求
            HalfOpen = 2   // 半开状态 - 尝试恢复
        }

        // 命令ID到指标的映射
        private readonly ConcurrentDictionary<string, CommandMetrics> _commandMetrics;
        
        // 全局熔断器指标
        private readonly CircuitBreakerGlobalMetrics _globalMetrics;
        
        // 锁对象，用于线程安全
        private readonly object _stateLock = new object();
        
        // 当前熔断器状态
        private CircuitState _currentState;
        
        // 最后一次状态改变时间
        private DateTime _lastStateChangeTime;
        
        // 最后一次成功处理时间
        private DateTime _lastSuccessfulExecutionTime;

        /// <summary>
        /// 获取当前熔断器状态
        /// </summary>
        public CircuitState CurrentState
        { 
            get { lock (_stateLock) { return _currentState; } }
            set 
            { 
                lock (_stateLock) 
                { 
                    if (_currentState != value) 
                    { 
                        _currentState = value;
                        _lastStateChangeTime = DateTime.UtcNow;
                        
                        // 记录状态变化
                        _globalMetrics.IncrementStateChanges();
                    }
                } 
            }
        }

        /// <summary>
        /// 获取最后一次状态改变时间
        /// </summary>
        public DateTime LastStateChangeTime
        { 
            get { lock (_stateLock) { return _lastStateChangeTime; } }
        }

        /// <summary>
        /// 获取最后一次成功执行时间
        /// </summary>
        public DateTime LastSuccessfulExecutionTime
        { 
            get { lock (_stateLock) { return _lastSuccessfulExecutionTime; } }
            set { lock (_stateLock) { _lastSuccessfulExecutionTime = value; } }
        }

        /// <summary>
        /// 获取全局指标
        /// </summary>
        public CircuitBreakerGlobalMetrics GlobalMetrics => _globalMetrics;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CircuitBreakerMetrics()
        {
            _commandMetrics = new ConcurrentDictionary<string, CommandMetrics>();
            _globalMetrics = new CircuitBreakerGlobalMetrics();
            _currentState = CircuitState.Closed; // 初始状态为关闭
            _lastStateChangeTime = DateTime.UtcNow;
            _lastSuccessfulExecutionTime = DateTime.UtcNow;
        }

        /// <summary>
        /// 记录命令执行开始
        /// </summary>
        /// <param name="commandId">命令ID</param>
        public void RecordExecutionStart(string commandId)
        {
            var metrics = GetOrCreateCommandMetrics(commandId);
            metrics.IncrementActiveExecutions();
            _globalMetrics.IncrementActiveExecutions();
        }

        /// <summary>
        /// 记录命令执行完成
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="isSuccess">是否成功</param>
        /// <param name="executionTimeMs">执行时间（毫秒）</param>
        public void RecordExecutionComplete(string commandId, bool isSuccess, long executionTimeMs)
        {
            var metrics = GetOrCreateCommandMetrics(commandId);
            metrics.DecrementActiveExecutions();
            metrics.RecordExecution(isSuccess, executionTimeMs);
            
            _globalMetrics.DecrementActiveExecutions();
            _globalMetrics.RecordExecution(isSuccess, executionTimeMs);
            
            if (isSuccess)
            {
                LastSuccessfulExecutionTime = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// 记录熔断器触发打开
        /// </summary>
        /// <param name="commandId">命令ID</param>
        public void RecordCircuitOpen(string commandId)
        {
            var metrics = GetOrCreateCommandMetrics(commandId);
            metrics.IncrementCircuitOpens();
            _globalMetrics.IncrementCircuitOpens();
            CurrentState = CircuitState.Open;
        }

        /// <summary>
        /// 记录熔断器关闭
        /// </summary>
        public void RecordCircuitClosed()
        {
            _globalMetrics.IncrementCircuitCloses();
            CurrentState = CircuitState.Closed;
        }

        /// <summary>
        /// 记录熔断器进入半开状态
        /// </summary>
        public void RecordCircuitHalfOpen()
        {
            _globalMetrics.IncrementCircuitHalfOpens();
            CurrentState = CircuitState.HalfOpen;
        }

        /// <summary>
        /// 获取命令的指标
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <returns>命令指标，如果不存在则返回null</returns>
        public CommandMetrics GetCommandMetrics(string commandId)
        {
            _commandMetrics.TryGetValue(commandId, out var metrics);
            return metrics;
        }

        /// <summary>
        /// 获取所有命令的指标
        /// </summary>
        /// <returns>命令指标字典</returns>
        public IReadOnlyDictionary<string, CommandMetrics> GetAllCommandMetrics()
        {
            return _commandMetrics;
        }

        /// <summary>
        /// 获取或创建命令指标
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <returns>命令指标</returns>
        private CommandMetrics GetOrCreateCommandMetrics(string commandId)
        {
            return _commandMetrics.GetOrAdd(commandId, id => new CommandMetrics(id));
        }

        /// <summary>
        /// 重置所有指标
        /// </summary>
        public void ResetAllMetrics()
        {
            _commandMetrics.Clear();
            _globalMetrics.Reset();
        }

        /// <summary>
        /// 记录命令执行（与RecordExecutionComplete功能兼容）
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="category">命令分类</param>
        /// <param name="isSuccess">是否成功</param>
        /// <param name="executionTime">执行时间</param>
        public void RecordCommandExecution(string commandId, string category, bool isSuccess, TimeSpan executionTime)
        {
            // 转换TimeSpan为毫秒并调用现有方法
            long executionTimeMs = (long)executionTime.TotalMilliseconds;
            RecordExecutionComplete(commandId, isSuccess, executionTimeMs);
        }

        /// <summary>
        /// 记录熔断器状态变化
        /// </summary>
        /// <param name="stateChangeType">状态变化类型</param>
        /// <param name="duration">持续时间</param>
        /// <param name="reason">原因</param>
        public void RecordCircuitStateChange(string stateChangeType, TimeSpan duration, string reason)
        {
            // 根据状态变化类型调用相应的记录方法
            switch (stateChangeType)
            {
                case "Break":
                case "Open":
                    RecordCircuitOpen("Global"); // 使用"Global"作为全局状态变化的命令ID
                    break;
                case "Reset":
                case "Close":
                    RecordCircuitClosed();
                    break;
                case "HalfOpen":
                    RecordCircuitHalfOpen();
                    break;
            }
        }

        /// <summary>
        /// 获取熔断器状态描述
        /// </summary>
        /// <returns>状态描述字符串</returns>
        public string GetStatusDescription()
        {
            var state = CurrentState;
            var stateText = state switch
            {
                CircuitState.Closed => "关闭",
                CircuitState.Open => "打开",
                CircuitState.HalfOpen => "半开",
                _ => "未知"
            };

            return $"熔断器状态: {stateText}, 最后状态变化时间: {LastStateChangeTime}, " +
                   $"最后成功执行时间: {LastSuccessfulExecutionTime}";
        }
    }

    /// <summary>
    /// 全局熔断器指标
    /// </summary>
    public class CircuitBreakerGlobalMetrics
    {
        // 总请求数
        private long _totalRequests;
        
        // 成功请求数
        private long _successfulRequests;
        
        // 失败请求数
        private long _failedRequests;
        
        // 当前活跃执行数
        private int _activeExecutions;
        
        // 熔断器打开次数
        private long _circuitOpens;
        
        // 熔断器关闭次数
        private long _circuitCloses;
        
        // 熔断器半开次数
        private long _circuitHalfOpens;
        
        // 状态变化次数
        private long _stateChanges;
        
        // 总执行时间
        private long _totalExecutionTimeMs;
        
        // 锁对象
        private readonly object _lock = new object();

        /// <summary>
        /// 获取总请求数
        /// </summary>
        public long TotalRequests => _totalRequests;

        /// <summary>
        /// 获取成功请求数
        /// </summary>
        public long SuccessfulRequests => _successfulRequests;

        /// <summary>
        /// 获取失败请求数
        /// </summary>
        public long FailedRequests => _failedRequests;

        /// <summary>
        /// 获取当前活跃执行数
        /// </summary>
        public int ActiveExecutions => _activeExecutions;

        /// <summary>
        /// 获取熔断器打开次数
        /// </summary>
        public long CircuitOpens => _circuitOpens;

        /// <summary>
        /// 获取熔断器关闭次数
        /// </summary>
        public long CircuitCloses => _circuitCloses;

        /// <summary>
        /// 获取熔断器半开次数
        /// </summary>
        public long CircuitHalfOpens => _circuitHalfOpens;

        /// <summary>
        /// 获取状态变化次数
        /// </summary>
        public long StateChanges => _stateChanges;

        /// <summary>
        /// 获取平均执行时间（毫秒）
        /// </summary>
        public double AverageExecutionTimeMs
        {
            get
            {
                lock (_lock)
                {
                    return _totalRequests > 0 ? (double)_totalExecutionTimeMs / _totalRequests : 0;
                }
            }
        }

        /// <summary>
        /// 获取成功率
        /// </summary>
        public double SuccessRate
        {
            get
            {
                lock (_lock)
                {
                    return _totalRequests > 0 ? (double)_successfulRequests / _totalRequests * 100 : 0;
                }
            }
        }

        /// <summary>
        /// 增加活跃执行数
        /// </summary>
        public void IncrementActiveExecutions()
        { 
            lock (_lock) { _activeExecutions++; }
        }

        /// <summary>
        /// 减少活跃执行数
        /// </summary>
        public void DecrementActiveExecutions()
        { 
            lock (_lock) { if (_activeExecutions > 0) _activeExecutions--; }
        }

        /// <summary>
        /// 记录执行结果
        /// </summary>
        /// <param name="isSuccess">是否成功</param>
        /// <param name="executionTimeMs">执行时间（毫秒）</param>
        public void RecordExecution(bool isSuccess, long executionTimeMs)
        {
            lock (_lock)
            {
                _totalRequests++;
                _totalExecutionTimeMs += executionTimeMs;
                
                if (isSuccess)
                {
                    _successfulRequests++;
                }
                else
                {
                    _failedRequests++;
                }
            }
        }

        /// <summary>
        /// 增加熔断器打开次数
        /// </summary>
        public void IncrementCircuitOpens()
        { 
            lock (_lock) { _circuitOpens++; }
        }

        /// <summary>
        /// 增加熔断器关闭次数
        /// </summary>
        public void IncrementCircuitCloses()
        { 
            lock (_lock) { _circuitCloses++; }
        }

        /// <summary>
        /// 增加熔断器半开次数
        /// </summary>
        public void IncrementCircuitHalfOpens()
        { 
            lock (_lock) { _circuitHalfOpens++; }
        }

        /// <summary>
        /// 增加状态变化次数
        /// </summary>
        public void IncrementStateChanges()
        { 
            lock (_lock) { _stateChanges++; }
        }

        /// <summary>
        /// 重置所有指标
        /// </summary>
        public void Reset()
        {
            lock (_lock)
            {
                _totalRequests = 0;
                _successfulRequests = 0;
                _failedRequests = 0;
                _activeExecutions = 0;
                _circuitOpens = 0;
                _circuitCloses = 0;
                _circuitHalfOpens = 0;
                _stateChanges = 0;
                _totalExecutionTimeMs = 0;
            }
        }
    }

    /// <summary>
    /// 命令级别的熔断器指标
    /// </summary>
    public class CommandMetrics
    {
        // 命令ID
        private readonly string _commandId;
        
        // 总请求数
        private long _totalRequests;
        
        // 成功请求数
        private long _successfulRequests;
        
        // 失败请求数
        private long _failedRequests;
        
        // 当前活跃执行数
        private int _activeExecutions;
        
        // 熔断器打开次数
        private long _circuitOpens;
        
        // 总执行时间
        private long _totalExecutionTimeMs;
        
        // 最大执行时间
        private long _maxExecutionTimeMs;
        
        // 最小执行时间
        private long _minExecutionTimeMs;
        
        // 锁对象
        private readonly object _lock = new object();

        /// <summary>
        /// 获取命令ID
        /// </summary>
        public string CommandId => _commandId;

        /// <summary>
        /// 获取总请求数
        /// </summary>
        public long TotalRequests => _totalRequests;

        /// <summary>
        /// 获取成功请求数
        /// </summary>
        public long SuccessfulRequests => _successfulRequests;

        /// <summary>
        /// 获取失败请求数
        /// </summary>
        public long FailedRequests => _failedRequests;

        /// <summary>
        /// 获取当前活跃执行数
        /// </summary>
        public int ActiveExecutions => _activeExecutions;

        /// <summary>
        /// 获取熔断器打开次数
        /// </summary>
        public long CircuitOpens => _circuitOpens;

        /// <summary>
        /// 获取平均执行时间（毫秒）
        /// </summary>
        public double AverageExecutionTimeMs
        {
            get
            {
                lock (_lock)
                {
                    return _totalRequests > 0 ? (double)_totalExecutionTimeMs / _totalRequests : 0;
                }
            }
        }

        /// <summary>
        /// 获取最大执行时间（毫秒）
        /// </summary>
        public long MaxExecutionTimeMs => _maxExecutionTimeMs;

        /// <summary>
        /// 获取最小执行时间（毫秒）
        /// </summary>
        public long MinExecutionTimeMs => _minExecutionTimeMs;

        /// <summary>
        /// 获取成功率
        /// </summary>
        public double SuccessRate
        {
            get
            {
                lock (_lock)
                {
                    return _totalRequests > 0 ? (double)_successfulRequests / _totalRequests * 100 : 0;
                }
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commandId">命令ID</param>
        public CommandMetrics(string commandId)
        {
            _commandId = commandId;
            _minExecutionTimeMs = long.MaxValue;
        }

        /// <summary>
        /// 增加活跃执行数
        /// </summary>
        public void IncrementActiveExecutions()
        { 
            lock (_lock) { _activeExecutions++; }
        }

        /// <summary>
        /// 减少活跃执行数
        /// </summary>
        public void DecrementActiveExecutions()
        { 
            lock (_lock) { if (_activeExecutions > 0) _activeExecutions--; }
        }

        /// <summary>
        /// 记录执行结果
        /// </summary>
        /// <param name="isSuccess">是否成功</param>
        /// <param name="executionTimeMs">执行时间（毫秒）</param>
        public void RecordExecution(bool isSuccess, long executionTimeMs)
        {
            lock (_lock)
            {
                _totalRequests++;
                _totalExecutionTimeMs += executionTimeMs;
                
                if (executionTimeMs > _maxExecutionTimeMs)
                {
                    _maxExecutionTimeMs = executionTimeMs;
                }
                
                if (executionTimeMs < _minExecutionTimeMs)
                {
                    _minExecutionTimeMs = executionTimeMs;
                }
                
                if (isSuccess)
                {
                    _successfulRequests++;
                }
                else
                {
                    _failedRequests++;
                }
            }
        }

        /// <summary>
        /// 增加熔断器打开次数
        /// </summary>
        public void IncrementCircuitOpens()
        { 
            lock (_lock) { _circuitOpens++; }
        }

        /// <summary>
        /// 重置指标
        /// </summary>
        public void Reset()
        {
            lock (_lock)
            {
                _totalRequests = 0;
                _successfulRequests = 0;
                _failedRequests = 0;
                _activeExecutions = 0;
                _circuitOpens = 0;
                _totalExecutionTimeMs = 0;
                _maxExecutionTimeMs = 0;
                _minExecutionTimeMs = long.MaxValue;
            }
        }
    }
}