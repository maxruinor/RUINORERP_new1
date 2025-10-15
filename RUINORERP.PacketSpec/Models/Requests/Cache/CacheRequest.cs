using System;using System.Collections.Generic;
using RUINORERP.PacketSpec.Models.Requests;
using MessagePack;
using RUINORERP.PacketSpec.Commands.Cache;

namespace RUINORERP.PacketSpec.Models.Requests.Cache
{
    /// <summary>
    /// 统一缓存请求模型 - 用于所有缓存相关操作
    /// 通过 OperationType 区分不同的缓存操作
    /// </summary>
    [Serializable]
    [MessagePackObject]
    public class CacheRequest : RequestBase
    {
        /// <summary>
        /// 表名
        /// </summary>
        [Key(11)]
        public string TableName { get; set; } = string.Empty;

        /// <summary>
        /// 缓存操作类型
        /// </summary>
        [Key(12)]
        public CacheOperation Operation { get; set; } = CacheOperation.Get;

        /// <summary>
        /// 缓存数据（用于设置、更新操作）
        /// </summary>
        [Key(13)]
        public object Data { get; set; }

        /// <summary>
        /// 操作参数字典（用于传递各种操作参数）
        /// </summary>
        [Key(14)]
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// 是否强制刷新缓存
        /// </summary>
        [Key(15)]
        public bool ForceRefresh { get; set; } = false;

        /// <summary>
        /// 上次请求时间，用于增量更新
        /// </summary>
        [Key(16)]
        public DateTime LastRequestTime { get; set; } = DateTime.MinValue;

        /// <summary>
        /// 创建缓存获取请求
        /// </summary>
        public static CacheRequest CreateGetRequest(string tableName, bool forceRefresh = false)
        {
            return new CacheRequest
            {
                TableName = tableName,
                Operation = CacheOperation.Get,
                ForceRefresh = forceRefresh,
            };
        }

        /// <summary>
        /// 创建缓存设置请求
        /// </summary>
        public static CacheRequest CreateSetRequest(string tableName, object data)
        {
            return new CacheRequest
            {
                TableName = tableName,
                Operation = CacheOperation.Set,
                Data = data,
            };
        }

        /// <summary>
        /// 创建缓存删除请求
        /// </summary>
        public static CacheRequest CreateDeleteRequest(string tableName, string primaryKeyName = null, object primaryKeyValue = null)
        {
            var request = new CacheRequest
            {
                TableName = tableName,
                Operation = CacheOperation.Remove,
            };

            if (!string.IsNullOrEmpty(primaryKeyName))
            {
                request.Parameters["PrimaryKeyName"] = primaryKeyName;
                request.Parameters["PrimaryKeyValue"] = primaryKeyValue;
            }

            return request;
        }

        /// <summary>
        /// 创建缓存清空请求
        /// </summary>
        public static CacheRequest CreateClearRequest(string tableName = null)
        {
            return new CacheRequest
            {
                TableName = tableName ?? string.Empty,
                Operation = CacheOperation.Clear,
            };
        }

        /// <summary>
        /// 创建缓存更新请求
        /// </summary>
        public static CacheRequest CreateUpdateRequest(string tableName, object data)
        {
            return new CacheRequest
            {
                TableName = tableName,
                Operation = CacheOperation.Update,
                Data = data,
            };
        }

        /// <summary>
        /// 创建缓存同步请求
        /// </summary>
        public static CacheRequest CreateSyncRequest(string syncMode = "FULL", List<string> cacheKeys = null, DateTime? lastSyncTime = null)
        {
            var request = new CacheRequest
            {
                Operation = CacheOperation.BatchUpdate, // 使用批量更新作为同步操作
                Parameters = new Dictionary<string, object>()
            };

            request.Parameters["SyncMode"] = syncMode;
            if (cacheKeys != null)
                request.Parameters["CacheKeys"] = cacheKeys;
            if (lastSyncTime.HasValue)
                request.Parameters["LastSyncTime"] = lastSyncTime.Value;

            return request;
        }

        /// <summary>
        /// 创建缓存获取请求（兼容旧版本）
        /// </summary>
        public static CacheRequest Create(string tableName, bool forceRefresh = false)
        {
            return CreateGetRequest(tableName, forceRefresh);
        }

        /// <summary>
        /// 获取参数值
        /// </summary>
        public T GetParameter<T>(string key, T defaultValue = default)
        {
            if (Parameters.TryGetValue(key, out var value))
            {
                try
                {
                    return CacheDataConverter.ConvertToType<T>(value);
                }
                catch
                {
                    return defaultValue;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 设置参数值
        /// </summary>
        public void SetParameter(string key, object value)
        {
            Parameters[key] = value;
        }
    }
}
