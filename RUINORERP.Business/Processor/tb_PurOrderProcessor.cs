
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/28/2024 18:56:47
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
    /// 采购订单，可能来自销售订单也可能来自生产需求也可以直接录数据
    /// </summary>
    public partial class tb_PurOrderProcessor : BaseProcessor
    {

        /// <summary>
        /// 样板方法
        /// </summary>
        /// <returns></returns>

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();


            var lambda = Expressionable.Create<tb_PurOrder>()
              .And(a => a.isdeleted == false)
              .AndIF(AuthorizeController.GetPurBizLimitedAuth(_appContext), t => t.Employee_ID == _appContext.CurUserInfo.UserInfo.Employee_ID)
             .ToExpression();//注意 这一句 不能少
            queryFilter.FilterLimitExpressions.Add(lambda);


            //供应商都 可以看到 暂时不限制
            var lambdaCV = Expressionable.Create<tb_CustomerVendor>()
                        .And(a => a.IsVendor == true)
                        .AndIF(AuthorizeController.GetExclusiveLimitedAuth(_appContext), a => a.IsExclusive == false)
                        .AndIF(AuthorizeController.GetPurBizLimitedAuth(_appContext), a => a.Employee_ID == _appContext.CurUserInfo.UserInfo.Employee_ID)
                        .OrIF(AuthorizeController.GetExclusiveLimitedAuth(_appContext), a => a.IsExclusive == true && a.Employee_ID == _appContext.CurUserInfo.UserInfo.Employee_ID)
                        .ToExpression();//注意 这一句 不能少

            //可以根据关联外键自动加载条件，条件用公共虚方法
            queryFilter.SetQueryField<tb_PurOrder>(c => c.PurOrderNo);
            queryFilter.SetQueryField<tb_PurOrder>(c => c.DepartmentID);
            queryFilter.SetQueryField<tb_PurOrder>(c => c.PurDate);
            queryFilter.SetQueryField<tb_PurOrder>(c => c.Employee_ID);
            queryFilter.SetQueryField<tb_PurOrder>(c => c.Paytype_ID);
            queryFilter.SetQueryField<tb_PurOrder>(c => c.Notes);
            queryFilter.SetQueryField<tb_PurOrder>(c => c.PrintStatus, QueryFieldType.CmbEnum, typeof(PrintStatus));
            queryFilter.SetQueryField<tb_PurOrder>(c => c.ApprovalStatus, QueryFieldType.CmbEnum, typeof(ApprovalStatus));
            queryFilter.SetQueryField<tb_PurOrder>(c => c.DataStatus, QueryFieldType.CmbEnum, typeof(DataStatus));

            queryFilter.SetQueryField<tb_PurOrder, tb_CustomerVendor>(c => c.CustomerVendor_ID, lambdaCV);


            //下面的枚举类型可以优化掉。这里暂时保留只是写法上的保留
            /*
            //审核状态
            QueryField queryFieldApprovalStatus = queryFilter.SetQueryField<tb_PurOrder>(c => c.ApprovalStatus);
            queryFieldApprovalStatus.QueryFieldDataPara = new QueryFieldEnumData();
            queryFieldApprovalStatus.FieldType = QueryFieldType.CmbEnum; //既然用接口声明了对应类型如枚举，这里有必要指定一下吗？
            if (queryFieldApprovalStatus.QueryFieldDataPara is QueryFieldEnumData enumData)
            {
                enumData.EnumType = typeof(ApprovalStatus);
                enumData.SetEnumValueColName<tb_PurOrder>(c => c.ApprovalStatus);
                enumData.AddSelectItem = true;
                ApprovalStatus enumbinddata = ApprovalStatus.已审核;
                enumData.BindDataSource = enumbinddata.GetListByEnum(1); ;
            }

            //数据状态
            QueryField queryFieldDataStatus = queryFilter.SetQueryField<tb_PurOrder>(c => c.DataStatus);
            queryFieldDataStatus.QueryFieldDataPara = new QueryFieldEnumData();
            queryFieldDataStatus.FieldType = QueryFieldType.CmbEnum;
            if (queryFieldDataStatus.QueryFieldDataPara is QueryFieldEnumData enumDataStatus)
            {
                enumDataStatus.EnumType = typeof(DataStatus);
                enumDataStatus.SetEnumValueColName<tb_PurOrder>(c => c.DataStatus);
                enumDataStatus.AddSelectItem = true;
                DataStatus enumbinddata = DataStatus.新建;
                enumDataStatus.BindDataSource = enumbinddata.GetListByEnum(1); ;
            }

            //打印状态
            QueryField queryFieldPrintStatus = queryFilter.SetQueryField<tb_PurOrder>(c => c.PrintStatus);

            queryFieldPrintStatus.QueryFieldDataPara = new QueryFieldEnumData(typeof(PrintStatus));
            queryFieldPrintStatus.FieldType = QueryFieldType.CmbEnum;
            if (queryFieldPrintStatus.QueryFieldDataPara is QueryFieldEnumData enumPrintStatus)
            {
                enumPrintStatus.SetEnumValueColName<tb_PurOrder>(c => c.PrintStatus);
                enumPrintStatus.AddSelectItem = true;
                enumPrintStatus.BindDataSource = EnumBindExt.GetListByEnum<PrintStatus>();
            }
            */

            return queryFilter;
        }

    }
}



