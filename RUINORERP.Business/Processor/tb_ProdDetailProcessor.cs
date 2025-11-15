
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/30/2024 22:46:07
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
using RUINORERP.Global;
using SqlSugar;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 产品详细表
    /// </summary>
    public partial class tb_ProdDetailProcessor : BaseProcessor
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();
            //可以根据关联外键自动加载条件，条件用公共虚方法
            queryFilter.SetQueryField<tb_ProdDetail>(c => c.SKU);
            queryFilter.SetQueryField<tb_ProdDetail>(c => c.ProdBaseID, typeof(tb_Prod));
            queryFilter.SetQueryField<tb_ProdDetail>(c => c.Is_enabled);
            queryFilter.SetQueryField<tb_ProdDetail>(c => c.Is_available);
            queryFilter.SetQueryField<tb_ManufacturingOrder>(c => c.DataStatus, QueryFieldType.CmbEnum, typeof(DataStatus));
            queryFilter.SetQueryField<tb_ProdDetail>(c => c.Notes);
            return queryFilter;
        }

    }
}



