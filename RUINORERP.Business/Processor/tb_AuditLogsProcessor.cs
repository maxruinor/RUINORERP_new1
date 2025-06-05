
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/14/2024 15:01:00
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
using RUINORERP.Global;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 标准物料表BOM明细-要适当冗余
    /// </summary>
    public partial class tb_AuditLogsProcessor : BaseProcessor
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();
            queryFilter.SetQueryField<tb_AuditLogs>(c => c.UserName);
            queryFilter.SetQueryField<tb_AuditLogs>(c => c.Employee_ID);
            //字段类型要int ，这里暂时注释掉。后面修改DB后再修改
            queryFilter.SetQueryField<tb_AuditLogs>(c => c.ActionType);
            //queryFilter.SetQueryField<tb_AuditLogs>(c => c.ActionStatus, QueryFieldType.CmbEnum, typeof(ActionStatus));
            queryFilter.SetQueryField<tb_AuditLogs>(c => c.ActionTime);
            queryFilter.SetQueryField<tb_AuditLogs>(c => c.ObjectId);
            queryFilter.SetQueryField<tb_AuditLogs>(c => c.ObjectNo);
            queryFilter.SetQueryField<tb_AuditLogs>(c => c.ObjectType, QueryFieldType.CmbEnum, typeof(BizType));
            queryFilter.SetQueryField<tb_AuditLogs>(c => c.DataContent);
            queryFilter.SetQueryField<tb_AuditLogs>(c => c.Notes);
            queryFilter.SetQueryField<tb_AuditLogs>(c => c.OldState);
            queryFilter.SetQueryField<tb_AuditLogs>(c => c.NewState);
            return queryFilter;
        }


    }
}



