
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2024 17:24:06
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using Newtonsoft.Json.Linq;
using RUINORERP.Business.CommService;
using RUINORERP.Common.Helper;
using System.Collections;
using System.Linq;
using Castle.Core.Resource;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 箱规表验证类
    /// </summary>
    public partial class tb_CRM_ContactValidator : BaseValidatorGeneric<tb_CRM_Contact>
    {
        public override void Initialize()
        {
            RuleFor(x => x.Contact_Name).NotNull().WithMessage("姓名:不能为空。");
            RuleFor(customer => customer.Contact_Name)
             .Custom((value, context) =>
             {
                 var customer = context.InstanceToValidate as tb_CRM_Contact; // 假设你的实体类名为Customer

                 // 确保customer不为null  并且是新增时才判断
                 if (customer != null && customer.Contact_id == 0)
                 {
                     string propertyName = context.PropertyName;
                     // 在这里使用 propertyName
                     // Console.WriteLine($"正在验证的属性: {propertyName}");
                     // 实际的唯一性验证逻辑
                     if (!BeUniqueNameContact(propertyName, value, nameof(tb_CRM_Contact.Customer_id), customer.Customer_id.ToString()))
                     {
                         context.AddFailure("同一个客户的联系人姓名不能重复。");
                     }
                 }
             });
        }



        public bool BeUniqueNameContact(string FiledName, string FieldValue,string groupKey, string groupValue)
        {
            // 在此处实现检查客户名称是否唯一的逻辑
            // 例如，查询数据库或其他数据存储来验证
            // 这里只是一个简单的示例，假设您有一个方法来检查唯一性
            bool isUnique = YourUniqueCheckMethodContact(FiledName, FieldValue, groupKey, groupValue);
            return isUnique;
        }

        private bool YourUniqueCheckMethodContact(string FiledName, string FieldValue, string groupKey, string groupValue)
        {
            string tableName = nameof(tb_CRM_Contact);
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
                        if (varJarray.Where(c => c[groupKey].ToString() == groupValue).FirstOrDefault(c => c[FiledName].ToString() == FieldValue) != null)
                        { // 模拟检查唯一性的逻辑，返回 true 表示唯一，false 表示不唯一
                            return false;
                        }

                        #endregion
                    }


                }
            }
            return true;
        }
    }
}

