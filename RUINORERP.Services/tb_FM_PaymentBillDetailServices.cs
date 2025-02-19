
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/19/2025 22:58:02
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
    /// 付款单明细
    /// </summary>
    public partial class tb_FM_PaymentBillDetailServices : BaseServices<tb_FM_PaymentBillDetail>, Itb_FM_PaymentBillDetailServices
    {
        IMapper _mapper;
        public tb_FM_PaymentBillDetailServices(IMapper mapper, IBaseRepository<tb_FM_PaymentBillDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}