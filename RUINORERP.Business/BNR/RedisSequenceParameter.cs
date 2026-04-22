using CacheManager.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using RUINORERP.Extensions.Redis;

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
                // 优先使用项目现有的 RedisConnectionHelper 获取原子递增
                var redisConnection = RedisConnectionHelper.Instance;
                if (redisConnection != null && redisConnection.IsConnected)
                {
                    var db = redisConnection.GetDatabase();
                    number = db.StringIncrement(redisKey);
                }
                else if (_redisDB != null)
                {
                    // 回退到直接注入的 IDatabase
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
