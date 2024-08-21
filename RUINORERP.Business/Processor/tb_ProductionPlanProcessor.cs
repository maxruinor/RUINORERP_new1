
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
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Model.Base;
using RUINORERP.Common.Extensions;
using RUINORERP.IServices.BASE;
using RUINORERP.Model.Context;
using System.Linq;
using RUINOR.Core;
using RUINORERP.Common.Helper;
using AutoMapper;
using RUINORERP.Global;
using RUINORERP.Business.Security;
using SqlSugar;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 生产计划表 应该是分析来的。可能来自于生产需求单，比方系统根据库存情况分析销售情况等也也可以手动。也可以程序分析
    /// </summary>
    public partial class tb_ProductionPlanProcessor : BaseProcessor
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();
            queryFilter.SetQueryField<tb_ProductionPlan>(c => c.PPNo);
            queryFilter.SetQueryField<tb_ProductionPlan>(c => c.PlanDate);
            queryFilter.SetQueryField<tb_ProductionPlan>(c => c.DepartmentID);
            queryFilter.SetQueryField<tb_ProductionPlan>(c => c.Employee_ID);
            queryFilter.SetQueryField<tb_ProductionPlan>(c => c.ProjectGroup_ID);

            //内部的公共部分，外部是特殊情况
            var lambda = Expressionable.Create<tb_SaleOrder>()
                            .And(t => t.isdeleted == false)
                            .And(t => t.DataStatus > 2) // 1 2 4 8 
                          .ToExpression();

            queryFilter.SetQueryField<tb_ProductionPlan, tb_SaleOrder>(c => c.SOrder_ID, c => c.SaleOrderNo, t => t.SOrderNo, true, lambda);

            queryFilter.SetQueryField<tb_ProductionPlan>(c => c.Priority, QueryFieldType.CmbEnum, typeof(Priority));
            queryFilter.SetQueryField<tb_ProductionPlan>(c => c.PrintStatus, QueryFieldType.CmbEnum, typeof(PrintStatus));
            queryFilter.SetQueryField<tb_ProductionPlan>(c => c.ApprovalStatus, QueryFieldType.CmbEnum, typeof(ApprovalStatus));
            queryFilter.SetQueryField<tb_ProductionPlan>(c => c.DataStatus, QueryFieldType.CmbEnum, typeof(DataStatus));
            queryFilter.SetQueryField<tb_ProductionPlan>(c => c.Notes);
            return queryFilter;
        }




    }
}



