
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
using System.Windows.Forms;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 字段信息表
    /// </summary>
    public partial class tb_FieldInfoProcessor : BaseProcessor
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();
            queryFilter.SetQueryField<tb_FieldInfo>(c => c.MenuID);
            queryFilter.SetQueryField<tb_FieldInfo>(c => c.FieldName);
            queryFilter.SetQueryField<tb_FieldInfo>(c => c.ClassPath);
            queryFilter.SetQueryField<tb_FieldInfo>(c => c.EntityName);
            queryFilter.SetQueryField<tb_FieldInfo>(c => c.FieldText);
            queryFilter.SetQueryField<tb_FieldInfo>(c => c.IsChild);
            queryFilter.SetQueryField<tb_FieldInfo>(c => c.ChildEntityName);
            queryFilter.SetQueryField<tb_FieldInfo>(c => c.IsForm);
      
            return queryFilter;
        }



    }
}



