
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/19/2025 22:57:57
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
    /// 应付款单
    /// </summary>
    public partial class tb_FM_PayableServices : BaseServices<tb_FM_Payable>, Itb_FM_PayableServices
    {
        IMapper _mapper;
        public tb_FM_PayableServices(IMapper mapper, IBaseRepository<tb_FM_Payable> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}