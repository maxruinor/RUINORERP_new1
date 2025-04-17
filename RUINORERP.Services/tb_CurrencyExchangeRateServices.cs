
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/16/2025 12:02:51
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
    /// 币别换算表
    /// </summary>
    public partial class tb_CurrencyExchangeRateServices : BaseServices<tb_CurrencyExchangeRate>, Itb_CurrencyExchangeRateServices
    {
        IMapper _mapper;
        public tb_CurrencyExchangeRateServices(IMapper mapper, IBaseRepository<tb_CurrencyExchangeRate> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}