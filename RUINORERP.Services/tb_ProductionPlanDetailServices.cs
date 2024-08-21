
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/24/2024 17:20:32
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
    /// 生产计划明细
    /// </summary>
    public partial class tb_ProductionPlanDetailServices : BaseServices<tb_ProductionPlanDetail>, Itb_ProductionPlanDetailServices
    {
        IMapper _mapper;
        public tb_ProductionPlanDetailServices(IMapper mapper, IBaseRepository<tb_ProductionPlanDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}