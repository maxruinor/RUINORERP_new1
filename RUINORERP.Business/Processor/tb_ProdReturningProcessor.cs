
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
using SqlSugar;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 入库单 非生产领料/退料
    /// </summary>
    public partial class tb_ProdReturningProcessor : BaseProcessor
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();
            var lambda = Expressionable.Create<tb_CustomerVendor>()
                       .And(t => t.isdeleted == false)
                       .And(t => t.Is_available == true)
                       .And(t => t.IsVendor == true)
                       .And(t => t.Is_enabled == true)
                       //.AndIF(AuthorizeController.GetSaleLimitedAuth(_appContext), t => t.Employee_ID == _appContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                       .ToExpression();//注意 这一句 不能少

            queryFilter.SetQueryField<tb_ProdReturning, tb_CustomerVendor>(c => c.CustomerVendor_ID, lambda);


            //可以根据关联外键自动加载条件，条件用公共虚方法
            queryFilter.SetQueryField<tb_ProdReturning>(c => c.ReturnNo);
            queryFilter.SetQueryField<tb_ProdReturning>(c => c.BorrowNO);
            queryFilter.SetQueryField<tb_ProdReturning>(c => c.BorrowID, true);
            queryFilter.SetQueryField<tb_ProdReturning>(c => c.Employee_ID, true);
            queryFilter.SetQueryField<tb_ProdReturning>(c => c.ReturnDate);
            queryFilter.SetQueryField<tb_ProdReturning>(c => c.PrintStatus, QueryFieldType.CmbEnum, typeof(PrintStatus));
            queryFilter.SetQueryField<tb_ProdReturning>(c => c.ApprovalStatus, QueryFieldType.CmbEnum, typeof(ApprovalStatus));
            queryFilter.SetQueryField<tb_ProdReturning>(c => c.DataStatus, QueryFieldType.CmbEnum, typeof(DataStatus));
            queryFilter.SetQueryField<tb_ProdReturning>(c => c.Notes);
            return queryFilter;
        }




    }
}



