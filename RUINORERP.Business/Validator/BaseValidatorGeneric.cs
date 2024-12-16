using FluentValidation;
using Newtonsoft.Json.Linq;
using RUINORERP.Business.CommService;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RUINORERP.Business
{
    public abstract class BaseValidatorGeneric<T> : AbstractValidator<T> where T : class
    {
        protected BaseValidatorGeneric()
        {

        }
        //public abstract void Initialize();

        public virtual void Initialize()
        {
        }


        public bool BeUniqueName(string FiledName, string FieldValue)
        {
            // 在此处实现检查客户名称是否唯一的逻辑
            // 例如，查询数据库或其他数据存储来验证
            // 这里只是一个简单的示例，假设您有一个方法来检查唯一性
            bool isUnique = YourUniqueCheckMethod(FiledName, FieldValue);
            return isUnique;
        }

        private bool YourUniqueCheckMethod(string FiledName, string FieldValue)
        {
            string tableName = typeof(T).Name;
            //只处理需要缓存的表
            KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
            if (BizCacheHelper.Manager.NewTableList.TryGetValue(tableName, out pair))
            {

                if (BizCacheHelper.Manager.CacheEntityList.Exists(tableName))
                {
                    var cachelist = BizCacheHelper.Manager.CacheEntityList.Get(tableName);

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
                    }
                    else if (TypeHelper.IsJArrayList(listType))
                    {
                        #region  jsonlist
                        JArray varJarray = (JArray)cachelist;
                        if (varJarray.FirstOrDefault(c => c[FiledName].ToString() == FieldValue) != null)
                        { // 模拟检查唯一性的逻辑，返回 true 表示唯一，false 表示不唯一
                            return false;
                        }

                        #endregion
                    }


                }
            }
            return true;
        }

        ///// <summary>
        ///// 为了实现个性化验证，可以在子类中重写此方法
        ///// </summary>
        //public virtual void Initialize()
        //{

        //}


        public bool IsValidEmailFormat(string email)
        {
            // 正则表达式，匹配邮箱格式
            string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            return Regex.IsMatch(email, pattern);
        }

        public bool IsValidPhoneOrLandlineFormat(string phone)
        {
            // 正则表达式，匹配11位或12位手机号码
            string mobilePattern = @"^1\d{10,11}$"; // 手机号码
            string landlinePattern = @"^(\d{3,4}-)?\d{7,8}$"; // 座机号码
            return Regex.IsMatch(phone, mobilePattern) || Regex.IsMatch(phone, landlinePattern);
        }

    }
}
