
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/19/2025 17:12:41
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
    /// 维修领料单明细
    /// </summary>
    public partial class tb_AS_RepairMaterialPickupDetailServices : BaseServices<tb_AS_RepairMaterialPickupDetail>, Itb_AS_RepairMaterialPickupDetailServices
    {
        IMapper _mapper;
        public tb_AS_RepairMaterialPickupDetailServices(IMapper mapper, IBaseRepository<tb_AS_RepairMaterialPickupDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}