
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 09:38:36
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
    /// 系统全局动态配置表 行转列
    /// </summary>
    public partial class tb_SysGlobalDynamicConfigServices : BaseServices<tb_SysGlobalDynamicConfig>, Itb_SysGlobalDynamicConfigServices
    {
        IMapper _mapper;
        public tb_SysGlobalDynamicConfigServices(IMapper mapper, IBaseRepository<tb_SysGlobalDynamicConfig> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}