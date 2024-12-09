
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/09/2024 21:09:55
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
using RUINORERP.Business.Security;
using RUINORERP.Global.EnumExt.CRM;
using RUINORERP.Global;
using SqlSugar;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 跟进计划表
    /// </summary>
    public partial class tb_CRM_FollowUpPlansProcessor:BaseProcessor 
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();

            //内部的公共部分，外部是特殊情况
            var lambda = Expressionable.Create<tb_CRM_FollowUpPlans>()
                            .And(t => t.isdeleted == false)
                           .AndIF(AuthorizeController.GetOwnershipControl(_appContext), t => t.Employee_ID == _appContext.CurUserInfo.UserInfo.Employee_ID)
                          .ToExpression();//注意 这一句 不能少

            queryFilter.FilterLimitExpressions.Add(lambda);


            queryFilter.SetQueryField<tb_CRM_FollowUpPlans>(c => c.PlanStartDate);
            queryFilter.SetQueryField<tb_CRM_FollowUpPlans>(c => c.Customer_id, true);
            queryFilter.SetQueryField<tb_CRM_FollowUpPlans>(c => c.Employee_ID, true);
            queryFilter.SetQueryField<tb_CRM_FollowUpPlans>(c => c.PlanStatus, QueryFieldType.CmbEnum, typeof(FollowUpPlanStatus));
            queryFilter.SetQueryField<tb_CRM_FollowUpPlans>(c => c.PlanContent);
            queryFilter.SetQueryField<tb_CRM_FollowUpPlans>(c => c.PlanSubject);
            queryFilter.SetQueryField<tb_CRM_FollowUpPlans>(c => c.PlanEndDate);
            queryFilter.SetQueryField<tb_CRM_FollowUpPlans>(c => c.Created_at);
            return queryFilter;
        }

    }
}



