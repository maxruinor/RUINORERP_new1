
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/23/2024 18:54:52
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
    /// 付款方式 交易方式，后面扩展有关账期 账龄分析的字段
    /// </summary>
    public partial class tb_PaymentMethodServices : BaseServices<tb_PaymentMethod>, Itb_PaymentMethodServices
    {
        IMapper _mapper;
        public tb_PaymentMethodServices(IMapper mapper, IBaseRepository<tb_PaymentMethod> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}