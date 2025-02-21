
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/12/2024 11:52:17
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


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 退料单(包括生产和托工） 在生产过程中或结束后，我们会根据加工任务（制令单）进行生产退料。这时就需要使用生产退料这个单据进行退料。生产退料单会影响到制令单的直接材料成本，它会冲减该制令单所发生的原料成本
    /// </summary>
    public partial class tb_MaterialReturnProcessor : BaseProcessor
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();
            //内部的公共部分，外部是特殊情况
            var lambda = Expressionable.Create<tb_MaterialReturn>()
                            .And(t => t.isdeleted == false)
                          .ToExpression();//注意 这一句 不能少
            //这个因为供应商和客户混在一起。限制条件在外面 调用时确定 
            //2024-4-11思路升级  条件可以合并，这里也可以限制。合并时要注意怎么联接
            queryFilter.FilterLimitExpressions.Add(lambda);
            queryFilter.SetQueryField<tb_MaterialReturn>(c => c.BillNo);
            queryFilter.SetQueryField<tb_MaterialReturn, tb_MaterialRequisition>(c => c.MR_ID, c => c.MaterialRequisitionNO, t => t.MaterialRequisitionNO);
            queryFilter.SetQueryField<tb_MaterialReturn>(c => c.CustomerVendor_ID);
            queryFilter.SetQueryField<tb_MaterialReturn>(c => c.Employee_ID);
            queryFilter.SetQueryField<tb_MaterialReturn>(c => c.DepartmentID);
            queryFilter.SetQueryField<tb_MaterialReturn>(c => c.ReturnDate);
            return queryFilter;
        }


    }
}



