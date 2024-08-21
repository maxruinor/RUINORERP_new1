
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:47:52
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
    /// 功能模块定义（仅限部分已经硬码并体现于菜单表中）
    /// </summary>
    public partial class tb_ModuleDefinitionServices : BaseServices<tb_ModuleDefinition>, Itb_ModuleDefinitionServices
    {
        IMapper _mapper;
        public tb_ModuleDefinitionServices(IMapper mapper, IBaseRepository<tb_ModuleDefinition> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}