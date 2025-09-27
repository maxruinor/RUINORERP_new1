using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.Extensions;
using RUINORERP.PacketSpec.Commands.Cache; // 引用PacketSpec中的缓存枚举

namespace RUINORERP.Business.Cache.Core
{
    /// <summary>
    /// 统一缓存服务 - 通用缓存核心功能
    /// 提供基础的缓存操作，不包含客户端或服务器特定功能
    /// </summary>
    public class UnifiedCacheService
    {
        private readonly CacheManager _cacheManager;
        private readonly ILogger<UnifiedCacheService> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceProvider">服务提供者</param>
        /// <param name="logger">日志记录器</param>
        public UnifiedCacheService(IServiceProvider serviceProvider, ILogger<UnifiedCacheService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // 创建缓存提供者列表
            var cacheProviders = new List<ICacheProvider>();

            // 创建内存缓存提供者
            var memoryCacheProvider = CreateMemoryCacheProvider(serviceProvider);
            cacheProviders.Add(memoryCacheProvider);

            // 尝试创建Redis缓存提供者
            var redisCacheProvider = CreateRedisCacheProvider(serviceProvider);
            if (redisCacheProvider != null)
            {
                cacheProviders.Add(redisCacheProvider);
            }

            // 创建统一缓存管理器
            var defaultPolicy = CachePolicy.Default;
            _cacheManager = new CacheManager(logger, defaultPolicy, cacheProviders);
        }

        /// <summary>
        /// 获取缓存管理器
        /// </summary>
        public CacheManager CacheManager => _cacheManager;

        /// <summary>
        /// 创建内存缓存提供者
        /// </summary>
        /// <param name="serviceProvider">服务提供者</param>
        /// <returns>内存缓存提供者</returns>
        private MemoryCacheProvider CreateMemoryCacheProvider(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<MemoryCacheProvider>>();
            var memoryCache = serviceProvider.GetRequiredService<Microsoft.Extensions.Caching.Memory.IMemoryCache>();
            var defaultPolicy = CachePolicy.Default;

            return new MemoryCacheProvider(logger, defaultPolicy, memoryCache);
        }

        /// <summary>
        /// 创建Redis缓存提供者
        /// </summary>
        /// <param name="serviceProvider">服务提供者</param>
        /// <returns>Redis缓存提供者，如果无法创建则返回null</returns>
        private RedisCacheProvider CreateRedisCacheProvider(IServiceProvider serviceProvider)
        {
            try
            {
                var logger = serviceProvider.GetRequiredService<ILogger<RedisCacheProvider>>();
                var redisCacheManager = serviceProvider.GetService<IRedisCacheManager>();
                var defaultPolicy = CachePolicy.Default;

                if (redisCacheManager != null)
                {
                    return new RedisCacheProvider(logger, defaultPolicy, redisCacheManager);
                }

                _logger.LogWarning("无法创建Redis缓存提供者，IRedisCacheManager服务未注册");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建Redis缓存提供者失败");
                return null;
            }
        }

        /// <summary>
        /// 获取缓存项
        /// </summary>
        /// <typeparam name="T">缓存项类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>缓存项</returns>
        public T Get<T>(string key)
        {
            return _cacheManager.Get<T>(key);
        }

        /// <summary>
        /// 异步获取缓存项
        /// </summary>
        /// <typeparam name="T">缓存项类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>缓存项</returns>
        public async Task<T> GetAsync<T>(string key)
        {
            return await _cacheManager.GetAsync<T>(key);
        }

        /// <summary>
        /// 设置缓存项
        /// </summary>
        /// <typeparam name="T">缓存项类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="expiration">过期时间</param>
        public void Set<T>(string key, T value, TimeSpan? expiration = null)
        {
            _cacheManager.Set(key, value, expiration);
        }

        /// <summary>
        /// 异步设置缓存项
        /// </summary>
        /// <typeparam name="T">缓存项类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="expiration">过期时间</param>
        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            await _cacheManager.SetAsync(key, value, expiration);
        }

        /// <summary>
        /// 添加或更新缓存项
        /// </summary>
        /// <typeparam name="T">缓存项类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="addValue">新增值</param>
        /// <param name="updateValueFactory">更新值工厂函数</param>
        /// <returns>操作结果</returns>
        public bool AddOrUpdate<T>(string key, T addValue, Func<T, T> updateValueFactory)
        {
            return _cacheManager.AddOrUpdate(key, addValue, updateValueFactory);
        }

        /// <summary>
        /// 异步添加或更新缓存项
        /// </summary>
        /// <typeparam name="T">缓存项类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="addValue">新增值</param>
        /// <param name="updateValueFactory">更新值工厂函数</param>
        /// <returns>操作结果</returns>
        public async Task<bool> AddOrUpdateAsync<T>(string key, T addValue, Func<T, T> updateValueFactory)
        {
            return await _cacheManager.AddOrUpdateAsync(key, addValue, updateValueFactory);
        }

        /// <summary>
        /// 移除缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>是否移除成功</returns>
        public bool Remove(string key)
        {
            return _cacheManager.Remove(key);
        }

        /// <summary>
        /// 异步移除缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>是否移除成功</returns>
        public async Task<bool> RemoveAsync(string key)
        {
            return await _cacheManager.RemoveAsync(key);
        }

        /// <summary>
        /// 检查缓存项是否存在
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>是否存在</returns>
        public bool Exists(string key)
        {
            return _cacheManager.Exists(key);
        }

        /// <summary>
        /// 清空所有缓存项
        /// </summary>
        public void Clear()
        {
            _cacheManager.Clear();
        }

        /// <summary>
        /// 异步清空所有缓存项
        /// </summary>
        public async Task ClearAsync()
        {
            await _cacheManager.ClearAsync();
        }

        /// <summary>
        /// 获取所有缓存键
        /// </summary>
        /// <returns>缓存键集合</returns>
        public IEnumerable<string> GetKeys()
        {
            return _cacheManager.GetKeys();
        }
    }

    /// <summary>
    /// 统一缓存服务扩展
    /// </summary>
    public static class UnifiedCacheServiceExtensions
    {
        /// <summary>
        /// 添加统一缓存服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddUnifiedCacheService(this IServiceCollection services)
        {
            // 注册内存缓存
            services.AddMemoryCache();

            // 注册统一缓存服务
            services.AddSingleton<UnifiedCacheService>();

            return services;
        }
    }
}