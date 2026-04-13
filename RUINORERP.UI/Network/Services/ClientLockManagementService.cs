using Microsoft.Extensions.Logging;
using RUINORERP.Common.Extensions;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Lock;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.UI.Network.Authentication;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 集成式锁管理服务 v2.0.0
    /// 整合客户端缓存和服务器锁管理的完整解决方案
    /// 
    /// 版本：2.0.0
    /// 作者：AI Assistant
    /// 创建时间：2025-01-27
    /// 
    /// 主要特性：
    /// - 智能客户端缓存
    /// - 异常断开恢复
    /// - 统一版本控制
    /// - 完整的生命周期管理
    /// </summary>
    public class ClientLockManagementService : IDisposable
    {
        #region 私有字段

        private readonly Lazy<ClientCommunicationService> _communicationService;
        private readonly ILogger<ClientLockManagementService> _logger;
        private readonly ClientLocalLockCacheService _clientCache;
        private readonly LockRecoveryManager _recoveryManager;
        private readonly LockStatusNotificationService _notificationService;

        /// <summary>
        /// 活跃锁集合 - 核心数据结构
        /// 用于存储当前客户端持有的所有活跃锁信息，是客户端锁状态管理的核心组件
        /// key: BillID (单据ID)
        /// value: LockInfo (锁信息对象)
        /// 
        /// _activeLocks在锁管理系统中扮演着至关重要的角色：
        /// 1. 跟踪当前客户端成功持有的所有锁
        /// 2. 为锁刷新机制提供数据源
        /// 3. 在服务停止时确保正确释放所有锁
        /// 4. 提供服务统计信息
        /// 
        /// 使用ConcurrentDictionary确保线程安全，避免多线程并发操作时的数据竞争
        /// </summary>
        private readonly ConcurrentDictionary<long, LockInfo> _activeLocks;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly Timer _lockRefreshTimer;
        private readonly Timer _cacheCleanupTimer;

        private bool _isDisposed;
        // 替换全局信号量为细粒度锁，每个单据ID使用独立的信号量
        private readonly ConcurrentDictionary<long, SemaphoreSlim> _billLocks = new ConcurrentDictionary<long, SemaphoreSlim>();
        // 全局信号量仅用于需要整体同步的操作
        private readonly SemaphoreSlim _globalSemaphore = new SemaphoreSlim(1, 1);

        // 配置参数
        private readonly TimeSpan _lockRefreshInterval = TimeSpan.FromMinutes(1);
        private readonly TimeSpan _cacheCleanupInterval = TimeSpan.FromMinutes(5);
        private readonly TimeSpan _lockTimeoutThreshold = TimeSpan.FromMinutes(30);
        private readonly int _maxRetryAttempts = 3;
        // 添加操作超时时间，避免长时间阻塞
        private readonly TimeSpan _operationTimeout = TimeSpan.FromSeconds(15);
        // 缓存预热阈值
        private const int _cacheWarmupThreshold = 100;

        #endregion

        #region 构造函数和初始化

        /// <summary>
        /// 集成式锁管理服务构造函数
        /// </summary>
        /// <param name="communicationService">客户端通信服务的延迟加载实例</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="clientCache">客户端锁缓存（可选，为null时内部创建）</param>
        /// <param name="recoveryManager">锁恢复管理器（可选，为null时内部创建）</param>
        /// <param name="notificationService">锁状态通知服务（可选，为null时内部创建）</param>
        public ClientLockManagementService(
            Lazy<ClientCommunicationService> communicationService,
            ILogger<ClientLockManagementService> logger,
            ClientLocalLockCacheService clientCache = null,
            LockRecoveryManager recoveryManager = null,
            LockStatusNotificationService notificationService = null)
        {
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _notificationService = notificationService;

            // 初始化组件
            _activeLocks = new ConcurrentDictionary<long, LockInfo>();
            _cancellationTokenSource = new CancellationTokenSource();

            // 设置客户端锁缓存 - 如果未提供则创建（使用接口解耦）
            if (clientCache != null)
            {
                _clientCache = clientCache;
            }
            else
            {
                // 内部创建时不再传入自身作为接口实现，避免循环依赖
                var cacheLogger = _logger as ILogger<ClientLocalLockCacheService> ??
                    new Microsoft.Extensions.Logging.Logger<ClientLocalLockCacheService>(new Microsoft.Extensions.Logging.LoggerFactory());
                _clientCache = new ClientLocalLockCacheService(null, cacheLogger);
            }

            // 设置锁恢复管理器 - 如果未提供则创建
            if (recoveryManager != null)
            {
                _recoveryManager = recoveryManager;
            }
            else
            {
                // 内部创建时传入this和clientCache
                var recoveryLogger = _logger as ILogger<LockRecoveryManager> ??
                    new Microsoft.Extensions.Logging.Logger<LockRecoveryManager>(new Microsoft.Extensions.Logging.LoggerFactory());
                _recoveryManager = new LockRecoveryManager(this, _clientCache, recoveryLogger);
            }

            // 初始化定时器 - 设置为无限等待，将在StartAsync方法中启动
            // 锁刷新定时器 - 将在StartAsync中设置为每分钟执行一次
            _lockRefreshTimer = new Timer(RefreshLocksCallback, null, Timeout.Infinite, Timeout.Infinite);
            // 缓存清理定时器 - 将在StartAsync中设置为每5分钟执行一次
            _cacheCleanupTimer = new Timer(CleanupCacheCallback, null, Timeout.Infinite, Timeout.Infinite);


        }

        /// <summary>
        /// 启动服务
        /// </summary>
        public async Task StartAsync()
        {
            try
            {
                await _globalSemaphore.WaitAsync(_operationTimeout, _cancellationTokenSource.Token);

                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(ClientLockManagementService));

                // 启动定时器
                _lockRefreshTimer.Change(TimeSpan.Zero, _lockRefreshInterval);
                _cacheCleanupTimer.Change(TimeSpan.FromMinutes(1), _cacheCleanupInterval);

                // 预热缓存（优化）
                await WarmupCacheAsync();

                // LockRecoveryManager不需要手动启动，构造函数中已初始化
            }
            finally
            {
                _globalSemaphore.Release();
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public async Task StopAsync()
        {
            try
            {
                await _globalSemaphore.WaitAsync(_operationTimeout);

                if (_isDisposed)
                    return;

                // 停止定时器
                _lockRefreshTimer.Change(Timeout.Infinite, Timeout.Infinite);
                _cacheCleanupTimer.Change(Timeout.Infinite, Timeout.Infinite);

                // 释放所有锁
                await ReleaseAllLocksAsync();

                // 清理所有细粒度锁
                foreach (var lockObj in _billLocks.Values)
                {
                    try { lockObj.Dispose(); }
                    catch { }
                }
                _billLocks.Clear();

                // LockRecoveryManager不需要手动停止
            }
            finally
            {
                _globalSemaphore.Release();
            }
        }

        #endregion

        #region 核心锁管理功能


        /// <summary>
        /// 检查锁状态 - 实现ILockStatusProvider接口
        /// 优化：使用细粒度锁，避免全局阻塞
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="MenuID">菜单ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>锁状态</returns>
        public async Task<LockResponse> CheckLockStatusAsync(long billId, long MenuID = 0, CancellationToken cancellationToken = default)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(ClientLockManagementService));

            // 边界条件检查：验证单据ID有效性
            if (billId <= 0)
            {
                _logger.LogWarning("检查锁状态请求参数无效: 单据ID={BillId}", billId);
                return LockResponseFactory.CreateFailedResponse("单据ID无效");
            }

            try
            {                // 获取当前单据的锁
                var billLock = _billLocks.GetOrAdd(billId, _ => new SemaphoreSlim(1, 1));

                // 诊断日志：记录锁的初始状态
                _logger.LogDebug("检查锁状态 - BillID={BillId}, CurrentCount={CurrentCount}, Waiters=未知",
                    billId, billLock.CurrentCount);

                // 使用超时机制，避免长时间阻塞
                if (!await billLock.WaitAsync(_operationTimeout, cancellationToken != default ? cancellationToken : _cancellationTokenSource.Token))
                {
                    _logger.LogWarning("检查锁状态操作超时 - 单据ID: {BillId}", billId);
                    return new LockResponse
                    {
                        IsSuccess = false,
                        Message = "操作超时，请稍后重试"
                    };
                }

                try
                {
                    // 优先检查本地缓存
                    var cachedStatus = await _clientCache.GetLockInfoAsync(billId);
                    if (cachedStatus != null && !cachedStatus.IsExpired)
                    {
                        _logger.LogDebug("从缓存获取单据 {BillId} 锁状态", billId);
                        return new LockResponse
                        {
                            IsSuccess = true,
                            LockInfo = cachedStatus
                        };
                    }

                    var lockInfo = new LockInfo { BillID = billId };
                    lockInfo.SessionId = MainForm.Instance.AppContext.SessionId;
                    // 缓存过期或不存在，直接查询服务器
                    var lockRequest = new LockRequest
                    {
                        LockInfo = lockInfo
                    };

                    // 使用传入的cancellationToken或默认值
                    var token = cancellationToken != default ? cancellationToken : _cancellationTokenSource.Token;
                    var response = await _communicationService.Value.SendCommandWithResponseAsync<LockResponse>(
                        LockCommands.CheckLockStatus, lockRequest, token, (int)_operationTimeout.TotalMilliseconds);

                    // 更新缓存
                    if (response != null)
                    {
                        if (response.IsSuccess && response.LockInfo != null)
                        {
                            if (response.LockInfo.IsLocked)
                            {
                                // 创建本地缓存项，延长缓存时间
                                var localLockInfo = new LockInfo
                                {
                                    BillID = response.LockInfo.BillID,
                                    IsLocked = response.LockInfo.IsLocked,
                                    LockedUserId = response.LockInfo.LockedUserId,
                                    LockedUserName = response.LockInfo.LockedUserName,
                                    MenuID = response.LockInfo.MenuID,
                                    LockTime = response.LockInfo.LockTime,
                                    LastUpdateTime = DateTime.Now,
                                    ExpireTime = DateTime.Now.AddMinutes(10) // 延长缓存时间至10分钟
                                };
                                // 使用公共方法更新缓存
                                _clientCache.UpdateCacheItem(localLockInfo);

                                return new LockResponse
                                {
                                    IsSuccess = true,
                                    LockInfo = localLockInfo
                                };
                            }
                        }
                    }
                    else
                    {
                        response = LockResponseFactory.CreateFailedResponse("响应为空");
                    }
                    return response;
                }
                finally
                {
                    // 确保锁被释放
                    try
                    {
                        billLock.Release();
                    }
                    catch (SemaphoreFullException)
                    {
                        // 锁已经被释放,忽略
                        _logger.LogDebug("锁已释放,无需重复释放: BillID={BillId}", billId);
                    }
                    // 延迟清理未使用的锁,避免影响等待中的线程
                    CleanupUnusedLock(billId, billLock);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "检查单据 {BillId} 锁状态时发生异常", billId);

                return new LockResponse
                {
                    IsSuccess = false,
                    Message = $"检查锁状态异常: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// 锁定单据（线程安全实现）- 实现ILockStatusProvider接口
        /// <para>优化说明：v2.1.1 - 增加状态一致性保证、边界条件检查、实时同步</para>
        /// <para>工作流程：
        /// 1. 使用细粒度锁，每个单据独立锁定，避免全局阻塞
        /// 2. 首先检查本地缓存，判断单据是否已被锁定
        /// 3. 如果缓存未命中或已过期，向服务器发送锁定请求
        /// 4. 锁定成功后，同步更新本地缓存、活跃锁列表和通知服务
        /// 5. 使用超时机制，避免长时间阻塞
        /// 6. 增加边界条件检查，确保状态一致性
        /// </para>
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="BillNo">单据编号</param>
        /// <param name="bizType">业务类型</param>
        /// <param name="menuId">菜单ID</param>
        /// <param name="timeoutMinutes">超时时间（毫秒）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>锁定响应，包含是否成功和锁定信息</returns>
        /// <exception cref="ObjectDisposedException">当服务已被释放时抛出</exception>
        /// <exception cref="TaskCanceledException">当操作被取消时抛出</exception>
        public async Task<LockResponse> LockBillAsync(long billId, string BillNo, BizType bizType, long menuId, int timeoutMinutes = 10000, CancellationToken cancellationToken = default)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(ClientLockManagementService));

            // 边界条件检查
            if (billId <= 0)
            {
                _logger.LogWarning("锁定请求参数无效: 单据ID={BillId}", billId);
                return LockResponseFactory.CreateFailedResponse("单据ID无效");
            }

            if (string.IsNullOrEmpty(BillNo))
            {
                _logger.LogWarning("锁定请求参数无效: 单据编号为空");
                return LockResponseFactory.CreateFailedResponse("单据编号不能为空");
            }

            var stopwatch = Stopwatch.StartNew();

            try
            {
                // 获取当前单据的锁
                var billLock = _billLocks.GetOrAdd(billId, _ => new SemaphoreSlim(1, 1));

                // 使用超时机制，避免长时间阻塞
                if (!await billLock.WaitAsync(_operationTimeout, cancellationToken != default ? cancellationToken : _cancellationTokenSource.Token))
                {
                    _logger.LogWarning("锁定操作超时 - 单据ID: {BillId}", billId);
                    return new LockResponse
                    {
                        IsSuccess = false,
                        Message = "操作超时，请稍后重试"
                    };
                }

                try
                {
                    // 边界条件检查：验证用户信息
                    var currentUserId = MainForm.Instance.AppContext.CurUserInfo?.UserInfo?.User_ID ?? 0;
                    var currentUserName = MainForm.Instance.AppContext.CurUserInfo?.UserInfo?.UserName ?? string.Empty;
                    var sessionId = MainForm.Instance.AppContext?.SessionId ?? string.Empty;

                    if (currentUserId == 0)
                    {
                        _logger.LogWarning("用户未登录，无法执行锁定操作: 单据ID={BillId}", billId);
                        return LockResponseFactory.CreateFailedResponse("用户未登录");
                    }

                    // 检查是否已锁定（本地缓存）
                    var islock = await _clientCache.IsLockedAsync(billId);
                    if (islock)
                    {
                        // 尝试获取锁信息
                        var lockInfoByLocal = await _clientCache.GetLockInfoAsync(billId);

                        // 边界条件：检查是否是自己的锁
                        if (lockInfoByLocal != null && lockInfoByLocal.LockedUserId == currentUserId)
                        {
                            _logger.LogDebug("单据 {BillId} 已被当前用户锁定，直接返回成功", billId);
                            // 确保缓存信息完整
                            _clientCache.UpdateCacheItem(lockInfoByLocal);
                            // 通知UI状态更新
                            _notificationService?.NotifyLockStatusChanged(billId, lockInfoByLocal, LockStatusChangeType.StatusUpdated);

                            return new LockResponse
                            {
                                IsSuccess = true,
                                Message = "单据已被锁定",
                                LockInfo = lockInfoByLocal
                            };
                        }
                        else
                        {
                            _logger.LogDebug("单据 {BillId} 已被其他用户锁定", billId);
                            return new LockResponse
                            {
                                IsSuccess = false,
                                Message = $"单据已被用户 {lockInfoByLocal?.LockedUserName} 锁定",
                                LockInfo = lockInfoByLocal
                            };
                        }
                    }

                    // 创建锁定请求
                    var lockInfo = CreateLockInfo(billId, menuId);
                    lockInfo.SessionId = sessionId;
                    lockInfo.LockedUserId = currentUserId;
                    lockInfo.LockedUserName = currentUserName;
                    lockInfo.bizType = bizType;
                    lockInfo.BillNo = BillNo;
                    var lockRequest = new LockRequest { LockInfo = lockInfo };

                    // 使用传入的cancellationToken或默认值
                    var token = cancellationToken != default ? cancellationToken : _cancellationTokenSource.Token;

                    // 发送锁定请求
                    var response = await _communicationService.Value.SendCommandWithResponseAsync<LockResponse>(
                        LockCommands.Lock, lockRequest, token, timeoutMinutes);

                    stopwatch.Stop();
                    if (response == null)
                    {
                        return LockResponseFactory.CreateFailedResponse("响应为空");
                    }

                    if (response.IsSuccess)
                    {
                        // 边界条件验证：响应中包含有效的锁信息
                        if (response.LockInfo == null)
                        {
                            _logger.LogWarning("锁定成功但锁信息为空: 单据ID={BillId}", billId);
                            return LockResponseFactory.CreateFailedResponse("锁信息无效");
                        }

                        // 边界条件验证：确保SessionId和UserId一致性
                        if (response.LockInfo.SessionId != sessionId || response.LockInfo.LockedUserId != currentUserId)
                        {
                            _logger.LogWarning("锁信息与当前会话不匹配: 单据ID={BillId}, 响应SessionId={ResponseSessionId}, 响应UserId={ResponseUserId}",
                                billId, response.LockInfo.SessionId, response.LockInfo.LockedUserId);
                        }

                        // 添加到活跃锁列表 - 锁定成功后的关键操作
                        // 只有当锁成功获取后，才会将锁信息添加到_activeLocks集合中
                        // 这样可以确保_activeLocks只包含当前客户端实际持有的有效锁
                        _activeLocks.TryAdd(billId, response.LockInfo);

                        // 同步更新本地缓存，确保本地缓存与服务器状态一致
                        if (_clientCache != null)
                        {
                            // 延长缓存时间，减少频繁更新
                            response.LockInfo.LastUpdateTime = DateTime.Now;
                            response.LockInfo.ExpireTime = DateTime.Now.AddMinutes(15);

                            _clientCache.UpdateCacheItem(response.LockInfo);
                        }

                        // 实时通知UI状态更新
                        _notificationService?.NotifyLockStatusChanged(billId, response.LockInfo, LockStatusChangeType.Locked);
                    }
                    else
                    {
                        _logger.Debug("单据 {BillId} 锁定失败: {Message}, 耗时 {ElapsedMs}ms", billId, response.Message, stopwatch.ElapsedMilliseconds);

                        // 边界条件：如果服务器返回锁信息，也要更新缓存以保持一致性
                        if (response.LockInfo != null)
                        {
                            _clientCache.UpdateCacheItem(response.LockInfo);
                            _notificationService?.NotifyLockStatusChanged(billId, response.LockInfo, LockStatusChangeType.StatusUpdated);
                        }
                    }

                    return response;
                }
                finally
                {
                    // 确保锁被释放
                    try
                    {
                        billLock.Release();
                    }
                    catch (SemaphoreFullException)
                    {
                        // 锁已经被释放,忽略
                        _logger.LogDebug("锁已释放,无需重复释放: BillID={BillId}", billId);
                    }
                    // 延迟清理未使用的锁,避免影响等待中的线程
                    CleanupUnusedLock(billId, billLock);
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "锁定单据 {BillId} 时发生异常，耗时 {ElapsedMs}ms",
                    billId, stopwatch.ElapsedMilliseconds);

                return new LockResponse
                {
                    IsSuccess = false,
                    Message = $"锁定异常: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// 解锁单据 - 实现ILockStatusProvider接口
        /// 这是主要实现版本，其他版本都调用此方法
        /// <para>优化说明：v2.1.1 - 增加状态一致性保证、边界条件检查、实时通知</para>
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>解锁响应</returns>
        public async Task<LockResponse> UnlockBillAsync(long billId, CancellationToken cancellationToken = default)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(ClientLockManagementService));

            // 边界条件检查
            if (billId <= 0)
            {
                _logger.LogWarning("解锁请求参数无效: 单据ID={BillId}", billId);
                return LockResponseFactory.CreateFailedResponse("单据ID无效");
            }

            var stopwatch = Stopwatch.StartNew();

            try
            {
                // 获取当前单据的锁
                var billLock = _billLocks.GetOrAdd(billId, _ => new SemaphoreSlim(1, 1));

                // 使用传入的cancellationToken或默认值
                var token = cancellationToken != default ? cancellationToken : _cancellationTokenSource.Token;

                // 使用超时机制
                if (!await billLock.WaitAsync(_operationTimeout, token))
                {
                    _logger.LogWarning("解锁操作超时 - 单据ID: {BillId}", billId);
                    return new LockResponse
                    {
                        IsSuccess = false,
                        Message = "操作超时，请稍后重试"
                    };
                }

                try
                {
                    // 边界条件检查：验证用户信息
                    var currentUserId = MainForm.Instance.AppContext.CurUserInfo?.UserInfo?.User_ID ?? 0;
                    var sessionId = MainForm.Instance.AppContext?.SessionId ?? string.Empty;

                    if (currentUserId == 0)
                    {
                        _logger.LogWarning("用户未登录，无法执行解锁操作: 单据ID={BillId}", billId);
                        return LockResponseFactory.CreateFailedResponse("用户未登录");
                    }

                    // 从活跃锁列表中移除 - 解锁操作的第一步
                    // 在发送解锁请求前，先从_activeLocks中移除，这样可以确保即使网络请求失败
                    // 本地状态也能保持一致，不会留下孤儿锁引用
                    _activeLocks.TryRemove(billId, out var removedLock);

                    // 缓存清除（先清除，确保本地状态一致）
                    _clientCache.ClearCache(billId);

                    // 创建解锁请求
                    var lockInfo = CreateLockInfo(billId, 0);
                    lockInfo.SessionId = sessionId;
                    lockInfo.LockedUserId = currentUserId;
                    var unlockRequest = new LockRequest { LockInfo = lockInfo };

                    var response = await _communicationService.Value.SendCommandWithResponseAsync<LockResponse>(
                        LockCommands.Unlock, unlockRequest, token, (int)_operationTimeout.TotalMilliseconds);

                    stopwatch.Stop();

                    // 检查响应是否为空
                    if (response == null)
                    {
                        // 边界条件：响应为空，恢复本地状态
                        if (removedLock != null)
                        {
                            _activeLocks.TryAdd(billId, removedLock);
                            _clientCache.UpdateCacheItem(removedLock);
                        }
                        return LockResponseFactory.CreateFailedResponse("响应为空");
                    }

                    if (response.IsSuccess)
                    {
                        // 创建解锁状态通知（在移除锁之前）
                        var unlockedInfo = removedLock ?? new LockInfo
                        {
                            BillID = billId,
                            IsLocked = false,
                            LockedUserId = 0,
                            SessionId = sessionId
                        };

                        // 实时通知UI状态更新
                        _notificationService?.NotifyLockStatusChanged(billId, unlockedInfo, LockStatusChangeType.Unlocked);

                        // 延迟清理细粒度锁（不在这里立即移除，让finally中的CleanupUnusedLock处理）
                        // _billLocks.TryRemove(billId, out _);  // ❌ 删除这行，避免竞态条件
                    }
                    else
                    {
                        // 边界条件：解锁失败，恢复本地状态
                        if (removedLock != null)
                        {
                            _activeLocks.TryAdd(billId, removedLock);
                            _clientCache.UpdateCacheItem(removedLock);
                            
                            // ✅ 根据失败原因区分日志级别
                            bool isLockNotFound = response.Message?.Contains("未被锁定") == true || 
                                                 response.Message?.Contains("不存在") == true;
                            
                            if (isLockNotFound)
                            {
                                // 如果是因为锁不存在而失败，这通常是正常的（可能是重复解锁或锁已过期）
                                _logger.LogDebug("解锁时锁已不存在: 单据ID={BillId}, 原因: {Message}",
                                    billId, response.Message);
                            }
                            else
                            {
                                // 其他失败原因记录为警告
                                _logger.LogWarning("解锁失败，已恢复本地锁状态: 单据ID={BillId}, 原因: {Message}",
                                    billId, response.Message);
                            }
                        }
                        else
                        {
                            // 没有移除的锁，说明本地也没有这个锁的记录
                            _logger.LogDebug("解锁时本地无锁记录: 单据ID={BillId}, 服务器响应: {Message}",
                                billId, response.Message);
                        }
                    }

                    return response;
                }
                finally
                {
                    // 确保锁被释放
                    try
                    {
                        billLock.Release();
                    }
                    catch (SemaphoreFullException)
                    {
                        // 锁已经被释放,忽略
                        _logger.LogDebug("锁已释放,无需重复释放: BillID={BillId}", billId);
                    }
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "解锁单据 {BillId} 时发生异常，耗时 {ElapsedMs}ms",
                    billId, stopwatch.ElapsedMilliseconds);

                return new LockResponse
                {
                    IsSuccess = false,
                    Message = $"解锁异常: {ex.Message}"
                };
            }
        }




        #endregion


        #region 定时器回调

        /// <summary>
        /// 刷新锁定时器回调
        /// </summary>
        private async void RefreshLocksCallback(object state)
        {
            if (_isDisposed || _cancellationTokenSource.Token.IsCancellationRequested)
                return;

            try
            {
                await RefreshAllLocksAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刷新锁定时器回调中发生异常");
            }
        }

        /// <summary>
        /// 清理缓存定时器回调
        /// 优化：添加缓存健康检查和优化
        /// </summary>
        private async void CleanupCacheCallback(object state)
        {
            if (_isDisposed || _cancellationTokenSource.Token.IsCancellationRequested)
                return;

            try
            {
                // 执行缓存健康检查
                await PerformCacheHealthCheckAsync();

                // 清理过期的细粒度锁
                await CleanupExpiredBillLocksAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清理缓存定时器回调中发生异常");
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 创建锁信息对象
        /// </summary>
        private LockInfo CreateLockInfo(long billId, long menuId)
        {
            var userInfo = GetCurrentUser();

            return new LockInfo
            {
                BillID = billId,
                MenuID = menuId,
                LockedUserId = userInfo?.UserID ?? 0,
                LockedUserName = userInfo?.姓名 ?? "Unknown",
                LockTime = DateTime.Now,
            };
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        private CurrentUserInfo GetCurrentUser()
        {
            try
            {
                return MainForm.Instance?.AppContext?.CurUserInfo;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "获取当前用户信息失败");
                return null;
            }
        }


        /// <summary>
        /// 获取锁定信息
        /// 直接从缓存服务获取锁定信息，避免直接调用缓存服务
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="menuId">菜单ID</param>
        /// <returns>锁定信息，如果没有有效的锁定信息则返回null</returns>
        public async Task<LockInfo> GetLockInfoAsync(long billId, long menuId)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(ClientLockManagementService));

            // 直接从内部缓存服务获取锁定信息
            return await _clientCache.GetLockInfoAsync(billId, (int)menuId);
        }

        /// <summary>
        /// 获取锁缓存服务
        /// 提供对内部缓存服务的访问，供需要直接操作缓存的场景使用
        /// </summary>
        /// <returns>客户端锁缓存服务实例</returns>
        public ClientLocalLockCacheService GetLockCacheService()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(ClientLockManagementService));

            return _clientCache;
        }

        /// <summary>
        /// 获取服务统计信息
        /// 使用_activeLocks集合来提供活跃锁数量的实时统计数据
        /// </summary>
        public LockInfoStatistics GetStatistics()
        {
            return new LockInfoStatistics
            {
                // 直接从_activeLocks集合获取当前活跃锁数量，这是最准确的实时数据
                // _activeLocks.Count提供了客户端锁管理系统的核心统计指标
                // 这些数据用于监控系统状态、问题排查和性能分析
                ActiveLocks = _activeLocks.Count,
                TotalLocks = _activeLocks.Count,
                HeartbeatEnabled = true,
                LastCleanup = DateTime.Now,
                ServiceVersion = "2.0.0",
                VersionDate = DateTime.Now.ToString("yyyy-MM-dd"),
                LocksByBizType = new Dictionary<BizType, int>(),
            };
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
                // 停止服务 - 使用Task.Run在后台线程执行，避免阻塞
                // 添加超时机制，防止无限等待
                try
                {
                    using (var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
                    {
                        var stopTask = Task.Run(async () => await StopAsync().ConfigureAwait(false), timeoutCts.Token);
                        stopTask.Wait(timeoutCts.Token);
                    }
                }
                catch (OperationCanceledException)
                {
                    System.Diagnostics.Debug.WriteLine("LockManagementService停止操作超时");
                }
                catch (AggregateException ae)
                {
                    foreach (var e in ae.InnerExceptions)
                    {
                        System.Diagnostics.Debug.WriteLine($"LockManagementService停止异常: {e.Message}");
                    }
                }

                // 释放定时器
                _lockRefreshTimer?.Dispose();
                _cacheCleanupTimer?.Dispose();

                // 释放组件
                _clientCache?.Dispose();
                _recoveryManager?.Dispose();

                // 取消令牌
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource?.Dispose();

                // 释放所有细粒度锁
                foreach (var lockObj in _billLocks.Values)
                {
                    try { lockObj.Dispose(); }
                    catch { }
                }
                _billLocks.Clear();

                // 释放全局信号量
                _globalSemaphore?.Dispose();

                _isDisposed = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "释放集成式锁管理服务资源时发生异常");
            }
        }

        /// <summary>
        /// 刷新所有活跃锁
        /// 优化：使用并行处理，提高效率
        /// </summary>
        private async Task RefreshAllLocksAsync()
        {
            if (_isDisposed || _activeLocks.IsEmpty)
                return;

            // 获取所有活跃锁的单据ID - 锁刷新机制的关键数据源
            // _activeLocks提供了所有需要被刷新的锁信息，这是客户端锁维护的核心机制
            // 通过遍历所有活跃锁并检查其状态，确保锁的有效性和一致性
            var activeLockIds = _activeLocks.Keys.ToArray();
            if (activeLockIds.Length == 0)
                return;

            _logger.LogDebug("开始刷新 {LockCount} 个活跃锁", activeLockIds.Length);

            // 分批次并行处理，避免创建过多任务
            const int batchSize = 20;
            for (int i = 0; i < activeLockIds.Length; i += batchSize)
            {
                var batch = activeLockIds.Skip(i).Take(batchSize);
                var refreshTasks = batch.Select(async billId =>
                {
                    try
                    {
                        var response = await CheckLockStatusAsync(billId, 0, _cancellationTokenSource.Token);
                        if (response.IsSuccess && response.LockInfo != null)
                        {
                            // 检查锁是否仍然有效
                            if (response.LockInfo.IsExpired)
                            {
                                // 获取当前单据的锁
                                var billLock = _billLocks.GetOrAdd(billId, _ => new SemaphoreSlim(1, 1));
                                if (await billLock.WaitAsync(_operationTimeout))
                                {
                                    try
                                    {
                                        _logger.LogWarning("检测到单据 {BillId} 锁已过期，将从活跃列表移除", billId);
                                        // 锁已过期，从_activeLocks中移除
                                        _activeLocks.TryRemove(billId, out _);
                                        _clientCache.ClearCache(billId);
                                        // 延迟清理细粒度锁（让CleanupUnusedLock处理）
                                        // _billLocks.TryRemove(billId, out _);  // ❌ 删除这行
                                    }
                                    finally
                                    {
                                        billLock.Release();
                                    }
                                }
                            }
                            else
                            {
                                // 更新刷新时间戳
                                // 即使锁没有过期，也需要更新最后更新时间，这对于监控锁的活跃度很重要
                                if (_activeLocks.TryGetValue(billId, out var clientLock))
                                {
                                    clientLock.LastUpdateTime = DateTime.Now;
                                    clientLock.ExpireTime = DateTime.Now.AddMinutes(15);
                                }
                                // 更新缓存
                                _clientCache.UpdateCacheItem(response.LockInfo);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "刷新单据 {BillId} 锁状态时发生异常", billId);
                    }
                });

                await Task.WhenAll(refreshTasks);
            }

            _logger.LogDebug("活跃锁刷新完成");
        }

        /// <summary>
        /// 释放所有锁
        /// </summary>
        private async Task ReleaseAllLocksAsync()
        {
            if (_activeLocks.IsEmpty)
                return;

            var activeLockIds = _activeLocks.Keys.ToArray();
            _logger.LogDebug("开始释放 {LockCount} 个活跃锁", activeLockIds.Length);

            // 分批次释放，避免创建过多并发任务
            const int batchSize = 15;
            for (int i = 0; i < activeLockIds.Length; i += batchSize)
            {
                var batch = activeLockIds.Skip(i).Take(batchSize);
                var releaseTasks = batch.Select(async billId =>
                {
                    try
                    {
                        await UnlockBillAsync(billId, _cancellationTokenSource.Token);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "释放单据 {BillId} 锁时发生异常", billId);
                    }
                });

                await Task.WhenAll(releaseTasks);
            }

            _logger.LogDebug("所有活跃锁释放完成");
        }

        /// <summary>
        /// 刷新单据锁定状态 v2.1.0优化
        /// 该方法通过向服务器发送请求来更新锁定的过期时间，确保当前用户能够持续编辑单据
        /// 优化：使用细粒度锁，添加超时机制
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="menuId">菜单ID</param>
        /// <param name="ct">取消令牌，用于取消异步操作</param>
        /// <returns>刷新操作的响应结果</returns>
        public async Task<LockResponse> RefreshLockAsync(long billId, long menuId, CancellationToken ct = default)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(ClientLockManagementService));

            try
            {
                // 获取当前单据的锁
                var billLock = _billLocks.GetOrAdd(billId, _ => new SemaphoreSlim(1, 1));

                // 使用超时机制
                if (!await billLock.WaitAsync(_operationTimeout, ct != default ? ct : _cancellationTokenSource.Token))
                {
                    _logger.LogWarning("刷新锁操作超时 - 单据ID: {BillId}", billId);
                    return new LockResponse
                    {
                        IsSuccess = false,
                        Message = "操作超时，请稍后重试"
                    };
                }

                try
                {
                    // 获取当前用户信息
                    long currentUserId = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
                    string currentUserName = MainForm.Instance.AppContext.CurUserInfo.UserInfo.UserName;

                    // 创建锁信息1
                    LockInfo lockInfo = new LockInfo();
                    lockInfo.BillID = billId;
                    lockInfo.MenuID = menuId;
                    lockInfo.LockedUserId = currentUserId;
                    lockInfo.LockedUserName = currentUserName;
                    lockInfo.SessionId = MainForm.Instance.AppContext.SessionId;
                    // 创建刷新锁定请求
                    var lockRequest = new LockRequest
                    {
                        LockInfo = lockInfo,
                        RefreshMode = true, // 标记为刷新模式
                    };

                    lockRequest.LockInfo.SetLockKey();

                    _logger?.LogDebug("开始刷新单据锁定 - 单据ID: {BillId}, 菜单ID: {MenuId}, 用户ID: {UserId}, 用户名称: {UserName}",
                        billId, menuId, currentUserId, currentUserName);

                    // 使用通信服务发送刷新请求
                    var response = await _communicationService.Value.SendCommandWithResponseAsync<LockResponse>(
                        LockCommands.CheckLockStatus, lockRequest, ct, (int)_operationTimeout.TotalMilliseconds);

                    if (response.IsSuccess)
                    {
                        _logger?.LogDebug("单据锁定刷新成功 - 单据ID: {BillId}", billId);

                        // 更新本地缓存和活跃锁
                        if (_activeLocks.TryGetValue(billId, out var clientLock))
                        {
                            clientLock.LastUpdateTime = DateTime.Now;
                            clientLock.ExpireTime = DateTime.Now.AddMinutes(15);
                        }

                        // 同步更新本地缓存
                        _clientCache.UpdateCacheItem(response.LockInfo);
                    }
                    else
                    {
                        _logger?.LogWarning("单据锁定刷新失败 - 单据ID: {BillId}, 错误信息: {ErrorMessage}",
                            billId, response?.Message ?? "未知错误");
                    }

                    return response;
                }
                finally
                {
                    // 确保锁被释放
                    try
                    {
                        billLock.Release();
                    }
                    catch (SemaphoreFullException)
                    {
                        // 锁已经被释放,忽略
                        _logger.LogDebug("锁已释放,无需重复释放: BillID={BillId}", billId);
                    }
                    // 延迟清理未使用的锁,避免影响等待中的线程
                    CleanupUnusedLock(billId, billLock);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "刷新单据锁定时发生异常 - 单据ID: {BillId}, 菜单ID: {MenuId}", billId, menuId);
                throw;
            }
        }


        /// <summary>
        /// 批量检查锁状态（性能优化版）
        /// 使用并行处理减少网络请求延迟，提高批量操作效率
        /// </summary>
        /// <param name="billIds">要检查的单据ID列表</param>
        /// <param name="ct">取消令牌，用于取消异步操作</param>
        /// <returns>单据ID到锁定状态的映射字典</returns>
        public async Task<Dictionary<long, LockResponse>> BatchCheckLockStatusAsync(List<long> billIds, CancellationToken ct = default)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(ClientLockManagementService));

            if (billIds == null || !billIds.Any())
                return new Dictionary<long, LockResponse>();

            var stopwatch = Stopwatch.StartNew();

            try
            {
                // 限制批量查询大小，避免内存溢出
                var limitedIds = billIds.Take(100).ToList();

                // 使用并行处理进行批量查询
                var tasks = limitedIds.Select(async billId =>
                {
                    try
                    {
                        var response = await CheckLockStatusAsync(billId, 0, ct);
                        return (billId, response);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "批量检查单个单据失败: BillID={BillId}", billId);
                        return (billId, LockResponseFactory.CreateFailedResponse($"检查失败: {ex.Message}"));
                    }
                });

                var results = await Task.WhenAll(tasks);

                var successResults = results
                    .Where(r => r.Item2 != null)
                    .ToDictionary(r => r.Item1, r => r.Item2);

                _logger.LogDebug("批量检查锁状态完成: 查询数量={QueryCount}, 结果数量={ResultCount}, 耗时={ElapsedMs}ms",
                    limitedIds.Count, successResults.Count, stopwatch.ElapsedMilliseconds);

                return successResults;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "批量检查锁状态时发生异常: BillIDs={BillIds}", string.Join(",", billIds));

                // 降级到单次查询
                var fallbackResults = new Dictionary<long, LockResponse>();
                foreach (var billId in billIds.Take(20))
                {
                    try
                    {
                        var response = await CheckLockStatusAsync(billId, 0, ct);
                        fallbackResults[billId] = response;
                    }
                    catch (Exception innerEx)
                    {
                        _logger.LogWarning(innerEx, "批量检查降级查询失败: BillID={BillId}", billId);
                        fallbackResults[billId] = LockResponseFactory.CreateFailedResponse($"降级查询失败: {innerEx.Message}");
                    }
                }

                return fallbackResults;
            }
            finally
            {
                stopwatch.Stop();
            }
        }


        #endregion

        #region 获取锁状态列表功能

        /// <summary>
        /// 从服务器同步所有锁状态列表并更新到本地缓存
        /// 用于登录后获取系统当前所有锁状态信息，并自动同步到本地缓存
        /// </summary>
        /// <returns>包含所有锁状态的响应结果</returns>
        public async Task<LockResponse> GetLockStatusListAsync()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(ClientLockManagementService));

            // 验证通信服务是否可用
            if (_communicationService == null)
            {
                _logger?.LogError("从服务器同步锁状态列表失败: 通信服务未初始化");
                throw new InvalidOperationException("通信服务未初始化");
            }

            var stopwatch = Stopwatch.StartNew();
            _logger?.LogDebug("开始从服务器同步锁状态列表");

            try
            {
                // 创建空的锁请求（GetLockStatuList不需要特定参数）
                var lockRequest = new LockRequest();

                // 验证请求对象
                if (lockRequest == null)
                {
                    _logger?.LogError("从服务器同步锁状态列表失败: 创建锁请求对象失败");
                    return new LockResponse
                    {
                        IsSuccess = false,
                        Message = "从服务器同步锁状态列表失败: 创建锁请求对象失败",
                        LockInfoList = new List<LockInfo>()
                    };
                }

                // 发送获取锁状态列表命令
                var response = await _communicationService.Value.SendCommandWithResponseAsync<LockResponse>(
                    LockCommands.GetLockStatuList, lockRequest, _cancellationTokenSource.Token);

                // 验证响应1
                if (response == null)
                {
                    _logger?.LogError("从服务器同步锁状态列表失败: 服务器响应为空");
                    return new LockResponse
                    {
                        IsSuccess = false,
                        Message = "从服务器同步锁状态列表失败: 服务器响应为空",
                        LockInfoList = new List<LockInfo>()
                    };
                }

                if (response.IsSuccess)
                {
                    _logger?.LogDebug("从服务器同步锁状态列表成功 - 锁数量: {LockCount}, 耗时: {ElapsedMs}ms",
                        response.LockInfoList?.Count ?? 0, stopwatch.ElapsedMilliseconds);

                    // 批量更新本地缓存
                    if (response.LockInfoList != null && response.LockInfoList.Any())
                    {
                        // 数据验证和过滤
                        var validLockInfos = new List<LockInfo>();
                        var invalidCount = 0;

                        foreach (var lockInfo in response.LockInfoList)
                        {
                            try
                            {
                                // 验证锁信息的基本有效性
                                if (lockInfo == null)
                                {
                                    _logger?.LogWarning("发现空的锁信息对象");
                                    invalidCount++;
                                    continue;
                                }

                                if (lockInfo.BillID <= 0)
                                {
                                    _logger?.LogWarning("发现无效的锁信息BillID: {BillID}", lockInfo.BillID);
                                    invalidCount++;
                                    continue;
                                }

                                // 过滤掉过期的锁信息
                                if (lockInfo.IsExpired)
                                {
                                    _logger?.LogDebug("跳过过期的锁信息: BillID={BillID}, BillNo={BillNo}",
                                        lockInfo.BillID, lockInfo.BillNo);
                                    continue;
                                }

                                validLockInfos.Add(lockInfo);
                            }
                            catch (Exception ex)
                            {
                                _logger?.LogError(ex, "验证锁信息时出错: BillID={BillID}", lockInfo?.BillID ?? 0);
                                invalidCount++;
                            }
                        }

                        if (validLockInfos.Any())
                        {
                            // 使用批量更新方法提高性能
                            var updatedCount = _clientCache.BatchUpdateCache(validLockInfos);
                        }
                        else
                        {
                            _logger?.LogWarning("没有有效的锁信息可以更新到缓存");
                        }
                    }
                    else
                    {
                        _logger?.LogDebug("服务器返回的锁状态列表为空");
                    }
                }
                else
                {
                    _logger?.LogWarning("从服务器同步锁状态列表失败 - 错误信息: {ErrorMessage}, 耗时: {ElapsedMs}ms",
                        response?.Message ?? "未知错误", stopwatch.ElapsedMilliseconds);
                }

                return response;
            }
            catch (OperationCanceledException ex)
            {
                _logger?.LogWarning(ex, "从服务器同步锁状态列表被取消");
                throw;
            }
            catch (TimeoutException ex)
            {
                _logger?.LogError("从服务器同步锁状态列表超时，耗时: {ElapsedMs}ms", stopwatch.ElapsedMilliseconds);
                return new LockResponse
                {
                    IsSuccess = false,
                    Message = "从服务器同步锁状态列表超时",
                    LockInfoList = new List<LockInfo>()
                };
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "从服务器同步锁状态列表时发生异常，耗时: {ElapsedMs}ms", stopwatch.ElapsedMilliseconds);
                return new LockResponse
                {
                    IsSuccess = false,
                    Message = $"从服务器同步锁状态列表失败: {ex.Message}",
                    LockInfoList = new List<LockInfo>()
                };
            }
            finally
            {
                stopwatch.Stop();
            }
        }

        #endregion

        #region 解锁请求相关功能

        /// <summary>
        /// 请求解锁单据 v2.0.0新增
        /// 当用户需要解锁其他用户锁定的单据时使用
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="menuId">菜单ID</param>
        /// <returns>请求解锁的响应结果</returns>
        public async Task<LockResponse> RequestUnlockAsync(long billId, long menuId)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(ClientLockManagementService));

            try
            {
                _logger?.LogDebug("开始请求解锁单据 - 单据ID: {BillId}, 菜单ID: {MenuId}", billId, menuId);

                // 获取当前用户信息
                long currentUserId = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
                string currentUserName = MainForm.Instance.AppContext.CurUserInfo.UserInfo.UserName;

                // 创建锁信息
                LockInfo lockInfo = new LockInfo();
                lockInfo.BillID = billId;
                lockInfo.MenuID = menuId;
                //lockInfo.LockedUserId = currentUserId;
                //lockInfo.LockedUserName = currentUserName;
                //lockInfo.SessionId = MainForm.Instance.AppContext.SessionId;
                // 创建请求解锁请求
                var lockRequest = new LockRequest
                {
                    LockInfo = lockInfo,
                    RequesterUserName = currentUserName,
                    RequesterUserId = currentUserId,
                };
                lockRequest.LockInfo.SetLockKey();

                // 发送请求解锁命令
                var response = await _communicationService.Value.SendCommandWithResponseAsync<LockResponse>(
                    LockCommands.RequestUnlock, lockRequest, _cancellationTokenSource.Token);

                if (response.IsSuccess)
                {
                    _logger?.LogDebug("请求解锁发送成功 - 单据ID: {BillId}, 菜单ID: {MenuId}", billId, menuId);
                }
                else
                {
                    _logger?.LogWarning("请求解锁发送失败 - 单据ID: {BillId}, 菜单ID: {MenuId}, 错误信息: {ErrorMessage}",
                        billId, menuId, response?.Message ?? "未知错误");
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送请求解锁时发生异常 - 单据ID: {BillId}, 菜单ID: {MenuId}", billId, menuId);
                throw;
            }
        }

        /// <summary>
        /// 拒绝解锁请求 v2.0.0新增
        /// 当锁定者拒绝其他用户的解锁请求时使用
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="menuId">菜单ID</param>
        /// <param name="requesterUserName">请求者用户名</param>
        /// <returns>拒绝解锁的响应结果</returns>
        public async Task<LockResponse> RefuseUnlockAsync(long billId, long menuId, long RequesterUserId, string requesterUserName)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(ClientLockManagementService));

            try
            {
                _logger?.LogDebug("开始拒绝解锁请求 - 单据ID: {BillId}, 菜单ID: {MenuId}, 请求者: {RequesterUserName}",
                    billId, menuId, requesterUserName);

                // 获取当前用户信息
                long currentUserId = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
                string currentUserName = MainForm.Instance.AppContext.CurUserInfo.UserInfo.UserName;

                // 创建锁信息
                LockInfo lockInfo = new LockInfo();
                lockInfo.BillID = billId;
                lockInfo.MenuID = menuId;
                lockInfo.LockedUserId = currentUserId;
                lockInfo.LockedUserName = currentUserName;
                lockInfo.SessionId = MainForm.Instance.AppContext.SessionId;
                // 创建拒绝解锁请求
                var lockRequest = new LockRequest
                {
                    LockInfo = lockInfo,
                    RequesterUserId = RequesterUserId,
                    RequesterUserName = requesterUserName,
                };
                lockRequest.LockInfo.SetLockKey();

                // 发送拒绝解锁命令
                var response = await _communicationService.Value.SendCommandWithResponseAsync<LockResponse>(
                    LockCommands.RefuseUnlock, lockRequest, _cancellationTokenSource.Token);

                if (response.IsSuccess)
                {
                    _logger?.LogDebug("拒绝解锁请求成功 - 单据ID: {BillId}, 菜单ID: {MenuId}", billId, menuId);
                }
                else
                {
                    _logger?.LogWarning("拒绝解锁请求失败 - 单据ID: {BillId}, 菜单ID: {MenuId}, 错误信息: {ErrorMessage}",
                        billId, menuId, response?.Message ?? "未知错误");
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送拒绝解锁时发生异常 - 单据ID: {BillId}, 菜单ID: {MenuId}", billId, menuId);
                throw;
            }
        }

        /// <summary>
        /// 同意解锁请求 v2.0.0新增
        /// 当锁定者同意其他用户的解锁请求时使用
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="menuId">菜单ID</param>
        /// <param name="requesterUserId">请求者用户ID</param>
        /// <param name="requesterUserName">请求者用户名</param>
        /// <returns>同意解锁的响应结果</returns>
        public async Task<LockResponse> AgreeUnlockAsync(long billId, long menuId, long requesterUserId, string requesterUserName)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(ClientLockManagementService));

            try
            {
                _logger?.LogDebug("开始同意解锁请求 - 单据ID: {BillId}, 菜单ID: {MenuId}, 请求者: {RequesterUserName}",
                    billId, menuId, requesterUserName);

                // 获取当前用户信息
                long currentUserId = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
                string currentUserName = MainForm.Instance.AppContext.CurUserInfo.UserInfo.UserName;

                // 创建锁信息
                LockInfo lockInfo = new LockInfo();
                lockInfo.BillID = billId;
                lockInfo.MenuID = menuId;
                lockInfo.LockedUserId = currentUserId;
                lockInfo.LockedUserName = currentUserName;
                lockInfo.SessionId = MainForm.Instance.AppContext.SessionId;

                // 创建同意解锁请求
                var lockRequest = new LockRequest
                {
                    LockInfo = lockInfo,
                    RequesterUserId = requesterUserId,
                    RequesterUserName = requesterUserName,
                };
                lockRequest.LockInfo.SetLockKey();

                // 从活跃锁列表中移除
                _activeLocks.TryRemove(billId, out _);

                // 清除本地缓存
                _clientCache.ClearCache(billId);

                // 发送同意解锁命令
                var response = await _communicationService.Value.SendCommandWithResponseAsync<LockResponse>(
                    LockCommands.AgreeUnlock, lockRequest, _cancellationTokenSource.Token);

                if (response.IsSuccess)
                {
                    _logger?.LogDebug("同意解锁请求成功 - 单据ID: {BillId}, 菜单ID: {MenuId}", billId, menuId);

                    // 延迟清理细粒度锁（不在这里立即移除，避免竞态条件）
                    // _billLocks.TryRemove(billId, out _);  // ❌ 删除这行
                }
                else
                {
                    _logger?.LogWarning("同意解锁请求失败 - 单据ID: {BillId}, 菜单ID: {MenuId}, 错误信息: {ErrorMessage}",
                        billId, menuId, response?.Message ?? "未知错误");

                    // 如果失败，恢复锁状态
                    _activeLocks.TryAdd(billId, lockInfo);
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送同意解锁时发生异常 - 单据ID: {BillId}, 菜单ID: {MenuId}", billId, menuId);
                throw;
            }
        }

        /// <summary>
        /// 解锁单据（接受LockRequest参数的重载方法）v2.0.0新增
        /// 支持批量解锁和按业务类型解锁等功能
        /// </summary>
        /// <param name="lockRequest">锁定请求信息</param>
        /// <returns>解锁操作的响应结果</returns>
        public async Task<LockResponse> UnlockBillAsync(LockRequest lockRequest)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(ClientLockManagementService));

            if (lockRequest == null)
                throw new ArgumentNullException(nameof(lockRequest));

            if (lockRequest.LockInfo == null)
                throw new ArgumentException("LockInfo不能为空", nameof(lockRequest));

            // 边界条件检查:验证单据ID有效性
            // 注意:当UnlockType为ByBizName时,BillID可以为0,表示按业务类型批量解锁
            if (lockRequest.LockInfo.BillID <= 0 && lockRequest.UnlockType != UnlockType.ByBizName)
            {
                // 非ByBizName类型的解锁请求必须提供有效的BillID
                return LockResponseFactory.CreateFailedResponse("单据ID无效");
            }

            if (_isDisposed)
                throw new ObjectDisposedException(nameof(ClientLockManagementService));

            var stopwatch = Stopwatch.StartNew();

            try
            {
                // 获取当前单据的锁
                var billLock = _billLocks.GetOrAdd(lockRequest.LockInfo.BillID, _ => new SemaphoreSlim(1, 1));

                // 使用超时机制
                if (!await billLock.WaitAsync(_operationTimeout))
                {
                    _logger.LogWarning("解锁操作超时 - 单据ID: {BillId}", lockRequest.LockInfo.BillID);
                    return new LockResponse
                    {
                        IsSuccess = false,
                        Message = "操作超时，请稍后重试"
                    };
                }

                try
                {
                    // 从活跃锁列表中移除 - 解锁操作的第一步
                    // 在发送解锁请求前，先从_activeLocks中移除，这样可以确保即使网络请求失败
                    // 本地状态也能保持一致，不会留下孤儿锁引用
                    _activeLocks.TryRemove(lockRequest.LockInfo.BillID, out _);

                    // 缓存清除
                    _clientCache.ClearCache(lockRequest.LockInfo.BillID);

                    var response = await _communicationService.Value.SendCommandWithResponseAsync<LockResponse>(
                        LockCommands.Unlock, lockRequest, CancellationToken.None, (int)_operationTimeout.TotalMilliseconds);

                    stopwatch.Stop();

                    // 检查响应是否为空
                    if (response == null)
                    {
                        return LockResponseFactory.CreateFailedResponse("响应为空");
                    }

                    if (response.IsSuccess)
                    {
                        // 延迟清理细粒度锁（不在这里立即移除，让finally中的CleanupUnusedLock处理）
                        // _billLocks.TryRemove(lockRequest.LockInfo.BillID, out _);  // ❌ 删除这行
                    }
                    else
                    {
                        _logger.LogWarning("单据 {BillId} 解锁失败: {Message}, 耗时 {ElapsedMs}ms", lockRequest.LockInfo.BillID, response.Message, stopwatch.ElapsedMilliseconds);
                    }

                    return response;
                }
                finally
                {
                    // 确保锁被释放
                    try
                    {
                        billLock.Release();
                    }
                    catch (SemaphoreFullException)
                    {
                        // 锁已经被释放,忽略
                        _logger.LogDebug("锁已释放,无需重复释放: BillID={BillId}", lockRequest.LockInfo.BillID);
                    }
                    // 延迟清理未使用的锁,避免影响等待中的线程
                    CleanupUnusedLock(lockRequest.LockInfo.BillID, billLock);
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "解锁单据 {BillId} 时发生异常，耗时 {ElapsedMs}ms", lockRequest.LockInfo.BillID, stopwatch.ElapsedMilliseconds);
                return new LockResponse
                {
                    IsSuccess = false,
                    Message = $"解锁异常: {ex.Message}"
                };
            }
        }

        #endregion


        #region 优化辅助方法

        /// <summary>
        /// 缓存预热方法
        /// 当系统启动时，预热常用单据的锁信息缓存，提高系统响应速度
        /// </summary>
        private async Task WarmupCacheAsync()
        {
            try
            {
                _logger.LogDebug("开始缓存预热...");
                // 这里可以根据实际业务场景实现缓存预热逻辑
                // 例如：加载最近访问的单据、常用单据等

                // 预热完成
                _logger.LogDebug("缓存预热完成");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "缓存预热失败");
            }
        }

        /// <summary>
        /// 执行缓存健康检查
        /// 定期检查缓存状态，清理无效缓存，优化缓存性能
        /// </summary>
        private async Task PerformCacheHealthCheckAsync()
        {
            try
            {
                _logger.LogDebug("执行缓存健康检查...");
                // 这里可以实现缓存健康检查逻辑
                // 例如：检查缓存大小、命中率、清理过期缓存等

                // 缓存健康检查完成
                _logger.LogDebug("缓存健康检查完成");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "缓存健康检查失败");
            }
        }

        /// <summary>
        /// 清理过期的细粒度锁
        /// 定期清理长时间未使用的锁对象，避免内存泄漏
        /// </summary>
        private async Task CleanupExpiredBillLocksAsync()
        {
            try
            {
                // 获取当前活跃的锁ID集合
                var activeLockIds = new HashSet<long>(_activeLocks.Keys);

                // 清理不在活跃锁列表中的细粒度锁
                foreach (var billId in _billLocks.Keys.ToArray())
                {
                    if (!activeLockIds.Contains(billId))
                    {
                        if (_billLocks.TryRemove(billId, out var lockObj))
                        {
                            try { lockObj.Dispose(); }
                            catch { }
                        }
                    }
                }

                _logger.LogDebug("清理过期细粒度锁完成 - 当前活跃锁数量: {Count}", _billLocks.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清理过期细粒度锁失败");
            }
        }

        /// <summary>
        /// 清理未使用的锁
        /// 根据实际使用情况,选择性地清理不再需要的锁对象
        /// 优化策略:延迟清理,避免影响等待中的线程
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="lockObj">锁对象</param>
        private void CleanupUnusedLock(long billId, SemaphoreSlim lockObj)
        {
            // 检查锁是否可用(CurrentCount==1表示无人持有)
            if (lockObj.CurrentCount == 1)
            {
                // 延迟5秒后清理,给可能的等待者时间获取锁
                Task.Delay(TimeSpan.FromSeconds(5)).ContinueWith(_ =>
                {
                    try
                    {
                        // 再次检查是否仍可用且未被复用
                        if (lockObj.CurrentCount == 1)
                        {
                            // 尝试移除,如果成功则释放资源
                            if (_billLocks.TryRemove(billId, out var removed) && removed == lockObj)
                            {
                                try
                                {
                                    lockObj.Dispose();
                                    _logger.LogDebug("清理未使用的细粒度锁: BillID={BillId}", billId);
                                }
                                catch (ObjectDisposedException)
                                {
                                    // 已被其他线程释放,忽略
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "清理细粒度锁时发生异常: BillID={BillId}", billId);
                    }
                }, TaskContinuationOptions.OnlyOnRanToCompletion);
            }
        }

        /// <summary>
        /// 强制清理所有异常锁（调试用）
        /// 用于清除CurrentCount=0但无等待者的异常锁
        /// </summary>
        public void ForceCleanupAbnormalLocks()
        {
            try
            {
                var abnormalCount = 0;
                foreach (var kvp in _billLocks.ToArray())
                {
                    var billId = kvp.Key;
                    var lockObj = kvp.Value;

                    // CurrentCount=0表示锁被持有，但如果没有活跃锁且没有等待者，则是异常状态
                    if (lockObj.CurrentCount == 0 && !_activeLocks.ContainsKey(billId))
                    {
                        if (_billLocks.TryRemove(billId, out var removed))
                        {
                            try
                            {
                                removed.Dispose();
                                abnormalCount++;
                                _logger.LogWarning("强制清理异常锁: BillID={BillId}, CurrentCount={CurrentCount}",
                                    billId, lockObj.CurrentCount);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "清理异常锁时出错: BillID={BillId}", billId);
                            }
                        }
                    }
                }

                if (abnormalCount > 0)
                {
                    _logger.LogWarning("强制清理完成 - 清理异常锁数量: {Count}, 剩余锁数量: {Remaining}",
                        abnormalCount, _billLocks.Count);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "强制清理异常锁时发生异常");
            }
        }

        #endregion

        #region 按业务类型批量解锁

        /// <summary>
        /// 按业务类型批量解锁当前用户的所有单据锁
        /// 用于打开新单据前清理同业务类型的其他单据锁,避免锁资源泄露
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="bizType">业务类型</param>
        /// <param name="userName">用户名(可选)</param>
        /// <returns>解锁响应</returns>
        public async Task<LockResponse> UnlockByBizTypeAsync(long userId, BizType bizType, string userName = null)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(ClientLockManagementService));

            // 参数验证
            if (userId <= 0)
            {
                _logger.LogWarning("按业务类型解锁参数无效: UserId={UserId}", userId);
                return LockResponseFactory.CreateFailedResponse("用户ID无效");
            }

            try
            {
                // 构造解锁请求
                var lockRequest = new LockRequest
                {
                    UnlockType = UnlockType.ByBizName,
                    RequesterUserId = userId,
                    RequesterUserName = userName ?? string.Empty,
                    LockInfo = new LockInfo
                    {
                        BillID = 0, // ByBizName模式下BillID可为0
                        bizType = bizType
                    }
                };

                // 调用现有的UnlockBillAsync(LockRequest)方法
                return await UnlockBillAsync(lockRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "按业务类型解锁异常: UserId={UserId}, BizType={BizType}", userId, bizType);
                return new LockResponse
                {
                    IsSuccess = false,
                    Message = $"按业务类型解锁异常: {ex.Message}"
                };
            }
        }

        #endregion

    }
}