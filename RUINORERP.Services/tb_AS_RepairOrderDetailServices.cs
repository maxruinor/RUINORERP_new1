
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/11/2025 15:43:14
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
    /// 维修工单明细
    /// </summary>
    public partial class tb_AS_RepairOrderDetailServices : BaseServices<tb_AS_RepairOrderDetail>, Itb_AS_RepairOrderDetailServices
    {
        IMapper _mapper;
        public tb_AS_RepairOrderDetailServices(IMapper mapper, IBaseRepository<tb_AS_RepairOrderDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}