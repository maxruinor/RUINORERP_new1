
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/22/2025 12:16:09
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
    public partial class tb_FM_PaymentDetailServices : BaseServices<tb_FM_PaymentDetail>, Itb_FM_PaymentDetailServices
    {
        IMapper _mapper;
        public tb_FM_PaymentDetailServices(IMapper mapper, IBaseRepository<tb_FM_PaymentDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}