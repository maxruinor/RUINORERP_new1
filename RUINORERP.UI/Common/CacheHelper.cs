using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using RUINORERP.Business;
using RUINORERP.Model;
using RUINORERP.Business.CommService;
using RUINORERP.Extensions.Middlewares;
using CacheManager.Core;
using CacheManager.SystemRuntimeCaching;
using RUINORERP.Common.Log4Net;
using RUINORERP.Common.Extensions;
using Microsoft.Extensions.Logging;
using System.Reflection;


namespace RUINORERP.UI.Common
{
    /// <summary>
    /// 缓存类 缓存了所有基础数据
    /// 是不是应该分类 。缓存的对象，通过对象的值来取值
    /// </summary>
    public class CacheHelper
    {
        public CacheHelper()
        {

        }

        private static CacheHelper _helper;
        public static CacheHelper Instance
        {
            get
            {
                if (_helper == null)
                {
                    _helper = new CacheHelper();
                    if (_manager == null)
                    {
                        //因为是静态，系统启动就调用这里。所以这里的才是最会执行的代码
                        var cache = CacheManager.Core.CacheFactory.Build<object>(p => p.WithMicrosoftLogging(log =>
                        {
                            // log.ClearProviders();
                            //f.AddConsole(LogLevel.Information);
                            //f.AddDebug(LogLevel.Verbose);
                            //log4net.config 配置文件中最后面 定的级别
                            Log4NetProvider log4NetProvider = new Log4NetProvider("Log4net_file.config");
                            log.AddProvider(log4NetProvider);
                        })
                        .WithSystemRuntimeCacheHandle()
                        .WithExpiration(ExpirationMode.None, TimeSpan.FromSeconds(120))
                        );

                        //  var cacheA = CacheFactory.FromConfiguration<object>("myCache");
                        //因为系统启动就调用这里。所以这里的才是最会执行的代码
                        var cacheEntity = CacheManager.Core.CacheFactory.Build<object>(p => p.WithMicrosoftLogging(log =>
                        {
                            // log.ClearProviders();
                            //f.AddConsole(LogLevel.Information);
                            //f.AddDebug(LogLevel.Verbose);
                            //log4net.config 配置文件中最后面 定的级别
                            Log4NetProvider log4NetProvider = new Log4NetProvider("Log4net_file.config");
                            log.AddProvider(log4NetProvider);
                        })
                        .WithSystemRuntimeCacheHandle()
                        .WithExpiration(ExpirationMode.None, TimeSpan.FromSeconds(120))
                        );


                        //因为系统启动就调用这里。所以这里的才是最会执行的代码
                        var cacheEntityList = CacheManager.Core.CacheFactory.Build<object>(p => p.WithMicrosoftLogging(log =>
                        {
                            // log.ClearProviders();
                            //f.AddConsole(LogLevel.Information);
                            //f.AddDebug(LogLevel.Verbose);
                            //log4net.config 配置文件中最后面 定的级别
                            Log4NetProvider log4NetProvider = new Log4NetProvider("Log4net_file.config");
                            log.AddProvider(log4NetProvider);
                        })
                        .WithSystemRuntimeCacheHandle()
                        .WithExpiration(ExpirationMode.None, TimeSpan.FromSeconds(120))
                        );


                        _manager = new MyCacheManager(cache, cacheEntity, cacheEntityList);
                    }
                }
                return _helper;
            }
        }

        private static MyCacheManager _manager;

        //private ConcurrentDictionary<string, object> _Dict = new ConcurrentDictionary<string, object>();

        //public ConcurrentDictionary<string, object> Dict { get => _Dict; set => _Dict = value; }
        public static MyCacheManager Manager { get => _manager; set => _manager = value; }



