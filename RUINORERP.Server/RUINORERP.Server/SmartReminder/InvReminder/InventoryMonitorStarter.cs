using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RUINORERP.Model.Context;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Server.SmartReminder.ReminderRuleStrategy;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.SmartReminder.InvReminder
{
    // 后台服务启动器
    public class InventoryMonitorStarter : BackgroundService
    {
        private readonly IInventoryMonitor _monitor;
        private readonly InventoryMonitorConfig _config;
        private readonly ILogger<InventoryMonitorStarter> _logger;
        public readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly ApplicationContext _appContext;
        public InventoryMonitorStarter(
               IInventoryMonitor monitor,
               IConfiguration configuration,
 ApplicationContext _AppContextData, IUnitOfWorkManage unitOfWorkManage,
        ILogger<InventoryMonitorStarter> logger)
        {
            _monitor = monitor;
            _appContext = _AppContextData;
            _unitOfWorkManage = unitOfWorkManage;
            _config = configuration
                .GetSection("InventoryMonitoring")
                .Get<InventoryMonitorConfig>() ?? new InventoryMonitorConfig();
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // 配置化启动 如果配置了启动参数 请修复代码并且打开注释
            //var config = Configuration.GetSection("InventoryMonitoring").Get<InventoryMonitorConfig>();
            //_monitor.StartMonitoring(config.CheckInterval);


            // 启动监控（间隔5分钟）
            //_monitor.StartMonitoring(TimeSpan.FromMinutes(5));
            //return Task.CompletedTask;

            try
            {
                _logger.LogInformation("开始初始化库存监控服务");

                // 加载所有必要的策略
                LoadRequiredStrategies();

                // 从配置启动监控
                if (_config.EnableRealTime)
                {
                    _monitor.StartMonitoring(_config.InitialDelay + _config.CheckInterval);
                    _logger.LogInformation("实时库存监控已启用");
                }
                else
                {
                    _monitor.StartMonitoring(_config.CheckInterval);
                    _logger.LogInformation("定时库存监控已启用");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "库存监控服务初始化失败");
                throw;
            }
            return Task.CompletedTask;
        }

        private void LoadRequiredStrategies()
        {
            // 确保添加了所有必要的策略
            _monitor.AddStrategy(_appContext.GetRequiredService<SafetyStockStrategy>());
            _monitor.AddStrategy(_appContext.GetRequiredService<RulesEngineStrategy>());
            _monitor.AddStrategy(_appContext.GetRequiredService<SalesTrendStrategy>());
        }
    }
}
