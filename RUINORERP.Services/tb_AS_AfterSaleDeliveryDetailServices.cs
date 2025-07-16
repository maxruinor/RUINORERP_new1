
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/16/2025 10:05:08
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
    /// 售后交付明细
    /// </summary>
    public partial class tb_AS_AfterSaleDeliveryDetailServices : BaseServices<tb_AS_AfterSaleDeliveryDetail>, Itb_AS_AfterSaleDeliveryDetailServices
    {
        IMapper _mapper;
        public tb_AS_AfterSaleDeliveryDetailServices(IMapper mapper, IBaseRepository<tb_AS_AfterSaleDeliveryDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}