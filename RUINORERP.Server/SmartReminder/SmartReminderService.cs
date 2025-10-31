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
        private readonly IConfiguration _configuration;
        private readonly SemaphoreSlim _checkLock = new(1, 1);
        private Timer _timer;
        private bool _isRunning;
        
        // 重试配置
        private const int MaxRetryAttempts = 3;
        private const int BaseDelayMs = 1000;
        private const int MaxDelayMs = 30000; // 最大30秒

        public SmartReminderService(
            ISmartReminderMonitor monitor,
            IConfiguration configuration,
            ApplicationContext appContext,
            IUnitOfWorkManage unitOfWorkManage,
            NotificationService notification,
            ILogger<SmartReminderService> logger)
        {
            _monitor = monitor;
            _configuration = configuration;
            _appContext = appContext;
            _unitOfWorkManage = unitOfWorkManage;
            _notification = notification;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("智能提醒服务正在启动...");
                
                // 加载所有必要的策略，带重试机制
                await RetryAsync(() => 
                {
                    LoadRequiredStrategies();
                    return Task.CompletedTask;
                }, "加载提醒策略", MaxRetryAttempts, stoppingToken);

                // 从配置文件读取监控间隔，默认为5分钟
                var monitorIntervalMinutes = _configuration.GetValue<int>("SmartReminder:MonitorIntervalMinutes", 5);
                StartMonitoring(TimeSpan.FromMinutes(monitorIntervalMinutes));

                _logger.LogInformation("智能提醒服务已启动，监控间隔: {Interval}分钟", monitorIntervalMinutes);
                
                // 注册取消令牌回调
                stoppingToken.Register(() =>
                {
                    _logger.LogInformation("收到停止信号，准备停止智能提醒服务...");
                    StopMonitoring();
                });

                // 等待取消令牌
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("智能提醒服务已正常停止");
                // 取消操作不需要重新抛出
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "智能提醒服务启动或运行时发生错误");
                throw; // 重新抛出异常以通知宿主服务发生错误
            }
        }

        private void LoadRequiredStrategies()
        {
            try
            {
                // 确保添加了所有必要的策略
                _monitor.AddStrategy(_appContext.GetRequiredService<SafetyStockStrategy>());
                _monitor.AddStrategy(_appContext.GetRequiredService<SalesTrendStrategy>());
                
                _logger.LogInformation("已成功加载提醒策略");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载提醒策略失败");
                throw; // 重新抛出异常以触发重试
            }
        }

        private void StartMonitoring(TimeSpan interval)
        {
            if (_isRunning)
            {
                _logger.LogWarning("监控服务已在运行中，请勿重复启动");
                return;
            }

            _timer = new Timer(
                TimerCallback,
                null,
                TimeSpan.FromSeconds(15), // 首次检查延迟，等待其他服务初始化
                interval);

            _isRunning = true;
            _logger.LogInformation("监控服务已启动，间隔: {Interval}", interval);
        }
        
        // Timer回调使用void返回类型，避免异常被吞没
        private void TimerCallback(object state)
        {
            try
            {
                // 不等待异步任务完成，避免阻塞Timer线程池
                _ = RetryAsync(async () => 
                    {
                        try
                        {
                            return await SafeCheckRemindersAsync();
                        }
                        catch (Exception innerEx)
                        {
                            // 捕获异步操作中的异常并记录详细信息
                            _logger.LogError(innerEx, "SafeCheckRemindersAsync执行过程中发生异常");
                            throw; // 重新抛出以触发重试逻辑
                        }
                    },
                    "定时器触发的提醒检查", MaxRetryAttempts, CancellationToken.None)
                    .ContinueWith(task =>
                    {
                        if (task.IsFaulted && task.Exception != null)
                        {
                            // 处理所有未观察到的异常，避免异常被吞没有
                            _logger.LogError(task.Exception, "安全检查提醒过程中发生未处理异常，已达到最大重试次数");
                            // 处理AggregateException，记录内部异常详情
                            foreach (var ex in task.Exception.Flatten().InnerExceptions)
                            {
                                _logger.LogError(ex, "重试过程中的内部异常详情");
                            }
                        }
                    }, TaskContinuationOptions.ExecuteSynchronously); // 同步执行，确保异常被立即处理
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "TimerCallback执行过程中发生同步异常");
            }
        }

        private void StopMonitoring()
        {
            if (!_isRunning)
            {
                return;
            }

            try
            {
                _timer?.Change(Timeout.Infinite, 0); // 立即停止触发
                _timer?.Dispose();
                _isRunning = false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "停止监控服务时发生错误");
            }
        }

        // 安全执行提醒检查，带异常处理
        private async Task SafeCheckRemindersAsync()
        {
            _logger.LogDebug("开始安全检查提醒任务 - {Time}", System.DateTime.Now);
            
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
                
                throw; // 重新抛出异常以触发重试
            }
            finally
            {
                _checkLock.Release();
                _logger.LogDebug("安全检查提醒任务完成 - {Time}", System.DateTime.Now);
            }
        }
        
        #region 重试机制实现
        /// <summary>
        /// 异步重试方法，用于处理可能因网络不稳定等原因失败的操作
        /// </summary>
        /// <param name="operation">要执行的异步操作</param>
        /// <param name="operationName">操作名称，用于日志记录</param>
        /// <param name="maxRetries">最大重试次数</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>操作的任务结果</returns>
        private async Task RetryAsync(Func<Task> operation, string operationName, int maxRetries, CancellationToken cancellationToken)
        {
            int attempt = 0;
            Exception lastException = null;

            while (true)
            {
                attempt++;
                try
                {
                    _logger.LogDebug("执行操作 '{OperationName}'，第 {Attempt}/{MaxRetries} 次尝试", 
                        operationName, attempt, maxRetries);

                    await operation();
                    _logger.LogDebug("操作 '{OperationName}' 执行成功，尝试次数: {Attempt}", 
                        operationName, attempt);
                    return;
                }
                catch (Exception ex)
                {
                    lastException = ex;

                    // 检查是否应该重试
                    if (attempt >= maxRetries || IsNonRetryableException(ex) || cancellationToken.IsCancellationRequested)
                    {
                        _logger.LogError(ex, "操作 '{OperationName}' 失败，已达到最大重试次数或发生不可重试的异常");
                        throw;
                    }

                    // 计算重试延迟（使用指数退避策略）
                    int delayMs = Math.Min(BaseDelayMs * (int)Math.Pow(2, attempt - 1), MaxDelayMs);
                    // 添加随机抖动以避免多个服务同时重试
                    int jitterMs = new Random().Next(0, 1000);
                    int totalDelayMs = delayMs + jitterMs;

                    _logger.LogWarning(ex, "操作 '{OperationName}' 失败，将在 {DelayMs}ms 后重试（第 {Attempt}/{MaxRetries} 次）",
                        operationName, totalDelayMs, attempt, maxRetries);

                    await Task.Delay(totalDelayMs, cancellationToken);
                }
            }
        }

        /// <summary>
        /// 确定异常是否可重试
        /// </summary>
        /// <param name="ex">要检查的异常</param>
        /// <returns>如果异常可重试，则为true；否则为false</returns>
        private bool IsNonRetryableException(Exception ex)
        {
            // 以下类型的异常通常不可重试
            return ex is ArgumentException ||
                   ex is ArgumentNullException ||
                   ex is ArgumentOutOfRangeException ||
                   ex is InvalidOperationException ||
                   ex is NotSupportedException ||
                   ex is UnauthorizedAccessException ||
                   ex is NotImplementedException;
        }
        #endregion

        private void EnterDegradedMode()
        {
            // 实现降级模式逻辑，如减少检查频率、停止非关键操作等
            if (_timer != null)
            {
                try
                {
                    _timer.Change(TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(30));
                    _logger.LogWarning("已调整为降级模式，检查间隔延长至30分钟");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "切换到降级模式时发生错误");
                }
            }
            else
            {
                _logger.LogWarning("无法切换到降级模式，Timer实例为空");
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