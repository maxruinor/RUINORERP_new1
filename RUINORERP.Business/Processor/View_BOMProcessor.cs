
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/10/2024 20:24:12
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


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// View_BOM
    /// </summary>
    public partial class View_BOMProcessor:BaseProcessor 
    {
        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();

            queryFilter.SetQueryField<View_BOM>(c => c.BOM_No);
            queryFilter.SetQueryField<View_BOM>(c => c.BOM_Name);
            queryFilter.SetQueryField<View_BOM>(c => c.DepartmentID);
            queryFilter.SetQueryField<View_BOM>(c => c.Doc_ID);
            //queryFilter.SetQueryField<View_BOM>(c => c.SKU);
            queryFilter.SetQueryField<View_BOM, View_ProdDetail>(c => c.ProdDetailID, c => c.SKU, r => r.SKU);
            queryFilter.SetQueryField<View_ProdBorrowing>(c => c.Type_ID, typeof(tb_ProductType));
            queryFilter.SetQueryField<View_BOM>(c => c.Notes);
            queryFilter.SetQueryField<View_BOM>(c => c.ApprovalStatus, QueryFieldType.CmbEnum, typeof(ApprovalStatus));
            queryFilter.SetQueryField<View_BOM>(c => c.DataStatus, QueryFieldType.CmbEnum, typeof(DataStatus));
            return queryFilter;
        }

    }
}



