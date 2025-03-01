using CacheManager.Core;
using RUINORERP.Common.Helper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using RUINORERP.Common.Extensions;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using System.Collections;
using SharpYaml.Tokens;
using Newtonsoft.Json.Linq;
using Mapster;
using System.Linq;
using System.Web.Caching;
using Fireasy.Common.Extensions;
using SqlSugar;

namespace RUINORERP.Extensions.Middlewares
{
    /// <summary>
    /// CacheManager引用第三方框架
    /// 这里这样设置为了在控制层中   方便添加删除修改，在UI层再加一层为了引用调用
    /// 当前系统暂时一般只处理基础性的资料不包含主子表的单据情况
    /// 三种缓存的操作方法，功能独立开来不要混合调用
    /// 
    /// 2023-10-25 相对于基础资料的缓存 应该只保留 CacheEntityList，单个实体可以从缓存列表中再找，删除也可以，更新也同理
    /// 暂时不去掉CacheEntity单个实体的代码
    /// 2024-11-6  经常一系列优化:缓存 由服务器和客户端共同维护，服务器缓存基础数据，客户端缓存基础数据，客户端缓存业务数据
    /// 上传分发等。使用了json 序列化，反序列化，内存占用小，传输速度快。
    /// List<JObject> 通过 AI暂时得到这个类型性能一般。相对各种反射要好一些。
    /// </summary>
    public class MyCacheManager
    {

        private ICacheManager<object> _cache;

        /// <summary>
        /// 目前是一个特殊的key对应的一个特殊的值
        /// 保存了缓存列表的信息概览 by watson 2024-11-22
        /// </summary>
        public ICacheManager<object> CacheInfoList { get => _cache; set => _cache = value; }


        private ICacheManager<object> _cacheEntityList;

        /// <summary>
        /// 缓存所有的基础数据的实体列表，通过表名寻找
        /// 得到实体列表，用于下拉等绑定 实际保存的是强类型，如果jobject则要转换一下
        /// 在服务器端：保存的是强类型List<Customer>  ,再传送到客户端时使用JArray再转换为List<object>
        /// 目前暂时不改格式。先优化转换。再看是否要统一。
        /// 2025-2-27 决定：所有缓存统一转换为List<object>类型
        /// </summary>
        public ICacheManager<object> CacheEntityList { get => _cacheEntityList; set => _cacheEntityList = value; }


        //  private ICacheManager<object> _cacheEntity;


        /// <summary>
        /// 缓存所有的基础数据的实体，通过表名，主键列名，和主键值的组合寻找
        /// tableName + ":" + key + ":" + KeyValue; 得到实体
        /// tb_Unit:ID:12312312为搜索的key，返回是一个实体,当然保存的也是实体
        /// </summary>
        //   public ICacheManager<object> CacheEntity { get => _cacheEntity; set => _cacheEntity = value; }



        private static MyCacheManager _manager;
        public static MyCacheManager Instance
        {
            get
            {
                if (_manager == null)
                {
                    var cache = CacheFactory.Build<object>(p => p.WithSystemRuntimeCacheHandle());
                    var cacheEntity = CacheFactory.Build<object>(p => p.WithSystemRuntimeCacheHandle());
                    var cacheEntityList = CacheFactory.Build<object>(p =>
                    p.WithSystemRuntimeCacheHandle()
                    //.WithExpiration(ExpirationMode.Absolute, TimeSpan.FromMinutes(30))

                    );
                    _manager = new MyCacheManager(cache, cacheEntity, cacheEntityList);
                }
                return _manager;
            }
        }

        private ConcurrentDictionary<string, List<KeyValuePair<string, string>>> _fkPairTableList = new ConcurrentDictionary<string, List<KeyValuePair<string, string>>>();

        private ConcurrentDictionary<string, KeyValuePair<string, string>> _newTableList = new ConcurrentDictionary<string, KeyValuePair<string, string>>();

        /// <summary>
        /// 保存要缓存的表以及对应的所有外键的字段及其对应名称字段名 外键字段在一个表中，不能同名，但是可以来自于同一个表，例如创建人，修改人
        /// 就是通过创建人（createby）的列找到关联的表，再通过表名找到对应的名称列 NewTableList
        /// string:tb_Favorite    
        /// ConcurrentDictionary<tb_product, List<KeyValuePair<createby, tb_emploxxx>>
        /// ConcurrentDictionary<tb_product, List<KeyValuePair<modifyby, tb_emploxxx>>
        /// </summary>
        public ConcurrentDictionary<string, List<KeyValuePair<string, string>>> FkPairTableList { get => _fkPairTableList; set => _fkPairTableList = value; }


