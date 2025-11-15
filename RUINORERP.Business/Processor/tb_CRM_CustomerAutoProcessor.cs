
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/09/2024 12:02:43
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
using RUINORERP.Global.EnumExt.CRM;
using RUINORERP.Global;
using SqlSugar;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 目标客户，公海客户 CRM系统中使用，给成交客户作外键引用
    /// </summary>
    public partial class tb_CRM_CustomerProcessor : BaseProcessor
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();

            //内部的公共部分，外部是特殊情况
            //这个是公海和目标客户共用 需要在UI层判断
            var lambda = Expressionable.Create<tb_CRM_Customer>()
                            .And(t => t.isdeleted == false)
                           //.AndIF(AuthorizeController.GetOwnershipControl(_appContext), t => t.Employee_ID == _appContext.CurUserInfo.UserInfo.Employee_ID)
                          .ToExpression();//注意 这一句 不能少

            queryFilter.FilterLimitExpressions.Add(lambda);
            queryFilter.SetQueryField<tb_CRM_Customer>(c => c.CustomerName);
            queryFilter.SetQueryField<tb_CRM_Customer>(c => c.Employee_ID, true);
            queryFilter.SetQueryField<tb_CRM_Customer>(c => c.LeadID, true);//如果视图就要typof(tableName)指定。因为视图没有设置关系
            queryFilter.SetQueryField<tb_CRM_Customer>(c => c.CityID, true);
            queryFilter.SetQueryField<tb_CRM_Customer>(c => c.ProvinceID, true);
            queryFilter.SetQueryField<tb_CRM_Customer>(c => c.Region_ID, true);
            queryFilter.SetQueryField<tb_CRM_Customer>(c => c.DepartmentID, true);
            queryFilter.SetQueryField<tb_CRM_Customer>(c => c.CustomerStatus, QueryFieldType.CmbEnum, typeof(CustomerStatus));
            queryFilter.SetQueryField<tb_CRM_Customer>(c => c.CustomerTags);
            queryFilter.SetQueryField<tb_CRM_Customer>(c => c.CustomerLevel);
            queryFilter.SetQueryField<tb_CRM_Customer>(c => c.GetCustomerSource);
            queryFilter.SetQueryField<tb_CRM_Customer>(c => c.Created_at);
            return queryFilter;
        }

    }
}



