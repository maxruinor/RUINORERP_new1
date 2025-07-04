﻿using Microsoft.Extensions.Logging;
using RUINORERP.Business.Security;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.Context;
using RUINORERP.Repository.UnitOfWorks;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.Processor
{

    public partial class tb_SaleOrderProcessor : BaseProcessor
    {
        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();

            var lambda = Expressionable.Create<tb_CustomerVendor>()
                       .And(t => t.isdeleted == false)
                       .And(t => t.IsCustomer == true)
                       .And(t => t.Is_enabled == true)
                       .AndIF(AuthorizeController.GetSaleLimitedAuth(_appContext), t => t.Employee_ID == _appContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                       .ToExpression();//注意 这一句 不能少

            queryFilter.SetQueryField<tb_SaleOrder, tb_CustomerVendor>(c => c.CustomerVendor_ID, lambda);

            //可以根据关联外键自动加载条件，条件用公共虚方法
            queryFilter.SetQueryField<tb_SaleOrder>(c => c.SOrderNo);

            //单选下拉条件
            //queryFilter.SetQueryField<tb_SaleOrder>(c => c.Employee_ID);

            //多选可控条件  在属性中可以自由切换
            queryFilter.SetQueryField<tb_SaleOrder>(c => c.Employee_ID, true, AdvQueryProcessType.CmbMultiChoiceCanIgnore);


            queryFilter.SetQueryField<tb_SaleOrder>(c => c.ProjectGroup_ID, true, AdvQueryProcessType.CmbMultiChoiceCanIgnore);
            queryFilter.SetQueryField<tb_SaleOrder>(c => c.PayStatus, QueryFieldType.CmbEnum, typeof(PayStatus));
            queryFilter.SetQueryField<tb_SaleOrder>(c => c.PlatformOrderNo);
            queryFilter.SetQueryField<tb_SaleOrder>(c => c.IsFromPlatform);
            queryFilter.SetQueryField<tb_SaleOrder>(c => c.IsCustomizedOrder);
            queryFilter.SetQueryField<tb_SaleOrder>(c => c.ShippingAddress);
            queryFilter.SetQueryField<tb_SaleOrder>(c => c.PrintStatus, QueryFieldType.CmbEnum, typeof(PrintStatus));
            queryFilter.SetQueryField<tb_SaleOrder>(c => c.ApprovalStatus, QueryFieldType.CmbEnum, typeof(ApprovalStatus));
            queryFilter.SetQueryField<tb_SaleOrder>(c => c.DataStatus, QueryFieldType.CmbEnum, typeof(DataStatus));
            queryFilter.SetQueryField<tb_SaleOrder>(c => c.Created_by, typeof(tb_Employee));
            queryFilter.SetQueryField<tb_SaleOrder>(c => c.SaleDate);
            queryFilter.SetQueryField<tb_SaleOrder>(c => c.Notes);

            //设置不可见的列，这里实现后。在列查查询时，应该可以不需要重复用BuildInvisibleCols()
            queryFilter.SetInvisibleCol<tb_SaleOrder>(c => c.TotalCost);

            return queryFilter;
        }

        /// <summary>
        /// 检测是否为重复订单
        /// </summary>
        /// <returns></returns>
        public bool CheckDuplicateOrders(tb_SaleOrder saleOrder)
        {
            bool rs = false;

            return rs;
        }


        public override List<string> GetSummaryCols()
        {
            List<Expression<Func<tb_SaleOrder, object>>> SummaryCols = new List<Expression<Func<tb_SaleOrder, object>>>();
            SummaryCols.Add(c => c.TotalQty);
            SummaryCols.Add(c => c.TotalAmount);
            SummaryCols.Add(c => c.TotalTaxAmount);
            SummaryCols.Add(c => c.ForeignTotalAmount);
            SummaryCols.Add(c => c.FreightIncome);
            SummaryCols.Add(c => c.Deposit);



            List<string> SummaryList = RuinorExpressionHelper.ExpressionListToStringList<tb_SaleOrder>(SummaryCols);
            return SummaryList;
        }


    }
}