        /// <summary>
        /// 保存了基础数据中的名值对，对表名为KEY
        /// </summary>
        public ConcurrentDictionary<string, KeyValuePair<string, string>> NewTableList { get => _newTableList; set => _newTableList = value; }

        //结合上面的表集合一起用。不然应该是在开始设计时NewTableList的key用Type.
        public ConcurrentDictionary<string, Type> NewTableTypeList = new ConcurrentDictionary<string, Type>();

        /// <summary>
        /// 为了把所有基础数据的表名 实际 和类型关联起来 在列表datagridview的cellFormating中显示名称时 通过关联的外键找到的基础数据的表名和值。从而得到名称
        /// 直接通过泛型参数得到Type
        /// </summary>
        // public ConcurrentDictionary<string, Type> TableTypeList { get => _TableTypeList; set => _TableTypeList = value; }


        public void SetFkColList<T>()
        {
            Type type = typeof(T);
            SetFkColList(type);
        }


        public void SetFkColList(Type type)
        {
            string tableName = type.Name;
            if (tableName.Contains("View"))
            {
                //视图暂时去掉
                return;
            }
            if (!FkPairTableList.ContainsKey(tableName))
            {
                List<KeyValuePair<string, string>> kvlist = new List<KeyValuePair<string, string>>();
                foreach (var field in type.GetProperties())
                {
                    //获取指定类型的自定义特性
                    object[] attrs = field.GetCustomAttributes(false);
                    foreach (var attr in attrs)
                    {
                        if (attr is FKRelationAttribute)
                        {
                            FKRelationAttribute fkrattr = attr as FKRelationAttribute;

                            //TODO:特殊处理：因 为fkrattr.FKTableName 如果是tb_ProdDetail 换为 视图，产品表没有缓存
                            //// SetDictDataSource<tb_ProdDetail>(k => k.ProdDetailID, v => v.SKU);//这个不能缓存 在girdsetvalue时
                            //SetDictDataSource<View_ProdDetail>(k => k.ProdDetailID.Value, v => v.CNName);
                            if (fkrattr.FKTableName == "tb_ProdDetail")
                            {
                                fkrattr.FKTableName = "View_ProdDetail";
                            }

                            KeyValuePair<string, string> kv = new KeyValuePair<string, string>(fkrattr.FK_IDColName, fkrattr.FKTableName);
                            kvlist.Add(kv);
                        }
                    }
                }
                if (kvlist.Count > 0)
                {
                    FkPairTableList.TryAdd(tableName, kvlist);
                }
            }
            else
            {
                //更新?
            }
        }


        /*
        public void AddCacheEntity<T>(object entity, Expression<Func<T, int>> expkey, Expression<Func<T, string>> expvalue)
        {
            string key = expkey.Body.ToString().Split('.')[1];
            string value = expvalue.Body.ToString().Split('.')[1];
            string tableName = expkey.Parameters[0].Type.Name;
            //只处理需要缓存的表
            if (TableList.ContainsKey(tableName))
            {
                //设置属性的值
                object xkey = typeof(T).GetProperty(key).GetValue(entity, null);
                object xValue = typeof(T).GetProperty(value).GetValue(entity, null);
                string dckey = tableName + ":" + key + ":" + xkey;
                if (!Cache.Exists(dckey))
                {
                    Cache.Add(dckey, xValue);
                }
            }
        }


        public void AddCacheEntity<T>(T entity)
        {
            if (entity == null)
            {
                return;
            }
            string tableName = typeof(T).Name;
            //只处理需要缓存的表
            if (TableList.ContainsKey(tableName))
            {
                //ID
                string key = TableList[tableName].Split(':')[0];
                //NAME 列名而已
                string value = TableList[tableName].Split(':')[1];
                //设置属性的值
                object xkey = typeof(T).GetProperty(key).GetValue(entity, null);
                object xValue = typeof(T).GetProperty(value).GetValue(entity, null);
                string dckey = tableName + ":" + key + ":" + xkey;
                if (!CacheEntity.Exists(dckey))
                {
                    CacheEntity.Add(dckey, entity);
                }
                else
                {
                    UpdateEntity<T>(entity);
                }

                //if (CacheEntityList.Exists(tableName))
                //{
                //   List<T> old= CacheEntityList.Get<T>(tableName) as List<T>;
                //    old.Add((T)entity);
                //    CacheEntityList.Update(tableName,o=> old);
                //    //CacheEntityList.TryUpdate(tableName, old, out old);
                //}

            }
        }

        public void AddCacheEntity<T>(object entity)
        {
            if (entity == null)
            {
                return;
            }
            string tableName = typeof(T).Name;
            //只处理需要缓存的表
            if (TableList.ContainsKey(tableName))
            {
                //ID
                string key = TableList[tableName].Split(':')[0];
                //NAME 列名而已
                string value = TableList[tableName].Split(':')[1];
                //设置属性的值
                object xkey = typeof(T).GetProperty(key).GetValue(entity, null);
                object xValue = typeof(T).GetProperty(value).GetValue(entity, null);
                string dckey = tableName + ":" + key + ":" + xkey;
                if (!CacheEntity.Exists(dckey))
                {
                    CacheEntity.Add(dckey, entity);
                }
                else
                {
                    UpdateEntity<T>(entity);
                }

                //if (CacheEntityList.Exists(tableName))
                //{
                //   List<T> old= CacheEntityList.Get<T>(tableName) as List<T>;
                //    old.Add((T)entity);
                //    CacheEntityList.Update(tableName,o=> old);
                //    //CacheEntityList.TryUpdate(tableName, old, out old);
                //}

            }
        }

        public void AddCacheEntity<T>(List<T> list)
        {
            if (list == null)
            {
                return;
            }
            foreach (var item in list)
            {
                AddCacheEntity<T>(item);
            }
        }
        */


