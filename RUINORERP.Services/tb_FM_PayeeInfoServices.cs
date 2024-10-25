
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/22/2024 18:15:11
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
    /// 收款信息，供应商报销人的收款账号
    /// </summary>
    public partial class tb_FM_PayeeInfoServices : BaseServices<tb_FM_PayeeInfo>, Itb_FM_PayeeInfoServices
    {
        IMapper _mapper;
        public tb_FM_PayeeInfoServices(IMapper mapper, IBaseRepository<tb_FM_PayeeInfo> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}