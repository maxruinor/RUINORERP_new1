using System;using System.Collections.Generic;
using RUINORERP.PacketSpec.Models.Requests;
using MessagePack;

namespace RUINORERP.PacketSpec.Models.Requests.Cache
{
    /// <summary>
    /// 缓存请求模型 - 用于客户端请求服务器缓存数据
    /// </summary>
    [Serializable]
    [MessagePackObject]
    public class CacheRequest : RequestBase
    {
        /// <summary>
        /// 请求的表名，如果为空则请求所有缓存数据
        /// </summary>
        [Key(11)]
        public string TableName { get; set; } = string.Empty;

        /// <summary>
        /// 是否强制刷新缓存
        /// </summary>
        [Key(12)]
        public bool ForceRefresh { get; set; } = false;

        /// <summary>
        /// 上次请求时间，用于增量更新
        /// </summary>
        [Key(13)]
        public DateTime LastRequestTime { get; set; } = DateTime.MinValue;

        /// <summary>
        /// 额外参数
        /// </summary>
        [Key(14)]
        public Dictionary<string, string> ExtraParameters { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 创建缓存请求
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="forceRefresh">是否强制刷新</param>
        /// <param name="clientInfo">客户端信息</param>
        /// <returns>缓存请求实例</returns>
        public static CacheRequest Create(string tableName, bool forceRefresh = false, string clientInfo = null)
        {
            return new CacheRequest
            {
                TableName = tableName,
                ForceRefresh = forceRefresh,
                OperationType = "Cache.Request",
            };
        }

        /// <summary>
        /// 创建缓存刷新请求
        /// </summary>
        public static CacheRequest CreateRefreshRequest(string tableName)
        {
            return new CacheRequest
            {
                TableName = tableName,
                ForceRefresh = true,
                OperationType = "Cache.Refresh"
            };
        }
    }

    /// <summary>
    /// 缓存删除请求
    /// </summary>
    public class CacheDeleteRequest
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 主键列名（可选，用于删除特定记录）
        /// </summary>
        public string PrimaryKeyName { get; set; }

        /// <summary>
        /// 主键值（可选，用于删除特定记录）
        /// </summary>
        public object PrimaryKeyValue { get; set; }
    }

    /// <summary>
    /// 缓存获取请求
    /// </summary>
    public class CacheGetRequest
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 主键列名（可选，用于获取特定记录）
        /// </summary>
        public string PrimaryKeyName { get; set; }

        /// <summary>
        /// 主键值（可选，用于获取特定记录）
        /// </summary>
        public object PrimaryKeyValue { get; set; }
    }

    /// <summary>
    /// 缓存设置请求
    /// </summary>
    public class CacheSetRequest
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 缓存数据
        /// </summary>
        public object Data { get; set; }
    }

    /// <summary>
    /// 缓存删除请求（单条记录）
    /// </summary>
    public class CacheRemoveRequest
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 删除键
        /// </summary>
        public string Key { get; set; }
    }

    /// <summary>
    /// 缓存同步请求
    /// </summary>
    public class CacheSyncRequest
    {
        /// <summary>
        /// 同步模式（FULL: 全量同步，INCREMENTAL: 增量同步）
        /// </summary>
        public string SyncMode { get; set; } = "FULL";

        /// <summary>
        /// 需要同步的缓存键列表（可选，为空时同步所有缓存）
        /// </summary>
        public List<string> CacheKeys { get; set; } = new List<string>();

        /// <summary>
        /// 上次同步时间（用于增量同步）
        /// </summary>
        public DateTime LastSyncTime { get; set; } = DateTime.MinValue;
    }

    /// <summary>
    /// 缓存更新请求
    /// </summary>
    public class CacheUpdateRequest
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 更新的数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; } = DateTime.Now;
    }
}
