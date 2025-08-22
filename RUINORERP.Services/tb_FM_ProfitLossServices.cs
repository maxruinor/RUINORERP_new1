
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/22/2025 20:05:17
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
    /// 损溢费用单
    /// </summary>
    public partial class tb_FM_ProfitLossServices : BaseServices<tb_FM_ProfitLoss>, Itb_FM_ProfitLossServices
    {
        IMapper _mapper;
        public tb_FM_ProfitLossServices(IMapper mapper, IBaseRepository<tb_FM_ProfitLoss> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}