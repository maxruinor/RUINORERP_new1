using System;
using System.Collections.Generic;

namespace RUINORERP.Business.Cache
{
    /// <summary>
    /// 缓存同步元数据接口
    /// 负责管理缓存同步所需的元数据，整合了旧版CacheInfo的功能
    /// 用于客户端和服务器之间的缓存状态同步和数据一致性维护
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
    }

    /// <summary>
    /// 缓存同步信息类
    /// 存储单个表的缓存同步元数据
    /// </summary>
    public class CacheSyncInfo
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 数据数量
        /// </summary>
        public int DataCount { get; set; }

        /// <summary>
        /// 估计内存大小（字节）
        /// </summary>
        public long EstimatedSize { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpirationTime { get; set; }

        /// <summary>
        /// 是否有过期设置
        /// </summary>
        public bool HasExpiration => ExpirationTime > DateTime.MaxValue.AddDays(-1);

        /// <summary>
        /// 源信息
        /// 用于存储额外的源数据信息
        /// </summary>
        public string SourceInfo { get; set; }

        /// <summary>
        /// 无参数构造函数
        /// 用于JSON序列化和反序列化
        /// </summary>
        public CacheSyncInfo()
        {
            LastUpdateTime = DateTime.Now;
            ExpirationTime = DateTime.MaxValue; // 默认永不过期
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tableName">表名</param>
        public CacheSyncInfo(string tableName) : this()
        {
            TableName = tableName;
        }

        /// <summary>
        /// 检查是否需要同步
        /// 当数据数量不同或缓存已过期时需要同步
        /// </summary>
        /// <param name="other">其他缓存同步信息</param>
        /// <returns>如果需要同步返回true</returns>
        public bool NeedsSync(CacheSyncInfo other)
        {
            if (other == null)
                return true;

            // 检查是否过期
            if (HasExpiration && ExpirationTime < DateTime.Now)
                return true;

            // 检查数据数量是否不同
            return DataCount != other.DataCount;
        }

        /// <summary>
        /// 检查是否需要同步（无参数版本）
        /// 当缓存已过期时需要同步
        /// </summary>
        /// <returns>如果需要同步返回true</returns>
        public bool NeedsSync()
        {
            // 检查是否过期
            return HasExpiration && ExpirationTime < DateTime.Now;
        }

        /// <summary>
        /// 创建缓存同步信息的副本
        /// </summary>
        /// <returns>缓存同步信息副本</returns>
        public CacheSyncInfo Clone()
        {
            return new CacheSyncInfo(TableName)
            {
                DataCount = DataCount,
                EstimatedSize = EstimatedSize,
                LastUpdateTime = LastUpdateTime,
                ExpirationTime = ExpirationTime
            };
        }
    }
}