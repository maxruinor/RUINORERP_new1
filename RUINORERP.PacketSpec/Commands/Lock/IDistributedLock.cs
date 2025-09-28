using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Commands.Lock
{
    /// <summary>
    /// 分布式锁接口
    /// </summary>
    public interface IDistributedLock
    {
        /// <summary>
        /// 尝试获取锁
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <param name="timeout">锁的超时时间</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>如果成功获取锁则返回true，否则返回false</returns>
        Task<bool> TryAcquireAsync(string key, TimeSpan timeout, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <returns>任务</returns>
        Task ReleaseAsync(string key);
        
        /// <summary>
        /// 检查锁是否存在
        /// </summary>
        /// <param name="key">锁的键</param>
        /// <returns>如果锁存在则返回true，否则返回false</returns>
        Task<bool> ExistsAsync(string key);
    }
}