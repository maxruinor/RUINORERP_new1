using Microsoft.Extensions.Logging;
using RUINORERP.Business.Cache;
using RUINORERP.PacketSpec.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Services.Cache
{
    /// <summary>
    /// 缓存工具类 - 提供缓存状态、统计等通用功能
    /// 注意：优化版本，简化实现并更好地利用业务层功能
    /// </summary>
    public class CacheUtility : IDisposable
    {
        private readonly ILogger _log;
        private readonly IEntityCacheManager _cacheManager;
        private bool _disposed = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CacheUtility(ILogger log, IEntityCacheManager cacheManager)
        {            
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
        }
        
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {            
            _disposed = true;
        }

     

        /// <summary>
        /// 清理指定表的缓存
        /// </summary>
        public void ClearCache(string tableName)
        {            
            if (_disposed)
            {                
                throw new ObjectDisposedException(GetType().Name);
            }

            try
            {                
                if (string.IsNullOrEmpty(tableName))
                {                    
                    _log?.LogWarning("清理缓存时表名为空");
                    return;
                }

                _cacheManager.DeleteEntityList(tableName);
            }
            catch (Exception ex)
            {                
                _log?.LogError(ex, "清理缓存失败，表名={0}", tableName);
            }
        }
    }

    /// <summary>
    /// 缓存状态信息
    /// </summary>
    public class CacheStatusInfo
    {        
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 记录数量
        /// </summary>
        public int RecordCount { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// 是否已缓存
        /// </summary>
        public bool IsCached { get; set; }
        
        /// <summary>
        /// 缓存状态
        /// </summary>
        public string Status { get; set; } = "Unknown";
    }

    /// <summary>
    /// 缓存统计信息
    /// </summary>
    public class CacheStatistics
    {        
        /// <summary>
        /// 总表数
        /// </summary>
        public int TotalTables { get; set; }

        /// <summary>
        /// 缓存表数
        /// </summary>
        public int CachedTableCount { get; set; }

        /// <summary>
        /// 错误表数
        /// </summary>
        public int ErrorTableCount { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// 缓存的表列表
        /// </summary>
        public List<string> CachedTables { get; set; } = new List<string>();
    }
}