using RUINORERP.Common.CustomAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.CommService
{

    /// <summary>
    /// 缓存管理器接口
    /// </summary>
    /// <typeparam name="T">缓存数据类型</typeparam>
    [NoWantIOC]
    public interface ICacheManager<T>
    {
        /// <summary>
        /// 获取缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>缓存值</returns>
        T Get(string key);

        /// <summary>
        /// 异步获取缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>缓存值</returns>
        Task<T> GetAsync(string key);

        /// <summary>
        /// 添加缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="expiration">过期时间</param>
        void Add(string key, T value, TimeSpan expiration);

        /// <summary>
        /// 异步添加缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="expiration">过期时间</param>
        Task AddAsync(string key, T value, TimeSpan expiration);

        /// <summary>
        /// 移除缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        void Remove(string key);

        /// <summary>
        /// 异步移除缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        Task RemoveAsync(string key);

        /// <summary>
        /// 检查缓存项是否存在
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>是否存在</returns>
        bool Exists(string key);

        /// <summary>
        /// 清空所有缓存
        /// </summary>
        void Clear();
    }
}