        /// <summary>
        /// 更新缓存列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newlist">强类型</param>
        public void UpdateEntityList<T>(List<T> newlist, bool HasExpire = false)
        {
            //newlist是引用类型不可以对他操作，不然会体现到上现操作。例如查询
            if (newlist == null)
            {
                return;
            }
            string tableName = typeof(T).Name;
            KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
            if (NewTableList.TryGetValue(tableName, out pair))
            {
                var objectList = newlist.Cast<object>().ToList();
                #region 处理新表
                //只处理需要缓存的表  并且基础信息的列查算是一次查出来？即使筛选则  新旧合并？
                if (CacheEntityList.Exists(tableName))
                {
                    //列中的数据，已经ADD delete正常操作了。存的旧值是正常的，新的中列表list如果在旧中没有就添加。其他不管？
                    var cachelist = CacheEntityList.Get(tableName);
                    if (cachelist != null)
                    {
                        //不管是强类型的集合还是json的集合，直接替换。（如果知道哪种情况性能列好可以默认哪种。以后再优化吧。TODO:by watson)
                        CacheEntityList.Update(tableName, k => objectList);
                        AddCacheInfo(tableName, objectList.Count, HasExpire);
                        //验证过  添加时相同KEY有过期时间。更新后还有效
                        //if (HasExpire)
                        //{
                        //    //设置一个区间的随机数。保证不会同时过期。
                        //    int rand = new Random().Next(2, 3);
                        //    //一个小时过期？
                        //    CacheEntityList.Expire(tableName, ExpirationMode.Absolute, TimeSpan.FromMinutes(rand));

                        //    //更新缓存表的信息
                        //    CacheInfo cacheInfo = new CacheInfo(tableName, newlist.Count);
                        //    cacheInfo.HasExpire = HasExpire;
                        //    cacheInfo.ExpirationTime = DateTime.Now.AddMinutes(rand);
                        //    MyCacheManager.Instance.Cache.AddOrUpdate(tableName, cacheInfo, c => cacheInfo);
                        //}
                    }
                    else
                    {
                        AddCacheEntityList(tableName, objectList, HasExpire);
                    }
                }
                else
                {
                    AddCacheEntityList(tableName, objectList, HasExpire);
                }
                #endregion
            }


        }

