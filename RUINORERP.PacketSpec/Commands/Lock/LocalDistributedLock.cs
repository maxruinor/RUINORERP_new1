using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RUINORERP.PacketSpec.Commands.Lock
{
    /// <summary>
    /// 本地分布式锁实现
    /// 提供基于内存的分布式锁功能，用于在单应用实例内管理锁定状态
    /// </summary>
    public class LocalDistributedLock : ILocalDistributedLock
    {
        private readonly ConcurrentDictionary<string, LockInfo> _locks = new ConcurrentDictionary<string, LockInfo>();
        private readonly ILogger _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器，可选</param>
        public LocalDistributedLock(ILogger<LocalDistributedLock> logger = null)
        {
            _logger = logger;
        }
        
        private class LockInfo
        {
            public SemaphoreSlim Semaphore { get; set; }
            public string LockValue { get; set; }
            public DateTime AcquireTime { get; set; }
            public TimeSpan Timeout { get; set; }
            
            public LockInfo(string lockValue = null)
            {
                Semaphore = new SemaphoreSlim(1, 1);
                LockValue = lockValue;
                AcquireTime = DateTime.UtcNow;
            }
        }
        
        /// <summary>
        /// 尝试获取锁
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <param name="timeout">锁的超时时间</param>
        /// <param name="lockValue">可选的锁值，用于确保只能释放自己创建的锁</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>如果成功获取锁则返回true，否则返回false</returns>
        /// <summary>
        /// 尝试获取锁（实现ILocalDistributedLock接口）
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <param name="lockValue">锁的值，用于验证锁的持有者</param>
        /// <param name="timeout">锁的超时时间</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>如果成功获取锁则返回true，否则返回false</returns>
        /// <exception cref="ArgumentNullException">当key或lockValue为null时抛出</exception>
        /// <exception cref="ArgumentException">当key或lockValue为空字符串时抛出</exception>
        /// <exception cref="DistributedLockException">当分布式锁操作失败时抛出</exception>
        public async Task<bool> TryAcquireAsync(string key, string lockValue, TimeSpan timeout, CancellationToken cancellationToken = default)
        {
            // 参数验证
            if (lockValue == null)
            {
                throw new ArgumentNullException(nameof(lockValue));
            }
            
            if (string.IsNullOrEmpty(lockValue))
            {
                throw new ArgumentException($"{nameof(lockValue)} cannot be null or empty");
            }
            
            // 调用现有的实现，调整参数顺序
            return await TryAcquireAsyncInternal(key, timeout, lockValue, cancellationToken);
        }
        
        /// <summary>
        /// 尝试获取锁（内部实现方法）
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <param name="timeout">锁的超时时间</param>
        /// <param name="lockValue">可选的锁值，用于确保只能释放自己创建的锁</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>如果成功获取锁则返回true，否则返回false</returns>
        /// <exception cref="ArgumentNullException">当key为null时抛出</exception>
        /// <exception cref="ArgumentException">当key为空字符串时抛出</exception>
        /// <exception cref="DistributedLockException">当分布式锁操作失败时抛出</exception>
        public async Task<bool> TryAcquireAsyncInternal(string key, TimeSpan timeout, string lockValue = null, CancellationToken cancellationToken = default)
        {
            try
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }
                
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentException($"{nameof(key)} cannot be null or empty");
                }

                _logger?.LogDebug("尝试获取本地锁: {Key}, 超时: {Timeout}", key, timeout);
                
                // 获取或创建锁信息
                var lockInfo = _locks.GetOrAdd(key, k => new LockInfo(lockValue));
                
                // 检查锁是否已过期
                CheckAndReleaseExpiredLock(key, lockInfo);
                
                // 尝试获取信号量
                bool acquired = await lockInfo.Semaphore.WaitAsync(timeout, cancellationToken);
                if (acquired)
                {
                    // 更新锁信息
                    lockInfo.LockValue = lockValue;
                    lockInfo.AcquireTime = DateTime.UtcNow;
                    lockInfo.Timeout = timeout;
                    
                    _logger?.LogInformation("成功获取本地锁: {Key}, 锁值: {LockValue}", key, lockValue ?? "(无)");
                }
                else
                {
                    _logger?.LogWarning("获取本地锁超时: {Key}, 超时: {Timeout}", key, timeout);
                }
                
                return acquired;
            }
            catch (OperationCanceledException)
            {
                _logger?.LogWarning("获取本地锁被取消: {Key}", key);
                throw;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException || ex is ArgumentException)
                {
                    throw;
                }
                
                _logger?.LogError(ex, "获取本地锁时发生错误: {Key}", key);
                throw new DistributedLockException("获取锁失败", key, "TryAcquire", ex);
            }
        }
        
        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <param name="lockValue">可选的锁值，用于确保只能释放自己创建的锁</param>
        /// <returns>如果释放成功则返回true，否则返回false</returns>
        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <param name="lockValue">可选的锁值，用于确保只能释放自己创建的锁</param>
        /// <returns>如果成功释放锁则返回true，否则返回false</returns>
        /// <exception cref="ArgumentNullException">当key为null时抛出</exception>
        /// <exception cref="ArgumentException">当key为空字符串时抛出</exception>
        /// <exception cref="DistributedLockException">当分布式锁操作失败时抛出</exception>
        public Task<bool> ReleaseAsync(string key, string lockValue = null)
        {
            try
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }
                
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentException($"{nameof(key)} cannot be null or empty");
                }

                _logger?.LogDebug("尝试释放本地锁: {Key}, 锁值: {LockValue}", key, lockValue ?? "(无)");

                if (_locks.TryGetValue(key, out var lockInfo))
                {
                    // 检查锁值是否匹配
                    if (!string.IsNullOrEmpty(lockValue) && lockInfo.LockValue != lockValue)
                    {
                        _logger?.LogWarning("锁值不匹配，拒绝释放: {Key}", key);
                        return Task.FromResult(false);
                    }
                    
                    try
                    {
                        lockInfo.Semaphore.Release();
                        _logger?.LogInformation("成功释放本地锁: {Key}", key);
                        return Task.FromResult(true);
                    }
                    catch (SemaphoreFullException ex)
                    {
                        _logger?.LogWarning(ex, "释放本地锁时发生信号量已满异常: {Key}", key);
                        return Task.FromResult(false);
                    }
                }
                
                _logger?.LogWarning("锁不存在: {Key}", key);
                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException || ex is ArgumentException)
                {
                    throw;
                }
                
                _logger?.LogError(ex, "释放本地锁时发生错误: {Key}", key);
                throw new DistributedLockException("释放锁失败", key, "Release", ex);
            }
        }
        
        /// <summary>
        /// 检查锁是否存在
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <returns>如果锁存在则返回true，否则返回false</returns>
        /// <summary>
        /// 检查锁是否存在
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <returns>如果锁存在则返回true，否则返回false</returns>
        /// <exception cref="ArgumentNullException">当key为null时抛出</exception>
        /// <exception cref="ArgumentException">当key为空字符串时抛出</exception>
        /// <exception cref="DistributedLockException">当分布式锁操作失败时抛出</exception>
        public Task<bool> ExistsAsync(string key)
        {
            try
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }
                
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentException($"{nameof(key)} cannot be null or empty");
                }

                bool exists = _locks.ContainsKey(key);
                _logger?.LogDebug("检查锁是否存在: {Key}, 结果: {Exists}", key, exists);
                return Task.FromResult(exists);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException || ex is ArgumentException)
                {
                    throw;
                }
                
                _logger?.LogError(ex, "检查锁是否存在时发生错误: {Key}", key);
                throw new DistributedLockException("检查锁存在状态失败", key, "Exists", ex);
            }
        }
        
        /// <summary>
        /// 检查锁是否被特定值持有
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <param name="lockValue">要检查的锁值</param>
        /// <returns>如果锁被指定值持有则返回true，否则返回false</returns>
        /// <summary>
        /// 检查锁是否被特定值持有
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <param name="lockValue">要检查的锁值</param>
        /// <returns>如果锁被指定值持有则返回true，否则返回false</returns>
        /// <exception cref="ArgumentNullException">当key或lockValue为null时抛出</exception>
        /// <exception cref="ArgumentException">当key或lockValue为空字符串时抛出</exception>
        /// <exception cref="DistributedLockException">当分布式锁操作失败时抛出</exception>
        public Task<bool> IsHeldByValueAsync(string key, string lockValue)
        {
            try
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }
                
                if (lockValue == null)
                {
                    throw new ArgumentNullException(nameof(lockValue));
                }
                
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentException($"{nameof(key)} cannot be null or empty");
                }
                
                if (string.IsNullOrEmpty(lockValue))
                {
                    throw new ArgumentException($"{nameof(lockValue)} cannot be null or empty");
                }

                _logger?.LogDebug("检查锁是否被特定值持有: {Key}, 锁值: {LockValue}", key, lockValue);
                
                if (_locks.TryGetValue(key, out var lockInfo))
                {
                    // 检查锁是否已过期
                    CheckAndReleaseExpiredLock(key, lockInfo);
                    
                    // 检查锁值是否匹配且锁被占用
                    bool isHeld = lockInfo.Semaphore.CurrentCount == 0 && 
                                 (string.IsNullOrEmpty(lockValue) || lockInfo.LockValue == lockValue);
                    
                    _logger?.LogDebug("锁持有检查结果: {Key}, 持有状态: {IsHeld}", key, isHeld);
                    return Task.FromResult(isHeld);
                }
                
                _logger?.LogDebug("锁不存在或已过期: {Key}", key);
                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException || ex is ArgumentException)
                {
                    throw;
                }
                
                _logger?.LogError(ex, "检查锁持有状态时发生错误: {Key}", key);
                throw new DistributedLockException("检查锁持有状态失败", key, "IsHeldByValue", ex);
            }
        }
        
        /// <summary>
        /// 为锁续期
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <param name="newTimeout">新的超时时间</param>
        /// <param name="lockValue">可选的锁值，用于验证</param>
        /// <returns>如果续期成功则返回true，否则返回false</returns>
        /// <summary>
        /// 为锁续期
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <param name="newTimeout">新的超时时间</param>
        /// <param name="lockValue">可选的锁值，用于验证</param>
        /// <returns>如果续期成功则返回true，否则返回false</returns>
        /// <exception cref="ArgumentNullException">当key为null时抛出</exception>
        /// <exception cref="ArgumentException">当key为空字符串时抛出</exception>
        /// <exception cref="DistributedLockException">当分布式锁操作失败时抛出</exception>
        public Task<bool> RenewAsync(string key, TimeSpan newTimeout, string lockValue = null)
        {
            try
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }
                
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentException($"{nameof(key)} cannot be null or empty");
                }

                _logger?.LogDebug("尝试为锁续期: {Key}, 新超时: {NewTimeout}, 锁值: {LockValue}", key, newTimeout, lockValue ?? "(无)");

                if (_locks.TryGetValue(key, out var lockInfo))
                {
                    // 检查锁值是否匹配
                    if (!string.IsNullOrEmpty(lockValue) && lockInfo.LockValue != lockValue)
                    {
                        _logger?.LogWarning("锁值不匹配，拒绝续期: {Key}", key);
                        return Task.FromResult(false);
                    }
                    
                    // 检查锁是否被占用
                    if (lockInfo.Semaphore.CurrentCount == 0)
                    {
                        // 更新超时时间和获取时间
                        lockInfo.Timeout = newTimeout;
                        lockInfo.AcquireTime = DateTime.UtcNow;
                        
                        _logger?.LogInformation("成功为锁续期: {Key}, 新超时: {NewTimeout}", key, newTimeout);
                        return Task.FromResult(true);
                    }
                    else
                    {
                        _logger?.LogWarning("锁未被占用，无法续期: {Key}", key);
                    }
                }
                else
                {
                    _logger?.LogWarning("锁不存在，无法续期: {Key}", key);
                }
                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException || ex is ArgumentException)
                {
                    throw;
                }
                
                _logger?.LogError(ex, "为锁续期时发生错误: {Key}", key);
                throw new DistributedLockException("锁续期失败", key, "Renew", ex);
            }
        }
        
        /// <summary>
        /// 获取锁的剩余时间
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <returns>锁的剩余过期时间，如果锁不存在则返回null</returns>
        /// <summary>
        /// 获取锁的剩余时间
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <returns>锁的剩余过期时间，如果锁不存在则返回null</returns>
        /// <exception cref="ArgumentNullException">当key为null时抛出</exception>
        /// <exception cref="ArgumentException">当key为空字符串时抛出</exception>
        /// <exception cref="DistributedLockException">当分布式锁操作失败时抛出</exception>
        public Task<TimeSpan?> GetRemainingTimeAsync(string key)
        {
            try
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }
                
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentException($"{nameof(key)} cannot be null or empty");
                }

                _logger?.LogDebug("获取锁的剩余时间: {Key}", key);

                if (_locks.TryGetValue(key, out var lockInfo))
                {
                    // 检查锁是否已过期
                    if (lockInfo.Semaphore.CurrentCount == 0 && lockInfo.Timeout > TimeSpan.Zero)
                    {
                        TimeSpan elapsed = DateTime.UtcNow - lockInfo.AcquireTime;
                        TimeSpan remaining = lockInfo.Timeout - elapsed;
                        TimeSpan? result = remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
                        _logger?.LogDebug("锁剩余时间: {Key}, 剩余: {Remaining}", key, result?.TotalSeconds.ToString() ?? "已过期");
                        return Task.FromResult(result);
                    }
                    else
                    {
                        _logger?.LogDebug("锁未被占用或无超时设置: {Key}", key);
                    }
                }
                else
                {
                    _logger?.LogDebug("锁不存在: {Key}", key);
                }
                return Task.FromResult<TimeSpan?>(null);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException || ex is ArgumentException)
                {
                    throw;
                }
                
                _logger?.LogError(ex, "获取锁剩余时间时发生错误: {Key}", key);
                throw new DistributedLockException("获取锁剩余时间失败", key, "GetRemainingTime", ex);
            }
        }
        
        /// <summary>
        /// 检查并释放过期的锁
        /// </summary>
        /// <summary>
        /// 检查并释放过期的锁
        /// </summary>
        private void CheckAndReleaseExpiredLock(string key, LockInfo lockInfo)
        {
            try
            {
                // 只检查有超时设置且被占用的锁
                if (lockInfo.Semaphore.CurrentCount == 0 && lockInfo.Timeout > TimeSpan.Zero)
                {
                    TimeSpan elapsed = DateTime.UtcNow - lockInfo.AcquireTime;
                    if (elapsed > lockInfo.Timeout)
                    {
                        _logger?.LogInformation("释放过期锁: {Key}, 超时时间: {Timeout}, 已用时间: {Elapsed}", 
                            key, lockInfo.Timeout, elapsed);
                        
                        try
                        {
                            lockInfo.Semaphore.Release();
                        }
                        catch (SemaphoreFullException ex)
                        {
                            _logger?.LogWarning(ex, "释放过期锁时发生信号量已满异常: {Key}", key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "检查过期锁时发生错误: {Key}", key);
                // 不抛出异常，避免影响主流程
            }
        }
    }
}