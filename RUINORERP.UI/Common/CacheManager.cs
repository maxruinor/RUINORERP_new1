using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using RUINORERP.Business.Cache;
using RUINORERP.Business.CommService;

namespace RUINORERP.UI.Common
{
    /// <summary>
    /// 缓存管理器静态包装类，提供便捷的缓存访问方法
    /// </summary>
    public static class CacheManager
    {
        private static readonly object _lock = new object();
        private static IEntityCacheManager _instance;

        /// <summary>
        /// 获取缓存管理器实例
        /// </summary>
        private static IEntityCacheManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            // 从依赖注入容器获取缓存管理器实例
                            _instance = Startup.GetFromFac<IEntityCacheManager>();
                        }
                    }
                }
                return _instance;
            }
        }

        #region 缓存查询方法

        /// <summary>
        /// 获取指定类型的实体列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns>实体列表</returns>
        public static List<T> GetEntityList<T>() where T : class
        {
            return Instance.GetEntityList<T>();
        }

        /// <summary>
        /// 根据表名获取指定类型的实体列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tableName">表名</param>
        /// <returns>实体列表</returns>
        public static List<T> GetEntityList<T>(string tableName) where T : class
        {
            return Instance.GetEntityList<T>(tableName);
        }

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="idValue">实体ID</param>
        /// <returns>实体对象</returns>
        public static T GetEntity<T>(object idValue) where T : class
        {
            return Instance.GetEntity<T>(idValue);
        }

        /// <summary>
        /// 根据表名和主键值获取实体
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKeyValue">主键值</param>
        /// <returns>实体对象</returns>
        public static object GetEntity(string tableName, object primaryKeyValue)
        {
            return Instance.GetEntity(tableName, primaryKeyValue);
        }

        /// <summary>
        /// 获取指定表名的显示值
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="idValue">ID值</param>
        /// <returns>显示值</returns>
        public static object GetDisplayValue(string tableName, object idValue)
        {
            return Instance.GetDisplayValue(tableName, idValue);
        }

        #endregion

        #region 缓存更新方法

        /// <summary>
        /// 更新实体列表缓存
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="list">实体列表</param>
        public static void UpdateEntityList<T>(List<T> list) where T : class
        {
            Instance.UpdateEntityList(list);
        }

        /// <summary>
        /// 更新单个实体缓存
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        public static void UpdateEntity<T>(T entity) where T : class
        {
            Instance.UpdateEntity(entity);
        }

        /// <summary>
        /// 根据表名更新缓存
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="list">实体列表</param>
        public static void UpdateEntityList(string tableName, object list)
        {
            Instance.UpdateEntityList(tableName, list);
        }

        /// <summary>
        /// 根据表名更新单个实体缓存
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="entity">实体对象</param>
        public static void UpdateEntity(string tableName, object entity)
        {
            Instance.UpdateEntity(tableName, entity);
        }

        #endregion

        #region 缓存删除方法

        /// <summary>
        /// 删除指定ID的实体缓存
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="idValue">实体ID</param>
        public static void DeleteEntity<T>(object idValue) where T : class
        {
            Instance.DeleteEntity<T>(idValue);
        }

        /// <summary>
        /// 删除实体列表缓存
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体列表</param>
        public static void DeleteEntityList<T>(List<T> entities) where T : class
        {
            Instance.DeleteEntityList(entities);
        }

        /// <summary>
        /// 删除指定表的整个实体列表缓存
        /// </summary>
        /// <param name="tableName">表名</param>
        public static void DeleteEntityList(string tableName)
        {
            Instance.DeleteEntityList(tableName);
        }

        /// <summary>
        /// 根据表名和主键删除实体缓存
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKeyValue">主键值</param>
        public static void DeleteEntity(string tableName, object primaryKeyValue)
        {
            Instance.DeleteEntity(tableName, primaryKeyValue);
        }

        /// <summary>
        /// 批量删除指定主键数组的实体缓存
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="idValues">主键值数组</param>
        public static void DeleteEntities<T>(object[] idValues) where T : class
        {
            Instance.DeleteEntities<T>(idValues);
        }

        /// <summary>
        /// 根据表名批量删除指定主键数组的实体缓存
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKeyValues">主键值数组</param>
        public static void DeleteEntities(string tableName, object[] primaryKeyValues)
        {
            Instance.DeleteEntities(tableName, primaryKeyValues);
        }

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
        /// <param name="cacheWholeRow">是否缓存整行数据</param>
        /// <param name="otherDisplayFieldExpressions">其他需要缓存的显示字段表达式</param>
        public static void InitializeTableSchema<T>(
            Expression<Func<T, object>> primaryKeyExpression,
            Expression<Func<T, object>> displayFieldExpression,
            bool isView = false,
            bool isCacheable = true,
            string description = null,
            bool cacheWholeRow = true,
            params Expression<Func<T, object>>[] otherDisplayFieldExpressions) where T : class
        {
            Instance.InitializeTableSchema(
                primaryKeyExpression,
                displayFieldExpression,
                isView,
                isCacheable,
                description,
                cacheWholeRow,
                otherDisplayFieldExpressions);
        }

        /// <summary>
        /// 获取实体类型
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>实体类型</returns>
        public static Type GetEntityType(string tableName)
        {
            return Instance.GetEntityType(tableName);
        }

        #endregion

        #region 缓存统计方法

        /// <summary>
        /// 缓存命中次数
        /// </summary>
        public static long CacheHits => Instance.CacheHits;

        /// <summary>
        /// 缓存未命中次数
        /// </summary>
        public static long CacheMisses => Instance.CacheMisses;

        /// <summary>
        /// 缓存命中率
        /// </summary>
        public static double HitRatio => Instance.HitRatio;

        /// <summary>
        /// 缓存写入次数
        /// </summary>
        public static long CachePuts => Instance.CachePuts;

        /// <summary>
        /// 缓存删除次数
        /// </summary>
        public static long CacheRemoves => Instance.CacheRemoves;

        /// <summary>
        /// 缓存项总数
        /// </summary>
        public static int CacheItemCount => Instance.CacheItemCount;

        /// <summary>
        /// 缓存大小（估计值，单位：字节）
        /// </summary>
        public static long EstimatedCacheSize => Instance.EstimatedCacheSize;

        /// <summary>
        /// 重置统计信息
        /// </summary>
        public static void ResetStatistics()
        {
            Instance.ResetStatistics();
        }

        /// <summary>
        /// 获取缓存项统计详情
        /// </summary>
        /// <returns>缓存项统计字典</returns>
        public static Dictionary<string, CacheItemStatistics> GetCacheItemStatistics()
        {
            return Instance.GetCacheItemStatistics();
        }

        /// <summary>
        /// 获取按表名分组的缓存统计
        /// </summary>
        /// <returns>表缓存统计字典</returns>
        public static Dictionary<string, TableCacheStatistics> GetTableCacheStatistics()
        {
            return Instance.GetTableCacheStatistics();
        }

        #endregion
    }
}