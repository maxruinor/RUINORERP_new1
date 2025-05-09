
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/19/2025 22:58:10
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
using RUINORERP.Global.EnumExt;
using RUINORERP.Global;
using SqlSugar;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 会计科目表，财务系统中使用
    /// </summary>
    public partial class tb_FM_SubjectProcessor:BaseProcessor 
    {
        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();
             
            //可以根据关联外键自动加载条件，条件用公共虚方法
            queryFilter.SetQueryField<tb_FM_Subject>(c => c.Subject_name);
            queryFilter.SetQueryField<tb_FM_Subject>(c => c.Subject_en_name);
            queryFilter.SetQueryField<tb_FM_Subject>(c => c.Subject_code);
            queryFilter.SetQueryField<tb_FM_Subject>(c => c.Subject_Type, false);
            queryFilter.SetQueryField<tb_FM_Subject>(c => c.Balance_direction);
            queryFilter.SetQueryField<tb_FM_Subject>(c => c.Notes);

            return queryFilter;
        }

    }
}



