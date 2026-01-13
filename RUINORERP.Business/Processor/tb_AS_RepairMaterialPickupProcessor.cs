
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/16/2025 10:31:29
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
    /// 维修领料单
    /// </summary>
    public partial class tb_AS_RepairMaterialPickupProcessor:BaseProcessor 
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();

            //可以根据关联外键自动加载条件，条件用公共虚方法
            queryFilter.SetQueryField<tb_AS_RepairMaterialPickup>(c => c.MaterialPickupNO);
            queryFilter.SetQueryField<tb_AS_RepairMaterialPickup>(c => c.RepairOrderNo);
            queryFilter.SetQueryField<tb_AS_RepairMaterialPickup>(c => c.DeliveryDate, AdvQueryProcessType.datetimeRange, false);
            queryFilter.SetQueryField<tb_AS_RepairMaterialPickup>(c => c.Employee_ID);
            queryFilter.SetQueryField<tb_AS_RepairMaterialPickup>(c => c.Created_at);
            queryFilter.SetQueryField<tb_AS_RepairMaterialPickup>(c => c.PrintStatus, QueryFieldType.CmbEnum, typeof(PrintStatus));
            queryFilter.SetQueryField<tb_AS_RepairMaterialPickup>(c => c.ApprovalStatus, QueryFieldType.CmbEnum, typeof(ApprovalStatus));
            queryFilter.SetQueryField<tb_AS_RepairMaterialPickup>(c => c.DataStatus, QueryFieldType.CmbEnum, typeof(DataStatus));

            return queryFilter;
        }

    }
}



