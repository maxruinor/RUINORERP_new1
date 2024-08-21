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

namespace RUINORERP.Business
{
    public  class DictServiceHelper
    {
     private static  CommonController bdc;
        public DictServiceHelper(CommonController _bdc)
        {
            Bdc = _bdc;
            //CommonController bdc = Startup.GetFromFac<CommonController>();
        }
        private static ConcurrentDictionary<string, object> _Dict = new ConcurrentDictionary<string, object>();

        public static ConcurrentDictionary<string, object> Dict { get => _Dict; set => _Dict = value; }
        public static CommonController Bdc { get => bdc; set => bdc = value; }

        /// <summary>
        /// 通过表和主键名去找，int为主键类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expkey"></param>
        /// <returns></returns>
        public static object GetDictValue<T>(Expression<Func<T, int>> expkey, object value)
        {
            //Lazy<>
            if (value == null)
            {
                return null;
            }
            string key = expkey.Body.ToString().Split('.')[1];
            string tableName = expkey.Parameters[0].Type.Name;
            key = tableName + ":" + key + ":" + value.ToString();
            Dict.TryGetValue(key, out value);
            return value;
        }


        /// <summary>
        /// 通过表和主键名去找，int为主键类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expkey"></param>
        /// <returns></returns>
        public static void GetCheckValue<T>(Expression<Func<T, int>> expkey, object value)
        {
            string key = expkey.Body.ToString().Split('.')[1];
            string tableName = expkey.Parameters[0].Type.Name;
            key = tableName + ":" + key + ":" + value.ToString();
            if (!Dict.ContainsKey(key))
            {
                Dict.TryAdd(key, value);
            }
            else
            {
                Dict[key] = value;
            }
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expkey"></param>
        /// <returns></returns>
        public static void DeleteValue<T>(Expression<Func<T, int>> expkey, object value)
        {
            string key = expkey.Body.ToString().Split('.')[1];
            string tableName = expkey.Parameters[0].Type.Name;
            key = tableName + ":" + key + ":" + value.ToString();
            if (Dict.ContainsKey(key))
            {
                Dict.TryRemove(key, out value);
            }
        }

        private static void SetDictDataSource<T>(Expression<Func<T, int>> expkey, Expression<Func<T, string>> expvalue)
        {
            string key = expkey.Body.ToString().Split('.')[1];
            string value = expvalue.Body.ToString().Split('.')[1];
            string tableName = expkey.Parameters[0].Type.Name;
            
            var list = Bdc.GetBindSource<T>(tableName);
            foreach (var item in list)
            {
                //设置属性的值
                object xkey = typeof(T).GetProperty(key).GetValue(item, null);
                object xValue = typeof(T).GetProperty(value).GetValue(item, null);
                //string pv = RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(item, key);
                string dckey = tableName + ":" + key + ":" + xkey;
                if (!_Dict.ContainsKey(dckey))
                {
                    _Dict.TryAdd(dckey, xValue);
                }
            }
        }

        /// <summary>
        /// 初始化数据字典
        /// </summary>
        public static void InitDict()
        {
            _Dict.Clear();
            SetDictDataSource<tb_Currency>(k => k.Currency_ID, v => v.CurrencyName);
            SetDictDataSource<tb_ProductType>(k => k.Type_ID, v => v.TypeName);
            SetDictDataSource<tb_Unit>(k => k.Unit_ID, v => v.UnitName);
            SetDictDataSource<tb_Department>(k => k.DepartmentID, v => v.DepartmentName);
            SetDictDataSource<tb_LocationType>(k => k.LocationType_ID, v => v.TypeName);
            SetDictDataSource<tb_Location>(k => k.Location_ID, v => v.Name);
            SetDictDataSource<tb_CustomerVendor>(k => k.CustomerVendor_ID, v => v.CVName);
            SetDictDataSource<tb_CustomerVendorType>(k => k.Type_ID, v => v.TypeName);
            SetDictDataSource<tb_ProductCategories>(k => k.category_ID, v => v.category_name);

        }

    }
}
