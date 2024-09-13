
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:45
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
    /// 预收预付单,冲销动作会在付款单和收款单中体现
    /// </summary>
    public partial class tb_FM_PrePaymentBillServices : BaseServices<tb_FM_PrePaymentBill>, Itb_FM_PrePaymentBillServices
    {
        IMapper _mapper;
        public tb_FM_PrePaymentBillServices(IMapper mapper, IBaseRepository<tb_FM_PrePaymentBill> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}