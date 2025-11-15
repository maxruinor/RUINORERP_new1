
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/20/2024 20:30:04
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
using RUINORERP.Global;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 产品拆分单
    /// </summary>
    public partial class tb_ProdSplitProcessor:BaseProcessor 
    {


        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();
            queryFilter.SetQueryField<tb_ProdSplit>(c => c.SplitNo);
            queryFilter.SetQueryField<tb_ProdSplit>(c => c.SKU);
            queryFilter.SetQueryField<tb_ProdSplit>(c => c.Location_ID);
            queryFilter.SetQueryField<tb_ProdSplit>(c => c.Employee_ID);
            queryFilter.SetQueryField<tb_ProdSplit>(c => c.BOM_No);
            queryFilter.SetQueryField<tb_ProdSplit>(c => c.Notes);
            queryFilter.SetQueryField<tb_ProdSplit>(c => c.ApprovalStatus, QueryFieldType.CmbEnum, typeof(ApprovalStatus));
            queryFilter.SetQueryField<tb_ProdSplit>(c => c.DataStatus, QueryFieldType.CmbEnum, typeof(DataStatus));
            queryFilter.SetQueryField<tb_ProdSplit>(c => c.PrintStatus, QueryFieldType.CmbEnum, typeof(PrintStatus));
            queryFilter.SetQueryField<tb_ProdSplit>(c => c.SplitDate);
            //设置不可见的列，这里实现后。在列查查询时，应该可以不需要重复用BuildInvisibleCols()
            queryFilter.SetInvisibleCol<tb_ProdSplit>(c => c.SplitID);

            return queryFilter;
        }

    }
}



