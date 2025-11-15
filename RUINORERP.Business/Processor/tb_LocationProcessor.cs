
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

using RUINORERP.Model.Base;
using RUINORERP.Common.Extensions;
using RUINORERP.IServices.BASE;
using RUINORERP.Model.Context;
using System.Linq;
using RUINOR.Core;
using RUINORERP.Common.Helper;
using SqlSugar;
using System.Linq.Dynamic.Core;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 库位表
    /// </summary>
    public partial class tb_LocationProcessor : BaseProcessor
    {

        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();
            //内部的公共部分，外部是特殊情况
            var lambda = Expressionable.Create<tb_Location>()
                            .And(t => t.Is_enabled == true)
                          .ToExpression();//注意 这一句 不能少
            //这个因为供应商和客户混在一起。限制条件在外面 调用时确定 
            //2024-4-11思路升级  条件可以合并，这里也可以限制。合并时要注意怎么联接
            queryFilter.FilterLimitExpressions.Add(lambda);
            queryFilter.SetQueryField<tb_Location>(c => c.Name);
            return queryFilter;
        }

        /* 一个时可以。多个时不行。基类统一处理了
        /// <summary>
        /// 通过表达式，获取列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public override List<tb_Location> GetListByLimitExp<tb_Location>(List<tb_Location> list)
        {
            List<tb_Location> listInstances = new List<tb_Location>();
            QueryFilter queryFilter = this.GetQueryFilter();
            if (queryFilter.FilterLimitExpressions != null && queryFilter.FilterLimitExpressions.Count > 0)
            {
                if (queryFilter.FilterLimitExpressions.Count > 1)
                {

                }
                //表达式中就一个条件时正常。多个时出错
                var filteredList = list.AsQueryable().Where(queryFilter.FilterLimitExpressions[0]).ToList();
                listInstances = filteredList;
            }
            return listInstances;
        }

        */

    }
}



