
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/20/2025 18:12:12
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
using RUINORERP.Global.EnumExt;
using RUINORERP.Global;
using SqlSugar;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 记录收款 与应收的匹配，核销表-支持多对多、行项级核销 
    /// </summary>
    public partial class tb_FM_PaymentSettlementProcessor:BaseProcessor 
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();

            var lambda = Expressionable.Create<tb_CustomerVendor>()
                       .And(t => t.isdeleted == false)
                       .And(t => t.Is_enabled == true)
                       .ToExpression();
            queryFilter.SetQueryField<tb_FM_PaymentSettlement, tb_CustomerVendor>(c => c.CustomerVendor_ID, lambda);
            //可以根据关联外键自动加载条件，条件用公共虚方法
     
            queryFilter.SetQueryField<tb_FM_PaymentSettlement>(c => c.SourceBillNO);
            queryFilter.SetQueryField<tb_FM_PaymentSettlement>(c => c.Employee_ID);
            queryFilter.SetQueryField<tb_FM_PaymentSettlement>(c => c.Currency_ID);
            queryFilter.SetQueryField<tb_FM_PaymentSettlement>(c => c.SettleDate, false);

            return queryFilter;
        }



    }
}



