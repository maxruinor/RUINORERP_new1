using MessagePack;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.PacketSpec.Models.Requests.Cache
{
    // 强类型缓存请求
    [MessagePackObject]
    public class TypedCacheRequest<T> : RequestBase
    {
        [Key(10)]
        public string TableName { get; set; }

        [Key(11)]
        public bool ForceRefresh { get; set; }

        [Key(12)]
        public DateTime LastRequestTime { get; set; }

        [Key(13)]
        public int PageIndex { get; set; } = 0;

        [Key(14)]
        public int PageSize { get; set; } = 1000;

        [Key(15)]
        public List<string> FilterFields { get; set; }

        [Key(16)]
        public Type DataType => typeof(T);
    }

    // 强类型缓存响应
    [MessagePackObject]
    public class TypedCacheResponse<T> : ResponseBase
    {
        [Key(10)]
        public CacheData<T> CacheData { get; set; }

        [Key(11)]
        public PagedCacheData<T> PagedData { get; set; }

        [Key(12)]
        public bool IsPartial { get; set; }
    }
}
