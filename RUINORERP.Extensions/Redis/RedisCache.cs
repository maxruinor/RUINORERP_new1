using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Extensions.Redis
{
    /// <summary>
    /// 分布式缓存
    /// </summary>
    public static class RedisCache
    {
        /// <summary>
        /// 单例工厂，每次初始化redis客户端从工厂中获取
        /// </summary>
        private static IRedisClientFactory _factory = RedisCacheClientFactory.Instance;


        /// <summary>
        /// 设置redis缓存
        /// </summary>
        /// <typeparam name="T">泛型类</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">泛型实体</param>
        /// <param name="expire">过期时间</param>
        /// <returns></returns>
        public static bool Set<T>(string key, T value, DateTime expire)
        {
            try
            {
                using (var client = GetClient())
                {
                    return client.Set<T>(key, value, expire);
                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            try
            {
                using (var client = GetClient())
                {
                    return client.Get<T>(key);
                }
            }
            catch
            {
                //如果redis出现异常，则直接返回默认值
                return default(T);
            }
        }
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            try
            {
                using (var client = GetClient())
                {
                    client.Remove(key);
                }
            }
            catch
            {

            }
        }
        public static void RemoveAll()
        {
            try
            {
                using (var client = GetClient())
                {
                    var keys = client.GetAllKeys();
                    client.RemoveAll(keys);
                }
            }
            catch
            {

            }
        }
        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <returns></returns>
        private static IRedisClient GetClient()
        {
            RedisClient client = null;
            if (string.IsNullOrEmpty(ConfigManager.RedisServer))
            {
                throw new ArgumentNullException("redis server ip is empty.");
            }
            if (string.IsNullOrEmpty(ConfigManager.RedisPwd))
            {
                throw new ArgumentNullException("redis server pwd is empty.");
            }
            client = _factory.CreateRedisClient(ConfigManager.RedisServer, ConfigManager.RedisPort);
            client.Password = ConfigManager.RedisPwd;
            client.Db = ConfigManager.RedisServerDb;
            return client;
        }
    }
}
