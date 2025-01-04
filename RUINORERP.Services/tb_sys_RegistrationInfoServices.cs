
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/30/2024 18:08:39
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
    /// 系统注册信息
    /// </summary>
    public partial class tb_sys_RegistrationInfoServices : BaseServices<tb_sys_RegistrationInfo>, Itb_sys_RegistrationInfoServices
    {
        IMapper _mapper;
        public tb_sys_RegistrationInfoServices(IMapper mapper, IBaseRepository<tb_sys_RegistrationInfo> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}