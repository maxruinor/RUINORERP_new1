
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/24/2025 20:25:45
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
    /// 系统配置表
    /// </summary>
    public partial class tb_SystemConfigServices : BaseServices<tb_SystemConfig>, Itb_SystemConfigServices
    {
        IMapper _mapper;
        public tb_SystemConfigServices(IMapper mapper, IBaseRepository<tb_SystemConfig> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}