
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/07/2024 20:48:13
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
    /// 制令单-生产工单 ，工单(MO)各种企业的叫法不一样，比如生产单，制令单，生产指导单，裁单，等等。其实都是同一个东西–MO,    来源于 销售订单，计划单，生产需求单，我在下文都以工单简称。https://blog.csdn.net/qq_37365475/article/details/106612960
    /// </summary>
    public partial class tb_ManufacturingOrderProcessor : BaseProcessor
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();
            //内部的公共部分，外部是特殊情况
            var lambda = Expressionable.Create<tb_ManufacturingOrder>()
                            .And(t => t.isdeleted == false)
                          .ToExpression();//注意 这一句 不能少
            //这个因为供应商和客户混在一起。限制条件在外面 调用时确定 
            //2024-4-11思路升级  条件可以合并，这里也可以限制。合并时要注意怎么联接
            queryFilter.FilterLimitExpressions.Add(lambda);
            queryFilter.SetQueryField<tb_ManufacturingOrder>(c => c.MONO);
            queryFilter.SetQueryField<tb_ManufacturingOrder, tb_ProductionDemand>(c => c.PDID, c => c.PDNO, t => t.PDNo);
            queryFilter.SetQueryField<tb_ManufacturingOrder>(c => c.CustomerPartNo);
            queryFilter.SetQueryField<tb_ManufacturingOrder>(c => c.BOM_No);
            queryFilter.SetQueryField<tb_ManufacturingOrder>(c => c.CNName);
            queryFilter.SetQueryField<tb_ManufacturingOrder>(c => c.SKU);
            //queryFilter.SetQueryField<tb_ManufacturingOrder>(c => c.ProdDetailID);
            queryFilter.SetQueryField<tb_ManufacturingOrder, View_ProdDetail>(c => c.ProdDetailID, c => c.SKU, r => r.SKU);
            queryFilter.SetQueryField<tb_ManufacturingOrder>(c => c.Type_ID);
            queryFilter.SetQueryField<tb_ManufacturingOrder>(c => c.Specifications);
            queryFilter.SetQueryField<tb_ManufacturingOrder>(c => c.CustomerVendor_ID);
            queryFilter.SetQueryField<tb_ManufacturingOrder>(c => c.PrintStatus, QueryFieldType.CmbEnum, typeof(PrintStatus));
            queryFilter.SetQueryField<tb_ManufacturingOrder>(c => c.ApprovalStatus, QueryFieldType.CmbEnum, typeof(ApprovalStatus));
            queryFilter.SetQueryField<tb_ManufacturingOrder>(c => c.DataStatus, QueryFieldType.CmbEnum, typeof(DataStatus));
            queryFilter.SetQueryField<tb_ManufacturingOrder>(c => c.Employee_ID);
            //queryFilter.SetQueryField<tb_ManufacturingOrder>(c => c.tb_location);
            queryFilter.SetQueryField<tb_ManufacturingOrder>(c => c.DepartmentID);
            queryFilter.SetQueryField<tb_ManufacturingOrder>(c => c.Created_at);
            return queryFilter;
        }

    }
}



