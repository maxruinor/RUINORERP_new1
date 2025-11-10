using System;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.PacketSpec.Commands
{

    /// <summary>
    /// 自适应熔断阈值管理器
    /// 根据系统负载和请求量动态调整熔断参数
    /// </summary>
    public class AdaptiveCircuitBreakerManager
    {
        private readonly int _baseFailureThreshold = 10; // 基础失败阈值
        private readonly TimeSpan _baseResetTime = TimeSpan.FromMinutes(1); // 基础重置时间
        private readonly object _lock = new();
        private int _currentRequestRate = 0;
        private int _failedRequests = 0;
        private int _successRequests = 0;
        private DateTime _lastMetricsUpdate = DateTime.Now;
        
        // 关联的熔断器指标收集器
        private readonly CircuitBreakerMetrics _circuitBreakerMetrics;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="circuitBreakerMetrics">熔断器指标收集器</param>
        public AdaptiveCircuitBreakerManager(CircuitBreakerMetrics circuitBreakerMetrics = null)
        {
            _circuitBreakerMetrics = circuitBreakerMetrics ?? new CircuitBreakerMetrics();
        }

        /// <summary>
        /// 获取关联的熔断器指标收集器
        /// </summary>
        public CircuitBreakerMetrics CircuitBreakerMetrics => _circuitBreakerMetrics;

        /// <summary>
        /// 更新请求指标
        /// </summary>
        /// <param name="isSuccess">请求是否成功</param>
        public void UpdateMetrics(bool isSuccess)
        {
            lock (_lock)
            {
                _currentRequestRate++;
                if (isSuccess)
                    _successRequests++;
                else
                    _failedRequests++;

                // 每分钟重置一次指标
                if (DateTime.Now - _lastMetricsUpdate > TimeSpan.FromMinutes(1))
                {
                    ResetMetrics();
                }
            }
        }

        /// <summary>
        /// 获取当前的失败阈值（根据系统负载自适应调整）
        /// </summary>
        /// <returns>调整后的失败阈值</returns>
        public int GetCurrentFailureThreshold()
        {
            lock (_lock)
            {
                // 基于请求率的自适应调整
                // 高负载时降低阈值，低负载时提高阈值
                if (_currentRequestRate > 1000) // 高负载
                    return Math.Max(5, _baseFailureThreshold / 2);
                else if (_currentRequestRate < 100) // 低负载
                    return _baseFailureThreshold * 2;
                return _baseFailureThreshold;
            }
        }

        /// <summary>
        /// 获取当前的重置时间（根据成功率自适应调整）
        /// </summary>
        /// <returns>调整后的重置时间</returns>
        public TimeSpan GetCurrentResetTime()
        {
            lock (_lock)
            {
                int totalRequests = _successRequests + _failedRequests;
                if (totalRequests == 0)
                    return _baseResetTime;

                double successRate = (double)_successRequests / totalRequests;

                // 基于成功率的自适应调整
                // 成功率低时延长重置时间，成功率高时缩短重置时间
                if (successRate < 0.5) // 成功率低
                    return TimeSpan.FromMilliseconds(_baseResetTime.TotalMilliseconds * 2);
                else if (successRate > 0.9) // 成功率高
                    return TimeSpan.FromMilliseconds(_baseResetTime.TotalMilliseconds / 2);
                return _baseResetTime;
            }
        }

        /// <summary>
        /// 重置指标
        /// </summary>
        private void ResetMetrics()
        {
            _currentRequestRate = 0;
            _successRequests = 0;
            _failedRequests = 0;
            _lastMetricsUpdate = DateTime.Now;
        }
        
        /// <summary>
        /// 更新请求指标并记录到熔断器指标收集器
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="isSuccess">请求是否成功</param>
        /// <param name="executionTimeMs">执行时间（毫秒）</param>
        public void UpdateMetricsWithCommandInfo(string commandId, bool isSuccess, long executionTimeMs = 0)
        {
            // 更新内部指标
            UpdateMetrics(isSuccess);
            
            // 如果提供了熔断器指标收集器，同步更新
            if (_circuitBreakerMetrics != null)
            {
                _circuitBreakerMetrics.RecordExecutionComplete(commandId, isSuccess, executionTimeMs);
            }
        }
        
        /// <summary>
        /// 记录熔断器打开事件
        /// </summary>
        /// <param name="commandId">命令ID</param>
        public void RecordCircuitOpen(string commandId)
        {
            if (_circuitBreakerMetrics != null)
            {
                _circuitBreakerMetrics.RecordCircuitOpen(commandId);
            }
        }
        
        /// <summary>
        /// 记录熔断器关闭事件
        /// </summary>
        public void RecordCircuitClosed()
        {
            if (_circuitBreakerMetrics != null)
            {
                _circuitBreakerMetrics.RecordCircuitClosed();
            }
        }
        
        /// <summary>
        /// 记录熔断器半开事件
        /// </summary>
        public void RecordCircuitHalfOpen()
        {
            if (_circuitBreakerMetrics != null)
            {
                _circuitBreakerMetrics.RecordCircuitHalfOpen();
            }
        }
    }
}
