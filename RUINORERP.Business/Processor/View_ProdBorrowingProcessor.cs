﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/07/2024 20:08:56
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
using SqlSugar;
using RUINORERP.Global;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 采购入库统计
    /// </summary>
    public partial class View_ProdBorrowingProcessor : BaseProcessor
    {
        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();

            //var lambda = Expressionable.Create<View_ProdBorrowing>()
            //              .AndIF(AuthorizeController.GetPurBizLimitedAuth(_appContext), t => t.Employee_ID == _appContext.CurUserInfo.UserInfo.Employee_ID)
            //              .ToExpression();//注意 这一句 不能少
            //queryFilter.FilterLimitExpressions.Add(lambda);

            var lambdacv = Expressionable.Create<tb_CustomerVendor>()
             .And(t => t.isdeleted == false)
             .And(t => t.Is_available == true)
             .And(t => t.IsVendor == true)
             .And(t => t.Is_enabled == true)
             //.AndIF(AuthorizeController.GetSaleLimitedAuth(_appContext), t => t.Employee_ID == _appContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
             .ToExpression();//注意 这一句 不能少
            queryFilter.SetQueryField<View_ProdBorrowing, tb_CustomerVendor>(c => c.CustomerVendor_ID, lambdacv);


            //这里情况应该是用下拉的。不是像编号这种
           // queryFilter.SetQueryField<View_ProdBorrowing>(c => c.Employee_ID,  typeof(tb_Employee));
            queryFilter.SetQueryField<View_ProdBorrowing>(c => c.Location_ID,  typeof(tb_Location));
            queryFilter.SetQueryField<View_ProdBorrowing>(c => c.ProdDetailID,  typeof(View_ProdDetail));
            queryFilter.SetQueryField<View_ProdBorrowing>(c => c.property);
            queryFilter.SetQueryField<View_ProdBorrowing>(c => c.BorrowNo);
            queryFilter.SetQueryField<View_ProdBorrowing>(c => c.Employee_ID, typeof(tb_Employee));
            queryFilter.SetQueryField<View_ProdBorrowing>(c => c.Out_date);
            queryFilter.SetQueryField<View_ProdBorrowing>(c => c.DueDate);
            queryFilter.SetQueryField<View_ProdBorrowing>(c => c.DataStatus, QueryFieldType.CmbEnum, typeof(DataStatus));
            queryFilter.SetQueryField<View_ProdBorrowing>(c => c.Notes);
            queryFilter.SetQueryField<View_ProdBorrowing>(c => c.CNName);
            queryFilter.SetQueryField<View_ProdBorrowing>(c => c.SKU);
            queryFilter.SetQueryField<View_ProdBorrowing>(c => c.Model);
            queryFilter.SetQueryField<View_ProdBorrowing>(c => c.Specifications);
            queryFilter.SetQueryField<View_ProdBorrowing>(c => c.Type_ID,   typeof(tb_ProductType));
            return queryFilter;
        }


    }
}



