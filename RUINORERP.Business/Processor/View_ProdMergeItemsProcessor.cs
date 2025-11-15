
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
    public partial class View_ProdMergeItemsProcessor : BaseProcessor
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();

            var lambda = Expressionable.Create<View_ProdMergeItems>()
                          .AndIF(AuthorizeController.GetSaleLimitedAuth(_appContext), t => t.Employee_ID == _appContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                          .ToExpression();//注意 这一句 不能少
            queryFilter.FilterLimitExpressions.Add(lambda);

            queryFilter.SetQueryField<View_ProdMergeItems>(c => c.MergeNo);
            queryFilter.SetQueryField<View_ProdMergeItems>(c => c.MergeDate);
            queryFilter.SetQueryField<View_ProdMergeItems>(c => c.Employee_ID,  typeof(tb_Employee));
            queryFilter.SetQueryField<View_ProdMergeItems>(c => c.Location_ID,  typeof(tb_Location));
            queryFilter.SetQueryField<View_ProdMergeItems>(c => c.ProdDetailID,  typeof(View_ProdDetail));
            queryFilter.SetQueryField<View_ProdMergeItems>(c => c.property);
            queryFilter.SetQueryField<View_ProdMergeItems>(c => c.CNName);
            queryFilter.SetQueryField<View_ProdMergeItems>(c => c.BOM_No);
            queryFilter.SetQueryField<View_ProdMergeItems>(c => c.Model);
            queryFilter.SetQueryField<View_ProdMergeItems>(c => c.Specifications);
            queryFilter.SetQueryField<View_ProdMergeItems>(c => c.ProductNo);
            queryFilter.SetQueryField<View_ProdMergeItems>(c => c.Category_ID,  typeof(tb_ProdCategories));
            return queryFilter;
        }


    }
}



