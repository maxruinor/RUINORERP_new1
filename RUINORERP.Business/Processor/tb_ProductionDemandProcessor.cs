
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
using RUINORERP.Global;
using SqlSugar;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 生产需求分析表 是一个中间表，由计划生产单或销售订单带入数据来分析，产生采购订单再产生制令单，分析时有三步，库存不足项（包括有成品材料所有项），采购产品建议，自制品成品建议,中间表保存记录而已，操作UI上会有生成采购订单，或生产单等操作
    /// </summary>
    public partial class tb_ProductionDemandProcessor : BaseProcessor
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();
            queryFilter.SetQueryField<tb_ProductionDemand>(c => c.PDNo);
            // queryFilter.SetQueryField<tb_ProductionDemand>(c => c.PPNo, true);

            var lambdaOI = Expressionable.Create<tb_ProductionPlan>()
                    .And(t => t.isdeleted == false)//0出1入
                    .ToExpression();//注意 这一句 不能少

            queryFilter.SetQueryField<tb_ProductionDemand, tb_ProductionPlan>(c => c.PPID, c => c.PPNo, t => t.PPNo);//外键
            queryFilter.SetQueryField<tb_ProductionDemand>(c => c.Employee_ID, true);
            queryFilter.SetQueryField<tb_ProductionDemand>(c => c.Notes);
            queryFilter.SetQueryField<tb_ProductionDemand>(c => c.ApprovalStatus, QueryFieldType.CmbEnum, typeof(ApprovalStatus));
            queryFilter.SetQueryField<tb_ProductionDemand>(c => c.DataStatus, QueryFieldType.CmbEnum, typeof(DataStatus));
            queryFilter.SetQueryField<tb_ProductionDemand>(c => c.PrintStatus, QueryFieldType.CmbEnum, typeof(PrintStatus));
            queryFilter.SetQueryField<tb_ProductionDemand>(c => c.AnalysisDate);
            //设置不可见的列，这里实现后。在列查查询时，应该可以不需要重复用BuildInvisibleCols()
            queryFilter.SetInvisibleCol<tb_ProductionDemand>(c => c.PPID);

            return queryFilter;
        }




    }
}




