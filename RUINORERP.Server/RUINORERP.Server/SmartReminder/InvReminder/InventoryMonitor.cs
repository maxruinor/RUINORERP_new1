using RUINORERP.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using RUINORERP.Server.Workflow.WFReminder;
using Microsoft.Extensions.Caching.Memory;
using System.Data.Common;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Model.Context;
using WorkflowCore.Primitives;
using CacheManager.Core;
using RUINORERP.Common;

namespace RUINORERP.Server.SmartReminder.InvReminder
{
    // 6. 库存监控服务
    public class InventoryMonitor : IInventoryMonitor, IDisposable
    {

        //通过注入的方式，把缓存操作接口通过构造函数注入
        private readonly IMemoryCache _cache;

        public readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly ApplicationContext _appContext;

        private readonly NotificationService _notification;
        private readonly List<IAlertStrategy> _strategies = new();
        private readonly ILogger<InventoryMonitor> _logger;
        //推荐在服务/后台使用
        private Timer _timer;

        public InventoryMonitor(IMemoryCache cache,
           ILogger<InventoryMonitor> logger, ApplicationContext _AppContextData, IUnitOfWorkManage unitOfWorkManage, NotificationService notification)
        {
            _cache = cache;
            _logger = logger;
            _appContext = _AppContextData;
            _unitOfWorkManage = unitOfWorkManage;
            _notification = notification;
            StartMonitoring(TimeSpan.FromMinutes(5));
        }
        private bool _isRunning;
        public bool IsRunning => _isRunning;

        public bool PerformQuickCheck()
        {
            try
            {
                // 这里可以执行一个简单的数据库查询或缓存检查
                var quickTestKey = "HealthCheck_QuickTest";
                _cache.Set(quickTestKey, DateTime.Now, TimeSpan.FromSeconds(5));
                return _cache.TryGetValue(quickTestKey, out _);
            }
            catch
            {
                return false;
            }
        }
        public void StartMonitoring(TimeSpan interval)
        {
            //    // 使用 Change 方法管理状态
            //_timer = new Timer(CheckInventoryCallback,
            //    null,
            //    TimeSpan.Zero,
            //    interval);
            // 首次检查延迟15秒，等待其他服务初始化
            // 在检查入口处添加过滤
            // if (!_scheduler.ShouldTrigger(currentRule)) return;


            _timer = new Timer(async _ => await CheckInventoryAsync(),
                null,
                dueTime: TimeSpan.FromSeconds(15),
                period: interval);

            //_timer = new Timer(async _ => await CheckInventoryAsync(),
            //    null, TimeSpan.Zero, interval);
        }
        // TODO 要如果调用修复？
        //private async void SafeCheckInventory()
        //{
        //    try
        //    {
        //        await CheckInventoryAsync();
        //    }
        //    catch (DbException ex)
        //    {
        //        _logger.LogError("数据库访问失败: {Message}", ex.Message);
        //        EnterDegradedMode();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogCritical("未处理异常: {StackTrace}", ex.StackTrace);
        //        StopMonitoring();
        //    }
        //}

        private readonly SemaphoreSlim _checkLock = new(1, 1);

        public async Task CheckInventoryAsync()
        {
            if (!await _checkLock.WaitAsync(TimeSpan.Zero)) return;

            try
            {
                // 执行检查
                var policies = await _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderRule>()
                                 .Where(p => p.IsEnabled)
                                 .ToListAsync();

                foreach (var policy in policies)
                {
                    var stock = await _unitOfWorkManage.GetDbClient().Queryable<tb_Inventory>()
                                       .FirstAsync(p => p.ProdDetailID > 0);

                    foreach (var strategy in _strategies.OrderBy(s => s.Priority))
                    {
                        await strategy.CheckAsync(policy, stock);
                    }
                }
            }
            finally
            {
                _checkLock.Release();
            }
        }

        private async void CheckInventoryCallback(object state)
        {
            try
            {
                await CheckInventoryAsync();
            }
            catch (Exception ex)
            {
                // 记录日志
                _logger.Error("库存检查失败", ex);
            }
        }



        private static readonly MemoryCache _policyCache = new(new MemoryCacheOptions());

        private async Task<List<tb_ReminderRule>> GetActivePoliciesAsync()
        {
            return await _policyCache.GetOrCreateAsync("ActivePolicies", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                return await _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderRule>()
                              .Where(p => p.IsEnabled)
                              .ToListAsync();
            });
        }
        public void AddStrategy(IAlertStrategy strategy) => _strategies.Add(strategy);

        public void Dispose()
        {
            // 分阶段停止
            _timer?.Change(Timeout.Infinite, 0); // 立即停止触发
            _timer?.Dispose();

            //// 等待正在执行的任务完成 TODO 请完善代码
            //var timeout = Task.Delay(5000);
            //var completion = Task.WhenAll(_runningTasks);
            //await Task.WhenAny(completion, timeout);
        }
    }

}
