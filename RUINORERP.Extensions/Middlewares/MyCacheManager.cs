// MyCacheManager.cs
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CacheManager.Core;
using Fireasy.Common.Extensions;
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

        /// <summary>
        /// 获取MyCacheManager的单例实例
        /// </summary>
        public static MyCacheManager Instance
        {
            get
            {
                lock (_globalLock)
                {
                    if (_instance == null)
                    {
                        // 创建默认缓存配置
                        var cache = CacheFactory.Build<object>(settings =>
                            settings
                                .WithSystemRuntimeCacheHandle()
                                .WithExpiration(ExpirationMode.None, TimeSpan.FromSeconds(120)));
                        _instance = new MyCacheManager(cache);
                    }
                    return _instance;
                }
            }
        }

        /// <summary>
        /// 初始化单例实例，支持自定义缓存配置
        /// 此方法应在应用程序启动时调用，确保使用正确的缓存配置
        /// </summary>
        /// <param name="cache">自定义的缓存管理器实例</param>
        public static void Initialize(ICacheManager<object> cache = null)
        {
            lock (_globalLock)
            {
                if (_instance != null)
                {
                    // 如果实例已存在，不重复初始化
                    return;
                }

                // 使用提供的缓存配置或默认配置
                cache = cache ?? CacheFactory.Build<object>(settings =>
                    settings
                        .WithSystemRuntimeCacheHandle()
                        .WithExpiration(ExpirationMode.None, TimeSpan.FromSeconds(120)));

                _instance = new MyCacheManager(cache);
            }
        }
        #endregion

        #region ====== 缓存器 ======
        public ICacheManager<object> Cache { get; }
        public ICacheManager<object> CacheEntity { get; }
        public ICacheManager<object> CacheEntityList { get; }
        public ICacheManager<object> CacheInfoList { get; }
        #endregion

        #region ====== 新字典全部保留 ======
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
        private readonly ConcurrentDictionary<string, long> _versions =
            new ConcurrentDictionary<string, long>();
        private readonly ConcurrentDictionary<string, CacheStatistics> _stats =
            new ConcurrentDictionary<string, CacheStatistics>();
        private readonly ConcurrentDictionary<string, LRUCacheTracker> _lru =
            new ConcurrentDictionary<string, LRUCacheTracker>();
        #endregion

        #region ====== 构造 ======
        public MyCacheManager(ICacheManager<object> cache)
        {
            Cache = cache ?? throw new ArgumentNullException(nameof(cache));
            CacheEntity = cache;
            CacheEntityList = cache;
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

            var raw = CacheEntityList.Get(tableName);
            if (raw == null) return l;
            if (raw is List<object> ol) return ol.OfType<T>().ToList();
            if (raw is JArray ja) return ja.ToObject<List<T>>() ?? l;
            if (CacheBizTypeHelper.IsGenericList(raw.GetType()))
                return ((IEnumerable)raw).Cast<T>().ToList();
            return l;
        }
        public List<object> GetEntityList_AII(string tableName)
        {
            var raw = CacheEntityList.Get(tableName);
            return raw is IList ls ? ls.Cast<object>().ToList() : new List<object>();
        }
        #endregion

        #region ====== 旧更新方法（单实体、列表、JArray、JObject） ======
        public void UpdateEntityList<T>(List<T> list, bool hasExpire = false) =>
            UpdateEntityList(typeof(T).Name, list, hasExpire);

        public void UpdateEntityList<T>(T entity) => UpdateEntityList(typeof(T).Name, entity);

        public void UpdateEntityList(string tableName, JArray jArray, bool hasExpire = false)
        {
            if (!NewTableList.ContainsKey(tableName)) return;

            var old = CacheEntityList.Get(tableName);
            var merged = old == null ? jArray : CombineJArrays(old as JArray ?? new JArray(), jArray, NewTableList[tableName].Key);
            CacheEntityList.Put(tableName, merged);
            if (hasExpire) SetExpire(tableName);
            PublishCacheChange(tableName, merged);
            IncrementVersion(tableName);
        }

        public void UpdateEntityList(string tableName, JObject entity)
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
            CacheEntityList.Put(tableName, arr);
            PublishCacheChange(tableName, arr);
            IncrementVersion(tableName);
        }

        public void UpdateEntityList(string tableName, object list, bool hasExpire = false)
        {
            if (!NewTableList.ContainsKey(tableName)) return;

            // 处理字符串格式的数据
            if (list is string jsonString)
            {
                try
                {
                    var jArray = JArray.Parse(jsonString);
                    list = jArray;
                }
                catch (Exception ex)
                {
                    // 如果解析失败，记录错误并返回
                    System.Diagnostics.Debug.WriteLine($"解析JSON字符串失败: {ex.Message}");
                    return;
                }
            }

            var old = CacheEntityList.Get(tableName);
            var merged = old == null ? list : CombineLists(old, list, tableName);
            CacheEntityList.Put(tableName, merged);
            if (hasExpire) SetExpire(tableName);
            PublishCacheChange(tableName, merged);
            IncrementVersion(tableName);
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
            if (!NewTableList.ContainsKey(tn)) return;
            var pk = NewTableList[tn].Key;
            var idValue = entity.GetPropertyValue(pk);
            if (idValue != null && long.TryParse(idValue.ToString(), out long id))
            {
                DeleteEntityList(tn, pk, id);
            }
        }

        public void DeleteEntityList<T>(long id)
        {
            var tn = typeof(T).Name;
            if (NewTableList.ContainsKey(tn))
            {
                var pk = NewTableList[tn].Key;
                DeleteEntityList(tn, pk, id);
            }
        }

        public void DeleteEntityList<T>(long[] ids) { foreach (var id in ids) DeleteEntity<T>(id); }
        public void DeleteEntityList(string tableName, string pkCol, long id)
        {
            if (!NewTableList.ContainsKey(tableName) || !CacheEntityList.Exists(tableName)) return;
            var raw = CacheEntityList.Get(tableName);
            bool changed = false;

            if (raw is IList list)
            {
                var item = list.Cast<object>().FirstOrDefault(o => o.GetPropertyValue(pkCol)?.ToString() == id.ToString());
                if (item != null)
                {
                    list.Remove(item);
                    changed = true;
                }
            }
            else if (raw is JArray ja)
            {
                var tok = ja.FirstOrDefault(t => t[pkCol]?.ToString() == id.ToString());
                if (tok != null)
                {
                    ja.Remove(tok);
                    changed = true;
                }
            }

            if (changed)
            {
                CacheEntityList.Put(tableName, raw);
                PublishCacheChange(tableName, raw);
                IncrementVersion(tableName);
            }
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
            // 使用字典提高查找效率
            var dict = new Dictionary<string, JToken>();
            foreach (var item in old)
            {
                var key = item[pk]?.ToString();
                if (!string.IsNullOrEmpty(key))
                    dict[key] = item;
            }

            foreach (var item in @new)
            {
                var key = item[pk]?.ToString();
                if (!string.IsNullOrEmpty(key))
                    dict[key] = item;
            }

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

            void AddAll(IEnumerable src)
            {
                if (src == null) return;
                foreach (var i in src)
                {
                    var key = i?.GetPropertyValue(pk)?.ToString();
                    if (!string.IsNullOrEmpty(key))
                        dict[key] = i;
                }
            }

            AddAll((IEnumerable)old);
            AddAll((IEnumerable)@new);
            foreach (var v in dict.Values) res.Add(v);
            return res;
        }


        // 1. 发布
        private void PublishCacheChange(string tableName, object data)
        {
            if (_subscribers.TryGetValue(tableName, out var list))
            {
                // 创建快照以避免在回调执行期间集合被修改
                Action<string, object>[] callbacks;
                lock (list)
                {
                    callbacks = list.ToArray();
                }

                foreach (var cb in callbacks)
                {
                    try
                    {
                        cb(tableName, data);
                    }
                    catch
                    {
                        // 忽略回调中的异常，避免影响其他订阅者
                    }
                }
            }
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

        #region ====== 从BizCacheHelper移植的方法 ======

        /// <summary>
        /// 通过表和主键名获取值
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="IdValue">主键值</param>
        /// <returns>返回指定字段的值</returns>
        public object GetValue(string tableName, object IdValue)
        {
            object entity = null;

            // 只处理需要缓存的表
            if (NewTableList.TryGetValue(tableName, out var pair))
            {
                string key = pair.Key;
                string valueField = pair.Value;

                if (CacheEntityList.Exists(tableName))
                {
                    var rslist = CacheEntityList.Get(tableName);

                    if (CacheBizTypeHelper.IsGenericList(rslist.GetType()))
                    {
                        var lastlist = ((IEnumerable<dynamic>)rslist).ToList();
                        if (lastlist != null)
                        {
                            foreach (var item in lastlist)
                            {
                                var id = RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(item, key);
                                if (id != null && id.ToString() == IdValue.ToString())
                                {
                                    entity = RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(item, valueField);
                                    break;
                                }
                            }
                        }
                    }
                    else if (rslist != null && CacheBizTypeHelper.IsJArrayList(rslist.GetType()))
                    {
                        var lastlist = ((IEnumerable<dynamic>)rslist).ToList();
                        if (lastlist != null)
                        {
                            foreach (var item in lastlist)
                            {
                                // 将item转换为JObject
                                var obj = JObject.Parse(item.ToString());

                                // 获取ID属性的值
                                var id = obj[key]?.ToString();

                                if (id != null && id == IdValue.ToString())
                                {
                                    // 获取显示字段的值
                                    var displayValue = obj[valueField]?.ToString();
                                    if (displayValue != null)
                                    {
                                        entity = displayValue;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return entity;
        }


        /// <summary>
        /// 获取指定表名的单个实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tableName">表名</param>
        /// <returns>实体对象，如果不存在则返回null</returns>
        public T GetEntity<T>(string tableName) where T : class
        {
            var list = GetEntityList_AI<T>(tableName);
            return list?.FirstOrDefault();
        }

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="IdValue">ID值</param>
        /// <returns>实体对象，如果不存在则返回null</returns>
        public T GetEntity<T>(object IdValue) where T : class
        {
            if (IdValue == null)
            {
                return default(T);
            }

            string tableName = typeof(T).Name;

            // 只处理需要缓存的表
            if (NewTableList.TryGetValue(tableName, out var pair))
            {
                string key = pair.Key;
                string keyValue = IdValue.ToString();

                // 设置属性的值
                if (CacheEntityList.Exists(tableName))
                {
                    var cachelist = CacheEntityList.Get(tableName);

                    // 获取原始 List<T> 的类型参数
                    Type listType = cachelist.GetType();

                    if (CacheBizTypeHelper.IsGenericList(listType))
                    {
                        List<object> list = cachelist as List<object>;
                        if (list == null)
                        {
                            list = new List<object>();
                        }

                        var entity = list.Find(t => t.GetPropertyValue(key)?.ToString() == IdValue.ToString());
                        if (entity != null)
                        {
                            return (T)entity;
                        }
                    }
                    else if (CacheBizTypeHelper.IsJArrayList(listType))
                    {
                        JArray varJarray = (JArray)cachelist;
                        JToken olditem = varJarray.FirstOrDefault(n => n[pair.Key]?.ToString() == IdValue.ToString());
                        if (olditem != null)
                        {
                            return olditem.ToObject<T>();
                        }
                    }
                }
            }

            return default(T);
        }

        /// <summary>
        /// 根据表名和主键值获取实体
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="PrimaryKeyValue">主键值</param>
        /// <returns>实体对象，如果不存在则返回null</returns>
        public object GetEntity(string tableName, object PrimaryKeyValue)
        {
            object entity = null;

            // 只处理需要缓存的表
            if (NewTableList.TryGetValue(tableName, out var pair))
            {
                string key = pair.Key;

                if (CacheEntityList.Exists(tableName))
                {
                    var cachelist = CacheEntityList.Get(tableName);

                    // 获取原始 List<T> 的类型参数
                    Type listType = cachelist.GetType();

                    if (CacheBizTypeHelper.IsGenericList(listType))
                    {
                        Type elementType = listType.GetGenericArguments()[0];
                        // 创建一个新的 List<object>
                        List<object> convertedList = new List<object>();

                        // 遍历原始列表并转换元素
                        foreach (var item in (IEnumerable)cachelist)
                        {
                            convertedList.Add(item);
                        }

                        entity = convertedList.FirstOrDefault(t => t.GetPropertyValue(key)?.ToString() == PrimaryKeyValue.ToString());
                    }
                    else if (CacheBizTypeHelper.IsJArrayList(listType))
                    {
                        JArray varJarray = (JArray)cachelist;
                        // 如果旧列表中有这个值，则直接获取
                        var olditem = varJarray.FirstOrDefault(n => n[pair.Key]?.ToString() == PrimaryKeyValue.ToString());
                        Type type = NewTableTypeList.GetValue(tableName);
                        if (olditem != null)
                        {
                            return olditem.ToObject(type);
                        }
                    }
                }
            }

            return entity;
        }

        /// <summary>
        /// 获取指定表名的实体列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tableName">表名</param>
        /// <returns>实体列表，如果不存在则返回空列表</returns>
        public List<T> GetEntityList<T>(string tableName) where T : class
        {
            return GetEntityList_AI<T>(tableName) ?? new List<T>();
        }
        /// <summary>
        /// 检查值
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="expkey">主键表达式</param>
        /// <param name="value">值</param>
        public void CheckValue<T>(Expression<Func<T, int>> expkey, object value)
        {
            var mb = expkey.GetMemberInfo();
            string key = mb.Name;
            string tableName = expkey.Parameters[0].Type.Name;
            key = tableName + ":" + key + ":" + value.ToString();

            // 这里可以根据需要实现具体的检查逻辑
        }

        /// <summary>
        /// 获取字典数据源（非泛型版本）
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>字典数据</returns>
        public ConcurrentDictionary<object, object> GetDictDataSource(string tableName)
        {
            if (CacheEntityList.Exists(tableName))
            {
                var cacheData = CacheEntityList.Get(tableName);
                if (cacheData != null)
                {
                    // 创建一个字典来存储数据
                    var result = new ConcurrentDictionary<object, object>();

                    // 这里简化处理，实际可能需要根据具体数据结构进行转换
                    if (cacheData is IEnumerable enumerable)
                    {
                        int index = 0;
                        foreach (var item in enumerable)
                        {
                            result.TryAdd(index++, item);
                        }
                    }

                    return result;
                }
            }

            return new ConcurrentDictionary<object, object>();
        }

        /// <summary>
        /// 设置多个表的数据源
        /// </summary>
        /// <param name="typeNames">表名列表</param>
        /// <param name="LoadData">是否加载数据</param>
        public void SetDictDataSource(List<string> typeNames, bool LoadData = true)
        {
            // 遍历类型名称数组
            foreach (string typeName in typeNames)
            {
                if (string.IsNullOrEmpty(typeName))
                {
                    continue;
                }

                SetDictDataSource(typeName, LoadData);
            }
        }

        /// <summary>
        /// 设置指定表的数据源
        /// </summary>
        /// <param name="typeName">表名</param>
        /// <param name="LoadData">是否加载数据</param>
        public void SetDictDataSource(string typeName, bool LoadData = true)
        {
            try
            {
                // 根据表名确定实体类型
                Type entityType = GetEntityTypeByTableName(typeName);
                if (entityType != null)
                {
                    // 使用反射调用SetDictDataSource方法
                    var method = typeof(MyCacheManager).GetMethod("SetDictDataSource", new Type[] { typeof(string), typeof(bool) });
                    var genericMethod = method.MakeGenericMethod(entityType);
                    genericMethod.Invoke(this, new object[] { typeName, LoadData });
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"SetDictDataSource 未知类型: {typeName}");
                }
            }
            catch (Exception ex)
            {
                // 记录错误日志
                System.Diagnostics.Debug.WriteLine($"设置表 {typeName} 的数据源失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取字典数据源
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tableName">表名</param>
        /// <returns>实体列表</returns>
        public List<T> GetDictDataSource<T>(string tableName) where T : class
        {
            return GetEntityList_AI<T>(tableName);
        }

        /// <summary>
        /// 初始化管理器
        /// </summary>
        public void InitManager()
        {
            try
            {
                // 清除现有缓存
                CacheEntityList.Clear();
                NewTableList.Clear();
                NewTableTypeList.Clear();
                FkPairTableList.Clear();
                TableSchema.Clear();
                TableTypes.Clear();
                ForeignKeyMappings.Clear();

                // 初始化数据字典
                InitDict();
            }
            catch (Exception ex)
            {
                // 记录错误日志
                System.Diagnostics.Debug.WriteLine($"初始化管理器失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 初始化数据字典
        /// </summary>
        public void InitDict()
        {
            try
            {
                // 清除现有缓存列表
                CacheEntityList.Clear();

                // 初始化缓存字典
                InitCacheDict();
            }
            catch (Exception ex)
            {
                // 记录错误日志
                System.Diagnostics.Debug.WriteLine($"初始化数据字典失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 初始化指定表的数据字典
        /// </summary>
        /// <param name="tableName">表名</param>
        public void InitDict(string tableName)
        {
            try
            {
                // 清除现有缓存
                CacheInfoList.Clear();
                CacheEntityList.Clear();
                NewTableList.Clear();

                // 设置指定表的数据源
                Type entityType = GetEntityTypeByTableName(tableName);
                if (entityType != null)
                {
                    var method = typeof(MyCacheManager).GetMethod("SetDictDataSource", new Type[] { typeof(string), typeof(bool) });
                    var genericMethod = method.MakeGenericMethod(entityType);
                    genericMethod.Invoke(this, new object[] { tableName, true });
                }
            }
            catch (Exception ex)
            {
                // 记录错误日志
                System.Diagnostics.Debug.WriteLine($"初始化表 {tableName} 的数据字典失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 初始化缓存字典
        /// </summary>
        public void InitCacheDict(bool LoadData = true)
        {
            try
            {
                // 清除现有缓存
                CacheInfoList.Clear();
                CacheEntityList.Clear();
                NewTableList.Clear();

                // 定义需要缓存的表名数组
                string[] typeNames = new string[]
                {
                    "tb_ProdBundle", "tb_Packing", "tb_CRM_Customer", "tb_CRM_Leads", "tb_CRM_Region",
                    "tb_CRM_Contact", "tb_Provinces", "tb_Cities", "tb_Company", "tb_Currency",
                    "tb_CurrencyExchangeRate", "tb_BOM_S", "tb_ProductType", "tb_PaymentMethod",
                    "tb_Unit", "tb_Department", "tb_LocationType", "tb_Location", "tb_CustomerVendor",
                    "tb_CustomerVendorType", "tb_ProdCategories", "tb_Prod", "View_ProdDetail",
                    "View_ProdInfo", "tb_ProdPropertyType", "tb_ProdProperty", "tb_Employee",
                    "tb_UserInfo", "tb_RoleInfo", "tb_MenuInfo", "tb_FieldInfo", "tb_ModuleDefinition",
                    "tb_BizType", "tb_StorageRack", "tb_OutInStockType", "tb_OnlineStoreInfo",
                    "tb_ProjectGroup", "tb_FM_Account", "tb_FM_ExpenseType", "tb_FM_Subject",
                    "tb_RolePropertyConfig", "tb_CartoonBox", "tb_Files", "tb_BOMConfigHistory",
                    "tb_BoxRules", "tb_FM_PayeeInfo", "tb_Unit_Conversion", "tb_ProdPropertyValue"
                };

                // 为每个表名初始化缓存
                foreach (string tableName in typeNames)
                {
                    try
                    {
                        // 根据表名确定实体类型
                        Type entityType = GetEntityTypeByTableName(tableName);
                        if (entityType != null)
                        {
                            // 使用反射调用SetDictDataSource方法
                            var method = typeof(MyCacheManager).GetMethod("SetDictDataSource", new Type[] { typeof(string), typeof(bool) });
                            var genericMethod = method.MakeGenericMethod(entityType);
                            genericMethod.Invoke(this, new object[] { tableName, LoadData });
                        }
                    }
                    catch (Exception ex)
                    {
                        // 记录错误日志，但继续处理其他表
                        System.Diagnostics.Debug.WriteLine($"初始化表 {tableName} 的缓存失败: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录错误日志
                System.Diagnostics.Debug.WriteLine($"初始化缓存字典失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据表名获取实体类型
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>实体类型</returns>
        private Type GetEntityTypeByTableName(string tableName)
        {
            // 这里可以根据实际项目中的命名规则来映射表名到实体类型
            // 假设实体类名与表名相同（去掉tb_前缀）
            string entityName = tableName.StartsWith("tb_") ? tableName.Substring(3) : tableName;

            // 尝试在当前程序集中查找实体类型
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type type = assembly.GetTypes()
                    .FirstOrDefault(t => t.Name.Equals(entityName, StringComparison.OrdinalIgnoreCase));

                if (type != null)
                {
                    return type;
                }
            }

            return null;
        }

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