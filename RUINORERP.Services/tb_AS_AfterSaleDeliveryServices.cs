
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/16/2025 10:05:07
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
    /// 售后交付单
    /// </summary>
    public partial class tb_AS_AfterSaleDeliveryServices : BaseServices<tb_AS_AfterSaleDelivery>, Itb_AS_AfterSaleDeliveryServices
    {
        IMapper _mapper;
        public tb_AS_AfterSaleDeliveryServices(IMapper mapper, IBaseRepository<tb_AS_AfterSaleDelivery> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}