
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/12/2024 11:45:46
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
using SqlSugar;
using RUINORERP.Global;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 领料单(包括生产和托工)
    /// </summary>
    public partial class tb_MaterialRequisitionProcessor : BaseProcessor
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();
            //内部的公共部分，外部是特殊情况
            var lambda = Expressionable.Create<tb_MaterialRequisition>()
                            .And(t => t.isdeleted == false)
                          .ToExpression();//注意 这一句 不能少
            //这个因为供应商和客户混在一起。限制条件在外面 调用时确定 
            //2024-4-11思路升级  条件可以合并，这里也可以限制。合并时要注意怎么联接
            queryFilter.FilterLimitExpressions.Add(lambda);
            queryFilter.SetQueryField<tb_MaterialRequisition>(c => c.MaterialRequisitionNO);
            queryFilter.SetQueryField<tb_MaterialRequisition, tb_ManufacturingOrder>(c => c.MOID, c => c.MONO, m => m.MONO);
            queryFilter.SetQueryField<tb_MaterialRequisition>(c => c.Employee_ID);
            queryFilter.SetQueryField<tb_MaterialRequisition>(c => c.ProjectGroup_ID, true, AdvQueryProcessType.CmbMultiChoiceCanIgnore);
            queryFilter.SetQueryField<tb_MaterialRequisition>(c => c.CustomerVendor_ID, true, AdvQueryProcessType.CmbMultiChoiceCanIgnore);
            queryFilter.SetQueryField<tb_MaterialRequisition>(c => c.DepartmentID, true, AdvQueryProcessType.CmbMultiChoiceCanIgnore);
            queryFilter.SetQueryField<tb_MaterialRequisition>(c => c.PrintStatus, QueryFieldType.CmbEnum, typeof(PrintStatus));
            queryFilter.SetQueryField<tb_MaterialRequisition>(c => c.ApprovalStatus, QueryFieldType.CmbEnum, typeof(ApprovalStatus));
            queryFilter.SetQueryField<tb_MaterialRequisition>(c => c.DataStatus, QueryFieldType.CmbEnum, typeof(DataStatus));
            queryFilter.SetQueryField<tb_MaterialRequisition>(c => c.DepartmentID);
            queryFilter.SetQueryField<tb_MaterialRequisition>(c => c.DeliveryDate);
            return queryFilter;
        }


    }
}



