using Microsoft.Extensions.Logging;
using RUINORERP.Global.EnumExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.SmartReminder
{
    public class MonitorHealthCheck : IHealthCheck
    {
        private readonly ISmartReminderMonitor _monitor;
        private readonly ILogger<MonitorHealthCheck> _logger;

        public MonitorHealthCheck(ISmartReminderMonitor monitor, ILogger<MonitorHealthCheck> logger)
        {
            _monitor = monitor;
            _logger = logger;
        }

        public Task<HealthCheckResult> CheckHealthAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                // 如果监控器正在运行且能够成功执行检查
                if (_monitor.IsRunning)
                {
                    // 执行简单检查验证
                    var testResult = _monitor.PerformQuickCheck();
                    if (testResult)
                    {
                        return Task.FromResult(new HealthCheckResult
                        {
                            Status = HealthStatus.Healthy,
                            Description = "库存监控服务正常运行",
                            CheckTime = DateTime.Now
                        });
                    }
                    else
                    {
                        return Task.FromResult(new HealthCheckResult
                        {
                            Status = HealthStatus.Degraded,
                            Description = "库存监控服务部分功能不可用",
                            CheckTime = DateTime.Now
                        });
                    }
                }
                else
                {
                    return Task.FromResult(new HealthCheckResult
                    {
                        Status = HealthStatus.Unhealthy,
                        Description = "库存监控服务未运行",
                        CheckTime = DateTime.Now
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "库存监控服务健康检查出错");
                return Task.FromResult(new HealthCheckResult
                {
                    Status = HealthStatus.Unhealthy,
                    Description = "库存监控服务健康检查失败",
                    ErrorMessage = ex.Message,
                    CheckTime = DateTime.Now
                });
            }
        }

        public bool PerformQuickCheck()
        {
            try
            {
                return _monitor.IsRunning;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "执行快速健康检查失败");
                return false;
            }
        }

        public Dictionary<string, bool> GetComponentHealthStatus()
        {
            var components = new Dictionary<string, bool>();
            try
            {
                components["监控服务"] = _monitor.IsRunning;
                components["快速检查"] = _monitor.PerformQuickCheck();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取组件健康状态失败");
            }
            return components;
        }

        public Dictionary<string, object> GetPerformanceMetrics()
        {
            var metrics = new Dictionary<string, object>();
            try
            {
                metrics["监控服务状态"] = _monitor.IsRunning ? "运行中" : "未运行";
                metrics["检查时间"] = DateTime.Now;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取性能指标失败");
            }
            return metrics;
        }
    }



    // TODO 要如果调用修复？
    //public Task<HealthCheckResult> CheckHealthAsync(
    //    HealthCheckContext context,
    //    CancellationToken cancellationToken = default)
    //{
    //    var status = _monitor.IsRunning ?
    //        HealthStatus.Healthy :
    //        HealthStatus.Unhealthy;

    //    return Task.FromResult(new HealthCheckResult(
    //        status,
    //        description: "库存监控服务状态"));
    //}
}


// TODO 要如果调用修复？
// 注册健康检查
//services.AddHealthChecks()
//.AddCheck<MonitorHealthCheck>("inventory_monitor");
