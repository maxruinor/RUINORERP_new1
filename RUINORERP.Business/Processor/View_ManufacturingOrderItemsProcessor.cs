using System;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Common.Extensions;
using RUINORERP.Model.Context;
using RUINORERP.Common.Helper;
using RUINORERP.Business.Security;
using SqlSugar;
using RUINORERP.Global;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 制令单明细统计分析处理器
    /// </summary>
    public partial class View_ManufacturingOrderItemsProcessor : BaseProcessor
    {
        /// <summary>
        /// 获取查询过滤器 - 配置制令单统计的查询条件
        /// </summary>
        /// <returns>查询过滤器</returns>
        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();

            // 添加权限限制表达式
            var lambda = Expressionable.Create<View_ManufacturingOrderItems>()
                .AndIF(AuthorizeController.GetSaleLimitedAuth(_appContext), 
                    t => t.Employee_ID == _appContext.CurUserInfo.UserInfo.Employee_ID)
                .ToExpression();
            queryFilter.FilterLimitExpressions.Add(lambda);

            // 客户条件 - 需求客户
            var lambdaCustomer = Expressionable.Create<tb_CustomerVendor>()
                       .And(t => t.isdeleted == false)
                       .And(t => t.IsCustomer == true)
                       .And(t => t.Is_enabled == true)
                       .ToExpression();

            queryFilter.SetQueryField<View_ManufacturingOrderItems, tb_CustomerVendor>(c => c.CustomerVendor_ID, lambdaCustomer);

            // 设置制令单相关的查询字段
            queryFilter.SetQueryField<View_ManufacturingOrderItems>(c => c.MONO);
            queryFilter.SetQueryField<View_ManufacturingOrderItems>(c => c.Created_at);
            queryFilter.SetQueryField<View_ManufacturingOrderItems>(c => c.Employee_ID, typeof(tb_Employee));
            queryFilter.SetQueryField<View_ManufacturingOrderItems>(c => c.DepartmentID, typeof(tb_Department));
            
            // 外发厂商条件 - 限制为其他类型
            var lambdaOutSource = Expressionable.Create<tb_CustomerVendor>()
                       .And(t => t.isdeleted == false)
                       .And(t => t.IsOther == true)
                       .And(t => t.Is_enabled == true)
                       .ToExpression();
            queryFilter.SetQueryField<View_ManufacturingOrderItems, tb_CustomerVendor>(c => c.CustomerVendor_ID_Out, lambdaOutSource);
            queryFilter.SetQueryField<View_ManufacturingOrderItems>(c => c.IsOutSourced);
            
            // 产品相关查询字段
            queryFilter.SetQueryField<View_ManufacturingOrderItems>(c => c.ProdDetailID, typeof(View_ProdDetail));
            queryFilter.SetQueryField<View_ManufacturingOrderItems>(c => c.property);
            queryFilter.SetQueryField<View_ManufacturingOrderItems>(c => c.SKU);
            queryFilter.SetQueryField<View_ManufacturingOrderItems>(c => c.CNName);
            queryFilter.SetQueryField<View_ManufacturingOrderItems>(c => c.Model);
            queryFilter.SetQueryField<View_ManufacturingOrderItems>(c => c.Specifications);
            queryFilter.SetQueryField<View_ManufacturingOrderItems>(c => c.ProductNo);
            queryFilter.SetQueryField<View_ManufacturingOrderItems>(c => c.Category_ID, typeof(tb_ProdCategories));
            queryFilter.SetQueryField<View_ManufacturingOrderItems>(c => c.Type_ID, typeof(tb_ProductType));
            
            // 状态相关查询字段
            queryFilter.SetQueryField<View_ManufacturingOrderItems>(c => c.DataStatus, QueryFieldType.CmbEnum, typeof(DataStatus));
            queryFilter.SetQueryField<View_ManufacturingOrderItems>(c => c.BOM_NO);
            queryFilter.SetQueryField<View_ManufacturingOrderItems>(c => c.Location_ID, typeof(tb_Location));

            return queryFilter;
        }
    }
}
