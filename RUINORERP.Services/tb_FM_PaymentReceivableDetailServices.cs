
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/19/2025 22:58:05
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
    /// 应收明细 如果一个销售订单多次发货时，销售出库单即可对应这里的明细
    /// </summary>
    public partial class tb_FM_PaymentReceivableDetailServices : BaseServices<tb_FM_PaymentReceivableDetail>, Itb_FM_PaymentReceivableDetailServices
    {
        IMapper _mapper;
        public tb_FM_PaymentReceivableDetailServices(IMapper mapper, IBaseRepository<tb_FM_PaymentReceivableDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}