
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/07/2025 14:22:20
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
    /// 发票
    /// </summary>
    public partial class tb_FM_InvoiceServices : BaseServices<tb_FM_Invoice>, Itb_FM_InvoiceServices
    {
        IMapper _mapper;
        public tb_FM_InvoiceServices(IMapper mapper, IBaseRepository<tb_FM_Invoice> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}