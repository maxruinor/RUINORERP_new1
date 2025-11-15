
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
using RUINORERP.Global.EnumExt;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 费用单
    /// </summary>
    public partial class tb_FM_ProfitLossProcessor : BaseProcessor
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();
            queryFilter.SetQueryField<tb_FM_ProfitLoss>(c => c.ProfitLossNo);
            queryFilter.SetQueryField<tb_FM_ProfitLoss>(c => c.SourceBillNo);
            queryFilter.SetQueryField<tb_FM_ProfitLoss,tb_Department>(c => c.DepartmentID);
            //可以根据关联外键自动加载条件，条件用公共虚方法
            queryFilter.SetQueryField<tb_FM_ProfitLoss>(c => c.IsExpenseType);
            queryFilter.SetQueryField<tb_FM_ProfitLoss>(c => c.ProjectGroup_ID);
            queryFilter.SetQueryField<tb_FM_ProfitLoss>(c => c.SourceBizType, QueryFieldType.CmbEnum, typeof(BizType));
            queryFilter.SetQueryField<tb_FM_ProfitLoss>(c => c.IsIncludeTax);
            queryFilter.SetQueryField<tb_FM_ProfitLoss>(c => c.PostDate, AdvQueryProcessType.datetimeRange, false);
            queryFilter.SetQueryField<tb_FM_ProfitLoss>(c => c.Created_by);
            queryFilter.SetQueryField<tb_FM_ProfitLoss>(c => c.Created_at, AdvQueryProcessType.datetimeRange, true);
            queryFilter.SetQueryField<tb_FM_ProfitLoss>(c => c.ApprovalStatus, QueryFieldType.CmbEnum, typeof(ApprovalStatus));
            queryFilter.SetQueryField<tb_FM_ProfitLoss>(c => c.Remark);

            return queryFilter;
        }




    }
}



