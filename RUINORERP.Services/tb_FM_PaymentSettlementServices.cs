
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/24/2025 20:27:04
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
    /// 记录收款 与应收的匹配，核销表 核销记录用于跟踪资金与债权债务的冲抵关系，确保财务数据可追溯。正常的收款，支付不需要保存核销记录
    /// </summary>
    public partial class tb_FM_PaymentSettlementServices : BaseServices<tb_FM_PaymentSettlement>, Itb_FM_PaymentSettlementServices
    {
        IMapper _mapper;
        public tb_FM_PaymentSettlementServices(IMapper mapper, IBaseRepository<tb_FM_PaymentSettlement> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}