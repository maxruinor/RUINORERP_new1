
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/07/2025 15:37:49
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
    /// 应收应付明细
    /// </summary>
    public partial class tb_FM_ReceivablePayableDetailServices : BaseServices<tb_FM_ReceivablePayableDetail>, Itb_FM_ReceivablePayableDetailServices
    {
        IMapper _mapper;
        public tb_FM_ReceivablePayableDetailServices(IMapper mapper, IBaseRepository<tb_FM_ReceivablePayableDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}