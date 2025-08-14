using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CacheManager.Core;
using global::RUINORERP.Business.Cache.Attributes;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Reflection;
using CacheManager.Core.Configuration;
using CacheManager.MicrosoftCachingMemory;
using System.Threading;
using CacheManager.SystemRuntimeCaching;


namespace RUINORERP.Business.Cache
{



    /// <summary>
    /// 缓存管理器工厂实现类（基于CacheManager官方API）
    /// </summary>
    public class CacheManagerFactory : ICacheManagerFactory, IDisposable
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly CacheConfiguration _defaultConfig;
        private readonly ConcurrentDictionary<string, ICacheManager<object>> _cacheManagers = new ConcurrentDictionary<string, ICacheManager<object>>();
        private readonly ConcurrentDictionary<Type, ICache> _businessCaches = new ConcurrentDictionary<Type, ICache>();
        private bool _disposed = false;

        public CacheManagerFactory(ILoggerFactory loggerFactory, CacheConfiguration defaultConfig)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _defaultConfig = defaultConfig ?? throw new ArgumentNullException(nameof(defaultConfig));
        }

        /// <summary>
        /// 获取指定名称的缓存管理器
        /// </summary>
        public ICacheManager<object> GetCacheManager(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("缓存名称不能为空", nameof(name));

            return _cacheManagers.GetOrAdd(name, key => CreateCacheManager(key));
        }

        /// <summary>
        /// 获取业务缓存实例
        /// </summary>
        public TCache GetBusinessCache<TCache>() where TCache : ICache
        {
            var cacheType = typeof(TCache);
            if (!cacheType.IsInterface)
                throw new InvalidOperationException($"类型 {cacheType.Name} 必须是接口");

            return (TCache)_businessCaches.GetOrAdd(cacheType, type =>
            {
                var implementationType = FindImplementationType(type);
                if (implementationType == null)
                    throw new InvalidOperationException($"未找到接口 {type.Name} 的实现类");

                return CreateBusinessCacheInstance(implementationType);
            });
        }

        /// <summary>
        /// 根据官方API创建缓存管理器
        /// </summary>
        private ICacheManager<object> CreateCacheManager(string name)
        {
            // 获取该缓存名称对应的策略
            var policy = _defaultConfig.GetPolicyForCache(name) ?? _defaultConfig.DefaultPolicy;

            // 使用官方推荐的CacheFactory.Build方式配置缓存
            return CacheFactory.Build<object>(settings =>
            {
                // 配置日志
                settings
               .WithMicrosoftLogging(_loggerFactory);

                // 配置重试策略
                settings.WithMaxRetries(3)
                       .WithRetryTimeout(50);

                // 根据存储类型配置缓存
                switch (policy.StorageType)
                {
                    case CacheStorageType.Memory:
                        ConfigureMemoryCache(settings, name);
                        break;
                    case CacheStorageType.Redis:
                        ConfigureRedisCache(settings, name, policy);
                        break;
                    case CacheStorageType.Hybrid:
                        ConfigureHybridCache(settings, name, policy);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"不支持的缓存存储类型: {policy.StorageType}");
                }

            });
        }

        /// <summary>
        /// 配置内存缓存
        /// </summary>
        private void ConfigureMemoryCache(ConfigurationBuilderCachePart settings, string name)
        {
            settings.WithSystemRuntimeCacheHandle($"{_defaultConfig.ApplicationName}_{name}");


        }

        /// <summary>
        /// 配置Redis缓存（遵循官方示例）
        /// </summary>
        private void ConfigureRedisCache(ConfigurationBuilderCachePart settings, string name, CachePolicy policy)
        {
            if (string.IsNullOrEmpty(_defaultConfig.RedisConnectionString))
                throw new InvalidOperationException("Redis缓存需要配置连接字符串");

            settings
               .WithSystemRuntimeCacheHandle()
               .And
               .WithRedisConfiguration("redis", config =>
               {
                   config.WithAllowAdmin()
                       .WithDatabase(0)
                       .WithEndpoint("localhost", 6379);
               })
               .WithMaxRetries(1000)
               .WithRetryTimeout(100)
               .WithRedisBackplane("redis")
               .WithRedisCacheHandle("redis", true);


            // settings
            //.WithRedisConfiguration("redis", _config.RedisConnectionString)
            //.WithRedisCacheHandle("redis", isBackplaneSource: true);


            // 解析Redis连接信息
            var redisConfig = ParseRedisConnectionString(_defaultConfig.RedisConnectionString);

            // 配置Redis连接
            settings.WithRedisConfiguration($"{name}_redis", config =>
            {
                config.WithEndpoint(redisConfig.Host, redisConfig.Port)
                      .WithDatabase(redisConfig.Database ?? 0);

                if (!string.IsNullOrEmpty(redisConfig.Password))
                {
                    config.WithPassword(redisConfig.Password);
                }

                if (policy.AllowAdmin)
                {
                    config.WithAllowAdmin();
                }
            })
            .WithRedisCacheHandle($"{name}_redis", isBackplaneSource: true);
        }

        /// <summary>
        /// 配置混合缓存（内存+Redis）
        /// </summary>
        private void ConfigureHybridCache(ConfigurationBuilderCachePart settings, string name, CachePolicy policy)
        {
            if (string.IsNullOrEmpty(_defaultConfig.RedisConnectionString))
                throw new InvalidOperationException("混合模式缓存需要配置Redis连接字符串");

            var redisConfig = ParseRedisConnectionString(_defaultConfig.RedisConnectionString);

            // 1. 配置内存缓存作为一级缓存
            settings.WithSystemRuntimeCacheHandle($"{_defaultConfig.ApplicationName}_{name}_memory")
                 .WithExpiration(policy.ExpirationMode, policy.Expiration);

            // 2. 配置Redis作为二级缓存和背板
            settings.WithRedisConfiguration($"{name}_redis", config =>
            {
                config.WithEndpoint(redisConfig.Host, redisConfig.Port)
                      .WithDatabase(redisConfig.Database ?? 0);

                if (!string.IsNullOrEmpty(redisConfig.Password))
                {
                    config.WithPassword(redisConfig.Password);
                }

                if (policy.AllowAdmin)
                {
                    config.WithAllowAdmin();
                }
            })
            .WithRedisCacheHandle($"{name}_redis", isBackplaneSource: true)
            .EnableCacheReplication(); // 启用缓存复制，保持内存和Redis同步
        }

        /// <summary>
        /// 解析Redis连接字符串
        /// </summary>
        private RedisConnectionInfo ParseRedisConnectionString(string connectionString)
        {
            // 简单解析Redis连接字符串（实际项目可使用更完善的解析逻辑）
            var parts = connectionString.Split(',');
            var hostPort = parts[0].Split(':');

            return new RedisConnectionInfo
            {
                Host = hostPort[0],
                Port = hostPort.Length > 1 ? int.Parse(hostPort[1]) : 6379,
                Password = parts.FirstOrDefault(p => p.StartsWith("password="))?.Split('=')[1],
                Database = parts.FirstOrDefault(p => p.StartsWith("database=")) != null
                    ? int.Parse(parts.First(p => p.StartsWith("database=")).Split('=')[1])
                    : 0
            };
        }

        /// <summary>
        /// 创建业务缓存实例
        /// </summary>
        private ICache CreateBusinessCacheInstance(Type implementationType)
        {
            var constructor = implementationType.GetConstructor(new[] {
                typeof(ICacheManager<object>),
                typeof(ILogger),
                typeof(CachePolicy)
            });

            if (constructor == null)
                throw new InvalidOperationException($"业务缓存类型 {implementationType.Name} 必须定义正确的构造函数");

            var cacheName = GetCacheNameForType(implementationType);
            var cacheManager = GetCacheManager(cacheName);
            var logger = _loggerFactory.CreateLogger(implementationType);
            var policy = _defaultConfig.GetPolicyForCache(cacheName) ?? _defaultConfig.DefaultPolicy;

            return (ICache)constructor.Invoke(new object[] { cacheManager, logger, policy });
        }

        /// <summary>
        /// 获取类型对应的缓存名称
        /// </summary>
        private string GetCacheNameForType(Type type)
        {
            var cacheNameAttr = type.GetCustomAttributes(typeof(CacheNameAttribute), false)
                .FirstOrDefault() as CacheNameAttribute;

            if (cacheNameAttr != null && !string.IsNullOrEmpty(cacheNameAttr.Name))
            {
                return cacheNameAttr.Name;
            }

            var typeName = type.Name;
            return typeName.EndsWith("Cache") ? typeName.Substring(0, typeName.Length - 5) : typeName;
        }

        /// <summary>
        /// 查找接口的实现类型
        /// </summary>
        private Type FindImplementationType(Type interfaceType)
        {
            return interfaceType.Assembly.GetTypes()
                .Where(t => interfaceType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .FirstOrDefault();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                foreach (var cacheManager in _cacheManagers.Values)
                {
                    cacheManager.Dispose();
                }
                _cacheManagers.Clear();
                _businessCaches.Clear();
            }

            _disposed = true;
        }

        ~CacheManagerFactory()
        {
            Dispose(false);
        }

        /// <summary>
        /// Redis连接信息内部类
        /// </summary>
        private class RedisConnectionInfo
        {
            public string Host { get; set; }
            public int Port { get; set; }
            public string Password { get; set; }
            public int? Database { get; set; }
        }
    }





}
