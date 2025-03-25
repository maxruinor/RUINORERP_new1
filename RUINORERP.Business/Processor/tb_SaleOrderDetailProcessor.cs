
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/31/2024 20:20:02
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
    /// 销售订单明细
    /// </summary>
    public partial class tb_SaleOrderDetailProcessor:BaseProcessor 
    {
        public override List<string> GetSummaryCols()
        {
            List<Expression<Func<tb_SaleOrderDetail, object>>> SummaryCols = new List<Expression<Func<tb_SaleOrderDetail, object>>>();
            SummaryCols.Add(c => c.Quantity);
            SummaryCols.Add(c => c.SubtotalUntaxedAmount);
            SummaryCols.Add(c => c.CommissionAmount);
            SummaryCols.Add(c => c.SubtotalTaxAmount);
            SummaryCols.Add(c => c.SubtotalCostAmount);
            SummaryCols.Add(c => c.SubtotalTransAmount);
            SummaryCols.Add(c => c.TotalDeliveredQty);
            SummaryCols.Add(c => c.TotalReturnedQty);
            List<string> SummaryList = RuinorExpressionHelper.ExpressionListToStringList<tb_SaleOrderDetail>(SummaryCols);
            return SummaryList;
        }


    }
}



