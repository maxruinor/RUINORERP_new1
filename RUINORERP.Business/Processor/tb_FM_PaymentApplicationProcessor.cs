
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/29/2024 13:46:09
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
using RUINORERP.Business.Security;
using RUINORERP.Global;
using SqlSugar;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 费用报销单
    /// </summary>
    public partial class tb_FM_PaymentApplicationProcessor : BaseProcessor
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();
            queryFilter.SetQueryField<tb_FM_PaymentApplication>(c => c.ApplicationNo);
            var lambda = Expressionable.Create<tb_CustomerVendor>()
                       .And(t => t.isdeleted == false)
                       .And(t => t.IsVendor == true)
                       .And(t => t.Is_enabled == true)
                       .ToExpression();
            queryFilter.SetQueryField<tb_FM_PaymentApplication, tb_CustomerVendor>(c => c.CustomerVendor_ID, lambda);
            //可以根据关联外键自动加载条件，条件用公共虚方法
 
            queryFilter.SetQueryField<tb_FM_PaymentApplication>(c => c.Employee_ID);
            queryFilter.SetQueryField<tb_FM_PaymentApplication>(c => c.Currency_ID);
            queryFilter.SetQueryField<tb_FM_PaymentApplication>(c => c.DepartmentID);
            queryFilter.SetQueryField<tb_FM_PaymentApplication>(c => c.PayReasonItems);
            queryFilter.SetQueryField<tb_FM_PaymentApplication>(c => c.InvoiceDate, false);
            queryFilter.SetQueryField<tb_FM_PaymentApplication>(c => c.PrintStatus, QueryFieldType.CmbEnum, typeof(PrintStatus));
            queryFilter.SetQueryField<tb_FM_PaymentApplication>(c => c.ApprovalStatus, QueryFieldType.CmbEnum, typeof(ApprovalStatus));
            queryFilter.SetQueryField<tb_FM_PaymentApplication>(c => c.DataStatus, QueryFieldType.CmbEnum, typeof(DataStatus));
            queryFilter.SetQueryField<tb_FM_PaymentApplication>(c => c.Notes);

            return queryFilter;
        }




    }
}



