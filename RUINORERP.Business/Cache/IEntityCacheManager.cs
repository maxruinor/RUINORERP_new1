using RUINORERP.Business.CommService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.Cache
{
    public interface IEntityCacheManager : ICacheStatistics
    {
        /// <summary>
        /// 缓存键类型枚举，用于区分不同类型的缓存
        /// </summary>
        enum CacheKeyType
        {
            /// <summary>
            /// 实体列表缓存
            /// </summary>
            List,
            /// <summary>
            /// 单个实体缓存
            /// </summary>
            Entity,
            /// <summary>
            /// 显示值缓存
            /// </summary>
            DisplayValue,
            /// <summary>
            /// 自定义查询缓存
            /// </summary>
            QueryResult
        }
        
        /// <summary>
        /// 生成缓存键
        /// </summary>
        /// <param name="type">缓存类型</param>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKeyValue">可选的主键值（用于实体和显示值缓存）</param>
        /// <returns>格式化的缓存键</returns>
        string GenerateCacheKey(CacheKeyType type, string tableName, object primaryKeyValue = null);
        
        #region 缓存查询方法
        /// <summary>
        /// 获取指定类型的实体列表
        /// </summary>
        List<T> GetEntityList<T>() where T : class;

        /// <summary>
        /// 根据表名获取指定类型的实体列表
        /// </summary>
        List<T> GetEntityList<T>(string tableName) where T : class;

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        T GetEntity<T>(object idValue) where T : class;

        /// <summary>
        /// 根据表名和主键值获取实体
        /// </summary>
        object GetEntity(string tableName, object primaryKeyValue);

        /// <summary>
        /// 获取指定表名的显示值
        /// </summary>
        object GetDisplayValue(string tableName, object idValue);
        
        /// <summary>
        /// 根据表名获取实体列表，返回强类型集合
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>实体列表，类型为表对应的强类型集合</returns>
        dynamic GetEntityListByTableName(string tableName);
        #endregion

        #region 缓存更新方法
        /// <summary>
        /// 更新实体列表缓存
        /// </summary>
        void UpdateEntityList<T>(List<T> list) where T : class;

        /// <summary>
        /// 更新单个实体缓存
        /// </summary>
        void UpdateEntity<T>(T entity) where T : class;

        /// <summary>
        /// 根据表名更新缓存
        /// </summary>
        void UpdateEntityList(string tableName, object list);

        /// <summary>
        /// 根据表名更新单个实体缓存
        /// </summary>
        void UpdateEntity(string tableName, object entity);
        #endregion

        #region 缓存删除方法
        /// <summary>
        /// 删除指定ID的实体缓存
        /// </summary>
        void DeleteEntity<T>(object idValue) where T : class;

        /// <summary>
        /// 删除实体列表缓存
        /// </summary>
        void DeleteEntityList<T>(List<T> entities) where T : class;

        /// <summary>
        /// 删除指定表的整个实体列表缓存
        /// </summary>
        /// <param name="tableName">表名</param>
        void DeleteEntityList(string tableName);

        /// <summary>
        /// 根据表名和主键删除实体缓存
        /// </summary>
        void DeleteEntity(string tableName, object primaryKeyValue);

        /// <summary>
        /// 批量删除指定主键数组的实体缓存
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="idValues">主键值数组</param>
        void DeleteEntities<T>(object[] idValues) where T : class;

        /// <summary>
        /// 根据表名批量删除指定主键数组的实体缓存
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKeyValues">主键值数组</param>
        void DeleteEntities(string tableName, object[] primaryKeyValues);
        #endregion

        #region 缓存初始化方法
        /// <summary>
        /// 初始化表结构信息
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="primaryKeyExpression">主键字段表达式</param>
        /// <param name="displayFieldExpression">主显示字段表达式</param>
        /// <param name="isView">是否是视图</param>
        /// <param name="isCacheable">是否需要缓存</param>
        /// <param name="description">表描述</param>
        /// <param name="cacheWholeRow">是否缓存整行数据（true）还是只缓存指定字段（false）</param>
        /// <param name="otherDisplayFieldExpressions">其他需要缓存的显示字段表达式</param>
        void InitializeTableSchema<T>(
            Expression<Func<T, object>> primaryKeyExpression,
            Expression<Func<T, object>> displayFieldExpression,
            bool isView = false,
            bool isCacheable = true,
            string description = null,
            bool cacheWholeRow = true,
            params Expression<Func<T, object>>[] otherDisplayFieldExpressions) where T : class;

        /// <summary>
        /// 获取实体类型
        /// </summary>
        Type GetEntityType(string tableName);

      
        #endregion
    }
}