
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
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Model.Base;
using RUINORERP.Common.Extensions;
using RUINORERP.IServices.BASE;
using RUINORERP.Model.Context;
using System.Linq;
using RUINOR.Core;
using RUINORERP.Common.Helper;
using RUINORERP.Business.Security;
using RUINORERP.Global;
using SqlSugar;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 产品基本信息表
    /// </summary>
    public partial class tb_ProdProcessor:BaseProcessor 
    {
        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();
            var lambda = Expressionable.Create<tb_CustomerVendor>()
                       .And(t => t.isdeleted == false)
                       .And(t => t.Is_available == true)
                          .And(t => t.IsVendor == true)
                       .And(t => t.Is_enabled == true)
                       .ToExpression();//注意 这一句 不能少

            queryFilter.SetQueryField<tb_Prod, tb_CustomerVendor>(c => c.CustomerVendor_ID, lambda);

            //可以根据关联外键自动加载条件，条件用公共虚方法
            queryFilter.SetQueryField<tb_Prod>(c => c.ProductNo);
            queryFilter.SetQueryField<tb_Prod>(c => c.CNName);
            queryFilter.SetQueryField<tb_Prod>(c => c.Model);
            queryFilter.SetQueryField<tb_Prod>(c => c.Brand);
            queryFilter.SetQueryField<tb_Prod>(c => c.Specifications);
            queryFilter.SetQueryField<tb_Prod>(c => c.Location_ID);
            queryFilter.SetQueryField<tb_Prod>(c => c.DepartmentID);
            queryFilter.SetQueryField<tb_Prod>(c => c.Rack_ID,false);
            queryFilter.SetQueryField<tb_Prod>(c => c.Type_ID);
            queryFilter.SetQueryField<tb_Prod>(c => c.ShortCode);
            queryFilter.SetQueryField<tb_Prod>(c => c.Category_ID, true);
            queryFilter.SetQueryField<tb_Prod>(c => c.DataStatus, QueryFieldType.CmbEnum, typeof(DataStatus));
            queryFilter.SetQueryField<tb_Prod>(c => c.Notes);
            return queryFilter;

        }

    }
}



