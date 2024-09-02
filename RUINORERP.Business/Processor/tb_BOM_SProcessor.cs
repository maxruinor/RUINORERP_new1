
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/29/2024 13:46:08
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
    /// 标准物料表BOM_BillOfMateria_S-要适当冗余? 生产是从0开始的。先有下级才有上级。
    /// </summary>
    public partial class tb_BOM_SProcessor : BaseProcessor
    {

        /// <summary>
        /// 返回查询当前指定实体的结果，并且提供查询条件和限制条件
        /// </summary>
        /// <param name="FilterFieldLimitExpression"></param>
        /// <returns></returns>
        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();

            queryFilter.SetQueryField<tb_BOM_S>(c => c.BOM_No);
            queryFilter.SetQueryField<tb_BOM_S>(c => c.BOM_Name);
            queryFilter.SetQueryField<tb_BOM_S>(c => c.DepartmentID);
            queryFilter.SetQueryField<tb_BOM_S>(c => c.Doc_ID);
            //queryFilter.SetQueryField<tb_BOM_S>(c => c.SKU);
            queryFilter.SetQueryField<tb_BOM_S, View_ProdDetail>(c => c.ProdDetailID, c => c.SKU, r => r.SKU);
            //queryFilter.SetQueryField<tb_BOM_S>(c => c.Specifications);
            queryFilter.SetQueryField<tb_BOM_S>(c => c.property);
            //queryFilter.SetQueryField<tb_BOM_S>(c => c.Type_ID);
            queryFilter.SetQueryField<tb_BOM_S>(c => c.Notes);
            queryFilter.SetQueryField<tb_BOM_S>(c => c.ApprovalStatus, QueryFieldType.CmbEnum, typeof(ApprovalStatus));
            queryFilter.SetQueryField<tb_BOM_S>(c => c.DataStatus, QueryFieldType.CmbEnum, typeof(DataStatus));
            return queryFilter;
        }




    }
}