        /// <summary>
        /// 更新缓存列表,
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="newJarryList">根据不同类型，强类型 或josn判断了;也可能是一个JObject对象 也可能是一个强类型对象集合。也可能是一个json对象集合Newtonsoft.Json.Linq.JArray</param>
        public void UpdateEntityList(string tableName, Newtonsoft.Json.Linq.JArray newJarryList)
        {
            //目前是接收来自服务器的数据。所有不会是强类型,思路是如果服务器行数大于0时，一次性清除本地的。再转换一下更新进去。
            if (newJarryList == null)
            {
                return;
            }

            //更新列表中的一个值
            KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
            if (NewTableList.TryGetValue(tableName, out pair))
            {
                #region 处理新表
                //只处理需要缓存的表  并且基础信息的列查算是一次查出来？即使筛选则  新旧合并？
                if (CacheEntityList.Exists(tableName))
                {
                    //列中的数据，已经ADD delete正常操作了。存的旧值是正常的，新的中列表list如果在旧中没有就添加。其他不管？
                    var cachelist = CacheEntityList.Get(tableName);
                    // 获取原始 List<T> 的类型参数
                    Type listType = cachelist.GetType();

                    if (TypeHelper.IsGenericList(listType))
                    {
                        // Type elementType = TypeHelper.GetFirstArgumentType(listType);
                        Type elementType = NewTableTypeList.GetValue(tableName);
                        //List<object> oldlist = (List<object>)cachelist;
                        List<object> newList = TypeHelper.ConvertJArrayToList(elementType, newJarryList);
                        if (cachelist.GetType().FullName.Contains("System.Collections.Generic.List`1[["))
                        {
                            List<object> newcachelist = new List<object>();
                            foreach (object item in (IEnumerable)cachelist)
                            {
                                newcachelist.Add(item);
                            }
                            // 合并JArray并排除重复项,因为有分页传所以不能全部替换
                            var combinedList = CombineLists(elementType, newcachelist, newList, pair.Key);
                            CacheEntityList.Update(tableName, k => combinedList);
                        }
                        else
                        {
                            #region  强类型
                            var lastlist = ((IEnumerable<dynamic>)cachelist).Select(item => Activator.CreateInstance(elementType)).ToList();

                            //如果是相同时 极有可能是 分页发送的 第一页100，第二页也100时，这时也要比较处理
                            if (newList.Count == lastlist.Count)
                            {
                                if (lastlist.Contains(newList))
                                {
                                    CacheEntityList.Update(tableName, k => newList);
                                }
                                else
                                {
                                    CacheEntityList.Update(tableName, k => newList);
                                }

                            }
                            else
                            {
                                // 合并JArray并排除重复项,因为有分页传所以不能全部替换
                                var combinedList = CombineLists(elementType, lastlist, newList, pair.Key);
                                CacheEntityList.Update(tableName, k => combinedList);
                            }


                            #endregion
                        }

                    }
                    else if (TypeHelper.IsJArrayList(listType))
                    {
                        #region  jsonlist
                        JArray oldlist = (JArray)cachelist;
                        //if (newJarryList.Count > 0 && oldlist.Count == newJarryList.Count)
                        //{
                        //    CacheEntityList.Update(tableName, k => newJarryList);
                        //}
                        //else
                        //{
                        // 合并JArray并排除重复项,因为有分页传所以不能全部替换
                        var combinedList = CombineJArrays(oldlist, newJarryList, pair.Key);
                        CacheEntityList.Update(tableName, k => combinedList);

                        //但是其中一行的一个字段变化了又如何。单行有更新。

                        // }
                        #endregion
                    }
                }
                else
                {
                    AddCacheEntityList(tableName, newJarryList);
                }
                #endregion
            }
            else
            {
                AddCacheEntityList(tableName, newJarryList);
            }
        }


        /// <summary>
        /// 按主键合并后排除重复，后面是不是可以按时间等优先级来处理
        /// </summary>
        /// <param name="elementType"></param>
        /// <param name="cacheList">缓存中本身存在的集合</param>
        /// <param name="newList">收到的集合</param>
        /// <param name="key"></param>
        /// <returns></returns>
        private List<object> CombineLists(Type elementType, List<object> cacheList, List<object> newList, string key)
        {
            var combinedList = cacheList
                .Concat(newList) // 合并两个列表
                .GroupBy(item => item.GetPropertyValue(key)) // 按键值分组 
                .Select(group => group.First()) // 从每个分组中选择第一个元素
                .Where(c => c.GetPropertyValue(key).IsNotEmptyOrNull() && c.GetPropertyValue(key).ToString() != "0") // 过滤条件
                .ToList(); // 转换为列表

            // combinedList = cacheList
            //.Concat(newList)
            //.ToLookup(item => item.GetPropertyValue(key)) // 按键值分组,会保留其它元素
            //.Select(group => group.First()) // 从每个分组中选择第一个元素（优先保留 cacheList 中的元素）
            //.Where(c => c.GetPropertyValue(key).IsNotEmptyOrNull() && c.GetPropertyValue(key).ToString() != "0") // 过滤条件
            //.ToList(); // 转换为列表

            //ToLookup 适合需要快速查找分组的场景，尤其是在需要多次查找时性能更好。
            //GroupBy 适合处理大型集合，因为它使用延迟执行，可以节省内存和提高性能。

            return combinedList;
        }



