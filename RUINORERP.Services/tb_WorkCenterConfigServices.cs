
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/31/2024 14:23:57
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
    /// 工作台配置表
    /// </summary>
    public partial class tb_WorkCenterConfigServices : BaseServices<tb_WorkCenterConfig>, Itb_WorkCenterConfigServices
    {
        IMapper _mapper;
        public tb_WorkCenterConfigServices(IMapper mapper, IBaseRepository<tb_WorkCenterConfig> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}