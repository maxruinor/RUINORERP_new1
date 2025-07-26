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
using RUINORERP.Model.ReminderModel;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model.ReminderModel.ReminderRules;

namespace RUINORERP.Server.SmartReminder
{
    //监控服务
    public class SmartReminderMonitor : ISmartReminderMonitor, IDisposable
    {

        //通过注入的方式，把缓存操作接口通过构造函数注入
        private readonly IMemoryCache _cache;

        public readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly ApplicationContext _appContext;

        private readonly NotificationService _notification;
        private readonly List<IReminderStrategy> _strategies = new();
        private readonly ILogger<SmartReminderMonitor> _logger;
        //推荐在服务/后台使用
        private Timer _timer;

        public SmartReminderMonitor(IMemoryCache cache,
           ILogger<SmartReminderMonitor> logger, ApplicationContext _AppContextData, IUnitOfWorkManage unitOfWorkManage, NotificationService notification)
        {
            _cache = cache;
            _logger = logger;
            _appContext = _AppContextData;
            _unitOfWorkManage = unitOfWorkManage;
            _notification = notification;
            //StartMonitoring(TimeSpan.FromMinutes(5));
        }
        private bool _isRunning;
        public bool IsRunning { get; set; }

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
            // 首次检查延迟15秒，等待其他服务初始化
            // 在检查入口处添加过滤
            // if (!_scheduler.ShouldTrigger(currentRule)) return;

            _timer = new Timer(async _ => await CheckRemindersAsync(),
                null,
                dueTime: TimeSpan.FromSeconds(15),
                period: interval);
            IsRunning = true;
        }

        private readonly SemaphoreSlim _checkLock = new(1, 1);

        public async Task CheckRemindersAsync()
        {
            Console.WriteLine("执行一行检测" + System.DateTime.Now);
            try
            {
                if (!await _checkLock.WaitAsync(TimeSpan.Zero)) return;

                //在线有不有人？有人才提醒？

                var activeRules = await GetActiveRulesAsync();
                foreach (var rule in activeRules)
                {
                    var irule = rule as IReminderRule;
                    var context = await CreateContextAsync(irule);

                    //这里要从数据库配置中转换过来
                    ReminderBizType reminderBiz = (ReminderBizType)irule.ReminderBizType;
                    if (reminderBiz == ReminderBizType.安全库存提醒)
                    {
                        var strategies = _strategies.Where(s => s.CanHandle(reminderBiz));
                        foreach (var strategy in strategies.OrderBy(s => s.Priority))
                        {
                            await strategy.CheckAsync(irule, context);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            finally
            {
                // 释放信号量,不然无法执行下一次任务
                _checkLock.Release();
            }

        }



        private async Task<IReminderContext> CreateContextAsync(IReminderRule rule)
        {
            return rule.ReminderBizType switch
            {
                (int)ReminderBizType.安全库存提醒 => await CreateInventoryContextAsync(rule),
                //ReminderBizType.单据审批提醒 => await CreateOrderContextAsync(rule),
                _ => throw new NotSupportedException()
            };
        }

        private async Task<IReminderContext> CreateInventoryContextAsync(IReminderRule rule)
        {
            var productIds = (rule.GetConfig<SafetyStockConfig>() as SafetyStockConfig).ProductIds;
            var stocks = await _unitOfWorkManage.GetDbClient().Queryable<tb_Inventory>()
                .Where(s => productIds.Contains(s.ProdDetailID))
                .WithCache()
                .ToListAsync();
            return new InventoryContext(stocks);
        }


        private static readonly MemoryCache _policyCache = new(new MemoryCacheOptions());

        public async Task<List<IReminderRule>> GetActiveRulesAsync()
        {
            // 尝试从缓存获取数据
            List<tb_ReminderRule> policies = null;
            bool cacheHit = _policyCache.TryGetValue("ActivePolicies", out policies);

            if (!cacheHit)
            {
                // 缓存未命中，从数据库查询
                policies = await _unitOfWorkManage.GetDbClient()
                    .Queryable<tb_ReminderRule>()
                    .Where(p => p.IsEnabled)
                    .ToListAsync();

                // 将结果存入缓存，设置5分钟过期
                _policyCache.Set("ActivePolicies", policies, TimeSpan.FromMinutes(5));
            }

            // 显式转换为接口列表
            var result = new List<IReminderRule>();
            foreach (var policy in policies)
            {
                result.Add(policy);
            }

            return result;
        }


        public void AddStrategy(IReminderStrategy strategy) => _strategies.Add(strategy);

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
