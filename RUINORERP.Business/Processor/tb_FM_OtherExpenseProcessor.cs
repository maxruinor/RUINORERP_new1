
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/29/2024 13:46:09
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
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Model.Base;
using RUINORERP.Common.Extensions;
using RUINORERP.IServices.BASE;
using RUINORERP.Model.Context;
using System.Linq;
using RUINOR.Core;
using RUINORERP.Common.Helper;
using RUINORERP.Global;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 其他费用记录表，账户管理，财务系统中使用,像基础资料一样单表操作简单
    /// </summary>
    public partial class tb_FM_OtherExpenseProcessor:BaseProcessor 
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();

            //var lambda = Expressionable.Create<tb_CustomerVendor>()
            //           .And(t => t.isdeleted == false)
            //           .And(t => t.Is_available == true)
            //           .And(t => t.IsCustomer == true)
            //           .And(t => t.Is_enabled == true)
            //           .AndIF(AuthorizeController.GetSaleLimitedAuth(_appContext), t => t.Employee_ID == _appContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
            //           .ToExpression();//注意 这一句 不能少

            // queryFilter.SetQueryField<tb_FM_ExpenseClaim, tb_CustomerVendor>(c => c.CustomerVendor_ID, lambda);

            //可以根据关联外键自动加载条件，条件用公共虚方法
            queryFilter.SetQueryField<tb_FM_OtherExpense>(c => c.ExpenseNo);
            queryFilter.SetQueryField<tb_FM_OtherExpense>(c => c.Employee_ID);
            queryFilter.SetQueryField<tb_FM_OtherExpense>(c => c.DocumentDate, false);

            queryFilter.SetQueryField<tb_FM_OtherExpense>(c => c.PrintStatus, QueryFieldType.CmbEnum, typeof(PrintStatus));
            queryFilter.SetQueryField<tb_FM_OtherExpense>(c => c.ApprovalStatus, QueryFieldType.CmbEnum, typeof(ApprovalStatus));
            queryFilter.SetQueryField<tb_FM_OtherExpense>(c => c.DataStatus, QueryFieldType.CmbEnum, typeof(DataStatus));
            queryFilter.SetQueryField<tb_FM_OtherExpense>(c => c.Notes);
            return queryFilter;
        }




    }
}



