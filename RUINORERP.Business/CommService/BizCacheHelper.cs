using CacheManager.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using RUINORERP.Model;
using RUINORERP.Extensions.Middlewares;
using CacheManager.SystemRuntimeCaching;
using RUINORERP.Common.Log4Net;
using RUINORERP.Common.Extensions;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Diagnostics;
using RUINORERP.Business.CommService;
using RUINORERP.Model.Context;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using RUINORERP.Common.Helper;
using Fireasy.Common.Extensions;
using Castle.Core.Internal;

namespace RUINORERP.Business.CommService
{
    /// <summary>
    /// 缓存帮助类
    /// 主要思路：以表名为key:值是两种情况：1）IsGenericList ，2）Jarrry  jsonList
    /// 注意：此类已弃用，请使用MyCacheManager.Instance代替
    /// </summary>
    [Obsolete("此类已弃用，请使用MyCacheManager.Instance代替", false)]
    public class BizCacheHelper
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<BizCacheHelper> _logger;

        public BizCacheHelper(ApplicationContext context, ILogger<BizCacheHelper> logger)
        {
            _context = context;
            _logger = logger;

        }

        private static BizCacheHelper _helper;
        public static BizCacheHelper Instance
        {
            get
            {
                if (_helper == null)
                {
                    InitManager();
                }
                return _helper;
            }
            set { _helper = value; }
        }

        private static MyCacheManager _manager;

        // 修正后的InitManager方法
        public static void InitManager()
        {
            // 初始化MyCacheManager单例，使用服务器端特定的缓存配置
            // 1. 创建日志工厂（如果需要自定义日志）
            var loggerFactory = LoggerFactory.Create(logBuilder =>
            {
                // 添加Log4NetProvider（你的日志配置）
                Common.Log4Net.Log4NetProvider log4NetProvider = new Common.Log4Net.Log4NetProvider("Log4net_file.config");
                logBuilder.AddProvider(log4NetProvider);
            });

            // 2. 构建缓存管理器（使用正确的Build重载）
            var cache = CacheFactory.Build<object>(
                builder => builder
                    .WithSystemRuntimeCacheHandle() // 使用系统运行时缓存
                    .WithExpiration(ExpirationMode.None, TimeSpan.FromSeconds(120)), // 过期配置
                loggerFactory // 传入日志工厂
            );
            
            MyCacheManager.Initialize(cache);
            _manager = MyCacheManager.Instance;
        }

        //private ConcurrentDictionary<string, object> _Dict = new ConcurrentDictionary<string, object>();

        //public ConcurrentDictionary<string, object> Dict { get => _Dict; set => _Dict = value; }
        
        /// <summary>
        /// 获取缓存管理器实例
        /// 注意：已弃用，请直接使用MyCacheManager.Instance
        /// </summary>
        [Obsolete("已弃用，请直接使用MyCacheManager.Instance", false)]
        public static MyCacheManager Manager { get => _manager; private set => _manager = value; }

