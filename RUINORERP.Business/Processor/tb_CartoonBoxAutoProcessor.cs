
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:29
// **************************************
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using RUINOR.Core;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.IServices;
using RUINORERP.IServices.BASE;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.Context;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 卡通箱规格表
    /// </summary>
    public partial class tb_CartoonBoxProcessor:BaseProcessor 
    {
        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();
            queryFilter.SetQueryField<tb_CartoonBox>(c => c.CartonName);
            queryFilter.SetQueryField<tb_CartoonBox>(c => c.Is_enabled);
            queryFilter.SetQueryField<tb_CartoonBox>(c => c.Color);
            queryFilter.SetQueryField<tb_CartoonBox>(c => c.Material);
            return queryFilter;
        }
    }
}



