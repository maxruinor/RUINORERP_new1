
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/15/2023 19:21:45
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
    /// 生产计划表
    /// </summary>
    public partial class tb_Prod_PlanServices : BaseServices<tb_Prod_Plan>, Itb_Prod_PlanServices
    {
        IMapper _mapper;
        public tb_Prod_PlanServices(IMapper mapper, IBaseRepository<tb_Prod_Plan> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}