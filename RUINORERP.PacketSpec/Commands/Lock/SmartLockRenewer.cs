using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading;

namespace RUINORERP.PacketSpec.Commands.Lock
{
    /// <summary>
    /// 智能锁续期管理器
    /// 负责自动为分布式锁进行续期，防止在用户操作期间锁过期导致的冲突
    /// </summary>
    public class SmartLockRenewer : IDisposable
    {
        private readonly ILogger<SmartLockRenewer> _logger;
        private readonly ILocalDistributedLock _distributedLock;
        private readonly ConcurrentDictionary<string, LockRenewalInfo> _locksToRenew;
        private readonly Timer _renewalTimer;
        private readonly TimeSpan _renewalInterval;
        private readonly TimeSpan _activityTimeout;
        private bool _disposed;

        /// <summary>
        /// 锁续期信息
        /// </summary>
        private class LockRenewalInfo
        {
            /// <summary>
            /// 锁键
            /// </summary>
            public string Key { get; }
            
            /// <summary>
            /// 锁值
            /// </summary>
            public string LockValue { get; }
            
            /// <summary>
            /// 续期时间间隔
            /// </summary>
            public TimeSpan RenewalInterval { get; }
            
            /// <summary>
            /// 上次活动时间
            /// </summary>
            public DateTime LastActivityTime { get; set; }
            
            /// <summary>
            /// 锁的总超时时间
            /// </summary>
            public TimeSpan LockTimeout { get; }

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="key">锁键</param>
            /// <param name="lockValue">锁值</param>
            /// <param name="lockTimeout">锁超时时间</param>
            /// <param name="renewalInterval">续期时间间隔</param>
            public LockRenewalInfo(string key, string lockValue, TimeSpan lockTimeout, TimeSpan renewalInterval)
            {
                Key = key;
                LockValue = lockValue;
                LockTimeout = lockTimeout;
                RenewalInterval = renewalInterval;
                LastActivityTime = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="distributedLock">分布式锁实例</param>
        /// <param name="renewalInterval">默认续期检查间隔，默认为30秒</param>
        /// <param name="activityTimeout">用户活动超时时间，默认为5分钟</param>
        public SmartLockRenewer(ILogger<SmartLockRenewer> logger, ILocalDistributedLock distributedLock,
            TimeSpan? renewalInterval = null, TimeSpan? activityTimeout = null)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _distributedLock = distributedLock ?? throw new ArgumentNullException(nameof(distributedLock));
            _locksToRenew = new ConcurrentDictionary<string, LockRenewalInfo>();
            _renewalInterval = renewalInterval ?? TimeSpan.FromSeconds(30);
            _activityTimeout = activityTimeout ?? TimeSpan.FromMinutes(5);
            _disposed = false;

            // 创建定时器用于定期检查和续期锁
            _renewalTimer = new Timer(async state => await CheckAndRenewLocksAsync(), null,
                _renewalInterval, _renewalInterval);

            _logger?.LogInformation("智能锁续期管理器已启动，续期检查间隔: {RenewalInterval}", _renewalInterval);
        }

        /// <summary>
        /// 注册锁用于自动续期
        /// </summary>
        /// <param name="key">锁键</param>
        /// <param name="lockValue">锁值</param>
        /// <param name="lockTimeout">锁超时时间</param>
        /// <param name="renewalInterval">续期间隔，默认使用构造函数中的值</param>
        /// <returns>注册是否成功</returns>
        public bool RegisterLock(string key, string lockValue, TimeSpan lockTimeout, TimeSpan? renewalInterval = null)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException($"{nameof(key)} cannot be null or empty");

            if (string.IsNullOrEmpty(lockValue))
                throw new ArgumentException($"{nameof(lockValue)} cannot be null or empty");

            if (lockTimeout <= TimeSpan.Zero)
                throw new ArgumentException($"{nameof(lockTimeout)} must be positive");

            var info = new LockRenewalInfo(key, lockValue, lockTimeout, renewalInterval ?? _renewalInterval);
            bool result = _locksToRenew.TryAdd(key, info);

            if (result)
            {
                _logger?.LogInformation("已注册锁用于自动续期: {Key}, 锁超时: {LockTimeout}", key, lockTimeout);
            }
            else
            {
                _logger?.LogWarning("锁已存在于续期列表中: {Key}", key);
            }

            return result;
        }

