
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/16/2024 11:43:13
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
using SqlSugar;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 销售退货统计分析
    /// </summary>
    public partial class View_StockOutItemsProcessor : BaseProcessor
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();

            var lambda = Expressionable.Create<View_StockOutItems>()
                          .AndIF(AuthorizeController.GetSaleLimitedAuth(_appContext), t => t.Employee_ID == _appContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                          .ToExpression();//注意 这一句 不能少
            queryFilter.FilterLimitExpressions.Add(lambda);

            queryFilter.SetQueryField<View_StockOutItems>(c => c.BillNo);
            queryFilter.SetQueryField<View_StockOutItems>(c => c.Bill_Date);
            var lambdacv = Expressionable.Create<tb_CustomerVendor>()
             .And(t => t.isdeleted == false)
             .And(t => t.IsCustomer == true)
             .And(t => t.Is_enabled == true)
            // .AndIF(AuthorizeController.GetSaleLimitedAuth(_appContext), t => t.Employee_ID == _appContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
             .ToExpression();//注意 这一句 不能少
            queryFilter.SetQueryField<View_StockOutItems, tb_CustomerVendor>(c => c.CustomerVendor_ID, lambdacv);
            queryFilter.SetQueryField<View_StockOutItems>(c => c.Employee_ID,  typeof(tb_Employee));
            
            queryFilter.SetQueryField<View_StockOutItems>(c => c.Location_ID,  typeof(tb_Location));
            queryFilter.SetQueryField<View_StockOutItems>(c => c.ProdDetailID,  typeof(View_ProdDetail));
            queryFilter.SetQueryField<View_StockOutItems>(c => c.property);
            queryFilter.SetQueryField<View_StockOutItems>(c => c.CNName);
            queryFilter.SetQueryField<View_StockOutItems>(c => c.SKU);
            queryFilter.SetQueryField<View_StockOutItems>(c => c.Model);
            queryFilter.SetQueryField<View_StockOutItems>(c => c.Specifications);
            queryFilter.SetQueryField<View_StockOutItems>(c => c.ProductNo);
            queryFilter.SetQueryField<View_StockOutItems>(c => c.RefNO);
            queryFilter.SetQueryField<View_StockOutItems>(c => c.Notes);

            var lambdaType = Expressionable.Create<tb_OutInStockType>()
                        .And(t => t.OutIn == false)
                        .And(t => t.Is_enabled == true)
                        .ToExpression();
            queryFilter.SetQueryField<View_StockOutItems>(c => c.Type_ID,  typeof(tb_OutInStockType),true,lambdaType);
            return queryFilter;
        }


    }
}



