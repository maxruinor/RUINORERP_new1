using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Global;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Lock;
using RUINORERP.Server.Network.Models;

namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// 高效的单据锁定信息管理系统
    /// 提供锁定信息的存储、查询、统计和监控功能
    /// 扩展功能：处理器状态和会话状态管理
    /// </summary>
    public class LockInfoManager
    {
        // 锁定信息存储 - 按单据ID索引
        private readonly ConcurrentDictionary<long, LockInfo> _lockInfoByBillId;

        // 用户锁定信息索引 - 按用户ID分组
        private readonly ConcurrentDictionary<long, HashSet<long>> _lockInfoByUserId;

        // 业务类型锁定信息索引 - 按业务类型分组
        private readonly ConcurrentDictionary<BizType, HashSet<long>> _lockInfoByBizType;

        // 锁定状态索引 - 按状态分组
        private readonly ConcurrentDictionary<LockStatus, HashSet<long>> _lockInfoByStatus;

        // 锁定历史记录 - 用于审计和分析
        private readonly ConcurrentQueue<LockHistoryRecord> _lockHistory;

        // 锁定监控数据 - 实时性能监控
        private readonly LockMonitor _lockMonitor;

        // 处理器状态信息
        private readonly ConcurrentDictionary<string, HandlerStateInfo> _handlerStates;

        // 会话状态信息 - 使用现有的SessionInfo类
        private readonly ConcurrentDictionary<string, SessionInfo> _sessionStates;

        // 日志记录器
        private readonly ILogger<LockInfoManager> _logger;

        // 配置参数
        private readonly LockInfoManagerConfig _config;

        // 后台清理任务
        private Timer _cleanupTimer;

        // 状态清理定时器
        private Timer _stateCleanupTimer;

        // 历史记录最大容量
        private const int MaxHistoryRecords = 10000;

        /// <summary>
        /// 构造函数
        /// </summary>
        public LockInfoManager(ILogger<LockInfoManager> logger, LockInfoManagerConfig config = null)
        {
            _lockInfoByBillId = new ConcurrentDictionary<long, LockInfo>();
            _lockInfoByUserId = new ConcurrentDictionary<long, HashSet<long>>();
            _lockInfoByBizType = new ConcurrentDictionary<BizType, HashSet<long>>();
            _lockInfoByStatus = new ConcurrentDictionary<LockStatus, HashSet<long>>();
            _lockHistory = new ConcurrentQueue<LockHistoryRecord>();
            _lockMonitor = new LockMonitor();
            _handlerStates = new ConcurrentDictionary<string, HandlerStateInfo>();
            _sessionStates = new ConcurrentDictionary<string, SessionInfo>();
            _logger = logger;
            _config = config ?? new LockInfoManagerConfig();

            // 启动后台清理任务
            _cleanupTimer = new Timer(
                CleanupExpiredLocks,
                null,
                TimeSpan.FromMinutes(5), // 5分钟后开始
                TimeSpan.FromMinutes(_config.CleanupIntervalMinutes) // 定期清理
            );

            // 启动状态清理定时器
            _stateCleanupTimer = new Timer(
                CleanupExpiredStates,
                null,
                _config.StateCleanupInterval,
                _config.StateCleanupInterval
            );

            _logger.LogInformation("锁定信息管理系统初始化完成: 默认超时={DefaultTimeout}分钟, 清理间隔={CleanupInterval}分钟, 自动续期={AutoExtend}",
                _config.DefaultLockTimeoutMinutes,
                _config.CleanupIntervalMinutes,
                _config.EnableAutoExtend);
        }

        /// <summary>
        /// 添加锁定信息
        /// </summary>
        public bool AddLockInfo(LockInfo lockInfo)
        {
            if (lockInfo == null || lockInfo.BillID <= 0)
            {
                _logger.LogError("无效的锁定信息: BillID={BillID}", lockInfo?.BillID ?? 0);
                return false;
            }

            try
            {
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();

                // 设置默认过期时间（如果未设置）
                if (lockInfo.ExpireTime == default || lockInfo.ExpireTime <= DateTime.Now)
                {
                    lockInfo.ExpireTime = DateTime.Now.AddMinutes(_config.DefaultLockTimeoutMinutes);
                    _logger.LogDebug("设置默认锁定过期时间: BillID={BillID}, ExpireTime={ExpireTime}",
                        lockInfo.BillID, lockInfo.ExpireTime);
                }

                // 限制最大锁定时间
                var maxExpireTime = DateTime.Now.AddMinutes(_config.MaxLockTimeoutMinutes);
                if (lockInfo.ExpireTime > maxExpireTime)
                {
                    lockInfo.ExpireTime = maxExpireTime;
                    _logger.LogDebug("限制最大锁定时间: BillID={BillID}, MaxExpireTime={MaxExpireTime}",
                        lockInfo.BillID, maxExpireTime);
                }

                // 添加到主存储
                if (!_lockInfoByBillId.TryAdd(lockInfo.BillID, lockInfo))
                {
                    _logger.LogWarning("锁定信息已存在: BillID={BillID}", lockInfo.BillID);
                    return false;
                }

                // 更新索引
                UpdateIndexes(lockInfo, true);

                // 记录历史
                AddToHistory(lockInfo, LockHistoryAction.Created);

                // 更新监控数据
                _lockMonitor.RecordLockAdded();

                _logger.LogDebug("锁定信息添加成功: BillID={BillID}, UserID={UserID}, ExpireTime={ExpireTime}, 耗时={ElapsedMs}ms",
                    lockInfo.BillID,
                    lockInfo.UserId,
                    lockInfo.ExpireTime,
                    stopwatch.ElapsedMilliseconds);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "添加锁定信息失败: BillID={BillID}", lockInfo.BillID);
                return false;
            }
        }

        /// <summary>
        /// 更新锁定信息
        /// </summary>
        public bool UpdateLockInfo(LockInfo lockInfo)
        {
            if (lockInfo == null || lockInfo.BillID <= 0)
            {
                _logger.LogError("无效的锁定信息: BillID={BillID}", lockInfo?.BillID ?? 0);
                return false;
            }

            try
            {
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();

                // 获取原始锁定信息
                if (!_lockInfoByBillId.TryGetValue(lockInfo.BillID, out var originalLockInfo))
                {
                    _logger.LogWarning("锁定信息不存在: BillID={BillID}", lockInfo.BillID);
                    return false;
                }

                // 限制最大锁定时间
                var maxExpireTime = DateTime.Now.AddMinutes(_config.MaxLockTimeoutMinutes);
                if (lockInfo.ExpireTime > maxExpireTime)
                {
                    lockInfo.ExpireTime = maxExpireTime;
                    _logger.LogDebug("限制最大锁定时间: BillID={BillID}, MaxExpireTime={MaxExpireTime}",
                        lockInfo.BillID, maxExpireTime);
                }

                // 更新主存储
                _lockInfoByBillId[lockInfo.BillID] = lockInfo;

                // 如果状态发生变化，更新索引
                if (originalLockInfo.Status != lockInfo.Status)
                {
                    UpdateStatusIndex(originalLockInfo.Status, lockInfo.Status, lockInfo.BillID);
                }

                // 如果用户发生变化，更新索引
                if (originalLockInfo.UserId != lockInfo.UserId)
                {
                    UpdateUserIndex(originalLockInfo.UserId, lockInfo.UserId, lockInfo.BillID);
                }

                // 记录历史
                AddToHistory(lockInfo, LockHistoryAction.Updated);

                _logger.LogDebug("锁定信息更新成功: BillID={BillID}, ExpireTime={ExpireTime}, 耗时={ElapsedMs}ms",
                    lockInfo.BillID,
                    lockInfo.ExpireTime,
                    stopwatch.ElapsedMilliseconds);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新锁定信息失败: BillID={BillID}", lockInfo.BillID);
                return false;
            }
        }

        /// <summary>
        /// 移除锁定信息
        /// </summary>
        public bool RemoveLockInfo(long billId)
        {
            if (billId <= 0)
            {
                _logger.LogError("无效的单据ID: BillID={BillID}", billId);
                return false;
            }

            try
            {
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();

                // 从主存储移除
                if (!_lockInfoByBillId.TryRemove(billId, out var lockInfo))
                {
                    _logger.LogWarning("锁定信息不存在: BillID={BillID}", billId);
                    return false;
                }

                // 更新索引
                UpdateIndexes(lockInfo, false);

                // 记录历史
                AddToHistory(lockInfo, LockHistoryAction.Removed);

                // 更新监控数据
                _lockMonitor.RecordLockRemoved();

                _logger.LogDebug("锁定信息移除成功: BillID={BillID}, 耗时={ElapsedMs}ms",
                    billId,
                    stopwatch.ElapsedMilliseconds);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "移除锁定信息失败: BillID={BillID}", billId);
                return false;
            }
        }

        /// <summary>
        /// 根据单据ID获取锁定信息
        /// </summary>
        public LockInfo GetLockInfoByBillId(long billId)
        {
            if (billId <= 0)
            {
                return null;
            }

            _lockInfoByBillId.TryGetValue(billId, out var lockInfo);
            return lockInfo;
        }

        /// <summary>
        /// 根据用户ID获取锁定信息列表
        /// </summary>
        public List<LockInfo> GetLockInfoByUserId(long userId)
        {
            if (userId <= 0)
            {
                return new List<LockInfo>();
            }

            if (!_lockInfoByUserId.TryGetValue(userId, out var billIds))
            {
                return new List<LockInfo>();
            }

            var result = new List<LockInfo>();
            foreach (var billId in billIds)
            {
                if (_lockInfoByBillId.TryGetValue(billId, out var lockInfo))
                {
                    result.Add(lockInfo);
                }
            }

            return result;
        }

        /// <summary>
        /// 根据业务类型获取锁定信息列表
        /// </summary>
        public List<LockInfo> GetLockInfoByBizType(BizType bizType)
        {
            if (!_lockInfoByBizType.TryGetValue(bizType, out var billIds))
            {
                return new List<LockInfo>();
            }

            var result = new List<LockInfo>();
            foreach (var billId in billIds)
            {
                if (_lockInfoByBillId.TryGetValue(billId, out var lockInfo))
                {
                    result.Add(lockInfo);
                }
            }

            return result;
        }

        /// <summary>
        /// 根据状态获取锁定信息列表
        /// </summary>
        public List<LockInfo> GetLockInfoByStatus(LockStatus status)
        {
            // 对于Unlocked状态，直接基于IsLocked和过期时间计算
            if (status == LockStatus.Unlocked)
            {
                var currentTime = DateTime.Now;
                return _lockInfoByBillId.Values
                    .Where(l => !l.IsLocked || l.ExpireTime <= currentTime)
                    .ToList();
            }

            // 对于AboutToExpire状态，基于IsLocked和即将过期状态计算
            if (status == LockStatus.AboutToExpire)
            {
                return _lockInfoByBillId.Values
                    .Where(l => l.IsLocked && l.IsAboutToExpire())
                    .ToList();
            }

            // 对于其他状态，使用索引查询
            if (!_lockInfoByStatus.TryGetValue(status, out var billIds))
            {
                return new List<LockInfo>();
            }

            var result = new List<LockInfo>();
            foreach (var billId in billIds)
            {
                if (_lockInfoByBillId.TryGetValue(billId, out var lockInfo))
                {
                    result.Add(lockInfo);
                }
            }

            return result;
        }

        /// <summary>
        /// 获取所有锁定信息
        /// </summary>
        public List<LockInfo> GetAllLockInfo()
        {
            return _lockInfoByBillId.Values.ToList();
        }

        /// <summary>
        /// 获取过期锁定信息
        /// </summary>
        public List<LockInfo> GetExpiredLockInfo()
        {
            var currentTime = DateTime.Now;
            return _lockInfoByBillId.Values
                .Where(lockInfo => lockInfo.ExpireTime <= currentTime)
                .ToList();
        }

        /// <summary>
        /// 获取锁定统计信息
        /// </summary>
        public LockInfoStatistics GetStatistics()
        {
            var allLocks = _lockInfoByBillId.Values;
            var currentTime = DateTime.Now;

            return new LockInfoStatistics
            {
                TotalLocks = allLocks.Count,
                ActiveLocks = allLocks.Count(l => l.IsLocked && l.ExpireTime > currentTime),
                ExpiredLocks = allLocks.Count(l => l.ExpireTime <= currentTime),
                RequestingUnlock = allLocks.Count(l => l.Status == LockStatus.RequestingUnlock),
                LocksByUser = _lockInfoByUserId.Count,
                LocksByBizType = _lockInfoByBizType.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Count
                ),
                LocksByStatus = allLocks.GroupBy(l => l.Status).ToDictionary(
                    g => g.Key,
                    g => g.Count()
                ),
                HistoryRecordCount = _lockHistory.Count,
                MonitorData = _lockMonitor.GetSnapshot()
            };
        }

        /// <summary>
        /// 获取锁定历史记录
        /// </summary>
        public List<LockHistoryRecord> GetLockHistory(int maxRecords = 100)
        {
            return _lockHistory.Take(maxRecords).ToList();
        }

        /// <summary>
        /// 获取锁定监控数据
        /// </summary>
        public LockMonitorSnapshot GetMonitorData()
        {
            return _lockMonitor.GetSnapshot();
        }

        /// <summary>
        /// 记录锁定失败
        /// </summary>
        public void RecordFailedLock(long billId, long userId, string reason)
        {
            try
            {
                _logger.LogWarning("记录锁定失败: BillID={BillID}, UserID={UserID}, Reason={Reason}", billId, userId, reason);
                _lockMonitor.RecordLockRemoved(); // 记录为移除，因为锁定失败
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "记录锁定失败时发生异常: BillID={BillID}, UserID={UserID}", billId, userId);
            }
        }

        /// <summary>
        /// 记录锁定成功
        /// </summary>
        public void RecordSuccessfulLock(long billId, long userId)
        {
            try
            {
                _logger.LogDebug("记录锁定成功: BillID={BillID}, UserID={UserID}", billId, userId);
                _lockMonitor.RecordLockAdded(); // 记录为添加，因为锁定成功
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "记录锁定成功时发生异常: BillID={BillID}, UserID={UserID}", billId, userId);
            }
        }

        /// <summary>
        /// 记录解锁操作
        /// </summary>
        public void RecordUnlock(long billId, long userId, string reason)
        {
            try
            {
                _logger.LogInformation("记录解锁操作: BillID={BillID}, UserID={UserID}, Reason={Reason}", billId, userId, reason);

                // 从锁定信息中移除
                if (_lockInfoByBillId.TryRemove(billId, out var lockInfo))
                {
                    // 更新索引
                    UpdateIndexes(lockInfo, false);

                    // 记录历史
                    AddToHistory(lockInfo, LockHistoryAction.Removed);

                    // 更新监控数据
                    _lockMonitor.RecordLockRemoved();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "记录解锁操作时发生异常: BillID={BillID}, UserID={UserID}", billId, userId);
            }
        }

        /// <summary>
        /// 强制移除锁定
        /// </summary>
        public bool ForceRemoveLock(long billId)
        {
            try
            {
                _logger.LogInformation("强制移除锁定: BillID={BillID}", billId);

                // 从锁定信息中移除
                if (_lockInfoByBillId.TryRemove(billId, out var lockInfo))
                {
                    // 更新索引
                    UpdateIndexes(lockInfo, false);

                    // 记录历史
                    AddToHistory(lockInfo, LockHistoryAction.ForceUnlocked);

                    // 更新监控数据
                    _lockMonitor.RecordLockRemoved();

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "强制移除锁定时发生异常: BillID={BillID}", billId);
                return false;
            }
        }

        /// <summary>
        /// 记录解锁请求
        /// </summary>
        public void RecordUnlockRequest(long billId, long requestUserId, long lockedUserId, string reason)
        {
            try
            {
                _logger.LogInformation("记录解锁请求: BillID={BillID}, RequestUserID={RequestUserID}, LockedUserID={LockedUserID}, Reason={Reason}",
                    billId, requestUserId, lockedUserId, reason);

                // 更新锁定状态为请求解锁
                if (_lockInfoByBillId.TryGetValue(billId, out var lockInfo))
                {
                    lockInfo.Status = LockStatus.RequestingUnlock;
                    UpdateLockInfo(lockInfo);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "记录解锁请求时发生异常: BillID={BillID}, RequestUserID={RequestUserID}", billId, requestUserId);
            }
        }

        /// <summary>
        /// 记录拒绝解锁
        /// </summary>
        public void RecordUnlockRefuse(long billId, long refuseUserId, long requestUserId, string reason)
        {
            try
            {
                _logger.LogInformation("记录拒绝解锁: BillID={BillID}, RefuseUserID={RefuseUserID}, RequestUserID={RequestUserID}, Reason={Reason}",
                    billId, refuseUserId, requestUserId, reason);

                // 更新锁定状态为已锁定
                if (_lockInfoByBillId.TryGetValue(billId, out var lockInfo))
                {
                    lockInfo.Status = LockStatus.Locked;
                    UpdateLockInfo(lockInfo);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "记录拒绝解锁时发生异常: BillID={BillID}, RefuseUserID={RefuseUserID}", billId, refuseUserId);
            }
        }

        /// <summary>
        /// 更新索引
        /// </summary>
        private void UpdateIndexes(LockInfo lockInfo, bool isAdd)
        {
            // 更新用户索引
            UpdateUserIndex(lockInfo.UserId, isAdd ? lockInfo.UserId : 0, lockInfo.BillID);

            // 更新业务类型索引
            if (lockInfo.BillData?.BizType != null)
            {
                UpdateBizTypeIndex(lockInfo.BillData.BizType, isAdd, lockInfo.BillID);
            }

            // 更新状态索引
            UpdateStatusIndex(lockInfo.Status, isAdd ? lockInfo.Status : 0, lockInfo.BillID);
        }

        /// <summary>
        /// 更新用户索引
        /// </summary>
        private void UpdateUserIndex(long oldUserId, long newUserId, long billId)
        {
            if (oldUserId > 0)
            {
                if (_lockInfoByUserId.TryGetValue(oldUserId, out var oldUserLocks))
                {
                    lock (oldUserLocks)
                    {
                        oldUserLocks.Remove(billId);
                        if (oldUserLocks.Count == 0)
                        {
                            _lockInfoByUserId.TryRemove(oldUserId, out _);
                        }
                    }
                }
            }

            if (newUserId > 0)
            {
                var userLocks = _lockInfoByUserId.GetOrAdd(newUserId, _ => new HashSet<long>());
                lock (userLocks)
                {
                    userLocks.Add(billId);
                }
            }
        }

        /// <summary>
        /// 更新业务类型索引
        /// </summary>
        private void UpdateBizTypeIndex(BizType bizType, bool isAdd, long billId)
        {
            var bizTypeLocks = _lockInfoByBizType.GetOrAdd(bizType, _ => new HashSet<long>());
            lock (bizTypeLocks)
            {
                if (isAdd)
                {
                    bizTypeLocks.Add(billId);
                }
                else
                {
                    bizTypeLocks.Remove(billId);
                    if (bizTypeLocks.Count == 0)
                    {
                        _lockInfoByBizType.TryRemove(bizType, out _);
                    }
                }
            }
        }

        /// <summary>
        /// 更新状态索引
        /// </summary>
        private void UpdateStatusIndex(LockStatus oldStatus, LockStatus newStatus, long billId)
        {
            // 只有在状态真正改变时才更新索引
            if (oldStatus == newStatus)
                return;

            if (oldStatus != 0 && oldStatus != LockStatus.Unlocked)
            {
                if (_lockInfoByStatus.TryGetValue(oldStatus, out var oldStatusLocks))
                {
                    lock (oldStatusLocks)
                    {
                        oldStatusLocks.Remove(billId);
                        if (oldStatusLocks.Count == 0)
                        {
                            _lockInfoByStatus.TryRemove(oldStatus, out _);
                        }
                    }
                }
            }

            if (newStatus != 0 && newStatus != LockStatus.Unlocked)
            {
                var statusLocks = _lockInfoByStatus.GetOrAdd(newStatus, _ => new HashSet<long>());
                lock (statusLocks)
                {
                    statusLocks.Add(billId);
                }
            }
        }

        /// <summary>
        /// 添加到历史记录
        /// </summary>
        private void AddToHistory(LockInfo lockInfo, LockHistoryAction action)
        {
            if (lockInfo == null)
            {
                _logger.LogWarning("尝试添加空锁定信息到历史记录");
                return;
            }

            try
            {
                var historyRecord = new LockHistoryRecord
                {
                    BillId = lockInfo.BillID,
                    UserId = lockInfo.UserId,
                    UserName = lockInfo.UserName,
                    Action = action,
                    Status = lockInfo.Status,
                    Timestamp = DateTime.Now,
                    Details = $"锁定ID: {lockInfo.LockId}, 过期时间: {lockInfo.ExpireTime}"
                };

                _lockHistory.Enqueue(historyRecord);

                // 控制历史记录数量
                while (_lockHistory.Count > MaxHistoryRecords)
                {
                    _lockHistory.TryDequeue(out _);
                }

                _logger.LogDebug("历史记录添加成功: BillID={BillID}, Action={Action}", lockInfo.BillID, action);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "添加历史记录失败: BillID={BillID}, Action={Action}", lockInfo.BillID, action);
            }
        }

        /// <summary>
        /// 清理过期锁
        /// </summary>
        private void CleanupExpiredLocks(object state)
        {
            try
            {
                var currentTime = DateTime.Now;
                var expiredLocks = GetExpiredLockInfo();

                if (expiredLocks.Count > 0)
                {
                    _logger.LogInformation("开始清理过期锁: 数量={Count}", expiredLocks.Count);

                    int cleanedCount = 0;
                    foreach (var lockInfo in expiredLocks)
                    {
                        try
                        {
                            if (RemoveLockInfo(lockInfo.BillID))
                            {
                                cleanedCount++;
                                _lockMonitor.RecordLockExpired();
                                _logger.LogDebug("清理过期锁: BillID={BillID}, UserID={UserID}, ExpireTime={ExpireTime}",
                                    lockInfo.BillID,
                                    lockInfo.UserId,
                                    lockInfo.ExpireTime);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "清理单个过期锁失败: BillID={BillID}", lockInfo.BillID);
                        }
                    }

                    _logger.LogInformation("过期锁清理完成: 清理数量={CleanedCount}, 总数={TotalCount}", cleanedCount, expiredLocks.Count);
                }

                // 自动续期检查
                if (_config.EnableAutoExtend)
                {
                    AutoExtendLocks();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清理过期锁时发生异常");
            }
        }

        /// <summary>
        /// 自动续期锁定
        /// </summary>
        private void AutoExtendLocks()
        {
            try
            {
                var currentTime = DateTime.Now;
                var thresholdTime = currentTime.AddMinutes(_config.ExtendThresholdMinutes);

                var locksToExtend = _lockInfoByBillId.Values
                    .Where(lockInfo => lockInfo.Status == LockStatus.Locked &&
                                     lockInfo.ExpireTime <= thresholdTime &&
                                     lockInfo.ExpireTime > currentTime)
                    .ToList();

                if (locksToExtend.Count > 0)
                {
                    _logger.LogInformation("开始自动续期锁定: 数量={Count}", locksToExtend.Count);

                    int extendedCount = 0;
                    foreach (var lockInfo in locksToExtend)
                    {
                        try
                        {
                            var newExpireTime = currentTime.AddMinutes(_config.ExtendDurationMinutes);

                            // 限制最大锁定时间
                            var maxExpireTime = currentTime.AddMinutes(_config.MaxLockTimeoutMinutes);
                            if (newExpireTime > maxExpireTime)
                            {
                                newExpireTime = maxExpireTime;
                            }

                            lockInfo.ExpireTime = newExpireTime;
                            extendedCount++;

                            _logger.LogDebug("自动续期锁定: BillID={BillID}, UserID={UserID}, NewExpireTime={NewExpireTime}",
                                lockInfo.BillID,
                                lockInfo.UserId,
                                newExpireTime);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "自动续期单个锁定失败: BillID={BillID}", lockInfo.BillID);
                        }
                    }

                    _logger.LogInformation("自动续期完成: 续期数量={ExtendedCount}", extendedCount);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "自动续期锁定时发生异常");
            }
        }

        /// <summary>
        /// 手动续期锁定
        /// </summary>
        public bool ExtendLock(long billId, long userId, int extendMinutes = 15)
        {
            if (billId <= 0 || userId <= 0)
            {
                _logger.LogError("无效的单据ID或用户ID: BillID={BillID}, UserID={UserID}", billId, userId);
                return false;
            }

            try
            {
                if (!_lockInfoByBillId.TryGetValue(billId, out var lockInfo))
                {
                    _logger.LogWarning("锁定信息不存在: BillID={BillID}", billId);
                    return false;
                }

                // 验证用户权限
                if (lockInfo.UserId != userId)
                {
                    _logger.LogWarning("用户无权续期此锁定: BillID={BillID}, CurrentUserID={CurrentUserID}, LockUserID={LockUserID}",
                        billId, userId, lockInfo.UserId);
                    return false;
                }

                // 验证锁定状态
                if (lockInfo.Status != LockStatus.Locked)
                {
                    _logger.LogWarning("锁定状态不允许续期: BillID={BillID}, Status={Status}", billId, lockInfo.Status);
                    return false;
                }

                var currentTime = DateTime.Now;
                var newExpireTime = lockInfo.ExpireTime?.AddMinutes(extendMinutes) ?? currentTime.AddMinutes(extendMinutes);

                // 限制最大锁定时间
                var maxExpireTime = currentTime.AddMinutes(_config.MaxLockTimeoutMinutes);
                if (newExpireTime > maxExpireTime)
                {
                    newExpireTime = maxExpireTime;
                    _logger.LogDebug("限制最大锁定时间: BillID={BillID}, MaxExpireTime={MaxExpireTime}",
                        billId, maxExpireTime);
                }

                lockInfo.ExpireTime = newExpireTime;

                _logger.LogInformation("手动续期锁定成功: BillID={BillID}, UserID={UserID}, NewExpireTime={NewExpireTime}",
                    billId, userId, newExpireTime);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "手动续期锁定失败: BillID={BillID}, UserID={UserID}", billId, userId);
                return false;
            }
        }

        /// <summary>
        /// 更新处理器状态
        /// </summary>
        /// <param name="handler">命令处理器</param>
        /// <param name="status">状态</param>
        /// <param name="additionalInfo">附加信息</param>
        public void UpdateHandlerState(ICommandHandler handler, HandlerStatus status, string additionalInfo = null)
        {
            var stateInfo = new HandlerStateInfo
            {
                HandlerId = handler.HandlerId,
                HandlerName = handler.Name,
                Status = status,
                LastUpdated = DateTime.Now,
                AdditionalInfo = additionalInfo,
                Statistics = handler.GetStatistics()
            };

            _handlerStates[handler.HandlerId] = stateInfo;
        }

        /// <summary>
        /// 获取处理器状态
        /// </summary>
        /// <param name="handlerId">处理器ID</param>
        /// <returns>处理器状态信息</returns>
        public HandlerStateInfo GetHandlerState(string handlerId)
        {
            return _handlerStates.TryGetValue(handlerId, out var state) ? state : null;
        }

        /// <summary>
        /// 获取所有处理器状态
        /// </summary>
        /// <returns>处理器状态信息列表</returns>
        public List<HandlerStateInfo> GetAllHandlerStates()
        {
            return _handlerStates.Values.ToList();
        }

        /// <summary>
        /// 更新会话状态
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="sessionInfo">会话信息</param>
        public void UpdateSessionState(string sessionId, SessionInfo sessionInfo)
        {
            if (sessionInfo == null)
            {
                _logger.LogError("会话信息不能为空");
                return;
            }

            sessionInfo.LastActivityTime = DateTime.Now;
            _sessionStates[sessionId] = sessionInfo;
        }

        /// <summary>
        /// 获取会话状态
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <returns>会话状态信息</returns>
        public SessionInfo GetSessionState(string sessionId)
        {
            return _sessionStates.TryGetValue(sessionId, out var state) ? state : null;
        }

        /// <summary>
        /// 获取所有会话状态
        /// </summary>
        /// <returns>会话状态信息列表</returns>
        public List<SessionInfo> GetAllSessionStates()
        {
            return _sessionStates.Values.ToList();
        }

        /// <summary>
        /// 获取活跃会话数
        /// </summary>
        /// <returns>活跃会话数</returns>
        public int GetActiveSessionCount()
        {
            // 使用SessionInfo的LastActivityTime判断会话是否活跃（5分钟内有过活动）
            var threshold = DateTime.Now.AddMinutes(-5);
            return _sessionStates.Values.Count(s => s.LastActivityTime >= threshold);
        }

        /// <summary>
        /// 获取会话统计信息
        /// </summary>
        /// <returns>会话统计信息</returns>
        public SessionStatistics GetSessionStatistics()
        {
            var allSessions = _sessionStates.Values.ToList();
            var activeCount = GetActiveSessionCount();

            // 使用SessionInfo.cs中的SessionStatistics类
            return SessionStatistics.Create(allSessions.Count);
        }



        /// <summary>
        /// 清理过期状态
        /// </summary>
        /// <param name="state">状态对象</param>
        private void CleanupExpiredStates(object state)
        {
            var now = DateTime.Now;

            // 清理会话状态 - 使用SessionInfo的LastActivityTime
            var expiredSessions = _sessionStates.Where(kvp =>
                now - kvp.Value.LastActivityTime > _config.StateExpiration).ToList();

            foreach (var expired in expiredSessions)
            {
                _sessionStates.TryRemove(expired.Key, out _);
            }

            // 清理处理器状态（如果需要）
            // 处理器状态通常不需要清理，因为它们与处理器生命周期绑定
        }

        /// <summary>
        /// 获取系统状态报告
        /// </summary>
        /// <returns>系统状态报告</returns>
        public string GetSystemStateReport()
        {
            var report = $"=== 系统状态报告 ===\n";
            report += $"生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n\n";

            // 处理器状态
            report += "== 处理器状态 ==\n";
            foreach (var handlerState in _handlerStates.Values)
            {
                report += $"  {handlerState.HandlerName}: {handlerState.Status}\n";
                if (!string.IsNullOrEmpty(handlerState.AdditionalInfo))
                {
                    report += $"    附加信息: {handlerState.AdditionalInfo}\n";
                }
            }

            report += "\n";

            // 会话状态统计
            var sessionStats = GetSessionStatistics();
            report += "== 会话状态统计 ==\n";
            report += $"  总会话数: {sessionStats.TotalConnections}\n";
            report += $"  活跃会话数: {sessionStats.CurrentConnections}\n";
            report += $"  峰值会话数: {sessionStats.PeakConnections}\n";

            return report;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            try
            {
                _logger.LogInformation("开始释放锁定信息管理系统资源");

                // 停止清理定时器
                _cleanupTimer?.Dispose();
                _cleanupTimer = null;

                // 停止状态清理定时器
                _stateCleanupTimer?.Dispose();
                _stateCleanupTimer = null;

                // 记录最终统计信息
                var finalStats = GetStatistics();
                if (finalStats != null)
                {
                    _logger.LogInformation("锁定系统最终统计: 总锁定数={TotalLocks}, 活跃锁定数={ActiveLocks}, 历史记录数={HistoryCount}",
                        finalStats.TotalLocks,
                        finalStats.ActiveLocks,
                        finalStats.HistoryRecordCount);
                }

                _logger.LogInformation("锁定信息管理系统已释放");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "释放锁定信息管理系统资源时发生异常");
            }
        }
    }

    /// <summary>
    /// 锁定信息管理系统配置
    /// </summary>
    public class LockInfoManagerConfig
    {
        public int CleanupIntervalMinutes { get; set; } = 10; // 清理间隔时间（分钟）
        public int MaxHistoryRecords { get; set; } = 10000; // 最大历史记录数
        public bool EnableDetailedLogging { get; set; } = true; // 是否启用详细日志
        public int DefaultLockTimeoutMinutes { get; set; } = 30; // 默认锁定超时时间（分钟）
        public int MaxLockTimeoutMinutes { get; set; } = 120; // 最大锁定超时时间（分钟）
        public bool EnableAutoExtend { get; set; } = true; // 是否启用自动续期
        public int ExtendThresholdMinutes { get; set; } = 5; // 自动续期阈值（分钟）
        public int ExtendDurationMinutes { get; set; } = 15; // 自动续期时长（分钟）
        public TimeSpan StateCleanupInterval { get; set; } = TimeSpan.FromMinutes(5); // 状态清理间隔
        public TimeSpan StateExpiration { get; set; } = TimeSpan.FromHours(1); // 状态过期时间
    }

    /// <summary>
    /// 锁定历史记录
    /// </summary>
    public class LockHistoryRecord
    {
        public long BillId { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public LockHistoryAction Action { get; set; }
        public LockStatus Status { get; set; }
        public DateTime Timestamp { get; set; }
        public string Details { get; set; }
    }

    /// <summary>
    /// 锁定历史操作类型
    /// </summary>
    public enum LockHistoryAction
    {
        Created,
        Updated,
        Removed,
        Expired,
        ForceUnlocked
    }

    /// <summary>
    /// 锁定统计信息
    /// </summary>
    public class LockInfoStatistics
    {
        public int TotalLocks { get; set; }
        public int ActiveLocks { get; set; }
        public int ExpiredLocks { get; set; }
        public int RequestingUnlock { get; set; }
        public int LocksByUser { get; set; }
        public Dictionary<BizType, int> LocksByBizType { get; set; }
        public Dictionary<LockStatus, int> LocksByStatus { get; set; }
        public int HistoryRecordCount { get; set; }
        public LockMonitorSnapshot MonitorData { get; set; }

        /// <summary>
        /// 成功锁定次数
        /// </summary>
        public long SuccessfulLocks { get; set; }

        /// <summary>
        /// 失败锁定次数
        /// </summary>
        public long FailedLocks { get; set; }

        /// <summary>
        /// 等待锁定数
        /// </summary>
        public int WaitingLocks { get; set; }

        /// <summary>
        /// 平均锁定时间（毫秒）
        /// </summary>
        public double AverageLockTime { get; set; }
    }

    /// <summary>
    /// 锁定监控器
    /// </summary>
    public class LockMonitor
    {
        private long _totalLocksAdded;
        private long _totalLocksRemoved;
        private long _totalLocksExpired;
        private long _peakConcurrentLocks;
        private long _currentConcurrentLocks;
        private DateTime _lastResetTime;

        public LockMonitor()
        {
            _lastResetTime = DateTime.Now;
        }

        public void RecordLockAdded()
        {
            Interlocked.Increment(ref _totalLocksAdded);
            var current = Interlocked.Increment(ref _currentConcurrentLocks);

            // 更新峰值
            long peak;
            do
            {
                peak = _peakConcurrentLocks;
            } while (current > peak && Interlocked.CompareExchange(ref _peakConcurrentLocks, current, peak) != peak);
        }

        public void RecordLockRemoved()
        {
            Interlocked.Increment(ref _totalLocksRemoved);
            Interlocked.Decrement(ref _currentConcurrentLocks);
        }

        public void RecordLockExpired()
        {
            Interlocked.Increment(ref _totalLocksExpired);
        }

        public LockMonitorSnapshot GetSnapshot()
        {
            return new LockMonitorSnapshot
            {
                TotalLocksAdded = _totalLocksAdded,
                TotalLocksRemoved = _totalLocksRemoved,
                TotalLocksExpired = _totalLocksExpired,
                PeakConcurrentLocks = _peakConcurrentLocks,
                CurrentConcurrentLocks = _currentConcurrentLocks,
                LastResetTime = _lastResetTime
            };
        }
    }

    /// <summary>
    /// 锁定监控快照
    /// </summary>
    public class LockMonitorSnapshot
    {
        public long TotalLocksAdded { get; set; }
        public long TotalLocksRemoved { get; set; }
        public long TotalLocksExpired { get; set; }
        public long PeakConcurrentLocks { get; set; }
        public long CurrentConcurrentLocks { get; set; }
        public DateTime LastResetTime { get; set; }
    }

    /// <summary>
    /// 处理器状态信息
    /// </summary>
    public class HandlerStateInfo
    {
        /// <summary>
        /// 处理器ID
        /// </summary>
        public string HandlerId { get; set; }

        /// <summary>
        /// 处理器名称
        /// </summary>
        public string HandlerName { get; set; }

        /// <summary>
        /// 处理器状态
        /// </summary>
        public HandlerStatus Status { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// 附加信息
        /// </summary>
        public string AdditionalInfo { get; set; }

        /// <summary>
        /// 统计信息
        /// </summary>
        public HandlerStatistics Statistics { get; set; }
    }

    // 会话信息相关类已移至 SessionInfo.cs，使用现有的会话管理类
}