        /// <summary>
        /// 取消锁的自动续期
        /// </summary>
        /// <param name="key">锁键</param>
        /// <returns>取消是否成功</returns>
        public bool UnregisterLock(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException($"{nameof(key)} cannot be null or empty");

            bool result = _locksToRenew.TryRemove(key, out _);

            if (result)
            {
                _logger?.LogInformation("已取消锁的自动续期: {Key}", key);
            }
            else
            {
                _logger?.LogWarning("锁不在续期列表中: {Key}", key);
            }

            return result;
        }

        /// <summary>
        /// 更新锁的活动时间，表明用户仍在操作
        /// </summary>
        /// <param name="key">锁键</param>
        /// <returns>更新是否成功</returns>
        public bool UpdateActivity(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException($"{nameof(key)} cannot be null or empty");

            if (_locksToRenew.TryGetValue(key, out var info))
            {
                info.LastActivityTime = DateTime.UtcNow;
                _logger?.LogDebug("已更新锁活动时间: {Key}", key);
                return true;
            }

            _logger?.LogWarning("更新活动时间失败，锁不在续期列表中: {Key}", key);
            return false;
        }

        /// <summary>
        /// 检查所有注册的锁并进行续期
        /// </summary>
        private async Task CheckAndRenewLocksAsync()
        {
            try
            {
                DateTime now = DateTime.UtcNow;

                foreach (var key in _locksToRenew.Keys)
                {
                    if (_locksToRenew.TryGetValue(key, out var info))
                    {
                        // 检查用户是否超时不活动
                        if (now - info.LastActivityTime > _activityTimeout)
                        {
                            _logger?.LogInformation("用户活动超时，取消锁续期: {Key}", key);
                            _locksToRenew.TryRemove(key, out _);
                            continue;
                        }

                        // 检查锁是否需要续期
                        bool shouldRenew = await ShouldRenewLockAsync(info);
                        if (shouldRenew)
                        {
                            await RenewLockAsync(info);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "检查和续期锁时发生错误");
            }
        }

        /// <summary>
        /// 确定是否应该续期锁
        /// </summary>
        /// <param name="info">锁续期信息</param>
        /// <returns>是否应该续期</returns>
        private async Task<bool> ShouldRenewLockAsync(LockRenewalInfo info)
        {
            try
            {
                // 检查锁是否存在且被正确持有
                bool isHeld = await _distributedLock.IsHeldByValueAsync(info.Key, info.LockValue);
                if (!isHeld)
                {
                    _logger?.LogWarning("锁不再被持有，将从续期列表中移除: {Key}", info.Key);
                    _locksToRenew.TryRemove(info.Key, out _);
                    return false;
                }

                // 获取锁的剩余时间
                TimeSpan? remainingTime = await _distributedLock.GetRemainingTimeAsync(info.Key);
                if (remainingTime.HasValue)
                {
                    // 当剩余时间小于续期间隔的1.5倍时进行续期
                    bool shouldRenew = remainingTime.Value < TimeSpan.FromTicks((long)(info.RenewalInterval.Ticks * 1.5));
                    _logger?.LogDebug("锁续期检查: {Key}, 剩余时间: {Remaining}, 是否续期: {ShouldRenew}", 
                        info.Key, remainingTime.Value.TotalSeconds, shouldRenew);
                    return shouldRenew;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "判断是否续期锁时发生错误: {Key}", info.Key);
                return false;
            }
        }

        /// <summary>
        /// 执行锁续期
        /// </summary>
        /// <param name="info">锁续期信息</param>
        private async Task RenewLockAsync(LockRenewalInfo info)
        {
            try
            {
                bool renewed = await _distributedLock.RenewAsync(info.Key, info.LockTimeout, info.LockValue);
                if (renewed)
                {
                    _logger?.LogInformation("成功为锁续期: {Key}, 新超时: {LockTimeout}", info.Key, info.LockTimeout);
                }
                else
                {
                    _logger?.LogWarning("锁续期失败: {Key}", info.Key);
                    _locksToRenew.TryRemove(info.Key, out _);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "为锁续期时发生错误: {Key}", info.Key);
                _locksToRenew.TryRemove(info.Key, out _);
            }
        }

        /// <summary>
        /// 获取续期管理器中的锁数量
        /// </summary>
        public int ActiveLocksCount => _locksToRenew.Count;

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">是否手动释放</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _renewalTimer?.Change(Timeout.Infinite, Timeout.Infinite);
                    _renewalTimer?.Dispose();
                    _locksToRenew.Clear();
                    _logger?.LogInformation("智能锁续期管理器已释放");
                }
                _disposed = true;
            }
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~SmartLockRenewer()
        {
            Dispose(false);
        }
    }
}