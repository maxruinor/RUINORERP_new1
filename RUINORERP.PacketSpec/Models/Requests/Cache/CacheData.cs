using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.PacketSpec.Models.Requests.Cache
{
    // 共享的缓存数据类型定义（客户端和服务器共用）
    [MessagePackObject]
    public class CacheData<T>
    {
        [Key(0)]
        public string TableName { get; set; }

        [Key(1)]
        public DateTime CacheTime { get; set; }

        [Key(2)]
        public DateTime ExpirationTime { get; set; }

        [Key(3)]
        public List<T> Data { get; set; }

        [Key(4)]
        public string Version { get; set; }

        [Key(5)]
        public bool HasMoreData { get; set; }
    }

    // 分页缓存数据
    [MessagePackObject]
    public class PagedCacheData<T>
    {
        [Key(0)]
        public CacheData<T> CacheData { get; set; }

        [Key(1)]
        public int PageIndex { get; set; }

        [Key(2)]
        public int PageSize { get; set; }

        [Key(3)]
        public int TotalCount { get; set; }

        [Key(4)]
        public int TotalPages { get; set; }
    }
}
