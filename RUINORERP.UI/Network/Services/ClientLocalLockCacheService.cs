using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Common.Extensions;
using RUINORERP.PacketSpec.Models.Lock;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.UI.Network.Interfaces;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 客户端锁缓存管理器
    /// 提供本地缓存以减少网络请求，提升用户体验
    /// 优化版本：使用接口解耦，避免循环依赖
    /// </summary>
    public class ClientLocalLockCacheService : IDisposable
    {
        private readonly ConcurrentDictionary<long, LockInfo> _localCache;
        private readonly Timer _cleanupTimer;
        private readonly Timer _syncTimer;
        private readonly ILogger<ClientLocalLockCacheService> _logger;
        private readonly ILockStatusProvider _lockStatusProvider;
        private readonly IUserInfoProvider _userInfoProvider;
        private readonly object _syncLock = new object();

        /// <summary>
        /// 缓存过期时间（分钟）
        /// </summary>
        private const int CACHE_EXPIRY_MINUTES = 5;

        /// <summary>
        /// 同步间隔（分钟）
        /// </summary>
        private const int SYNC_INTERVAL_MINUTES = 2;


        private long currentUserId = 0;
        private string currentUserName = string.Empty;
        /// <summary>
        /// 构造函数 - 优化版本，使用接口解耦避免循环依赖
        /// </summary>
        /// <param name="lockStatusProvider">锁状态提供者</param>
        /// <param name="userInfoProvider">用户信息提供者（可选）</param>
        /// <param name="logger">日志记录器</param>
        public ClientLocalLockCacheService(
            ILockStatusProvider lockStatusProvider,
            IUserInfoProvider userInfoProvider = null,
            ILogger<ClientLocalLockCacheService> logger = null)
        {
            _lockStatusProvider = lockStatusProvider ?? throw new ArgumentNullException(nameof(lockStatusProvider));
            _userInfoProvider = userInfoProvider ?? new DefaultUserInfoProvider();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _localCache = new ConcurrentDictionary<long, LockInfo>();

            // 获取当前用户信息 - 统一获取逻辑
            var userInfo = _userInfoProvider.GetCurrentUserInfo();
            currentUserId = userInfo.userId;
            currentUserName = userInfo.userName;

            // 启动定时清理过期缓存的定时器
            _cleanupTimer = new Timer(CleanupExpiredCache, null,
                TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));

            // 启动定时同步活跃锁的定时器
            _syncTimer = new Timer(SyncActiveLocks, null,
                TimeSpan.FromMinutes(SYNC_INTERVAL_MINUTES), TimeSpan.FromMinutes(SYNC_INTERVAL_MINUTES));

            _logger.LogInformation("客户端锁缓存管理器已初始化");
        }



        /// <summary>
        /// 统一的缓存获取方法 - 优化版本
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="requireLockInfo">是否需要完整的锁定信息</param>
        /// <returns>缓存结果</returns>
        private async Task<(LockInfo LockInfo, bool FromCache)> GetFromCacheOrServerAsync(long billId, bool requireLockInfo = false)
        {
            try
            {
                // 先从本地缓存查询
                if (_localCache.TryGetValue(billId, out var cachedInfo))
                {
                    if (!cachedInfo.IsExpired)
                    {
                        _logger.LogDebug($"从缓存获取锁定信息: 文档 {billId}, 锁定状态: {cachedInfo.IsLocked}");
                        return (cachedInfo, true);
                    }

                    // 缓存已过期，移除
                    _localCache.TryRemove(billId, out _);
                }

                // 缓存未命中或已过期，查询服务器
                if (_lockStatusProvider != null)
                {
                    var lockResponse = await _lockStatusProvider.CheckLockStatusAsync(billId);
                    if (lockResponse?.IsSuccess == true && lockResponse.LockInfo != null)
                    {
                        UpdateCache(lockResponse.LockInfo);
                        return (lockResponse.LockInfo, false);
                    }
                }

                // 如果无法从服务器获取，返回未锁定状态
                var unlockedInfo = new LockInfo
                {
                    BillID = billId,
                    IsLocked = false,
                    UserId = currentUserId,
                    UserName = currentUserName,
                    LastUpdateTime = DateTime.Now,
                    ExpireTime = DateTime.Now.AddMinutes(CACHE_EXPIRY_MINUTES)
                };

                UpdateCache(unlockedInfo);
                return (unlockedInfo, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取文档 {billId} 锁定信息失败");
                return (null, false);
            }
        }

        /// <summary>
        /// 检查文档是否被锁定（优化版本）
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <returns>是否被锁定</returns>
        public async Task<bool> IsLockedAsync(long billId)
        {
            var result = await GetFromCacheOrServerAsync(billId, false);
            return result.LockInfo?.IsLocked ?? false;
        }

        /// <summary>
        /// 获取锁定信息（优化版本）
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <returns>锁定信息</returns>
        public async Task<LockInfo> GetLockInfoAsync(long billId)
        {
            var result = await GetFromCacheOrServerAsync(billId, true);
            return result.LockInfo?.IsLocked == true ? result.LockInfo : null;
        }


        /// <summary>
        /// 解锁文档（更新缓存）
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns>是否解锁成功</returns>
        public async Task<bool> UnlockAsync(long billId, long userId)
        {
            try
            {

                // 解锁成功，更新缓存
                UpdateCacheAfterUnlock(billId);

                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"解锁文档 {billId} 失败");
                return false;
            }
        }

        /// <summary>
        /// 刷新锁的过期时间
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns>是否刷新成功</returns>
        public async Task<bool> RefreshLockAsync(long billId, long userId)
        {
            try
            {

                // 刷新成功，更新缓存的过期时间
                if (_localCache.TryGetValue(billId, out var cachedInfo))
                {
                    cachedInfo.ExpireTime = DateTime.Now.AddMinutes(CACHE_EXPIRY_MINUTES);
                    cachedInfo.LastUpdateTime = DateTime.Now;
                }
                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"刷新文档 {billId} 的锁过期时间失败");
                return false;
            }
        }

        /// <summary>
        /// 批量检查锁定状态
        /// </summary>
        /// <param name="billIds">单据ID列表</param>
        /// <returns>锁定状态字典</returns>
        public async Task<Dictionary<long, bool>> BatchCheckLockStatusAsync(List<long> billIds)
        {
            try
            {
                var result = new Dictionary<long, bool>();
                var needQueryFromServer = new List<long>();
                //有问题
                // 先从缓存中获取
                foreach (var billId in billIds)
                {
                    if (_localCache.TryGetValue(billId, out var cachedInfo))
                    {
                        if (!cachedInfo.IsExpired)
                        {
                            result[billId] = cachedInfo.IsLocked;
                        }
                        else
                        {
                            needQueryFromServer.Add(billId);
                        }
                    }
                    else
                    {
                        needQueryFromServer.Add(billId);
                    }
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "批量检查锁定状态失败");
                return billIds.ToDictionary(id => id, _ => false);
            }
        }

        /// <summary>
        /// 清除指定单据的缓存
        /// </summary>
        /// <param name="billId">单据ID</param>
        public void ClearCache(long billId)
        {
            _localCache.TryRemove(billId, out _);
            _logger.LogDebug($"清除文档 {billId} 的锁缓存");
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public void ClearAllCache()
        {
            _localCache.Clear();
            _logger.LogInformation("清除所有锁缓存");
        }

        /// <summary>
        /// 获取缓存统计信息
        /// </summary>
        /// <returns>缓存统计</returns>
        public CacheStatistics GetCacheStatistics()
        {
            var cacheItems = _localCache.Values.ToList();
            return new CacheStatistics
            {
                TotalCachedItems = cacheItems.Count,
                ExpiredItems = cacheItems.Count(c => c.IsExpired),
                LockedItems = cacheItems.Count(c => c.IsLocked),
                OwnedByCurrentUserItems = cacheItems.Count(c => c.IsOwnedByCurrentUser(currentUserId)),
                LastCleanup = DateTime.Now
            };
        }



        /// <summary>
        /// 更新缓存（基于锁定信息）
        /// </summary>
        /// <param name="lockInfo">锁定信息</param>
        private void UpdateCache(LockInfo lockInfo)
        {
            try
            {
                _localCache.AddOrUpdate(lockInfo.BillID, lockInfo, (key, oldValue) => lockInfo);
                _logger.LogDebug($"更新锁缓存: 文档 {lockInfo.BillID}, 锁定用户: {lockInfo.UserName}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"更新锁缓存失败: 文档 {lockInfo?.BillID}");
            }
        }

        /// <summary>
        /// 更新缓存（解锁后）
        /// </summary>
        /// <param name="billId">单据ID</param>
        private void UpdateCacheAfterUnlock(long billId)
        {
            try
            {
                var cachedInfo = new LockInfo
                {
                    BillID = billId,
                    IsLocked = false,
                    LastUpdateTime = DateTime.Now,
                    ExpireTime = DateTime.Now.AddMinutes(CACHE_EXPIRY_MINUTES),
                    UserId = currentUserId,
                };

                _localCache.AddOrUpdate(billId, cachedInfo, (key, oldValue) => cachedInfo);
                _logger.LogDebug($"更新锁缓存（解锁）: 文档 {billId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"更新锁缓存失败: 文档 {billId}");
            }
        }

        /// <summary>
        /// 定时清理过期缓存
        /// </summary>
        /// <param name="state">状态对象</param>
        private void CleanupExpiredCache(object state)
        {
            try
            {
                var expiredKeys = _localCache
                    .Where(kvp => kvp.Value.IsExpired)
                    .Select(kvp => kvp.Key)
                    .ToList();

                foreach (var key in expiredKeys)
                {
                    _localCache.TryRemove(key, out _);
                }

                if (expiredKeys.Count > 0)
                {
                    _logger.LogDebug($"清理了 {expiredKeys.Count} 个过期锁缓存项");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清理过期锁缓存时发生错误");
            }
        }

        /// <summary>
        /// 定时同步活跃锁
        /// </summary>
        /// <param name="state">状态对象</param>
        private void SyncActiveLocks(object state)
        {
            try
            {
                var activeLocks = _localCache.Values
                    .Where(c => c.IsLocked && !c.IsExpired)
                    .Select(c => c.BillID)
                    .ToList();
                //在这里可以实现批量同步逻辑  向服务器同步
                //if (activeLocks.Count > 0)
                //{
                //    _logger.LogDebug($"同步 {activeLocks.Count} 个活跃锁的状态");
                //    这里可以批量查询服务器以更新缓存
                //    为避免阻塞，使用Task.Run
                //   _ = Task.Run(async () =>
                //   {
                //       try
                //       {
                //           foreach (var billId in activeLocks)
                //           {
                //               await QueryFromServerAsync(billId);
                //           }
                //       }
                //       catch (Exception ex)
                //       {
                //           _logger.LogError(ex, "同步活跃锁时发生错误");
                //       }
                //   });
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "同步活跃锁时发生错误");
            }
        }

        /// <summary>
        /// 判断是否为当前用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>是否为当前用户</returns>
        private bool IsCurrentUser(long userId)
        {
            // 这里应该从当前登录用户信息中获取
            // 暂时使用简单判断
            try
            {
                var currentUserId = RUINORERP.Model.Context.ApplicationContext.Current?.CurrentUser?.UserID ?? 0;
                return currentUserId == userId;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _cleanupTimer?.Dispose();
            _syncTimer?.Dispose();
            _localCache.Clear();
            _logger.LogInformation("客户端锁缓存管理器已释放资源");
        }
    }

    /// <summary>
    /// 缓存统计信息
    /// </summary>
    public class CacheStatistics
    {
        public int TotalCachedItems { get; set; }
        public int ExpiredItems { get; set; }
        public int LockedItems { get; set; }
        public int OwnedByCurrentUserItems { get; set; }
        public DateTime LastCleanup { get; set; }
    }
}