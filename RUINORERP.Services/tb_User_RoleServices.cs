
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/05/2024 23:44:22
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
    /// 用户角色关系表
    /// </summary>
    public partial class tb_User_RoleServices : BaseServices<tb_User_Role>, Itb_User_RoleServices
    {
        IMapper _mapper;
        public tb_User_RoleServices(IMapper mapper, IBaseRepository<tb_User_Role> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}