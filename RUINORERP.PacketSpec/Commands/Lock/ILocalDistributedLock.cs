using System;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Commands.Lock
{
    /// <summary>
    /// 分布式锁接口
    /// 定义了分布式锁的基本操作方法
    /// </summary>
    public interface ILocalDistributedLock
    {
        /// <summary>
        /// 尝试获取锁
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <param name="lockValue">锁的值，用于验证锁的持有者</param>
        /// <param name="timeout">锁的超时时间</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>如果成功获取锁则返回true，否则返回false</returns>
        /// <exception cref="ArgumentNullException">当key或lockValue为null时抛出</exception>
        /// <exception cref="ArgumentException">当key或lockValue为空字符串时抛出</exception>
        /// <exception cref="DistributedLockException">当分布式锁操作失败时抛出</exception>
        Task<bool> TryAcquireAsync(string key, string lockValue, TimeSpan timeout, CancellationToken cancellationToken = default);

        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <param name="lockValue">锁的值，用于验证锁的持有者</param>
        /// <returns>如果成功释放锁则返回true，否则返回false</returns>
        /// <exception cref="ArgumentNullException">当key或lockValue为null时抛出</exception>
        /// <exception cref="ArgumentException">当key或lockValue为空字符串时抛出</exception>
        /// <exception cref="DistributedLockException">当分布式锁操作失败时抛出</exception>
        Task<bool> ReleaseAsync(string key, string lockValue);

        /// <summary>
        /// 检查锁是否存在
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <returns>如果锁存在则返回true，否则返回false</returns>
        /// <exception cref="ArgumentNullException">当key为null时抛出</exception>
        /// <exception cref="ArgumentException">当key为空字符串时抛出</exception>
        /// <exception cref="DistributedLockException">当分布式锁操作失败时抛出</exception>
        Task<bool> ExistsAsync(string key);

        /// <summary>
        /// 检查锁是否被特定值持有
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <param name="lockValue">要检查的锁值</param>
        /// <returns>如果锁被指定值持有则返回true，否则返回false</returns>
        /// <exception cref="ArgumentNullException">当key或lockValue为null时抛出</exception>
        /// <exception cref="ArgumentException">当key或lockValue为空字符串时抛出</exception>
        /// <exception cref="DistributedLockException">当分布式锁操作失败时抛出</exception>
        Task<bool> IsHeldByValueAsync(string key, string lockValue);

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
        Task<bool> RenewAsync(string key, TimeSpan newTimeout, string lockValue = null);

        /// <summary>
        /// 获取锁的剩余时间
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <returns>锁的剩余过期时间，如果锁不存在则返回null</returns>
        /// <exception cref="ArgumentNullException">当key为null时抛出</exception>
        /// <exception cref="ArgumentException">当key为空字符串时抛出</exception>
        /// <exception cref="DistributedLockException">当分布式锁操作失败时抛出</exception>
        Task<TimeSpan?> GetRemainingTimeAsync(string key);
    }
}