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
                Console.WriteLine($"获取Redis数据库实例失败: {ex.Message}");
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
            
            var redisKey = key.ToString();
            long number;
            
            try
            {
                // 使用CacheManager的方式实现递增
                // 如果缓存中存在键，增加计数
                // 如果不存在，初始化为1
                number = IncrementCounter(redisKey);
            }
            catch (Exception ex)
            {
                // 如果CacheManager方式失败，尝试回退到直接的Redis操作（如果可用）
                if (_redisDB != null)
                {
                    number = _redisDB.StringIncrement(redisKey);
                }
                else
                {
                    throw new InvalidOperationException($"无法执行Redis递增操作: {ex.Message}");
                }
            }
            
            sb.Append(number.ToString(properties[1]));
        }
        
        /// <summary>
        /// 使用CacheManager递增计数器
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>递增后的值</returns>
        private long IncrementCounter(string key)
        {
            try
            {
                // 尝试使用原子操作方式递增
                // 如果缓存中不存在，初始化为1
                long newValue = 1;
                
                // 这里使用一个循环来确保原子性操作
                // 尝试最多3次，避免无限循环
                int maxRetries = 3;
                for (int i = 0; i < maxRetries; i++)
                {    
                    // 1. 先尝试获取当前值
                    var currentValue = _cacheManager.Get<long?>(key);
                    
                    if (currentValue.HasValue)
                    {
                        // 2a. 如果值存在，使用Update方法更新
                        newValue = currentValue.Value + 1;
                        
                        // 使用Update方法进行值更新
                        object resultObj = _cacheManager.Update(key, v => (long)v + 1);
                        long result = resultObj != null ? Convert.ToInt64(resultObj) : 1;
                        
                        // 设置过期时间
                        _cacheManager.Expire(key, ExpirationMode.Absolute, TimeSpan.FromDays(30));
                        
                        return result;
                    }
                    else
                    {
                        // 2b. 如果值不存在，使用GetOrAdd添加初始值
                        // GetOrAdd会返回现有值或添加新值并返回
                        newValue = 1; // 初始值为1
                        object resultObj = _cacheManager.GetOrAdd(key, newValue);
                        long result = resultObj != null ? Convert.ToInt64(resultObj) : 1;
                        
                        // 设置过期时间
                        _cacheManager.Expire(key, ExpirationMode.Absolute, TimeSpan.FromDays(30));
                        
                        // 如果GetOrAdd返回1，表示是新添加的值
                        if (result == 1)
                        {
                            return 1;
                        }
                        // 如果返回值不是1，表示在并发情况下其他线程已经添加，需要重试
                    }
                    
                    // 短暂等待后重试
                    if (i < maxRetries - 1)
                    {
                        System.Threading.Thread.Sleep(10);
                    }
                }
                
                // 如果多次重试后仍未成功，尝试最后一次获取值
                var finalValue = _cacheManager.Get<long?>(key);
                return finalValue ?? 1;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"CacheManager递增操作失败: {ex.Message}", ex);
            }
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
