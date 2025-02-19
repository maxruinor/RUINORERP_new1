
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/19/2025 22:58:06
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
    /// 付款申请单明细-对应的应付单据项目
    /// </summary>
    public partial class tb_FM_PaymentRequestDetailServices : BaseServices<tb_FM_PaymentRequestDetail>, Itb_FM_PaymentRequestDetailServices
    {
        IMapper _mapper;
        public tb_FM_PaymentRequestDetailServices(IMapper mapper, IBaseRepository<tb_FM_PaymentRequestDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}