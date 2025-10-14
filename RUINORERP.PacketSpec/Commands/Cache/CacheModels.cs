using MessagePack;
using System;

namespace RUINORERP.PacketSpec.Commands.Cache
{
    /// <summary>
    /// 缓存条目信息
    /// </summary>
    [MessagePackObject]
    public class CacheEntryInfo
    {
        /// <summary>
        /// 缓存键
        /// </summary>
        [Key(0)]
        public string Key { get; set; }

        /// <summary>
        /// 缓存值类型
        /// </summary>
        [Key(1)]
        public string Type { get; set; }

        /// <summary>
        /// 创建时间（UTC）
        /// </summary>
        [Key(2)]
        public DateTime CreatedTimeUtc { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        [Key(3)]
        public TimeSpan? Expiration { get; set; }

        /// <summary>
        /// 是否已过期
        /// </summary>
        [IgnoreMember]
        public bool IsExpired => Expiration.HasValue &&
            DateTime.UtcNow > CreatedTimeUtc.Add(Expiration.Value);
    }

    /// <summary>
    /// 缓存操作类型
    /// </summary>
    public enum CacheOperation
    {
        /// <summary>
        /// 设置
        /// </summary>
        Set,

        /// <summary>
        /// 批量设置
        /// </summary>
        BatchSet,

        /// <summary>
        /// 删除
        /// </summary>
        Remove,

        /// <summary>
        /// 批量删除
        /// </summary>
        BatchRemove,

        /// <summary>
        /// 清空
        /// </summary>
        Clear,

        /// <summary>
        /// 更新
        /// </summary>
        Update,

        /// <summary>
        /// 批量更新
        /// </summary>
        BatchUpdate
    }

}
