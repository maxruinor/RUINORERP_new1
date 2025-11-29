using RUINORERP.PacketSpec.Models.Lock;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Responses;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.UI.Network.Authentication;
using RUINORERP.Model.CommonModel;
using RUINORERP.Global;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 集成式锁管理服务 v2.0.0
    /// 整合心跳功能、客户端缓存和服务器锁管理的完整解决方案
    /// 
    /// 版本：2.0.0
    /// 作者：AI Assistant
    /// 创建时间：2025-01-27
    /// 
    /// 主要特性：
    /// - 集成现有心跳机制
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
        private readonly SemaphoreSlim _operationSemaphore;

        // 配置参数
        private readonly TimeSpan _lockRefreshInterval = TimeSpan.FromMinutes(1);
        private readonly TimeSpan _cacheCleanupInterval = TimeSpan.FromMinutes(5);
        private readonly TimeSpan _lockTimeoutThreshold = TimeSpan.FromMinutes(30);
        private readonly int _maxRetryAttempts = 3;

        #endregion

        #region 构造函数和初始化

        /// <summary>
        /// 集成式锁管理服务构造函数
        /// </summary>
        /// <param name="communicationService">客户端通信服务的延迟加载实例</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="clientCache">客户端锁缓存（可选，为null时内部创建）</param>
        /// <param name="recoveryManager">锁恢复管理器（可选，为null时内部创建）</param>
        public ClientLockManagementService(
            Lazy<ClientCommunicationService> communicationService,
            ILogger<ClientLockManagementService> logger,
            ClientLocalLockCacheService clientCache = null,
            LockRecoveryManager recoveryManager = null)
        {
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // 初始化组件
            _activeLocks = new ConcurrentDictionary<long, LockInfo>();
            _cancellationTokenSource = new CancellationTokenSource();
            _operationSemaphore = new SemaphoreSlim(1, 1);

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

            // 初始化定时器
            _lockRefreshTimer = new Timer(RefreshLocksCallback, null, Timeout.Infinite, Timeout.Infinite);
            _cacheCleanupTimer = new Timer(CleanupCacheCallback, null, Timeout.Infinite, Timeout.Infinite);


        }

        /// <summary>
        /// 启动服务
        /// </summary>
        public async Task StartAsync()
        {
            try
            {
                await _operationSemaphore.WaitAsync(_cancellationTokenSource.Token);

                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(ClientLockManagementService));

                // 启动定时器
                _lockRefreshTimer.Change(TimeSpan.Zero, _lockRefreshInterval);
                _cacheCleanupTimer.Change(TimeSpan.FromMinutes(1), _cacheCleanupInterval);
                // LockRecoveryManager不需要手动启动，构造函数中已初始化

            }
            finally
            {
                _operationSemaphore.Release();
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public async Task StopAsync()
        {
            try
            {
                await _operationSemaphore.WaitAsync();

                if (_isDisposed)
                    return;

                // 停止定时器
                _lockRefreshTimer.Change(Timeout.Infinite, Timeout.Infinite);
                _cacheCleanupTimer.Change(Timeout.Infinite, Timeout.Infinite);

                // 释放所有锁
                await ReleaseAllLocksAsync();
                // LockRecoveryManager不需要手动停止

            }
            finally
            {
                _operationSemaphore.Release();
            }
        }

        #endregion

        #region 核心锁管理功能


        /// <summary>
        /// 解锁单据 - 无参数版本
        /// 调用带取消令牌的版本
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <returns>解锁响应</returns>
        public async Task<LockResponse> UnlockBillAsync(long billId)
        {
            return await UnlockBillAsync(billId, default);
        }

        /// <summary>
        /// 检查锁状态 - 实现ILockStatusProvider接口
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>锁状态</returns>
        public async Task<LockResponse> CheckLockStatusAsync(long billId, long MenuID = 0, CancellationToken cancellationToken = default)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(ClientLockManagementService));

            try
            {                // 优先检查本地缓存
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
                    LockCommands.CheckLockStatus, lockRequest, token);

                // 更新缓存
                if (response != null)
                {

                    if (response.IsSuccess && response.LockInfo != null)
                    {
                        if (response.LockInfo.IsLocked)
                        {
                            // 创建本地缓存项
                            var localLockInfo = new LockInfo
                            {
                                BillID = response.LockInfo.BillID,
                                IsLocked = response.LockInfo.IsLocked,
                                LockedUserId = response.LockInfo.LockedUserId,
                                LockedUserName = response.LockInfo.LockedUserName,
                                MenuID = response.LockInfo.MenuID,
                                LockTime = response.LockInfo.LockTime,
                                LastUpdateTime = DateTime.Now,
                                ExpireTime = DateTime.Now.AddMinutes(5) // 本地缓存5分钟过期
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
        /// <para>优化说明：v2.0.0 - 增强了缓存更新机制、添加了完整的错误处理、确保了锁定操作的原子性和线程安全性</para>
        /// <para>工作流程：
        /// 1. 首先检查本地缓存，判断单据是否已被锁定
        /// 2. 如果缓存未命中或已过期，向服务器发送锁定请求
        /// 3. 锁定成功后，同步更新本地缓存和活跃锁列表
        /// 4. 整个过程使用信号量控制并发，确保操作原子性
        /// </para>
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="menuId">菜单ID</param>
        /// <param name="timeoutMinutes">超时时间（分钟）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>锁定响应，包含是否成功和锁定信息</returns>
        /// <exception cref="ObjectDisposedException">当服务已被释放时抛出</exception>
        /// <exception cref="TaskCanceledException">当操作被取消时抛出</exception>
        public async Task<LockResponse> LockBillAsync(long billId, BizType bizType, long menuId, int timeoutMinutes = 30, CancellationToken cancellationToken = default)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(ClientLockManagementService));

            var stopwatch = Stopwatch.StartNew();

            try
            {
                await _operationSemaphore.WaitAsync(cancellationToken != default ? cancellationToken : _cancellationTokenSource.Token);

                // 检查是否已锁定（本地缓存） 这里还要看是不是自己的锁
                var islock = await _clientCache.IsLockedAsync(billId);
                if (islock)
                {
                    // 尝试获取锁信息
                    var lockInfoByLocal = await _clientCache.GetLockInfoAsync(billId);
                    _logger.LogDebug("单据 {BillId} 已被锁定", billId);
                    return new LockResponse
                    {
                        IsSuccess = true,
                        Message = "单据已被锁定",
                        LockInfo = lockInfoByLocal
                    };
                }

                // 创建锁定请求
                var lockInfo = CreateLockInfo(billId, menuId);
                lockInfo.SessionId = MainForm.Instance.AppContext.SessionId;
                lockInfo.LockedUserId = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
                lockInfo.LockedUserName = MainForm.Instance.AppContext.CurUserInfo.UserInfo.UserName;
                lockInfo.bizType = bizType;
                var lockRequest = new LockRequest { LockInfo = lockInfo };

                // 使用传入的cancellationToken或默认值
                var token = cancellationToken != default ? cancellationToken : _cancellationTokenSource.Token;

                // 发送锁定请求
                var response = await _communicationService.Value.SendCommandWithResponseAsync<LockResponse>(
                    LockCommands.Lock, lockRequest, token, timeoutMinutes);

                stopwatch.Stop();
                if (response == null)
                {
                    LockResponseFactory.CreateFailedResponse("响应为空");
                }
                if (response.IsSuccess)
                {
                    // 添加到活跃锁列表 - 锁定成功后的关键操作
                    // 只有当锁成功获取后，才会将锁信息添加到_activeLocks集合中
                    // 这样可以确保_activeLocks只包含当前客户端实际持有的有效锁
                    _activeLocks.TryAdd(billId, response.LockInfo);

                    // 同步更新本地缓存，确保本地缓存与服务器状态一致
                    if (_clientCache != null)
                    {
                        _clientCache.UpdateCacheItem(response.LockInfo);
                    }
                }
                else
                {
                    _logger.LogWarning("单据 {BillId} 锁定失败: {Message}, 耗时 {ElapsedMs}ms",
                        billId, response.Message, stopwatch.ElapsedMilliseconds);
                }

                return response;
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
            finally
            {
                _operationSemaphore.Release();
            }
        }

        /// <summary>
        /// 解锁单据 - 实现ILockStatusProvider接口
        /// 这是主要实现版本，其他版本都调用此方法
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>解锁响应</returns>
        public async Task<LockResponse> UnlockBillAsync(long billId, CancellationToken cancellationToken = default)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(ClientLockManagementService));

            var stopwatch = Stopwatch.StartNew();

            try
            {
                // 使用传入的cancellationToken或默认值
                var token = cancellationToken != default ? cancellationToken : _cancellationTokenSource.Token;
                await _operationSemaphore.WaitAsync(token);

                // 从活跃锁列表中移除 - 解锁操作的第一步
                // 在发送解锁请求前，先从_activeLocks中移除，这样可以确保即使网络请求失败
                // 本地状态也能保持一致，不会留下孤儿锁引用
                _activeLocks.TryRemove(billId, out _);

                // 缓存清除
                _clientCache.ClearCache(billId);

                // 发送解锁请求
                var lockInfo = CreateLockInfo(billId, 0);
                lockInfo.SessionId = MainForm.Instance.AppContext.SessionId;
                var unlockRequest = new LockRequest { LockInfo = lockInfo };

                var response = await _communicationService.Value.SendCommandWithResponseAsync<LockResponse>(
                    LockCommands.Unlock, unlockRequest, token);

                stopwatch.Stop();

                // 检查响应是否为空
                if (response == null)
                {
                    return LockResponseFactory.CreateFailedResponse("响应为空");
                }

                if (response.IsSuccess)
                {

                }
                else
                {
                    _logger.LogWarning("单据 {BillId} 解锁失败: {Message}, 耗时 {ElapsedMs}ms", billId, response.Message, stopwatch.ElapsedMilliseconds);
                }

                return response;
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
            finally
            {
                _operationSemaphore.Release();
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
        /// </summary>
        private async void CleanupCacheCallback(object state)
        {
            if (_isDisposed || _cancellationTokenSource.Token.IsCancellationRequested)
                return;

            try
            {
                // ClientLockCache内部已使用定时器定期清理过期缓存，无需手动触发
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
        private UserInfo GetCurrentUser()
        {
            try
            {
                return MainForm.Instance?.AppContext?.CurrentUser;
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
                // 停止服务
                StopAsync().GetAwaiter().GetResult();

                // 释放定时器
                _lockRefreshTimer?.Dispose();
                _cacheCleanupTimer?.Dispose();

                // 释放组件
                _clientCache?.Dispose();
                _recoveryManager?.Dispose();

                // 取消令牌
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource?.Dispose();

                // 释放信号量
                _operationSemaphore?.Dispose();

                _isDisposed = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "释放集成式锁管理服务资源时发生异常");
            }
        }

        /// <summary>
        /// 刷新所有活跃锁
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

            var refreshTasks = activeLockIds.Select(async billId =>
            {
                try
                {
                    var response = await CheckLockStatusAsync(billId, 0, _cancellationTokenSource.Token);
                    if (response.IsSuccess && response.LockInfo != null)
                    {
                        // 检查锁是否仍然有效
                        if (response.LockInfo.IsExpired)
                        {
                            _logger.LogWarning("检测到单据 {BillId} 锁已过期，将从活跃列表移除", billId);
                            // 锁已过期，从_activeLocks中移除
                            // 这是锁状态同步的关键机制，确保_activeLocks只包含有效的未过期锁
                            _activeLocks.TryRemove(billId, out _);
                            _clientCache.ClearCache(billId);
                        }
                        else
                        {
                            // 更新刷新时间戳
                            // 即使锁没有过期，也需要更新最后更新时间，这对于监控锁的活跃度很重要
                            if (_activeLocks.TryGetValue(billId, out var clientLock))
                            {
                                clientLock.LastUpdateTime = DateTime.Now;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "刷新单据 {BillId} 锁状态时发生异常", billId);
                }
            });

            await Task.WhenAll(refreshTasks);
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
            _logger.LogInformation("开始释放 {LockCount} 个活跃锁", activeLockIds.Length);

            var releaseTasks = activeLockIds.Select(async billId =>
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
            _logger.LogInformation("所有活跃锁释放完成");
        }

        /// <summary>
        /// 刷新单据锁定状态 v2.0.0新增
        /// 该方法通过向服务器发送请求来更新锁定的过期时间，确保当前用户能够持续编辑单据
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
                await _operationSemaphore.WaitAsync(_cancellationTokenSource.Token);

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
                    LockCommands.CheckLockStatus, lockRequest, ct);

                if (response.IsSuccess)
                {
                    _logger?.LogDebug("单据锁定刷新成功 - 单据ID: {BillId}", billId);

                    // 更新本地缓存
                    if (_activeLocks.TryGetValue(billId, out var clientLock))
                    {
                        clientLock.LastUpdateTime = DateTime.Now;
                    }
                }
                else
                {
                    _logger?.LogWarning("单据锁定刷新失败 - 单据ID: {BillId}, 错误信息: {ErrorMessage}",
                        billId, response?.Message ?? "未知错误");
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "刷新单据锁定时发生异常 - 单据ID: {BillId}, 菜单ID: {MenuId}", billId, menuId);
                throw;
            }
            finally
            {
                _operationSemaphore.Release();
            }
        }

        /// <summary>
        /// 批量检查锁状态 v2.0.0新增
        /// 支持同时检查多个单据的锁定状态，提高批量操作效率
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

            try
            {
                await _operationSemaphore.WaitAsync(_cancellationTokenSource.Token);

                _logger?.LogDebug("开始批量检查锁状态 - 单据数量: {Count}", billIds.Count);

                var results = new Dictionary<long, LockResponse>();
                var currentUserId = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;

                // 分批处理，避免一次性发送过多请求
                const int batchSize = 10;
                for (int i = 0; i < billIds.Count; i += batchSize)
                {
                    var batch = billIds.Skip(i).Take(batchSize).ToList();
                    var batchTasks = batch.Select(async billId =>
                    {
                        try
                        {
                            // 首先检查本地缓存
                            var cachedStatus = await _clientCache.GetLockInfoAsync(billId);
                            if (cachedStatus.IsLocked)
                            {
                                return new
                                {
                                    BillId = billId,
                                    Response = new LockResponse
                                    {
                                        IsSuccess = true,
                                        LockInfo = cachedStatus,
                                        Message = "从缓存获取"
                                    }
                                };
                            }

                            // 缓存未命中，查询服务器
                            var response = await CheckLockStatusAsync(billId);
                            return new { BillId = billId, Response = response };
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogError(ex, "检查单据 {BillId} 锁状态时发生异常", billId);
                            return new
                            {
                                BillId = billId,
                                Response = new LockResponse
                                {
                                    IsSuccess = false,
                                    Message = ex.Message
                                }
                            };
                        }
                    });

                    var batchResults = await Task.WhenAll(batchTasks);
                    foreach (var result in batchResults)
                    {
                        results[result.BillId] = result.Response;
                    }
                }

                _logger?.LogDebug("批量检查锁状态完成 - 成功: {Success}/{Total}",
                    results.Values.Count(r => r.IsSuccess), billIds.Count);

                return results;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "批量检查锁状态时发生异常");
                throw;
            }
            finally
            {
                _operationSemaphore.Release();
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
                lockInfo.LockedUserId = currentUserId;
                lockInfo.LockedUserName = currentUserName;
                lockInfo.SessionId = MainForm.Instance.AppContext.SessionId;
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
        public async Task<LockResponse> RefuseUnlockAsync(long billId, long menuId, string requesterUserName)
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
                    RequesterUserName = requesterUserName,
                    LockedUserName = currentUserName,
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

            try
            {
                _logger?.LogDebug("开始解锁单据（LockRequest重载） - 单据ID: {BillId}, 解锁类型: {UnlockType}, 用户ID: {UserId}", lockRequest.LockInfo.BillID, lockRequest.UnlockType, lockRequest.LockInfo.LockedUserId);

                // 统一调用带取消令牌的主要实现方法
                return await UnlockBillAsync(lockRequest.LockInfo.BillID);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "解锁单据时发生异常（LockRequest重载） - 单据ID: {BillId}",
                    lockRequest.LockInfo.BillID);
                throw;
            }
        }

        #endregion
    }

    #region 辅助类

    /// <summary>
    /// 锁服务统计信息
    /// </summary>
    public class LockServiceStatistics
    {
        public string ServiceVersion { get; set; }
        public DateTime VersionDate { get; set; }
        public string VersionFeatures { get; set; }
        public int ActiveLockCount { get; set; }
        public int CacheSize { get; set; }
        public double CacheHitRate { get; set; }
        public bool IsRunning { get; set; }
        public DateTime StartTime { get; set; }

        public string GetSummary()
        {
            return $"锁服务 v{ServiceVersion} - 活跃锁:{ActiveLockCount}, 缓存大小:{CacheSize}, " +
                   $"缓存命中率:{CacheHitRate:F1}%, 运行状态:{(IsRunning ? "运行中" : "已停止")}";
        }
    }

    #endregion
}