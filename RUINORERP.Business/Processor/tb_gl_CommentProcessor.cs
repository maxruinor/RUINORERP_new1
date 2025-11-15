
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/27/2024 14:41:01
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
using RUINORERP.Global.EnumExt.CRM;
using RUINORERP.Global;
using SqlSugar;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 全局级批注表-对于重点关注的业务帮助记录和跟踪相关的额外信息，提高沟通效率和透明度
    /// </summary>
    public partial class tb_gl_CommentProcessor : BaseProcessor
    {
        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();

            //内部的公共部分，外部是特殊情况
            var lambda = Expressionable.Create<tb_gl_Comment>()
                           .AndIF(AuthorizeController.GetOwnershipControl(_appContext), t => t.Employee_ID == _appContext.CurUserInfo.UserInfo.Employee_ID)
                          .ToExpression();//注意 这一句 不能少

            queryFilter.FilterLimitExpressions.Add(lambda);
            queryFilter.SetQueryField<tb_gl_Comment>(c => c.BizTypeID, QueryFieldType.CmbEnum, typeof(BizType));
            queryFilter.SetQueryField<tb_gl_Comment>(c => c.CommentContent);
            queryFilter.SetQueryField<tb_gl_Comment>(c => c.Employee_ID);
            queryFilter.SetQueryField<tb_gl_Comment>(c => c.Created_at);


            return queryFilter;
        }

    }
}



