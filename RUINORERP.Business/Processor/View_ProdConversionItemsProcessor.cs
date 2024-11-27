
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
using SqlSugar;
using RUINORERP.Global;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 销售订单统计分析
    /// </summary>
    public partial class View_ProdConversionItemsProcessor : BaseProcessor
    {
        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();
             

            //可以根据关联外键自动加载条件，条件用公共虚方法
            queryFilter.SetQueryField<View_ProdConversionItems>(c => c.ConversionDate);
            queryFilter.SetQueryField<View_ProdConversionItems>(c => c.ConversionNo);
            queryFilter.SetQueryField<View_ProdConversionItems>(c => c.Employee_ID, true);
            
            queryFilter.SetQueryField<View_ProdConversionItems>(c => c.SKU_from);
            queryFilter.SetQueryField<View_ProdConversionItems>(c => c.SKU_to);
            queryFilter.SetQueryField<View_ProdConversionItems>(c => c.Specifications_from);
            queryFilter.SetQueryField<View_ProdConversionItems>(c => c.Specifications_to);
            queryFilter.SetQueryField<View_ProdConversionItems>(c => c.Model_from);
            queryFilter.SetQueryField<View_ProdConversionItems>(c => c.Model_to);
            
            queryFilter.SetQueryField<View_ProdConversionItems>(c => c.ApprovalStatus, QueryFieldType.CmbEnum, typeof(ApprovalStatus));
            queryFilter.SetQueryField<View_ProdConversionItems>(c => c.DataStatus, QueryFieldType.CmbEnum, typeof(DataStatus));
            queryFilter.SetQueryField<View_ProdConversionItems>(c => c.property_from);
            queryFilter.SetQueryField<View_ProdConversionItems>(c => c.property_to);
            
            return queryFilter;
        }


    }
}



