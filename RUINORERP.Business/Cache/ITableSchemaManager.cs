using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace RUINORERP.Business.Cache
{
    /// <summary>
    /// 表结构信息管理器接口，统一管理所有表的元数据信息
    /// </summary>
    public interface ITableSchemaManager
    {
        #region 公共属性
        /// <summary>
        /// 获取所有注册的表结构信息
        /// </summary>
        IReadOnlyDictionary<string, TableSchemaInfo> TableSchemas { get; }

        /// <summary>
        /// 获取所有需要缓存的表名
        /// </summary>
        IEnumerable<string> CacheableTableNames { get; }
        #endregion

        #region 注册方法
        /// <summary>
        /// 注册表结构信息（泛型方法）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="primaryKeyExpression">主键字段表达式</param>
        /// <param name="displayFieldExpression">主显示字段表达式</param>
        /// <param name="isView">是否是视图</param>
        /// <param name="isCacheable">是否需要缓存</param>
        /// <param name="description">表描述</param>
        /// <param name="cacheWholeRow">是否缓存整行数据（true）还是只缓存指定字段（false）</param>
        /// <param name="otherDisplayFieldExpressions">其他需要缓存的显示字段表达式</param>
        void RegisterTableSchema<T>(
            Expression<Func<T, object>> primaryKeyExpression,
            Expression<Func<T, object>> displayFieldExpression,
            bool isView = false,
            bool isCacheable = true,
            string description = null,
            bool cacheWholeRow = true,
            params Expression<Func<T, object>>[] otherDisplayFieldExpressions) where T : class;

        /// <summary>
        /// 注册表结构信息（非泛型方法）
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="primaryKeyField">主键字段名</param>
        /// <param name="displayField">主显示字段名</param>
        /// <param name="isView">是否是视图</param>
        /// <param name="isCacheable">是否需要缓存</param>
        /// <param name="description">表描述</param>
        /// <param name="otherDisplayFields">其他需要缓存的显示字段名</param>
        /// <param name="cacheWholeRow">是否缓存整行数据（true）还是只缓存指定字段（false）</param>
        void RegisterTableSchema(
            Type entityType,
            string primaryKeyField,
            string displayField,
            bool isView = false,
            bool isCacheable = true,
            string description = null,
            bool cacheWholeRow = true,
            params string[] otherDisplayFields);
        #endregion

        #region 查询方法
        /// <summary>
        /// 根据表名获取表结构信息
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>表结构信息，如果不存在返回null</returns>
        TableSchemaInfo GetSchemaInfo(string tableName);

        /// <summary>
        /// 根据实体类型获取表结构信息
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>表结构信息，如果不存在返回null</returns>
        TableSchemaInfo GetSchemaInfo(Type entityType);

        /// <summary>
        /// 根据表名获取实体类型
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>实体类型，如果不存在返回null</returns>
        Type GetEntityType(string tableName);

        /// <summary>
        /// 获取主键字段名
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>主键字段名</returns>
        string GetPrimaryKeyField(string tableName);

        /// <summary>
        /// 获取主显示字段名
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>主显示字段名</returns>
        string GetDisplayField(string tableName);

        /// <summary>
        /// 获取所有显示字段名列表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>显示字段名列表</returns>
        List<string> GetDisplayFields(string tableName);

        /// <summary>
        /// 获取外键关系列表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>外键关系列表</returns>
        List<ForeignKeyRelation> GetForeignKeys(string tableName);

        /// <summary>
        /// 检查表是否已注册
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>是否已注册</returns>
        bool ContainsTable(string tableName);

        /// <summary>
        /// 检查实体类型是否已注册
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>是否已注册</returns>
        bool ContainsType(Type entityType);
        #endregion

        #region 辅助方法
        /// <summary>
        /// 获取所有已注册的表名
        /// </summary>
        /// <returns>表名列表</returns>
        List<string> GetAllTableNames();

        /// <summary>
        /// 获取所有需要缓存的表名
        /// </summary>
        /// <returns>需要缓存的表名列表</returns>
        List<string> GetCacheableTableNamesList();

        /// <summary>
        /// 根据表类型获取表名列表
        /// </summary>
        /// <param name="tableType">表类型</param>
        /// <returns>指定类型的表名列表</returns>
        List<string> GetTableNamesByType(TableType tableType);

        /// <summary>
        /// 获取指定类型且需要缓存的表名列表
        /// </summary>
        /// <param name="tableType">表类型</param>
        /// <returns>指定类型且需要缓存的表名列表</returns>
        List<string> GetCacheableTableNamesByType(TableType tableType);

        /// <summary>
        /// 获取所有基础业务表名（通常在系统初始化时需要加载）
        /// </summary>
        /// <returns>基础业务表名列表</returns>
        List<string> GetBaseBusinessTableNames();

        /// <summary>
        /// 清除所有注册的表结构信息
        /// </summary>
        void Clear();

        /// <summary>
        /// 获取统计信息
        /// </summary>
        /// <returns>统计信息字符串</returns>
        string GetStatistics();

        /// <summary>
        /// 获取所有表结构信息的详细字符串表示
        /// </summary>
        /// <returns>所有表结构信息的字符串表示</returns>
        string GetAllSchemaInfoString();

        /// <summary>
        /// 获取所有表结构信息
        /// </summary>
        /// <returns>所有表结构信息</returns>
        IEnumerable<TableSchemaInfo> GetAllSchemaInfo();
        #endregion
    }
}