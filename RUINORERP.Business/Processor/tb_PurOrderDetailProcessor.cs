
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
    public partial class tb_PurOrderDetailProcessor : BaseProcessor 
    {
        public override List<string> GetSummaryCols()
        {
            List<Expression<Func<tb_PurOrderDetail, object>>> SummaryCols = new List<Expression<Func<tb_PurOrderDetail, object>>>();
            SummaryCols.Add(c => c.Quantity);
            SummaryCols.Add(c => c.SubtotalAmount);
            SummaryCols.Add(c => c.TotalReturnedQty);
            SummaryCols.Add(c => c.DeliveredQuantity);
            SummaryCols.Add(c => c.TaxAmount);
            List<string> SummaryList = ExpressionHelper.ExpressionListToStringList<tb_PurOrderDetail>(SummaryCols);
            return SummaryList;
        }


    }
}



