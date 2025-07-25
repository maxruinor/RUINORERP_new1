
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/24/2025 20:27:14
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
    /// 价格调整单明细
    /// </summary>
    public partial class tb_FM_PriceAdjustmentDetailServices : BaseServices<tb_FM_PriceAdjustmentDetail>, Itb_FM_PriceAdjustmentDetailServices
    {
        IMapper _mapper;
        public tb_FM_PriceAdjustmentDetailServices(IMapper mapper, IBaseRepository<tb_FM_PriceAdjustmentDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}