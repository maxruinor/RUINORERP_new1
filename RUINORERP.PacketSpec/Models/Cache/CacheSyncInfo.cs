using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.PacketSpec.Models.Cache
{
    /// <summary>
    /// 缓存同步信息类，业务层的缓存架构中也会使用此类
    /// 存储单个表的缓存同步元数据
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class CacheSyncInfo
    {
        /// <summary>
        /// 表名
        /// </summary>
        [JsonProperty]
        public string TableName { get; set; }

        /// <summary>
        /// 数据数量
        /// </summary>
        [JsonProperty]
        public int DataCount { get; set; }

        /// <summary>
        /// 估计内存大小（字节）
        /// </summary>
        [JsonProperty]
        public long EstimatedSize { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        [JsonProperty]
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        [JsonProperty]
        public DateTime ExpirationTime { get; set; }

        /// <summary>
        /// 是否有过期设置
        /// </summary>
        public bool HasExpiration => ExpirationTime > DateTime.MaxValue.AddDays(-1);

        /// <summary>
        /// 源信息
        /// 用于存储额外的源数据信息
        /// </summary>
        [JsonProperty]
        public string SourceInfo { get; set; }

        /// <summary>
        /// 无参数构造函数
        /// 用于JSON序列化和反序列化
        /// </summary>
        public CacheSyncInfo()
        {
            LastUpdateTime = DateTime.Now;
            ExpirationTime = DateTime.MaxValue; // 默认永不过期
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tableName">表名</param>
        public CacheSyncInfo(string tableName) : this()
        {
            TableName = tableName;
        }

        /// <summary>
        /// 检查是否需要同步
        /// 当数据数量不同或缓存已过期时需要同步
        /// </summary>
        /// <param name="other">其他缓存同步信息</param>
        /// <returns>如果需要同步返回true</returns>
        public bool NeedsSync(CacheSyncInfo other)
        {
            if (other == null)
                return true;

            // 检查是否过期
            if (HasExpiration && ExpirationTime < DateTime.Now)
                return true;

            // 检查数据数量是否不同
            return DataCount != other.DataCount;
        }

        /// <summary>
        /// 检查是否需要同步（无参数版本）
        /// 当缓存已过期时需要同步
        /// </summary>
        /// <returns>如果需要同步返回true</returns>
        public bool NeedsSync()
        {
            // 检查是否过期
            return HasExpiration && ExpirationTime < DateTime.Now;
        }

        /// <summary>
        /// 创建缓存同步信息的副本
        /// </summary>
        /// <returns>缓存同步信息副本</returns>
        public CacheSyncInfo Clone()
        {
            return new CacheSyncInfo(TableName)
            {
                DataCount = DataCount,
                EstimatedSize = EstimatedSize,
                LastUpdateTime = LastUpdateTime,
                ExpirationTime = ExpirationTime
            };
        }
    }
}
