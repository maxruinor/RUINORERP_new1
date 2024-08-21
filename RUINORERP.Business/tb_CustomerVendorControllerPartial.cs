
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/22/2023 17:06:12
// **************************************
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.IServices;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Model;
using FluentValidation.Results;
using RUINORERP.Services;
using SqlSugar;
using RUINORERP.Common.Helper;
using RUINORERP.Business.Security;
using System.Linq;
using System.Reflection;

namespace RUINORERP.Business
{
    /// <summary>
    /// 客户厂商表
    /// </summary>
    public partial class tb_CustomerVendorController<T>
    {

        public virtual async Task<List<tb_CustomerVendor>> QueryAsync(bool isSupplier)
        {
            List<tb_CustomerVendor> list = new List<tb_CustomerVendor>();
            if (isSupplier)
            {
                Expression<Func<tb_CustomerVendor, bool>> exp = Expressionable.Create<tb_CustomerVendor>() //创建表达式
              .And(it => it.IsVendor == isSupplier)
              .ToExpression();//注意 这一句 不能少

                list = await _tb_CustomerVendorServices.Query(exp);
            }
            else
            {
                Expression<Func<tb_CustomerVendor, bool>> exp1 = it => it.IsVendor != isSupplier;
                list = await _tb_CustomerVendorServices.Query(exp1);
            }

            return list;
        }

        
       

         


        /// <summary>
        /// 获取查询生成的字段,传到公共查询UI生成，有时个别字段也要限制性条件，如下拉数据源的限制，这里也可以传入
        /// 暂时不处理  TODO 没有完成
        /// </summary>
        /// <returns></returns>
        public override List<KeyValuePair<string, Expression<Func<T, bool>>>> GetQueryConditionsListWithlimited(Expression<Func<T, bool>> Conditions)
        {
            List<KeyValuePair<string, Expression<Func<T, bool>>>> QueryConditions = new List<KeyValuePair<string, Expression<Func<T, bool>>>>();

            var lambda = Expressionable.Create<tb_CustomerVendor>()
                           .And(t => t.IsCustomer == true)
                            .And(t => t.isdeleted == false)
                             .And(t => t.Is_available == true)
                              .And(t => t.Is_enabled == true)
                              .AndIF(AuthorizeController.GetSaleLimitedAuth(_appContext) && !_appContext.IsSuperUser, t => t.Employee_ID == _appContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户
                           .ToExpression();//注意 这一句 不能少

            QueryConditions.Add(new KeyValuePair<string, Expression<Func<T, bool>>>(ExpressionHelper.ExpressionToString<tb_CustomerVendor>(c => c.CVName), null));
            QueryConditions.Add(new KeyValuePair<string, Expression<Func<T, bool>>>(ExpressionHelper.ExpressionToString<tb_CustomerVendor>(c => c.CVCode), null));
            QueryConditions.Add(new KeyValuePair<string, Expression<Func<T, bool>>>(ExpressionHelper.ExpressionToString<tb_CustomerVendor>(c => c.Contact), null));
            QueryConditions.Add(new KeyValuePair<string, Expression<Func<T, bool>>>(ExpressionHelper.ExpressionToString<tb_CustomerVendor>(c => c.Notes), null));
            QueryConditions.Add(new KeyValuePair<string, Expression<Func<T, bool>>>(ExpressionHelper.ExpressionToString<tb_CustomerVendor>(c => c.Phone), null));
            QueryConditions.Add(new KeyValuePair<string, Expression<Func<T, bool>>>(ExpressionHelper.ExpressionToString<tb_CustomerVendor>(c => c.Address), null));
            QueryConditions.Add(new KeyValuePair<string, Expression<Func<T, bool>>>(ExpressionHelper.ExpressionToString<tb_CustomerVendor>(c => c.Employee_ID), null));
            QueryConditions.Add(new KeyValuePair<string, Expression<Func<T, bool>>>(ExpressionHelper.ExpressionToString<tb_CustomerVendor>(c => c.Paytype_ID), null));
            QueryConditions.Add(new KeyValuePair<string, Expression<Func<T, bool>>>(ExpressionHelper.ExpressionToString<tb_CustomerVendor>(c => c.CVName), null));


            //Expression<Func<T, bool>> expCondition,

            /*
             //创建表达式
            var lambda = Expressionable.Create<tb_CustomerVendor>()
                            .And(t => t.IsCustomer == true)
                             .And(t => t.isdeleted == false)
                              .And(t => t.Is_available == true)
                               .And(t => t.Is_enabled == true)
                               .AndIF(AppContext.SysConfig.SaleBizLimited && !AppContext.IsSuperUser, t => t.Employee_ID == AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户
                            .ToExpression();//注意 这一句 不能少
             */

            // List<string> qlist = Common.Helper.ExpressionHelper.ExpressionListToStringList(QueryConditions);
            return QueryConditions;
        }

    }
}