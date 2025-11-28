using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Models.Lock;
using RUINORERP.Server.Network.Models;
namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// 孤儿锁检测器
    /// 检测和清理因客户端异常断开而产生的孤儿锁
    /// </summary>
    public class OrphanedLockDetector : IDisposable
    {
        private readonly IntegratedServerLockManager _lockManager;
        private readonly ILogger<OrphanedLockDetector> _logger;
        private readonly Timer _detectionTimer;
        private readonly ConcurrentDictionary<string, ClientSessionInfo> _clientSessions;

        // 检测间隔和超时设置
        private readonly TimeSpan _detectionInterval = TimeSpan.FromMinutes(2);  // 每2分钟检测一次
        private readonly TimeSpan _sessionTimeout = TimeSpan.FromMinutes(5);      // 5分钟无心跳视为会话超时
        private readonly TimeSpan _lockMaxLifetime = TimeSpan.FromHours(8);       // 锁最大生命周期8小时

        /// <summary>
        /// 客户端会话信息
        /// </summary>
        private class ClientSessionInfo
        {
            private readonly TimeSpan _sessionTimeout;

            public string SessionId { get; set; }
            public long UserId { get; set; }
            public string UserName { get; set; }
            public string ClientAddress { get; set; }
            public DateTime LastHeartbeat { get; set; }
            public DateTime ConnectTime { get; set; }
            public bool IsActive => DateTime.Now <= LastHeartbeat.Add(_sessionTimeout);

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="sessionTimeout">会话超时时间</param>
            public ClientSessionInfo(TimeSpan sessionTimeout)
            {
                _sessionTimeout = sessionTimeout;
            }

            /// <summary>
            /// 更新心跳时间
            /// </summary>
            public void UpdateHeartbeat()
            {
                LastHeartbeat = DateTime.Now;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="lockManager">锁管理器</param>
        /// <param name="logger">日志记录器</param>
        public OrphanedLockDetector(IntegratedServerLockManager lockManager, ILogger<OrphanedLockDetector> logger)
        {
            _lockManager = lockManager ?? throw new ArgumentNullException(nameof(lockManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _clientSessions = new ConcurrentDictionary<string, ClientSessionInfo>();

            // 创建孤儿锁检测定时器，但不启动
            _detectionTimer = new Timer(DetectAndCleanupOrphanedLocks, null,
                Timeout.Infinite, Timeout.Infinite);

            _logger.LogInformation("孤儿锁检测器已初始化");
        }

        /// <summary>
        /// 启动孤儿锁检测器
        /// </summary>
        public async Task StartAsync()
        {
            try
            {
                // 启动检测定时器
                _detectionTimer.Change(_detectionInterval, _detectionInterval);

                _logger.LogInformation("孤儿锁检测器已启动");
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "启动孤儿锁检测器时发生异常");
                throw;
            }
        }

        /// <summary>
        /// 停止孤儿锁检测器
        /// </summary>
        public async Task StopAsync()
        {
            try
            {
                // 停止检测定时器
                _detectionTimer.Change(Timeout.Infinite, Timeout.Infinite);

                _logger.LogInformation("孤儿锁检测器已停止");
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "停止孤儿锁检测器时发生异常");
            }
        }

        /// <summary>
        /// 检测和清理孤儿锁（供外部调用）
        /// </summary>
        /// <param name="serverLockInfos">服务器锁信息列表</param>
        /// <returns>清理结果</returns>
        public async Task DetectAndCleanupAsync(List<LockInfo> serverLockInfos)
        {
            try
            {
                _logger.LogDebug("开始检测和清理孤儿锁");

                var orphanedCount = 0;
                var now = DateTime.Now;

                foreach (var lockInfo in serverLockInfos)
                {
                    // 检查锁是否过期
                    if (lockInfo.ExpireTime < now)
                    {
                        // 检查会话是否仍然活跃
                        if (!_clientSessions.ContainsKey(lockInfo.SessionId) ||
                            !_clientSessions[lockInfo.SessionId].IsActive)
                        {
                            // 释放孤儿锁
                            await _lockManager.ForceUnlockAsync($"lock:document:{lockInfo.BillID}", lockInfo.LockedUserId);
                            orphanedCount++;

                            _logger.LogWarning("清理孤儿锁: BillId={BillId}, UserId={UserId}, SessionId={SessionId}",
                                lockInfo.BillID, lockInfo.LockedUserId, lockInfo.SessionId);
                        }
                    }
                }

                if (orphanedCount > 0)
                {
                    _logger.LogInformation("清理了 {Count} 个孤儿锁", orphanedCount);
                }

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "检测和清理孤儿锁时发生异常");
            }
        }

        /// <summary>
        /// 获取孤儿锁数量
        /// </summary>
        /// <returns>孤儿锁数量</returns>
        public int GetOrphanedLockCount()
        {
            try
            {
                var orphanedCount = 0;
                var now = DateTime.Now;

                foreach (var session in _clientSessions.Values)
                {
                    if (!session.IsActive)
                    {
                        orphanedCount++;
                    }
                }

                return orphanedCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取孤儿锁数量时发生异常");
                return 0;
            }
        }

        /// <summary>
        /// 注册客户端会话
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="clientAddress">客户端地址</param>
        public void RegisterClientSession(string sessionId, long userId, string userName, string clientAddress = null)
        {
            try
            {
                var sessionInfo = new ClientSessionInfo(_sessionTimeout)
                {
                    SessionId = sessionId,
                    UserId = userId,
                    UserName = userName,
                    ClientAddress = clientAddress ?? "Unknown",
                    ConnectTime = DateTime.Now,
                    LastHeartbeat = DateTime.Now
                };

                _clientSessions.AddOrUpdate(sessionId, sessionInfo, (key, existing) => sessionInfo);

                _logger.LogDebug($"注册客户端会话: {sessionId}, 用户: {userName}, 地址: {clientAddress}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"注册客户端会话失败: {sessionId}");
            }
        }

        /// <summary>
        /// 更新客户端心跳
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <returns>是否更新成功</returns>
        public bool UpdateClientHeartbeat(string sessionId)
        {
            try
            {
                if (_clientSessions.TryGetValue(sessionId, out var sessionInfo))
                {
                    sessionInfo.UpdateHeartbeat();
                    _logger.LogDebug($"更新客户端心跳: {sessionId}, 最后心跳: {sessionInfo.LastHeartbeat}");
                    return true;
                }
                else
                {
                    _logger.LogWarning($"未找到客户端会话: {sessionId}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"更新客户端心跳失败: {sessionId}");
                return false;
            }
        }

        /// <summary>
        /// 注销客户端会话
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        public void UnregisterClientSession(string sessionId)
        {
            try
            {
                if (_clientSessions.TryRemove(sessionId, out var sessionInfo))
                {
                    _logger.LogInformation($"注销客户端会话: {sessionId}, 用户: {sessionInfo.UserName}, 连接时长: {DateTime.Now - sessionInfo.ConnectTime}");

                    // 释放该会话持有的所有锁
                    ReleaseSessionLocks(sessionInfo);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"注销客户端会话失败: {sessionId}");
            }
        }

        /// <summary>
        /// 检测和清理孤儿锁
        /// </summary>
        /// <param name="state">状态对象</param>
        private async void DetectAndCleanupOrphanedLocks(object state)
        {
            try
            {
                _logger.LogDebug("开始检测孤儿锁");

                // 1. 检测超时的客户端会话
                var timeoutSessions = _clientSessions.Values
                    .Where(session => !session.IsActive)
                    .ToList();

                foreach (var timeoutSession in timeoutSessions)
                {
                    _logger.LogWarning($"检测到超时会话: {timeoutSession.SessionId}, 用户: {timeoutSession.UserName}, 最后心跳: {timeoutSession.LastHeartbeat}");

                    // 从会话列表中移除
                    _clientSessions.TryRemove(timeoutSession.SessionId, out _);

                    // 释放该会话持有的所有锁
                    ReleaseSessionLocks(timeoutSession);
                }

                // 2. 检测长期有效的锁（可能因客户端异常未释放）
                var longLivedLocks = await DetectLongLivedLocksAsync();

                foreach (var longLivedLock in longLivedLocks)
                {
                    _logger.LogWarning($"检测到长期有效锁: 单据 {longLivedLock.BillID}, 用户: {longLivedLock.LockedUserName}, 锁定时长: {DateTime.Now - longLivedLock.LockTime}");

                    // 检查对应会话是否活跃
                    var sessionActive = _clientSessions.Values
                        .Any(session => session.SessionId == longLivedLock.SessionId && session.IsActive);

                    if (!sessionActive)
                    {
                        // 会话不活跃，释放锁
                        await _lockManager.ForceUnlockAsync($"lock:document:{longLivedLock.BillID}", longLivedLock.LockedUserId);
                        _logger.LogWarning($"释放孤儿锁: 单据 {longLivedLock.BillID}, 原因: 会话不活跃");
                    }
                }

                // 3. 清理过期的会话信息
                CleanupExpiredSessions();

                // 4. 记录检测统计信息
                LogDetectionStatistics();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "检测孤儿锁时发生错误");
            }
        }

        /// <summary>
        /// 释放会话持有的锁
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        private async void ReleaseSessionLocks(ClientSessionInfo sessionInfo)
        {
            try
            {
                // 获取该用户持有的所有锁
                var userLocks = await _lockManager.GetUserLocksAsync(sessionInfo.UserId);

                foreach (var userLock in userLocks)
                {
                    // 只释放属于该会话的锁
                    if (userLock.SessionId == sessionInfo.SessionId)
                    {
                        var unlocked = await _lockManager.UnlockAsync($"lock:document:{userLock.BillID}", sessionInfo.UserId);

                        if (unlocked.IsSuccess && unlocked.LockInfo.Status==LockStatus.Unlocked)
                        {
                            _logger.LogInformation($"释放会话锁成功: 单据 {userLock.BillID}, 会话 {sessionInfo.SessionId}, 用户: {sessionInfo.UserName}");
                        }
                        else
                        {
                            _logger.LogWarning($"释放会话锁失败: 单据 {userLock.BillID}, 会话 {sessionInfo.SessionId}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"释放会话锁失败: 会话 {sessionInfo.SessionId}");
            }
        }

        /// <summary>
        /// 检测长期有效的锁
        /// </summary>
        /// <returns>长期有效的锁列表</returns>
        private async Task<List<LockInfo>> DetectLongLivedLocksAsync()
        {
            try
            {
                var longLivedLocks = new List<LockInfo>();
                var cutoffTime = DateTime.Now.Subtract(_lockMaxLifetime);

                // 这里需要从SimplifiedServerLockManager获取所有锁信息
                // 由于SimplifiedServerLockManager没有直接提供这个方法，我们需要扩展它
                // 暂时返回空列表，在实际实施时需要修改SimplifiedServerLockManager

                return longLivedLocks;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "检测长期有效锁时发生错误");
                return new List<LockInfo>();
            }
        }

        /// <summary>
        /// 清理过期的会话信息
        /// </summary>
        private void CleanupExpiredSessions()
        {
            try
            {
                var cleanupThreshold = DateTime.Now.Subtract(TimeSpan.FromDays(1)); // 保留1天的会话记录
                var expiredSessions = _clientSessions.Values
                    .Where(session => session.LastHeartbeat < cleanupThreshold)
                    .Select(session => session.SessionId)
                    .ToList();

                foreach (var expiredSessionId in expiredSessions)
                {
                    _clientSessions.TryRemove(expiredSessionId, out _);
                }

                if (expiredSessions.Count > 0)
                {
                    _logger.LogDebug($"清理过期会话记录: {expiredSessions.Count} 个");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清理过期会话信息时发生错误");
            }
        }

        /// <summary>
        /// 记录检测统计信息
        /// </summary>
        private void LogDetectionStatistics()
        {
            try
            {
                var totalSessions = _clientSessions.Count;
                var activeSessions = _clientSessions.Values.Count(session => session.IsActive);
                var inactiveSessions = totalSessions - activeSessions;

                _logger.LogInformation($"孤儿锁检测统计 - 总会话: {totalSessions}, 活跃: {activeSessions}, 非活跃: {inactiveSessions}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "记录检测统计信息时发生错误");
            }
        }



        /// <summary>
        /// 手动触发孤儿锁检测
        /// </summary>
        /// <returns>检测结果</returns>
        public async Task<OrphanedLockDetectionResult> TriggerDetectionAsync()
        {
            try
            {
                _logger.LogInformation("手动触发孤儿锁检测");

                DetectAndCleanupOrphanedLocks(null);

                // 等待检测完成
                await Task.Delay(1000);


                return new OrphanedLockDetectionResult
                {
                    DetectionTime = DateTime.Now,
                    Success = true,
                    Message = "手动检测完成"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "手动触发孤儿锁检测时发生错误");
                return new OrphanedLockDetectionResult
                {
                    DetectionTime = DateTime.Now,
                    Success = false,
                    Message = $"检测失败: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            try
            {
                _detectionTimer?.Dispose();

                // 清理所有活跃会话
                foreach (var session in _clientSessions.Values)
                {
                    ReleaseSessionLocks(session);
                }

                _clientSessions.Clear();

                _logger.LogInformation("孤儿锁检测器已释放资源");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "释放孤儿锁检测器资源时发生错误");
            }
        }
    }


    /// <summary>
    /// 孤儿锁检测结果
    /// </summary>
    public class OrphanedLockDetectionResult
    {
        public DateTime DetectionTime { get; set; }
        public int CleanedSessions { get; set; }
        public int RemainingActiveSessions { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
