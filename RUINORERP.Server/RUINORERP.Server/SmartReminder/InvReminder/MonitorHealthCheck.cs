using Microsoft.Extensions.Logging;
using RUINORERP.Global.EnumExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.SmartReminder.InvReminder
{
    public class MonitorHealthCheck : IHealthCheck
    {
        private readonly IInventoryMonitor _monitor;
        private readonly ILogger<MonitorHealthCheck> _logger;

        public MonitorHealthCheck(IInventoryMonitor monitor, ILogger<MonitorHealthCheck> logger)
        {
            _monitor = monitor;
            _logger = logger;
        }

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
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
                        return Task.FromResult(new HealthCheckResult(
                            HealthStatus.Healthy,
                            description: "库存监控服务正常运行"));
                    }
                    else
                    {
                        return Task.FromResult(new HealthCheckResult(
                            HealthStatus.Degraded,
                            description: "库存监控服务部分功能不可用"));
                    }
                }
                else
                {
                    return Task.FromResult(new HealthCheckResult(
                        HealthStatus.Unhealthy,
                        description: "库存监控服务未运行"));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "库存监控服务健康检查出错");
                return Task.FromResult(new HealthCheckResult(
                    HealthStatus.Unhealthy,
                    exception: ex,
                    description: "库存监控服务健康检查失败"));
            }
           
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
 