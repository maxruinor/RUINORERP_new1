
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/27/2024 18:30:50
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
    /// 采购入库退回单明细
    /// </summary>
    public partial class tb_MRP_ReworkReturnDetailProcessor:BaseProcessor 
    {
        public override List<string> GetSummaryCols()
        {
            List<Expression<Func<tb_MRP_ReworkReturnDetail, object>>> SummaryCols = new List<Expression<Func<tb_MRP_ReworkReturnDetail, object>>>();
            SummaryCols.Add(c => c.Quantity);
            SummaryCols.Add(c => c.SubtotalCostAmount);
            SummaryCols.Add(c => c.SubtotalReworkFee);
            SummaryCols.Add(c => c.DeliveredQuantity);
            List<string> SummaryList = RuinorExpressionHelper.ExpressionListToStringList<tb_MRP_ReworkReturnDetail>(SummaryCols);
            return SummaryList;
        }

    }
}



