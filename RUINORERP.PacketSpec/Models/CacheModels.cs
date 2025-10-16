using MessagePack;
using System;

namespace RUINORERP.PacketSpec.Commands.Cache
{
    /// <summary>
    /// 缓存条目信息模型 - 用于缓存元数据管理
    /// </summary>
    [MessagePackObject]
    public class CacheEntryInfo
    {
        /// <summary>
        /// 缓存键
        /// </summary>
        [Key(0)]
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// 缓存值类型
        /// </summary>
        [Key(1)]
        public string Type { get; set; } = "object";

        /// <summary>
        /// 创建时间（UTC）
        /// </summary>
        [Key(2)]
        public DateTime CreatedTimeUtc { get; set; } = DateTime.UtcNow;

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

        /// <summary>
        /// 获取剩余过期时间
        /// </summary>
        [IgnoreMember]
        public TimeSpan? RemainingExpiration
        {
            get
            {
                if (!Expiration.HasValue) return null;
                var remaining = CreatedTimeUtc.Add(Expiration.Value) - DateTime.UtcNow;
                return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
            }
        }

        /// <summary>
        /// 创建缓存条目信息
        /// </summary>
        public static CacheEntryInfo Create(string key, string type, TimeSpan? expiration = null)
        {
            return new CacheEntryInfo
            {
                Key = key,
                Type = type,
                CreatedTimeUtc = DateTime.UtcNow,
                Expiration = expiration
            };
        }

        /// <summary>
        /// 刷新缓存条目的创建时间
        /// </summary>
        public void Refresh()
        {
            CreatedTimeUtc = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// 缓存操作类型枚举 - 简化的核心操作类型
    /// </summary>
    public enum CacheOperation
    {
        /// <summary>
        /// 获取缓存（支持单个和批量获取）
        /// </summary>
        Get = 0,

        /// <summary>
        /// 设置缓存（支持单个和批量设置，包含更新操作）
        /// </summary>
        Set = 1,

        /// <summary>
        /// 删除缓存（支持单个和批量删除）
        /// </summary>
        Remove = 2,

        /// <summary>
        /// 清空所有缓存
        /// </summary>
        Clear = 3,

        /// <summary>
        /// 管理操作（包含：Exists、Statistics、Refresh、Size、Keys等）
        /// </summary>
        Manage = 4
    }

    /// <summary>
    /// 缓存操作结果状态
    /// </summary>
    public enum CacheOperationStatus
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 0,

        /// <summary>
        /// 失败
        /// </summary>
        Failed = 1,

        /// <summary>
        /// 缓存不存在
        /// </summary>
        NotFound = 2,

        /// <summary>
        /// 操作超时
        /// </summary>
        Timeout = 3
    }

    /// <summary>
    /// 缓存统计信息
    /// </summary>
    [MessagePackObject]
    public class CacheStatistics
    {
        /// <summary>
        /// 总条目数
        /// </summary>
        [Key(0)]
        public int TotalEntries { get; set; }

        /// <summary>
        /// 总大小（字节）
        /// </summary>
        [Key(1)]
        public long TotalSize { get; set; }

        /// <summary>
        /// 命中率
        /// </summary>
        [Key(2)]
        public double HitRate { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Key(3)]
        public DateTime CreatedTime { get; set; }
    }
}
