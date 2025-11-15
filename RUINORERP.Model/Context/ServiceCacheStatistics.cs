using System;

namespace RUINORERP.Model.Context
{
    /// <summary>
    /// 服务缓存统计信息
    /// </summary>
    public class ServiceCacheStatistics
    {
        /// <summary>
        /// 总缓存大小
        /// </summary>
        public int CacheSize { get; set; }
        
        /// <summary>
        /// 最大缓存容量
        /// </summary>
        public int MaxCacheSize { get; set; }
        
        /// <summary>
        /// 缓存命中率(百分比)
        /// </summary>
        public double HitRate { get; set; }
        
        /// <summary>
        /// 缓存命中次数
        /// </summary>
        public int HitCount { get; set; }
        
        /// <summary>
        /// 缓存未命中次数
        /// </summary>
        public int MissCount { get; set; }
        
        /// <summary>
        /// 服务类型缓存大小
        /// </summary>
        public int ServiceCacheSize { get; set; }
        
        /// <summary>
        /// 命名服务缓存大小
        /// </summary>
        public int NamedServiceCacheSize { get; set; }
        
        /// <summary>
        /// 重写ToString方法，提供格式化的统计信息
        /// </summary>
        /// <returns>格式化的统计信息字符串</returns>
        public override string ToString()
        {
            return $"缓存统计: 命中率={HitRate:F2}% ({HitCount}/{HitCount + MissCount}), " +
                   $"总缓存大小={CacheSize}, 最大容量={MaxCacheSize}, " +
                   $"服务缓存={ServiceCacheSize}, 命名服务缓存={NamedServiceCacheSize}";
        }
    }
}