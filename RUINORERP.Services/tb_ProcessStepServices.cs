
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:48:09
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
    /// 流程步骤
    /// </summary>
    public partial class tb_ProcessStepServices : BaseServices<tb_ProcessStep>, Itb_ProcessStepServices
    {
        IMapper _mapper;
        public tb_ProcessStepServices(IMapper mapper, IBaseRepository<tb_ProcessStep> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}