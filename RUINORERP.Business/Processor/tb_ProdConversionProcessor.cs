
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/10/2024 14:15:52
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


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 产品转换单 A变成B出库,AB相近。可能只是换说明书或刷机  A  数量  加或减 。B数量增加或减少。
    /// </summary>
    public partial class tb_ProdConversionProcessor:BaseProcessor 
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();
            queryFilter.SetQueryField<tb_ProdConversion>(c => c.ConversionNo);
            queryFilter.SetQueryField<tb_ProdConversion>(c => c.Reason);
            queryFilter.SetQueryField<tb_ProdConversion>(c => c.ConversionDate);
            queryFilter.SetQueryField<tb_ProdConversion>(c => c.Employee_ID);
            return queryFilter;
        }
    }
}