        /*
         代码解释
        ToLookup：
ToLookup 将集合中的元素按指定的键值分组，并返回一个 ILookup<TKey, TElement> 对象。
这样可以确保每个键值对应的分组中包含所有相关的元素。
Concat：
连接：
将 cacheList 和 newList 合并为一个列表。
GroupBy：
分组：
按照 item.GetPropertyValue(key) 的值对合并后的列表进行分组。这里假设 GetPropertyValue 是一个扩展方法，用于获取对象的属性值。
Select：
选择：
从每个分组中选择第一个元素。这会确保每个分组中只有一个元素被选中，从而排除重复项。
Where：  其中：
过滤掉 key 值为空或为 "0" 的元素。这里假设 IsNotEmptyOrNull 是一个扩展方法，用于检查字符串是否为空或 null。
ToList：
收件人列表：
将结果转换为列表。
         */

        private JArray CombineJArrays(JArray cacheArray, JArray newArray, string key)
        {
            var combinedArray = cacheArray
                .Concat(newArray)
                .GroupBy(token => token[key])
                .Select(group => group.First())
                .ToArray();

            return new JArray(combinedArray);
        }




        /// <summary>
        /// 更新缓存列表,
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="newJObj">根据不同类型，强类型 或josn判断了;也可能是一个JObject对象 也可能是一个强类型对象集合。也可能是一个json对象集合Newtonsoft.Json.Linq.JArray</param>
        public void UpdateEntityList(string tableName, JObject newJObj)
        {
            if (newJObj == null)
            {
                return;
            }

            //更新列表中的一个值
            KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
            if (NewTableList.TryGetValue(tableName, out pair))
            {
                #region 处理新表
                //只处理需要缓存的表  并且基础信息的列查算是一次查出来？即使筛选则  新旧合并？
                if (CacheEntityList.Exists(tableName))
                {
                    //列中的数据，已经ADD delete正常操作了。存的旧值是正常的，新的中列表list如果在旧中没有就添加。其他不管？
                    var cachelist = CacheEntityList.Get(tableName);
                    // 获取原始 List<T> 的类型参数
                    Type listType = cachelist.GetType();
                    if (TypeHelper.IsGenericList(listType))
                    {
                        //Type elementType = TypeHelper.GetFirstArgumentType(listType);
                        Type elementType = NewTableTypeList.GetValue(tableName);

                        #region  强类型
                        // 创建一个新的 List<object>
                        List<object> oldlist = new List<object>();
                        // 遍历原始列表并转换元素
                        foreach (object item in (IEnumerable)cachelist)
                        {
                            //或直接在这里取。取到返回也可以
                            oldlist.Add(item);
                        }
                        var newTObj = newJObj.ToObject(elementType);
                        // 获取DepartmentID属性的值
                        //                        var Newid = newTObj.GetPropertyValue(pair.Key).ToString();
                        var Newid = newJObj[pair.Key]?.ToString();
                        var itemToUpdate = oldlist.FirstOrDefault(n => n.GetPropertyValue(pair.Key).ToString() == Newid);
                        if (itemToUpdate != null)
                        {
                            // 使用反射更新属性
                            //这里也像下面一样。直接删除添加新的？
                            //    然后要所有的缓存都看一下是不是都是List<JObject> 的类型
                            //    存储和使用的时候都统一一下？
                            //foreach (var property in newObj)
                            //{
                            //    var propInfo = elementType.GetProperty(property.Key);
                            //    if (propInfo != null && propInfo.CanWrite)
                            //    {
                            //        propInfo.SetValue(itemToUpdate, property.Value.ToObject(propInfo.PropertyType));
                            //    }
                            //}
                            oldlist.Remove(itemToUpdate);
                        }

                        // 如果在旧列表中没有找到，添加新对象
                        oldlist.Add(newTObj);

                        CacheEntityList.Update(tableName, k => oldlist);
                        #endregion
                    }
                    else if (TypeHelper.IsJArrayList(listType))
                    {
                        #region  jsonlist
                        JArray varJarray = (JArray)cachelist;
                        var Newid = newJObj[pair.Key]?.ToString();
                        //如果旧列表中有这个值，则直接删除，把新的添加上
                        var olditem = varJarray.FirstOrDefault(n => n[pair.Key].ToString() == Newid); ;
                        if (olditem != null)
                        {
                            varJarray.Remove(olditem);
                        }
                        varJarray.Add(newJObj);
                        CacheEntityList.Update(tableName, v => varJarray);
                        #endregion
                    }
                }
                else
                {
                    AddCacheEntityList(tableName, newJObj);
                }
                #endregion
            }
            else
            {
                AddCacheEntityList(tableName, newJObj);
            }
        }
        /// <summary>
        /// 更新列表中的一个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void UpdateEntityList<T>(T entity)
        {
            string tableName = typeof(T).Name;
            KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
            if (NewTableList.TryGetValue(tableName, out pair))
            {
                if (CacheEntityList.Exists(tableName))
                {
                    string key = pair.Key;
                    string Newid = entity.GetPropertyValue<T>(key).ToString();
                    var cachelist = CacheEntityList.Get(tableName);
                    if (cachelist != null)
                    {
                        // 获取原始 List<T> 的类型参数
                        Type listType = cachelist.GetType();
                        if (TypeHelper.IsGenericList(listType))
                        {
                            //Type elementType = TypeHelper.GetFirstArgumentType(listType);
                            Type elementType = NewTableTypeList.GetValue(tableName);
                            #region  强类型
                            // 创建一个新的 List<object>
                            List<object> oldlist = new List<object>();
                            // 遍历原始列表并转换元素
                            foreach (object item in (IEnumerable)cachelist)
                            {
                                //或直接在这里取。取到返回也可以
                                oldlist.Add(item);
                            }
                            //如果旧列表中有这个值，则直接删除，把新的添加上
                            var olditem = oldlist.FirstOrDefault(n => n.GetPropertyValue(pair.Key).ToString() == Newid);
                            if (olditem != null)
                            {
                                oldlist.Remove(olditem);
                            }
                            oldlist.Add(entity);
                            CacheEntityList.Update(tableName, v => oldlist);
                            #endregion
                        }
                        else if (TypeHelper.IsJArrayList(listType))
                        {
                            #region  非强类型
                            JObject jObject = JObject.FromObject(entity);
                            //var Newid = jObject[pair.Key]?.ToString();
                            JArray varJarray = (JArray)cachelist;
                            //如果旧列表中有这个值，则直接删除，把新的添加上
                            var olditem = varJarray.FirstOrDefault(n => n[key].ToString() == Newid); ;
                            if (olditem != null)
                            {
                                varJarray.Remove(olditem);
                            }
                            varJarray.Add(jObject);
                            CacheEntityList.Update(tableName, v => varJarray);
                            #endregion
                        }
                    }
                    else
                    {
                        List<T> clist = new List<T>();
                        clist.Add((T)entity);
                        CacheEntityList.Add(tableName, clist as List<object>);
                    }
                }
                else
                {
                    List<T> clist = new List<T>();
                    clist.Add((T)entity);
                    CacheEntityList.Add(tableName, clist as List<object>);
                }
            }

        }


