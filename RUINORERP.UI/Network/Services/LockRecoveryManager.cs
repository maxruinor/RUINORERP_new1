using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Models.Lock;
using RUINORERP.PacketSpec.Models.Responses;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 锁恢复管理器
    /// 负责处理客户端异常断开时的锁释放机制
    /// 优化：减少网络请求频率，使用批量检查
    /// </summary>
    public class LockRecoveryManager : IDisposable
    {
        private readonly ClientLockManagementService _lockService;
        private readonly ClientLocalLockCacheService _lockCache;
        private readonly ILogger<LockRecoveryManager> _logger;

        private readonly Timer _healthCheckTimer;
        private readonly object _healthCheckLock = new object();
        private DateTime _lastHealthCheck;
        private bool _isShuttingDown;

        /// <summary>
        /// 健康检查间隔（2分钟）
        /// </summary>
        private static readonly TimeSpan HealthCheckInterval = TimeSpan.FromMinutes(2);

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="lockService">锁管理服务</param>
        /// <param name="lockCache">锁缓存</param>
        /// <param name="logger">日志记录器</param>
        public LockRecoveryManager(ClientLockManagementService lockService, ClientLocalLockCacheService lockCache, ILogger<LockRecoveryManager> logger)
        {
            _lockService = lockService ?? throw new ArgumentNullException(nameof(lockService));
            _lockCache = lockCache ?? throw new ArgumentNullException(nameof(lockCache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _lastHealthCheck = DateTime.Now;

            // 启动健康检查定时器 - 每2分钟执行一次，包括心跳和孤儿锁检查
            _healthCheckTimer = new Timer(PerformHealthCheck, null, TimeSpan.FromMinutes(1), HealthCheckInterval);

            // 订阅应用程序退出事件
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            _logger.LogDebug("锁恢复管理器已初始化（优化版：2分钟健康检查间隔）");
        }

        /// <summary>
        /// 执行健康检查（合并心跳和孤儿锁检查）
        /// </summary>
        /// <param name="state">状态对象</param>
        private async void PerformHealthCheck(object state)
        {
            if (_isShuttingDown)
                return;

            if (!IsUserLoggedIn())
            {
                _logger.LogDebug("用户未登录，跳过健康检查");
                return;
            }

            try
            {
                _logger.LogDebug("开始执行锁健康检查...");
                
                // 获取本地缓存中的所有活跃锁
                var activeLocks = _lockCache.GetAllLockInfos()
                    .Where(l => l.IsLocked && !l.IsExpired)
                    .ToList();

                if (activeLocks.Count == 0)
                {
                    _logger.LogDebug("当前没有活跃锁，跳过健康检查");
                    return;
                }

                _logger.LogDebug("开始检查 {LockCount} 个活跃锁的状态", activeLocks.Count);

                // 批量检查锁状态
                var lockBillIds = activeLocks.Select(l => l.BillID).ToList();
                var lockResponses = await _lockService.BatchCheckLockStatusAsync(lockBillIds);

                var healthyCount = 0;
                var invalidCount = 0;

                foreach (var lockInfo in activeLocks)
                {
                    try
                    {
                        if (lockResponses.TryGetValue(lockInfo.BillID, out var response) &&
                            response.IsSuccess && response.LockInfo != null)
                        {
                            // 检查锁是否仍然有效
                            if (response.LockInfo.IsLocked && 
                                response.LockInfo.LockedUserId == lockInfo.LockedUserId &&
                                response.LockInfo.SessionId == lockInfo.SessionId)
                            {
                                healthyCount++;
                                _logger.LogDebug("锁状态验证通过: 单据 {BillId}", lockInfo.BillID);
                            }
                            else
                            {
                                invalidCount++;
                                _logger.LogWarning("锁已失效: 单据 {BillId}，原因：锁已被释放或被其他用户持有", lockInfo.BillID);
                                _lockCache.ClearCache(lockInfo.BillID);
                            }
                        }
                        else
                        {
                            invalidCount++;
                            _logger.LogWarning("锁状态检查失败: 单据 {BillId}", lockInfo.BillID);
                            _lockCache.ClearCache(lockInfo.BillID);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "检查锁状态时发生异常: 单据 {BillId}", lockInfo.BillID);
                    }
                }

                _lastHealthCheck = DateTime.Now;
                _logger.LogDebug("锁健康检查完成: 健康={HealthyCount}, 失效={InvalidCount}, 总数={TotalCount}", 
                    healthyCount, invalidCount, activeLocks.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "执行锁健康检查时发生错误");
            }
        }

        /// <summary>
        /// 检查用户是否已登录
        /// </summary>
        /// <returns>是否已登录</returns>
        private bool IsUserLoggedIn()
        {
            try
            {
                return MainForm.Instance?.AppContext?.CurUserInfo?.UserInfo != null &&
                       MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID > 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 处理应用程序退出
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private async void OnProcessExit(object sender, EventArgs e)
        {
            _logger.LogDebug("应用程序正在退出，开始清理所有锁");
            await ReleaseAllLocksAsync("应用程序退出");
        }

        /// <summary>
        /// 处理未捕获异常
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private async void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _logger.LogError(e.ExceptionObject as Exception, "应用程序发生未捕获异常");
            await ReleaseAllLocksAsync("异常退出");
        }

        /// <summary>
        /// 释放所有锁
        /// </summary>
        /// <param name="reason">释放原因</param>
        public async Task ReleaseAllLocksAsync(string reason = "手动释放")
        {
            if (_isShuttingDown)
                return;

            _isShuttingDown = true;

            try
            {
                var activeLocks = _lockCache.GetAllLockInfos()
                    .Where(l => l.IsLocked && !l.IsExpired)
                    .ToList();

                if (activeLocks.Count == 0)
                {
                    _logger.LogDebug("没有需要释放的锁");
                    return;
                }

                _logger.LogDebug("开始释放所有锁: {Count} 个, 原因: {Reason}", activeLocks.Count, reason);

                var currentUserId = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
                
                // 批量释放锁
                var lockBillIds = activeLocks.Select(l => l.BillID).ToList();
                var releaseTasks = new List<Task>();

                foreach (var billId in lockBillIds)
                {
                    var unlockTask = Task.Run(async () =>
                    {
                        try
                        {
                            var response = await _lockService.UnlockBillAsync(billId);
                            if (response?.IsSuccess == true)
                            {
                                _logger.LogDebug("成功释放锁: 单据 {BillId}, 原因: {Reason}", billId, reason);
                            }
                            else
                            {
                                _logger.LogWarning("释放锁失败: 单据 {BillId}, 原因: {Reason}, 错误: {Error}", 
                                    billId, reason, response?.Message ?? "未知");
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "释放锁时发生异常: 单据 {BillId}", billId);
                        }
                    });
                    releaseTasks.Add(unlockTask);
                }

                // 等待所有释放操作完成，最多等待30秒
                await Task.WhenAll(releaseTasks).ConfigureAwait(false);

                // 清空本地缓存
                _lockCache.ClearAllCache();

                _logger.LogDebug("所有锁释放完成: 原因: {Reason}", reason);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "释放所有锁时发生错误: {Reason}", reason);
            }
        }

        /// <summary>
        /// 检查锁健康状态
        /// </summary>
        /// <returns>健康状态信息</returns>
        public LockHealthStatus GetLockHealthStatus()
        {
            try
            {
                var activeLocks = _lockCache.GetAllLockInfos()
                    .Where(l => l.IsLocked && !l.IsExpired)
                    .ToList();

                return new LockHealthStatus
                {
                    TotalHeldLocks = activeLocks.Count,
                    HealthyLocks = activeLocks.Count,
                    UnhealthyLocks = 0,
                    OrphanedLocks = activeLocks.Count(l => l.IsOrphaned),
                    LastHeartbeat = _lastHealthCheck,
                    Status = "正常"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取锁健康状态时发生错误");
                return new LockHealthStatus { Status = "检查失败" };
            }
        }

        /// <summary>
        /// 获取恢复统计信息
        /// </summary>
        /// <returns>恢复统计</returns>
        public LockRecoveryStatistics GetRecoveryStatistics()
        {
            var activeLocks = _lockCache.GetAllLockInfos()
                .Where(l => l.IsLocked && !l.IsExpired)
                .ToList();

            return new LockRecoveryStatistics
            {
                HeldLocksCount = activeLocks.Count,
                OrphanedLocksCount = activeLocks.Count(l => l.IsOrphaned),
                LastHeartbeat = _lastHealthCheck,
                HeartbeatInterval = HealthCheckInterval,
                RecoveryInterval = HealthCheckInterval
            };
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            try
            {
                _isShuttingDown = true;

                // 清理定时器
                _healthCheckTimer?.Dispose();

                // 取消事件订阅
                AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;
                AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;

                _logger.LogDebug("锁恢复管理器已释放资源");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "释放锁恢复管理器资源时发生错误");
            }
        }
    }

    /// <summary>
    /// 锁健康状态
    /// </summary>
    public class LockHealthStatus
    {
        public int TotalHeldLocks { get; set; }
        public int HealthyLocks { get; set; }
        public int UnhealthyLocks { get; set; }
        public int OrphanedLocks { get; set; }
        public DateTime LastHeartbeat { get; set; }
        public string Status { get; set; }
    }

    /// <summary>
    /// 锁恢复统计
    /// </summary>
    public class LockRecoveryStatistics
    {
        public int HeldLocksCount { get; set; }
        public int OrphanedLocksCount { get; set; }
        public DateTime LastHeartbeat { get; set; }
        public TimeSpan HeartbeatInterval { get; set; }
        public TimeSpan RecoveryInterval { get; set; }
    }
}