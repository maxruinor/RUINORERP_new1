
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/18/2023 17:03:55
// **************************************
using AutoMapper;
using RUINORERP.IRepository.Base;
using RUINORERP.IServices;
using RUINORERP.Model;
using RUINORERP.Services.BASE;
using System.Threading.Tasks;
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Common;
using Autofac.Extras.DynamicProxy;

namespace RUINORERP.Services
{
    /// <summary>
    /// 部门表
    /// </summary>
    public partial class tb_DepartmentServices : BaseServices<tb_Department>, Itb_DepartmentServices
    {


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        [Caching(AbsoluteExpiration = 10)]
        public async Task<List<tb_Department>> GetDepartments()
        {
            var list = await base.Query(a => a.DepartmentID > 0, a => a.DepartmentID);
            return list;

        }



    }
}