        /// <summary>
        /// 更新缓存列表中对应表的一个实体
        /// </summary>
        /// <param name="type"></param>
        /// <param name="newlist">jsonlist</param>
        public void UpdateEntityList(Type type, List<object> newlist)
        {
            //newlist是引用类型不可以对他操作，不然会体现到上现操作。例如查询
            if (newlist == null)
            {
                return;
            }
            string tableName = type.Name;
            UpdateEntityList(tableName, newlist);
        }

        /// <summary>
        /// 更新缓存列表中对应表的一个列表集合
        /// 没有使用不知道有不有问题
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="newlist">很可能是JSONLIST</param>
        public void UpdateEntityList(string tableName, List<object> newlist)
        {
            //newlist是引用类型不可以对他操作，不然会体现到上现操作。例如查询
            if (newlist == null)
            {
                return;
            }
            KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
            if (NewTableList.TryGetValue(tableName, out pair))
            {
                #region 处理新表
                //只处理需要缓存的表  并且基础信息的列查算是一次查出来？即使筛选则  新旧合并？
                if (CacheEntityList.Exists(tableName))
                {
                    //列中的数据，已经ADD delete正常操作了。存的旧值是正常的，新的中列表list如果在旧中没有就添加。其他不管？
                    List<object> oldlist = CacheEntityList.Get(tableName) as List<object>;
                    if (oldlist == null)
                    {
                        oldlist = new List<object>();
                    }
                    object obj = null;
                    Type elementType = NewTableTypeList.GetValue(tableName);
                    //100个100个传过来时要合并
                    // 合并JArray并排除重复项,因为有分页传所以不能全部替换
                    var combinedList = CombineLists(elementType, oldlist, newlist, pair.Key);
                    CacheEntityList.Update(tableName, k => combinedList);

                    //CacheEntityList.TryUpdate(tableName, k => newlist, out obj);
                }
                else
                {
                    CacheEntityList.Add(tableName, newlist);
                }
                #endregion
            }

        }




        public void AddCacheEntityList(string tableName, object objList)
        {
            if (objList == null)
            {
                return;
            }
            //只处理需要缓存的表
            if (!CacheEntityList.Exists(tableName))
            {
                CacheEntityList.Add(tableName, objList);
            }
        }



