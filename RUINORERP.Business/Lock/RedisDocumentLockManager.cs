using System;
using System.Threading.Tasks;
using StackExchange.Redis;
using Newtonsoft.Json;
using RUINORERP.PacketSpec.Models.Lock;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Business.Lock
{
    /// <summary>
    /// Redis文档锁定管理器实现
    /// 基于Redis实现的分布式文档锁定管理器
    /// </summary>
    public class RedisDocumentLockManager : IDocumentLockManager
    {
        private readonly IDatabase _redisDb;
        private readonly ConnectionMultiplexer _redisConnection;
        private readonly ILogger<RedisDocumentLockManager> _logger;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="redisConnection">Redis连接多路复用器</param>
        /// <param name="logger">日志记录器</param>
        public RedisDocumentLockManager(ConnectionMultiplexer redisConnection, ILogger<RedisDocumentLockManager> logger)
        {
            _redisConnection = redisConnection ?? throw new ArgumentNullException(nameof(redisConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _redisDb = _redisConnection.GetDatabase();
        }
        
        /// <summary>
        /// 尝试锁定文档
        /// </summary>
        /// <param name="lockKey">锁定键</param>
        /// <param name="userId">用户ID</param>
        /// <param name="operationId">操作ID</param>
        /// <param name="expireSeconds">过期时间（秒）</param>
        /// <returns>是否锁定成功</returns>
        public async Task<bool> TryLockAsync(string lockKey, long userId, long operationId, int expireSeconds = 300)
        {
            try
            {
                // 创建锁定信息
                var lockInfo = new LockInfo
                {
                    LockKey = lockKey,
                    BillID = long.Parse(lockKey.Split(':')[2]), // 从lock:document:{billId}格式中提取BillID
                    UserId = userId,
                    OperationId = operationId,
                    LockTime = DateTime.Now,
                    ExpireTime = DateTime.Now.AddSeconds(expireSeconds)
                };
                
                string lockInfoJson = JsonConvert.SerializeObject(lockInfo);
                
                // 使用Redis的SetNX命令实现分布式锁
                bool acquired = await _redisDb.StringSetAsync(lockKey, lockInfoJson, TimeSpan.FromSeconds(expireSeconds), When.NotExists);
                
                if (acquired)
                {
                    _logger.LogInformation($"成功获取文档锁: {lockKey}, 用户ID: {userId}, 操作ID: {operationId}");
                }
                else
                {
                    // 检查锁是否已过期
                var existingLockInfo = await GetLockInfoAsync(lockKey);
                if (existingLockInfo != null && existingLockInfo.IsExpired())
                    {
                        // 尝试使用Lua脚本实现原子性的锁替换
                        var script = @"
                            local current = redis.call('get', KEYS[1])
                            if current then
                                local info = cjson.decode(current)
                                if info.IsExpired == true or info.ExpireTime < tonumber(ARGV[1]) then
                                    return redis.call('set', KEYS[1], ARGV[2], 'EX', ARGV[3])
                                end
                            end
                            return 0
                        ";
                        
                        // 修复LuaScript转换问题，直接使用字符串脚本
                        object result = await _redisDb.ScriptEvaluateAsync(
                            script,
                            new RedisKey[] { lockKey },
                            new RedisValue[] { DateTimeOffset.Now.ToUnixTimeMilliseconds(), lockInfoJson, expireSeconds });
                        
                        acquired = result.ToString() == "OK";
                            if (acquired)
                            {
                                _logger.LogWarning($"获取了过期的文档锁: {lockKey}, 旧用户ID: {existingLockInfo.UserId}, 新用户ID: {userId}");
                            }
                    }
                }
                
                return acquired;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"尝试锁定文档失败: {lockKey}");
                return false;
            }
        }
        
        /// <summary>
        /// 解锁文档
        /// </summary>
        /// <param name="lockKey">锁定键</param>
        /// <param name="userId">用户ID</param>
        /// <returns>是否解锁成功</returns>
        public async Task<bool> UnlockAsync(string lockKey, long userId)
        {
            try
            {
                // 使用Lua脚本确保只有锁的拥有者才能解锁
                var script = @"
                    local current = redis.call('get', KEYS[1])
                    if current then
                        local info = cjson.decode(current)
                        if info.UserId == tonumber(ARGV[1]) then
                            return redis.call('del', KEYS[1])
                        end
                    end
                    return 0
                ";
                
                // 修复LuaScript转换问题，直接使用字符串脚本
                object result = await _redisDb.ScriptEvaluateAsync(
                    script,
                    new RedisKey[] { lockKey },
                    new RedisValue[] { userId });
                
                bool unlocked = (long)result > 0;
                        if (unlocked)
                        {
                            _logger.LogInformation($"成功释放文档锁: {lockKey}, 用户ID: {userId}");
                        }
                        else
                        {
                            _logger.LogWarning($"释放文档锁失败: {lockKey}, 用户ID: {userId}, 可能不是锁的拥有者");
                        }
                
                return unlocked;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"解锁文档失败: {lockKey}");
                return false;
            }
        }
        
        /// <summary>
        /// 检查文档是否被锁定
        /// </summary>
        /// <param name="lockKey">锁定键</param>
        /// <returns>是否被锁定</returns>
        public async Task<bool> IsLockedAsync(string lockKey)
        {
            try
            {
                var lockInfo = await GetLockInfoAsync(lockKey);
                // 如果没有锁定信息或锁定已过期，则认为未锁定
                return lockInfo != null && !lockInfo.IsExpired();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"检查文档锁定状态失败: {lockKey}");
                return false;
            }
        }
        
        /// <summary>
        /// 获取锁定信息
        /// </summary>
        /// <param name="lockKey">锁定键</param>
        /// <returns>锁定信息，如果未锁定则返回null</returns>
        public async Task<LockInfo> GetLockInfoAsync(string lockKey)
        {
            try
            {
                string lockInfoJson = await _redisDb.StringGetAsync(lockKey);
                if (!string.IsNullOrEmpty(lockInfoJson))
                {
                    try
                    {
                        // 尝试反序列化到统一的锁定信息基类
                        return JsonConvert.DeserializeObject<LockInfo>(lockInfoJson);
                    }
                    catch (JsonException)
                    {
                        // 兼容旧格式的锁定信息
                        _logger.LogWarning($"尝试使用兼容模式解析锁定信息: {lockKey}");
                        // 这里可以添加更多兼容逻辑
                        return null;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取锁定信息失败: {lockKey}");
                return null;
            }
        }
    }
}