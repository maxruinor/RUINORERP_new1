
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/18/2025 10:33:38
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
using RUINORERP.Business.Security;
using RUINORERP.Global;
using SqlSugar;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 蓄水登记表
    /// </summary>
    public partial class tb_EOP_WaterStorageProcessor:BaseProcessor 
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();
 
            //可以根据关联外键自动加载条件，条件用公共虚方法
            queryFilter.SetQueryField<tb_EOP_WaterStorage>(c => c.WSRNo);

            queryFilter.SetQueryField<tb_EOP_WaterStorage>(c => c.PlatformOrderNo);

            //多选可控条件  在属性中可以自由切换
            queryFilter.SetQueryField<tb_EOP_WaterStorage>(c => c.Employee_ID, true, AdvQueryProcessType.CmbMultiChoiceCanIgnore);

            queryFilter.SetQueryField<tb_EOP_WaterStorage>(c => c.ProjectGroup_ID, true, AdvQueryProcessType.CmbMultiChoiceCanIgnore);
            queryFilter.SetQueryField<tb_EOP_WaterStorage>(c => c.PlatformType, QueryFieldType.CmbEnum, typeof(PlatformType));
            queryFilter.SetQueryField<tb_EOP_WaterStorage>(c => c.TrackNo);
            queryFilter.SetQueryField<tb_EOP_WaterStorage>(c => c.ShippingAddress);
            queryFilter.SetQueryField<tb_EOP_WaterStorage>(c => c.Created_by, typeof(tb_Employee));
            queryFilter.SetQueryField<tb_EOP_WaterStorage>(c => c.OrderDate);
            queryFilter.SetQueryField<tb_EOP_WaterStorage>(c => c.PrintStatus, QueryFieldType.CmbEnum, typeof(PrintStatus));
            queryFilter.SetQueryField<tb_EOP_WaterStorage>(c => c.ApprovalStatus, QueryFieldType.CmbEnum, typeof(ApprovalStatus));
            queryFilter.SetQueryField<tb_EOP_WaterStorage>(c => c.DataStatus, QueryFieldType.CmbEnum, typeof(DataStatus));
            queryFilter.SetQueryField<tb_EOP_WaterStorage>(c => c.Notes);

            return queryFilter;
        }

        /// <summary>
        /// 检测是否为重复订单
        /// </summary>
        /// <returns></returns>
        public bool CheckDuplicateOrders(tb_EOP_WaterStorage saleOrder)
        {
            bool rs = false;

            return rs;
        }


        public override List<string> GetSummaryCols()
        {
            List<Expression<Func<tb_EOP_WaterStorage, object>>> SummaryCols = new List<Expression<Func<tb_EOP_WaterStorage, object>>>();
            SummaryCols.Add(c => c.TotalAmount);
            SummaryCols.Add(c => c.PlatformFeeAmount);

            List<string> SummaryList = RuinorExpressionHelper.ExpressionListToStringList<tb_EOP_WaterStorage>(SummaryCols);
            return SummaryList;
        }

    }
}



