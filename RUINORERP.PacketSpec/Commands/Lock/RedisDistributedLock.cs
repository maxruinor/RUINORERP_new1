using System;
using System.Threading;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace RUINORERP.PacketSpec.Commands.Lock
{
    /// <summary>
    /// Redis分布式锁实现
    /// </summary>
    public class RedisDistributedLock : IDistributedLock
    {
        private readonly IConnectionMultiplexer _redisConnection;
        private readonly string _lockPrefix;

        public RedisDistributedLock(IConnectionMultiplexer redisConnection, string lockPrefix = "")
        {
            _redisConnection = redisConnection ?? throw new ArgumentNullException(nameof(redisConnection));
            _lockPrefix = lockPrefix ?? "";
        }

        private IDatabase Database => _redisConnection.GetDatabase();

        /// <summary>
        /// 尝试获取锁
        /// </summary>
        public async Task<bool> TryAcquireAsync(string key, TimeSpan timeout, string lockValue = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var lockKey = _lockPrefix + key;
                var finalLockValue = lockValue ?? Guid.NewGuid().ToString();
                return await Database.StringSetAsync(lockKey, finalLockValue, timeout, When.NotExists);
            }
            catch (Exception ex)
            {
                throw new DistributedLockException($"获取分布式锁失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 释放锁
        /// </summary>
        public async Task<bool> ReleaseAsync(string key, string lockValue = null)
        {
            try
            {
                var lockKey = _lockPrefix + key;
                
                // 如果没有提供锁值，先获取当前锁值
                if (string.IsNullOrEmpty(lockValue))
                {
                    lockValue = await Database.StringGetAsync(lockKey);
                    if (lockValue == null) // 键不存在
                        return false;
                }
                
                // 使用Lua脚本确保原子性地检查并删除锁
                var luaScript = @"if redis.call('get', KEYS[1]) == ARGV[1] then return redis.call('del', KEYS[1]) else return 0 end";
                var result = await Database.ScriptEvaluateAsync(luaScript, new RedisKey[] { lockKey }, new RedisValue[] { lockValue });
                return (long)result > 0;
            }
            catch (Exception ex)
            {
                throw new DistributedLockException($"释放分布式锁失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 检查锁是否存在
        /// </summary>
        public async Task<bool> ExistsAsync(string key)
        {
            try
            {
                var lockKey = _lockPrefix + key;
                return await Database.KeyExistsAsync(lockKey);
            }
            catch (Exception ex)
            {
                throw new DistributedLockException($"检查分布式锁存在性失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 检查锁是否被特定值持有
        /// </summary>
        public async Task<bool> IsHeldByValueAsync(string key, string lockValue)
        {
            try
            {
                var lockKey = _lockPrefix + key;
                var currentValue = await Database.StringGetAsync(lockKey);
                return currentValue == lockValue;
            }
            catch (Exception ex)
            {
                throw new DistributedLockException($"检查分布式锁持有者失败: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// 为锁续期
        /// </summary>
        public async Task<bool> RenewAsync(string key, TimeSpan newTimeout, string lockValue = null)
        {
            try
            {
                var lockKey = _lockPrefix + key;
                
                // 如果没有提供锁值，先获取当前锁值
                if (string.IsNullOrEmpty(lockValue))
                {
                    lockValue = await Database.StringGetAsync(lockKey);
                    if (lockValue == null) // 键不存在
                        return false;
                }
                
                // 使用Lua脚本确保原子性地检查并续期锁
                var luaScript = @"if redis.call('get', KEYS[1]) == ARGV[1] then return redis.call('pexpire', KEYS[1], ARGV[2]) else return 0 end";
                var result = await Database.ScriptEvaluateAsync(luaScript, 
                    new RedisKey[] { lockKey }, 
                    new RedisValue[] { lockValue, ((long)newTimeout.TotalMilliseconds).ToString() });
                
                return (long)result > 0;
            }
            catch (Exception ex)
            {
                throw new DistributedLockException($"续期分布式锁失败: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// 获取锁的剩余时间
        /// </summary>
        public async Task<TimeSpan?> GetRemainingTimeAsync(string key)
        {
            try
            {
                var lockKey = _lockPrefix + key;
                var ttl = await Database.KeyTimeToLiveAsync(lockKey);
                return ttl;
            }
            catch (Exception ex)
            {
                throw new DistributedLockException($"获取分布式锁剩余时间失败: {ex.Message}", ex);
            }
        }
    }
}
