
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
using SqlSugar;
using RUINORERP.Global;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 销售订单统计分析
    /// </summary>
    public partial class View_SaleOrderItemsProcessor : BaseProcessor
    {
        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();

            var lambda = Expressionable.Create<tb_CustomerVendor>()
                       .And(t => t.isdeleted == false)
                       .And(t => t.Is_available == true)
                       .And(t => t.IsCustomer == true)
                       .And(t => t.Is_enabled == true)
                       .AndIF(AuthorizeController.GetSaleLimitedAuth(_appContext), t => t.Employee_ID == _appContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                       .ToExpression();//注意 这一句 不能少

            queryFilter.SetQueryField<View_SaleOrderItems, tb_CustomerVendor>(c => c.CustomerVendor_ID, lambda);

            //可以根据关联外键自动加载条件，条件用公共虚方法
            queryFilter.SetQueryField<View_SaleOrderItems>(c => c.SaleDate);
            queryFilter.SetQueryField<View_SaleOrderItems>(c => c.SOrderNo);
            queryFilter.SetQueryField<View_SaleOrderItems>(c => c.Employee_ID, true);
            queryFilter.SetQueryField<View_SaleOrderItems>(c => c.PayStatus, QueryFieldType.CmbEnum, typeof(PayStatus));
            queryFilter.SetQueryField<View_SaleOrderItems>(c => c.SKU);
            queryFilter.SetQueryField<View_SaleOrderItems>(c => c.CNName);
            queryFilter.SetQueryField<View_SaleOrderItems>(c => c.Model);
            queryFilter.SetQueryField<View_SaleOrderItems>(c => c.Specifications);
            queryFilter.SetQueryField<View_SaleOrderItems>(c => c.ProjectGroup_ID,  typeof(tb_ProjectGroup));
            queryFilter.SetQueryField<View_SaleOrderItems>(c => c.Category_ID);
            queryFilter.SetQueryField<View_SaleOrderItems>(c => c.ApprovalStatus, QueryFieldType.CmbEnum, typeof(ApprovalStatus));
            queryFilter.SetQueryField<View_SaleOrderItems>(c => c.DataStatus, QueryFieldType.CmbEnum, typeof(DataStatus));
            queryFilter.SetQueryField<View_SaleOrderItems>(c => c.property);
            queryFilter.SetQueryField<View_SaleOrderItems>(c => c.ProductNo);
            return queryFilter;
        }


    }
}



