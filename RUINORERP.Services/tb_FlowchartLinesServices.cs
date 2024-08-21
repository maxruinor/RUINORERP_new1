
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/03/2023 23:30:57
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
    /// 流程图线
    /// </summary>
    public partial class tb_FlowchartLinesServices : BaseServices<tb_FlowchartLines>, Itb_FlowchartLinesServices
    {
        IMapper _mapper;
        public tb_FlowchartLinesServices(IMapper mapper, IBaseRepository<tb_FlowchartLines> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}