
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/12/2025 21:29:57
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
    /// 开票资料表
    /// </summary>
    public partial class tb_BillingInformationServices : BaseServices<tb_BillingInformation>, Itb_BillingInformationServices
    {
        IMapper _mapper;
        public tb_BillingInformationServices(IMapper mapper, IBaseRepository<tb_BillingInformation> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}