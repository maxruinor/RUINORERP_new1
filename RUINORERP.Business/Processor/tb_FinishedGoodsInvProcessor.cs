
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/12/2024 13:19:19
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


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 成品入库单 要进一步完善
    /// </summary>
    public partial class tb_FinishedGoodsInvProcessor:BaseProcessor 
    {
       
        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();
            queryFilter.SetQueryField<tb_FinishedGoodsInv>(c => c.DeliveryBillNo);
            queryFilter.SetQueryField<tb_FinishedGoodsInv>(c => c.DepartmentID);
            queryFilter.SetQueryField<tb_FinishedGoodsInv>(c => c.Employee_ID);
            queryFilter.SetQueryField<tb_FinishedGoodsInv>(c => c.MONo);
            queryFilter.SetQueryField<tb_FinishedGoodsInv>(c => c.DeliveryDate);
            queryFilter.SetQueryField<tb_FinishedGoodsInv>(c => c.PrintStatus, QueryFieldType.CmbEnum, typeof(PrintStatus));
            queryFilter.SetQueryField<tb_FinishedGoodsInv>(c => c.ApprovalStatus, QueryFieldType.CmbEnum, typeof(ApprovalStatus));
            queryFilter.SetQueryField<tb_FinishedGoodsInv>(c => c.DataStatus, QueryFieldType.CmbEnum, typeof(DataStatus));
            return queryFilter;
        }

        
    }
}



