
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2023 23:26:31
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
    /// 权限表
    /// </summary>
    public class tb_PermissionServices : BaseServices<tb_Permission>, Itb_PermissionServices
    {
        IMapper _mapper;
        public tb_PermissionServices(IMapper mapper, IBaseRepository<tb_Permission> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}