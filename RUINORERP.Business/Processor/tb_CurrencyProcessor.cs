
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/19/2025 22:56:53
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
using SqlSugar;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 币别资料表-备份第一行数据后删除重建 如果不行则直接修改字段删除字段
    /// </summary>
    public partial class tb_CurrencyProcessor:BaseProcessor 
    {
        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();

            //内部的公共部分，外部是特殊情况
            var lambda = Expressionable.Create<tb_Currency>()
                            .And(t => t.Is_enabled == true)
                          .ToExpression();//注意 这一句 不能少
            queryFilter.FilterLimitExpressions.Add(lambda);
            queryFilter.SetQueryField<tb_Currency>(c => c.Country);
            queryFilter.SetQueryField<tb_Currency>(c => c.CurrencyCode);
            queryFilter.SetQueryField<tb_Currency>(c => c.CurrencyName);
            queryFilter.SetQueryField<tb_Currency>(c => c.Is_enabled);
            return queryFilter;
        }


    }
}



