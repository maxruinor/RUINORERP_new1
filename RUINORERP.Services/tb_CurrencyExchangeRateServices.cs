
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/19/2025 22:56:55
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
    /// 币别换算表-暂时不使用如果ERP系统需要支持多币种，通常需要在所有涉及外币的业务单据和凭证中添加外币和本币两个字段来保存对应的金额
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