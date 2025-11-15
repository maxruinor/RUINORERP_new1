
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/14/2024 18:29:34
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
using SqlSugar;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 调拨单-两个仓库之间的库存转移
    /// </summary>
    public partial class tb_StockTransferProcessor : BaseProcessor
    {
        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();
            //可以根据关联外键自动加载条件，条件用公共虚方法
            queryFilter.SetQueryField<tb_StockTransfer>(c => c.StockTransferNo);
            queryFilter.SetQueryFieldByAlias<tb_StockTransfer, tb_Location>(a => a.Location_ID_from, null, b => b.Location_ID, b => b.Name);
            queryFilter.SetQueryFieldByAlias<tb_StockTransfer, tb_Location>(a => a.Location_ID_to, null, b => b.Location_ID, b => b.Name);
            queryFilter.SetQueryField<tb_StockTransfer>(c => c.Employee_ID, true);
            queryFilter.SetQueryField<tb_StockTransfer>(c => c.Transfer_date);
            queryFilter.SetQueryField<tb_StockTransfer>(c => c.PrintStatus, QueryFieldType.CmbEnum, typeof(PrintStatus));
            queryFilter.SetQueryField<tb_StockTransfer>(c => c.ApprovalStatus, QueryFieldType.CmbEnum, typeof(ApprovalStatus));
            queryFilter.SetQueryField<tb_StockTransfer>(c => c.DataStatus, QueryFieldType.CmbEnum, typeof(DataStatus));
            queryFilter.SetQueryField<tb_StockTransfer>(c => c.Notes);
            return queryFilter;
        }
    }
}