        /// <summary>
        /// 根据类型和ID获取实体
        /// 注意：已弃用，请使用MyCacheManager.Instance.GetEntity方法
        /// </summary>
        [Obsolete("已弃用，请使用MyCacheManager.Instance.GetEntity方法", false)]
        public T GetEntity<T>(object IdValue)
        {
            object entity = new object();
            try
            {
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
                        var cachelist = Manager.CacheEntityList.Get(tableName);
                        // 获取原始 List<T> 的类型参数
                        Type listType = cachelist.GetType();
                        if (TypeHelper.IsGenericList(listType))
                        {
                            List<object> list = Manager.CacheEntityList.Get(tableName) as List<object>;
                            if (list == null)
                            {
                                list = new List<object>();
                            }
                            //List<T> list = Manager.CacheEntityList.Get(tableName) as List<T>;
                            entity = list.Find(t => t.GetPropertyValue(key).ToString() == IdValue.ToString());
                            if (entity != null)
                            {
                                return (T)entity;
                            }
                        }
                        else if (TypeHelper.IsJArrayList(listType))
                        {
                            JArray varJarray = (JArray)cachelist;
                            JToken olditem = varJarray.FirstOrDefault(n => n[pair.Key].ToString() == IdValue.ToString());
                            if (olditem != null)
                            {
                                return olditem.ToObject<T>();
                            }
                            else
                            {
                                T prodDetail = default(T);// _context.Db.Queryable<T>().Where(p => p.GetPropertyValue(key).ToString().Equals(KeyValue)).Single();
                                return prodDetail;
                            }
                        }

                    }
                }
                if (entity == null)
                {
                    return default(T);
                }
                else if (entity.ToString() == "System.Object")
                {
                    return default(T);
                }
                else
                {
                    if (entity.GetType().Name != "Object")
                    {
                        return default(T);
                    }
                }
                return (T)entity;
            }
            catch (Exception ex)
            {
                _logger.LogError("BizCacheHelper->GetEntity:" + ex.Message + ex.StackTrace);
            }
            return default(T);
        }

        /// <summary>
        /// 根据表名和主键值获取实体
        /// 注意：已弃用，请使用MyCacheManager.Instance.GetEntity方法
        /// </summary>
        [Obsolete("已弃用，请使用MyCacheManager.Instance.GetEntity方法", false)]
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

                    if (TypeHelper.IsGenericList(listType))
                    {
                        Type elementType = listType.GetGenericArguments()[0];
                        // 创建一个新的 List<object>
                        List<object> convertedList = new List<object>();

                        // 遍历原始列表并转换元素
                        foreach (var item in (IEnumerable)cachelist)
                        {
                            //或直接在这里取。取到返回也可以
                            convertedList.Add(item);
                        }
                        entity = convertedList.FirstOrDefault(t => t.GetPropertyValue(key).ToString() == PrimaryKeyValue.ToString());
                        if (entity != null)
                        {
                            return entity;
                        }

                    }
                    else if (TypeHelper.IsJArrayList(listType))
                    {
                        #region  jsonlist
                        JArray varJarray = (JArray)cachelist;
                        //如果旧列表中有这个值，则直接删除，把新的添加上
                        var olditem = varJarray.FirstOrDefault(n => n[pair.Key].ToString() == PrimaryKeyValue.ToString());
                        //Type type = Assembly.LoadFrom(Global.GlobalConstants.ModelDLL_NAME).GetType(Global.GlobalConstants.Model_NAME + "." + tableName);
                        Type type = Manager.NewTableTypeList.GetValue(tableName);
                        if (olditem != null)
                        {
                            return olditem.ToObject(type);
                        }
                        #endregion
                    }


                }
            }
            return entity;
        }


        /// <summary>
        /// 通过表和主键名去找，int为主键类型
        /// 重点重要学习代码
        /// </summary>
        /// <typeparam name="tableName">表名</typeparam>
        /// <param name="IdValue">对应主键的值</param>
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
                    var rslist = Manager.CacheEntityList.Get(tableName);
                    if (TypeHelper.IsGenericList(rslist.GetType()))
                    {
                        //var firstArgumentType = GetFirstArgumentType(rslist.GetType());
                        //listType.MakeGenericType(GetFirstArgumentType(rslist.GetType()));
                        //将List<T>类型的结果是object的转换为指定类型的List 学习 重要 TODO 重点学习代码
                        //var lastlist = ((IEnumerable<dynamic>)rslist).Select(item => Activator.CreateInstance(mytype)).ToList();
                        var lastlist = ((IEnumerable<dynamic>)rslist).ToList();
                        if (lastlist != null)
                        {
                            //entity = lastlist.Find(t => t.GetPropertyValue(key).ToString() == IdValue.ToString());
                            //entity = lastlist.Find(t => t.GetPropertyValue(key).ToString() == IdValue.ToString());
                            //if (entity != null)
                            //{
                            //    //return (T)entity;
                            //}


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
                        }
                    }
                    else if (rslist != null && TypeHelper.IsJArrayList(rslist.GetType()))
                    {

                        var lastlist = ((IEnumerable<dynamic>)rslist).ToList();
                        if (lastlist != null)
                        {
                            foreach (var item in lastlist)
                            {
                                // 将item转换为JObject
                                var obj = JObject.Parse(item.ToString());

                                // 获取DepartmentID属性的值
                                var id = obj[key]?.ToString();

                                if (id != null && id == IdValue.ToString())
                                {
                                    // 假设你想要获取的属性是DepartmentName
                                    var departmentName = obj[pair.Value]?.ToString();
                                    if (departmentName != null)
                                    {
                                        entity = departmentName;
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


        }


        /// <summary>
        /// 保存，并且初始化值
        /// </summary>
        /// <typeparam name="T">键值对的表实体</typeparam>
        /// <param name="expkey">ID</param>
        /// <param name="expvalue">Name</param>
        public List<T> SetDictDataSource<T>(Expression<Func<T, long>> expkey, Expression<Func<T, string>> expvalue, bool LoadData = true
            , bool AutoLoad = false
            ) where T : class
        {
            List<T> lastList = null;
            string tableName = typeof(T).Name;
            Stopwatch stopwatch = Stopwatch.StartNew();
            var mb = expkey.GetMemberInfo();
            string idColName = mb.Name;
            var mbv = expvalue.GetMemberInfo();
            string nameColName = mbv.Name;
            try
            {
                if (!Manager.NewTableList.ContainsKey(tableName))
                {
                    Manager.NewTableList.TryAdd(tableName, new KeyValuePair<string, string>(idColName, nameColName));
                    Manager.NewTableTypeList.TryAdd(tableName, typeof(T));
                }
                //要么指定加载，要么指定自动加载
                if (LoadData || AutoLoad)
                {
                    //第一次先查询一次载入
                    ICommonController bdc = _context.GetRequiredService<ICommonController>();
                    var list = bdc.GetBindSource<T>(tableName);
                    //设置绑定数据源
                    //只处理需要缓存的表
                    if (Manager.NewTableList.ContainsKey(tableName))
                    {
                        //设置属性的值
                        Manager.UpdateEntityList<T>(list, true);
                    }
                    lastList = list;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("SetDictDataSource:" + tableName + "===>" + ex.Message);
            }
            stopwatch.Stop();
            //_logger.LogInformation($"初始化SetDictDataSource: {tableName} 执行时间：{stopwatch.ElapsedMilliseconds} 毫秒");
            return lastList;
        }

        // 添加GetDictDataSource方法
        public ConcurrentDictionary<object, object> GetDictDataSource(string tableName)
        {
            if (Manager.CacheEntityList.Exists(tableName))
            {
                var cacheData = Manager.CacheEntityList.Get(tableName);
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

        public void SetDictDataSource(List<string> typeNames, bool LoadData = true)
        {
            // 遍历类型名称数组
            foreach (string typeName in typeNames)
            {
                if (typeName.IsNullOrEmpty())
                {
                    continue;
                }
                SetDictDataSource(typeName, LoadData);

            }
        }


        /// <summary>
        /// 缓存列表
        /// </summary>
        public string[] typeNames = {
            "tb_Company",
            "tb_Currency",
            "tb_CurrencyExchangeRate",
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
            "View_ProdInfo",
            "tb_ProdPropertyType",
            "tb_ProdProperty",
            "tb_ProdPropertyValue",
            "tb_Employee",
            "tb_UserInfo",
            "tb_RoleInfo",
            "tb_MenuInfo",
            //"tb_FieldInfo",
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
            "tb_FM_PayeeInfo",
            "tb_Files",
            "tb_CRM_Customer",
            //"tb_CRM_FollowUpPlans",
             "tb_CRM_Leads",
             "tb_CRM_Region",
             "tb_CRM_Contact",
             "tb_Provinces",
             "tb_Cities",
             "tb_Packing",
             "tb_ProdBundle",
            "tb_BOMConfigHistory",
            "tb_RolePropertyConfig",
            "tb_Unit_Conversion"
        };


        //.Select
        public void SetDictDataSource(string typeName, bool LoadData = true)
        {
            // 遍历类型名称数组
            switch (typeName)
            {
                case "tb_ProdBundle":
                    SetDictDataSource<tb_ProdBundle>(k => k.BundleID, v => v.BundleName, LoadData);
                    break;
                case "tb_Packing":
                    SetDictDataSource<tb_Packing>(k => k.Pack_ID, v => v.PackagingName, LoadData);
                    break;

                case "tb_CRM_Customer":
                    SetDictDataSource<tb_CRM_Customer>(k => k.Customer_id, v => v.CustomerName, LoadData);
                    break;
                //case "tb_CRM_FollowUpPlans":
                //    SetDictDataSource<tb_CRM_FollowUpPlans>(k => k.PlanID, v => v.PlanSubject, LoadData);
                //    break;
                case "tb_CRM_Leads":
                    SetDictDataSource<tb_CRM_Leads>(k => k.LeadID, v => v.CustomerName, LoadData);
                    break;
                case "tb_CRM_Region":
                    SetDictDataSource<tb_CRM_Region>(k => k.Region_ID, v => v.Region_Name, LoadData);
                    break;
                case "tb_CRM_Contact":
                    SetDictDataSource<tb_CRM_Contact>(k => k.Contact_id, v => v.Contact_Name, LoadData);
                    break;
                case "tb_Provinces":
                    SetDictDataSource<tb_Provinces>(k => k.ProvinceID, v => v.ProvinceCNName, LoadData);
                    break;
                case "tb_Cities":
                    SetDictDataSource<tb_Cities>(k => k.CityID, v => v.CityCNName, LoadData);
                    break;
                case "tb_Company":
                    SetDictDataSource<tb_Company>(k => k.ID, v => v.CNName, LoadData);
                    break;
                case "tb_Currency":
                    SetDictDataSource<tb_Currency>(k => k.Currency_ID, v => v.CurrencyName, LoadData);
                    break;
                case "tb_CurrencyExchangeRate":
                    SetDictDataSource<tb_CurrencyExchangeRate>(k => k.ExchangeRateID, v => v.ConversionName, LoadData);
                    break;
                case "tb_BOM_S":
                    SetDictDataSource<tb_BOM_S>(k => k.BOM_ID, v => v.BOM_Name, LoadData);
                    break;
                case "tb_ProductType":
                    SetDictDataSource<tb_ProductType>(k => k.Type_ID, v => v.TypeName, LoadData);
                    break;
                case "tb_PaymentMethod":
                    SetDictDataSource<tb_PaymentMethod>(k => k.Paytype_ID, v => v.Paytype_Name, LoadData);
                    break;
                case "tb_Unit":
                    SetDictDataSource<tb_Unit>(k => k.Unit_ID, v => v.UnitName, LoadData);
                    break;
                case "tb_Department":
                    SetDictDataSource<tb_Department>(k => k.DepartmentID, v => v.DepartmentName, LoadData);
                    break;
                case "tb_LocationType":
                    SetDictDataSource<tb_LocationType>(k => k.LocationType_ID, v => v.TypeName, LoadData);
                    break;
                case "tb_Location":
                    SetDictDataSource<tb_Location>(k => k.Location_ID, v => v.Name, LoadData);
                    break;
                case "tb_CustomerVendor":
                    SetDictDataSource<tb_CustomerVendor>(k => k.CustomerVendor_ID, v => v.CVName, LoadData);
                    break;
                case "tb_CustomerVendorType":
                    SetDictDataSource<tb_CustomerVendorType>(k => k.Type_ID, v => v.TypeName, LoadData);
                    break;
                case "tb_ProdCategories":
                    SetDictDataSource<tb_ProdCategories>(k => k.Category_ID, v => v.Category_name, LoadData);
                    break;
                case "tb_Prod":
                    SetDictDataSource<tb_Prod>(k => k.ProdBaseID, v => v.CNName, LoadData);
                    break;
                case "View_ProdDetail":
                    SetDictDataSource<View_ProdDetail>(k => k.ProdDetailID, v => v.CNName, LoadData);
                    break;
                case "View_ProdInfo":
                    SetDictDataSource<View_ProdInfo>(k => k.ProdDetailID.Value, v => v.CNName, LoadData);
                    break;
                case "tb_ProdPropertyType":
                    SetDictDataSource<tb_ProdPropertyType>(k => k.PropertyType_ID, v => v.PropertyTypeName, LoadData);
                    break;
                case "tb_ProdProperty":
                    SetDictDataSource<tb_ProdProperty>(k => k.Property_ID, v => v.PropertyName, LoadData);
                    break;
                case "tb_Employee":
                    SetDictDataSource<tb_Employee>(k => k.Employee_ID, v => v.Employee_Name, LoadData);
                    break;
                case "tb_UserInfo":
                    SetDictDataSource<tb_UserInfo>(k => k.User_ID, v => v.UserName, LoadData);
                    break;
                case "tb_RoleInfo":
                    SetDictDataSource<tb_RoleInfo>(k => k.RoleID, v => v.RoleName, LoadData);
                    break;
                case "tb_MenuInfo":
                    SetDictDataSource<tb_MenuInfo>(k => k.MenuID, v => v.MenuName, LoadData);
                    break;
                case "tb_FieldInfo":
                    SetDictDataSource<tb_FieldInfo>(k => k.FieldInfo_ID, v => v.FieldName, LoadData);
                    break;
                case "tb_ModuleDefinition":
                    SetDictDataSource<tb_ModuleDefinition>(k => k.ModuleID, v => v.ModuleName, LoadData);
                    break;
                case "tb_BizType":
                    SetDictDataSource<tb_BizType>(k => k.Type_ID, v => v.TypeName, LoadData);
                    break;
                case "tb_StorageRack":
                    SetDictDataSource<tb_StorageRack>(k => k.Rack_ID, v => v.RackName, LoadData);
                    break;
                case "tb_OutInStockType":
                    SetDictDataSource<tb_OutInStockType>(k => k.Type_ID, v => v.TypeName, LoadData);
                    break;
                case "tb_OnlineStoreInfo":
                    SetDictDataSource<tb_OnlineStoreInfo>(k => k.Store_ID, v => v.StoreName, LoadData);
                    break;
                case "tb_ProjectGroup":
                    SetDictDataSource<tb_ProjectGroup>(k => k.ProjectGroup_ID, v => v.ProjectGroupName, LoadData);
                    break;
                case "tb_FM_Account":
                    SetDictDataSource<tb_FM_Account>(k => k.Account_id, v => v.Account_name, LoadData);
                    break;
                case "tb_FM_ExpenseType":
                    SetDictDataSource<tb_FM_ExpenseType>(k => k.ExpenseType_id, v => v.Expense_name, LoadData);
                    break;
                case "tb_FM_Subject":
                    SetDictDataSource<tb_FM_Subject>(k => k.Subject_id, v => v.Subject_name, LoadData);
                    break;
                case "tb_RolePropertyConfig":
                    SetDictDataSource<tb_RolePropertyConfig>(k => k.RolePropertyID, v => v.RolePropertyName, LoadData);
                    break;
                case "tb_CartoonBox":
                    SetDictDataSource<tb_CartoonBox>(k => k.CartonID, v => v.CartonName, LoadData);
                    break;
                case "tb_Files":
                    SetDictDataSource<tb_Files>(k => k.Doc_ID, v => v.FileName, LoadData);
                    break;
                case "tb_BOMConfigHistory":
                    SetDictDataSource<tb_BOMConfigHistory>(k => k.BOM_S_VERID, v => v.VerNo, LoadData);
                    break;
                case "tb_BoxRules":
                    SetDictDataSource<tb_BoxRules>(k => k.BoxRules_ID, v => v.BoxRuleName, LoadData);
                    break;
                case "tb_FM_PayeeInfo":
                    SetDictDataSource<tb_FM_PayeeInfo>(k => k.PayeeInfoID, v => v.Account_name, LoadData);
                    break;
                case "tb_Unit_Conversion":
                    SetDictDataSource<tb_Unit_Conversion>(k => k.UnitConversion_ID, v => v.UnitConversion_Name, LoadData);
                    break;
                case "tb_ProdPropertyValue":
                    SetDictDataSource<tb_ProdPropertyValue>(k => k.PropertyValueID, v => v.PropertyValueName, LoadData);
                    break;
                default:
                    // Console.WriteLine("未知类型: " + typeName);
                    _logger.LogInformation("SetDictDataSource 未知类型" + typeName);
                    break;
            }
        }


        /// <summary>
        /// 初始化数据字典,并且提取出结果
        /// </summary>
        public void InitDict(string tableName)
        {
            //typeNames.ToList<string>()
            try
            {
                Manager.CacheInfoList.Clear();
                Manager.CacheEntityList.Clear();
                Manager.NewTableList.Clear();
                SetDictDataSource(tableName, true);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("InitDict" + ex.Message);
            }
        }


        /// <summary>
        /// 初始化数据字典,并且提取出结果
        /// </summary>
        public void InitCacheDict(bool LoadData = true)
        {
            try
            {
                Manager.CacheInfoList.Clear();
                Manager.CacheEntityList.Clear();
                Manager.NewTableList.Clear();

                SetDictDataSource(typeNames.ToList<string>(), LoadData);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("InitDict" + ex.Message);
            }
        }

    }
}