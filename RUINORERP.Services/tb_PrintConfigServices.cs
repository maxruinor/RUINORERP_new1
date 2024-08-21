
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/26/2024 10:52:22
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
    /// 报表打印配置表
    /// </summary>
    public partial class tb_PrintConfigServices : BaseServices<tb_PrintConfig>, Itb_PrintConfigServices
    {
        IMapper _mapper;
        public tb_PrintConfigServices(IMapper mapper, IBaseRepository<tb_PrintConfig> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}