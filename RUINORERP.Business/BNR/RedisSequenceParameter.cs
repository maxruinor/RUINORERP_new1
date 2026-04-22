using CacheManager.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace RUINORERP.Business.BNR
{

    /// <summary>
    /// {redis:key/format} 
    /// Redis序号参数处理器
    /// 使用CacheManager来管理Redis连接和操作
    /// </summary>
    [ParameterType("redis")]
    public class RedisSequenceParameter : IParameterHandler
    {
        private readonly ICacheManager<object> _cacheManager;
        private IDatabase _redisDB;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public RedisSequenceParameter()
        {

        }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheManager">缓存管理器</param>
        public RedisSequenceParameter(ICacheManager<object> cacheManager)
        {
            _cacheManager = cacheManager;
            
            // 尝试从CacheManager中获取Redis数据库实例
            // 这依赖于CacheManager的具体配置实现
            try
            {
                // 如果需要直接操作Redis，可以通过反射或具体实现获取底层的Redis连接
                // 这里保留兼容原来的实现方式
            }
            catch (Exception ex)
            {
                // 记录错误但不中断程序
                System.Diagnostics.Debug.WriteLine($"获取Redis数据库实例失败: {ex.Message}");
            }
        }

        public object Factory { get; set; }


      
        /// <summary>
        /// 执行参数处理
        /// </summary>
        /// <param name="sb">字符串构建器，用于存储处理结果</param>
        /// <param name="value">参数值，格式为key/format</param>
        public void Execute(StringBuilder sb, string value)
        {
            string[] properties = value.Split('/');
            StringBuilder key = new StringBuilder();
            string[] items = RuleAnalysis.Execute(properties[0]);
            
            foreach (string p in items)
            {
                string[] sps = RuleAnalysis.GetProperties(p);
                IParameterHandler handler = null;
                if (((BNRFactory)Factory).Handlers.TryGetValue(sps[0], out handler))
                {
                    handler.Execute(key, sps[1]);
                }
            }
            
            var redisKey = $"SEQ:{key.ToString()}";
            long number;
            
            try
            {
                // 优先尝试通过 CacheManager 获取底层 Redis 连接进行原子递增
                // 如果 CacheManager 不支持直接访问 IDatabase，则回退到原有逻辑
                if (_cacheManager != null)
                {
                    // 尝试获取 StackExchange.Redis 的 IDatabase 实例
                    // 注意：这里假设 CacheManager 内部封装了 SE.Redis，实际项目中可能需要根据具体配置调整
                    var handle = _cacheManager.GetBaseHandle();
                    if (handle is StackExchange.Redis.IDatabase db)
                    {
                        number = db.StringIncrement(redisKey);
                    }
                    else
                    {
                        // 降级方案：使用原有的非原子逻辑（仅在不支持原子操作的缓存实现下）
                        number = IncrementCounterFallback(redisKey);
                    }
                }
                else if (_redisDB != null)
                {
                    // 直接使用了注入的 IDatabase
                    number = _redisDB.StringIncrement(redisKey);
                }
                else
                {
                    throw new InvalidOperationException("Redis 连接未正确初始化，无法生成原子序号。");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Redis 序号生成失败 (Key: {redisKey}): {ex.Message}", ex);
            }
            
            // 格式化输出
            if (properties.Length > 1)
            {
                sb.Append(number.ToString(properties[1]));
            }
            else
            {
                sb.Append(number.ToString());
            }
        }
        
        /// <summary>
        /// 降级方案：当无法获取原子 IDatabase 时使用
        /// </summary>
        private long IncrementCounterFallback(string key)
        {
            // 警告：此方法在高并发下可能存在竞争条件，仅作为最后手段
            long newValue = 1;
            int maxRetries = 5;
            for (int i = 0; i < maxRetries; i++)
            {    
                var currentValue = _cacheManager.Get<long?>(key);
                
                if (currentValue.HasValue)
                {
                    newValue = currentValue.Value + 1;
                    object resultObj = _cacheManager.Update(key, v => (long)v + 1);
                    long result = resultObj != null ? Convert.ToInt64(resultObj) : newValue;
                    _cacheManager.Expire(key, ExpirationMode.Absolute, TimeSpan.FromDays(30));
                    return result;
                }
                else
                {
                    newValue = 1;
                    object resultObj = _cacheManager.GetOrAdd(key, newValue);
                    long result = resultObj != null ? Convert.ToInt64(resultObj) : 1;
                    _cacheManager.Expire(key, ExpirationMode.Absolute, TimeSpan.FromDays(30));
                    if (result == 1) return 1;
                }
                System.Threading.Thread.Sleep(10);
            }
            var finalValue = _cacheManager.Get<long?>(key);
            return finalValue ?? 1;
        }
    
      
        /*
        public void Execute(StringBuilder sb, string value)
        {
            string[] properties = value.Split('/');
            StringBuilder key = new StringBuilder();
            string[] items = RuleAnalysis.Execute(properties[0]);
            foreach (string p in items)
            {
                string[] sps = RuleAnalysis.GetProperties(p);
                key.Append(sps[0]);
                break;
            }
            var redisKey = key.ToString();
            var number = mRedisDB.StringIncrement(redisKey);
            sb.Append(number.ToString(properties[1]));

        }
        */

        //void IParameterHandler.Execute(StringBuilder sb, string value)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
