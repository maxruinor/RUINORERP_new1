
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/04/2025 14:48:21
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
    /// 行级权限规则
    /// </summary>
    public partial class tb_RowAuthPolicyServices : BaseServices<tb_RowAuthPolicy>, Itb_RowAuthPolicyServices
    {
        IMapper _mapper;
        public tb_RowAuthPolicyServices(IMapper mapper, IBaseRepository<tb_RowAuthPolicy> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}