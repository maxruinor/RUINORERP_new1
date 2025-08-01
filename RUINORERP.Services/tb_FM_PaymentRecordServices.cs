
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/01/2025 12:16:50
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
    /// 收付款记录表
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