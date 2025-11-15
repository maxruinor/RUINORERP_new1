using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Commands.Lock
{
    /// <summary>
    /// 分布式锁接口 - 提供分布式环境下的锁机制
    /// 用于在分布式系统中确保资源的互斥访问，防止并发冲突
    /// </summary>
    public interface IDistributedLock
    {
        /// <summary>
        /// 尝试获取锁
        /// </summary>
        /// <param name="key">锁的键，用于标识要锁定的资源</param>
        /// <param name="timeout">锁的超时时间，防止死锁</param>
        /// <param name="lockValue">可选的锁值，用于确保只能释放自己创建的锁</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>如果成功获取锁则返回true，否则返回false</returns>
        /// <exception cref="ArgumentNullException">当key为null时抛出</exception>
        /// <exception cref="ArgumentException">当key为空字符串时抛出</exception>
        /// <exception cref="DistributedLockException">当分布式锁操作失败时抛出</exception>
        Task<bool> TryAcquireAsync(string key, TimeSpan timeout, string lockValue = null, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <param name="lockValue">可选的锁值，用于确保只能释放自己创建的锁</param>
        /// <returns>如果成功释放锁则返回true，否则返回false</returns>
        /// <exception cref="ArgumentNullException">当key为null时抛出</exception>
        /// <exception cref="ArgumentException">当key为空字符串时抛出</exception>
        /// <exception cref="DistributedLockException">当分布式锁操作失败时抛出</exception>
        Task<bool> ReleaseAsync(string key, string lockValue = null);
        
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
    
    /// <summary>
    /// 分布式锁异常类
    /// 用于表示分布式锁操作过程中发生的异常
    /// </summary>
    public class DistributedLockException : Exception
    {
        /// <summary>
        /// 锁的键
        /// </summary>
        public string Key { get; }
        
        /// <summary>
        /// 操作类型
        /// </summary>
        public string Operation { get; }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常消息</param>
        public DistributedLockException(string message) : base(message) { }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="innerException">内部异常</param>
        public DistributedLockException(string message, Exception innerException) : base(message, innerException) { }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="key">锁的键</param>
        /// <param name="operation">操作类型</param>
        public DistributedLockException(string message, string key, string operation) 
            : base(message)
        {
            Key = key;
            Operation = operation;
        }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="key">锁的键</param>
        /// <param name="operation">操作类型</param>
        /// <param name="innerException">内部异常</param>
        public DistributedLockException(string message, string key, string operation, Exception innerException) 
            : base(message, innerException)
        {
            Key = key;
            Operation = operation;
        }
    }
}