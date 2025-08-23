
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
    /// 采购退货单 当采购发生退回时，您可以将它记录在本系统的采购退回作业中。在实际业务中虽然发生了采购但实际货物却还未入库，采购退回作业可退回订金、退回数量处理。采购退货单可以由采购订单转入，也可以手动录入新增单据,一般没有金额变化的，可以直接作废采购单。有订单等才需要做退回
    /// </summary>
    public partial class tb_PurOrderReProcessor : BaseProcessor
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();

            var lambda = Expressionable.Create<tb_CustomerVendor>()
                       .And(t => t.isdeleted == false)
                       .And(t => t.IsVendor == true)
                       .And(t => t.Is_enabled == true)
                       .AndIF(AuthorizeController.GetPurBizLimitedAuth(_appContext), t => t.Employee_ID == _appContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                       .OrIF(AuthorizeController.GetExclusiveLimitedAuth(_appContext), t => t.IsExclusive == true && t.Employee_ID == _appContext.CurUserInfo.UserInfo.Employee_ID)
                       .ToExpression();//注意 这一句 不能少

            queryFilter.SetQueryField<tb_PurOrderRe, tb_CustomerVendor>(c => c.CustomerVendor_ID, lambda);

            //可以根据关联外键自动加载条件，条件用公共虚方法
            queryFilter.SetQueryField<tb_PurOrderRe>(c => c.PurReturnNo);
            queryFilter.SetQueryField<tb_PurOrderRe, tb_PurOrder>(c => c.PurOrder_ID, c => c.PurOrderNo, r => r.PurOrderNo);
            queryFilter.SetQueryField<tb_PurOrderRe>(c => c.TrackNo);
            queryFilter.SetQueryField<tb_PurOrderRe>(c => c.PrintStatus, QueryFieldType.CmbEnum, typeof(PrintStatus));
            queryFilter.SetQueryField<tb_PurOrderRe>(c => c.ApprovalStatus, QueryFieldType.CmbEnum, typeof(ApprovalStatus));
            queryFilter.SetQueryField<tb_PurOrderRe>(c => c.DataStatus, QueryFieldType.CmbEnum, typeof(DataStatus));
            queryFilter.SetQueryField<tb_PurOrderRe>(c => c.ReturnDate);
            queryFilter.SetQueryField<tb_PurOrderRe>(c => c.Notes);
            return queryFilter;
        }


    }


}