        public T GetEntity<T>(object IdValue)
        {
            object entity = new object();
            //Lazy<>
            if (IdValue == null)
            {
                return default(T);
            }
            string tableName = typeof(T).Name;
            //只处理需要缓存的表
            KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
            if (Manager.NewTableList.TryGetValue(tableName, out pair))
            {
                string key = pair.Key;
                string KeyValue = IdValue.ToString();
                //设置属性的值
                if (Manager.CacheEntityList.Exists(tableName))
                {
                    List<T> list = Manager.CacheEntityList.Get(tableName) as List<T>;
                    entity = list.Find(t => t.GetPropertyValue(key).ToString() == IdValue.ToString());
                    if (entity != null)
                    {
                        return (T)entity;
                    }
                }
            }
            return (T)entity;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public object GetEntity(string tableName, object PrimaryKeyValue)
        {
            object entity = new object();
            //只处理需要缓存的表
            KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
            if (Manager.NewTableList.TryGetValue(tableName, out pair))
            {
                string key = pair.Key;
                string KeyValue = PrimaryKeyValue.ToString();

                if (Manager.CacheEntityList.Exists(tableName))
                {
                    var cachelist = Manager.CacheEntityList.Get(tableName);

                    // 获取原始 List<T> 的类型参数
                    Type listType = cachelist.GetType();
                    Type elementType = listType.GetGenericArguments()[0];


                    // 创建一个新的 List<object>
                    List<object> convertedList = new List<object>();

                    // 遍历原始列表并转换元素
                    foreach (var item in (IEnumerable)cachelist)
                    {
                        //或直接在这里取。取到返回也可以
                        convertedList.Add(item);
                    }

                    entity = convertedList.Find(t => t.GetPropertyValue(key).ToString() == PrimaryKeyValue.ToString());
                    if (entity != null)
                    {
                        return entity;
                    }
                }
            }
            return entity;
        }




        /// <summary>
        /// 通过表和主键名去找，int为主键类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expkey"></param>
        /// <returns></returns>
        public object GetValue<T>(object IdValue)
        {
            object entity = new object();
            //Lazy<>
            if (IdValue == null)
            {
                return default(T);
            }
            string tableName = typeof(T).Name;

            //  (T)Convert.ChangeType(value, typeof(T))


            //只处理需要缓存的表
            KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
            if (Manager.NewTableList.TryGetValue(tableName, out pair))
            {
                string key = pair.Key;
                string KeyValue = IdValue.ToString();
                //设置属性的值
                if (Manager.CacheEntityList.Exists(tableName))
                {
                    List<T> list = Manager.CacheEntityList.Get(tableName) as List<T>;
                    if (list != null)
                    {
                        var obj = list.Find(t => t.GetPropertyValue(key).ToString() == IdValue.ToString());
                        if (obj != null)
                        {
                            entity = obj.GetPropertyValue(pair.Value);
                        }
                    }

                }
            }
            return entity;
        }

        private bool IsGenericList(Type type)
        {
            return (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>)));
        }

        private Type GetFirstArgumentType(Type type)
        {
            return type.GetGenericArguments()[0];
        }

        private List<U> ConvertListToProxy<T, U>(List<T> source, U target)
        {
            var lst = source.Select(x => PropertyCopier.CopyFrom(x, target)).ToList();
            return lst;
        }

        /// <summary>
        /// 通过表和主键名去找，int为主键类型
        /// 重点重要学习代码
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expkey"></param>
        /// <returns></returns>
        public object GetValue(string tableName, object IdValue)
        {
            object entity = new object();
            //只处理需要缓存的表
            KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
            if (Manager.NewTableList.TryGetValue(tableName, out pair))
            {
                string key = pair.Key;
                string KeyValue = IdValue.ToString();
                //设置属性的值
                if (Manager.CacheEntityList.Exists(tableName))
                {
                    //Type listType = typeof(List<>);
                    var rslist = Manager.CacheEntityList.Get(tableName);
                    if (IsGenericList(rslist.GetType()))
                    {
                        //var firstArgumentType = GetFirstArgumentType(rslist.GetType());
                        //listType.MakeGenericType(GetFirstArgumentType(rslist.GetType()));
                        //将List<T>类型的结果是object的转换为指定类型的List 学习 重要 TODO 重点学习代码
                        //var lastlist = ((IEnumerable<dynamic>)rslist).Select(item => Activator.CreateInstance(mytype)).ToList();
                        var lastlist = ((IEnumerable<dynamic>)rslist).ToList();
                        if (lastlist != null)
                        {
                            foreach (var item in lastlist)
                            {
                                var id = RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(item, key);
                                if (id != null)
                                {
                                    if (id.ToString() == IdValue.ToString())
                                    {
                                        entity = RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(item, pair.Value);
                                        break;
                                    }
                                }

                            }
                            //var obj = lastlist.Find(t => t.GetPropertyValue(key) == IdValue);
                            //if (obj != null)
                            // {
                            // entity = obj.GetPropertyValue(pair.Value);
                            //}
                        }

                    }
                }
            }
            return entity;
        }

