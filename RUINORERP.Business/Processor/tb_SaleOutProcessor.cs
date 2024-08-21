
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
    /// 销售出库单
    /// </summary>
    public partial class tb_SaleOutProcessor : BaseProcessor
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

            queryFilter.SetQueryField<tb_SaleOut, tb_CustomerVendor>(c => c.CustomerVendor_ID, lambda);

            //可以根据关联外键自动加载条件，条件用公共虚方法
            queryFilter.SetQueryField<tb_SaleOut>(c => c.SaleOutNo);
            queryFilter.SetQueryField<tb_SaleOut, tb_SaleOrder>(t => t.SOrder_ID, t => t.SaleOrderNo, c => c.SOrderNo);
            queryFilter.SetQueryField<tb_SaleOut>(c => c.Employee_ID, true);
            queryFilter.SetQueryField<tb_SaleOut>(c => c.ProjectGroup_ID, true, AdvQueryProcessType.CmbMultiChoiceCanIgnore);
            queryFilter.SetQueryField<tb_SaleOut>(c => c.IsFromPlatform);
            queryFilter.SetQueryField<tb_SaleOut>(c => c.PlatformOrderNo);
            queryFilter.SetQueryField<tb_SaleOut>(c => c.Paytype_ID, true);
            queryFilter.SetQueryField<tb_SaleOut>(c => c.PayStatus, QueryFieldType.CmbEnum, typeof(PayStatus));
            queryFilter.SetQueryField<tb_SaleOut>(c => c.ShippingAddress);
            queryFilter.SetQueryField<tb_SaleOut>(c => c.PrintStatus, QueryFieldType.CmbEnum, typeof(PrintStatus));
            queryFilter.SetQueryField<tb_SaleOut>(c => c.ApprovalStatus, QueryFieldType.CmbEnum, typeof(ApprovalStatus));
            queryFilter.SetQueryField<tb_SaleOut>(c => c.DataStatus, QueryFieldType.CmbEnum, typeof(DataStatus));
            queryFilter.SetQueryField<tb_SaleOut>(c => c.OutDate);
            queryFilter.SetQueryField<tb_SaleOut>(c => c.Notes);

            //设置不可见的列，这里实现后。在列查查询时，应该可以不需要重复用BuildInvisibleCols()
            queryFilter.SetInvisibleCol<tb_SaleOut>(c => c.SOrder_ID);

            return queryFilter;
        }
    }
}



