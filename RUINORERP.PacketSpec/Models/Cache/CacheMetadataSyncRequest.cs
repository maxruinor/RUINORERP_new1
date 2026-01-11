using Newtonsoft.Json;
using RUINORERP.PacketSpec.Models.Core;
using System;
using System.Collections.Generic;

namespace RUINORERP.PacketSpec.Models.Cache
{
    /// <summary>
    /// 缓存元数据同步请求
    /// 用于服务器向客户端同步缓存元数据
    /// </summary>
    [Serializable]
    public class CacheMetadataSyncRequest : RequestBase
    {
        /// <summary>
        /// 同步操作类型
        /// </summary>
        public CacheSyncOperation Operation { get; set; } = CacheSyncOperation.FullSync;

        /// <summary>
        /// 缓存元数据字典
        /// 序列化为JSON字符串传输
        /// </summary>
        public string SerializedSyncData { get; set; }

        /// <summary>
        /// 同步时间戳（服务器时间）
        /// </summary>
        public DateTime SyncTimestamp { get; set; } = DateTime.Now;

        /// <summary>
        /// 服务器会话ID
        /// </summary>
        public string ServerSessionId { get; set; }

        /// <summary>
        /// 同步版本号
        /// 用于增量同步
        /// </summary>
        public int SyncVersion { get; set; } = 1;

        /// <summary>
        /// 是否强制覆盖客户端现有数据
        /// </summary>
        public bool ForceOverwrite { get; set; } = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CacheMetadataSyncRequest()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="syncData">要同步的缓存元数据</param>
        /// <param name="operation">同步操作类型</param>
        /// <param name="forceOverwrite">是否强制覆盖</param>
        public CacheMetadataSyncRequest(Dictionary<string, CacheSyncInfo> syncData,
            CacheSyncOperation operation = CacheSyncOperation.FullSync, bool forceOverwrite = false)
        {
            Operation = operation;
            ForceOverwrite = forceOverwrite;
            SerializedSyncData = JsonConvert.SerializeObject(syncData, Formatting.None);
            SyncTimestamp = DateTime.Now;
        }

        /// <summary>
        /// 反序列化同步数据
        /// </summary>
        /// <returns>反序列化的缓存元数据字典</returns>
        public Dictionary<string, CacheSyncInfo> DeserializeSyncData()
        {
            if (string.IsNullOrEmpty(SerializedSyncData))
                return new Dictionary<string, CacheSyncInfo>();

            try
            {
                return JsonConvert.DeserializeObject<Dictionary<string, CacheSyncInfo>>(SerializedSyncData);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"反序列化缓存同步数据失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 验证同步请求的有效性
        /// </summary>
        /// <returns>如果请求有效返回true</returns>
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(SerializedSyncData) &&
                   SyncTimestamp > DateTime.MinValue &&
                   !string.IsNullOrEmpty(ServerSessionId);
        }
    }

    /// <summary>
    /// 缓存同步操作类型
    /// </summary>
    public enum CacheSyncOperation
    {
        /// <summary>
        /// 完全同步
        /// </summary>
        FullSync = 1,

        /// <summary>
        /// 增量同步
        /// </summary>
        IncrementalSync = 2,

        /// <summary>
        /// 选择性同步
        /// </summary>
        SelectiveSync = 3,

        /// <summary>
        /// 强制刷新
        /// </summary>
        ForceRefresh = 4
    }

    /// <summary>
    /// 缓存元数据同步响应
    /// </summary>
    [Serializable]
    public class CacheMetadataSyncResponse : ResponseBase
    {
        /// <summary>
        /// 缓存元数据字典
        /// </summary>
        public Dictionary<string, CacheSyncInfo> CacheMetadataData { get; set; }

        /// <summary>
        /// 成功更新的表数量
        /// </summary>
        public int UpdatedCount { get; set; }

        /// <summary>
        /// 跳过的表数量
        /// </summary>
        public int SkippedCount { get; set; }

        /// <summary>
        /// 客户端同步完成时间戳
        /// </summary>
        public DateTime ClientSyncTimestamp { get; set; } = DateTime.Now;

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public CacheMetadataSyncResponse() : base()
        {
            CacheMetadataData = new Dictionary<string, CacheSyncInfo>();
        }

        /// <summary>
        /// 构造函数（成功响应）
        /// </summary>
        /// <param name="updatedCount">成功更新的表数量</param>
        /// <param name="skippedCount">跳过的表数量</param>
        /// <param name="cacheMetadataData">缓存元数据</param>
        public CacheMetadataSyncResponse(int updatedCount, int skippedCount, Dictionary<string, CacheSyncInfo> cacheMetadataData) : base()
        {
            IsSuccess = true;
            UpdatedCount = updatedCount;
            SkippedCount = skippedCount;
            ClientSyncTimestamp = DateTime.Now;
            CacheMetadataData = cacheMetadataData ?? new Dictionary<string, CacheSyncInfo>();
        }

        /// <summary>
        /// 构造函数（失败响应）
        /// </summary>
        /// <param name="errorMessage">错误消息</param>
        public CacheMetadataSyncResponse(string errorMessage) : base()
        {
            IsSuccess = false;
            ErrorMessage = errorMessage;
            ClientSyncTimestamp = DateTime.Now;
            CacheMetadataData = new Dictionary<string, CacheSyncInfo>();
        }
    }
}
