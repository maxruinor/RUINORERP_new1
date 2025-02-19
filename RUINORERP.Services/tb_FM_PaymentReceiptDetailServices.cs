
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/19/2025 22:58:04
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
    /// 收款单明细-对应的应收单据项目
    /// </summary>
    public partial class tb_FM_PaymentReceiptDetailServices : BaseServices<tb_FM_PaymentReceiptDetail>, Itb_FM_PaymentReceiptDetailServices
    {
        IMapper _mapper;
        public tb_FM_PaymentReceiptDetailServices(IMapper mapper, IBaseRepository<tb_FM_PaymentReceiptDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}