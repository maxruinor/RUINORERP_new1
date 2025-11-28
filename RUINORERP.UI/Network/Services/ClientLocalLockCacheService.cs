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

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 客户端锁缓存管理器
    /// 提供本地缓存功能，不再与服务器直接交互
    /// </summary>
    public class ClientLocalLockCacheService : IDisposable
    {
        private readonly ConcurrentDictionary<long, LockInfo> _localCache;
        private readonly Timer _cleanupTimer;
        private readonly Timer _syncTimer;
        private readonly ILogger<ClientLocalLockCacheService> _logger;
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
        /// 构造函数 - 移除对ILockStatusProvider的依赖
        /// </summary>
        /// <param name="userInfoProvider">用户信息提供者（可选）</param>
        /// <param name="logger">日志记录器</param>
        public ClientLocalLockCacheService(
            IUserInfoProvider userInfoProvider = null,
            ILogger<ClientLocalLockCacheService> logger = null)
        {
            _userInfoProvider = userInfoProvider ?? new DefaultUserInfoProvider();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _localCache = new ConcurrentDictionary<long, LockInfo>();

            // 获取当前用户信息
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
        /// 统一的缓存获取方法 - 只从本地缓存获取
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="requireLockInfo">是否需要完整的锁定信息</param>
        /// <returns>缓存结果</returns>
        private async Task<(LockInfo LockInfo, bool FromCache)> GetFromCacheOrServerAsync(long billId, bool requireLockInfo = false)
        {
            try
            {
                // 只从本地缓存查询
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

                // 如果缓存中没有，返回未锁定状态的信息
                var unlockedInfo = new LockInfo
                {
                    BillID = billId,
                    IsLocked = false,
                    LockedUserId = currentUserId,
                    LockedUserName = currentUserName,
                    LastUpdateTime = DateTime.Now,
                    ExpireTime = DateTime.Now.AddMinutes(CACHE_EXPIRY_MINUTES)
                };

                UpdateCache(unlockedInfo);
                return (unlockedInfo, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取文档 {billId} 锁定信息失败");
                return (null, false);
            }
        }

        /// <summary>
        /// 检查文档是否被锁定
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <returns>是否被锁定</returns>
        public async Task<bool> IsLockedAsync(long billId)
        {
            var result = await GetFromCacheOrServerAsync(billId, false);
            return result.LockInfo?.IsLocked ?? false;
        }

        /// <summary>
        /// 获取锁定信息
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
                // 只从本地缓存查询
                if (_localCache.TryGetValue(billId, out var cachedInfo))
                {
                    // 缓存已过期，移除
                    _localCache.TryRemove(billId, out _);
                }
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
                
                // 只从缓存中获取
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
                            _localCache.TryRemove(billId, out _);
                            result[billId] = false;
                        }
                    }
                    else
                    {
                        result[billId] = false;
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
        /// 更新缓存项（公共方法，用于外部调用）
        /// </summary>
        /// <param name="lockInfo">锁定信息</param>
        public void UpdateCacheItem(LockInfo lockInfo)
        {
            if (lockInfo == null)
                throw new ArgumentNullException(nameof(lockInfo));
            
            UpdateCache(lockInfo);
            _logger.LogDebug($"通过外部调用更新锁缓存: 文档 {lockInfo.BillID}, 锁定状态: {lockInfo.IsLocked}");
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
                _logger.LogDebug($"更新锁缓存: 文档 {lockInfo.BillID}, 锁定用户: {lockInfo.LockedUserName}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"更新锁缓存失败: 文档 {lockInfo?.BillID}");
            }
        }

    

        /// <summary>
        /// 清理过期缓存
        /// </summary>
        /// <param name="state">状态对象</param>
        private void CleanupExpiredCache(object state)
        {
            try
            {
                int removedCount = 0;
                var now = DateTime.Now;

                foreach (var key in _localCache.Keys.ToList())
                {
                    if (_localCache.TryGetValue(key, out var lockInfo) && lockInfo.IsExpired)
                    {
                        if (_localCache.TryRemove(key, out _))
                        {
                            removedCount++;
                            _logger.LogDebug($"清理过期缓存: 文档 {key}");
                        }
                    }
                }

                if (removedCount > 0)
                {
                    _logger.LogInformation($"清理了 {removedCount} 个过期的锁缓存项");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清理过期缓存时发生异常");
            }
        }

        /// <summary>
        /// 同步活跃锁
        /// </summary>
        /// <param name="state">状态对象</param>
        private void SyncActiveLocks(object state)
        {
            try
            {
                // 这里只维护本地缓存，不再与服务器同步
                var activeLocks = _localCache.Values.Count(l => l.IsLocked && !l.IsExpired);
                _logger.LogInformation($"当前活跃锁数量: {activeLocks}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "同步活跃锁时发生异常");
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源（内部方法）
        /// </summary>
        /// <param name="disposing">是否释放托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _cleanupTimer?.Dispose();
                _syncTimer?.Dispose();
                _localCache.Clear();
            }
        }

        /// <summary>
        /// 缓存统计信息类
        /// </summary>
        public class CacheStatistics
        {
            public int TotalCachedItems { get; set; }
            public int ExpiredItems { get; set; }
            public int LockedItems { get; set; }
            public int OwnedByCurrentUserItems { get; set; }
            public DateTime LastCleanup { get; set; }

            public override string ToString()
            {
                return $"总缓存项: {TotalCachedItems}, 过期项: {ExpiredItems}, 锁定项: {LockedItems}, 当前用户锁定项: {OwnedByCurrentUserItems}";
            }
        }
    }
    
    /// <summary>
    /// 默认用户信息提供者
    /// </summary>
    public class DefaultUserInfoProvider : IUserInfoProvider
    {
        public (long userId, string userName) GetCurrentUserInfo()
        {
            // 返回默认值，实际使用时应该被替换为真实的用户信息提供者
            return (0, "DefaultUser");
        }
    }
    
    /// <summary>
    /// 用户信息提供者接口
    /// </summary>
    public interface IUserInfoProvider
    {
        (long userId, string userName) GetCurrentUserInfo();
    }
}