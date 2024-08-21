
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/31/2024 20:15:39
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
    /// 产品套装表
    /// </summary>
    public partial class tb_ProdMergeProcessor : BaseProcessor 
    {
        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();
            queryFilter.SetQueryField<tb_ProdMerge>(c => c.MergeNo);
            queryFilter.SetQueryField<tb_ProdMerge>(c => c.MergeDate);
            queryFilter.SetQueryField<tb_ProdMerge>(c => c.Notes);
            queryFilter.SetQueryField<tb_ProdMerge>(c => c.BOM_No);
            queryFilter.SetQueryField<tb_ProdMerge>(c => c.Created_at);
            return queryFilter;
        }


    }
}



