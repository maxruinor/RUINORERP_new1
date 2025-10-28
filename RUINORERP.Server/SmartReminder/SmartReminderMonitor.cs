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

        private readonly IUnitOfWorkManage _unitOfWorkManage;
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "健康检查执行失败");
                return false;
            }
        }
        public void StartMonitoring(TimeSpan interval)
        {
            // 首次检查延迟15秒，等待其他服务初始化
            // 在检查入口处添加过滤
            // if (!_scheduler.ShouldTrigger(currentRule)) return;

            _timer = new Timer(TimerCallback,
                null,
                dueTime: TimeSpan.FromSeconds(15),
                period: interval);
            IsRunning = true;
        }
        
        // 使用void返回类型的Timer回调，内部处理异步任务
        private void TimerCallback(object state)
        {
            // 不等待异步任务完成，避免Timer回调中出现异常被吞掉
            _ = CheckRemindersAsync().ContinueWith(task =>
            {
                if (task.IsFaulted && task.Exception != null)
                {
                    _logger.LogError(task.Exception, "定时检查提醒规则时发生未处理异常");
                }
            });
        }

        private readonly SemaphoreSlim _checkLock = new(1, 1);

        public async Task CheckRemindersAsync()
        {
            _logger.LogDebug("开始执行检测任务 - {Time}", System.DateTime.Now);
            bool lockAcquired = false;
            IReminderContext reminderContext = null;
            
            try
            {
                if (!await _checkLock.WaitAsync(TimeSpan.Zero))
                {
                    _logger.LogDebug("检测任务仍在执行中，跳过本次调用");
                    return;
                }
                lockAcquired = true;

                //在线有不有人？有人才提醒？

                var activeRules = await GetActiveRulesAsync();
                foreach (var rule in activeRules)
                {
                    reminderContext = null; // 重置上下文引用，确保不会引用到前一个规则的上下文
                    
                    try
                    {
                        var irule = rule as IReminderRule;
                        if (irule == null)
                        {
                            _logger.LogWarning("规则类型转换失败: {RuleType}", rule.GetType().Name);
                            continue;
                        }
                         
                        reminderContext = await CreateContextAsync(irule);

                        //这里要从数据库配置中转换过来
                        ReminderBizType reminderBiz = (ReminderBizType)irule.ReminderBizType;
                        if (reminderBiz == ReminderBizType.安全库存提醒)
                        {
                            var strategies = _strategies.Where(s => s.CanHandle(reminderBiz));
                            foreach (var strategy in strategies.OrderBy(s => s.Priority))
                            {
                                await strategy.CheckAsync(irule, reminderContext);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "处理规则 {RuleId} 时出错", rule?.GetType().Name);
                        // 继续处理下一个规则，不中断整个流程
                    }
                    finally
                    {
                        // 释放上下文资源
                        if (reminderContext is IDisposable disposableContext)
                        {
                            disposableContext.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "执行提醒检查时发生未处理异常");
            }
            finally
            {
                // 确保释放信号量
                if (lockAcquired)
                {
                    _checkLock.Release();
                    lockAcquired = false;
                }
                _logger.LogDebug("检测任务执行完成 - {Time}", System.DateTime.Now);
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
            // 确保始终返回一个有效的上下文，即使出错
            IReminderContext resultContext = new InventoryContext(new List<tb_Inventory>());
            
            try
            {
                var productIds = (rule.GetConfig<SafetyStockConfig>() as SafetyStockConfig)?.ProductIds;
                if (productIds == null || productIds.Count == 0)
                {
                    _logger.LogWarning("产品ID列表为空，无法创建库存上下文");
                    return resultContext; // 返回空上下文
                }
                
                // 移除using块，让SqlSugar自己管理连接生命周期
                var db = _unitOfWorkManage.GetDbClient();
                var stocks = await db.Queryable<tb_Inventory>()
                    .Where(s => productIds.Contains(s.ProdDetailID))
                    .ToListAsync();
                
                resultContext = new InventoryContext(stocks);
                return resultContext;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建库存上下文失败");
                return resultContext; // 返回空上下文
            }
        }


        public async Task<List<IReminderRule>> GetActiveRulesAsync()
        {
            // 使用注入的缓存而非静态缓存，确保依赖注入一致性
            const string cacheKey = "ActivePolicies";
            
            // 尝试从缓存获取数据
            if (_cache.TryGetValue(cacheKey, out List<tb_ReminderRule> policies))
            {
                _logger.LogDebug("从缓存获取活跃规则，数量: {Count}", policies.Count);
                
                // 显式转换为接口列表
                return policies.Select(p => p as IReminderRule).ToList();
            }

            try
            {
                // 缓存未命中，从数据库异步查询
                // 移除using块，让SqlSugar自己管理连接生命周期
                var db = _unitOfWorkManage.GetDbClient();
                policies = await db.Queryable<tb_ReminderRule>()
                    .Where(p => p.IsEnabled)
                    .ToListAsync();

                _logger.LogDebug("从数据库获取活跃规则，数量: {Count}", policies.Count);
                
                // 将结果存入缓存，设置5分钟过期和滑动过期
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                    .RegisterPostEvictionCallback((key, value, reason, state) =>
                    {
                        _logger.LogDebug("规则缓存已过期或被移除，原因: {Reason}", reason);
                    });
                
                _cache.Set(cacheKey, policies, cacheOptions);
                
                // 显式转换为接口列表
                return policies.Select(p => p as IReminderRule).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取活跃规则失败");
                return new List<IReminderRule>();
            }
        }


        public void AddStrategy(IReminderStrategy strategy) => _strategies.Add(strategy);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // 分阶段停止和资源释放
                if (_timer != null)
                {
                    try
                    {
                        _timer.Change(Timeout.Infinite, 0); // 立即停止触发
                    }
                    catch (ObjectDisposedException) { }
                    
                    _timer.Dispose();
                    _timer = null;
                }
                
                if (_checkLock != null)
                {
                    _checkLock.Dispose();
                    // 注意：不要将_checkLock设置为null，因为它在析构函数中可能仍会被访问
                }
                
                _logger.LogInformation("SmartReminderMonitor已释放资源");
            }
        }
        
        // 析构函数
        ~SmartReminderMonitor()
        {
            Dispose(false);
        }


    }

}
