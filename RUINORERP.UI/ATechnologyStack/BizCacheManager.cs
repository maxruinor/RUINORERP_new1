using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ATechnologyStack
{
    using System;
    using System.Collections.Concurrent;
    using System.Data;
    using System.Linq;
    using System.Threading;
    using RUINORERP.Model;
    using SqlSugar;

    public class CacheItem<T>
    {
        public DateTime ExpireTime { get; set; }
        public T Data { get; set; }
        public string Key { get; set; }
        public Func<T> Loader { get; set; }
        public int CacheSeconds { get; set; } = 300; // 默认5分钟
    }

    public class BizCacheManager
    {
        private static readonly Lazy<BizCacheManager> _instance = new Lazy<BizCacheManager>(() => new BizCacheManager());
        private readonly ConcurrentDictionary<string, object> _cache = new ConcurrentDictionary<string, object>();
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        public static BizCacheManager Instance => _instance.Value;

        private BizCacheManager()
        {
            // 初始化SqlSugar AOP
            SqlSugarSetup();
        }

        private void SqlSugarSetup()
        {
            var db = new SqlSugarClient(new ConnectionConfig()
            {
                // 配置数据库连接
            });

            // AOP事件 - 当数据变更时自动清除缓存
            db.Aop.OnLogExecuted = (sql, paras) =>
            {
                if (sql.StartsWith("UPDATE", StringComparison.OrdinalIgnoreCase) ||
                    sql.StartsWith("INSERT", StringComparison.OrdinalIgnoreCase) ||
                    sql.StartsWith("DELETE", StringComparison.OrdinalIgnoreCase))
                {
                    var tableName = GetTableNameFromSql(sql);
                    if (!string.IsNullOrEmpty(tableName))
                    {
                        Remove(tableName);
                    }
                }
            };
        }

        public T GetOrCreate<T>(string cacheKey, Func<T> loader, int cacheSeconds = 300)
        {
            _lock.EnterUpgradeableReadLock();
            try
            {
                if (_cache.TryGetValue(cacheKey, out var cachedItem))
                {
                    var item = (CacheItem<T>)cachedItem;
                    if (DateTime.Now < item.ExpireTime)
                    {
                        return item.Data;
                    }
                }

                _lock.EnterWriteLock();
                try
                {
                    var newItem = new CacheItem<T>
                    {
                        Data = loader(),
                        ExpireTime = DateTime.Now.AddSeconds(cacheSeconds),
                        Loader = loader,
                        CacheSeconds = cacheSeconds
                    };

                    _cache.AddOrUpdate(cacheKey, newItem, (k, v) => newItem);
                    return newItem.Data;
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }
            finally
            {
                _lock.ExitUpgradeableReadLock();
            }
        }

        public void Remove(string cacheKey)
        {
            _cache.TryRemove(cacheKey, out _);
        }

        public void Refresh<T>(string cacheKey)
        {
            if (_cache.TryGetValue(cacheKey, out var cachedItem))
            {
                var item = (CacheItem<T>)cachedItem;
                item.Data = item.Loader();
                item.ExpireTime = DateTime.Now.AddSeconds(item.CacheSeconds);
            }
        }

        private string GetTableNameFromSql(string sql)
        {
            // 这里需要实现SQL解析逻辑，建议根据实际SQL格式进行优化
            // 示例简单实现，实际项目需要更严谨的解析
            var parts = sql.Split(' ');
            if (sql.StartsWith("UPDATE", StringComparison.OrdinalIgnoreCase))
            {
                return parts[1];
            }
            if (sql.StartsWith("DELETE", StringComparison.OrdinalIgnoreCase))
            {
                return parts[2];
            }
            if (sql.StartsWith("INSERT", StringComparison.OrdinalIgnoreCase))
            {
                return parts[2];
            }
            return null;
        }
    }

    // 使用示例

    /*
    public class ProjectGroupService
    {
        private readonly SqlSugarClient _db;

        public ProjectGroupService()
        {
            _db = new SqlSugarClient(new ConnectionConfig()
            {
                // 数据库配置
            });
        }

        public List<tb_ProjectGroup> GetAllProjectGroups()
        {
            return BizCacheManager.Instance.GetOrCreate("tb_ProjectGroup",
                () => _db.Queryable<tb_ProjectGroup>().ToList(),
                300); // 缓存5分钟
        }

        public List<tb_ProjectGroupAccountMappers> GetMappings()
        {
            return BizCacheManager.Instance.GetOrCreate("tb_ProjectGroupAccountMappers",
                () => _db.Queryable<tb_ProjectGroupAccountMappers>().ToList(),
                300);
        }
    }
    */



}
