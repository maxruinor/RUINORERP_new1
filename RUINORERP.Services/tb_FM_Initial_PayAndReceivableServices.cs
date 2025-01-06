
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:41
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
    /// 期初应收应付账款表
    /// </summary>
    public partial class tb_FM_Initial_PayAndReceivableServices : BaseServices<tb_FM_Initial_PayAndReceivable>, Itb_FM_Initial_PayAndReceivableServices
    {
        IMapper _mapper;
        public tb_FM_Initial_PayAndReceivableServices(IMapper mapper, IBaseRepository<tb_FM_Initial_PayAndReceivable> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}