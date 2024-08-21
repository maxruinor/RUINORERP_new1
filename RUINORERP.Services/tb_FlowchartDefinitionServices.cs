
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:47:08
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
    /// 流程图定义
    /// </summary>
    public partial class tb_FlowchartDefinitionServices : BaseServices<tb_FlowchartDefinition>, Itb_FlowchartDefinitionServices
    {
        IMapper _mapper;
        public tb_FlowchartDefinitionServices(IMapper mapper, IBaseRepository<tb_FlowchartDefinition> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}