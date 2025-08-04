
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/04/2025 11:58:52
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
    /// 应收应付表
    /// </summary>
    public partial class tb_FM_ReceivablePayableServices : BaseServices<tb_FM_ReceivablePayable>, Itb_FM_ReceivablePayableServices
    {
        IMapper _mapper;
        public tb_FM_ReceivablePayableServices(IMapper mapper, IBaseRepository<tb_FM_ReceivablePayable> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}