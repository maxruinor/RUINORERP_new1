
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/01/2024 21:32:58
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
    /// 
    /// </summary>
    public partial class View_ProdDetailProcessor : BaseProcessor
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

            queryFilter.SetQueryField<View_ProdDetail, tb_CustomerVendor>(c => c.CustomerVendor_ID, lambda);

            //可以根据关联外键自动加载条件，条件用公共虚方法
            queryFilter.SetQueryField<View_ProdDetail>(c => c.ProductNo);
            queryFilter.SetQueryField<View_ProdDetail>(c => c.SKU);
            queryFilter.SetQueryField<View_ProdDetail>(c => c.CNName);
            queryFilter.SetQueryField<View_ProdDetail>(c => c.Specifications);
            queryFilter.SetQueryField<View_ProdDetail>(c => c.Location_ID, typeof(tb_Location));
            queryFilter.SetQueryField<View_ProdDetail>(c => c.DepartmentID);
            queryFilter.SetQueryField<View_ProdDetail>(c => c.Rack_ID, typeof(tb_StorageRack));
            queryFilter.SetQueryField<View_ProdDetail>(c => c.Type_ID, typeof(tb_ProductType));
            queryFilter.SetQueryField<View_ProdDetail>(c => c.Model);
            queryFilter.SetQueryField<View_ProdDetail>(c => c.ShortCode);
            queryFilter.SetQueryField<View_ProdDetail>(c => c.Category_ID, typeof(tb_ProdCategories));
            queryFilter.SetQueryField<View_Inventory>(c => c.LastInventoryDate);
            queryFilter.SetQueryField<View_Inventory>(c => c.LatestStorageTime);
            queryFilter.SetQueryField<View_Inventory>(c => c.LatestOutboundTime);
            queryFilter.SetQueryField<View_ProdDetail>(c => c.Notes);
            return queryFilter;




        }




    }
}



