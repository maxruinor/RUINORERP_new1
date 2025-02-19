
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/19/2025 22:57:59
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
    /// 付款单是付款的执行不需要审核只需要一个付款状态
    /// </summary>
    public partial class tb_FM_PaymentServices : BaseServices<tb_FM_Payment>, Itb_FM_PaymentServices
    {
        IMapper _mapper;
        public tb_FM_PaymentServices(IMapper mapper, IBaseRepository<tb_FM_Payment> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}