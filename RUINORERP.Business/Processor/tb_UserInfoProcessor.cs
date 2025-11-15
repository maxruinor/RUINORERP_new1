
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


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 用户表
    /// </summary>
    public partial class tb_UserInfoProcessor : BaseProcessor
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();
            queryFilter.SetQueryField<tb_UserInfo>(c => c.UserName);
            queryFilter.SetQueryField<tb_UserInfo>(c => c.Employee_ID);
            queryFilter.SetQueryField<tb_UserInfo>(c => c.IsSuperUser);
            queryFilter.SetQueryField<tb_UserInfo>(c => c.is_available);
            queryFilter.SetQueryField<tb_UserInfo>(c => c.is_enabled);
            queryFilter.SetQueryField<tb_UserInfo>(c => c.Notes);
            return queryFilter;
        }




    }
}



