using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CacheManager.Core;
using Microsoft.Extensions.Logging;
using RUINORERP.Common.Log4Net;
using RUINORERP.Model.Context;
namespace RUINORERP.Business.CommService
{
  
        /// <summary>
        /// 缓存管理器实现
        /// 复用系统现有缓存配置，与BizCacheHelper共享缓存机制
        /// </summary>
        /// <typeparam name="T">缓存数据类型</typeparam>
        public class CacheManager<T> : ICacheManager<T>
        {
            private readonly ICacheManager<object> _cache;
            private readonly ILogger<CacheManager<T>> _logger;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="context">应用上下文</param>
            /// <param name="logger">日志组件</param>
            public CacheManager(ApplicationContext context, ILogger<CacheManager<T>> logger)
            {
                _logger = logger;

                // 初始化缓存管理器，复用BizCacheHelper的配置
                if (BizCacheHelper.Manager == null)
                {
                    BizCacheHelper.InitManager();
                }

               // _cache = BizCacheHelper.Manager.CacheInfoList;
            }

            /// <summary>
            /// 获取缓存项
            /// </summary>
            public T Get(string key)
            {
                try
                {
                    if (string.IsNullOrEmpty(key))
                        return default;

                    var value = _cache.Get(key);
                    return value != null ? (T)value : default;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"获取缓存失败，Key: {key}");
                    return default;
                }
            }

            /// <summary>
            /// 异步获取缓存项
            /// </summary>
            public async Task<T> GetAsync(string key)
            {
                try
                {
                    if (string.IsNullOrEmpty(key))
                        return default;

                    var value = await _cache.GetAsync(key);
                    return value != null ? (T)value : default;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"异步获取缓存失败，Key: {key}");
                    return default;
                }
            }

            /// <summary>
            /// 添加缓存项
            /// </summary>
            public void Add(string key, T value, TimeSpan expiration)
            {
                try
                {
                    if (string.IsNullOrEmpty(key) || value == null)
                        return;

                    // 设置缓存及过期时间
                    _cache.Put(key, value, expiration);
                    _logger.LogDebug($"添加缓存成功，Key: {key}，过期时间: {expiration}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"添加缓存失败，Key: {key}");
                }
            }

            /// <summary>
            /// 异步添加缓存项
            /// </summary>
            public async Task AddAsync(string key, T value, TimeSpan expiration)
            {
                try
                {
                    if (string.IsNullOrEmpty(key) || value == null)
                        return;

                    // 异步设置缓存及过期时间
                    await _cache.PutAsync(key, value, expiration);
                    _logger.LogDebug($"异步添加缓存成功，Key: {key}，过期时间: {expiration}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"异步添加缓存失败，Key: {key}");
                }
            }

            /// <summary>
            /// 移除缓存项
            /// </summary>
            public void Remove(string key)
            {
                try
                {
                    if (string.IsNullOrEmpty(key))
                        return;

                    _cache.Remove(key);
                    _logger.LogDebug($"移除缓存成功，Key: {key}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"移除缓存失败，Key: {key}");
                }
            }

            /// <summary>
            /// 异步移除缓存项
            /// </summary>
            public async Task RemoveAsync(string key)
            {
                try
                {
                    if (string.IsNullOrEmpty(key))
                        return;

                    await _cache.RemoveAsync(key);
                    _logger.LogDebug($"异步移除缓存成功，Key: {key}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"异步移除缓存失败，Key: {key}");
                }
            }

            /// <summary>
            /// 检查缓存项是否存在
            /// </summary>
            public bool Exists(string key)
            {
                try
                {
                    return !string.IsNullOrEmpty(key) && _cache.Exists(key);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"检查缓存存在性失败，Key: {key}");
                    return false;
                }
            }

            /// <summary>
            /// 清空所有缓存
            /// </summary>
            public void Clear()
            {
                try
                {
                    _cache.Clear();
                    _logger.LogInformation("清空所有缓存成功");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "清空所有缓存失败");
                }
            }
        }
    }

 