        /*
        public object GetValue(string tableName, object IdValue)
        {
            object entity = new object();
            //Lazy<>
            if (IdValue == null)
            {
                return null;
            }
            //只处理需要缓存的表
            KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
            if (Manager.NewTableList.TryGetValue(tableName, out pair))
            {
                string key = pair.Key;
                string KeyValue = IdValue.ToString();
                //设置属性的值
                if (Manager.CacheEntityList.Exists(tableName))
                {
                    List<object> list = Manager.CacheEntityList.Get(tableName) as List<object>;
                    var obj = list.Find(t => t.GetPropertyValue(key) == IdValue);
                    if (obj != null)
                    {
                        entity = obj.GetPropertyValue(pair.Value);
                    }
                }
            }
            return entity;
        }
        */
        /// <summary>
        /// 通过表和主键名去找，int为主键类型
        /// 在UI层修改值，实际会在控制层来总体控制，要么生成。要么注入AOP来判断
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expkey"></param>
        /// <returns></returns>
        public void CheckValue<T>(Expression<Func<T, int>> expkey, object value)
        {

            var mb = expkey.GetMemberInfo();
            string key = mb.Name;
            string tableName = expkey.Parameters[0].Type.Name;
            key = tableName + ":" + key + ":" + value.ToString();
            //if (!Dict.ContainsKey(key))
            //{
            //    Dict.TryAdd(key, value);
            //}
            //else
            //{
            //    Dict[key] = value;
            //}

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expkey"></param>
        /// <returns></returns>
        public void DeleteEntity<T>(T value)
        {
            Manager.DeleteEntityList<T>(value);
        }



        /// <summary>
        /// 保存，并且初始化值
        /// </summary>
        /// <typeparam name="T">键值对的表实体</typeparam>
        /// <param name="expkey">ID</param>
        /// <param name="expvalue">Name</param>
        public void SetDictDataSource<T>(Expression<Func<T, long>> expkey, Expression<Func<T, string>> expvalue) where T : class
        {
            var mb = expkey.GetMemberInfo();
            string idColName = mb.Name;
            var mbv = expvalue.GetMemberInfo();
            string nameColName = mbv.Name;
            string tableName = typeof(T).Name;
            try
            {

                if (!Manager.NewTableList.ContainsKey(tableName))
                {
                    Manager.NewTableList.TryAdd(tableName, new KeyValuePair<string, string>(idColName, nameColName));
                }
                //if (!Manager.TableTypeList.ContainsKey(tableName))
                //{
                //    Manager.TableTypeList.TryAdd(tableName, typeof(T));
                //}

                //第一次先查询一次载入
                CommonController bdc = Startup.GetFromFac<CommonController>();
                //BaseController<T> bdc = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
                var list = bdc.GetBindSource<T>(tableName);
                //设置绑定数据源
                //只处理需要缓存的表
                if (Manager.NewTableList.ContainsKey(tableName))
                {
                    //设置属性的值
                    if (!Manager.CacheEntityList.Exists(tableName))
                    {
                        Manager.UpdateEntityList<T>(list);
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError("SetDictDataSource:" + tableName + "===>" + ex.Message);
            }
        }


        /*
        /// <summary>
        /// 保存，并且初始化值
        /// </summary>
        /// <typeparam name="T">键值对的表实体</typeparam>
        /// <param name="expkey">ID</param>
        /// <param name="expvalue">Name</param>
        public void SetDictDataSource(Type type, string keyColName, string nameColName)
        {
            string tableName = type.Name;
            try
            {

                if (!Manager.NewTableList.ContainsKey(tableName))
                {
                    Manager.NewTableList.TryAdd(tableName, new KeyValuePair<string, string>(keyColName, nameColName));
                }

                //// 要创建的实体类名
                //string className = "YourEntityClassName";

                //// 使用 Assembly.Load 加载包含实体类的程序集
                //Assembly assembly = Assembly.Load("YourAssemblyName");

                //// 使用 Type.GetType 获取实体类的类型
                //Type entityType = assembly.GetType(className);

                //// 使用 Activator.CreateInstance 创建实体的实例
                //object entityInstance = Activator.CreateInstance(entityType);


                //第一次先查询一次载入
                CommonController bdc = Startup.GetFromFac<CommonController>();
                var list = bdc.GetBindSource<T>(tableName);

                //设置绑定数据源
                //只处理需要缓存的表
                if (Manager.NewTableList.ContainsKey(tableName))
                {
                    //设置属性的值
                    if (!Manager.CacheEntityList.Exists(tableName))
                    {
                        Manager.UpdateEntityList(type, list);
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError("SetDictDataSource:" + tableName + "===>" + ex.Message);
            }
        }
        */

        public void SetDictDataSource(List<string> typeNames)
        {
            // 遍历类型名称数组
            foreach (string typeName in typeNames)
            {
                switch (typeName)
                {
                    case "tb_Currency":
                        SetDictDataSource<tb_Currency>(k => k.Currency_ID, v => v.CurrencyName);
                        break;
                    case "tb_BOM_S":
                        SetDictDataSource<tb_BOM_S>(k => k.BOM_ID, v => v.BOM_Name);
                        break;
                    case "tb_ProductType":
                        SetDictDataSource<tb_ProductType>(k => k.Type_ID, v => v.TypeName);
                        break;
                    case "tb_PaymentMethod":
                        SetDictDataSource<tb_PaymentMethod>(k => k.Paytype_ID, v => v.Paytype_Name);
                        break;
                    case "tb_Unit":
                        SetDictDataSource<tb_Unit>(k => k.Unit_ID, v => v.UnitName);
                        break;
                    case "tb_Department":
                        SetDictDataSource<tb_Department>(k => k.DepartmentID, v => v.DepartmentName);
                        break;
                    case "tb_LocationType":
                        SetDictDataSource<tb_LocationType>(k => k.LocationType_ID, v => v.TypeName);
                        break;
                    case "tb_Location":
                        SetDictDataSource<tb_Location>(k => k.Location_ID, v => v.Name);
                        break;
                    case "tb_CustomerVendor":
                        SetDictDataSource<tb_CustomerVendor>(k => k.CustomerVendor_ID, v => v.CVName);
                        break;
                    case "tb_CustomerVendorType":
                        SetDictDataSource<tb_CustomerVendorType>(k => k.Type_ID, v => v.TypeName);
                        break;
                    case "tb_ProdCategories":
                        SetDictDataSource<tb_ProdCategories>(k => k.Category_ID, v => v.Category_name);
                        break;
                    case "tb_Prod":
                        SetDictDataSource<tb_Prod>(k => k.ProdBaseID, v => v.CNName);
                        break;
                    case "View_ProdDetail":
                        SetDictDataSource<View_ProdDetail>(k => k.ProdDetailID, v => v.CNName);
                        break;
                    case "tb_ProdPropertyType":
                        SetDictDataSource<tb_ProdPropertyType>(k => k.PropertyType_ID, v => v.PropertyTypeName);
                        break;
                    case "tb_ProdProperty":
                        SetDictDataSource<tb_ProdProperty>(k => k.Property_ID, v => v.PropertyName);
                        break;
                    case "tb_Employee":
                        SetDictDataSource<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name);
                        break;
                    case "tb_UserInfo":
                        SetDictDataSource<tb_UserInfo>(k => k.User_ID, v => v.UserName);
                        break;
                    case "tb_RoleInfo":
                        SetDictDataSource<tb_RoleInfo>(k => k.RoleID, v => v.RoleName);
                        break;
                    case "tb_MenuInfo":
                        SetDictDataSource<tb_MenuInfo>(k => k.MenuID, v => v.MenuName);
                        break;
                    case "tb_FieldInfo":
                        SetDictDataSource<tb_FieldInfo>(k => k.FieldInfo_ID, v => v.FieldName);
                        break;
                    case "tb_ModuleDefinition":
                        SetDictDataSource<tb_ModuleDefinition>(k => k.ModuleID, v => v.ModuleName);
                        break;
                    case "tb_BizType":
                        SetDictDataSource<tb_BizType>(k => k.Type_ID, v => v.TypeName);
                        break;
                    case "tb_StorageRack":
                        SetDictDataSource<tb_StorageRack>(k => k.Rack_ID, v => v.RackName);
                        break;
                    case "tb_OutInStockType":
                        SetDictDataSource<tb_OutInStockType>(k => k.Type_ID, v => v.TypeName);
                        break;
                    case "tb_OnlineStoreInfo":
                        SetDictDataSource<tb_OnlineStoreInfo>(k => k.Store_ID, v => v.StoreName);
                        break;
                    case "tb_ProjectGroup":
                        SetDictDataSource<tb_ProjectGroup>(k => k.ProjectGroup_ID, v => v.ProjectGroupName);
                        break;
                    case "tb_FM_Account":
                        SetDictDataSource<tb_FM_Account>(k => k.Account_id, v => v.Account_name);
                        break;
                    case "tb_FM_ExpenseType":
                        SetDictDataSource<tb_FM_ExpenseType>(k => k.ExpenseType_id, v => v.Expense_name);
                        break;
                    case "tb_FM_Subject":
                        SetDictDataSource<tb_FM_Subject>(k => k.subject_id, v => v.subject_name);
                        break;
                    case "tb_RolePropertyConfig":
                        SetDictDataSource<tb_RolePropertyConfig>(k => k.RolePropertyID, v => v.RolePropertyName);
                        break;
                    case "tb_CartoonBox":
                        SetDictDataSource<tb_CartoonBox>(k => k.CartonID, v => v.CartonName);
                        break;
                    case "tb_Files":
                        SetDictDataSource<tb_Files>(k => k.Doc_ID, v => v.FileName);
                        break;
                    case "tb_BOMConfigHistory":
                        SetDictDataSource<tb_BOMConfigHistory>(k => k.BOM_S_VERID, v => v.VerNo);
                        break;

                    case "tb_BoxRules":
                        SetDictDataSource<tb_BoxRules>(k => k.BoxRules_ID, v => v.BoxRuleName);
                        break;
                    case "tb_Unit_Conversion":
                        SetDictDataSource<tb_Unit_Conversion>(k => k.UnitConversion_ID, v => v.UnitConversion_Name);
                        break;
                    default:
                        // Console.WriteLine("未知类型: " + typeName);
                        MainForm.Instance.logger.LogError("SetDictDataSource 未知类型" + typeName);
                        break;
                }

            }
        }


        /// <summary>
        /// 初始化数据字典,并且提取出结果
        /// </summary>
        public void InitDict()
        {

            try
            {
                Manager.Cache.Clear();
                Manager.CacheEntityList.Clear();
                Manager.NewTableList.Clear();
                string[] typeNames = {
            "tb_Currency",
            "tb_BOM_S",
            "tb_ProductType",
            "tb_PaymentMethod",
            "tb_Unit",
            "tb_Department",
            "tb_LocationType",
            "tb_Location",
            "tb_CustomerVendor",
            "tb_CustomerVendorType",
            "tb_ProdCategories",
            "tb_Prod", 
            // "tb_ProdDetail", // 这个不能缓存，在 GridSetValue 时
            "View_ProdDetail",
            "tb_ProdPropertyType",
            "tb_ProdProperty",
            "tb_Employee",
            "tb_UserInfo",
            "tb_RoleInfo",
            "tb_MenuInfo",
            "tb_FieldInfo",
            "tb_ModuleDefinition", 
            // "tb_Files", // 要完善
            "tb_BizType",
            "tb_StorageRack",
            "tb_OutInStockType",
            "tb_OnlineStoreInfo",
            "tb_ProjectGroup",
            "tb_FM_Account",
            "tb_FM_ExpenseType",
            "tb_FM_Subject",
            "tb_BoxRules",
            "tb_CartoonBox",
            "tb_Files",
            "tb_BOMConfigHistory",
            "tb_RolePropertyConfig",
            "tb_Unit_Conversion"
        };
                SetDictDataSource(typeNames.ToList<string>());
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError("InitDict" + ex.Message);
            }
        }

    }





    public static class PropertyCopier
    {
        public static U CopyFrom<T, U>(T source, U target)
        {
            var targetObj = Activator.CreateInstance<U>();
            foreach (var targetProp in targetObj.GetType().GetProperties())
            {
                var sourceProp = source.GetType().GetProperty(targetProp.Name);
                if (sourceProp == null)
                {
                    var sourceField = source.GetType().GetField(targetProp.Name);
                    if (sourceField == null)
                    {
                        throw new Exception("there is no field or property with same name");
                    }
                    targetProp.SetValue(targetObj, sourceField.GetValue(source));
                }
                else
                {
                    targetProp.SetValue(targetObj, sourceProp.GetValue(source));
                }
            }
            return targetObj;
        }
    }

    public class TypeToT
    {
        public static List<C> GetList<C>(List<C> paralist) where C : class
        {
            List<C> list = new List<C>();

            return list;
        }

    }
}
