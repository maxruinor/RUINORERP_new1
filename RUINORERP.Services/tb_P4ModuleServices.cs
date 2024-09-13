
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:00
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
    /// 模块权限表（暂时没有使用，逻辑上用菜单的代替了）
    /// </summary>
    public partial class tb_P4ModuleServices : BaseServices<tb_P4Module>, Itb_P4ModuleServices
    {
        IMapper _mapper;
        public tb_P4ModuleServices(IMapper mapper, IBaseRepository<tb_P4Module> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}