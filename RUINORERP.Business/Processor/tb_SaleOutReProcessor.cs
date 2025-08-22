
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
using RUINORERP.Business.Security;
using RUINORERP.Global;
using SqlSugar;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 销售出库退回单
    /// </summary>
    public partial class tb_SaleOutReProcessor : BaseProcessor
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();

            var lambda = Expressionable.Create<tb_CustomerVendor>()
                       .And(t => t.isdeleted == false)
                
                       .And(t => t.IsCustomer == true)
                       .And(t => t.Is_enabled == true)
                       //.AndIF(AuthorizeController.GetSaleLimitedAuth(_appContext), t => t.Employee_ID == _appContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                       .ToExpression();//注意 这一句 不能少
            queryFilter.SetQueryField<tb_SaleOutRe>(c => c.ReturnNo);
            queryFilter.SetQueryField<tb_SaleOutRe>(c => c.RefundStatus);
            queryFilter.SetQueryField<tb_SaleOutRe>(c => c.Paytype_ID);
            queryFilter.SetQueryField<tb_SaleOut, tb_CustomerVendor>(c => c.CustomerVendor_ID, lambda);

            //可以根据关联外键自动加载条件，条件用公共虚方法
            queryFilter.SetQueryField<tb_SaleOutRe>(c => c.PlatformOrderNo);
            queryFilter.SetQueryField<tb_SaleOutRe, tb_SaleOut>(c => c.SaleOut_MainID, c => c.SaleOut_NO, t => t.SaleOutNo);
            queryFilter.SetQueryField<tb_SaleOutRe>(c => c.Employee_ID, true);
            queryFilter.SetQueryField<tb_SaleOutRe>(c => c.IsFromPlatform);
            queryFilter.SetQueryField<tb_SaleOutRe>(c => c.ProjectGroup_ID, true, AdvQueryProcessType.CmbMultiChoiceCanIgnore);
            queryFilter.SetQueryField<tb_SaleOutRe>(c => c.PayStatus, QueryFieldType.CmbEnum, typeof(PayStatus));
            queryFilter.SetQueryField<tb_SaleOutRe>(c => c.PrintStatus, QueryFieldType.CmbEnum, typeof(PrintStatus));
            queryFilter.SetQueryField<tb_SaleOutRe>(c => c.ApprovalStatus, QueryFieldType.CmbEnum, typeof(ApprovalStatus));
            queryFilter.SetQueryField<tb_SaleOutRe>(c => c.DataStatus, QueryFieldType.CmbEnum, typeof(DataStatus));
            queryFilter.SetQueryField<tb_SaleOutRe>(c => c.ReturnDate);
            queryFilter.SetQueryField<tb_SaleOutRe>(c => c.ReturnReason);
            queryFilter.SetQueryField<tb_SaleOutRe>(c => c.Notes);
            return queryFilter;
        }


    }
}



