
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:47:27
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
    /// 付款单 中有两种情况，1）如果有应收款，可以抵扣而少付款，如果有预付款也可以抵扣。
    /// </summary>
    public partial class tb_FM_PaymentBillServices : BaseServices<tb_FM_PaymentBill>, Itb_FM_PaymentBillServices
    {
        IMapper _mapper;
        public tb_FM_PaymentBillServices(IMapper mapper, IBaseRepository<tb_FM_PaymentBill> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}