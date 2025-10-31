using RUINORERP.Business.Cache;
using RUINORERP.Business.CommService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.Cache
{
    /// <summary>
    /// 实体缓存静态帮助类
    /// 提供静态方法直接访问实体缓存服务，避免在每个类中都需要注入服务实例
    /// </summary>
    public static class EntityCacheHelper
    {
        private static IEntityCacheManager _currentService;

        /// <summary>
        /// 设置当前使用的实体缓存服务实例
        /// 此方法应在应用程序启动时调用，通常在依赖注入容器配置完成后
        /// </summary>
        /// <param name="service">实体缓存服务实例</param>
        public static void SetCurrent(IEntityCacheManager service)
        {
            _currentService = service;
        }

        /// <summary>
        /// 获取当前实体缓存服务实例
        /// 如果尚未设置，则会抛出异常
        /// </summary>
        public static IEntityCacheManager Current
        {
            get
            {
                if (_currentService == null)
                {
                    throw new InvalidOperationException("实体缓存服务未初始化，请先调用SetCurrent方法设置服务实例");
                }
                return _currentService;
            }
        }

        #region 缓存键生成方法
        /// <summary>
        /// 生成缓存键
        /// </summary>
        /// <param name="type">缓存类型</param>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKeyValue">可选的主键值（用于实体和显示值缓存）</param>
        /// <returns>格式化的缓存键</returns>
        public static string GenerateCacheKey(IEntityCacheManager.CacheKeyType type, string tableName, object primaryKeyValue = null)
        {
            return Current.GenerateCacheKey(type, tableName, primaryKeyValue);
        }
        #endregion

        #region 缓存查询方法
        /// <summary>
        /// 获取指定类型的实体列表
        /// </summary>
        public static List<T> GetEntityList<T>() where T : class
        {
            return Current.GetEntityList<T>();
        }

        /// <summary>
        /// 根据表名获取指定类型的实体列表
        /// </summary>
        public static List<T> GetEntityList<T>(string tableName) where T : class
        {
            return Current.GetEntityList<T>(tableName);
        }

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        public static T GetEntity<T>(object idValue) where T : class
        {
            return Current.GetEntity<T>(idValue);
        }

        /// <summary>
        /// 根据表名和主键值获取实体
        /// </summary>
        public static object GetEntity(string tableName, object primaryKeyValue)
        {
            return Current.GetEntity(tableName, primaryKeyValue);
        }

        /// <summary>
        /// 获取指定表名的显示值
        /// </summary>
        public static object GetDisplayValue(string tableName, object idValue)
        {
            return Current.GetDisplayValue(tableName, idValue);
        }
        #endregion

        #region 缓存更新方法
        /// <summary>
        /// 更新实体列表缓存
        /// </summary>
        public static void UpdateEntityList<T>(List<T> list) where T : class
        {
            Current.UpdateEntityList(list);
        }

        /// <summary>
        /// 更新单个实体缓存
        /// </summary>
        public static void UpdateEntity<T>(T entity) where T : class
        {
            Current.UpdateEntity(entity);
        }

        /// <summary>
        /// 根据表名更新缓存
        /// </summary>
        public static void UpdateEntityList(string tableName, object list)
        {
            Current.UpdateEntityList(tableName, list);
        }

        /// <summary>
        /// 根据表名更新单个实体缓存
        /// </summary>
        public static void UpdateEntity(string tableName, object entity)
        {
            Current.UpdateEntity(tableName, entity);
        }
        #endregion

        #region 缓存删除方法
        /// <summary>
        /// 删除指定ID的实体缓存
        /// </summary>
        public static void DeleteEntity<T>(object idValue) where T : class
        {
            Current.DeleteEntity<T>(idValue);
        }

        /// <summary>
        /// 删除实体列表缓存
        /// </summary>
        public static void DeleteEntityList<T>(List<T> entities) where T : class
        {
            Current.DeleteEntityList(entities);
        }

        /// <summary>
        /// 删除指定表的整个实体列表缓存
        /// </summary>
        /// <param name="tableName">表名</param>
        public static void DeleteEntityList(string tableName)
        {
            Current.DeleteEntityList(tableName);
        }

        /// <summary>
        /// 根据表名和主键删除实体缓存
        /// </summary>
        public static void DeleteEntity(string tableName, object primaryKeyValue)
        {
            Current.DeleteEntity(tableName, primaryKeyValue);
        }

        /// <summary>
        /// 批量删除指定主键数组的实体缓存
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="idValues">主键值数组</param>
        public static void DeleteEntities<T>(object[] idValues) where T : class
        {
            Current.DeleteEntities<T>(idValues);
        }

        /// <summary>
        /// 根据表名批量删除指定主键数组的实体缓存
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKeyValues">主键值数组</param>
        public static void DeleteEntities(string tableName, object[] primaryKeyValues)
        {
            Current.DeleteEntities(tableName, primaryKeyValues);
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
        /// <param name="cacheWholeRow">是否缓存整行数据（true）还是只缓存指定字段（false）</param>
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
            Current.InitializeTableSchema(
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
        public static Type GetEntityType(string tableName)
        {
            return Current.GetEntityType(tableName);
        }
        #endregion

        #region 缓存同步元数据方法
        /// <summary>
        /// 获取指定表的缓存同步元数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>缓存同步元数据</returns>
        public static CacheSyncInfo GetTableSyncInfo(string tableName)
        {
            return Current.GetTableSyncInfo(tableName);
        }

        /// <summary>
        /// 设置表缓存的过期时间
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="expirationTime">过期时间</param>
        public static void SetTableCacheExpiration(string tableName, DateTime expirationTime)
        {
            Current.SetTableCacheExpiration(tableName, expirationTime);
        }
        #endregion

        #region 缓存统计方法
        /// <summary>
        /// 缓存命中次数
        /// </summary>
        public static long CacheHits => Current.CacheHits;

        /// <summary>
        /// 缓存未命中次数
        /// </summary>
        public static long CacheMisses => Current.CacheMisses;

        /// <summary>
        /// 缓存命中率
        /// </summary>
        public static double HitRatio => Current.HitRatio;

        /// <summary>
        /// 缓存写入次数
        /// </summary>
        public static long CachePuts => Current.CachePuts;

        /// <summary>
        /// 缓存删除次数
        /// </summary>
        public static long CacheRemoves => Current.CacheRemoves;

        /// <summary>
        /// 缓存项总数
        /// </summary>
        public static int CacheItemCount => Current.CacheItemCount;

        /// <summary>
        /// 缓存大小（估计值，单位：字节）
        /// </summary>
        public static long EstimatedCacheSize => Current.EstimatedCacheSize;

        /// <summary>
        /// 重置统计信息
        /// </summary>
        public static void ResetStatistics()
        {
            Current.ResetStatistics();
        }

        /// <summary>
        /// 获取缓存项统计详情
        /// </summary>
        public static Dictionary<string, CacheItemStatistics> GetCacheItemStatistics()
        {
            return Current.GetCacheItemStatistics();
        }

        /// <summary>
        /// 获取按表名分组的缓存统计
        /// </summary>
        public static Dictionary<string, TableCacheStatistics> GetTableCacheStatistics()
        {
            return Current.GetTableCacheStatistics();
        }
        #endregion
    }
}