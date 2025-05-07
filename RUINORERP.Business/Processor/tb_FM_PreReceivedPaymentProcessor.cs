
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/18/2025 13:55:15
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
using SqlSugar;
using RUINORERP.Global.EnumExt;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 预收付款单
    /// </summary>
    public partial class tb_FM_PreReceivedPaymentProcessor : BaseProcessor
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();

            var lambda = Expressionable.Create<tb_CustomerVendor>()
                       .And(t => t.isdeleted == false)
                       .And(t => t.Is_enabled == true)
                       .ToExpression();
            queryFilter.SetQueryField<tb_FM_PreReceivedPayment, tb_CustomerVendor>(c => c.CustomerVendor_ID, lambda);
            //可以根据关联外键自动加载条件，条件用公共虚方法
            queryFilter.SetQueryField<tb_FM_PreReceivedPayment>(c => c.PreRPNO);
            queryFilter.SetQueryField<tb_FM_PreReceivedPayment>(c => c.SourceBillNo);
            queryFilter.SetQueryField<tb_FM_PreReceivedPayment>(c => c.Employee_ID);
            queryFilter.SetQueryField<tb_FM_PreReceivedPayment>(c => c.Currency_ID);
            queryFilter.SetQueryField<tb_FM_PreReceivedPayment>(c => c.DepartmentID);
            queryFilter.SetQueryField<tb_FM_PreReceivedPayment>(c => c.PrePaymentReason);
            queryFilter.SetQueryField<tb_FM_PreReceivedPayment>(c => c.PrePayDate, false);
     
            queryFilter.SetQueryField<tb_FM_PreReceivedPayment>(c => c.PrintStatus, QueryFieldType.CmbEnum, typeof(PrintStatus));
            queryFilter.SetQueryField<tb_FM_PreReceivedPayment>(c => c.ApprovalStatus, QueryFieldType.CmbEnum, typeof(ApprovalStatus));
            queryFilter.SetQueryField<tb_FM_PreReceivedPayment>(c => c.PrePaymentStatus, QueryFieldType.CmbEnum, typeof(PrePaymentStatus));
            queryFilter.SetQueryField<tb_FM_PreReceivedPayment>(c => c.Remark);

            return queryFilter;
        }


    }
}



