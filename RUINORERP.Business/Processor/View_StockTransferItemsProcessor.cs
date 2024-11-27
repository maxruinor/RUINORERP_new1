
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


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 销售退货统计分析
    /// </summary>
    public partial class View_StockTransferItemsProcessor : BaseProcessor
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();

            var lambda = Expressionable.Create<View_StockTransferItems>()
                          .AndIF(AuthorizeController.GetSaleLimitedAuth(_appContext), t => t.Employee_ID == _appContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                          .ToExpression();//注意 这一句 不能少
            queryFilter.FilterLimitExpressions.Add(lambda);

            queryFilter.SetQueryField<View_StockTransferItems>(c => c.StockTransferNo);
            queryFilter.SetQueryField<View_StockTransferItems>(c => c.Transfer_date);
    
  
            queryFilter.SetQueryField<View_StockTransferItems>(c => c.Employee_ID,  typeof(tb_Employee));
            
            queryFilter.SetQueryField<View_StockTransferItems>(c => c.Location_ID_from,  typeof(tb_Location));
            queryFilter.SetQueryField<View_StockTransferItems>(c => c.Location_ID_to, typeof(tb_Location));
            queryFilter.SetQueryField<View_StockTransferItems>(c => c.ProdDetailID,  typeof(View_ProdDetail));
            queryFilter.SetQueryField<View_StockTransferItems>(c => c.property);
            queryFilter.SetQueryField<View_StockTransferItems>(c => c.CNName);
            queryFilter.SetQueryField<View_StockTransferItems>(c => c.SKU);
            queryFilter.SetQueryField<View_StockTransferItems>(c => c.Model);
            queryFilter.SetQueryField<View_StockTransferItems>(c => c.Specifications);
            queryFilter.SetQueryField<View_StockTransferItems>(c => c.ProductNo);
            
            
            return queryFilter;
        }


    }
}



