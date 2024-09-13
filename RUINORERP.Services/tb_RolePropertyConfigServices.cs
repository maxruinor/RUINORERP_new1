
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:26
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
    /// 角色属性配置不同角色权限功能等不一样
    /// </summary>
    public partial class tb_RolePropertyConfigServices : BaseServices<tb_RolePropertyConfig>, Itb_RolePropertyConfigServices
    {
        IMapper _mapper;
        public tb_RolePropertyConfigServices(IMapper mapper, IBaseRepository<tb_RolePropertyConfig> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}