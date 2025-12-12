using Azure.Core;
using Microsoft.Extensions.Logging;
using RUINORERP.Global;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Lock;
using RUINORERP.PacketSpec.Models.Messaging;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.Network.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Services
{
    /// 集成式服务器锁管理器 v2.0.0
    /// 整合会话管理、心跳监控和分布式锁管理的完整服务器端解决方案
    /// 
    /// 版本：2.0.0
    /// 作者：AI Assistant  
    /// 创建时间：2025-01-27
    /// 
    /// 主要特性：
    /// - 无Redis依赖的内存分布式锁
    /// - 与心跳系统深度集成
    /// - 自动孤儿锁检测和清理
    /// - 简化的数据结构设计
    /// - 统一的版本控制
    /// </summary>
    public class ServerLockManager : ILockManagerService, IDisposable
    {

        #region 私有字段

        private readonly ISessionService _sessionService;
        private readonly ILogger<ServerLockManager> _logger;
        private readonly OrphanedLockDetector _orphanedLockDetector;

        // 简化的单一数据结构 - 按单据ID索引
        private readonly ConcurrentDictionary<long, LockInfo> _documentLocks;

        // 定时器
        private readonly Timer _cleanupTimer;
        private readonly Timer _maintenanceTimer;

        // 配置参数
        private readonly TimeSpan _cleanupInterval = TimeSpan.FromMinutes(2);
        private readonly TimeSpan _maintenanceInterval = TimeSpan.FromMinutes(10);
        private readonly TimeSpan _defaultLockTimeout = TimeSpan.FromMinutes(30);
        private readonly TimeSpan _maxLockTimeout = TimeSpan.FromHours(8);
        private readonly TimeSpan _sessionTimeout = TimeSpan.FromMinutes(5);
        private readonly TimeSpan _unlockRequestTimeout = TimeSpan.FromMinutes(10); // 解锁请求超时时间

        // 用于存储解锁请求的字典，键为单据ID，值为锁定请求信息
        private readonly ConcurrentDictionary<long, UnlockRequestInfo> _unlockRequests = new ConcurrentDictionary<long, UnlockRequestInfo>();

        private bool _isDisposed;
        private readonly object _lock = new object();

        #endregion

        #region 构造函数和初始化

        /// <summary>
        /// 广播锁定状态变化给所有客户端
        /// </summary>
        /// <param name="lockedDocument">锁定文档信息</param>
        public async Task BroadcastLockStatusAsync(LockInfo lockedDocument)
        {
            var lockedDocuments = new List<LockInfo> { lockedDocument };
            await BroadcastLockStatusAsync(lockedDocuments);
        }

        /// <summary>
        /// 广播锁定状态变化给所有客户端（与BroadcastLockStatusAsync相同功能）
        /// </summary>
        /// <param name="lockedDocuments">锁定文档信息列表</param>
        public async Task BroadcastLockStatusToAllClientsAsync(IEnumerable<LockInfo> lockedDocuments)
        {
            await BroadcastLockStatusAsync(lockedDocuments);
        }

        /// <summary>
        /// 广播锁定状态变化到所有客户端
        /// </summary>
        /// <param name="lockedDocuments">锁定的单据信息列表</param>
        public async Task BroadcastLockStatusAsync(IEnumerable<LockInfo> lockedDocuments, bool NeedReponse = false)
        {
            try
            {
                // 创建广播数据
                var broadcastData = new LockRequest
                {
                    LockedDocuments = lockedDocuments?.ToList() ?? new List<LockInfo>(),
                    Timestamp = DateTime.UtcNow
                };

                // 获取所有用户会话
                var sessions = _sessionService.GetAllUserSessions();

                // 向所有会话发送消息并等待响应
                int successCount = 0;
                foreach (var session in sessions)
                {
                    if (session is SessionInfo sessionInfo)
                    {
                        if (NeedReponse)
                        {
                            var responsePacket = await _sessionService.SendCommandAndWaitForResponseAsync(
                            session.SessionID,
                         LockCommands.BroadcastLockStatus,
                            broadcastData
                             );

                            if (responsePacket?.Response is MessageResponse response && response.IsSuccess)
                            {
                                successCount++;
                            }
                        }
                        else
                        {
                            var responsePacket = await _sessionService.SendCommandAsync(session.SessionID, LockCommands.BroadcastLockStatus, broadcastData);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"广播锁定状态变化到所有客户端时发生异常: {ex.Message}");
            }
        }


        /// <summary>
        /// 集成式服务器锁管理器构造函数
        /// </summary>
        /// <param name="sessionService">会话管理服务（系统已有）</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="broadcastService">广播服务</param>
        public ServerLockManager(
            ISessionService sessionService,
            ILogger<ServerLockManager> logger,
            IGeneralBroadcastService broadcastService)
        {
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // 初始化核心数据结构
            _documentLocks = new ConcurrentDictionary<long, LockInfo>();

            // 初始化孤儿锁检测器
            // 注意：这里通过LoggerFactory创建特定于OrphanedLockDetector的日志记录器
            // 由于OrphanedLockDetector是ServerLockManager的内部依赖，直接实例化是合理的
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var orphanedLogger = loggerFactory.CreateLogger<OrphanedLockDetector>();
            _orphanedLockDetector = new OrphanedLockDetector(this, orphanedLogger);

            // 初始化定时器
            _cleanupTimer = new Timer(CleanupCallback, null, Timeout.Infinite, Timeout.Infinite);
            _maintenanceTimer = new Timer(MaintenanceCallback, null, Timeout.Infinite, Timeout.Infinite);


        }

        /// <summary>
        /// 启动服务
        /// </summary>
        public async Task StartAsync()
        {
            try
            {
                lock (_lock)
                {
                    if (_isDisposed)
                        throw new ObjectDisposedException(nameof(ServerLockManager));

                    // 启动定时器
                    _cleanupTimer.Change(TimeSpan.FromMinutes(1), _cleanupInterval);
                    _maintenanceTimer.Change(TimeSpan.FromMinutes(2), _maintenanceInterval);

                    // 订阅会话服务的断开事件，实现会话断开时自动释放锁定
                    if (_sessionService != null)
                    {
                        _sessionService.SessionDisconnected += HandleSessionDisconnected;
                    }

                    // 启动孤儿锁检测器
                    _ = _orphanedLockDetector.StartAsync();
                }

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "启动集成式服务器锁管理器时发生异常");
                throw;
            }
        }

        /// <summary>
        /// 处理会话断开事件
        /// 当会话断开时，自动释放该会话名下的所有锁定
        /// </summary>
        /// <param name="sessionInfo">断开的会话信息</param>
        private void HandleSessionDisconnected(SessionInfo sessionInfo)
        {
            try
            {
                if (sessionInfo == null)
                {
                    return;
                }
                // 异步调用解锁方法，不阻塞事件处理
                _ = ReleaseAllLocksBySessionIdAsync(sessionInfo.SessionID);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理会话断开事件时发生异常");
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public async Task StopAsync()
        {
            try
            {
                lock (_lock)
                {
                    if (_isDisposed)
                        return;

                    // 停止定时器
                    _cleanupTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    _maintenanceTimer.Change(Timeout.Infinite, Timeout.Infinite);

                    // 取消订阅会话服务的断开事件
                    if (_sessionService != null)
                    {
                        _sessionService.SessionDisconnected -= HandleSessionDisconnected;
                    }

                    // 停止孤儿锁检测器
                    _orphanedLockDetector.StopAsync().GetAwaiter().GetResult();

                    // 清理所有锁
                    var lockIds = _documentLocks.Keys.ToList();
                    foreach (var lockId in lockIds)
                    {
                        _documentLocks.TryRemove(lockId, out _);
                    }
                }

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "停止集成式服务器锁管理器时发生异常");
            }
        }

        #endregion

        #region 核心锁管理功能



        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="request">解锁请求</param>
        /// <returns>解锁响应</returns>
        public async Task<LockResponse> ReleaseLockAsync(LockInfo lockInfo)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(ServerLockManager));

            if (lockInfo == null || lockInfo.BillID <= 0)
            {
                return CreateErrorResponse(lockInfo, "无效的锁信息");
            }

            try
            {
                if (_documentLocks.TryGetValue(lockInfo.BillID, out var existingLock))
                {
                    // 验证锁的所有者
                    if (existingLock.LockedUserId == lockInfo.LockedUserId &&
                        existingLock.SessionId == lockInfo.SessionId)
                    {
                        // 移除锁
                        if (_documentLocks.TryRemove(lockInfo.BillID, out _))
                        {
                            // 广播锁定状态更新
                            existingLock.IsLocked = false;
                            await BroadcastLockStatusAsync(existingLock);


                            return new LockResponse
                            {
                                IsSuccess = true,
                                Message = "解锁成功"
                            };
                        }
                    }
                    else
                    {
                        _logger.LogWarning("用户 {UserId} 尝试释放不属于自己的锁 {BillId}",
                            lockInfo.LockedUserId, lockInfo.BillID);
                        return CreateErrorResponse(lockInfo, "无权限释放此锁");
                    }
                }
                else
                {
                    _logger.LogDebug("尝试释放不存在的锁 {BillId}", lockInfo.BillID);
                    return CreateErrorResponse(lockInfo, "锁不存在");
                }

                return CreateErrorResponse(lockInfo, "解锁失败");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "释放锁 {BillId} 时发生异常", lockInfo.BillID);
                return CreateErrorResponse(lockInfo, $"解锁异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 检查锁状态
        /// </summary>
        /// <param name="request">锁状态查询请求</param>
        /// <returns>锁状态响应</returns>
        public async Task<LockResponse> CheckLockStatusAsync(LockRequest request)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(ServerLockManager));

            var lockInfo = request.LockInfo;
            if (lockInfo == null || lockInfo.BillID <= 0)
            {
                return CreateErrorResponse(lockInfo, "无效的锁信息");
            }

            try
            {
                if (_documentLocks.TryGetValue(lockInfo.BillID, out var existingLock))
                {
                    // 检查锁是否已过期
                    if (existingLock.IsExpired)
                    {
                        // 清理过期锁
                        _documentLocks.TryRemove(lockInfo.BillID, out _);
                        _logger.LogDebug("清理过期的锁 {BillId}", lockInfo.BillID);

                        return new LockResponse
                        {
                            IsSuccess = true,
                            Message = "锁已过期"
                        };
                    }

                    // 更新心跳信息
                    existingLock.LastHeartbeat = DateTime.Now;
                    existingLock.HeartbeatCount++;

                    return new LockResponse
                    {
                        IsSuccess = true,
                        Message = "锁状态查询成功",
                        LockInfo = existingLock
                    };
                }
                else
                {
                    return new LockResponse
                    {
                        IsSuccess = true,
                        Message = "单据未被锁定"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "检查锁状态 {BillId} 时发生异常", lockInfo.BillID);
                return CreateErrorResponse(lockInfo, $"查询异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 强制解锁（管理员功能）
        /// </summary>
        /// <param name="request">强制解锁请求</param>
        /// <returns>强制解锁响应</returns>
        public async Task<LockResponse> ForceUnlockAsync(LockRequest request)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(ServerLockManager));

            var lockInfo = request.LockInfo;
            if (lockInfo == null || lockInfo.BillID <= 0)
            {
                return CreateErrorResponse(lockInfo, "无效的锁信息");
            }

            try
            {
                if (_documentLocks.TryGetValue(lockInfo.BillID, out var existingLock))
                {
                    // 强制移除锁
                    if (_documentLocks.TryRemove(lockInfo.BillID, out _))
                    {
                        _logger.LogWarning("管理员强制释放单据 {BillId} 的锁，原锁主: {UserId}", lockInfo.BillID, existingLock.LockedUserId);

                        return new LockResponse
                        {
                            IsSuccess = true,
                            Message = "强制解锁成功"
                        };
                    }
                }

                return CreateErrorResponse(lockInfo, "锁不存在");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "强制解锁 {BillId} 时发生异常", lockInfo.BillID);
                return CreateErrorResponse(lockInfo, $"强制解锁异常: {ex.Message}");
            }
        }



        #endregion

        #region 会话锁定管理

        /// <summary>
        /// 按会话ID释放该会话名下的所有锁定
        /// 当客户端会话断开时调用此方法清理所有相关锁定
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <returns>释放的锁定数量</returns>
        public async Task<int> ReleaseAllLocksBySessionIdAsync(string sessionId)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(ServerLockManager));

            if (string.IsNullOrEmpty(sessionId))
            {
                return 0;
            }

            try
            {
                // 查找所有与会话ID关联的锁定
                var locksToRelease = _documentLocks
                    .Where(kvp => kvp.Value.SessionId == sessionId)
                    .Select(kvp => kvp.Value)
                    .ToList();

                if (locksToRelease.Count == 0)
                {
                    return 0;
                }

                int releasedCount = 0;
                foreach (var lockInfo in locksToRelease)
                {
                    // 移除锁定
                    if (_documentLocks.TryRemove(lockInfo.BillID, out _))
                    {
                        releasedCount++;
                        // 广播锁定状态更新
                        lockInfo.IsLocked = false;
                        await BroadcastLockStatusAsync(lockInfo);
                    }
                }

                return releasedCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "释放会话 {sessionId} 的锁定时发生异常", sessionId);
                return 0;
            }
        }

        #endregion

        #region 心跳集成

        /// <summary>
        /// 处理心跳中的锁信息
        /// 当客户端发送心跳时，更新对应锁的心跳时间
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="activeLocks">活跃锁列表</param>
        public async Task ProcessHeartbeatLocksAsync(string sessionId, long[] activeLocks)
        {
            if (_isDisposed || activeLocks == null || activeLocks.Length == 0)
                return;

            try
            {
                var now = DateTime.Now;
                var updatedCount = 0;

                foreach (var billId in activeLocks)
                {
                    if (_documentLocks.TryGetValue(billId, out var lockInfo))
                    {
                        // 验证会话匹配
                        if (lockInfo.SessionId == sessionId)
                        {
                            lockInfo.LastHeartbeat = now;
                            lockInfo.HeartbeatCount++;
                            updatedCount++;
                        }
                    }
                }

                if (updatedCount > 0)
                {
                    _logger.LogDebug("通过心跳更新了 {Count} 个锁的时间戳", updatedCount);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理心跳锁信息时发生异常");
            }

            await Task.CompletedTask;
        }

        #endregion

        #region 定时器回调

        /// <summary>
        /// 清理定时器回调
        /// 清理过期锁和孤儿锁
        /// </summary>
        private async void CleanupCallback(object state)
        {
            if (_isDisposed)
                return;

            try
            {
                await CleanupExpiredLocksAsync();
                await CleanupExpiredUnlockRequestsAsync(); // 添加清理过期解锁请求
                await _orphanedLockDetector.DetectAndCleanupAsync(_documentLocks.Values.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清理定时器回调中发生异常");
            }
        }

        /// <summary>
        /// 维护定时器回调
        /// 执行统计信息收集和性能监控
        /// </summary>
        private async void MaintenanceCallback(object state)
        {
            if (_isDisposed)
                return;

            try
            {
                await CleanupExpiredLocksAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "维护定时器回调中发生异常");
            }
        }

        /// <summary>
        /// 清理过期锁
        /// </summary>
        private async Task CleanupExpiredLocksAsync()
        {
            var now = DateTime.Now;
            var expiredLocks = new List<(long billId, LockInfo lockInfo)>();

            // 查找过期锁
            foreach (var kvp in _documentLocks)
            {
                if (kvp.Value.IsExpired)
                {
                    expiredLocks.Add((kvp.Key, kvp.Value));
                }
            }

            // 移除过期锁
            foreach (var (billId, lockInfo) in expiredLocks)
            {
                if (_documentLocks.TryRemove(billId, out _))
                {
                    // _logger.LogInformation("清理过期锁 {BillId}, 锁主: {UserId}, 锁定时间: {LockTime}",billId, lockInfo.UserId, lockInfo.LockTime);
                }
            }

            if (expiredLocks.Count > 0)
            {
                //  _logger.LogInformation("清理了 {Count} 个过期锁", expiredLocks.Count);
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// 清理过期的解锁请求
        /// </summary>
        private async Task CleanupExpiredUnlockRequestsAsync()
        {
            var now = DateTime.Now;
            var expiredRequests = new List<(long billId, UnlockRequestInfo requestInfo)>();

            // 查找过期的解锁请求
            foreach (var kvp in _unlockRequests)
            {
                if (kvp.Value.IsExpired)
                {
                    expiredRequests.Add((kvp.Key, kvp.Value));
                }
            }

            // 移除过期的解锁请求
            foreach (var (billId, requestInfo) in expiredRequests)
            {
                if (_unlockRequests.TryRemove(billId, out _))
                {
                    //                    _logger.LogDebug("清理过期解锁请求 {BillId}, 请求者: {RequesterId}, 请求时间: {RequestTime}",                        billId, requestInfo.Request.RequesterId, requestInfo.CreatedTime);
                }
            }

            if (expiredRequests.Count > 0)
            {
                _logger.LogDebug("清理了 {Count} 个过期的解锁请求", expiredRequests.Count);
            }

            await Task.CompletedTask;
        }


        #endregion

        #region 辅助方法

        /// <summary>
        /// 获取锁定单据的用户ID
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <returns>锁定该单据的用户ID，若单据未被锁定则返回0</returns>
        public long GetLockedUserId(long billId)
        {
            try
            {
                if (_documentLocks.TryGetValue(billId, out var lockInfo))
                {
                    if (lockInfo.IsLocked)
                    {
                        _logger.LogDebug("获取锁定用户ID: 单据ID={BillId}, 锁定用户ID={UserId}", billId, lockInfo.LockedUserId);
                        return lockInfo.LockedUserId;
                    }
                }
                _logger.LogDebug("获取锁定用户ID: 单据ID={BillId} 未被锁定", billId);
                return 0; // 返回0表示单据未被锁定或不存在
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取锁定用户ID时发生异常: 单据ID={BillId}", billId);
                return 0;
            }
        }

        /// <summary>
        /// 根据业务类型解锁用户的所有单据 - 优化版本
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="bizType">业务类型</param>
        /// <returns>异步任务，返回包含详细信息的LockResponse</returns>
        public async Task<LockResponse> UnlockDocumentsByBizNameAsync(long userId, int bizType)
        {
            try
            {
                // 统一参数验证
                if (userId <= 0)
                {
                    _logger.LogWarning("根据业务类型解锁单据参数无效: UserId={UserId}", userId);
                    return CreateErrorResponse(new LockInfo(), "用户ID无效");
                }

                _logger.LogDebug("开始根据业务类型解锁单据: 用户ID={UserId}, 业务类型={BizType}", userId, bizType);

                // 查找指定业务类型的所有单据（优化查询逻辑）
                var locksToUnlock = _documentLocks
                    .Where(kvp => kvp.Value.IsLocked &&
                                   kvp.Value.LockedUserId == userId &&
                                   kvp.Value.bizType == (BizType)bizType)
                    .Select(kvp => kvp.Key)
                    .ToList();

                int unlockedCount = 0;
                var unlockTasks = locksToUnlock.Select(async billId =>
                {
                    var result = await ExecuteUnlockAsync(billId, userId, false, "按业务类型解锁");
                    if (result.IsSuccess)
                        unlockedCount++;
                    return result;
                });

                // 并行执行解锁操作
                await Task.WhenAll(unlockTasks);

                _logger.LogDebug("根据业务类型解锁单据完成: 用户ID={UserId}, 业务类型={BizType}, 成功解锁={UnlockedCount}个单据",
                    userId, bizType, unlockedCount);

                return new LockResponse
                {
                    IsSuccess = true,
                    Message = $"成功解锁{unlockedCount}个{(BizType)bizType}类型的单据",
                    LockInfo = null // 批量操作不返回具体锁信息
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "根据业务类型解锁单据时发生异常: 用户ID={UserId}, 业务类型={BizType}", userId, bizType);
                return CreateErrorResponse(new LockInfo(), $"按业务类型解锁异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 检查用户是否有权限修改指定单据
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns>如果用户有权限修改单据则返回true，否则返回false</returns>
        public bool HasPermissionToModifyDocument(long billId, long userId)
        {
            try
            {
                // 如果单据未被锁定，任何人都可以修改
                if (!_documentLocks.TryGetValue(billId, out var lockInfo) || !lockInfo.IsLocked)
                {
                    _logger.LogDebug("权限检查: 单据ID={BillId} 未被锁定，用户ID={UserId} 可以修改", billId, userId);
                    return true;
                }

                // 如果单据被锁定，只有锁定该单据的用户可以修改
                bool hasPermission = lockInfo.LockedUserId == userId;
                _logger.LogDebug("权限检查: 单据ID={BillId}, 用户ID={UserId}, 锁定用户ID={LockedUserId}, 权限结果={HasPermission}",
                    billId, userId, lockInfo.LockedUserId, hasPermission);
                return hasPermission;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "权限检查时发生异常: 单据ID={BillId}, 用户ID={UserId}", billId, userId);
                return false;
            }
        }

        /// <summary>
        /// 获取锁定项数量
        /// </summary>
        /// <returns>当前系统中的锁定项数量</returns>
        public int GetLockItemCount()
        {
            try
            {

                // 统计当前锁定的单据数量
                int lockCount = _documentLocks.Count(c => c.Value.IsLocked);
                _logger.LogDebug("获取锁定项数量: 当前锁定项数={LockCount}", lockCount);
                return lockCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取锁定项数量时发生异常");
                return 0;
            }
        }

        /// <summary>
        /// 获取所有锁定单据
        /// </summary>
        /// <returns>所有锁定单据的锁定信息集合</returns>
        public List<LockInfo> GetAllLockedDocuments()
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                // 验证内部状态
                if (_documentLocks == null)
                {
                    _logger.LogError("内部锁字典未初始化");
                    return new List<LockInfo>();
                }

                // 创建锁定信息副本集合，避免直接引用
                var lockedDocuments = new List<LockInfo>();
                var expiredCount = 0;
                var invalidCount = 0;

                foreach (var kvp in _documentLocks)
                {
                    try
                    {
                        var lockInfo = kvp.Value;

                        // 验证锁信息对象
                        if (lockInfo == null)
                        {
                            _logger.LogWarning("发现空的锁信息对象: BillID={BillID}", kvp.Key);
                            invalidCount++;
                            continue;
                        }

                        // 验证关键属性
                        if (lockInfo.BillID <= 0)
                        {
                            _logger.LogWarning("发现无效的锁信息BillID: {BillID}", lockInfo.BillID);
                            invalidCount++;
                            continue;
                        }

                        // 只返回锁定的单据信息
                        if (!lockInfo.IsLocked)
                        {
                            continue;
                        }

                        // 检查是否过期
                        if (lockInfo.IsExpired)
                        {
                            expiredCount++;
                            _logger.LogDebug("跳过过期的锁信息: BillID={BillID}, BillNo={BillNo}, 过期时间={ExpireTime}",
                                lockInfo.BillID, lockInfo.BillNo, lockInfo.ExpireTime);
                            continue;
                        }

                        // 确保关键属性不为空
                        if (string.IsNullOrWhiteSpace(lockInfo.LockKey))
                        {
                            _logger.LogWarning("锁信息LockKey为空，尝试生成: BillID={BillID}", lockInfo.BillID);
                            lockInfo.SetLockKey();
                        }

                        if (string.IsNullOrWhiteSpace(lockInfo.LockedUserName))
                        {
                            _logger.LogWarning("锁信息LockedUserName为空: BillID={BillID}, UserID={UserID}",
                                lockInfo.BillID, lockInfo.LockedUserId);
                        }

                        // 创建锁信息的深拷贝，避免直接引用
                        var infoCopy = CreateLockInfoCopy(lockInfo);
                        lockedDocuments.Add(infoCopy);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "处理锁信息时出错: BillID={BillID}", kvp.Key);
                        invalidCount++;
                    }
                }

              

                return lockedDocuments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取所有锁定单据信息时发生异常，耗时: {ElapsedMs}ms", stopwatch.ElapsedMilliseconds);
                // 出错时返回空集合
                return new List<LockInfo>();
            }
            finally
            {
                stopwatch.Stop();
            }
        }

        /// <summary>
        /// 创建锁信息的深拷贝
        /// </summary>
        /// <param name="source">源锁信息</param>
        /// <returns>锁信息的副本</returns>
        private LockInfo CreateLockInfoCopy(LockInfo source)
        {
            if (source == null)
                return null;

            try
            {
                return new LockInfo
                {
                    // 复制所有可写属性
                    LockKey = source.LockKey,
                    BillID = source.BillID,
                    BillNo = source.BillNo,
                    LockedUserId = source.LockedUserId,
                    LockedUserName = source.LockedUserName,
                    LockTime = source.LockTime,
                    ExpireTime = source.ExpireTime,
                    Remark = source.Remark,
                    MenuID = source.MenuID,
                    BizName = source.BizName,
                    MenuName = source.MenuName,
                    bizType = source.bizType,
                    SessionId = source.SessionId,
                    IsLocked = source.IsLocked,
                    LastHeartbeat = source.LastHeartbeat,
                    LastUpdateTime = source.LastUpdateTime,
                    HeartbeatCount = source.HeartbeatCount,
                    Type = source.Type,
                    Duration = source.Duration
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建锁信息副本时出错: BillID={BillID}", source?.BillID ?? 0);
                throw;
            }
        }

        /// <summary>
        /// 获取指定单据的锁定信息
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <returns>锁定信息对象，如果未锁定则返回null</returns>
        public LockInfo GetLockInfo(long billId)
        {
            try
            {

                // 验证输入参数
                if (billId <= 0)
                {
                    _logger.LogWarning("获取锁定信息请求参数无效: BillID={BillId}", billId);
                    return null;
                }

                _logger.LogDebug("尝试获取单据锁定信息: BillID={BillId}", billId);

                // 从字典中查找锁信息
                if (_documentLocks.TryGetValue(billId, out var lockInfo))
                {
                    _logger.LogDebug("成功获取单据锁定信息: BillID={BillId}, UserId={UserId}, 锁定状态={IsLocked}, 过期状态={IsExpired}",
                        billId, lockInfo.LockedUserId, lockInfo.IsLocked, lockInfo.IsExpired);

                    // 返回锁定信息的副本，避免直接引用
                    return new LockInfo
                    {
                        // 复制所有可写属性
                        LockKey = lockInfo.LockKey,
                        BillID = lockInfo.BillID,
                        BillNo = lockInfo.BillNo,
                        LockedUserId = lockInfo.LockedUserId,
                        LockedUserName = lockInfo.LockedUserName,
                        LockTime = lockInfo.LockTime,
                        ExpireTime = lockInfo.ExpireTime,
                        Remark = lockInfo.Remark,
                        MenuID = lockInfo.MenuID,
                        BizName = lockInfo.BizName,
                        MenuName = lockInfo.MenuName,
                        bizType = lockInfo.bizType,
                        SessionId = lockInfo.SessionId,
                        IsLocked = lockInfo.IsLocked,
                        LastHeartbeat = lockInfo.LastHeartbeat,
                        LastUpdateTime = lockInfo.LastUpdateTime,
                        HeartbeatCount = lockInfo.HeartbeatCount,
                        Type = lockInfo.Type,
                        Duration = lockInfo.Duration
                    };
                }
                else
                {
                    _logger.LogDebug("单据未被锁定: BillID={BillId}", billId);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取单据锁定信息时发生异常: BillID={BillId}", billId);
                return null;
            }
        }


        public async Task<LockResponse> RequestUnlockDocumentAsync(LockRequest request)
        {
            try
            {

                // 检查单据是否被锁定
                if (!_documentLocks.TryGetValue(request.LockInfo.BillID, out var lockInfo) || !lockInfo.IsLocked)
                {
                    return CreateErrorResponse(request.LockInfo, "单据未被锁定，无需解锁请求");
                }

                // 检查是否已存在解锁请求
                if (_unlockRequests.ContainsKey(request.LockInfo.BillID))
                {
                    return CreateErrorResponse(request.LockInfo, "该单据已有待处理的解锁请求");
                }

                // 创建解锁请求信息，包含过期时间
                var unlockRequestInfo = new UnlockRequestInfo(request, _unlockRequestTimeout);

                // 存储解锁请求信息
                if (_unlockRequests.TryAdd(request.LockInfo.BillID, unlockRequestInfo))
                {
                    // 根据LockInfo中的信息发送请求到锁定的客户端
                    await SendUnlockRequestToLockingClientAsync(lockInfo, request);

                    return new LockResponse
                    {
                        IsSuccess = true,
                        Message = "解锁请求已发送，请等待锁定者响应",
                        LockInfo = request.LockInfo
                    };
                }

                return CreateErrorResponse(request.LockInfo, "解锁请求失败，请重试");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "请求解锁时发生异常: BillID={BillId}", request?.LockInfo?.BillID ?? 0);
                return CreateErrorResponse(new LockInfo { BillID = request?.LockInfo?.BillID ?? 0 },
                    $"请求解锁异常: {ex.Message}");
            }
        }


        public async Task<LockResponse> RefuseUnlockRequestAsync(LockRequest request)
        {
            try
            {
                if (request == null || request.LockInfo == null)
                {
                    _logger.LogWarning("拒绝解锁请求: 请求对象或锁定信息为空");
                    return CreateErrorResponse(new LockInfo { BillID = request?.LockInfo?.BillID ?? 0 }, "拒绝解锁请求参数无效");
                }

                // 检查是否存在解锁请求
                if (!_unlockRequests.TryGetValue(request.LockInfo.BillID, out var storedRequestInfo))
                {
                    return CreateErrorResponse(new LockInfo { BillID = request.LockInfo.BillID }, "该单据没有待处理的解锁请求");
                }

                // 获取存储的解锁请求
                var storedRequest = storedRequestInfo.Request;

                // 检查当前操作用户是否是锁定者本人
                // 获取当前单据的锁定信息
                if (!_documentLocks.TryGetValue(request.LockInfo.BillID, out var currentLock))
                {
                    return CreateErrorResponse(new LockInfo { BillID = request.LockInfo.BillID }, "单据未被锁定或不存在");
                }

                // 验证当前操作用户是否是锁定者本人
                if (currentLock.LockedUserId != request.LockInfo.LockedUserId)
                {
                    return CreateErrorResponse(new LockInfo { BillID = request.LockInfo.BillID }, "只有锁定者本人可以拒绝解锁请求");
                }

                // 移除解锁请求
                if (_unlockRequests.TryRemove(request.LockInfo.BillID, out _))
                {
                    // 通知请求者其解锁请求被拒绝，包含拒绝原因
                   
                    await NotifyUnlockRequestRefusedAsync(storedRequest.RequesterUserId, request.LockInfo.BillID, request.LockInfo.LockedUserId);

                    return new LockResponse
                    {
                        IsSuccess = true,
                        Message = "已拒绝解锁请求",
                        LockInfo = new LockInfo { BillID = request.LockInfo.BillID }
                    };
                }

                _logger.LogWarning("拒绝解锁请求失败: 无法移除解锁请求: 单据ID={BillId}", request.LockInfo.BillID);
                return CreateErrorResponse(new LockInfo { BillID = request.LockInfo.BillID }, "拒绝解锁请求失败，请重试");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "拒绝解锁请求时发生异常: 单据ID={BillId}", request?.LockInfo.BillID ?? 0);
                return CreateErrorResponse(new LockInfo { BillID = request?.LockInfo?.BillID ?? 0 }, $"拒绝解锁请求异常: {ex.Message}");
            }
        }



        /// <summary>
        /// 通用解锁验证方法 - 统一解锁前的验证逻辑
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="forceUnlock">是否强制解锁</param>
        /// <returns>验证结果</returns>
        private (bool IsValid, LockInfo LockInfo, string ErrorMessage) ValidateUnlockRequest(long billId, long userId, bool forceUnlock = false)
        {
            // 参数验证
            if (billId <= 0)
                return (false, null, "单据ID无效");

            if (userId <= 0 && !forceUnlock)
                return (false, null, "用户ID无效");

            // 检查单据是否存在并被锁定
            if (!_documentLocks.TryGetValue(billId, out var lockInfo) || !lockInfo.IsLocked)
                return (false, null, "单据未被锁定或不存在");

            // 非强制模式下验证权限
            if (!forceUnlock && lockInfo.LockedUserId != userId)
                return (false, lockInfo, "无权限释放此锁");

            return (true, lockInfo, null);
        }



        /// <summary>
        /// 通用解锁执行方法 - 统一解锁执行逻辑
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="userId">操作用户ID</param>
        /// <param name="forceUnlock">是否强制解锁</param>
        /// <param name="operationType">操作类型描述</param>
        /// <returns>解锁响应</returns>
        private async Task<LockResponse> ExecuteUnlockAsync(long billId, long userId, bool forceUnlock, string operationType)
        {
            try
            {
                var validation = ValidateUnlockRequest(billId, userId, forceUnlock);
                if (!validation.IsValid)
                {
                    return CreateErrorResponse(new LockInfo { BillID = billId }, validation.ErrorMessage);
                }

                // 记录原锁定用户信息（用于强制解锁的审计）
                long originalUserId = validation.LockInfo.LockedUserId;

                if (forceUnlock)
                {
                    _logger.LogWarning("强制解锁: 单据ID={BillId}, 原锁定用户ID={OriginalUserId}, 操作用户ID={UserId}",
                        billId, originalUserId, userId);
                }

                // 执行解锁操作
                if (_documentLocks.TryRemove(billId, out _))
                {
                    var message = forceUnlock ? "强制解锁成功" : "解锁成功";

                    if (forceUnlock)
                    {
                     //   _logger.LogInformation("{OperationType}完成: 单据ID={BillId}", operationType, billId);
                    }
                    validation.LockInfo.IsLocked = false;

                    // 广播锁定状态更新
                    await BroadcastLockStatusAsync(validation.LockInfo);

                    return new LockResponse
                    {
                        IsSuccess = true,
                        Message = message,
                        LockInfo = validation.LockInfo
                    };
                }

                return CreateErrorResponse(validation.LockInfo, $"{operationType}失败，无法移除锁");
            }
            catch (Exception ex)
            {
                string msg = $"{operationType}时发生异常: 单据ID={billId}";
                _logger.LogError(ex, msg);
                return LockResponseFactory.CreateFailedResponse(ex.Message + msg);
            }
        }



        /// <summary>
        /// 强制解锁指定单据 - 重构后使用通用解锁方法
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <returns>解锁响应结果</returns>
        public async Task<LockResponse> ForceUnlockDocumentAsync(long billId)
        {
            // 强制解锁不需要用户ID验证，传入0作为占位符
            return await ExecuteUnlockAsync(billId, 0, true, "强制解锁");
        }

        /// <summary>
        /// 尝试锁定单据 (ILockManagerService接口方法)
        /// </summary>
        /// <param name="lockInfo">锁定信息</param>
        /// <returns>锁定结果，包含成功状态和详细信息</returns>
        public async Task<LockResponse> TryLockDocumentAsync(LockInfo lockInfo)
        {
            if (lockInfo == null || lockInfo.BillID <= 0)
            {
                return CreateErrorResponse(lockInfo, "锁定信息无效或单据ID错误");
            }

            try
            {
                // 检查是否已被锁定
                if (_documentLocks.TryGetValue(lockInfo.BillID, out var existingLock))
                {
                    // 检查锁是否已过期
                    if (!existingLock.IsExpired)
                    {
                        // 锁仍然有效
                        _logger.LogDebug("单据 {BillId} 已被用户 {UserId} 在时间 {LockTime} 锁定",
                            lockInfo.BillID, existingLock.LockedUserId, existingLock.LockTime);

                        return new LockResponse
                        {
                            IsSuccess = false,
                            Message = $"单据已被用户 {existingLock.LockedUserName} (ID: {existingLock.LockedUserId}) 锁定",
                            LockInfo = existingLock
                        };
                    }
                    else
                    {
                        // 锁已过期，清理
                        _documentLocks.TryRemove(lockInfo.BillID, out _);
                        _logger.LogDebug("清理过期的锁 {BillId}", lockInfo.BillID);
                    }
                }

                // 创建新锁
                var serverLockInfo = new LockInfo
                {
                    LockKey = lockInfo.LockKey,
                    BillID = lockInfo.BillID,
                    BillNo = lockInfo.BillNo,
                    LockedUserId = lockInfo.LockedUserId,
                    LockedUserName = lockInfo.LockedUserName,
                    LockTime = DateTime.Now, // 使用当前时间作为锁定时间
                    ExpireTime = lockInfo.ExpireTime,
                    Remark = lockInfo.Remark,
                    MenuID = lockInfo.MenuID,
                    BizName = lockInfo.BizName,
                    MenuName = lockInfo.MenuName,
                    bizType = lockInfo.bizType,
                    SessionId = lockInfo.SessionId,
                    IsLocked = true,
                    LastHeartbeat = DateTime.Now, // 使用当前时间作为最后心跳时间
                    LastUpdateTime = DateTime.Now, // 使用当前时间作为最后更新时间
                    HeartbeatCount = 1, // 重置心跳次数为1
                    Type = lockInfo.Type, // 使用传入的锁定类型
                    Duration = lockInfo.Duration
                };

                // 添加到锁集合
                if (_documentLocks.TryAdd(lockInfo.BillID, serverLockInfo))
                {
                    // 广播锁定状态更新
                    await BroadcastLockStatusAsync(serverLockInfo);

                    LockResponse lockResponse = new LockResponse();
                    lockResponse.IsSuccess = true;
                    lockResponse.Message = "锁定成功";
                    lockResponse.LockInfo = serverLockInfo;

                    return lockResponse;
                }
                else
                {
                    // 并发情况下可能被其他线程锁定
                    return CreateErrorResponse(lockInfo, "锁定失败，请重试");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取锁 {BillId} 时发生异常", lockInfo.BillID);
                return CreateErrorResponse(lockInfo, $"锁定异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 解锁指定单据 - 重构后使用通用解锁方法
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns>解锁响应结果</returns>
        public async Task<LockResponse> UnlockDocumentAsync(long billId, long userId)
        {
            return await ExecuteUnlockAsync(billId, userId, false, "解锁");
        }

        /// <summary>
        /// 获取指定单据的锁定信息 (ILockManagerService接口方法)
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <returns>锁定信息，如果未锁定则返回null</returns>
        LockInfo ILockManagerService.GetLockInfo(long billId) => GetLockInfo(billId);

        /// <summary>
        /// 获取所有锁定的单据信息 (ILockManagerService接口方法)
        /// </summary>
        /// <returns>锁定信息列表</returns>
        List<LockInfo> ILockManagerService.GetAllLockedDocuments() => GetAllLockedDocuments();

        /// <summary>
        /// 根据业务类型解锁单据 (ILockManagerService接口方法)
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="billType">单据类型</param>
        /// <returns>解锁结果，包含成功状态和详细信息</returns>
        async Task<LockResponse> ILockManagerService.UnlockDocumentsByBizNameAsync(long userId, int billType)
            => await UnlockDocumentsByBizNameAsync(userId, billType);

        /// <summary>
        /// 强制解锁单据（管理员操作）(ILockManagerService接口方法)
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <returns>解锁结果，包含成功状态和详细信息</returns>
        async Task<LockResponse> ILockManagerService.ForceUnlockDocumentAsync(long billId)
        {
            // 强制解锁不需要用户ID验证，传入0作为占位符
            return await ExecuteUnlockAsync(billId, 0, true, "强制解锁");
        }



        /// <summary>
        /// 发送解锁请求到锁定客户端
        /// </summary>
        /// <param name="lockInfo">锁定信息</param>
        /// <param name="request">解锁请求</param>
        /// <returns>发送任务</returns>
        private async Task SendUnlockRequestToLockingClientAsync(LockInfo lockInfo, LockRequest request)
        {
            try
            {
                // 获取锁定者的会话信息
                var lockingSession = _sessionService.GetSession(lockInfo.SessionId);
                if (lockingSession == null)
                {
                    _logger.LogWarning("无法找到锁定者的会话: SessionID={SessionID}, BillID={BillID}",
                        lockInfo.SessionId, lockInfo.BillID);
                    return;
                }

                // 创建解锁请求通知
                var unlockNotification = new LockRequest
                {
                    LockInfo = lockInfo,
                    RequesterUserId = request.RequesterUserId,
                    RequesterUserName = request.RequesterUserName,
                    Timestamp = DateTime.UtcNow
                };


                // 发送解锁请求到锁定客户端
                await _sessionService.SendCommandAsync(
                    lockingSession.SessionID,
                    LockCommands.RequestUnlock,
                    unlockNotification);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "发送解锁请求到锁定客户端时发生异常: BillID={BillID}, SessionID={SessionID}",
                    lockInfo.BillID, lockInfo.SessionId);
            }
        }

        /// <summary>
        /// 通知请求者其解锁请求被拒绝
        /// </summary>
        /// <param name="requesterUserId">请求解锁的用户ID</param>
        /// <param name="billId">单据ID</param>
        /// <param name="refusedByUserId">拒绝请求的用户ID（锁定者）</param>
        /// <returns>通知任务</returns>
        private async Task NotifyUnlockRequestRefusedAsync(long requesterUserId, long billId, long refusedByUserId )
        {
            try
            {
                // 获取请求者的所有会话
                var requesterSession = _sessionService.GetSession(requesterUserId);
                if (requesterSession == null)
                {
                    _logger.LogWarning("无法找到请求者的会话: RequesterUserId={RequesterUserId}, BillID={BillID}",
                        requesterUserId, billId);
                    return;
                }

                // 获取拒绝者的用户名（如果可能）
                string refusedByUserName = "未知用户";
                if (_documentLocks.TryGetValue(billId, out var lockInfo) && lockInfo.LockedUserId == refusedByUserId)
                {
                    refusedByUserName = lockInfo.LockedUserName;
                }

                // 创建拒绝通知，包含拒绝原因
                var refuseNotification = new LockRequest
                {
                    LockInfo = new LockInfo
                    {
                        BillID = billId,
                        BillNo= lockInfo?.BillNo,
                        BizName= lockInfo?.BizName,
                        bizType= lockInfo.bizType,
                        LockedUserId = refusedByUserId,
                        LockedUserName = refusedByUserName,
                        IsLocked = true // 单据仍然被锁定
                    },
                    RequesterUserId = requesterUserId,
                    Timestamp = DateTime.UtcNow,
                };

                // 向请求者的所有会话发送拒绝通知
                await _sessionService.SendCommandAsync(
                    requesterSession.SessionID,
                    LockCommands.RefuseUnlock,
                    refuseNotification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "通知请求者解锁请求被拒绝时发生异常: RequesterUserId={RequesterUserId}, BillID={BillID}",
                    requesterUserId, billId);
            }
        }

        /// <summary>
        /// 拒绝解锁请求 (ILockManagerService接口方法)
        /// </summary>
        /// <param name="refuseInfo">拒绝信息</param>
        /// <returns>拒绝结果，包含成功状态和详细信息</returns>
        Task<LockResponse> ILockManagerService.RefuseUnlockRequestAsync(LockRequest refuseInfo) => RefuseUnlockRequestAsync(refuseInfo);



        /// <summary>
        /// 同意解锁请求
        /// </summary>
        /// <param name="agreeInfo">同意信息</param>
        /// <returns>同意结果，包含成功状态和详细信息</returns>
        public async Task<LockResponse> AgreeUnlockRequestAsync(LockRequest agreeInfo)
        {
            // 使用try-finally确保请求被移除
            bool shouldRemoveRequest = false;
            long billId = 0;

            try
            {
                if (agreeInfo == null || agreeInfo.LockInfo?.BillID <= 0)
                {
                    _logger.LogWarning("同意解锁请求参数无效: BillID={BillId}", agreeInfo?.LockInfo?.BillID ?? 0);
                    return CreateErrorResponse(new LockInfo { BillID = agreeInfo?.LockInfo?.BillID ?? 0 }, "同意解锁请求参数无效");
                }

                billId = agreeInfo.LockInfo.BillID;
                _logger.LogDebug("处理同意解锁请求: 单据ID={BillId}, 同意者ID={LockedUserId}", billId, agreeInfo.LockInfo.LockedUserId);

                // 检查是否存在解锁请求
                if (!_unlockRequests.TryGetValue(billId, out var storedRequestInfo))
                {
                    return CreateErrorResponse(agreeInfo.LockInfo, "该单据没有待处理的解锁请求");
                }

                // 标记需要移除请求
                shouldRemoveRequest = true;

                // 获取存储的解锁请求
                var storedRequest = storedRequestInfo.Request;
                var lockInfo = storedRequestInfo.Request.LockInfo;

                // 修正逻辑：检查同意者是否是锁定者本人
                if (agreeInfo.LockInfo.LockedUserId != lockInfo.LockedUserId)
                {
                    _logger.LogWarning("同意解锁请求失败: 用户ID={UserId} 不是单据ID={BillId} 的锁定者，无权同意请求",
                        agreeInfo.LockInfo.LockedUserId, billId);
                    return CreateErrorResponse(agreeInfo.LockInfo, "只有锁定者本人可以同意解锁请求");
                }

                // 解锁单据
                var unlockResult = await UnlockDocumentAsync(billId, agreeInfo.LockInfo.LockedUserId);

                return unlockResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理同意解锁请求时发生异常: BillID={BillId}", billId);
                return CreateErrorResponse(new LockInfo { BillID = billId }, $"同意解锁请求异常: {ex.Message}");
            }
            finally
            {
                // 确保在所有情况下都移除请求
                if (shouldRemoveRequest && billId > 0)
                {
                    _unlockRequests.TryRemove(billId, out _);
                    _logger.LogDebug("已移除解锁请求: 单据ID={BillId}", billId);
                }
            }
        }

        /// <summary>
        /// 检查用户是否有权限修改单据 (ILockManagerService接口方法)
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns>是否有权限</returns>
        bool ILockManagerService.HasPermissionToModifyDocument(long billId, long userId)
            => HasPermissionToModifyDocument(billId, userId);

        /// <summary>
        /// 获取锁定单据的用户ID (ILockManagerService接口方法)
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <returns>锁定用户ID，如果未锁定则返回0</returns>
        long ILockManagerService.GetLockedUserId(long billId)
            => GetLockedUserId(billId);

        /// <summary>
        /// 获取锁定项数量 (ILockManagerService接口方法)
        /// </summary>
        /// <returns>锁定项数量</returns>
        int ILockManagerService.GetLockItemCount()
            => GetLockItemCount();

        /// <summary>
        /// 获取锁定统计信息 (ILockManagerService接口方法)
        /// </summary>
        /// <returns>锁定统计信息</returns>
        LockInfoStatistics ILockManagerService.GetLockStatistics()
            => GetLockStatistics();



        /// <summary>
        /// 获取锁统计信息
        /// </summary>
        /// <returns>锁统计信息对象</returns>
        public LockInfoStatistics GetLockStatistics()
        {
            try
            {
                var now = DateTime.Now;
                var lockInfos = _documentLocks.Values.ToList();

                // 计算总锁数
                int totalLockCount = lockInfos.Count;

                // 计算活跃锁数
                int activeLockCount = lockInfos.Count(c => !c.IsExpired);

                // 计算过期锁数
                int expiredLockCount = lockInfos.Count(c => c.IsExpired);

                // 计算锁定的用户数（去重）
                int uniqueUsersCount = lockInfos.Select(c => c.LockedUserId).Distinct().Count();

                // 计算平均锁定年龄（分钟）
                double avgLockAge = 0;
                if (totalLockCount > 0)
                {
                    var totalMinutes = lockInfos.Sum(c => (now - c.LockTime).TotalMinutes);
                    avgLockAge = Math.Round(totalMinutes / totalLockCount, 2);
                }

                // 计算待处理的解锁请求数量
                int requestingUnlockCount = _unlockRequests.Count(kvp => !kvp.Value.IsExpired);

                // 创建统计对象
                var stats = new LockInfoStatistics
                {
                    TotalLocks = totalLockCount,
                    ActiveLocks = activeLockCount,
                    ExpiredLocks = expiredLockCount,
                    LocksByUser = uniqueUsersCount,
                    RequestingUnlock = requestingUnlockCount, // 更新待处理解锁请求数量

                    AverageLockAge = avgLockAge,
                    HeartbeatEnabled = true,
                    LocksByBizType = new Dictionary<BizType, int>(),
                };

                // 统计按状态分布
                foreach (var lockInfo in lockInfos)
                {

                    // 统计业务类型（如果有）
                    if (lockInfo.bizType != null && lockInfo.bizType != 0)
                    {
                        if (stats.LocksByBizType.ContainsKey(lockInfo.bizType))
                            stats.LocksByBizType[lockInfo.bizType]++;
                        else
                            stats.LocksByBizType[lockInfo.bizType] = 1;
                    }
                }

                _logger.LogDebug("获取锁统计信息成功: 总锁数={Total}, 活跃={Active}, 过期={Expired}",
                    totalLockCount, activeLockCount, expiredLockCount);

                return stats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取锁统计信息时发生异常");

                // 返回基本统计信息
                return new LockInfoStatistics
                {
                    TotalLocks = 0,
                    ActiveLocks = 0,
                    ExpiredLocks = 0,
                    LocksByUser = 0,
                    RequestingUnlock = 0,

                    AverageLockAge = 0,
                    HeartbeatEnabled = true,
                    LocksByBizType = new Dictionary<BizType, int>(),
                };
            }
        }

        /// <summary>
        /// 创建错误响应 - 使用统一工厂
        /// </summary>
        /// <param name="lockInfo">锁信息</param>
        /// <param name="message">错误消息</param>
        /// <returns>错误响应</returns>
        private LockResponse CreateErrorResponse(LockInfo lockInfo, string message)
        {
            return LockResponseFactory.CreateFailedResponse(message, lockInfo);
        }


        #endregion

        #region 扩展方法

        /// <summary>
        /// 获取用户锁列表（供OrphanedLockDetector使用）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>用户锁列表</returns>
        public async Task<List<LockInfo>> GetUserLocksAsync(long userId)
        {
            try
            {
                var userLocks = _documentLocks.Values
                    .Where(lockInfo => lockInfo.LockedUserId == userId)
                    .Select(lockInfo => lockInfo)
                    .ToList();

                await Task.CompletedTask;
                return userLocks;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取用户锁列表时发生异常: UserId={UserId}", userId);
                return new List<LockInfo>();
            }
        }

        /// <summary>
        /// 通用解锁方法（供OrphanedLockDetector使用）
        /// </summary>
        /// <param name="lockKey">锁键</param>
        /// <param name="userId">用户ID</param>
        /// <returns>异步任务，返回包含详细信息的LockResponse</returns>
        public async Task<LockResponse> UnlockAsync(string lockKey, long userId)
        {
            try
            {
                // 参数验证
                if (string.IsNullOrEmpty(lockKey))
                    return LockResponseFactory.CreateFailedResponse("锁键为空");

                if (userId <= 0)
                    return LockResponseFactory.CreateFailedResponse("用户ID小于0");

                // 从锁键中提取单据ID
                if (!long.TryParse(lockKey.Replace("lock:document:", ""), out long billId))
                    return LockResponseFactory.CreateFailedResponse("锁键格式无效，无法解析单据ID");

                // 使用统一解锁方法
                return await ExecuteUnlockAsync(billId, userId, false, "孤儿锁清理解锁");
            }
            catch (Exception ex)
            {
                return LockResponseFactory.CreateFailedResponse("孤儿锁清理解锁：" + ex.Message);
            }
        }

        /// <summary>
        /// 强制解锁方法（供OrphanedLockDetector使用）
        /// </summary>
        /// <param name="lockKey">锁键</param>
        /// <param name="userId">用户ID</param>
        /// <returns>异步任务，返回包含详细信息的LockResponse</returns>
        public async Task<LockResponse> ForceUnlockAsync(string lockKey, long userId)
        {
            try
            {
                // 参数验证
                if (string.IsNullOrEmpty(lockKey))
                    return LockResponseFactory.CreateFailedResponse("锁键为空");

                if (userId <= 0)
                    return LockResponseFactory.CreateFailedResponse("用户ID小于0");

                // 从锁键中提取单据ID
                if (!long.TryParse(lockKey.Replace("lock:document:", ""), out long billId))
                    return LockResponseFactory.CreateFailedResponse("锁键格式无效，无法解析单据ID");

                // 使用统一强制解锁方法
                return await ExecuteUnlockAsync(billId, userId, true, "孤儿锁清理强制解锁");
            }
            catch (Exception ex)
            {
                return LockResponseFactory.CreateFailedResponse("孤儿锁清理解锁：" + ex.Message);
            }
        }

        /// <summary>
        /// 获取孤儿锁数量（供OrphanedLockDetector使用）
        /// </summary>
        /// <returns>孤儿锁数量</returns>
        public int GetOrphanedLockCount()
        {
            try
            {
                var now = DateTime.Now;
                var orphanedCount = _documentLocks.Values
                    .Count(lockInfo => lockInfo.ExpireTime < now);

                return orphanedCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取孤儿锁数量时发生异常");
                return 0;
            }
        }

        #endregion

        #region IDisposable 实现

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
                return;

            try
            {
                // 停止服务
                StopAsync().GetAwaiter().GetResult();

                // 释放定时器
                _cleanupTimer?.Dispose();
                _maintenanceTimer?.Dispose();

                // 释放孤儿锁检测器
                _orphanedLockDetector?.Dispose();

                // 清理所有锁
                _documentLocks.Clear();

                _isDisposed = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "释放集成式服务器锁管理器资源时发生异常");
            }
        }

        #endregion
    }


}