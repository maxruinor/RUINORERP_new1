
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/19/2025 22:58:07
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
    /// 预付款单
    /// </summary>
    public partial class tb_FM_PrePayServices : BaseServices<tb_FM_PrePay>, Itb_FM_PrePayServices
    {
        IMapper _mapper;
        public tb_FM_PrePayServices(IMapper mapper, IBaseRepository<tb_FM_PrePay> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}