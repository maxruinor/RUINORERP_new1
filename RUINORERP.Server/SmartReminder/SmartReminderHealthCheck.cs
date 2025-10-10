
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using RUINORERP.Global.EnumExt;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.SmartReminder
{
    //public class HealthCheckResult
    //{
    //    private HealthStatus HealthStatus;
    //    private string description;
    //    private System.Exception exception;
    //    public HealthCheckResult(HealthStatus healthy, string description, System.Exception exception=null)
    //    {
    //        this.HealthStatus = healthy;
    //        this.description = description;
    //        this.exception = exception;
    //    }
    //}

    // File: Services/SmartReminderHealthCheck.cs
    public class SmartReminderHealthCheck : IHealthCheck
    {
        private readonly ISmartReminderMonitor _monitor;
        private readonly ILogger _logger;

        public SmartReminderHealthCheck(
            ISmartReminderMonitor monitor,
            ILogger<SmartReminderHealthCheck> logger)
        {
            _monitor = monitor;
            _logger = logger;
        }

        public  Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var status = _monitor.IsRunning ?
                    HealthStatus.Healthy :
                    HealthStatus.Unhealthy;

                //var redisHealth = await CheckRedisConnection();
                //var dbHealth = await CheckDatabaseConnection();

                var data = new Dictionary<string, object>
                {
                    //["Redis"] = redisHealth,
                    //["Database"] = dbHealth,
                    //["ActiveWorkflows"] = _workflowHost.GetActiveWorkflows().Count,
                    ["LastCheck"] = DateTime.Now,
                    ["ActiveRules"] = _monitor.GetActiveRulesAsync()
                };

                return Task.FromResult(new HealthCheckResult(
                    status,
                    description: "智能提醒系统运行状态",
                    data: data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "健康检查失败");
                return Task.FromResult(HealthCheckResult.Unhealthy(ex.Message));
            }
        }
    }

}