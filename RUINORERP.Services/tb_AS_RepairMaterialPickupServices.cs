
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/22/2025 18:02:28
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
    /// 维修领料单
    /// </summary>
    public partial class tb_AS_RepairMaterialPickupServices : BaseServices<tb_AS_RepairMaterialPickup>, Itb_AS_RepairMaterialPickupServices
    {
        IMapper _mapper;
        public tb_AS_RepairMaterialPickupServices(IMapper mapper, IBaseRepository<tb_AS_RepairMaterialPickup> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}