using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RUINORERP.Model.Context;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Server.SmartReminder.InvReminder;
using RUINORERP.Server.SmartReminder.ReminderRuleStrategy;
using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.SmartReminder
{

    // 统一的智能提醒服务入口
    public class SmartReminderService : BackgroundService
    {
        private readonly ISmartReminderMonitor _monitor;
        private readonly ILogger<SmartReminderService> _logger;
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly ApplicationContext _appContext;
        private readonly NotificationService _notification;
        private readonly SemaphoreSlim _checkLock = new(1, 1);
        private Timer _timer;
        private bool _isRunning;

        public SmartReminderService(
            ISmartReminderMonitor monitor,
            IConfiguration configuration,
            ApplicationContext appContext,
            IUnitOfWorkManage unitOfWorkManage,
            NotificationService notification,
            ILogger<SmartReminderService> logger)
        {
            _monitor = monitor;
            _appContext = appContext;
            _unitOfWorkManage = unitOfWorkManage;
            _notification = notification;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("开始初始化智能提醒监控服务");

                // 加载所有必要的策略
                LoadRequiredStrategies();

                StartMonitoring(TimeSpan.FromMinutes(1000));
                _logger.LogInformation("实时提醒监控已启用");

                // 注册取消令牌回调
                stoppingToken.Register(() =>
                {
                    _logger.LogInformation("收到服务停止请求，正在清理资源...");
                    StopMonitoring();
                });



            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "智能提醒监控服务初始化失败");
                throw;
            }

            return Task.CompletedTask;
        }

        private void LoadRequiredStrategies()
        {
            // 确保添加了所有必要的策略
            _monitor.AddStrategy(_appContext.GetRequiredService<SafetyStockStrategy>());
            _monitor.AddStrategy(_appContext.GetRequiredService<SalesTrendStrategy>());
            //_logger.LogInformation("已加载 {StrategyCount} 个提醒策略", _monitor.Strategies.Count);
        }

        private void StartMonitoring(TimeSpan interval)
        {
            if (_isRunning)
            {
                _logger.LogWarning("监控服务已在运行中，请勿重复启动");
                return;
            }

            _timer = new Timer(
                async _ => await SafeCheckRemindersAsync(),
                null,
                TimeSpan.FromSeconds(15), // 首次检查延迟，等待其他服务初始化
                interval);

            _isRunning = true;
            _logger.LogInformation("提醒监控服务已启动，检查间隔: {Interval}", interval);
        }

        private void StopMonitoring()
        {
            if (!_isRunning)
            {
                _logger.LogInformation("监控服务未运行，无需停止");
                return;
            }

            try
            {
                _timer?.Change(Timeout.Infinite, 0); // 立即停止触发
                _timer?.Dispose();
                _isRunning = false;
                _logger.LogInformation("提醒监控服务已停止");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "停止监控服务时发生错误");
            }
        }

        // 安全执行提醒检查，带异常处理
        private async Task SafeCheckRemindersAsync()
        {
            Console.WriteLine("A执行一行检测" + System.DateTime.Now);
            

                if (!await _checkLock.WaitAsync(TimeSpan.Zero))
            {
                _logger.LogDebug("上一次检查尚未完成，跳过本次执行");
                return;
            }

            try
            {
                _logger.LogDebug("开始执行提醒检查...");
                await _monitor.CheckRemindersAsync();
                _logger.LogDebug("提醒检查完成");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "执行提醒检查时发生错误");

                // 降级处理逻辑
                if (ex is DbException)
                {
                    _logger.LogCritical("数据库访问失败，进入降级模式");
                    EnterDegradedMode();
                }
            }
            finally
            {
                _checkLock.Release();
            }
        }

        private void EnterDegradedMode()
        {
            // 实现降级模式逻辑，如减少检查频率、停止非关键操作等
            if (_timer != null)
            {
                _timer.Change(TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(30));
                _logger.LogWarning("已调整为降级模式，检查间隔延长至30分钟");
            }
        }

        public bool PerformQuickCheck()
        {
            try
            {
                // 执行健康检查
                _logger.LogDebug("执行服务健康检查...");
                return _monitor.PerformQuickCheck();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "健康检查失败");
                return false;
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("正在优雅地停止智能提醒监控服务...");
            StopMonitoring();
            await base.StopAsync(cancellationToken);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _timer?.Dispose();
                _checkLock?.Dispose();
            }
        }
    }
     



}