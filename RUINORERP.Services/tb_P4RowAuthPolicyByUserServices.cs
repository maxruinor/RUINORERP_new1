
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/29/2025 20:39:09
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
    /// 行级权限规则-用户关联表
    /// </summary>
    public partial class tb_P4RowAuthPolicyByUserServices : BaseServices<tb_P4RowAuthPolicyByUser>, Itb_P4RowAuthPolicyByUserServices
    {
        IMapper _mapper;
        public tb_P4RowAuthPolicyByUserServices(IMapper mapper, IBaseRepository<tb_P4RowAuthPolicyByUser> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}