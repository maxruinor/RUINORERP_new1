﻿
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
using RUINORERP.Extensions.Middlewares;
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
    /// 字段信息表
    /// </summary>
    public partial class tb_ButtonInfoProcessor:BaseProcessor 
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();

            queryFilter.SetQueryField<tb_ButtonInfo>(c => c.BtnName);
            queryFilter.SetQueryField<tb_ButtonInfo>(c => c.BtnText);
            queryFilter.SetQueryField<tb_ButtonInfo>(c => c.ClassPath);
            queryFilter.SetQueryField<tb_ButtonInfo>(c => c.MenuID);
            queryFilter.SetQueryField<tb_ButtonInfo>(c => c.Notes);
            queryFilter.SetQueryField<tb_ButtonInfo>(c => c.FormName);
            queryFilter.SetQueryField<tb_ButtonInfo>(c => c.ButtonType);
            queryFilter.SetQueryField<tb_ButtonInfo>(c => c.HotKey);
            return queryFilter;
        }


    }
}



