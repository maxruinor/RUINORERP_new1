
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/01/2025 12:16:53
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
    /// 收付款记录明细表
    /// </summary>
    public partial class tb_FM_PaymentRecordDetailServices : BaseServices<tb_FM_PaymentRecordDetail>, Itb_FM_PaymentRecordDetailServices
    {
        IMapper _mapper;
        public tb_FM_PaymentRecordDetailServices(IMapper mapper, IBaseRepository<tb_FM_PaymentRecordDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}