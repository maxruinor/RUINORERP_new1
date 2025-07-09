
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/08/2025 19:05:30
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
    /// 维修工单  工时费 材料费
    /// </summary>
    public partial class tb_AS_RepairOrderServices : BaseServices<tb_AS_RepairOrder>, Itb_AS_RepairOrderServices
    {
        IMapper _mapper;
        public tb_AS_RepairOrderServices(IMapper mapper, IBaseRepository<tb_AS_RepairOrder> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}