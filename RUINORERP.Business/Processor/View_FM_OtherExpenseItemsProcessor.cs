﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/19/2024 01:04:32
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
    /// 费用报销统计分析
    /// </summary>
    public partial class View_FM_OtherExpenseItemsProcessor : BaseProcessor
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();

            //可以根据关联外键自动加载条件，条件用公共虚方法
            queryFilter.SetQueryField<View_FM_OtherExpenseItems>(c => c.ExpenseName);
            queryFilter.SetQueryField<View_FM_OtherExpenseItems>(c => c.DocumentDate);
            queryFilter.SetQueryField<View_FM_OtherExpenseItems>(c => c.ExpenseNo);
            queryFilter.SetQueryField<View_FM_OtherExpenseItems>(c => c.Employee_ID, typeof(tb_Employee));
            queryFilter.SetQueryField<View_FM_OtherExpenseItems>(c => c.Currency_ID, typeof(tb_Currency));
            queryFilter.SetQueryField<View_FM_OtherExpenseItems>(c => c.DepartmentID, typeof(tb_Department));
            queryFilter.SetQueryField<View_FM_OtherExpenseItems>(c => c.CustomerVendor_ID, typeof(tb_CustomerVendor));
            queryFilter.SetQueryField<View_FM_OtherExpenseItems>(c => c.ExpenseType_id, typeof(tb_FM_ExpenseType));
            queryFilter.SetQueryField<View_FM_OtherExpenseItems>(c => c.ProjectGroup_ID, typeof(tb_ProjectGroup));
            queryFilter.SetQueryField<View_FM_OtherExpenseItems>(c => c.Account_id, typeof(tb_FM_Account));
            queryFilter.SetQueryField<View_FM_OtherExpenseItems>(c => c.EXPOrINC);
            queryFilter.SetQueryField<View_FM_OtherExpenseItems>(c => c.ApprovalStatus, QueryFieldType.CmbEnum, typeof(ApprovalStatus));
            queryFilter.SetQueryField<View_FM_OtherExpenseItems>(c => c.DataStatus, QueryFieldType.CmbEnum, typeof(DataStatus));

            return queryFilter;
        }

    }
}



