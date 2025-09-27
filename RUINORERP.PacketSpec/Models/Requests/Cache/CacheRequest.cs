using System;using System.Collections.Generic;
using RUINORERP.PacketSpec.Models.Requests;

namespace RUINORERP.PacketSpec.Models.Requests.Cache
{
    /// <summary>
    /// 缓存请求模型 - 用于客户端请求服务器缓存数据
    /// </summary>
    [Serializable]
    public class CacheRequest : RequestBase
    {
        /// <summary>
        /// 会话ID
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// 请求的表名，如果为空则请求所有缓存数据
        /// </summary>
        public string TableName { get; set; } = string.Empty;

        /// <summary>
        /// 是否强制刷新缓存
        /// </summary>
        public bool ForceRefresh { get; set; } = false;

        /// <summary>
        /// 上次请求时间，用于增量更新
        /// </summary>
        public DateTime LastRequestTime { get; set; } = DateTime.MinValue;

        /// <summary>
        /// 客户端版本号
        /// </summary>
        public string ClientVersion { get; set; } = string.Empty;

        /// <summary>
        /// 额外参数
        /// </summary>
        public Dictionary<string, string> ExtraParameters { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 创建缓存请求
        /// </summary>
        public static CacheRequest Create(string tableName, bool forceRefresh = false)
        {
            return new CacheRequest
            {
                TableName = tableName,
                ForceRefresh = forceRefresh,
                OperationType = "Cache.Request"
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
}
