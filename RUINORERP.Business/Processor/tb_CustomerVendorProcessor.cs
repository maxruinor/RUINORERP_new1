using Microsoft.Extensions.Logging;
using RUINORERP.Business.Security;
using RUINORERP.Common.Helper;
using RUINORERP.IServices;
using RUINORERP.Model;
using RUINORERP.Model.Context;
using RUINORERP.Repository.UnitOfWorks;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.Processor
{
    public partial class tb_CustomerVendorProcessor : BaseProcessor
    {
        /// <summary>
        /// 返回查询当前指定实体的结果，并且提供查询条件和限制条件
        /// </summary>
        /// <param name="FilterFieldLimitExpression"></param>
        /// <returns></returns>
        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();

            //内部的公共部分，外部是特殊情况
            var lambda = Expressionable.Create<tb_CustomerVendor>()
                            .And(t => t.isdeleted == false)
                            //.And(t => t.Is_available == true)
                            .AndIF(AuthorizeController.GetExclusiveLimitedAuth(_appContext), t => t.IsExclusive == false)
                           // .And(t => t.Is_enabled == true)
                           .OrIF(AuthorizeController.GetExclusiveLimitedAuth(_appContext), t => t.IsExclusive == true && t.Employee_ID == _appContext.CurUserInfo.UserInfo.Employee_ID)
                          .ToExpression();//注意 这一句 不能少
            //这个因为供应商和客户混在一起。限制条件在外面 调用时确定 
            //2024-4-11思路升级  条件可以合并，这里也可以限制。合并时要注意怎么联接
            queryFilter.FilterLimitExpressions.Add(lambda);

            queryFilter.SetQueryField<tb_CustomerVendor>(c => c.CVName);
            queryFilter.SetQueryField<tb_CustomerVendor>(c => c.Employee_ID);
            queryFilter.SetQueryField<tb_CustomerVendor>(c => c.IsCustomer);
            queryFilter.SetQueryField<tb_CustomerVendor>(c => c.IsVendor);
            queryFilter.SetQueryField<tb_CustomerVendor>(c => c.IsOther);
            queryFilter.SetQueryField<tb_CustomerVendor>(c => c.Is_enabled);
            queryFilter.SetQueryField<tb_CustomerVendor>(c => c.CVCode);
            queryFilter.SetQueryField<tb_CustomerVendor>(c => c.Contact);
            queryFilter.SetQueryField<tb_CustomerVendor>(c => c.Notes);
            queryFilter.SetQueryField<tb_CustomerVendor>(c => c.Phone);
            queryFilter.SetQueryField<tb_CustomerVendor>(c => c.Address);

            ///////queryFilter.SetQueryField<tb_CustomerVendor>(c => c.IsCustomer); 这种公用的表用来区别不同业务逻辑的。不能当条件加入


            queryFilter.SetQueryField<tb_CustomerVendor>(c => c.Paytype_ID);

            return queryFilter;
        }

    }
}
