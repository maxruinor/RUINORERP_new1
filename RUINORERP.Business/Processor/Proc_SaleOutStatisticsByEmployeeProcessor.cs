
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/16/2024 19:35:41
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
    /// 
    /// </summary>
    public partial class Proc_SaleOutStatisticsByEmployeeProcessor : BaseProcessor
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();
            queryFilter.SetQueryField<Proc_SaleOutStatisticsByEmployeePara>(c => c.Start, AdvQueryProcessType.datetime);
            queryFilter.SetQueryField<Proc_SaleOutStatisticsByEmployeePara>(c => c.End, AdvQueryProcessType.datetime);
            queryFilter.SetQueryField<Proc_SaleOutStatisticsByEmployeePara>(c => c.Employee_ID, true, AdvQueryProcessType.CmbMultiChoiceCanIgnore);
            queryFilter.SetQueryField<Proc_SaleOutStatisticsByEmployeePara>(c => c.ProjectGroup_ID, true, AdvQueryProcessType.CmbMultiChoiceCanIgnore);
            return queryFilter;
        }
        public override List<string> GetSummaryCols()
        {
            List<Expression<Func<Proc_SaleOutStatisticsByEmployee, object>>> SummaryCols = new List<Expression<Func<Proc_SaleOutStatisticsByEmployee, object>>>();
            SummaryCols.Add(c => c.实际成交数量);
            SummaryCols.Add(c => c.佣金返还);
            SummaryCols.Add(c => c.佣金返点);
            SummaryCols.Add(c => c.退货数量);
            SummaryCols.Add(c => c.退货金额);
            SummaryCols.Add(c => c.退货税额);
            SummaryCols.Add(c => c.销售税额);
            SummaryCols.Add(c => c.出库成交金额);
            SummaryCols.Add(c => c.总销售出库数量);
            SummaryCols.Add(c => c.实际成交数量);
            SummaryCols.Add(c => c.实际成交金额);
            SummaryCols.Add(c => c.成本);
            SummaryCols.Add(c => c.毛利润);
            List<string> SummaryList = RuinorExpressionHelper.ExpressionListToStringList<Proc_SaleOutStatisticsByEmployee>(SummaryCols);
            return SummaryList;
        }

    }
}



