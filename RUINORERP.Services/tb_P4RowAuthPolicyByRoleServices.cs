
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/06/2025 15:41:57
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
    /// 行级权限规则-角色关联表
    /// </summary>
    public partial class tb_P4RowAuthPolicyByRoleServices : BaseServices<tb_P4RowAuthPolicyByRole>, Itb_P4RowAuthPolicyByRoleServices
    {
        IMapper _mapper;
        public tb_P4RowAuthPolicyByRoleServices(IMapper mapper, IBaseRepository<tb_P4RowAuthPolicyByRole> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}