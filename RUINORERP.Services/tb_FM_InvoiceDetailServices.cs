﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/07/2025 14:22:22
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
    /// 发票明细
    /// </summary>
    public partial class tb_FM_InvoiceDetailServices : BaseServices<tb_FM_InvoiceDetail>, Itb_FM_InvoiceDetailServices
    {
        IMapper _mapper;
        public tb_FM_InvoiceDetailServices(IMapper mapper, IBaseRepository<tb_FM_InvoiceDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}