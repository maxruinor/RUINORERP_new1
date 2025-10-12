using MessagePack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.PacketSpec.Models.Requests.Cache
{
    /// <summary>
    /// 共享的缓存数据类型定义（客户端和服务器共用）- 非泛型版本
    /// </summary>
    [MessagePackObject]
    public class CacheData
    {
        /// <summary>
        /// 表名
        /// </summary>
        [Key(0)]
        public string TableName { get; set; }

        /// <summary>
        /// 缓存时间
        /// </summary>
        [Key(1)]
        public DateTime CacheTime { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        [Key(2)]
        public DateTime ExpirationTime { get; set; }

        /// <summary>
        /// 数据（使用JArray存储，支持动态类型）
        /// </summary>
        [Key(3)]
        public JArray Data { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        [Key(4)]
        public string Version { get; set; }

        /// <summary>
        /// 是否有更多数据
        /// </summary>
        [Key(5)]
        public bool HasMoreData { get; set; }
    }

    /// <summary>
    /// 分页缓存数据 - 非泛型版本
    /// </summary>
    [MessagePackObject]
    public class PagedCacheData
    {
        /// <summary>
        /// 缓存数据
        /// </summary>
        [Key(0)]
        public CacheData CacheData { get; set; }

        /// <summary>
        /// 页索引
        /// </summary>
        [Key(1)]
        public int PageIndex { get; set; }

        /// <summary>
        /// 页大小
        /// </summary>
        [Key(2)]
        public int PageSize { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        [Key(3)]
        public int TotalCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        [Key(4)]
        public int TotalPages { get; set; }
    }
}
