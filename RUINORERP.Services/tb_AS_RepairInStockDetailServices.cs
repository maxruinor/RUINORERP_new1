
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/11/2025 15:53:34
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
    /// 维修入库单明细
    /// </summary>
    public partial class tb_AS_RepairInStockDetailServices : BaseServices<tb_AS_RepairInStockDetail>, Itb_AS_RepairInStockDetailServices
    {
        IMapper _mapper;
        public tb_AS_RepairInStockDetailServices(IMapper mapper, IBaseRepository<tb_AS_RepairInStockDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}