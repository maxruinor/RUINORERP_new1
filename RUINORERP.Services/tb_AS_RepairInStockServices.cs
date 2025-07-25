
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/24/2025 20:25:35
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
    /// 维修入库单
    /// </summary>
    public partial class tb_AS_RepairInStockServices : BaseServices<tb_AS_RepairInStock>, Itb_AS_RepairInStockServices
    {
        IMapper _mapper;
        public tb_AS_RepairInStockServices(IMapper mapper, IBaseRepository<tb_AS_RepairInStock> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}