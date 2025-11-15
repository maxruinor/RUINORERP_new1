
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/29/2025 20:39:10
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
using RUINORERP.Business.RowLevelAuthService;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 行级权限规则
    /// </summary>
    public partial class tb_RowAuthPolicyProcessor:BaseProcessor 
    {
        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();

            //可以根据关联外键自动加载条件，条件用公共虚方法
            queryFilter.SetQueryField<tb_RowAuthPolicy>(c => c.PolicyName);
            queryFilter.SetQueryField<tb_RowAuthPolicy>(c => c.IsEnabled);
            queryFilter.SetQueryField<tb_RowAuthPolicy>(c => c.PolicyDescription);
            queryFilter.SetQueryField<tb_RowAuthPolicy>(c => c.TargetTable);
            queryFilter.SetQueryField<tb_RowAuthPolicy>(c => c.TargetEntity);
            queryFilter.SetQueryField<tb_RowAuthPolicy>(c => c.DefaultRuleEnum, QueryFieldType.CmbEnum, typeof(RowLevelAuthRule));
            queryFilter.SetQueryField<tb_RowAuthPolicy>(c => c.EntityType);
            queryFilter.SetQueryField<tb_RowAuthPolicy>(c => c.JoinTable);
            queryFilter.SetQueryField<tb_RowAuthPolicy>(c => c.JoinOnClause );
            queryFilter.SetQueryField<tb_RowAuthPolicy>(c => c.JoinType);
            queryFilter.SetQueryField<tb_RowAuthPolicy>(c => c.IsJoinRequired);

            return queryFilter;
        }

    }
}



