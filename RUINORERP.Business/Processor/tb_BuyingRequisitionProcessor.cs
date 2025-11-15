
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/13/2024 10:41:37
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
    /// 请购单，可能来自销售订单,也可以来自其他日常需求也可能来自生产需求也可以直接录数据，是一个纯业务性的数据表
   
    /// </summary>
    public partial class tb_BuyingRequisitionProcessor:BaseProcessor 
    {
        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();
            queryFilter.SetQueryField<tb_BuyingRequisition>(c => c.PuRequisitionNo);
            queryFilter.SetQueryField<tb_BuyingRequisition>(c => c.RefBillNO);
            queryFilter.SetQueryField<tb_BuyingRequisition>(c => c.ApplicationDate);
            queryFilter.SetQueryField<tb_BuyingRequisition>(c => c.DepartmentID);
            queryFilter.SetQueryField<tb_BuyingRequisition>(c => c.Employee_ID);
            queryFilter.SetQueryField<tb_BuyingRequisition>(c => c.PrintStatus, QueryFieldType.CmbEnum, typeof(PrintStatus));
            queryFilter.SetQueryField<tb_BuyingRequisition>(c => c.ApprovalStatus, QueryFieldType.CmbEnum, typeof(ApprovalStatus));
            queryFilter.SetQueryField<tb_BuyingRequisition>(c => c.DataStatus, QueryFieldType.CmbEnum, typeof(DataStatus));
            return queryFilter;
        }

    }
}



