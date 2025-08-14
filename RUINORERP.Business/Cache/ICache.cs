using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CacheManager.Core;



namespace RUINORERP.Business.Cache
{

        /// <summary>
        /// 缓存基础接口
        /// </summary>
        public interface ICache
        {
            /// <summary>
            /// 缓存名称（用于区分不同业务缓存）
            /// </summary>
            string CacheName { get; }

            /// <summary>
            /// 获取缓存
            /// </summary>
            /// <typeparam name="T">数据类型</typeparam>
            /// <param name="key">缓存键</param>
            /// <returns>缓存值</returns>
            T Get<T>(string key);

            /// <summary>
            /// 异步获取缓存
            /// </summary>
            Task<T> GetAsync<T>(string key);

            /// <summary>
            /// 设置缓存
            /// </summary>
            /// <typeparam name="T">数据类型</typeparam>
            /// <param name="key">缓存键</param>
            /// <param name="value">缓存值</param>
            /// <param name="expiration">过期时间</param>
            void Set<T>(string key, T value, TimeSpan? expiration = null);

            /// <summary>
            /// 异步设置缓存
            /// </summary>
            Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);

            /// <summary>
            /// 添加或更新缓存
            /// </summary>
            /// <typeparam name="T">数据类型</typeparam>
            /// <param name="key">缓存键</param>
            /// <param name="addValue">新增值</param>
            /// <param name="updateValueFactory">更新值工厂</param>
            void AddOrUpdate<T>(string key, T addValue, Func<T, T> updateValueFactory);

            /// <summary>
            /// 移除缓存
            /// </summary>
            /// <param name="key">缓存键</param>
            bool Remove(string key);

            /// <summary>
            /// 异步移除缓存
            /// </summary>
            Task<bool> RemoveAsync(string key);

            /// <summary>
            /// 批量移除缓存
            /// </summary>
            /// <param name="pattern">键匹配模式</param>
            void RemoveByPattern(string pattern);

            /// <summary>
            /// 检查缓存是否存在
            /// </summary>
            bool Exists(string key);

            /// <summary>
            /// 清空缓存
            /// </summary>
            void Clear();

            /// <summary>
            /// 获取缓存键集合
            /// </summary>
            IEnumerable<string> GetKeys();
        }

    }
