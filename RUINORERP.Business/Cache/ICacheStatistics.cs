using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.Cache
{
    /// <summary>
    /// 缓存统计接口
    /// </summary>
    public interface ICacheStatistics
    {
        /// <summary>
        /// 缓存命中次数
        /// </summary>
        long CacheHits { get; }

        /// <summary>
        /// 缓存未命中次数
        /// </summary>
        long CacheMisses { get; }

        /// <summary>
        /// 缓存命中率
        /// </summary>
        double HitRatio { get; }

        /// <summary>
        /// 缓存写入次数
        /// </summary>
        long CachePuts { get; }

        /// <summary>
        /// 缓存删除次数
        /// </summary>
        long CacheRemoves { get; }

        /// <summary>
        /// 缓存项总数
        /// </summary>
        int CacheItemCount { get; }

        /// <summary>
        /// 缓存大小（估计值，单位：字节）
        /// </summary>
        long EstimatedCacheSize { get; }

        /// <summary>
        /// 重置统计信息
        /// </summary>
        void ResetStatistics();

        /// <summary>
        /// 获取缓存项统计详情
        /// </summary>
        Dictionary<string, CacheItemStatistics> GetCacheItemStatistics();

        /// <summary>
        /// 获取按表名分组的缓存统计
        /// </summary>
        Dictionary<string, TableCacheStatistics> GetTableCacheStatistics();
    }

    /// <summary>
    /// 缓存项统计信息
    /// </summary>
    public class CacheItemStatistics
    {
        /// <summary>
        /// 缓存键
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 缓存类型（EntityList, Entity, DisplayValue）
        /// </summary>
        public string CacheType { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 最后访问时间
        /// </summary>
        public DateTime LastAccessedTime { get; set; }

        /// <summary>
        /// 访问次数
        /// </summary>
        public long AccessCount { get; set; }

        /// <summary>
        /// 大小（估计值，单位：字节）
        /// </summary>
        public long EstimatedSize { get; set; }

        /// <summary>
        /// 缓存值类型
        /// </summary>
        public string ValueType { get; set; }
    }

    /// <summary>
    /// 表缓存统计信息
    /// </summary>
    public class TableCacheStatistics
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 实体列表缓存数量
        /// </summary>
        public int EntityListCount { get; set; }

        /// <summary>
        /// 单个实体缓存数量
        /// </summary>
        public int EntityCount { get; set; }

        /// <summary>
        /// 显示值缓存数量
        /// </summary>
        public int DisplayValueCount { get; set; }

        /// <summary>
        /// 总缓存项数量
        /// </summary>
        public int TotalItemCount => EntityListCount + EntityCount + DisplayValueCount;

        /// <summary>
        /// 表缓存总大小（估计值，单位：字节）
        /// </summary>
        public long EstimatedTotalSize { get; set; }

        /// <summary>
        /// 表缓存命中率
        /// </summary>
        public double HitRatio { get; set; }
    }
}