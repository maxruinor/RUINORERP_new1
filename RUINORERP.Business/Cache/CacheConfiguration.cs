using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CacheManager.Core;

namespace RUINORERP.Business.Cache
{
        /// <summary>
        /// 缓存全局配置
        /// </summary>
        public class CacheConfiguration
        {
            /// <summary>
            /// 应用程序名称（用于Redis键前缀）
            /// </summary>
            public string ApplicationName { get; set; } = "RUINORERP";

            /// <summary>
            /// 缓存名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Redis连接字符串
            /// </summary>
            public string RedisConnectionString { get; set; }

            /// <summary>
            /// 默认缓存策略
            /// </summary>
            public CachePolicy DefaultPolicy { get; set; } = new CachePolicy
            {
                StorageType = CacheStorageType.Memory,
                ExpirationMode = ExpirationMode.Absolute,
                Expiration = TimeSpan.FromMinutes(30)
            };

            /// <summary>
            /// 业务特定的缓存策略
            /// </summary>
            public Dictionary<string, CachePolicy> BusinessPolicies { get; set; } = new Dictionary<string, CachePolicy>();

            /// <summary>
            /// 是否启用缓存统计
            /// </summary>
            public bool EnableStatistics { get; set; } = false;

            /// <summary>
            /// 获取指定缓存的策略
            /// </summary>
            public CachePolicy GetPolicyForCache(string cacheName)
            {
                if (BusinessPolicies.TryGetValue(cacheName, out var policy))
                {
                    return policy;
                }
                return DefaultPolicy;
            }

            /// <summary>
            /// 克隆配置
            /// </summary>
            public CacheConfiguration Clone()
            {
                return new CacheConfiguration
                {
                    ApplicationName = this.ApplicationName,
                    Name = this.Name,
                    RedisConnectionString = this.RedisConnectionString,
                    DefaultPolicy = this.DefaultPolicy.Clone(),
                    BusinessPolicies = new Dictionary<string, CachePolicy>(this.BusinessPolicies),
                    EnableStatistics = this.EnableStatistics
                };
            }
        }
}
