
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/24/2025 18:44:33
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
    /// 价格调整单
    /// </summary>
    public partial class tb_FM_PriceAdjustmentServices : BaseServices<tb_FM_PriceAdjustment>, Itb_FM_PriceAdjustmentServices
    {
        IMapper _mapper;
        public tb_FM_PriceAdjustmentServices(IMapper mapper, IBaseRepository<tb_FM_PriceAdjustment> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}