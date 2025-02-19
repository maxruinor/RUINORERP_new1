
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
    /// 应收款单
    /// </summary>
    public partial class tb_FM_PaymentReceivableServices : BaseServices<tb_FM_PaymentReceivable>, Itb_FM_PaymentReceivableServices
    {
        IMapper _mapper;
        public tb_FM_PaymentReceivableServices(IMapper mapper, IBaseRepository<tb_FM_PaymentReceivable> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}