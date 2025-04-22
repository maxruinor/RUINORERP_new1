
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/22/2025 12:16:07
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
    /// 付款申请单-即为付款单
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