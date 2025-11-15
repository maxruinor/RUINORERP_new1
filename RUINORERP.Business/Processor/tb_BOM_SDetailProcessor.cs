
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


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 标准物料表BOM明细-要适当冗余
    /// </summary>
    public partial class tb_BOM_SDetailProcessor : BaseProcessor
    {


        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();
            queryFilter.SetQueryField<tb_BOM_SDetail>(c => c.ProdDetailID, typeof(View_ProdDetail));
            queryFilter.SetQueryField<tb_BOM_SDetail>(c => c.BOM_ID);
            queryFilter.SetQueryField<tb_BOM_SDetail>(c => c.SKU);
            queryFilter.SetQueryField<tb_BOM_SDetail>(c => c.Unit_ID);
            return queryFilter;
        }


    }
}



