
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/07/2024 20:08:56
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


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 采购入库统计
    /// </summary>
    public partial class View_PurEntryItemsProcessor : BaseProcessor
    {
        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();

            //这里经办人是仓库了。不是采购。限制不对。暂时去掉。除非关联一下采购订单。
            //视图关联订单把采购员带出。多关联一个字段采购员。后面再优化。TODO:

            //var lambda = Expressionable.Create<View_PurEntryItems>()
            //              .AndIF(AuthorizeController.GetPurBizLimitedAuth(_appContext), t => t.Employee_ID == _appContext.CurUserInfo.UserInfo.Employee_ID)
            //              .ToExpression();//注意 这一句 不能少
            //queryFilter.FilterLimitExpressions.Add(lambda);

            queryFilter.SetQueryField<View_PurEntryItems>(c => c.PurEntryNo);
            queryFilter.SetQueryField<View_PurEntryItems>(c => c.EntryDate);
            queryFilter.SetQueryField<View_PurEntryItems>(c => c.PurOrder_NO);
            var lambdacv = Expressionable.Create<tb_CustomerVendor>()
             .And(t => t.isdeleted == false)
    
             .And(t => t.IsVendor == true)
             .And(t => t.Is_enabled == true)
             //.AndIF(AuthorizeController.GetSaleLimitedAuth(_appContext), t => t.Employee_ID == _appContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
             .ToExpression();//注意 这一句 不能少
            queryFilter.SetQueryField<View_PurEntryItems, tb_CustomerVendor>(c => c.CustomerVendor_ID, lambdacv);


            //这里情况应该是用下拉的。不是像编号这种
            queryFilter.SetQueryField<View_PurEntryItems>(c => c.Employee_ID,  typeof(tb_Employee));
            queryFilter.SetQueryField<View_PurEntryItems>(c => c.Location_ID,  typeof(tb_Location));
            queryFilter.SetQueryField<View_PurEntryItems>(c => c.DepartmentID,  typeof(tb_Department));
            queryFilter.SetQueryField<View_PurEntryItems>(c => c.ProdDetailID,  typeof(View_ProdDetail));
            queryFilter.SetQueryField<View_PurEntryItems>(c => c.property);
          
            queryFilter.SetQueryField<View_PurEntryItems>(c => c.CNName);
            queryFilter.SetQueryField<View_PurEntryItems>(c => c.SKU);
            queryFilter.SetQueryField<View_PurEntryItems>(c => c.Model);
            queryFilter.SetQueryField<View_PurEntryItems>(c => c.Specifications);
            queryFilter.SetQueryField<View_PurEntryItems>(c => c.Type_ID,   typeof(tb_ProductType));
            return queryFilter;
        }


    }
}



