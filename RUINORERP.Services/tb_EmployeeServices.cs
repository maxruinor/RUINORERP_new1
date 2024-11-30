
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/29/2024 23:20:19
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


namespace RUINORERP.Services
{
    /// <summary>
    /// 员工表
    /// </summary>
    public partial class tb_EmployeeServices : BaseServices<tb_Employee>, Itb_EmployeeServices
    {
        IMapper _mapper;
        public tb_EmployeeServices(IMapper mapper, IBaseRepository<tb_Employee> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}