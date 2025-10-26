
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.SmartReminder
{
    /// <summary>
    /// 智能提醒系统的健康检查实现
    /// </summary>
    public class SmartReminderHealthCheck : IHealthCheck
    {
        private readonly ISmartReminderMonitor _monitor;
        private readonly ILogger<SmartReminderHealthCheck> _logger;
        private DateTime _lastCheckTime;
        private int _successCheckCount;
        private int _failedCheckCount;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="monitor">智能提醒监控器</param>
        /// <param name="logger">日志记录器</param>
        public SmartReminderHealthCheck(
            ISmartReminderMonitor monitor,
            ILogger<SmartReminderHealthCheck> logger)
        {
            _monitor = monitor;
            _logger = logger;
        }

        /// <summary>
        /// 执行完整的健康检查
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>健康检查结果</returns>
        public async Task<HealthCheckResult> CheckHealthAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _lastCheckTime = DateTime.Now;
                var status = _monitor.IsRunning ?
                    HealthStatus.Healthy :
                    HealthStatus.Unhealthy;

                // 获取活跃规则数量
                var activeRules = await _monitor.GetActiveRulesAsync();

                var result = new HealthCheckResult
                {
                    Status = status,
                    Description = "智能提醒系统运行状态",
                    CheckTime = _lastCheckTime,
                    Data = new Dictionary<string, object>
                    {
                        ["LastCheck"] = _lastCheckTime,
                        ["ActiveRulesCount"] = activeRules.Count,
                        ["SuccessCheckCount"] = _successCheckCount,
                        ["FailedCheckCount"] = _failedCheckCount
                    }
                };

                // 更新统计信息
                if (status == HealthStatus.Healthy)
                {
                    _successCheckCount++;
                }
                else
                {
                    _failedCheckCount++;
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "健康检查失败");
                _failedCheckCount++;
                
                return new HealthCheckResult
                {
                    Status = HealthStatus.Unhealthy,
                    Description = "智能提醒系统健康检查失败",
                    CheckTime = DateTime.Now,
                    ErrorMessage = ex.Message,
                    Data = new Dictionary<string, object>
                    {
                        ["ExceptionType"] = ex.GetType().Name,
                        ["ExceptionMessage"] = ex.Message
                    }
                };
            }
        }

        /// <summary>
        /// 执行快速健康检查
        /// </summary>
        /// <returns>系统是否健康</returns>
        public bool PerformQuickCheck()
        {
            try
            {
                return _monitor.PerformQuickCheck();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "快速健康检查失败");
                return false;
            }
        }

        /// <summary>
        /// 获取组件健康状态
        /// </summary>
        /// <returns>组件健康状态字典</returns>
        public Dictionary<string, bool> GetComponentHealthStatus()
        {
            var statusDict = new Dictionary<string, bool>
            {
                ["SmartReminderMonitor"] = _monitor.IsRunning,
                ["LastHealthCheck"] = _lastCheckTime > DateTime.Now.AddMinutes(-5)
            };

            return statusDict;
        }

        /// <summary>
        /// 获取系统性能指标
        /// </summary>
        /// <returns>性能指标字典</returns>
        public Dictionary<string, object> GetPerformanceMetrics()
        {
            var metrics = new Dictionary<string, object>
            {
                ["LastCheckTime"] = _lastCheckTime,
                ["SuccessCheckCount"] = _successCheckCount,
                ["FailedCheckCount"] = _failedCheckCount,
                ["SuccessRate"] = _successCheckCount + _failedCheckCount > 0 
                    ? Math.Round((double)_successCheckCount / (_successCheckCount + _failedCheckCount) * 100, 2)
                    : 0
            };

            return metrics;
        }
    }

}