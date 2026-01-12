using System;using System.Collections.Generic;using System.Threading.Tasks;

namespace RUINORERP.Business.Cache
{
    /// <summary>
    /// 缓存数据提供者接口，用于定义从数据源获取缓存数据的方法
    /// 解决缓存过期时需要立即从服务器获取最新数据的问题
    /// </summary>
    public interface ICacheDataProvider
    {
        /// <summary>
        /// 获取实体列表数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tableName">表名</param>
        /// <returns>实体列表</returns>
        List<T> GetEntityListFromSource<T>(string tableName) where T : class;

        /// <summary>
        /// 根据ID获取实体数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tableName">表名</param>
        /// <param name="idValue">主键值</param>
        /// <returns>实体对象</returns>
        T GetEntityFromSource<T>(string tableName, object idValue) where T : class;

        /// <summary>
        /// 异步获取实体列表数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tableName">表名</param>
        /// <returns>实体列表</returns>
        Task<List<T>> GetEntityListFromSourceAsync<T>(string tableName) where T : class;

        /// <summary>
        /// 异步根据ID获取实体数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tableName">表名</param>
        /// <param name="idValue">主键值</param>
        /// <returns>实体对象</returns>
        Task<T> GetEntityFromSourceAsync<T>(string tableName, object idValue) where T : class;
    }
}