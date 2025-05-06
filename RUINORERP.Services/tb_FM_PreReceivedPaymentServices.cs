
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/06/2025 10:30:41
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
    /// 预收付款单
    /// </summary>
    public partial class tb_FM_PreReceivedPaymentServices : BaseServices<tb_FM_PreReceivedPayment>, Itb_FM_PreReceivedPaymentServices
    {
        IMapper _mapper;
        public tb_FM_PreReceivedPaymentServices(IMapper mapper, IBaseRepository<tb_FM_PreReceivedPayment> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}