using RUINORERP.Business.CommService;
using RUINORERP.Common.CustomAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.Cache
{
    [NoWantIOC]
    public interface IEntityCacheManager
    {
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
        void InitializeTableSchema<T>(
            Expression<Func<T, object>> primaryKeyExpression,
            Expression<Func<T, object>> displayFieldExpression,
            bool isView = false,
            bool isCacheable = true,
            string description = null) where T : class;

        /// <summary>
        /// 获取实体类型
        /// </summary>
        Type GetEntityType(string tableName);

        /// <summary>
        /// 序列化缓存数据
        /// </summary>
        /// <param name="data">要序列化的数据</param>
        /// <param name="type">序列化方式</param>
        /// <returns>序列化后的字节数组</returns>
        byte[] SerializeCacheData<T>(T data, CacheSerializationHelper.SerializationType type = CacheSerializationHelper.SerializationType.Json);

        /// <summary>
        /// 反序列化缓存数据
        /// </summary>
        /// <param name="data">序列化后的字节数组</param>
        /// <param name="type">序列化方式</param>
        /// <returns>反序列化后的对象</returns>
        T DeserializeCacheData<T>(byte[] data, CacheSerializationHelper.SerializationType type = CacheSerializationHelper.SerializationType.Json);
        #endregion
    }
}