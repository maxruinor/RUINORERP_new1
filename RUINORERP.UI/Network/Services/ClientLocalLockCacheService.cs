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
        private const int CACHE_EXPIRY_MINUTES = 15;

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
        /// 检查文档是否被锁定（支持MenuID参数）
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="menuId">菜单ID</param>
        /// <returns>是否被锁定</returns>
        public async Task<bool> IsLockedAsync(long billId, int menuId = 0)
        {
            var lockInfo = await GetLockInfoAsync(billId, menuId);
            
            // 线程安全检查：确保只有有效的锁定信息才返回true
            return lockInfo != null && lockInfo.IsLocked && !lockInfo.IsExpired;
        }

        /// <summary>
        /// 获取锁定信息（支持MenuID参数）
        /// <para>优化说明：线程安全实现，确保只返回有效的锁定信息，防止返回过期或无效的锁定状态</para>
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="menuId">菜单ID（可选，用于额外的权限验证）</param>
        /// <returns>锁定信息，如果没有有效的锁定信息则返回null</returns>
        public async Task<LockInfo> GetLockInfoAsync(long billId, long menuId = 0)
        {
            // 调用内部方法获取缓存或默认值
            var result = await GetFromCacheOrServerAsync(billId, true);
            
            // 线程安全检查：确保返回的锁定信息是有效的
            // 三重验证：确保锁定信息存在、已锁定状态、未过期
            if (result.LockInfo != null && result.LockInfo.IsLocked && !result.LockInfo.IsExpired)
            {
                // 验证MenuID匹配（如果提供）
                if (menuId > 0 && result.LockInfo.MenuID > 0 && result.LockInfo.MenuID != menuId)
                {
                    _logger.LogWarning($"MenuID不匹配，单据[{billId}]缓存的MenuID:[{result.LockInfo.MenuID}]，请求的MenuID:[{menuId}]");
                }
                return result.LockInfo;
            }
            // 如果没有有效的锁定信息，返回null
            return null;
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
        /// 获取所有缓存的锁定信息
        /// 供管理界面使用，避免反射访问私有字段
        /// </summary>
        /// <returns>所有缓存的锁定信息列表</returns>
        public List<LockInfo> GetAllLockInfos()
        {
            try
            {
                // 返回缓存中所有项目的副本
                return _localCache.Values.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取所有锁定信息失败");
                return new List<LockInfo>();
            }
        }

        /// <summary>
        /// 更新缓存项（公共方法，用于外部调用）
        /// <para>优化说明：v2.0.0 - 增强了参数验证、添加了完整的过期时间管理、确保了操作的原子性和线程安全性</para>
        /// </summary>
        /// <param name="lockInfo">锁定信息对象</param>
        /// <exception cref="ArgumentNullException">当锁定信息为空时抛出</exception>
        /// <exception cref="ArgumentException">当单据ID无效时抛出</exception>
        public void UpdateCacheItem(LockInfo lockInfo)
        {
            // 参数验证：确保锁定信息不为空
            if (lockInfo == null)
                throw new ArgumentNullException(nameof(lockInfo));
            
            // 添加线程安全的验证：确保锁定信息有效
            if (lockInfo.BillID <= 0)
                throw new ArgumentException("单据ID必须大于0", nameof(lockInfo));
            
            // 确保设置过期时间：防止缓存项无限期保留
            if (lockInfo.ExpireTime == DateTime.MinValue)
            {
                lockInfo.ExpireTime = DateTime.Now.AddMinutes(CACHE_EXPIRY_MINUTES);
            }
            
            // 确保设置更新时间：用于冲突检测和版本控制
            if (lockInfo.LastUpdateTime == DateTime.MinValue)
            {
                lockInfo.LastUpdateTime = DateTime.Now;
            }
            
            // 线程安全的更新操作：使用显式锁确保缓存更新的原子性
            // 防止并发更新导致的数据不一致或锁状态冲突
            lock (_syncLock)
            {
                UpdateCache(lockInfo);
            }
            
            // 记录详细的操作日志，便于问题排查和性能监控
            _logger.LogDebug($"通过外部调用更新锁缓存: 文档 {lockInfo.BillID}, 锁定状态: {lockInfo.IsLocked}, 锁定用户: {lockInfo.LockedUserName}");
        }

        /// <summary>
        /// 清除指定单据的缓存
        /// 确保线程安全的清除操作
        /// </summary>
        /// <param name="billId">单据ID</param>
        public void ClearCache(long billId)
        {
            if (billId <= 0)
                return;
            
            // 线程安全的移除操作
            bool removed = _localCache.TryRemove(billId, out var removedInfo);
            
            if (removed && removedInfo != null)
            {
                _logger.LogDebug($"清除文档 {billId} 的锁缓存, 之前状态: {(removedInfo.IsLocked ? "已锁定" : "未锁定")}");
            }
            else
            {
                _logger.LogDebug($"文档 {billId} 的锁缓存不存在，无需清除");
            }
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
        /// 确保操作的原子性和线程安全性，正确处理所有LockInfo属性
        /// </summary>
        /// <param name="lockInfo">锁定信息</param>
        private void UpdateCache(LockInfo lockInfo)
        {
            try
            {
                // 参数验证
                if (lockInfo == null || lockInfo.BillID <= 0)
                {
                    _logger.LogWarning("无效的锁定信息: {BillID}", lockInfo?.BillID);
                    return;
                }

                // 准备缓存条目，确保所有必要属性都被正确设置
                var cacheEntry = PrepareCacheEntry(lockInfo);
                
                // 原子操作：确保缓存更新的原子性
                _localCache.AddOrUpdate(lockInfo.BillID, cacheEntry, (key, oldValue) => 
                {
                    // 更复杂的更新策略，考虑更多属性和场景
                    if (ShouldUpdateCache(oldValue, cacheEntry))
                    {
                        _logger.LogDebug($"更新锁缓存: 文档 {lockInfo.BillID}, 单据编号: {lockInfo.BillNo}, 锁定用户: {lockInfo.LockedUserName}, 菜单ID: {lockInfo.MenuID}");
                        return cacheEntry;
                    }
                    return oldValue; // 保留较新的缓存项
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"更新锁缓存失败: 文档 {lockInfo?.BillID}, 单据编号: {lockInfo?.BillNo}");
            }
        }

        /// <summary>
        /// 准备缓存条目，确保所有必要属性都被正确设置
        /// </summary>
        /// <param name="lockInfo">原始锁定信息</param>
        /// <returns>准备好的缓存条目</returns>
        private LockInfo PrepareCacheEntry(LockInfo lockInfo)
        {
            // 深拷贝锁定信息，确保缓存中存储的是独立的副本
            var cacheEntry = lockInfo.Clone() as LockInfo;
            
            // 确保所有必要属性都被正确设置
            if (cacheEntry == null)
            {
                cacheEntry = new LockInfo();
                // 复制所有重要属性
                CopyLockInfoProperties(lockInfo, cacheEntry);
            }
            else
            {
                // 确保关键属性不为空或默认值
                EnsureCriticalProperties(cacheEntry);
            }
            
            return cacheEntry;
        }

        /// <summary>
        /// 复制LockInfo的所有属性
        /// </summary>
        /// <param name="source">源对象</param>
        /// <param name="target">目标对象</param>
        private void CopyLockInfoProperties(LockInfo source, LockInfo target)
        {
            // 复制所有基本属性
            target.BillID = source.BillID;
            target.BillNo = source.BillNo ?? string.Empty;
            target.LockedUserId = source.LockedUserId;
            target.LockedUserName = source.LockedUserName ?? string.Empty;
            target.MenuID = source.MenuID;
            target.MenuName = source.MenuName ?? string.Empty;
            target.BizName = source.BizName ?? string.Empty;
            target.Remark = source.Remark ?? string.Empty;
            target.SessionId = source.SessionId ?? string.Empty;
            target.IsLocked = source.IsLocked;
            target.Type = source.Type;
            target.LockTime = source.LockTime;
            target.LastUpdateTime = source.LastUpdateTime;
            target.LastHeartbeat = source.LastHeartbeat;
            target.HeartbeatCount = source.HeartbeatCount;
            target.Duration = source.Duration;
            
            // 确保过期时间被正确设置
            if (source.ExpireTime.HasValue)
            {
                target.ExpireTime = source.ExpireTime;
            }
            else
            {
                target.ExpireTime = DateTime.Now.AddMinutes(CACHE_EXPIRY_MINUTES);
            }
        }

        /// <summary>
        /// 确保关键属性不为空或默认值
        /// </summary>
        /// <param name="lockInfo">锁定信息</param>
        private void EnsureCriticalProperties(LockInfo lockInfo)
        {
            // 确保字符串属性不为null
            lockInfo.BillNo = lockInfo.BillNo ?? string.Empty;
            lockInfo.LockedUserName = lockInfo.LockedUserName ?? string.Empty;
            lockInfo.MenuName = lockInfo.MenuName ?? string.Empty;
            lockInfo.BizName = lockInfo.BizName ?? string.Empty;
            lockInfo.Remark = lockInfo.Remark ?? string.Empty;
            lockInfo.SessionId = lockInfo.SessionId ?? string.Empty;
            
            // 确保时间戳被正确设置
            if (lockInfo.LastUpdateTime == DateTime.MinValue)
            {
                lockInfo.LastUpdateTime = DateTime.Now;
            }
            
            if (lockInfo.LastHeartbeat == DateTime.MinValue)
            {
                lockInfo.LastHeartbeat = DateTime.Now;
            }
            
            if (lockInfo.LockTime == DateTime.MinValue && lockInfo.IsLocked)
            {
                lockInfo.LockTime = DateTime.Now;
            }
            
            // 确保过期时间被设置
            if (!lockInfo.ExpireTime.HasValue || lockInfo.ExpireTime.Value <= DateTime.Now)
            {
                lockInfo.ExpireTime = DateTime.Now.AddMinutes(CACHE_EXPIRY_MINUTES);
            }
        }

        /// <summary>
        /// 决定是否应该更新缓存
        /// 考虑多种因素，包括更新时间、过期状态、锁定状态等
        /// </summary>
        /// <param name="oldValue">旧缓存值</param>
        /// <param name="newValue">新缓存值</param>
        /// <returns>是否应该更新</returns>
        private bool ShouldUpdateCache(LockInfo oldValue, LockInfo newValue)
        {
            // 1. 如果旧值已过期，总是更新
            if (oldValue.IsExpired)
            {
                return true;
            }
            
            // 2. 如果新值的更新时间晚于旧值，更新
            if (newValue.LastUpdateTime > oldValue.LastUpdateTime)
            {
                return true;
            }
            
            // 3. 如果锁定状态发生变化，更新
            if (newValue.IsLocked != oldValue.IsLocked)
            {
                return true;
            }
            
            // 4. 如果锁定人发生变化，更新
            if (newValue.IsLocked && 
                (newValue.LockedUserId != oldValue.LockedUserId || 
                 !string.Equals(newValue.LockedUserName, oldValue.LockedUserName, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }
            
            // 5. 如果关键业务信息发生变化，更新
            if (!string.Equals(newValue.BillNo, oldValue.BillNo, StringComparison.OrdinalIgnoreCase) ||
                newValue.MenuID != oldValue.MenuID ||
                !string.Equals(newValue.BizName, oldValue.BizName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            
            return false;
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