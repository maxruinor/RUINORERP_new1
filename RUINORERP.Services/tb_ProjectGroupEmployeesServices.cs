
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/24/2025 14:14:54
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
    /// 项目及成员关系表
    /// </summary>
    public partial class tb_ProjectGroupEmployeesServices : BaseServices<tb_ProjectGroupEmployees>, Itb_ProjectGroupEmployeesServices
    {
        IMapper _mapper;
        public tb_ProjectGroupEmployeesServices(IMapper mapper, IBaseRepository<tb_ProjectGroupEmployees> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}