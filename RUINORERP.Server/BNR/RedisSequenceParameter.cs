using CacheManager.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace RUINORERP.Server.BNR
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

        public BNRFactory Factory { get; set; }


      
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
                if (Factory.Handlers.TryGetValue(sps[0], out handler))
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
            // 获取当前值
            var currentValue = _cacheManager.Get<long>(key);
            
            // 如果不存在，初始化为1
            if (currentValue == 0)
            {
                _cacheManager.Add(key, 1);
                return 1;
            }
            
            // 递增并更新
            long newValue = currentValue + 1;
            _cacheManager.Put(key, newValue);
            return newValue;
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
