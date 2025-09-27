// MyCacheManager.cs
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CacheManager.Core;
using Newtonsoft.Json.Linq;
using RUINORERP.Common.Extensions;
using RUINORERP.Global.CustomAttribute;
using SharpYaml.Tokens;

namespace RUINORERP.Extensions.Middlewares
{
    /// <summary>
    /// 企业级缓存管理器（100% 兼容旧调用，同时支持并发、版本、LRU、统计）
    /// </summary>
    public class MyCacheManager
    {

        #region ====== 发布/订阅核心字段 ======
        /// <summary>
        /// 表名 → 回调列表 的字典（线程安全）
        /// </summary>
        private readonly ConcurrentDictionary<string, List<Action<string, object>>> _subscribers =
            new ConcurrentDictionary<string, List<Action<string, object>>>();
        #endregion


        #region ====== 单例 ======
        private static MyCacheManager _instance;
        private static readonly object _globalLock = new object();
        public static MyCacheManager Instance
        {
            get
            {
                lock (_globalLock)
                {
                    if (_instance == null)
                    {
                        var c = CacheFactory.Build<object>(b => b.WithSystemRuntimeCacheHandle());
                        _instance = new MyCacheManager(c, c, c);
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region ====== 缓存器 ======
        public ICacheManager<object> Cache { get; }
        public ICacheManager<object> CacheEntity { get; }
        public ICacheManager<object> CacheEntityList { get; }
        public ICacheManager<object> CacheInfoList { get; }
        #endregion

        #region ====== 旧字典全部保留 ======
        public ConcurrentDictionary<string, KeyValuePair<string, string>> NewTableList { get; } =
            new ConcurrentDictionary<string, KeyValuePair<string, string>>();
        public ConcurrentDictionary<string, Type> NewTableTypeList { get; } =
            new ConcurrentDictionary<string, Type>();
        public ConcurrentDictionary<string, List<KeyValuePair<string, string>>> FkPairTableList { get; } =
            new ConcurrentDictionary<string, List<KeyValuePair<string, string>>>();
        public ConcurrentDictionary<string, KeyValuePair<string, string>> TableSchema { get; } =
            new ConcurrentDictionary<string, KeyValuePair<string, string>>();
        public ConcurrentDictionary<string, Type> TableTypes { get; } =
            new ConcurrentDictionary<string, Type>();
        public ConcurrentDictionary<string, List<KeyValuePair<string, string>>> ForeignKeyMappings { get; } =
            new ConcurrentDictionary<string, List<KeyValuePair<string, string>>>();
        #endregion

        #region ====== 新能力字段 ======
        private readonly ConcurrentDictionary<string, ReaderWriterLockSlim> _locks =
            new ConcurrentDictionary<string, ReaderWriterLockSlim>();
        private readonly ConcurrentDictionary<string, long> _versions =
            new ConcurrentDictionary<string, long>();
        private readonly ConcurrentDictionary<string, CacheStatistics> _stats =
            new ConcurrentDictionary<string, CacheStatistics>();
        private readonly ConcurrentDictionary<string, LRUCacheTracker> _lru =
            new ConcurrentDictionary<string, LRUCacheTracker>();
        #endregion

        #region ====== 构造 ======
        public MyCacheManager(ICacheManager<object> cache, ICacheManager<object> cacheEntity, ICacheManager<object> cacheEntityList)
        {
            Cache = cache;
            CacheEntity = cacheEntity;
            CacheEntityList = cacheEntityList;
            CacheInfoList = CacheFactory.Build<object>(b => b.WithSystemRuntimeCacheHandle());
        }
        #endregion

        #region ====== 旧配置方法 ======
        public void RegisterTableSchema<T>(string idColumn, string nameColumn)
        {
            var tn = typeof(T).Name;
            TableSchema.TryAdd(tn, new KeyValuePair<string, string>(idColumn, nameColumn));
            TableTypes.TryAdd(tn, typeof(T));
            NewTableList.TryAdd(tn, new KeyValuePair<string, string>(idColumn, nameColumn));
            NewTableTypeList.TryAdd(tn, typeof(T));
        }

        public void LoadForeignKeyMappings<T>()
        {
            var tn = typeof(T).Name;
            if (ForeignKeyMappings.ContainsKey(tn) || tn.Contains("View")) return;
            var fks = typeof(T).GetProperties()
                .SelectMany(p => p.GetCustomAttributes<FKRelationAttribute>()
                    .Select(a => new KeyValuePair<string, string>(
                        a.FK_IDColName,
                        a.FKTableName == "tb_ProdDetail" ? "View_ProdDetail" : a.FKTableName)))
                .ToList();
            if (fks.Any()) ForeignKeyMappings.TryAdd(tn, fks);
        }

        public void SetFkColList<T>() => SetFkColList(typeof(T));
        public void SetFkColList(Type type)
        {
            var tn = type.Name;
            if (tn.Contains("View") || FkPairTableList.ContainsKey(tn)) return;
            var lst = new List<KeyValuePair<string, string>>();
            foreach (var p in type.GetProperties())
            {
                var attr = (FKRelationAttribute)p.GetCustomAttributes(false)
                    .FirstOrDefault(a => a is FKRelationAttribute);
                if (attr == null) continue;
                var fkTn = attr.FKTableName == "tb_ProdDetail" ? "View_ProdDetail" : attr.FKTableName;
                lst.Add(new KeyValuePair<string, string>(attr.FK_IDColName, fkTn));
            }
            if (lst.Count > 0) FkPairTableList.TryAdd(tn, lst);
        }
        #endregion

        #region ====== 旧查询方法 ======
        public List<T> GetEntityList<T>() => GetEntityList_AI<T>(typeof(T).Name);
        public List<T> GetEntityList_AI<T>(string tableName)
        {
            var l = new List<T>();
            if (!NewTableList.ContainsKey(tableName)) return l;
            var rw = _locks.GetOrAdd(tableName, _ => new ReaderWriterLockSlim());
            rw.EnterReadLock();
            try
            {
                var raw = CacheEntityList.Get(tableName);
                if (raw == null) return l;
                if (raw is List<object> ol) return ol.OfType<T>().ToList();
                if (raw is JArray ja) return ja.ToObject<List<T>>() ?? l;
                if (CacheBizTypeHelper.IsGenericList(raw.GetType()))
                    return ((IEnumerable)raw).Cast<T>().ToList();
                return l;
            }
            finally { rw.ExitReadLock(); }
        }
        public List<object> GetEntityList_AII(string tableName)
        {
            var rw = _locks.GetOrAdd(tableName, _ => new ReaderWriterLockSlim());
            rw.EnterReadLock();
            try
            {
                var raw = CacheEntityList.Get(tableName);
                return raw is IList ls ? ls.Cast<object>().ToList() : new List<object>();
            }
            finally { rw.ExitReadLock(); }
        }
        #endregion

        #region ====== 旧更新方法（单实体、列表、JArray、JObject） ======
        public void UpdateEntityList<T>(List<T> list, bool hasExpire = false) =>
            UpdateEntityList(typeof(T).Name, list, hasExpire);

        public void UpdateEntityList<T>(T entity) => UpdateEntityList(typeof(T).Name, entity);

        public void UpdateEntityList(string tableName, JArray jArray, bool hasExpire = false)
        {
            var rw = _locks.GetOrAdd(tableName, _ => new ReaderWriterLockSlim());
            rw.EnterWriteLock();
            try
            {
                if (!NewTableList.ContainsKey(tableName)) return;
                var old = CacheEntityList.Get(tableName);
                var merged = old == null ? jArray : CombineJArrays(old as JArray ?? new JArray(), jArray, NewTableList[tableName].Key);
                CacheEntityList.AddOrUpdate(tableName, merged, k => merged);
                if (hasExpire) SetExpire(tableName);
                PublishCacheChange(tableName, merged);
                IncrementVersion(tableName);
            }
            finally { rw.ExitWriteLock(); }
        }

        public void UpdateEntityList(string tableName, JObject entity)
        {
            var rw = _locks.GetOrAdd(tableName, _ => new ReaderWriterLockSlim());
            rw.EnterWriteLock();
            try
            {
                if (!NewTableList.ContainsKey(tableName)) return;
                var pk = NewTableList[tableName].Key;
                var id = entity[pk]?.ToString();
                if (string.IsNullOrEmpty(id)) return;
                var old = CacheEntityList.Get(tableName);
                JArray arr = old as JArray ?? new JArray();
                var exist = arr.FirstOrDefault(t => t[pk]?.ToString() == id);
                if (exist != null) arr.Remove(exist);
                arr.Add(entity);
                CacheEntityList.AddOrUpdate(tableName, arr, k => arr);
                PublishCacheChange(tableName, arr);
                IncrementVersion(tableName);
            }
            finally { rw.ExitWriteLock(); }
        }

        public void UpdateEntityList(string tableName, object list, bool hasExpire = false)
        {
            var rw = _locks.GetOrAdd(tableName, _ => new ReaderWriterLockSlim());
            rw.EnterWriteLock();
            try
            {
                if (!NewTableList.ContainsKey(tableName)) return;
                var old = CacheEntityList.Get(tableName);
                var merged = old == null ? list : CombineLists(old, list, tableName);
                CacheEntityList.AddOrUpdate(tableName, merged, k => merged);
                if (hasExpire) SetExpire(tableName);
                PublishCacheChange(tableName, merged);
                IncrementVersion(tableName);
            }
            finally { rw.ExitWriteLock(); }
        }

        public void UpdateEntityList(Type type, List<object> newlist) =>
            UpdateEntityList(type.Name, newlist);
        #endregion

        #region ====== 旧添加方法 ======
        public void AddCacheEntityList<T>(string tableName, List<T> list, bool hasExpire = false) =>
            UpdateEntityList(tableName, list.Cast<object>().ToList(), hasExpire);
        #endregion

        #region ====== 旧删除方法 ======
        public void DeleteEntity<T>(object idValue) => DeleteEntityList(typeof(T).Name, NewTableList[typeof(T).Name].Key, long.Parse(idValue.ToString()));
        public void DeleteEntityList<T>(T entity)
        {
            var tn = typeof(T).Name;
            var pk = NewTableList[tn].Key;
            long id = entity.GetPropertyValue(pk).ToLong();
            DeleteEntityList(tn, pk, id);
        }

        public void DeleteEntityList<T>(long id)
        {
            var tn = typeof(T).Name;
            var pk = NewTableList[tn].Key;
            DeleteEntityList(tn, pk, id);
        }

        public void DeleteEntityList<T>(long[] ids) { foreach (var id in ids) DeleteEntity<T>(id); }
        public void DeleteEntityList(string tableName, string pkCol, long id)
        {
            var rw = _locks.GetOrAdd(tableName, _ => new ReaderWriterLockSlim());
            rw.EnterWriteLock();
            try
            {
                if (!NewTableList.ContainsKey(tableName) || !CacheEntityList.Exists(tableName)) return;
                var raw = CacheEntityList.Get(tableName);
                if (raw is IList list)
                {
                    var item = list.Cast<object>().FirstOrDefault(o => o.GetPropertyValue(pkCol)?.ToString() == id.ToString());
                    if (item != null) list.Remove(item);
                }
                if (raw is JArray ja)
                {
                    var tok = ja.FirstOrDefault(t => t[pkCol]?.ToString() == id.ToString());
                    if (tok != null) ja.Remove(tok);
                }
                PublishCacheChange(tableName, raw);
                IncrementVersion(tableName);
            }
            finally { rw.ExitWriteLock(); }
        }
        #endregion

        #region ====== 内部工具 ======
        private void SetExpire(string tableName)
        {
            int min = new Random().Next(60, 120);
            CacheEntityList.Expire(tableName, ExpirationMode.Absolute, TimeSpan.FromMinutes(min));
        }
        private JArray CombineJArrays(JArray old, JArray @new, string pk)
        {
            var dict = old.ToDictionary(t => t[pk]?.ToString(), t => t);
            foreach (var n in @new) dict[n[pk]?.ToString()] = n;
            return new JArray(dict.Values);
        }
        private object CombineLists(object old, object @new, string tableName)
        {
            if (!NewTableList.ContainsKey(tableName)) return @new;
            var pk = NewTableList[tableName].Key;
            var type = NewTableTypeList[tableName];
            var listType = typeof(List<>).MakeGenericType(type);
            IList res = (IList)Activator.CreateInstance(listType);
            var dict = new Dictionary<string, object>();
            void AddAll(IEnumerable src) { foreach (var i in src) dict[i.GetPropertyValue(pk)?.ToString()] = i; }
            AddAll((IEnumerable)old);
            AddAll((IEnumerable)@new);
            foreach (var v in dict.Values) res.Add(v);
            return res;
        }


        // 1. 发布
        private void PublishCacheChange(string tableName, object data)
        {
            if (_subscribers.TryGetValue(tableName, out var list))
                foreach (var cb in list.ToList())
                    Task.Run(() => cb(tableName, data));   // 异步回调
        }

        // 2. 订阅
        public void SubscribeCacheChange(string tableName, Action<string, object> callback)
        {
            if (callback == null) return;
            var list = _subscribers.GetOrAdd(tableName, _ => new List<Action<string, object>>());
            lock (list) { if (!list.Contains(callback)) list.Add(callback); }
        }

        // 3. 取消订阅
        public void UnsubscribeCacheChange(string tableName, Action<string, object> callback)
        {
            if (_subscribers.TryGetValue(tableName, out var list))
                lock (list) { list.Remove(callback); if (list.Count == 0) _subscribers.TryRemove(tableName, out _); }
        }

        private void IncrementVersion(string tableName) => _versions.AddOrUpdate(tableName, 1, (k, v) => v + 1);
        #endregion

        #region ====== 统计/LRU（保留新能力） ======
        public CacheStatistics GetCacheStatistics(string tableName) => _stats.GetOrAdd(tableName, _ => new CacheStatistics());
        public class CacheStatistics
        {
            public long TotalAccesses { get; set; }
            public long Hits { get; set; }
            public double HitRatio => TotalAccesses == 0 ? 0 : (double)Hits / TotalAccesses;
            public int ItemCount { get; set; }
            public DateTime LastAccessTime { get; set; } = DateTime.Now;
            public DateTime CreationTime { get; set; } = DateTime.Now;
        }
        public class LRUCacheTracker
        {
            private readonly object _lock = new object();
            private readonly LinkedList<string> _order = new LinkedList<string>();
            private readonly Dictionary<string, LinkedListNode<string>> _map = new Dictionary<string, LinkedListNode<string>>();
            public void RecordAccess(string key) { lock (_lock) { if (_map.ContainsKey(key)) _order.Remove(_map[key]); _map[key] = _order.AddFirst(key); } }
            public void Cleanup(int max) { lock (_lock) { while (_order.Count > max) { var last = _order.Last; _order.RemoveLast(); _map.Remove(last.Value); } } }
        }
        #endregion
    }

    #region ====== 类型帮助（保留） ======
    public static class CacheBizTypeHelper
    {
        public static bool IsGenericList(Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(List<>);
        public static bool IsJArrayList(Type t) => t == typeof(JArray);
        public static object ConvertJArrayToList(Type elementType, JArray ja)
        {
            if (ja == null) return null;
            var listType = typeof(List<>).MakeGenericType(elementType);
            IList list = (IList)Activator.CreateInstance(listType);
            foreach (var tok in ja) list.Add(tok.ToObject(elementType));
            return list;
        }
    }
    #endregion
}