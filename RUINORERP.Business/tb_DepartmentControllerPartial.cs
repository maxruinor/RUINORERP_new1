
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/18/2023 17:03:55
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
using RUINORERP.Common;
using RUINORERP.Extensions.AOP;

namespace RUINORERP.Business
{
    /// <summary>
    /// 部门表
    /// </summary>
    [Intercept(typeof(BaseDataCacheAOP))]
    public partial class tb_DepartmentController<T>
    {
        [Caching(AbsoluteExpiration = 10)]
        public virtual tb_Department GetDepartmentByID(object id)
        {
            tb_Department entity =  _tb_DepartmentServices.QueryById(id);
            return entity;
        }


        public enum DepartmentCode
        {
            Finance = 1,
            Marketing = 2,
            Sales = 3
        }

        public static string GetDepartmentCodeString(DepartmentCode departmentCode)
        {
            switch (departmentCode)
            {
                case DepartmentCode.Finance:
                    return "F";
                case DepartmentCode.Marketing:
                    return "M";
                case DepartmentCode.Sales:
                    return "S";
                default:
                    throw new ArgumentException("无效的部门代号");
            }
        }

    }
}