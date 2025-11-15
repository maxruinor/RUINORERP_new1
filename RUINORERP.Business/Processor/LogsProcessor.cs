
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/29/2024 13:46:08
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


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 
    /// </summary>
    public partial class LogsProcessor:BaseProcessor 
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();
            queryFilter.SetQueryField<Logs>(c => c.User_ID);
            queryFilter.SetQueryField<Logs>(c => c.Operator);
            queryFilter.SetQueryField<Logs>(c => c.Level);
            queryFilter.SetQueryField<Logs>(c => c.ModName);
            queryFilter.SetQueryField<Logs>(c => c.ActionName);
            queryFilter.SetQueryField<Logs>(c => c.MachineName);
            queryFilter.SetQueryField<Logs>(c => c.IP);
            queryFilter.SetQueryField<Logs>(c => c.Message);
            queryFilter.SetQueryField<Logs>(c => c.Logger);
            queryFilter.SetQueryField<Logs>(c => c.Date);
            queryFilter.SetQueryField<Logs>(c => c.Exception);
            return queryFilter;
        }

    }
}



