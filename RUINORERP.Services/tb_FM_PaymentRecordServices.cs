
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/27/2025 15:20:53
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
    /// 收款付款记录表-记录所有资金流动一批订单可分账户分批付 记录真实资金流动，用户需确保其 与银行流水一致
    /// </summary>
    public partial class tb_FM_PaymentRecordServices : BaseServices<tb_FM_PaymentRecord>, Itb_FM_PaymentRecordServices
    {
        IMapper _mapper;
        public tb_FM_PaymentRecordServices(IMapper mapper, IBaseRepository<tb_FM_PaymentRecord> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}