
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/14/2024 15:01:00
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

using RUINORERP.Model.Base;
using RUINORERP.Common.Extensions;
using RUINORERP.IServices.BASE;
using RUINORERP.Model.Context;
using System.Linq;
using RUINOR.Core;
using RUINORERP.Common.Helper;
using RUINORERP.Business.Security;
using RUINORERP.Global;
using SqlSugar;
using RUINORERP.Global.EnumExt;


namespace RUINORERP.Business.Processor
{
 
    public partial class tb_AS_AfterSaleApplyProcessor : BaseProcessor
    {
        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();

            var lambda = Expressionable.Create<tb_CustomerVendor>()
                         .And(t => t.isdeleted == false)
                         .And(t => t.IsCustomer == true)
                         .And(t => t.Is_enabled == true)
                         .AndIF(AuthorizeController.GetSaleLimitedAuth(_appContext), t => t.Employee_ID == _appContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                         .ToExpression();//注意 这一句 不能少

            queryFilter.SetQueryField<tb_AS_AfterSaleApply, tb_CustomerVendor>(c => c.CustomerVendor_ID, lambda);

            //可以根据关联外键自动加载条件，条件用公共虚方法
            queryFilter.SetQueryField<tb_AS_AfterSaleApply>(c => c.ASApplyNo);
            queryFilter.SetQueryField<tb_AS_AfterSaleApply>(c => c.CustomerSourceNo);
            queryFilter.SetQueryField<tb_AS_AfterSaleApply>(c => c.ProjectGroup_ID);
            queryFilter.SetQueryField<tb_AS_AfterSaleApply>(c => c.ApplyDate, AdvQueryProcessType.datetimeRange, false);
            //queryFilter.SetQueryField<tb_AS_AfterSaleApply>(c => c.PreDeliveryDate, AdvQueryProcessType.datetimeRange, false);
            queryFilter.SetQueryField<tb_AS_AfterSaleApply>(c => c.Employee_ID);
            queryFilter.SetQueryField<tb_AS_AfterSaleApply>(c => c.Created_at);
            queryFilter.SetQueryField<tb_AS_AfterSaleApply>(c => c.ASProcessStatus, QueryFieldType.CmbEnum, typeof(ASProcessStatus));
            queryFilter.SetQueryField<tb_AS_AfterSaleApply>(c => c.ExpenseBearerType, QueryFieldType.CmbEnum, typeof(ExpenseBearerType));
            queryFilter.SetQueryField<tb_AS_AfterSaleApply>(c => c.ExpenseAllocationMode, QueryFieldType.CmbEnum, typeof(ExpenseAllocationMode));
            queryFilter.SetQueryField<tb_AS_AfterSaleApply>(c => c.PrintStatus, QueryFieldType.CmbEnum, typeof(PrintStatus));
            queryFilter.SetQueryField<tb_AS_AfterSaleApply>(c => c.ApprovalStatus, QueryFieldType.CmbEnum, typeof(ApprovalStatus));
            queryFilter.SetQueryField<tb_AS_AfterSaleApply>(c => c.DataStatus, QueryFieldType.CmbEnum, typeof(DataStatus));

 

            return queryFilter;
        }


    }
}



