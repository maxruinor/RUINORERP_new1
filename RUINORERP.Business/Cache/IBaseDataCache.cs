using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using global::RUINORERP.Model;
using RUINORERP.Model.Base;
using System.Linq.Expressions;
namespace RUINORERP.Business.Cache
{
    /// <summary>
    /// 基础数据缓存接口
    /// 用于缓存产品、客户、供应商等基础资料
    /// </summary>
    public interface IBaseDataCache : ICache
    {
        /// <summary>
        /// 获取实体列表
        /// </summary>
        List<T> GetEntityList<T>() where T : BaseEntity;

        /// <summary>
        /// 异步获取实体列表
        /// </summary>
        Task<List<T>> GetEntityListAsync<T>() where T : BaseEntity;

        /// <summary>
        /// 获取单个实体
        /// </summary>
        T GetEntity<T>(long id) where T : BaseEntity;

        /// <summary>
        /// 异步获取单个实体
        /// </summary>
        Task<T> GetEntityAsync<T>(long id) where T : BaseEntity;

        /// <summary>
        /// 设置实体列表缓存
        /// </summary>
        void SetEntityList<T>(List<T> entities, TimeSpan? expiration = null) where T : BaseEntity;

        /// <summary>
        /// 异步设置实体列表缓存
        /// </summary>
        Task SetEntityListAsync<T>(List<T> entities, TimeSpan? expiration = null) where T : BaseEntity;

        /// <summary>
        /// 更新单个实体缓存
        /// </summary>
        void UpdateEntity<T>(T entity) where T : BaseEntity;

        /// <summary>
        /// 异步更新单个实体缓存
        /// </summary>
        Task UpdateEntityAsync<T>(T entity) where T : BaseEntity;

        /// <summary>
        /// 删除实体缓存
        /// </summary>
        void RemoveEntity<T>(long id) where T : BaseEntity;

        /// <summary>
        /// 异步删除实体缓存
        /// </summary>
        Task RemoveEntityAsync<T>(long id) where T : BaseEntity;

        /// <summary>
        /// 根据条件查询缓存中的实体
        /// </summary>
        List<T> QueryEntities<T>(Expression<Func<T, bool>> predicate) where T : BaseEntity;
    }
}