        public void AddCacheEntityList<T>(string tableName, List<T> objList)
        {
            if (objList == null)
            {
                return;
            }
            //只处理需要缓存的表
            if (!CacheEntityList.Exists(tableName))
            {
                var objectList = objList.Cast<object>().ToList();
                CacheEntityList.Add(tableName, objectList);
            }
        }

        public void AddCacheEntityList<T>(string tableName, List<T> objList, bool HasExpire = false)
        {
            if (objList == null)
            {
                return;
            }
            //只处理需要缓存的表
            if (!CacheEntityList.Exists(tableName))
            {
                var objectList = objList.Cast<object>().ToList();
                CacheEntityList.Add(tableName, objectList);
                AddCacheInfo(tableName, objectList.Count, HasExpire);
            }
        }


        private void AddCacheInfo(string tableName, int count, bool HasExpire = false)
        {
            CacheInfo lastCacheInfo = new CacheInfo(tableName, count);
            lastCacheInfo.HasExpire = HasExpire;
            MyCacheManager.Instance.CacheInfoList.AddOrUpdate(tableName, lastCacheInfo, c => lastCacheInfo);

            if (HasExpire)
            {
                //设置一个区间的随机数。保证不会同时过期。
                int rand = new Random().Next(60, 120);
                //一个小时过期？
                CacheEntityList.Expire(tableName, ExpirationMode.Absolute, TimeSpan.FromMinutes(rand));
                lastCacheInfo.ExpirationTime = DateTime.Now.AddMinutes(rand);
                //更新缓存表的信息
                MyCacheManager.Instance.CacheInfoList.Update(tableName, c => lastCacheInfo);
            }
        }




