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
    /// </summary>
    public class LockRecoveryManager : IDisposable
    {
        private readonly ClientLockManagementService _lockService;
        private readonly ClientLocalLockCacheService _lockCache;
        private readonly ILogger<LockRecoveryManager> _logger;

        // 客户端心跳检测
        private readonly Timer _heartbeatTimer;
        private readonly Timer _lockRecoveryTimer;
        private readonly object _heartbeatLock = new object();

        // 客户端持有的锁信息
        private readonly Dictionary<long, LockInfo> _heldLocks;
        private DateTime _lastHeartbeat;
        private bool _isShuttingDown;

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

            _heldLocks = new Dictionary<long, LockInfo>();
            _lastHeartbeat = DateTime.Now;

            // 启动心跳定时器 - 每30秒发送一次心跳
            _heartbeatTimer = new Timer(SendHeartbeat, null, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30));

            // 启动锁恢复定时器 - 每1分钟检查一次孤儿锁
            _lockRecoveryTimer = new Timer(RecoverOrphanedLocks, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));

            // 订阅应用程序退出事件
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            _logger.LogInformation("锁恢复管理器已初始化");
        }

        /// <summary>
        /// 注册持有的锁
        /// </summary>
        /// <param name="BillID">单据ID</param>
        /// <param name="sessionId">会话ID</param>
        public void RegisterHeldLock(long BillID, string sessionId)
        {
            try
            {
                lock (_heartbeatLock)
                {
                    _heldLocks[BillID] = new LockInfo
                    {
                        BillID = BillID,
                        SessionId = sessionId,
                        LockTime = DateTime.Now,
                        LastHeartbeat = DateTime.Now,
                        Type = LockType.Exclusive
                    };
                }

                _logger.LogDebug($"注册持有的锁: 单据 {BillID}, 会话 {sessionId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"注册持有的锁失败: 单据 {BillID}");
            }
        }

        /// <summary>
        /// 注销持有的锁
        /// </summary>
        /// <param name="BillID">单据ID</param>
        public void UnregisterHeldLock(long BillID)
        {
            try
            {
                lock (_heartbeatLock)
                {
                    if (_heldLocks.Remove(BillID))
                    {
                        _logger.LogDebug($"注销持有的锁: 单据 {BillID}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"注销持有的锁失败: 单据 {BillID}");
            }
        }

        /// <summary>
        /// 获取所有持有的锁
        /// </summary>
        /// <returns>持有的锁列表</returns>
        public List<LockInfo> GetHeldLocks()
        {
            lock (_heartbeatLock)
            {
                return new List<LockInfo>(_heldLocks.Values);
            }
        }

        /// <summary>
        /// 发送心跳
        /// </summary>
        /// <param name="state">状态对象</param>
        private async void SendHeartbeat(object state)
        {
            if (_isShuttingDown)
                return;

            try
            {


                if (MainForm.Instance.AppContext.CurrentUser == null || MainForm.Instance.AppContext.CurUserInfo.UserInfo == null)
                {
                    return;
                }

                var currentUserId = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
                if (currentUserId == 0)
                {
                    _logger.LogDebug("用户未登录，跳过心跳发送");
                    return;
                }

                // 获取当前持有的所有锁
                var heldLocks = GetHeldLocks();
                if (heldLocks.Count == 0)
                {
                    _logger.LogDebug("当前没有持有锁，跳过心跳发送");
                    return;
                }

                bool allLocksValid = true;
                var invalidLocks = new List<long>();

                foreach (var heldLock in heldLocks)
                {
                    // 检查锁是否仍然有效
                    var lockResponse = await _lockService.CheckLockStatusAsync(heldLock.BillID);

                    if (lockResponse == null || !lockResponse.IsSuccess ||
                        lockResponse.LockInfo?.SessionId != heldLock.SessionId)
                    {
                        allLocksValid = false;
                        invalidLocks.Add(heldLock.BillID);
                        _logger.LogWarning($"心跳检测发现无效锁: 单据 {heldLock.BillID}, 会话 {heldLock.SessionId}");
                    }
                    else
                    {
                        // 更新最后心跳时间
                        lock (_heartbeatLock)
                        {
                            if (_heldLocks.TryGetValue(heldLock.BillID, out var lockInfo))
                            {
                                lockInfo.LastHeartbeat = DateTime.Now;
                            }
                        }
                    }
                }

                // 如果有无效锁，从本地列表中移除
                if (invalidLocks.Count > 0)
                {
                    foreach (var BillID in invalidLocks)
                    {
                        UnregisterHeldLock(BillID);
                        _lockCache.ClearCache(BillID);
                    }
                }

                _lastHeartbeat = DateTime.Now;

                if (allLocksValid && heldLocks.Count > 0)
                {
                    _logger.LogDebug($"心跳成功: {heldLocks.Count} 个锁仍然有效");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "发送心跳时发生错误");
            }
        }

        /// <summary>
        /// 恢复孤儿锁
        /// </summary>
        /// <param name="state">状态对象</param>
        private async void RecoverOrphanedLocks(object state)
        {
            if (_isShuttingDown)
                return;

            try
            {
                if (MainForm.Instance.AppContext.CurrentUser == null || MainForm.Instance.AppContext.CurUserInfo.UserInfo == null)
                {
                    return;
                }


                var currentUserId = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
                if (currentUserId == 0)
                    return;

                List<LockInfo> orphanedLocks;
                lock (_heartbeatLock)
                {
                    orphanedLocks = _heldLocks.Values.Where(l => l.IsOrphaned).ToList();
                }

                if (orphanedLocks.Count == 0)
                {
                    _logger.LogDebug("没有发现孤儿锁");
                    return;
                }

                _logger.LogWarning($"发现 {orphanedLocks.Count} 个孤儿锁，开始恢复");

                foreach (var orphanedLock in orphanedLocks)
                {
                    try
                    {
                        // 尝试重新验证锁状态
                        var lockResponse = await _lockService.CheckLockStatusAsync(orphanedLock.BillID);

                        if (lockResponse == null || !lockResponse.IsSuccess)
                        {
                            // 锁已不存在，从本地移除
                            UnregisterHeldLock(orphanedLock.BillID);
                            _lockCache.ClearCache(orphanedLock.BillID);
                            _logger.LogInformation($"孤儿锁已自动清理: 单据 {orphanedLock.BillID}");
                        }
                        else if (lockResponse.LockInfo?.LockedUserId == currentUserId)
                        {
                            // 锁仍然属于当前用户，重置心跳时间
                            lock (_heartbeatLock)
                            {
                                if (_heldLocks.TryGetValue(orphanedLock.BillID, out var lockInfo))
                                {
                                    lockInfo.LastHeartbeat = DateTime.Now;
                                }
                            }
                            _logger.LogInformation($"孤儿锁验证通过，重置心跳: 单据 {orphanedLock.BillID}");
                        }
                        else
                        {
                            // 锁被其他用户持有，清除本地缓存
                            UnregisterHeldLock(orphanedLock.BillID);
                            _lockCache.ClearCache(orphanedLock.BillID);
                            _logger.LogWarning($"孤儿锁被其他用户持有: 单据 {orphanedLock.BillID}, 持有用户: {lockResponse.LockInfo?.LockedUserName}");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"恢复孤儿锁失败: 单据 {orphanedLock.BillID}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "恢复孤儿锁时发生错误");
            }
        }

        /// <summary>
        /// 处理应用程序退出
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private async void OnProcessExit(object sender, EventArgs e)
        {
            _logger.LogInformation("应用程序正在退出，开始清理所有锁");
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
                var heldLocks = GetHeldLocks();
                if (heldLocks.Count == 0)
                {
                    _logger.LogInformation("没有需要释放的锁");
                    return;
                }

                _logger.LogInformation($"开始释放所有锁: {heldLocks.Count} 个, 原因: {reason}");

                var currentUserId = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
                var releaseTasks = new List<Task>();

                foreach (var heldLock in heldLocks)
                {
                    var unlockTask = Task.Run(async () =>
                    {
                        try
                        {
                            // 尝试正常解锁
                            var response = await _lockService.UnlockBillAsync(heldLock.BillID, currentUserId);

                            if (response?.IsSuccess == true)
                            {
                                _logger.LogInformation($"成功释放锁: 单据 {heldLock.BillID}, 原因: {reason}");
                            }
                            else
                            {
                                // 如果正常解锁失败，尝试强制解锁
                                var forceResponse = await _lockService.UnlockBillAsync(heldLock.BillID, currentUserId);
                                if (forceResponse?.IsSuccess == true)
                                {
                                    _logger.LogWarning($"强制释放锁: 单据 {heldLock.BillID}, 原因: {reason}");
                                }
                                else
                                {
                                    _logger.LogError($"释放锁失败: 单据 {heldLock.BillID}, 原因: {reason}");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"释放锁时发生异常: 单据 {heldLock.BillID}");
                        }
                    });

                    releaseTasks.Add(unlockTask);
                }

                // 等待所有释放操作完成，最多等待30秒
                await Task.WhenAll(releaseTasks).ConfigureAwait(false);

                // 清空本地锁列表
                lock (_heartbeatLock)
                {
                    _heldLocks.Clear();
                }

                _logger.LogInformation($"所有锁释放完成: 原因: {reason}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"释放所有锁时发生错误: {reason}");
            }
        }

        /// <summary>
        /// 检查锁健康状态
        /// </summary>
        /// <returns>健康状态信息</returns>
        public async Task<LockHealthStatus> CheckLockHealthAsync()
        {
            try
            {
                var heldLocks = GetHeldLocks();
                var currentUserId = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;


                var healthyLocks = new List<long>();
                var unhealthyLocks = new List<long>();
                var orphanedLocks = heldLocks.FindAll(l => l.IsOrphaned).ConvertAll(l => l.BillID);

                foreach (var heldLock in heldLocks)
                {
                    try
                    {
                        var lockResponse = await _lockService.CheckLockStatusAsync(heldLock.BillID);

                        if (lockResponse?.IsSuccess == true &&
                            lockResponse.LockInfo?.LockedUserId == currentUserId &&
                            lockResponse.LockInfo?.SessionId == heldLock.SessionId)
                        {
                            healthyLocks.Add(heldLock.BillID);
                        }
                        else
                        {
                            unhealthyLocks.Add(heldLock.BillID);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"检查锁健康状态失败: 单据 {heldLock.BillID}");
                        unhealthyLocks.Add(heldLock.BillID);
                    }
                }

                return new LockHealthStatus
                {
                    TotalHeldLocks = heldLocks.Count,
                    HealthyLocks = healthyLocks.Count,
                    UnhealthyLocks = unhealthyLocks.Count,
                    OrphanedLocks = orphanedLocks.Count,
                    LastHeartbeat = _lastHeartbeat,
                    Status = unhealthyLocks.Count == 0 ? "健康" : "异常"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "检查锁健康状态时发生错误");
                return new LockHealthStatus { Status = "检查失败" };
            }
        }

        /// <summary>
        /// 获取恢复统计信息
        /// </summary>
        /// <returns>恢复统计</returns>
        public LockRecoveryStatistics GetRecoveryStatistics()
        {
            var heldLocks = GetHeldLocks();

            return new LockRecoveryStatistics
            {
                HeldLocksCount = heldLocks.Count,
                OrphanedLocksCount = heldLocks.Count(l => l.IsOrphaned),
                LastHeartbeat = _lastHeartbeat,
                HeartbeatInterval = TimeSpan.FromSeconds(30),
                RecoveryInterval = TimeSpan.FromMinutes(1)
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
                _heartbeatTimer?.Dispose();
                _lockRecoveryTimer?.Dispose();

                // 取消事件订阅
                AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;
                AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;

                _logger.LogInformation("锁恢复管理器已释放资源");
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