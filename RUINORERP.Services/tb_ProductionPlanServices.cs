
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:16
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
    /// 生产计划表 应该是分析来的。可能来自于生产需求单，比方系统根据库存情况分析销售情况等也也可以手动。也可以程序分析
    /// </summary>
    public partial class tb_ProductionPlanServices : BaseServices<tb_ProductionPlan>, Itb_ProductionPlanServices
    {
        IMapper _mapper;
        public tb_ProductionPlanServices(IMapper mapper, IBaseRepository<tb_ProductionPlan> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}