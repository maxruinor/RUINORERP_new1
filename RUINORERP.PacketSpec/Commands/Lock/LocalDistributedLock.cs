using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Commands.Lock
{
    /// <summary>
    /// 基于本地信号量的分布式锁实现（用于单节点环境）
    /// </summary>
    public class LocalDistributedLock : IDistributedLock
    {
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new ConcurrentDictionary<string, SemaphoreSlim>();
        
        /// <summary>
        /// 尝试获取锁
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <param name="timeout">锁的超时时间</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>如果成功获取锁则返回true，否则返回false</returns>
        public async Task<bool> TryAcquireAsync(string key, TimeSpan timeout, CancellationToken cancellationToken = default)
        {
            var semaphore = _locks.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));
            return await semaphore.WaitAsync(timeout, cancellationToken);
        }
        
        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <returns>任务</returns>
        public Task ReleaseAsync(string key)
        {
            if (_locks.TryGetValue(key, out var semaphore))
            {
                try
                {
                    semaphore.Release();
                }
                catch (SemaphoreFullException)
                {
                    // 忽略信号量已满的异常
                }
            }
            
            return Task.CompletedTask;
        }
        
        /// <summary>
        /// 检查锁是否存在
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <returns>如果锁存在则返回true，否则返回false</returns>
        public Task<bool> ExistsAsync(string key)
        {
            return Task.FromResult(_locks.ContainsKey(key));
        }
    }
}