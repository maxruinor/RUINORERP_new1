
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/17/2023 14:51:22
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
    /// 菜单程序集信息表
    /// </summary>
    public class tb_MenuAssemblyInfoServices : BaseServices<tb_MenuInfo>, Itb_MenuAssemblyInfoServices
    {
        IMapper _mapper;
        public tb_MenuAssemblyInfoServices(IMapper mapper, IBaseRepository<tb_MenuInfo> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}