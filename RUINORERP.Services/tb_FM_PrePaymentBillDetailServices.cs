
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:45
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
    /// 预付款表明细
    /// </summary>
    public partial class tb_FM_PrePaymentBillDetailServices : BaseServices<tb_FM_PrePaymentBillDetail>, Itb_FM_PrePaymentBillDetailServices
    {
        IMapper _mapper;
        public tb_FM_PrePaymentBillDetailServices(IMapper mapper, IBaseRepository<tb_FM_PrePaymentBillDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}