using System;
using System.Collections.Generic;

namespace RUINORERP.Business.Cache
{
    /// <summary>
    /// 基础表缓存管理器接口
    /// 定义管理基础表缓存信息的核心功能
    /// </summary>
    public interface IBaseTableCacheManager
    {
        /// <summary>
        /// 获取所有基础表的缓存信息
        /// </summary>
        /// <returns>所有基础表的缓存信息列表</returns>
        List<BaseTableCacheInfo> GetAllBaseTablesCacheInfo();

        /// <summary>
        /// 获取指定表的缓存信息
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>表的缓存信息，如果不存在则返回null</returns>
        BaseTableCacheInfo GetBaseTableCacheInfo(string tableName);

        /// <summary>
        /// 验证表缓存数据的完整性
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>如果缓存数据完整返回true，否则返回false</returns>
        bool ValidateTableCacheIntegrity(string tableName);

        /// <summary>
        /// 获取所有缓存不完整的表
        /// </summary>
        /// <returns>缓存不完整的表名列表</returns>
        List<string> GetTablesWithIncompleteCache();

        /// <summary>
        /// 更新基础表缓存信息并验证完整性
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="entityList">实体列表</param>
        /// <returns>如果更新成功且缓存完整返回true，否则返回false</returns>
        bool UpdateBaseTableCache<T>(string tableName, List<T> entityList) where T : class;

        /// <summary>
        /// 刷新缓存不完整的表
        /// </summary>
        /// <param name="refreshAction">刷新操作，接收表名作为参数</param>
        /// <returns>成功刷新的表数量</returns>
        int RefreshIncompleteTables(Action<string> refreshAction);

        /// <summary>
        /// 设置基础表缓存信息
        /// 用于更新和保存基础表的缓存元数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="cacheInfo">缓存信息对象</param>
        void SetBaseTableCacheInfo(string tableName, BaseTableCacheInfo cacheInfo);
    }
}