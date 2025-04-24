
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/24/2025 10:38:00
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
    /// 销售订单
    /// </summary>
    public partial class tb_SaleOrderServices : BaseServices<tb_SaleOrder>, Itb_SaleOrderServices
    {
        IMapper _mapper;
        public tb_SaleOrderServices(IMapper mapper, IBaseRepository<tb_SaleOrder> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}