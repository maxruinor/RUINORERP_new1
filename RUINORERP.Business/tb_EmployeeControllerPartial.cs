
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/18/2023 17:03:56
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
using Autofac.Extras.DynamicProxy;
using RUINORERP.Extensions.AOP;
using RUINORERP.Common;

namespace RUINORERP.Business
{
    /// <summary>
    /// 员工表
    /// </summary>
    [Intercept(typeof(BaseDataCacheAOP))]
    public partial class tb_EmployeeController<T>
    {
        public async Task<List<tb_Employee>> QueryInUse()
        {
            //这里要限制在使用中的。可用 启用等
            List<tb_Employee> list = await _tb_EmployeeServices.QueryAsync();
            return list;
        }

        [Caching(AbsoluteExpiration = 10)]
        public virtual tb_Employee GetEmployeeByID(object id)
        {
            tb_Employee entity = _tb_EmployeeServices.QueryById(id);
            return entity;
        }

        public virtual List<tb_Employee> GetEmployeeall()
        {
            List<tb_Employee> entity = _unitOfWorkManage.GetDbClient().Queryable<tb_Employee>()
                .Where(e => e.Is_available == true).ToList();
            return entity;
        }
    }
}