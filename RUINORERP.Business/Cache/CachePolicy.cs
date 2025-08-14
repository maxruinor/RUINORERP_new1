using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CacheManager.Core;
using Microsoft.Extensions.Caching.Memory;

namespace RUINORERP.Business.Cache
{



    /// <summary>
    /// 缓存策略配置
    /// </summary>
    public class CachePolicy
    {
        public static CachePolicy Default => new CachePolicy
        {
            Expiration = TimeSpan.FromMinutes(30),
            ExpirationMode = ExpirationMode.Sliding,
            StorageType = CacheStorageType.Memory
        };


        /// <summary>
        /// 复制策略
        /// </summary>
        public CachePolicy Clone()
        {
            return new CachePolicy
            {
                Expiration = this.Expiration,
                ExpirationMode = this.ExpirationMode,
                AllowNullValues = this.AllowNullValues,
                Priority = this.Priority,
                StorageType = this.StorageType,
            };
        }

        /// <summary>
        /// 默认过期时间
        /// </summary>
        public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(30);

        /// <summary>
        /// 过期模式
        /// </summary>
        public ExpirationMode ExpirationMode { get; set; } = ExpirationMode.Sliding;

        /// <summary>
        /// 是否允许缓存空值
        /// </summary>
        public bool AllowNullValues { get; set; } = false;

        /// <summary>
        /// 缓存优先级
        /// </summary>
        public CacheItemPriority Priority { get; set; } = CacheItemPriority.Normal;

        /// <summary>
        /// 缓存存储类型
        /// </summary>
        public CacheStorageType StorageType { get; set; } = CacheStorageType.Memory;


    }

    /// <summary>
    /// 缓存存储类型
    /// </summary>
    public enum CacheStorageType
    {
        /// <summary>
        /// 内存缓存
        /// </summary>
        Memory,

        /// <summary>
        /// Redis分布式缓存
        /// </summary>
        Redis,

        /// <summary>
        /// 混合模式（内存+Redis）
        /// </summary>
        Hybrid
    }


}
