using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RUINORERP.Business.Cache
{
    /// <summary>
    /// 通用缓存服务接口
    /// 提供简单统一的缓存访问方式
    /// </summary>
    /// <typeparam name="T">缓存数据类型</typeparam>
    public interface ICacheService<T> where T : class
    {
        /// <summary>
        /// 缓存名称
        /// </summary>
        string CacheName { get; }

        /// <summary>
        /// 获取单个缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>缓存值</returns>
        T Get(string key);

        /// <summary>
        /// 异步获取单个缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>缓存值</returns>
        Task<T> GetAsync(string key);

        /// <summary>
        /// 设置单个缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="expiration">过期时间</param>
        void Set(string key, T value, TimeSpan? expiration = null);

        /// <summary>
        /// 异步设置单个缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="expiration">过期时间</param>
        Task SetAsync(string key, T value, TimeSpan? expiration = null);

        /// <summary>
        /// 获取或设置缓存项
        /// 如果缓存不存在，则通过factory函数获取数据并缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="factory">数据获取工厂函数</param>
        /// <param name="expiration">过期时间</param>
        /// <returns>缓存值</returns>
        T GetOrSet(string key, Func<T> factory, TimeSpan? expiration = null);

        /// <summary>
        /// 异步获取或设置缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="factory">数据获取工厂函数</param>
        /// <param name="expiration">过期时间</param>
        /// <returns>缓存值</returns>
        Task<T> GetOrSetAsync(string key, Func<Task<T>> factory, TimeSpan? expiration = null);

        /// <summary>
        /// 移除缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>是否移除成功</returns>
        bool Remove(string key);

        /// <summary>
        /// 异步移除缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>是否移除成功</returns>
        Task<bool> RemoveAsync(string key);

        /// <summary>
        /// 检查缓存是否存在
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>缓存是否存在</returns>
        bool Exists(string key);

        /// <summary>
        /// 清空缓存
        /// </summary>
        void Clear();

        /// <summary>
        /// 获取所有缓存键
        /// </summary>
        /// <returns>缓存键集合</returns>
        IEnumerable<string> GetKeys();
    }

    /// <summary>
    /// 实体缓存服务接口
    /// 专门处理实体对象的缓存
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface IEntityCacheService<T> : ICacheService<T> where T : class
    {
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id">实体ID</param>
        /// <returns>实体对象</returns>
        T GetEntity(long id);

        /// <summary>
        /// 异步获取实体
        /// </summary>
        /// <param name="id">实体ID</param>
        /// <returns>实体对象</returns>
        Task<T> GetEntityAsync(long id);

        /// <summary>
        /// 设置实体缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="id">实体ID</param>
        /// <param name="expiration">过期时间</param>
        void SetEntity(T entity, long id, TimeSpan? expiration = null);

        /// <summary>
        /// 异步设置实体缓存
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="id">实体ID</param>
        /// <param name="expiration">过期时间</param>
        Task SetEntityAsync(T entity, long id, TimeSpan? expiration = null);

        /// <summary>
        /// 移除实体缓存
        /// </summary>
        /// <param name="id">实体ID</param>
        /// <returns>是否移除成功</returns>
        bool RemoveEntity(long id);

        /// <summary>
        /// 异步移除实体缓存
        /// </summary>
        /// <param name="id">实体ID</param>
        /// <returns>是否移除成功</returns>
        Task<bool> RemoveEntityAsync(long id);

        /// <summary>
        /// 获取实体列表
        /// </summary>
        /// <returns>实体列表</returns>
        List<T> GetEntityList();

        /// <summary>
        /// 异步获取实体列表
        /// </summary>
        /// <returns>实体列表</returns>
        Task<List<T>> GetEntityListAsync();

        /// <summary>
        /// 设置实体列表缓存
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <param name="expiration">过期时间</param>
        void SetEntityList(List<T> entities, TimeSpan? expiration = null);

        /// <summary>
        /// 异步设置实体列表缓存
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <param name="expiration">过期时间</param>
        Task SetEntityListAsync(List<T> entities, TimeSpan? expiration = null);

        /// <summary>
        /// 获取或设置实体列表缓存
        /// 如果缓存不存在，则通过factory函数获取数据并缓存
        /// </summary>
        /// <param name="factory">数据获取工厂函数</param>
        /// <param name="expiration">过期时间</param>
        /// <returns>实体列表</returns>
        List<T> GetOrSetEntityList(Func<List<T>> factory, TimeSpan? expiration = null);

        /// <summary>
        /// 异步获取或设置实体列表缓存
        /// </summary>
        /// <param name="factory">数据获取工厂函数</param>
        /// <param name="expiration">过期时间</param>
        /// <returns>实体列表</returns>
        Task<List<T>> GetOrSetEntityListAsync(Func<Task<List<T>>> factory, TimeSpan? expiration = null);
    }
}