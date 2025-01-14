﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/07/2025 21:48:21
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
    /// 部门表是否分层
    /// </summary>
    public partial class tb_DepartmentServices : BaseServices<tb_Department>, Itb_DepartmentServices
    {
        IMapper _mapper;
        public tb_DepartmentServices(IMapper mapper, IBaseRepository<tb_Department> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}