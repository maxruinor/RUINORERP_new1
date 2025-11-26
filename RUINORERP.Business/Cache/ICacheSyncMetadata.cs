using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RUINORERP.PacketSpec.Models.Cache;

namespace RUINORERP.Business.Cache
{
    /// <summary>
    /// 缓存同步元数据接口
    /// 定义缓存同步元数据的管理方法
    /// 提供缓存状态验证和完整性检查功能
    /// </summary>
    public interface ICacheSyncMetadata
    {
        /// <summary>
        /// 获取指定表的缓存同步元数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>缓存同步元数据，如果不存在则返回null</returns>
        CacheSyncInfo GetTableSyncInfo(string tableName);

        /// <summary>
        /// 更新指定表的缓存同步元数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="dataCount">数据数量</param>
        /// <param name="estimatedSize">估计大小（字节）</param>
        void UpdateTableSyncInfo(string tableName, int dataCount, long estimatedSize = 0);

        /// <summary>
        /// 设置表缓存的过期时间
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="expirationTime">过期时间</param>
        void SetTableExpiration(string tableName, DateTime expirationTime);

        /// <summary>
        /// 检查表缓存是否过期
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>如果缓存已过期或不存在返回true</returns>
        bool IsTableExpired(string tableName);

        /// <summary>
        /// 获取所有表的缓存同步元数据
        /// </summary>
        /// <returns>所有表的缓存同步元数据字典</returns>
        Dictionary<string, CacheSyncInfo> GetAllTableSyncInfo();

        /// <summary>
        /// 从同步元数据中移除指定表
        /// </summary>
        /// <param name="tableName">表名</param>
        void RemoveTableSyncInfo(string tableName);

        /// <summary>
        /// 清理过期的缓存同步元数据
        /// </summary>
        void CleanupExpiredSyncInfo();

        /// <summary>
        /// 验证表缓存数据的完整性
        /// 只验证元数据是否存在且有效，不直接访问实体缓存
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>如果缓存信息有效返回true，否则返回false</returns>
        bool ValidateTableCacheIntegrity(string tableName);

        /// <summary>
        /// 获取所有缓存不完整的表
        /// </summary>
        /// <returns>缓存不完整的表名列表</returns>
        List<string> GetTablesWithIncompleteCache();

        /// <summary>
        /// 刷新缓存信息不完整的表
        /// </summary>
        /// <param name="refreshAction">刷新操作，接收表名作为参数</param>
        /// <returns>成功执行刷新操作的表数量</returns>
        int RefreshIncompleteTables(Action<string> refreshAction);

        /// <summary>
        /// 批量更新所有表的缓存同步元数据
        /// 用于客户端和服务器之间的批量同步
        /// </summary>
        /// <param name="syncData">要同步的缓存元数据字典</param>
        /// <param name="overwriteExisting">是否覆盖已存在的元数据，默认false（只更新不存在的）</param>
        void BatchUpdateSyncMetadata(Dictionary<string, CacheSyncInfo> syncData, bool overwriteExisting = false);
    }


}