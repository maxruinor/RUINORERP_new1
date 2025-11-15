using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Global;
using RUINORERP.PacketSpec.Models.Lock;

namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// 高效的单据锁定信息管理系统
    /// 提供锁定信息的存储、查询、统计和监控功能
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
        
        // 日志记录器
        private readonly ILogger<LockInfoManager> _logger;
        
        // 配置参数
        private readonly LockInfoManagerConfig _config;
        
        // 后台清理任务
        private Timer _cleanupTimer;
        
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
            _logger = logger;
            _config = config ?? new LockInfoManagerConfig();
            
            // 启动后台清理任务
            _cleanupTimer = new Timer(
                CleanupExpiredLocks,
                null,
                TimeSpan.FromMinutes(5), // 5分钟后开始
                TimeSpan.FromMinutes(_config.CleanupIntervalMinutes) // 定期清理
            );
            
            _logger.LogInformation("锁定信息管理系统初始化完成");
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
                
                _logger.LogDebug("锁定信息添加成功: BillID={BillID}, UserID={UserID}, 耗时={ElapsedMs}ms", 
                    lockInfo.BillID, 
                    lockInfo.UserId, 
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
                
                _logger.LogDebug("锁定信息更新成功: BillID={BillID}, 耗时={ElapsedMs}ms", 
                    lockInfo.BillID, 
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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清理过期锁时发生异常");
            }
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
}