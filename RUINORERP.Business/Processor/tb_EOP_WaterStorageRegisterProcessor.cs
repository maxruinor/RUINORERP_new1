
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/17/2025 15:16:28
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
using SqlSugar;
using RUINORERP.Business.Security;
using RUINORERP.Global;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 蓄水登记表
    /// </summary>
    public partial class tb_EOP_WaterStorageRegisterProcessor:BaseProcessor 
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();

            //可以根据关联外键自动加载条件，条件用公共虚方法
            queryFilter.SetQueryField<tb_EOP_WaterStorageRegister>(c => c.WSRNo);
            
            //多选可控条件  在属性中可以自由切换
            queryFilter.SetQueryField<tb_EOP_WaterStorageRegister>(c => c.Employee_ID, true, AdvQueryProcessType.CmbMultiChoiceCanIgnore);
            queryFilter.SetQueryField<tb_EOP_WaterStorageRegister>(c => c.ProjectGroup_ID, true, AdvQueryProcessType.CmbMultiChoiceCanIgnore);
            queryFilter.SetQueryField<tb_EOP_WaterStorageRegister>(c => c.PlatformType, QueryFieldType.CmbEnum, typeof(PlatformType));
            queryFilter.SetQueryField<tb_EOP_WaterStorageRegister>(c => c.PlatformOrderNo);
            queryFilter.SetQueryField<tb_EOP_WaterStorageRegister>(c => c.ShippingAddress);
            queryFilter.SetQueryField<tb_EOP_WaterStorageRegister>(c => c.Created_by, typeof(tb_Employee));
            queryFilter.SetQueryField<tb_EOP_WaterStorageRegister>(c => c.OrderDate);
            queryFilter.SetQueryField<tb_EOP_WaterStorageRegister>(c => c.Notes);

            //设置不可见的列，这里实现后。在列查查询时，应该可以不需要重复用BuildInvisibleCols()
            queryFilter.SetInvisibleCol<tb_EOP_WaterStorageRegister>(c => c.TotalAmount);

            return queryFilter;
        }



    }
}