        /// <summary>
        /// TODO
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void DeleteEntityList<T>(T entity)
        {
            string tableName = typeof(T).Name;
            KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
            if (NewTableList.TryGetValue(tableName, out pair))
            {
                string key = pair.Key;
                string Newid = entity.GetPropertyValue<T>(key).ToString();
                object newkey = typeof(T).GetProperty(key).GetValue(entity, null);
                if (CacheEntityList.Exists(tableName))
                {
                    var cachelist = CacheEntityList.Get(tableName);
                    // 获取原始 List<T> 的类型参数
                    Type listType = cachelist.GetType();
                    if (TypeHelper.IsGenericList(listType))
                    {
                        //Type elementType = TypeHelper.GetFirstArgumentType(listType);
                        Type elementType = NewTableTypeList.GetValue(tableName);
                        #region  强类型
                        // 创建一个新的 List<object>
                        List<object> oldlist = new List<object>();
                        // 遍历原始列表并转换元素
                        foreach (object item in (IEnumerable)cachelist)
                        {
                            //或直接在这里取。取到返回也可以
                            oldlist.Add(item);
                        }
                        //如果旧列表中有这个值，则直接删除，把新的添加上
                        var olditem = oldlist.FirstOrDefault(n => n.GetPropertyValue(pair.Key).ToString() == Newid);
                        if (olditem != null)
                        {
                            oldlist.Remove(olditem);
                        }

                        CacheEntityList.Update(tableName, v => oldlist);
                        #endregion
                    }
                    else if (TypeHelper.IsJArrayList(listType))
                    {
                        #region  非强类型
                        JObject jObject = JObject.FromObject(entity);
                        //var Newid = jObject[pair.Key]?.ToString();
                        JArray varJarray = (JArray)cachelist;
                        //如果旧列表中有这个值，则直接删除，把新的添加上
                        var olditem = varJarray.FirstOrDefault(n => n[key].ToString() == Newid); ;
                        if (olditem != null)
                        {
                            varJarray.Remove(olditem);
                        }

                        CacheEntityList.Update(tableName, v => varJarray);
                        #endregion
                    }
                }
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void DeleteEntityList<T>(long[] IDs)
        {
            foreach (int id in IDs)
            {
                DeleteEntityList<T>(id);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expkey"></param>
        /// <returns></returns>
        public void DeleteEntityList<T>(long ID)
        {
            string tableName = typeof(T).Name;
            KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
            if (NewTableList.TryGetValue(tableName, out pair))
            {
                string key = pair.Key;

                if (CacheEntityList.Exists(tableName))
                {
                    var cachelist = CacheEntityList.Get(tableName);
                    // 获取原始 List<T> 的类型参数
                    Type listType = cachelist.GetType();
                    if (TypeHelper.IsGenericList(listType))
                    {
                        //Type elementType = TypeHelper.GetFirstArgumentType(listType);
                        Type elementType = NewTableTypeList.GetValue(tableName);
                        #region  强类型
                        // 创建一个新的 List<object>
                        List<object> oldlist = new List<object>();
                        // 遍历原始列表并转换元素
                        foreach (object item in (IEnumerable)cachelist)
                        {
                            //或直接在这里取。取到返回也可以
                            oldlist.Add(item);
                        }
                        //如果旧列表中有这个值，则直接删除，把新的添加上
                        var olditem = oldlist.FirstOrDefault(n => n.GetPropertyValue(pair.Key).ToString() == ID.ToString());
                        if (olditem != null)
                        {
                            oldlist.Remove(olditem);
                        }

                        CacheEntityList.Update(tableName, v => oldlist);
                        #endregion
                    }
                    else if (TypeHelper.IsJArrayList(listType))
                    {
                        #region  非强类型
                        JArray varJarray = (JArray)cachelist;
                        //如果旧列表中有这个值，则直接删除，把新的添加上
                        var olditem = varJarray.FirstOrDefault(n => n[key].ToString() == ID.ToString()); ;
                        if (olditem != null)
                        {
                            varJarray.Remove(olditem);
                        }

                        CacheEntityList.Update(tableName, v => varJarray);
                        #endregion
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expkey"></param>
        /// <returns></returns>
        public void DeleteEntityList(string tableName, string PKColName, long ID)
        {
            KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
            if (NewTableList.TryGetValue(tableName, out pair))
            {
                string key = pair.Key;
                if (key != PKColName)
                {
                    return;
                }
                if (CacheEntityList.Exists(tableName))
                {
                    var cachelist = CacheEntityList.Get(tableName);
                    // 获取原始 List<T> 的类型参数
                    Type listType = cachelist.GetType();
                    if (TypeHelper.IsGenericList(listType))
                    {
                        //Type elementType = TypeHelper.GetFirstArgumentType(listType);
                        Type elementType = NewTableTypeList.GetValue(tableName);
                        #region  强类型
                        // 创建一个新的 List<object>
                        List<object> oldlist = new List<object>();
                        // 遍历原始列表并转换元素
                        foreach (object item in (IEnumerable)cachelist)
                        {
                            //或直接在这里取。取到返回也可以
                            oldlist.Add(item);
                        }
                        //如果旧列表中有这个值，则直接删除，把新的添加上
                        var olditem = oldlist.FirstOrDefault(n => n.GetPropertyValue(pair.Key).ToString() == ID.ToString());
                        if (olditem != null)
                        {
                            oldlist.Remove(olditem);
                        }

                        CacheEntityList.Update(tableName, v => oldlist);
                        #endregion
                    }
                    else if (TypeHelper.IsJArrayList(listType))
                    {
                        #region  非强类型
                        JArray varJarray = (JArray)cachelist;
                        //如果旧列表中有这个值，则直接删除，把新的添加上
                        var olditem = varJarray.FirstOrDefault(n => n[key].ToString() == ID.ToString()); ;
                        if (olditem != null)
                        {
                            varJarray.Remove(olditem);
                        }

                        CacheEntityList.Update(tableName, v => varJarray);
                        #endregion
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cache">一般缓存</param>
        /// <param name="cacheEntity">实体缓存</param>
        /// <param name="cacheEntitylist">列表缓存</param>
        public MyCacheManager(ICacheManager<object> cache, ICacheManager<object> cacheEntity, ICacheManager<object> cacheEntitylist)
        {
            CacheInfoList = cache ?? throw new ArgumentNullException(nameof(cache));
            // _cacheEntity = cacheEntity ?? throw new ArgumentNullException(nameof(cacheEntity));
            _cacheEntityList = cacheEntitylist ?? throw new ArgumentNullException(nameof(cacheEntitylist));

            _manager = this;
        }

        private static void MostSimpleCacheManager()
        {
            var config = new ConfigurationBuilder()
                .WithSystemRuntimeCacheHandle()
                .Build();

            var cache = new BaseCacheManager<string>(config);
            // or
            var cache2 = CacheFactory.FromConfiguration<string>(config);
        }

        private static void MostSimpleCacheManagerB()
        {
            var cache = new BaseCacheManager<string>(
                new CacheManagerConfiguration()
                    .Builder
                    .WithSystemRuntimeCacheHandle()
                    .Build());
        }

        private static void MostSimpleCacheManagerC()
        {
            var cache = CacheFactory.Build<string>(
                p => p.WithSystemRuntimeCacheHandle());

        }

    }

}
